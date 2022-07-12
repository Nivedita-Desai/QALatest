using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebAPI.Controllers
{
    public class RequestResetPasswordController : ApiController
    {
        public HttpResponseMessage post([FromBody] WebAPI.Models.User user)
        {
            object status;

            status = WebAPI.Models.User.changePasswordById(user);

            var jsonNew = new
            {
                result = status
            };

            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}
