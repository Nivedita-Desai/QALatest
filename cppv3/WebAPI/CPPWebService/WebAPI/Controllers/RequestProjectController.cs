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
    //[Authorize]
    public class RequestProjectController : System.Web.Http.ApiController
    {
        //
        // GET: /Project/
        public HttpResponseMessage Get(String ProgramID = "null", String ProgramElementID = "null", String ProjectID = "null", String KeyStroke = "null")
        {
            List<Project> ProjectList = WebAPI.Models.Project.getProject(ProgramID, ProgramElementID, ProjectID, KeyStroke);

            var jsonNew = new
            {
                result = ProjectList
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}