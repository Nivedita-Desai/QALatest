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
    public class InvoiceLine
    {
        public String CostLineItem { get; set; }
        public String FullCostLineItem { get; set; }
        public float Amount { get; set; }
        public int Quantity { get; set; }
        public DateTime Date { get; set; }
        public String Description { get; set; }

        public InvoiceLine(string cli, string fcc, float a, int q, DateTime d, string desc)
        {
            String[] lineItem = fcc.Split('.');

            this.CostLineItem = cli;
            this.FullCostLineItem = fcc;
            this.Amount = a;
            this.Quantity = q;
            this.Date = d;
            this.Description = desc;
        }
    }
}