using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.Reflection;
using System.Diagnostics;
using System.Data.OleDb;
using WebAPI.Controllers;
using MySql.Data.MySqlClient;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WebAPI.Helper;

namespace WebAPI.Models
{
    [Table("userlicensemapping")] 
    public class User_LicenseMapping
    {
        [NotMapped]
        public int Operation;

        [NotMapped]
        public String userName;

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        public Int32 userId { get; set; }

        public string licenseKey { get; set; }

        public string productId { get; set; }
        public DateTime expirationDate { get; set; }
        public string licenseStatus { get; set; }

        public static List<User_LicenseMapping> GetUserAllLicenseMappings()
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);

            List<User_LicenseMapping> User_LicenseMapping = new List<User_LicenseMapping>();
            try
            {

                using (var ctx = new CPPDbContext())
                {
                    User_LicenseMapping = ctx.User_LicenseMapping.Where(a => a.expirationDate.Date > DateTime.Now.Date).OrderBy(a => a.id).ToList();
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

            return User_LicenseMapping;
        }

        public static String registerUser_LicenseMapping(User_LicenseMapping user_LicenseMapping)
        {

            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String result = "";
            try
            {
                using (var ctx = new CPPDbContext())
                {

                    User userId = ctx.User.Where(u => u.UserID == user_LicenseMapping.userName).FirstOrDefault();
                    user_LicenseMapping.userId = userId.Id;
                    User_LicenseMapping retreivedUserLicenseMapping = new User_LicenseMapping();
                    retreivedUserLicenseMapping = ctx.User_LicenseMapping.Where(u => (u.userId == user_LicenseMapping.userId
                                                                                  && u.productId == user_LicenseMapping.productId
                                                                                  && u.licenseKey == user_LicenseMapping.licenseKey)).FirstOrDefault();

                    if (retreivedUserLicenseMapping == null)
                    {
                        //register
                        ctx.User_LicenseMapping.Add(user_LicenseMapping);
                        ctx.SaveChanges();

                        result += user_LicenseMapping.licenseKey  + " has been created successfully. \n";
                    }
                    else
                    {
                        result += user_LicenseMapping.licenseKey + " failed to be created, entry must be unique. \n";
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



        public static String updateUser_LicenseMapping(User_LicenseMapping user_LicenseMapping)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String result = "";



            try
            {
                using (var ctx = new CPPDbContext())
                {
                    User userId = ctx.User.Where(u => u.UserID == user_LicenseMapping.userName).FirstOrDefault();
                    user_LicenseMapping.userId = userId.Id;
                    User_LicenseMapping retreivedUser_LicenseMapping = new User_LicenseMapping();
                    retreivedUser_LicenseMapping = ctx.User_LicenseMapping.Where(u => u.userId == user_LicenseMapping.userId
                    && u.licenseKey == user_LicenseMapping.licenseKey).FirstOrDefault();
                    ServiceClass sc = new ServiceClass();

                    User_LicenseMapping duplicateUser_LicenseMapping = ctx.User_LicenseMapping.Where(a => (a.userId == user_LicenseMapping.userId
                                                                                  && a.licenseKey == user_LicenseMapping.licenseKey   
                                                                                  && a.licenseStatus == user_LicenseMapping.licenseStatus
                                                                                  )).FirstOrDefault();
                   


                    if (duplicateUser_LicenseMapping != null)
                    {
                        result += user_LicenseMapping.licenseKey + " failed to be updated, non-unique entry will be created.\n";
                    }
                    else if (retreivedUser_LicenseMapping != null)
                    {
                        if (retreivedUser_LicenseMapping.expirationDate.Date > DateTime.Now.Date)
                        {
                            retreivedUser_LicenseMapping.licenseKey = user_LicenseMapping.licenseKey;
                            retreivedUser_LicenseMapping.licenseStatus = user_LicenseMapping.licenseStatus;
                            if (user_LicenseMapping.expirationDate != null)
                                retreivedUser_LicenseMapping.expirationDate = user_LicenseMapping.expirationDate;
                            ctx.SaveChanges();

                            result += user_LicenseMapping.licenseKey + " has been updated successfully.\n";
                        }
                        else
                        {
                            retreivedUser_LicenseMapping.licenseStatus = "LICENSE_EXPIRED";
                            ctx.SaveChanges();
                            result += user_LicenseMapping.licenseKey + " license Key expired.\n";
                        }
                    }
                    else
                    {
                        result += user_LicenseMapping.licenseKey + " failed to be updated, it does not exist.\n";
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
        public static String deleteUser_LicenseMapping(User_LicenseMapping user_LicenseMapping)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String result = "";
            String entryName = "";
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    User_LicenseMapping retreivedUser_LicenseMapping = new User_LicenseMapping();
                    retreivedUser_LicenseMapping = ctx.User_LicenseMapping.Where(u => u.id == user_LicenseMapping.id).FirstOrDefault();

                  
                    if (retreivedUser_LicenseMapping != null)
                    {
                        ctx.User_LicenseMapping.Remove(retreivedUser_LicenseMapping);
                        ctx.SaveChanges();
                        result = user_LicenseMapping.licenseKey + " has been deleted successfully.\n";
                    }
                    else
                    {
                        result = user_LicenseMapping.licenseKey + " failed to be updated, it does not exist.\n";
                    }
                }

            }
            catch (Exception ex)
            {
                result = ex.Message;
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

        public static List<User_LicenseMapping> GetUserLicenseMappingById(String userName)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);

            List<User_LicenseMapping> User_LicenseMapping = new List<User_LicenseMapping>();
            User_LicenseMapping userMapping = new User_LicenseMapping();
            try
            {

                using (var ctx = new CPPDbContext())
                {
                    User userId = ctx.User.Where(u => u.UserID == userName).FirstOrDefault();
                    Int32 userID = userId.Id;
                    if (userId.Role != "Admin")
                    {
                        Employee emp = ctx.Employee.Where(e => e.ID == userId.EmployeeID).FirstOrDefault();
                        userID = ctx.User.Where(u => u.UserID == emp.CreatedBy).FirstOrDefault().Id;
                    }
                    userMapping = ctx.User_LicenseMapping.Where(l => l.userId == userID && l.licenseStatus == "LICENSE_VALID").FirstOrDefault();
                    if (userMapping != null)
                    {
                        if (userMapping.expirationDate.Date < DateTime.Now.Date)
                        {
                            userMapping.licenseStatus = "LICENSE_EXPIRED";
                            ctx.SaveChanges();
                        }
                    }

                    User_LicenseMapping = ctx.User_LicenseMapping.Where(l => l.userId == userID && l.licenseStatus == "LICENSE_VALID").
                        OrderByDescending(u =>u.id).ToList();

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

            return User_LicenseMapping;
        }
    }

    
}