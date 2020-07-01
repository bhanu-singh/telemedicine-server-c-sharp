using ImTech.Notification.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImTech.Notification.Server
{
    public abstract class MessageProcessor : IMessageProcessor
    {
        public virtual void Process(object message)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
