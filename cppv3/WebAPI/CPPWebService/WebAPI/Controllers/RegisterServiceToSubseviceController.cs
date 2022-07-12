using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;
namespace WebAPI.Controllers
{
    public class RegisterServiceToSubseviceController : ApiController
    {
         public HttpResponseMessage Post([FromBody] List<ServiceToSubserviceMapping> service_to_subservice_list)
        {

            String status = "";
            foreach (var service_to_subservice in service_to_subservice_list)
            {

                if (service_to_subservice.Operation == 1)
                    status += WebAPI.Models.ServiceToSubserviceMapping.registerServiceToSubserviceMappingList(service_to_subservice);

                if (service_to_subservice.Operation == 2)
                    status += WebAPI.Models.ServiceToSubserviceMapping.updateServiceToSubserviceMapping(service_to_subservice);

                if (service_to_subservice.Operation == 3)
                    status += WebAPI.Models.ServiceToSubserviceMapping.deleteServiceToSubserviceMapping(service_to_subservice);

                //4 Do nothing
                if (service_to_subservice.Operation == 4)
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
