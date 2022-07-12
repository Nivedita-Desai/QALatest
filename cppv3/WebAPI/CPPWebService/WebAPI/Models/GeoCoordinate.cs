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
    [Table("geo_coordinate")]
    public class GeoCoordinate
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GeoCoordinateID { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }

        public int GeoLocationID { get; set; }
        [ForeignKey("GeoLocationID")]
        //[IgnoreDataMember]
        public virtual GeoLocation GeoLocation { get; set; }
    }
}