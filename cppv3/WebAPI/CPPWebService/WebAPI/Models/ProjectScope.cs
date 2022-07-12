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

namespace WebAPI.Models
{
    [Table("project_scope")]
    public class ProjectScope
    {
        [NotMapped]
        public int Operation;
       // public bool isNew;
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int ProjectID { get; set; }
        public String Area { get; set; }
        public String Description { get; set; }
        public String ImpactType { get; set; }
        public String ImpactDescription { get; set; }
        public String Asset { get; set; }

        //Nivedita 10022022
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string DeletedBy { get; set; }
        //[ForeignKey("ProjectID")]
        //public virtual Project project { get; set; }

        public static List<ProjectScope> getProjectScope(int projectID)
        {
           List<ProjectScope> MatchedProjectScopeList = new List<ProjectScope>();

           using (var ctx = new CPPDbContext())
           {
               try
               {
                   MatchedProjectScopeList = ctx.ProjectScope.Where(p => p.ProjectID == projectID).ToList();
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
           }
          
            return MatchedProjectScopeList;
        }

        public static String registerProjectScope(ProjectScope projectScope)
        {
            String status = "";
            using (var ctx = new CPPDbContext())
            {

                try
                {
                    ctx.ProjectScope.Add(projectScope);
                    ctx.SaveChanges();
                    status = "Success";
                }
                catch (Exception ex)
                {
                    status = ex.InnerException.ToString();
                }
                finally
                {

                }
            }

            return status;
        }
        public static String deleteProjectScope(ProjectScope projectScope)
        {
 
            String delete_result = "";
            bool OkForDelete = false;
            using (var ctx = new CPPDbContext())
            {
                try
                {
                    // query to check existence of object in database
                    ProjectScope ps = ctx.ProjectScope.Where(p => p.Id == projectScope.Id).FirstOrDefault();
                    if (ps != null)
                    {
                        OkForDelete = true;
                    }
                    else
                    {
                            delete_result += "Area " + projectScope.Area + " for ProjectID " + projectScope.ProjectID + " does not exist in system";

                    }
                
                 
                    // delete from database
                    if (OkForDelete)
                    {
                        ctx.ProjectScope.Remove(ps);
                        ctx.SaveChanges();

                        delete_result = "Success";
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
            }
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Exit Point", Logger.logLevel.Debug);

            return delete_result;
        }
        public static String updateProjectScope(ProjectScope projectScope)
        {
         
            String update_result = "";
            bool OkForUpdate = false;
            using (var ctx = new CPPDbContext())
            {
                try
                {
                    // query to check existence of object in database

                    ProjectScope ps = ctx.ProjectScope.Where(p => p.Id == projectScope.Id).FirstOrDefault();
                    // check the database
                     if (ps != null)
                    {
                        OkForUpdate = true;
                    }
                    else
                    {
                        update_result += "Area " + projectScope.Area + " for ProjectId " + projectScope.ProjectID + " does not exist in system";

                    }

                    // update database entry
                    if (OkForUpdate)
                    {
                        ps.Area = projectScope.Area;
                        ps.Description = projectScope.Description;
                        ps.ImpactDescription = projectScope.Description;
                        ps.ImpactType = projectScope.ImpactType;
                        ps.Asset = projectScope.Asset;
                        ctx.SaveChanges();
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

                }
            }
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Exit Point", Logger.logLevel.Debug);
            
            return update_result;
        }
    }

}