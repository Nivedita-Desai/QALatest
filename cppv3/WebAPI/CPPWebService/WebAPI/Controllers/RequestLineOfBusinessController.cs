using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;
namespace WebAPI.Controllers
{
    public class RequestLineOfBusinessController : ApiController
    {

        public HttpResponseMessage GET()
        {


            List<LineOfBusiness> lobs = new List<LineOfBusiness>();
            
            using (var ctx = new CPPDbContext())
            {
                lobs = ctx.LineOfBusiness.ToList();
            } 

            var json = new
            {
                result = lobs
            };
            return Request.CreateResponse(HttpStatusCode.OK, json);
        }
    }
}
