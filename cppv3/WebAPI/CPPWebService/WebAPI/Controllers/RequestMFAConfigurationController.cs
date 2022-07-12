using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    //[Authorize]
    public class RequestMFAConfigurationController : ApiController
    {
        public HttpResponseMessage Get()
        {


            MFAConfiguration MFADetails = WebAPI.Models.MFAConfiguration.getMFADetails();


            var jsonNew = new
            {
                result = MFADetails
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}
