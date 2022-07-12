using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebAPI.Controllers
{
    public class RequestGetUserController : ApiController
    {
        public HttpResponseMessage post([FromBody] WebAPI.Models.User usr)
        {
            object user;

            user = WebAPI.Models.User.getUserById(usr.Id);

            var jsonNew = new
            {
                result = user
            };

            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}
