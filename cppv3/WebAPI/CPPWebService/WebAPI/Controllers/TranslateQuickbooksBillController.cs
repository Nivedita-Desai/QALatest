using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Hosting;
using System.Web.Http;
using Excel = Microsoft.Office.Interop.Excel;
using WebAPI.Models;
using System.Runtime.InteropServices;
using System.IO;
//using DocumentFormat.OpenXml.Packaging;
//using DocumentFormat.OpenXml.Spreadsheet;
using ClosedXML.Excel;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    public class TranslateQuickbooksBillController : ApiController
    {
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public JArray get()
        {

            var filePath = HostingEnvironment.MapPath(@"~/App_Data/QB/");
            if (!System.IO.Directory.Exists(filePath))
                System.IO.Directory.CreateDirectory(filePath);


            var fileName = "Bill_Ref1234555.xlsx";

           // JArray costList = this.getQBItemsExcelInterop(filePath);
            var outputPath = HostingEnvironment.MapPath(@"~/App_Data/QB/Actual/");
            if (!System.IO.Directory.Exists(outputPath))
                System.IO.Directory.CreateDirectory(outputPath);
            var outPutFileName = "actual_Ref1234555.xlsx";

            //writeToActualFileExcelInterop(costList, outputFile);


            //ClosedXML get items
            JArray costList = this.getQBItemsClosedXml(String.Concat(filePath,fileName));
            //ClosedXML writing to file
            writeToActualFileExcelClosedXml(costList, String.Concat(outputPath, outPutFileName));
            
           

            return costList;
        }

       
        [HttpPost]
        public async Task<HttpResponseMessage> Post()
        {
            logger.Debug("started post method");
            var workingFolder = HostingEnvironment.MapPath(@"~/App_Data/QB/");
            var outputPath = HostingEnvironment.MapPath(@"~/App_Data/QB/Actual/");
            List<string> files = new List<string>();
            List<string> fileActuals = new List<string>();
            // Keep track of responses for the front end.
            List<UploadResponse> uploadResponses = new List<UploadResponse>();
            string fileName = "";
            string fullPath = "";
            var watch = System.Diagnostics.Stopwatch.StartNew();
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
                    //fileActuals.Add(fileName);

                    // Add unique datetime to filename
                    fileName = createFileName(fileName);
                    fileActuals.Add(fileName);
                    fullPath = Path.Combine(workingFolder, fileName);
                    File.Move(fileData.LocalFileName, fullPath);
                    files.Add(fullPath);
                }
                int i = 0;
                List<String> outputFiles = new List<string>();
                List<String> outputFullPaths = new List<string>();
                foreach(var file in files)
                {
                    String outputFile = readingQuickBookBills(fileActuals[i]);
                    String outputFullPath = Path.Combine(outputPath, outputFile);
                    outputFiles.Add(outputFile);
                    outputFullPaths.Add(outputFullPath);
                    i++;
                }
                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
                System.Diagnostics.Debug.WriteLine("Finished in " + elapsedMs);


                 watch = System.Diagnostics.Stopwatch.StartNew();
                ActualsUploadController actualController = new ActualsUploadController();
               //uploadResponses =  actualController.processFiles(outputFullPaths, outputFiles);
               uploadResponses = actualController.processFilesClosedXML(outputFullPaths, outputFiles);
                watch.Stop();
                 elapsedMs = watch.ElapsedMilliseconds;
                System.Diagnostics.Debug.WriteLine("Finished in " + elapsedMs);
            }
            catch(Exception ex)
            {

            }

            var jsonResponse = new
            {
                result = uploadResponses
            };
            logger.Error("Result " + jsonResponse);
            return Request.CreateResponse(HttpStatusCode.OK, jsonResponse);
        }
        public String readingQuickBookBills(String fileName)
        {
            var filePath = HostingEnvironment.MapPath(@"~/App_Data/QB/");
            if (!System.IO.Directory.Exists(filePath))
                System.IO.Directory.CreateDirectory(filePath);

            // JArray costList = this.getQBItemsExcelInterop(filePath);
            var outputPath = HostingEnvironment.MapPath(@"~/App_Data/QB/Actual/");
            if (!System.IO.Directory.Exists(outputPath))
                System.IO.Directory.CreateDirectory(outputPath);
            var outPutFileName = String.Concat("actual_", fileName);
            outPutFileName = createFileName(outPutFileName);
            //writeToActualFileExcelInterop(costList, outputFile);


            //ClosedXML get items
            JArray costList = this.getQBItemsClosedXml(String.Concat(filePath, fileName));
            //ClosedXML writing to file
            writeToActualFileExcelClosedXml(costList, String.Concat(outputPath, outPutFileName));

            return outPutFileName;
        }

        private string createFileName(string fileName)
        {
            logger.Error("Creating File Name");
            logger.Error(Path.GetFileNameWithoutExtension(fileName) + DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetExtension(fileName));
            return Path.GetFileNameWithoutExtension(fileName) + DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetExtension(fileName);
        }
        public void writeToActualFileExcelClosedXml(JArray costList, String outputFile)
        {
            var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("Sheet 1");
            String s = "HELLO";
            //Row 1
            ws.Cell("A1").Value = "PO #:";
            String refNumber = (costList.First() as dynamic).LinkedTxnRefNumber;
            ws.Cell("B1").Value = refNumber;


            //Row 2
            ws.Cell("A2").Value = "Project Name";
            String customer = (costList.First() as dynamic).ItemCustomerFullName;
            ws.Cell("B2").Value = customer.Split(':')[2].ToString().Split('.')[1]; ;

            //Row 3
            ws.Cell("A3").Value = "Full Cost Code";
            ws.Cell("B3").Value = "Amount";
            ws.Cell("C3").Value = "Quantity";
            ws.Cell("D3").Value = "Week Ending";
            ws.Cell("E3").Value = "Description";

            //Row 4+
            int i = 4;
            foreach (dynamic item in costList)
            {
                String code = getCostCode(item.ItemCustomerFullName.Value, item.ItemFullName.Value);

                ws.Cell(String.Concat("A", i)).Value = code;
                ws.Cell(String.Concat("B", i)).Value = item.LineTotal.Value;
                ws.Cell(String.Concat("C", i)).Value = item.Quantity.Value;
                ws.Cell(String.Concat("D", i)).Value = item.TxnDate.Value;
                ws.Cell(String.Concat("E", i)).Value = item.Desc.Value;

                i++;

            }

            if (File.Exists(outputFile))
                wb.Save();
            else
                wb.SaveAs(outputFile);
        }

        public void writeToActualFileExcelInterop(JArray costList, String outputFile)
        {
            try
            {
                Excel.Application xlResultApp = new Excel.Application();
                xlResultApp.Visible = true;

                Excel.Workbook xlResultWorkBook = xlResultApp.Workbooks.Add("");
                Excel._Worksheet xlResultSheet = (Excel._Worksheet)xlResultWorkBook.ActiveSheet;

                //Row 1
                xlResultSheet.Cells[1, 1] = "PO #:";
                xlResultSheet.Cells[1, 2] = (costList.First() as dynamic).RefNumber;

                //Row 2
                xlResultSheet.Cells[2, 1] = "Project Name";
                String customer = (costList.First() as dynamic).ItemCustomerFullName;
                xlResultSheet.Cells[2, 2] = customer.Split(':')[2].ToString().Split('.')[1]; ;
                //Row 3 header
                xlResultSheet.Cells[3, 1] = "Full Cost Code";
                xlResultSheet.Cells[3, 2] = "Amount";
                xlResultSheet.Cells[3, 3] = "Quantity";
                xlResultSheet.Cells[3, 4] = "Week Ending";
                xlResultSheet.Cells[3, 5] = "Description";

                //Writing data
                int i = 4;
                foreach (dynamic item in costList)
                {
                    String code = getCostCode(item.ItemCustomerFullName.Value, item.ItemFullName.Value);

                    xlResultSheet.Cells[i, 1] = code;
                    xlResultSheet.Cells[i, 2] = item.LineTotal.Value;
                    xlResultSheet.Cells[i, 3] = item.Quantity.Value;
                    xlResultSheet.Cells[i, 4] = item.TxnDate.Value;
                    xlResultSheet.Cells[i, 5] = item.Desc.Value;

                    i++;

                }

                //Saving file
                xlResultApp.Visible = false;
                xlResultApp.UserControl = false;
                xlResultWorkBook.SaveAs(outputFile, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookDefault, Type.Missing, Type.Missing,
                            false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange,
                            Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

                xlResultWorkBook.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }

        public JArray getQBItemsClosedXml(String filePath)
        {
            //4 - TxnDate  [D]
            //10 - RefNumber [J]
            //19 - LinkedTxnRefNumber (PO number) [S]
            // 31 - Item Full Name [AE]
            //34   - Desc [AH]
            //35 - Quantity  [AI]
            //36 - UOM [AJ]
            //37 - Cost (RATE) [AK]
            // 38 - ItemAmount( Total cost) [AL]
            //39 - ItemCustomerFullName () 4.ONT:01.Information Technology:0089.Terminal 5 LAX:02.Second Floor:5.102.102a [AM]
            dynamic jObject = null;
            JArray costList = new JArray();
            try
            {
                using (var excelWorkBook = new XLWorkbook(filePath))
                {
                    var noneEmtpyDataRows = excelWorkBook.Worksheet(1).RowsUsed();
                    var ws = excelWorkBook.Worksheet(1);

                    for (int i = 0; i < noneEmtpyDataRows.Count(); i++)
                    {
                        jObject = new JObject() as dynamic;
                        var dataCells = noneEmtpyDataRows.ElementAt(i).Cells();
                        var count = dataCells.Count();

                        if (i == 0)
                            continue;

                        var rowID = i + 1;
                        jObject.TxnDate = ws.Cell(String.Concat("D", rowID)).Value;
                        jObject.RefNumber = ws.Cell(String.Concat("J", rowID)).Value;
                        jObject.LinkedTxnRefNumber = ws.Cell(String.Concat("S", rowID)).Value;
                        jObject.ItemFullName = ws.Cell(String.Concat("AE", rowID)).Value;
                        jObject.Desc = ws.Cell(String.Concat("AH", rowID)).Value;
                        jObject.Quantity = ws.Cell(String.Concat("AI", rowID)).Value;
                        jObject.UOM = ws.Cell(String.Concat("AJ", rowID)).Value;
                        jObject.Rate = ws.Cell(String.Concat("AK", rowID)).Value;
                        jObject.LineTotal = ws.Cell(String.Concat("AL", rowID)).Value;
                        jObject.ItemCustomerFullName = ws.Cell(String.Concat("AM", rowID)).Value;

                        //var txnDate = ws.Cell("F2").Value;

                        if (i > 0)
                            costList.Add(jObject);

                    }
                    //foreach (var dataRow in noneEmtpyDataRows)
                    //{

                    //    foreach (var dataCel in dataRow.Cells())
                    //    {
                    //        var data = dataCel.Value;
                    //    }
                    //}
                }
            }
            catch(Exception ex)
            {

            }
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            return costList;
        }
        public JArray getQBItemsExcelInterop (String filePath)
        {
            Excel.Application xlApp = new Excel.Application();

            Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(filePath);
            Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
            Excel.Range xlRange = xlWorksheet.UsedRange;

            int rowCount = xlRange.Rows.Count;
            int colCount = xlRange.Columns.Count;
            //4 - TxnDate
            //10 - RefNumber 
            //19 - LinkedTxnRefNumber (PO number)
            // 31 - Item Full Name
            //34   - Desc
            //35 - Quantity 
            //36 - UOM
            //37 - Cost (RATE)
            // 38 - ItemAmount( Total cost)
            //39 - ItemCustomerFullName () 4.ONT:01.Information Technology:0089.Terminal 5 LAX:02.Second Floor:5.102.102a
            dynamic jObject = null;
            JArray costList = new JArray();
            for (int i = 1; i <= rowCount; i++)
            {
                jObject = new JObject() as dynamic;
                for (int j = 1; j <= colCount; j++)
                {
                    if (i == 1) //First row is header
                        break;

                    if (xlRange.Cells[i, j] != null && xlRange.Cells[i, j].Value2 != null)
                    {
                        switch (j)
                        {
                            case 4:
                                jObject.TxnDate = xlRange.Cells[i, j].Value2.ToString();
                                break;
                            case 10:
                                jObject.RefNumber = xlRange.Cells[i, j].Value2.ToString(); break;
                            case 19:
                                jObject.LinkedTxnRefNumber = xlRange.Cells[i, j].Value2.ToString(); break;
                            case 31:
                                jObject.ItemFullName = xlRange.Cells[i, j].Value2.ToString(); break;
                            case 34:
                                jObject.Desc = xlRange.Cells[i, j].Value2.ToString(); break;
                            case 35:
                                jObject.Quantity = xlRange.Cells[i, j].Value2.ToString(); break;
                            case 36:
                                jObject.UOM = xlRange.Cells[i, j].Value2.ToString(); break;
                            case 37:
                                jObject.Rate = xlRange.Cells[i, j].Value2.ToString(); break;
                            case 38:
                                jObject.LineTotal = xlRange.Cells[i, j].Value2.ToString(); break;
                            case 39:
                                jObject.ItemCustomerFullName = xlRange.Cells[i, j].Value2.ToString(); break;
                            default:
                                Console.Write("Nothing here");
                                break;


                        };


                    }

                }
                if (i > 1)
                    costList.Add(jObject);
            }


            //release com objects to fully kill excel process from running in the background
            Marshal.ReleaseComObject(xlRange);
            Marshal.ReleaseComObject(xlWorksheet);

            //close and release
            xlWorkbook.Close();
            Marshal.ReleaseComObject(xlWorkbook);

            //quit and release
            xlApp.Quit();
            Marshal.ReleaseComObject(xlApp);

            return costList;
        }
        public String getCostCode(String ItemCustomerFullName, String ItemFullName)
        {
            Subcontractor subcontractor = null;
            Material material = null;
            String costCode = "";
            using(var ctx = new CPPDbContext())
            {
                if (ItemFullName.Substring(0, 2) == "BM")
                    material = ctx.Material.Where(a => a.UniqueIdentityNumber == ItemFullName).FirstOrDefault();
                else if (ItemFullName.Substring(0, 2) == "BS")
                    subcontractor = ctx.Subcontractor.Where(a => a.UniqueIdentityNumber == ItemFullName).FirstOrDefault();


                int clientID = int.Parse(ItemCustomerFullName.Split(':')[0].ToString().Split('.')[0]);
                int divisionID = int.Parse(ItemCustomerFullName.Split(':')[1].ToString().Split('.')[0]);
                String projectNumber = ItemCustomerFullName.Split(':')[2].ToString().Split('.')[0];
                String projectElementNumber = ItemCustomerFullName.Split(':')[3].ToString().Split('.')[0];
                String phase = ItemCustomerFullName.Split(':')[4].ToString().Split('.')[0];
                String CategoryID = ItemCustomerFullName.Split(':')[4].ToString().Split('.')[1];
                String SubCategoryID = ItemCustomerFullName.Split(':')[4].ToString().Split('.')[2];
               

                if (ItemFullName.Substring(0, 2) == "BM")
                {
                   costCode = ctx.Database.SqlQuery<String>("CALL get_cost_code_from_customer(@CostType,@ProjectNumber,@ProjectElementNumber,@ActivityPhaseCode,@CategoryID,@SubCategoryID,@UniqueIdentifyNumber)",
                                                            new MySql.Data.MySqlClient.MySqlParameter("@CostType",'U'),
                                                            new MySql.Data.MySqlClient.MySqlParameter("@ProjectNumber", projectNumber),
                                                            new MySql.Data.MySqlClient.MySqlParameter("@ProjectElementNumber", projectElementNumber),
                                                            new MySql.Data.MySqlClient.MySqlParameter("@ActivityPhaseCode", phase),
                                                            new MySql.Data.MySqlClient.MySqlParameter("@CategoryID", CategoryID),
                                                            new MySql.Data.MySqlClient.MySqlParameter("@SubCategoryID", SubCategoryID),
                                                            new MySql.Data.MySqlClient.MySqlParameter("@UniqueIdentifyNumber", ItemFullName)
                                                        ).FirstOrDefault();
               
                }else if(ItemFullName.Substring(0,2) == "BS")
                {
                    costCode= ctx.Database.SqlQuery<String>("CALL get_cost_code_from_customer(@CostType,@ProjectNumber,@ProjectElementNumber,@ActivityPhaseCode,@CategoryID,@SubCategoryID,@UniqueIdentifyNumber)",
                                                          new MySql.Data.MySqlClient.MySqlParameter("@CostType", 'L'),
                                                          new MySql.Data.MySqlClient.MySqlParameter("@ProjectNumber", projectNumber),
                                                          new MySql.Data.MySqlClient.MySqlParameter("@ProjectElementNumber", projectElementNumber),
                                                          new MySql.Data.MySqlClient.MySqlParameter("@ActivityPhaseCode", phase),
                                                          new MySql.Data.MySqlClient.MySqlParameter("@CategoryID", CategoryID),
                                                          new MySql.Data.MySqlClient.MySqlParameter("@SubCategoryID", SubCategoryID),
                                                          new MySql.Data.MySqlClient.MySqlParameter("@UniqueIdentifyNumber", ItemFullName)
                                                      ).FirstOrDefault();
                }


            }

            return costCode;
        }

        
    }
}
