using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebAPI.Models.Api
{
    [Table("api_error")]
    public class ApiError
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public String ApiErrorID { get; set; }
        public String Message { get; set; }
        public String RequestMethod { get; set; }
        public String RequestUri { get; set; }
        public DateTime TimeUtc { get; set; }

    }
}