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
    [Table("project_location")]
    public class ProjectLocation
    {
        [NotMapped]
        public int Operation;

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProjectLocationID { get; set; }
        public string UserDefinedLocation { get; set; }

        public int ProjectID { get; set; }
        [ForeignKey("ProjectID")]
        [IgnoreDataMember]
        public virtual Project Project { get; set; }

        public int GeoLocationID { get; set; }
        [ForeignKey("GeoLocationID")]
        [IgnoreDataMember]
        public virtual GeoLocation GeoLocation { get; set; }

        //ProjectLocation() { }
        //ProjectLocation(String prg_id, String prg_name, String prgele_id, String prgele_name, String prj_id, String prj_name, String mgr, String sponsor, String id, String shape, String gloc, String uloc, String rgb)
        //{ ProgramID = prg_id; ProgramName = prg_name; ProgramElementID = prgele_id; ProgramElementName = prgele_name; DT_RowId = id; ProjectID = prj_id; ProjectName = prj_name; ProjectManager = mgr; ProjectSponsor = sponsor; GeoLocationID = id; ShapeType = shape; GeocodedLocation = gloc; UserDefinedLocation = uloc; RGBValue = rgb; }

        //From RequestProjectLocationController
        public static List<ProjectLocation> getProjectLocation(String ProgramID, String ProgramElementID, String ProjectID)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            List<ProjectLocation> MatchedProjectLocationList = new List<ProjectLocation>();

/*
            MySqlConnection conn = null;
            MySqlDataReader reader = null;
 */ 
            try
            {

                using (var ctx = new CPPDbContext())
                {
                    ctx.Database.Log = msg => Trace.WriteLine(msg);
                    if (ProgramID != "null")
                    {
                        int pgmId = int.Parse(ProgramID);
                        IQueryable<ProjectLocation> locations = ctx.ProjectLocation.Include("Project").Include("GeoLocation").Where(p => p.Project.Program.ProgramID == pgmId);
                        MatchedProjectLocationList = locations.ToList<ProjectLocation>();
                    }
                    else if (ProgramElementID != "null")
                    {
                        int pgmEltId = int.Parse(ProgramElementID);
                        IQueryable<ProjectLocation> locations = ctx.ProjectLocation.Include("Project").Include("GeoLocation").Where(p => p.Project.ProgramElement.ProgramElementID == pgmEltId);
                        MatchedProjectLocationList = locations.ToList<ProjectLocation>();
                    }
                    else if (ProjectID != "null")
                    {
                        int pgmEltId = int.Parse(ProjectID);
                        IQueryable<ProjectLocation> locations = ctx.ProjectLocation.Include("Project").Include("GeoLocation").Where(p => p.Project.ProjectID == pgmEltId);
                        MatchedProjectLocationList = locations.ToList<ProjectLocation>();
                    }

                }
/*
                // create and open a connection object
                conn = ConnectionManager.getConnection();
                conn.Open();

                String query = "SELECT prg.ProgramID, prg.ProgramName, prge.ProgramElementID, prge.ProgramElementName, prj.ProjectID, prj.ProjectName, prj.ProjectManager, prj.ProjectSponsor, prj_loc.GeoLocationID, prj_loc.ShapeType, prj_loc.GeocodedLocation, prj_loc.UserDefinedLocation, prj_loc.RGBValue";
                query += " FROM";
                query += " program prg, program_element prge, project prj, project_location prj_loc";
                query += " WHERE 1=1";
                query += " AND prg.ProgramID = prge.ProgramID";
                query += " AND prge.ProgramElementID = prj.ProgramElementID";
                query += " AND prj.ProjectID = prj_loc.ProjectID";
                if (ProgramID != "null")
                    query += " AND prg.ProgramID = '" + ProgramID + "'";
                if (ProgramElementID != "null")
                    query += " AND prge.ProgramElementID = '" + ProgramElementID + "'";
                if (ProjectID != "null")
                    query += " AND prj.ProjectID = '" + ProjectID + "'";

                MySqlCommand command = new MySqlCommand(query, conn);


                using (reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ProjectLocation RetreivedProjectLocation = new ProjectLocation(reader.GetValue(0).ToString().Trim(), reader.GetValue(1).ToString().Trim(), reader.GetValue(2).ToString().Trim(), reader.GetValue(3).ToString().Trim(), reader.GetValue(4).ToString().Trim(), reader.GetValue(5).ToString().Trim(), reader.GetValue(6).ToString().Trim(), reader.GetValue(7).ToString().Trim(), reader.GetValue(8).ToString().Trim(), reader.GetValue(9).ToString().Trim(), reader.GetValue(10).ToString().Trim(), reader.GetValue(11).ToString().Trim(), reader.GetValue(12).ToString().Trim());
                        MatchedProjectLocationList.Add(RetreivedProjectLocation);
                    }
                }
*/


            }
            catch (Exception ex)
            {
                var stackTrace = new StackTrace(ex, true);
                var line = stackTrace.GetFrame(0).GetFileLineNumber();
                Logger.LogExceptions(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, ex.Message, line.ToString(), Logger.logLevel.Exception);
            }
/*
            finally
            {
                if (conn != null) conn.Close();
                if (reader != null) reader.Close();

            }
*/ 
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Exit Point", Logger.logLevel.Debug);
            return MatchedProjectLocationList;

        }

        public static String registerProjectLocation(ProjectLocation projLocation)
        {

            String result = "";
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    ctx.Database.Log = msg => Trace.WriteLine(msg);
                    ctx.ProjectLocation.Add(projLocation);
                    ctx.SaveChanges();
                    result = "Success";
                }
            }
            catch (Exception ex)
            {
                var stackTrace = new StackTrace(ex, true);
                var line = stackTrace.GetFrame(0).GetFileLineNumber();
                Logger.LogExceptions(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, ex.Message, line.ToString(), Logger.logLevel.Exception);
            }
            return result;
        }


        public static String deleteProjectLocation(int ProjectLocationID)
        {

            String result = "";
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    ctx.Database.Log = msg => Trace.WriteLine(msg);
                    int projLocationId = ProjectLocationID;
                    ProjectLocation projLocation = ctx.ProjectLocation.First(p => p.ProjectLocationID == projLocationId);
                    ctx.ProjectLocation.Remove(projLocation);
                    ctx.SaveChanges();
                    result = "Success";
                }
            }
            catch (Exception ex)
            {
                var stackTrace = new StackTrace(ex, true);
                var line = stackTrace.GetFrame(0).GetFileLineNumber();
                Logger.LogExceptions(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, ex.Message, line.ToString(), Logger.logLevel.Exception);
            }
            return result;
        }

/*
        //From RegisterProjectLocationControlloer
        public static String registerProjectLocation(String GeoLocationID, String ProjectID, String ShapeType, String GeocodedLocation, String UserDefinedLocation, String RGBValue)
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

                //Check if location already exists in system for this project
                String query = "SELECT ProjectID from project_location";
                query += " WHERE";
                query += " ProjectID = '" + ProjectID + "'";
                MySqlCommand command = new MySqlCommand(query, conn);
                using (reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            if (reader.GetValue(0).ToString().Trim() == ProjectID)
                                register_result += "Location for Project '" + ProjectID + "' already exists in system";
                        }
                    }
                    else
                        OKForRegister = true;
                }

                //Program does not already exist in system. Insert new program row in 'program' table
                if (OKForRegister)
                {
                    //write to DB
                    query = "INSERT INTO project_location ( GeoLocationID, ProjectID, ShapeType, GeocodedLocation, UserDefinedLocation, RGBValue ) VALUES";
                    query += " (";
                    query += "'" + GeoLocationID + "', ";
                    query += "'" + ProjectID + "', ";
                    query += "'" + ShapeType + "', ";
                    query += "'" + GeocodedLocation + "', ";
                    query += "'" + UserDefinedLocation + "', ";
                    query += "'" + RGBValue + "'";
                    query += ")";
                    command = new MySqlCommand(query, conn);
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
                if (conn != null) conn.Close();
                if (reader != null) reader.Close();

            }
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Exit Point", Logger.logLevel.Debug);

            return register_result;
        }

        public static String updateProjectLocation(String GeoLocationID, String ProjectID, String ShapeType, String GeocodedLocation, String UserDefinedLocation, String RGBValue)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            MySqlConnection conn = null;
            MySqlDataReader reader = null;
            String update_result = "";
            bool OKForUpdate = false;
            try
            {
                // create and open a connection object
                conn = ConnectionManager.getConnection();
                conn.Open();

                //Check if program exists in system
                String query = "SELECT GeoLocationID from project_location";
                query += " WHERE 1=1";
                query += " AND GeoLocationID = '" + GeoLocationID + "'";
                MySqlCommand command = new MySqlCommand(query, conn);
                using (reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();

                        if (reader.GetValue(0).ToString().Trim() == GeoLocationID)
                            OKForUpdate = true;

                    }
                    else
                        update_result += "GeoLocation ID '" + GeoLocationID + "' does not exist in system";
                }

                //Update the Program
                if (OKForUpdate)
                {
                    //write to DB
                    query = "UPDATE project_location SET";
                    query += " GeoLocationID = '" + GeoLocationID + "',";
                    query += " ProjectID = '" + ProjectID + "',";
                    query += " ShapeType = '" + ShapeType + "',";
                    query += " GeocodedLocation = '" + GeocodedLocation + "'";
                    query += " UserDefinedLocation = '" + UserDefinedLocation + "'";
                    query += " RGBValue = '" + RGBValue + "'";
                    query += " WHERE";
                    query += " GeoLocationID = '" + GeoLocationID + "'";
                    command = new MySqlCommand(query, conn);
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
                if (conn != null) conn.Close();
                if (reader != null) reader.Close();
            }
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Exit Point", Logger.logLevel.Debug);

            return update_result;
        }

        public static String deleteProjectLocation(String GeoLocationID)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            MySqlConnection conn = null;
            MySqlDataReader reader = null;
            String delete_result = "";
            bool OKForDelete = false;
            if (GeoLocationID == "null")
            {
                delete_result = "Please pass a GeoLocationID";
                return delete_result;
            }
            try
            {
                // create and open a connection object
                conn = ConnectionManager.getConnection();
                conn.Open();

                //Check if program exists in system
                String query = "SELECT GeoLocationID from project_location";
                query += " WHERE 1=1";
                query += " AND GeoLocationID = '" + GeoLocationID + "'";
                MySqlCommand command = new MySqlCommand(query, conn);
                using (reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();

                        if (reader.GetValue(0).ToString().Trim() == GeoLocationID)
                            OKForDelete = true;

                    }
                    else
                        delete_result += "GeoLocationID '" + GeoLocationID + "' does not exist in system";
                }

                //Delete the Program
                if (OKForDelete)
                {
                    //write to DB
                    query = "DELETE FROM project_location";
                    query += " WHERE";
                    query += " GeoLocationID = '" + GeoLocationID + "'";
                    command = new MySqlCommand(query, conn);
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
                if (conn != null) conn.Close();
                if (reader != null) reader.Close();
            }
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Exit Point", Logger.logLevel.Debug);

            return delete_result;
        }
*/ 
    }
}