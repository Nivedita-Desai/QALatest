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
    public class RegisterLookupPhaseCodeController :  System.Web.Http.ApiController
    {
        //
        // GET: /RegisterLookupPhaseCode/
        //public HttpResponseMessage Get(int Operation, String PhaseID, String PhaseDescription, String PhaseCode)
        public HttpResponseMessage Post([FromBody] List<PhaseCode> phase_code_list)
        {

            String status = "";
            foreach(var phase_code in phase_code_list){
                if (phase_code.Operation == 1)
                    status += WebAPI.Models.PhaseCode.registerPhaseCode(phase_code);

                if (phase_code.Operation == 2)
                    status += WebAPI.Models.PhaseCode.updatePhaseCode(phase_code);

                if (phase_code.Operation == 3)
                    status += WebAPI.Models.PhaseCode.deletePhaseCode(phase_code);

                if (phase_code.Operation == 4)
                    status += "";
            }

            var jsonNew = new
            {
                result = status
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);


            //String status = "";
            //if (phase_code.Operation == 1)
            //    status = WebAPI.Models.PhaseCode.registerPhaseCode(phase_code.PhaseDescription, phase_code.Code);

            //if (phase_code.Operation == 2)
            //    status = WebAPI.Models.PhaseCode.updatePhaseCode(phase_code.PhaseID,phase_code.PhaseDescription, phase_code.Code);

            //if (phase_code.Operation == 3)
            //    status = WebAPI.Models.PhaseCode.deletePhaseCode(phase_code.PhaseDescription, phase_code.Code);


            //var jsonNew = new
            //{
            //    result = status
            //};
            //return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
	}
}