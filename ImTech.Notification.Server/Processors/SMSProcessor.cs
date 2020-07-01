using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImTech.Notification.Messages;
using System.Net;
using System.Collections.Specialized;
using System.IO;
using Sinch.ServerSdk;
using System.Web;

namespace ImTech.Notification.Server
{
    public class SMSProcessor : MessageProcessor
    {
        public static SMSProcessor Instance = new SMSProcessor();

        public override void Process(object message)
        {
            sendSMS(((BaseMessage)message).UserMobileNo, GetMessage((BaseMessage)message));
            LogManager.Instance.Log("SMS sent to " + ((BaseMessage)message).UserMobileNo);
        }
        public void sendSMS(string number, string messageText)
        {
            if (!number.Contains('+'))
                number = "+91" + number;
            ///Send SMS using C#
            //Your authentication key
            string authKey = "135582A1Idd8pCx58704cf7";
            //Multiple mobiles numbers separated by comma
            string mobileNumber = number;
            //Sender ID,While using route4 sender id should be 6 characters long.
            string senderId = "MEDLYT";
            //Your message to send, Add URL encoding here.
            string message = HttpUtility.UrlEncode(messageText);

            //Prepare you post parameters
            StringBuilder sbPostData = new StringBuilder();
            sbPostData.AppendFormat("authkey={0}", authKey);
            sbPostData.AppendFormat("&mobiles={0}", mobileNumber);
            sbPostData.AppendFormat("&message={0}", messageText);
            sbPostData.AppendFormat("&sender={0}", senderId);
            sbPostData.AppendFormat("&route={0}", "4");
            if (number.StartsWith("+91"))
                sbPostData.AppendFormat("&country={0}", "91");
            else
            {
                sbPostData.AppendFormat("&country={0}", "0");
            }
            sbPostData.AppendFormat("&ignoreNdnc={0}", "1");
            sbPostData.AppendFormat("&response={0}", "json");

            try
            {
                //Call Send SMS API
                string sendSMSUri = "https://control.msg91.com/api/sendhttp.php";
                //Create HTTPWebrequest
                HttpWebRequest httpWReq = (HttpWebRequest)WebRequest.Create(sendSMSUri);
                //Prepare and Add URL Encoded data
                UTF8Encoding encoding = new UTF8Encoding();
                byte[] data = encoding.GetBytes(sbPostData.ToString());
                //Specify post method
                httpWReq.Method = "POST";
                httpWReq.ContentType = "application/x-www-form-urlencoded";
                httpWReq.ContentLength = data.Length;
                using (Stream stream = httpWReq.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
                //Get the response
                HttpWebResponse response = (HttpWebResponse)httpWReq.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string responseString = reader.ReadToEnd();

                //Close the response
                reader.Close();
                response.Close();
            }
            catch (SystemException ex)
            {
                LogManager.Instance.Log(ex.Message);
            }
            catch (Exception ex)
            {
                LogManager.Instance.Log(ex.Message);
            }

        }
        private string GetMessage(BaseMessage sMSMessage)
        {
            string Message = string.Empty;
            switch (sMSMessage.MessageType)
            {
                case MessageType.User_OTP:
                    Message = Common.ReplacePlaceHolders("Your OTP to register on MedLyte is [OTP]", sMSMessage);
                    break;
                case MessageType.User_Registration:
                    Message = Common.ReplacePlaceHolders("Dear [PatientName],Welcome to MedLyte.You can now consult your favourite doctor on MedLyte.To add your doctor on MedLyte, use the unique Doctor Code given by your doctor. ", sMSMessage);
                    break;
                case MessageType.Forgot_Password:
                    Message = Common.ReplacePlaceHolders("Your password has been reset please login using your new password [Password]. ", sMSMessage);
                    break;
                case MessageType.Consultation_User:
                    Message = Common.ReplacePlaceHolders("Your consultation request has been successfully sent to Dr [DoctorName]. You will be notified once your doctor accepts your consultation request. For any queries, contact us at support@medlyte.com. ", sMSMessage);
                    break;
                case MessageType.Booking_Accepted_Patient:
                    Message = Common.ReplacePlaceHolders("Dr [DoctorName] has now accept your consultation request. Please log in to your account on MedLyte to consult with the doctor. In case of call/video consultation, the doctor will call you at the time of appointment. For any queries, reach out to us at support@medlyte.com", sMSMessage);
                    break;
                case MessageType.Consultation_Doctor:
                    Message = Common.ReplacePlaceHolders("You have received a new consultation request in your virtual clinic. Please log in to MedLyte to accept the consultation request.If you have any queries, send us an email to support@medlyte.com", sMSMessage);
                    break;
                default:
                    break;
            }
            return Message;
        }
    }
}
