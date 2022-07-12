using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RequestCustomCostOverheadController : ApiController
    {
        public HttpResponseMessage Get(int TrendID)
        {


            List<TrendCostOverhead> trendCostOverheads = new List<TrendCostOverhead>();

            trendCostOverheads = TrendCostOverhead.getTrendCustomCostOverhead(TrendID);


            var jsonNew = new
            {
                result = trendCostOverheads
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}
