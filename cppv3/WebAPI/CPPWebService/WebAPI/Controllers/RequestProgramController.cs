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
    public class RequestProgramController : System.Web.Http.ApiController
    {
        //
        // GET: /User/
        public HttpResponseMessage Get(String OrganizationID = "null",  String ProgramID = "null", String KeyStroke = "null")
        {

            List<Program> ProgramList = WebAPI.Models.Program.getProgram(OrganizationID, ProgramID, KeyStroke);
            //List<Program> ProgramList = WebAPI.Models.Program.getProgramLookup(OrganizationID, ProgramID, KeyStroke);


            var jsonNew = new
            {
                result = ProgramList
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}