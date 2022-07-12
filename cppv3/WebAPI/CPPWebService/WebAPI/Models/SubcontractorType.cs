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
    [Table("subcontractor_type")]
    public class SubcontractorType : Audit
    {
        [NotMapped]
        public int Operation;



        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SubcontractorTypeID { get; set; }
        public String SubcontractorTypeName { get; set; }
        public String SubcontractorTypeDescription { get; set; }
        public String UniqueIdentityNumber { get; set; }
        //public String CreatedBy { get; set; }
        //public String LastUpdatedBy { get; set; }
        //public DateTime CreatedDate { get; set; }
        //public DateTime LastUpdatedDate { get; set; }

        public static List<SubcontractorType> GetSubcontractorType()
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);

            List<SubcontractorType> subcontractorTypeList = new List<SubcontractorType>();
            try
            {

                using (var ctx = new CPPDbContext())
                {
                    subcontractorTypeList = ctx.SubcontractorType.OrderBy(a => a.SubcontractorTypeName).ToList();
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

            return subcontractorTypeList;
        }


        public static String registerSubcontractorType(SubcontractorType subcontractorType)
        {

            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String result = "";
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    SubcontractorType retrievedSubcontractorType = new SubcontractorType();
                    retrievedSubcontractorType = ctx.SubcontractorType.Where(u => u.SubcontractorTypeName == subcontractorType.SubcontractorTypeName).FirstOrDefault();

                    if (retrievedSubcontractorType == null)
                    {
                        //register
                        ctx.SubcontractorType.Add(subcontractorType);
                        ctx.SaveChanges();
                        //result = "Success";
                        result += subcontractorType.SubcontractorTypeName + " has been created successfully.\n";
                    }
                    else
                    {
                        result += subcontractorType.SubcontractorTypeName + "' failed to be created, it already exist.\n";
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
        public static String updateSubcontractorType(SubcontractorType subcontractorType)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String result = "";

            try
            {
                using (var ctx = new CPPDbContext())
                {
                    SubcontractorType retrievedSubcontractorType = new SubcontractorType();
                    retrievedSubcontractorType = ctx.SubcontractorType.Where(u => u.SubcontractorTypeID == subcontractorType.SubcontractorTypeID).FirstOrDefault();

                    SubcontractorType duplicateSubcontractorType = new SubcontractorType();
                    duplicateSubcontractorType = ctx.SubcontractorType.Where(u => u.SubcontractorTypeName == subcontractorType.SubcontractorTypeName
                                                                                    && u.SubcontractorTypeID != subcontractorType.SubcontractorTypeID).FirstOrDefault();

                    if (duplicateSubcontractorType != null)
                    {
                        result = subcontractorType.SubcontractorTypeName + " failed to be updated, duplicate will be created.\n";
                    }
                    else if (retrievedSubcontractorType != null)
                    {
                        CopyUtil.CopyFields<SubcontractorType>(subcontractorType, retrievedSubcontractorType);
                        ctx.Entry(retrievedSubcontractorType).State = System.Data.Entity.EntityState.Modified;
                        ctx.SaveChanges();
                        result += subcontractorType.SubcontractorTypeName + " has been updated successfully.\n";
                    }
                    else
                    {
                        result += subcontractorType.SubcontractorTypeName + " failed to be updated, it does not exist.\n";
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
        public static String deleteSubcontractorType(SubcontractorType subcontractorType)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String result = "";
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    SubcontractorType retrievedSubcontractorType = new SubcontractorType();
                    retrievedSubcontractorType = ctx.SubcontractorType.Where(u => u.SubcontractorTypeID == subcontractorType.SubcontractorTypeID).FirstOrDefault();

                    if (retrievedSubcontractorType != null)
                    {
                        ctx.SubcontractorType.Remove(retrievedSubcontractorType);
                        ctx.SaveChanges();
                        result = subcontractorType.SubcontractorTypeName + " has been deleted successfully.\n";
                    }
                    else
                    {
                        result = subcontractorType.SubcontractorTypeName + " failed to be deleted, it does not exist.\n";
                    }
                }

            }
            catch (Exception ex)
            {
                result = subcontractorType.SubcontractorTypeName + " failed to be deleted due to dependencies.\n";
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
					nextUniqueIdentityNumber = ctx.SubcontractorType.Max(u => u.UniqueIdentityNumber);
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