using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RequestMainACController : ApiController
    {

        public HttpResponseMessage Get(String Phase, String OrganizationID, String ProjectId)
        {


            List<ActivityCategory> SubActivityCategoryList = WebAPI.Models.ActivityCategory.getMainCategory(Phase, OrganizationID, ProjectId);


            var jsonNew = new
            {
                result = SubActivityCategoryList
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}
