using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;
namespace WebAPI.Controllers
{
    public class RequestMaterialCategoryController : ApiController
    {
        public HttpResponseMessage Get()
        {

            List<MaterialCategory> materialCategoryList = new List<MaterialCategory>();
            materialCategoryList = WebAPI.Models.MaterialCategory.getMaterialCategory();


            var jsonNew = new
            {
                result = materialCategoryList
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}
