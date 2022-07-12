using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RegisterProjectClassPhaseController : ApiController
    {
        public HttpResponseMessage Post([FromBody] List<ProjectClassPhase> project_class_phase_list)
        {

            String status = "";
            foreach (var project_class_phase in project_class_phase_list)
            {

                if (project_class_phase.Operation == 1)
                    status += WebAPI.Models.ProjectClassPhase.registerProjectClassPhaseList(project_class_phase);

                if (project_class_phase.Operation == 2)
                    status += WebAPI.Models.ProjectClassPhase.updateProjectClassPhase(project_class_phase);

                if (project_class_phase.Operation == 3)
                    status += WebAPI.Models.ProjectClassPhase.deleteProjectClassPhase(project_class_phase);

                //4 Do nothing
                if (project_class_phase.Operation == 4)
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
