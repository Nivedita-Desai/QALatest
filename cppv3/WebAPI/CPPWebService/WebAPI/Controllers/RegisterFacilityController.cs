using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RegisterFacilityController : System.Web.Http.ApiController
    {
        public HttpResponseMessage Post([FromBody] Facility facility)
        {
            String status = "";
            //String status;

            if (facility.Operation == 1)
            {
                status = WebAPI.Models.Facility.registerFacility(facility);
            }
            else if (facility.Operation == 2)
            {
                status = WebAPI.Models.Facility.updateFacility(facility);
            }
            else if (facility.Operation == 3)
            {
                status = WebAPI.Models.Facility.deleteFacility(facility);
            } else if (facility.Operation == 4)
            {
                status = "Success";
            }

            //status = WebAPI.Models.TrendFund.testContext();
            var jsonNew = new
            {
                result = status
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}
