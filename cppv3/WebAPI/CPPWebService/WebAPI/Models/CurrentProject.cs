using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    [Table("currentproject")]
    public class CurrentProjectTrend
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ActivityID { get; set; }
        public int? ProjectID { get; set; }
        public int? TrendNumber { get; set; }
        public int? PhaseCode { get; set; }
        public String BudgetCategory { get; set; }
        public String BudgetSubCategory { get; set; }
        public String ActivityStartDate { get; set; }
        public String ActivityEndDate { get; set; }
        public Double? FTECost { get; set; }
        public Double? LumpsumCost { get; set; }
        public Double? UnitCost { get; set; }
        public Double? PercentageBasisCost { get; set; }
        public int? OrganizationID { get; set; }
        public int? BudgetID { get; set; }
        public int? TrendID { get; set; }
        public String CostType { get; set; }
        public String Granularity { get; set; }
        public int? ActID { get; set; }
        public String CostStartDate { get; set; }
        public String CostEndDate { get; set; }
        public String CostDescription { get; set; }
        public String CostBoxValue { get; set; }
        public String CostMultiplier { get; set; }
        public String FTECostDays { get; set; }
        public String CostTotal { get; set; }
        public String LookupID { get; set; }
        public Double? MergedCostBoxValue { get; set; }
        public Double? MergedFteCostDays { get; set; }
        public Double? MergedTotalCost { get; set; }

        public String CostRowID { get; set; }
    }
}