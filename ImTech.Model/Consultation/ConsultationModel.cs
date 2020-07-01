using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImTech.Model
{
    public class ConsultationModel : BaseModel
    {
        public ConsultationModel()
        {
            ConsultationTime = new InternalDateTime();
        }
        public string Description { get; set; }

        public InternalDateTime ConsultationTime { get; set; }

        public Int64 PatientId { get; set; }

        public Int64 DoctorId { get; set; }

        public int ConsultationModeID { get; set; }

        public int ConsultationStatusID { get; set; }

        public string ConsultationMode { get; set; }

        public string ConsultationStatus { get; set; }

        public DoctorModel Doctor { get; set; }

        public PatientDetail Patient { get; set; }

        public string PrescriptionList { get; set; }

        public string CaseNotesList { get; set; }

        public IEnumerable<PrescreptionModel> Prescriptions { get; set; }

        public IEnumerable<CaseNoteModel> CaseNotes { get; set; }

        public decimal AmountCharged { get; set; }

    }
}
