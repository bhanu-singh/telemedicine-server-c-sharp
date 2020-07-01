using System;
using System.Web;
using System.Web.Http;

namespace ImTech.Service
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            UnityConfig.RegisterComponents();
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", "*");
            if (HttpContext.Current.Request.HttpMethod != "OPTIONS") return;
            HttpContext.Current.Response.AddHeader("Access-Control-Allow-Credentials", "true");
            HttpContext.Current.Response.AddHeader("Access-Control-Allow-Headers", "Authorization");
            HttpContext.Current.Response.AddHeader("Cache-Control", "no-cache");
            HttpContext.Current.Response.AddHeader("Access-Control-Allow-Methods", "GET, POST");
            HttpContext.Current.Response.AddHeader("Access-Control-Allow-Headers", "Content-Type, Accept");
            HttpContext.Current.Response.AddHeader("Access-Control-Max-Age", "1728000");
            HttpContext.Current.Response.End();


            //HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", "*");
            //if (HttpContext.Current.Request.HttpMethod != "OPTIONS") return;
            //HttpContext.Current.Response.AddHeader("Access-Control-Allow-Credentials", "true");
            //HttpContext.Current.Response.AddHeader("Access-Control-Allow-Headers", "Authorization");
            //HttpContext.Current.Response.AddHeader("Cache-Control", "no-cache");
            //HttpContext.Current.Response.AddHeader("Access-Control-Allow-Methods", "GET, POST");
            //HttpContext.Current.Response.AddHeader("Access-Control-Allow-Headers", "Content-Type, application/x-www-form-urlencoded; charset=UTF-8");
            //HttpContext.Current.Response.AddHeader("Access-Control-Max-Age", "1728000");
            //HttpContext.Current.Response.End();
        }
    }
}
