using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RequestWrapController : ApiController
    {
        public HttpResponseMessage Get()
        {

            List<Wrap> wrapList = new List<Wrap>();
            wrapList = WebAPI.Models.Wrap.getWrap();


            var jsonNew = new
            {
                result = wrapList
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}
