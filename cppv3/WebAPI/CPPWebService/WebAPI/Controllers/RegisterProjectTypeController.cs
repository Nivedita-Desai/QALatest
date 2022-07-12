using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RegisterProjectTypeController : ApiController
    {
        public HttpResponseMessage Post([FromBody] List<ProjectType> project_type_list)
        {

            String status = "";
            foreach (var project_type in project_type_list)
            {

                if (project_type.Operation == 1)
                    status += WebAPI.Models.ProjectType.registerProjectType(project_type);

                if (project_type.Operation == 2)
                    status += WebAPI.Models.ProjectType.updateProjectType(project_type);

                if (project_type.Operation == 3)
                    status += WebAPI.Models.ProjectType.deleteProjectType(project_type);

                //4 Do nothing
                if (project_type.Operation == 4)
                    status += "";
            }

            var jsonNew = new
            {
                result = status
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}
