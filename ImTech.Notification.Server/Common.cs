using ImTech.Notification.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImTech.Notification.Server
{
    public class Common
    {
        public static DateTime CurrentTime
        {
            get
            {
                return DateTime.Now;
            }
        }

        public static string ReplacePlaceHolders(string template, BaseMessage messageData)
        {
            string retMessage = string.Empty;
            if (messageData is OTP)
                retMessage = template.Replace("[OTP]", ((OTP)messageData).OTPCode);
            if (messageData is UserRegistration)
                retMessage = template.Replace("[PatientName]", ((UserRegistration)messageData).UserName);
            if (messageData is ForgotPassword)
                retMessage = template.Replace("[Password]", ((ForgotPassword)messageData).Password);
            if (messageData is ConsultationDetails)
                retMessage = template.Replace("[PatientName]", ((ConsultationDetails)messageData).PatientName)
                    .Replace("[ConsultationType]", ((ConsultationDetails)messageData).ConsultationType)
                    .Replace("[ConsultationTime]", ((ConsultationDetails)messageData).ConsultationTime)
                    .Replace("[ConsultationDay]", ((ConsultationDetails)messageData).ConsultationDay)
                    .Replace("[Amount]", ((ConsultationDetails)messageData).ConsultationCharges.ToString("0.00"))
                    .Replace("[DoctorName]", ((ConsultationDetails)messageData).DoctorName);


            return retMessage;
        }
        public static string GetNotificationMessage(BaseMessage sMSMessage)
        {
            string Message = string.Empty;
            switch (sMSMessage.MessageType)
            {
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
