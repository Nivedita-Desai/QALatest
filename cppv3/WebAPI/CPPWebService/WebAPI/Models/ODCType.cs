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
using WebAPI.Models;

namespace WebAPI.Models
{
    [Table("odc_type")]
    public class ODCType : Audit
    {
        [NotMapped]
        public int Operation;



        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ODCTypeID { get; set; }
        public String ODCTypeName { get; set; }
        public String ODCDescription { get; set; }
        public String UniqueIdentityNumber { get; set; }
        //public String CreatedBy { get; set; }
        //public String LastUpdatedBy { get; set; }
        //public DateTime CreatedDate { get; set; }
        //public DateTime LastUpdatedDate { get; set; }

        public static List<ODCType> GetODCType()
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);

            List<ODCType> odcTypeList = new List<ODCType>();
            try
            {

                using (var ctx = new CPPDbContext())
                {
                    odcTypeList = ctx.ODCType.OrderBy(a => a.UniqueIdentityNumber).ToList();
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

            return odcTypeList;
        }


        public static String registerODCType(ODCType odcType)
        {

            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String result = "";
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    ODCType retrievedODCType = new ODCType();
                    retrievedODCType = ctx.ODCType.Where(u => u.ODCTypeName == odcType.ODCTypeName).FirstOrDefault();

                    ODCType duplicateUniqueIdentityNumber = new ODCType();
                    duplicateUniqueIdentityNumber = ctx.ODCType.Where(u => u.UniqueIdentityNumber == odcType.UniqueIdentityNumber).FirstOrDefault();

                    if (duplicateUniqueIdentityNumber != null)
                    {
                        result += odcType.ODCTypeName + "' failed to be created, duplicate unique identifier found.\n";
                    }
                    else if (retrievedODCType == null)
                    {
                        //register
                        ctx.ODCType.Add(odcType);
                        ctx.SaveChanges();
                        result += odcType.ODCTypeName + " has been created successfully.\n";
                    }
                    else
                    {
                        result += odcType.ODCTypeName + " failed to be created, duplicate type.\n";
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
        public static String updateODCType(ODCType odcType)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String result = "";

            try
            {
                using (var ctx = new CPPDbContext())
                {
                    ODCType retrievedODCType = new ODCType();
                    retrievedODCType = ctx.ODCType.Where(u => u.ODCTypeID == odcType.ODCTypeID).FirstOrDefault();

                    ODCType duplicateODCType = new ODCType();
                    duplicateODCType = ctx.ODCType.Where(u => (u.ODCTypeName == odcType.ODCTypeName)
                                                                && u.ODCTypeID != odcType.ODCTypeID).FirstOrDefault();

                    ODCType duplicateUniqueIdentityNumber = new ODCType();
                    duplicateUniqueIdentityNumber = ctx.ODCType.Where(u => u.UniqueIdentityNumber == odcType.UniqueIdentityNumber && u.ODCTypeID != odcType.ODCTypeID).FirstOrDefault();

                    if (duplicateODCType != null)
                    {
                        result += odcType.ODCTypeName + " failed to be updated, duplicate type will be created.\n";
                    }
                    else if (duplicateUniqueIdentityNumber != null)
                    {
                        result += odcType.ODCTypeName + "' failed to be created, duplicate unique identifier found.\n";
                    }
                    else if (retrievedODCType != null)
                    {
                        CopyUtil.CopyFields<ODCType>(odcType, retrievedODCType);
                        ctx.Entry(retrievedODCType).State = System.Data.Entity.EntityState.Modified;
                        ctx.SaveChanges();
                        result += odcType.ODCTypeName + " has been updated successfully.\n";
                    }
                    else
                    {
                        result += odcType.ODCTypeName + " failed to be updated, it does not exist.\n";
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
        public static String deleteODCType(ODCType odcType)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String result = "";
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    ODCType retrievedODCType = new ODCType();
                    retrievedODCType = ctx.ODCType.Where(u => u.ODCTypeID== odcType.ODCTypeID).FirstOrDefault();

                    if (retrievedODCType != null)
                    {
                        ctx.ODCType.Remove(retrievedODCType);
                        ctx.SaveChanges();
                        result += odcType.ODCTypeName + " has been deleted successfully.\n";
                    }
                    else
                    {
                        result += odcType.ODCTypeName + " failed to be deleted, it does not exist.\n";
                    }
                }

            }
            catch (Exception ex)
            {
                result += odcType.ODCTypeName + " failed to be deleted due to dependencies.\n";
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
					nextUniqueIdentityNumber = ctx.ODCType.Max(u => u.UniqueIdentityNumber);
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