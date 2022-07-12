using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RegisterLocationController : ApiController
    {
        public HttpResponseMessage Post([FromBody] List<Location> location_list)
        {

            String status = "";
            foreach (var location in location_list)
            {

                if (location.Operation == 1)
                    status += WebAPI.Models.Location.registerLocation(location);

                if (location.Operation == 2)
                    status += WebAPI.Models.Location.updateLocation(location);

                if (location.Operation == 3)
                    status += WebAPI.Models.Location.deleteLocation(location);

                //4 Do nothing
                if (location.Operation == 4)
                    status += "";
            }

            var jsonNew = new
            {
                result = status
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}
