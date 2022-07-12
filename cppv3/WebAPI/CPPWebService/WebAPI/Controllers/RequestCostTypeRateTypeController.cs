using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RequestCostTypeRateTypeController : ApiController
    {
        public HttpResponseMessage Get()
        {

            List<CostTypeRateType> costTypeRateTypeList = new List<CostTypeRateType>();
            costTypeRateTypeList = WebAPI.Models.CostTypeRateType.GetCostTypeRateType();


            var jsonNew = new
            {
                result = costTypeRateTypeList
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}
