using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImTech.DataBase
{
   public class ConfigurationManager
    {
        private static string GetValue(string key, string defaultValue = "")
        {
            if (System.Configuration.ConfigurationManager.AppSettings.AllKeys.Any(k => k == key))
            {
                return System.Configuration.ConfigurationManager.AppSettings[key];
            }

            return defaultValue;
        }
        public static string EncryptionKey
        {
            get
            {
                return GetValue("EncryptionKey");
            }
        }

        public static string ConnectionString
        {
            get
            {
                return GetValue("ConnectionString");
            }
        }
        public static string AWSAccessKey
        {
            get
            {
                return GetValue("AWSAccessKey");
            }
        }
        public static string AWSSecrateAccessKey
        {
            get
            {
                return GetValue("AWSSecrateAccessKey");
            }
        }
        public static string AWSRegion
        {
            get
            {
                return GetValue("AWSRegion");
            }
        }
        public static string QueueURL
        {
            get
            {
                return GetValue("qURL");
            }
        }
    }
}
