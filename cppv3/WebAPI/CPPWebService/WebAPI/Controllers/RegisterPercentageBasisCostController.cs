using System;
using System.Collections.Generic;
using System.IO;
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
    public class RegisterPercentageBasisCostController : System.Web.Http.ApiController
    {
        //
        // GET: /RegisterPercentageBasisCost/
        //public HttpResponseMessage Get(int Operation, String ProgramID = "null", String ProgramElementID = "null", String ProjectID = "null", String TrendNumber = "null", String ActivityID = "null", String PercentageBasisCostID = "null", String PercentageBasisCostStartDate = "null", String PercentageBasisCostEndDate = "null", String PercentageBasisDescription = "null", String PercentageBasisBaseAmount = "null", String PercentageBasisPercentageValue = "null", String PercentageBasisCost = "null")
        public HttpResponseMessage Post([FromBody] CostPercentage PercentageCost)
        {

            String status = "";

          //  status = WebAPI.Models.CostPercentage.updateCostPercentage(PercentageCost.Operation, PercentageCost.ProgramID, PercentageCost.ProgramElementID, PercentageCost.ProjectID, PercentageCost.TrendNumber, PercentageCost.ActivityID, PercentageCost.PercentageBasisCostID, PercentageCost.PercentageBasisCostStartDate, PercentageCost.PercentageBasisCostEndDate, PercentageCost.PercentageBasisDescription, PercentageCost.PercentageBasisBaseAmount, PercentageCost.PercentageBasisPercentageValue, PercentageCost.Granularity);
            status = "";

            var jsonNew = new
            {
                result = status
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}