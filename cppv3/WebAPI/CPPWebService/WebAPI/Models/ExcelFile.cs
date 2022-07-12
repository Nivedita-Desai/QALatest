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
using System.Reflection;
using Microsoft.Office.Interop.Excel;
using _Excel = Microsoft.Office.Interop.Excel;

using System.IO;
using System.Threading.Tasks;
using System.Web;
using ClosedXML.Excel;
using ExcelDataReader;
using DataTable = System.Data.DataTable;

namespace WebAPI.Models
{
    public class ExcelFile
    {
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        string path = "";
        _Application excel = new _Excel.Application();
        Workbook wb;
        Worksheet ws;
        public bool created = false;

        public ExcelFile(string path)
        {
            try
            {
                logger.Debug("Creating excel file for " + path);
                this.path = path;
                var workbooks = excel.Workbooks;
                this.wb = workbooks.Open(path);
                //this.wb = excel.Workbooks.Open(path);
                logger.Debug("Successfully opened file to create excel");
                this.ws = wb.Worksheets[1];
                this.created = true;
            }
            catch (Exception e)
            {
                Console.Write("Failed to create excel file: " + e);
                logger.Error("Failed to create excel file: " + e);
            }
        }

        public string isExcelFile(string path, string fileName)
        {
            return null;
        }

        public string ReadCell(int i, int j)
        {            
            i++; j++;

            if (ws.Cells[i, j].Value2 != null)
                return ws.Cells[i, j].Value2.ToString();
            else
                return "";
        }
        public List<string> ReadRow(int i, int maxRowLength = -1)
        {
            List<string> row = new List<string>();

            int currentColumn = 0;
            while ((maxRowLength == -1 & this.ReadCell(i, currentColumn) != "") | (currentColumn < maxRowLength))
            {
                row.Add(this.ReadCell(i, currentColumn));
                currentColumn++;
            }

            return row;
        }

        public List<List<string>> Read(int maxRow = -1)
        {
            List<List<string>> rows = new List<List<string>>();

            // Find lowest row
            if (maxRow == -1)
            {
                maxRow = 0;
                while (this.ReadCell(maxRow, 0) != "")
                {
                    maxRow++;
                }
            }

            for (int rowI = 0; rowI < maxRow; rowI++)
            {
                rows.Add(this.ReadRow(rowI));
            }

            return rows;
        }

        // Swapnil 09-10-2020
        public static void KillSpecificExcelFileProcess(string excelFileName)
        {
            //var processes = from p in Process.GetProcessesByName("EXCEL")
            //                select p;
            var processes = Process.GetProcesses();

            foreach (var process in processes)
            {
                if (process.ProcessName == "EXCEL")
                    process.Kill();
            }
        }

        // Swapnil 09-10-2020
        public static DataTable ExcelFileData(string filePath)
        {
            try
            {
                DataTable dtExcelRecords = new DataTable();

                Stream stream = File.OpenRead(filePath);
                IExcelDataReader reader = null;

                FileInfo fi = new FileInfo(filePath);

                if (fi.Extension == ".xls")
                {
                    reader = ExcelReaderFactory.CreateBinaryReader(stream);
                }
                else if (fi.Extension == ".xlsx")
                {
                    reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                }
                else if (fi.Extension == ".csv")
                {
                    reader = ExcelReaderFactory.CreateCsvReader(stream);
                }

                DataSet result = reader.AsDataSet();
                reader.Close();
                dtExcelRecords = result.Tables[0];

                return dtExcelRecords;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }





    }
}