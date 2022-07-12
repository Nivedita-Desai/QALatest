using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RegisterUserLicenseMappingController : ApiController
    {
      
        public HttpResponseMessage Post([FromBody] User_LicenseMapping userLicenseMapping)
        {

            String status = "";
            // foreach (var userLicenseMapping in UserLicenseMappingList)
            // {
            try { 
                /*var jobject = objects.Children();

                var first = jobject.FirstOrDefault();

                var userLicesneStr = first.FirstOrDefault().ToString();*/
               // var userLicenseMapping = JsonConvert.DeserializeObject<User_LicenseMapping>(userLicesneStr);
                if (userLicenseMapping.Operation == 1)
                    status += WebAPI.Models.User_LicenseMapping.registerUser_LicenseMapping(userLicenseMapping);

                if (userLicenseMapping.Operation == 2)
                    status += WebAPI.Models.User_LicenseMapping.updateUser_LicenseMapping(userLicenseMapping);

                if (userLicenseMapping.Operation == 3)
                    status += WebAPI.Models.User_LicenseMapping.deleteUser_LicenseMapping(userLicenseMapping);

                //4 Do nothing
                if (userLicenseMapping.Operation == 4)
                    status += "";
            //}
        }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
    var jsonNew = new
            {
                result = status
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}