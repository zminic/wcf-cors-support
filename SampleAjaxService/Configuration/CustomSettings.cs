using System.Configuration;

namespace SampleAjaxService.Configuration
{
    public class CustomSettings: ConfigurationSection
    {
        [ConfigurationProperty("CorsSupport", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(CorsSupport), AddItemName = "Domain")]
        public CorsSupport CorsSupport
        {
            get { return (CorsSupport)this["CorsSupport"]; }
            set { this["CorsSupport"] = value; }
        }
    }
}
