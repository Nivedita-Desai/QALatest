using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebAPI.Controllers
{
    public class RegisterActualCostExcelController : ApiController
    {
        public HttpResponseMessage Post([FromBody] String base64ExcelFile)
        {
            byte[] byteArrayExcelFile = Convert.FromBase64String(base64ExcelFile);

            String status = "";


            var jsonNew = new
            {
                result = status
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}
