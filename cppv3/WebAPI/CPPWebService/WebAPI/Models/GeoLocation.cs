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
    [Table("geo_location")]
    public class GeoLocation
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GeoLocationID { get; set; }
        public string ShapeType { get; set; }
        public string LocationDescription { get; set; }
        public string RGBValue { get; set; }

        public virtual ICollection<GeoCoordinate> GeoCoordinates { get; set; }
    }
}