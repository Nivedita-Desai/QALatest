using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RequestSubAcativityCategoryController : ApiController
    {
        public HttpResponseMessage Get(String CategoryID,String s = "nulll")
        {


            List<ActivityCategory> MainActivityCategoryList = WebAPI.Models.ActivityCategory.getSubCategory( CategoryID,s);


            var jsonNew = new
            {
                result = MainActivityCategoryList
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}
