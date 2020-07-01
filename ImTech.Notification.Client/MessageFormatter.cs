using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImTech.Notification.Messages;
using System.Web;
using ImTech.Model;

namespace ImTech.Notification.Client
{
    public class MessageFormatter
    {
        public ImTech.Notification.Messages.BaseMessage FormatMessage(BaseModel model, ImTech.Notification.Messages.MessageType messageType, List<object> additionalParameters)
        {
            BaseMessage message = null;
            if (messageType == MessageType.User_OTP)
            {
                message = new OTP();
                message.DelMedium = "SE";
                //if (model.From == (int)DeviceTypes.User_Android)
                //    message.DelMedium += "A";
                //if (model.From == (int)DeviceTypes.User_IOS)
                //    message.DelMedium += "I";
                message.UserEmail = Convert.ToString(additionalParameters[0]);
                message.UserMobileNo = Convert.ToString(additionalParameters[1]);
                ((OTP)message).OTPCode = Convert.ToString(additionalParameters[2]);
                message.UserDeviceToken = Convert.ToString(additionalParameters[3]);
            }
            if (messageType == MessageType.User_Registration)
            {
                message = new UserRegistration();
                message.DelMedium = "SE";
                message.UserEmail = Convert.ToString(additionalParameters[0]);
                message.UserMobileNo = Convert.ToString(additionalParameters[1]);
                ((UserRegistration)message).UserName = Convert.ToString(additionalParameters[2]);
            }
            if (messageType == MessageType.Forgot_Password)
            {
                message = new ForgotPassword();
                message.DelMedium = "S";
                message.UserEmail = Convert.ToString(additionalParameters[0]);
                message.UserMobileNo = Convert.ToString(additionalParameters[1]);
                ((ForgotPassword)message).Password = Convert.ToString(additionalParameters[2]);
            }
            if (messageType == MessageType.Consultation_User)
            {
                message = new ConsultationDetails();
                message.DelMedium = "SE";
                message.UserEmail = Convert.ToString(additionalParameters[0]);
                message.UserMobileNo = Convert.ToString(additionalParameters[1]);
                ((ConsultationDetails)message).DoctorName = Convert.ToString(additionalParameters[5]);
                ((ConsultationDetails)message).PatientName = Convert.ToString(additionalParameters[4]);
                ((ConsultationDetails)message).ConsultationType = Convert.ToString(additionalParameters[7]);
                ((ConsultationDetails)message).ConsultationCharges = Convert.ToDecimal(additionalParameters[6]);
            }
            if (messageType == MessageType.Consultation_Doctor)
            {
                message = new ConsultationDetails();
                message.DelMedium = "SE";
                message.UserEmail = Convert.ToString(additionalParameters[2]);
                message.UserMobileNo = Convert.ToString(additionalParameters[3]);
                ((ConsultationDetails)message).DoctorName = Convert.ToString(additionalParameters[5]);
                ((ConsultationDetails)message).PatientName = Convert.ToString(additionalParameters[4]);
                ((ConsultationDetails)message).ConsultationType = Convert.ToString(additionalParameters[7]);
                ((ConsultationDetails)message).ConsultationCharges = Convert.ToDecimal(additionalParameters[6]);
            }
            if (messageType == MessageType.Booking_Accepted_Patient)
            {
                message = new ConsultationDetails();
                message.DelMedium = "SE";
                message.UserEmail = Convert.ToString(additionalParameters[0]);
                message.UserMobileNo = Convert.ToString(additionalParameters[1]);
                ((ConsultationDetails)message).DoctorName = Convert.ToString(additionalParameters[5]);
                ((ConsultationDetails)message).PatientName = Convert.ToString(additionalParameters[4]);
                ((ConsultationDetails)message).ConsultationType = Convert.ToString(additionalParameters[7]);
                ((ConsultationDetails)message).ConsultationCharges = Convert.ToDecimal(additionalParameters[6]);
                if (additionalParameters[8] != null)
                {
                    ((ConsultationDetails)message).ConsultationTime = (new InternalDateTime(Convert.ToInt64(additionalParameters[8]))).GetDateTime("MM/dd/yy H:mm", "India Standard Time");
                    ((ConsultationDetails)message).ConsultationDay = (new InternalDateTime(Convert.ToInt64(additionalParameters[8]))).GetDateTime().ToString("dddd");
                }
            }
            if (messageType == MessageType.Booking_End_Patient)
            {
                message = new ConsultationDetails();
                message.DelMedium = "SE";
                message.UserEmail = Convert.ToString(additionalParameters[0]);
                message.UserMobileNo = Convert.ToString(additionalParameters[1]);
                ((ConsultationDetails)message).DoctorName = Convert.ToString(additionalParameters[5]);
                ((ConsultationDetails)message).PatientName = Convert.ToString(additionalParameters[4]);
                ((ConsultationDetails)message).ConsultationType = Convert.ToString(additionalParameters[7]);
                ((ConsultationDetails)message).ConsultationCharges = Convert.ToDecimal(additionalParameters[6]);
                if (additionalParameters[8] != null)
                {
                    ((ConsultationDetails)message).ConsultationTime = (new InternalDateTime(Convert.ToInt64(additionalParameters[8]))).GetDateTime("MM/dd/yy H:mm", "India Standard Time");
                    ((ConsultationDetails)message).ConsultationDay = (new InternalDateTime(Convert.ToInt64(additionalParameters[8]))).GetDateTime().ToString("dddd");
                }
            }
            if (messageType == MessageType.Booking_End_Doctor)
            {
                message = new ConsultationDetails();
                message.DelMedium = "SE";
                message.UserEmail = Convert.ToString(additionalParameters[2]);
                message.UserMobileNo = Convert.ToString(additionalParameters[3]);
                ((ConsultationDetails)message).DoctorName = Convert.ToString(additionalParameters[5]);
                ((ConsultationDetails)message).PatientName = Convert.ToString(additionalParameters[4]);
                ((ConsultationDetails)message).ConsultationType = Convert.ToString(additionalParameters[7]);
                ((ConsultationDetails)message).ConsultationCharges = Convert.ToDecimal(additionalParameters[6]);
                if (additionalParameters[8] != null)
                {
                    ((ConsultationDetails)message).ConsultationTime = (new InternalDateTime(Convert.ToInt64(additionalParameters[8]))).GetDateTime("MM/dd/yy H:mm", "India Standard Time");
                    ((ConsultationDetails)message).ConsultationDay = (new InternalDateTime(Convert.ToInt64(additionalParameters[8]))).GetDateTime().ToString("dddd");
                }
            }
            if (messageType == MessageType.Booking_Cancelled_Patient)
            {
                message = new ConsultationDetails();
                message.DelMedium = "SE";
                message.UserEmail = Convert.ToString(additionalParameters[0]);
                message.UserMobileNo = Convert.ToString(additionalParameters[1]);
                ((ConsultationDetails)message).DoctorName = Convert.ToString(additionalParameters[5]);
                ((ConsultationDetails)message).PatientName = Convert.ToString(additionalParameters[4]);
                ((ConsultationDetails)message).ConsultationType = Convert.ToString(additionalParameters[7]);
                ((ConsultationDetails)message).ConsultationCharges = Convert.ToDecimal(additionalParameters[6]);
                if (additionalParameters[8] != null)
                {
                    ((ConsultationDetails)message).ConsultationTime = (new InternalDateTime(Convert.ToInt64(additionalParameters[8]))).GetDateTime("MM/dd/yy H:mm", "India Standard Time");
                    ((ConsultationDetails)message).ConsultationDay = (new InternalDateTime(Convert.ToInt64(additionalParameters[8]))).GetDateTime().ToString("dddd");
                }
            }
            if (messageType == MessageType.Booking_Cancelled_Doctor)
            {
                message = new ConsultationDetails();
                message.DelMedium = "SE";
                message.UserEmail = Convert.ToString(additionalParameters[2]);
                message.UserMobileNo = Convert.ToString(additionalParameters[3]);
                ((ConsultationDetails)message).DoctorName = Convert.ToString(additionalParameters[5]);
                ((ConsultationDetails)message).PatientName = Convert.ToString(additionalParameters[4]);
                ((ConsultationDetails)message).ConsultationType = Convert.ToString(additionalParameters[7]);
                ((ConsultationDetails)message).ConsultationCharges = Convert.ToDecimal(additionalParameters[6]);
                if (additionalParameters[8] != null)
                {
                    ((ConsultationDetails)message).ConsultationTime = (new InternalDateTime(Convert.ToInt64(additionalParameters[8]))).GetDateTime("MM/dd/yy H:mm", "India Standard Time");
                    ((ConsultationDetails)message).ConsultationDay = (new InternalDateTime(Convert.ToInt64(additionalParameters[8]))).GetDateTime().ToString("dddd");
                }
            }
            message.MessageType = messageType;
            return message;
        }
    }
}
