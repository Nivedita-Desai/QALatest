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
    [Table("facility")]
    public class Facility
    {
        [NotMapped]
        public int Operation;

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public String Location { get; set; }
        public String Name { get; set; }
        public String Note { get; set; }


        //Navigation Property-enable lazy loading
        public virtual ICollection<FacilityAsset> facilityAssets{ get; set; }

        [NotMapped]
        public List<Asset> assetList;

        //update all facilities
        public static String updateAllFacilities(List<Facility> facilityList)
        {
            foreach (var facility in facilityList)
            {
                if (facility.Operation == 1)
                {
                    //Create
                    Facility.registerFacility(facility);
                    foreach (var fa in facility.facilityAssets)
                    {
                        if (fa.Asset.Operation == 1)
                        {
                            //Create new asset
                            Asset.registerAsset(fa.Asset);  
                            //create asset component
                            if (fa.Asset.assetComponents.Count() != 0)
                            {
                                //register asset Component
                                foreach(var asComp in fa.Asset.assetComponents){
                                    AssetComponent.registerAssetComponent(asComp);
                                    if (asComp.assetComponentHistories.Count != 0)
                                    {
                                        //register asset Component History
                                        foreach (var asCompHist in asComp.assetComponentHistories)
                                        {
                                            AssetComponentHistory.registerAssetComponentHistory(asCompHist);
                                        }
                                    }
                                }
                            }
                            //Create Asset history
                            if (fa.Asset.assetHistories.Count() != 0)
                            {
                                foreach (var asHist in fa.Asset.assetHistories)
                                {
                                    AssetHistory.registerAssetHistory(asHist);
                                }
                            }
                        }
                    }
                }
                else if(facility.Operation == 2)
                {//Update
                    Facility.updateFacility(facility);
                    foreach (var fa in facility.facilityAssets)
                    {
                        if (fa.Asset.Operation == 1)
                        {
                            //Create new Asset
                        }
                        else if (fa.Asset.Operation == 2)
                        {

                        }
                        else if (fa.Asset.Operation == 3)
                        {

                        }
                    }
                }
                else if (facility.Operation == 3)
                {//Delete

                }
            }
            return "";
        }
        //Get all Facilities
        public static List<Facility> getAllFacilities()
        {

            List<Facility> facilityList = new List<Facility>();
       
            using (var ctx = new CPPDbContext())
            {
                try
                {

                    facilityList = ctx.Facility.Include(fa=>fa.facilityAssets).ToList();
                    foreach (var facility in facilityList)
                    {

                        foreach (var item in facility.facilityAssets)
                        {
                            item.Asset = ctx.Asset
                                            .Include(ar => ar.assetSupplier)
                                            .Include(f => f.assetComponents.Select(ach => ach.assetComponentHistories))
                                            .Include(h => h.assetHistories)
                                            .Where(i => i.ID == item.AssetID).FirstOrDefault();
                        }
                    }

                }
                catch (Exception ex)
                {
                    var stackTrace = new StackTrace(ex, true);
                    var line = stackTrace.GetFrame(0).GetFileLineNumber();
                    Logger.LogExceptions(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, ex.Message, line.ToString(), Logger.logLevel.Exception);

                }
            }

            return facilityList;
        }
        public static Facility testGetMany(int id)
        {

            Facility facilityList = new Facility();
            Facility copy = new Facility();
            using(var ctx = new CPPDbContext()){
                try
                {
                   
                    List<Asset> assetList = new List<Asset>();
                    //facilityList = ctx.Facility.Include("facilityAssets.Asset").
                    //                           Include("facilityAssets.Asset.assetHistories").
                    //                            Include("facilityAssets.Asset.assetComponents").Where(c => c.ID == id).FirstOrDefault();

                    facilityList = ctx.Facility.
                                Include(f => f.facilityAssets.Select(a=>a.Asset)).Where(c => c.ID == id).FirstOrDefault();
                    //copy = facilityList;

                    foreach (var item in facilityList.facilityAssets)
                    {
                        item.Asset = ctx.Asset
                                        .Include(ar => ar.assetSupplier)
                                        .Include(f => f.assetComponents.Select(ach => ach.assetComponentHistories))
                                        .Include(h => h.assetHistories)
                                        .Where(i=>i.ID == item.ID).FirstOrDefault();
                    }
                }
                catch (Exception ex)
                {
                    var stackTrace = new StackTrace(ex, true);
                    var line = stackTrace.GetFrame(0).GetFileLineNumber();
                    Logger.LogExceptions(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, ex.Message, line.ToString(), Logger.logLevel.Exception);
         
                }
            }

            return facilityList;
        }
        public static String testManyToMany(int id){
             String status = "";
             Facility facility1 = new Facility();
             Asset asset1 = new Asset();
             Asset asset2 = new Asset();
             facility1.Name = "CPP";
             facility1.Location = "California";
             facility1.Note = "Just a note";

             asset1.Cost = 2000;
             asset1.Name = "DELL PC";
             asset1.Note = "Too expensive";
             asset1.Tag = "OC";
             asset1.Status = "new";
             asset1.AcquisitionDate = new DateTime();
             asset1.EOL = "WTF";

             asset2.Cost = 4000;
             asset2.Name = "Alienware PC";
             asset2.Note = "Too cheap";
             asset2.Tag = "PC";
             asset2.Status = "used";
             asset2.AcquisitionDate = new DateTime();
             asset2.EOL = "WTF";


             //facility1.Assets.Add(asset1);
             //facility1.Assets.Add(asset2);
             using (var ctx = new CPPDbContext())
             {
                 try
                 {
                    // Asset asset1 = ctx.Asset.Where(a => a.ID == 4).FirstOrDefault();
                     //Asset asset2 = ctx.Asset.Where(a => a.ID == 5).FirstOrDefault();

                     FacilityAsset FA1 = new FacilityAsset();
                     FA1.Asset = asset1;
                     FA1.Facility = facility1;
                     FacilityAsset FA2 = new FacilityAsset();
                     FA2.Asset = asset2;
                     FA2.Facility = facility1;


                     ctx.FacilityAsset.Add(FA1);
                     ctx.FacilityAsset.Add(FA2);
                     ctx.SaveChanges();
                     status = "Success";
                 }
                 catch (Exception ex)
                 {
                     var stackTrace = new StackTrace(ex, true);
                     var line = stackTrace.GetFrame(0).GetFileLineNumber();
                     Logger.LogExceptions(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, ex.Message, line.ToString(), Logger.logLevel.Exception);
           
                 }
             }


            return status;
        }
        public static List<Facility> getFacilityByID(int FacilityID)
        {
            List<Facility> MatchedFacilityList = new List<Facility>();

            using (var ctx = new CPPDbContext())
            {
                try
                {
                    
                    MatchedFacilityList = ctx.Facility.Where(a => a.ID == FacilityID)
                                                      .Include(fa=>fa.facilityAssets).ToList()
                                                      .ToList();
                    foreach (var facility in MatchedFacilityList)
                    {
                        foreach (var item in facility.facilityAssets)
                        {
                            item.Asset = ctx.Asset
                                                .Include(ar => ar.assetSupplier)
                                                .Include(f => f.assetComponents.Select(ach => ach.assetComponentHistories))
                                                .Include(h => h.assetHistories)
                                                .Where(i => i.ID == item.AssetID).FirstOrDefault();
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
            }

            return MatchedFacilityList;
        }

        public static String registerFacility(Facility facility)
        {
            String status = "";
            using (var ctx = new CPPDbContext())
            {

                try
                {
                    ctx.Facility.Add(facility);
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
        public static String deleteFacility(Facility facility)
        {

            String delete_result = "";
            bool OkForDelete = false;
            using (var ctx = new CPPDbContext())
            {
                try
                {
                    // query to check existence of object in database
                    Facility facilityObj = ctx.Facility.Where(f => f.ID == facility.ID).FirstOrDefault();
                    if (facilityObj != null)
                    {
                        OkForDelete = true;
                    }
                    else
                    {
                        delete_result += "Facility " + facilityObj.Name + " does not exist in system";

                    }


                    // delete from database
                    if (OkForDelete)
                    {
                        ctx.Facility.Remove(facilityObj);
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
        public static String updateFacility(Facility facility)
        {

            String update_result = "";
            bool OkForUpdate = false;
            using (var ctx = new CPPDbContext())
            {
                try
                {
                    // query to check existence of object in database

                    Facility facilityObj = ctx.Facility.Where(f => f.ID == facility.ID).FirstOrDefault();
                    // check the database
                    if (facilityObj != null)
                    {
                        OkForUpdate = true;
                    }
                    else
                    {
                        update_result += "Facility " + facility.Name + " does not exist in system";

                    }

                    // update database entry
                    if (OkForUpdate)
                    {
                        using (var dbCtx = new CPPDbContext())
                        {
                            facilityObj = facility;
                            dbCtx.Entry(facilityObj).State = System.Data.Entity.EntityState.Modified;
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