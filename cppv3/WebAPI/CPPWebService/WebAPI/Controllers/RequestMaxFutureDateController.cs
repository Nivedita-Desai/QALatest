using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebAPI.Controllers
{
    public class RequestMaxFutureDateController : System.Web.Http.ApiController
    {

        public HttpResponseMessage Get(String ProjectID ="null" )
        {


            String maxFutureDate = WebAPI.Models.Trend.getMaxFutureDate(ProjectID);


            var jsonNew = new
            {
                result = maxFutureDate
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}
