using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models.StoredProcedure
{
    public class Unit : Audit
    {
        public String PhaseCode { get; set; }
        public String BudgetCategory { get; set; }
        public String BudgetSubCategory { get; set; }
        public String UnitCostID { get; set; }
        public String Granularity { get; set; }
        public String ActivityID { get; set; }
        public String UnitCostStartDate { get; set; }
        public String UnitCostEndDate { get; set; }
        public String UnitDescription { get; set; }
        public String UnitQuantity { get; set; }
        public String UnitPrice { get; set; }
        public String UnitCost { get; set; }
        public String UnitType { get; set; }
        public String UnitType_ID { get; set; }
        public String CostTrackTypeID { get; set; }
        public String EstimatedCostID { get; set; }
        public String MaterialID { get; set; }
        public String MaterialCategoryID { get; set; }
        public String OriginalCost { get; set; }
        public String RawUnitPrice { get; set; }

    }
}