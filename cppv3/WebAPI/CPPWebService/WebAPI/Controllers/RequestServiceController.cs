using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebAPI.Controllers
{
    [Authorize]
    public class RequestServiceController : ApiController
    {


        [HttpGet]
      
        public HttpResponseMessage RequestApproval(String UserID = "null",String Role = "null", String TrendId = "null", String ProjectId = "null")
        {

            if (UserID != "null" && TrendId != "null" && ProjectId != "null")
            {
                WebAPI.Services.MailServices.SendApprovalEmail(UserID, UserID, Role, TrendId, ProjectId, "email");

                var jsonNew = new
                {
                    result = "The request has been sent!"
                };
                return Request.CreateResponse(HttpStatusCode.OK, jsonNew);

            }
            else return Request.CreateResponse(HttpStatusCode.BadRequest);

          }


        }
}
