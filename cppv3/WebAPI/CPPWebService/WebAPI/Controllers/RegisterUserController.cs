using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Web;
using System.Web.Http;

using WebAPI.Models;

namespace WebAPI.Controllers
{
    
    public class RegisterUserController : System.Web.Http.ApiController
    {
        //
        // GET: /RegisterUser/
        public HttpResponseMessage Post([FromBody] List<User> userList)
        {

           String status = "";
            Boolean isUpdateFromEmail = true;
           foreach (var user in userList)
           {
               if (user.Operation == 1)
               {               //  status = WebAPI.Models.User.registerUser(user.FullName, user.UserID,  user.AccessControlList, user.LoginPassword, user.Email);
                   status += WebAPI.Models.User.registerUser(user);
                   // Send email notification
                   //if (status.Contains("successfully"))    //TODO - If enabled. Cause iis to stop working. Maybe if email is invalid.
                       //WebAPI.Services.MailServices.SendUserRegistration(user.FirstName + " " + user.MiddleName + " " + user.LastName, user.UserID, user.Email);
               }

               if (user.Operation == 2)
                   status += WebAPI.Models.User.updateUser(user);

               if (user.Operation == 3)
                   status += WebAPI.Models.User.deleteUser(user);

                if (user.Operation == 4) //Update password from Email
                    status += WebAPI.Models.User.updateUser(user);
            }
            var jsonNew = new
            {
                result = status
            };

            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
	}
}