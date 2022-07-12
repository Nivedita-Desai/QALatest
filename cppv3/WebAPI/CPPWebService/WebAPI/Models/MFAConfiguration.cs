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
    [Table("mfa_configuration_detail")]
    public class MFAConfiguration
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MFAConfigID { get; set; }
        public int? IsMFAEnabled { get; set; }
        public int ReminderMailDays { get; set; }
        public int ApprovalCodeValidity { get; set; }

        public static String updateMFADetails(MFAConfiguration mfaDetails)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String result = "";

            try
            {
                using (var ctx = new CPPDbContext())
                {
                    MFAConfiguration retreivedMFADetails = new MFAConfiguration();
                    retreivedMFADetails = ctx.MFAConfigurationDetails.Where(u => u.MFAConfigID == 1).FirstOrDefault();
                    
                    if (retreivedMFADetails != null )
                    {
                        retreivedMFADetails.ReminderMailDays = mfaDetails.ReminderMailDays;
                        retreivedMFADetails.ApprovalCodeValidity = mfaDetails.ApprovalCodeValidity;
                        retreivedMFADetails.IsMFAEnabled = mfaDetails.IsMFAEnabled;
                        ctx.Entry(retreivedMFADetails).State = System.Data.Entity.EntityState.Modified;
                        ctx.SaveChanges();
                        result += " MFA details has been updated successfully.\n";
                    }
                    else
                    {
                        result += " failed to be updated, it does not exist.\n";
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
        public static MFAConfiguration getMFADetails()
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            MFAConfiguration MFADetails = new MFAConfiguration();

            try
            {

                using (var ctx = new CPPDbContext())
                {

                    ctx.Database.Log = msg => Trace.WriteLine(msg);
                    MFADetails = ctx.MFAConfigurationDetails.Where(u => u.MFAConfigID == 1).FirstOrDefault();

                }


            }
            catch (Exception ex)
            {
                var stackTrace = new StackTrace(ex, true);
                var line = stackTrace.GetFrame(0).GetFileLineNumber();
                Logger.LogExceptions(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, ex.Message, line.ToString(), Logger.logLevel.Exception);
            }
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Exit Point", Logger.logLevel.Debug);
            return MFADetails;

        }
        

    }
}