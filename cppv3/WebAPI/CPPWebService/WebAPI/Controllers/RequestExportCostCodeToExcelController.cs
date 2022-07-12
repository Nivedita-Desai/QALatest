using NPOI.HSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;
using System.Web;
using System.Configuration;
using NPOI.SS.UserModel;
using NPOI.HSSF.Util;
using System.IO;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    public class RequestExportCostCodeToExcelController : ApiController
    {
        public HttpResponseMessage Get()
        {
            try
            {
                List<ExportCostCode> costCodeList = new List<ExportCostCode>();
                using (var ctx = new CPPDbContext())
                {
                    costCodeList = ctx.Database.SqlQuery<ExportCostCode>("call SpGetCostCodesToExport()")
                                                      .ToList();
                    var tempArticles = costCodeList;

                    //Create new Excel Workbook
                    var workbook = new HSSFWorkbook();

                    //Create new Excel Sheet
                    var sheet = workbook.CreateSheet();

                    //(Optional) set the width of the columns
                    sheet.SetColumnWidth(0, 20 * 256);//Title
                    sheet.SetColumnWidth(1, 20 * 256);//Section
                    sheet.SetColumnWidth(2, 20 * 256);//Views
                    sheet.SetColumnWidth(3, 20 * 256);//Downloads
                    sheet.SetColumnWidth(4, 20 * 256);//Status

                    //Create a header row
                    var headerRow = sheet.CreateRow(0);
                    headerRow.CreateCell(0).SetCellValue("CostCode");
                    headerRow.CreateCell(1).SetCellValue("Description");
                    headerRow.CreateCell(2).SetCellValue("Division");

                    //(Optional) freeze the header row so it is not scrolled
                    sheet.CreateFreezePane(0, 1, 0, 1);

                    int rowNumber = 1;

                    //Populate the sheet with values from the grid data

                    foreach (ExportCostCode objArticles in tempArticles)
                    {
                        //Create a new Row
                        var row = sheet.CreateRow(rowNumber++);

                        //Set the Values for Cells
                        row.CreateCell(0).SetCellValue(objArticles.CostCode);
                        row.CreateCell(1).SetCellValue(objArticles.Description);
                        row.CreateCell(2).SetCellValue(objArticles.Division);

                        List<CostFTE> costData = CostFTE.getCostLineItem(objArticles.CostCode);
                            foreach (CostFTE data in costData)
                            {
                            CostFTE.updateIsExportedFlag(data.ID);
                        }
                        }
                    string cdt = "CostCode"+ DateTime.Now.ToString("yyyyMMddHHmmss");
                    string filePath_Name = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["CostCodeExcel"] + cdt + ".xls");
                    //if (!File.Exists(filePath_Name))
                    //{
                        FileStream fs = new FileStream(filePath_Name, FileMode.CreateNew);
                        workbook.Write(fs);
                        fs.Close();
                    //}
                    
                    List<string> to = new List<string>();
                    //to.Add("Swapnil@softlabsgroup.com"); 

                    string sendFileTo = ConfigurationManager.AppSettings["sendCostCodeFileTo"];
                    to.Add(sendFileTo);

                    String subject = "CPP - Export CostCode"; 
                    string message = "";

                    var sendMailThread = new Task(() => {

                        WebAPI.Services.MailServices.SendMailWithAttachment(to, "", "", subject, message, filePath_Name);
                    });
                    sendMailThread.Start();
                    
                }
            }
            catch (Exception ex)
            {
               
            }
            finally
            {
            }
            return Request.CreateResponse(HttpStatusCode.OK);
        }
        }
}
