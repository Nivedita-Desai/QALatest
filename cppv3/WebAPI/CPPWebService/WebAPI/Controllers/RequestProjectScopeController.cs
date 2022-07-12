using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebAPI.Controllers
{
    public class RequestProjectScopeController : ApiController
    {

        public HttpResponseMessage Get(int ProjectID)
        {
            object status;
            //String status;
            status = WebAPI.Models.ProjectScope.getProjectScope( ProjectID);
            //status = WebAPI.Models.TrendFund.testContext();
            var jsonNew = new
            {
                result = status
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }

    }
}
