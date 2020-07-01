using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace ImTech.Service
{
    public static class Validator
    {
        public static bool EmailValidation(string email)
        {

            if (String.IsNullOrEmpty(email))
                return false;
            // Return true if strIn is in valid e-mail format.
            try
            {
                return Regex.IsMatch(email,
                      @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                      @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                      RegexOptions.IgnoreCase);
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }

        }

        public static bool PhoneValidation(string number)
        {
            if (string.IsNullOrEmpty(number))
            {
                return false;
            }
            try
            {
                long no;
                if (long.TryParse(number.Trim(), out no))
                {
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }



}