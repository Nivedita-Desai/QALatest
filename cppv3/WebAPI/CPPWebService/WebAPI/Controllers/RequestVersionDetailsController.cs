using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RequestVersionDetailsController : ApiController
    {

        public HttpResponseMessage Get(String operation = "null",String programElementID = "null", String organizationID = "null")
        {

            List<Versionmaster> projectVersionDetails = new List<Versionmaster>();
            if (operation == "1")
            {
                projectVersionDetails = WebAPI.Models.Versionmaster.getProjectVersionDetails(programElementID);
            }
            else
            {
                if(organizationID != "null")
                {
                    if (organizationID != "0")
                    {
                        projectVersionDetails = WebAPI.Models.Versionmaster.getVersionDetailsByOrgID(organizationID);
                    }
                    else
                    {
                        projectVersionDetails = WebAPI.Models.Versionmaster.getVersionDetails();
                    }
                }
                 
                else
                    projectVersionDetails = WebAPI.Models.Versionmaster.getVersionDetails();
            }

            var jsonNew = new
            {
                result = projectVersionDetails
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }


    }
}
