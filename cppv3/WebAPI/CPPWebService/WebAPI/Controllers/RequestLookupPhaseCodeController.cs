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
    //[Authorize]   //luan here temporarily
    public class RequestLookupPhaseCodeController : System.Web.Http.ApiController
    {
        //
        // GET: /RequestLookupPhaseCode/
        public HttpResponseMessage Get(String PhaseDescription = "null", String Code = "null", String ProjectID = "null")
        {


            List<PhaseCode> PhaseCodeList = WebAPI.Models.PhaseCode.getPhaseCode(PhaseDescription, Code, ProjectID);


            var jsonNew = new
            {
                result = PhaseCodeList
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
	}
}