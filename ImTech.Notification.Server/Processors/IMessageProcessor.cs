using ImTech.Notification.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImTech.Notification.Server
{
    public interface IMessageProcessor : IDisposable
    {
        void Process(object message);
    }
}
