using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RequestClientController : ApiController
    {
        public HttpResponseMessage Get()
        {

            List<Client> clientList = new List<Client>();
            clientList = WebAPI.Models.Client.getClient();


            var jsonNew = new
            {
                result = clientList
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}
