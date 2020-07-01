using ImTech.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImTech.Service.ViewModels.Doctor
{
    public class DoctorLogonViewModel : BaseModel
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string PhoneNumber { get; set; }

        public string LoginLocation { get; set; }

        public string RequestURL { get; set; }

        public InternalDateTime LoginDate { get; set; }

        public string DeviceId { get; set; }
    }
}