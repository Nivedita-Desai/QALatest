using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Web;
using System.Web.Http;

using WebAPI.Models;
using WebAPI.Helper;
using System;

namespace WebAPI.Controllers
{
    public class RequestProgramElementOnChangeOrderController : ApiController
    {
        public System.Net.Http.HttpResponseMessage Get(int programELementID, DateTime projectEndDate)
        {
            var throttler = new Throttler("RegisterProgramElementOnChangeOrderController");
            String status = "";

            status = WebAPI.Models.ProgramElement.updatePojectEndDateOnChangeOrder(programELementID, projectEndDate);

            var jsonNew = new
            {
                result = status
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}