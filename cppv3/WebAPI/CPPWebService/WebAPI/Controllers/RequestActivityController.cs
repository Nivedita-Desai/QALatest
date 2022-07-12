using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json.Linq;

using WebAPI.Models;
using Newtonsoft.Json;
//using System.Web.Script.Serialization;

namespace WebAPI.Controllers
{
    //[Authorize]   //luan here temporarily
    public class RequestActivityController : System.Web.Http.ApiController
    {
        //
        // GET: /RequestActivity/

        public HttpResponseMessage Get(String CostBreakdown = "0", String ProgramID = "null", String ProgramElementID = "null", String ProjectID = "null", String TrendNumber = "null", String PhaseCode = "null", String ActivityID = "null", String BudgetCategory = "null", String BudgetSubCategory = "null")
        {
            List<Activity> ActivityList = new List<Activity>();
            List<JObject> jsonList = new List<JObject>();
            //Activity Details
            if (CostBreakdown == "0")
            {
                ActivityList = WebAPI.Models.Activity.getActivityDetails(ProgramID, ProgramElementID, ProjectID, TrendNumber, PhaseCode, ActivityID, BudgetCategory, BudgetSubCategory);
            }
            //Activity Details + Cost Breakdowns
            if (CostBreakdown == "1")
            {
               
                ActivityList = WebAPI.Models.Activity.getActivityBreakdowns(ProgramID, ProgramElementID, ProjectID, TrendNumber, PhaseCode, ActivityID, BudgetCategory, BudgetSubCategory);
            }



            var s = JsonConvert.SerializeObject(ActivityList, Formatting.Indented,
                         new JsonSerializerSettings()
                         {
                             ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                         });
            JObject jsonN = JObject.Parse("{\"result\": " + s + "}");


            return Request.CreateResponse(HttpStatusCode.OK, jsonN);
           /* var jsonNew = new
            {
                result = SerializeJSon<List<Activity>>(ActivityList)
            };*/

            //To pass JSONObject instead of String
            //String json = SerializeJSon<List<Activity>>(ActivityList);
            //json = json.ToString().Replace("\\", "");
            //JObject jsonN = JObject.Parse("{\"result\": " + json + "}");
            //return Request.CreateResponse(HttpStatusCode.OK, jsonN);
        }
        public static string SerializeJSon<T>(T t)
        {
            MemoryStream stream = new MemoryStream();
            DataContractJsonSerializer ds = new DataContractJsonSerializer(typeof(T));
            DataContractJsonSerializerSettings s = new DataContractJsonSerializerSettings();
            ds.WriteObject(stream, t);
            string jsonString = System.Text.Encoding.UTF8.GetString(stream.ToArray());
            stream.Close();
            //jsonString = jsonString.Replace("\\", "");
            return jsonString;
        }
    }
}