using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebAPI.Controllers
{
    public class RequestProgramFundController : ApiController
    {

        public HttpResponseMessage Get(int ProgramID)
        {
            object status;
            status = WebAPI.Models.ProgramFund.getProgramFund(ProgramID);
            var jsonNew = new
            {
                result = status
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}
