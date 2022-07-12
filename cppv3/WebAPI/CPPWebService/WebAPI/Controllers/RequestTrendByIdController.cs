using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RequestTrendByIdController : ApiController
    {
        [Authorize]
        public HttpResponseMessage Get(String trendId = "null",int projectId=0)
        {


            Trend trend = WebAPI.Models.Trend.getTrendById(trendId,projectId);


            var jsonNew = new
            {
                result = trend
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}
