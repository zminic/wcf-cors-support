using System.ServiceModel;
using System.ServiceModel.Web;

namespace SampleAjaxService
{
    [ServiceContract]
    public class AjaxService
    {
        [OperationContract]
        public string GetData(string data)
        {
            return "you entered: " + data;
        }
    }
}