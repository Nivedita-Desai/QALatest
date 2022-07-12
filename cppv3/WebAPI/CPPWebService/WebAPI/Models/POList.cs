using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    public class POList
    {
        public int ID { get; set; }
        public String PurchaseOrderNumber { get; set; }
        public String PurchaseOrderBaseNumber { get; set; } //P
        public String ProjectNumber { get; set; }
        public String OrderNumber { get; set; }
        public String Description { get; set; }
        public int ProjectID { get; set; }
        public string Status { get; set; }
        public int? VendorId { get; set; }
        public string Reason { get; set; }
        public string projectName { get; set; }
        public string projectElementName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public String CreatedBy { get; set; }
        public String UpdatedBy { get; set; }
    }
}