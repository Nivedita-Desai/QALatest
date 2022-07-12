using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RequestPhaseForGanttAtContractLevelController : ApiController
    {
        [HttpGet]
        [Route("Request/Phases/")]
        public HttpResponseMessage GetPhase(String PhaseDescription = "null", String Code = "null", String ProgramID = "null")
        {
            List<ViewContractGantt> PhaseCodeList = WebAPI.Models.ViewContractGantt.getPhaseCode(PhaseDescription, Code, ProgramID);

            var jsonNew = new
            {
                result = PhaseCodeList
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}
