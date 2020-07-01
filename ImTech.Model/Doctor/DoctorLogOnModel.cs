using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImTech.Model
{
    public class DoctorLogOnModel : BaseModel
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string PhoneNumber { get; set; }

        public string LoginLocation { get; set; }

        public string RequestURL { get; set; }
        public string DeviceId { get; set; }
        public InternalDateTime LoginDate { get; set; }
    }
}
