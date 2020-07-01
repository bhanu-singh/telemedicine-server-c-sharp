using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft;
using Newtonsoft.Json;
using System.Text;

namespace ImTech.Service
{
    public class WebCommon
    {
        public static WebCommon Instance = new WebCommon();

        public const string Alphabet =
"abcdefghijklmnopqrstuvwyxzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        Random rand = new Random();
        public T GetObject<T>(object json)
        {
            return JsonConvert.DeserializeObject<T>(Convert.ToString(json));
        }

        public string GetOTP()
        {

            char[] chars = new char[6];
            for (int i = 0; i < 6; i++)
            {
                chars[i] = Alphabet[rand.Next(Alphabet.Length)];
            }
            return new string(chars);
        }

        public enum SystemEntities
        {
            User = 1,
            Doctor = 2,
            Admin = 3
        }
        public enum PaymentStatus
        {
            PaymentInitiated = 0,
            Authorise = 1,
            AuthoriseFailed = 2,
            Capture = 3,
            CaptureFailed = 4,
            Failed = 5,
            Success = 6
        }
       
        public static bool NotificationEnabled = true;
    }
}