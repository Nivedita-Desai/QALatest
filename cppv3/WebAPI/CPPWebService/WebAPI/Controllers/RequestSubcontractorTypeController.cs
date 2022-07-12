using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RequestSubcontractorTypeController : ApiController
    {
        public HttpResponseMessage Get()
        {

            List<SubcontractorType> subcontractorTypeList = new List<SubcontractorType>();
            subcontractorTypeList = WebAPI.Models.SubcontractorType.GetSubcontractorType();


            var jsonNew = new
            {
                result = subcontractorTypeList
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}