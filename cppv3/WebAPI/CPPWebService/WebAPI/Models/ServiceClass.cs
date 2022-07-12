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
    [Table("services")]
    public class ServiceClass : Audit
    {
        [NotMapped]
        public int Operation;

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public String Code { get; set; }
        public String Description { get; set; }
        public int VersionId { get; set; }

        //public DateTime CreatedDate { get; set; }
        //public DateTime UpdatedDate { get; set; }
        //public String CreatedBy { get; set; }
        //public String UpdatedBy { get; set; }

        public static List<ServiceClass> getServices(string ProjectID)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);

            List<ServiceClass> serviceClassList = new List<ServiceClass>();
            try
            {

                using (var ctx = new CPPDbContext())
                {
                   
                    Versionmaster latestVersion;
                    
                    string versionId;
                    if (ProjectID != "null")
                    {
                        int projectid = Int32.Parse(ProjectID);
                        versionId = ctx.Project.Where(a => a.ProjectID == projectid).Select(a => a.VersionId).FirstOrDefault();
                        int version = Int32.Parse(versionId);
                        serviceClassList = ctx.ServiceClass.Where(a => a.VersionId == version).OrderBy(a => a.ID).ToList();
                    }
                    else
                    {
                        latestVersion = ctx.VersionMaster.OrderByDescending(a => a.CreatedDate).FirstOrDefault();
                        serviceClassList = ctx.ServiceClass.Where(a => a.VersionId == latestVersion.Id).OrderBy(a => a.ID).ToList();
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

            return serviceClassList;
        }

        public static ServiceClass getServiceById(int serviceId)
        {
            ServiceClass serviceClass = null;
            try
            {

                using (var ctx = new CPPDbContext())
                {
                    serviceClass = ctx.ServiceClass.Where(x => x.ID == serviceId).FirstOrDefault();
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

            return serviceClass;
        }

        public static String registerServiceClass(ServiceClass serviceclass)
        {

            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String result = "";
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    ServiceClass retreivedServiceClass = new ServiceClass();
                    retreivedServiceClass = ctx.ServiceClass.Where(u => u.Description == serviceclass.Description
                                                                        || u.ID == serviceclass.ID || u.Code==serviceclass.Code).FirstOrDefault();
                    Versionmaster latestVersion = ctx.VersionMaster.OrderByDescending(a => a.CreatedDate).FirstOrDefault();
                    serviceclass.VersionId = latestVersion.Id;

                    if (retreivedServiceClass == null)
                    {
                        //register
                        ctx.ServiceClass.Add(serviceclass);
                        ctx.SaveChanges();
                        result += serviceclass.Description + " has been created successfully.\n";
                    }
                    else
                    {
                        result += serviceclass.Description + "' failed to be created, duplicate Service is not allowed.\n";
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
        public static String updateServiceClass(ServiceClass serviceclass)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String result = "";

            try
            {
                using (var ctx = new CPPDbContext())
                {
                    ServiceClass retreivedServiceClass = new ServiceClass();
                    retreivedServiceClass = ctx.ServiceClass.Where(u => u.ID == serviceclass.ID).FirstOrDefault();

                    ServiceClass duplicateserviceClass = ctx.ServiceClass.Where(a => (a.ID != serviceclass.ID
                                                                                  && (a.Description == serviceclass.Description))
                                                                                  || (a.ID != serviceclass.ID
                                                                                  && (a.Code == serviceclass.Code))).FirstOrDefault();
                    Versionmaster latestVersion = ctx.VersionMaster.OrderByDescending(a => a.CreatedDate).FirstOrDefault();
                    serviceclass.VersionId = latestVersion.Id;
                    if (retreivedServiceClass != null && retreivedServiceClass.ID!=serviceclass.ID)
                    {
                        return "Updating the project division code is unavailable at this moment (" + serviceclass.Description + "). \n";
                    }
                    else if (duplicateserviceClass != null)
                    {
                        result += serviceclass.Description + " failed to be updated, duplicate of services is not allowed.\n";
                    }
                    else if (retreivedServiceClass != null)
                    {
                        CopyUtil.CopyFields<ServiceClass>(serviceclass, retreivedServiceClass);
                        ctx.Entry(retreivedServiceClass).State = System.Data.Entity.EntityState.Modified;
                        ctx.SaveChanges();
                        result += serviceclass.Description + " has been updated successfully.\n";
                    }
                    else
                    {
                        result += serviceclass.Description + " failed to be updated, it does not exist.\n";
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
        public static String deleteServiceClass(ServiceClass serviceclass)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String result = "";
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    ServiceClass retreivedServiceClass = new ServiceClass();
                    retreivedServiceClass = ctx.ServiceClass.Where(u => u.ID == serviceclass.ID).FirstOrDefault();

                    if (retreivedServiceClass != null)
                    {
                        ctx.ServiceClass.Remove(retreivedServiceClass);
                        ctx.SaveChanges();
                        result = serviceclass.Description + " has been deleted successfully.\n";
                    }
                    else
                    {
                        result = serviceclass.Description + " falied to be deleted, it does not exist.\n";
                    }
                }

            }
            catch (Exception ex)
            {
                result += serviceclass.Description + " failed to be deleted due to dependencies.\n";
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