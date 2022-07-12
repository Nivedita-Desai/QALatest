using System;
using System.Collections.Generic;
using System.IO;
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
    public class RegisterPhaseController : System.Web.Http.ApiController
    {
        //
        // GET: /RegisterPhase/
        public HttpResponseMessage Get(int Operation, String ProjectID = "null", String TrendNumber = "null", String PhaseCode = "null", String PhaseStartDate = "null", String PhaseEndDate = "null")
        {

            String status = "";

            if ( Operation == 1)
                status = WebAPI.Models.Phase.registerPhase(ProjectID, TrendNumber, PhaseCode, PhaseStartDate, PhaseEndDate);

            if (Operation == 2)
                status = "Phase update not available";
            //No update of Phase

            if (Operation == 3)
                status = WebAPI.Models.Phase.deletePhase(ProjectID, TrendNumber, PhaseCode);

            if (Operation == 4)
                status = "Success";

            var jsonNew = new
            {
                result = status
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
	}
}