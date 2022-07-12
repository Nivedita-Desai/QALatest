using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Owin.Security.OAuth;
using WebAPI.Models;
using Microsoft.Owin.Security;

namespace WebAPI.Providers
{
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {

            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
            context.Response.Headers.Add("Access-Control-Allow-Methods", new[] { "GET, PUT, DELETE, POST,OPTIONS" });
            context.Response.Headers.Add("Access-Control-Allow-Headers", new[] { "Content-Type, Accept, Authorization,Ignore,contentType" });
            context.Response.Headers.Add("Access-Control-Max-Age", new[] { "1728000" });

            User user = WebAPI.Models.User.authenticateLogin(context.UserName, context.Password);
            if (user.UserID != context.UserName.ToLower())
            {
                context.SetError("Invalid_grant", "The user name or password is incorrect");
                return;
            }
                
            if (user.Role == "User")
            {
                context.SetError("Ivanlid_grant", "You are not authorized for access. Please wait until the Admin assign you a role!");
                return;
            }
            

            //var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            //identity.AddClaim(new Claim("sub", context.UserName));
            //identity.AddClaim(new Claim("role", "user"));
            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
            identity.AddClaim(new Claim(ClaimTypes.Role,user.Role));
            var props = new AuthenticationProperties(new Dictionary<string, string>
                {
                    { "userName", user.UserID },
                    { "userId", user.Id.ToString() },
                    { "role", user.Role },
                    { "acl", user.AccessControlList },
                    { "threshold", user.Threshold },
                    { "employeeID", user.EmployeeID.ToString() },
                    { "passwordChangeRequired", user.PasswordChangeRequired.ToString() }
                });
            // identity.AddClaim(new Claim(ClaimTypes.Role, "Supervisor"));
            var ticket = new AuthenticationTicket(identity, props);
            context.Validated(ticket);

        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }
    }
}