using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;
namespace WebAPI.Controllers
{
    public class RequestManufacturerController : ApiController
    {
        public HttpResponseMessage Get()
        {

            List<Manufacturer> ManufacturerList = new List<Manufacturer>();
            ManufacturerList = WebAPI.Models.Manufacturer.getManufacturer();


            var jsonNew = new
            {
                result = ManufacturerList
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}
