using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RequestPhaseForGanttAtProjectLevelController : ApiController
    {
        [HttpGet]
        [Route("Request/Phases/")]
        public HttpResponseMessage GetPhase(String PhaseDescription = "null", String Code = "null", String ProgramElementID = "null")
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
