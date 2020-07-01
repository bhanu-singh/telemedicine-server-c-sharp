using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImTech.DataBase;
using ImTech.Model;
using System.Net.Mail;

namespace ImTech.DataServices
{
    public class Logger : ILogger
    {
        IDataBaseService dataBaseService;

        public Logger(IDataBaseService databaseService)
        {
            this.dataBaseService = databaseService;
        }

        public void LogInfo(LogMessage logMessage)
        {
            throw new NotImplementedException();
        }

        public void LogWarning(LogMessage logMessage)
        {
            throw new NotImplementedException();
        }

        public void LogError(LogMessage logMessage)
        {

            var pShortMessage = new Parameter("@Log", logMessage.Summary);

            var pLongMessage = new Parameter("@StackTrace", logMessage.Exception);

            //   this.LogEmail(logMessage);
            var r = dataBaseService.ExecuteScalar(StoredProcedures.InsertLog
                   , DBCommandType.Procedure
                   , pShortMessage
                   , pLongMessage
                   );

        }

        public void LogEmail(LogMessage logMessage)
        {
            //ConfigurationManager config = new ConfigurationManager();
            //SmtpClient client = new SmtpClient();
            //client.Port = ConfigurationManager.LogMailPort;
            //client.Host = ConfigurationManager.LogMailHost;
            //client.EnableSsl = true;
            //client.Timeout = 10000;
            //client.DeliveryMethod = SmtpDeliveryMethod.Network;

            //MailMessage mm = new MailMessage();
            //if (!string.IsNullOrEmpty(ConfigurationManager.LogMailTo))
            //{
            //    string[] mailTo = ConfigurationManager.LogMailTo.Split(';');
            //    for (int count = 0; count < mailTo.Length; count++)
            //    {
            //        MailAddress addrTo = new MailAddress(mailTo[count]);
            //        mm.To.Add(addrTo);
            //    }
            //}
            //if (!string.IsNullOrEmpty(ConfigurationManager.LogMailCC))
            //{
            //    string[] mailCC = ConfigurationManager.LogMailCC.Split(';');
            //    for (int count = 0; count < mailCC.Length; count++)
            //    {
            //        MailAddress addrCC = new MailAddress(mailCC[count]);
            //        mm.CC.Add(addrCC);
            //    }
            //}

            //mm.BodyEncoding = UTF8Encoding.UTF8;

            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("Application: {0}\r\n", logMessage.Application));
            sb.Append(string.Format("IP: {0}\r\n", logMessage.IP));
            sb.Append(string.Format("Exception: {0}\r\n", logMessage.Exception));




            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient(ConfigurationManager.SMTPServer);

            mail.From = new MailAddress(ConfigurationManager.SMTPFromAddress);
            mail.To.Add(ConfigurationManager.LogMailTo);
            SmtpServer.Port = ConfigurationManager.SMTPPort;
            SmtpServer.Credentials = new System.Net.NetworkCredential(ConfigurationManager.SMTPUserNamne, ConfigurationManager.SMTPPassword);
            SmtpServer.EnableSsl = true;

            SmtpServer.Send(mail);

            mail.Subject = ConfigurationManager.LogMailSubject;
            mail.Body = sb.ToString();
            mail.IsBodyHtml = true;

        }
    }
}
