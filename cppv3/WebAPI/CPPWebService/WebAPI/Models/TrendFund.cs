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

namespace WebAPI.Models
{
    [Table("trend_fund")]
    public class TrendFund
    {
        [NotMapped]
        public int Operation;

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
       
        public int Id { get; set; }
        public String FundName { get; set; }
        public double FundAssign { get; set; }
        public double FundUsed { get; set; }
        public double FundRemaining { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int TrendID { get; set; }
        public int ProjectID { get; set; }
        //public int ProgramFundID { get; set; }

        [ForeignKey("TrendID")]
        public virtual  Trend trend { get; set; }
        
        //not mapped
        [NotMapped]
        List<ProgramFund> programFundList { get; set; }
        //Nivedita 10022022
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string DeletedBy { get; set; }

        public static List<TrendFund> getTrendFund(int trendID, int projectID)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            List<TrendFund> MatchedTrendFundList = new List<TrendFund>();
            MySqlConnection conn = null;
            MySqlDataReader reader = null;
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    MatchedTrendFundList = ctx.TrendFund.Where(t => t.TrendID == trendID && t.ProjectID == projectID).ToList();
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
            return MatchedTrendFundList;
        }

        public static String registerTrendFund(TrendFund trendFund)
        {
                //user.FullName, user.UserID, user.FirstName, user.MiddleName, user.LastName, user.LoginPassword, user.Email, user.Role
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String register_result = "";
            var todayDate = DateTime.Today.ToString("yyyy-MM-dd");
      
            try
            {

                using (var ctx = new CPPDbContext())
                {
                    TrendFund retrievedTrendFund = new TrendFund();
                    retrievedTrendFund = ctx.TrendFund.Where(t => t.ProjectID == trendFund.ProjectID && t.FundName == trendFund.FundName
                                                                        &&  t.TrendID == trendFund.TrendID).FirstOrDefault();
                    if (retrievedTrendFund == null)
                    {
                        //Register
                        trendFund.DateCreated = Convert.ToDateTime(todayDate);
                        ctx.TrendFund.Add(trendFund);
                        ctx.SaveChanges();
                        register_result = "Success";
                    }
                    else
                    {
                        //Already Exist
                        register_result += "Fund " + trendFund.FundName + " already exists in system";
                    }
                }
              
            }
            catch (Exception ex)
            {
                var stackTrace = new StackTrace(ex, true);
                var line = stackTrace.GetFrame(0).GetFileLineNumber();
                Logger.LogExceptions(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, ex.Message, line.ToString(), Logger.logLevel.Exception);
                register_result = ex.Message;
            }
            finally
            {
     
            }
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Exit Point", Logger.logLevel.Debug);

            return register_result;
        }

        public static String updateTrendFund(TrendFund trendFund)
        {
            //user.FullName, user.UserID, user.FirstName, user.MiddleName, user.LastName, user.LoginPassword, user.Email, user.Role
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String update_result = "";
    
            var todayDate = DateTime.Today.ToString("yyyy-MM-dd");
            trendFund.DateUpdated = Convert.ToDateTime(todayDate);
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    TrendFund retrievedTrendFund = new TrendFund();
                    retrievedTrendFund = ctx.TrendFund.Where(t => t.TrendID == trendFund.TrendID && t.ProjectID == trendFund.ProjectID).FirstOrDefault();
                    if (retrievedTrendFund != null)
                    {
                        //Update
                        retrievedTrendFund = trendFund;
                        using (var dbCtx = new CPPDbContext())
                        {
                            dbCtx.Entry(retrievedTrendFund).State = System.Data.Entity.EntityState.Modified;
                            dbCtx.SaveChanges();
                            update_result = "Success";
                        }
                    }
                    else
                    {
                        update_result += "Fund " + trendFund.FundName + " does not  exists in system";
                    }
                }
            
            }
            catch (Exception ex)
            {
                var stackTrace = new StackTrace(ex, true);
                var line = stackTrace.GetFrame(0).GetFileLineNumber();
                Logger.LogExceptions(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, ex.Message, line.ToString(), Logger.logLevel.Exception);
                update_result = ex.Message;
            }
            finally
            {
            }
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Exit Point", Logger.logLevel.Debug);

            return update_result;
        }
        public static String deleteTrendFund(TrendFund trendFund)
        {
            //user.FullName, user.UserID, user.FirstName, user.MiddleName, user.LastName, user.LoginPassword, user.Email, user.Role
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);

            String delete_result = "";
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    TrendFund retrievedTrendFund = new TrendFund();
                    retrievedTrendFund = ctx.TrendFund.Where(t => t.TrendID == trendFund.TrendID && t.ProjectID == trendFund.ProjectID).FirstOrDefault();
                    if (retrievedTrendFund != null)
                    {
                        //Delete
                        //Nivedita 10022022
                        //ctx.CostLineItemTracker.Remove(costLineItem);
                        retrievedTrendFund.IsDeleted = true;
                        retrievedTrendFund.DeletedDate = DateTime.Now;
                        retrievedTrendFund.DeletedBy = "";
                        //ctx.TrendFund.Remove(retrievedTrendFund);
                        ctx.SaveChanges();
                        delete_result = "Success";
                    }
                    else
                    {
                        delete_result += "Fund " + trendFund.FundName + " does not exists in system";
                    }
                }
                
            }
            catch (Exception ex)
            {
                var stackTrace = new StackTrace(ex, true);
                var line = stackTrace.GetFrame(0).GetFileLineNumber();
                Logger.LogExceptions(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, ex.Message, line.ToString(), Logger.logLevel.Exception);
                delete_result = ex.Message;
            }
            finally
            {
            }
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Exit Point", Logger.logLevel.Debug);

            return delete_result;
        }
     
    }
}