using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Web;
using WebAPI.Controllers;
using WebAPI.Models;

namespace WebAPI.Models
{
    public class DocumentView
    {

        public int Operation;
        public String DocumentTypeName { get; set; }

        public int DocumentID { get; set; }
        public int? ProjectID { get; set; }
        public int? TrendNumber { get; set; } //Jignesh-TDM-06-01-2020
        public int? ProgramElementID { get; set; }
		public int? ProgramID { get; set; }
		public int? ContractID { get; set; }
		public int? ChangeOrderID { get; set; }
        public int? ModificationNumber { get; set; } //Jignesh-ModificationPopUpChanges
        public int DocumentTypeID { get; set; }
        public String DocumentName { get; set; }
        public String DocumentDescription { get; set; }
		public String ExecutionDate { get; set; }
		public byte[] DocumentBinaryData { get; set; }
        public int? Size { get; set; }
        public Boolean IsCorrupt { get; set; }


        public string DocumentBase64 { get; set; }

        public String CreatedBy { get; set; }
        public String LastUpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }

        public string TrendName { get; set; }
        public string ChangeOrderName { get; set; }
        public string ProjectElementName { get; set; }
        public string ProjectName { get; set; }
        public string ContractName { get; set; }

    }
}