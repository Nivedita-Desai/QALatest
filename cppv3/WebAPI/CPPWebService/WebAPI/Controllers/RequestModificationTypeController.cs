using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RequestModificationTypeController : ApiController
    {
        public HttpResponseMessage Get() 
        {
            List<ModificationType> modificationTypes = new List<ModificationType>();
            modificationTypes = WebAPI.Models.ModificationType.GetModificationTypes();


            var jsonNew = new
            {
                result = modificationTypes
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}
