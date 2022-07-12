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
    public class RequestOrganizationController : System.Web.Http.ApiController
    {
        //
        // GET: /User/
        public HttpResponseMessage Get(String OrganizationID = "null", String KeyStroke = "null")
        {


            List<Organization> OrganizationList = WebAPI.Models.Organization.getOrganization(OrganizationID,  KeyStroke);


            var jsonNew = new
            {
                result = OrganizationList
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}