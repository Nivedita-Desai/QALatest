using System;
using System.Collections.Generic;
using System.IO;
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

namespace WebAPI.Controllers
{
    [Authorize]
    public class RegisterLookupTrendStatusController : System.Web.Http.ApiController
    {
        //
        // GET: /RegisterLookupTrendStatus/
       public HttpResponseMessage Get(int Operation, String StatusID, String StatusDescription)
       {
           
            String status = "";
           /*
            if ( Operation == 1)
                status = WebAPI.Models.TrendStatus.registerTrendStatus(StatusID, StatusDescription);

            if (Operation == 2)
                status = WebAPI.Models.TrendStatus.updateTrendStatus(StatusID, StatusDescription);

            if (Operation == 3)
                status = WebAPI.Models.TrendStatus.deleteTrendStatus(StatusID, StatusDescription);
           */

            var jsonNew = new
            {
                result = status
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
	}
}