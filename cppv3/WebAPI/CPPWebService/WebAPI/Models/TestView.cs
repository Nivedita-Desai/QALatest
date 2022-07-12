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
    [Table("testView")]
    public class TestView
    {


        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FundTypeId { get; set; }
        public String Fund { get; set; }
        public String Amount { get; set; }
        public String Availability { get; set; }
        public String BalanceRemaining { get; set; }

        
    }
}