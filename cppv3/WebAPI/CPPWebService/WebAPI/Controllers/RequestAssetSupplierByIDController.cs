using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebAPI.Controllers
{
    public class RequestAssetSupplierByIDController : ApiController
    {
        public HttpResponseMessage get(int SupplierID)
        {
            object status;
            //String status;
            status = WebAPI.Models.AssetSupplier.getAssetSupplierByID(SupplierID);
            //status = WebAPI.Models.TrendFund.testContext();
            var jsonNew = new
            {
                result = status
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}
