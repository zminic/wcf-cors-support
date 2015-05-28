using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using SampleAjaxService.Configuration;

namespace SampleAjaxService
{
    public class EnableCorsSupportBehavior: IEndpointBehavior
    {
        public void Validate(ServiceEndpoint endpoint)
        {
            
        }

        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
            
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            endpointDispatcher.DispatchRuntime.MessageInspectors.Add(new CorsEnablingMessageInspector());
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            
        }
    }

    public class CorsEnablingMessageInspector : IDispatchMessageInspector
    {
        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            var httpRequest = (HttpRequestMessageProperty)request.Properties[HttpRequestMessageProperty.Name];

            return new
            {
                origin = httpRequest.Headers["Origin"],
                handlePreflight = httpRequest.Method.Equals("OPTIONS", StringComparison.InvariantCultureIgnoreCase)
            };
        }

        public void BeforeSendReply(ref Message reply, object correlationState)
        {
            var state = (dynamic)correlationState;

            var config = ConfigurationManager.GetSection("customSettings") as CustomSettings;

            if (config == null)
                throw new InvalidOperationException("Missing CORS configuration");

            var domain = config.CorsSupport.OfType<CorsDomain>().FirstOrDefault(d => d.Name == state.origin);

            if (domain != null)
            {
                // handle request preflight
                if (state.handlePreflight)
                {
                    reply = Message.CreateMessage(MessageVersion.None, "PreflightReturn");

                    var httpResponse = new HttpResponseMessageProperty();
                    reply.Properties.Add(HttpResponseMessageProperty.Name, httpResponse);

                    httpResponse.SuppressEntityBody = true;
                    httpResponse.StatusCode = HttpStatusCode.OK;
                }

                // add allowed origin info
                var response = (HttpResponseMessageProperty)reply.Properties[HttpResponseMessageProperty.Name];
                response.Headers.Add("Access-Control-Allow-Origin", domain.Name);

                if (!string.IsNullOrEmpty(domain.AllowMethods))
                    response.Headers.Add("Access-Control-Allow-Methods", domain.AllowMethods);

                if (!string.IsNullOrEmpty(domain.AllowHeaders))
                    response.Headers.Add("Access-Control-Allow-Headers", domain.AllowHeaders);

                if (domain.AllowCredentials)
                    response.Headers.Add("Access-Control-Allow-Credentials", "true");
            }
        }
    }

    public class EnableCorsSupportBehaviorElement : BehaviorExtensionElement
    {
        protected override object CreateBehavior()
        {
            return new EnableCorsSupportBehavior();
        }

        public override Type BehaviorType
        {
            get { return typeof(EnableCorsSupportBehavior); }
        }
    }
}
