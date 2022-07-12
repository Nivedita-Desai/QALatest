using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;
using Newtonsoft.Json.Linq;



using System.IO;
using System.Threading.Tasks;
using System.Web;


using System.Diagnostics;
using Newtonsoft.Json;
using ClosedXML.Excel;

namespace WebAPI.Controllers
{
    public class ActualsUploadController : ApiController
    {
        private readonly string workingFolder = HttpRuntime.AppDomainAppPath + @"Uploads\";
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        [HttpPost]
        [ActionName("Post")]
        public async Task<HttpResponseMessage> Post()
        {
            logger.Debug("started post method");
            List<string> files = new List<string>();
            List<string> fileActuals = new List<string>();
            // Keep track of responses for the front end.
            List<UploadResponse> uploadResponses = new List<UploadResponse>();
            string fileName = "";
            string fullPath = "";
            try
            {
                if (!Directory.Exists(workingFolder))
                    Directory.CreateDirectory(workingFolder);
                logger.Debug(Directory.Exists(workingFolder));
                var streamProvider = new MultipartFormDataStreamProvider(workingFolder);
                await Request.Content.ReadAsMultipartAsync(streamProvider);


                // Store the file, rename with datetime.
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
                    fileActuals.Add(fileName);

                    // Add unique datetime to filename
                    fileName = createFileName(fileName);

                    fullPath = Path.Combine(workingFolder, fileName);
                    File.Move(fileData.LocalFileName, fullPath);
                    files.Add(fullPath);
                }


                uploadResponses = processFiles(files,  fileActuals);
               
            }
            catch(Exception ex)
            {
                logger.Error(ex.ToString());
            }
           
            
            var jsonResponse = new
            {
                result = uploadResponses
            };
            logger.Error("Result " + jsonResponse);
            return Request.CreateResponse(HttpStatusCode.OK, jsonResponse);
        }

        public List<UploadResponse> processFilesClosedXML(List<String> files, List<String> fileActuals)
        {
            // For each file, create an actual and update the database
            List<UploadResponse> uploadResponses = new List<UploadResponse>();
            //ExcelFile currentExcel = new ExcelFile(fullPath);
         
            List<Actual> actuals = new List<Actual>();
            for (int fileI = 0; fileI < files.Count; fileI++)
            {
                string currentFile = files[fileI];
          

           
                    logger.Debug("New excel created " + currentFile);
                    // Filter actuals and put that in the response
                    Actual currentActual = getAcutalClosedXML(currentFile);
                    int errors = currentActual.saveActual();
                    int successes = currentActual.ActualLines.Count - errors;
                    actuals.Add(currentActual);

                    UploadResponse currentResponse = new UploadResponse(currentActual);
                    logger.Debug("UploadResponse " + currentResponse.actualLinesThatNeedResponse);
                    uploadResponses.Add(currentResponse);
            }

            return uploadResponses;
        }
        /// <summary>
        /// 
        /// process uploaded files
        /// </summary>
        /// <param name="files"> list of files with full path</param>
        /// <param name="fileActuals"> List of file name</param>
        /// <returns></returns>
        public List<UploadResponse> processFiles(List<String> files,  List<String> fileActuals)
        {
            // For each file, create an actual and update the database
            List<UploadResponse> uploadResponses = new List<UploadResponse>();
            //ExcelFile currentExcel = new ExcelFile(fullPath);
            ExcelFile currentExcel = null;
            List<Actual> actuals = new List<Actual>();
            for (int fileI = 0; fileI < files.Count; fileI++)
            {
                string currentFile = files[fileI];
                currentExcel = new ExcelFile(currentFile);

                if (currentExcel.created)
                {
                    logger.Debug("New excel created " + currentFile);
                    // Filter actuals and put that in the response
                    Actual currentActual = getActual(currentExcel, fileActuals[fileI]);
                    int errors = currentActual.saveActual();
                    int successes = currentActual.ActualLines.Count - errors;
                    actuals.Add(currentActual);

                    UploadResponse currentResponse = new UploadResponse(currentActual);
                    logger.Debug("UploadResponse " + currentResponse.actualLinesThatNeedResponse);
                    uploadResponses.Add(currentResponse);
                }
                else
                {
                    // Create file error response
                    Actual faultyActual = new Actual(fileActuals[fileI], null, null);

                    UploadResponse currentResponse = new UploadResponse(faultyActual);
                    currentResponse.errorInExcelFile();
                    uploadResponses.Add(currentResponse);
                    logger.Debug("Error in creating excel." + currentFile);
                    Console.Write("Error in creating excel.");
                }
            }

            return uploadResponses;
        }


        private Actual getAcutalClosedXML(String fileName)
        {
            string PONumber = null;
            // Starting cost row
            List<ActualLine> aLs = new List<ActualLine>();
            ActualLine currentActualLineItem;
            using (var excelWorkBook = new XLWorkbook(fileName))
            {
                var noneEmtpyDataRows = excelWorkBook.Worksheet(1).RowsUsed();
                var ws = excelWorkBook.Worksheet(1);
                for (int currentRowI = 4; currentRowI < noneEmtpyDataRows.Count(); currentRowI++)
                {
                    try
                    {

                      

                        var fullLineItem = ws.Cell(String.Concat("A", currentRowI)).Value.ToString();
                        var amount = ws.Cell(String.Concat("B", currentRowI)).Value.ToString();
                        string quantity = ws.Cell(String.Concat("C", currentRowI)).Value.ToString();
                        string date = ws.Cell(String.Concat("D", currentRowI)).Value.ToString();
                     
                        currentActualLineItem = new ActualLine(fullLineItem, PONumber, amount, quantity, date, currentRowI);
                    }
                    catch (Exception e)
                    {
                        Console.Write(e);
                        Console.Write("Format error");
                        logger.Error(e.ToString());
                        currentActualLineItem = new Models.ActualLine(null, null, null, null, null, currentRowI);
                    }
                    aLs.Add(currentActualLineItem);
                }
            }
    
        

            Actual actual = new Actual(fileName, PONumber, aLs);
            return actual;
        }
        // Returns the actual from an excel
        private Actual getActual(ExcelFile excel, string fileName)
        {

            string PONumber = null;
            // Starting cost row
            List<ActualLine> aLs = new List<ActualLine>();
            ActualLine currentActualLineItem;
            try
            {
                logger.Debug("Reading from excel file");
                List<List<string>> rows = excel.Read();
                logger.Debug("Get Actual");
                logger.Debug("Number of rows from excel : " + rows.Count);
                if(rows[0].Count() > 1)
                     PONumber = rows[0][1];
                logger.Debug("PO Number : " + PONumber);

                for (int currentRowI = 3; currentRowI < rows.Count; currentRowI++)
                {
                    try
                    {

                        List<string> currentRow = rows[currentRowI];

                        string fullLineItem = currentRow[0];
                        string amount = currentRow[1];
                        string quantity = currentRow[2];
                        string date = currentRow[3];
                        logger.Debug("Current row " + currentRowI);
                        logger.Debug("FullLineItem : " + fullLineItem + ";" + "amount:" + amount);
                        currentActualLineItem = new ActualLine(fullLineItem, PONumber, amount, quantity, date, currentRowI);
                    }
                    catch (Exception e)
                    {
                        Console.Write(e);
                        Console.Write("Format error");
                        logger.Error(e.ToString());
                        currentActualLineItem = new Models.ActualLine(null, null, null, null, null, currentRowI);
                    }

                    aLs.Add(currentActualLineItem);
                }

            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }

            logger.Debug("Actual lines count " +aLs.Count);
            Actual actual = new Actual(fileName, PONumber, aLs);
            
            return actual;
        }
        
        // Creates a file name with the unique date and time attached to the end
        private string createFileName(string fileName)
        {
            logger.Error("Creating File Name");
            logger.Error(Path.GetFileNameWithoutExtension(fileName) + DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetExtension(fileName));
            return Path.GetFileNameWithoutExtension(fileName) + DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetExtension(fileName);
        }
    }
}