using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RequestProjectClassPhaseController : ApiController
    {
        public HttpResponseMessage Get()
        {

            List<ProjectClassPhase> projectClassPhaseList = new List<ProjectClassPhase>();
            projectClassPhaseList = WebAPI.Models.ProjectClassPhase.getProjectClassPhase();


            var jsonNew = new
            {
                result = projectClassPhaseList
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}
