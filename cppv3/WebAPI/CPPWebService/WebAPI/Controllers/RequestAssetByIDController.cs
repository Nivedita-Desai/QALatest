using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace WebAPI.Controllers
{
    public class RequestAssetByIDController : ApiController
    {

        public HttpResponseMessage Get(int AssetID)
        {
            object status;
            //String status;
            status = WebAPI.Models.Asset.getAssetByID(AssetID);
            //status = WebAPI.Models.TrendFund.testContext();
            //var jsonNew = new
            //{
            //    result = status
            //};

            var i = JsonConvert.SerializeObject(status, Formatting.Indented,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
            var jsonNew = JArray.Parse(i);
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }

    }
}
