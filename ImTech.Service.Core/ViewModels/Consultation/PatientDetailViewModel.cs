using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImTech.Service.ViewModels
{
    public class PatientDetailViewModel : ViewModelBase
    {
        public PatientDetailViewModel()
        {
            Files = new List<string>();
        }
        public Int64 UserID { get; set; }

        public string PatientName { get; set; }

        public Int16 PatientAge { get; set; }

        public string PatientPhone { get; set; }

        public Int64 DoctorId { get; set; }

        public char PatientGender { get; set; }

        public string Complaints { get; set; }

        public int ConsultationType { get; set; }

        public string FileId { get; set; }

        public List<string> Files { get; set; }

        public Int16 Case { get; set; }

        public Int16 PreferredTime { get; set; }

    }

    public enum CaseOption
    {
        New = 1,
        FollowUp = 2
    }
    public enum PreferredTimeOption
    {
        Morning = 1,
        Afternoon = 2,
        Evening = 3,
        Urgent = 4
    }
}