using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RegisterTrendFundController : System.Web.Http.ApiController
    {
        public HttpResponseMessage Post([FromBody] List<TrendFund> trendFund)
        {

            String status = "";
            foreach (var trend in trendFund)
            {
                if (trend.Operation == 1)
                    status = WebAPI.Models.TrendFund.registerTrendFund(trend);

                if (trend.Operation == 2)
                    status = WebAPI.Models.TrendFund.updateTrendFund(trend);

                if (trend.Operation == 3)
                    status = WebAPI.Models.TrendFund.deleteTrendFund(trend);

                if (trend.Operation == 4)
                    status = "Success";

            }
            var jsonNew = new
            {
                result = status
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }

    }
}
