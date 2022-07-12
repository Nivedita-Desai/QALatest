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
    [Table ("activity_category")]
    public class ActivityCategory : Audit
    {
        [NotMapped]
        public int Operation { get; set; }
        [NotMapped]
        public int SuccessTask { get; set; }
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public String CategoryID { get; set; }
        public String CategoryDescription { get; set; }
        public String SubCategoryID { get; set; }
        [StringLength(60)]
        public String SubCategoryDescription { get; set; }
        public String Phase { get; set; }        //Should be foreign key
        public String Services { get; set; }        //Aditya: Add Services column 04062022
        public int? OrganizationID { get; set; }
        //ON DELETE RESTRICT
        //Nullable foreign key
        [ForeignKey("OrganizationID")]
        public virtual Organization Organization { get; set; }

        public int? VersionId { get; set; }


        //public String DT_RowId { get; set; }
        //ActivityCategory(String cat_id, String cat_desc, String sub_cat_id, String sub_cat_desc, String phase)
        //{ DT_RowId = string.Concat(CategoryID, SubCategoryID);  CategoryID = cat_id;
        //CategoryDescription = cat_desc; SubCategoryID = sub_cat_id; SubCategoryDescription = sub_cat_desc; Phase = phase;
        //}
        //ActivityCategory() { }

        //From RequestLookupActivityCategoryController
        public static List<ActivityCategory> getActivityCategory(String CategoryID, String CategoryDescription, String SubCategoryID, String SubCategoryDescription)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            List<ActivityCategory> MatchedActivityCategoryList = new List<ActivityCategory>();

           
        
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    Versionmaster latestVersion = ctx.VersionMaster.OrderByDescending(a => a.CreatedDate).FirstOrDefault();
                    MatchedActivityCategoryList = ctx.ActivityCategory.Where(a => a.VersionId == latestVersion.Id).ToList();

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
            return MatchedActivityCategoryList;

        }

        public static List<ActivityCategory> getActivityCategoryByOrgID(int OrganizationID, int VersionID)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            List<ActivityCategory> MatchedActivityCategoryList = new List<ActivityCategory>();

            try
            {
                using (var ctx = new CPPDbContext())
                {
                    //Versionmaster latestVersion = ctx.VersionMaster.Where(a => a.OrganizationID == OrganizationID).OrderByDescending(a => a.CreatedDate).FirstOrDefault();
                    if (OrganizationID.Equals(0))
                    {
                        MatchedActivityCategoryList = ctx.ActivityCategory.Where(a => a.OrganizationID == null && a.VersionId == VersionID).OrderBy(a => a.CategoryID).ThenBy(a => a.SubCategoryID).ToList();
                    }
                    else
                    {
                        MatchedActivityCategoryList = ctx.ActivityCategory.Where(a => a.OrganizationID == OrganizationID && a.VersionId == VersionID).OrderBy(a => a.CategoryID).ThenBy(a => a.SubCategoryID).ToList();
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
            return MatchedActivityCategoryList;

        }

        public static List<ActivityCategory> getMainCategory(String phase, String OrganizationID, String ProjectId)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            List<ActivityCategory> MatchedActivityCategoryList = new List<ActivityCategory>();
            //string activityPhase = "";
            int orgID = Convert.ToInt16(OrganizationID);
            int? prjId = Convert.ToInt32(ProjectId);
            //activityPhase = getPhaseNameByCode(phase);
            try
            {

                using (var ctx = new CPPDbContext())
                {
                    Project ProjectDetails = ctx.Project.Where(p => p.ProjectID == prjId).FirstOrDefault();
                    int? versionId = Convert.ToInt32(ProjectDetails.VersionId);
                    int TotalActivityCount = ctx.ActivityCategory.Where(p => p.VersionId == versionId).Count();
                    int TotalServiceCount = ctx.ActivityCategory.Where(p => p.VersionId == versionId && p.Services == null).Count();

                    if (TotalActivityCount== TotalServiceCount)
                    {
                        MatchedActivityCategoryList = ctx.ActivityCategory.Where(p => p.OrganizationID == orgID && p.VersionId == versionId)
                                                            .OrderBy(a => a.CategoryDescription).Distinct().ToList();
                    }
                    else
                    {
                        MatchedActivityCategoryList = ctx.ActivityCategory.Where(p => p.OrganizationID == orgID && p.VersionId == versionId && p.Services == ProjectDetails.ProjectClassID.ToString())
                                                        .OrderBy(a => a.CategoryDescription).Distinct().ToList();
                    }
                    //MatchedActivityCategoryList = ctx.ActivityCategory.Where(p => /*(p.Phase == activityPhase || p.Phase == "All") &&*/ p.OrganizationID == orgID && p.VersionId == versionId)
                    //                                    .OrderBy(a => a.CategoryDescription).Distinct().ToList();
                    
                    MatchedActivityCategoryList = MatchedActivityCategoryList.GroupBy(a => a.CategoryID).Select(a => a.First()).OrderBy(a=>a.CategoryID).ToList();
                    
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
            return MatchedActivityCategoryList;
        }


        public static List<ActivityCategory> getSubCategory(int OrganizationID, String CategoryID, String phase, String VersionId, String ProjectId)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            List<ActivityCategory> MatchedActivityCategoryList = new List<ActivityCategory>();
     
            //string activityPhase = "";
            //activityPhase = getPhaseNameByCode(phase);
            int? verId = Convert.ToInt32(VersionId);
            int? prjId = Convert.ToInt32(ProjectId);
            try
            {

                using (var ctx = new CPPDbContext())
                {
                    int TotalActivityCount = ctx.ActivityCategory.Where(p => p.VersionId == verId).Count();
                    int TotalServiceCount = ctx.ActivityCategory.Where(p => p.VersionId == verId && p.Services == null).Count();

                    Project ProjectDetails = ctx.Project.Where(p => p.ProjectID == prjId).FirstOrDefault();

                    if (TotalActivityCount == TotalServiceCount)
                    {
                        MatchedActivityCategoryList = ctx.ActivityCategory.
                            Where(a => a.CategoryID == CategoryID && /*(a.Phase == activityPhase || a.Phase == "All") &&*/ (a.OrganizationID == null || a.OrganizationID == OrganizationID) && a.VersionId == verId )
                            .OrderBy(a => a.SubCategoryID)
                            .ToList();
                    }
                    else
                    {
                        MatchedActivityCategoryList = ctx.ActivityCategory.
                            Where(a => a.CategoryID == CategoryID && /*(a.Phase == activityPhase || a.Phase == "All") &&*/ (a.OrganizationID == null || a.OrganizationID == OrganizationID) && a.VersionId == verId && a.Services == ProjectDetails.ProjectClassID.ToString())
                            .OrderBy(a => a.SubCategoryID)
                            .ToList();
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
            return MatchedActivityCategoryList;
        }
        public static String registerActivityCategory(ActivityCategory activity, Versionmaster latestVersion)
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
                    /*Versionmaster latestVersion = ctx.VersionMaster.Where(s => s.OrganizationID == activity.OrganizationID)
                                                    .OrderByDescending(a => a.CreatedDate).FirstOrDefault();*/
                    activity.VersionId = latestVersion.Id;
                    ActivityCategory retrievedActivityCategory = new ActivityCategory();
                    retrievedActivityCategory = ctx.ActivityCategory.Where(s => s.CategoryID == activity.CategoryID
                                                                                //&& s.CategoryDescription == activity.CategoryDescription
                                                                                && (s.SubCategoryID == activity.SubCategoryID
                                                                                || s.SubCategoryDescription == activity.SubCategoryDescription)
                                                                                && s.OrganizationID == activity.OrganizationID
                                                                                //&& s.Phase == activity.Phase Subtask
                                                                                && s.Services == activity.Services //Aditya 04062022
                                                                                && s.VersionId == activity.VersionId).FirstOrDefault();

                    if (retrievedActivityCategory == null)
                    {
                        //register

                        
                    ctx.ActivityCategory.Add(activity);
                        ctx.SaveChanges();
                        //register_result += "Subcategory ID " + activity.SubCategoryID + " has been created successfully.\n";    //front end relying the word "successfully"
                        register_result += "SubCategory ID " + activity.SubCategoryID + " has been created successfully.\n";    /*Tanmay - 01/12/2021*/
                        activity.SuccessTask++; // Narayan - incriment success task - 14/06/2022
                    }
                    else
                    {
                        if (retrievedActivityCategory.SubCategoryID == activity.SubCategoryID
                            || retrievedActivityCategory.SubCategoryDescription == activity.SubCategoryDescription)
                        {
                            //register_result += "Subcategory " + activity.SubCategoryID +  " - " + activity.SubCategoryDescription 
                            //                + " for Category ID " + activity.CategoryID + " - " + activity.CategoryDescription 
                            //                + " failed to be created, it already exist.\n";

                            register_result += "Subtask " + activity.SubCategoryID + " - " + activity.SubCategoryDescription 
                                            + " for Task ID " + activity.CategoryID + " - " + activity.CategoryDescription
                                            + " failed to be created, it already exist.\n"; // Manasi
                        }
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
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Exit Point", Logger.logLevel.Debug);

            return register_result;
        }

        public static String registerSingleActivityCategory(ActivityCategory activity)
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
                    ActivityCategory retrievedActivityCategory = new ActivityCategory();
                    retrievedActivityCategory = ctx.ActivityCategory.Where(s => s.CategoryID == activity.CategoryID
                                                                                //&& s.CategoryDescription == activity.CategoryDescription
                                                                                && (s.SubCategoryID == activity.SubCategoryID
                                                                                || s.SubCategoryDescription == activity.SubCategoryDescription)
                                                                                && s.OrganizationID == activity.OrganizationID
                                                                                //&& s.Phase == activity.Phase
                                                                                ).FirstOrDefault();

                    if (retrievedActivityCategory == null)
                    {
                        //register
                        //Versionmaster latestVersion = ctx.VersionMaster.Where(s => s.OrganizationID == activity.OrganizationID)
                        //                                .OrderByDescending(a => a.CreatedDate).FirstOrDefault();
                        //activity.VersionId = latestVersion.Id;
                        ctx.ActivityCategory.Add(activity);
                        ctx.SaveChanges();
                        //register_result += "Subcategory ID " + activity.SubCategoryID + " has been created successfully.\n";    //front end relying the word "successfully"
                        register_result += "SubCategory " + activity.SubCategoryID + " has been created successfully.\n";    //front end relying the word "successfully" updated by Manasi
                    }
                    else
                    {
                        if (retrievedActivityCategory.SubCategoryID == activity.SubCategoryID
                            || retrievedActivityCategory.SubCategoryDescription == activity.SubCategoryDescription)
                        {
                            //register_result += "Subcategory " + activity.SubCategoryID +  " - " + activity.SubCategoryDescription 
                            //                + " for Category ID " + activity.CategoryID + " - " + activity.CategoryDescription 
                            //                + " failed to be created, it already exist.\n";

                            register_result += "Subtask " + activity.SubCategoryID + " - " + activity.SubCategoryDescription
                                            + " for Task ID " + activity.CategoryID + " - " + activity.CategoryDescription
                                            + " failed to be created, it already exist.\n"; // Manasi
                        }
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
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Exit Point", Logger.logLevel.Debug);

            return register_result;
        }
        public static String updateActivityCategory(ActivityCategory activity)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            MySqlConnection conn = null;
            MySqlDataReader reader = null;
            String update_result = "";
            bool OKForUpdate = false;
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    ActivityCategory retrievedActivityCategory = new ActivityCategory();
                    retrievedActivityCategory = ctx.ActivityCategory.Where(c => c.CategoryID == activity.CategoryID).FirstOrDefault();
                    if (retrievedActivityCategory != null)
                    {
                        CopyUtil.CopyFields<ActivityCategory>(activity, retrievedActivityCategory);
                        using (var dbCtx = new CPPDbContext())
                        {
                            dbCtx.Entry(retrievedActivityCategory).State = System.Data.Entity.EntityState.Modified;
                            dbCtx.SaveChanges();
                            update_result = "Success";
                        }
                    }
                    else
                    {
                        update_result += "Category ID '" + activity.CategoryID + "' does not exist in system";
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
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Exit Point", Logger.logLevel.Debug);

            return update_result;
        }


        public static String updateActivityCategorySubCategory(ActivityCategory activity, Versionmaster latestVersion)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            MySqlConnection conn = null;
            MySqlDataReader reader = null;
            String update_result = "";
            bool OKForUpdate = false;
            try
            {

                using (var ctx = new CPPDbContext())
                {
                    activity.VersionId = latestVersion.Id;
                    ActivityCategory oldVersionActivityCategory = ctx.ActivityCategory.Where(s => s.ID == activity.ID).FirstOrDefault(); // Narayan - getting old version data before modification - 10/06/2022
                    ActivityCategory retrievedActivityCategory = new ActivityCategory();
                    retrievedActivityCategory = ctx.ActivityCategory.Where(s =>
                                                                            //s.ID == activity.ID
                                                                            s.SubCategoryID == oldVersionActivityCategory.SubCategoryID
                                                                            && s.CategoryID == oldVersionActivityCategory.CategoryID
                                                                            && s.Services == oldVersionActivityCategory.Services
                                                                            && s.VersionId == latestVersion.Id
                                                                            ).FirstOrDefault(); // Narayan - old version data compare with same new version data - 10/06/2022



                    ActivityCategory duplicateActivityCategory = new ActivityCategory();
                    duplicateActivityCategory = ctx.ActivityCategory.Where(s => s.SubCategoryID == activity.SubCategoryID
                                                                                && s.SubCategoryDescription == activity.SubCategoryDescription
                                                                                && s.CategoryID == activity.CategoryID
                                                                                && s.CategoryDescription == activity.CategoryDescription
                                                                                //&& s.ID != activity.ID
                                                                                && s.OrganizationID == activity.OrganizationID
                                                                                //&& s.Phase == activity.Phase 
                                                                                && s.Services == activity.Services //Aditya 04062022
                                                                                && s.VersionId == activity.VersionId).FirstOrDefault();

                    if (retrievedActivityCategory != null && (retrievedActivityCategory.CategoryID != activity.CategoryID || retrievedActivityCategory.SubCategoryID != activity.SubCategoryID))
                    {
                        return "Updating the Category ID or Subcategory ID is unavailable at this moment (" + activity.CategoryID + " - " + activity.SubCategoryID + "). \n";
                    }
                    else if (duplicateActivityCategory != null)  //duplicate found
                    {
                        if (duplicateActivityCategory.SubCategoryID == activity.SubCategoryID
                            || duplicateActivityCategory.SubCategoryDescription == activity.SubCategoryDescription)
                        {
                            update_result += "Subcategory ID " + activity.SubCategoryID + " failed to be updated, duplicate entry will be created.\n";
                        }
                    }
                    else if (retrievedActivityCategory != null)     //OK to update
                    {
                        activity.ID = retrievedActivityCategory.ID;
                        CopyUtil.CopyFields<ActivityCategory>(activity, retrievedActivityCategory);

                        ctx.Entry(retrievedActivityCategory).State = System.Data.Entity.EntityState.Modified;
                        ctx.SaveChanges();
                        update_result += "Subcategory ID " + activity.SubCategoryID + " has been updated successfully.\n";
                        activity.SuccessTask++; // Narayan - incriment success task - 14/06/2022
                    }
                    else    //Entry not found
                    {
                        update_result += "Subcategory ID " + activity.SubCategoryID + " failed to be updated, it does not exist.\n";
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
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Exit Point", Logger.logLevel.Debug);

            return update_result;
        }
        public static String deleteActivityCategory(ActivityCategory activity, Versionmaster latestVersion)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String result = "";
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    ActivityCategory oldVersionActivityCategory = ctx.ActivityCategory.Where(s => s.ID == activity.ID).FirstOrDefault(); // Narayan - getting old version data before deletion - 14/06/2022
                    ActivityCategory retrievedActivity = new ActivityCategory();
                    retrievedActivity = ctx.ActivityCategory.Where(s =>
                                                                            //s.ID == activity.ID
                                                                            s.SubCategoryID == oldVersionActivityCategory.SubCategoryID
                                                                            && s.CategoryID == oldVersionActivityCategory.CategoryID
                                                                            && s.Services == oldVersionActivityCategory.Services
                                                                            // && s.Phase == activity.Phase
                                                                            && s.VersionId == latestVersion.Id).FirstOrDefault(); // Narayan - old version data compare with same new version data - 14/06/2022
                    if (retrievedActivity != null)
                    {
                        //Delete
                        ctx.ActivityCategory.Remove(retrievedActivity);
                        ctx.SaveChanges();
                        result += "Subcategory ID " + activity.SubCategoryID + " has been deleted successfully.\n";
                        activity.SuccessTask++; // Narayan - incriment success task - 14/06/2022
                    }
                    else
                    {
                        result += "Subcategory ID " + activity.SubCategoryID + " failed to be deleted, it does not exist.\n";
                    }
                }
            }
            catch (Exception ex)
            {
                result += "Subcategory ID " + activity.SubCategoryID + " failed to be deleted due to dependencies.\n";
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

        public static String getPhaseNameByCode(String phaseCode)
        {
            var ctx = new CPPDbContext();
            var phaseList = ctx.PhaseCode.ToList();
            String result = "";
            int i = 0;
            bool isInt = int.TryParse(phaseCode, out i);

            if (isInt)
            { 
                foreach (var phase in phaseList)
                {
                    //  if (phase.PhaseID * 1000 == Convert.ToInt16(phaseCode))
                    if (phase.PhaseID  == Convert.ToInt16(phaseCode))
                    {
                        result = phase.ActivityPhaseCode;
                    }
                }
            }

            return result;
        }

        internal static List<ActivityCategory> getMainCategoryByProgram(int ProgramID, string Phase)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            List<ActivityCategory> MatchedActivityCategoryList = new List<ActivityCategory>();
            string activityPhase = "";
            activityPhase = getPhaseNameByCode(Phase);
            try
            {
              

                using (var ctx = new CPPDbContext())
                {
                    List<ProgramCategory> programCategoryList = ctx.ProgramCategory.Where(p => p.ProgramID == ProgramID).ToList();
                    Program program = ctx.Program.Where(a => a.ProgramID == ProgramID).FirstOrDefault();
                    int orgId = int.Parse(program.OrganizationID);
                    foreach (var cat in programCategoryList)
                    {
                        ActivityCategory actCat = ctx.ActivityCategory.Where(a => a.ID == cat.ActivityCategoryID && (a.Phase == activityPhase || a.Phase=="All") && a.OrganizationID == orgId).FirstOrDefault();
                        if (actCat != null)
                            MatchedActivityCategoryList.Add(actCat);

                        MatchedActivityCategoryList.OrderBy(a => a.CategoryID).Distinct().ToList() ;
                    }
                    //if (activityPhase == "All")
                    //{

                    //    MatchedActivityCategoryList = ctx.ActivityCategory.OrderBy(a => a.CategoryID).Distinct().ToList();
                    //    MatchedActivityCategoryList = MatchedActivityCategoryList.GroupBy(a => a.CategoryID).Select(a => a.First()).ToList();
                    //}
                    //else
                    //{
                    //    MatchedActivityCategoryList = ctx.ActivityCategory.Where(p => p.Phase == activityPhase || p.Phase == "All")
                    //                                    .OrderBy(a => a.CategoryID).Distinct().ToList();
                    //    MatchedActivityCategoryList = MatchedActivityCategoryList.GroupBy(a => a.CategoryID).Select(a => a.First()).ToList();
                    //}
                }
                //// create and open a connection object
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
            MatchedActivityCategoryList = MatchedActivityCategoryList.GroupBy(a => a.CategoryID).Select(a => a.First()).ToList(); ;
            return MatchedActivityCategoryList;
        }

        internal static List<ActivityCategory> getSubCategoryByProgram(int ProgramID, string Phase, string CategoryID)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            List<ActivityCategory> MatchedActivityCategoryList = new List<ActivityCategory>();
            List<ActivityCategory> MatchedSubActivityCategoryList = new List<ActivityCategory>();
            string activityPhase = "";
            activityPhase = getPhaseNameByCode(Phase);
            try
            {


                using (var ctx = new CPPDbContext())
                {
                    List<ProgramCategory> programCategoryList = ctx.ProgramCategory.Where(p => p.ProgramID == ProgramID).ToList();
                    Program program = ctx.Program.Where(a => a.ProgramID == ProgramID).FirstOrDefault();
                    int orgId = int.Parse(program.OrganizationID);
                    foreach (var cat in programCategoryList)
                    {
                        var catId = cat.ActivityCategoryID;
                        ActivityCategory actCat = ctx.ActivityCategory.Where(a => a.ID == catId && a.CategoryID == CategoryID && (a.Phase == activityPhase || a.Phase == "All") && a.OrganizationID == orgId).FirstOrDefault();
                        if (actCat != null)
                            MatchedActivityCategoryList.Add(actCat);
                    }

                }
                //// create and open a connection object
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
           // MatchedActivityCategoryList = MatchedActivityCategoryList.GroupBy(a => a.CategoryID).Select(a => a.First()).ToList(); ;
            return MatchedActivityCategoryList;
        }
    }
}