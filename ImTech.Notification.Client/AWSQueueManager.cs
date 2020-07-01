using Amazon;
using Amazon.SQS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImTech.Notification.Messages;
using Amazon.SQS.Model;
using Newtonsoft.Json;
using System.Web;
using ImTech.DataBase;
using ImTech.Model;
namespace ImTech.Notification.Client
{
    public class AWSQueueManager
    {
        IAmazonSQS sqs;
        MessageFormatter msgFrmt;
        
        public AWSQueueManager()
        {
            
            sqs = new AmazonSQSClient(ConfigurationManager.AWSAccessKey, ConfigurationManager.AWSSecrateAccessKey, RegionEndpoint.GetBySystemName(ConfigurationManager.AWSRegion));
            msgFrmt = new MessageFormatter();
        }
        public void SendMessage(BaseModel model, MessageType messageType, List<object> additionalParameters)
        {
            ImTech.Notification.Messages.BaseMessage msgToSend = msgFrmt.FormatMessage(model, messageType, additionalParameters);
          
            SendMessageRequest smr = new SendMessageRequest(ConfigurationManager.QueueURL, JsonConvert.SerializeObject(msgToSend));
            sqs.SendMessage(smr);
        }
    }
}
