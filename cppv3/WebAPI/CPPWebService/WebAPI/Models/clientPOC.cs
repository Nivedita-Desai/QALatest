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
    [Table("client_poc")]                                                                                   //Tanmay - 15/12/2021
    public class clientPOC:Audit
    {
        [NotMapped]
        public int Operation;
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ClientPOCID { get; set; }
        public int ClientID { get; set; }
        public String BillingPOC { get; set; }
        public String BillingPOCDescription { get; set; }
        public String BillingPOCPhone1 { get; set; }
        public String BillingPOCPhone2 { get; set; }
        public String BillingPOCEmail { get; set; }
        public String BillingPOCAddressLine1 { get; set; }
        public String BillingPOCAddressLine2 { get; set; }
        public String BillingPOCCity { get; set; }
        public String BillingPOCState { get; set; }
        public String BillingPOCPONo { get; set; }
        public String UniqueIdentityNumber { get; set; }

        public static List<clientPOC> getClientPOC()            //Tanmay - 15/12/2021
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);

            List<clientPOC> clientPOCList = new List<clientPOC>();
            try
            {

                using (var ctx = new CPPDbContext())
                {
                    clientPOCList = ctx.ClientPOC.OrderBy(a => a.UniqueIdentityNumber).ToList();
                }

            }
            catch (Exception ex)
            {
                var innerEX = ex.InnerException.ToString();
                var stackTrace = new StackTrace(ex, true);
                var line = stackTrace.GetFrame(0).GetFileLineNumber();
                Logger.LogExceptions(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, ex.Message, line.ToString(), Logger.logLevel.Exception);
            }
            finally
            {
            }
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Exit Point", Logger.logLevel.Debug);

            return clientPOCList;
        }

        public static String registerClientPOC(clientPOC clientPOC)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String result = "";
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    clientPOC retreivedClient = new clientPOC();
                    retreivedClient = ctx.ClientPOC.Where(u => u.ClientID == clientPOC.ClientID).FirstOrDefault();

                    clientPOC duplicateUniqueIdentityNumber = new clientPOC();
                    duplicateUniqueIdentityNumber = ctx.ClientPOC.Where(u => u.UniqueIdentityNumber == clientPOC.UniqueIdentityNumber).FirstOrDefault();

                    if (duplicateUniqueIdentityNumber != null)
                    {
                        result += clientPOC.BillingPOC + "' failed to be created, duplicate unique identifier found.\n";
                    }
                    else if (retreivedClient == null)
                    {
                        //register
                        ctx.ClientPOC.Add(clientPOC);
                        ctx.SaveChanges();
                        result = clientPOC.BillingPOC + " has been created successfully.\n";
                    }
                    else
                    {
                        result += clientPOC.BillingPOC + "' failed to be created, it already exist.\n";
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

        public static String updateClientPOC(clientPOC clientPOC)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String result = "";

            try
            {
                using (var ctx = new CPPDbContext())
                {
                    clientPOC retreivedClient = new clientPOC();
                    retreivedClient = ctx.ClientPOC.Where(u => u.ClientID == clientPOC.ClientID).FirstOrDefault();
                    
                    int ClientId = retreivedClient.ClientID;
                    clientPOC duplicateClient = new clientPOC();
                    duplicateClient = ctx.ClientPOC.Where(u => u.ClientID == clientPOC.ClientID
                                                            && u.ClientPOCID != clientPOC.ClientPOCID).FirstOrDefault();

                    clientPOC duplicateUniqueIdentityNumber = new clientPOC();
                    duplicateUniqueIdentityNumber = ctx.ClientPOC.Where(u => u.UniqueIdentityNumber == clientPOC.UniqueIdentityNumber && u.ClientPOCID != clientPOC.ClientPOCID).FirstOrDefault();

                    if (duplicateClient != null)
                    {
                        result += clientPOC.BillingPOC + " failed to be updated, duplicate will be created.\n";
                    }
                    else if (duplicateUniqueIdentityNumber != null)
                    {
                        result += clientPOC.BillingPOC + "' failed to be created, duplicate unique identifier found.\n";
                    }
                    else if (retreivedClient != null)
                    {
                        CopyUtil.CopyFields<clientPOC>(clientPOC, retreivedClient);
                        retreivedClient.ClientID = ClientId;
                        ctx.Entry(retreivedClient).State = System.Data.Entity.EntityState.Modified;
                        ctx.SaveChanges();
                        result += clientPOC.BillingPOC + " has been updated successfully.\n";
                    }
                    else
                    {
                        result += clientPOC.BillingPOC + " failed to be updated, it does not exist.\n";
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

        public static String deleteClientPOC(clientPOC clientPOC)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String result = "";
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    clientPOC retreivedClient = new clientPOC();
                    retreivedClient = ctx.ClientPOC.Where(u => u.ClientPOCID == clientPOC.ClientPOCID).FirstOrDefault();

                    if (retreivedClient != null)
                    {
                        ctx.ClientPOC.Remove(retreivedClient);
                        ctx.SaveChanges();
                        result = clientPOC.BillingPOC + " has been deleted successfully.\n";
                    }
                    else
                    {
                        result = clientPOC.BillingPOC + " failed to be deleted, it does not exist.\n";
                    }
                }

            }
            catch (Exception ex)
            {
                result = clientPOC.BillingPOC + " failed to be deleted due to dependencies.\n";
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

        public static String nextUniqueIdentityNumber()
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String nextUniqueIdentityNumber = "";
            int CntClientPOC;

            try
            {
                using (var ctx = new CPPDbContext())
                {
                    CntClientPOC = ctx.ClientPOC.Count();
                    if (CntClientPOC >= 1)
                        nextUniqueIdentityNumber = ctx.ClientPOC.Max(u => u.UniqueIdentityNumber);
                    else
                        nextUniqueIdentityNumber = "PC00000";
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

            return nextUniqueIdentityNumber;
        }


    }
}