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
    
    public class RegisterEmployeeController : System.Web.Http.ApiController
    {
        //
        // GET: /RegisterUser/
        public HttpResponseMessage Post([FromBody] List<Employee> employeeList)
        {

            String status = "";
            foreach (var employee in employeeList)
            {
                switch (employee.Operation)
                {
                    case 1:
                        status += WebAPI.Models.Employee.add(employee);
                        break;
                    case 2:
                        status += WebAPI.Models.Employee.update(employee);
                        break;
                    case 3:
                        status += WebAPI.Models.Employee.delete(employee);
                        break;
                    case 4:
                        status += "";
                        break;
                }
                //if (employee.Operation == 1)
                //    status = WebAPI.Models.Employee.add(employee);

                //if (employee.Operation == 2)
                //    status = WebAPI.Models.Employee.update(employee);

                //if (employee.Operation == 3)
                //    status = WebAPI.Models.Employee.delete(employee);

            }
            var jsonNew = new
            {
                result = status
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}