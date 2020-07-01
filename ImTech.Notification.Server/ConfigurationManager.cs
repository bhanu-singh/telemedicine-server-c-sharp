using Amazon;
using System.Linq;
using System;
namespace ImTech.Notification.Server
{
    public class ConfigurationManager
    {
        public static string LogFolderPath = string.Empty;

        private static string GetValue(string key, string defaultValue = "")
        {
            if (System.Configuration.ConfigurationManager.AppSettings.AllKeys.Any(k => k == key))
            {
                return System.Configuration.ConfigurationManager.AppSettings[key];
            }

            return defaultValue;
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
        public static RegionEndpoint AWSRegion
        {
            get
            {
                return RegionEndpoint.GetBySystemName(GetValue("AWSRegion"));
            }
        }
        public static string QueueURL
        {
            get
            {
                return GetValue("qURL");
            }
        }

        public static string SMTPServer
        {
            get
            {
                return GetValue("SMTPServer");
            }
        }

        public static int SMTPPort
        {
            get
            {
                int port;
                string kValue = GetValue("SMTPPort", "0");

                if (int.TryParse(kValue, out port))
                {
                    return port;
                }

                return 0;
            }
        }

        public static string SMTPUserNamne
        {
            get
            {
                return GetValue("SMTPUserNamne");
            }
        }
        public static string SMTPFromAddress
        {
            get
            {
                return GetValue("SMTPFromAddress");
            }
        }
        public static string BCC
        {
            get
            {
                return GetValue("Bcc");
            }
        }

        public static string SMTPPassword
        {
            get
            {
                return GetValue("SMTPPassword");
            }
        }
        public static bool TestMode
        {
            get
            {
                return Convert.ToBoolean(GetValue("TestMode"));
            }
        }
        public static string SMSUserName
        {
            get
            {
                return GetValue("SMSUserName");
            }
        }
        public static string SMSHash
        {
            get
            {
                return GetValue("SMSHash");
            }
        }
        public static string SMSAPIURL
        {
            get
            {
                return GetValue("SMSAPIURL");
            }
        }
        public static bool Test
        {
            get
            {
                return Convert.ToBoolean(GetValue("Test"));
            }

        }
    }
}
