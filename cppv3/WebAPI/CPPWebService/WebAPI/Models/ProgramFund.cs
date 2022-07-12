using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Web;
using WebAPI.Controllers;
using log4net;
namespace WebAPI.Models
{
     [Table("program_fund")]
    public class ProgramFund
    {
        readonly static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        [NotMapped]
        public int Operation;

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public String FundName { get; set; }
        public double FundAmount { get; set; }
        public double FundUsed { get; set; }
        public double FundRemaining { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int ProgramID { get; set; }
        public double FundRequest { get; set; }
        public int FundTypeID { get; set; }

        [ForeignKey("FundTypeID")]
        public virtual FundType fundType { get; set; }
      
         [ForeignKey("ProgramID")]
         public virtual Program program{get;set;}
   
        public static List<ProgramFund> getProgramFund(int programID)
        {
  
            List<ProgramFund> MatchedProgramFundList = new List<ProgramFund>();
            MySqlConnection conn = null;
            MySqlDataReader reader = null;
            try
            {
                IFormatProvider yyyymmddFormat = new System.Globalization.CultureInfo(String.Empty, false);
                using (var ctx = new CPPDbContext())
                {
                    ctx.Configuration.ProxyCreationEnabled = false;
                    MatchedProgramFundList = ctx.ProgramFund.Where(p => p.ProgramID == programID).ToList();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
               
                log.Debug(ex.Message);
               
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
                if (reader != null)
                {
                    reader.Close();
                }
            }
            return MatchedProgramFundList;

        }
        public static String registerProgramFund(ProgramFund programFund)
        {
            //user.FullName, user.UserID, user.FirstName, user.MiddleName, user.LastName, user.LoginPassword, user.Email, user.Role
            List<ProgramFund> programFundList = new List<ProgramFund>();
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            try{
                using (var ctx = new CPPDbContext())
                 {

                 //Temporary fix for Getting FundTypeID, may be retrieve fundTypeID on front-end in the future
                     var pf = ctx.ProgramFund.Where(a => a.FundName == programFund.FundName && a.ProgramID == programFund.ProgramID).FirstOrDefault();
                     if (pf != null)
                     {
                         //update
                         pf = programFund;
                         var dbCtx = new CPPDbContext();

                         dbCtx.Entry(pf).State = System.Data.Entity.EntityState.Modified;
                         dbCtx.SaveChanges();
                     }
                     else
                     {
                         //Register
                         FundType fType = FundType.getFundTypeByName(programFund.FundName);
                         if (fType != null)
                         {
                             programFund.FundTypeID = fType.FundTypeId;
                         }

                         ctx.ProgramFund.Add(programFund);
                         ctx.SaveChanges();
                     }
                 }
            }
            catch(Exception ex)
            {
                 var stackTrace = new StackTrace(ex, true);
                var line = stackTrace.GetFrame(0).GetFileLineNumber();
                Logger.LogExceptions(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, ex.Message, line.ToString(), Logger.logLevel.Exception);
        
            }
            return "";
            
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Exit Point", Logger.logLevel.Debug);

            //return register_result;
        }
        //delete a fund
        public static String deleteProgramFund(ProgramFund programFund)
        {
            //user.FullName, user.UserID, user.FirstName, user.MiddleName, user.LastName, user.LoginPassword, user.Email, user.Role
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            MySqlConnection conn = null;
            MySqlDataReader reader = null;
            String delete_result = "";
            bool OkForDelete = false;
           

            var temp = "";
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    ProgramFund proFund = ctx.ProgramFund
                                                .Where(p => p.FundName == programFund.FundName && p.ProgramID == programFund.ProgramID)
                                                    .FirstOrDefault();
                    if (proFund != null)
                    {
                        //delete
                        ctx.ProgramFund.Remove(proFund);
                        ctx.SaveChanges();
                        delete_result = "Success";
                    }
                    else {
                        delete_result += "Fund " + programFund.FundName + " already exists in system";
                    }
                }
                // create and open a connection object
                //conn = ConnectionManager.getConnection();
                //conn.Open();

                ////Check if user already exists in system
                //String query = "SELECT FundName FROM program_fund";
                //query += " WHERE 1=1";
                //query += " AND FundName = '" + programFund.FundName + "'";
                //query += " And ProgramID = " + programFund.ProgramID;


                //MySqlCommand command = new MySqlCommand(query, conn);
                //using (reader = command.ExecuteReader())
                //{
                //    if (reader.HasRows)
                //    {
                //        reader.Read();
                //        if (reader.GetValue(0).ToString().Trim() == programFund.FundName)
                //            OkForDelete = true;
                //    }
                //    else
                //        delete_result += "Fund " + programFund.FundName + " already exists in system";
                //}
                ////User does not already exist in system. Insert new user row in 'user' table
                //if (OkForDelete)
                //{
                //    //write to DB
                //    query = "delete from program_fund";
                //    query += " Where 1=1 And  FundName = '" + programFund.FundName + "'";
                //    query += " And ProgramID = " + programFund.ProgramID;

                //    command = new MySqlCommand(query, conn);
                //    command.ExecuteNonQuery();


                //    delete_result = "Success";
                //}

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
                if (conn != null)
                {
                    conn.Close();
                }
                if (reader != null)
                {
                    reader.Close();
                }
            }
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Exit Point", Logger.logLevel.Debug);

            return delete_result;
        }

        public static String updateProgramFund(ProgramFund programFund)
        {
            //user.FullName, user.UserID, user.FirstName, user.MiddleName, user.LastName, user.LoginPassword, user.Email, user.Role
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            MySqlConnection conn = null;
            MySqlDataReader reader = null;
            String update_result = "";
            bool OkForUpdate = false;
            var todayDate = DateTime.Today.ToString("yyyy-MM-dd");

            var temp = "";
            try
            {
                // create and open a connection object
                conn = ConnectionManager.getConnection();
                conn.Open();

                //Check if user already exists in system
                String query = "SELECT FundName FROM program_fund";
                query += " WHERE 1=1";
                query += " AND FundName = '" + programFund.FundName + "'";
                query += " And ProgramID = " + programFund.ProgramID;


                MySqlCommand command = new MySqlCommand(query, conn);
                command.Parameters.AddWithValue("@FundName", programFund.FundName);
                command.Parameters.AddWithValue("@ProgramID", programFund.ProgramID);
                using (reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        if (reader.GetValue(0).ToString().Trim() == programFund.FundName)
                            OkForUpdate = true;
                    }
                    else
                        update_result += "Fund " + programFund.FundName + " does not  exists in system";
                }
                //User does not already exist in system. Insert new user row in 'user' table
                if (OkForUpdate)
                {
                    //write to DB
                    query = "update program_fund";
                    query += " set FundAmount = @FundAmount,";
                    query += " FundUsed = @FundUsed,";
                    query += " FundRemaining = @FundRemaining,";
                    query += " UpdatedDate = @todayDate,";
                    query += " FundRequest = @FundRequest";
                    query += " Where FundName = @FundName";
                    query += " And ProgramID = @ProgramID" ;


                    command = new MySqlCommand(query, conn);
                    command.Parameters.AddWithValue("@FundAmount", programFund.FundAmount);
                    command.Parameters.AddWithValue("@FundUsed", programFund.FundUsed);
                    command.Parameters.AddWithValue("@FundRemaining", programFund.FundRemaining);
                    command.Parameters.AddWithValue("@todayDate", todayDate);
                    command.Parameters.AddWithValue("@FundRequest", programFund.FundRequest);
                    command.Parameters.AddWithValue("@FundName", programFund.FundName);
                    command.Parameters.AddWithValue("@ProgramID", programFund.ProgramID);
                    command.ExecuteNonQuery();


                    update_result = "Success";
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
                if (conn != null)
                {
                    conn.Close();
                }
                if (reader != null)
                {
                    reader.Close();
                }
            }
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Exit Point", Logger.logLevel.Debug);

            return update_result;
        }
    }
}