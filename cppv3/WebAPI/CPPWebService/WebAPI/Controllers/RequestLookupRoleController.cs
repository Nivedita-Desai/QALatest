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
     // [Authorize]
    public class RequestLookupRoleController : System.Web.Http.ApiController
    {
        //
        // GET: /User/
        public HttpResponseMessage Get(String Role = "null")
        {


            List<UserRole> matchedRoleList = WebAPI.Models.UserRole.getRole(Role);


            var jsonNew = new
            {
                result = matchedRoleList
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}