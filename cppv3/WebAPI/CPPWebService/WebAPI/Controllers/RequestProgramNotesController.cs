using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RequestProgramNotesController : ApiController
    {
        public HttpResponseMessage Get(int? programId = null)
        {

            if (programId != null)
            {
                List<ProgramNotes> programNotesList = ProgramNotes.getProgramNotes((int)programId);
                var jsonNew = new
                {
                    result = "success",
                    data = programNotesList
                };
                return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
            }
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
