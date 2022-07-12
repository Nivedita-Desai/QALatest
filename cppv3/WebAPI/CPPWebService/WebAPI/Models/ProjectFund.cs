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
    [Table("ProjectFund")]
    public class ProjectFund
    {
        [NotMapped]
        public int Operation;

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public String FundName {get;set;}
        public int Id {get;set;}
        public int FundAmount {get; set;}
        public DateTime AppliedDate{get; set;}
        public int ProjectID {get; set;}


        //From RequestLookupFundTypeController
        public static List<ProjectFund> getProjectFund(int projectID)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            List<ProjectFund> MatchedProjectFundList = new List<ProjectFund>();
            MySqlConnection conn = null;
            MySqlDataReader reader = null;
            try
            {
                conn = ConnectionManager.getConnection();
                conn.Open();
                String query = "SELECT * FROM project_fund";
                query += " WHERE 1=1";
                query += " AND ProjectID = @projectID";
                MySqlCommand command = new MySqlCommand(query, conn);
                command.Parameters.AddWithValue("@projectID", projectID);

                IFormatProvider yyyymmddFormat = new System.Globalization.CultureInfo(String.Empty, false);
                using (reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ProjectFund RetreivedProjectFund = new ProjectFund();
                        //RetreivedApprovalMatrix.Id = reader.getValue(0);
                        //RetreivedApprovalMatrix
                        RetreivedProjectFund.Id= Convert.ToInt32(reader[0].ToString().Trim());
                        RetreivedProjectFund.FundName = reader[1].ToString().Trim();
                        RetreivedProjectFund.FundAmount= Convert.ToInt32(reader[2].ToString().Trim());
                        RetreivedProjectFund.AppliedDate = Convert.ToDateTime(reader[3].ToString().Trim());
                        RetreivedProjectFund.ProjectID = Convert.ToInt16(reader[4].ToString().Trim());
                        MatchedProjectFundList.Add(RetreivedProjectFund);
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
                if (conn != null)
                {
                    conn.Close();
                }
                if (reader != null)
                {
                    reader.Close();
                }
            }
            return MatchedProjectFundList;

        }


        public static String registerProjectFund(ProjectFund projectFund)
        {
            //user.FullName, user.UserID, user.FirstName, user.MiddleName, user.LastName, user.LoginPassword, user.Email, user.Role
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            MySqlConnection conn = null;
            MySqlDataReader reader = null;
            String register_result = "";
            bool OKForRegister = false;
            var todayDate = DateTime.Today.ToString("yyyy-MM-dd");
            
            var temp = "";
            try
            {
                // create and open a connection object
                conn = ConnectionManager.getConnection();
                conn.Open();
        
                //Check if user already exists in system
                String query = "SELECT FundName FROM project_fund";
                query += " WHERE 1=1";
                query += " AND FundName = @FundName";
                query += " And ProjectID = @ProjectID" ;


                MySqlCommand command = new MySqlCommand(query, conn);
                command.Parameters.AddWithValue("@FundName", projectFund.FundName);
                command.Parameters.AddWithValue("@ProjectID", projectFund.ProjectID);
                using (reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        if (reader.GetValue(0).ToString().Trim() == projectFund.FundName)
                            register_result += "Fund " + projectFund.FundName + " already exists in system";
                    }
                    else
                        OKForRegister = true;
                }
                //User does not already exist in system. Insert new user row in 'user' table
                if (OKForRegister)
                {
                    //write to DB
                    query = "INSERT INTO project_fund ( FundName, FundAmount, AppliedDate, ProjectID) VALUES";
                    query += " (";
                    query += "@FundName, ";
                    query += "@FundAmount, ";
                    query += "@todayDate,";
                    query += "@ProjectID";
                    query += ")";
                    command = new MySqlCommand(query, conn);
                    command.Parameters.AddWithValue("@FundName", projectFund.FundName);
                    command.Parameters.AddWithValue("@FundAmount", projectFund.FundAmount);
                    command.Parameters.AddWithValue("@todayDate", todayDate);
                    command.Parameters.AddWithValue("@ProjectID", projectFund.ProjectID);
                    command.ExecuteNonQuery();
            


                    query = "update_project_fund"; //Stored Procedure
                    command = new MySqlCommand(query, conn);
                    command.CommandType = CommandType.StoredProcedure;

                    //For Create New
                    command.Parameters.Add(new MySqlParameter("_Operation", 1));
                    command.Parameters.Add(new MySqlParameter("_ProjectID", projectFund.ProjectID));
                  
                    command.ExecuteNonQuery();
                    register_result = "Success";
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

            return register_result;
        }
        public static String updateProjectFund(ProjectFund projectFund)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            MySqlConnection conn = null;
            MySqlDataReader reader = null;
            String update_result = "";
            var todayDate = DateTime.Today.ToString("yyyy-MM-dd");
            bool OKForUpdate = false;
            try
            {
                // create and open a connection object
                conn = ConnectionManager.getConnection();
                conn.Open();

                //Check if FTE Position exists in system
                String query = "SELECT Id from project_fund";
                query += " WHERE 1=1";
                query += " AND Id = @Id";
                MySqlCommand command = new MySqlCommand(query, conn);
                command.Parameters.AddWithValue("@Id", projectFund.Id);
                using (reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();

                        if (Convert.ToInt16(reader.GetValue(0).ToString().Trim()) == projectFund.Id)
                            OKForUpdate = true;

                    }
                    else
                        update_result += "Fund '" + projectFund.FundName + "' does not exist in system";
                }

                //Update the Program
                if (OKForUpdate)
                {
                    //write to DB
                    query = "UPDATE project_fund SET";
                    query += " FundName = @FundName,";
                    query += " FundAmount = @FundAmount,";
                    query += " AppliedDate = @todayDate,";
                    query += " ProjectID = @ProjectID";
                    query += " WHERE";
                    query += " Id = @Id";
                    command = new MySqlCommand(query, conn);
                    command.Parameters.AddWithValue("@FundName", projectFund.FundName);
                    command.Parameters.AddWithValue("@FundAmount", projectFund.FundAmount);
                    command.Parameters.AddWithValue("@todayDate", todayDate);
                    command.Parameters.AddWithValue("@ProjectID", projectFund.ProjectID);
                    command.Parameters.AddWithValue("@Id", projectFund.Id);
                    command.ExecuteNonQuery();
                    update_result = "Success";
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

        public static String deleteProjectFund(ProjectFund projectFund)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            MySqlConnection conn = null;
            MySqlDataReader reader = null;
            String delete_result = "";
            bool OKForDelete = false;
            try
            {
                // create and open a connection object
                conn = ConnectionManager.getConnection();
                conn.Open();

                //Check if FTE Position exists in system
                String query = "SELECT Id from project_fund";
                query += " WHERE 1=1";
                query += " AND Id = @Id";
                MySqlCommand command = new MySqlCommand(query, conn);
                command.Parameters.AddWithValue("@Id", projectFund.Id);
                using (reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();

                        if (Convert.ToInt16(reader.GetValue(0).ToString().Trim()) == projectFund.Id)
                            OKForDelete = true;

                    }
                    else
                        delete_result += "Fund '" + projectFund.FundName + "' does not exist in system";
                }

                //Delete the Program
                if (OKForDelete)
                {
                    //write to DB
                    query = "DELETE FROM project_fund";
                    query += " WHERE";
                    query += " 1 = 1";
                    query += " And Id = @Id ";
                    command = new MySqlCommand(query, conn);
                    command.Parameters.AddWithValue("@Id", projectFund.Id);
                    command.ExecuteNonQuery();
                    delete_result = "Success";
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
    
    }
}