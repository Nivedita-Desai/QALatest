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

namespace WebAPI.Controllers
{
    public class TimesheetUploadController : ApiController
    {
        private readonly string workingFolder = HttpRuntime.AppDomainAppPath + @"\Uploads\";

        [HttpPost]
        public async Task<HttpResponseMessage> Post()
        {
            List<string> files = new List<string>();
            List<string> fileActuals = new List<string>();

            string fileName = "";
            string fullPath = "";
            var streamProvider = new MultipartFormDataStreamProvider(workingFolder);
            await Request.Content.ReadAsMultipartAsync(streamProvider);

            // Store the file, rename if duplicate
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

            // Keep track of responses for the front end.
            List<UploadResponse> uploadResponses = new List<UploadResponse>();

            // For each file, create an actual and update the database
            ExcelFile currentExcel = new ExcelFile(fullPath);//TODO: check if this errors out
            List<Actual> actuals = new List<Actual>();
            for (int fileI = 0; fileI < files.Count; fileI++)
            {
                string currentFile = files[fileI];
                currentExcel = new ExcelFile(currentFile);

                if (currentExcel.created)
                {
                    // Filter actuals and put that in the response
                    //TODO: need a unique identifier for timesheet file or a way to omit it
                    Actual currentActual = getActual(currentExcel, fileActuals[fileI]);
                    int errors = currentActual.saveActual();
                    int successes = currentActual.ActualLines.Count - errors;
                    actuals.Add(currentActual);

                    UploadResponse currentResponse = new UploadResponse(currentActual);
                    uploadResponses.Add(currentResponse);
                }
                else
                {
                    // Create file error response
                    Actual faultyActual = new Actual(fileActuals[fileI], null, null);

                    UploadResponse currentResponse = new UploadResponse(faultyActual);
                    currentResponse.errorInExcelFile();
                    uploadResponses.Add(currentResponse);

                    Console.Write("Error in creating excel.");
                }
            }

            var jsonResponse = new
            {
                result = uploadResponses
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonResponse);
        }


        // Returns the actual from an excel
        private Actual getActual(ExcelFile excel, string fileName)
        {
            List<List<string>> rows = excel.Read();

            string PONumber = "N/A";

            // Starting cost row
            List<ActualLine> aLs = new List<ActualLine>();
            ActualLine currentActualLineItem;

            for (int currentRowI = 3; currentRowI < rows.Count; currentRowI++)
            {
                try
                {
                    List<string> currentRow = rows[currentRowI];

                    string fullLineItem = currentRow[10];
                    string amount = currentRow[8];
                    string quantity = "0";
                    string date = currentRow[4];

                    currentActualLineItem = new ActualLine(fullLineItem, PONumber, amount, quantity, date, currentRowI);
                }
                catch (Exception e)
                {
                    Console.Write(e);
                    Console.Write("Format error");
                    currentActualLineItem = new Models.ActualLine(null, null, null, null, null, currentRowI);
                }

                aLs.Add(currentActualLineItem);
            }

            Actual actual = new Actual(fileName, PONumber, aLs);

            return actual;
        }


        // Creates a file name with the unique date and time attached to the end
        private string createFileName(string fileName)
        {
            return Path.GetFileNameWithoutExtension(fileName) + DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetExtension(fileName);
        }
    }
}