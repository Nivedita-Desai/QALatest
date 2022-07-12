using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Web;
using System.Web.Http;
using WebAPI.Reports.FACSumDataSetTableAdapters;
using log4net;
using WebAPI.Reports.AllTrendDataSetTableAdapters;
using WebAPI.Reports.ForecastTrendDataSetTableAdapters;
using WebAPI.Reports.DataSet1TableAdapters;
using WebAPI.Reports.ProgramBreakdownDataSetTableAdapters;
using WebAPI.Reports.BudgetCheckListTableAdapters;

using WebAPI.Models;
using System.Configuration;

namespace WebAPI.Controllers
{

    public class RequestReportController : ApiController
    {
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // declare report types
        public static string FACSumReport = "WebAPI.Reports.FACSumReport.rdlc";
        public static string AllTrendReport = "WebAPI.Reports.AllTrendReport.rdlc";
        public static string ForecastTrendReport = "WebAPI.Reports.ForecastTrendReport.rdlc";
        public static string ProgramBreakdownReport = "WebAPI.Reports.ProgramBreakdownReport.rdlc";
        public static string BudgetCheckListReport = "WebAPI.Reports.BudgetCheckList.rdlc";
        public static string TestReport = ""; // used for testing on the generic test function

        public string fileName = "";
        public string filePath = "";

        // param1 = ProjectID or ProgramID
        // param2 = TrendID or 0
        // ReportType = type of report to be generated
        //      FACSum              param1-ProjectID, param2-TrendID
        //      AllTrend            param1-ProjectID
        //      ForecastTrend       param1-ProjectID, param2-TrendID
        //      ProgramBreakdown    param1-ProgramID
        //      Test                param1-ProjectID, param2=TrendID

        public HttpResponseMessage Get(String projectID = "null", String reportType = "null")
        {
            string token = "";
            string response;
            string tableauTrustedURI = ConfigurationManager.AppSettings["TableauTrustedURI"];
            string tableauUserName = ConfigurationManager.AppSettings["TableauUserName"];
            string tableauPublicURI = ConfigurationManager.AppSettings["TableauPublicURI"]; 
            //HttpWebRequest wc = WebRequest.Create("http://birdi-amag:8000/trusted" + "?username=developers") as HttpWebRequest;
            HttpWebRequest wc = WebRequest.Create(tableauTrustedURI + "?username=" + tableauUserName) as HttpWebRequest;

            //with site_id target_site=<site id>
            //HttpWebRequest wc = WebRequest.Create("http://birdi-amag:8000/trusted" + "?username=developers&target_site=cpp") as HttpWebRequest;
            wc.Method = "POST";

            try
            {
                //Send the web request and parse the response into a string

                HttpWebResponse wr = wc.GetResponse() as HttpWebResponse;
                Stream receiveStream = wr.GetResponseStream();
                StreamReader readStream = new StreamReader(receiveStream, System.Text.Encoding.UTF8);
                token = readStream.ReadToEnd();
                receiveStream.Close();
                readStream.Close();
                wr.Close();
                readStream.Dispose();
                receiveStream.Dispose();

                if (token.Equals("-1"))
                {
                    throw new Exception ("Cannot connect to Tableau server.");
                }
                
                List<Project> ProjectList = WebAPI.Models.Project.getProject("null", "null", projectID, "null");

                //response = "http://birdi-amag:8000/trusted/" + token + "/views/BudgetedvsActualAllProjectsRunningTotal/" + reportType + "?&ProjectName=" + HttpUtility.UrlEncode(ProjectList[0].ProjectName) + "&:showShareOptions=false&:refresh=yes";
                response = tableauPublicURI + "/" + token + "/views/BudgetedvsActualAllProjectsRunningTotal/" + reportType + "?&ProjectName=" + HttpUtility.UrlEncode(ProjectList[0].ProjectName) + "&:showShareOptions=false&:refresh=yes";

                //With /t/<site-name> where <site-name> = site name, not site id
                //response = "http://birdi-amag:8000/trusted/" + token + "/t/cpp/views/BudgetedvsActualAllProjectsRunningTotal/" + reportType + "?&ProjectName=" + HttpUtility.UrlEncode(ProjectList[0].ProjectName) + "&:showShareOptions=false&:refresh=yes";
            }
            catch (WebException we)
            {
                //Catch failed request and return the response code 
                return Request.CreateResponse(HttpStatusCode.ServiceUnavailable, ((HttpWebResponse)we.Response).StatusCode.ToString() + ": " +  we.Message);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.ServiceUnavailable, ex.Message);
            }

            //if (Request.RequestUri.GetLeftPart(UriPartial.Authority).ToString().IndexOf("localhost") != -1)
            //{
            //    //response = "http://birdi-amag:8000/trusted/" + token + "/views/BudgetedvsActualAllProjectsRunningTotal/FTERT2?&ProjectName=Asphalt%20strength%20study&:showShareOptions=false&:refresh=yes";
            //    response = tableauPublicURI + "/" + token + "/views/BudgetedvsActualAllProjectsRunningTotal/FTERT2?&ProjectName=Asphalt%20strength%20study&:showShareOptions=false&:refresh=yes";

            //    //With /t/<site-name> where <site-name> = site name, not site id
            //    //response = "http://birdi-amag:8000/trusted/" + token + "/t/cpp/views/BudgetedvsActualAllProjectsRunningTotal/FTERT2?&ProjectName=Asphalt%20strength%20study&:showShareOptions=false&:refresh=yes";
            //}
            var jsonNew = new
            {
                result = response
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);

            //string hostName = Dns.GetHostName(); // Retrive the Name of HOST  
            //Console.WriteLine(hostName);
            //// Get the IP  
            //string myIP = Dns.GetHostByName(hostName).AddressList[0].ToString();
            //Console.WriteLine("My IP Address is :" + myIP);
            //Console.ReadKey();

            //List<Project> ProjectList = WebAPI.Models.Project.getProject("null", "null", projectID, "null");

            //var jsonNew = new
            //{
            //    result = ProjectList
            //};
            //return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
        

        public HttpResponseMessage Get(int param1, string param2, String ReportType)
        {
            log4net.Config.XmlConfigurator.Configure();

            HttpResponseMessage result = null;

            // generate report by using the third parameter in the http get
            if (ReportType == "FACSum")
            {
                fileName = string.Concat("Budget Report.pdf");
                filePath = HttpContext.Current.Server.MapPath("~/Reports/" + fileName);
                try
                {
                    //create table adapters
                    FSMainTableAdapter fsMainTA = new FSMainTableAdapter();
                    FSSum2TableAdapter fsSum2TA = new FSSum2TableAdapter();
                    FSProjectTableAdapter fsProjectTA = new FSProjectTableAdapter();
                    FSDatesTableAdapter fsDatesTA = new FSDatesTableAdapter();
                    BudgetCheckListTableAdapter TA1 = new BudgetCheckListTableAdapter();
                    BudgetCheckListSecondAdapter TA2 = new BudgetCheckListSecondAdapter();
                    BudgetCheckListThirdAdapter TA3 = new BudgetCheckListThirdAdapter();
                    //create datatables by attaching paramters to the table adapters
                    DataTable fsMainDT = fsMainTA.GetData(param1, param2);
                    DataTable fsSummaryDT = fsSum2TA.GetData(param1, param2);
                    DataTable fsProjectDT = fsProjectTA.GetData(param1);
                    DataTable fsDatesDT = fsDatesTA.GetData(param1, param2);
                    DataTable DT1 = TA1.GetData(param1, param2);
                    DataTable DT2 = TA2.GetData(param1, param2);
                    DataTable DT3 = TA3.GetData(param1, param2);

                    //generate report from data tables
                    GenerateFSR(DT1, DT2, DT3, fsMainDT, fsSummaryDT, fsProjectDT, fsDatesDT, filePath);

                    result = Request.CreateResponse(HttpStatusCode.OK);
                    result.Content = new StreamContent(new FileStream(filePath, FileMode.Open));
                    result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                    result.Content.Headers.ContentDisposition.FileName = fileName;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.InnerException);
                }
            } // end 

            else if (ReportType == "AllTrend")
            {
                fileName = string.Concat("Project Report.pdf");
                filePath = HttpContext.Current.Server.MapPath("~/Reports/" + fileName);
                try
                {
                    //create table adapters
                    ATTrendTableAdapter atTrendTA = new ATTrendTableAdapter();
                    ATProjectTableAdapter atProjectTA = new ATProjectTableAdapter();

                    //create datatables by attaching paramters to the table adapters
                    DataTable atTrendDT = atTrendTA.GetData(param1);
                    DataTable atProjectDT = atProjectTA.GetData(param1);

                    //generate report from data tables
                    GenerateATR(atTrendDT, atProjectDT, filePath);

                    result = Request.CreateResponse(HttpStatusCode.OK);
                    result.Content = new StreamContent(new FileStream(filePath, FileMode.Open));
                    result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                    result.Content.Headers.ContentDisposition.FileName = fileName;
                }
                catch (Exception ex)
                {
                    logger.Error(ex);
                    Console.WriteLine(ex.InnerException);
                }
            } // end AllTrendReport - ProjectReport

            else if (ReportType == "ForecastTrend")
            {
                fileName = string.Concat("Project Change Detail Worksheet.pdf");
                filePath = HttpContext.Current.Server.MapPath("~/Reports/" + fileName);
                try
                {
                    //create table adapters
                    ForecastTrendTableAdapter ftForecastTA = new ForecastTrendTableAdapter();
                    PhaseDateDetailsTableAdapter ftPhaseDateTA = new PhaseDateDetailsTableAdapter();
                    TrendDetailsTableAdapter ftTrendTA = new TrendDetailsTableAdapter();
                    Group2SummaryTableAdapter ftGroup2TA = new Group2SummaryTableAdapter();

                    //create datatables by attaching paramters to the table adapters
                    DataTable ftForecastDT = ftForecastTA.GetData(param1); //, Convert.ToInt16(param2));
                    DataTable ftPhaseDateDT = ftPhaseDateTA.GetData(Convert.ToInt16(param1), Convert.ToInt16(param2));
                    DataTable ftTrendDT = ftTrendTA.GetData(param1, Convert.ToInt16(param2));
                    DataTable ftGroup2DT = ftGroup2TA.GetData(param1, Convert.ToInt16(param2));
                    
                    //generate report from data tables
                   GenerateFTR(ftForecastDT, ftPhaseDateDT, ftTrendDT, ftGroup2DT, filePath);

                    result = Request.CreateResponse(HttpStatusCode.OK);
                    result.Content = new StreamContent(new FileStream(filePath, FileMode.Open));
                    result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                    result.Content.Headers.ContentDisposition.FileName = fileName;
                }
                catch (Exception ex)
                {
                    logger.Error(ex);
                    Console.WriteLine(ex.InnerException);
                }
            } // end ForecastTrendReport

            else if (ReportType == "ProgramBreakdown")
            {
                fileName = string.Concat("Program Breakdown Report.pdf");
                filePath = HttpContext.Current.Server.MapPath("~/Reports/" + fileName);
                try
                {
                    //create table adapters
                    PBProgramElementTableAdapter pbProgramElementTA = new PBProgramElementTableAdapter();
                    PBProgramTableAdapter pbProgramTA = new PBProgramTableAdapter();

                    //create datatables by attaching paramters to the table adapters
                    DataTable pbProgramElementDT = pbProgramElementTA.GetData(param1);
                    DataTable pbProgramDT = pbProgramTA.GetData(param1);

                    //generate report from data tables
                    GeneratePBR(pbProgramElementDT, pbProgramDT, filePath);

                    result = Request.CreateResponse(HttpStatusCode.OK);
                    result.Content = new StreamContent(new FileStream(filePath, FileMode.Open));
                    result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                    result.Content.Headers.ContentDisposition.FileName = fileName;
                }
                catch (Exception ex)
                {
                    logger.Error(ex);
                    Console.WriteLine(ex.InnerException);
                }
            } // end ProgramBreakdownReport

            else if (ReportType == "Test")
            {
                fileName = string.Concat("test.pdf");
                filePath = HttpContext.Current.Server.MapPath("~/Reports/" + fileName);
                try
                {
                    //create table adapters
                    check_list_budgetTableAdapter TA1 = new check_list_budgetTableAdapter();
                    check_list_budget_secondTableAdapter TA2 = new check_list_budget_secondTableAdapter();
                    check_list_budget_thirdTableAdapter TA3 = new check_list_budget_thirdTableAdapter();
                    // TableAdapter TA2 = new TableAdapter();

                    //create datatables by attaching paramters to the table adapters
                    DataTable DT1 = TA1.GetData(param1, param2);
                    DataTable DT2 = TA2.GetData(param1, param2);
                    DataTable DT3 = TA3.GetData(param1, param2);
                    // DataTable DT2 = TA2.GetData(param1, param2);

                    //generate report from data tables
                    GenerateTestPDF(DT1, DT2, DT3, filePath);

                    result = Request.CreateResponse(HttpStatusCode.OK);
                    result.Content = new StreamContent(new FileStream(filePath, FileMode.Open));
                    result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                    result.Content.Headers.ContentDisposition.FileName = fileName;
                }
                catch (Exception ex)
                {
                    logger.Error(ex);
                    Console.WriteLine(ex.InnerException);
                }
            } // end test

            return result;
        }

        private void GenerateTestPDF(DataTable DT1, DataTable DT2, DataTable DT3, string filePath)
        {
            log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            log4net.Config.XmlConfigurator.Configure();
            string binPath = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "bin");
            logger.Error("BudgetCheckListReport" + BudgetCheckListReport);
            logger.Error(binPath);
            var assembly = Assembly.Load(System.IO.File.ReadAllBytes(binPath + "\\WebAPI.dll"));
            logger.Error(assembly);

            try
            {
                using (Stream stream = assembly.GetManifestResourceStream(BudgetCheckListReport))
                {
                    logger.Error("Pass using Stream");
                    ReportViewer viewer = new ReportViewer();
                    viewer.LocalReport.EnableExternalImages = true;
                    logger.Debug(stream);
                    viewer.LocalReport.LoadReportDefinition(stream);
                    logger.Error("Pass view report");
                    Warning[] warnings;
                    string[] streamids;
                    string mimeType;
                    string encoding;
                    string filenameExtension;

                    // attach datatables to datasources in rdlc
                    viewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", DT1));
                    viewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet2", DT2));
                    viewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet3", DT3));

                    logger.Error(viewer.LocalReport.DataSources);

                    viewer.LocalReport.Refresh();
                    logger.Error("before bytes");

                    byte[] bytes = viewer.LocalReport.Render(
                        "PDF", null, out mimeType, out encoding, out filenameExtension,
                        out streamids, out warnings);

                    using (FileStream fs = new FileStream(filePath, FileMode.Create))
                    {
                        logger.Error("Inside Filestream");
                        fs.Write(bytes, 0, bytes.Length);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                Console.WriteLine(ex.InnerException);
            }
        }

        public static void GenerateFSR(DataTable DT1, DataTable DT2, DataTable DT3, DataTable mainDT, DataTable summaryDT, DataTable projectDT, DataTable datesDT, string filePath)
        {
            string binPath = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "bin");
            var assembly = Assembly.Load(System.IO.File.ReadAllBytes(binPath + "\\WebAPI.dll"));

            try
            {
                using (Stream stream = assembly.GetManifestResourceStream(FACSumReport))
                {
                    ReportViewer viewer = new ReportViewer();
                    viewer.LocalReport.EnableExternalImages = true;
                    viewer.LocalReport.LoadReportDefinition(stream);

                    Warning[] warnings;
                    string[] streamids;
                    string mimeType;
                    string encoding;
                    string filenameExtension;

                    // attach datatables to datasources in rdlc
                    viewer.LocalReport.DataSources.Add(new ReportDataSource("FSMain", mainDT));
                    viewer.LocalReport.DataSources.Add(new ReportDataSource("FSSummary", summaryDT));
                    viewer.LocalReport.DataSources.Add(new ReportDataSource("FSProject", projectDT));
                    viewer.LocalReport.DataSources.Add(new ReportDataSource("FSDates", datesDT));
                    viewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", DT1));
                    viewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet2", DT2));
                    viewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet3", DT3));
                    viewer.LocalReport.Refresh();

                    byte[] bytes = viewer.LocalReport.Render(
                        "PDF", null, out mimeType, out encoding, out filenameExtension,
                        out streamids, out warnings);

                    using (FileStream fs = new FileStream(filePath, FileMode.Create))
                    {
                        fs.Write(bytes, 0, bytes.Length);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }
        }

        //alltrend report
        public static void GenerateATR(DataTable trendDT, DataTable projectDT, string filePath)
        {
            string binPath = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "bin");
            var assembly = Assembly.Load(System.IO.File.ReadAllBytes(binPath + "\\WebAPI.dll"));
            try
            {
                using (Stream stream = assembly.GetManifestResourceStream(AllTrendReport))
                {
                    ReportViewer viewer = new ReportViewer();
                    viewer.LocalReport.EnableExternalImages = true;
                    viewer.LocalReport.LoadReportDefinition(stream);

                    Warning[] warnings;
                    string[] streamids;
                    string mimeType;
                    string encoding;
                    string filenameExtension;

                    // attach datatables to datasources in rdlc
                    viewer.LocalReport.DataSources.Add(new ReportDataSource("ATTrend", trendDT));
                    viewer.LocalReport.DataSources.Add(new ReportDataSource("ATProject", projectDT));
                    viewer.LocalReport.Refresh();

                    byte[] bytes = viewer.LocalReport.Render(
                        "PDF", null, out mimeType, out encoding, out filenameExtension,
                        out streamids, out warnings);

                    using (FileStream fs = new FileStream(filePath, FileMode.Create))
                    {
                        fs.Write(bytes, 0, bytes.Length);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }
        }

        //forecasttrend report
        public static void GenerateFTR(DataTable ftForecastDT, DataTable ftPhaseDateDT, DataTable ftTrendDT, DataTable ftGroup2DT, string filePath)
        {
            string binPath = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "bin");
            var assembly = Assembly.Load(System.IO.File.ReadAllBytes(binPath + "\\WebAPI.dll"));
            try
            {
                using (Stream stream = assembly.GetManifestResourceStream(ForecastTrendReport))
                {
                    ReportViewer viewer = new ReportViewer();
                    viewer.LocalReport.EnableExternalImages = true;
                    viewer.LocalReport.LoadReportDefinition(stream);

                    Warning[] warnings;
                    string[] streamids;
                    string mimeType;
                    string encoding;
                    string filenameExtension;

                    // attach datatables to datasources in rdlc
                    viewer.LocalReport.DataSources.Add(new ReportDataSource("Report_ForecastForecastTrend", ftForecastDT));
                    viewer.LocalReport.DataSources.Add(new ReportDataSource("Report_ForecastPhaseDataDetails", ftPhaseDateDT));
                    viewer.LocalReport.DataSources.Add(new ReportDataSource("Report_ForecastTrendDetails", ftTrendDT));
                    viewer.LocalReport.DataSources.Add(new ReportDataSource("Report_ForecastGroup2Summary", ftGroup2DT));
                    viewer.LocalReport.Refresh();

                    byte[] bytes = viewer.LocalReport.Render(
                        "PDF", null, out mimeType, out encoding, out filenameExtension,
                        out streamids, out warnings);

                    using (FileStream fs = new FileStream(filePath, FileMode.Create))
                    {
                        fs.Write(bytes, 0, bytes.Length);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }
        }

        //program breakdown report
        public static void GeneratePBR(DataTable pbProgramElementDT, DataTable pbProgramDT, string filePath)
        {
            string binPath = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "bin");
            var assembly = Assembly.Load(System.IO.File.ReadAllBytes(binPath + "\\WebAPI.dll"));
            try
            {
                using (Stream stream = assembly.GetManifestResourceStream(ProgramBreakdownReport))
                {
                    ReportViewer viewer = new ReportViewer();
                    viewer.LocalReport.EnableExternalImages = true;
                    viewer.LocalReport.LoadReportDefinition(stream);

                    Warning[] warnings;
                    string[] streamids;
                    string mimeType;
                    string encoding;
                    string filenameExtension;

                    // attach datatables to datasources in rdlc
                    viewer.LocalReport.DataSources.Add(new ReportDataSource("PBProgramElement", pbProgramElementDT));
                    viewer.LocalReport.DataSources.Add(new ReportDataSource("PBProgram", pbProgramDT));

                    byte[] bytes = viewer.LocalReport.Render(
                        "PDF", null, out mimeType, out encoding, out filenameExtension,
                        out streamids, out warnings);

                    using (FileStream fs = new FileStream(filePath, FileMode.Create))
                    {
                        fs.Write(bytes, 0, bytes.Length);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }
        }

        //generic test report
        public static void GeneratePDF(DataTable dt1, DataTable dt2, string filePath)
        {
            string binPath = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "bin");
            var assembly = Assembly.Load(System.IO.File.ReadAllBytes(binPath + "\\WebAPI.dll"));
            try
            {
                using (Stream stream = assembly.GetManifestResourceStream(TestReport))
                {
                    ReportViewer viewer = new ReportViewer();
                    viewer.LocalReport.EnableExternalImages = true;
                    viewer.LocalReport.LoadReportDefinition(stream);

                    Warning[] warnings;
                    string[] streamids;
                    string mimeType;
                    string encoding;
                    string filenameExtension;

                    // attach datatables to datasources in rdlc
                    //viewer.LocalReport.DataSources.Add(new ReportDataSource("DT1", dt1));
                    //viewer.LocalReport.DataSources.Add(new ReportDataSource("DT2", dt2));

                    byte[] bytes = viewer.LocalReport.Render(
                        "PDF", null, out mimeType, out encoding, out filenameExtension,
                        out streamids, out warnings);

                    using (FileStream fs = new FileStream(filePath, FileMode.Create))
                    {
                        fs.Write(bytes, 0, bytes.Length);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }
        }
    }
}
