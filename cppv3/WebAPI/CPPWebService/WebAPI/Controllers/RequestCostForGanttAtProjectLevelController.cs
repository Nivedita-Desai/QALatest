using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RequestCostForGanttAtProjectLevelController : ApiController
    {
        //
        // GET: /RequestCost/
        public HttpResponseMessage Get(String ProjectID = "null", String TrendNumber = "null", String PhaseCode = "null", String ActivityID = "null", String Granularity = "null",
        String BudgetID = "null", String ViewLabor = "null", String BudgetCategory = "null", String BudgetSubCategory = "null", String ProgramElementID = "null"
                    
                    )
        {


            List<Cost> CostList = WebAPI.Models.ViewProgramElementGanttCost.getCosts(ProjectID, TrendNumber, PhaseCode, ActivityID, Granularity, BudgetID, BudgetCategory, BudgetSubCategory, ViewLabor, ProgramElementID);

            CostList = CostList.OrderBy(a => a.CostLineItemID).ToList();

            CostList = CostList.OrderBy(a => a.CostLineItemID.Substring(17, 2)).ToList();

            /*var jsonNew = new
            {
                result = CostList
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);*/

            String json = SerializeJSon<List<Cost>>(CostList);
            json = json.ToString().Replace("\\", "");
            JObject jsonN = JObject.Parse("{ \"MaxFTECostID\": \" " + Cost.MaxFCostID +
                                        "\", \"MaxLumpsumCostID\": \"" + Cost.MaxLCostID +
                                        "\", \"MaxUnitCostID\": \"" + Cost.MaxUCostID +
                                        "\", \"MaxODCCostID\": \"" + Cost.MaxODCCostID +
                                        "\", \"MaxPercentageCostID\": \"" + Cost.MaxPCostID +
                                        "\",  \"CostRow\": " + json + "}");
            Cost.MaxFCostID = Cost.MaxLCostID = Cost.MaxUCostID = Cost.MaxPCostID = 0;
            return Request.CreateResponse(HttpStatusCode.OK, jsonN);
        }

        public static string SerializeJSon<T>(T t)
        {

            MemoryStream stream = new MemoryStream();
            DataContractJsonSerializer ds = new DataContractJsonSerializer(typeof(T));
            DataContractJsonSerializerSettings s = new DataContractJsonSerializerSettings();
            ds.WriteObject(stream, t);
            string jsonString = System.Text.Encoding.UTF8.GetString(stream.ToArray());
            stream.Close();

            return jsonString;
        }
    }
}
