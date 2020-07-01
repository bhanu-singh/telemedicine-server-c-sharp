using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImTech.DataServices
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
        public static string FileSystem
        {
            get
            {
                return GetValue("FileSystem");
            }
        }
        public static string FileUploadPath
        {
            get
            {
                return GetValue("FileUploadPath");
            }
        }
        public static string FileSaparator
        {
            get
            {
                return GetValue("FileSaparator");
            }
        }
        public static string AWSBucketName
        {
            get
            {
                return GetValue("AWSBucketName");
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
        public static string DefaultPassword
        {
            get
            {
                return GetValue("DefaultPassword");
            }
        }
        public static string LogMailSubject
        {
            get
            {
                return GetValue("LogMailSubject");
            }
        }

        public static string LogMailTo
        {
            get
            {
                return GetValue("LogMailTo");
            }
        }
        public static string LogMailCC
        {
            get
            {
                return GetValue("LogMailCC");
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
        public static string SMTPPassword
        {
            get
            {
                return GetValue("SMTPPassword");
            }
        }
        public static string SMSUserName
        {
            get
            {
                return GetValue("SMSUserName");
            }
        }

    }
}
