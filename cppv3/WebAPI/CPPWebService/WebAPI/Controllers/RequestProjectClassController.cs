using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RequestProjectClassController : ApiController
    {
        public HttpResponseMessage Get()
        {

            List<ProjectClass> projectClassList = new List<ProjectClass>();
            projectClassList = WebAPI.Models.ProjectClass.getProjectClass();


            var jsonNew = new
            {
                result = projectClassList
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}
