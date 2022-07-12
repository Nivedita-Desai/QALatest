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
    public class RegisterLookupFTEPositionController : System.Web.Http.ApiController
    {
     
        public HttpResponseMessage Post([FromBody] List<FTEPosition> fte_position_list)
        {

            String status = "";
            foreach (var fte_position in fte_position_list)
            {
                if (fte_position.Operation == 1)
                    status += WebAPI.Models.FTEPosition.registerFTEPosition(fte_position);

                if (fte_position.Operation == 2)
                    status += WebAPI.Models.FTEPosition.updateFTEPosition(fte_position);

                if (fte_position.Operation == 3)
                    status += WebAPI.Models.FTEPosition.deleteFTEPosition(fte_position);

                if (fte_position.Operation == 4)
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