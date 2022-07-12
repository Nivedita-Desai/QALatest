using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using WebAPI.Models.Api;

namespace WebAPI.CustomHandler
{
    public class MessageLogging
    {
        private readonly static string workingFolder = HttpRuntime.AppDomainAppPath + @"Uploads\";
        public void IncomingMessageAsync(ApiLog apiLog)
        {
            apiLog.RequestType = "Request";

            using(var ctx = new CPPDbContext())
            {
                ctx.ApiLog.Add(apiLog);
                ctx.SaveChanges();
            }
        }

        public void OutgoingMessageAsync(ApiLog apiLog)
        {
            apiLog.RequestType = "Response";
            using (var ctx = new CPPDbContext())
            {
                ctx.ApiLog.Add(apiLog);
                ctx.SaveChanges();
            }
        }
        public static bool CreateFile(string filePath_Name)
        {
            try
            {
                if (!File.Exists(filePath_Name))
                {
                    FileStream fs = new FileStream(filePath_Name, FileMode.CreateNew);
                    fs.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool WriteFile(string msg, string filePath_Name)
        {
            try
            {
                File.AppendAllText(filePath_Name, msg);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static void LogException(string msg)
        {
            string filePath_Name = workingFolder + "CPP_QB_LoggFile_" + DateTime.Now.ToString("dddd_MM_dd_yyyy") + ".txt";
            if (CreateFile(filePath_Name))
                WriteFile(msg, filePath_Name);
        }
    }
}