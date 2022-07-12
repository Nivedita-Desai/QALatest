using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RequestCostTypeController : ApiController
    {
        public HttpResponseMessage Get()
        {

            List<CostType> costTypeList = new List<CostType>();
            costTypeList = WebAPI.Models.CostType.GetCostType();


            var jsonNew = new
            {
                result = costTypeList
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}
