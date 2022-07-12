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
    [Table("manufacturer")]
    public class Manufacturer : Audit
    {
        [NotMapped]
        public int Operation;

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ManufacturerID { get; set; }
        public String ManufacturerName { get; set; }
        public String ManufacturerDescription { get; set; }
        //public DateTime CreatedDate { get; set; }
        //public DateTime UpdatedDate { get; set; }
        //public String CreatedBy { get; set; }
        //public String UpdatedBy { get; set; }


        public static List<Manufacturer> getManufacturer()
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);

            List<Manufacturer> ManufacturerList = new List<Manufacturer>();
            try
            {

                using (var ctx = new CPPDbContext())
                {
                    ManufacturerList = ctx.Manufacturer.OrderBy(a => a.ManufacturerName).ToList();
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

            return ManufacturerList;
        }


        public static String registerManufacturer(Manufacturer Manufacturer)
        {

            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String result = "";
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    Manufacturer retrievedManufacturer = new Manufacturer();
                    retrievedManufacturer = ctx.Manufacturer.Where(u => u.ManufacturerName == Manufacturer.ManufacturerName).FirstOrDefault();

                    if (retrievedManufacturer == null)
                    {
                        //register
                        ctx.Manufacturer.Add(Manufacturer);
                        ctx.SaveChanges();
                        //result = "Success";
                        result += Manufacturer.ManufacturerName + " has been created successfully.\n";
                    }
                    else
                    {
                        result += Manufacturer.ManufacturerName + " failed to be created, it already exist.\n";
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
        public static String updateManufacturer(Manufacturer Manufacturer)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String result = "";

            try
            {
                using (var ctx = new CPPDbContext())
                {
                    Manufacturer retrievedManufacturer = new Manufacturer();
                    retrievedManufacturer = ctx.Manufacturer.Where(u => u.ManufacturerID == Manufacturer.ManufacturerID).FirstOrDefault();

                    Manufacturer duplicateManufacturer = new Manufacturer();
                    duplicateManufacturer = ctx.Manufacturer.Where(u => u.ManufacturerName == Manufacturer.ManufacturerName
                                                                                && u.ManufacturerID != Manufacturer.ManufacturerID).FirstOrDefault();

                    if (duplicateManufacturer != null)
                    {
                        result += Manufacturer.ManufacturerName + " failed to be updated, duplicate will be created.\n";
                    }
                    else if (retrievedManufacturer != null)
                    {
                        CopyUtil.CopyFields<Manufacturer>(Manufacturer, retrievedManufacturer);
                        ctx.Entry(retrievedManufacturer).State = System.Data.Entity.EntityState.Modified;
                        ctx.SaveChanges();
                        result = Manufacturer.ManufacturerName + " has been updated successfully.\n";
                    }
                    else
                    {
                        result = Manufacturer.ManufacturerName + " failed to be updated, it does not exist.\n";
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
        public static String deleteManufacturer(Manufacturer Manufacturer)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String result = "";
            try
            {

                using (var ctx = new CPPDbContext())
                {
                    Manufacturer retrievedManufacturer = new Manufacturer();
                    retrievedManufacturer = ctx.Manufacturer.Where(u => u.ManufacturerID == Manufacturer.ManufacturerID).FirstOrDefault();

                    if (retrievedManufacturer != null)
                    {
                        ctx.Manufacturer.Remove(retrievedManufacturer);
                        ctx.SaveChanges();
                        result = Manufacturer.ManufacturerName + " has been deleted successfully.\n";
                    }
                    else
                    {
                        result = Manufacturer.ManufacturerName + " failed to be deleted, it does not exist.\n";
                    }
                }

            }
            catch (Exception ex)
            {
                result = Manufacturer.ManufacturerName + " failed to be deleted due to dependencies.\n";
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