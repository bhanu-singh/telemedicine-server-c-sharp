using ImTech.DataBase;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace ImTech.Service.App_Start
{
    public class MyAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        private DataServices.DataServices _dataServices;

        public MyAuthorizationServerProvider(DataServices.DataServices dataService)
        {
            _dataServices = dataService;
        }


        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
           // throw new Exception("called");

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            if (context != null && !string.IsNullOrEmpty(context.UserName) && !string.IsNullOrEmpty(context.Password))
            {
                Model.LoginModel model = new Model.LoginModel()
                {
                    Email = context.UserName,
                    Password = context.Password,
                    HashPassword = Common.SecurityManager.EncryptText(context.Password)
                };
                model = _dataServices.LoginService.ValidateUserLogin(model);
                if (model != null && model.Success == true)
                {
                    identity.AddClaim(new Claim(ClaimTypes.Role, ((Role)model.RoleId).ToString()));
                    context.Validated(identity);
                    return;
                }
            }
            context.SetError("invalid_grant", "Provided username or password is incorrect");
            return;
            //https://www.youtube.com/watch?v=rMA69bVv0U8

        }
    }

    public enum Role
    {
        User = 1,
        Doctor = 2,
        Admin = 3,
    }
}