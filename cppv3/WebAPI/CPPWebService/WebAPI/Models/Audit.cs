using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WebAPI.Models
{

    public abstract class Audit
    {
      
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public String CreatedBy { get; set; }
        public String UpdatedBy { get; set; }

    }
}