using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models.ExportToMPP
{
    public class ExportData
    {
        public int PhaseID { get; set; }
        public string PhaseDescription { get; set; }
        public string Code { get; set; }
        public string ActivityPhaseCode { get; set; }
        public int ProjectID { get; set; }
        public int Order { get; set; }
        public string BudgetCategory { get; set; }
        public string BudgetSubCategory { get; set; }
        public string Price { get; set; }
        public string ActivityStartDate { get; set; }
        public string ActivityEndDate { get; set; }
        public string OriginalActivityStartDate { get; set; }
        public string OriginalActivityEndDate { get; set; }
        public string PercentageCompletion { get; set; }
        public long Duration { get; set; }
        public double WorkingHours { get; set; }
        //public long EmployeeID { get; set; }
        public string EmployeeID { get; set; }
        public string EmpName { get; set; }

        public List<ExportForecastData> lstExportForecastData { get; set; }

        public List<ExportCurrentData> lstExportCurrentData { get; set; }
    }
    public class TotalCost
    {
        public Int64 ActivityID { get; set; }
        public long PhaseID { get; set; }
        public string PhaseDescription { get; set; }
        public string Code { get; set; }
        public string ActivityPhaseCode { get; set; }
        public int ProjectID { get; set; }
        //public int Order { get; set; }
        public string Order { get; set; }
        public string BudgetCategory { get; set; }
        public string BudgetSubCategory { get; set; }
        public double Price { get; set; }
        public string ActivityStartDate { get; set; }
        public string ActivityEndDate { get; set; }
        public string OriginalActivityStartDate { get; set; }
        public string OriginalActivityEndDate { get; set; }
        public decimal PercentageCompletion { get; set; }
        public long Duration { get; set; }
        public double WorkingHours { get; set; }
        //public long EmployeeID { get; set; }
        public string EmployeeID { get; set; }
        public string EmpName { get; set; }
        public string TrendNumber { get; set; }
        public Int64 PhaseCode { get; set; }
        public string Granularity { get; set; }
    }

    public class ExportForecastData
    {
        public Int64 ActivityID { get; set; }
        public int ProjectID { get; set; }
        public string TrendNumber { get; set; }
        public Int64 PhaseCode { get; set; }
        public string PhaseDescription { get; set; }
        public string BudgetCategory { get; set; }
        public string BudgetSubCategory { get; set; }
        public string ActivityStartDate { get; set; }
        public string ActivityEndDate { get; set; }
        public double Price { get; set; }
        public double PercentageBasisCost { get; set; }
        public Int64 OrganizationID { get; set; }
        public Int64 BudgetID { get; set; }
        public Int64 TrendID { get; set; }
        public string OriginalActivityStartDate { get; set; }
        public string OriginalActivityEndDate { get; set; }
        public decimal PercentageCompletion { get; set; }
        public int order { get; set; }
        public double Workinghours { get; set; }
        public string Granularity { get; set; }
        public long Duration { get; set; }
        //public long EmployeeID { get; set; }
        public string EmployeeID { get; set; }
    }

    public class ExportCurrentData
    {
        public Int64 ActivityID { get; set; }
        public int ProjectID { get; set; }
        public string TrendNumber { get; set; }
        public Int64 PhaseCode { get; set; }
        public string PhaseDescription { get; set; }
        public string BudgetCategory { get; set; }
        public string BudgetSubCategory { get; set; }
        public string ActivityStartDate { get; set; }
        public string ActivityEndDate { get; set; }
        public double Price { get; set; }
        public double PercentageBasisCost { get; set; }
        public Int64 OrganizationID { get; set; }
        public Int64 BudgetID { get; set; }
        public Int64 TrendID { get; set; }
        public string OriginalActivityStartDate { get; set; }
        public string OriginalActivityEndDate { get; set; }
        public decimal PercentageCompletion { get; set; }
        public int order { get; set; }
        public double Workinghours { get; set; }
        public string Granularity { get; set; }
        public long Duration { get; set; }
        //public long EmployeeID { get; set; }
        public string EmployeeID { get; set; }
    }
}