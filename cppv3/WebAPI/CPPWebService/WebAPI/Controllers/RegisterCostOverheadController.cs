using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RegisterCostOverheadController : ApiController
    {
        public HttpResponseMessage Post([FromBody] List<CostOverhead> costOverheadList)
        {

            String status = "";
            foreach (var costOverhead in costOverheadList)
            {

                if (costOverhead.Operation == 1)
                    status += WebAPI.Models.CostOverhead.registerCostOverhead(costOverhead);


                if (costOverhead.Operation == 2)
                    status += WebAPI.Models.CostOverhead.updateCostOverhead(costOverhead);

                if (costOverhead.Operation == 3)
                    status += WebAPI.Models.CostOverhead.deleteCostOverhead(costOverhead);

                //4 Do nothing
                if (costOverhead.Operation == 4)
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
