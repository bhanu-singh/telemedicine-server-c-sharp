using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImTech.Notification.Messages;
using System.Net.Mail;
using System.IO;
namespace ImTech.Notification.Server
{
    public class EmailProcessor : MessageProcessor
    {
        Dictionary<MessageType, string> subjectNames = new Dictionary<MessageType, string>();

        public EmailProcessor()
        {
            subjectNames.Add(MessageType.User_OTP, "MedLyte OTP.");
            subjectNames.Add(MessageType.User_Registration, "Welcome to MedLyte.");
            subjectNames.Add(MessageType.Consultation_User, "Consultation Details.");
            subjectNames.Add(MessageType.Consultation_Doctor, "Consultation Details.");
            subjectNames.Add(MessageType.Forgot_Password, "Password.");
            subjectNames.Add(MessageType.Booking_Accepted_Patient, "Consultation Details.");
            subjectNames.Add(MessageType.Booking_End_Patient, "Consultation Details.");
            subjectNames.Add(MessageType.Booking_End_Doctor, "Consultation Details.");
            subjectNames.Add(MessageType.Booking_Rejected_Patient, "Consultation Details.");
            subjectNames.Add(MessageType.Booking_Cancelled_Patient, "Consultation Details.");
            subjectNames.Add(MessageType.Booking_Cancelled_Doctor, "Consultation Details.");
        }

        private string ReadTemplate(string templateName)
        {

            var path = string.Format("{0}/EmailTemplate/{1}.html", AppDomain.CurrentDomain.BaseDirectory, templateName);

            return File.ReadAllText(path);
        }

        private string PrepareInvoiceItem(MessageData messageData)
        {
            StringBuilder invoiceBuilder = new StringBuilder();
            invoiceBuilder.Append("");

            invoiceBuilder.Append("<table  border='1' cellspacing='1' cellpadding='1' >");
            invoiceBuilder.Append("<thead><tr><td class='bodyContent' >Description</td><td class='bodyContent'>Quantity</td><td class='bodyContent'>Unit Price</td><td class='bodyContent'>Tax</td><td class='bodyContent'>Amount</td></tr></thead>");
            invoiceBuilder.Append("<tbody>   ");

            foreach (var item in messageData.InvoiceItems)
            {
                invoiceBuilder.Append("<tr>");
                invoiceBuilder.Append(string.Format("<td class='bodyContent'>{0}</td>", item.Description));
                invoiceBuilder.Append(string.Format("<td class='bodyContent'>{0}</td>", item.Quantity));
                invoiceBuilder.Append(string.Format("<td class='bodyContent'>{0}</td>", item.Price));
                invoiceBuilder.Append(string.Format("<td class='bodyContent'>{0}</td>", messageData.InvoiceServiceTax));
                invoiceBuilder.Append(string.Format("<td class='bodyContent'>{0}</td>", item.Amount));
                invoiceBuilder.Append("</tr>");
            }

            invoiceBuilder.Append("<tr>");
            invoiceBuilder.Append("<td class='bodyContent' ></td>");
            invoiceBuilder.Append("<td class='bodyContent'></td>");
            invoiceBuilder.Append("<td class='bodyContent'></td>");
            invoiceBuilder.Append("<td class='bodyContent'>Subtotal</td>");
            invoiceBuilder.Append(string.Format("<td class='bodyContent'>{0}</td>", messageData.InvoiceSubTotal));
            invoiceBuilder.Append("</tr>");
            invoiceBuilder.Append("<tr>");
            invoiceBuilder.Append("<td class='bodyContent' ></td>");
            invoiceBuilder.Append("<td class='bodyContent'></td>");
            invoiceBuilder.Append("<td class='bodyContent'></td>");
            invoiceBuilder.Append("<td class='bodyContent'>Service Tax (" + messageData.InvoiceServiceTax + ")</td>");
            invoiceBuilder.Append(string.Format("<td class='bodyContent'>{0}</td>", (Convert.ToDouble(messageData.InvoiceTotalAmount) - Convert.ToDouble(messageData.InvoiceSubTotal)).ToString("0.00")));
            invoiceBuilder.Append("</tr>    ");
            invoiceBuilder.Append("<tr>");
            invoiceBuilder.Append("<td class='bodyContent' ></td>");
            invoiceBuilder.Append("<td class='bodyContent'></td>");
            invoiceBuilder.Append("<td class='bodyContent'></td>");
            invoiceBuilder.Append("<td class='bodyContent'>Total</td>");
            invoiceBuilder.Append(string.Format("<td class='bodyContent'>{0}</td>", messageData.InvoiceTotalAmount));
            invoiceBuilder.Append("</tr>");
            invoiceBuilder.Append("</tbody></table>");

            return invoiceBuilder.ToString();
        }
        
        private EmailMessage GetProcessedMessage(BaseMessage message)
        {
            var template = ReadTemplate(message.MessageType.ToString());

            var em = new EmailMessage();
            em.UserEmail = message.UserEmail;
            em.Subject = subjectNames[message.MessageType];
            em.Content = Common.ReplacePlaceHolders(template, message);

            return em;
        }

        private bool SendEmail(EmailMessage message)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(ConfigurationManager.SMTPServer);

                mail.From = new MailAddress(ConfigurationManager.SMTPFromAddress);
                if (!ConfigurationManager.Test)
                {
                    mail.To.Add(message.UserEmail);
                }
                else
                {
                    mail.To.Add(ConfigurationManager.BCC);
                }
                if (!string.IsNullOrEmpty(message.CCAddress))
                    mail.CC.Add(message.CCAddress);
                if (!string.IsNullOrEmpty(ConfigurationManager.BCC))
                    mail.Bcc.Add(ConfigurationManager.BCC);

                mail.Subject = message.Subject;
                mail.Body = message.Content;
                mail.IsBodyHtml = true;

                #region attachment
                //if (message.MessageType == MessageType.Booking_Lawyer || message.MessageType == MessageType.Booking_LawyerReschedule_User || message.MessageType == MessageType.Booking_User || message.MessageType == MessageType.Booking_UserReschedule_Lawyer)
                //{
                //    //INITIALIZING MEETING DETAILS

                //    string schLocation = message.Data.BookingType;
                //    string schSubject = "Booking Schedule";
                //    string schDescription = "Booking Schedule";
                //    System.DateTime schBeginDate = Convert.ToDateTime(message.Data.BookingTime);
                //    System.DateTime schEndDate = schBeginDate.AddMinutes(Convert.ToInt32(message.Data.BookingDuration));

                //    //PUTTING THE MEETING DETAILS INTO AN ARRAY OF STRING

                //    String[] contents = { "BEGIN:VCALENDAR",
                //              "PRODID:-//Flo Inc.//FloSoft//EN",
                //              "BEGIN:VEVENT",
                //              "DTSTART:" + schBeginDate.ToUniversalTime().ToString("yyyyMMdd\\THHmmss\\Z"),
                //              "DTEND:" + schEndDate.ToUniversalTime().ToString("yyyyMMdd\\THHmmss\\Z"),
                //              "LOCATION:" + schLocation,
                //         "DESCRIPTION;ENCODING=QUOTED-PRINTABLE:" + schDescription,
                //              "SUMMARY:" + schSubject, "PRIORITY:3",
                //         "END:VEVENT", "END:VCALENDAR" };

                //    /*THE METHOD 'WriteAllLines' CREATES A FILE IN THE SPECIFIED PATH WITH 
                //   THE SPECIFIED NAME,WRITES THE ARRAY OF CONTENTS INTO THE FILE AND CLOSES THE
                //    FILE.SUPPOSE THE FILE ALREADY EXISTS IN THE SPECIFIED LOCATION,THE CONTENTS 
                //   IN THE FILE ARE OVERWRITTEN*/
                //    var gid = Guid.NewGuid();
                //    System.IO.File.WriteAllLines("Temp\\" + gid + "Meeting.ics", contents);

                //    Attachment mailAttachment = new Attachment("Temp\\" + gid + "Meeting.ics");
                //                       //ADD THE ATTACHMENT TO THE EMAIL

                //    mail.Attachments.Add(mailAttachment);

                //}

                #endregion

                SmtpServer.Port = ConfigurationManager.SMTPPort;
                SmtpServer.Credentials = new System.Net.NetworkCredential(ConfigurationManager.SMTPUserNamne, ConfigurationManager.SMTPPassword);
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                LogManager.Instance.Log(ex.Message);
            }
            return false;
        }

        public override void Process(object message)
        {
            try
            {
                var emailMessage = (BaseMessage)message;

                var processedMessage = GetProcessedMessage(emailMessage);
                LogManager.Instance.Log("Email Message Formatting Completed..");
                if (SendEmail(processedMessage))
                {
                    LogManager.Instance.Log("Email Message Sent..");
                }
            }
            catch (Exception ex)
            {
                LogManager.Instance.Log(ex.Message);
            }
        }
    }
}
