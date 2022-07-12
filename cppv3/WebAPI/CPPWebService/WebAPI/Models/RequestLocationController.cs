using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RequestLocationController : ApiController
    {
        public HttpResponseMessage Get()
        {

            List<Location> locationList = new List<Location>();
            locationList = WebAPI.Models.Location.getLocation();


            var jsonNew = new
            {
                result = locationList
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}
