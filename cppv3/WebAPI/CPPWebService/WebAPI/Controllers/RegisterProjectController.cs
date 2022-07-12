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
using WebAPI.Helper;

namespace WebAPI.Controllers
{
    [Authorize]
    public class RegisterProjectController : System.Web.Http.ApiController
    {
        //
        // GET: /RegisterProject/
        //public HttpResponseMessage Get(int Operation, String ProgramID = "null", String ProgramElementID = "null", String ProjectID = "null", String ProjectName = "null", String ProjectManager = "null", String ProjectSponsor = "null")
        public HttpResponseMessage Post([FromBody] Project project)  
        {
            var throttler = new Throttler("RegisterProject");
            String status = "";

            if (project.Operation == 1) {
                if (throttler.RequestShouldBeThrottled())
                {
                    status = "You are trying multiple request at a time";
                }
                else
                {
                    status = WebAPI.Models.Project.registerProject(project);
                }
            }

            if (project.Operation == 2)
                status = WebAPI.Models.Project.updateProject(project);

            if (project.Operation == 3)
                status = WebAPI.Models.Project.deleteProject(project);
            //----Vaishnavi 30-03-2022----//
            if (project.Operation == 5)
                status = WebAPI.Models.Project.closeProject(project);

            if (project.Operation == 4)
                status = "Success";

            var jsonNew = new
            {
                result = status
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}