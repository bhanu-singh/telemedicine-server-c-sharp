using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using ImTech.Model;
using ImTech.DataServices;
namespace ImTech.Service.Filters
{
    public class GlobalExceptionHandlerAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext filterContext)
        {

            base.OnException(filterContext);

            ILogger logger = LogFactory.Create();
            //PrincipalUser user = filterContext.RequestContext.HttpContext.User as PrincipalUser;
            logger.LogError(new LogMessage
            {
                Summary = filterContext.Exception.Message,
                Exception = Convert.ToString(filterContext.Exception.StackTrace),
                IP = HttpContext.Current.Request.UserHostAddress,
                UserId = 0,
                UserType = 0,
                Application = "Web"
            });
        }
    }
}