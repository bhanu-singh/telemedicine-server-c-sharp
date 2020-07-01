using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImTech.Service.ViewModels
{
    public class PaymentViewModel : ViewModelBase
    {
       

        public Int64 ConsultationID { get; set; }

        public int PaymentStatusID { get; set; }

        public int? PromotionalID { get; set; }

        public string GatewayResponse { get; set; }

        public decimal AmountCharged { get; set; }

        public decimal AmountActual { get; set; }

        public decimal DiscountAmount { get; set; }

        
    }
}