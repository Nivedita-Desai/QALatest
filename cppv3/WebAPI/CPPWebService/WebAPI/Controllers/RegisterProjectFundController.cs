using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RegisterProjectFundController : System.Web.Http.ApiController
    {
        public HttpResponseMessage Post([FromBody] ProjectFund projectFund)
        {

            String status = "";
            if (projectFund.Operation == 1)
                status = WebAPI.Models.ProjectFund.registerProjectFund(projectFund);

            if (projectFund.Operation == 2)
                status = WebAPI.Models.ProjectFund.updateProjectFund(projectFund);

            if (projectFund.Operation == 3)
                status = WebAPI.Models.ProjectFund.deleteProjectFund(projectFund);

            if (projectFund.Operation == 4)
                status = "Success";

            var jsonNew = new
            {
                result = status
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }

    }
}
