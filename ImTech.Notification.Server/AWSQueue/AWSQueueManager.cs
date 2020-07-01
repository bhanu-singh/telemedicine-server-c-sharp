using System;
using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;
using Newtonsoft.Json;

namespace ImTech.Notification.Server
{

    public class AWSQueueManager : INotifierQueueManager
    {
        IAmazonSQS sqs;

        public OnQueueReceive OnQueueReceive
        {
            get
            ;
            set
           ;
        }

        public AWSQueueManager()
        {
            LogManager.Instance.Log("Initializing AWS Queue...", true);
            sqs = new AmazonSQSClient(ConfigurationManager.AWSAccessKey, ConfigurationManager.AWSSecrateAccessKey, ConfigurationManager.AWSRegion);
            LogManager.Instance.Log("AWS Queue Initialized...", true);

        }

        public void Start()
        {
            LogManager.Instance.Log("Starting AWS Queue Listener", true);
            while (true)
            {
                ProcessMessage();
            }
        }
        private void ProcessMessage()
        {
            try
            {
                //Receiving a message
                ReceiveMessageRequest receiveMessageRequest = new ReceiveMessageRequest();
                receiveMessageRequest.QueueUrl = ConfigurationManager.QueueURL;
                ReceiveMessageResponse receiveMessageResponse = sqs.ReceiveMessage(receiveMessageRequest);
                foreach (Amazon.SQS.Model.Message message in receiveMessageResponse.Messages)
                {
                    try
                    {
                        var mlmessage = Map(message);
                        if (mlmessage.DelMedium != null)
                        {
                            LogManager.Instance.Log("Message Received of Type --> " + Convert.ToString(mlmessage.MessageType), true);
                            LogManager.Instance.Log("Message Body --> " + message.Body, false);
                            OnQueueReceive(mlmessage);

                            //delete
                            DeleteMessageRequest deleteRequest = new DeleteMessageRequest();
                            deleteRequest.QueueUrl = ConfigurationManager.QueueURL;
                            deleteRequest.ReceiptHandle = message.ReceiptHandle;
                            if (!ConfigurationManager.Test)
                                sqs.DeleteMessage(deleteRequest);

                        }
                    }
                    catch (Exception ex)
                    {
                        LogManager.Instance.Log(ex.Message);
                    }


                }
            }
            catch (Exception ex)
            {
                LogManager.Instance.Log(ex.Message);
            }
        }

        private ImTech.Notification.Messages.BaseMessage Map(Amazon.SQS.Model.Message message)
        {
            ImTech.Notification.Messages.BaseMessage objRet = null;
            objRet = JsonConvert.DeserializeObject<ImTech.Notification.Messages.BaseMessage>(message.Body);
            objRet = Map(message.Body, objRet.MessageType);
            return objRet;
        }
        private ImTech.Notification.Messages.BaseMessage Map(string Json, ImTech.Notification.Messages.MessageType type)
        {
            switch (type)
            {
                case ImTech.Notification.Messages.MessageType.User_OTP:
                    {
                        return JsonConvert.DeserializeObject<ImTech.Notification.Messages.OTP>(Json);
                    }
                case ImTech.Notification.Messages.MessageType.Consultation_User:
                    {
                        return JsonConvert.DeserializeObject<ImTech.Notification.Messages.ConsultationDetails>(Json);
                    }
                case ImTech.Notification.Messages.MessageType.Forgot_Password:
                    {
                        return JsonConvert.DeserializeObject<ImTech.Notification.Messages.ForgotPassword>(Json);
                    }
                case ImTech.Notification.Messages.MessageType.User_Registration:
                    {
                        return JsonConvert.DeserializeObject<ImTech.Notification.Messages.UserRegistration>(Json);
                    }
                case ImTech.Notification.Messages.MessageType.Booking_Accepted_Patient:
                    {
                        return JsonConvert.DeserializeObject<ImTech.Notification.Messages.ConsultationDetails>(Json);
                    }
                case ImTech.Notification.Messages.MessageType.Booking_Cancelled_Patient:
                    {
                        return JsonConvert.DeserializeObject<ImTech.Notification.Messages.ConsultationDetails>(Json);
                    }
                case ImTech.Notification.Messages.MessageType.Booking_End_Patient:
                    {
                        return JsonConvert.DeserializeObject<ImTech.Notification.Messages.ConsultationDetails>(Json);
                    }
                case ImTech.Notification.Messages.MessageType.Booking_End_Doctor:
                    {
                        return JsonConvert.DeserializeObject<ImTech.Notification.Messages.ConsultationDetails>(Json);
                    }
                case ImTech.Notification.Messages.MessageType.Booking_Rejected_Patient:
                    {
                        return JsonConvert.DeserializeObject<ImTech.Notification.Messages.ConsultationDetails>(Json);
                    }
                case ImTech.Notification.Messages.MessageType.Consultation_Doctor:
                    {
                        return JsonConvert.DeserializeObject<ImTech.Notification.Messages.ConsultationDetails>(Json);
                    }
                case ImTech.Notification.Messages.MessageType.Booking_Cancelled_Doctor:
                    {
                        return JsonConvert.DeserializeObject<ImTech.Notification.Messages.ConsultationDetails>(Json);
                    }
                
                default:
                    return new Messages.BaseMessage();
            }
        }

        public void Restart()
        {
            while (true)
            {
                ProcessMessage();
            }
        }
    }
}
