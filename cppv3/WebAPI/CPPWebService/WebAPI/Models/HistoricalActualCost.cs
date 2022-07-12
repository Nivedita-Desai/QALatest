using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    [Table("historical_actual_cost")]
    public class HistoricalActualCost
    {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public String LineItem { get; set;}
        public String PoNumber { get; set; }
        public Double Amount { get; set; }
        public String Date { get; set; }
        public int Quantity { get; set; }
        public Boolean DuplicateOccurence { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

    }
}