using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImTech.Model
{
   public class OTPModel:BaseModel
    {
        public string otpMobileNo { get; set; }

        public string otpCode { get; set; }

        public Int64? UserId { get; set; }

    }
}
