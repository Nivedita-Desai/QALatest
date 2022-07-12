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
using Newtonsoft.Json.Linq;
using System.Diagnostics;
//using System.Web.Script.Serialization;
namespace WebAPI.Controllers
{
    //[Authorize]
    public class RequestCostController : System.Web.Http.ApiController
    {
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //
        // GET: /RequestCost/
        public HttpResponseMessage Get(String ProjectID = "null", String TrendNumber = "null", String PhaseCode = "null", String ActivityID = "null", String Granularity = "null",
                                        String BudgetID = "null", String ViewLabor = "null", String BudgetCategory = "null", String BudgetSubCategory = "null"
                    
            )
        {

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            List<Cost> CostList = WebAPI.Models.Cost.getCosts(ProjectID, TrendNumber, PhaseCode, ActivityID, Granularity, BudgetID, BudgetCategory, BudgetSubCategory, ViewLabor);

            //CostList = CostList.OrderBy(a => Convert.ToDecimal(a.CostLineItemID)).ToList();

            CostList = CostList.OrderBy(
                                            a => 
                                            { 
                                                var orderID = a.CostLineItemID.Substring(18, 2); 
                                                return int.Parse(orderID); 
                                            }
                                       ).ToList();
            //CostList = CostList.OrderBy(a => a.DT_RowID).ToList();

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
            stopWatch.Stop();
            logger.Debug("Getting costs elapsed time " + stopWatch.ElapsedMilliseconds);
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