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
    public class IOSProcessor : MessageProcessor
    {
        public static IOSProcessor Instance = new IOSProcessor();

        public override void Process(object message)
        {
            sendMessage(((BaseMessage)message).UserMobileNo, Common.GetNotificationMessage((BaseMessage)message));
            LogManager.Instance.Log("IOS sent to " + ((BaseMessage)message).UserDeviceToken);
        }
        public void sendMessage(string number, string messageText)
        {


        }
    }
}
