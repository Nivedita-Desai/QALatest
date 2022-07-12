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
    [Table("client")]
    public class Client : Audit
    {
        [NotMapped]
        public int Operation;

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ClientID { get; set; }
        public String ClientName { get; set; }
        public String ClientDescription { get; set; }
        public String ClientPhone{ get; set; }                //Tanmay - 15/12/2021
        public String ClientEmail { get; set; }
        public String ClientAddressLine1 { get; set; }
        public String ClientAddressLine2 { get; set; }
        public String ClientCity { get; set; }
        public String ClientState { get; set; }
        public String ClientPONo { get; set; }
        public String UniqueIdentityNumber { get; set; }
        //public DateTime CreatedDate { get; set; }
        //public DateTime UpdatedDate { get; set; }
        //public String CreatedBy { get; set; }
        //public String UpdatedBy { get; set; }

        public static List<Client> getClient()
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);

            List<Client> clientList = new List<Client>();
            try
            {

                using (var ctx = new CPPDbContext())
                {
                    clientList = ctx.Client.OrderBy(a => a.UniqueIdentityNumber).ToList();
                    foreach (var i in clientList)
                    {
                        if (i.ClientPhone != null)
                            i.ClientPhone = i.ClientPhone.Replace("-", "");
                    }

                }

            }
            catch (Exception ex)
            {
                var innerEX = ex.InnerException.ToString();         //Tanmay - 15/12/2021
                var stackTrace = new StackTrace(ex, true);
                var line = stackTrace.GetFrame(0).GetFileLineNumber();
                Logger.LogExceptions(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, ex.Message, line.ToString(), Logger.logLevel.Exception);
            }
            finally
            {
            }
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Exit Point", Logger.logLevel.Debug);

            return clientList;
        }


        public static String registerClient(Client client)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String result = "";
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    Client retreivedClient = new Client();
                    retreivedClient = ctx.Client.Where(u => u.ClientName == client.ClientName).FirstOrDefault();

                    Client duplicateUniqueIdentityNumber = new Client();
                    duplicateUniqueIdentityNumber = ctx.Client.Where(u => u.UniqueIdentityNumber == client.UniqueIdentityNumber).FirstOrDefault();

                    if (duplicateUniqueIdentityNumber != null)
                    {
                        result += client.ClientName + "' failed to be created, duplicate unique identifier found.\n";
                    }
                    else if (retreivedClient == null)
                    {
                        //register
                        ctx.Client.Add(client);
                        ctx.SaveChanges();
                        result = client.ClientName + " has been created successfully.\n";
                    }
                    else
                    {
                        result += client.ClientName + "' failed to be created, it already exist.\n";
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

        public static String updateClient(Client client)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String result = "";

            try
            {
                using (var ctx = new CPPDbContext())
                {
                    Client retreivedClient = new Client();
                    retreivedClient = ctx.Client.Where(u => u.ClientID == client.ClientID).FirstOrDefault();

                    Client duplicateClient = new Client();
                    duplicateClient = ctx.Client.Where(u => u.ClientName == client.ClientName
                                                            && u.ClientID != client.ClientID).FirstOrDefault();

                    Client duplicateUniqueIdentityNumber = new Client();
                    duplicateUniqueIdentityNumber = ctx.Client.Where(u => u.UniqueIdentityNumber == client.UniqueIdentityNumber && u.ClientID != client.ClientID).FirstOrDefault();

                    if (duplicateClient != null)
                    {
                        result += client.ClientName + " failed to be updated, duplicate will be created.\n";
                    }
                    else if (duplicateUniqueIdentityNumber != null)
                    {
                        result += client.ClientName + "' failed to be created, duplicate unique identifier found.\n";
                    }
                    else if (retreivedClient != null)
                    {
                        CopyUtil.CopyFields<Client>(client, retreivedClient);
                        ctx.Entry(retreivedClient).State = System.Data.Entity.EntityState.Modified;
                        ctx.SaveChanges();
                        result += client.ClientName + " has been updated successfully.\n";
                    }
                    else
                    {
                        result += client.ClientName + " failed to be updated, it does not exist.\n";
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
        public static String deleteClient(Client client)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String result = "";
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    Client retreivedClient = new Client();
                    retreivedClient = ctx.Client.Where(u => u.ClientName == client.ClientName).FirstOrDefault();

                    if (retreivedClient != null)
                    {
                        ctx.Client.Remove(retreivedClient);
                        ctx.SaveChanges();
                        result = client.ClientName + " has been deleted successfully.\n";
                    }
                    else
                    {
                        result = client.ClientName + " failed to be deleted, it does not exist.\n";
                    }
                }

            }
            catch (Exception ex)
            {
                result = client.ClientName + " failed to be deleted due to dependencies.\n";
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

            try
            {
                using (var ctx = new CPPDbContext())
                {
                    nextUniqueIdentityNumber = ctx.Client.Max(u => u.UniqueIdentityNumber);
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