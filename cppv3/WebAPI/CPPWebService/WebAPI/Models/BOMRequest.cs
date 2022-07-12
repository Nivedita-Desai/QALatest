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
    [Table("bom_request")]
    public class BOMRequest : Audit
    {
        [NotMapped]
        public int Operation;

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BOMRequestID { get; set; }
        public String ProjectNumber { get; set; }
        public String ProjectElementNumber { get; set; }
        public String Manufacturer { get; set; }
        public String Vendor { get; set; }
        public String PartNumber { get; set; }
        public String SerialNumber { get; set; }
        public String Description { get; set; }
        public String UM { get; set; }
        public String UnitCost { get; set; }
        public String ExtCost { get; set; }
        public String InitialQty { get; set; }
        public String AvailableQty { get; set; }
        public String RequestedQty { get; set; }
        public String RequestedDate { get; set; }
        public String ActualReleaseDate { get; set; }
        public String PurchaseOrderNumber { get; set; }
        public String Status { get; set; }
        public String Invoiced { get; set; }
        //public DateTime CreatedDate { get; set; }
        //public DateTime UpdatedDate { get; set; }
        //public String CreatedBy { get; set; }
        //public String UpdatedBy { get; set; }
        public int? ProgramElementID { get; set; }
        public int? ProjectID { get; set; }
        public int? ManufacturerID { get; set; }
        public int? MaterialID { get; set; }
        public int? InventoryID { get; set; }
        public int? VendorID { get; set; }


        public static List<BOMRequest> getBOMRequest()
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);

            List<BOMRequest> BOMRequestList = new List<BOMRequest>();
            try
            {

                using (var ctx = new CPPDbContext())
                {
                    BOMRequestList = ctx.BOMRequest.OrderByDescending(a => a.BOMRequestID).ToList();
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

            return BOMRequestList;
        }


        public static String registerBOMRequest(BOMRequest BOMRequest)
        {

            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String result = "";
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    BOMRequest retrievedBOMRequest = new BOMRequest();
                    retrievedBOMRequest = ctx.BOMRequest.Where(u => u.BOMRequestID == BOMRequest.BOMRequestID).FirstOrDefault();

                    if (retrievedBOMRequest == null)
                    {
                        //register
                        ctx.BOMRequest.Add(BOMRequest);
                        ctx.SaveChanges();
                        //result = "Success";
                        result += BOMRequest.BOMRequestID + " has been created successfully.\n";
                    }
                    else
                    {
                        result += BOMRequest.BOMRequestID + " failed to be created, it already exist.\n";
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
        public static String updateBOMRequest(BOMRequest BOMRequest)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String result = "";

            try
            {
                using (var ctx = new CPPDbContext())
                {
                    BOMRequest retrievedBOMRequest = new BOMRequest();
                    retrievedBOMRequest = ctx.BOMRequest.Where(u => u.BOMRequestID == BOMRequest.BOMRequestID).FirstOrDefault();

                    if (retrievedBOMRequest != null)
                    {
                        CopyUtil.CopyFields<BOMRequest>(BOMRequest, retrievedBOMRequest);
                        ctx.Entry(retrievedBOMRequest).State = System.Data.Entity.EntityState.Modified;
                        ctx.SaveChanges();
                        result = BOMRequest.BOMRequestID + " has been updated successfully.\n";
                    }
                    else
                    {
                        result = BOMRequest.BOMRequestID + " failed to be updated, it does not exist.\n";
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
        public static String deleteBOMRequest(BOMRequest BOMRequest)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String result = "";
            try
            {

                using (var ctx = new CPPDbContext())
                {
                    BOMRequest retrievedBOMRequest = new BOMRequest();
                    retrievedBOMRequest = ctx.BOMRequest.Where(u => u.BOMRequestID == BOMRequest.BOMRequestID).FirstOrDefault();

                    if (retrievedBOMRequest != null)
                    {
                        ctx.BOMRequest.Remove(retrievedBOMRequest);
                        ctx.SaveChanges();
                        result = BOMRequest.BOMRequestID + " has been deleted successfully.\n";
                    }
                    else
                    {
                        result = BOMRequest.BOMRequestID + " failed to be deleted, it does not exist.\n";
                    }
                }

            }
            catch (Exception ex)
            {
                result = BOMRequest.BOMRequestID + " failed to be deleted due to dependencies.\n";
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