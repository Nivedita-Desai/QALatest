using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RequestSubActivityCategoryProgramController : ApiController
    {
        public HttpResponseMessage Get(int ProgramID, String Phase, String CategoryID)
        {

            List<ActivityCategory> SubActivityCategoryList = WebAPI.Models.ActivityCategory.getSubCategoryByProgram(ProgramID, Phase, CategoryID);


            var jsonNew = new
            {
                result = SubActivityCategoryList
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}
