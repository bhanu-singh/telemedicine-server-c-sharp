using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImTech.Model
{
    public class MessageModel : BaseModel
    {
        public int ConsultationId { get; set; }
        public string TextMessage { get; set; }
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
    }
}
