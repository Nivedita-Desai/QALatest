using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RegisterUnitTypeController : ApiController
    {
        public HttpResponseMessage Post([FromBody] List<UnitType> unit_type_list)
        {

            String status = "";
            foreach (var unit_type in unit_type_list)
            {

                if (unit_type.Operation == 1)
                    status += WebAPI.Models.UnitType.registerUnitType(unit_type );

                if (unit_type.Operation == 2)
                    status += WebAPI.Models.UnitType.updateUnitType(unit_type);

                if (unit_type.Operation == 3)
                    status += WebAPI.Models.UnitType.deleteUnitType(unit_type);

                //4 Do nothing
                if (unit_type.Operation == 4)
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
