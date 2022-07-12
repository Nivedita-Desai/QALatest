using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;
namespace WebAPI.Controllers
{
    public class RegisterProjectScopeController : System.Web.Http.ApiController
    {
        public HttpResponseMessage Post([FromBody] List<ProjectScope> projectScopeList )
        {
            String status ="";
            //String status;
            foreach (var projectScope in projectScopeList)
            {
                if (projectScope.Operation == 1)
                {
                    status = WebAPI.Models.ProjectScope.registerProjectScope(projectScope);
                }
                else if (projectScope.Operation == 2)
                {
                    status = WebAPI.Models.ProjectScope.updateProjectScope(projectScope);
                }
                else if (projectScope.Operation == 3)
                {
                    status = WebAPI.Models.ProjectScope.deleteProjectScope(projectScope);
                }
            }
            //status = WebAPI.Models.TrendFund.testContext();
            var jsonNew = new
            {
                result = status
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}
