using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RequestFacilityByIDController : ApiController
    {

        public HttpResponseMessage Get(int FacilityID)
        {
            List<Facility> facility;
            //String status;
            //status = WebAPI.Models.Facility.getFacilityByID(FacilityID);
            //facility = WebAPI.Models.Facility.testGetMany(FacilityID);
            //status = WebAPI.Models.TrendFund.testContext();
            if (FacilityID == 0)
            {
                facility = WebAPI.Models.Facility.getAllFacilities();
            }
            else
            {
                facility = WebAPI.Models.Facility.getFacilityByID(FacilityID);
            }

            var s = JsonConvert.SerializeObject(facility, Formatting.Indented,
                              new JsonSerializerSettings()
                              {
                                  ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                              });
            //JObject json = JObject.Parse(s);
            var json = JArray.Parse(s);
            return Request.CreateResponse(HttpStatusCode.OK, json);
        }

    }
}
