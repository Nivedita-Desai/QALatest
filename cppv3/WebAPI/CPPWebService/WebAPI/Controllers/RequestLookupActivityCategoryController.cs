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
//using System.Web.Script.Serialization;
namespace WebAPI.Controllers
{
    [Authorize]
    public class RequestLookupActivityCategoryController : System.Web.Http.ApiController
    {
        //
        // GET: /RequestLookupActivityCategory/
        public HttpResponseMessage Get(String CategoryID = "null", String CategoryDescription = "null", String SubCategoryID = "null", String SubCategoryDescription = "null")
        {


            List<ActivityCategory> ActivityCategoryList = WebAPI.Models.ActivityCategory.getActivityCategory(CategoryID, CategoryDescription, SubCategoryID, SubCategoryDescription);


            var jsonNew = new
            {
                result = ActivityCategoryList
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }

        public HttpResponseMessage GetByOrgID(int OrganizationID, int VersionID)
        {


            List<ActivityCategory> ActivityCategoryList = WebAPI.Models.ActivityCategory.getActivityCategoryByOrgID(OrganizationID, VersionID);


            var jsonNew = new
            {
                result = ActivityCategoryList
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}