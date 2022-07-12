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
    [Table("inventory")]
    public class Inventory : Audit
    {
        [NotMapped]
        public int Operation;

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InventoryID { get; set; }
        public String InventoryName { get; set; } // Jignesh 02-12-2020
        public String InventoryDescription { get; set; } // Jignesh 02-12-2020
        public int? ProgramElementID { get; set; }
        public int? ProjectID { get; set; }
        public int? MaterialID { get; set; }
        public String AvailableQty { get; set; }
        public String PendingQty { get; set; }
        //public DateTime CreatedDate { get; set; }
        //public DateTime UpdatedDate { get; set; }
        //public String CreatedBy { get; set; }
        //public String UpdatedBy { get; set; }



        public static List<Inventory> getInventory()
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);

            List<Inventory> InventoryList = new List<Inventory>();
            try
            {

                using (var ctx = new CPPDbContext())
                {
                    InventoryList = ctx.Inventory.OrderByDescending(a => a.InventoryID).ToList();
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

            return InventoryList;
        }


        public static String registerInventory(Inventory Inventory)
        {

            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String result = "";
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    Inventory retrievedInventory = new Inventory();
                    retrievedInventory = ctx.Inventory.Where(u => u.InventoryID == Inventory.InventoryID).FirstOrDefault();

                    if (retrievedInventory == null)
                    {
                        //register
                        ctx.Inventory.Add(Inventory);
                        ctx.SaveChanges();
                        //result = "Success";
                        result += Inventory.InventoryID + " has been created successfully.\n";
                    }
                    else
                    {
                        result += Inventory.InventoryID + " failed to be created, it already exist.\n";
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
        public static String updateInventory(Inventory Inventory)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String result = "";

            try
            {
                using (var ctx = new CPPDbContext())
                {
                    Inventory retrievedInventory = new Inventory();
                    retrievedInventory = ctx.Inventory.Where(u => u.InventoryID == Inventory.InventoryID).FirstOrDefault();

                    if (retrievedInventory != null)
                    {
                        CopyUtil.CopyFields<Inventory>(Inventory, retrievedInventory);
                        ctx.Entry(retrievedInventory).State = System.Data.Entity.EntityState.Modified;
                        ctx.SaveChanges();
                        result = Inventory.InventoryID + " has been updated successfully.\n";
                    }
                    else
                    {
                        result = Inventory.InventoryID + " failed to be updated, it does not exist.\n";
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
        public static String deleteInventory(Inventory Inventory)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String result = "";
            try
            {

                using (var ctx = new CPPDbContext())
                {
                    Inventory retrievedInventory = new Inventory();
                    retrievedInventory = ctx.Inventory.Where(u => u.InventoryID == Inventory.InventoryID).FirstOrDefault();

                    if (retrievedInventory != null)
                    {
                        ctx.Inventory.Remove(retrievedInventory);
                        ctx.SaveChanges();
                        result = Inventory.InventoryID + " has been deleted successfully.\n";
                    }
                    else
                    {
                        result = Inventory.InventoryID + " failed to be deleted, it does not exist.\n";
                    }
                }

            }
            catch (Exception ex)
            {
                result = Inventory.InventoryID + " failed to be deleted due to dependencies.\n";
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