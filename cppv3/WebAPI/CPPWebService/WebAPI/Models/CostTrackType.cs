using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    [Table("cost_track_type")]
    public class CostTrackType
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int UniqueID { get; set; }
        public String Type { get; set; }
        public String Description { get; set; }

        //Audit Fields
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public String CreatedBy { get; set; }
        public String UpdatedBy { get; set; }


    }
}