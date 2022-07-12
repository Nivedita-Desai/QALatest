using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebAPI.Controllers
{
    public class RequestFutureTrendController : ApiController
    {

        public HttpResponseMessage Get(int ProjectID, String Granularity)
        {

            object status ;


           status = WebAPI.Models.FutureTrend.GetFutureProject(ProjectID, Granularity);

           // status = "";

            var jsonNew = new
            {
                result = status
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}
