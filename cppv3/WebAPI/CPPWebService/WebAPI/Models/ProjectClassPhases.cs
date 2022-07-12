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
    [Table("project_class_phase")]
    public class ProjectClassPhase : Audit
    {
        [NotMapped]
        public int Operation;

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int ProjectClassID { get; set; }
        public int PhaseID { get; set; }
        public int Order { get; set; }
        //public DateTime CreatedDate { get; set; }
        //public DateTime UpdatedDate { get; set; }
        //public String CreatedBy { get; set; }
        //public String UpdatedBy { get; set; }

        [ForeignKey("ProjectClassID")]
        public virtual ProjectClass ProjectClass { get; set; }

        [ForeignKey("PhaseID")]
        public virtual PhaseCode PhaseCode { get; set; }

        public static List<ProjectClassPhase> getProjectClassPhase()
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);

            List<ProjectClassPhase> projectClassPhaseList = new List<ProjectClassPhase>();
            try
            {

                using (var ctx = new CPPDbContext())
                {
                    projectClassPhaseList = ctx.ProjectClassPhase.OrderBy(a => a.ProjectClassID).ThenBy(b => b.Order).ToList();
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

            return projectClassPhaseList;
        }


        public static String registerProjectClassPhaseList(ProjectClassPhase projectClassPhase)
        {

            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String result = "";
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    ProjectClassPhase retreivedProjectClassPhase = new ProjectClassPhase();
                    retreivedProjectClassPhase = ctx.ProjectClassPhase.Where(u => (u.ProjectClassID == projectClassPhase.ProjectClassID
                                                                                  && u.PhaseID == projectClassPhase.PhaseID)
                                                                                  || (u.Order == projectClassPhase.Order && u.ProjectClassID == projectClassPhase.ProjectClassID)).FirstOrDefault();

                    if (retreivedProjectClassPhase == null)
                    {
                        //register
                        ctx.ProjectClassPhase.Add(projectClassPhase);
                        ctx.SaveChanges();

                        ProjectClass projectClass = ctx.ProjectClass.Where(p => p.ProjectClassID == projectClassPhase.ProjectClassID).FirstOrDefault();
                        PhaseCode phaseCode = ctx.PhaseCode.Where(p => p.PhaseID == projectClassPhase.PhaseID).FirstOrDefault();

                        result += projectClass.ProjectClassName + " - " + phaseCode.PhaseDescription + " has been created successfully. \n";
                    }
                    else
                    {
                        ProjectClass projectClass = ctx.ProjectClass.Where(p => p.ProjectClassID == projectClassPhase.ProjectClassID).FirstOrDefault();
                        PhaseCode phaseCode = ctx.PhaseCode.Where(p => p.PhaseID == projectClassPhase.PhaseID).FirstOrDefault();

                        result += projectClass.ProjectClassName + " - " + phaseCode.PhaseDescription + " failed to be created, entry must be unique. \n";
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
        public static String updateProjectClassPhase(ProjectClassPhase projectClassPhase)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String result = "";



            try
            {
                using (var ctx = new CPPDbContext())
                {
                    ProjectClassPhase retreivedProjectClassPhase = new ProjectClassPhase();
                    retreivedProjectClassPhase = ctx.ProjectClassPhase.Where(u => u.ID == projectClassPhase.ID).FirstOrDefault();
                    ProjectClass pc = new ProjectClass();

                    ProjectClassPhase duplicateProjectClassPhase = ctx.ProjectClassPhase.Where(a => (a.ProjectClassID == projectClassPhase.ProjectClassID
                                                                                  && a.PhaseID == projectClassPhase.PhaseID 
                                                                                  && a.ID != projectClassPhase.ID)
                                                                                  //|| (a.ProjectClassID == projectClassPhase.ProjectClassID
                                                                                  && (a.ProjectClassID == projectClassPhase.ProjectClassID    //Manasi 14-07-2020
                                                                                  && a.Order == projectClassPhase.Order
                                                                                  && a.ID != projectClassPhase.ID)).FirstOrDefault();

                    ProjectClass projectClass = ctx.ProjectClass.Where(p => p.ProjectClassID == projectClassPhase.ProjectClassID).FirstOrDefault();
                    PhaseCode phaseCode = ctx.PhaseCode.Where(p => p.PhaseID == projectClassPhase.PhaseID).FirstOrDefault();


                    if (duplicateProjectClassPhase != null)
                    {
                        result += projectClass.ProjectClassName + " - " + phaseCode.PhaseDescription + " failed to be updated, non-unique entry will be created.\n";
                    }
                    else if (retreivedProjectClassPhase != null)
                    {
                        pc = retreivedProjectClassPhase.ProjectClass;
                        //CopyUtil.CopyFields<ProjectClassPhase>(projectClassPhase, retreivedProjectClassPhase);
                        //retreivedProjectClassPhase.ProjectClass = pc;
                        retreivedProjectClassPhase.Order = projectClassPhase.Order;
                        //retreivedProjectClassPhase.PhaseCode = projectClassPhase.PhaseCode;
                        retreivedProjectClassPhase.PhaseID = projectClassPhase.PhaseID;
                        retreivedProjectClassPhase.UpdatedBy = projectClassPhase.UpdatedBy;
                        retreivedProjectClassPhase.UpdatedDate = projectClassPhase.UpdatedDate;
                        ctx.Entry(retreivedProjectClassPhase).State = System.Data.Entity.EntityState.Modified;
                        ctx.SaveChanges();

                        result += projectClass.ProjectClassName + " - " + phaseCode.PhaseDescription + " has been updated successfully.\n";
                    }
                    else
                    {
                        result += projectClass.ProjectClassName + " - " + phaseCode.PhaseDescription + " failed to be updated, it does not exist.\n";
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
        public static String deleteProjectClassPhase(ProjectClassPhase projectClassPhase)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String result = "";
            String entryName = "";
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    ProjectClassPhase retreivedProjectClassPhase = new ProjectClassPhase();
                    retreivedProjectClassPhase = ctx.ProjectClassPhase.Where(u => u.ID == projectClassPhase.ID).FirstOrDefault();

                    ProjectClass projectClass = ctx.ProjectClass.Where(p => p.ProjectClassID == projectClassPhase.ProjectClassID).FirstOrDefault();
                    PhaseCode phaseCode = ctx.PhaseCode.Where(p => p.PhaseID == projectClassPhase.PhaseID).FirstOrDefault();

                    entryName = projectClass.ProjectClassName + " - " + phaseCode.Code;

                    if (retreivedProjectClassPhase != null)
                    {
                        ctx.ProjectClassPhase.Remove(retreivedProjectClassPhase);
                        ctx.SaveChanges();
                        result = entryName + " has been deleted successfully.\n";
                    }
                    else
                    {
                        result = entryName + " failed to be updated, it does not exist.\n";
                    }
                }

            }
            catch (Exception ex)
            {
                result = entryName + " failed to be deleted due to dependencies.\n";
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