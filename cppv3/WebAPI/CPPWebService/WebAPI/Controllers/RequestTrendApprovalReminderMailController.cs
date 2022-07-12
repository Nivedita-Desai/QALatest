using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebAPI.Controllers
{
    public class RequestTrendApprovalReminderMailController : ApiController
    {
        public HttpResponseMessage Get()
        {


            WebAPI.Models.Trend.SendReminderApprovalEmail();


            var jsonNew = new
            {
                sucess = true
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}
