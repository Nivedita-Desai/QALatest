using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.Reflection;
using System.Diagnostics;
using System.Data.OleDb;
using WebAPI.Controllers;
using MySql.Data.MySqlClient;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.SqlServer;

namespace WebAPI.Models
{
    public class EstimateLine
    {
        public String CostID { get; set; }
        public String costLineItem { get; set; }
        public String WBS { get; set; }
        public String CostType { get; set; }
        public String Type { get; set; }
        public String Name { get; set; }
        public String Quantity { get; set; }
        public String UnitType { get; set; }
        public String UnitCost { get; set; }
        public String Total { get; set; }
        public String Client { get; set; }
        public String TaxType { get; set; }
    }
}