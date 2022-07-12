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

using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace WebAPI.Models
{
    [Table("document")]
    public class Document : IAudit
    {
        [NotMapped]
        public int Operation;

		[NotMapped]
		public String DocumentSet;

		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DocumentID { get; set; }
        public int? ProjectID { get; set; }
        public int? TrendNumber { get; set; }
        public int? ProgramElementID { get; set; }
		public int? ProgramID{ get; set; }
		public int? ChangeOrderID { get; set; }
		public int? ContractID { get; set; }
		public int? DocumentTypeID { get; set; }
        public int? ModificationNumber { get; set; } //Jignesh-ModificationPopUpChanges
        public String DocumentName { get; set; }
        public String FileTye { get; set; }
        public String DocumentDescription { get; set; }
		public String ExecutionDate { get; set; }
		public byte[] DocumentBinaryData { get; set; }
        public int? Size { get; set; }
        public Boolean IsCorrupt { get; set; }

        [Column(TypeName = "LONGTEXT")]
        public string DocumentBase64 { get; set; }

        public String CreatedBy { get; set; }
        public String LastUpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }

        //ON DELETE RESTRICT
        [ForeignKey("ProjectID")]
        public virtual Project Project { get; set; }

		//ON DELETE RESTRICT
		[ForeignKey("ProgramElementID")]
		public virtual ProgramElement ProgramElement { get; set; }

		//ON DELETE RESTRICT
		[ForeignKey("ProgramID")]
		public virtual Program Program { get; set; }

		//ON DELETE RESTRICT
		[ForeignKey("ContractID")]
		public virtual Contract Contract { get; set; }

		//ON DELETE RESTRICT
		[ForeignKey("DocumentTypeID")]
        public virtual DocumentType DocumentType { get; set; }

        //Nivedita 10022022
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string DeletedBy { get; set; }

        public static List<DocumentView> GetDocument(String DocumentSet, int projectID)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);

            List<DocumentView> documentList = new List<DocumentView>();



            try
            {

                using (var ctx = new CPPDbContext())
                {
                    if (DocumentSet == "Project")
                    {
                        //documentList = ctx.Document.Where(a => a.Project == projectID).OrderBy(a => a.DocumentName).ToList();
                        documentList = ctx.Document.Where(a => a.ProjectID == projectID && a.IsDeleted == false).
                                        Join(ctx.DocumentType, doc => doc.DocumentTypeID, docType => docType.DocumentTypeID, (doc, docType) => new { doc, docType }).
                                        OrderByDescending(a => a.doc.CreatedDate).
                                        //OrderBy(doc => doc.LastUpdatedDate).
                                        //Where( x => x.doc.ProjectID = projectID).
                                        Select(m => new DocumentView
                                        {
                                            DocumentID = m.doc.DocumentID,
                                            ProjectID = m.doc.ProjectID.Value,
                                            ProgramElementID = m.doc.ProgramElementID.Value,
                                            ProgramID = m.doc.ProgramID.Value,
                                            ContractID = m.doc.ContractID.Value,
                                            ChangeOrderID = m.doc.ChangeOrderID.Value,
                                            DocumentTypeID = m.doc.DocumentTypeID.Value,
                                            DocumentName = m.doc.DocumentName,
                                            DocumentDescription = m.doc.DocumentDescription,
                                            ExecutionDate = m.doc.ExecutionDate,
                                            IsCorrupt = m.doc.IsCorrupt,
                                            Size = m.doc.Size,
                                            //DocumentBinaryData = m.doc.DocumentBinaryData,
                                            DocumentBase64 = m.doc.DocumentBase64,
                                            CreatedBy = m.doc.CreatedBy,
                                            LastUpdatedBy = m.doc.LastUpdatedBy,
                                            CreatedDate = m.doc.CreatedDate,
                                            LastUpdatedDate = m.doc.LastUpdatedDate,
                                            TrendNumber = m.doc.TrendNumber, // Jignesh-TDM-06-01-2020
                                            DocumentTypeName = m.docType.DocumentTypeName,
                                        }).
                                        ToList();
                    }
                    else if (DocumentSet == "ProgramElement")
                    {
                        //documentList = ctx.Document.Where(a => a.Project == projectID).OrderBy(a => a.DocumentName).ToList();
                        documentList = ctx.Document.Where(a => a.ProgramElementID == projectID && a.IsDeleted == false).
                                        Join(ctx.DocumentType, doc => doc.DocumentTypeID, docType => docType.DocumentTypeID, (doc, docType) => new { doc, docType }).
                                        OrderByDescending(a => a.doc.CreatedDate).
                                        //OrderBy(doc => doc.LastUpdatedDate).
                                        //Where( x => x.doc.ProjectID = projectID).
                                        Select(m => new DocumentView
                                        {
                                            DocumentID = m.doc.DocumentID,
                                            ProjectID = m.doc.ProjectID.Value,
                                            ProgramElementID = m.doc.ProgramElementID.Value,
                                            ProgramID = m.doc.ProgramID.Value,
                                            ContractID = m.doc.ContractID.Value,
                                            ChangeOrderID = m.doc.ChangeOrderID.Value,
                                            DocumentTypeID = m.doc.DocumentTypeID.Value,
                                            DocumentName = m.doc.DocumentName,
                                            DocumentDescription = m.doc.DocumentDescription,
                                            ExecutionDate = m.doc.ExecutionDate,
                                            IsCorrupt = m.doc.IsCorrupt,
                                            Size = m.doc.Size,
                                            //DocumentBinaryData = m.doc.DocumentBinaryData,
                                            DocumentBase64 = m.doc.DocumentBase64,
                                            CreatedBy = m.doc.CreatedBy,
                                            LastUpdatedBy = m.doc.LastUpdatedBy,
                                            CreatedDate = m.doc.CreatedDate,
                                            LastUpdatedDate = m.doc.LastUpdatedDate,

                                            DocumentTypeName = m.docType.DocumentTypeName,
                                        }).
                                        ToList();
                    }
                    else if (DocumentSet == "Program")
                    {
                        //documentList = ctx.Document.Where(a => a.Project == projectID).OrderBy(a => a.DocumentName).ToList();
                        documentList = ctx.Document.Where(a => a.ProgramID == projectID && a.IsDeleted == false).
                                        Join(ctx.DocumentType, doc => doc.DocumentTypeID, docType => docType.DocumentTypeID, (doc, docType) => new { doc, docType }).
                                        OrderByDescending(a => a.doc.CreatedDate).
                                        //OrderBy(doc => doc.LastUpdatedDate).
                                        //Where( x => x.doc.ProjectID = projectID).
                                        Select(m => new DocumentView
                                        {
                                            DocumentID = m.doc.DocumentID,
                                            ProjectID = m.doc.ProjectID.Value,
                                            ProgramElementID = m.doc.ProgramID.Value,
                                            ProgramID = m.doc.ProgramID.Value,
                                            ContractID = m.doc.ContractID.Value,
                                            ChangeOrderID = m.doc.ChangeOrderID.Value,
                                            DocumentTypeID = m.doc.DocumentTypeID.Value,
                                            DocumentName = m.doc.DocumentName,
                                            DocumentDescription = m.doc.DocumentDescription,
                                            ExecutionDate = m.doc.ExecutionDate,
                                            IsCorrupt = m.doc.IsCorrupt,
                                            Size = m.doc.Size,
                                            //DocumentBinaryData = m.doc.DocumentBinaryData,
                                            DocumentBase64 = m.doc.DocumentBase64,
                                            CreatedBy = m.doc.CreatedBy,
                                            LastUpdatedBy = m.doc.LastUpdatedBy,
                                            CreatedDate = m.doc.CreatedDate,
                                            LastUpdatedDate = m.doc.LastUpdatedDate,
                                            ModificationNumber = m.doc.ModificationNumber, //Jignesh-ModificationPopUpChanges ( Jignesh-08-02-2021 )
                                            DocumentTypeName = m.docType.DocumentTypeName,
                                        }).
                                        ToList();
                    }
                    else if (DocumentSet == "ProgramContract")
                    {
                        //documentList = ctx.Document.Where(a => a.Project == projectID).OrderBy(a => a.DocumentName).ToList();
                        documentList = ctx.Document.Where(a => a.ContractID == projectID && a.IsDeleted == false).
                                        Join(ctx.DocumentType, doc => doc.DocumentTypeID, docType => docType.DocumentTypeID, (doc, docType) => new { doc, docType }).
                                        OrderByDescending(a => a.doc.CreatedDate).
                                        //OrderBy(doc => doc.LastUpdatedDate).
                                        //Where( x => x.doc.ProjectID = projectID).
                                        Select(m => new DocumentView
                                        {
                                            DocumentID = m.doc.DocumentID,
                                            ProjectID = m.doc.ProjectID.Value,
                                            ProgramElementID = m.doc.ProgramElementID.Value,
                                            ProgramID = m.doc.ProgramID.Value,
                                            ContractID = m.doc.ContractID.Value,
                                            ChangeOrderID = m.doc.ChangeOrderID.Value,
                                            DocumentTypeID = m.doc.DocumentTypeID.Value,
                                            DocumentName = m.doc.DocumentName,
                                            DocumentDescription = m.doc.DocumentDescription,
                                            ExecutionDate = m.doc.ExecutionDate,
                                            IsCorrupt = m.doc.IsCorrupt,
                                            Size = m.doc.Size,
                                            //DocumentBinaryData = m.doc.DocumentBinaryData,
                                            DocumentBase64 = m.doc.DocumentBase64,
                                            CreatedBy = m.doc.CreatedBy,
                                            LastUpdatedBy = m.doc.LastUpdatedBy,
                                            CreatedDate = m.doc.CreatedDate,
                                            LastUpdatedDate = m.doc.LastUpdatedDate,

                                            DocumentTypeName = m.docType.DocumentTypeName,
                                        }).
                                        ToList();
                    }
                    else if (DocumentSet == "ProgramElementChangeOrder")
                    {
                        //documentList = ctx.Document.Where(a => a.Project == projectID).OrderBy(a => a.DocumentName).ToList();
                        documentList = ctx.Document.Where(a => a.ChangeOrderID == projectID && a.IsDeleted == false).
                                        Join(ctx.DocumentType, doc => doc.DocumentTypeID, docType => docType.DocumentTypeID, (doc, docType) => new { doc, docType }).
                                        OrderByDescending(a => a.doc.CreatedDate).
                                        //OrderBy(doc => doc.LastUpdatedDate).
                                        //Where( x => x.doc.ProjectID = projectID).
                                        Select(m => new DocumentView
                                        {
                                            DocumentID = m.doc.DocumentID,
                                            ProjectID = m.doc.ProjectID.Value,
                                            ProgramElementID = m.doc.ProgramElementID.Value,
                                            ProgramID = m.doc.ProgramID.Value,
                                            ContractID = m.doc.ContractID.Value,
                                            ChangeOrderID = m.doc.ChangeOrderID.Value,
                                            DocumentTypeID = m.doc.DocumentTypeID.Value,
                                            DocumentName = m.doc.DocumentName,
                                            DocumentDescription = m.doc.DocumentDescription,
                                            ExecutionDate = m.doc.ExecutionDate,
                                            IsCorrupt = m.doc.IsCorrupt,
                                            Size = m.doc.Size,
                                            //DocumentBinaryData = m.doc.DocumentBinaryData,
                                            DocumentBase64 = m.doc.DocumentBase64,
                                            CreatedBy = m.doc.CreatedBy,
                                            LastUpdatedBy = m.doc.LastUpdatedBy,
                                            CreatedDate = m.doc.CreatedDate,
                                            LastUpdatedDate = m.doc.LastUpdatedDate,

                                            DocumentTypeName = m.docType.DocumentTypeName,
                                        }).
                                        ToList();
                    }
                    else if (DocumentSet == "ProjectViewAll")
                    {
                        documentList = (from a in ctx.Document
                                        join b in ctx.ProgramElement on a.ProgramElementID equals b.ProgramElementID
                                        join c in ctx.DocumentType on a.DocumentTypeID equals c.DocumentTypeID
                                        where b.ProgramElementID == projectID && a.IsDeleted == false
                                        select new DocumentView
                                        {
                                            DocumentID = a.DocumentID,
                                            ProjectID = a.ProjectID.Value,
                                            ProgramElementID = a.ProgramElementID.Value,
                                            ProgramID = a.ProgramID.Value,
                                            ContractID = a.ContractID.Value,
                                            ChangeOrderID = a.ChangeOrderID.Value,
                                            DocumentTypeID = a.DocumentTypeID.Value,
                                            DocumentName = a.DocumentName,
                                            DocumentDescription = a.DocumentDescription,
                                            ExecutionDate = a.ExecutionDate,
                                            IsCorrupt = a.IsCorrupt,
                                            Size = a.Size,
                                            DocumentBase64 = a.DocumentBase64,
                                            CreatedBy = a.CreatedBy,
                                            LastUpdatedBy = a.LastUpdatedBy,
                                            CreatedDate = a.CreatedDate,
                                            LastUpdatedDate = a.LastUpdatedDate,
                                            DocumentTypeName = c.DocumentTypeName,
                                            TrendName = "",
                                            ChangeOrderName = "",
                                            ProjectElementName = b.ProgramElementName,
                                            ProjectName = "",
                                            ContractName = ""

                                        }).Union(from b in ctx.ProgramElement
                                                 join d in ctx.ChangeOrder on b.ProgramElementID equals d.ProgramElementID
                                                 join a in ctx.Document on d.ChangeOrderID equals a.ChangeOrderID
                                                 join c in ctx.DocumentType on a.DocumentTypeID equals c.DocumentTypeID
                                                 where b.ProgramElementID == projectID && a.IsDeleted == false
                                                 select new DocumentView
                                                 {
                                                     DocumentID = a.DocumentID,
                                                     ProjectID = a.ProjectID.Value,
                                                     ProgramElementID = a.ProgramElementID.Value,
                                                     ProgramID = a.ProgramID.Value,
                                                     ContractID = a.ContractID.Value,
                                                     ChangeOrderID = a.ChangeOrderID.Value,
                                                     DocumentTypeID = a.DocumentTypeID.Value,
                                                     DocumentName = a.DocumentName,
                                                     DocumentDescription = a.DocumentDescription,
                                                     ExecutionDate = a.ExecutionDate,
                                                     IsCorrupt = a.IsCorrupt,
                                                     Size = a.Size,
                                                     DocumentBase64 = a.DocumentBase64,
                                                     CreatedBy = a.CreatedBy,
                                                     LastUpdatedBy = a.LastUpdatedBy,
                                                     CreatedDate = a.CreatedDate,
                                                     LastUpdatedDate = a.LastUpdatedDate,
                                                     DocumentTypeName = "",
                                                     TrendName = "",
                                                     ChangeOrderName = d.ChangeOrderName,
                                                     ProjectElementName = b.ProgramElementName,
                                                     ProjectName = "",
                                                     ContractName = ""

                                                 }).Union(from b in ctx.ProgramElement
                                                          join d in ctx.Project on b.ProgramElementID equals d.ProgramElementID
                                                          join a in ctx.Document on d.ProjectID equals a.ProjectID
                                                          join c in ctx.DocumentType on a.DocumentTypeID equals c.DocumentTypeID
                                                          where b.ProgramElementID == projectID && a.TrendNumber == null && a.IsDeleted == false
                                                          select new DocumentView
                                                          {
                                                              DocumentID = a.DocumentID,
                                                              ProjectID = a.ProjectID.Value,
                                                              ProgramElementID = a.ProgramElementID.Value,
                                                              ProgramID = a.ProgramID.Value,
                                                              ContractID = a.ContractID.Value,
                                                              ChangeOrderID = a.ChangeOrderID.Value,
                                                              DocumentTypeID = a.DocumentTypeID.Value,
                                                              DocumentName = a.DocumentName,
                                                              DocumentDescription = a.DocumentDescription,
                                                              ExecutionDate = a.ExecutionDate,
                                                              IsCorrupt = a.IsCorrupt,
                                                              Size = a.Size,
                                                              DocumentBase64 = a.DocumentBase64,
                                                              CreatedBy = a.CreatedBy,
                                                              LastUpdatedBy = a.LastUpdatedBy,
                                                              CreatedDate = a.CreatedDate,
                                                              LastUpdatedDate = a.LastUpdatedDate,
                                                              DocumentTypeName = c.DocumentTypeName,
                                                              TrendName = "",
                                                              ChangeOrderName = "",
                                                              ProjectElementName = b.ProgramElementName,
                                                              ProjectName = d.ProjectName,
                                                              ContractName = ""

                                                          }).Union(from b in ctx.ProgramElement
                                                                   join d in ctx.Project on b.ProgramElementID equals d.ProgramElementID
                                                                   join e in ctx.Trend on d.ProjectID equals e.ProjectID
                                                                   //join a in ctx.Document on e.TrendNumber equals a.TrendNumber.ToString() 
                                                                   join a in ctx.Document on new { X1 = e.TrendNumber, X2 = e.ProjectID.ToString() } equals new { X1 = a.TrendNumber.ToString(), X2 = a.ProjectID.ToString() }
                                                                   join c in ctx.DocumentType on a.DocumentTypeID equals c.DocumentTypeID
                                                                   where b.ProgramElementID == projectID && a.TrendNumber != null && a.IsDeleted == false
                                                                   select new DocumentView
                                                                   {
                                                                       DocumentID = a.DocumentID,
                                                                       ProjectID = a.ProjectID.Value,
                                                                       ProgramElementID = a.ProgramElementID.Value,
                                                                       ProgramID = a.ProgramID.Value,
                                                                       ContractID = a.ContractID.Value,
                                                                       ChangeOrderID = a.ChangeOrderID.Value,
                                                                       DocumentTypeID = a.DocumentTypeID.Value,
                                                                       DocumentName = a.DocumentName,
                                                                       DocumentDescription = a.DocumentDescription,
                                                                       ExecutionDate = a.ExecutionDate,
                                                                       IsCorrupt = a.IsCorrupt,
                                                                       Size = a.Size,
                                                                       DocumentBase64 = a.DocumentBase64,
                                                                       CreatedBy = a.CreatedBy,
                                                                       LastUpdatedBy = a.LastUpdatedBy,
                                                                       CreatedDate = a.CreatedDate,
                                                                       LastUpdatedDate = a.LastUpdatedDate,
                                                                       DocumentTypeName = c.DocumentTypeName,
                                                                       TrendName = e.TrendDescription,
                                                                       ChangeOrderName = "",
                                                                       ProjectElementName = b.ProgramElementName,
                                                                       ProjectName = d.ProjectName,
                                                                       ContractName = ""

                                                                   }).OrderByDescending(a => a.CreatedDate).ToList();
                    }
                    else if (DocumentSet == "ContractViewAll")
                    {
                        documentList = (from a in ctx.Document
                                        join b in ctx.Program on a.ProgramID equals b.ProgramID
                                        join c in ctx.DocumentType on a.DocumentTypeID equals c.DocumentTypeID
                                        where b.ProgramID == projectID && a.IsDeleted == false
                                        select new DocumentView
                                        {
                                            DocumentID = a.DocumentID,
                                            ProjectID = a.ProjectID.Value,
                                            ProgramElementID = a.ProgramElementID.Value,
                                            ProgramID = a.ProgramID.Value,
                                            ContractID = a.ContractID.Value,
                                            ChangeOrderID = a.ChangeOrderID.Value,
                                            DocumentTypeID = a.DocumentTypeID.Value,
                                            DocumentName = a.DocumentName,
                                            DocumentDescription = a.DocumentDescription,
                                            ExecutionDate = a.ExecutionDate,
                                            IsCorrupt = a.IsCorrupt,
                                            Size = a.Size,
                                            DocumentBase64 = a.DocumentBase64,
                                            CreatedBy = a.CreatedBy,
                                            LastUpdatedBy = a.LastUpdatedBy,
                                            CreatedDate = a.CreatedDate,
                                            LastUpdatedDate = a.LastUpdatedDate,
                                            DocumentTypeName = c.DocumentTypeName,
                                            TrendName = "",
                                            ChangeOrderName = "",
                                            ProjectElementName = "",
                                            ProjectName = "",
                                            ContractName = b.ContractName,
                                            ModificationNumber = a.ModificationNumber // Jignesh-25-02-2021

                                        }).Union(from e in ctx.Program
                                                 join b in ctx.ProgramElement on e.ProgramID equals b.ProgramID
                                                 join a in ctx.Document on b.ProgramElementID equals a.ProgramElementID
                                                 join c in ctx.DocumentType on a.DocumentTypeID equals c.DocumentTypeID
                                                 where e.ProgramID == projectID && a.IsDeleted == false
                                                 select new DocumentView
                                                 {
                                                     DocumentID = a.DocumentID,
                                                     ProjectID = a.ProjectID.Value,
                                                     ProgramElementID = a.ProgramElementID.Value,
                                                     ProgramID = a.ProgramID.Value,
                                                     ContractID = a.ContractID.Value,
                                                     ChangeOrderID = a.ChangeOrderID.Value,
                                                     DocumentTypeID = a.DocumentTypeID.Value,
                                                     DocumentName = a.DocumentName,
                                                     DocumentDescription = a.DocumentDescription,
                                                     ExecutionDate = a.ExecutionDate,
                                                     IsCorrupt = a.IsCorrupt,
                                                     Size = a.Size,
                                                     DocumentBase64 = a.DocumentBase64,
                                                     CreatedBy = a.CreatedBy,
                                                     LastUpdatedBy = a.LastUpdatedBy,
                                                     CreatedDate = a.CreatedDate,
                                                     LastUpdatedDate = a.LastUpdatedDate,
                                                     DocumentTypeName = c.DocumentTypeName,
                                                     TrendName = "",
                                                     ChangeOrderName = "",
                                                     ProjectElementName = b.ProgramElementName,
                                                     ProjectName = "",
                                                     ContractName = e.ContractName,
                                                     ModificationNumber = a.ModificationNumber // Jignesh-25-02-2021

                                                 }).Union(from e in ctx.Program
                                                          join b in ctx.ProgramElement on e.ProgramID equals b.ProgramID
                                                          join f in ctx.ChangeOrder on b.ProgramElementID equals f.ProgramElementID
                                                          join a in ctx.Document on f.ChangeOrderID equals a.ChangeOrderID
                                                          join c in ctx.DocumentType on a.DocumentTypeID equals c.DocumentTypeID
                                                          where e.ProgramID == projectID && a.IsDeleted == false
                                                          select new DocumentView
                                                          {
                                                              DocumentID = a.DocumentID,
                                                              ProjectID = a.ProjectID.Value,
                                                              ProgramElementID = a.ProgramElementID.Value,
                                                              ProgramID = a.ProgramID.Value,
                                                              ContractID = a.ContractID.Value,
                                                              ChangeOrderID = a.ChangeOrderID.Value,
                                                              DocumentTypeID = a.DocumentTypeID.Value,
                                                              DocumentName = a.DocumentName,
                                                              DocumentDescription = a.DocumentDescription,
                                                              ExecutionDate = a.ExecutionDate,
                                                              IsCorrupt = a.IsCorrupt,
                                                              Size = a.Size,
                                                              DocumentBase64 = a.DocumentBase64,
                                                              CreatedBy = a.CreatedBy,
                                                              LastUpdatedBy = a.LastUpdatedBy,
                                                              CreatedDate = a.CreatedDate,
                                                              LastUpdatedDate = a.LastUpdatedDate,
                                                              DocumentTypeName = "",
                                                              TrendName = "",
                                                              ChangeOrderName = f.ChangeOrderName,
                                                              ProjectElementName = b.ProgramElementName,
                                                              ProjectName = "",
                                                              ContractName = e.ContractName,
                                                              ModificationNumber = a.ModificationNumber // Jignesh-25-02-2021

                                                          }).Union(from e in ctx.Program
                                                                   join b in ctx.ProgramElement on e.ProgramID equals b.ProgramID
                                                                   join f in ctx.Project on b.ProgramElementID equals f.ProgramElementID
                                                                   join a in ctx.Document on f.ProjectID equals a.ProjectID
                                                                   join c in ctx.DocumentType on a.DocumentTypeID equals c.DocumentTypeID
                                                                   where e.ProgramID == projectID && a.TrendNumber == null && a.IsDeleted == false
                                                                   select new DocumentView
                                                                   {
                                                                       DocumentID = a.DocumentID,
                                                                       ProjectID = a.ProjectID.Value,
                                                                       ProgramElementID = a.ProgramElementID.Value,
                                                                       ProgramID = a.ProgramID.Value,
                                                                       ContractID = a.ContractID.Value,
                                                                       ChangeOrderID = a.ChangeOrderID.Value,
                                                                       DocumentTypeID = a.DocumentTypeID.Value,
                                                                       DocumentName = a.DocumentName,
                                                                       DocumentDescription = a.DocumentDescription,
                                                                       ExecutionDate = a.ExecutionDate,
                                                                       IsCorrupt = a.IsCorrupt,
                                                                       Size = a.Size,
                                                                       DocumentBase64 = a.DocumentBase64,
                                                                       CreatedBy = a.CreatedBy,
                                                                       LastUpdatedBy = a.LastUpdatedBy,
                                                                       CreatedDate = a.CreatedDate,
                                                                       LastUpdatedDate = a.LastUpdatedDate,
                                                                       DocumentTypeName = c.DocumentTypeName,
                                                                       TrendName = "",
                                                                       ChangeOrderName = "",
                                                                       ProjectElementName = b.ProgramElementName,
                                                                       ProjectName = f.ProjectName,
                                                                       ContractName = e.ContractName,
                                                                       ModificationNumber = a.ModificationNumber // Jignesh-25-02-2021

                                                                   }).Union(from e in ctx.Program
                                                                            join b in ctx.ProgramElement on e.ProgramID equals b.ProgramID
                                                                            join f in ctx.Project on b.ProgramElementID equals f.ProgramElementID
                                                                            join d in ctx.Trend on f.ProjectID equals d.ProjectID
                                                                            //join a in ctx.Document on d.TrendNumber equals a.TrendNumber.ToString()
                                                                            join a in ctx.Document on new { X1 = d.TrendNumber, X2 = d.ProjectID.ToString() } equals new { X1 = a.TrendNumber.ToString(), X2 = a.ProjectID.ToString() }
                                                                            join c in ctx.DocumentType on a.DocumentTypeID equals c.DocumentTypeID
                                                                            where e.ProgramID == projectID && a.TrendNumber != null && a.IsDeleted == false
                                                                            select new DocumentView
                                                                            {
                                                                                DocumentID = a.DocumentID,
                                                                                ProjectID = a.ProjectID.Value,
                                                                                ProgramElementID = a.ProgramElementID.Value,
                                                                                ProgramID = a.ProgramID.Value,
                                                                                ContractID = a.ContractID.Value,
                                                                                ChangeOrderID = a.ChangeOrderID.Value,
                                                                                DocumentTypeID = a.DocumentTypeID.Value,
                                                                                DocumentName = a.DocumentName,
                                                                                DocumentDescription = a.DocumentDescription,
                                                                                ExecutionDate = a.ExecutionDate,
                                                                                IsCorrupt = a.IsCorrupt,
                                                                                Size = a.Size,
                                                                                DocumentBase64 = a.DocumentBase64,
                                                                                CreatedBy = a.CreatedBy,
                                                                                LastUpdatedBy = a.LastUpdatedBy,
                                                                                CreatedDate = a.CreatedDate,
                                                                                LastUpdatedDate = a.LastUpdatedDate,
                                                                                DocumentTypeName = c.DocumentTypeName,
                                                                                TrendName = d.TrendDescription,
                                                                                ChangeOrderName = "",
                                                                                ProjectElementName = b.ProgramElementName,
                                                                                ProjectName = f.ProjectName,
                                                                                ContractName = e.ContractName,
                                                                                ModificationNumber = a.ModificationNumber // Jignesh-25-02-2021

                                                                            }).OrderByDescending(a => a.CreatedDate).ToList();
                    }
                    else if (DocumentSet == "ProjectElementViewAll")
                    {
                        documentList = (
                                                          from d in ctx.Project
                                                          join a in ctx.Document on d.ProjectID equals a.ProjectID
                                                          join c in ctx.DocumentType on a.DocumentTypeID equals c.DocumentTypeID
                                                          where d.ProjectID == projectID && a.TrendNumber == null && a.IsDeleted == false
                                                          select new DocumentView
                                                          {
                                                              DocumentID = a.DocumentID,
                                                              ProjectID = a.ProjectID.Value,
                                                              ProgramElementID = a.ProgramElementID.Value,
                                                              ProgramID = a.ProgramID.Value,
                                                              ContractID = a.ContractID.Value,
                                                              ChangeOrderID = a.ChangeOrderID.Value,
                                                              DocumentTypeID = a.DocumentTypeID.Value,
                                                              DocumentName = a.DocumentName,
                                                              DocumentDescription = a.DocumentDescription,
                                                              ExecutionDate = a.ExecutionDate,
                                                              IsCorrupt = a.IsCorrupt,
                                                              Size = a.Size,
                                                              DocumentBase64 = a.DocumentBase64,
                                                              CreatedBy = a.CreatedBy,
                                                              LastUpdatedBy = a.LastUpdatedBy,
                                                              CreatedDate = a.CreatedDate,
                                                              LastUpdatedDate = a.LastUpdatedDate,
                                                              DocumentTypeName = c.DocumentTypeName,
                                                              TrendName = "",
                                                              ChangeOrderName = "",
                                                              ProjectElementName = "",
                                                              ProjectName = d.ProjectName,
                                                              ContractName = ""

                                                          }).Union(from d in ctx.Project
                                                                   join e in ctx.Trend on d.ProjectID equals e.ProjectID
                                                                   //join a in ctx.Document on e.TrendNumber equals a.TrendNumber.ToString() 
                                                                   join a in ctx.Document on new { X1 = e.TrendNumber, X2 = e.ProjectID.ToString() } equals new { X1 = a.TrendNumber.ToString(), X2 = a.ProjectID.ToString() }
                                                                   join c in ctx.DocumentType on a.DocumentTypeID equals c.DocumentTypeID
                                                                   where d.ProjectID == projectID && a.TrendNumber != null && a.IsDeleted == false
                                                                   select new DocumentView
                                                                   {
                                                                       DocumentID = a.DocumentID,
                                                                       ProjectID = a.ProjectID.Value,
                                                                       ProgramElementID = a.ProgramElementID.Value,
                                                                       ProgramID = a.ProgramID.Value,
                                                                       ContractID = a.ContractID.Value,
                                                                       ChangeOrderID = a.ChangeOrderID.Value,
                                                                       DocumentTypeID = a.DocumentTypeID.Value,
                                                                       DocumentName = a.DocumentName,
                                                                       DocumentDescription = a.DocumentDescription,
                                                                       ExecutionDate = a.ExecutionDate,
                                                                       IsCorrupt = a.IsCorrupt,
                                                                       Size = a.Size,
                                                                       DocumentBase64 = a.DocumentBase64,
                                                                       CreatedBy = a.CreatedBy,
                                                                       LastUpdatedBy = a.LastUpdatedBy,
                                                                       CreatedDate = a.CreatedDate,
                                                                       LastUpdatedDate = a.LastUpdatedDate,
                                                                       DocumentTypeName = c.DocumentTypeName,
                                                                       TrendName = e.TrendDescription,
                                                                       ChangeOrderName = "",
                                                                       ProjectElementName = "",
                                                                       ProjectName = d.ProjectName,
                                                                       ContractName = ""

                                                                   }).OrderByDescending(a => a.CreatedDate).ToList();
                    }
                    #region what is this?
                    //int x = 0;
                    //documentList = (from doc in ctx.Document
                    //                  join docType in ctx.DocumentType on doc.DocumentTypeID equals docType.DocumentTypeID
                    //                  //join t in dbContext.tbl_Title on e.TID equals t.TID
                    //                  where doc.ProjectID == projectID
                    //                  select new Document
                    //                  {
                    //                      DocumentID = doc.DocumentID,
                    //                      ProjectID = doc.ProjectID,
                    //                      DocumentTypeID = doc.DocumentTypeID,
                    //                      DocumentName = doc.DocumentName,
                    //                      DocumentDescription = doc.DocumentDescription,
                    //                      DocumentBinaryData = doc.DocumentBinaryData,
                    //                      DocumentBase64 = doc.DocumentBase64,
                    //                      CreatedBy = doc.CreatedBy,
                    //                      LastUpdatedBy = doc.LastUpdatedBy,
                    //                      CreatedDate = doc.CreatedDate,
                    //                      LastUpdatedDate = doc.LastUpdatedDate,

                    //                      DocumentTypeName = docType.DocumentTypeName,
                    //                  }).ToList();

                    //employee_FTEDEscList = ctx.Employee.
                    //                Join(ctx.FtePosition, e => e.FTEPositionID, fte => fte.Id, (e, fte) => new { e, fte }).
                    //                OrderBy(x => x.e.Name).
                    //                Select(m => new Employee_FTEDEsc
                    //                {
                    //                    ID = m.e.ID,
                    //                    FTEPositionID = m.e.FTEPositionID,
                    //                    PositionDesc = m.fte.PositionDescription,
                    //                    Name = m.e.Name,
                    //                    HourlyRate = m.e.HourlyRate,
                    //                    isActive = m.e.isActive
                    //                }).
                    //                ToList();
                    #endregion
                }

            }
            catch (Exception ex)
            {
                var stackTrace = new StackTrace(ex, true);
                var line = stackTrace.GetFrame(0).GetFileLineNumber();
                Logger.LogExceptions(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, ex.Message, line.ToString(), Logger.logLevel.Exception);
            }
            finally
            {
            }
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Exit Point", Logger.logLevel.Debug);

            return documentList;
        }


        public static Document GetDocumentByDocID(int documentID)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);

            Document document = new Document();
            try
            {

                using (var ctx = new CPPDbContext())
                {
                    //documentList = ctx.Document.Where(a => a.Project == projectID).OrderBy(a => a.DocumentName).ToList();
                    document = ctx.Document.Where(a => a.DocumentID == documentID && a.IsDeleted == false).FirstOrDefault();

                    //This is for new storage method, else legacy
                    if (document.DocumentBinaryData == null)
                    {
                        //document data list
                        List<DocumentData> documentDataList = ctx.DocumentData.Where(b => b.DocumentID == documentID).OrderBy(c => c.Order).ToList();

                        //Append binary data here if null
                        IEnumerable<byte> fullDocumentData = Enumerable.Empty<byte>();

                        for (int x = 0; x < documentDataList.Count; x++)
                        {
                            fullDocumentData = fullDocumentData.Concat(documentDataList[x].DocumentBinaryData);
                        }

                        document.DocumentBinaryData = fullDocumentData.ToArray();
                    }

                    if (document.DocumentID == 0)
                    {
                        throw new Exception("DocumentID" + documentID.ToString() + " not found.");
                    }
                }

            }
            catch (Exception ex)
            {
                var stackTrace = new StackTrace(ex, true);
                var line = stackTrace.GetFrame(0).GetFileLineNumber();
                Logger.LogExceptions(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, ex.Message, line.ToString(), Logger.logLevel.Exception);
            }
            finally
            {
            }
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Exit Point", Logger.logLevel.Debug);

            return document;
        }

        public static String registerDocument(Document document)
        {

            //Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            //Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String result = "";
            Document documentCopy = new Document();
            int newlyCreatedDocumentID = 0;
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    Document retrievedDocument = new Document();

                    if(document != null)
                    {

                        if (document.DocumentSet == "Project")
                        {
                            retrievedDocument = ctx.Document.Where(u => u.DocumentName == document.DocumentName && u.ProjectID == document.ProjectID).FirstOrDefault();
                        }
                        else if (document.DocumentSet == "ProgramElement")
                        {
                            retrievedDocument = ctx.Document.Where(u => u.DocumentName == document.DocumentName && u.ProgramElementID == document.ProgramElementID).FirstOrDefault();
                        }
                        else if (document.DocumentSet == "Program")
                        {
                            retrievedDocument = ctx.Document.Where(u => u.DocumentName == document.DocumentName && u.ProgramID == document.ProgramID).FirstOrDefault();
                        }
                        else if (document.DocumentSet == "ProgramContract")
                        {
                            //Delete old list
                            if (document.ContractID != null)
                            {
                                List<Document> documentList = ctx.Document.Where(t => t.ContractID == document.ContractID).ToList();
                                for (int x = 0; x < documentList.Count; x++)
                                {
                                    deleteDocument(documentList[x]);
                                }
                                retrievedDocument = null;
                            }
                            else
                            {
                                retrievedDocument = ctx.Document.Where(u => u.DocumentName == document.DocumentName && u.ContractID == document.ContractID).FirstOrDefault();
                            }
                        }
                        else if (document.DocumentSet == "ProgramElementChangeOrder")
                        {
                            //Delete old list
                            if (document.ChangeOrderID != null)
                            {
                                List<Document> documentList = ctx.Document.Where(t => t.ChangeOrderID == document.ChangeOrderID).ToList();
                                for (int x = 0; x < documentList.Count; x++)
                                {
                                    deleteDocument(documentList[x]);
                                }
                                retrievedDocument = null;
                            }
                            else
                            {
                                retrievedDocument = ctx.Document.Where(u => u.DocumentName == document.DocumentName && u.ChangeOrderID == document.ChangeOrderID).FirstOrDefault();
                            }
                        }

                        if (retrievedDocument == null)
                        {
                            //Copy saved for uploading document data only
                            documentCopy = (Document)document.MemberwiseClone();

                            //Set size
                            document.Size = document.DocumentBinaryData.Length;

                            //Save null to the data part in the document table
                            document.DocumentBinaryData = null;

                            //Assume corrupt until all data loaded
                            document.IsCorrupt = true;

                            //register
                            ctx.Document.Add(document);

                            ctx.SaveChanges();

                            newlyCreatedDocumentID = document.DocumentID;

                            //result = "Success";
                            result += "File uploaded successfully";

                            //Upload only document data here
                            if (documentCopy.DocumentBinaryData != null)
                            {
                                String result2 = DocumentData.registerDocumentData(documentCopy, newlyCreatedDocumentID);
                                if (result2 != "Success")
                                    result = result2;
                            }
                        }
                        else
                        {
                            result += "A file with same name is already uploaded. If you want to upload this file, please rename the file and then try again.";
                        }

                    }

					
                }
            }
            catch (Exception ex)
            {
                var stackTrace = new StackTrace(ex, true);
                var line = stackTrace.GetFrame(0).GetFileLineNumber();
                result = ex.ToString();
            }

            return result;
        }
        public static String updateDocument(Document document)
        {
            //Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            //Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String result = "";

            try
            {
                using (var ctx = new CPPDbContext())
                {
                    Document retrievedDocument = new Document();
                    retrievedDocument = ctx.Document.Where(u => u.DocumentID == document.DocumentID).FirstOrDefault();
                    if (retrievedDocument != null)
                    {
                        //update
                        using (var dbCtx = new CPPDbContext())
                        {
                            var createDate = retrievedDocument.CreatedDate;
                            var modificationnumber = retrievedDocument.ModificationNumber;
                            if (document.FileTye == null)
                            {
                                document.FileTye = retrievedDocument.FileTye;
                            }
                            if (document.DocumentBase64 == null)
                            {
                                document.DocumentBase64 = retrievedDocument.DocumentBase64;
                            }
                            if (document.DocumentBinaryData == null)
                            {
                                document.DocumentBinaryData = retrievedDocument.DocumentBinaryData;
                            }
                            retrievedDocument = document;
                            retrievedDocument.CreatedDate = createDate;
                            retrievedDocument.ModificationNumber = modificationnumber;
                            
                            dbCtx.Entry(retrievedDocument).State = System.Data.Entity.EntityState.Modified;
                            dbCtx.SaveChanges();
                            result += retrievedDocument.DocumentName + " has been updated successfully.\n";
                        }
                    }
                    else
                    {
                        result += retrievedDocument.DocumentName + " failed to be updated, it does not exist.\n";
                    }
                }

            }
            catch (Exception ex)
            {
                var stackTrace = new StackTrace(ex, true);
                var line = stackTrace.GetFrame(0).GetFileLineNumber();
                Logger.LogExceptions(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, ex.Message, line.ToString(), Logger.logLevel.Exception);


            }
            finally
            {

            }
           // Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Exit Point", Logger.logLevel.Debug);

            return result;

        }
        public static String deleteDocument(Document document)
        {
            //Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            //Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String result = "";
            String entryName = "";
            bool isCaughtException = true;
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    Document retrievedDocument = new Document();
                    retrievedDocument = ctx.Document.Where(u => u.DocumentID == document.DocumentID).FirstOrDefault();
                    List <DocumentData> retrievedDocumentDataList = ctx.DocumentData.Where(w => w.DocumentID == document.DocumentID).ToList();
                    entryName = retrievedDocument.DocumentName;
                    if (retrievedDocument != null)
                    {
                        //Delete the document data records
                        for (int x = 0; x < retrievedDocumentDataList.Count; x++)
                        {
                            retrievedDocumentDataList[x].IsDeleted = true;
                            retrievedDocumentDataList[x].DeletedDate = DateTime.Now;
                            retrievedDocumentDataList[x].DeletedBy = document.DeletedBy;

                            //ctx.DocumentData.Remove(retrievedDocumentDataList[x]);
                            ctx.SaveChanges();
                        }

                        //Delete the document record
                        retrievedDocument.IsDeleted = true;
                        retrievedDocument.DeletedDate = DateTime.Now;
                        retrievedDocument.DeletedBy = document.DeletedBy;

                        //ctx.Document.Remove(retrievedDocument);
                        ctx.SaveChanges();

                        result += retrievedDocument.DocumentName + " has been deleted successfully.\n";
                        isCaughtException = false;
                    }
                    else
                    {
                        result += retrievedDocument.DocumentName + " failed to be deleted, it does not exist.\n";
                        isCaughtException = false;
                    }
                }

            }
            catch (Exception ex)
            {
                isCaughtException = true;
                var stackTrace = new StackTrace(ex, true);
                var line = stackTrace.GetFrame(0).GetFileLineNumber();
                Logger.LogExceptions(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, ex.Message, line.ToString(), Logger.logLevel.Exception);
            }
            finally
            {
                if (isCaughtException)
                {
                    result += entryName + " failed to be deleted due to dependencies.\n";
                }
            }
            //Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Exit Point", Logger.logLevel.Debug);

            return result;
        }

        public static String deleteDocumentByDocIDs(List<int> docIDs)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String result = "";
            String entryName = "";
            
            bool isCaughtException = true;
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    foreach(int docID in docIDs)
                    { 
                        Document retrievedDocument = new Document();
                        retrievedDocument = ctx.Document.Where(u => u.DocumentID == docID).FirstOrDefault();
                        List<DocumentData> retrievedDocumentDataList = ctx.DocumentData.Where(w => w.DocumentID == docID).ToList();

                        entryName = retrievedDocument.DocumentName;
                        if (retrievedDocument != null)
                        {

                            //Delete the document data records
                            for (int x = 0; x < retrievedDocumentDataList.Count; x++)
                            {
                                ctx.DocumentData.Remove(retrievedDocumentDataList[x]);
                                ctx.SaveChanges();
                            }

                            ctx.Document.Remove(retrievedDocument);
                            //ctx.SaveChanges();
                            result += retrievedDocument.DocumentName + " has been deleted successfully.\n";
                            isCaughtException = false;
                        }
                        else
                        {
                            result += retrievedDocument.DocumentName + " failed to be deleted, it does not exist.\n";
                            isCaughtException = false;
                        }
                    }

                    ctx.SaveChanges();
                    result = "Deleted";

                }

            }
            catch (Exception ex)
            {
                isCaughtException = true;
                var stackTrace = new StackTrace(ex, true);
                var line = stackTrace.GetFrame(0).GetFileLineNumber();
                Logger.LogExceptions(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, ex.Message, line.ToString(), Logger.logLevel.Exception);
            }
            finally
            {
                if (isCaughtException)
                {
                    result += entryName + " failed to be deleted due to dependencies.\n";
                }
            }
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Exit Point", Logger.logLevel.Debug);

            return result;
        }

        public static String updateDocumentImage(Document document, Byte[] byteData)
        {
            String result = "";

            try
            {
                MySqlConnection con = new MySqlConnection();
                con.ConnectionString = ConfigurationManager.ConnectionStrings["CPP_MySQL"].ConnectionString;
                // Create SQL Command 
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = "update document set DocumentBinaryData = @DocumentBinaryData where DocumentID = @DocumentID ";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = con;
                MySqlParameter DocumentID = new MySqlParameter("@DocumentID", MySqlDbType.Int64);
                DocumentID.Value = document.DocumentID;
                cmd.Parameters.Add(DocumentID);
                MySqlParameter DocumentBinaryData = new MySqlParameter("@DocumentBinaryData", MySqlDbType.LongBlob, byteData.Length);// document.DocumentBinaryData.Length);

                DocumentBinaryData.Value = byteData; // document.DocumentBinaryData;
                cmd.Parameters.Add(DocumentBinaryData);
                con.Open();
                int result2 = cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            finally
            {
                
            }

            return result;
        }

        // Jignesh-TDM-06-01-2020
        public static String registerTrendDocument(Document document)
        {
            String result = "";
            Document documentCopy = new Document();
            int newlyCreatedDocumentID = 0;
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    Document retrievedDocument = new Document();
                    
                    retrievedDocument = ctx.Document.Where(u => u.DocumentName == document.DocumentName && u.ProjectID == document.ProjectID && u.TrendNumber == document.TrendNumber).FirstOrDefault();
                    if (retrievedDocument == null)
                    {
                        //Copy saved for uploading document data only
                        documentCopy = (Document)document.MemberwiseClone();

                        //Set size
                        document.Size = document.DocumentBinaryData.Length;

                        //Save null to the data part in the document table
                        document.DocumentBinaryData = null;

                        //Assume corrupt until all data loaded
                        document.IsCorrupt = true;

                        //register
                        ctx.Document.Add(document);

                        ctx.SaveChanges();

                        newlyCreatedDocumentID = document.DocumentID;

                        //result = "Success";
                        result += "File uploaded successfully";

                        //Upload only document data here
                        if (documentCopy.DocumentBinaryData != null)
                        {
                            String result2 = DocumentData.registerDocumentData(documentCopy, newlyCreatedDocumentID);
                            if (result2 != "Success")
                                result = result2;
                        }
                    }
                    else
                    {
                        result += "A file with same name is already uploaded. If you want to upload this file, please rename the file and then try again.";
                    }
                }
            }
            catch (Exception ex)
            {
                var stackTrace = new StackTrace(ex, true);
                var line = stackTrace.GetFrame(0).GetFileLineNumber();
                result = ex.ToString();
            }

            return result;
        }
        //---------------------------------------------------------------------------------------------------
        //==================================== Jignesh-ModificationPopUpChanges ========================================
        public static String registerModificationDocument(Document document)
        {
            String result = "";
            Document documentCopy = new Document();
            int newlyCreatedDocumentID = 0;
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    Document retrievedDocument = new Document();
                    
                    retrievedDocument = ctx.Document.Where(u => u.DocumentName == document.DocumentName && u.ProgramID == document.ProgramID && u.ModificationNumber == document.ModificationNumber).FirstOrDefault();
                    if (retrievedDocument == null)
                    {
                        //Copy saved for uploading document data only
                        documentCopy = (Document)document.MemberwiseClone();

                        //Set size
                        document.Size = document.DocumentBinaryData.Length;

                        //Save null to the data part in the document table
                        document.DocumentBinaryData = null;

                        //Assume corrupt until all data loaded
                        document.IsCorrupt = true;

                        //register
                        ctx.Document.Add(document);

                        ctx.SaveChanges();

                        newlyCreatedDocumentID = document.DocumentID;

                        //result = "Success";
                        result += "File uploaded successfully";

                        //Upload only document data here
                        if (documentCopy.DocumentBinaryData != null)
                        {
                            String result2 = DocumentData.registerDocumentData(documentCopy, newlyCreatedDocumentID);
                            if (result2 != "Success")
                                result = result2;
                        }
                    }
                    else
                    {
                        result += "A file with same name is already uploaded. If you want to upload this file, please rename the file and then try again.";
                    }
                }
            }
            catch (Exception ex)
            {
                var stackTrace = new StackTrace(ex, true);
                var line = stackTrace.GetFrame(0).GetFileLineNumber();
                result = ex.ToString();
            }

            return result;
        }
        //===================================================================================================================
    }
}