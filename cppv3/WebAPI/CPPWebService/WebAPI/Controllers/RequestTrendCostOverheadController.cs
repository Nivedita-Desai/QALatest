using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RequestTrendCostOverheadController : ApiController
    {
        
        public HttpResponseMessage Get(int ProjectID, int TrendNumber)
        {
            var currentDate = DateTime.Now;

            List<TrendCostOverhead> trendCostOverheads = null;
            using (var ctx = new CPPDbContext())
            {
                String tNumber = TrendNumber.ToString();
                //------------------Manasi--------------------------------
                if (tNumber == "2000")
                {
                    Trend ForcastTrend = ctx.Trend.Where(a => a.ProjectID == ProjectID && a.TrendNumber != "1000").FirstOrDefault();
                    if (ForcastTrend != null)
                    {
                        trendCostOverheads = ctx.TrendCostOverhead
                                                .Include("CostOverhead")
                                                .Include("CostOverhead.CostRateType")
                                                .Include("CostOverhead.CostType")
                                                .Where(a => a.TrendID == ForcastTrend.TrendID)
                                                .ToList();
                    }
                }
                //---------------------------------------------------------
                else
                {
                    Trend trend = ctx.Trend.Where(a => a.ProjectID == ProjectID && a.TrendNumber == tNumber).FirstOrDefault();
                    if (trend != null)
                    {
                        trendCostOverheads = ctx.TrendCostOverhead
                                                .Include("CostOverhead")
                                                .Include("CostOverhead.CostRateType")
                                                .Include("CostOverhead.CostType")
                                                .Where(a => a.TrendID == trend.TrendID)
                                                .ToList();
                    }
                    //else
                    //{
                    //    return Request.CreateResponse(HttpStatusCode.NotFound,new { });
                    //}
                }

            }

            var jsonNew = new
            {
                result = trendCostOverheads
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}
