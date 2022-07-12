using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace WebAPI.Helper
{
    public class DateUtility
    {

        public static DateTime firstDayOfWeek(DateTime date)
        {

            DayOfWeek fdow = CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
            int offset = fdow - date.DayOfWeek;
            DateTime fdowDate = date.AddDays(offset + 1);


            return fdowDate;

        }

        public static DateTime firstDayOfMonth(DateTime date)
        {
            var firstDayOfMonth  = new DateTime(date.AddMonths(1).Year, date.AddMonths(1).Month, 1);
            return firstDayOfMonth;

        }

        //Manasi 09-04-2021
        public static DateTime firstDayOfYear(DateTime date)
        {
            int year = date.Year;
            var firstDayOfYear = new DateTime(year, 1, 1);
            //var firstDayOfYear = new DateTime(date.AddMonths(1).Year, date.AddMonths(1).Month, 1);
            return firstDayOfYear;

        }

        public static String getSqlDateFormat()
        {
            return "yyyy-MM-dd HH:mm:ss";
        }


        public static DateTime getCutOffDate(String Granularity)
        {
            DateTime cutOffDate = new DateTime();
            if (Granularity == "week")
                cutOffDate = DateUtility.firstDayOfWeek(DateTime.Now);
            else if (Granularity == "month")
            {
                cutOffDate = DateUtility.firstDayOfMonth(DateTime.Now);
            }
            //Manasi 09-04-2021
            else if (Granularity == "year")
            {
                cutOffDate = DateUtility.firstDayOfYear(DateTime.Now);
            }
            return cutOffDate;
        }

    }
}