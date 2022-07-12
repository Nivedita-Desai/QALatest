using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RequestActivitiesForGanttAtContractLevelController : ApiController
    {
        [HttpGet]
        [Route("Request/Activities/")]
        public HttpResponseMessage GetActivities(String CostBreakdown = "0", String ProgramID = "null", String ProgramElementID = "null", String ProjectID = "null", String TrendNumber = "null", String PhaseCode = "null", String ActivityID = "null", String BudgetCategory = "null", String BudgetSubCategory = "null")
        {
            List<Activity> ActivityList = new List<Activity>();
            List<JObject> jsonList = new List<JObject>();

            if (CostBreakdown == "0")
            {
                ActivityList = WebAPI.Models.ViewContractGanttActivities.getActivityDetails(ProgramID, ProgramElementID, ProjectID, TrendNumber, PhaseCode, ActivityID, BudgetCategory, BudgetSubCategory);
            }

            var s = JsonConvert.SerializeObject(ActivityList, Formatting.Indented,
                   new JsonSerializerSettings()
                   {
                       ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                   });
            JObject jsonN = JObject.Parse("{\"result\": " + s + "}");


            return Request.CreateResponse(HttpStatusCode.OK, jsonN);

        }
    }
}
