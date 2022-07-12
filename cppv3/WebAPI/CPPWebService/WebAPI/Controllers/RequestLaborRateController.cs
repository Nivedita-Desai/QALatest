using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebAPI.Controllers
{
    public class RequestLaborRateController : ApiController
    {
        public HttpResponseMessage Get(int TrendID, int PositionID, int EmployeeID)
        {
            Double laborRate = 0.0;
            using (var ctx = new CPPDbContext())
            {
                laborRate = ctx.Database.SqlQuery<Double>("Call get_labor_rate(@PositionID, @EmployeeID, @TrendID)",
                                            new MySql.Data.MySqlClient.MySqlParameter("@PositionID", PositionID),
                                              new MySql.Data.MySqlClient.MySqlParameter("@EmployeeID", EmployeeID),
                                                new MySql.Data.MySqlClient.MySqlParameter("@TrendID", TrendID)
                                            ).FirstOrDefault();
            }


            var jsonNew = new
            {
                result = laborRate
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}
