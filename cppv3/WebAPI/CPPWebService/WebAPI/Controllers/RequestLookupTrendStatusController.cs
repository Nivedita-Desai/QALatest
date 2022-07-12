using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Web;
using System.Web.Http;

using WebAPI.Models;
//using System.Web.Script.Serialization;

namespace WebAPI.Controllers
{
    [Authorize]
    public class RequestLookupTrendStatusController : System.Web.Http.ApiController
    {
        //
        // GET: /User/
        public HttpResponseMessage Get(String StatusID = "null", String StatusDescription = "null")
        {


            List<TrendStatus> TrendStatusList = WebAPI.Models.TrendStatus.getAllTrendStatus();


            var jsonNew = new
            {
                result = TrendStatusList
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}