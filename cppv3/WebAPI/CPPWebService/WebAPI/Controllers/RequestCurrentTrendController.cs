using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;
namespace WebAPI.Controllers
{
    public class RequestCurrentTrendController : ApiController
    {

        public HttpResponseMessage Get(int ProjectID, String Granularity)
        {

            //DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Activity));
            //MemoryStream stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(act.ToString()));
            //Activity activity = (Activity)ser.ReadObject(stream);



            object status = "";


            //         status = WebAPI.Models.CurrentTrend.GetCurrentProject(ProjectID, Granularity);

            //  status = WebAPI.Models.CurrentTrend.GetCurrentTrend(ProjectID, Granularity);
            //for(int i = 0; i < 20; i++)
            //{
                WebAPI.Models.CurrentTrendTest.mergeTrends(ProjectID);
            //}
           
            //status = "";
        

            var jsonNew = new
            {
                result = status
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}
