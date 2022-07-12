using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RegisterProjectClassController : ApiController
    {
        public HttpResponseMessage Post([FromBody] List<ProjectClass> project_class_list)
        {

            String status = "";
            foreach (var project_class in project_class_list)
            {

                if (project_class.Operation == 1)
                    status += WebAPI.Models.ProjectClass.registerProjectClass(project_class);

                if (project_class.Operation == 2)
                    status += WebAPI.Models.ProjectClass.updateProjectClass(project_class);

                if (project_class.Operation == 3)
                    status += WebAPI.Models.ProjectClass.deleteProjectClass(project_class);

                //4 Do nothing
                if (project_class.Operation == 4)
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
