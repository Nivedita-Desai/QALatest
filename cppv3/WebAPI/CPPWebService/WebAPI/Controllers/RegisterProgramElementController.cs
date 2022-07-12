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
using WebAPI.Helper;

namespace WebAPI.Controllers
{
    [Authorize]
    public class RegisterProgramElementController : System.Web.Http.ApiController
    {
        private int program_elementOperation;
        //
        // GET: /RegisterProgram/
        //public HttpResponseMessage Get(int Operation, String ProgramID = "null", String ProgramElementID = "null", String ProgramElementName = "null", String ProgramElementManager = "null", String ProgramELementSponsor = "null")
        public HttpResponseMessage Post([FromBody] ProgramElement program_element)  
        {
            var throttler = new Throttler("RegisterProgram");

            String status = "";
            if (program_element.Operation == 1) {
                if (throttler.RequestShouldBeThrottled())
                {
                    status = "You are trying multiple request at a time";
                }
                else
                {
                    status = WebAPI.Models.ProgramElement.registerProgramElement(program_element);
                }
            }
                

            if (program_element.Operation == 2)
                status = WebAPI.Models.ProgramElement.updateProgramElement(program_element);

            if (program_element.Operation == 3)
                status = WebAPI.Models.ProgramElement.deleteProgramElement(program_element);
            //----Vaishnavi 30-03-2022----//
            if (program_element.Operation == 5)
                status = WebAPI.Models.ProgramElement.closeProgramElement(program_element);

            if (program_element.Operation == 4)
                status = "Success";

            var jsonNew = new
            {
                result = status
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}