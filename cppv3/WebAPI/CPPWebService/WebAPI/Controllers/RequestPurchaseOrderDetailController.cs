using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models.StoredProcedure;

namespace WebAPI.Controllers
{
    public class RequestPurchaseOrderDetailController : ApiController
    {
        public HttpResponseMessage Get(int ProjectID)
        {
            List<PurchaseOrderDetailSP> poDetails = new List<PurchaseOrderDetailSP>();
            poDetails = PurchaseOrderDetailSP.getPurchaseOrderDetail(ProjectID);
            var jsonNew = new
            {
                result = poDetails
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}
