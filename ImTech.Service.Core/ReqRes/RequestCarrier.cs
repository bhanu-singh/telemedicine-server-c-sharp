using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImTech.Service
{
    public class RequestCarrier
    {
        public string From { get; set; }

        public int TanentId { get; set; }

        public object PayLoad { get; set; }

        public Int64? UserId { get; set; }

        public string DeviceId { get; set; }

    }
}