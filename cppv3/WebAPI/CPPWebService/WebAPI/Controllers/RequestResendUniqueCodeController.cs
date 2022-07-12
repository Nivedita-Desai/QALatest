using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RequestResendUniqueCodeController : ApiController
    {
        // GET: RequestResendUniqueCode
        public HttpResponseMessage Get(int ProjectID, int TrendId, String UserID, string UniqueCode)
        {
            var ctx = new CPPDbContext();
            WebAPI.Services.MailServices.EmailToGetTrendApprovedByCode(UserID, Convert.ToString(TrendId), Convert.ToString(ProjectID), UniqueCode,true);
           
            var jsonNew = new
            {
                result = "Code resended to user"
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}