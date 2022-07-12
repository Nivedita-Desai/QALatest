using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RequestTrendsForChangeOrderController : ApiController
    {
        public HttpResponseMessage Get(String ProjectID = "null")
        {


            if (ProjectID == null || ProjectID == "")
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            //List<Trend> TrendList = WebAPI.Models.Trend.getAllTrendsByProjectID(ProjectID);
            List<Trend> TrendList = WebAPI.Models.Trend.GetAllTrendsForChangeOrder(ProjectID);// Jignesh-TDM-06-01-2020

            var jsonNew = new
            {
                result = TrendList
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}
