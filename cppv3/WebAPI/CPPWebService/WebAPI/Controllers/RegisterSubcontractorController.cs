using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RegisterSubcontractorController : ApiController
    {
        public HttpResponseMessage Post([FromBody] List<Subcontractor> subcontractor_list)
        {

            String status = "";
            foreach (var subcontractor in subcontractor_list)
            {

                if (subcontractor.Operation == 1)
                    status += WebAPI.Models.Subcontractor.registerSubcontractor(subcontractor);

                if (subcontractor.Operation == 2)
                    status += WebAPI.Models.Subcontractor.updateSubcontractor(subcontractor);

                if (subcontractor.Operation == 3)
                    status += WebAPI.Models.Subcontractor.deleteSubcontractor(subcontractor);

                //4 Do nothing
                if (subcontractor.Operation == 4)
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
