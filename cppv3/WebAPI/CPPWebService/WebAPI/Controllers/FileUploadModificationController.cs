using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class FileUploadModificationController : ApiController
    {
        private readonly string workingFolder = HttpRuntime.AppDomainAppPath + @"\Uploads\";

        [HttpPost]
        public async Task<HttpResponseMessage> Post(int? programID, int? ModId,
             int? docTypeID, string SpecialNote, String DocumentName)
        {
            // Jignesh-24-02-2021 Remove parameter ", string ExecutionDate" from above
            var ctx = new CPPDbContext();
            if (Request.Content.IsMimeMultipartContent())
            {
                string fileName = "";
                if (!Directory.Exists(workingFolder)) Directory.CreateDirectory(workingFolder);
                var streamProvider = new MultipartFormDataStreamProvider(workingFolder);

                string result = "";
                try
                {
                    await Request.Content.ReadAsMultipartAsync(streamProvider);

                    foreach (MultipartFileData fileData in streamProvider.FileData)
                    {
                        if (string.IsNullOrEmpty(fileData.Headers.ContentDisposition.FileName))
                        {
                            return Request.CreateResponse(HttpStatusCode.NotAcceptable, "This request is not properly formatted");
                        }

                        fileName = fileData.Headers.ContentDisposition.FileName;

                        fileName = DocumentName;

                        if (fileName.StartsWith("\"") && fileName.EndsWith("\""))
                        {
                            fileName = fileName.Trim('"');
                        }
                        if (fileName.Contains(@"/") || fileName.Contains(@"\"))
                        {
                            fileName = Path.GetFileName(fileName);
                        }

                        File.Move(fileData.LocalFileName, Path.Combine(workingFolder, fileName));

                        Document doc = new Document();
                        doc.Operation = 1;

                        doc.ProgramID = programID;
                        doc.ModificationNumber = ModId;
                        doc.DocumentTypeID = docTypeID;
                        //doc.DocumentSet = DocumentSet;

                        var ProjectCreatedBy = from pr in ctx.Program
                                               where pr.ProgramID == programID
                                               select pr.CreatedBy;
                        doc.CreatedBy = ProjectCreatedBy.FirstOrDefault();

                        doc.ProgramElementID = null;

                        if (!String.IsNullOrEmpty(SpecialNote))
                        {
                            doc.DocumentDescription = SpecialNote;
                        }

                        // Jignesh-24-02-2021 Comment below sectoin
                        //if (!String.IsNullOrEmpty(ExecutionDate))
                        //{
                        //    String abc = ExecutionDate;

                        //    for (int i = 2; i <= abc.Length - 3; i += 2)
                        //    {
                        //        abc = abc.Insert(i, "/");
                        //        i++;
                        //    }

                        //    doc.ExecutionDate = abc;
                        //}

                        doc.ProjectID = null;

                        doc.ContractID = null;

                        doc.ChangeOrderID = null;

                        if (docTypeID == 0)
                        {
                            doc.DocumentTypeID = null;
                        }
                        else
                        {
                            doc.DocumentTypeID = docTypeID;
                        }

                        doc.DocumentName = fileName;

                        doc.DocumentName = DocumentName;

                        string StrContentType = fileData.Headers.ContentType.MediaType;
                        doc.FileTye = StrContentType;

                        //Base64Encode
                        var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(fileName);
                        doc.DocumentBase64 = System.Convert.ToBase64String(plainTextBytes);

                        //Revert it back test
                        //Base64Decode
                        var base64EncodedBytes = System.Convert.FromBase64String(doc.DocumentBase64);
                        var planText = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);

                        //Get Binary Data from file
                        byte[] byteData;
                        FileStream fs = new FileStream(Path.Combine(workingFolder, fileName), FileMode.Open, FileAccess.Read);
                        BinaryReader br = new BinaryReader(fs);
                        byteData = br.ReadBytes((int)fs.Length);
                        br.Close();
                        fs.Close();
                        doc.DocumentBinaryData = byteData;

                        doc.CreatedDate = DateTime.Now;

                        result = WebAPI.Models.Document.registerModificationDocument(doc);

                        File.Delete(Path.Combine(workingFolder, fileName));

                    }

                    return Request.CreateResponse(HttpStatusCode.OK, result);
                }
                catch (Exception ex)
                {
                    var stackTrace = new StackTrace(ex, true);
                    var line = stackTrace.GetFrame(0).GetFileLineNumber();
                    return Request.CreateResponse(HttpStatusCode.NotAcceptable, "Error: " + ex.Message + "line: " + line);
                }
                finally
                {

                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotAcceptable, "This request is not properly formatted");
            }
        }





        [HttpGet]
        [Route("Request/getDocument/")]
        public HttpResponseMessage GetDocument(int documentId)
        {

            //string fileName = Path.GetFileName(path);
            Document document = new Document();
            document = WebAPI.Models.Document.GetDocumentByDocID(documentId);

            string base64String = Convert.ToBase64String(document.DocumentBinaryData);

            var jsonNew = new
            {
                //result = Path.Combine(filePath, fileName)
                result = base64String,
                fileName = document.DocumentName
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}
