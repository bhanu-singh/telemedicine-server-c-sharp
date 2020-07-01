using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImTech.Notification.Messages
{
    public class EmailMessage : BaseMessage
    {
        public string CCAddress { get; set; }

        public string BCCAddress { get; set; }

        public string Subject { get; set; }

       

        public string Content { get; set; }


    }
}
