using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Configuration;
using System.Diagnostics;
using System.Threading;

namespace WebAPI.Models
{
    public class MppNoRespondKillerJob : IJob
    {
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                logger.Info("before attempt process kill - killer job");
                main();
                logger.Info("after attempt process kill - killer job");
            }
            catch (Exception ex)
            {
                logger.Info(ex);
            }
        }

        public void main()
        {
            foreach (Process proc in Process.GetProcessesByName("WINPROJ"))
            {
                if (!proc.Responding)
                {
                    //Thread.Sleep(10000);
                    logger.Info("Before Process Kill - killer job");
                    proc.Kill();
                    logger.Info("After Process Kill - killer job");
                }
            }
        }

    }
}
