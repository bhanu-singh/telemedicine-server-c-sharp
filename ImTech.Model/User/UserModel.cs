using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImTech.Model
{
    public class UserModel : BaseModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string HashPassword { get; set; }

        public string PhoneNumber { get; set; }

        public bool IsPhoneNoVerified { get; set; }

        public string DeviceId { get; set; }
    }
}
