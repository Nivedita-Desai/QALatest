using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    interface IAudit
    {
        String CreatedBy { get; set; }
        String LastUpdatedBy { get; set; }
        DateTime CreatedDate { get; set; }
        DateTime LastUpdatedDate { get; set; }
    }
}
