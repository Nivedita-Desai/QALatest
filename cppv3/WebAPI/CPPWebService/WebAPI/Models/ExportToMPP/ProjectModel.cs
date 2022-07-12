using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models.ExportToMPP
{
    public class ProjectModel
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
    }
    public class ResourceModel
    {
        public long EmployeeId { get; set; }
        public int ResourceId { get; set; }
        public string EmpName { get; set; }
        public decimal? HourlyRate { get; set; }
        public double Workinghours { get; set; }
        public double MaxUnits { get; set; }
        public Int64 Duration { get; set; }
        public String CostType { get; set; }
    }
}