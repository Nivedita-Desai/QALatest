using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RegisterServiceClassController : ApiController
    {
        public HttpResponseMessage Post([FromBody] List<ServiceClass> service_class_list)
        {

            String status = "";
            foreach (var service_class in service_class_list)
            {

                if (service_class.Operation == 1)
                    status += WebAPI.Models.ServiceClass.registerServiceClass(service_class);

                if (service_class.Operation == 2)
                    status += WebAPI.Models.ServiceClass.updateServiceClass(service_class);

                if (service_class.Operation == 3)
                    status += WebAPI.Models.ServiceClass.deleteServiceClass(service_class);

                //4 Do nothing
                if (service_class.Operation == 4)
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
