using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImTech.Notification.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            LogManager.Instance.Log("Starting Up....................");
            LogManager.Instance.Log("Receiving Queue :" + ConfigurationManager.QueueURL);
            LogManager.Instance.Log("Receiving Region :" + ConfigurationManager.AWSRegion);
            ConfigurationManager.LogFolderPath = System.IO.Directory.GetCurrentDirectory() + "\\Log\\";
            LogManager.Instance.Log("Logging Path Congigured....");
            LogManager.Instance.Log("Initializing Processors....");
            IMessageProcessor emailProcessor = new EmailProcessor(), smsProcessor = new SMSProcessor(),gcmProcessor=new GCMProcessor(),iosProcessor=new IOSProcessor();

            NotificationManager notificationManager = new NotificationManager(emailProcessor, smsProcessor, gcmProcessor, iosProcessor);

            notificationManager.Start();
            LogManager.Instance.Log("Notifications System Started Successfully...");
        }
    }
}
