using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RequestEmployeeController : ApiController
    {

        public HttpResponseMessage Get()
        {


            //List<Organization> OrganizationList = WebAPI.Models.Organization.getOrganization(OrganizationID, KeyStroke);
            List<Employee> employeeList = WebAPI.Models.Employee.getAllEmployees();

            var jsonNew = new
            {
                result = employeeList
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }

        public HttpResponseMessage Get(int OrganizationID)
        {


            //List<Organization> OrganizationList = WebAPI.Models.Organization.getOrganization(OrganizationID, KeyStroke);
            List<Employee> employeeList = WebAPI.Models.Employee.getEmployeesByOrgID(OrganizationID);

            var jsonNew = new
            {
                result = employeeList
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}
