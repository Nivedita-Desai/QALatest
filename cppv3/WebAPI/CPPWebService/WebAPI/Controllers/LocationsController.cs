using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Web;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class LocationsController : ApiController
    {
        [Authorize]
        public HttpResponseMessage Get()
        {
            List<GISObject> ProjectLocationList = WebAPI.Models.GISObject.getLocation("76");
            var jsonNew = new
            {
                result = ProjectLocationList
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
	}
}