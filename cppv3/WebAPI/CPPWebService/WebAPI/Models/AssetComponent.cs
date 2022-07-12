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
    public class AssetComponent
    {
        [NotMapped]
        public int Operation;

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public String Tag { get; set; }
        public String Name { get; set; }
        public DateTime AcquisitionDate { get; set; }
        public String Note { get; set; }
        public double Cost { get; set; }


        //Foreign Key

        public int AssetID { get; set; }
        [ForeignKey("AssetID")]
        //Navigation Property-enable lazy loading
        public virtual Asset asset { get; set; }
        public virtual ICollection<AssetComponentHistory> assetComponentHistories { get; set; }



        public static List<AssetComponent> getAssetComponentByID(int assetComponentID)
        {
            List<AssetComponent> MatchedAssetComponentList = new List<AssetComponent>();

            using (var ctx = new CPPDbContext())
            {
                try
                {
                    //Facility facility = ctx.Facility.Include(assetComponentHIstories).Where(a => a.ID == assetComponentID).FirstOrDefault();
                    MatchedAssetComponentList = ctx.AssetComponent
                        .Include("assetComponentHistories")
                        .Where(a => a.ID == assetComponentID)
                        .ToList();
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

            return MatchedAssetComponentList;
        }

        public static String registerAssetComponent(AssetComponent assetComponent)
        {
            String status = "";
            using (var ctx = new CPPDbContext())
            {

                try
                {
                    ctx.AssetComponent.Add(assetComponent);
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
        public static String deleteAssetComponent(AssetComponent assetComponent)
        {

            String delete_result = "";
            bool OkForDelete = false;
            using (var ctx = new CPPDbContext())
            {
                try
                {
                    // query to check existence of object in database
                    AssetComponent assetComponentObj = ctx.AssetComponent.Include("assetComponentHistories").Where(f => f.ID == assetComponent.ID).FirstOrDefault();
                    if (assetComponentObj != null)
                    {
                        OkForDelete = true;
                    }
                    else
                    {
                        delete_result += "Asset Component " + assetComponentObj.Name + " does not exist in system";

                    }


                    // delete from database
                    if (OkForDelete)
                    {
                        ctx.AssetComponent.Remove(assetComponentObj);
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
        public static String updateAssetComponent(AssetComponent assetComponent)
        {

            String update_result = "";
            bool OkForUpdate = false;
            using (var ctx = new CPPDbContext())
            {
                try
                {
                    // query to check existence of object in database

                    AssetComponent assetComponentObj= ctx.AssetComponent.Where(f => f.ID == assetComponent.ID).FirstOrDefault();
                    // check the database
                    if (assetComponentObj != null)
                    {
                        OkForUpdate = true;
                    }
                    else
                    {
                        update_result += "Asset Component " + assetComponentObj.Name + " does not exist in system";

                    }

                    // update database entry
                    if (OkForUpdate)
                    {
                        using (var dbCtx = new CPPDbContext())
                        {
                            assetComponentObj = assetComponent;
                            dbCtx.Entry(assetComponentObj).State = System.Data.Entity.EntityState.Modified;
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