using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;

namespace ImTech.Service.Common
{
    public class MedLyteExceptionFilterAttribute : ExceptionFilterAttribute
    {
        //private static readonly ILog log = LogManager.GetLogger(typeof(LogExceptionFilter));
        public override void OnException(HttpActionExecutedContext context)
        {
            //log.Error("Unhandeled Exception", actionExecutedContext.Exception);
            base.OnException(context);
        }
    }
}