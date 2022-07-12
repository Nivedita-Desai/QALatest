using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models.StoredProcedure
{
    public class CostDetails
    {
        
        public decimal AmountOrQuantity  { get; set; }
        public decimal UtilizedAmountOrQuantity { get; set; }
        public decimal BalancedAmountOrQuantity { get; set; }
        public decimal RequestedAmountOrQuantity { get; set; }
        public decimal TotalCost { get; set; }
        public decimal TCost { get; set; }
    }
}