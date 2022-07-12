using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RegisterImportCoAdDataController : ApiController
    {
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public HttpResponseMessage Get()
        {
            try
            {


                //Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
                //Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
                logger.Info("in import coad");
                List<string> outlookFiles = new List<string>(); // Swapnil 08-10-2020

                logger.Info("before get attachmentfile");
                outlookFiles = OutlookEmails.GetAttachedFiles(); // Swapnil 08-10-2020

                logger.Info("file count : " + outlookFiles.Count);
                EmployeeTimeCard.ImportEmpTimeCardData(outlookFiles); // Swapnil 08-10-2020

                logger.Info("After import time card");

                //Swapnil  08-10-2020
                for (int i = 0; i < outlookFiles.Count; i++)
                {
                    string excelFileName = Path.GetFileName(outlookFiles[i]);
                    ExcelFile.KillSpecificExcelFileProcess(excelFileName);
                    File.Delete(outlookFiles[i]);
                }
            }
            catch (Exception ex)
            {
                var stackTrace = new StackTrace(ex, true);
                var line = stackTrace.GetFrame(0).GetFileLineNumber();
                Logger.LogExceptions(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, ex.InnerException.ToString(), line.ToString(), Logger.logLevel.Exception);
            }
            finally
            {
            }
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Exit Point", Logger.logLevel.Debug);

            //var ctx = new CPPDbContext();
            var jsonNew = new
            {
                result = "Success"
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}
