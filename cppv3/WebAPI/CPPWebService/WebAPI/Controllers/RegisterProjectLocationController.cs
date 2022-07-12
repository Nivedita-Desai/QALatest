using System;
using System.Collections.Generic;
using System.IO;
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
    [Authorize]
    public class RegisterProjectLocationController : System.Web.Http.ApiController
    {
        //
        // GET: /RegisterProjectLocation/
        //public HttpResponseMessage Get(int Operation, String GeoLocationID = "null", String ProjectID = "null", String ShapeType = "null", String GeocodedLocation = "null", String UserDefinedLocation = "null", String RGBValue = "null")
        public HttpResponseMessage Post([FromBody] ProjectLocation project_location)  
        {

            String status = "";

            if (project_location.Operation == 1)
                status = WebAPI.Models.ProjectLocation.registerProjectLocation(project_location);

            if (project_location.Operation == 2)
                //status = WebAPI.Models.ProjectLocation.updateProjectLocation(project_location.GeoLocationID, project_location.ProjectID, project_location.ShapeType, project_location.GeocodedLocation, project_location.UserDefinedLocation, project_location.RGBValue);

            if (project_location.Operation == 3)
                status = WebAPI.Models.ProjectLocation.deleteProjectLocation(project_location.ProjectLocationID);

            if (project_location.Operation == 4)
                status = "Success";


            var jsonNew = new
            {
                result = status
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
	}
}