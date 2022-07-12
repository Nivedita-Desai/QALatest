using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
  
    public class RequestFundTypeController : System.Web.Http.ApiController
    {
        //
        // GET: /RequestFTECost/
        public HttpResponseMessage Get(String Id = "null",String Fund = "null", String Amount = "null", String avail = "null", String BalRemain ="null")
        {

            List<FundType> FundTypeList = new List<FundType>();
            FundTypeList = WebAPI.Models.FundType.getFundType(Id, Fund, Amount, avail, BalRemain);


            var jsonNew = new
            {
                result = FundTypeList
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }

        public HttpResponseMessage Get(int OrganizationID)
        {

            List<FundType> FundTypeList = new List<FundType>();
            FundTypeList = WebAPI.Models.FundType.getFundType(OrganizationID);


            var jsonNew = new
            {
                result = FundTypeList
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}
