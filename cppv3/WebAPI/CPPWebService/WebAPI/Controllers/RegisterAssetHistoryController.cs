using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RegisterAssetHistoryController : ApiController
    {
        public HttpResponseMessage Post([FromBody] AssetHistory assetHistory)
        {
            String status = "";
            //String status;

            if (assetHistory.Operation == 1)
            {
                status = WebAPI.Models.AssetHistory.registerAssetHistory(assetHistory);
            }
            else if (assetHistory.Operation == 2)
            {
                status = WebAPI.Models.AssetHistory.updateAssetHistory(assetHistory);
            }
            else if (assetHistory.Operation == 3)
            {
                status = WebAPI.Models.AssetHistory.deleteAssetHistory(assetHistory);
            } else if (assetHistory.Operation == 4)
            {
                status = "Success";
            }

            //status = WebAPI.Models.TrendFund.testContext();
            var jsonNew = new
            {
                result = status
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}
