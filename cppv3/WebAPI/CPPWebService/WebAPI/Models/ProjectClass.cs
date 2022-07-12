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
    [Table("project_class")]
    public class ProjectClass : Audit
    {
        [NotMapped]
        public int Operation;

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProjectClassID { get; set; }
        public String ProjectClassName { get; set; }
        public String ProjectClassDescription { get; set; }
        public String ProjectClassLineItemID { get; set; }
        //public DateTime CreatedDate { get; set; }
        //public DateTime UpdatedDate { get; set; }
        //public String CreatedBy { get; set; }
        //public String UpdatedBy { get; set; }

        public static List<ProjectClass> getProjectClass()
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);

            List<ProjectClass> projectClassList = new List<ProjectClass>();
            try
            {

                using (var ctx = new CPPDbContext())
                {
                    projectClassList = ctx.ProjectClass.OrderBy(a => a.ProjectClassLineItemID).ToList();
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

            return projectClassList;
        }

        public static ProjectClass getProjectClassById(int projectClassID)
        {
            ProjectClass projectClass = null;
            try
            {

                using (var ctx = new CPPDbContext())
                {
                    projectClass = ctx.ProjectClass.Where(x => x.ProjectClassID == projectClassID).FirstOrDefault();
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

            return projectClass;
        }
        public static List<ProjectClass> getProjectClassByProgramId(int programID)
        {
            List<ProjectClass> projectClasses = new List<ProjectClass>();
            try
            {

                using (var ctx = new CPPDbContext())
                {
                    var pojectClassIDs = ctx.ProgramElement.Where(x => x.ProgramID == programID).Select(x => x.ProjectClassID).ToList();
                    projectClasses = ctx.ProjectClass.Where(x => pojectClassIDs.Contains(x.ProjectClassID)).ToList();
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

            return projectClasses;
        }
        public static List<ProjectClass> getProjectClassByProgramElemId(int programElemID)
        {
            List<ProjectClass> projectClasses = new List<ProjectClass>();
            try
            {

                using (var ctx = new CPPDbContext())
                {
                    var pojectClassIDs = ctx.ProgramElement.Where(x => x.ProgramElementID == programElemID).Select(x => x.ProjectClassID).ToList();
                    projectClasses = ctx.ProjectClass.Where(x => pojectClassIDs.Contains(x.ProjectClassID)).ToList();
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

            return projectClasses;
        }

        public static String registerProjectClass(ProjectClass projectClass)
        {

            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String result = "";
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    ProjectClass retreivedProjectClass = new ProjectClass();
                    retreivedProjectClass = ctx.ProjectClass.Where(u => u.ProjectClassName == projectClass.ProjectClassName
                                                                        || u.ProjectClassLineItemID == projectClass.ProjectClassLineItemID).FirstOrDefault();

                    if (retreivedProjectClass == null)
                    {
                        //register
                        ctx.ProjectClass.Add(projectClass);
                        ctx.SaveChanges();
                        result += projectClass.ProjectClassName + " has been created successfully.\n";
                    }
                    else
                    {
                        result += projectClass.ProjectClassName + "' failed to be created, duplicate division or line item # is not allowed.\n";
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
        public static String updateProjectClass(ProjectClass projectClass)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String result = "";

            try
            {
                using (var ctx = new CPPDbContext())
                {
                    ProjectClass retreivedProjectClass = new ProjectClass();
                    retreivedProjectClass = ctx.ProjectClass.Where(u => u.ProjectClassID == projectClass.ProjectClassID).FirstOrDefault();

                    ProjectClass duplicateProjectClass = ctx.ProjectClass.Where(a => (a.ProjectClassID != projectClass.ProjectClassID
                                                                                  && (a.ProjectClassLineItemID == projectClass.ProjectClassLineItemID
                                                                                  || a.ProjectClassName == projectClass.ProjectClassName))).FirstOrDefault();

                    if (retreivedProjectClass != null && retreivedProjectClass.ProjectClassLineItemID != projectClass.ProjectClassLineItemID)
                    {
                        return "Updating the project division code is unavailable at this moment (" + projectClass.ProjectClassName + "). \n";
                    }
                    else if (duplicateProjectClass != null)
                    {
                        result += projectClass.ProjectClassName + " failed to be updated, duplicate of division or line item # will be created.\n";
                    }
                    else if (retreivedProjectClass != null)
                    {
                        CopyUtil.CopyFields<ProjectClass>(projectClass, retreivedProjectClass);
                        ctx.Entry(retreivedProjectClass).State = System.Data.Entity.EntityState.Modified;
                        ctx.SaveChanges();
                        result += projectClass.ProjectClassName + " has been updated successfully.\n";
                    }
                    else
                    {
                        result += projectClass.ProjectClassName + " failed to be updated, it does not exist.\n";
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
        public static String deleteProjectClass(ProjectClass projectClass)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String result = "";
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    ProjectClass retreivedProjectClass = new ProjectClass();
                    retreivedProjectClass = ctx.ProjectClass.Where(u => u.ProjectClassID == projectClass.ProjectClassID).FirstOrDefault();

                    if (retreivedProjectClass != null)
                    {
                        ctx.ProjectClass.Remove(retreivedProjectClass);
                        ctx.SaveChanges();
                        result = projectClass.ProjectClassName + " has been deleted successfully.\n";
                    }
                    else
                    {
                        result = projectClass.ProjectClassName + " falied to be deleted, it does not exist.\n";
                    }
                }

            }
            catch (Exception ex)
            {
                result += projectClass.ProjectClassName + " failed to be deleted due to dependencies.\n";
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