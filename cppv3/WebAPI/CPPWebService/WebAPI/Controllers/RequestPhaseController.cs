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
    public class RequestPhaseController : System.Web.Http.ApiController
    {
        //
        // GET: /RegisterPhase/
        public HttpResponseMessage Get(String ProjectID = "null", String TrendNumber = "null", String PhaseCode = "null", String PhaseStartDate = "null", String PhaseEndDate = "null")
        {


            List<Phase> PhaseList = WebAPI.Models.Phase.getPhase(ProjectID, TrendNumber, PhaseCode, PhaseStartDate, PhaseEndDate);


            var jsonNew = new
            {
                result = PhaseList
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
	}
}