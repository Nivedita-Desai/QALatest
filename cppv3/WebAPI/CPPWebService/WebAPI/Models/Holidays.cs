using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;
using WebAPI.Helper;

namespace WebAPI.Models
{
    [Table("holidays")]
    public class Holidays : Audit
    {
        [NotMapped]
        public int Operation;

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public String Description { get; set; }
        public String Type_Of_Holiday { get; set; }

        public DateTime HolidayDate { get; set; }

        [NotMapped]
        public DateTime HolidayDateForPreviousYear { get; set; }
        //public static List<Holidays> getHolidays()
        //{
        //    Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
        //    Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);

        //    List<Holidays> holidaysList = new List<Holidays>();
        //    try
        //    {

        //        using (var ctx = new CPPDbContext())
        //        {

        //            holidaysList = ctx.Holidays.OrderBy(a => a.ID).ToList();

        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        var stackTrace = new StackTrace(ex, true);
        //        var line = stackTrace.GetFrame(0).GetFileLineNumber();
        //        Logger.LogExceptions(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, ex.Message, line.ToString(), Logger.logLevel.Exception);
        //    }
        //    finally
        //    {
        //    }
        //    Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Exit Point", Logger.logLevel.Debug);

        //    return holidaysList;
        //}

        public static List<Holidays> getHolidays(string year)
        {
            List<Holidays> holidaysList = new List<Holidays>();
           List<Holidays> holidaysSelectedList = new List<Holidays>();
            try
            {

                using (var ctx = new CPPDbContext())
                {
                    holidaysList = ctx.Holidays.ToList();
                    foreach(var i in holidaysList) {

                        //int selectedyear = Convert.ToInt32(year);

                        var result = i.HolidayDate.Year;
                        string date = result.ToString();
                        bool yr = year.Contains(date);
                        if (yr == true)
                        {
                            holidaysSelectedList.Add(i);
                        }
                        //if (i.HolidayDate.Year < selectedyear)
                        //{
                        //    if (i.Type_Of_Holiday == "P")
                        //    {

                        //        var newdate = new DateTime(selectedyear, i.HolidayDate.Month, i.HolidayDate.Day);

                        //        i.HolidayDate = newdate;

                        //        holidaysSelectedList.Add(i);
                        //    }
                        //}
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

            return holidaysSelectedList;
        }

        public static String registerHolidays(Holidays holidays)
        {

            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String result = "";
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    Holidays retreivedholidays = new Holidays();
                    retreivedholidays = ctx.Holidays.Where(u => u.Description == holidays.Description || u.HolidayDate==holidays.HolidayDate).FirstOrDefault();


                    if (retreivedholidays == null)
                    {
                        //register
                        ctx.Holidays.Add(holidays);
                        ctx.SaveChanges();
                        result += holidays.Description + " has been created successfully.\n";
                    }
                    else
                    {
                        result += holidays.Description + "' failed to be created, duplicate holiday is not allowed.\n";
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
        public static String updateHolidays(Holidays holidays)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String result = "";

            try
            {
                using (var ctx = new CPPDbContext())
                {
                    Holidays retreivedHolidays = new Holidays();
                    retreivedHolidays = ctx.Holidays.Where(u => u.ID == holidays.ID).FirstOrDefault();

                    Holidays duplicateHolidays = ctx.Holidays.Where(a => (a.Description == holidays.Description && a.HolidayDate == holidays.HolidayDate)).FirstOrDefault();



                    //if (retreivedCertifiedPayroll != null && retreivedCertifiedPayroll.ID != certifiedpayroll.ID)
                    //{
                    //    return "Updating the project division code is unavailable at this moment (" + certifiedpayroll.Description + "). \n";
                    //}
                    if (duplicateHolidays != null)
                    {
                        result += holidays.Description + " failed to be updated, duplicate of holiday is not allowed.\n";
                    }
                    else if (retreivedHolidays != null)
                    {
                        CopyUtil.CopyFields<Holidays>(holidays, retreivedHolidays);
                        ctx.Entry(retreivedHolidays).State = System.Data.Entity.EntityState.Modified;
                        ctx.SaveChanges();
                        result += holidays.Description + " has been updated successfully.\n";
                    }
                    else
                    {
                        result += holidays.Description + " failed to be updated, it does not exist.\n";
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
        public static String deleteHolidays(Holidays holidays)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String result = "";
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    Holidays retreivedHolidays = new Holidays();
                    retreivedHolidays = ctx.Holidays.Where(u => u.ID == holidays.ID).FirstOrDefault();

                    if (retreivedHolidays != null)
                    {
                        ctx.Holidays.Remove(retreivedHolidays);
                        ctx.SaveChanges();
                        result = holidays.Description + " has been deleted successfully.\n";
                    }
                    else
                    {
                        result = holidays.Description + " failed to be deleted, it does not exist.\n";
                    }
                }

            }
            catch (Exception ex)
            {
                //result += serviceclass.Description + " failed to be deleted due to dependencies.\n";
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
        public static List<Holidays> RegisterFetchPreviousPermanentHolidays(Holidays holidays)
        {

            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String result = "";
            List<Holidays> retreivedholidaysList = new List<Holidays>();
            Holidays duplicateholiday = new Holidays();
            try
            {
                using (var ctx = new CPPDbContext())
                {
                  

                    retreivedholidaysList = ctx.Holidays.ToList();
                    int previousYear = holidays.HolidayDate.Year - 1;

                    foreach (var i in retreivedholidaysList)
                    {
                        string stryear = previousYear.ToString();
                        string strholidaydateyear = i.HolidayDate.Year.ToString();
                        bool checkyear = stryear.Contains(strholidaydateyear);
                        var newdate = new DateTime(holidays.HolidayDate.Year, i.HolidayDate.Month, i.HolidayDate.Day);
                        duplicateholiday = ctx.Holidays.Where(u => u.HolidayDate == newdate).FirstOrDefault();

                        if (checkyear == true && i.Type_Of_Holiday == "P" && duplicateholiday == null)
                        {
                            i.HolidayDate = newdate;
                            ctx.Holidays.Add(i);
                            ctx.SaveChanges();
                          


                        }

                    }
                    result = "Success";
                  retreivedholidaysList = ctx.Holidays.Where(u => u.HolidayDate.Year == previousYear).ToList();

                    

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

            return retreivedholidaysList;
        }
    }
}