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
    public class AssetComponentHistory
    {
        [NotMapped]
        public int Operation;

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public String Description { get; set; }
        public String Note { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double Cost { get; set; }

        //Foreign Key

        public int Asset_Component_ID { get; set; }
        [ForeignKey("Asset_Component_ID")]
        //Navigation Property-enable lazy loading
        public virtual AssetComponent assetComponent{ get; set; }


        public static List<AssetComponentHistory> getAssetComponentHistoryByID(int assetComponentHistoryID)
        {
            List<AssetComponentHistory> MatchedAssetComponentHistoryList = new List<AssetComponentHistory>();

            using (var ctx = new CPPDbContext())
            {
                try
                {

                    MatchedAssetComponentHistoryList = ctx.AssetComponentHistory.Where(a => a.ID == assetComponentHistoryID).ToList();
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

            return MatchedAssetComponentHistoryList;
        }

        public static String registerAssetComponentHistory(AssetComponentHistory assetComponentHistory)
        {
            String status = "";
            using (var ctx = new CPPDbContext())
            {

                try
                {
                    ctx.AssetComponentHistory.Add(assetComponentHistory);
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
        public static String deleteAssetComponentHistory(AssetComponentHistory assetComponentHistory)
        {

            String delete_result = "";
            bool OkForDelete = false;
            using (var ctx = new CPPDbContext())
            {
                try
                {
                    // query to check existence of object in database
                    AssetComponentHistory assetComponentHistoryObj= ctx.AssetComponentHistory.Where(f => f.ID == assetComponentHistory.ID).FirstOrDefault();
                    if (assetComponentHistoryObj != null)
                    {
                        OkForDelete = true;
                    }
                    else
                    {
                        delete_result += "Asset Component History ID " + assetComponentHistoryObj.ID + " does not exist in system";

                    }


                    // delete from database
                    if (OkForDelete)
                    {
                        ctx.AssetComponentHistory.Remove(assetComponentHistoryObj);
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
        public static String updateAssetComponentHistory(AssetComponentHistory assetComponentHistory)
        {

            String update_result = "";
            bool OkForUpdate = false;
            using (var ctx = new CPPDbContext())
            {
                try
                {
                    // query to check existence of object in database

                    AssetComponentHistory assetComponentHistoryObj = ctx.AssetComponentHistory.Where(f => f.ID == assetComponentHistory.ID).FirstOrDefault();
                    // check the database
                    if (assetComponentHistoryObj != null)
                    {
                        OkForUpdate = true;
                    }
                    else
                    {
                        update_result += "Asset history ID " + assetComponentHistoryObj.ID + " does not exist in system";

                    }

                    // update database entry
                    if (OkForUpdate)
                    {
                        using (var dbCtx = new CPPDbContext())
                        {
                            assetComponentHistoryObj = assetComponentHistory;
                            dbCtx.Entry(assetComponentHistoryObj).State = System.Data.Entity.EntityState.Modified;
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