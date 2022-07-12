using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Data.Entity;
namespace WebAPI.Models
{
    [Table("asset")]
    public class Asset
    {
        [NotMapped]
        public int Operation;
        // public bool isNew;
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int Facility_ID { get; set; }
        public int? Asset_Type_ID { get; set; }
        public int? Asset_Supplier_ID { get; set; }
        //public int? ParentID { get; set; }
        public String Tag { get; set; }
        public String Name { get; set; }
        public DateTime AcquisitionDate { get; set; }
        public Double Cost { get; set; }
        public String EOL { get; set; }
        public String Status { get; set; }
        public String Note { get; set; }


        //Navigation Properties
        public virtual ICollection<FacilityAsset> facilityAssets{ get;set; }
        public virtual ICollection<AssetComponent> assetComponents { get; set; }
       // public virtual Asset asset { get; set; }
        [ForeignKey("Asset_Supplier_ID")]
        public virtual AssetSupplier assetSupplier { get; set; }
        [ForeignKey("Facility_ID")]
        public virtual Facility facility { get; set; }
        public virtual ICollection<AssetHistory> assetHistories { get; set; }
        //[ForeignKey("ProjectID")]
        //public virtual Project project { get; set; }

        public static List<Asset> getAssetByID(int AssetID)
        {
            List<Asset> MatchedAssetList = new List<Asset>();

            using (var ctx = new CPPDbContext())
            {
                try
                {
                    MatchedAssetList = ctx.Asset
                        .Where(a => a.ID == AssetID)
                        .Include(f => f.assetComponents.Select(ach => ach.assetComponentHistories))
                        .Include(h => h.assetHistories)
                        .Include(fa => fa.facility)
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

            return MatchedAssetList;
        }

        public static String registerAsset(Asset asset)
        {
            String status = "";
            using (var ctx = new CPPDbContext())
            {

                try
                {
                    ctx.Asset.Add(asset);
                    ctx.SaveChanges();

                    var maxId = ctx.Asset.Max(a => a.ID);
                    var newlyInsertedAsset = ctx.Asset.Where(a => a.ID == maxId).FirstOrDefault();

                    FacilityAsset fa = new FacilityAsset();
                    fa.AssetID = newlyInsertedAsset.ID;
                    fa.FacilityID = newlyInsertedAsset.Facility_ID;

                    ctx.FacilityAsset.Add(fa);
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
        public static String deleteAsset(Asset asset)
        {

            String delete_result = "";
            bool OkForDelete = false;
            using (var ctx = new CPPDbContext())
            {
                try
                {
                    // query to check existence of object in database
                    Asset assetObj = ctx.Asset.Where(a => a.ID == asset.ID)
                            .Include(f => f.assetComponents.Select(ach => ach.assetComponentHistories))
                            .Include(h => h.assetHistories)
                            .Include(fa => fa.facility)
                            .FirstOrDefault();
                    if (assetObj != null)
                    {
                        OkForDelete = true;
                    }
                    else
                    {
                        delete_result += "Asset " + assetObj.Name + " does not exist in system";

                    }


                    // delete from database
                    if (OkForDelete)
                    {
                        FacilityAsset fa = ctx.FacilityAsset.Where(fas => fas.AssetID == assetObj.ID).FirstOrDefault();//new FacilityAsset();

                        ctx.FacilityAsset.Remove(fa);
                        ctx.SaveChanges();

                        ctx.Asset.Remove(assetObj);
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
        public static String updateAsset(Asset asset)
        {

            String update_result = "";
            bool OkForUpdate = false;
            using (var ctx = new CPPDbContext())
            {
                try
                {
                    // query to check existence of object in database

                    Asset assetObj = ctx.Asset.Where(a => a.ID == asset.ID).FirstOrDefault();
                    // check the database
                    if (assetObj != null)
                    {
                        OkForUpdate = true;
                    }
                    else
                    {
                        update_result += "Asset " + assetObj.Name+ " does not exist in system";

                    }

                    // update database entry
                    if (OkForUpdate)
                    {
                        using (var dbCtx = new CPPDbContext())
                        {
                            assetObj = asset;
                            dbCtx.Entry(assetObj).State = System.Data.Entity.EntityState.Modified;
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