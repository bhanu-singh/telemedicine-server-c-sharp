using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImTech.Notification.Server
{
    public class LogManager
    {
        AutoQueue<object> _loggerQ;

        public static LogManager Instance = new LogManager();
        public LogManager()
        {
            _loggerQ = new AutoQueue<object>();
            _loggerQ.OnReceive += OnLogReceived;
            _loggerQ.Start();
        }

        private void OnLogReceived(object message)
        {
            try
            {
                FileWriter.LogFileWrite(message.ToString());
            }
            catch
            {

            }
            finally
            {

            }
        }

        public void Log(object message, bool Isconsole = true)
        {
            try
            {
                if (Isconsole)
                    Console.WriteLine(Common.CurrentTime + "  :" + Convert.ToString(message));
                _loggerQ.Enqueue(message);
            }
            catch
            {
            }
            finally
            {
                
            }
        }

    }
}
