using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RequestUnitTypeController : ApiController
    {
        public HttpResponseMessage Get()
        {

            List<UnitType> FundTypeList = new List<UnitType>();
            FundTypeList = WebAPI.Models.UnitType.getUnitType();


            var jsonNew = new
            {
                result = FundTypeList
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }

    }
}
