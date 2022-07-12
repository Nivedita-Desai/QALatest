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

namespace WebAPI.Controllers
{
    [Authorize]
    public class RequestTrendController : System.Web.Http.ApiController
    {
        //
        // GET: /RequestTrend/
        public HttpResponseMessage Get(String ProgramID = "null", String ProgramElementID = "null", String ProjectID = "null", String TrendNumber = "null", String KeyStroke = "null")
        {


            List<Trend> TrendList = WebAPI.Models.Trend.getTrend(ProgramID, ProgramElementID, ProjectID, TrendNumber, KeyStroke);


            var jsonNew = new
            {
                result = TrendList
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }



	}
}