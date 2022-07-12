using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RegisterCertifiedPayrollController : ApiController
    {
        public HttpResponseMessage Post([FromBody] List<CertifiedPayroll> certifiedpayroll_list)
        {

            String status = "";
            foreach (var certifiedPayroll in certifiedpayroll_list)
            {

                if (certifiedPayroll.Operation == 1)
                    status += WebAPI.Models.CertifiedPayroll.registerCertifiedPayroll(certifiedPayroll);

                if (certifiedPayroll.Operation == 2)
                    status += WebAPI.Models.CertifiedPayroll.updateCertifiedPayroll(certifiedPayroll);

                if (certifiedPayroll.Operation == 3)
                    status += WebAPI.Models.CertifiedPayroll.deleteCertifiedPayroll(certifiedPayroll);

                //4 Do nothing
                if (certifiedPayroll.Operation == 4)
                    status += "";
            }

            var jsonNew = new
            {
                result = status
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}
