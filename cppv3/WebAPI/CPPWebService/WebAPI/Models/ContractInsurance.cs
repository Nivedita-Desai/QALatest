using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using WebAPI.Helper;
using System.Diagnostics;

namespace WebAPI.Models
{
    [Table("contract_insurance")]
    public class ContractInsurance : Audit
    {
        readonly static log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        [NotMapped]
        public int Operation;
        public int Id { get; set; }
        public string Type { get; set; }
        public string Limit { get; set; }
        public int ProgramID { get; set; }


        public static void SaveInsurance(ContractInsurance contractInsurance)
        {

            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            List<ContractInsurance> ContractInsurance = new List<ContractInsurance>();

            try
            {
                using (var ctx = new CPPDbContext())
                {

                    ctx.ContractInsurances.Add(contractInsurance);
                    ctx.SaveChanges();

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
        }

        public static void UpdateInsurance(ContractInsurance contractInsurance)
        {

            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String result = "";

            try
            {
                using (var ctx = new CPPDbContext())
                {
                    ContractInsurance retreivedInsurance = new ContractInsurance();
                    retreivedInsurance = ctx.ContractInsurances.Where(p => p.Id == contractInsurance.Id).FirstOrDefault();

                    if (retreivedInsurance != null)
                    {
                        CopyUtil.CopyFields<ContractInsurance>(contractInsurance, retreivedInsurance);
                        ctx.Entry(retreivedInsurance).State = System.Data.Entity.EntityState.Modified;
                        ctx.SaveChanges();
                        result = contractInsurance.Type + " has been updated successfully.\n";
                    }
                    else
                    {
                        result += contractInsurance.Type + " failed to be updated, it does not exist.\n";
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
        }


        public static List<ContractInsurance> GetContractInsurances(int programId)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            List<ContractInsurance> contractInsuranceList = new List<ContractInsurance>();

            Program programid = new Program();
            try
            {
                using (var ctx = new CPPDbContext())
                {

                    contractInsuranceList = ctx.ContractInsurances.Where(u => u.ProgramID == programId).ToList();


                    return contractInsuranceList;

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
            return contractInsuranceList;
        }
    }
}