using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImTech.Service.Common
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
        public static string PaymentGatewayKey
        {
            get
            {
                return GetValue("PaymentGatewayKey");
            }
        }
        public static string PaymentGatewaySecret
        {
            get
            {
                return GetValue("PaymentGatewaySecret");
            }
        }
        public static string DefaultPassword
        {
            get
            {
                return GetValue("DefaultPassword");
            }
        }
    }
}