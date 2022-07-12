using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Web;
using WebAPI.Controllers;
using WebAPI.Helper;
using WebAPI.Models;

namespace WebAPI.Models
{
    [Table("subcontractor")]
    public class Subcontractor : Audit
    {
        [NotMapped]
        public int Operation;



        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SubcontractorID { get; set; }
        public int SubcontractorTypeID { get; set; }
        public String SubcontractorName { get; set; }
        public String SubcontractorDescription { get; set; }
        public String UniqueIdentityNumber { get; set; }
        public bool MarkUp { get; set; }
        //public String CreatedBy { get; set; }
        //public String LastUpdatedBy { get; set; }
        //public DateTime CreatedDate { get; set; }
        //public DateTime LastUpdatedDate { get; set; }

        [ForeignKey("SubcontractorTypeID")]
        public virtual SubcontractorType SubcontractorType{ get; set; }

        public static List<Subcontractor> GetSubcontractor()
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);

            List<Subcontractor> subcontractorList = new List<Subcontractor>();
            try
            {

                using (var ctx = new CPPDbContext())
                {
                    subcontractorList = ctx.Subcontractor.OrderBy(a => a.UniqueIdentityNumber).ToList();
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

            return subcontractorList;
        }


        public static String registerSubcontractor(Subcontractor subcontractor)
        {

            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String result = "";
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    Subcontractor retrievedSubcontractor = new Subcontractor();
                    retrievedSubcontractor = ctx.Subcontractor.Where(u => u.SubcontractorName == subcontractor.SubcontractorName
                                                                          && u.SubcontractorTypeID == subcontractor.SubcontractorTypeID).FirstOrDefault();

                    Subcontractor duplicateUniqueIdentityNumber = new Subcontractor();
                    duplicateUniqueIdentityNumber = ctx.Subcontractor.Where(u => u.UniqueIdentityNumber == subcontractor.UniqueIdentityNumber).FirstOrDefault();

                    if (duplicateUniqueIdentityNumber != null)
                    {
                        result += subcontractor.SubcontractorName + "' failed to be created, duplicate unique identifier found.\n";
                    }
                    else if (retrievedSubcontractor == null)
                    {
                        //register
                        ctx.Subcontractor.Add(subcontractor);
                        ctx.SaveChanges();
                        //result = "Success";
                        result += subcontractor.SubcontractorName + " has been created successfully.\n";
                    }
                    else
                    {
                        result += subcontractor.SubcontractorName + "' failed to be created, it already exist.\n";
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
        public static String updateSubcontractor(Subcontractor subcontractor)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String result = "";

            try
            {
                using (var ctx = new CPPDbContext())
                {
                    Subcontractor retrievedSubcontractor = new Subcontractor();
                    retrievedSubcontractor = ctx.Subcontractor.Where(u => u.SubcontractorID == subcontractor.SubcontractorID).FirstOrDefault();

                    Subcontractor duplicateSubcontractor = new Subcontractor();
                    duplicateSubcontractor = ctx.Subcontractor.Where(u => u.SubcontractorName == subcontractor.SubcontractorName
                                                                            && u.SubcontractorTypeID == subcontractor.SubcontractorTypeID
                                                                            && u.SubcontractorID != subcontractor.SubcontractorID).FirstOrDefault();

                    Subcontractor duplicateUniqueIdentityNumber = new Subcontractor();
                    duplicateUniqueIdentityNumber = ctx.Subcontractor.Where(u => u.UniqueIdentityNumber == subcontractor.UniqueIdentityNumber && u.SubcontractorID != subcontractor.SubcontractorID).FirstOrDefault();

                    if (duplicateSubcontractor != null)
                    {
                        result = subcontractor.SubcontractorName + " failed to be updated, duplicate will be created.\n";
                    }
                    else if (duplicateUniqueIdentityNumber != null)
                    {
                        result += subcontractor.SubcontractorName + "' failed to be created, duplicate unique identifier found.\n";
                    }
                    else if (retrievedSubcontractor != null)
                    {
                        CopyUtil.CopyFields<Subcontractor>(subcontractor, retrievedSubcontractor);
                        ctx.Entry(retrievedSubcontractor).State = System.Data.Entity.EntityState.Modified;
                        ctx.SaveChanges();
                        result += subcontractor.SubcontractorName + " has been updated successfully.\n";
                    }
                    else
                    {
                        result += subcontractor.SubcontractorName + " failed to be updated, it does not exist.\n";
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
        public static String deleteSubcontractor(Subcontractor subcontractor)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String result = "";
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    Subcontractor retrievedSubcontractor = new Subcontractor();
                    retrievedSubcontractor = ctx.Subcontractor.Where(u => u.SubcontractorID == subcontractor.SubcontractorID).FirstOrDefault();

                    if (retrievedSubcontractor != null)
                    {
                        ctx.Subcontractor.Remove(retrievedSubcontractor);
                        ctx.SaveChanges();
                        result = subcontractor.SubcontractorName + " has been deleted successfully.\n";
                    }
                    else
                    {
                        result = subcontractor.SubcontractorName + " failed to be deleted, it does not exist.\n";
                    }
                }

            }
            catch (Exception ex)
            {
                result = subcontractor.SubcontractorName + " failed to be updated due to dependencies.\n";
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
                    nextUniqueIdentityNumber = ctx.Subcontractor.Max(u => u.UniqueIdentityNumber);
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