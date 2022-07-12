using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RegisterFundTypeController : ApiController
    {
     
            //
            // GET: /RegisterLookupPhaseCode/
            //public HttpResponseMessage Get(int Operation, String PhaseID, String PhaseDescription, String PhaseCode)
            public HttpResponseMessage Post([FromBody] List<FundType> fund_type_list)
            {
                String status = "";
                foreach (var fund_type in fund_type_list)
                {
                    if (fund_type.Operation == 1)
                        status += WebAPI.Models.FundType.registerFundType(fund_type);

                    if (fund_type.Operation == 2)
                        status += WebAPI.Models.FundType.updateFundType(fund_type);

                    if (fund_type.Operation == 3)
                        status += WebAPI.Models.FundType.deleteFundType(fund_type);

                    if (fund_type.Operation == 4)
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
