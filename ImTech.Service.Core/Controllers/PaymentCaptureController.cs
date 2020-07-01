using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ImTech.Service.Controllers
{
    public class PaymentCaptureController : ApiController
    {
        [HttpPost]
        public IHttpActionResult Redirect(object requestCarrier)
        {
            var response = new ResponseCarrier() { Status = true, PayLoad = requestCarrier, ErrorMessage = "" };
            return Json(response);
        }

        [HttpPost]
        public IHttpActionResult Cancel(object requestCarrier)
        {
            var response = new ResponseCarrier() { Status = true, PayLoad = requestCarrier, ErrorMessage = "" };
            return Json(response);
        }


        [HttpPost]
        public IHttpActionResult RSAKey(object requestCarrier)
        {
            var response = new ResponseCarrier() { Status = true, PayLoad = requestCarrier, ErrorMessage = "" };
            return Json(response);
        }
    }
}
