using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RegisterCostRowController : ApiController
    {
        public HttpResponseMessage POST([FromBody] List<CostRow> costRows)
        {

            object status = "";
            CostRow costRow = new CostRow();
            var ctx = new CPPDbContext();
            var costType = costRows.FirstOrDefault().CostType;
            var lineID = costRows.FirstOrDefault().CostID.Split('_')[1];
            var activityID = costRows.FirstOrDefault().ActivityID;
            var granularity = costRows.FirstOrDefault().Granularity;
            var activity = Activity.getActivityByID(activityID);
            var trend = Trend.getTrendById(activity.TrendNumber.ToString(),activity.ProjectID);
            var project = ctx.Project.Where(a => a.ProjectID == trend.ProjectID).FirstOrDefault();
            if(costType == "F" || costType == "FTE")
            {
                List<CostFTE> ftes = costRow.convertToFTE( costRows );
                foreach(var fte in ftes)
                {
                    Cost.saveFTECost(2, project.ProgramID.ToString(), project.ProgramElementID.ToString(), project.ProjectID.ToString(), trend.TrendNumber, fte.ActivityID.ToString(), fte.FTECostID, fte.FTEStartDate, fte.FTEEndDate, fte.FTEPosition, fte.FTEValue,
                                                        fte.FTEHourlyRate, fte.FTEHours, fte.FTECost, fte.Granularity, fte.FTEPositionID, fte.CostTrackTypeID, fte.EstimatedCostID, fte.EmployeeID, fte.CostLineItemID);

                }
                Scaling.scaling(Convert.ToInt16(activityID), Convert.ToInt16(lineID), granularity, "F");
            }else if(costType == "L")
            {
                List<CostLumpsum> lumpsums = costRow.convertToLumpsum(costRows);
                foreach(var lumpsum in lumpsums)
                {
                    Cost.saveLumpsumCost("2", project.ProgramID.ToString(), project.ProgramElementID.ToString(), project.ProjectID.ToString(), trend.TrendNumber, lumpsum.ActivityID.ToString(), lumpsum.LumpsumCostID, lumpsum.LumpsumCostStartDate, lumpsum.LumpsumCostEndDate,
                        lumpsum.LumpsumDescription, lumpsum.LumpsumCost, lumpsum.Granularity, lumpsum.CostTrackTypeID, lumpsum.EstimatedCostID, lumpsum.SubcontractorTypeID, lumpsum.SubcontractorID, lumpsum.CostLineItemID);
                }
                Scaling.scaling(Convert.ToInt16(activityID),Convert.ToInt16(lineID),granularity,"L");
            }else if(costType == "ODC")
            {
                List<CostODC> odcs = costRow.convertToODC(costRows);
                foreach(var odc in odcs)
                {
                    Cost.saveODCCost("2", project.ProgramID.ToString(), project.ProgramElementID.ToString(), project.ProjectID.ToString(), trend.TrendNumber, odc.ActivityID.ToString(), odc.ODCCostID, odc.ODCStartDate,
                                        odc.ODCEndDate, odc.ODCQuantity, 0, "", odc.ODCCost, odc.Granularity, odc.ODCTypeID, odc.CostTrackTypeID, odc.EstimatedCostID, odc.CostLineItemID);
                }
                Scaling.scaling(Convert.ToInt16(activityID), Convert.ToInt16(lineID), granularity, "ODC");
            }else if(costType == "U")
            {
                List<CostUnit> units = costRow.convertToMaterial(costRows);
                var unitCostID = units.FirstOrDefault().UnitCostID;
                //UnitType type = ctx.UnitType.Where(a => a.UnitAbbr == unitType).FirstOrDefault();
                CostUnit currentCost = ctx.CostUnit.Where(a => a.UnitCostID == unitCostID).FirstOrDefault();
                foreach(var unit in units)
                {
                    Cost.saveUnitCost("2", project.ProgramID.ToString(), project.ProgramElementID.ToString(), project.ProjectID.ToString(), trend.TrendNumber, unit.ActivityID.ToString(), unit.UnitCostID,
                        unit.UnitCostStartDate, unit.UnitCostEndDate, currentCost.UnitDescription, unit.UnitQuantity, unit.MaterialCategoryID.ToString(), unit.MaterialID.ToString(), unit.UnitPrice,
                        unit.UnitCost, unit.Granularity, currentCost.UnitType, currentCost.UnitType_ID.ToString(), unit.CostTrackTypeID, unit.EstimatedCostID, unit.CostLineItemID);
                }
                Scaling.scaling(Convert.ToInt16(activityID), Convert.ToInt16(lineID), granularity, "U");
            }
            


            var jsonNew = new
            {
                result = status
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}
