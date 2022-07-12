using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RequestGanttViewCostController : ApiController
    {
        public HttpResponseMessage Get(String ContractID = "null", String ProjectID = "null",String ElementID = "null", String TrendNumber = "null", String PhaseCode = "null", String ActivityID = "null", String Granularity = "null",
                                        String BudgetID = "null", String ViewLabor = "null", String BudgetCategory = "null", String BudgetSubCategory = "null"

            )
        {

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            List<GanttViewCost> CostList = WebAPI.Models.GanttViewCost.getCosts(ContractID, ProjectID, ElementID, TrendNumber, PhaseCode, ActivityID, Granularity, BudgetID, BudgetCategory, BudgetSubCategory, ViewLabor);

            CostList = CostList.OrderBy(a => a.CostLineItemID).ToList();

            CostList = CostList.OrderBy(a => a.CostLineItemID.Substring(17, 2)).ToList();

            String json = SerializeJSon<List<GanttViewCost>>(CostList);
            json = json.ToString().Replace("\\", "");
            JObject jsonN = JObject.Parse("{ \"MaxFTECostID\": \" " + GanttViewCost.MaxFCostID +
                                        "\", \"MaxLumpsumCostID\": \"" + GanttViewCost.MaxLCostID +
                                        "\", \"MaxUnitCostID\": \"" + GanttViewCost.MaxUCostID +
                                        "\", \"MaxODCCostID\": \"" + GanttViewCost.MaxODCCostID +
                                        "\", \"MaxPercentageCostID\": \"" + GanttViewCost.MaxPCostID +
                                        "\",  \"CostRow\": " + json + "}");
            GanttViewCost.MaxFCostID = GanttViewCost.MaxLCostID = GanttViewCost.MaxUCostID = GanttViewCost.MaxPCostID = 0;
            stopWatch.Stop();
            //logger.Debug("Getting costs elapsed time " + stopWatch.ElapsedMilliseconds);
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
