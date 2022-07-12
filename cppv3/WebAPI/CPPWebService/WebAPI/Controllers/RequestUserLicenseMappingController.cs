using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RequestUserLicenseMappingController : ApiController
    {
        
       /* public HttpResponseMessage Get()
        {
            List<User_LicenseMapping> UserLicenseMappingList = new List<User_LicenseMapping>();
            UserLicenseMappingList = WebAPI.Models.User_LicenseMapping.GetUser_LicenseMappings();


            var jsonNew = new
            {
                result = UserLicenseMappingList
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }*/

        public HttpResponseMessage Get(String userName)
        {
            List<User_LicenseMapping> UserLicenseMappingList = new List<User_LicenseMapping>();
            UserLicenseMappingList = WebAPI.Models.User_LicenseMapping.GetUserLicenseMappingById(userName);


            var jsonNew = new
            {
                result = UserLicenseMappingList
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}