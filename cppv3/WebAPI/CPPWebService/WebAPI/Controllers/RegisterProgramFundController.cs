using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RegisterProgramFundController : System.Web.Http.ApiController
    {
        public HttpResponseMessage Post([FromBody] ProgramFund programFund)
        {

            String status = "";
            if (programFund.Operation == 1)
                status = WebAPI.Models.ProgramFund.registerProgramFund(programFund);

            if (programFund.Operation == 2)
                status = WebAPI.Models.ProgramFund.updateProgramFund(programFund);

            //if (programFund.Operation == 3)
            //    status = WebAPI.Models.ProjectFund.deleteProjectFund(projectFund);

            if (programFund.Operation == 4)
                status = "Success";

            var jsonNew = new
            {
                result = status
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }

    }
}
