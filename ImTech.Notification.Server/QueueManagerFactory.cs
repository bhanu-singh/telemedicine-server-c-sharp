namespace ImTech.Notification.Server
{
    public class QueueManagerFactory
    {
        public static INotifierQueueManager GetQueueManager()
        {
            return new AWSQueueManager();
        }
    }
}
