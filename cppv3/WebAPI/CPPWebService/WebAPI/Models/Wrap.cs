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
    [Table("wrap")]
    public class Wrap : Audit
    {
        [NotMapped]
        public int Operation;

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public String Description { get; set; }

        public static List<Wrap> getWrap()
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);

            List<Wrap> wrapList = new List<Wrap>();
            try
            {

                using (var ctx = new CPPDbContext())
                {
                    wrapList = ctx.Wrap.OrderBy(a => a.ID).ToList();
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

            return wrapList;
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

        public static String registerWrap(Wrap wrap)
        {

            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String result = "";
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    Wrap retreivedWrap = new Wrap();
                    retreivedWrap = ctx.Wrap.Where(u => u.Description == wrap.Description).FirstOrDefault();


                    if (retreivedWrap == null)
                    {
                        //register
                        ctx.Wrap.Add(wrap);
                        ctx.SaveChanges();
                        result += wrap.Description + " has been created successfully.\n";
                    }
                    else
                    {
                        result += wrap.Description + "' failed to be created, duplicate wrap is not allowed.\n";
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
        public static String updateWrap(Wrap wrap)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String result = "";

            try
            {
                using (var ctx = new CPPDbContext())
                {
                    Wrap retreivedWrap = new Wrap();
                    retreivedWrap = ctx.Wrap.Where(u => u.ID == wrap.ID).FirstOrDefault();

                    Wrap duplicateWrap = ctx.Wrap.Where(a => (a.ID != wrap.ID
                                                             && a.Description == wrap.Description)).FirstOrDefault();


                    //if (retreivedCertifiedPayroll != null && retreivedCertifiedPayroll.ID != certifiedpayroll.ID)
                    //{
                    //    return "Updating the project division code is unavailable at this moment (" + certifiedpayroll.Description + "). \n";
                    //}
                    if (duplicateWrap != null)
                    {
                        result += wrap.Description + " failed to be updated, duplicate of wrap is not allowed.\n";
                    }
                    else if (retreivedWrap != null)
                    {
                        CopyUtil.CopyFields<Wrap>(wrap, retreivedWrap);
                        ctx.Entry(retreivedWrap).State = System.Data.Entity.EntityState.Modified;
                        ctx.SaveChanges();
                        result += wrap.Description + " has been updated successfully.\n";
                    }
                    else
                    {
                        result += wrap.Description + " failed to be updated, it does not exist.\n";
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
        public static String deleteWrap(Wrap wrap)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String result = "";
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    Wrap retreivedWrap = new Wrap();
                    retreivedWrap = ctx.Wrap.Where(u => u.ID == wrap.ID).FirstOrDefault();

                    if (retreivedWrap != null)
                    {
                        ctx.Wrap.Remove(retreivedWrap);
                        ctx.SaveChanges();
                        result = wrap.Description + " has been deleted successfully.\n";
                    }
                    else
                    {
                        result = wrap.Description + " failed to be deleted, it does not exist.\n";
                    }
                }

            }
            catch (Exception ex)
            {
                //result += serviceclass.Description + " failed to be deleted due to dependencies.\n";
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