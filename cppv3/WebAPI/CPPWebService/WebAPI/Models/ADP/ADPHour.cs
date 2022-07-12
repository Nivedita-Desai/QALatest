using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models.ADP
{
    public class ADPHour
    {
        public String Name { get; set; }
        public String YearWeek { get; set; }
        public String StartDate { get; set; }
        public String EndDate { get; set; }
        public String Hours { get; set; }
        public String PayCode { get; set; }
        public String WorkJobs { get; set; }
        public String WorkDepartment { get; set; }
        public String Description { get; set; }
        public String SalaryRate { get; set; }
        public String Salary { get; set; }
        public String Total { get; set; }

    }
}