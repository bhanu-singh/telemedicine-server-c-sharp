using ImTech.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImTech.Service.ViewModels
{
    public class ConsultationViewListModel : ViewModelBase
    {
        public ConsultationViewListModel()
        {
            ConsultationTime = new InternalDateTime();
        }
        public string Description { get; set; }

        public InternalDateTime ConsultationTime { get; set; }

        public Int64 PatientId { get; set; }

        public string PatientName { get; set; }

        public string DoctorFirstName { get; set; }

        public string DoctorLastName { get; set; }

        public Int64 DoctorId { get; set; }

        public int ConsultationModeID { get; set; }

        public int ConsultationStatusID { get; set; }

        public string ConsultationMode { get; set; }

        public string ConsultationStatus { get; set; }

        public List<string> PrescriptionList { get; set; }

        public List<string> CaseNoteList { get; set; }
    }
}