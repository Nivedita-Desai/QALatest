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

namespace WebAPI.Controllers
{
    [Authorize]
    public class RegisterOrganizationController : System.Web.Http.ApiController
    {
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //
        // GET: /RegisterProgram/
        //public HttpResponseMessage Get(int Operation, String ProgramID = "null", String ProgramName = "null", String ProgramManager = "null", String ProgramSponsor = "null")
        public HttpResponseMessage Post([FromBody] Organization organization)    
        {
            String userName = RequestContext.Principal.Identity.Name;
            String status = "";

            if (organization.Operation == 1)
                status = WebAPI.Models.Organization.registerOrganization(organization, userName);

            if (organization.Operation == 2)
                status = WebAPI.Models.Organization.updateOrganization(organization,userName);

            if (organization.Operation == 3)
                status = WebAPI.Models.Organization.deleteOrganization(organization,userName);

            if (organization.Operation == 4)
                status = "Success";


            var jsonNew = new
            {
                result = status
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
	}
}