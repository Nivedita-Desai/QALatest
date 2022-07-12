using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RegisterAssetController : System.Web.Http.ApiController
    {
        public HttpResponseMessage Post([FromBody] Asset asset)
        {
            String status = "";
            //String status;

                if (asset.Operation == 1)
                {
                    status = WebAPI.Models.Asset.registerAsset(asset);
                }
                 else if (asset.Operation == 2)
                {
                    status = WebAPI.Models.Asset.updateAsset(asset);
                }
                 else if (asset.Operation == 3)
                {
                    status = WebAPI.Models.Asset.deleteAsset(asset);
                } else if (asset.Operation == 4)
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
