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
using WebAPI.Controllers;

namespace WebAPI.Models
{
    public class InsperityExportJob : IJob
    {
        //readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        RequestExportInsperityCostCodeToExcelController requestExportInsperityCostCodeToExcelController = new RequestExportInsperityCostCodeToExcelController();
        public void Execute(IJobExecutionContext context)
        {
            try
            {
               // logger.Info("before InsperityExportJob - killer job");
               /*var baseURL= System.Web.Hosting.HostingEnvironment.SiteName;
              //  String baseURL1 = System.Web.Hosting.HostingEnvironment.ApplicationHost.GetVirtualPath();
                String baseURL2 = System.Web.Hosting.HostingEnvironment.ApplicationID;
                String base3= baseURL2 + "/Request/ExportInsperityCostCodeToExcel";*/
                // var baseUrl = Request.RequestUri.GetLeftPart(UriPartial.Authority)
                requestExportInsperityCostCodeToExcelController.Get();
                // logger.Info("after InsperityExportJob - killer job");
            }
            catch (Exception ex)
            {
               // logger.Info(ex);
            }
        }

       

    }
}
