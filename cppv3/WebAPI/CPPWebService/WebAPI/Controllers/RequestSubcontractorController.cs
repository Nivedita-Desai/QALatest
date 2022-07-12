using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RequestSubcontractorController : ApiController
    {
        public HttpResponseMessage Get()
        {

            List<Subcontractor> subcontractorList = new List<Subcontractor>();
            subcontractorList = WebAPI.Models.Subcontractor.GetSubcontractor();


            var jsonNew = new
            {
                result = subcontractorList
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}