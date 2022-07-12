using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Web;
using WebAPI.Controllers;
using WebAPI.Helper;
using WebAPI.Models;

namespace WebAPI.Models
{
    [Table("trend_status_code")]
    public class TrendStatusCode : Audit
    {
        [NotMapped]
        public int Operation;



        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TrendStatusCodeID { get; set; }
        public String TrendStatusCodeNumber { get; set; }
        public String TrendStatusCodeName { get; set; }
        public String TrendStatusCodeDescription { get; set; }
        //public String CreatedBy { get; set; }
        //public String LastUpdatedBy { get; set; }
        //public DateTime CreatedDate { get; set; }
        //public DateTime LastUpdatedDate { get; set; }

        public static List<TrendStatusCode> GetTrendStatusCode()
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);

            List<TrendStatusCode> trendStatusCodeList = new List<TrendStatusCode>();
            try
            {

                using (var ctx = new CPPDbContext())
                {
                    trendStatusCodeList = ctx.TrendStatusCode.OrderBy(a => a.TrendStatusCodeNumber).ToList();
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

            return trendStatusCodeList;
        }


        public static String registerTrendStatusCode(TrendStatusCode trendStatusCode)
        {

            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String result = "";
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    TrendStatusCode retrievedTrendStatusCode = new TrendStatusCode();
                    retrievedTrendStatusCode = ctx.TrendStatusCode.Where(u => u.TrendStatusCodeName == trendStatusCode.TrendStatusCodeName || u.TrendStatusCodeNumber == trendStatusCode.TrendStatusCodeNumber).FirstOrDefault();

                    TrendStatusCode duplicateTrendStatusCode = new TrendStatusCode();
                    duplicateTrendStatusCode = ctx.TrendStatusCode.Where(u => (u.TrendStatusCodeNumber == trendStatusCode.TrendStatusCodeNumber)).FirstOrDefault();


                    if (retrievedTrendStatusCode == null)
                    {
                        //register
                        ctx.TrendStatusCode.Add(trendStatusCode);
                        ctx.SaveChanges();
                        //result = "Success";
                        result += trendStatusCode.TrendStatusCodeName + " has been created successfully.\n";
                    }
                    else if (duplicateTrendStatusCode != null)
                    {
                        result += trendStatusCode.TrendStatusCodeName + " failed to be created, code already exist.\n";
                    }
                    else
                    {
                        result += trendStatusCode.TrendStatusCodeName + " failed to be created, it already exist.\n";
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
            }

            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Exit Point", Logger.logLevel.Debug);

            return result;
        }
        public static String updateTrendStatusCode(TrendStatusCode trendStatusCode)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String result = "";

            try
            {
                using (var ctx = new CPPDbContext())
                {
                    TrendStatusCode retrievedTrendStatusCode = new TrendStatusCode();
                    retrievedTrendStatusCode = ctx.TrendStatusCode.Where(u => u.TrendStatusCodeID == trendStatusCode.TrendStatusCodeID).FirstOrDefault();

                    TrendStatusCode duplicateTrendStatusCode = new TrendStatusCode();
                    duplicateTrendStatusCode = ctx.TrendStatusCode.Where(u => (u.TrendStatusCodeName == trendStatusCode.TrendStatusCodeName || u.TrendStatusCodeNumber == trendStatusCode.TrendStatusCodeNumber)
                                                                                && u.TrendStatusCodeID != trendStatusCode.TrendStatusCodeID).FirstOrDefault();

                    if (duplicateTrendStatusCode != null)
                    {
                        result = trendStatusCode.TrendStatusCodeName + " failed to be updated, duplicate will be created.\n";
                    }
                    else if (retrievedTrendStatusCode != null)
                    {
                        CopyUtil.CopyFields<TrendStatusCode>(trendStatusCode, retrievedTrendStatusCode);
                        ctx.Entry(retrievedTrendStatusCode).State = System.Data.Entity.EntityState.Modified;
                        ctx.SaveChanges();
                        result += trendStatusCode.TrendStatusCodeName + " has been updated successfully.\n";
                    }
                    else
                    {
                        result += trendStatusCode.TrendStatusCodeName + " failed to be updated, it does not exist.\n";
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
            }
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Exit Point", Logger.logLevel.Debug);

            return result;

        }
        public static String deleteTrendStatusCode(TrendStatusCode trendStatusCode)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String result = "";
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    TrendStatusCode retrievedTrendStatusCode = new TrendStatusCode();
                    retrievedTrendStatusCode = ctx.TrendStatusCode.Where(u => u.TrendStatusCodeID == trendStatusCode.TrendStatusCodeID).FirstOrDefault();
                    if (retrievedTrendStatusCode != null)
                    {
                        ctx.TrendStatusCode.Remove(retrievedTrendStatusCode);
                        ctx.SaveChanges();
                        result = trendStatusCode.TrendStatusCodeName + " has been deleted successfully.\n";
                    }
                    else
                    {
                        result = trendStatusCode.TrendStatusCodeName + " failed to be deleted, it does not exist.\n";
                    }
                }

            }
            catch (Exception ex)
            {
                result = trendStatusCode.TrendStatusCodeName + " failed to be deleted due to dependencies.\n";
                var stackTrace = new StackTrace(ex, true);
                var line = stackTrace.GetFrame(0).GetFileLineNumber();
                Logger.LogExceptions(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, ex.Message, line.ToString(), Logger.logLevel.Exception);
            }
            finally
            {
            }
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Exit Point", Logger.logLevel.Debug);

            return result;
        }
    }
}