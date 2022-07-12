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
using WebAPI.Helper;

namespace WebAPI.Models
{
    [Table("phase_lookup")]
    public class PhaseCode : Audit
    {
        [NotMapped]
        public int Operation;

        [NotMapped]
        public int Order { get; set; }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PhaseID{get;set; }

        public String PhaseDescription { get; set; }
        public String Code { get; set; }
        public String ActivityPhaseCode { get; set; }

        public String PhaseNote { get; set; }

        public int VersionId { get; set; }

        //PhaseCode(int phaseId, String desc, String code)
        //{ PhaseID = phaseId; PhaseDescription = desc; Code = code; }
        //PhaseCode() { }

        //From RequestLookupPhaseCodeController
        public static List<PhaseCode> getPhaseCode(String PhaseDescription, String PhaseCode, String ProjectID)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            List<PhaseCode> MatchedPhaseCodeList = new List<PhaseCode>();

            MySqlConnection conn = null;
            MySqlDataReader reader = null;
            try
            {
                //Luan experimental
                if (ProjectID != "null")
                {
                    conn = ConnectionManager.getConnection();
                    conn.Open();

                     var query = String.Format("call get_phases_by_project_id({0})", ProjectID);// phase mapping by division
                   // var query = String.Format("Call get_phase_by_lob({0})", ProjectID);
                    MySqlCommand command = new MySqlCommand(query, conn);

                    using (reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PhaseCode pc = new PhaseCode();
                            pc.PhaseID = Convert.ToInt32(reader.GetValue(0));
                            pc.PhaseDescription = reader.GetValue(1).ToString();
                            pc.Code = reader.GetValue(2).ToString();
                            pc.ActivityPhaseCode = reader.GetValue(3).ToString();
                            //reader.getValue(4) return: ProjectID
                            pc.Order = Convert.ToInt32(reader.GetValue(5));

                            MatchedPhaseCodeList.Add(pc);
                        }
                    }

                            //MatchedPhaseCodeList = ctx.Database.SqlQuery<PhaseCode>("call get_phases_by_project_id(@_ProjectID)",
                            //            new MySql.Data.MySqlClient.MySqlParameter("@_ProjectID", ProjectID)).ToList();

                }
                else
                {
                    using (var ctx = new CPPDbContext())
                    {
                        Versionmaster latestVersion = ctx.VersionMaster.OrderByDescending(a => a.CreatedDate).FirstOrDefault();
                        MatchedPhaseCodeList = ctx.PhaseCode.Where(a => a.VersionId == latestVersion.Id).ToList();
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
            return MatchedPhaseCodeList;

        }


        public static String registerPhaseCode(PhaseCode phase)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            MySqlConnection conn = null;
            MySqlDataReader reader = null;
            String register_result = "";
            bool OKForRegister = false;
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    PhaseCode retrievedPhase = new PhaseCode();
                    retrievedPhase = ctx.PhaseCode.Where(p => p.Code == phase.Code && p.PhaseDescription == phase.PhaseDescription)
                        .FirstOrDefault();
                    Versionmaster latestVersion = ctx.VersionMaster.OrderByDescending(a => a.CreatedDate).FirstOrDefault();
                    phase.VersionId = latestVersion.Id;
                    // PhaseCode maxPhaseCode = ctx.PhaseCode.OrderByDescending(a=>a.ActivityPhaseCode).FirstOrDefault();
                    //int  maxCode = Convert.ToInt16(maxPhaseCode.ActivityPhaseCode);
                    // maxCode++;

                    // String newCode = maxCode.ToString().PadLeft(2,'0');

                    if (retrievedPhase == null)
                    {
                        //register
                       // phase.ActivityPhaseCode = newCode;
                        ctx.PhaseCode.Add(phase);
                        ctx.SaveChanges();
                        register_result += phase.PhaseDescription + " has been created successfully.\n";
                    }
                    else
                    {
                        register_result += phase.PhaseDescription + " failed to be created, it already exist.\n";
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

            return register_result;
        }

        public static String updatePhaseCode(PhaseCode phase)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String result = "";
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    PhaseCode retrievedPhaseCode = new PhaseCode();
                    retrievedPhaseCode = ctx.PhaseCode.Where(p =>  p.PhaseID == phase.PhaseID).FirstOrDefault();

                    PhaseCode duplicatePhaseCode = new PhaseCode();
                    duplicatePhaseCode = ctx.PhaseCode.Where(u => (u.Code == phase.ActivityPhaseCode || u.PhaseDescription == phase.PhaseDescription)    //u.Code == phase.Code
                                                                    && u.PhaseID != phase.PhaseID).FirstOrDefault();
                    Versionmaster latestVersion = ctx.VersionMaster.OrderByDescending(a => a.CreatedDate).FirstOrDefault();
                    phase.VersionId = latestVersion.Id;

                    if (retrievedPhaseCode != null && retrievedPhaseCode.Code != phase.Code)
                    {
                        return "Updating the phase code is unavailable at this moment (" + phase.PhaseDescription + "). \n";
                    }
                    else if (duplicatePhaseCode != null)
                    {
                        result = phase.Code + " failed to be updated, duplicate will be created.\n";
                    }
                    else if (retrievedPhaseCode != null)
                    {
                        CopyUtil.CopyFields<PhaseCode>(phase, retrievedPhaseCode);

                        using (var dbCtx = new CPPDbContext())
                        {
                            dbCtx.Entry(retrievedPhaseCode).State = System.Data.Entity.EntityState.Modified;
                            dbCtx.SaveChanges();
                            result = phase.PhaseDescription + " has been updated successfully.\n";
                        }
                    }
                    else
                    {
                        result += phase.PhaseDescription + " failed to be updated, it does not exist.\n";
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

        public static String deletePhaseCode(PhaseCode phase)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String result = "";
            try
            {

                using (var ctx = new CPPDbContext())
                {
                    PhaseCode retrievedPhaseCode = new PhaseCode();
                    retrievedPhaseCode = ctx.PhaseCode.Where(p => p.PhaseID == phase.PhaseID).FirstOrDefault();
                    if (retrievedPhaseCode != null)
                    {
                        //delete
                        ctx.PhaseCode.Remove(retrievedPhaseCode);
                        ctx.SaveChanges();
                        result += phase.PhaseDescription + " has been deleted successfully.\n";
                    }
                    else
                    {
                        result += phase.PhaseDescription + " failed to be deleted, it does not exist.\n";
                    }
                }
            }
            catch (Exception ex)
            {
                result += phase.PhaseDescription + " failed to be deleted due to dependencies.\n";
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