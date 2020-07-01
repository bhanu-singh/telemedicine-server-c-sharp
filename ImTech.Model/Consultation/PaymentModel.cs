using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImTech.Model
{
    public class PaymentModel : BaseModel
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
