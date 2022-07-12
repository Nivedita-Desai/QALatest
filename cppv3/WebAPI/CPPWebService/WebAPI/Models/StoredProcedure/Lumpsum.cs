using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Models.StoredProcedure
{
    class Lumpsum : Audit
    {
        public String PhaseCode { get; set; }
        public String BudgetCategory { get; set; }
        public String BudgetSubCategory { get; set; }
        public String LumpsumCostID { get; set; }
        public String ActivityID { get; set; }
        public String LumpsumCostStartDate { get; set; }
        public String LumpsumCostEndDate { get; set; }
        public String LumpsumDescription { get; set; }
        public String LumpsumCost { get; set; }
        public String Granularity {get;set;}
        public String CostTrackTypeID { get; set; }
        public String EstimatedCostID { get; set; }
        public String SubcontractorTypeID { get; set; }
        public String SubcontractorID { get; set; }
        public String OriginalCost { get; set; }

    }
}
