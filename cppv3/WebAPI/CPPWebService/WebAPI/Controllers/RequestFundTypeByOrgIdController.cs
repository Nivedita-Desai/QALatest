using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RequestFundTypeByOrgIdController : System.Web.Http.ApiController
    {
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
