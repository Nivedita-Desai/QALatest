using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers.AdminReports
{
    public class RequestServiceToSubserviceController : ApiController
    {
        public HttpResponseMessage Get()
        {

            List<ServiceToSubserviceMapping> serviceToSubserviceMappingList = new List<ServiceToSubserviceMapping>();
            serviceToSubserviceMappingList = WebAPI.Models.ServiceToSubserviceMapping.getSubServices();


            var jsonNew = new
            {
                result = serviceToSubserviceMappingList
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}
