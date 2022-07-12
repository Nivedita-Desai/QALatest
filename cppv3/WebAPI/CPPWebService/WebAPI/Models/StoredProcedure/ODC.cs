using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models.StoredProcedure
{
    public class ODC : Audit
    {
        public String PhaseCode { get; set; }
        public String BudgetCategory { get; set; }
        public String BudgetSubCategory { get; set; }
        public String ODCCostID { get; set; }
        public String ActivityID { get; set; }
        public String ODCStartDate { get; set; }
        public String ODCEndDate { get; set; }
        public String ODCQuantity { get; set; }
        public String ODCPrice { get; set; }
        public String Granularity { get; set; }
        public String EstimatedCostID { get; set; }
        //public String CreatedBy { get; set; }
        //public String LastUpdatedBy { get; set; }
        public String ODCTypeID { get; set; }
        public String CostTrackTypeID { get; set; }
        //public String CreatedDate { get; set; }
        //public String LastUpdatedDate { get; set; }
        public String ODCCost { get; set; }
        public String OriginalCost { get; set; }
    }
}