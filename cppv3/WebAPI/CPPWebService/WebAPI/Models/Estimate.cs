using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Diagnostics;
using WebAPI.Controllers;
using MySql.Data.MySqlClient;
using Excel = Microsoft.Office.Interop.Excel;

using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Windows.Forms;

namespace WebAPI.Models
{
    public class Estimate
    {
        public String ProjectID { get; set; }
        public List<EstimateLine> EstimateLines { get; set; }
        public String fileName { get; set; }

        readonly static log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        // Create an Estimate object by getting project information from the database.
        public static Estimate getEstimate<T>(T ProjectIDParam, T TrendNumberParam)
        {
            logger.Debug("Start getting estimates");
            string ProjectID = ProjectIDParam.ToString();
            string TrendNumber = TrendNumberParam.ToString();

            Estimate est = new Models.Estimate();
            est.ProjectID = ProjectID;

            List<EstimateLine> estimateLines = new List<EstimateLine>();

            MySqlConnection conn = null;
            MySqlDataReader reader = null;
            MySqlCommand command = new MySqlCommand();
            try
            {
                conn = ConnectionManager.getConnection();
                conn.Open();

                // Update cost_fte in Activity
                var query = String.Format("call get_estimate({0}, {1})", ProjectID, TrendNumber);

                command = new MySqlCommand(query, conn);
                command.ExecuteNonQuery();
                command = new MySqlCommand(query, conn);
                using (reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {

                        EstimateLine estimateLine = new EstimateLine();

                        estimateLine.CostID = reader.GetValue(0).ToString();
                        string phase = reader.GetValue(1).ToString();
                        string mainCategory = reader.GetValue(2).ToString();
                        string subCategory = reader.GetValue(3).ToString();
                        estimateLine.costLineItem = reader.GetValue(11).ToString();
                        estimateLine.WBS = mainCategory + ":" + subCategory;
                        string costType = reader.GetValue(4).ToString();
                        estimateLine.CostType = costType;
                        estimateLine.Type = reader.GetValue(5).ToString();
                        estimateLine.Name = reader.GetValue(6).ToString();
                        estimateLine.Quantity = reader.GetValue(7).ToString();
                        estimateLine.UnitType = reader.GetValue(8).ToString();
                        estimateLine.UnitCost = reader.GetValue(9).ToString();
                        estimateLine.Total = reader.GetValue(10).ToString();
                        estimateLine.Client = reader.GetValue(12).ToString();
                        string tax = "Tax";
                        if (costType == "Labor")
                        {
                            tax = "LBR";
                        }
                        estimateLine.TaxType = tax;

                        estimateLines.Add(estimateLine);
                    }
                }
            }
            catch (Exception ex)
            {
                var stackTrace = new StackTrace(ex, true);
                var line = stackTrace.GetFrame(0).GetFileLineNumber();
                Logger.LogExceptions(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, ex.Message, line.ToString(), Logger.logLevel.Exception);
                logger.Error(ex.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
                if (reader != null) reader.Close();
            }
            
            est.EstimateLines = estimateLines;

            return est;
        }

        public static List<Estimate> getEstimates()
        {
            // Get all project ids
            List<int> ProjectIDs = new List<int>();
            using (var ctx = new CPPDbContext())
            {
                ProjectIDs = ctx.Project.Select(a=>a.ProjectID).ToList();
            }

            // Create list of estimates based on project ids
            List<Estimate> ests = new List<Estimate>();
            for (int projectI = 0; projectI < ProjectIDs.Count; projectI++)
            {
                int currentProjectID = ProjectIDs[projectI];
                Estimate currentEstimate = getEstimate(currentProjectID, -1);
                ests.Add(currentEstimate);
            }

            return ests;
        }
        

        public static string CreateCSV(string ProjectID, string TrendNumber)
        {
            // Get the estimate for this ProjectID.
            logger.Debug("Starting exporting estimated costs for projectid " + ProjectID);
            List<Estimate> ests = new List<Estimate>();
            if (Int32.Parse(ProjectID) == -1)
            {
                ests = WebAPI.Models.Estimate.getEstimates();
            }
            else
            {
                ests = new List<Estimate>{ WebAPI.Models.Estimate.getEstimate(ProjectID, TrendNumber) };
            }
            
            Excel.Application oXL;
            Excel._Workbook oWB;
            Excel._Worksheet oSheet;

            try
            {
                // Start Excel and get Application object.
                logger.Debug("Start Excel and get Application object");
                oXL = new Excel.Application();
                oXL.Visible = false;

                // Get a new workbook.
                oWB = (Excel._Workbook)(oXL.Workbooks.Add(Missing.Value));
                oSheet = (Excel._Worksheet)oWB.ActiveSheet;
                logger.Debug("Created excel workbook");

                // Add table headers
                oSheet.Cells[1, 1] = "COST LINE ITEM"; // Zed: Other2
                oSheet.Cells[1, 2] = "ITEM NAME"; // Zed: ItemRefFullName (id Planning:Other:Other)
                oSheet.Cells[1, 3] = "COST TYPE"; // Zed: Other1
                oSheet.Cells[1, 4] = "TYPE";
                oSheet.Cells[1, 5] = "NAME";
                oSheet.Cells[1, 6] = "QUANTITY"; // Zed: Quantity
                oSheet.Cells[1, 7] = "UNIT TYPE"; // Zed: Description
                oSheet.Cells[1, 8] = "UNIT COST"; // Zed: Rate
                oSheet.Cells[1, 9] = "TOTAL"; // Zed: Total
                oSheet.Cells[1, 10] = "TAX"; // Zed: SalesTaxCode
                oSheet.Cells[1, 11] = "ESTIMATEREFNUMBER"; // Zed: EstimateRefNumber
                oSheet.Cells[1, 12] = "CUSTOMERREFFULLNAME"; // Zed: CustomerRefFullName
                logger.Debug("Added excel headers");

                // Allocate excel line for each estimate line item in every estimate.
                int estimateLinesCount = 0;
                for (int esti = 0; esti < ests.Count; esti++)
                {
                    List<EstimateLine> currentEstimateLines = ests[esti].EstimateLines;
                    estimateLinesCount += currentEstimateLines.Count;
                }
                string[,] eLs = new string[estimateLinesCount, 13];


                // For each estimate, add its lines to the excel file.
                int rowNumber = 0;
                for (int esti = 0; esti < ests.Count; esti++)
                {
                    Estimate est = ests[esti];
                    // For each estimate line in the estimate, add a row in the excel file.
                    for (int estLinesi = 0; estLinesi < est.EstimateLines.Count; estLinesi++)
                    {
                        EstimateLine eL = est.EstimateLines[estLinesi];

                        eLs[rowNumber, 0] = eL.costLineItem;
                        eLs[rowNumber, 1] = eL.WBS;
                        eLs[rowNumber, 2] = eL.CostType;
                        eLs[rowNumber, 3] = eL.Type;
                        eLs[rowNumber, 4] = eL.Name;
                        eLs[rowNumber, 5] = (eL.Quantity == "N/A") ? "1" : eL.Quantity;
                        eLs[rowNumber, 6] = eL.UnitType;
                        eLs[rowNumber, 7] = (eL.UnitCost == "N/A") ? "1" : eL.UnitCost;
                        eLs[rowNumber, 8] = eL.Total;
                        eLs[rowNumber, 9] = eL.TaxType;
                        eLs[rowNumber, 10] = "Q19-0001"; //TODO: does this need to be here for zed axis
                        eLs[rowNumber, 11] = eL.Client;
                        rowNumber++;
                    }
                }
                int lastColumnNumber = estimateLinesCount + 1;
                oSheet.get_Range("A2", "M" + lastColumnNumber.ToString()).Value2 = eLs;
                logger.Debug("Added rows");


                // Save the file and return the path.
                string workingFolder = HttpRuntime.AppDomainAppPath + @"Uploads\";
                string fileName = "Estimate" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
                string filePath = Path.Combine(workingFolder, fileName);


                if (!Directory.Exists(workingFolder))
                    Directory.CreateDirectory(workingFolder);

                oWB.SaveCopyAs(filePath);
                logger.Debug("Saved copy of file");

                return filePath;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                logger.Error(e.ToString());
                return "error";
            }
        }
    }
}