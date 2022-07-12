using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebAPI.Models.StoredProcedure
{
    [Table("temporary_cost")]
    public class TemporaryCost : Audit
    {


        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public String ProjectID { get; set; }
        public String TrendNumber { get; set; }
        public String PhaseCode { get; set; }
        public String ActivityID { get; set; }
        public String MainCategory { get; set; }
        public String SubCategory { get; set; }
        public String BudgetID { get; set; }

        public String CostType { get; set; } //Labor, subcontractor, ODC, Material
        public String Type { get; set; } // FTEPositionID, MaterialCategoryID,ODCTypeID, SubcontractorTypeID 
        public String Name { get; set; } //EmployeeID, MaterialID, SubcontractorID

        //FTE Only
        public String FTEHours { get; set; }
        public String Description { get; set; } //LumpsumDescription, UnitDescription
        public String UnitType_ID { get; set; } //

        public String CellValue { get; set; }// Cell value FTEValue, ODCQuantity, ODCCost, UnitQuantity
        public String BaseRate { get; set; }// (FTE => FTEHourlyRate, UnitPrice)
        public String TotalCost { get; set; }
        public String CostTrackTypeID { get; set; }
        public String EstimatedCostID { get; set; }
        public String CostID { get; set; }
        public String Granularity { get; set; }
        public String StartDate { get; set; }
        public String EndDate { get; set; }
        //public String LastUpdatedBy { get; set; }
        //public String CreatedBy { get; set; }
        //public String LastUpdatedDate { get; set; }
        //public String CreatedDate { get; set; }

        public String LineItem { get; set; } //x.xxxx.x.xx.xxx.xx
        public String OriginalCost { get; set; }



       
    }
}