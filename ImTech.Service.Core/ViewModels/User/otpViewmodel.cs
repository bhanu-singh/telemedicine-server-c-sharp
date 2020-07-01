using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImTech.Service.ViewModels
{
    public class otpViewmodel : ViewModelBase
    {
        public string otpMobileNo { get; set; }

        public string otpCode { get; set; }

        public Int64? UserId { get; set; }
    }
}