using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Web;
using System.Web.Http;

using WebAPI.Models;



namespace WebAPI.Controllers
{
    [Authorize]
    public class RequestEstimateController : System.Web.Http.ApiController
    {
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //
        // GET: /RequestEstimate/
        public HttpResponseMessage Get(String ProjectID = "null", String TrendNumber = "null")
        {
            logger.Error("accepting request to generate estimate ...");
            String filePath = WebAPI.Models.Estimate.CreateCSV(ProjectID, TrendNumber);
            logger.Debug("Response " + filePath);
            
            
            // Convert the file to a byte array and give to front end.
            Byte[] bytes = System.IO.File.ReadAllBytes(filePath);
            string base64 = Convert.ToBase64String(bytes);
            return Request.CreateResponse(HttpStatusCode.OK, base64);
        }
    }
}