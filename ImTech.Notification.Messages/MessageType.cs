using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImTech.Notification.Messages
{
    public enum MessageType
    {
        User_OTP,
        User_Registration,
        Consultation_User,
        Consultation_Doctor,
        Forgot_Password,
        Booking_Accepted_Patient,
        Booking_End_Patient,
        Booking_End_Doctor,
        Booking_Rejected_Patient,
        Booking_Cancelled_Patient,
        Booking_Cancelled_Doctor

    };
}
