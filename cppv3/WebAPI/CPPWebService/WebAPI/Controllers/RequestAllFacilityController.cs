using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RequestAllFacilityController : ApiController
    {

        public HttpResponseMessage Get()
        {
            List<Facility> facilityList;
            //String status;
            //status = WebAPI.Models.Facility.getFacilityByID(FacilityID);
            facilityList = WebAPI.Models.Facility.getAllFacilities();
            //status = WebAPI.Models.TrendFund.testContext();

            List<JObject> jlist = new List<JObject>();
            foreach (var item in facilityList)
            {
                var s = JsonConvert.SerializeObject(item, Formatting.Indented,
                                  new JsonSerializerSettings()
                                  {
                                      ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                                  });


                JObject json = JObject.Parse(s);
                jlist.Add(json);
            }
            return Request.CreateResponse(HttpStatusCode.OK, jlist);
        }
    }
}
