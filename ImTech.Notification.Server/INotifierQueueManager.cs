using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImTech.Notification.Messages;
namespace ImTech.Notification.Server
{
    public delegate void OnQueueReceive(BaseMessage objMessage);
   public interface INotifierQueueManager
    {
        void Start();
        void Restart();
        OnQueueReceive OnQueueReceive { get; set; }
    }
}
