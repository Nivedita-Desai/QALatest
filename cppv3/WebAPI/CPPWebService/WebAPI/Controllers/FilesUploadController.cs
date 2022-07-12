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
    public class FilesUploadController : ApiController
    {
        private readonly string workingFolder = HttpRuntime.AppDomainAppPath + @"\Uploads\";

        [HttpPost]
        public async Task<HttpResponseMessage> Post(String DocumentSet, int? projectID, int? ProgramElementID, int? ProgramID, int? ContractID,
            int? ChangeOrderID, int? docTypeID)
        {
            //Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            //Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);

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

                        //Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "File Saved", Logger.logLevel.Debug);
                        Document doc = new Document();
                        doc.Operation = 1;

						doc.ProjectID = projectID;
						doc.DocumentTypeID = docTypeID;
						doc.DocumentSet = DocumentSet;

						if (projectID == 0)
                        {
							doc.ProjectID = null;
                        }
						else {
							doc.ProjectID = projectID;
						}

						if (ProgramElementID == 0)
						{
							doc.ProgramElementID = null;
						}
						else
						{
							doc.ProgramElementID = ProgramElementID;
						}

						if (ProgramID == 0)
						{
							doc.ProgramID = null;
						}
						else
						{
							doc.ProgramID = ProgramID;
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

						doc.DocumentName = fileName;

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

                        doc.CreatedDate = DateTime.Now;

                        //Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Calling registerDocument", Logger.logLevel.Debug);
                        result = WebAPI.Models.Document.registerDocument(doc);
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

                        File.Delete(Path.Combine(workingFolder, fileName));



                    }


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

        //[HttpPost]
        //public async Task<HttpResponseMessage> Post(int projectID, int docTypeID)
        //{
        //    try
        //    {
        //        //var fileuploadPath = ConfigurationManager.AppSettings["FileUploadLocation"];
        //        string result = "";
        //        if (!Directory.Exists(workingFolder)) Directory.CreateDirectory(workingFolder);

        //        var provider = new MultipartFormDataStreamProvider(workingFolder);
        //        var content = new StreamContent(HttpContext.Current.Request.GetBufferlessInputStream(true));
        //        foreach (var header in Request.Content.Headers)
        //        {
        //            content.Headers.TryAddWithoutValidation(header.Key, header.Value);
        //        }

        //        await content.ReadAsMultipartAsync(provider);

        //        //var provider = new MultipartFormDataStreamProvider(workingFolder);
        //        //var content = new StreamContent(HttpContext.Current.Request.GetBufferlessInputStream(true));
        //        //foreach (var header in Request.Content.Headers)
        //        //{
        //        //    content.Headers.TryAddWithoutValidation(header.Key, header.Value);
        //        //}

        //        //await content.ReadAsMultipartAsync(provider);

        //        string uploadingFileName = provider.FileData.Select(x => x.LocalFileName).FirstOrDefault();
        //        string originalFileName = String.Concat(workingFolder, "\\" + (provider.Contents[0].Headers.ContentDisposition.FileName).Trim(new Char[] { '"' }));

        //        if (File.Exists(originalFileName))
        //        {
        //            File.Delete(originalFileName);
        //        }

        //        File.Move(uploadingFileName, originalFileName);

        //        string fileName = originalFileName;
        //        if (fileName.StartsWith("\"") && fileName.EndsWith("\""))
        //        {
        //            fileName = fileName.Trim('"');
        //        }
        //        if (fileName.Contains(@"/") || fileName.Contains(@"\"))
        //        {
        //            fileName = Path.GetFileName(fileName);
        //        }
        //        Document doc = new Document();
        //        doc.Operation = 1;
        //        doc.ProjectID = projectID;
        //        doc.DocumentTypeID = docTypeID;
        //        doc.DocumentName = fileName;

        //        //Base64Encode
        //        var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(fileName);
        //        doc.DocumentBase64 = System.Convert.ToBase64String(plainTextBytes);

        //        //Revert it back test
        //        //Base64Decode
        //        var base64EncodedBytes = System.Convert.FromBase64String(doc.DocumentBase64);
        //        var planText = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);

        //        //Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Before get ByteData", Logger.logLevel.Debug);

        //        //Get Binary Data from file
        //        byte[] byteData;
        //        FileStream fs = new FileStream(Path.Combine(workingFolder, fileName), FileMode.Open, FileAccess.Read);
        //        BinaryReader br = new BinaryReader(fs);
        //        byteData = br.ReadBytes((int)fs.Length);
        //        br.Close();
        //        fs.Close();
        //        doc.DocumentBinaryData = byteData;

        //        doc.CreatedDate = DateTime.Now;

        //        //Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Calling registerDocument", Logger.logLevel.Debug);
        //        result = WebAPI.Models.Document.registerDocument(doc);

        //        File.Delete(Path.Combine(workingFolder, fileName));

        //        //return true;
        //        return Request.CreateResponse(HttpStatusCode.OK, result);
        //    }
        //    catch (Exception ex)
        //    {
        //        var stackTrace = new StackTrace(ex, true);
        //        var line = stackTrace.GetFrame(0).GetFileLineNumber();
        //        //Logger.LogExceptions(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, ex.Message, line.ToString(), Logger.logLevel.Exception);
        //        return Request.CreateResponse(HttpStatusCode.NotAcceptable, "Error: " + ex.Message + "line: " + line);
        //    }
        //    finally
        //    {

        //    }


        //}

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
    }
}

