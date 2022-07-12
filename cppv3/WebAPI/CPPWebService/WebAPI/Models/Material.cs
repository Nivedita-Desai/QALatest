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
    [Table("material")]
    public class Material : Audit
    {
        [NotMapped]
        public int Operation { get; set; }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public String Name { get; set; }
        public Double Cost { get; set; }
        public int UnitTypeID { get; set; }
        public int MaterialCategoryID { get; set; }
        public int? VendorID { get; set; }
        public String Description { get; set; }
        public String UniqueIdentityNumber { get; set; }
        public String PartNumber { get; set; }
        //public DateTime CreatedDate { get; set; }
        //public DateTime UpdatedDate { get; set; }
        //public String CreatedBy { get; set; }
        //public String UpdatedBy { get; set; }

        //ON DELETE RESTRICT
        [ForeignKey("UnitTypeID")]
        public virtual UnitType UnitType { get; set; }

        //ON DELETE RESTRICT
        [ForeignKey("MaterialCategoryID")]
        public virtual MaterialCategory MaterialCategory { get; set; }

        //ON DELETE RESTRICT
        [ForeignKey("VendorID")]
        public virtual Vendor Vendor { get; set; }


        public static List<Material> getMaterial()
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);

            List<Material> materialList = new List<Material>();
            try
            {

                using (var ctx = new CPPDbContext())
                {
                    materialList = ctx.Material.OrderBy(a => a.UniqueIdentityNumber).ToList();
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

            return materialList;
        }


        public static String registerMaterial(Material material)
        {

            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String result = "";
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    Material retrievedMaterial = new Material();
                    retrievedMaterial = ctx.Material.Where(u => u.Name == material.Name && u.MaterialCategoryID == material.MaterialCategoryID).FirstOrDefault();
            //        Material retrievedMaterial2 = new Material();
              //      retrievedMaterial2 = ctx.Material.Where(u => u.MaterialCategory == material.MaterialCategory).FirstOrDefault();

                    Material duplicateUniqueIdentityNumber = new Material();
                    duplicateUniqueIdentityNumber = ctx.Material.Where(u => u.UniqueIdentityNumber == material.UniqueIdentityNumber).FirstOrDefault();

                    if(duplicateUniqueIdentityNumber != null)
                    {
                        result += material.Name + "' failed to be created, duplicate unique identifier found.\n";
                    }
                    else if (retrievedMaterial == null)
                    {
                        //register
                        ctx.Material.Add(material);
                        ctx.SaveChanges();
                        //result = "Success";
                        result += material.Name + " has been created successfully.\n";
                    }
       //             else if (retrievedMaterial2 == null)
         //           {
                        //register
           //             ctx.Material.Add(material);
             //           ctx.SaveChanges();
               //         result = "Success";
                 //       result += material.Name + " has been created successfully.\n";
                  //  }
                    else
                    {
                        result += material.Name + "' failed to be created, it already exist.\n";
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
        public static String updateMaterial(Material material)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String result = "";

            try
            {
                using (var ctx = new CPPDbContext())
                {
                    Material retrievedMaterial = new Material();
                    retrievedMaterial = ctx.Material.Where(u => u.ID == material.ID).FirstOrDefault();

                    Material duplicateMaterial = new Material();
                    duplicateMaterial = ctx.Material.Where(u => u.Name == material.Name
                                                                && u.ID != material.ID && u.MaterialCategoryID == material.MaterialCategoryID).FirstOrDefault();

                    Material duplicateUniqueIdentityNumber = new Material();
                    duplicateUniqueIdentityNumber = ctx.Material.Where(u => u.UniqueIdentityNumber == material.UniqueIdentityNumber && u.ID != material.ID).FirstOrDefault();

                    if (duplicateMaterial != null)
                    {
                        result += material.Name + " failed to be updated, duplicate will be created.\n";
                    }
                    else if (duplicateUniqueIdentityNumber != null)
                    {
                        result += material.Name + "' failed to be created, duplicate unique identifier found.\n";
                    }
                    else if (retrievedMaterial != null)
                    {
                        CopyUtil.CopyFields<Material>(material, retrievedMaterial);
                        ctx.Entry(retrievedMaterial).State = System.Data.Entity.EntityState.Modified;
                        ctx.SaveChanges();
                        result += material.Name + " has been updated successfully.\n";
                    }
                    else
                    {
                        result += material.Name + " failed to be updated, it does not exist.\n";
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
        public static String deleteMaterial(Material material)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String result = "";
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    Material retrievedMaterial = new Material();
                    retrievedMaterial = ctx.Material.Where(u => u.ID == material.ID).FirstOrDefault();
                    if (retrievedMaterial != null)
                    {
                        //delete
                        List<Material> materialList = new List<Material>();
                        materialList = ctx.Material.Where(cu => cu.ID == retrievedMaterial.ID).ToList();

                        ctx.Material.Remove(retrievedMaterial);
                        ctx.SaveChanges();
                        result += material.Name + " has been deleted successfully.\n";
                    }
                    else
                    {
                        result += material.Name + " failed to be deleted, it does not exist.\n";
                    }
                }

            }
            catch (Exception ex)
            {
                result += material.Name + " failed to be deleted due to dependencies";
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
                    nextUniqueIdentityNumber = ctx.Material.Max(u => u.UniqueIdentityNumber);
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