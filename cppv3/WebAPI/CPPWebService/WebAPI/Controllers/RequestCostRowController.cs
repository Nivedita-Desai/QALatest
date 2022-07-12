using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RequestCostRowController : ApiController
    {

        public HttpResponseMessage Get(int ActivityID, String Granularity, String LineID, String CostType)  //int LineID   Manasi 13-08-2020
        {

            object status = "";
            List<CostRow> rows = new List<CostRow>();
            CostRow obj = new CostRow();
            if(CostType == "FTE" ||  CostType == "F")
            {
                List<CostFTE> fteCosts = CostFTE.getCostRow(ActivityID, Granularity, LineID, CostType);
                rows = obj.convertFromFTE(fteCosts);
            }else if(CostType == "L")
            {
                List<CostLumpsum> lumpsumCosts = CostLumpsum.getCostRow(ActivityID.ToString(), Granularity, LineID.ToString(), CostType);
                rows = obj.convertFromLumpsum(lumpsumCosts);
            }else if(CostType == "ODC")
            {
                List<CostODC> odcCosts = CostODC.getCostRow(ActivityID.ToString(), Granularity, LineID.ToString(), CostType);
                rows = obj.convertFromODC(odcCosts);
            }else if(CostType == "U")
            {
                List<CostUnit> materialCosts = CostUnit.getCostRow(ActivityID.ToString(), Granularity, LineID.ToString(), CostType);
                rows = obj.convertFromMaterial(materialCosts);
            }


            var jsonNew = new
            {
                result = rows
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}
