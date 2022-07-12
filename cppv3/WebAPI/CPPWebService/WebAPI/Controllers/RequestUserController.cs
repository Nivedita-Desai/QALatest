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
//using System.Web.Script.Serialization;

namespace WebAPI.Controllers
{
    //[Authorize(Roles ="Admin")]   //luan here temporarily
    public class RequestUserController : System.Web.Http.ApiController
    {
        //
        // GET: /User/
        public HttpResponseMessage Get(String UserID = "null", String FirstName = "null", String LastName = "null", String Role = "null")
        {


            List<User> LoggedInUser = WebAPI.Models.User.getUser();

            
            var jsonNew = new
            {
                result = LoggedInUser
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }

        public HttpResponseMessage Get(String EmployeeListID, String Dummy)
        {


            List<User> userListOfApproval = WebAPI.Models.User.getUserListOfApproval(EmployeeListID);


            var jsonNew = new
            {
                result = userListOfApproval
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }

        public HttpResponseMessage Get(String UserID)
        {


            User user = WebAPI.Models.User.getUserByUserID(UserID);


            var jsonNew = new
            {
                result = user
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}