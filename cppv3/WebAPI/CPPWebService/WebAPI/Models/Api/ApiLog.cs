using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebAPI.Models.Api
{
    [Table("api_log")]
    public class ApiLog
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public string Host { get; set; }
        public String Headers { get; set; }
        public String StatusCode { get; set; }
        public DateTime TimeUtc { get; set; }
        public String RequestBody { get; set; }
        public String RequestedMethod { get; set; }
        public String UserHostAddress { get; set; }
        public String UserAgent { get; set; }
        public String AbsoluteUri { get; set; }
        public String RequestType { get; set; }


    }
}