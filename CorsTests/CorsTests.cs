using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CorsTests
{
    [TestClass]
    public class CorsTests
    {
        [TestMethod]
        public async Task CorsBehavior_DoesNotAllowOriginsNotDefinedInConfiguration()
        {
            var methodAddress = new Uri("http://localhost:5000/AjaxService/GetData");

            var client = new HttpClient();

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await client.SendAsync(new HttpRequestMessage(new HttpMethod("OPTIONS"), methodAddress)
            {
                Headers = { {"Origin","unknownOrigin"} }
            });

            Assert.IsTrue(response.StatusCode == HttpStatusCode.MethodNotAllowed);
        }

        [TestMethod]
        public async Task CorsBehavior_CorrectlyRespondsToPreflightForConfiguredOrigin()
        {
            var methodAddress = new Uri("http://localhost:5000/AjaxService/GetData");

            var client = new HttpClient();

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await client.SendAsync(new HttpRequestMessage(new HttpMethod("OPTIONS"), methodAddress)
            {
                Headers = { { "Origin", "http://somedomain" } }
            });

            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(response.Headers.Any(x => x.Key == "Access-Control-Allow-Origin" && x.Value.Contains("http://somedomain")));
        }

        [TestMethod]
        public async Task CorsBehavior_CorrectlySetsHeadersForConfiguredOrigin_RegularRequest()
        {
            var methodAddress = new Uri("http://localhost:5000/AjaxService/GetData");

            var client = new HttpClient();

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await client.SendAsync(new HttpRequestMessage(new HttpMethod("POST"), methodAddress)
            {
                Content = new StringContent("{\"data\":\"test\"}", Encoding.UTF8, "application/json"),
                Headers = { { "Origin", "http://anotherdomain" } }
            });

            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(response.Headers.Any(x => x.Key == "Access-Control-Allow-Origin" && x.Value.Contains("http://anotherdomain")));
            Assert.IsTrue((await response.Content.ReadAsStringAsync()).Contains("you entered: test"));
        }
    }
}