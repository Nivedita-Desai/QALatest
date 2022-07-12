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
//using System.Web.Script.Serialization;
namespace WebAPI.Controllers
{
    [Authorize]
    public class RegisterLookupRoleController : System.Web.Http.ApiController
    {
        //
        // GET: /RegisterLookupRole/
        //public HttpResponseMessage Get(int Operation, String Role, String AccessControlList)
        public HttpResponseMessage Post([FromBody] List<UserRole> roles)
        {

            String status = "";
            foreach (var role in roles)
            {
                if (role.Operation == 1)
                    status += WebAPI.Models.UserRole.registerRole(role.Role, role.AccessControlList);

                if (role.Operation == 2)
                    status += WebAPI.Models.UserRole.updateRole(role.Role, role.AccessControlList);

                if (role.Operation == 3)
                    status += WebAPI.Models.UserRole.deleteRole(role.Role);

                if (role.Operation == 4)
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