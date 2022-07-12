using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RequestMaterialController : ApiController
    {
        public HttpResponseMessage Get()
        {

            List<Material> materialList = new List<Material>();
            materialList = WebAPI.Models.Material.getMaterial();


            var jsonNew = new
            {
                result = materialList
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}
