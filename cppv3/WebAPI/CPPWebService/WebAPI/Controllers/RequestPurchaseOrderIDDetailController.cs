using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;
using WebAPI.Models.StoredProcedure;

namespace WebAPI.Controllers
{
    public class RequestPurchaseOrderIDDetailController : ApiController
    {
        public HttpResponseMessage Get(int PurchaseOrderID)
        {
            List<PurchaseOrderDetailSP> poDetails = new List<PurchaseOrderDetailSP>();
            poDetails = PurchaseOrderDetailSP.getPurchaseOrderIDDetail(PurchaseOrderID);
            PurchaseOrder po = PurchaseOrder.getPO(PurchaseOrderID);
            var jsonNew = new
            {
                result = new { poDetails, po }
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }

    }
}
