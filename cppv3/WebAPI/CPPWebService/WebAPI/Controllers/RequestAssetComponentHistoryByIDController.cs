using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebAPI.Controllers
{
    public class RequestAssetComponentHistoryByIDController : ApiController
    {
        public HttpResponseMessage get(int AssetComponentHistoryID)
        {
            object status;
            //String status;
            status = WebAPI.Models.AssetComponentHistory.getAssetComponentHistoryByID(AssetComponentHistoryID);
            //status = WebAPI.Models.TrendFund.testContext();
            var jsonNew = new
            {
                result = status
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}
