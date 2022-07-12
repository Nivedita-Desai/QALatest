using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RegisterTrendStatusCodeController : ApiController
    {
        public HttpResponseMessage Post([FromBody] List<TrendStatusCode> trend_status_code_list)
        {

            String status = "";
            foreach (var trend_status_code in trend_status_code_list)
            {

                if (trend_status_code.Operation == 1)
                    status += WebAPI.Models.TrendStatusCode.registerTrendStatusCode(trend_status_code);

                if (trend_status_code.Operation == 2)
                    status += WebAPI.Models.TrendStatusCode.updateTrendStatusCode(trend_status_code);

                if (trend_status_code.Operation == 3)
                    status += WebAPI.Models.TrendStatusCode.deleteTrendStatusCode(trend_status_code);

                //4 Do nothing
                if (trend_status_code.Operation == 4)
                    status += "";
            }

            var jsonNew = new
            {
                result = status
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}
