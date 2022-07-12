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
    [Table("document_data")]
    public class DocumentData
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DocumentDataID { get; set; }
        public int DocumentID { get; set; }
        public int Order { get; set; }
        public byte[] DocumentBinaryData { get; set; }
        public int? Size { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string DeletedBy { get; set; }

        //ON DELETE RESTRICT
        //[ForeignKey("ProjectID")]
        //public virtual Project Project { get; set; }


        public static List<DocumentData> GetDocumentData(int documentID)
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

            return null;    //TODO
        }

        public static String registerDocumentData(Document document, int documentID)
        {
            String result = "";
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    //Luan experimental - split document data here
                    //ctx.Database.ExecuteSqlCommand("SET GLOBAL max_allowed_packet=1024*1024*1024;");
                    //System.ArraySegment<byte> test = new ArraySegment<byte>(document.DocumentBinaryData, x, 10000);
                    int perUploadSize = 3000000;
                    int blockLength = 0;
                    int dataLengthLeft = document.DocumentBinaryData.Length;
                    int i = 0;
                    int order = 1;

                    //The whole point is to find blockLength, cannot have fixed size blockLength
                    while (dataLengthLeft > 0)
                    {
                        //Last part
                        if (dataLengthLeft <= perUploadSize)
                        {
                            blockLength = dataLengthLeft;
                        }
                        else //Keep uploading
                        {
                            blockLength = perUploadSize;
                        }

                        byte[] buffer = new byte[blockLength];
                        Buffer.BlockCopy(document.DocumentBinaryData, i, buffer, 0, blockLength);

                        //Insert into Document Data table here
                        DocumentData dd = new DocumentData();
                        dd.DocumentBinaryData = buffer;
                        dd.DocumentID = documentID;
                        dd.Order = order;
                        dd.Size = blockLength;
                        ctx.DocumentData.Add(dd);

                        ctx.SaveChanges();

                        Debug.WriteLine(buffer);

                        dataLengthLeft -= perUploadSize;
                        i += blockLength;
                        order++;
                    }

                    //Mark IsCorrupt false
                    Document retrievedDocument = ctx.Document.Where(u => u.DocumentID == documentID).FirstOrDefault();
                    retrievedDocument.IsCorrupt = false;
                    ctx.Entry(retrievedDocument).State = System.Data.Entity.EntityState.Modified;
                    ctx.SaveChanges();

                    result = "Success";
                }
            }
            catch (Exception ex)
            {
                var stackTrace = new StackTrace(ex, true);
                var line = stackTrace.GetFrame(0).GetFileLineNumber();
                result = ex.ToString();

                //Delete document on fail
                Document.deleteDocument(document);
            }

            return result;
        }

        public static String deleteDocumentData(Document document)
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
                    entryName = retrievedDocument.DocumentName;
                    if (retrievedDocument != null)
                    {
                        ctx.Document.Remove(retrievedDocument);
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
    }
}