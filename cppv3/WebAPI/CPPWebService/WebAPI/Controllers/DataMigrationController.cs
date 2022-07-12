using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class DataMigrationController : System.Web.Http.ApiController
    {
        [HttpGet]
        [Route("Request/Migration/")]
        public HttpResponseMessage GetProject()
        {

            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            List<DataMigration> projectList = new List<DataMigration>();
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    projectList = ctx.Database.SqlQuery<DataMigration>("call getProjectForMigration()")
                                                      .ToList();

                    foreach (var item in projectList)
                    {
                        ctx.Database.ExecuteSqlCommand("call CostCodeFormation(@v_ProjectId)",
                             new MySql.Data.MySqlClient.MySqlParameter("@v_ProjectId", item.ProjectId));

                        WebAPI.Models.DataMigration.mergeTrends(item.ProjectId);
                    }
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
