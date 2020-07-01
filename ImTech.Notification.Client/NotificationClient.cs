using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Web.Hosting;
using ImTech.Model;
using ImTech.Notification.Messages;

namespace ImTech.Notification.Client
{
    public class NotificationClient
    {
        public static NotificationClient Instance = new NotificationClient();
        private AWSQueueManager aWSQueueManager;
        public NotificationClient()
        {
            aWSQueueManager = new AWSQueueManager();
        }
        public List<object> GetAdditionalInfoInstance()
        {
            return new List<object>();
        }
        public void SendMessage(BaseModel model=null, MessageType messageType=Messages.MessageType.User_OTP, List<object> additionalParameters = null)
        {
            HostingEnvironment.QueueBackgroundWorkItem(ct => aWSQueueManager.SendMessage(model, messageType, additionalParameters));
            //aWSQueueManager.SendMessage(model, messageType);
        }
    }
}
