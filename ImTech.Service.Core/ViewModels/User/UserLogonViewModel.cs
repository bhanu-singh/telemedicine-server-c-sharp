using ImTech.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImTech.Service.ViewModels
{
    public class UserLogonViewModel : ViewModelBase
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string PhoneNumber { get; set; }

        public string LoginLocation { get; set; }

        public string RequestURL { get; set; }

        public InternalDateTime LoginDate { get; set; }

        public string DeviceId { get; set; }


    }
    public class ChangePasswordViewModel : ViewModelBase
    {
        public string Email { get; set; }

        public string OldPassword { get; set; }

        public string NewPassword { get; set; }

        public string NewHashPassword { get; set; }

    }
}