using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    //[Authorize(Roles = "Accounting")]
    [Authorize]
    public class RequestPOListController : ApiController
    {
        // GET api/<controller>
        public HttpResponseMessage Get(int ProjectID)
        {
            List<POList> poList = new List<POList>();

            poList = WebAPI.Models.PurchaseOrder.getPOList(ProjectID);

            var jsonNew = new
            {
                result = poList
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}