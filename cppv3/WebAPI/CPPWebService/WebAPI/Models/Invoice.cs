using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.Reflection;
using System.Diagnostics;
using System.Data.OleDb;
using WebAPI.Controllers;
using MySql.Data.MySqlClient;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.SqlServer;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;

namespace WebAPI.Models
{
    public class Invoice
    {
        public int ProjectID { get; set; }
        public List<InvoiceLine> InvoiceLines { get; set; }
        readonly static log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        // Create an Estimate object by getting project information from the database.
        public Invoice(int ProjectID)
        {
            this.ProjectID = ProjectID;

            List<InvoiceLine> invoiceLines = new List<InvoiceLine>();

            MySqlConnection conn = null;
            MySqlDataReader reader = null;
            MySqlCommand command = new MySqlCommand();
            try
            {
                conn = ConnectionManager.getConnection();
                conn.Open();

                // Update cost_fte in Activity
                var query = String.Format("call get_invoice({0})", ProjectID);

                command = new MySqlCommand(query, conn);
                command.ExecuteNonQuery();
                command = new MySqlCommand(query, conn);
                using (reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string CostID = reader.GetValue(0).ToString();
                        string phase = reader.GetValue(1).ToString();
                        string mainCategory = reader.GetValue(2).ToString();
                        string subCategory = reader.GetValue(3).ToString();
                        string costLineItem = reader.GetValue(11).ToString();
                        string costLineItemFull = reader.GetValue(12).ToString();
                        string WBS = mainCategory + ":" + subCategory;
                        string costType = reader.GetValue(4).ToString();
                        string Type = reader.GetValue(5).ToString();
                        string Name = reader.GetValue(6).ToString();
                        string Quantity = reader.GetValue(7).ToString();
                        int q = 0;
                        try
                        {
                            q = Int32.Parse(Quantity);
                        }
                        catch
                        {
                            q = 0;
                        }
                        string UnitType = reader.GetValue(8).ToString();
                        string UnitCost = reader.GetValue(9).ToString();
                        string Total = reader.GetValue(10).ToString();
                        float f = float.Parse(Total, System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
                        string Date = reader.GetValue(13).ToString();
                        DateTime d = DateTime.Parse(Date);
                        
                        string description = phase + ' ' + mainCategory + ' ' + subCategory + ' ' + Type + ' ' + Name;

                        InvoiceLine invoiceLine = new InvoiceLine(costLineItem, costLineItemFull, f, q, d, description);

                        invoiceLines.Add(invoiceLine);
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
                if (conn != null) conn.Close();
                if (reader != null) reader.Close();
            }
            
            this.InvoiceLines = invoiceLines;
        }
        

        public static string CreateCSV(int ProjectID)
        {
            // Get the estimate for this ProjectID.
            logger.Debug("Get the estimate for this ProjectID " + ProjectID );
            Invoice inv = new Invoice(ProjectID);
            
            Excel.Application oXL;
            Excel._Workbook oWB;
            Excel._Worksheet oSheet;

            try
            {
                // Start Excel and get Application object.
                oXL = new Excel.Application();
                oXL.Visible = false;

                // Get a new workbook.
                oWB = (Excel._Workbook)(oXL.Workbooks.Add(Missing.Value));
                oSheet = (Excel._Worksheet)oWB.ActiveSheet;

                // Add table headers
                oSheet.Cells[1, 1] = "PO #:";
                oSheet.Cells[2, 1] = "Project Name";
                oSheet.Cells[1, 2] = "XXX";
                oSheet.Cells[2, 2] = "XXX";

                oSheet.Cells[3, 1] = "Full Cost Code";
                oSheet.Cells[3, 2] = "Amount";
                oSheet.Cells[3, 3] = "Quantity";
                oSheet.Cells[3, 4] = "Week Ending";
                oSheet.Cells[3, 5] = "Desciption";

                // Allocate excel line for each invoice line item
                int invoiceLinesCount = inv.InvoiceLines.Count;


                // For each estimate, add its lines to the excel file.
                string[,] iLs = new string[invoiceLinesCount + 5, 5];

                // For each estimate line in the estimate, add a row in the excel file.
                int rowNumber = 0;
                for (int invLinesi = 0; invLinesi < inv.InvoiceLines.Count; invLinesi++)
                {
                    InvoiceLine iL = inv.InvoiceLines[invLinesi];

                    iLs[rowNumber, 0] = iL.CostLineItem;
                    iLs[rowNumber, 1] = iL.Amount.ToString();
                    iLs[rowNumber, 2] = iL.Quantity.ToString();
                    iLs[rowNumber, 3] = iL.Date.ToString();
                    iLs[rowNumber, 4] = iL.Description;
                    rowNumber++;
                }
                int lastColumnNumber = invoiceLinesCount + 4;
                oSheet.get_Range("A4", "E" + lastColumnNumber.ToString()).Value2 = iLs;


                // Save the file and return the path.
                string workingFolder = HttpRuntime.AppDomainAppPath + @"Uploads\";
                string fileName = "Estimate" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
                string filePath = Path.Combine(workingFolder, fileName);

                oWB.SaveCopyAs(filePath);

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