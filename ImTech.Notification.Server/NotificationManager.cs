using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImTech.Notification.Messages;

namespace ImTech.Notification.Server
{
    public class NotificationManager
    {
        private AutoQueue<BaseMessage> _emailAutoQ;
        private AutoQueue<BaseMessage> _smsAutoQ;
        private AutoQueue<BaseMessage> _gcmAutoQ;
        private AutoQueue<BaseMessage> _iosAutoQ;
        private INotifierQueueManager _queueManager;

        private IMessageProcessor _emailProcessor;
        private IMessageProcessor _smsProcessor;
        private IMessageProcessor _gcmProcessor;
        private IMessageProcessor _iosProcessor;

        public NotificationManager(IMessageProcessor emailProcessor, IMessageProcessor smsProcessor, IMessageProcessor gcmProcessor, IMessageProcessor iosProcessor)
        {
            _emailProcessor = emailProcessor;
            _smsProcessor = smsProcessor;
            _gcmProcessor = gcmProcessor;
            _iosProcessor = iosProcessor;
            LogManager.Instance.Log("Initializing Message AutoQues...", true);
            _emailAutoQ = new AutoQueue<BaseMessage>();
            _smsAutoQ = new AutoQueue<BaseMessage>();
            _gcmAutoQ = new AutoQueue<BaseMessage>();
            _iosAutoQ = new AutoQueue<BaseMessage>();
            _emailAutoQ.OnReceive += OnEmailReceive;
            _smsAutoQ.OnReceive += OnSmsReceive;
            _gcmAutoQ.OnReceive += OnGCMReceived;
            _iosAutoQ.OnReceive += OnIOSReceived;
            LogManager.Instance.Log("AutoQues Initialized...", true);
            _queueManager = QueueManagerFactory.GetQueueManager();
            _queueManager.OnQueueReceive += ProcessQueueReceived;
        }


        public void Start()
        {
            _emailAutoQ.Start();
            LogManager.Instance.Log("Email Queue Started", true);
            _smsAutoQ.Start();
            LogManager.Instance.Log("SMS Queue Started", true);
            _queueManager.Start();
        }
        private void ProcessQueueReceived(BaseMessage message)
        {
            if (message.DelMedium.Contains('E'))
                _emailAutoQ.Enqueue(message);
            if (message.DelMedium.Contains('S'))
                _smsAutoQ.Enqueue(message);
            if (message.DelMedium.Contains('A'))
                _gcmAutoQ.Enqueue(message);
            if (message.DelMedium.Contains('I'))
                _gcmAutoQ.Enqueue(message);
        }
        private void OnIOSReceived(object message)
        {
            _iosProcessor.Process((BaseMessage)message);
        }
        private void OnGCMReceived(object message)
        {
            _gcmProcessor.Process((BaseMessage)message);
        }
        private void OnEmailReceive(object emailmessage)
        {
            _emailProcessor.Process((BaseMessage)emailmessage);
        }

        private void OnSmsReceive(object smsmessage)
        {
            _smsProcessor.Process((BaseMessage)smsmessage);
        }

    }
}
