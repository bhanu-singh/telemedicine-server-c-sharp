using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImTech.Service.ViewModels
{
    public class MessageViewModel : ViewModelBase
    {
        public int ConsultationId { get; set; }
        public string TextMessage { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
    }

}