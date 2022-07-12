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
    public class Actual
    {
        public string FileNameClient { get; set; }  // The file name that was uploaded, not the name of the file saved.
        public string PONumber { get; set; }
        public List<ActualLine> ActualLines { get; set; }

        public Actual(String FN, String PO, List<ActualLine> AL)
        {
            FileNameClient = FN;
            PONumber = PO;
            ActualLines = AL;
        }

        // Save the actual to the database and return the number of errors.
        public int saveActual()
        {
            int errors = 0;

            foreach (ActualLine al in this.ActualLines)
            {
                bool success = al.saveActualLine();
                if (!success)
                {
                    errors++;
                }
            }

            return errors;
        }
    }
}