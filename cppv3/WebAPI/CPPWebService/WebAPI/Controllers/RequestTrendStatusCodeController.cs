using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RequestTrendStatusCodeController : ApiController
    {
        public HttpResponseMessage Get()
        {

            List<TrendStatusCode> trendStatusCodeList = new List<TrendStatusCode>();
            trendStatusCodeList = WebAPI.Models.TrendStatusCode.GetTrendStatusCode();


            var jsonNew = new
            {
                result = trendStatusCodeList
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}