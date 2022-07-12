using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RegisterAssetSupplierController : ApiController
    {
        public HttpResponseMessage Post([FromBody] AssetSupplier assetSupplier)
        {
            String status = "";
            //String status;

            if (assetSupplier.Operation == 1)
            {
                status = WebAPI.Models.AssetSupplier.registerAssetSupplier(assetSupplier);
            }
            else if (assetSupplier.Operation == 2)
            {
                status = WebAPI.Models.AssetSupplier.updateAssetSupplier(assetSupplier);
            }
            else if (assetSupplier.Operation == 3)
            {
                status = WebAPI.Models.AssetSupplier.deleteAssetSupplier(assetSupplier);
            } else if (assetSupplier.Operation == 4)
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
