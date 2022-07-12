using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RequestCurrentCostOverheadController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage Get()
        {

            var currentDate = DateTime.Now;

            List<CostOverhead> costOverHeads = null;
            using (var ctx = new CPPDbContext())
            {
                costOverHeads = ctx.CostOverhead.Where(a => a.StartDate < currentDate && a.EndDate > currentDate).ToList();

            }

            var jsonNew = new
            {
                result = costOverHeads
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}