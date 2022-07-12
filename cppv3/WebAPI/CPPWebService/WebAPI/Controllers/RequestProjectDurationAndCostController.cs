using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RequestProjectDurationAndCostController : ApiController
    {

        public HttpResponseMessage Get(int ProjectID, int TrendID)
        {

            List<List<String>> obj = new List<List<String>>();
           // obj = WebAPI.Models.Activity.getActivityByID(ActivityID);

            obj = getProjectDuartionAndCost(ProjectID, TrendID);
            var jsonNew = new
            {
                result = obj
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }

        public static List<List<String>> getProjectDuartionAndCost(int ProjectID, int TrendID)
        {
            List<List<String>> obj = new List<List<String>>();
            List<String> baselineObj = new List<String>();
            List<String> forecastObject = new List<String>();
            List<String> currentObject = new List<String>();


            baselineObj = getBaselineInfo(ProjectID, TrendID);
            currentObject = WebAPI.Models.Trend.getCurrentTrendDate(ProjectID.ToString());
            forecastObject = WebAPI.Models.Trend.getForeCostTrendDate(ProjectID);

            obj.Add(baselineObj);
            obj.Add(currentObject);
            obj.Add(forecastObject);


            return obj;
        }


        public static List<String> getBaselineInfo(int ProjectID, int TrendID)
        {
            List<String> baselineObj = new List<String>();

            Logger.LogDebug(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            MySqlConnection conn = null;
            MySqlDataReader reader = null;
            //Baseline
            try
            {
                // create and open a connection object
                conn = ConnectionManager.getConnection();
                conn.Open();

                String query = "SELECT  PostTrendStartDate, PostTrendEndDate,PostTrendCost from trend";
                query += " WHERE 1=1";
                query += " AND ProjectID = " + ProjectID;
                query += " And TrendNumber = 0";
                MySqlCommand command = new MySqlCommand(query, conn);
                

                using (reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        baselineObj.Add(reader.GetValue(0).ToString().Trim());
                        baselineObj.Add(reader.GetValue(1).ToString().Trim());
                        baselineObj.Add(reader.GetValue(2).ToString().Trim());

                    }
                }
            }
            catch (Exception ex)
            {
                var stackTrace = new StackTrace(ex, true);
                var line = stackTrace.GetFrame(0).GetFileLineNumber();
                Logger.LogExceptions(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, ex.Message, line.ToString(), Logger.logLevel.Exception);
            }
            finally
            {
                if (conn != null) conn.Close();
                if (reader != null) reader.Close();

            }
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Exit Point", Logger.logLevel.Debug);

            return baselineObj;
        }
    }
}
