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
    [Table("certified_payroll")]
    public class CertifiedPayroll : Audit
    {
        [NotMapped]
        public int Operation;

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public String Description { get; set; }

        public static List<CertifiedPayroll> getCertifiedPayroll()
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);

            List<CertifiedPayroll> certifiedpayrollList = new List<CertifiedPayroll>();
            try
            {

                using (var ctx = new CPPDbContext())
                {
                    certifiedpayrollList = ctx.CertifiedPayroll.OrderBy(a => a.ID).ToList();
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

            return certifiedpayrollList;
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

        public static String registerCertifiedPayroll(CertifiedPayroll certifiedpayroll)
        {

            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String result = "";
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    CertifiedPayroll retreivedCertifiedPayroll = new CertifiedPayroll();
                    retreivedCertifiedPayroll = ctx.CertifiedPayroll.Where(u => u.Description == certifiedpayroll.Description).FirstOrDefault();


                    if (retreivedCertifiedPayroll == null)
                    {
                        //register
                        ctx.CertifiedPayroll.Add(certifiedpayroll);
                        ctx.SaveChanges();
                        result += certifiedpayroll.Description + " has been created successfully.\n";
                    }
                    else
                    {
                        result += certifiedpayroll.Description + "' failed to be created, duplicate Service is not allowed.\n";
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
        public static String updateCertifiedPayroll(CertifiedPayroll certifiedpayroll)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String result = "";

            try
            {
                using (var ctx = new CPPDbContext())
                {
                    CertifiedPayroll retreivedCertifiedPayroll = new CertifiedPayroll();
                    retreivedCertifiedPayroll = ctx.CertifiedPayroll.Where(u => u.ID == certifiedpayroll.ID).FirstOrDefault();

                    CertifiedPayroll duplicateCertifiedPayroll = ctx.CertifiedPayroll.Where(a => (a.ID != certifiedpayroll.ID
                                                                                  && a.Description == certifiedpayroll.Description)).FirstOrDefault();


                    //if (retreivedCertifiedPayroll != null && retreivedCertifiedPayroll.ID != certifiedpayroll.ID)
                    //{
                    //    return "Updating the project division code is unavailable at this moment (" + certifiedpayroll.Description + "). \n";
                    //}
                    if (duplicateCertifiedPayroll != null)
                    {
                        result += certifiedpayroll.Description + " failed to be updated, duplicate of certified Payroll is not allowed.\n";
                    }
                    else if (retreivedCertifiedPayroll != null)
                    {
                        CopyUtil.CopyFields<CertifiedPayroll>(certifiedpayroll, retreivedCertifiedPayroll);
                        ctx.Entry(retreivedCertifiedPayroll).State = System.Data.Entity.EntityState.Modified;
                        ctx.SaveChanges();
                        result += certifiedpayroll.Description + " has been updated successfully.\n";
                    }
                    else
                    {
                        result += certifiedpayroll.Description + " failed to be updated, it does not exist.\n";
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
        public static String deleteCertifiedPayroll(CertifiedPayroll certifiedpayroll)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String result = "";
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    CertifiedPayroll retreivedCertifiedPayroll = new CertifiedPayroll();
                    retreivedCertifiedPayroll = ctx.CertifiedPayroll.Where(u => u.ID == certifiedpayroll.ID).FirstOrDefault();

                    if (retreivedCertifiedPayroll != null)
                    {
                        ctx.CertifiedPayroll.Remove(retreivedCertifiedPayroll);
                        ctx.SaveChanges();
                        result = certifiedpayroll.Description + " has been deleted successfully.\n";
                    }
                    else
                    {
                        result = certifiedpayroll.Description + " failed to be deleted, it does not exist.\n";
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