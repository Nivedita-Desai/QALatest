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
    //ODC
    [Authorize]
    public class RequestODCCostController : System.Web.Http.ApiController
    {
        //
        // GET: /RequestODCCost/
        public HttpResponseMessage Get(String ActivityID = "null", String Granularity = "null")
        {
            //Granularity = "week";
            String TrendNumber = "";
            List<CostODC> ODCCostList = WebAPI.Models.CostODC.getCostODC(ActivityID, Granularity,TrendNumber,null,null,null,null,null);


            var jsonNew = new
            {
                result = ODCCostList
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}