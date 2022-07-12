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
    public class RequestUnitCostController : System.Web.Http.ApiController
    {
        //
        // GET: /RequestLumpsumCost/
        public HttpResponseMessage Get(String ActivityID = "null", String Granularity = "null")
        {

            String trendNumber = "";
            List<CostUnit> UnitCostList = WebAPI.Models.CostUnit.getCostUnit(ActivityID, Granularity,trendNumber,null,null,null,null,null);


            var jsonNew = new
            {
                result = UnitCostList
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}