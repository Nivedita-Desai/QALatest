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
    public class RegisterODCCostController : System.Web.Http.ApiController
    {
        //
        // GET: /RegisterUnitCost/
        //public HttpResponseMessage Get(int Operation, String ProgramID = "null", String ProgramElementID = "null", String ProjectID = "null", String TrendNumber = "null", String ActivityID = "null", String UnitCostID = "null", String UnitCostStartDate = "null", String UnitCostEndDate = "null", String UnitDescription = "null", String UnitQuantity = "null", String UnitPrice = "null", String UnitCost = "null")
        public HttpResponseMessage Post([FromBody] CostODC ODCCost)
        {

            String status = "";


            // status = WebAPI.Models.CostUnit.updateCostUnit(UnitCost.Operation, UnitCost.ProgramID, UnitCost.ProgramElementID, UnitCost.ProjectID, UnitCost.TrendNumber, UnitCost.ActivityID, UnitCost.UnitCostID, UnitCost.UnitCostStartDate, UnitCost.UnitCostEndDate, UnitCost.UnitDescription, UnitCost.UnitQuantity, UnitCost.UnitPrice, UnitCost.Granularity, UnitCost.UnitType);


            var jsonNew = new
            {
                result = status
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}