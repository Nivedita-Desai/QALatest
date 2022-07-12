using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RequestValidateTokenController : ApiController
    {
        public HttpResponseMessage post([FromBody] RandomToken token)
        {
            User status = new User();

            status = RandomToken.validateToken(token);
            var testStatus = "Success";
         //   var jsonNew = new
           // {
             //   result = testStatus
            //};
            var s = JsonConvert.SerializeObject(status, Formatting.Indented,
                       new JsonSerializerSettings()
                       {
                           ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                       });
            JObject json = JObject.Parse(s);
            return Request.CreateResponse(HttpStatusCode.OK, json);
        }
    }
}
