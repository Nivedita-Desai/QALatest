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

namespace WebAPI.Models
{
    [Table("trend_status")]
    public class TrendStatus
    {
        [Key]
        public int StatusID { get; set; }
        public string StatusDescription { get; set; }
        
        public static List<TrendStatus> getAllTrendStatus()
        {

            using (var ctx = new CPPDbContext())
                {
                    ctx.Database.Log = msg => Trace.WriteLine(msg);
                    
                    IQueryable<TrendStatus> trendStatuses = ctx.TrendStatus;
                    return trendStatuses.ToList<TrendStatus>();
                   
                }
            
        }

    }
}