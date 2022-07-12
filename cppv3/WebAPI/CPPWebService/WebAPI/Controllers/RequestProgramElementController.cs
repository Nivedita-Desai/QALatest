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
    public class RequestProgramElementController : System.Web.Http.ApiController
    {
        //
        // GET: /User/
        public HttpResponseMessage Get(String ProgramID = "null", String ProgramElementID = "null", String KeyStroke = "null")
        {


            List<ProgramElement> ProgramElementList = WebAPI.Models.ProgramElement.getProgramElement(ProgramID, ProgramElementID, KeyStroke);


            var jsonNew = new
            {
                result = ProgramElementList
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}