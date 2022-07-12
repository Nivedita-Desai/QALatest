using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Web;
using WebAPI.Helper;

namespace WebAPI.Models
{
    [Table("trendapprovaltracklog")]
    public class TrendApprovalTrackLog
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int ProjectId { get; set; }
        public int TrendId { get; set; }

        public String UserID { get; set; }
        public String UniqueCode { get; set; }
        public string CurrentThreshold { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ExpiredDate { get; set; }

        public byte IsActive { get; set; }

        public byte IsApproved { get; set; }

        [NotMapped]
        public String UserEmailid { get; set; }


        [NotMapped]
        public Boolean IsRecreated { get; set; }


        [NotMapped]
        public Boolean IsCodeVerified { get; set; }


    }
}