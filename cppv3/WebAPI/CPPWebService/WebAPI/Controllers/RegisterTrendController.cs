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
    public class RegisterTrendController : System.Web.Http.ApiController
    {
        //
        // POST: /RegisterTrend/
        public HttpResponseMessage Post([FromBody] Trend trend)
        {
            String status = "";

            if (trend.Operation == 1)
                status = WebAPI.Models.Trend.registerTrend(trend);

            if (trend.Operation == 5)   //Updating the trend setup only
                status = WebAPI.Models.Trend.updateTrendSetup(trend);

            if (trend.Operation == 2)
            {
                status = WebAPI.Models.Trend.updateTrend(trend);

                if (status == "Official Approved")      //done approving
                {
                    WebAPI.Models.CurrentTrendTest.mergeTrends(trend.ProjectID);
                    status = "Approval is successful.";
                }
            }

            if(trend.Operation == 5)    //Update trend status code only
            {
                status = WebAPI.Models.Trend.updateTrendStatusCode(trend);
            }

            if (trend.Operation == 3)
                status = WebAPI.Models.Trend.deleteTrend(trend);
            if (trend.Operation == 4)
            {
                status = WebAPI.Models.Trend.updateTrendDate(trend);

            }
            //----Vaishnavi 30-03-2022----//
            if (trend.Operation == 6)
            {
                status = WebAPI.Models.Trend.closeTrend(trend);

            }
            //----Vaishnavi 30-03-2022----//

            var jsonNew = new
            {
                result = status
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
	}
}