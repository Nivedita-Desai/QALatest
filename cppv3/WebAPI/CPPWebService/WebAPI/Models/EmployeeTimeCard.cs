using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using WebAPI.Services;

namespace WebAPI.Models
{
    [Table("employee_time_card")]
    public class EmployeeTimeCard
    {
        public int Id { get; set; }
        //public int EmployeeNumber { get; set; }
        public int EEIdentifier { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public DateTime ApplyToDate { get; set; }
        //public string ClockInType { get; set; }
        //public string ClockInTime { get; set; }
        //public string ClockOutType { get; set; }
        //public string ClockOutTime { get; set; }
        public string PayType { get; set; }
        public int DeptCode { get; set; }
        //public string Day { get; set; }
        //public string Department { get; set; }
        public string JobCode { get; set; }
        //public string Difference { get; set; }
        public decimal PayRate { get; set; }
        public decimal RegularHrs { get; set; }
        public decimal OTHrs { get; set; }
        public decimal DTHrs { get; set; }
        public decimal ShiftDifferentialAddRate { get; set; }
        public decimal ShiftDifferentialHrs { get; set; }

        //public decimal TotalPaid { get; set; }
        //public decimal Unpaid { get; set; }//

        public decimal Earnings { get; set; }
        public int IsCalculated { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }


        readonly static log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static void ImportEmpTimeCardData(List<String> files)
        {
            String subject = "CPP - Co-Ad Import Response";
            List<string> to = new List<string>();
            string toMailId = ConfigurationManager.AppSettings["Co-Ad_To_MailId"].ToString();
            to.Add(toMailId);
            string message = string.Empty;
            try
            {
                if (files.Count > 0)
                {
                    for (int fileI = 0; fileI < files.Count; fileI++)
                    {
                        string currentFile = files[fileI];
                        FileInfo fi = new FileInfo(currentFile);
                        if (fi.Extension == ".xls" || fi.Extension == ".xlsx")
                        {
                            DataTable excelDataTable = ExcelFile.ExcelFileData(currentFile);
                            using (var ctx = new CPPDbContext())
                            {
                                for (int i = 1; i < excelDataTable.Rows.Count; i++)
                                {

                                    EmployeeTimeCard excelData = new EmployeeTimeCard();
                                    excelData.EEIdentifier = Convert.ToInt32(excelDataTable.Rows[i]["Column0"]);
                                    excelData.LastName = Convert.ToString(excelDataTable.Rows[i]["Column1"]);
                                    excelData.FirstName = Convert.ToString(excelDataTable.Rows[i]["Column2"]);
                                    //excelData.Date = Convert.ToDateTime(excelDataTable.Rows[i]["Column3"]);
                                    //excelData.ApplyToDate = DateTime.ParseExact(Convert.ToString(excelDataTable.Rows[i]["Column3"]), "MM/dd/yyyy", CultureInfo.InvariantCulture);
                                    excelData.ApplyToDate = DateTime.Parse(Convert.ToString(excelDataTable.Rows[i]["Column3"]));
                                    excelData.PayType = Convert.ToString(excelDataTable.Rows[i]["Column4"]);
                                    excelData.DeptCode = Convert.ToInt32(excelDataTable.Rows[i]["Column5"]);
                                    excelData.JobCode = Convert.ToString(excelDataTable.Rows[i]["Column6"]);
                                    excelData.PayRate = Convert.ToDecimal(excelDataTable.Rows[i]["Column7"]);
                                    excelData.RegularHrs = Convert.ToDecimal(excelDataTable.Rows[i]["Column8"] != DBNull.Value ? excelDataTable.Rows[i]["Column8"] : 0);
                                    excelData.OTHrs = Convert.ToDecimal(excelDataTable.Rows[i]["Column9"] != DBNull.Value ? excelDataTable.Rows[i]["Column9"] : 0);
                                    excelData.DTHrs = Convert.ToDecimal(excelDataTable.Rows[i]["Column10"] != DBNull.Value ? excelDataTable.Rows[i]["Column10"] : 0);
                                    excelData.ShiftDifferentialAddRate = Convert.ToDecimal(excelDataTable.Rows[i]["Column11"] != DBNull.Value ? excelDataTable.Rows[i]["Column11"] : 0);
                                    excelData.ShiftDifferentialHrs = Convert.ToDecimal(excelDataTable.Rows[i]["Column12"] != DBNull.Value ? excelDataTable.Rows[i]["Column12"] : 0);
                                    excelData.Earnings = Convert.ToDecimal(excelDataTable.Rows[i]["Column13"] != DBNull.Value ? excelDataTable.Rows[i]["Column13"] : 0);
                                    excelData.CreatedDate = DateTime.Now;
                                    excelData.IsCalculated = 0;
                                    ctx.EmployeeTimeCard.Add(excelData);
                                    ctx.SaveChanges();
                                }
                                ctx.Database.ExecuteSqlCommand("CALL SpSaveImportedDataFromCostCode()");
                                message = "Successfully!! File imported.";
                                MailServices.SendMail(to, "", "", subject, message);
                            }
                        }
                        else
                        {
                            message = "Sorry!! Please check attached file is not in .xls or .xlxs format.";
                            MailServices.SendMail(to, "", "", subject, message);
                        }

                    }
                }
                else
                {
                    message = "Sorry!! There is no file attached to the mail to import.";
                    MailServices.SendMail(to, "", "", subject, message);
                }
            }
            catch (Exception ex)
            {
                message = "Sorry!! There is an following issue to import Co-Ad file. " + ex.Message.ToString();
                MailServices.SendMail(to, "", "", subject, message);
                throw ex;
            }
        }



    }
}