using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;
namespace WebAPI.Controllers
{
    public class RequestVendorController : ApiController
    {
        public HttpResponseMessage Get()
        {

            List<Vendor> VendorList = new List<Vendor>();
            VendorList = WebAPI.Models.Vendor.getVendor();


            var jsonNew = new
            {
                result = VendorList
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}
