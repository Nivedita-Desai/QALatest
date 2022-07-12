using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Web;

namespace WebAPI.Models
{
    public class AssetSupplier
    {
        [NotMapped]
        public int Operation;

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public String Supplier { get; set; }
        public String Note { get; set; }

        //Navigation Property-enable lazy loading
        public virtual ICollection<Asset> assets { get; set; }

        public static List<AssetSupplier> getAssetSupplierByID(int supplierID)
        {
            List<AssetSupplier> MatchedAssetSupplierList = new List<AssetSupplier>();

            using (var ctx = new CPPDbContext())
            {
                try
                {

                    MatchedAssetSupplierList = ctx.AssetSupplier.Where(a => a.ID == supplierID).ToList();
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

            return MatchedAssetSupplierList;
        }

        public static String registerAssetSupplier(AssetSupplier assetSupplier)
        {
            String status = "";
            using (var ctx = new CPPDbContext())
            {

                try
                {
                    ctx.AssetSupplier.Add(assetSupplier);
                    ctx.SaveChanges();
                    status = "Success";
                }
                catch (Exception ex)
                {
                    status = ex.InnerException.ToString();
                }
                finally
                {

                }
            }

            return status;
        }
        public static String deleteAssetSupplier(AssetSupplier assetSupplier)
        {

            String delete_result = "";
            bool OkForDelete = false;
            using (var ctx = new CPPDbContext())
            {
                try
                {
                    // query to check existence of object in database
                    AssetSupplier assetSupplierObj = ctx.AssetSupplier.Where(f => f.ID == assetSupplier.ID).FirstOrDefault();
                    if (assetSupplierObj != null)
                    {
                        OkForDelete = true;
                    }
                    else
                    {
                        delete_result += "Supplier " + assetSupplierObj.Supplier + " does not exist in system";

                    }


                    // delete from database
                    if (OkForDelete)
                    {
                        ctx.AssetSupplier.Remove(assetSupplierObj);
                        ctx.SaveChanges();

                        delete_result = "Success";
                    }

                }
                catch (Exception ex)
                {
                    var stackTrace = new StackTrace(ex, true);
                    var line = stackTrace.GetFrame(0).GetFileLineNumber();
                    Logger.LogExceptions(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, ex.Message, line.ToString(), Logger.logLevel.Exception);
                    delete_result = ex.Message;
                }
                finally
                {

                }
            }
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Exit Point", Logger.logLevel.Debug);

            return delete_result;
        }
        public static String updateAssetSupplier(AssetSupplier assetSupplier)
        {

            String update_result = "";
            bool OkForUpdate = false;
            using (var ctx = new CPPDbContext())
            {
                try
                {
                    // query to check existence of object in database

                    AssetSupplier assetSupplierObj = ctx.AssetSupplier.Where(f => f.ID == assetSupplier.ID).FirstOrDefault();
                    // check the database
                    if (assetSupplierObj != null)
                    {
                        OkForUpdate = true;
                    }
                    else
                    {
                        update_result += "Supplier " + assetSupplierObj.Supplier + " does not exist in system";

                    }

                    // update database entry
                    if (OkForUpdate)
                    {
                        using (var dbCtx = new CPPDbContext())
                        {
                            assetSupplierObj = assetSupplier;
                            dbCtx.Entry(assetSupplierObj).State = System.Data.Entity.EntityState.Modified;
                            dbCtx.SaveChanges();
                            update_result = "Success";
                        }
                    }
                }
                catch (Exception ex)
                {
                    var stackTrace = new StackTrace(ex, true);
                    var line = stackTrace.GetFrame(0).GetFileLineNumber();
                    Logger.LogExceptions(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, ex.Message, line.ToString(), Logger.logLevel.Exception);
                    update_result = ex.Message;
                }
                finally
                {

                }
            }
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Exit Point", Logger.logLevel.Debug);

            return update_result;
        }

    }
}