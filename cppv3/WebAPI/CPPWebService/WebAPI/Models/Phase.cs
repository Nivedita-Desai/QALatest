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

namespace WebAPI.Models
{
    public class Phase
    {
        public String ProjectID;
        public String TrendNumber;
        public String PhaseCode;
        public String PhaseDescription;
        public String PostTrendPhaseStartDate;
        public String PostTrendPhaseEndDate;
        public String PreTrendPhaseStartDate;
        public String PreTrendPhaseEndDate;
        public int PreTrendPhaseCost;
        public int PostTrendPhaseCost;
        public int VersionId;
        Phase() { }

        //From RequestPhaseController
        public static List<Phase> getPhase(String ProjectID, String TrendNumber, String PhaseCode, String PhaseStartDate, String PhaseEndDate)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            List<Phase> MatchedPhaseList = new List<Phase>();
            MySqlConnection conn = null;
            MySqlDataReader reader = null;
            MySqlDataReader pretrend_reader = null;
            MySqlConnection pretrend_conn = null;
            try
            {

                // create and open a connection object
                conn = ConnectionManager.getConnection();
                conn.Open();

                String query = "SELECT ProjectID, TrendNumber, PhaseCode, sum(FTECost), sum(LumpsumCost), sum(UnitCost), sum(PercentageBasisCost), min(ActivityStartDate), max(ActivityEndDate),PhaseDescription";
                query += " FROM";
                query += " vcosts";
                query += " WHERE 1=1";
                if (ProjectID != "null")
                {
                    query += " AND ProjectID = @ProjectID";
                }
                if (TrendNumber != "null")
                {
                    query += " AND TrendNumber = @TrendNumber " ;
                }
                if (PhaseCode != "null")
                {
                    query += " AND PhaseCode = @PhaseCode ";
                }
                /*if (PhaseStartDate != "null")
                {
                    query += " AND PhaseStartDate = " + PhaseStartDate;
                }
                if (PhaseEndDate != "null")
                {
                    query += " AND PhaseEndDate = " + PhaseEndDate;
                }*/
                query += " group by ProjectID, PhaseCode, TrendNumber;";

                
                MySqlCommand command = new MySqlCommand(query, conn);
                command.Parameters.AddWithValue("@ProjectID", ProjectID);
                command.Parameters.AddWithValue("@TrendNumber", TrendNumber);
                command.Parameters.AddWithValue("@PhaseCode", PhaseCode);

                using (reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        //Phase RetreivedPhase = new Phase(reader.GetValue(0).ToString().Trim(), reader.GetValue(1).ToString().Trim(), reader.GetValue(2).ToString().Trim(), reader.GetValue(3).ToString().Trim(), reader.GetValue(4).ToString().Trim());
                        Phase RetreivedPhase = new Phase();

                        RetreivedPhase.ProjectID = reader.GetValue(0).ToString().Trim();
                        RetreivedPhase.TrendNumber = reader.GetValue(1).ToString().Trim();
                        RetreivedPhase.PhaseCode = reader.GetValue(2).ToString().Trim();
                        RetreivedPhase.PhaseDescription = reader.GetValue(9).ToString().Trim();
                        RetreivedPhase.PostTrendPhaseStartDate = reader.GetDateTime(7).ToString("yyyy-MM-dd").Trim();
                        RetreivedPhase.PostTrendPhaseEndDate = reader.GetDateTime(8).ToString("yyyy-MM-dd").Trim();
                        //RetreivedPhase.PostTrendPhaseCost = int.Parse(reader.GetValue(3).ToString().Trim()) + int.Parse(reader.GetValue(3).ToString().Trim()) + int.Parse(reader.GetValue(5).ToString().Trim()) + int.Parse(reader.GetValue(6).ToString().Trim());
                        int temp = 0;
                        RetreivedPhase.PostTrendPhaseCost = 0;
                        if (int.TryParse(reader.GetValue(3).ToString().Trim(), out temp))
                            RetreivedPhase.PostTrendPhaseCost += int.Parse(reader.GetValue(3).ToString().Trim());
                        if (int.TryParse(reader.GetValue(4).ToString().Trim(), out temp))
                            RetreivedPhase.PostTrendPhaseCost += int.Parse(reader.GetValue(4).ToString().Trim());
                        if (int.TryParse(reader.GetValue(5).ToString().Trim(), out temp))
                            RetreivedPhase.PostTrendPhaseCost += int.Parse(reader.GetValue(5).ToString().Trim());
                        if (int.TryParse(reader.GetValue(6).ToString().Trim(), out temp))
                            RetreivedPhase.PostTrendPhaseCost += int.Parse(reader.GetValue(6).ToString().Trim());

                        if (int.Parse(RetreivedPhase.TrendNumber) == 0)
                        {
                            RetreivedPhase.PreTrendPhaseCost = 0;
                            RetreivedPhase.PreTrendPhaseStartDate = "Not Applicable";
                            RetreivedPhase.PreTrendPhaseEndDate = "Not Applicable";
                        }
                        else
                        {
                           
                            String pretrend_query = "SELECT sum(vc.FTECost), sum(vc.LumpsumCost), sum(vc.UnitCost), sum(vc.PercentageBasisCost), min(vc.ActivityStartDate), max(vc.ActivityEndDate)";
                            pretrend_query += " FROM";
                            pretrend_query += " vcosts vc, trend t";
                            pretrend_query += " WHERE 1=1";
                            pretrend_query += " AND vc.ProjectID = @ProjectID";
                            pretrend_query += " AND t.PreviousApprovedTrend =  @TrendNumber";
                            pretrend_query += " AND vc.PhaseCode = @PhaseCode";
                            pretrend_query += " AND vc.ProjectID = t.ProjectID";
                            /*if (PhaseStartDate != "null")
                            {
                                pretrend_query += " AND PhaseStartDate = " + RetreivedPhase.PhaseStartDate;
                            }
                            if (PhaseEndDate != "null")
                            {
                                pretrend_query += " AND PhaseEndDate = " + RetreivedPhase.PhaseEndDate;
                            }*/
                            pretrend_query += " group by vc.ProjectID, vc.PhaseCode, vc.TrendNumber;";
                            pretrend_conn = ConnectionManager.getConnection();
                            pretrend_conn.Open();
                            MySqlCommand pretrend_command = new MySqlCommand(pretrend_query, pretrend_conn);
                            pretrend_command.Parameters.AddWithValue("@ProjectID", RetreivedPhase.ProjectID);
                            pretrend_command.Parameters.AddWithValue("@TrendNumber", RetreivedPhase.TrendNumber);
                            pretrend_command.Parameters.AddWithValue("@PhaseCode", RetreivedPhase.PhaseCode);
                            using (pretrend_reader = pretrend_command.ExecuteReader())
                            {
                                if (pretrend_reader.HasRows)
                                {
                                    while (pretrend_reader.Read())
                                    {
                                        //RetreivedPhase.PreTrendPhaseCost = int.Parse(reader.GetValue(0).ToString().Trim()) + int.Parse(reader.GetValue(1).ToString().Trim()) + int.Parse(reader.GetValue(2).ToString().Trim()) + int.Parse(reader.GetValue(3).ToString().Trim());
                                        RetreivedPhase.PostTrendPhaseStartDate = pretrend_reader.GetDateTime(4).ToString("yyyy-MM-dd").Trim();
                                        RetreivedPhase.PostTrendPhaseEndDate = pretrend_reader.GetDateTime(5).ToString("yyyy-MM-dd").Trim();
                                        temp = 0;
                                        RetreivedPhase.PreTrendPhaseCost = 0;
                                        if (int.TryParse(pretrend_reader.GetValue(0).ToString().Trim(), out temp))
                                            RetreivedPhase.PreTrendPhaseCost += int.Parse(pretrend_reader.GetValue(0).ToString().Trim());
                                        if (int.TryParse(pretrend_reader.GetValue(1).ToString().Trim(), out temp))
                                            RetreivedPhase.PreTrendPhaseCost += int.Parse(pretrend_reader.GetValue(1).ToString().Trim());
                                        if (int.TryParse(pretrend_reader.GetValue(2).ToString().Trim(), out temp))
                                            RetreivedPhase.PreTrendPhaseCost += int.Parse(pretrend_reader.GetValue(2).ToString().Trim());
                                        if (int.TryParse(pretrend_reader.GetValue(3).ToString().Trim(), out temp))
                                            RetreivedPhase.PreTrendPhaseCost += int.Parse(pretrend_reader.GetValue(3).ToString().Trim());                                    
                                    }
                                }
                                else
                                {
                                    RetreivedPhase.PreTrendPhaseCost = 0;
                                    RetreivedPhase.PreTrendPhaseStartDate = "Not Applicable";
                                    RetreivedPhase.PreTrendPhaseEndDate = "Not Applicable";
                                }
                            }
                        }
                        MatchedPhaseList.Add(RetreivedPhase);
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
                if (pretrend_conn != null) pretrend_conn.Close();
                if (pretrend_reader != null) pretrend_reader.Close();
            }
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Exit Point", Logger.logLevel.Debug);
            return MatchedPhaseList;

        }


        public static String registerPhase(String ProjectID, String TrendNumber, String PhaseCode, String PhaseStartDate, String PhaseEndDate)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            MySqlConnection conn = null;
            MySqlDataReader reader = null;
            String register_result = "";
            bool OKForRegister = false;
            try
            {
                // create and open a connection object
                conn = ConnectionManager.getConnection();
                conn.Open();

                //Check if phase already exists in system for this project and this trend
                String query = "SELECT PhaseCode from project";
                query += " WHERE 1=1";
                query += " AND ProjectID = @ProjectID";
                query += " AND TrendNumber = @TrendNumber";
                MySqlCommand command = new MySqlCommand(query, conn);
                command.Parameters.AddWithValue("@ProjectID", ProjectID);
                command.Parameters.AddWithValue("@TrendNumber", TrendNumber);
                
                using (reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        if (reader.GetValue(0).ToString().Trim() == PhaseCode)
                            register_result += "Phase '" + PhaseCode + "' already exists for this Trend";
                    }
                    else
                        OKForRegister = true;
                }

                //Program Element does not already exist in system. Insert new program element row in 'program element' table
                if (OKForRegister)
                {
                    //write to DB
                    query = "INSERT INTO phase (ProjectID, TrendNumber, PhaseCode, PhaseStartDate, PhaseEndDate) VALUES";
                    query += " (";
                    query += "@ProjectID, ";
                    query += "@TrendNumber, ";
                    query += "@PhaseCode, ";
                    query += "@PhaseStartDate, ";
                    query += "@PhaseEndDate";
                    query += ")";
                    command = new MySqlCommand(query, conn);
                    command.Parameters.AddWithValue("@ProjectID", ProjectID);
                    command.Parameters.AddWithValue("@TrendNumber", TrendNumber);
                    command.Parameters.AddWithValue("@PhaseCode", PhaseCode);
                    command.Parameters.AddWithValue("@PhaseStartDate", PhaseStartDate);
                    command.Parameters.AddWithValue("@PhaseEndDate", PhaseEndDate);
                    command.ExecuteNonQuery();
                    register_result = "Success";
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

            return register_result;
        }




        public static String deletePhase(String ProjectID, String TrendNumber, String PhaseCode)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            MySqlConnection conn = null;
            MySqlDataReader reader = null;
            String delete_result = "";
            if (ProjectID == "null" || TrendNumber == "null" || PhaseCode == "null")
            {
                return "Please pass the ProjectID, TrendNumber and PhaseCode to delete";
            }
            bool OKForDelete = false;
            try
            {
                // create and open a connection object
                conn = ConnectionManager.getConnection();
                conn.Open();

                //Check if program exists in system
                String query = "SELECT ProjectID, TrendNumber, PhaseCode from phase";
                query += " WHERE 1=1";
                query += " AND ProjectID = @ProjectID";
                query += " AND TrendNumber = @TrendNumber";
                query += " AND PhaseCode = @PhaseCode";
                MySqlCommand command = new MySqlCommand(query, conn);
                command.Parameters.AddWithValue("@ProjectID", ProjectID);
                command.Parameters.AddWithValue("@TrendNumber", TrendNumber);
                command.Parameters.AddWithValue("@PhaseCode", PhaseCode);
                using (reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();

                        if (reader.GetValue(0).ToString().Trim() == ProjectID)
                            OKForDelete = true;

                    }
                    else
                        delete_result += "Phase '" + PhaseCode + "' does not exist in system";
                }

                //Delete the Program
                if (OKForDelete)
                {
                    //write to DB
                    query = "DELETE FROM phase";
                    query += " WHERE";
                    query += " ProjectID = @ProjectID";
                    query += " AND TrendNumber = @TrendNumber";
                    query += " AND PhaseCode = @PhaseCode";
                    command = new MySqlCommand(query, conn);
                    command.Parameters.AddWithValue("@ProjectID", ProjectID);
                    command.Parameters.AddWithValue("@TrendNumber", TrendNumber);
                    command.Parameters.AddWithValue("@PhaseCode", PhaseCode);
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