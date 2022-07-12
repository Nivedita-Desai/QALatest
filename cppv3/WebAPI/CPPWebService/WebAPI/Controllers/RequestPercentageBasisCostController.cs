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
    public class RequestPercentageBasisCostController : System.Web.Http.ApiController
    {
        //
        // GET: /RequestPercentageBasisCost/
        public HttpResponseMessage Get(String ActivityID = "null")
        {


            List<CostPercentage> PercentageBasisCostList = WebAPI.Models.CostPercentage.getCostPercentage(ActivityID);


            var jsonNew = new
            {
                result = PercentageBasisCostList
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}