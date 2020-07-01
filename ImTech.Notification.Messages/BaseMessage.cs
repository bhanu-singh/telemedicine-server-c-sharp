using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImTech.Notification.Messages
{
 public class BaseMessage
    {
        public string DelMedium { get; set; }

        public MessageType MessageType { get; set; }

        public string UserMobileNo { get; set; }

        public string UserEmail { get; set; }

        public string UserDeviceToken { get; set; }

        public MessageData Data { get; set; }



    }
}
