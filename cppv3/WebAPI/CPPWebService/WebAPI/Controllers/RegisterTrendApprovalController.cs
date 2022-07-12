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
    public class RegisterTrendApprovalController : System.Web.Http.ApiController
    {
        //
        // GET: /RegisterTrendApproval/
        public HttpResponseMessage Get(String ProjectID, String TrendNumber, String TrendStatus, String ApprovedBy, String ApprovedDate, double TrendTotalValue)
        {
            String status = "";

            //Submitted for approval
            //if (TrendStatus == "5")  
            //{
            //    status = WebAPI.Models.Trend.submitForApproval(int.Parse(ProjectID), TrendNumber, int.Parse(TrendStatus), ApprovedBy, ApprovedDate, TrendTotalValue);
            //}
            //Approved
            if (TrendStatus == "1")
            {
                status = WebAPI.Models.Trend.approveTrend(int.Parse(ProjectID), TrendNumber, int.Parse(TrendStatus), ApprovedBy, ApprovedDate, TrendTotalValue);
            }
            //Rejected
            else if (TrendStatus == "2")
                status = WebAPI.Models.Trend.rejectTrend(int.Parse(ProjectID), TrendNumber, int.Parse(TrendStatus), ApprovedBy, ApprovedDate);
            var jsonNew = new
            {
                result = status
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}