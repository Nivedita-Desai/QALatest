using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RegisterBOMRequestController : ApiController
    {
        public HttpResponseMessage Post([FromBody] List<BOMRequest> BOMRequestList)
        {

            String status = "";
            foreach (var BOMRequest in BOMRequestList)
            {

                if (BOMRequest.Operation == 1)
                    status += WebAPI.Models.BOMRequest.registerBOMRequest(BOMRequest);

                if (BOMRequest.Operation == 2)
                    status += WebAPI.Models.BOMRequest.updateBOMRequest(BOMRequest);

                if (BOMRequest.Operation == 3)
                    status += WebAPI.Models.BOMRequest.deleteBOMRequest(BOMRequest);

                //4 Do nothing
                if (BOMRequest.Operation == 4)
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
