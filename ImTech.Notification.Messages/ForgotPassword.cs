﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImTech.Notification.Messages
{
   public class ForgotPassword : BaseMessage
    {
        public string Password { get; set; }
    }
}
