using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImTech.Model
{
    public class UserLogOnModel : BaseModel
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string PhoneNumber { get; set; }

        public string LoginLocation { get; set; }

        public string RequestURL { get; set; }

        public string DeviceId { get; set; }
    }
    public class ChangePasswordModel : BaseModel
    {
        public string Email { get; set; }

        public string OldPassword { get; set; }

        public string NewPassword { get; set; }

        public string NewHashPassword { get; set; }

    }
}
