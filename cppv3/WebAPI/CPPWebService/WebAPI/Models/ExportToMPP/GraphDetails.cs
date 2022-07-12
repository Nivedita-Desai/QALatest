using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models.ExportToMPP
{
    public class GraphDetails
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double TotalCost { get; set; }
        public double ATD { get; set; }
        public double BTD { get; set; }
        public double BTC { get; set; }
        public double EAC { get; set; }
        public double FTC { get; set; }
    }


    public class Data
    {
        public List<string> labels { get; set; }
        public List<yData> datasets { get; set; }
    }

    public class yData
    {
        public string label { get; set; }
        public List<string> data { get; set; }
        public bool fill { get; set; }
        public string borderColor { get; set; }
        public bool spanGaps { get; set; }
    }

    public class GoogleChart
    {
        public List<object> chartData { get; set; }
    }
}