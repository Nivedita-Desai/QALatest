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
//using System.Web.Script.Serialization;

namespace WebAPI.Controllers
{
 
    public class RequestProjectLocationController : System.Web.Http.ApiController
    {
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //
        // GET: /RequestProjectLocation/
     //   [Authorize]
        public HttpResponseMessage Get(String OrganizationID)
            //(String ProgramID = "367", String ProgramElementID = "410", String ProjectID = "603")
            //(String ProgramID, String ProgramElementID, String ProjectID)
            //(String ProgramID = "null", String ProgramElementID = "null", String ProjectID = "null")
        {
            //List<ProjectLocation> ProjectLocationList = WebAPI.Models.ProjectLocation.getProjectLocation(ProgramID, ProgramElementID, ProjectID);
            logger.Error("OrganizationID" + OrganizationID);
            List<GISObject> ProjectLocationList = WebAPI.Models.GISObject.getLocation(OrganizationID);//(ProgramID, ProgramElementID, ProjectID);
            var jsonNew = new
            {
                result = ProjectLocationList
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
	}
}