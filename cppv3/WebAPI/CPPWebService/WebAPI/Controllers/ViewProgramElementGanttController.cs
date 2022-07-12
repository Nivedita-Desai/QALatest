using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    //[Authorize]  
    public class ViewProgramElementGanttController : ApiController
    {
       

        
        [Route("Request/ProjectActivity/")]
        public HttpResponseMessage Phase(String PhaseDescription = "null", String Code = "null", String ProgramElementID = "null")
        {
            List<ViewPrgmElementGantt> PhaseCodeList = WebAPI.Models.ViewPrgmElementGantt.getPhaseCode(PhaseDescription, Code, ProgramElementID);

            var jsonNew = new
            {
                result = PhaseCodeList
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}