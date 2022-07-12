using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebAPI.Controllers
{
    public class RequestTrendCurrentDateController : ApiController
    {

        [Authorize]
        public HttpResponseMessage Get(String ProjectID = "null")
        {


            List<String> listDate = WebAPI.Models.Trend.getCurrentTrendDate(ProjectID);


            var jsonNew = new
            {
                result = listDate
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}
