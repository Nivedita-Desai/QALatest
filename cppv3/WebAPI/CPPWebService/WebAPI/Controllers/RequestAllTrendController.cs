using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RequestAllTrendController : ApiController
    {
        public HttpResponseMessage Get(String ProjectID = "null")
        {


            if(ProjectID == null || ProjectID == "")
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            List<Trend> TrendList = WebAPI.Models.Trend.getAllTrendsByProjectID(ProjectID);
            //Nivedita22-12-2021
            Trend TrendBaseline = TrendList.Where(a => a.TrendDescription == "Baseline" && a.TrendNumber == "0").FirstOrDefault();
            int BaselineTrendStatusID = TrendBaseline.TrendStatusID;

            //Manasi 09-11-2020
            foreach (var item in TrendList)
            {
                if (item.TrendDescription == "Current" && item.TrendNumber == "1000")
                {
                    if (BaselineTrendStatusID == 1)
                    {
                        item.TrendDescription = "Budget vs Actual";
                    }
                    else
                    {
                        TrendList.Remove(item);
                    }
                    break;
                }
            }

            Trend forecast = new Trend()
            {
                TrendID = 2000,
                TrendNumber = "2000",
                TrendDescription = "Forecast",
                TrendStatusID = 3 //unapproved

            };

            Trend current = new Trend()
            {
                TrendID = 3000,
                TrendNumber = "3000",
                TrendDescription = "Current",
                TrendStatusID = 3 //unapproved

            };

            TrendList.Add(current);
            TrendList.Add(forecast);
            foreach(Trend trend in TrendList)
            {
                if(trend.TrendNumber != "0" && trend.TrendNumber != "1000" && trend.TrendNumber != "2000" && trend.TrendNumber != "3000")
                    trend.TrendDescription = "Trend " + trend.TrendNumber;
            }

            var jsonNew = new
            {
                result = TrendList
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }

    }
}
