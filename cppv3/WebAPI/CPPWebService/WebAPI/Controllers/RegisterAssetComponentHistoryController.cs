using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RegisterAssetComponentHistoryController : ApiController
    {
        public HttpResponseMessage Post([FromBody] AssetComponentHistory assetComponentHistory)
        {
            String status = "";
            //String status;

            if (assetComponentHistory.Operation == 1)
            {
                status = WebAPI.Models.AssetComponentHistory.registerAssetComponentHistory(assetComponentHistory);
            }
            else if (assetComponentHistory.Operation == 2)
            {
                status = WebAPI.Models.AssetComponentHistory.updateAssetComponentHistory(assetComponentHistory);
            }
            else if (assetComponentHistory.Operation == 3)
            {
                status = WebAPI.Models.AssetComponentHistory.deleteAssetComponentHistory(assetComponentHistory);
            } else if (assetComponentHistory.Operation == 4)
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
