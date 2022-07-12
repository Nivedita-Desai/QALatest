using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RegisterClientPOCController : ApiController
    {
        public HttpResponseMessage Post([FromBody] List<clientPOC> clientPOC_list)
        {

            String status = "";
            foreach (var client in clientPOC_list)
            {

                if (client.Operation == 1)
                    status += WebAPI.Models.clientPOC.registerClientPOC(client);

                if (client.Operation == 2)
                    status += WebAPI.Models.clientPOC.updateClientPOC(client);

                if (client.Operation == 3)
                    status += WebAPI.Models.clientPOC.deleteClientPOC(client);

                 //4 Do nothing
                if (client.Operation == 4)
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