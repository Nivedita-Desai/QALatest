using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RegisterODCTypeController : ApiController
    {
        public HttpResponseMessage Post([FromBody] List<ODCType> odc_type_list)
        {

            String status = "";
            foreach (var odc_type in odc_type_list)
            {

                if (odc_type.Operation == 1)
                    status += WebAPI.Models.ODCType.registerODCType(odc_type);

                if (odc_type.Operation == 2)
                    status += WebAPI.Models.ODCType.updateODCType(odc_type);

                if (odc_type.Operation == 3)
                    status += WebAPI.Models.ODCType.deleteODCType(odc_type);

                //4 Do nothing
                if (odc_type.Operation == 4)
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
