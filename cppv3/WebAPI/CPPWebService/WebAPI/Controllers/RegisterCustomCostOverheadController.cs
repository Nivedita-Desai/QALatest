using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RegisterCustomCostOverheadController : ApiController
    {

        //
        // GET: /RegisterPhase/
        public HttpResponseMessage POST([FromBody] List<TrendCostOverhead> trendCostOverHeads)
        {

            String status = "";
            String succes = "";
            TrendCostOverhead existTrendCostOverhead = new TrendCostOverhead();
            string costType = "";
            int counter = 0;
            using (var ctx = new CPPDbContext())
            {
                foreach (var trendCost in trendCostOverHeads)
                {
                    //TrendCostOverhead existTrendCostOverhead = ctx.TrendCostOverhead
                    existTrendCostOverhead = ctx.TrendCostOverhead
                                                                        .Include("CostOverhead") //Loading Navigation Properties
                                                                         .Include("CostOverhead.CostRateType")
                                                                         .Include("CostOverhead.CostType")
                                                                    .Where(a => a.ID == trendCost.ID).FirstOrDefault();

                    if (existTrendCostOverhead != null
                                && existTrendCostOverhead.CurrentMarkup != trendCost.CurrentMarkup)
                    {

                        var CostType = "";
                        if (existTrendCostOverhead.CostOverhead != null && existTrendCostOverhead.CostOverhead.CostType != null)
                        {
                            CostType = existTrendCostOverhead.CostOverhead.CostType.AbbreviatedName;
                            //String succes = TrendCostOverhead.applyNewMarkup(trendCost.TrendID, trendCost.ID, CostType, trendCost.CurrentMarkup);
                            succes = TrendCostOverhead.applyNewMarkup(trendCost.TrendID, trendCost.ID, CostType, trendCost.CurrentMarkup, trendCost.Justification);
                            costType = string.IsNullOrEmpty(costType) ? costType + existTrendCostOverhead.CostOverhead.CostType.Type : costType + ", " + existTrendCostOverhead.CostOverhead.CostType.Type;
                            //if (succes == "success")
                            //         status += "Apply new markup for " + existTrendCostOverhead.CostOverhead.CostType.Type + " Sucessfully" + "\n";
                            //    else
                            //        status += "Error applying new markup for " + existTrendCostOverhead.CostOverhead.CostType.Type + "\n";
                            counter++;
                        }
                        else
                            status += "Unable to match cost type for " + existTrendCostOverhead.CostOverhead.CostType.Type + "\n";

                    }
                   //Manasi 22-07-2020
                    else
                    {
                        if (counter == 0)
                            succes = "Unchanged";
                    }
                }

                if (succes == "success")
                    status += "New mark up applied successfully for " + costType + "\n";
                else if (succes == "Unchanged")
                    status += "No changes to be saved."+ "\n";
                else
                    status += "Error applying new markup for " + costType + "\n";
            }
          

            var jsonNew = new
            {
                result = status
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}
