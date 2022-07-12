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
    public class RequestClientPOCController : ApiController
    {
        public HttpResponseMessage Get()            //Tanmay - 15/12/2021
        {
            List<clientPOC> clientList = new List<clientPOC>();
            clientList = WebAPI.Models.clientPOC.getClientPOC();

            var jsonNew = new
            {
                result = clientList
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}