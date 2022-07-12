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
    [Table("location")]
    public class Location : Audit
    {
        [NotMapped]
        public int Operation;

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LocationID { get; set; }
        public String LocationName { get; set; }
        public String AddressLine { get; set; }
        public String City { get; set; }
        public String State { get; set; }
        public String ZipCode { get; set; }
        public String Country { get; set; }
        public String LocationDescription { get; set; }
        public String UniqueIdentityNumber { get; set; }
        //public DateTime CreatedDate { get; set; }
        //public DateTime UpdatedDate { get; set; }
        //public String CreatedBy { get; set; }
        //public String UpdatedBy { get; set; }

        public static List<Location> getLocation()
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);

            List<Location> locationList = new List<Location>();
            try
            {

                using (var ctx = new CPPDbContext())
                {
                    locationList = ctx.Location.OrderBy(a => a.LocationName).ToList();
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

            return locationList;
        }


        public static String registerLocation(Location location)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String result = "";
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    Location retrievedLocation = new Location();
                    retrievedLocation = ctx.Location.Where(u => u.LocationName == location.LocationName).FirstOrDefault();

                   // Location duplicateUniqueIdentityNumber = new Location();
                   // duplicateUniqueIdentityNumber = ctx.Location.Where(u => u.UniqueIdentityNumber == location.UniqueIdentityNumber).FirstOrDefault();

                   // if (duplicateUniqueIdentityNumber != null)
                   // {
                   //      result += location.LocationName + " failed to be created, duplicate unique identifier found.\n";
                   // }
                    if (retrievedLocation == null)
                    {
                        //register
                        ctx.Location.Add(location);
                        ctx.SaveChanges();
                        result = location.LocationName + " has been created successfully.\n";
                    }
                    else
                    {
                        result += location.LocationName + "' failed to be created, it already exist.\n";
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

        public static String updateLocation(Location location)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String result = "";

            try
            {
                using (var ctx = new CPPDbContext())
                {
                    Location retrievedLocation = new Location();
                    retrievedLocation = ctx.Location.Where(u => u.LocationID == location.LocationID).FirstOrDefault();

                    Location duplicateLocation = new Location();
                    duplicateLocation = ctx.Location.Where(u => u.LocationName == location.LocationName
                                                            && u.LocationID != location.LocationID).FirstOrDefault();

                   // Location duplicateUniqueIdentityNumber = new Location();
                   // duplicateUniqueIdentityNumber = ctx.Location.Where(u => u.UniqueIdentityNumber == location.UniqueIdentityNumber && u.LocationID != location.LocationID).FirstOrDefault();

                    if (duplicateLocation != null)
                    {
                        result += location.LocationName + " failed to be updated, duplicate will be created.\n";
                    }
                  //  else if (duplicateUniqueIdentityNumber != null)
                   // {
                   //     result += location.LocationName + "' failed to be created, duplicate unique identifier found.\n";
                  //  }
                    else if (retrievedLocation != null)
                    {
                        CopyUtil.CopyFields<Location>(location, retrievedLocation);
                        ctx.Entry(retrievedLocation).State = System.Data.Entity.EntityState.Modified;
                        ctx.SaveChanges();
                        result += location.LocationName + " has been updated successfully.\n";
                    }
                    else
                    {
                        result += location.LocationName + " failed to be updated, it does not exist.\n";
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
        public static String deleteLocation(Location location)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String result = "";
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    Location retrievedLocation = new Location();
                    retrievedLocation = ctx.Location.Where(u => u.LocationName == location.LocationName).FirstOrDefault();

                    if (retrievedLocation != null)
                    {
                        ctx.Location.Remove(retrievedLocation);
                        ctx.SaveChanges();
                        result = location.LocationName + " has been deleted successfully.\n";
                    }
                    else
                    {
                        result = location.LocationName + " failed to be deleted, it does not exist.\n";
                    }
                }

            }
            catch (Exception ex)
            {
                result = location.LocationName + " failed to be deleted due to dependencies.\n";
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
                    nextUniqueIdentityNumber = ctx.Location.Max(u => u.UniqueIdentityNumber);
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