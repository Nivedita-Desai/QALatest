using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    [Table("line_of_business")] 
    public class LineOfBusiness : Audit
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public String LOBUniqueID { get; set; }
        public String LOBName { get; set; }
        public String LOBDescription { get; set; }



    }
}