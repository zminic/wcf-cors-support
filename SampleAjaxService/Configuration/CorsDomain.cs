using System;
using System.Configuration;

namespace SampleAjaxService.Configuration
{
    public class CorsDomain : ConfigurationElement
    {
        [ConfigurationProperty("Name", IsRequired = true)]
        public string Name
        {
            get { return Convert.ToString(this["Name"]); }
            set { this["Name"] = value; }
        }

        [ConfigurationProperty("AllowMethods", IsRequired = true)]
        public string AllowMethods
        {
            get { return Convert.ToString(this["AllowMethods"]); }
            set { this["AllowMethods"] = value; }
        }

        [ConfigurationProperty("AllowHeaders")]
        public string AllowHeaders
        {
            get { return Convert.ToString(this["AllowHeaders"]); }
            set { this["AllowHeaders"] = value; }
        }

        [ConfigurationProperty("AllowCredentials")]
        public bool AllowCredentials
        {
            get { return Convert.ToBoolean(this["AllowCredentials"]); }
            set { this["AllowCredentials"] = value; }
        }
    }
}
