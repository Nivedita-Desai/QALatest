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

namespace WebAPI.Models
{
    [Table("fund_types")]
    public class FundType
    {
        [NotMapped]
        public int Operation;

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FundTypeId { get; set; }
        public int OrganizationID { get; set; }
        public String Fund { get; set; }
        public String Amount { get; set; }
        public String Availability { get; set; }
        public String BalanceRemaining { get; set; }

        //ON DELETE RESTRICT
        [ForeignKey("OrganizationID")]
        public virtual Organization Organization { get; set; }

        //FundType(String fundTypeId, String fund, String amount, String availability, String balanceRemaining)
        //{ FundTypeId = fundTypeId; Fund = fund; Amount = amount; Availability = availability; BalanceRemaining = balanceRemaining; }
        //FundType() { }
        public static FundType getFundTypeByName(String Fund)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            FundType MatchedFundList = new FundType();
            MySqlConnection conn = null;
            MySqlDataReader reader = null;
            try
            {

                using (var ctx = new CPPDbContext())
                {
                    MatchedFundList = ctx.FundType.Where(f => f.Fund == Fund).Single();
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
                if (conn != null) conn.Close();
                if (reader != null) reader.Close();

            }
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Exit Point", Logger.logLevel.Debug);
            return MatchedFundList;

        }
        ////From RequestLookupFundTypeController
        public static List<FundType> getFundType(String FundTypeId, String Fund, String Amount, String Availability, String BalanceRemaining)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            List<FundType> MatchedFundList = new List<FundType>();
            MySqlConnection conn = null;
            MySqlDataReader reader = null;
            try
            {
                // create and open a connection object
                using (var ctx = new CPPDbContext())
                {
                    MatchedFundList = ctx.FundType.OrderBy(a=>a.Fund).ToList();
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
                if (conn != null) conn.Close();
                if (reader != null) reader.Close();

            }
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Exit Point", Logger.logLevel.Debug);
            return MatchedFundList;

        }

        public static List<FundType> getFundType(int OrganizationID)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            List<FundType> MatchedFundList = new List<FundType>();
            MySqlConnection conn = null;
            MySqlDataReader reader = null;
            try
            {
                // create and open a connection object
                using (var ctx = new CPPDbContext())
                {
                    MatchedFundList = ctx.FundType.Where(a => a.OrganizationID.Equals(OrganizationID)).OrderBy(a => a.Fund).ToList();
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
                if (conn != null) conn.Close();
                if (reader != null) reader.Close();

            }
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Exit Point", Logger.logLevel.Debug);
            return MatchedFundList;

        }
        public static String registerFundType(FundType fund)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            MySqlConnection conn = null;
            MySqlDataReader reader = null;
            String result = "";
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    FundType retrievedFund = new FundType();
                    retrievedFund = ctx.FundType.Where(f => f.Fund == fund.Fund && f.OrganizationID == fund.OrganizationID).FirstOrDefault();

                    if (retrievedFund == null)
                    {
                        ctx.FundType.Add(fund);
                        ctx.SaveChanges();
                        result += fund.Fund +  " has been created successfully.\n";
                    }
                    else
                    {
                        result += fund.Fund + " failed to be created, it already exist.\n";
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
                if (conn != null) conn.Close();
                if (reader != null) reader.Close();
            }
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Exit Point", Logger.logLevel.Debug);

            return result;
        }
        public static String updateFundType(FundType fund)
        {

            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
        
            String update_result = "";
 
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    FundType retrievedFund = ctx.FundType.Where(f => f.FundTypeId == fund.FundTypeId).FirstOrDefault();

                    FundType duplicateFundType = new FundType();
                    duplicateFundType = ctx.FundType.Where(f => f.Fund == fund.Fund
                                                                && f.OrganizationID == fund.OrganizationID
                                                                && f.FundTypeId != fund.FundTypeId).FirstOrDefault();

                    if (duplicateFundType != null)
                    {
                        update_result = retrievedFund.Fund + " failed to be updated, duplicate will be created.\n";
                    }
                    else if (retrievedFund != null)
                    {
                        using (var dbCtx = new CPPDbContext())
                        {
                            retrievedFund = fund;
                            dbCtx.Entry(retrievedFund).State = System.Data.Entity.EntityState.Modified;
                            dbCtx.SaveChanges();
                            update_result = fund.Fund + " has been update successfully.\n";
                        }
                    }
                    else
                    {
                        update_result += fund.Fund + " failed to be updated, it does not exist.\n";
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
            return update_result;
        }

        public static String deleteFundType(FundType fund)
        {

            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);         
            String result = "";

            try
            {
                using (var ctx = new CPPDbContext())
                {
                    FundType retrievedFund = ctx.FundType.Where(f => f.FundTypeId == fund.FundTypeId).Single();
                    if (retrievedFund != null)
                    {
                        //delete
                        ctx.FundType.Remove(retrievedFund);
                        ctx.SaveChanges();
                        result = fund.Fund + " has been deleted successfully.\n";
                    }
                    else
                    {
                        result += fund.Fund + " failed to be deleted, it does not exist.\n";
                    }
                }
            }
            catch (Exception ex)
            {
                result += fund.Fund + " failed to be deleted due to dependencies.\n";
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