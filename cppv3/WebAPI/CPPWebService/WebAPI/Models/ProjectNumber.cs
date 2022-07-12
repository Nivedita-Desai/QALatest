using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{


    [Table("project_number")]
    public class ProjectNumber
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public String projectNumber { get; set; }
        public String projectElementNumber { get; set; }
        public Boolean isUSed { get; set; }
        public String CreatedBy { get; set; }
        public String UpdatedBy { get; set;}
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public String OrganizationID { get; set; }


    }
}