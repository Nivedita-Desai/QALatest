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
    public class ServiceToSubserviceMapping : Audit
    {
        [NotMapped]
        public int Operation;

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int ProjectClassID { get; set; }
        public int PhaseID { get; set; }
        public int Order { get; set; }
        public int VersionId { get; set; }
        //public DateTime CreatedDate { get; set; }
        //public DateTime UpdatedDate { get; set; }
        //public String CreatedBy { get; set; }
        //public String UpdatedBy { get; set; }

        [ForeignKey("ProjectClassID")]
        public virtual ProjectClass ProjectClass { get; set; }

        [ForeignKey("ProjectClassID")]
        public virtual ServiceClass ServiceClass { get; set; }

        [ForeignKey("PhaseID")]
        public virtual PhaseCode PhaseCode { get; set; }

        public static List<ServiceToSubserviceMapping> getSubServices()
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);

            List<ServiceToSubserviceMapping> serviceToSubserviceMapping = new List<ServiceToSubserviceMapping>();
            try
            {

                using (var ctx = new CPPDbContext())
                {
                    Versionmaster latestVersion = ctx.VersionMaster.OrderByDescending(a => a.CreatedDate).FirstOrDefault();
                    serviceToSubserviceMapping = ctx.ServiceToSubserviceMapping.Where(v=>v.VersionId == latestVersion.Id).OrderBy(a => a.ProjectClassID).ThenBy(b => b.Order).ToList();
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

            return serviceToSubserviceMapping;
        }


        public static String registerServiceToSubserviceMappingList(ServiceToSubserviceMapping serviceToSubservices)
        {

            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String result = "";
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    ServiceToSubserviceMapping retreivedProjectClassPhase = new ServiceToSubserviceMapping();
                    retreivedProjectClassPhase = ctx.ServiceToSubserviceMapping.Where(u => (u.ProjectClassID == serviceToSubservices.ProjectClassID
                                                                                  && u.PhaseID == serviceToSubservices.PhaseID)
                                                                                  || (u.Order == serviceToSubservices.Order && u.ProjectClassID == serviceToSubservices.ProjectClassID)).FirstOrDefault();
                    Versionmaster latestVersion = ctx.VersionMaster.OrderByDescending(a => a.CreatedDate).FirstOrDefault();
                    serviceToSubservices.VersionId = latestVersion.Id;
                    if (retreivedProjectClassPhase == null)
                    {
                        //register
                        ctx.ServiceToSubserviceMapping.Add(serviceToSubservices);
                        ctx.SaveChanges();

                        ServiceClass serviceClass = ctx.ServiceClass.Where(p => p.ID == serviceToSubservices.ProjectClassID).FirstOrDefault();
                        PhaseCode phaseCode = ctx.PhaseCode.Where(p => p.PhaseID == serviceToSubservices.PhaseID).FirstOrDefault();

                        result += serviceClass.Description + " - " + phaseCode.PhaseDescription + " has been created successfully. \n";
                    }
                    else
                    {
                        ServiceClass serviceClass = ctx.ServiceClass.Where(p => p.ID == serviceToSubservices.ProjectClassID).FirstOrDefault();
                        PhaseCode phaseCode = ctx.PhaseCode.Where(p => p.PhaseID == serviceToSubservices.PhaseID).FirstOrDefault();

                        result += serviceClass.Description + " - " + phaseCode.PhaseDescription + " failed to be created, entry must be unique. \n";
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
        public static String updateServiceToSubserviceMapping(ServiceToSubserviceMapping serviceToSubservices)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String result = "";



            try
            {
                using (var ctx = new CPPDbContext())
                {
                    ServiceToSubserviceMapping retreivedProjectClassPhase = new ServiceToSubserviceMapping();
                    retreivedProjectClassPhase = ctx.ServiceToSubserviceMapping.Where(u => u.ID == serviceToSubservices.ID).FirstOrDefault();
                    ServiceClass sc = new ServiceClass();

                    //ServiceToSubserviceMapping duplicateProjectClassPhase = ctx.ServiceToSubserviceMapping.Where(a => (a.ProjectClassID == serviceToSubservices.ProjectClassID
                    //                                                              && a.PhaseID == serviceToSubservices.PhaseID
                    //                                                              && a.ID != serviceToSubservices.ID)
                    //                                                              //|| (a.ProjectClassID == projectClassPhase.ProjectClassID
                    //                                                              && (a.ProjectClassID == serviceToSubservices.ProjectClassID    //Manasi 14-07-2020
                    //                                                              && a.Order == serviceToSubservices.Order
                    //                                                              && a.ID != serviceToSubservices.ID)).FirstOrDefault();

                    ServiceToSubserviceMapping duplicateProjectClassPhase = ctx.ServiceToSubserviceMapping.Where(a => (a.ProjectClassID == serviceToSubservices.ProjectClassID
                                                                                  && a.PhaseID == serviceToSubservices.PhaseID && a.ID != serviceToSubservices.ID)
                                                                                  ||(a.ProjectClassID == serviceToSubservices.ProjectClassID    //vaishnavi
                                                                                  && a.Order == serviceToSubservices.Order && a.ID != serviceToSubservices.ID)).FirstOrDefault();

                    ServiceClass serviceClass = ctx.ServiceClass.Where(p => p.ID == serviceToSubservices.ProjectClassID).FirstOrDefault();
                    PhaseCode phaseCode = ctx.PhaseCode.Where(p => p.PhaseID == serviceToSubservices.PhaseID).FirstOrDefault();


                    if (duplicateProjectClassPhase != null)
                    {
                        result += serviceClass.Description + " - " + phaseCode.PhaseDescription + " failed to be updated,entry must be unique.\n";
                    }
                    else if (retreivedProjectClassPhase != null)
                    {
                        sc = retreivedProjectClassPhase.ServiceClass;
                        //CopyUtil.CopyFields<ProjectClassPhase>(projectClassPhase, retreivedProjectClassPhase);
                        //retreivedProjectClassPhase.ProjectClass = pc;
                        retreivedProjectClassPhase.Order = serviceToSubservices.Order;
                        //retreivedProjectClassPhase.PhaseCode = projectClassPhase.PhaseCode;
                        retreivedProjectClassPhase.PhaseID = serviceToSubservices.PhaseID;
                        retreivedProjectClassPhase.UpdatedBy = serviceToSubservices.UpdatedBy;
                        retreivedProjectClassPhase.UpdatedDate = serviceToSubservices.UpdatedDate;
                        ctx.Entry(retreivedProjectClassPhase).State = System.Data.Entity.EntityState.Modified;
                        ctx.SaveChanges();

                        result += serviceClass.Description + " - " + phaseCode.PhaseDescription + " has been updated successfully.\n";
                    }
                    else
                    {
                        result += serviceClass.Description + " - " + phaseCode.PhaseDescription + " failed to be updated, it does not exist.\n";
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
        public static String deleteServiceToSubserviceMapping(ServiceToSubserviceMapping serviceToSubservices)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String result = "";
            String entryName = "";
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    ServiceToSubserviceMapping retreivedProjectClassPhase = new ServiceToSubserviceMapping();
                    retreivedProjectClassPhase = ctx.ServiceToSubserviceMapping.Where(u => u.ID == serviceToSubservices.ID).FirstOrDefault();

                    ServiceClass serviceClass = ctx.ServiceClass.Where(p => p.ID == serviceToSubservices.ProjectClassID).FirstOrDefault();
                    PhaseCode phaseCode = ctx.PhaseCode.Where(p => p.PhaseID == serviceToSubservices.PhaseID).FirstOrDefault();

                    entryName = serviceClass.Description + " - " + phaseCode.Code;

                    if (retreivedProjectClassPhase != null)
                    {
                        ctx.ServiceToSubserviceMapping.Remove(retreivedProjectClassPhase);
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