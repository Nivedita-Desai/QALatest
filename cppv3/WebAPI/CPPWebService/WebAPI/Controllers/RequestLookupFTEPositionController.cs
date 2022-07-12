using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Web;
using System.Web.Http;
using WebAPI.Models;
//using System.Web.Script.Serialization;

namespace WebAPI.Controllers
{
      [Authorize]
    public class RequestLookupFTEPositionController : System.Web.Http.ApiController
    {
        //
        // GET: /RequestLookupFTEPosition/
        public HttpResponseMessage Get(int Id = -1, String PositionDescription = "null")
        {
            List<FTEPosition> FTEPositionList = new List<FTEPosition>();

            FTEPositionList = WebAPI.Models.FTEPosition.getFTEPosition(Id, PositionDescription);


            var jsonNew = new
            {
                result = FTEPositionList
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
	}
}