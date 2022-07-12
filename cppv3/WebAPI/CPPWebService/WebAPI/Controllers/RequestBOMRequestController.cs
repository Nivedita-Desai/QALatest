using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;
namespace WebAPI.Controllers
{
    public class RequestBOMRequestController : ApiController
    {
        public HttpResponseMessage Get()
        {

            List<BOMRequest> BOMRequestList = new List<BOMRequest>();
            BOMRequestList = WebAPI.Models.BOMRequest.getBOMRequest();


            var jsonNew = new
            {
                result = BOMRequestList
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}
