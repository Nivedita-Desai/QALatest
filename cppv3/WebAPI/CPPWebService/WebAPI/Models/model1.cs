using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    [Table("model1")]
    public class model1
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PhaseID { get; set; }

        public String PhaseDescription { get; set; }
    }
}