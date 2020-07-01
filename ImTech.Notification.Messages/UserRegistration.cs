using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImTech.Notification.Messages
{
   public class UserRegistration:BaseMessage
    {
        public string UserName { get; set; }
    }
}
