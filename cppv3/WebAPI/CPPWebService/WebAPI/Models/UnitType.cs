using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Web;
using WebAPI.Controllers;
using WebAPI.Helper;

namespace WebAPI.Models
{
    [Table("unit_type")]
    public class UnitType : Audit
    {
        [NotMapped]
        public int Operation;



        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UnitID { get; set; }
        public String UnitName { get; set; }
        public String UnitAbbr { get; set; }
        
        public static List<UnitType> getUnitType()
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);

            List<UnitType> unitTypeList = new List<UnitType>();
            try
            {

                using (var ctx = new CPPDbContext())
                {
                    unitTypeList = ctx.UnitType.OrderBy(a=>a.UnitName).ToList();
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

            return unitTypeList;
        }


        public static String registerUnitType(UnitType unitType)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String result = "";
            try
            {
                using(var ctx = new CPPDbContext()){
                    UnitType retrievedUnitType = new UnitType();
                    retrievedUnitType = ctx.UnitType.Where(u => u.UnitName == unitType.UnitName).FirstOrDefault();
                    
                    if(retrievedUnitType == null){
                        //register
                        ctx.UnitType.Add(unitType);
                        ctx.SaveChanges();
                        result += unitType.UnitName + " has been created successfully.\n";
                    }
                    else
                    {
                        result += unitType.UnitName + " failed to be created, it already exist.\n";
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
        public static String updateUnitType(UnitType unitType)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String result = "";

            try
            {
                using (var ctx = new CPPDbContext())
                {
                    UnitType retrievedUnitType = new UnitType();
                    retrievedUnitType = ctx.UnitType.Where(u => u.UnitID == unitType.UnitID).FirstOrDefault();

                    UnitType duplicateUnitType = new UnitType();
                    duplicateUnitType = ctx.UnitType.Where(u => u.UnitName == unitType.UnitName
                                                                && u.UnitID != unitType.UnitID).FirstOrDefault();

                    if (duplicateUnitType != null)
                    {
                        result = unitType.UnitName + " failed to be updated, duplicate will be created.\n";
                    }
                    else if (retrievedUnitType != null)
                    {
                        CopyUtil.CopyFields<UnitType>(unitType, retrievedUnitType);
                        ctx.Entry(retrievedUnitType).State = System.Data.Entity.EntityState.Modified;
                        ctx.SaveChanges();
                        result += unitType.UnitName + " has been updated successfully.\n";
                    }
                    else
                    {
                        result += unitType.UnitName + " failed to be updated, it does not exist.\n";
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
        public static String deleteUnitType(UnitType unitType)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String result = "";
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    UnitType retrievedUnitType = new UnitType();
                    retrievedUnitType = ctx.UnitType.Where(u => u.UnitID == unitType.UnitID).FirstOrDefault();
                    if (retrievedUnitType != null)
                    {
                        ctx.UnitType.Remove(retrievedUnitType);
                        ctx.SaveChanges();
                        result = unitType.UnitName + " has been deleted successfully.\n";
                    }
                    else
                    {
                        result = unitType.UnitName + " failed to be deleted, it does not exist.\n";
                    }
                }
                
            }
            catch (Exception ex)
            {
                result = unitType.UnitName + " failed to be deleted due to dependencies.\n";
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