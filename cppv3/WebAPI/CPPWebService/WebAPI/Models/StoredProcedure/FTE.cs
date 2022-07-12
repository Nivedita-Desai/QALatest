using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models.StoredProcedure
{
    public class FTE : Audit
    {
        public String PhaseCode { get; set; }
        public String BudgetCategory { get; set; }
        public String BudgetSubCategory { get; set; }
        public String FTECostID { get; set; }
        public String FTEPosition { get; set; }
        public String FTEPositionID { get; set; }
        public String ActivityID { get; set; }
        public String FTEStartDate { get; set; }
        public String FTEEndDate { get; set; }
        public String FTEValue { get; set; }
        public String FTEHourlyRate { get; set; }
        public String FTEHours { get; set; }
        public String FTECost { get; set; }
        public String Granularity { get; set; }
        public String CostTrackTypeID { get; set; }
        public String EstimatedCostID { get; set; }
        public String EmployeeID { get; set; }
        public String OriginalCost { get; set; }

        public String RawFTEHourlyRate { get; set; }

    }
}