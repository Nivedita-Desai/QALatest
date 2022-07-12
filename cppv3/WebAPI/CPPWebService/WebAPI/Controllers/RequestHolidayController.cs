using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RequestHolidayController : ApiController
    {
        public HttpResponseMessage Get(string year)
        {

            List<Holidays> holidaysList = new List<Holidays>();
            holidaysList = WebAPI.Models.Holidays.getHolidays(year);


            var jsonNew = new
            {
                result = holidaysList
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}
