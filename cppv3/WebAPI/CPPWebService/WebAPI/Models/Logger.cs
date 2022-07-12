using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;

namespace WebAPI.Models
{
    public class Logger
    {
        public enum logLevel
        {
            Debug, Exception, TimerStart, TimerEnd, Info
        };

        public static void LogExceptions(String className, String method, String message, String lineNumber, logLevel level)
        {
            //try
            //{
            //    string msg = "\n" + method +
            //     message + "\t" + className + "\t" + lineNumber
            //      + "\n" + "------------------------------------------------------------------------------------------";

            //    //Get the path from the config
            //    string configLogFile = ConfigurationManager.AppSettings["logFileName"].ToString();
            //    string path = "~/" + configLogFile;
            //    string p = System.Web.HttpContext.Current.Server.MapPath(path).ToString();
            //    if (!File.Exists(p))
            //    {
            //        FileStream fs = new FileStream(p, FileMode.CreateNew);
            //        fs.Close();
            //    }
            //    File.AppendAllText(path, msg);
            //    //Check if the file exists or not, If not create one
            //    //if (!File.Exists(HttpContext.Current.Server.MapPath(configLogFile)))
            //    //{
            //    //    File.Create(System.Web.HttpContext.Current.Server.MapPath(path)).Close();
            //    //}

            //    ////Open the file for reading
            //    //using (StreamWriter w = File.AppendText(System.Web.HttpContext.Current.Server.MapPath(path)))
            //    //{
            //    //    //Get the log level
            //    //    string configLogLevel = ConfigurationManager.AppSettings["loglevel"].ToString();

            //    //    int configintlevel;
            //    //    bool isNumeric = int.TryParse(configLogLevel, out configintlevel);

            //    //    //If the log level is set only for EXCEPTIONS
            //    //    if (isNumeric && level == logLevel.Exception && configintlevel > 0)
            //    //    {
            //    //        w.Write("EXCEPTION :: ");
            //    //        w.WriteLine(" {0}", DateTime.Now.ToString(CultureInfo.InvariantCulture));
            //    //        w.WriteLine(" AT {0} :: {1} - {2}", className, method, lineNumber);
            //    //        w.WriteLine(" EXception Message ::  " + message);
            //    //    }


            //    //    w.Flush();
            //    //    w.Close();
            ////}
            //}
            //catch (Exception ex)
            //{
            //    var stackTrace = new StackTrace(ex, true);
            //    var line = stackTrace.GetFrame(0).GetFileLineNumber();
            //    //Logger.LogExceptions(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, ex.Message, line.ToString(), Logger.logLevel.Exception);
            //}
            try
            {
                //Get the path from the config

                string configLogFile = ConfigurationManager.AppSettings["logFileName"].ToString();
                string path = "~/" + configLogFile;
                //string path = configLogFile;

                //Check if the file exists or not, If not create one
                if (!File.Exists(System.Web.HttpContext.Current.Server.MapPath(path)))
                {
                    File.Create(System.Web.HttpContext.Current.Server.MapPath(path)).Close();
                }

                //Open the file for reading
                using (StreamWriter w = File.AppendText(System.Web.HttpContext.Current.Server.MapPath(path)))
                {
                    string msg = "\n" + method +
                         message + "\t" + className + "\t" + lineNumber
                         + "\n" + "------------------------------------------------------------------------------------------";
                    w.Write(msg);
                   
                    w.Flush();
                    w.Close();
                }
            }
            catch (Exception ex)
            {
                var stackTrace = new StackTrace(ex, true);
                var line = stackTrace.GetFrame(0).GetFileLineNumber();
                //Logger.LogExceptions(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, ex.Message, line.ToString(), Logger.logLevel.Exception);
            }
        }
        

        public static void LogDebug(String className, String method, String message, logLevel level)
        {
            try
            {
                //Get the path from the config
                
                string configLogFile = ConfigurationManager.AppSettings["logFileName"].ToString();
                string path = "~/" + configLogFile;
                //string path = configLogFile;

                //Check if the file exists or not, If not create one
                if (!File.Exists(System.Web.HttpContext.Current.Server.MapPath(path)))
                {
                    File.Create(System.Web.HttpContext.Current.Server.MapPath(path)).Close();
                }

                //Open the file for reading
                using (StreamWriter w = File.AppendText(System.Web.HttpContext.Current.Server.MapPath(path)))
                {
                    //Get the log level
                    string configLogLevel = ConfigurationManager.AppSettings["loglevel"].ToString();

                    int configintlevel;
                    bool isNumeric = int.TryParse(configLogLevel, out configintlevel);


                    //If the log level is set  for DEBUG
                    if (isNumeric && level == logLevel.Debug && configintlevel > 1)
                    {
                        w.Write("DEBUG :: ");
                        w.WriteLine("\n AT {0} :: {1} ", className, method);
                        w.WriteLine(" Input parameters - {0}", message);
                    }
                    else if (isNumeric && level == logLevel.Info && configintlevel > 1)
                    {
                        w.Write("INFO :: ");
                        w.WriteLine("\n AT {0} :: {1} ", className, method);
                        w.WriteLine(" {0}", message);
                    }

                   //If the log level is set  for QUERY TIMER
                    else if (isNumeric && (level == logLevel.TimerStart) && configintlevel > 2)
                    {
                        w.Write(" TIMER : Timer Start time - ");
                        w.WriteLine(" AT {0} :: {1} ", className, method);
                        w.WriteLine("{0}", message);
                    }
                    //If the log level is set  for QUERY TIMER
                    else if (isNumeric && (level == logLevel.TimerEnd) && configintlevel > 2)
                    {
                        w.Write(" TIMER : Timer End time - ");
                        w.WriteLine(" AT {0} :: {1} ", className, method);
                        w.WriteLine("{0}", message);
                    }

                    w.Flush();
                    w.Close();
                }
            }
            catch (Exception ex)
            {
                var stackTrace = new StackTrace(ex, true);
                var line = stackTrace.GetFrame(0).GetFileLineNumber();
                Logger.LogExceptions(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, ex.Message, line.ToString(), Logger.logLevel.Exception);
            }
        }

    }

}