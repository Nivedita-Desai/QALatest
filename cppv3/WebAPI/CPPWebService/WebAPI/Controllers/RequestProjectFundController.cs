using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebAPI.Controllers
{
    public class RequestProjectFundController : ApiController
    {

        public HttpResponseMessage Get(int ProjectID)
        {
            object status;
            status = WebAPI.Models.ProjectFund.getProjectFund(ProjectID);
            var jsonNew = new
            {
                result = status
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}
