using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RequestCertifiedPayrollController : ApiController
    {
        public HttpResponseMessage Get()
        {

            List<CertifiedPayroll> certifiedpayrollList = new List<CertifiedPayroll>();
            certifiedpayrollList = WebAPI.Models.CertifiedPayroll.getCertifiedPayroll();


            var jsonNew = new
            {
                result = certifiedpayrollList
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}
