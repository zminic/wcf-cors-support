using System;
using System.ServiceModel;

namespace SampleAjaxService
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var serviceHost = new ServiceHost(typeof (AjaxService)))
            {
                serviceHost.Open();

                Console.WriteLine("Service is ready. Press <Enter> to stop the service.");

                Console.ReadKey();

                serviceHost.Close();
            }
        }
    }
}
