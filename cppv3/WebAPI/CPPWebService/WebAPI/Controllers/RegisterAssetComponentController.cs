using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RegisterAssetComponentController : ApiController
    {
        public HttpResponseMessage Post([FromBody] AssetComponent assetComponent)
        {
            String status = "";
            //String status;

            if (assetComponent.Operation == 1)
            {
                status = WebAPI.Models.AssetComponent.registerAssetComponent(assetComponent);
            }
            else if (assetComponent.Operation == 2)
            {
                status = WebAPI.Models.AssetComponent.updateAssetComponent(assetComponent);
            }
            else if (assetComponent.Operation == 3)
            {
                status = WebAPI.Models.AssetComponent.deleteAssetComponent(assetComponent);
            }
            else if (assetComponent.Operation == 4)
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
