using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RegisterApprovalMatrixController : ApiController
    {
        public HttpResponseMessage Post([FromBody] List<ApprovalMatrix> AM_list)
        {

            String status = "";
            foreach (var AM in AM_list)
            {
                if (AM.Operation == 1)
                    status += WebAPI.Models.ApprovalMatrix.registerApprovalMatrix(AM);

                if (AM.Operation == 2)
                    status += WebAPI.Models.ApprovalMatrix.updateApprovalMatrix(AM);

                if (AM.Operation == 3)
                    status += WebAPI.Models.ApprovalMatrix.deleteApprovalMatrix(AM);

                if (AM.Operation == 4)
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
