using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RequestCostOverheadController : ApiController
    {

        public HttpResponseMessage Get()
        {

            List<CostOverhead> costOverheadList = new List<CostOverhead>();
            costOverheadList = WebAPI.Models.CostOverhead.GetCostOverhead();


            var jsonNew = new
            {
                result = costOverheadList
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}
