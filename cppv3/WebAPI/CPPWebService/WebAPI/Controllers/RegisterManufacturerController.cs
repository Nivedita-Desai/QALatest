using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RegisterManufacturerController : ApiController
    {
        public HttpResponseMessage Post([FromBody] List<Manufacturer> ManufacturerList)
        {

            String status = "";
            foreach (var Manufacturer in ManufacturerList)
            {

                if (Manufacturer.Operation == 1)
                    status += WebAPI.Models.Manufacturer.registerManufacturer(Manufacturer);

                if (Manufacturer.Operation == 2)
                    status += WebAPI.Models.Manufacturer.updateManufacturer(Manufacturer);

                if (Manufacturer.Operation == 3)
                    status += WebAPI.Models.Manufacturer.deleteManufacturer(Manufacturer);

                //4 Do nothing
                if (Manufacturer.Operation == 4)
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
