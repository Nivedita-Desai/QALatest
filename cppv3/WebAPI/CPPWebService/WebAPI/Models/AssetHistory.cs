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
    public class AssetHistory
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
    
        public int AssetID { get; set; }
        [ForeignKey("AssetID")]
        //Navigation Property-enable lazy loading
        public virtual Asset asset{ get; set; }


        public static List<AssetHistory> getAssetHistoryByID(int historyID)
        {
            List<AssetHistory> MatchedAssetHistoryList = new List<AssetHistory>();

            using (var ctx = new CPPDbContext())
            {
                try
                {

                    MatchedAssetHistoryList = ctx.AssetHistory.Where(a => a.ID == historyID).ToList();
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

            return MatchedAssetHistoryList;
        }

        public static String registerAssetHistory(AssetHistory assetHistory)
        {
            String status = "";
            using (var ctx = new CPPDbContext())
            {

                try
                {
                    ctx.AssetHistory.Add(assetHistory);
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
        public static String deleteAssetHistory(AssetHistory assetHistory)
        {

            String delete_result = "";
            bool OkForDelete = false;
            using (var ctx = new CPPDbContext())
            {
                try
                {
                    // query to check existence of object in database
                    AssetHistory assetHistoryObj= ctx.AssetHistory.Where(f => f.ID == assetHistory.ID).FirstOrDefault();
                    if (assetHistoryObj != null)
                    {
                        OkForDelete = true;
                    }
                    else
                    {
                        delete_result += "Asset History " + assetHistoryObj.ID+ " does not exist in system";

                    }


                    // delete from database
                    if (OkForDelete)
                    {
                        ctx.AssetHistory.Remove(assetHistoryObj);
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
        public static String updateAssetHistory(AssetHistory assetHistory)
        {

            String update_result = "";
            bool OkForUpdate = false;
            using (var ctx = new CPPDbContext())
            {
                try
                {
                    // query to check existence of object in database

                    AssetHistory assetHistoryObj= ctx.AssetHistory.Where(f => f.ID == assetHistory.ID).FirstOrDefault();
                    // check the database
                    if (assetHistoryObj != null)
                    {
                        OkForUpdate = true;
                    }
                    else
                    {
                        update_result += "Asset history ID " + assetHistoryObj.ID+ " does not exist in system";

                    }

                    // update database entry
                    if (OkForUpdate)
                    {
                        using (var dbCtx = new CPPDbContext())
                        {
                            assetHistoryObj = assetHistory;
                            dbCtx.Entry(assetHistoryObj).State = System.Data.Entity.EntityState.Modified;
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