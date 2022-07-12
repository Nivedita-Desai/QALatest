using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RequestProjectTypeController : ApiController
    {
        public HttpResponseMessage Get()
        {

            List<ProjectType> projectTypeList = new List<ProjectType>();
            projectTypeList = WebAPI.Models.ProjectType.getProjectType();


            var jsonNew = new
            {
                result = projectTypeList
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}
