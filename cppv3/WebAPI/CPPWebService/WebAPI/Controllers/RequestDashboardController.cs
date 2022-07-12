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
    //[Authorize]
    public class RequestDashboardController : System.Web.Http.ApiController
    {
        //
        // GET: /Project/
        public HttpResponseMessage Get(String Command, String ID)
        {
            if (Command == "GetTrends")
            {
                var v = WebAPI.Models.Trend.getTrend("null", "null", ID, "null", "null");
                var jsonNew = new
                {
                    result = v
                };
                return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
            }
            else if (Command == "GetProjects")
            {

                var v = WebAPI.Models.Project.projectByOid(ID);
                var jsonNew = new
                {
                    result = v
                };
                return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
            }
            else{

                var v = "error";
                var jsonNew = new
                {
                    result = v
                };
                return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
            }
            
        }
    }
}