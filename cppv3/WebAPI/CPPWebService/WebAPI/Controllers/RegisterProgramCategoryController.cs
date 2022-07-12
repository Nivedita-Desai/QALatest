using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RegisterProgramCategoryController : System.Web.Http.ApiController
    {
        public HttpResponseMessage Post([FromBody] ProgramCategory programCategory)
        {

            String status = "";
            if (programCategory.Operation == 1)
                status = WebAPI.Models.ProgramCategory.registerProgramCategory(programCategory);

            if (programCategory.Operation == 2)
                status = WebAPI.Models.ProgramCategory.updateProgramCategory(programCategory);

            //if (programFund.Operation == 3)
            //    status = WebAPI.Models.ProjectFund.deleteProjectFund(projectFund);


            var jsonNew = new
            {
                result = status
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }

    }
}
