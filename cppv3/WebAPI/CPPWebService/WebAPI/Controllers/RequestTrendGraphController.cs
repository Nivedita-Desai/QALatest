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
namespace WebAPI.Controllers
{
    [Authorize]
    public class RequestTrendGraphController : System.Web.Http.ApiController
    {
        //
        // GET: /RequestTrendGraph/
        public HttpResponseMessage Get(String ProjectID = "null")
        {
            /*
            List<ProgramWBS> WBSList = new List<ProgramWBS>();
            WBSList = WebAPI.Models.ProgramWBS.getWBSDetails(ProgramID, ProgramElementID, ProjectID, TrendNumber, PhaseCode, ActivityID, BudgetCategory, BudgetSubCategory);
            */
            TrendGraph TrendGraphList = new TrendGraph();
            TrendGraphList = WebAPI.Models.TrendGraph.getTrendGraph(ProjectID);


            /* var jsonNew = new
             {
                 result = SerializeJSon<List<Activity>>(ActivityList)
             };*/

            //To pass JSONObject instead of String
            /*String json = SerializeJSon<TrendGraph>(TrendGraphList);
            json = json.ToString().Replace("\\", "");
            //JObject jsonN = JObject.Parse("{ \"name\": \"San Francisco Airport\", \"level\": \"Root\",  \"children\": " + json + "}");
            JObject jsonN = JObject.Parse(json);
            return Request.CreateResponse(HttpStatusCode.OK, jsonN);*/

            var jsonNew = new
            {
                result = TrendGraphList
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
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