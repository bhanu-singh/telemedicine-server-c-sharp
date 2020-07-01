using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Security.OAuth;
using System.Web.Http;
using ImTech.Service.App_Start;

[assembly: OwinStartup(typeof(ImTech.Service.Startup))]

namespace ImTech.Service
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {

            // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
                      
            var myProvider = new MyAuthorizationServerProvider(new DataServices.DataServices(new ImTech.DataBase.DataBaseService()));
            OAuthAuthorizationServerOptions options = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(365),
                Provider = myProvider
            };
            app.UseOAuthAuthorizationServer(options);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
            HttpConfiguration config = new HttpConfiguration();
            WebApiConfig.Register(config);
        }

    }
}
