using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using WebAPI.Helper;
using WebAPI.Models.StoredProcedure;

namespace WebAPI.Models
{
  [NotMapped]
    public class CostRow : TemporaryCost
    {
   
        public String TypeStr;
       
        public String NameStr;
      
        public String OriginalRate;
  
      


        public List<CostRow> convertFromFTE(List<CostFTE> costs)
        {
            List<CostRow> rows = new List<CostRow>();
            foreach (var cost in costs)
            {
                CostRow row = new CostRow()
                {
                   // ID = cost.ID,
                    ActivityID = cost.ActivityID.ToString(),
                    MainCategory = "",
                    SubCategory = "",
                    BudgetID = "",
                    CostType = "FTE", 
                    Type = cost.FTEPositionID.ToString(),  //PositionID
                    TypeStr = cost.FTEPosition,
                    CellValue = cost.FTEValue,
                    FTEHours = cost.FTEHours,
                    TotalCost = cost.FTECost,
                    Name = cost.EmployeeID.ToString(), //Name = EmployeeID
                    Description = "",
                    BaseRate = cost.FTEHourlyRate,
                    OriginalCost = cost.OriginalCost,
                    EstimatedCostID = cost.EstimatedCostID.ToString(),
                    LineItem = cost.CostLineItemID,
                    CostTrackTypeID = cost.CostTrackTypeID.ToString(),
                    StartDate = cost.FTEStartDate,   //Start Date
                    EndDate = cost.FTEEndDate,    //End Date
                    Granularity = cost.Granularity, //Granularity,
                    CostID = cost.FTECostID,  //Cost ID
                    OriginalRate= cost.RawFTEHourlyRate
                    

                };
                rows.Add(row);
            }

            return rows;
        }

        public List<CostFTE> convertToFTE(List<CostRow> rows)
        {
            List<CostFTE> costs = new List<CostFTE>();
            foreach (var row in rows)
            {
                CostFTE cost = new CostFTE()
                {
                    FTECostID = row.CostID,
                    Granularity = row.Granularity,
                    ActivityID = int.Parse(row.ActivityID),
                    FTEStartDate = DateTime.Parse(row.StartDate).ToString(DateUtility.getSqlDateFormat()),
                    FTEEndDate = DateTime.Parse(row.EndDate).ToString(DateUtility.getSqlDateFormat()),
                    FTEPosition = row.TypeStr,
                    FTEPositionID = int.Parse(row.Type),
                    FTEValue = row.CellValue,
                    FTEHourlyRate = row.BaseRate,
                    FTEHours = row.FTEHours,
                    FTECost = row.TotalCost,
                    CostTrackTypeID = int.Parse(row.CostTrackTypeID),
                    EstimatedCostID = int.Parse(row.EstimatedCostID),
                    EmployeeID = int.Parse(row.Name),
                    CostLineItemID = row.LineItem,
                    OriginalCost = row.OriginalCost,
                    RawFTEHourlyRate = row.OriginalRate,
                    ProjectID = row.ProjectID

                };
                costs.Add(cost);

            }


            return costs;

        }

        public List<CostRow> convertFromMaterial(List<CostUnit> costs)
        {
            List<CostRow> rows = new List<CostRow>();
            foreach (var cost in costs)
            {
                CostRow row = new CostRow()
                {
                    // ID = cost.ID,
                    ActivityID = cost.ActivityID.ToString(),
                    MainCategory = "",
                    SubCategory = "",
                    BudgetID = "",
                    CostType = "U",
                    Type = cost.MaterialCategoryID.ToString(),  //PositionID
                    TypeStr = cost.UnitDescription,
                    CellValue = cost.UnitQuantity,
                    FTEHours = "",
                    TotalCost = cost.UnitCost,
                    Name = cost.MaterialID.ToString(), //Name = EmployeeID
                    Description = "",
                    BaseRate = cost.UnitPrice,
                    OriginalCost = cost.OriginalCost,
                    EstimatedCostID = cost.EstimatedCostID.ToString(),
                    LineItem = cost.CostLineItemID,
                    CostTrackTypeID = cost.CostTrackTypeID.ToString(),
                    StartDate = cost.UnitCostStartDate,   //Start Date
                    EndDate = cost.UnitCostEndDate,    //End Date
                    Granularity = cost.Granularity, //Granularity,
                    CostID = cost.UnitCostID,  //Cost ID
                    OriginalRate = cost.RawUnitPrice

                };
                rows.Add(row);
            }

            return rows;
        }

       public List<CostUnit> convertToMaterial(List<CostRow> rows)
        {
            List<CostUnit> units = new List<CostUnit>();

            foreach(var row in rows)
            {
                CostUnit unit = new CostUnit()
                {
                    ID = row.ID,
                    UnitCostID = row.CostID,
                    Granularity = row.Granularity,
                    ActivityID = int.Parse(row.ActivityID),
                    UnitCostStartDate = DateTime.Parse(row.StartDate).ToString(DateUtility.getSqlDateFormat()),
                    UnitCostEndDate = DateTime.Parse(row.EndDate).ToString(DateUtility.getSqlDateFormat()),
                    UnitDescription = row.Description,
                    UnitQuantity = row.CellValue,
                    UnitPrice = row.BaseRate,
                    UnitCost = row.TotalCost,
                    UnitType = "",
                    UnitType_ID = (row.UnitType_ID != null)? int.Parse(row.UnitType_ID):0,
                    CostTrackTypeID = int.Parse(row.CostTrackTypeID),
                    EstimatedCostID = int.Parse(row.EstimatedCostID),
                    MaterialID = int.Parse(row.Name),
                    MaterialCategoryID = int.Parse(row.Type),
                    CostLineItemID = row.LineItem,
                    OriginalCost = row.OriginalCost,
                    RawUnitPrice = row.OriginalRate
                    
                };

                units.Add(unit);
            }

            return units;
        }

        public List<CostRow> convertFromLumpsum(List<CostLumpsum> costs)
        {
            List<CostRow> rows = new List<CostRow>();
            foreach (var cost in costs)
            {
                CostRow row = new CostRow()
                {
                    // ID = cost.ID,
                    ActivityID = cost.ActivityID.ToString(),
                    MainCategory = "",
                    SubCategory = "",
                    BudgetID = "",
                    CostType = "L",
                    Type = cost.SubcontractorTypeID.ToString(),  //PositionID
                    TypeStr = cost.LumpsumDescription,
                    //CellValue = cost.OriginalCost,
                    CellValue = cost.LumpsumCost,   //Manasi 05-08-2020
                    //FTEHours = "",
                    FTEHours = "N/A",   //Manasi 05-08-2020
                    TotalCost = cost.LumpsumCost,
                    Name = cost.SubcontractorID.ToString(), //Name = EmployeeID
                    Description = "",
                    BaseRate = "",
                    OriginalCost = cost.OriginalCost,
                    EstimatedCostID = cost.EstimatedCostID.ToString(),
                    LineItem = cost.CostLineItemID,
                    CostTrackTypeID = cost.CostTrackTypeID.ToString(),
                    StartDate = cost.LumpsumCostStartDate,   //Start Date
                    EndDate = cost.LumpsumCostEndDate,    //End Date
                    Granularity = cost.Granularity, //Granularity,
                    CostID = cost.LumpsumCostID,  //Cost ID
                    //OriginalRate = ""
                    OriginalRate = "N/A"

                };
                rows.Add(row);
            }

            return rows;
        }
        public List<CostLumpsum> convertToLumpsum(List<CostRow> rows)
        {
            List<CostLumpsum> lumpsumCosts = new List<CostLumpsum>();

            foreach(var row in rows)
            {
                CostLumpsum lumpsum = new CostLumpsum()
                {
                    ID = row.ID,
                    LumpsumCostID = row.CostID,
                    ActivityID = int.Parse(row.ActivityID),
                    LumpsumCostStartDate = DateTime.Parse(row.StartDate).ToString(DateUtility.getSqlDateFormat()),
                    LumpsumCostEndDate = DateTime.Parse(row.EndDate).ToString(DateUtility.getSqlDateFormat()),
                    LumpsumDescription = row.TypeStr,
                    LumpsumCost = row.CellValue,
                    Granularity = row.Granularity,
                    CostTrackTypeID = int.Parse(row.CostTrackTypeID),
                    EstimatedCostID = int.Parse(row.EstimatedCostID),
                    SubcontractorTypeID = int.Parse(row.Type),
                    SubcontractorID = int.Parse(row.Name),
                    CostLineItemID = row.LineItem,
                    OriginalCost = row.OriginalCost,
                    ProjectID = row.ProjectID
                };

                lumpsumCosts.Add(lumpsum);
            }

            return lumpsumCosts;
        }

        public List<CostRow> convertFromODC(List<CostODC> costs)
        {
            List<CostRow> rows = new List<CostRow>();
            foreach (var cost in costs)
            {
                CostRow row = new CostRow()
                {
                    // ID = cost.ID,
                    ActivityID = cost.ActivityID.ToString(),
                    MainCategory = "",
                    SubCategory = "",
                    BudgetID = "",
                    CostType = "ODC",
                    Type = cost.ODCTypeID.ToString(),  //PositionID
                    TypeStr = "",
                    //CellValue = cost.OriginalCost,  
                    CellValue = cost.ODCQuantity,     //Manasi 05-08-2020
                    //FTEHours = "",
                    FTEHours = "N/A",
                    TotalCost = cost.ODCCost,
                    Name = "", //Name = EmployeeID
                    Description = "",
                    BaseRate = "",
                    OriginalCost = cost.OriginalCost,
                    EstimatedCostID = cost.EstimatedCostID.ToString(),
                    LineItem = cost.CostLineItemID,
                    CostTrackTypeID = cost.CostTrackTypeID.ToString(),
                    StartDate = cost.ODCStartDate,   //Start Date
                    EndDate = cost.ODCEndDate,    //End Date
                    Granularity = cost.Granularity, //Granularity,
                    CostID = cost.ODCCostID,  //Cost ID
                    //OriginalRate = ""
                    OriginalRate = "N/A"

                };
                rows.Add(row);
            }

            return rows;
        }
        public List<CostODC> convertToODC(List<CostRow> rows)
        {
            List<CostODC> odcs = new List<CostODC>();

            foreach(var row in rows)
            {
                CostODC odc = new CostODC()
                {
                    ID = row.ID,
                    ActivityID = int.Parse(row.ActivityID),
                    ODCStartDate = DateTime.Parse(row.StartDate).ToString(DateUtility.getSqlDateFormat()),
                    ODCEndDate = DateTime.Parse(row.EndDate).ToString(DateUtility.getSqlDateFormat()),
                    ODCQuantity = row.CellValue,
                    ODCPrice = "",
                    ODCCost = row.CellValue,
                    Granularity  = row.Granularity,
                    EstimatedCostID = int.Parse(row.EstimatedCostID),
                    ODCCostID = row.CostID,
                    ODCTypeID= int.Parse(row.Type),
                    CostTrackTypeID = int.Parse(row.CostTrackTypeID),
                    CostLineItemID = row.LineItem,
                    OriginalCost = row.OriginalCost

                };

                odcs.Add(odc);
            }

            return odcs;
        }

    }
}