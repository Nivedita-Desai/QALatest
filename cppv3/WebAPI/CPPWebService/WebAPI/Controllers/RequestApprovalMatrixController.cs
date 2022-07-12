using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RequestApprovalMatrixController : ApiController
    {
        public HttpResponseMessage Get()
        {


            List<ApprovalMatrix> ApprovalMatrixList = WebAPI.Models.ApprovalMatrix.getApprovalMatrix();


            var jsonNew = new
            {
                result = ApprovalMatrixList
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}
