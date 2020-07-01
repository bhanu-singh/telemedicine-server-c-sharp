using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImTech.Model
{
    public class PatientDetail : BaseModel
    {
        public Int64 UserID { get; set; }

        public string PatientName { get; set; }

        public Int16 PatientAge { get; set; }

        public string PatientPhone { get; set; }

        public char PatientGender { get; set; }

        public string Complaints { get; set; }

        public int ConsultationType { get; set; }

        public Int64 DoctorId { get; set; }

        public string FileId { get; set; }

        public string Files { get; set; }

        public Int16 Case { get; set; }

        public Int16 PreferredTime { get; set; }

    }
}
