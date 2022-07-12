using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RequestSubACController : ApiController
    {

        public HttpResponseMessage Get(int OrganizationID, String Phase,String CategoryID, String VersionId,String ProjectId)
        {


            List<ActivityCategory> MainActivityCategoryList = WebAPI.Models.ActivityCategory.getSubCategory(OrganizationID, CategoryID, Phase, VersionId,ProjectId);


            var jsonNew = new
            {
                result = MainActivityCategoryList
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}
