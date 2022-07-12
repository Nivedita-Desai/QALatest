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
    [Table("vendor")]
    public class Vendor : Audit
    {
        [NotMapped]
        public int Operation;

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VendorID { get; set; }
        public String VendorName { get; set; }
        public String VendorDescription { get; set; }
        public String UniqueIdentityNumber { get; set; }
        //public DateTime CreatedDate { get; set; }
        //public DateTime UpdatedDate { get; set; }
        //public String CreatedBy { get; set; }
        //public String UpdatedBy { get; set; }


        public static List<Vendor> getVendor()
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);

            List<Vendor> VendorList = new List<Vendor>();
            try
            {

                using (var ctx = new CPPDbContext())
                {
                    VendorList = ctx.Vendor.OrderBy(a => a.VendorName).ToList();
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

            return VendorList;
        }


        public static String registerVendor(Vendor Vendor)
        {

            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String result = "";
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    Vendor retrievedVendor = new Vendor();
                    retrievedVendor = ctx.Vendor.Where(u => u.VendorName == Vendor.VendorName).FirstOrDefault();

                    if (retrievedVendor == null)
                    {
                        //register
                        ctx.Vendor.Add(Vendor);
                        ctx.SaveChanges();
                        //result = "Success";
                        result += Vendor.VendorName + " has been created successfully.\n";
                    }
                    else
                    {
                        result += Vendor.VendorName + " failed to be created, it already exist.\n";
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
        public static String updateVendor(Vendor Vendor)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String result = "";

            try
            {
                using (var ctx = new CPPDbContext())
                {
                    Vendor retrievedVendor = new Vendor();
                    retrievedVendor = ctx.Vendor.Where(u => u.VendorID == Vendor.VendorID).FirstOrDefault();

                    Vendor duplicateVendor = new Vendor();
                    duplicateVendor = ctx.Vendor.Where(u => u.VendorName == Vendor.VendorName
                                                                                && u.VendorID != Vendor.VendorID).FirstOrDefault();

                    if (duplicateVendor != null)
                    {
                        result += Vendor.VendorName + " failed to be updated, duplicate will be created.\n";
                    }
                    else if (retrievedVendor != null)
                    {
                        CopyUtil.CopyFields<Vendor>(Vendor, retrievedVendor);
                        ctx.Entry(retrievedVendor).State = System.Data.Entity.EntityState.Modified;
                        ctx.SaveChanges();
                        result = Vendor.VendorName + " has been updated successfully.\n";
                    }
                    else
                    {
                        result = Vendor.VendorName + " failed to be updated, it does not exist.\n";
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
        public static String deleteVendor(Vendor Vendor)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String result = "";
            try
            {

                using (var ctx = new CPPDbContext())
                {
                    Vendor retrievedVendor = new Vendor();
                    retrievedVendor = ctx.Vendor.Where(u => u.VendorID == Vendor.VendorID).FirstOrDefault();

                    if (retrievedVendor != null)
                    {
                        ctx.Vendor.Remove(retrievedVendor);
                        ctx.SaveChanges();
                        result = Vendor.VendorName + " has been deleted successfully.\n";
                    }
                    else
                    {
                        result = Vendor.VendorName + " failed to be deleted, it does not exist.\n";
                    }
                }

            }
            catch (Exception ex)
            {
                result = Vendor.VendorName + " failed to be deleted due to dependencies.\n";
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
                    nextUniqueIdentityNumber = ctx.Vendor.Max(u => u.UniqueIdentityNumber);
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