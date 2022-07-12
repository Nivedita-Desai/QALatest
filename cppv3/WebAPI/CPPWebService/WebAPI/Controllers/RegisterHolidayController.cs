using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RegisterHolidayController : ApiController
    {
        public HttpResponseMessage Post([FromBody] List<Holidays> holidays_list)
        {

            String status = "";
            foreach (var holidays in holidays_list)
            {

                if (holidays.Operation == 1)
                    status += WebAPI.Models.Holidays.registerHolidays(holidays);

                if (holidays.Operation == 2)
                    status += WebAPI.Models.Holidays.updateHolidays(holidays);

                if (holidays.Operation == 3)
                    status += WebAPI.Models.Holidays.deleteHolidays(holidays);
                if (holidays.Operation == 5)
                {
                    List<Holidays> holidaysList = new List<Holidays>();
                    
                    holidaysList = WebAPI.Models.Holidays.RegisterFetchPreviousPermanentHolidays(holidays);

                    {
                        var jsonNew = new
                        {
                            result = holidaysList
                        };
                        return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
                    }
                }


                //4 Do nothing
                if (holidays.Operation == 4)
                    status += "";
            }

            var jsonNew1 = new
            {
                result = status
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew1);
        }
    }
}
