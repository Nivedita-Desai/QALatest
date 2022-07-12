using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using WebAPI.Models;
//using System.Web.Script.Serialization;

namespace WebAPI.Controllers
{
    public class RequestUserLoginController : System.Web.Http.ApiController
    {
        //
        // GET: /RequestUserLogin/
        public HttpResponseMessage Get(String UserID = "null", String Password = "null")
        {
            User LoggedInUser = WebAPI.Models.User.authenticateLogin(UserID, Password);

            var jsonNew = new
            {
                result = LoggedInUser
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
	}
}