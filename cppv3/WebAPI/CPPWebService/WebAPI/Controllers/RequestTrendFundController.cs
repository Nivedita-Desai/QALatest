using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebAPI.Controllers
{
    public class RequestTrendFundController : ApiController
    {

        public HttpResponseMessage Get(int TrendID, int ProjectID)
        {
            object status;
            //String status;
            status = WebAPI.Models.TrendFund.getTrendFund(TrendID, ProjectID);
           //status = WebAPI.Models.TrendFund.testContext();
            var jsonNew = new
            {
                result = status
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}
