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
    public class RegisterFTECostController : System.Web.Http.ApiController
    {
        //
        // GET: /RegisterFTECost/
        //public HttpResponseMessage Get(int Operation, FTECost.String ProgramID = "null", FTECost.String ProgramElementID = "null", FTECost.String ProjectID = "null", FTECost.String TrendNumber = "null", FTECost.String ActivityID = "null", FTECost.String FTECostID = "null", FTECost.String FTEStartDate = "null", FTECost.String FTEEndDate = "null", FTECost.String FTEPosition = "null", FTECost.String FTEValue = "null", FTECost.String FTEHourlyRate = "null", FTECost.String FTEHours = "null", FTECost.String FTECost = "null")
        public HttpResponseMessage Post([FromBody] CostFTE FTECost)
        {

            String status = "";


        //    status = WebAPI.Models.CostFTE.updateCostFTE(FTECost.Operation, FTECost.ProgramID, FTECost.ProgramElementID, FTECost.ProjectID, FTECost.TrendNumber, FTECost.ActivityID,  FTECost.FTEStartDate, FTECost.FTEEndDate, FTECost.FTEPosition, FTECost.FTEValue, FTECost.FTEHourlyRate, FTECost.FTEHours, FTECost.FTECost, FTECost.Granularity);


            var jsonNew = new
            {
                result = status
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}