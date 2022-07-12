using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RequestMainActivityCategoryProgramController : ApiController
    {
        public HttpResponseMessage Get(int ProgramID, String Phase)
        {

            List<ActivityCategory> MainActivityCategoryList = WebAPI.Models.ActivityCategory.getMainCategoryByProgram(ProgramID, Phase);


            var jsonNew = new
            {
                result = MainActivityCategoryList
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}
