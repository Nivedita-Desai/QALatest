using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RegisterClientController : ApiController
    {
        public HttpResponseMessage Post([FromBody] List<Client> client_list)
        {

            String status = "";
            foreach (var client in client_list)
            {

                if (client.Operation == 1)
                    status += WebAPI.Models.Client.registerClient(client);

                if (client.Operation == 2)
                    status += WebAPI.Models.Client.updateClient(client);

                if (client.Operation == 3)
                    status += WebAPI.Models.Client.deleteClient(client);

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
