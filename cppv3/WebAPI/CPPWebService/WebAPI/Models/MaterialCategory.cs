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
    [Table("material_category")]
    public class MaterialCategory : Audit
    {
        [NotMapped]
        public int Operation;

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public String UniqueIdentityNumber { get; set; }
        //public DateTime CreatedDate { get; set; }
        //public DateTime UpdatedDate { get; set; }
        //public String CreatedBy { get; set; }
        //public String UpdatedBy { get; set; }


        public static List<MaterialCategory> getMaterialCategory()
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);

            List<MaterialCategory> materialCategoryList = new List<MaterialCategory>();
            try
            {

                using (var ctx = new CPPDbContext())
                {
                    materialCategoryList = ctx.MaterialCategory.OrderBy(a => a.Name).ToList();
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

            return materialCategoryList;
        }


        public static String registerMaterialCategory(MaterialCategory materialCategory)
        {

            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String result = "";
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    MaterialCategory retrievedMaterialCategory = new MaterialCategory();
                    retrievedMaterialCategory = ctx.MaterialCategory.Where(u => u.Name == materialCategory.Name).FirstOrDefault();

                    if (retrievedMaterialCategory == null)
                    {
                        //register
                        ctx.MaterialCategory.Add(materialCategory);
                        ctx.SaveChanges();
                        //result = "Success";
                        result += materialCategory.Name + " has been created successfully.\n";
                    }
                    else
                    {
                        result += materialCategory.Name +  " failed to be created, it already exist.\n";
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
        public static String updateMaterialCategory(MaterialCategory materialCategory)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String result = "";

            try
            {
                using (var ctx = new CPPDbContext())
                {
                    MaterialCategory retrievedMaterialCategory = new MaterialCategory();
                    retrievedMaterialCategory = ctx.MaterialCategory.Where(u => u.ID == materialCategory.ID).FirstOrDefault();

                    MaterialCategory duplicateMaterialCategory = new MaterialCategory();
                    duplicateMaterialCategory = ctx.MaterialCategory.Where(u => u.Name == materialCategory.Name
                                                                                && u.ID != materialCategory.ID).FirstOrDefault();

                    if (duplicateMaterialCategory != null)
                    {
                        result += materialCategory.Name + " failed to be updated, duplicate will be created.\n";
                    }
                    else if (retrievedMaterialCategory != null)
                    {
                        CopyUtil.CopyFields<MaterialCategory>(materialCategory, retrievedMaterialCategory);
                        ctx.Entry(retrievedMaterialCategory).State = System.Data.Entity.EntityState.Modified;
                        ctx.SaveChanges();
                        result = materialCategory.Name + " has been updated successfully.\n";
                    }
                    else
                    {
                        result = materialCategory.Name + " failed to be updated, it does not exist.\n";
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
        public static String deleteMaterialCategory(MaterialCategory materialCategory)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String result = "";
            try
            {

                using (var ctx = new CPPDbContext())
                {
                    MaterialCategory retrievedMaterialCategory = new MaterialCategory();
                    retrievedMaterialCategory = ctx.MaterialCategory.Where(u => u.ID == materialCategory.ID).FirstOrDefault();

                    if (retrievedMaterialCategory != null)
                    {
                        ctx.MaterialCategory.Remove(retrievedMaterialCategory);
                        ctx.SaveChanges();
                        result = materialCategory.Name + " has been deleted successfully.\n";
                    }
                    else
                    {
                        result = materialCategory.Name + " failed to be deleted, it does not exist.\n";
                    }
                }

            }
            catch (Exception ex)
            {
                result = materialCategory.Name + " failed to be deleted due to dependencies.\n";
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
					nextUniqueIdentityNumber = ctx.MaterialCategory.Max(u => u.UniqueIdentityNumber);
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