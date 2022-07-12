using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebAPI.Controllers
{
    public class RequestAssetHistoryByIDController : ApiController
    {
        public HttpResponseMessage get(int AssetHistoryID)
        {
            object status;
            //String status;
            status = WebAPI.Models.AssetHistory.getAssetHistoryByID(AssetHistoryID);
            //status = WebAPI.Models.TrendFund.testContext();
            var jsonNew = new
            {
                result = status
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }

    }
}
