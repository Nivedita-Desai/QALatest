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
    public class RequestAdditionalInfoController : ApiController
    {
        public HttpResponseMessage Get(int ProgramID = 0)
        {

           Program ProgramList = WebAPI.Models.Program.GetProgramAdditionalInfo(ProgramID);
            //List<Program> ProgramList = WebAPI.Models.Program.getProgramLookup(OrganizationID, ProgramID, KeyStroke);


            var jsonNew = new
            {
                result = ProgramList
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}
