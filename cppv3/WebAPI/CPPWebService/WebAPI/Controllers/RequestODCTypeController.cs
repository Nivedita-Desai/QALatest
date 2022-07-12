using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RequestODCTypeController : ApiController
    {
        public HttpResponseMessage Get()
        {

            List<ODCType> ODCTypeList = new List<ODCType>();
            ODCTypeList = WebAPI.Models.ODCType.GetODCType();


            var jsonNew = new
            {
                result = ODCTypeList
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}
