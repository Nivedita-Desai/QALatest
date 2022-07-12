using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using WebAPI.Controllers;
using WebAPI.Helper;

namespace WebAPI.Models.ExportToMPP
{
    public class ExportProcess
    {

        //public dynamic Export(int ProjectId, int TrendNumber, string granularity)
        //{
        //    var parameters = new[]
        //    {
        //        new MySqlParameter("@v_ProjectId", ProjectId),
        //        new MySqlParameter("@v_TrendNumber", TrendNumber),
        //        new MySqlParameter("@v_Granularity", granularity)
        //    };
        //    var exportData = repository.ExecuteStoredProcedureList<ExportData>("GetData", parameters);
        //    return exportData;
        //}

        //public dynamic getProject()
        //{
        //    List<SelectListItem> projectDetails = (from p in repository.SetReadOnly<project>().AsEnumerable()
        //                                           select new SelectListItem
        //                                           {
        //                                               Text = p.ProjectName,
        //                                               Value = p.ProjectID.ToString()
        //                                           }).ToList();

        //    return projectDetails;

        //}

        //    //int i = 1;
        //    //List<ResourceModel> Resources = (from Res in repository.SetReadOnly<employee>().AsEnumerable()
        //    //                                 select new ResourceModel
        //    //                                 {
        //    //                                     EmployeeId = Res.ID,
        //    //                                     ResourceId = i++,
        //    //                                     EmpName = Res.FirstName + " " + Res.LastName,
        //    //                                     HourlyRate = Res.HourlyRate 
        //    //                                 }).ToList();
        //    //return Resources;
        //}



        public List<ExportData> Export(int ProjectId, int TrendNumber, string granularity)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            List<ExportData> exportDataList = new List<ExportData>();
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    exportDataList = ctx.Database.SqlQuery<ExportData>("call GetData(@v_ProjectId, @v_TrendNumber, @v_Granularity)",
                                                      new MySql.Data.MySqlClient.MySqlParameter("@v_ProjectId", ProjectId),
                                                      new MySql.Data.MySqlClient.MySqlParameter("@v_TrendNumber", TrendNumber),
                                                      new MySql.Data.MySqlClient.MySqlParameter("@v_Granularity", granularity)
                                                      )
                                                      .ToList();
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
            }
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Exit Point", Logger.logLevel.Debug);
            return exportDataList;

        }

        public string getProName(int projectId)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            string pName = "";
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    pName = (from pro in ctx.Project.Where(p => p.ProjectID == projectId) select pro.ProjectName).FirstOrDefault();
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
            }
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Exit Point", Logger.logLevel.Debug);
            return pName;
        }

        public dynamic getTotalCost(int ProjectId, int TrendNumber, string granularity, string query)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            List<TotalCost> totalCostList = new List<TotalCost>();
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    DateTime cutOffDate = DateUtility.getCutOffDate(granularity);
                    totalCostList = ctx.Database.SqlQuery<TotalCost>("call " + query + " (@v_ProjectId, @v_TrendNumber, @v_Granularity)",
                                                      new MySql.Data.MySqlClient.MySqlParameter("@v_ProjectId", ProjectId),
                                                      new MySql.Data.MySqlClient.MySqlParameter("@v_TrendNumber", TrendNumber),
                                                      new MySql.Data.MySqlClient.MySqlParameter("@v_Granularity", granularity)
                                                      )
                                                      .ToList();
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
            }
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Exit Point", Logger.logLevel.Debug);
            return totalCostList;

        }

        public dynamic getResouces(int ProjectId, int TrendNumber, string granularity, DateTime cutoffdate)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            List<ResourceModel> resourceList = new List<ResourceModel>();
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    DateTime cutOffDate = DateUtility.getCutOffDate(granularity);
                    resourceList = ctx.Database.SqlQuery<ResourceModel>("call GetResources(@v_ProjectId, @v_TrendNumber, @v_Granularity, @v_CutOffDate)",
                                                      new MySql.Data.MySqlClient.MySqlParameter("@v_ProjectId", ProjectId),
                                                      new MySql.Data.MySqlClient.MySqlParameter("@v_TrendNumber", TrendNumber),
                                                      new MySql.Data.MySqlClient.MySqlParameter("@v_Granularity", granularity),
                                                      new MySql.Data.MySqlClient.MySqlParameter("@v_CutOffDate", cutOffDate)
                                                      )
                                                      .ToList();
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
            }
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Exit Point", Logger.logLevel.Debug);
            return resourceList;

        }

        public dynamic ExportForecastMainPhase(int ProjectId, string granularity)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            List<ExportForecastData> exportForecastMainPhase = new List<ExportForecastData>();
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    DateTime cutOffDate = DateUtility.getCutOffDate(granularity);
                    exportForecastMainPhase = ctx.Database.SqlQuery<ExportForecastData>("call GetForecastCostByMainPhase(@v_ProjectId, @v_Granularity)",
                                                      new MySql.Data.MySqlClient.MySqlParameter("@v_ProjectId", ProjectId),
                                                      new MySql.Data.MySqlClient.MySqlParameter("@v_Granularity", granularity)
                                                      )
                                                      .ToList();
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
            }
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Exit Point", Logger.logLevel.Debug);
            return exportForecastMainPhase;
        }

        public dynamic ExportForecastSubPhase(int ProjectId, string granularity)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            List<ExportForecastData> exportForecastMainPhase = new List<ExportForecastData>();
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    DateTime cutOffDate = DateUtility.getCutOffDate(granularity);
                    exportForecastMainPhase = ctx.Database.SqlQuery<ExportForecastData>("call GetForecastCostBySubPhase(@v_ProjectId, @v_Granularity)",
                                                      new MySql.Data.MySqlClient.MySqlParameter("@v_ProjectId", ProjectId),
                                                      new MySql.Data.MySqlClient.MySqlParameter("@v_Granularity", granularity)
                                                      )
                                                      .ToList();
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
            }
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Exit Point", Logger.logLevel.Debug);
            return exportForecastMainPhase;
        }

        public dynamic ExportCurrentMainPhase(int ProjectId, string granularity)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            List<ExportCurrentData> exportCurrentMainPhase = new List<ExportCurrentData>();
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    DateTime cutOffDate = DateUtility.getCutOffDate(granularity);
                    exportCurrentMainPhase = ctx.Database.SqlQuery<ExportCurrentData>("call GetCurrentCostByMainPhase(@v_ProjectId, @v_Granularity)",
                                                      new MySql.Data.MySqlClient.MySqlParameter("@v_ProjectId", ProjectId),
                                                      new MySql.Data.MySqlClient.MySqlParameter("@v_Granularity", granularity)
                                                      )
                                                      .ToList();
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
            }
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Exit Point", Logger.logLevel.Debug);
            return exportCurrentMainPhase;
        }

        public dynamic ExportCurrentSubPhase(int ProjectId, string granularity)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            List<ExportCurrentData> exportCurrentSubPhase = new List<ExportCurrentData>();
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    DateTime cutOffDate = DateUtility.getCutOffDate(granularity);
                    exportCurrentSubPhase = ctx.Database.SqlQuery<ExportCurrentData>("call GetCurrentCostBySubPhase(@v_ProjectId, @v_Granularity)",
                                                      new MySql.Data.MySqlClient.MySqlParameter("@v_ProjectId", ProjectId),
                                                      new MySql.Data.MySqlClient.MySqlParameter("@v_Granularity", granularity)
                                                      )
                                                      .ToList();
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
            }
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Exit Point", Logger.logLevel.Debug);
            return exportCurrentSubPhase;
        }
    }
}