using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.ExceptionHandling;
using WebAPI.Models.Api;

namespace WebAPI.CustomHandler
{
    public class UnhandledExceptionLogger : ExceptionLogger
    {
        
            public override void Log(ExceptionLoggerContext context)
            {
                var ex = context.Exception;

                string strLogText = "";
                strLogText += Environment.NewLine + "Source ---\n{0}" + ex.Source;
                strLogText += Environment.NewLine + "StackTrace ---\n{0}" + ex.StackTrace;
                strLogText += Environment.NewLine + "TargetSite ---\n{0}" + ex.TargetSite;

                if (ex.InnerException != null)
                {
                    strLogText += Environment.NewLine + "Inner Exception is {0}" + ex.InnerException;//error prone
                }
                if (ex.HelpLink != null)
                {
                    strLogText += Environment.NewLine + "HelpLink ---\n{0}" + ex.HelpLink;//error prone
                }

                var requestedURi = (string)context.Request.RequestUri.AbsoluteUri;
                var requestMethod = context.Request.Method.ToString();
                var timeUtc = DateTime.Now;

               
                ApiError apiError = new ApiError()
                {
                    Message = strLogText,
                    RequestUri = requestedURi,
                    RequestMethod = requestMethod,
                    TimeUtc = DateTime.Now
                };
             using (var ctx = new CPPDbContext())
            {
                ctx.ApiError.Add(apiError);
                ctx.SaveChanges();
            }
            }
        }
    
}