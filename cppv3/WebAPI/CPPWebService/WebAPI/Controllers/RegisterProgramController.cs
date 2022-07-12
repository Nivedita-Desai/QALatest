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
    public class RegisterProgramController : System.Web.Http.ApiController
    {
        //
        // GET: /RegisterProgram/
        //public HttpResponseMessage Get(int Operation, String ProgramID = "null", String ProgramName = "null", String ProgramManager = "null", String ProgramSponsor = "null")
        public HttpResponseMessage Post([FromBody] Program program)    
        {
            var throttler = new Throttler("RegisterProgram");
            
            String status = "";

            if (program.Operation == 1) //Create new program
            {
                if (throttler.RequestShouldBeThrottled()) {
                    status = "You are trying multiple request at a time";
                }
                else
                {
                    program.Operation = 0;
                    status = WebAPI.Models.Program.registerProgram(program);
                }
            }
            else if (program.Operation == 2) //Update Program details
            {
                status = WebAPI.Models.Program.updateProgram(program);
            }
            else if (program.Operation == 3) //Delete Program
            {
                status = WebAPI.Models.Program.deleteProgram(program);
            }
            //----Vaishnavi 30-03-2022----//
            else if (program.Operation == 5) //Delete Program
            {
                status = WebAPI.Models.Program.closeProgram(program);
            }
            //----Vaishnavi 30-03-2022----//
            else if (program.Operation == 4)
            {
                status = "Success";
            }
            else if (program.Operation == 6) // Update Additional Program Details
            {
                status = WebAPI.Models.Program.UpdateAdditionalProgramDetails(program);

            }


            var jsonNew = new
            {
                result = status
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
	}
}