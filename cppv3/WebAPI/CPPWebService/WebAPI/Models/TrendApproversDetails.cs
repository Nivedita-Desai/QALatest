using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    [Table("trendapproversdetails")]
    public class TrendApproversDetails
    {

        public int Id { get; set; }
        public int ApproverMatrixId { get; set; }
        public int EmpId { get; set; }
        public string EmpName { get; set; }
        public int ProjectElementId { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdatedBy { get; set; }

        [NotMapped]
        public int UserId { get; set; }

    }
}