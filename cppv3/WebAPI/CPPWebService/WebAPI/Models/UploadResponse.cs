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

namespace WebAPI.Models
{
    public class UploadResponse
    {
        public int success = 0; // Green
        public int duplicate = 1; // Blue
        public int error = 2; // Red
        public static string duplicateResponse = "Duplicate: line {0} ({1})";
        public static string formatErrorResponse = "Format Error: line {0}";
        public static string nonExistentCostErrorResponse = "Non existent Cost Error: line {0} ({1})";
        public int response { get; set; }
        public string fileName { get; set; }
        public List<ActualLine> actualLinesThatNeedResponse { get; set; }
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public UploadResponse(Actual a)
        {
            try
            {
                fileName = a.FileNameClient;

                this.response = this.success;
                this.actualLinesThatNeedResponse = new List<ActualLine>();
        
                if(a.ActualLines == null)
                {
                    logger.Debug("No Actual lines found in Actual object");
                }else
                    logger.Debug("Actual Lines from actual object " + a.ActualLines);
                // Filter off successful actual line items.
                for (int aL = 0; aL < a.ActualLines.Count; aL++)
                    {
                        ActualLine currentActual = a.ActualLines[aL];
                        addActualLine(currentActual);
                    }
            }
            catch(Exception ex)
            {
                logger.Error(ex.ToString());
            }
           
        }

        // Alter this for a given actual line item.
        public void addActualLine(ActualLine al)
        {
            if (al.isDuplicate() | al.isNonExistentCostError() | al.isFormatError())
            {
                this.actualLinesThatNeedResponse.Add(al);
            }

            // Adjust the error for the upload response
            if (al.isDuplicate() & this.response != this.error)
            {
                this.response = this.duplicate;
            }
            else if (al.isFormatError() | al.isNonExistentCostError())
            {
                this.response = this.error;
            }
        }

        // Alter this due to a faulty excel file.
        public void errorInExcelFile()
        {
            this.response = this.error;
        }
    }
}