using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using WebAPI.Providers;
using WebAPI.Models;

[assembly: OwinStartup(typeof(WebAPI.Startup))]

namespace WebAPI
{
    public class Startup
    {
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void Configuration(IAppBuilder app)
        {

            ConfigureOAuth(app);
            HttpConfiguration config = new HttpConfiguration();
            WebApiConfig.Register(config);
            app.UseWebApi(config);

            logger.Info("Before scheduler start");
            Scheduler scheduler = new Scheduler();  //MPPNoRespondKillerJob
            scheduler.start();
           logger.Info("After scheduler start");

            //WebAPI.Models.Employee employee = new WebAPI.Models.Employee();
            //employee.FirstName = "Luan";
            //employee.LastName = "Khong";
            //employee.Name = "Luan Khong";
            //employee.FTEPositionID = 1;
            //employee.OrganizationID = 75;
            //employee.IsExempt = true;
            //employee.IsUnion = true;
            //employee.CreatedDate = new DateTime();
            //employee.UpdatedDate = new DateTime();

            //String status = "";
            //switch (1)
            //{
            //	case 1:
            //		status += WebAPI.Models.Employee.add(employee);
            //		break;
            //	case 2:
            //		status += WebAPI.Models.Employee.update(employee);
            //		break;
            //	case 3:
            //		status += WebAPI.Models.Employee.delete(employee);
            //		break;
            //	case 4:
            //		status += "";
            //		break;
            //}
            //var jsonNew = new
            //{
            //	result = status
            //};
        }
        public void ConfigureOAuth(IAppBuilder app)
        {
            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
               //AccessTokenExpireTimeSpan = TimeSpan.FromSeconds(30),
                Provider = new SimpleAuthorizationServerProvider()
            };

            // Token Generation
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

        }

    }
}
