using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RegisterSubcontractorTypeController : ApiController
    {
        public HttpResponseMessage Post([FromBody] List<SubcontractorType> subcontractor_type_list)
        {

            String status = "";
            foreach (var subcontractor_type in subcontractor_type_list)
            {

                if (subcontractor_type.Operation == 1)
                    status += WebAPI.Models.SubcontractorType.registerSubcontractorType(subcontractor_type);

                if (subcontractor_type.Operation == 2)
                    status += WebAPI.Models.SubcontractorType.updateSubcontractorType(subcontractor_type);

                if (subcontractor_type.Operation == 3)
                    status += WebAPI.Models.SubcontractorType.deleteSubcontractorType(subcontractor_type);

                //4 Do nothing
                if (subcontractor_type.Operation == 4)
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
