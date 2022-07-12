using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RequestPrimeController : ApiController
    {
        public HttpResponseMessage Get()
        {

            List<Prime> PrimeList = new List<Prime>();
            PrimeList = WebAPI.Models.Prime.getPrime();


            var jsonNew = new
            {
                result = PrimeList
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}
