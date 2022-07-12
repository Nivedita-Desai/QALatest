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
    public class RequestFTECostController : System.Web.Http.ApiController
    {
        //
        // GET: /RequestFTECost/
        public HttpResponseMessage Get(String ActivityID = "null", String Granularity = "null")
        {

            String phaseNumber = "";
            List<CostFTE> FTECostList = WebAPI.Models.CostFTE.getCostFTE(ActivityID, Granularity, phaseNumber,null,null,null,null,null);


            var jsonNew = new
            {
                result = FTECostList
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}