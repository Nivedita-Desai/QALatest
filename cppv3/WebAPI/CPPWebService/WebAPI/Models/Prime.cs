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
    [Table("primes")]
    public class Prime : Audit
    {
        [NotMapped]
        public int Operation;

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        //public String Type { get; set; }
        public String Name { get; set; }

        //public DateTime CreatedDate { get; set; }
        //public DateTime UpdatedDate { get; set; }
        //public String CreatedBy { get; set; }
        //public String UpdatedBy { get; set; }

        public static List<Prime> getPrime()
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);

            List<Prime> PrimeList = new List<Prime>();
            try
            {

                using (var ctx = new CPPDbContext())
                {
                    PrimeList = ctx.Prime.OrderBy(a => a.id).ToList();
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

            return PrimeList;
        }

        public static Prime getPrimeById(int primeId)
        {
            Prime Prime = null;
            try
            {

                using (var ctx = new CPPDbContext())
                {
                    Prime = ctx.Prime.Where(x => x.id == primeId).FirstOrDefault();
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

            return Prime;
        }

        public static String registerPrime(Prime Prime)
        {

            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String result = "";
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    Prime retreivedPrime = new Prime();
                    retreivedPrime = ctx.Prime.Where(u => u.Name == Prime.Name
                                                                        || u.id == Prime.id 
                                                                        //|| u.Type==Prime.Type
                                                                        ).FirstOrDefault();
                    

                    if (retreivedPrime == null)
                    {
                        //register
                        ctx.Prime.Add(Prime);
                        ctx.SaveChanges();
                        result += Prime.Name + " has been created successfully.\n";
                    }
                    else
                    {
                        result += Prime.Name + "' failed to be created, duplicate Prime is not allowed.\n";
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
        public static String updatePrime(Prime Prime)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String result = "";

            try
            {
                using (var ctx = new CPPDbContext())
                {
                    Prime retreivedPrime = new Prime();
                    retreivedPrime = ctx.Prime.Where(u => u.id == Prime.id).FirstOrDefault();

                    Prime duplicatePrime = ctx.Prime.Where(a => (a.id != Prime.id
                                                                                  && (a.Name == Prime.Name))
                                                                                  //|| (a.id != Prime.id
                                                                                  
                          //                                                        && (a.Type == Prime.Type)
                                                                                  //)
                                                                                  ).FirstOrDefault();
                                                                                  

                    if (retreivedPrime != null && retreivedPrime.id != Prime.id)
                    {
                        return "Updating the prime is unavailable at this moment (" + Prime.Name + "). \n";
                    }
                    else if (duplicatePrime != null)
                    {
                        result += Prime.Name + " failed to be updated, duplicate of Prime is not allowed.\n";
                    }
                    else if (retreivedPrime != null)
                    {
                        CopyUtil.CopyFields<Prime>(Prime, retreivedPrime);
                        ctx.Entry(retreivedPrime).State = System.Data.Entity.EntityState.Modified;
                        ctx.SaveChanges();
                        result += Prime.Name + " has been updated successfully.\n";
                    }
                    else
                    {
                        result += Prime.Name + " failed to be updated, it does not exist.\n";
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
        public static String deletePrime(Prime Prime)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String result = "";
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    Prime retreivedPrime = new Prime();
                    retreivedPrime = ctx.Prime.Where(u => u.id == Prime.id).FirstOrDefault();

                    if (retreivedPrime != null)
                    {
                        ctx.Prime.Remove(retreivedPrime);
                        ctx.SaveChanges();
                        result = Prime.Name + " has been deleted successfully.\n";
                    }
                    else
                    {
                        result = Prime.Name + " falied to be deleted, it does not exist.\n";
                    }
                }

            }
            catch (Exception ex)
            {
                result += Prime.Name + " failed to be deleted due to dependencies.\n";
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