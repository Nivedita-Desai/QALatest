using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RequestNewPurchaseOrderNumberController : ApiController
    {
        public HttpResponseMessage Get(int ProjectID)
        {

            PurchaseOrder po = new PurchaseOrder();
            po = po.getNewPurchaseOrder(ProjectID);
            var jsonNew = new
            {
                result = po
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}
