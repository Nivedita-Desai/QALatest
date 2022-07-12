using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;



using System.IO;
using System.Threading.Tasks;
using System.Web;


using System.Diagnostics;
using System.Net.Sockets;
using System.Reflection;

namespace WebAPI.Controllers
{
    public class FileUploadNewController : ApiController
    {
        private readonly string workingFolder = HttpRuntime.AppDomainAppPath + @"\Uploads\";

        [HttpPost]
        public async Task<HttpResponseMessage> Post(String DocumentSet, int? projectID, int? ProgramElementID, int? ProgramID,
            int? ContractID, int? ChangeOrderID, int? docTypeID, string SpecialNote, String DocumentName, int DocID = 0, int editOperation = 0, bool noFile = false)
        {
            // Jignesh-24-02-2021 Remove parameter ", string ExecutionDate" from above
            //Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            //Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            var ctx = new CPPDbContext();
            if (Request.Content.IsMimeMultipartContent())
            {
                string fileName = "";
                if (!Directory.Exists(workingFolder)) Directory.CreateDirectory(workingFolder);
                var streamProvider = new MultipartFormDataStreamProvider(workingFolder);
                Document doc = new Document();

                string result = "";
                try
                {
                    if (!noFile)
                    {
                        await Request.Content.ReadAsMultipartAsync(streamProvider);
                    }

                    foreach (MultipartFileData fileData in streamProvider.FileData)
                    {
                        if (string.IsNullOrEmpty(fileData.Headers.ContentDisposition.FileName))
                        {
                            return Request.CreateResponse(HttpStatusCode.NotAcceptable, "This request is not properly formatted");
                        }
                        // string FileType = fileData.Headers.ContentType;    


                        fileName = fileData.Headers.ContentDisposition.FileName;

                        //Luan filename override
                        fileName = DocumentName;

                        if (fileName.StartsWith("\"") && fileName.EndsWith("\""))
                        {
                            fileName = fileName.Trim('"');
                        }
                        if (fileName.Contains(@"/") || fileName.Contains(@"\"))
                        {
                            fileName = Path.GetFileName(fileName);
                        }
                        //   File.Delete(Path.Combine(workingFolder, fileName));
                        File.Move(fileData.LocalFileName, Path.Combine(workingFolder, fileName));

                        // var ContentTypes = streamProvider.FileData.Select(entry => entry.Headers.ContentType.MediaType);
                        string StrContentType = fileData.Headers.ContentType.MediaType;
                        doc.FileTye = StrContentType;

                        //Base64Encode
                        var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(fileName);
                        doc.DocumentBase64 = System.Convert.ToBase64String(plainTextBytes);

                        //Revert it back test
                        //Base64Decode
                        var base64EncodedBytes = System.Convert.FromBase64String(doc.DocumentBase64);
                        var planText = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);

                        //Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Before get ByteData", Logger.logLevel.Debug);

                        //Get Binary Data from file
                        byte[] byteData;
                        FileStream fs = new FileStream(Path.Combine(workingFolder, fileName), FileMode.Open, FileAccess.Read);
                        BinaryReader br = new BinaryReader(fs);
                        byteData = br.ReadBytes((int)fs.Length);
                        br.Close();
                        fs.Close();
                        doc.DocumentBinaryData = byteData;
                        // Pritesh end
                    }

                    //Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "File Saved", Logger.logLevel.Debug);

                    doc.Operation = 1;

                        doc.ProjectID = projectID;
                        doc.DocumentTypeID = docTypeID;
                        doc.DocumentSet = DocumentSet;

                        if (projectID == 0)
                        {
                            doc.ProjectID = null;
                        }
                        else
                        {
                            doc.ProjectID = projectID;


                            var ProjectCreatedBy = from pr in ctx.Project
                                                   where pr.ProjectID == projectID
                                                   select pr.CreatedBy;
                            doc.CreatedBy = ProjectCreatedBy.FirstOrDefault();

                        }

                        if (ProgramElementID == 0)
                        {
                            doc.ProgramElementID = null;
                        }
                        else
                        {
                            doc.ProgramElementID = ProgramElementID;

                            var ProjectCreatedBy = from pr in ctx.ProgramElement
                                                   where pr.ProgramElementID == ProgramElementID
                                                   select pr.CreatedBy;
                            doc.CreatedBy = ProjectCreatedBy.FirstOrDefault();
                        }

                        if (!String.IsNullOrEmpty(SpecialNote))
                        {
                            doc.DocumentDescription = SpecialNote;
                        }

                        // Jignesh-24-02-2021 Comment below sectoin
                        //if (!String.IsNullOrEmpty(ExecutionDate))
                        //{
                        //    String abc = ExecutionDate;

                        //    for (int i = 2; i <= abc.Length-3; i += 2)
                        //    {
                        //        abc = abc.Insert(i, "/");
                        //        i++;
                        //    }

                        //    doc.ExecutionDate = abc;
                        //}

                        if (ProgramID == 0)
                        {
                            doc.ProgramID = null;
                        }
                        else
                        {
                            doc.ProgramID = ProgramID;

                            var ProjectCreatedBy = from pr in ctx.Program
                                                   where pr.ProgramID == ProgramID
                                                   select pr.CreatedBy;
                            doc.CreatedBy = ProjectCreatedBy.FirstOrDefault();
                        }

                        if (ContractID == 0)
                        {
                            doc.ContractID = null;
                        }
                        else
                        {
                            doc.ContractID = ContractID;
                        }

                        if (ChangeOrderID == 0)
                        {
                            doc.ChangeOrderID = null;
                        }
                        else
                        {
                            doc.ChangeOrderID = ChangeOrderID;
                        }

                        if (docTypeID == 0)
                        {
                            doc.DocumentTypeID = null;
                        }
                        else
                        {
                            doc.DocumentTypeID = docTypeID;
                        }


                        //Pritesh Start
                        doc.DocumentName = fileName;

                        //Luan override
                        doc.DocumentName = DocumentName;

                        doc.DocumentID = DocID;

                    if (editOperation == 1)
                    {
                        doc.LastUpdatedDate = DateTime.Now;
                        result = WebAPI.Models.Document.updateDocument(doc);
                    }
                    else
                    {
                        doc.CreatedDate = DateTime.Now;
                        //Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Calling registerDocument", Logger.logLevel.Debug);
                        result = WebAPI.Models.Document.registerDocument(doc);
                    }
                        //if (result.IndexOf(" has been created successfully").Equals(-1))
                        //{
                        //    throw new Exception(result);
                        //}

                        //Get Binary Data from file
                        //byte[] byteData;
                        //FileStream fs = new FileStream(Path.Combine(workingFolder, fileName), FileMode.Open, FileAccess.Read);
                        //BinaryReader br = new BinaryReader(fs);
                        //byteData = br.ReadBytes((int)fs.Length);
                        //br.Close();
                        //fs.Close();
                        //doc.DocumentBinaryData = byteData;

                        //result = WebAPI.Models.Document.updateDocumentImage(doc, byteData);

                    if(!noFile)
                        File.Delete(Path.Combine(workingFolder, fileName));



                    //}


                    //return Request.CreateResponse(HttpStatusCode.OK, "File Uploaded.");
                    return Request.CreateResponse(HttpStatusCode.OK, result);
                }
                catch (Exception ex)
                {
                    var stackTrace = new StackTrace(ex, true);
                    var line = stackTrace.GetFrame(0).GetFileLineNumber();
                    //Logger.LogExceptions(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, ex.Message, line.ToString(), Logger.logLevel.Exception);
                    return Request.CreateResponse(HttpStatusCode.NotAcceptable, "Error: " + ex.Message + "line: " + line);
                }
                finally
                {
                    //foreach (var file in Directory.GetFiles(workingFolder))
                    //{
                    //    File.Delete(file);
                    //}
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotAcceptable, "This request is not properly formatted");
            }
        }

        

        [HttpPost]
        //Retain oriinal Post rutine.
        public async Task<HttpResponseMessage> ProcessFile(int projectID, int docTypeID, int temp)
        {
            if (Request.Content.IsMimeMultipartContent())
            {
                string fileName = "";
                var streamProvider = new MultipartFormDataStreamProvider(workingFolder);
                await Request.Content.ReadAsMultipartAsync(streamProvider);
                foreach (MultipartFileData fileData in streamProvider.FileData)
                {
                    if (string.IsNullOrEmpty(fileData.Headers.ContentDisposition.FileName))
                    {
                        return Request.CreateResponse(HttpStatusCode.NotAcceptable, "This request is not properly formatted");
                    }
                    fileName = fileData.Headers.ContentDisposition.FileName;
                    if (fileName.StartsWith("\"") && fileName.EndsWith("\""))
                    {
                        fileName = fileName.Trim('"');
                    }
                    if (fileName.Contains(@"/") || fileName.Contains(@"\"))
                    {
                        fileName = Path.GetFileName(fileName);
                    }
                    File.Move(fileData.LocalFileName, Path.Combine(workingFolder, fileName));


                    using (Microsoft.VisualBasic.FileIO.TextFieldParser MyReader = new Microsoft.VisualBasic.FileIO.TextFieldParser(Path.Combine(workingFolder, fileName)))
                    {
                        MyReader.TextFieldType = Microsoft.VisualBasic.FileIO.FieldType.Delimited;
                        MyReader.SetDelimiters(",");
                        string[] currentRow = null;


                        /*
                         * CostType                   Table
                         * Labor                     Cost_fte
                         * Contractor/SubContractor  cost_lumpsum
                         * Material                  cost_unitcost
                         * ODC                       cost_odc
                         * 
                         */
                        //parameters For costFTE
                        int operation = 0;
                        String programID = "";
                        String programElementID = "";
                        //String projectID = "";
                        String trendNumber = "";

                        while (!MyReader.EndOfData)
                        {
                            try
                            {
                                currentRow = MyReader.ReadFields();

                                //foreach (var currentField in currentRow)
                                //{
                                //    //set values for your object here
                                //    Console.WriteLine(currentField);
                                //}

                                CostFTE costFTE = new CostFTE();
                                CostLumpsum costLumpsum = new CostLumpsum();
                                CostUnit costUnit = new CostUnit();
                                CostODC costODC = new CostODC();

                                //This is all make up. Once we have a real csv file, we can remap them.

                                // May need to read from table before assigning new values
                                costFTE.FTECostID = currentRow[0];
                                //costFTE.EmployeeID = int.Parse(currentRow[1]);

                                //string status = WebAPI.Models.CostFTE.updateCostFTE(operation, programID, programElementID, projectID, trendNumber,
                                //                                                    costFTE.ActivityID.ToString(), costFTE.FTECostID, costFTE.FTEStartDate, costFTE.FTEEndDate, costFTE.FTEPosition,
                                //                                                    costFTE.FTEValue, costFTE.FTEHourlyRate, costFTE.FTEHours, costFTE.FTECost, "", //<-Scale costFTE.Granularity, 
                                //                                                    "", //<-FTEIDList costFTE.FTEPositionID, costFTE.CostTrackTypeID, costFTE.EstimatedCostID, 
                                //                                                    costFTE.EmployeeID, "" //<-CostTrackTypes
                                //                                                    );



                            }
                            catch (Microsoft.VisualBasic.FileIO.MalformedLineException ex)
                            {
                                //handle the exception
                            }
                        }
                    }


                    //Proess csv file
                    //using (StreamReader readFile = new StreamReader(Path.Combine(workingFolder, fileName)))
                    //{
                    //    string line;
                    //    string[] row;
                    //    List<string> parsedData = new List<string>();

                    //    //parameters For costFTE
                    //    int operation = 0;
                    //    String programID = "";
                    //    String programElementID = "";
                    //    String projectID = "";
                    //    String trendNumber = "";

                    //    while ((line = readFile.ReadLine()) != null)
                    //    {
                    //        row = line.Split(',');
                    //        //parsedData.Add(row);

                    //        CostFTE costFTE = new CostFTE();
                    //        CostODC costODC = new CostODC();

                    //        //This is all make up. Once we have a real csv file, we can remap them.

                    //        // May need to read from table before assigning new values
                    //        costFTE.FTECostID = row[0];
                    //        costFTE.EmployeeID = int.Parse(row[1]);

                    //        string status = WebAPI.Models.CostFTE.updateCostFTE(operation, programID, programElementID, projectID, trendNumber, 
                    //                                                            costFTE.ActivityID.ToString(), costFTE.FTECostID, costFTE.FTEStartDate, costFTE.FTEEndDate, costFTE.FTEPosition, 
                    //                                                            costFTE.FTEValue, costFTE.FTEHourlyRate, costFTE.FTEHours, costFTE.FTECost, "", //<-Scale costFTE.Granularity, 
                    //                                                            "", //<-FTEIDList costFTE.FTEPositionID, costFTE.CostTrackTypeID, costFTE.EstimatedCostID, 
                    //                                                            costFTE.EmployeeID, "" //<-CostTrackTypes
                    //                                                            );


                    //    }
                    //}

                }


                return Request.CreateResponse(HttpStatusCode.OK, "File Uploaded.");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotAcceptable, "This request is not properly formatted");
            }
        }

        [HttpGet]
        public HttpResponseMessage GetIP()
        {
            string myIP = "";

            IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (IPAddress addr in localIPs)
            {
                if (addr.AddressFamily == AddressFamily.InterNetwork)
                {
                    myIP += addr + ", ";
                }
            }


            var jsonNew = new
            {
                result = myIP
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
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