using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RequestActivityIDController : ApiController
    {
        [Authorize]
        public HttpResponseMessage Get(String ActivityID = "null")
        {


            Activity Activity = WebAPI.Models.Activity.getActivityByID(ActivityID);
            var ctx = new CPPDbContext();
            Trend trend = ctx.Trend.Where(a => a.TrendID == Activity.TrendID).FirstOrDefault();
            List<CostFTE> FTECostList = new List<CostFTE>();
            List<CostLumpsum> LumpsumCostList = new List<CostLumpsum>();
            List<CostUnit> UnitCostList = new List<CostUnit>();
            List<CostODC> ODCCostList = new List<CostODC>();
            List<CostPercentage> PercentageBasisCostList = new List<CostPercentage>();
            FTECostList = WebAPI.Models.CostFTE.getCostFTE(Activity.ActivityID.ToString(), "week", Activity.TrendNumber, Activity.ProjectID.ToString(), Activity.PhaseCode.ToString(), "", Activity.BudgetCategory, Activity.BudgetSubCategory);
            LumpsumCostList = WebAPI.Models.CostLumpsum.getCostLumpsum(Activity.ActivityID.ToString(), "week", Activity.TrendNumber, Activity.ProjectID.ToString(), Activity.PhaseCode.ToString(), "", Activity.BudgetCategory, Activity.BudgetSubCategory);
            UnitCostList = WebAPI.Models.CostUnit.getCostUnit(Activity.ActivityID.ToString(), "week", Activity.TrendNumber, Activity.ProjectID.ToString(), Activity.PhaseCode.ToString(), "", Activity.BudgetCategory, Activity.BudgetSubCategory);
            ODCCostList = WebAPI.Models.CostODC.getCostODC(Activity.ActivityID.ToString(), "week", Activity.TrendNumber, Activity.ProjectID.ToString(), Activity.PhaseCode.ToString(), "", Activity.BudgetCategory, Activity.BudgetSubCategory);

            double totalFTEActual = 0, totalLumpsumActual = 0, totalUnitActual = 0, totalODCActual = 0;
            double totalFTEForecast = 0, totalLumpsumForecast = 0, totalUnitForecast = 0, totalODCForecast = 0;
            double totalFTEBudget = 0, totalLumpsumBudget = 0, totalUnitBudget = 0, totalODCBudget = 0;
            ////FTE
            //for (int y = 0; y < FTECostList.Count; y++)
            //{
            //    if (FTECostList[y].CostTrackTypeID == 3 || FTECostList[y].CostTrackTypeID == 4)
            //    {
            //        var costTrackTypeAr = FTECostList[y].CostTrackTypes.Split(',');
            //        var costAr = FTECostList[y].FTECost.Split(',');
            //        for (int z = 0; z < costTrackTypeAr.Length; z++)
            //        {
            //            if (costTrackTypeAr[z].Trim() == "3")
            //            {
            //                totalFTEActual += Convert.ToDouble(costAr[z]);
            //            }
            //            else if (costTrackTypeAr[z].Trim() == "4")
            //            {
            //                totalFTEForecast += Convert.ToDouble(costAr[z]);
            //            }
            //        }
            //    }

            //    if (true)//FTECostList[y].CostTrackTypeID == 1)
            //    {
            //        string[] costs = FTECostList[y].FTECost.Split(',');
            //        double scale = float.Parse(FTECostList[y].RawFTEHourlyRate) / float.Parse(FTECostList[y].FTEHourlyRate);
            //        foreach (string cost in costs)
            //        {
            //            totalFTEBudget += float.Parse(cost) * scale;
            //        }
            //    }
            //}

            ////Lumpsum
            //for (int y = 0; y < LumpsumCostList.Count; y++)
            //{
            //    if (LumpsumCostList[y].CostTrackTypeID == 3 || LumpsumCostList[y].CostTrackTypeID == 4)
            //    {
            //        var costTrackTypeAr = LumpsumCostList[y].CostTrackTypes.Split(',');
            //        var costAr = LumpsumCostList[y].LumpsumCost.Split(',');
            //        for (int z = 0; z < costTrackTypeAr.Length; z++)
            //        {
            //            if (costTrackTypeAr[z].Trim() == "3")
            //            {
            //                totalLumpsumActual += Convert.ToDouble(costAr[z]);
            //            }
            //            else if (costTrackTypeAr[z].Trim() == "4")
            //            {
            //                totalLumpsumForecast += Convert.ToDouble(costAr[z]);
            //            }
            //        }
            //    }
            //    if (true)//ODCCostList[y].CostTrackTypeID == 1)
            //    {
            //        string[] costs = LumpsumCostList[y].OriginalCost.Split(',');
            //        foreach (string cost in costs)
            //        {
            //            totalLumpsumBudget += float.Parse(cost);
            //        }
            //    }
            //}

            ////Unit
            //for (int y = 0; y < UnitCostList.Count; y++)
            //{
            //    if (UnitCostList[y].CostTrackTypeID == 3 || UnitCostList[y].CostTrackTypeID == 4)
            //    {
            //        var costTrackTypeAr = UnitCostList[y].CostTrackTypes.Split(',');
            //        var costAr = UnitCostList[y].UnitCost.Split(',');
            //        for (int z = 0; z < costTrackTypeAr.Length; z++)
            //        {
            //            if (costTrackTypeAr[z].Trim() == "3")
            //            {
            //                totalUnitActual += Convert.ToDouble(costAr[z]);
            //            }
            //            else if (costTrackTypeAr[z].Trim() == "4")
            //            {
            //                totalUnitForecast += Convert.ToDouble(costAr[z]);
            //            }
            //        }
            //    }
            //    if (true)//ODCCostList[y].CostTrackTypeID == 1)
            //    {
            //        string[] costs = UnitCostList[y].UnitQuantity.Split(',');
            //        float price = float.Parse(UnitCostList[y].RawUnitPrice);
            //        foreach (string cost in costs)
            //        {
            //            totalUnitBudget += float.Parse(cost) * price;
            //        }
            //    }
            //}

            ////ODC
            //for (int y = 0; y < ODCCostList.Count; y++)
            //{
            //    if (ODCCostList[y].CostTrackTypeID == 3 || ODCCostList[y].CostTrackTypeID == 4)
            //    {
            //        var costTrackTypeAr = ODCCostList[y].CostTrackTypes.Split(',');
            //        var costAr = ODCCostList[y].ODCCost.Split(',');
            //        for (int z = 0; z < costTrackTypeAr.Length; z++)
            //        {
            //            if (costTrackTypeAr[z].Trim() == "3")
            //            {
            //                totalODCActual += Convert.ToDouble(costAr[z]);
            //            }
            //            else if (costTrackTypeAr[z].Trim() == "4")
            //            {
            //                totalODCForecast += Convert.ToDouble(costAr[z]);
            //            }
            //        }
            //    }
            //    if (true)//ODCCostList[y].CostTrackTypeID == 1)
            //    {
            //        string[] costs = ODCCostList[y].OriginalCost.Split(',');
            //        foreach (string cost in costs)
            //        {
            //            totalODCBudget += float.Parse(cost);
            //        }
            //    }
            //}

            //Activity.FTECostActual = totalFTEActual;
            //Activity.FTECostForecast = totalFTEForecast;
            //Activity.LumpsumCostActual = totalLumpsumActual;
            //Activity.LumpsumCostForecast = totalLumpsumForecast;
            //Activity.UnitCostActual = totalUnitActual;
            //Activity.UnitCostForecast = totalUnitForecast;
            //Activity.OdcCostActual = totalODCActual;
            //Activity.OdcCostForecast = totalODCForecast;
            //Activity.FTEBudget = totalFTEBudget;
            //Activity.LumpsumBudget = totalLumpsumBudget;
            //Activity.OdcBudget = totalODCBudget;
            //Activity.UnitBudget = totalUnitBudget;
            List<Activity> act = new List<Activity>();
            act.Add(Activity);
            //ActivityCategory category = ctx.ActivityCategory.Where(a=>a.CategoryID == Activity.)
            var phaseCode = Activity.PhaseCode.ToString();
            WebAPI.Models.Activity.processCost(act, trend.TrendNumber, trend.ProjectID.ToString(), phaseCode, Activity.BudgetCategory, Activity.BudgetSubCategory);

            var jsonNew = new
            {
                result = Activity
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}
