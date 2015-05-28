using System.Configuration;

namespace SampleAjaxService.Configuration
{
    public class CorsSupport : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new CorsDomain();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            var service = (CorsDomain)element;

            return service.Name;
        }
    }
}
