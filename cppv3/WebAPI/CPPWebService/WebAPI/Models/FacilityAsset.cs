using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    [Table("facility_asset")]
    public class FacilityAsset
    {
          [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int FacilityID { get; set; }
        public int AssetID { get; set; }

        public virtual Facility Facility{ get; set; }
        public virtual Asset Asset { get; set; }
    }
}