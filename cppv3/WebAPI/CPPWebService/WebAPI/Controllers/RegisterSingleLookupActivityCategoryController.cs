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
    public class RegisterSingleLookupActivityCategoryController : System.Web.Http.ApiController
    {
        //
        // GET: /RegisterLookupActivityCategory/
        //public HttpResponseMessage Get(int Operation, String CategoryID, String CategoryDescription, String SubCategoryID, String SubCategoryDescription)
        public HttpResponseMessage Post([FromBody] List<ActivityCategory> act_category_list)
        {
            try
            {
                String status = "";
                foreach (var act_category in act_category_list)
                {
                    if (act_category.Operation == 1)
                        status += WebAPI.Models.ActivityCategory.registerSingleActivityCategory(act_category);
                }
                var jsonNew = new
                {
                    result = status
                };
                return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
            }
            catch (Exception ex)
            {

                throw ex;

            }




        }
    }
}