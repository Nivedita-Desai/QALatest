using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    //[Authorize]
    public class RegisterMFAConfigurationController : ApiController
    {
        public HttpResponseMessage Post([FromBody] MFAConfiguration mfaDeatils)
        {

            String status = "";

                status = WebAPI.Models.MFAConfiguration.updateMFADetails(mfaDeatils);

            var jsonNew = new
            {
                result = status
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }

    }
}