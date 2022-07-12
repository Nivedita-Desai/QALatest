using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    [Table("line_of_business")]
    public class LobPhase : Audit
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int ProjectLobID { get; set; }

        public int PhaseCode { get; set; }
        public int Order { get; set; }

        //TODO - enable foreign key constraitn later
        //[ForeignKey("ProjectClassID")]
        //public virtual ProjectClass ProjectClass { get; set; }

        //[ForeignKey("PhaseID")]
        //public virtual PhaseCode PhaseCode { get; set; }
    }
}