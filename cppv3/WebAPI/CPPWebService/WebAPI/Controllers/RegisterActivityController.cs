using System;
using System.Collections.Generic;
using System.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RegisterActivityController : System.Web.Http.ApiController
    {
        //
        // POST: /RegisterActivity/
      /*  public HttpResponseMessage Post([FromBody] Activity activity)
        {
            String status = "";

            if (activity.Operation == 1)
                status = WebAPI.Models.Activity.registerActivity(activity);

            if (activity.Operation == 2)
                status = WebAPI.Models.Activity.updateActivity(activity);

            if (activity.Operation == 3)
                status = WebAPI.Models.Activity.deleteActivity(activity);


            var jsonNew = new
            {
                result = status
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
        */


       public HttpResponseMessage Post([FromBody] Activity activity)
        {

            //DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Activity));
            //MemoryStream stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(act.ToString()));
            //Activity activity = (Activity)ser.ReadObject(stream);
            
            
            
            String status = "";

            if (activity.Operation == 1)
                status = WebAPI.Models.Activity.registerActivity(activity);

            if (activity.Operation == 2)
                status = WebAPI.Models.Activity.updateActivity(activity);

            if (activity.Operation == 3)
                status = WebAPI.Models.Activity.deleteActivity(activity);
            if (activity.Operation == 4)
                status = WebAPI.Models.Activity.updateTaskDate(activity);

            var jsonNew = new
            {
                result = status
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }

       

    }
}