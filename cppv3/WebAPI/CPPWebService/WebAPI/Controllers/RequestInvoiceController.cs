using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Web;
using System.Web.Http;

using WebAPI.Models;
//using System.Web.Script.Serialization;



namespace WebAPI.Controllers
{
    [Authorize]
    public class RequestInvoiceController : System.Web.Http.ApiController
    {
        public HttpResponseMessage Get(String ProjectID = "null")
        {
            String filePath = WebAPI.Models.Invoice.CreateCSV(Int32.Parse(ProjectID));
            

            // Convert the file to a byte array and give to front end.
            Byte[] bytes = System.IO.File.ReadAllBytes(filePath);
            string base64 = Convert.ToBase64String(bytes);
            return Request.CreateResponse(HttpStatusCode.OK, base64);
        }
    }
}