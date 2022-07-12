using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RequestUserByTokenController : ApiController
    {
        public HttpResponseMessage post([FromBody] RandomToken  token)
        {
            object status;
            RandomToken newToken = new RandomToken();
            newToken.Token = token.Token;
            User user = WebAPI.Models.RandomToken.validateToken(newToken);
            String userName = "";
            if (user.Id == 0)
                user = null;
            else
                user.RandomToken = null;

            var jsonNew = new
            {
                result = user
            };

            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}
