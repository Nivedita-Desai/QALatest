using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;
namespace WebAPI.Controllers
{
    public class RequestLobPhaseController : ApiController
    {
        public HttpResponseMessage GET(int lobID)
        {

            List<PhaseCode> lobPhases = new List<PhaseCode>();

            using(var ctx = new CPPDbContext())
            {
                lobPhases = ctx.Database.SqlQuery<PhaseCode>("CALL get_phase_by_lob(@lobID)",
                                            new MySql.Data.MySqlClient.MySqlParameter("@lobID", lobID)
                                        )  
                                        .ToList();
            }

            var json = new
            {
                result = lobPhases
            };

            return Request.CreateResponse(HttpStatusCode.OK, json);
        }
    }
}
