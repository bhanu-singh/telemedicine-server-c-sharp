using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImTech.Notification.Messages
{
   public class ConsultationDetails : BaseMessage
    {
        public string PatientName { get; set; }

        public string DoctorName { get; set; }

        public string ConsultationType { get; set; }

        public decimal ConsultationCharges { get; set; }

        public decimal DiscountAmount { get; set; }


        public string ConsultationTime { get; set; }

        public string ConsultationDay { get; set; }



    }
}
