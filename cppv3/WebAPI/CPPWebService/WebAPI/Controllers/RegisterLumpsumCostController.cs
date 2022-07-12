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
    public class RegisterLumpsumCostController : System.Web.Http.ApiController
    {
        //
        // GET: /RegisterLumpsumCost/
        //public HttpResponseMessage Get(int Operation, String ProgramID = "null", String ProgramElementID = "null", String ProjectID = "null", String TrendNumber = "null", String ActivityID = "null", String LumpsumCostID = "null", String LumpsumCostStartDate = "null", String LumpsumCostEndDate = "null", String LumpsumDescription = "null", String LumpsumCost = "null")
        public HttpResponseMessage Post([FromBody] CostLumpsum LumpsumCost)
        {

            String status = "";
           // status = WebAPI.Models.CostLumpsum.updateCostLumpsum(LumpsumCost.Operation, LumpsumCost.ProgramID, LumpsumCost.ProgramElementID, LumpsumCost.ProjectID, LumpsumCost.TrendNumber, LumpsumCost.ActivityID, LumpsumCost.LumpsumCostID, LumpsumCost.LumpsumCostStartDate, LumpsumCost.LumpsumCostEndDate, LumpsumCost.LumpsumDescription, LumpsumCost.LumpsumCost, LumpsumCost.Granularity);
            
            var jsonNew = new
            {
                result = status
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}