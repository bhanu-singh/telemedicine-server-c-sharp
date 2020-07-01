using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImTech.Notification.Messages
{
    public class Item
    {
        public string Description { get; set; }

        public string Quantity { get; set; }

        public string Price { get; set; }

        public string Tax { get; set; }

        public string Amount { get; set; }
    }

    public class MessageData
    {
        public MessageData()
        {
            InvoiceItems = new List<Item>();
        }
        public string CustomerName { get; set; }
        public string CustomerContactNo { get; set; }
        public string CustomerAddress { get; set; }

        public string LawyerName { get; set; }
        public string LawyerContactNo { get; set; }

        public string BookingType { get; set; }
        public string BookingTime { get; set; }
        public string BookingDuration { get; set; }
        public string BookingAddress { get; set; }


        public string LeadRef { get; set; }
        public string Question { get; set; }
        public string QuestionAskedDate { get; set; }
        public string QuestionPaymentOption { get; set; }
        public string QuestionService { get; set; }
        public string QuestionLegalArea { get; set; }

        public string DefaultPassword { get; set; }

        public string ResetPasswordURL { get; set; }


        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public List<Item> InvoiceItems { get; set; }
        public string InvoiceSubTotal { get; set; }
        public string InvoiceServiceTax { get; set; }
        public string InvoiceTotalAmount { get; set; }
    }
}
