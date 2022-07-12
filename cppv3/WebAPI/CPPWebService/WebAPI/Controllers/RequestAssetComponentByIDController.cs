using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;
namespace WebAPI.Controllers
{
    public class RequestAssetComponentByIDController : ApiController
    {

        public HttpResponseMessage get(int AssetComponentID)
        {
            List<AssetComponent> assetList = new List<AssetComponent>();
            List<JObject> jsonList = new List<JObject>();
            //String status;
            assetList = WebAPI.Models.AssetComponent.getAssetComponentByID(AssetComponentID);
            //status = WebAPI.Models.TrendFund.testContext();
            //var jsonNew = new
            //{
            //    result = status
            //};
            //return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
            foreach (var t in assetList)
            {
                var s = JsonConvert.SerializeObject(t, Formatting.Indented,
                         new JsonSerializerSettings()
                         {
                             ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                         });
                JObject json = JObject.Parse(s);
                jsonList.Add(json);
            }
            return Request.CreateResponse(HttpStatusCode.OK, jsonList);
        }
    }
}
