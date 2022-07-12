using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Web;
using WebAPI.Helper;

namespace WebAPI.Models
{
    [Table ("project_type")]
    public class ProjectType : Audit
    {
        [NotMapped]
        public int Operation;

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProjectTypeID { get; set; }
        public String ProjectTypeName { get; set; }
        public String ProjectTypeDescription { get; set; }
        public String ProjectTypeLineItemID { get; set; }
        //public DateTime CreatedDate { get; set; }
        //public DateTime UpdatedDate { get; set; }
        //public String CreatedBy { get; set; }
        //public String UpdatedBy { get; set; }

        public static List<ProjectType> getProjectType()
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);

            List<ProjectType> projectTypeList = new List<ProjectType>();
            try
            {

                using (var ctx = new CPPDbContext())
                {
                    projectTypeList = ctx.ProjectType.OrderBy(a => a.ProjectTypeName).ToList();
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

            return projectTypeList;
        }


        public static String registerProjectType(ProjectType projectType)
        {

            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String result = "";
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    ProjectType retreivedProjectType = new ProjectType();
                    retreivedProjectType = ctx.ProjectType.Where(u => u.ProjectTypeName == projectType.ProjectTypeName).FirstOrDefault();

                    if (retreivedProjectType == null)
                    {
                        //register
                        ctx.ProjectType.Add(projectType);
                        ctx.SaveChanges();
                        result = projectType.ProjectTypeName + " has been created successfully.\n";
                    }
                    else
                    {
                        result += projectType.ProjectTypeName + "' failed to be created, it already exist.\n";
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
        public static String updateProjectType(ProjectType projectType)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String result = "";

            try
            {
                using (var ctx = new CPPDbContext())
                {
                    ProjectType retreivedProjectType = new ProjectType();
                    retreivedProjectType = ctx.ProjectType.Where(u => u.ProjectTypeID == projectType.ProjectTypeID).FirstOrDefault();

                    ProjectType duplicateProjectType = new ProjectType();
                    duplicateProjectType = ctx.ProjectType.Where(u => u.ProjectTypeName == projectType.ProjectTypeName
                                                                        && u.ProjectTypeID != projectType.ProjectTypeID).FirstOrDefault();

                    if (duplicateProjectType != null)
                    {
                        result = projectType.ProjectTypeName + " failed to be updated, duplicate will be created.\n";
                    }
                    else if (retreivedProjectType != null)
                    {
                        CopyUtil.CopyFields<ProjectType>(projectType, retreivedProjectType);
                        ctx.Entry(retreivedProjectType).State = System.Data.Entity.EntityState.Modified;
                        ctx.SaveChanges();
                        result = projectType.ProjectTypeName + " has been updated successfully.\n";
                    }
                    else
                    {
                        result += projectType.ProjectTypeName + " failed to be updated, it does not exist.\n";
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
        public static String deleteProjectType(ProjectType projectType)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String result = "";
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    ProjectType retreivedProjectType = new ProjectType();
                    retreivedProjectType = ctx.ProjectType.Where(u => u.ProjectTypeID == projectType.ProjectTypeID).FirstOrDefault();
                    if (retreivedProjectType != null)
                    {
                        ctx.ProjectType.Remove(retreivedProjectType);
                        ctx.SaveChanges();
                        result = projectType.ProjectTypeName + " has been deleted successfully.\n";
                    }
                    else
                    {
                        result = projectType.ProjectTypeName + " failed to be deleted, it does not exist.\n";
                    }
                }

            }
            catch (Exception ex)
            {
                result += projectType.ProjectTypeName + " failed to be deleted due to dependencies.\n";
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