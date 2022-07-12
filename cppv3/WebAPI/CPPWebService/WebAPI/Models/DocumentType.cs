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
using WebAPI.Helper;
using WebAPI.Models;

namespace WebAPI.Models
{
    [Table("document_type")]
    public class DocumentType : Audit
    {
        [NotMapped]
        public int Operation;

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DocumentTypeID { get; set; }
        public String DocumentTypeName { get; set; }
        public String DocumentTypeDescription { get; set; }
        //public String CreatedBy { get; set; }
        //public String LastUpdatedBy { get; set; }
        //public DateTime CreatedDate { get; set; }
        //public DateTime LastUpdatedDate { get; set; }

        public static List<DocumentType> GetDocumentType()
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);

            List<DocumentType> documentTypeList = new List<DocumentType>();
            try
            {

                using (var ctx = new CPPDbContext())
                {
                    documentTypeList = ctx.DocumentType.OrderBy(a => a.DocumentTypeName).ToList();
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

            return documentTypeList;
        }


        public static String registerDocumentType(DocumentType documentType)
        {

            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String result = "";
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    DocumentType retrievedDocumentType = new DocumentType();
                    retrievedDocumentType = ctx.DocumentType.Where(u => u.DocumentTypeName == documentType.DocumentTypeName).FirstOrDefault();

                    if (retrievedDocumentType == null)
                    {
                        //register
                        ctx.DocumentType.Add(documentType);
                        ctx.SaveChanges();
                        //result = "Success";
                        result += documentType.DocumentTypeName + " has been created successfully.\n";
                    }
                    else
                    {
                        result += documentType.DocumentTypeName + "' failed to be created, it already exist.\n";
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

            return result;
        }
        public static String updateDocumentType(DocumentType documentType)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String result = "";

            try
            {
                using (var ctx = new CPPDbContext())
                {
                    DocumentType retrievedDocumentType = new DocumentType();
                    retrievedDocumentType = ctx.DocumentType.Where(u => u.DocumentTypeID == documentType.DocumentTypeID).FirstOrDefault();

                    DocumentType duplicateDocumentType = new DocumentType();
                    duplicateDocumentType = ctx.DocumentType.Where(u => u.DocumentTypeName == documentType.DocumentTypeName
                                                                        && u.DocumentTypeID != documentType.DocumentTypeID).FirstOrDefault();

                    if(duplicateDocumentType != null)
                    {
                        result += documentType.DocumentTypeName + " failed to be updated, duplicate will be created.\n";
                    }
                    else if (retrievedDocumentType != null)
                    {
                        CopyUtil.CopyFields<DocumentType>(documentType, retrievedDocumentType);
                        ctx.Entry(retrievedDocumentType).State = System.Data.Entity.EntityState.Modified;
                        ctx.SaveChanges();
                        result += documentType.DocumentTypeName + " has been updated successfully.\n";
                    }
                    else
                    {
                        result += documentType.DocumentTypeName + " failed to be updated, it does not exist.\n";
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

            return result;

        }
        public static String deleteDocumentType(DocumentType documentType)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String result = "";
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    DocumentType retrievedDocumentType = new DocumentType();
                    retrievedDocumentType = ctx.DocumentType.Where(u => u.DocumentTypeID == documentType.DocumentTypeID).FirstOrDefault();

                    if (retrievedDocumentType != null)
                    {
                        ctx.DocumentType.Remove(retrievedDocumentType);
                        ctx.SaveChanges();
                        result += documentType.DocumentTypeName + " has been deleted successfully.\n";
                    }
                    else
                    {
                        result += documentType.DocumentTypeName + " failed to be deleted, it does not exist.\n";
                    }
                }

            }
            catch (Exception ex)
            {
                result += documentType.DocumentTypeName + " failed to be deleted due to dependencies.\n";
                var stackTrace = new StackTrace(ex, true);
                var line = stackTrace.GetFrame(0).GetFileLineNumber();
                Logger.LogExceptions(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, ex.Message, line.ToString(), Logger.logLevel.Exception);
            }
            finally
            {
            }
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Exit Point", Logger.logLevel.Debug);

            return result;
        }
    }
}