using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Web;
using WebAPI.Controllers;

namespace WebAPI.Models
{
    public class CurrentTrend
    {
        public static int MaxFCostID = 0;
        public static int MaxLCostID = 0;
        public static int MaxUCostID = 0;
        public static int MaxPCostID = 0;
        public static Trend getBaselineTrend(MySqlConnection conn, MySqlDataReader reader, int ProjectID)
        {
            Trend baselineTrend = new Trend();
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);

            conn = null;
            reader = null;
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    baselineTrend = ctx.Trend.Where(t => t.TrendNumber == "0" && t.ProjectID == ProjectID).FirstOrDefault();
                }
                // create and open a connection object
                //conn = ConnectionManager.getConnection();
                //conn.Open();

                //String query = "SELECT * FROM trend";
                //query += " WHERE 1=1 AND TrendNumber = 0 And ProjectID = " + ProjectID;


                //MySqlCommand command = new MySqlCommand(query, conn);
                //Nullable<DateTime> dt = null; //assign this if date is null

                //using (reader = command.ExecuteReader())
                //{
                //    while (reader.Read())
                //    {
                //        baselineTrend.TrendNumber = reader.GetValue(0).ToString().Trim();
                //        baselineTrend.ProjectID = Convert.ToInt16((reader.GetValue(1).ToString().Trim()));

                //        baselineTrend.TrendDescription = reader.GetValue(2).ToString().Trim();
                //        baselineTrend.TrendJustification = reader.GetValue(3).ToString().Trim();
                //        baselineTrend.TrendImpact = reader.GetValue(4).ToString().Trim();
                //        baselineTrend.CreatedOn = reader.GetValue(5).ToString().Trim();
                //        baselineTrend.ApprovalFrom = reader.GetValue(6).ToString().Trim();
                //        var ApprovalDate = reader.GetValue(7).ToString().Trim();
                //        baselineTrend.ApprovalDate = (ApprovalDate != "") ? Convert.ToDateTime(ApprovalDate) : dt;

                //        baselineTrend.PreTrendCost = reader.GetValue(9).ToString().Trim();
                //        baselineTrend.PostTrendCost = reader.GetValue(10).ToString().Trim();
                //        var preTrendStartDate = reader.GetValue(11).ToString().Trim();
                //        var preTrendEndDate = reader.GetValue(12).ToString().Trim();
                //        baselineTrend.PreTrendStartDate = (preTrendStartDate != "") ? Convert.ToDateTime(preTrendStartDate) : dt;
                //        baselineTrend.PreTrendEndDate = (preTrendEndDate != "") ? Convert.ToDateTime(preTrendEndDate) : dt;

                //        var postTrendStartDate = reader.GetValue(13).ToString().Trim();
                //        var postTrendEndDate = reader.GetValue(14).ToString().Trim();
                //        baselineTrend.PostTrendStartDate = (postTrendStartDate != "") ? Convert.ToDateTime(postTrendStartDate) : dt;
                //        baselineTrend.PostTrendEndDate = (postTrendEndDate != "") ? Convert.ToDateTime(postTrendEndDate) : dt;
                //        baselineTrend.TrendStatusID = Convert.ToInt16(reader.GetValue(15).ToString().Trim());
                //    }
                //}



            }
            catch (Exception ex)
            {
                var stackTrace = new StackTrace(ex, true);
                var line = stackTrace.GetFrame(0).GetFileLineNumber();
                Logger.LogExceptions(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, ex.Message, line.ToString(), Logger.logLevel.Exception);
            }
            finally
            {
                //if (conn != null) conn.Close();
                //if (reader != null) reader.Close();

            }
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Exit Point", Logger.logLevel.Debug);



            return baselineTrend;
        }
        public static List<Trend> getApprovalTrends(MySqlConnection conn, MySqlDataReader reader, int ProjectID)
        {
            List<Trend> approvalTrendList = new List<Trend>();
            Trend baselineTrend = new Trend();
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);

            conn = null;
            reader = null;
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    approvalTrendList = ctx.Trend.Where(t => t.TrendStatusID == 1 && t.TrendNumber != "0" && t.ProjectID == ProjectID).ToList();
                }
                // create and open a connection object
                //conn = ConnectionManager.getConnection();
                //conn.Open();

                //String query = "SELECT * FROM trend";
                //query += " WHERE 1=1 AND TrendStatusID = 1 and TrendNumber <> 0 And ProjectID = " + ProjectID;

                //MySqlCommand command = new MySqlCommand(query, conn);
                //Nullable<DateTime> dt = null; //assign this if date is null
                //using (reader = command.ExecuteReader())
                //{
                //    while (reader.Read())
                //    {
                //        Trend trend = new Trend();
                //        trend.ProjectID = Convert.ToInt16((reader.GetValue(1).ToString().Trim()));
                //        trend.TrendNumber = reader.GetValue(0).ToString().Trim();
                //        trend.TrendDescription = reader.GetValue(2).ToString().Trim();
                //        trend.TrendJustification = reader.GetValue(3).ToString().Trim();
                //        trend.TrendImpact = reader.GetValue(4).ToString().Trim();
                //        trend.CreatedOn = reader.GetValue(5).ToString().Trim();
                //        trend.ApprovalFrom = reader.GetValue(6).ToString().Trim();
                //        var ApprovalDate = reader.GetValue(7).ToString().Trim();
                //        trend.ApprovalDate = (ApprovalDate != "") ? Convert.ToDateTime(ApprovalDate) : dt;

                //        trend.PreTrendCost = reader.GetValue(9).ToString().Trim();
                //        trend.PostTrendCost = reader.GetValue(10).ToString().Trim();
                //        var preTrendStartDate = reader.GetValue(11).ToString().Trim();
                //        var preTrendEndDate = reader.GetValue(12).ToString().Trim();
                //        trend.PreTrendStartDate = (preTrendStartDate != "") ? Convert.ToDateTime(preTrendStartDate) : dt;
                //        trend.PreTrendEndDate = (preTrendEndDate != "") ? Convert.ToDateTime(preTrendEndDate) : dt;

                //        var postTrendStartDate = reader.GetValue(13).ToString().Trim();
                //        var postTrendEndDate = reader.GetValue(14).ToString().Trim();
                //        trend.PostTrendStartDate = (postTrendStartDate != "") ? Convert.ToDateTime(postTrendStartDate) : dt;
                //        trend.PostTrendEndDate = (postTrendEndDate != "") ? Convert.ToDateTime(postTrendEndDate) : dt;
                //        trend.TrendStatusID = Convert.ToInt16(reader.GetValue(15).ToString().Trim());
                //        approvalTrendList.Add(trend);
                //    }
                //}



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
            return approvalTrendList;
        }

        public static Activity getActivityByID(String ActivityID)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            Activity MatchedActivity = new Activity();
            MySqlConnection conn = null;
            MySqlDataReader reader = null;
            try
            {
                var AId = Convert.ToInt16(ActivityID);
                using (var ctx = new CPPDbContext()){
                    MatchedActivity = ctx.Activity.Where(a => a.ActivityID == AId).FirstOrDefault();
                }
                // create and open a connection object
                //conn = ConnectionManager.getConnection();
                //conn.Open();

                //String query = "SELECT * FROM activity";
                //query += " WHERE 1=1";
                //if (ActivityID != "null")
                //    query += " AND ActivityID like '%" + ActivityID + "%'";

                //MySqlCommand command = new MySqlCommand(query, conn);

                //using (reader = command.ExecuteReader())
                //{
                //    while (reader.Read())
                //    {
                //        // Activity RetreivedActivity = new Activity(reader.GetValue(0).ToString().Trim(), reader.GetValue(1).ToString().Trim(), reader.GetValue(2).ToString().Trim(), reader.GetValue(3).ToString().Trim());
                //        MatchedActivity.ActivityID = Convert.ToInt16(reader.GetValue(0).ToString().Trim());
                //        MatchedActivity.ProjectID = Convert.ToInt16(reader.GetValue(1).ToString().Trim());
                //        MatchedActivity.TrendNumber = reader.GetValue(2).ToString().Trim();
                //        MatchedActivity.PhaseCode = Convert.ToInt16(reader.GetValue(3).ToString().Trim());
                //        MatchedActivity.BudgetCategory = reader.GetValue(4).ToString().Trim();
                //        MatchedActivity.BudgetSubCategory = reader.GetValue(5).ToString().Trim();
                //        MatchedActivity.ActivityStartDate = reader.GetValue(6).ToString().Trim();
                //        MatchedActivity.ActivityEndDate = reader.GetValue(7).ToString().Trim();
                //        MatchedActivity.FTECost = Convert.ToDouble(reader.GetValue(8).ToString().Trim());
                //        MatchedActivity.LumpsumCost = Convert.ToDouble(reader.GetValue(9).ToString().Trim());
                //        MatchedActivity.UnitCost = Convert.ToDouble(reader.GetValue(10).ToString().Trim());
                //        MatchedActivity.PercentageBasisCost = Convert.ToDouble(reader.GetValue(11).ToString().Trim());
                //    }
                //}

            }
            catch (Exception ex)
            {
                var stackTrace = new StackTrace(ex, true);
                var line = stackTrace.GetFrame(0).GetFileLineNumber();
                Logger.LogExceptions(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, ex.Message, line.ToString(), Logger.logLevel.Exception);
            }
            finally
            {
                //if (conn != null)
                //{
                //    conn.Close();
                //}
                //if (reader != null)
                //{
                //    reader.Close();
                //}
            }
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Exit Point", Logger.logLevel.Debug);
            return MatchedActivity;

        }
        public static List<Activity> getActivity(MySqlConnection conn, MySqlDataReader reader, int ProjectID, String TrendNumber)
        {
            List<Activity> activityList = new List<Activity>();
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);

            conn = null;
            reader = null;
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    activityList = ctx.Activity.Where(a => a.ProjectID == ProjectID && a.TrendNumber == TrendNumber).ToList();
                }
                // create and open a connection object
                //conn = ConnectionManager.getConnection();
                //conn.Open();

                //String query = "SELECT * FROM activity";
                //query += " WHERE 1=1 AND TrendNumber = '" + TrendNumber + "' And ProjectID = " + ProjectID;


                //MySqlCommand command = new MySqlCommand(query, conn);
                //Nullable<DateTime> dt = null; //assign this if date is null

                //using (reader = command.ExecuteReader())
                //{
                //    while (reader.Read())
                //    {
                //        Activity activity = new Activity();
                //        activity.ActivityID = Convert.ToInt16(reader.GetValue(0).ToString().Trim());
                //        activity.ProjectID = Convert.ToInt16(reader.GetValue(1).ToString().Trim());
                //        activity.TrendNumber = reader.GetValue(2).ToString().Trim();
                //        activity.PhaseCode = Convert.ToInt16(reader.GetValue(3).ToString().Trim());
                //        activity.BudgetCategory = reader.GetValue(4).ToString().Trim();
                //        activity.BudgetSubCategory = reader.GetValue(5).ToString().Trim();
                //        activity.ActivityStartDate = reader.GetValue(6).ToString().Trim();
                //        activity.ActivityEndDate = reader.GetValue(7).ToString().Trim();
                //        activity.FTECost = Convert.ToDouble(reader.GetValue(8).ToString().Trim());
                //        activity.LumpsumCost = Convert.ToDouble(reader.GetValue(9).ToString().Trim());
                //        activity.UnitCost = Convert.ToDouble(reader.GetValue(10).ToString().Trim());
                //        activity.PercentageBasisCost = Convert.ToDouble(reader.GetValue(11).ToString().Trim());
                //        activityList.Add(activity);

                //    }
                //}



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




            return activityList;
        }
        public static List<Cost> getCosts(List<Activity> matchedActivityList, String Granularity)
        {
            int CostNumber = 0;
            List<Cost> matchedCostList = new List<Cost>();

            //List<Activity> matchedActivityList = WebAPI.Models.Activity.getActivityDetails("null", "null", ProjectID, TrendNumber, PhaseCode, ActivityID, "null", "null");



            foreach (Activity ReturnedActivity in matchedActivityList)
            {
                String trendNumber = ""; //Not using this for now
                List<CostFTE> FTECostList = WebAPI.Models.CostFTE.getCostFTE(ReturnedActivity.ActivityID.ToString(), 
                                                                        Granularity, trendNumber,ReturnedActivity.ProjectID.ToString()
                                                                        ,ReturnedActivity.PhaseCode.ToString(),ReturnedActivity.BudgetID.ToString(),ReturnedActivity.BudgetCategory,ReturnedActivity.BudgetSubCategory);
                List<CostLumpsum> LumpsumCostList = WebAPI.Models.CostLumpsum.getCostLumpsum(ReturnedActivity.ActivityID.ToString(), Granularity, trendNumber,
                                                                            ReturnedActivity.ProjectID.ToString(),ReturnedActivity.PhaseCode.ToString(),ReturnedActivity.BudgetID.ToString(),
                                                                            ReturnedActivity.BudgetCategory,ReturnedActivity.BudgetSubCategory);
                List<CostUnit> UnitCostList = WebAPI.Models.CostUnit.getCostUnit(ReturnedActivity.ActivityID.ToString(), Granularity,trendNumber,
                                                                          ReturnedActivity.ProjectID.ToString(), ReturnedActivity.PhaseCode.ToString(), ReturnedActivity.BudgetID.ToString(),
                                                                                                                                ReturnedActivity.BudgetCategory, ReturnedActivity.BudgetSubCategory);
                List<CostPercentage> PercentageBasisCostList = WebAPI.Models.CostPercentage.getCostPercentage(ReturnedActivity.ActivityID.ToString());

                foreach (CostFTE ReturnedFTECost in FTECostList)
                {
                    Cost tempCost = new Cost();
                    tempCost.CostType = "F";
                    tempCost.ActivityID = ReturnedFTECost.ActivityID.ToString();
                    tempCost.CostID = ReturnedFTECost.FTECostID;
                    tempCost.TextBoxID = ReturnedFTECost.TextBoxID;
                    tempCost.StartDate = ReturnedFTECost.FTEStartDate;
                    tempCost.EndDate = ReturnedFTECost.FTEEndDate;
                    tempCost.Description = ReturnedFTECost.FTEPosition;
                    tempCost.TextBoxValue = ReturnedFTECost.FTEValue;
                    tempCost.Base = ReturnedFTECost.FTEHourlyRate;
                    tempCost.Scale = ReturnedFTECost.Granularity;

                    tempCost.FTECost = ReturnedFTECost.FTECost;
                    tempCost.FTEHours = ReturnedFTECost.FTEHours;
                    tempCost.FTEPositionID = ReturnedFTECost.FTEPositionID;
                    tempCost.DT_RowID = CostNumber;
                    CostNumber += 1;
                    matchedCostList.Add(tempCost);

                    if (MaxFCostID < int.Parse(tempCost.CostID))
                        MaxFCostID = int.Parse(tempCost.CostID);

                }

                foreach (CostLumpsum ReturnedLumpsumCost in LumpsumCostList)
                {
                    Cost tempCost = new Cost();
                    tempCost.CostType = "L";
                    tempCost.ActivityID = ReturnedLumpsumCost.ActivityID.ToString();
                    tempCost.CostID = ReturnedLumpsumCost.LumpsumCostID;
                    tempCost.TextBoxID = ReturnedLumpsumCost.TextBoxID;
                    tempCost.StartDate = ReturnedLumpsumCost.LumpsumCostStartDate;
                    tempCost.EndDate = ReturnedLumpsumCost.LumpsumCostEndDate;
                    tempCost.Description = ReturnedLumpsumCost.LumpsumDescription;
                    tempCost.TextBoxValue = ReturnedLumpsumCost.LumpsumCost;
                    tempCost.Scale = ReturnedLumpsumCost.Granularity;
                    tempCost.DT_RowID = CostNumber;
                    CostNumber += 1;
                    matchedCostList.Add(tempCost);

                    if (MaxLCostID < int.Parse(tempCost.CostID))
                        MaxLCostID = int.Parse(tempCost.CostID);
                }


                foreach (CostUnit ReturnedUnitCost in UnitCostList)
                {
                    Cost tempCost = new Cost();
                    tempCost.CostType = "U";
                    tempCost.ActivityID = ReturnedUnitCost.ActivityID.ToString();
                    tempCost.CostID = ReturnedUnitCost.UnitCostID;
                    tempCost.TextBoxID = ReturnedUnitCost.TextBoxID;
                    tempCost.StartDate = ReturnedUnitCost.UnitCostStartDate;
                    tempCost.EndDate = ReturnedUnitCost.UnitCostEndDate;
                    tempCost.Description = ReturnedUnitCost.UnitDescription;
                    tempCost.TextBoxValue = ReturnedUnitCost.UnitQuantity;
                    tempCost.Base = ReturnedUnitCost.UnitPrice;
                    tempCost.UnitType = ReturnedUnitCost.UnitType;
                    tempCost.Scale = ReturnedUnitCost.Granularity;
                    tempCost.DT_RowID = CostNumber;
                    CostNumber += 1;
                    matchedCostList.Add(tempCost);

                    if (MaxUCostID < int.Parse(tempCost.CostID))
                        MaxUCostID = int.Parse(tempCost.CostID);
                }

                foreach (CostPercentage ReturnedPercentageBasisCost in PercentageBasisCostList)
                {
                    Cost tempCost = new Cost();
                    tempCost.CostType = "P";
                    tempCost.ActivityID = ReturnedPercentageBasisCost.ActivityID.ToString();
                    tempCost.CostID = ReturnedPercentageBasisCost.PercentageBasisCostID;
                    tempCost.TextBoxID = ReturnedPercentageBasisCost.TextBoxID;
                    tempCost.StartDate = ReturnedPercentageBasisCost.PercentageBasisCostStartDate;
                    tempCost.EndDate = ReturnedPercentageBasisCost.PercentageBasisCostEndDate;
                    tempCost.Description = ReturnedPercentageBasisCost.PercentageBasisDescription;
                    tempCost.TextBoxValue = ReturnedPercentageBasisCost.PercentageBasisPercentageValue;
                    tempCost.Base = ReturnedPercentageBasisCost.PercentageBasisBaseAmount;
                    tempCost.Scale = ReturnedPercentageBasisCost.Granularity;
                    tempCost.DT_RowID = CostNumber;
                    CostNumber += 1;
                    matchedCostList.Add(tempCost);

                    if (MaxPCostID > int.Parse(tempCost.CostID))
                        MaxPCostID = int.Parse(tempCost.CostID);
                }
            }
            return matchedCostList;
        }
        //Check to see if 2 activities are the same
        public static bool isSameActivity(Activity baselineActivity, Activity trendActivity)
        {
            bool result = false;

            if ((baselineActivity.ProjectID == trendActivity.ProjectID) && (baselineActivity.PhaseCode == trendActivity.PhaseCode) &&
                       (baselineActivity.BudgetCategory == trendActivity.BudgetCategory) && (baselineActivity.BudgetSubCategory == trendActivity.BudgetSubCategory))
            {
                result = true;
            }

            return result;
        }
        public static Activity mergedActivity(Activity baselineActivity, Activity trendActivity)
        {
            Activity mergedActivity = new Activity();
            mergedActivity.ActivityID = baselineActivity.ActivityID;
            mergedActivity.ProjectID = baselineActivity.ProjectID;
            mergedActivity.TrendNumber = baselineActivity.TrendNumber;
            mergedActivity.PhaseCode = baselineActivity.PhaseCode;
            mergedActivity.BudgetCategory = baselineActivity.BudgetCategory;
            mergedActivity.BudgetSubCategory = baselineActivity.BudgetSubCategory;

            if (Convert.ToDateTime(baselineActivity.ActivityStartDate) >= Convert.ToDateTime(trendActivity.ActivityStartDate))
            {
                mergedActivity.ActivityStartDate = trendActivity.ActivityStartDate;
            }
            else
            {
                mergedActivity.ActivityStartDate = baselineActivity.ActivityStartDate;
            }

            mergedActivity.ActivityEndDate = (Convert.ToDateTime(baselineActivity.ActivityEndDate) >= Convert.ToDateTime(trendActivity.ActivityEndDate))
                                                    ? baselineActivity.ActivityEndDate : trendActivity.ActivityEndDate;
            var baselineFTE = (baselineActivity.FTECost != 0.0) ? Convert.ToDouble(baselineActivity.FTECost) : 0.0;
            var trendFTE = (trendActivity.FTECost != 0.0) ? Convert.ToDouble(trendActivity.FTECost) : 0.0;
            mergedActivity.FTECost = baselineFTE + trendFTE;

            var baselineLumpsum = (baselineActivity.LumpsumCost != 0.0) ? Convert.ToDouble(baselineActivity.LumpsumCost) : 0.0;
            var trendLumpsum = (trendActivity.LumpsumCost != 0.0) ? Convert.ToDouble(trendActivity.LumpsumCost) : 0.0;
            mergedActivity.LumpsumCost = baselineLumpsum + trendLumpsum;

            var baselineUnit = (baselineActivity.UnitCost != 0.0) ? Convert.ToDouble(baselineActivity.UnitCost) : 0.0;
            var trendUnit = (trendActivity.UnitCost != 0.0) ? Convert.ToDouble(trendActivity.UnitCost) : 0.0;
            mergedActivity.UnitCost = baselineUnit + trendUnit;
            mergedActivity.PercentageBasisCost = baselineActivity.PercentageBasisCost;


            return mergedActivity;
        }
        //Merge Activities together
        public static List<Activity> combineActivities(List<Activity> allApprovedActivityList, List<Activity> baselineActivityList)
        {
            List<Activity> mergedActivityList = new List<Activity>();
            List<Activity> tempActivityList = new List<Activity>();


            foreach (var act in allApprovedActivityList)
            {
                foreach (var baselineAct in baselineActivityList)
                {
                    if (isSameActivity(baselineAct, act))
                    {
                        Activity tempAct = mergedActivity(baselineAct, act);
                        tempActivityList.Add(tempAct);
                        mergedActivityList.Add(tempAct);
                    }

                }
            }



            foreach (var baseAct in baselineActivityList)
            {
                var check = false;
                foreach (var act in tempActivityList)
                {
                    if (isSameActivity(baseAct, act))
                    {
                        check = true;
                    }
                }
                if (check == false)
                {
                    mergedActivityList.Add(baseAct);
                }
                else
                {
                    check = false;
                }
            }



            foreach (var aprAct in allApprovedActivityList)
            {
                var check = false;
                foreach (var act in tempActivityList)
                {
                    if (isSameActivity(aprAct, act))
                    {
                        check = true;
                    }
                }
                if (check == false)
                {
                    mergedActivityList.Add(aprAct);
                }
                else
                {
                    check = true;
                }
            }


            return mergedActivityList;
        }
        //Get matched activities

        public static List<String> convertToCorrectDateTime(Cost cost, String Granularity)
        {
            List<String> result = new List<String>();
            String startDate = "";
            String endDate = "";
            String textBoxValue = "";

            String[] startDtArray = cost.StartDate.Split(',');
            String[] endDtArray = cost.EndDate.Split(',');
            String[] textArray = cost.TextBoxValue.Split(',');

            if (Granularity == "week")
            {

                for (int i = 0; i < startDtArray.Length - 1; i++)
                {
                    var start = Convert.ToDateTime(startDtArray[i]);
                    var end = Convert.ToDateTime(startDtArray[i + 1]);
                    var eDt = Convert.ToDateTime(endDtArray[i]);

                    var diff = end - start;
                    if (diff.Days > 7)
                    {
                        var days = diff.Days;
                        startDate += startDtArray[i] + " ,";
                        endDate += endDtArray[i] + " ,";
                        textBoxValue += textArray[i] + " ,";
                        while (days != 7)
                        {
                            startDate += start.AddDays(7).Date.ToString("yyyy-MM-dd") + " ,";
                            endDate += eDt.AddDays(7).Date.ToString("yyyy-MM-dd") + " ,";
                            textBoxValue += "0 ,";
                            days = days - 7;
                            start = start.AddDays(7);
                            eDt = eDt.AddDays(7);
                        }

                    }
                    else
                    {
                        startDate += startDtArray[i] + ",";
                        endDate += endDtArray[i] + ",";
                        textBoxValue += textArray[i] + ",";
                    }



                }
                startDate += startDtArray[startDtArray.Length - 1] + " ,";
                endDate += endDtArray[endDtArray.Length - 1] + " ,";
                textBoxValue += textArray[textArray.Length - 1] + " ,";
                startDate = startDate.Remove(startDate.Length - 1);
                endDate = endDate.Remove(endDate.Length - 1);
                textBoxValue = textBoxValue.Remove(textBoxValue.Length - 1);
            }
            else if (Granularity == "month")
            {
                for (int i = 0; i < startDtArray.Length - 1; i++)
                {
                    var start = Convert.ToDateTime(startDtArray[i]);
                    var end = Convert.ToDateTime(startDtArray[i + 1]);
                    var eDt = Convert.ToDateTime(endDtArray[i]);

                    var diff = end.Month - start.Month;
                    if (diff > 1)
                    {
                        var days = diff;
                        startDate += startDtArray[i] + " ,";
                        endDate += endDtArray[i] + " ,";
                        textBoxValue += textArray[i] + " ,";
                        while (days != 1)
                        {
                            var startEndOfMonth = new DateTime(start.Year, start.Month, DateTime.DaysInMonth(start.Year, start.Month));
                            startDate += startEndOfMonth.ToString("yyyy-MM-dd") + " ,";
                            var endEndOfMonth = new DateTime(eDt.Year, eDt.Month, DateTime.DaysInMonth(eDt.Year, eDt.Month));
                            //  endDate += eDt.AddDays(7).Date.ToString("yyyy-MM-dd") + " ,";
                            endDate += endEndOfMonth.ToString("yyyy-MM-dd");
                            textBoxValue += "0 ,";
                            days = days - 1;
                            start = start.AddDays(1);
                            eDt = eDt.AddDays(1);
                        }

                    }
                    else
                    {
                        startDate += startDtArray[i] + ",";
                        endDate += endDtArray[i] + ",";
                        textBoxValue += textArray[i] + ",";
                    }
                }
                startDate += startDtArray[startDtArray.Length - 1] + " ,";
                endDate += endDtArray[endDtArray.Length - 1] + " ,";
                textBoxValue += textArray[textArray.Length - 1] + " ,";
                startDate = startDate.Remove(startDate.Length - 1);
                endDate = endDate.Remove(endDate.Length - 1);
                textBoxValue = textBoxValue.Remove(textBoxValue.Length - 1);
            }
            else if (Granularity == "year")
            {
                for (int i = 0; i < startDtArray.Length - 1; i++)
                {
                    var start = Convert.ToDateTime(startDtArray[i]);
                    var end = Convert.ToDateTime(startDtArray[i + 1]);
                    var eDt = Convert.ToDateTime(endDtArray[i]);

                    var diff = end.Year - start.Year;
                    if (diff > 1)
                    {
                        var days = diff;
                        startDate += startDtArray[i] + " ,";
                        endDate += endDtArray[i] + " ,";
                        textBoxValue += textArray[i] + " ,";
                        while (days != 1)
                        {
                            var startEndOfYear = new DateTime(start.Year, 12, 31);
                            startDate += startEndOfYear.ToString("yyyy-MM-dd") + " ,";
                            var endEndOfYear = new DateTime(eDt.Year, 12, 31);
                            //  endDate += eDt.AddDays(7).Date.ToString("yyyy-MM-dd") + " ,";
                            endDate += endEndOfYear.ToString("yyyy-MM-dd");
                            textBoxValue += "0 ,";
                            days = days - 1;
                            start = start.AddDays(1);
                            eDt = eDt.AddDays(1);
                        }

                    }
                    else
                    {
                        startDate += startDtArray[i] + ",";
                        endDate += endDtArray[i] + ",";
                        textBoxValue += textArray[i] + ",";
                    }
                }
                startDate += startDtArray[startDtArray.Length - 1] + " ,";
                endDate += endDtArray[endDtArray.Length - 1] + " ,";
                textBoxValue += textArray[textArray.Length - 1] + " ,";
                startDate = startDate.Remove(startDate.Length - 1);
                endDate = endDate.Remove(endDate.Length - 1);
                textBoxValue = textBoxValue.Remove(textBoxValue.Length - 1);
            }
            result.Add(startDate);
            result.Add(endDate);
            result.Add(textBoxValue);
            return result;
        }
        public static List<List<Activity>> matchedActivities(List<Activity> allApprovedActivityList, List<Activity> baselineActivityList)
        {
            List<List<Activity>> returnedMatchedList = new List<List<Activity>>();
            List<Activity> matchedBaselineActivity = new List<Activity>();
            List<Activity> matchedTrendActivity = new List<Activity>();


            foreach (var act in allApprovedActivityList)
            {
                foreach (var baselineAct in baselineActivityList)
                {
                    if (isSameActivity(baselineAct, act))
                    {
                        matchedBaselineActivity.Add(baselineAct);
                        matchedTrendActivity.Add(act);
                    }

                }
            }

            returnedMatchedList.Add(matchedBaselineActivity);
            returnedMatchedList.Add(matchedTrendActivity);

            return returnedMatchedList;
        }
        //Get unmatched activities
        public static List<Activity> getUnmatchedActivity(List<Activity> allApprovedActivityList, List<Activity> baselineActivityList)
        {
            List<Activity> unMatchedActivityList = new List<Activity>();
            List<Activity> tempActivityList = new List<Activity>();
            List<Activity> matchedActivityList = new List<Activity>();

            foreach (var act in allApprovedActivityList)
            {
                bool isMatched = false;
                foreach (var baselineAct in baselineActivityList)
                {

                    if (isSameActivity(baselineAct, act))
                    {
                        isMatched = true;
                        matchedActivityList.Add(act);
                    }

                }
                if (isMatched == false)
                {
                    tempActivityList.Add(act);
                }
            }

            unMatchedActivityList = tempActivityList;
            foreach (var baselineAct in baselineActivityList)
            {
                bool isMatched = false;

                foreach (var act in matchedActivityList)
                {
                    if (isSameActivity(baselineAct, act))
                    {
                        isMatched = true;
                    }
                }
                if (isMatched == false)
                {
                    unMatchedActivityList.Add(baselineAct);
                }
            }

            return unMatchedActivityList;
        }
        public static Cost mergeLumpsum(Cost baseCost, Cost actCost, String Granularity)
        {
            List<String> baseline = convertToCorrectDateTime(baseCost, Granularity);
            String[] baseStartDate = baseline[0].Split(',');
            String[] baseEndDate = baseline[1].Split(',');
            String[] baseTextBoxVal = baseline[2].Split(',');


            List<String> act = convertToCorrectDateTime(actCost, Granularity);
            String[] actStartDate = act[0].Split(',');
            String[] actEndDate = act[1].Split(',');
            String[] actTextBoxVal = act[2].Split(',');

            List<String> newStartDate = new List<String>();
            List<String> newEndDate = new List<String>();
            List<String> newTextBoxVal = new List<String>();
            Cost lumpsumCost = new Cost();
            lumpsumCost = baseCost;
            for (int i = 0; i < baseEndDate.Length; i++)
            {
                for (int j = 0; j < actEndDate.Length; j++)
                {
                    if (baseEndDate[i].Trim() == actEndDate[j].Trim())
                    {
                        var sumTextBoxValue = Convert.ToInt16(baseTextBoxVal[i]) + Convert.ToInt16(actTextBoxVal[j]);
                        newStartDate.Add(baseStartDate[i]);
                        newEndDate.Add(baseEndDate[i]);

                        newTextBoxVal.Add(sumTextBoxValue.ToString());
                    }

                }
            }
            //If there is a gap between the baseline end date and the start date of the approved trends
            if (newStartDate.Count == 0)
            {
                if (Convert.ToDateTime(baseEndDate[baseEndDate.Length - 1]) < Convert.ToDateTime(actStartDate[0]))
                {
                    if (Granularity == "week" || Granularity == "month")
                    {


                        if (Granularity == "week")
                        {
                            for (int i = 0; i < baseEndDate.Length; i++)
                            {
                                //1. get date for base line
                                if (baseEndDate.Length - 1 == i)
                                {
                                    var start = Convert.ToDateTime(baseEndDate[baseEndDate.Length - 1]);
                                    if (start.DayOfWeek != DayOfWeek.Sunday)
                                    {
                                        var weekStart = start.AddDays(-(int)start.DayOfWeek);
                                        var end = weekStart.AddDays(7).AddSeconds(-1);
                                        newStartDate.Add(baseStartDate[i]);
                                        newEndDate.Add(end.ToString("yyyy-MM-dd"));
                                        newTextBoxVal.Add(baseTextBoxVal[i]);
                                    }
                                    else
                                    {
                                        newStartDate.Add(baseStartDate[i]);
                                        newEndDate.Add(baseEndDate[i]);
                                        newTextBoxVal.Add(baseTextBoxVal[i]);
                                    }
                                }
                                else
                                {
                                    newStartDate.Add(baseStartDate[i]);
                                    newEndDate.Add(baseEndDate[i]);
                                    newTextBoxVal.Add(baseTextBoxVal[i]);
                                }

                            }
                            //2.find gap between trends and add missing dates
                            var baseEndWeek = Convert.ToDateTime(newEndDate[newEndDate.Count - 1]);
                            var baseWeek = baseEndWeek.Day;
                            var baseStartWeek = Convert.ToDateTime(actStartDate[0]);
                            var actWeek = baseStartWeek.Day;
                            var diffWeek = baseStartWeek - baseEndWeek;
                            var diff = diffWeek.Days;
                            while (diff > 7)
                            {
                                var baseNextStartWeek = baseEndWeek.AddDays(1);
                                var baseNextEndOfWeek = baseNextStartWeek.AddDays(7).AddSeconds(-1);

                                newStartDate.Add(baseNextStartWeek.ToString("yyyy-MM-dd"));
                                newEndDate.Add(baseNextEndOfWeek.ToString("yyyy-MM-dd"));
                                newTextBoxVal.Add("0");
                                baseNextStartWeek = baseNextEndOfWeek;
                                diff = diff - 7;
                            }

                            //3. add date for approved trends
                            for (int i = 0; i < actEndDate.Length; i++)
                            {
                                newStartDate.Add(actStartDate[i]);
                                newEndDate.Add(actEndDate[i]);
                                newTextBoxVal.Add(actTextBoxVal[i]);
                            }
                        }
                        else if (Granularity == "month")
                        {
                            //1. get date for base line 
                            for (int i = 0; i < baseEndDate.Length; i++)
                            {
                                if (baseEndDate.Length - 1 == i)
                                {
                                    var start = Convert.ToDateTime(baseEndDate[baseEndDate.Length - 1]);
                                    if (isEndOfMonth(start) == false)
                                    {
                                        var end = new DateTime(start.Year, start.Month,
                                                    DateTime.DaysInMonth(start.Year, start.Month));
                                        newStartDate.Add(baseStartDate[i]);
                                        newEndDate.Add(end.ToString("yyyy-MM-dd"));
                                        newTextBoxVal.Add(baseTextBoxVal[i]);
                                    }
                                }
                                else
                                {
                                    newStartDate.Add(baseStartDate[i]);
                                    newEndDate.Add(baseEndDate[i]);
                                    newTextBoxVal.Add(baseTextBoxVal[i]);
                                }
                            }


                            var baseEndMonth = Convert.ToDateTime(newEndDate[newEndDate.Count - 1]);
                            var baseMonth = baseEndMonth.Month;
                            var baseStartMonth = Convert.ToDateTime(actStartDate[0]);
                            var actMonth = baseStartMonth.Month;
                            var diff = actMonth - baseMonth - 1;
                            //2.find gap between trends and add missing dates
                            while (diff > 0)
                            {
                                var baseNextStartMonth = baseEndMonth.AddDays(1);
                                var baseNextEndOfMonth = new DateTime(baseNextStartMonth.Year, baseNextStartMonth.Month,
                                                    DateTime.DaysInMonth(baseNextStartMonth.Year, baseNextStartMonth.Month));

                                newStartDate.Add(baseNextStartMonth.ToString("yyyy-MM-dd"));
                                newEndDate.Add(baseNextEndOfMonth.ToString("yyyy-MM-dd"));
                                newTextBoxVal.Add("0");
                                baseNextStartMonth = baseNextEndOfMonth;
                                diff--;
                            }
                            //3. add date for approved trends
                            for (int i = 0; i < actEndDate.Length; i++)
                            {
                                if (i == 0)
                                {
                                    var start = Convert.ToDateTime(actEndDate[0]);
                                    if (isStartOfMonth(start) == false)
                                    {
                                        var newStart = new DateTime(start.Year, start.Month, 1);
                                        newStartDate.Add(newStart.ToString("yyyy-MM-dd"));
                                        newEndDate.Add(actEndDate[i]);
                                        newTextBoxVal.Add(actTextBoxVal[i]);
                                    }
                                }
                                else
                                {
                                    newStartDate.Add(actStartDate[i]);
                                    newEndDate.Add(actEndDate[i]);
                                    newTextBoxVal.Add(actTextBoxVal[i]);
                                }
                            }
                        }


                    }

                    else if (Granularity == "year")
                    {
                        var baseEndYear = Convert.ToDateTime(baseEndDate[baseEndDate.Length - 1]);
                        var baseYear = baseEndYear.Year;
                        var actStartYear = Convert.ToDateTime(actStartDate[0]);
                        var actYear = actStartYear.Year;
                        var diff = actYear - baseYear;
                        if (diff > 0)
                        {

                        }
                        else
                        {
                            newStartDate.Add(baseStartDate[0]);
                            newEndDate.Add(actEndDate[0]);
                            var total = 0.0;
                            foreach (var baseItem in baseTextBoxVal)
                            {
                                total += Convert.ToDouble(baseItem);

                            }
                            foreach (var actItem in actTextBoxVal)
                            {
                                total += Convert.ToDouble(actItem);
                            }
                            newTextBoxVal.Add(total.ToString());
                        }
                    }
                }
                else if (Convert.ToDateTime(actEndDate[baseEndDate.Length - 1]) < Convert.ToDateTime(baseStartDate[0]))
                {
                    for (int i = 0; i < actEndDate.Length; i++)
                    {
                        newStartDate.Add(actStartDate[i]);
                        newEndDate.Add(actEndDate[i]);
                        newTextBoxVal.Add(actTextBoxVal[i]);
                    }
                    for (int i = 0; i < baseEndDate.Length; i++)
                    {
                        newStartDate.Add(baseStartDate[i]);
                        newEndDate.Add(baseEndDate[i]);
                        newTextBoxVal.Add(baseTextBoxVal[i]);
                    }
                }
            }
            List<String> preStartDate = new List<String>();
            List<String> preEndDate = new List<String>();
            List<String> preTextBoxValue = new List<String>();

            List<String> postStartDate = new List<String>();
            List<String> postEndDate = new List<String>();
            List<String> postTextBoxValue = new List<String>();
            //Baseline Pre start date
            if (newStartDate.Count != 0)
            {
                if (Convert.ToDateTime(baseStartDate[0]) < Convert.ToDateTime(newStartDate.ElementAt(0).ToString()))
                {
                    for (int i = 0; i < baseStartDate.Length; i++)
                    {

                        if (Convert.ToDateTime(baseStartDate[i]) < Convert.ToDateTime(newStartDate.ElementAt(0).ToString()))
                        {
                            preStartDate.Add(baseStartDate[i]);
                            preEndDate.Add(baseEndDate[i]);
                            preTextBoxValue.Add(baseTextBoxVal[i]);
                        }


                    }
                }
                //Trend Pre Start Date
                else if (Convert.ToDateTime(actStartDate[0]) < Convert.ToDateTime(newStartDate.ElementAt(0).ToString()))
                {
                    for (int i = 0; i < actStartDate.Length; i++)
                    {

                        if (Convert.ToDateTime(actStartDate[i]) < Convert.ToDateTime(newStartDate.ElementAt(0).ToString()))
                        {
                            preStartDate.Add(actStartDate[i]);
                            preEndDate.Add(actEndDate[i]);
                            preTextBoxValue.Add(actTextBoxVal[i]);
                        }
                        if (Convert.ToDateTime(actEndDate[i]) > Convert.ToDateTime(newEndDate.ElementAt(newEndDate.Count)))
                        {
                            postStartDate.Add(baseStartDate[i]);
                            postEndDate.Add(baseEndDate[i]);
                            postTextBoxValue.Add(baseTextBoxVal[i]);

                        }

                    }
                }

                if (Convert.ToDateTime(baseEndDate[baseEndDate.Length - 1]) > Convert.ToDateTime(newEndDate.LastOrDefault().ToString()))
                {
                    for (int i = 0; i < baseStartDate.Length; i++)
                    {
                        if (Convert.ToDateTime(baseEndDate[i]) > Convert.ToDateTime(newEndDate.LastOrDefault().ToString()))
                        {
                            postStartDate.Add(baseStartDate[i]);
                            postEndDate.Add(baseEndDate[i]);
                            postTextBoxValue.Add(baseTextBoxVal[i]);

                        }
                    }
                }
                else if (Convert.ToDateTime(actEndDate[actEndDate.Length - 1]) > Convert.ToDateTime(newEndDate.LastOrDefault().ToString()))
                {
                    for (int i = 0; i < actStartDate.Length; i++)
                    {
                        if (Convert.ToDateTime(actEndDate[i]) > Convert.ToDateTime(newEndDate.LastOrDefault().ToString()))
                        {
                            postStartDate.Add(actStartDate[i]);
                            postEndDate.Add(actEndDate[i]);
                            postTextBoxValue.Add(actTextBoxVal[i]);

                        }

                    }
                }

                if (preStartDate.Count > 0)
                {
                    for (int i = preStartDate.Count - 1; i != -1; i--)
                    {
                        newStartDate.Insert(0, preStartDate[i]);
                        newEndDate.Insert(0, preEndDate[i]);
                        newTextBoxVal.Insert(0, preTextBoxValue[i]);
                    }
                }
                if (postStartDate.Count > 0)
                {
                    for (int i = 0; i < postStartDate.Count; i++)
                    {
                        newStartDate.Insert(newStartDate.Count, postStartDate[i]);
                        newEndDate.Insert(newEndDate.Count, postEndDate[i]);
                        newTextBoxVal.Insert(newTextBoxVal.Count, postTextBoxValue[i]);
                    }
                }
            }
            List<string> textBoxIdList = new List<String>();
            //baselineActivityList = getActivity(conn, reader, ProjectID, baselineTrend.TrendNumber);  //Get Baseline Activity
            // Activity activity = getActivityByID()
            for (int i = 0; i < newTextBoxVal.Count; i++)
            {
                textBoxIdList.Add(i.ToString());
            }

            String textBoxId = String.Join(",", textBoxIdList);
            String startDate = String.Join(",", newStartDate);
            String endDate = String.Join(",", newEndDate);
            String textBoxValue = String.Join(",", newTextBoxVal);

            lumpsumCost.StartDate = startDate;
            lumpsumCost.EndDate = endDate;
            lumpsumCost.TextBoxValue = textBoxValue;
            lumpsumCost.TextBoxID = textBoxId;

            return lumpsumCost;

        }

        public static Cost mergeFte(Cost baseCost, Cost actCost, String Granularity)
        {
            String[] baseStartDate = baseCost.StartDate.Split(',');
            String[] baseEndDate = baseCost.EndDate.Split(',');
            String[] baseTextBoxVal = baseCost.TextBoxValue.Split(',');
            String[] baseFteCost = baseCost.FTECost.Split(',');
            String[] baseFteHours = baseCost.FTEHours.Split(',');

            String[] actStartDate = actCost.StartDate.Split(',');
            String[] actEndDate = actCost.EndDate.Split(',');
            String[] actTextBoxVal = actCost.TextBoxValue.Split(',');
            String[] actFteCost = actCost.FTECost.Split(',');
            String[] actFteHours = actCost.FTEHours.Split(',');

            List<String> newStartDate = new List<String>();
            List<String> newEndDate = new List<String>();
            List<String> newTextBoxVal = new List<String>();
            List<String> newFteCost = new List<String>();
            List<String> newFteHours = new List<String>();
            Cost cost = new Cost();
            cost = baseCost;
            for (int i = 0; i < baseEndDate.Length; i++)
            {
                for (int j = 0; j < actEndDate.Length; j++)
                {
                    if (baseEndDate[i].Trim() == actEndDate[j].Trim())
                    {
                        var sumTextBoxValue = Convert.ToInt16(baseTextBoxVal[i]) + Convert.ToInt16(actTextBoxVal[j]);
                        var sumFteHours = Convert.ToInt16(baseFteHours[i]) + Convert.ToInt16(actFteHours[j]);
                        var sumFteCost = Convert.ToInt16(baseFteCost[i]) + Convert.ToInt16(actFteCost[j]);
                        newStartDate.Add(baseStartDate[j]);
                        newEndDate.Add(baseEndDate[j]);

                        newTextBoxVal.Add(sumTextBoxValue.ToString());
                        newFteCost.Add(sumFteCost.ToString());
                        newFteHours.Add(sumFteHours.ToString());
                    }

                }
            }
            List<String> preStartDate = new List<String>();
            List<String> preEndDate = new List<String>();
            List<String> preTextBoxValue = new List<String>();
            List<String> preFteHour = new List<String>();
            List<String> preFteCost = new List<String>();

            List<String> postStartDate = new List<String>();
            List<String> postEndDate = new List<String>();
            List<String> postTextBoxValue = new List<String>();
            List<String> postFteHour = new List<String>();
            List<String> postFteCost = new List<String>();

            //Baseline Pre start date
            if (Convert.ToDateTime(baseStartDate[0]) < Convert.ToDateTime(newStartDate.ElementAt(0).ToString()))
            {
                for (int i = 0; i < baseStartDate.Length; i++)
                {

                    if (Convert.ToDateTime(baseStartDate[i]) < Convert.ToDateTime(newStartDate.ElementAt(0).ToString()))
                    {
                        preStartDate.Add(baseStartDate[i]);
                        preEndDate.Add(baseEndDate[i]);
                        preTextBoxValue.Add(baseTextBoxVal[i]);
                        preFteHour.Add(baseFteHours[i]);
                        preFteCost.Add(baseFteCost[i]);
                    }


                }
            }
            //Trend Pre Start Date
            else if (Convert.ToDateTime(actStartDate[0]) < Convert.ToDateTime(newStartDate.ElementAt(0).ToString()))
            {
                for (int i = 0; i < actStartDate.Length; i++)
                {

                    if (Convert.ToDateTime(actStartDate[i]) < Convert.ToDateTime(newStartDate.ElementAt(0).ToString()))
                    {
                        preStartDate.Add(actStartDate[i]);
                        preEndDate.Add(actEndDate[i]);
                        preTextBoxValue.Add(actTextBoxVal[i]);
                        preFteHour.Add(actFteHours[i]);
                        preFteCost.Add(actFteCost[i]);
                    }


                }
            }

            if (Convert.ToDateTime(baseEndDate[baseEndDate.Length - 1]) > Convert.ToDateTime(newEndDate.LastOrDefault().ToString()))
            {
                for (int i = 0; i < baseStartDate.Length; i++)
                {
                    if (Convert.ToDateTime(baseEndDate[i]) > Convert.ToDateTime(newEndDate.LastOrDefault().ToString()))
                    {
                        postStartDate.Add(baseStartDate[i]);
                        postEndDate.Add(baseEndDate[i]);
                        postTextBoxValue.Add(baseTextBoxVal[i]);
                        postFteCost.Add(baseFteCost[i]);
                        postFteHour.Add(baseFteHours[i]);

                    }
                }
            }
            else if (Convert.ToDateTime(actEndDate[actEndDate.Length - 1]) > Convert.ToDateTime(newEndDate.LastOrDefault().ToString()))
            {
                for (int i = 0; i < actStartDate.Length; i++)
                {
                    if (Convert.ToDateTime(actEndDate[i]) > Convert.ToDateTime(newEndDate.LastOrDefault().ToString()))
                    {
                        postStartDate.Add(actStartDate[i]);
                        postEndDate.Add(actEndDate[i]);
                        postTextBoxValue.Add(actTextBoxVal[i]);
                        postFteCost.Add(actFteCost[i]);
                        postFteHour.Add(actFteHours[i]);

                    }

                }
            }

            if (preStartDate.Count > 0)
            {
                for (int i = preStartDate.Count - 1; i != -1; i--)
                {
                    newStartDate.Insert(0, preStartDate[i]);
                    newEndDate.Insert(0, preEndDate[i]);
                    newTextBoxVal.Insert(0, preTextBoxValue[i]);
                    newFteCost.Insert(0, preFteCost[i]);
                    newFteHours.Insert(0, preFteHour[i]);
                }
            }
            if (postStartDate.Count > 0)
            {
                for (int i = 0; i < postStartDate.Count; i++)
                {
                    newStartDate.Insert(newStartDate.Count, postStartDate[i]);
                    newEndDate.Insert(newEndDate.Count, postEndDate[i]);
                    newTextBoxVal.Insert(newTextBoxVal.Count, postTextBoxValue[i]);
                    newFteCost.Insert(newFteCost.Count, postFteCost[i]);
                    newFteHours.Insert(newFteHours.Count, postFteHour[i]);
                }
            }

            List<string> textBoxIdList = new List<String>();
            for (int i = 0; i < newTextBoxVal.Count; i++)
            {
                textBoxIdList.Add(i.ToString());
            }

            String textBoxId = String.Join(",", textBoxIdList);
            String startDate = String.Join(",", newStartDate);
            String endDate = String.Join(",", newEndDate);
            String textBoxValue = String.Join(",", newTextBoxVal);
            String fteHour = String.Join(",", newFteHours);
            String fteCost = String.Join(",", newFteCost);

            cost.StartDate = startDate;
            cost.EndDate = endDate;
            cost.TextBoxValue = textBoxValue;
            cost.TextBoxID = textBoxId;
            cost.FTECost = fteCost;
            cost.FTEHours = fteHour;

            return cost;
        }

        public static Cost mergeUnitCost(Cost baseCost, Cost actCost, String Granularity)
        {
            String[] baseStartDate = baseCost.StartDate.Split(',');
            String[] baseEndDate = baseCost.EndDate.Split(',');
            String[] baseTextBoxVal = baseCost.TextBoxValue.Split(',');

            String[] actStartDate = actCost.StartDate.Split(',');
            String[] actEndDate = actCost.EndDate.Split(',');
            String[] actTextBoxVal = actCost.TextBoxValue.Split(',');

            List<String> newStartDate = new List<String>();
            List<String> newEndDate = new List<String>();
            List<String> newTextBoxVal = new List<String>();
            Cost cost = new Cost();
            cost = baseCost;
            for (int i = 0; i < baseEndDate.Length; i++)
            {
                for (int j = 0; j < actEndDate.Length; j++)
                {
                    if (baseEndDate[i] == actEndDate[j])
                    {
                        var sumTextBoxValue = Convert.ToInt16(baseTextBoxVal[i]) + Convert.ToInt16(actTextBoxVal[j]);
                        newStartDate.Add(baseStartDate[j]);
                        newEndDate.Add(baseEndDate[j]);

                        newTextBoxVal.Add(sumTextBoxValue.ToString());
                    }

                }
            }
            List<String> preStartDate = new List<String>();
            List<String> preEndDate = new List<String>();
            List<String> preTextBoxValue = new List<String>();

            List<String> postStartDate = new List<String>();
            List<String> postEndDate = new List<String>();
            List<String> postTextBoxValue = new List<String>();
            //Baseline Pre start date
            if (Convert.ToDateTime(baseStartDate[0]) < Convert.ToDateTime(newStartDate.ElementAt(0).ToString()))
            {
                for (int i = 0; i < baseStartDate.Length; i++)
                {

                    if (Convert.ToDateTime(baseStartDate[i]) < Convert.ToDateTime(newStartDate.ElementAt(0).ToString()))
                    {
                        preStartDate.Add(baseStartDate[i]);
                        preEndDate.Add(baseEndDate[i]);
                        preTextBoxValue.Add(baseTextBoxVal[i]);
                    }


                }
            }
            //Trend Pre Start Date
            else if (Convert.ToDateTime(actStartDate[0]) < Convert.ToDateTime(newStartDate.ElementAt(0).ToString()))
            {
                for (int i = 0; i < actStartDate.Length; i++)
                {

                    if (Convert.ToDateTime(actStartDate[i]) < Convert.ToDateTime(newStartDate.ElementAt(0).ToString()))
                    {
                        preStartDate.Add(actStartDate[i]);
                        preEndDate.Add(actEndDate[i]);
                        preTextBoxValue.Add(actTextBoxVal[i]);
                    }
                    if (Convert.ToDateTime(actEndDate[i]) > Convert.ToDateTime(newEndDate.ElementAt(newEndDate.Count)))
                    {
                        postStartDate.Add(baseStartDate[i]);
                        postEndDate.Add(baseEndDate[i]);
                        postTextBoxValue.Add(baseTextBoxVal[i]);

                    }

                }
            }

            if (Convert.ToDateTime(baseEndDate[baseEndDate.Length - 1]) > Convert.ToDateTime(newEndDate.LastOrDefault().ToString()))
            {
                for (int i = 0; i < baseStartDate.Length; i++)
                {
                    if (Convert.ToDateTime(baseEndDate[i]) > Convert.ToDateTime(newEndDate.LastOrDefault().ToString()))
                    {
                        postStartDate.Add(baseStartDate[i]);
                        postEndDate.Add(baseEndDate[i]);
                        postTextBoxValue.Add(baseTextBoxVal[i]);

                    }
                }
            }
            else if (Convert.ToDateTime(actEndDate[actEndDate.Length - 1]) > Convert.ToDateTime(newEndDate.LastOrDefault().ToString()))
            {
                for (int i = 0; i < actStartDate.Length; i++)
                {
                    if (Convert.ToDateTime(actEndDate[i]) > Convert.ToDateTime(newEndDate.LastOrDefault().ToString()))
                    {
                        postStartDate.Add(actStartDate[i]);
                        postEndDate.Add(actEndDate[i]);
                        postTextBoxValue.Add(actTextBoxVal[i]);

                    }

                }
            }

            if (preStartDate.Count > 0)
            {
                for (int i = preStartDate.Count; i != 0; i--)
                {
                    newStartDate.Insert(0, preStartDate[i]);
                    newEndDate.Insert(0, preEndDate[i]);
                    newTextBoxVal.Insert(0, preTextBoxValue[i]);
                }
            }
            if (postStartDate.Count > 0)
            {
                for (int i = 0; i < postStartDate.Count; i++)
                {
                    newStartDate.Insert(newStartDate.Count, postStartDate[i]);
                    newEndDate.Insert(newEndDate.Count, postEndDate[i]);
                    newTextBoxVal.Insert(newTextBoxVal.Count, postTextBoxValue[i]);
                }
            }

            List<string> textBoxIdList = new List<String>();
            for (int i = 0; i < newTextBoxVal.Count; i++)
            {
                textBoxIdList.Add(i.ToString());
            }

            String textBoxId = String.Join(",", textBoxIdList);
            String startDate = String.Join(",", newStartDate);
            String endDate = String.Join(",", newEndDate);
            String textBoxValue = String.Join(",", newTextBoxVal);

            cost.StartDate = startDate;
            cost.EndDate = endDate;
            cost.TextBoxValue = textBoxValue;
            cost.TextBoxID = textBoxId;

            return cost;
        }

        public static List<object> mergeApprovedTrends(List<Trend> ApprovedTrends, int ProjectID, String Granularity)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);


            List<object> obj = new List<object>();
            List<Trend> approvedTrendList = new List<Trend>();
            List<Activity> trend0ActivityList = new List<Activity>();
            List<Activity> approvedActivityList = new List<Activity>();
            List<Activity> allApprovedActivityList = new List<Activity>();
            List<Project> project = new List<Project>();
            List<Cost> costList = new List<Cost>();
            List<Cost> baselineCostList = new List<Cost>();
            List<List<Cost>> allCostList = new List<List<Cost>>();
            List<Activity> mergedActivityList = new List<Activity>();
            Trend trend0 = new Trend();
            MySqlConnection conn = null;
            MySqlDataReader reader = null;
            trend0 = ApprovedTrends[0];
            bool isFirst = false;
            trend0ActivityList = getActivity(conn, reader, ProjectID, trend0.TrendNumber);  //Get Baseline Activity
            baselineCostList = getCosts(trend0ActivityList, Granularity);  //
            if (ApprovedTrends.Count == 1)
            {
                mergedActivityList = trend0ActivityList;

            }
            for (int k = 1; k < ApprovedTrends.Count; k++)
            {
                //Get activity
                approvedActivityList = getActivity(conn, reader, ProjectID, ApprovedTrends[k].TrendNumber);

                //allApprovedActivityList.Add(approvedActivityList);

                //merge activities
                mergedActivityList = combineActivities(approvedActivityList, trend0ActivityList);

                //Costs
                //get activitiesthat are the same
                List<Activity> matchedBaselineActivityList = new List<Activity>();
                List<Activity> matchedApprovedTrendActivityList = new List<Activity>();
                List<Activity> notMatchedActivityList = new List<Activity>();
                List<Cost> baselineCost = new List<Cost>();
                List<Cost> trendActivityCost = new List<Cost>();
                List<Cost> unMatchedActivityCost = new List<Cost>();
                List<Cost> mergedCostList = new List<Cost>();
                List<Cost> matchedBaseCost = new List<Cost>();
                List<Cost> matchedActCost = new List<Cost>();
                List<Cost> tempCost = new List<Cost>();
                List<List<Activity>> returnedMatchedActivity = matchedActivities(approvedActivityList, trend0ActivityList);
                matchedBaselineActivityList = returnedMatchedActivity[0];
                matchedApprovedTrendActivityList = returnedMatchedActivity[1];

                notMatchedActivityList = getUnmatchedActivity(approvedActivityList, trend0ActivityList);
                if (isFirst == false)
                {
                    baselineCost = getCosts(matchedBaselineActivityList, Granularity);
                }
                else
                {
                    foreach (var cost in baselineCostList)
                    {
                        bool isMatch = false;
                        foreach (var activity in matchedBaselineActivityList)
                        {
                            if (cost.ActivityID == activity.ActivityID.ToString())
                            {
                                baselineCost.Add(cost);
                                isMatch = true;
                            }
                        }
                        if (isMatch == false)
                        {
                            tempCost.Add(cost);
                        }
                    }
                }
                trendActivityCost = getCosts(matchedApprovedTrendActivityList, Granularity);


                foreach (var actCost in trendActivityCost)
                {
                    foreach (var baseCost in baselineCost)
                    {
                        if (actCost.CostType == baseCost.CostType && actCost.Description == baseCost.Description)
                        {
                            if (actCost.CostType == "F")
                            {
                                Cost fteCost = new Cost();
                                fteCost = mergeFte(baseCost, actCost, Granularity);
                                mergedCostList.Add(fteCost);


                            }
                            else if (actCost.CostType == "L")
                            {
                                Cost LumpsumCost = new Cost();
                                LumpsumCost = mergeLumpsum(baseCost, actCost, Granularity);
                                mergedCostList.Add(LumpsumCost);
                            }
                            else if (actCost.CostType == "U")
                            {
                                if (actCost.Base == baseCost.Base && actCost.UnitType == baseCost.UnitType)
                                {
                                    Cost unitCost = new Cost();
                                    unitCost = mergeUnitCost(baseCost, actCost, Granularity);
                                    mergedCostList.Add(unitCost);
                                }
                            }
                            matchedBaseCost.Add(baseCost);
                            matchedActCost.Add(actCost);

                        }

                    }
                }
                for (int i = 0; i < matchedActCost.Count; i++)
                {
                    foreach (var actCost in trendActivityCost)
                    {
                        if (actCost.ActivityID == matchedActCost[i].ActivityID)
                        {
                            if (actCost.CostType == matchedActCost[i].CostType && actCost.Description == matchedActCost[i].Description)
                            {

                            }
                            else
                            {
                                var isMatch = isCostExisted(actCost, mergedCostList);
                                if (isMatch == false)
                                {
                                    Cost cost = new Cost();
                                    cost = actCost;
                                    cost.ActivityID = matchedBaseCost[i].ActivityID;
                                    mergedCostList.Add(cost);
                                }
                            }

                        }
                    }
                }
                //Add non-match approved trend Cost
                for (int i = 0; i < trendActivityCost.Count; i++)
                {
                    bool isMatched = false;
                    foreach (var cost in mergedCostList)
                    {
                        if (trendActivityCost[i].CostType == cost.CostType && trendActivityCost[i].Description == cost.Description)
                        {
                            isMatched = true;
                        }
                    }
                    if (isMatched == false)
                    {
                        mergedCostList.Add(trendActivityCost[i]);
                    }
                }
                //Add non-matched Base Cost
                for (int i = 0; i < baselineCost.Count; i++)
                {
                    bool isMatch = false;
                    foreach (var cost in mergedCostList)
                    {
                        //if(baseCost.ActivityID != )
                        if (cost.CostType == baselineCost[i].CostType && cost.Description == baselineCost[i].Description)
                        {
                            isMatch = true;
                        }
                    }
                    if (isMatch == false)
                    {
                        mergedCostList.Add(baselineCost[i]);
                    }
                }
                //Find cost that don't have same activities id
                List<Cost> costWithoutActivitiyID = new List<Cost>();
                foreach (var cost in mergedCostList)
                {
                    bool isMatch = false;
                    foreach (var act in mergedActivityList)
                    {
                        if (cost.ActivityID == act.ActivityID.ToString())
                        {
                            isMatch = true;
                        }
                    }
                    if (isMatch == false)
                    {
                        costWithoutActivitiyID.Add(cost);
                    }
                }
                //now find the cost's activities
                foreach (var cost in costWithoutActivitiyID)
                {
                    Activity activity = getActivityByCost(cost, approvedActivityList);
                    if (activity.BudgetCategory != null)
                    {
                        foreach (var act in mergedActivityList)
                        {
                            if (act.BudgetCategory == activity.BudgetCategory && act.BudgetSubCategory == activity.BudgetSubCategory)
                            {
                                cost.ActivityID = act.ActivityID.ToString();
                            }
                        }

                    }

                }
                //now update the merged cost list
                foreach (var cost in costWithoutActivitiyID)
                {
                    foreach (var orgCost in mergedCostList)
                    {
                        if (cost.CostType == orgCost.CostType && cost.Description == orgCost.Description)
                        {
                            orgCost.ActivityID = cost.ActivityID;
                        }
                    }
                }
                //Get unmatched Activity's Cost
                if (isFirst == false)
                {
                    unMatchedActivityCost = getCosts(notMatchedActivityList, Granularity);
                }
                else
                {
                    foreach (var cost in baselineCostList)
                    {
                        foreach (var activity in notMatchedActivityList)
                        {
                            if (cost.ActivityID == activity.ActivityID.ToString())
                            {
                                unMatchedActivityCost.Add(cost);
                            }
                            else
                            {
                                //Cost cost = getCosts(activity)
                            }
                        }
                    }

                    foreach (var activity in notMatchedActivityList)
                    {
                        bool isMatch = false;
                        foreach (var cost in baselineCostList)
                        {
                            if (cost.ActivityID == activity.ActivityID.ToString())
                            {
                                isMatch = true;
                            }
                        }
                        if (isMatch == false)
                        {
                            List<Activity> act = new List<Activity>();
                            act.Add(activity);
                            List<Cost> costL = getCosts(act, Granularity);
                            foreach (var cost in costL)
                            {
                                mergedCostList.Add(cost);
                            }
                        }
                    }
                }

                foreach (var cost in unMatchedActivityCost)
                {
                    mergedCostList.Add(cost);
                }
                //mergedActivityList

                //
                List<Cost> testCost = new List<Cost>();
                if (isFirst == true)
                {
                    foreach (var cost in tempCost)
                    {
                        bool isMatch = false;
                        foreach (var c in mergedCostList)
                        {
                            if (cost.CostType == c.CostType && cost.Description == c.Description && cost.ActivityID == c.ActivityID)
                            {
                                isMatch = true;
                            }
                        }
                        if (isMatch == false)
                        {
                            testCost.Add(cost);
                        }
                    }
                    //n
                    foreach (var cost in testCost)
                    {
                        mergedCostList.Add(cost);
                    }
                }
                trend0ActivityList = mergedActivityList;
                baselineCostList = mergedCostList;
                isFirst = true;
            }
            obj.Add(mergedActivityList);
            obj.Add(baselineCostList);



            return obj;
        }
        //MAIN FUNCTION IMPLEMENTATION
        public static object GetCurrentProject(int ProjectID, String Granularity)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            List<object> obj = new List<object>();
            List<object> apprTrends = new List<object>();

            List<Trend> approvedTrendList = new List<Trend>();
            List<Activity> baselineActivityList = new List<Activity>();
            List<Activity> approvedActivityList = new List<Activity>();
            List<List<Activity>> allApprovedActivityList = new List<List<Activity>>();
            List<Project> project = new List<Project>();
            List<Cost> costList = new List<Cost>();
            List<Cost> baselineCostList = new List<Cost>();
            List<List<Cost>> allCostList = new List<List<Cost>>();
            List<Activity> mergedActivityList = new List<Activity>();
            Trend baselineTrend = new Trend();
            MySqlConnection conn = null;
            MySqlDataReader reader = null;
            Trend lastTrend = new Trend();
            project = WebAPI.Models.Project.getProject("null", "null", Convert.ToString(ProjectID), "null");  //Get the Project Information
            List<PhaseCode> PhaseCodeList = WebAPI.Models.PhaseCode.getPhaseCode("null", "null", "null");  //Get list of phaseCode
            baselineTrend = getBaselineTrend(conn, reader, ProjectID);  //Get baseline trend
            approvedTrendList = getApprovalTrends(conn, reader, ProjectID); //get Approval trends as a list
            List<Activity> mergedApprovedActivity = new List<Activity>();
            List<Cost> mergedApprovedCost = new List<Cost>();
            List<Cost> mergedCostList = new List<Cost>();

            baselineActivityList = getActivity(conn, reader, ProjectID, baselineTrend.TrendNumber);  //Get Baseline Activity
            baselineCostList = getCosts(baselineActivityList, Granularity);  //Get baseline Costs
            //1. We need to merged all approved trends together
            //return : list of merged activites and a list of merged costs
            if (approvedTrendList.Count == 0)
            {
                mergedCostList = baselineCostList;
                mergedActivityList = baselineActivityList;
            }
            else
            {
                apprTrends = mergeApprovedTrends(approvedTrendList, ProjectID, Granularity);
                mergedApprovedActivity = (List<Activity>)apprTrends[0]; //list of merged activities from approved trends
                mergedApprovedCost = (List<Cost>)apprTrends[1];     //List of merged Costs from approved trends


                //2. Then we need to merge activities of the approved trends with the baseline activities
                mergedActivityList = combineActivities(mergedApprovedActivity, baselineActivityList);

                ////Costs
                //get activitiesthat are the same
                List<Activity> matchedBaselineActivityList = new List<Activity>();
                List<Activity> matchedApprovedTrendActivityList = new List<Activity>();
                List<Activity> notMatchedActivityList = new List<Activity>();
                List<Cost> baselineCost = new List<Cost>();
                List<Cost> trendActivityCost = new List<Cost>();
                List<Cost> unMatchedActivityCost = new List<Cost>();


                List<Cost> matchedBaseCost = new List<Cost>();
                List<Cost> matchedActCost = new List<Cost>();
                List<Cost> returnedCostWithSameActivity = new List<Cost>();

                //3. Specify which activities from baseline matches approved trends
                List<List<Activity>> returnedMatchedActivity = matchedActivities(mergedApprovedActivity, baselineActivityList);
                matchedBaselineActivityList = returnedMatchedActivity[0];              //List that stores matched approved trend activities
                matchedApprovedTrendActivityList = returnedMatchedActivity[1];          //List that stores matched baseline activities

                notMatchedActivityList = getUnmatchedActivity(mergedApprovedActivity, baselineActivityList); //List that stores unmatched activities from both approved trends and baseline

                //4. for activities in baseline that does not have a match, we simply get the costs of that activities
                baselineCost = getCosts(matchedBaselineActivityList, Granularity);


                trendActivityCost = mergedApprovedCost;
                List<Cost> testCost = new List<Cost>();
                //5. get only costs that have same activityID as matchedApprovedTrendActivityList
                foreach (var cost in trendActivityCost)
                {
                    bool isMatch = false;
                    foreach (var act in matchedApprovedTrendActivityList)
                    {
                        if (cost.ActivityID == act.ActivityID.ToString())
                        {
                            isMatch = true;
                            returnedCostWithSameActivity.Add(cost);     //
                        }
                    }
                    if (isMatch == false)
                    {
                        testCost.Add(cost);   //List of costs that are not 
                    }
                }

                //6. Merge costs that has the same activities and same phase 
                foreach (var actCost in returnedCostWithSameActivity)
                {
                    Activity activity1 = new Activity();
                    foreach (var act in matchedApprovedTrendActivityList)
                    {
                        if (act.ActivityID.ToString() == actCost.ActivityID)
                            activity1 = act;
                    }
                    foreach (var baseCost in baselineCost)
                    {
                        Activity activity2 = new Activity();
                        foreach (var act2 in baselineActivityList)
                        {
                            if (act2.ActivityID.ToString() == baseCost.ActivityID)

                                activity2 = act2;

                        }
                        if (actCost.CostType == baseCost.CostType && actCost.Description == baseCost.Description)
                        {
                            if (isSameActivity(activity1, activity2))
                            {
                                if (actCost.CostType == "F")
                                {
                                    Cost fteCost = new Cost();
                                    fteCost = mergeFte(baseCost, actCost, Granularity);
                                    mergedCostList.Add(fteCost);


                                }
                                else if (actCost.CostType == "L")
                                {

                                    Cost LumpsumCost = new Cost();
                                    LumpsumCost = mergeLumpsum(baseCost, actCost, Granularity);

                                    mergedCostList.Add(LumpsumCost);
                                }
                                else if (actCost.CostType == "U")
                                {
                                    if (actCost.Base == baseCost.Base && actCost.UnitType == baseCost.UnitType)
                                    {
                                        Cost unitCost = new Cost();
                                        unitCost = mergeUnitCost(baseCost, actCost, Granularity);
                                        mergedCostList.Add(unitCost);
                                    }
                                }
                                matchedBaseCost.Add(baseCost);
                                matchedActCost.Add(actCost);
                            }
                        }

                    }
                }

                //For Costs that do not match but have the same activities and same phase, we need to copy the activity ID 
                for (int i = 0; i < matchedActCost.Count; i++)
                {
                    foreach (var actCost in trendActivityCost)
                    {
                        if (actCost.ActivityID == matchedActCost[i].ActivityID)
                        {
                            if (actCost.CostType == matchedActCost[i].CostType && actCost.Description == matchedActCost[i].Description)
                            {

                            }
                            else
                            {
                                var isMatch = isCostExisted(actCost, mergedCostList);
                                if (isMatch == false)
                                {
                                    Cost cost = new Cost();
                                    cost = actCost;
                                    cost.ActivityID = matchedBaseCost[i].ActivityID;
                                    mergedCostList.Add(cost);
                                }
                            }

                        }
                    }
                }

                //7. Add non-match approved trend Cost - Costs that do not have same activity ID and phase
                for (int i = 0; i < trendActivityCost.Count; i++)
                {
                    bool isMatched = false;
                    foreach (var cost in mergedCostList)
                    {
                        if (trendActivityCost[i].CostType == cost.CostType && trendActivityCost[i].Description == cost.Description)
                        {
                            isMatched = true;
                        }
                    }
                    if (isMatched == false)
                    {
                        mergedCostList.Add(trendActivityCost[i]);
                    }
                }
                //8. Add non-matched Base Cost
                for (int i = 0; i < baselineCost.Count; i++)
                {
                    bool isMatch = false;
                    foreach (var cost in mergedCostList)
                    {
                        //if(baseCost.ActivityID != )
                        if (cost.CostType == baselineCost[i].CostType && cost.Description == baselineCost[i].Description)
                        {
                            isMatch = true;
                        }
                    }
                    if (isMatch == false)
                    {
                        mergedCostList.Add(baselineCost[i]);
                    }
                }
                //Find cost that don't have same activities id
                List<Cost> costWithoutActivitiyID = new List<Cost>();
                foreach (var cost in mergedCostList)
                {
                    bool isMatch = false;
                    foreach (var act in mergedActivityList)
                    {
                        if (cost.ActivityID == act.ActivityID.ToString())
                        {
                            isMatch = true;
                        }
                    }
                    if (isMatch == false)
                    {
                        costWithoutActivitiyID.Add(cost);
                    }
                }
                //now find the baselinecost's activities
                foreach (var cost in costWithoutActivitiyID)
                {
                    Activity activity = getActivityByCost(cost, mergedApprovedActivity);
                    if (activity.BudgetCategory != null)
                    {
                        foreach (var act in mergedActivityList)
                        {
                            if (isSameActivity(act, activity))
                            {
                                cost.ActivityID = act.ActivityID.ToString() ;
                            }
                        }

                    }

                }
                //now update activityID for the  merged cost list
                foreach (var cost in costWithoutActivitiyID)
                {
                    foreach (var orgCost in mergedCostList)
                    {
                        if (cost.CostType == orgCost.CostType && cost.Description == orgCost.Description)
                        {
                            orgCost.ActivityID = cost.ActivityID;
                        }
                    }
                }
                //Get unmatched Activity's Cost
                // unMatchedActivityCost = getCosts(notMatchedActivityList);
                List<Cost> tempCost = new List<Cost>();
                //9. Get costs
                foreach (var cost in testCost)
                {
                    bool isMatch = false;
                    foreach (var c in mergedCostList)
                    {
                        if (cost.CostType == c.CostType && cost.Description == c.Description && cost.ActivityID == c.ActivityID)
                        {
                            isMatch = true;
                        }
                    }
                    if (isMatch == false)
                    {
                        tempCost.Add(cost);
                    }
                }

                foreach (var cost in tempCost)
                {
                    mergedCostList.Add(cost);
                }

                //10.
                foreach (var act in notMatchedActivityList)
                {
                    bool isMatch = false;
                    foreach (var activity in baselineActivityList)
                    {
                        if (act.ActivityID == activity.ActivityID && act.PhaseCode == activity.PhaseCode &&
                            act.BudgetSubCategory == activity.BudgetSubCategory && act.BudgetCategory == activity.BudgetCategory)
                        {
                            isMatch = true;
                        }

                    }
                    if (isMatch == true)
                    {
                        List<Activity> actL = new List<Activity>();
                        actL.Add(act);
                        List<Cost> costL = getCosts(actL, Granularity);
                        foreach (var cost in costL)
                        {
                            mergedCostList.Add(cost);
                        }

                    }
                }

                //fix all cost date so that they are in the correct Position
                List<Cost> resultCost = new List<Cost>();
                foreach (var cost in mergedCostList)
                {
                    var isMatch = false;
                    foreach (var act in mergedActivityList)
                    {

                        if (cost.ActivityID == act.ActivityID.ToString())
                        {
                            Cost temporaryCost = new Cost();
                            temporaryCost = fixCostDate(cost, act);
                            resultCost.Add(temporaryCost);
                            isMatch = true;

                        }
                    }
                    if (isMatch == false)
                    {
                        resultCost.Add(cost);
                    }
                }
            }

            //Quick hack
            using (var ctx = new CPPDbContext())
            {
                try
                {
                    List<Trend> currentTrendList = ctx.Trend.Where(a => a.TrendStatusID == 1 && a.ProjectID == ProjectID).OrderBy(a=>a.TrendNumber).ToList();
                     lastTrend = currentTrendList[currentTrendList.Count - 1];
                }
                catch
                {

                }
            }

            //save activity
            //foreach(Activity activity in mergedActivityList)
            //{
            //    try
            //    {
            //        var tempActivityId = activity.ActivityID;
            //        activity.ActivityID = 0;
            //        activity.TrendNumber = "1000";

            //        String result = WebAPI.Models.Activity.registerActivity(activity);
            //        var activityId = result.Split(',')[1];


            //        foreach(Cost cost in mergedCostList)
            //        {
            //            if (cost.CostType == "F")
            //                string fteCostStatus = WebAPI.Models.CostFTE.updateCostFTE(cost.Operation, cost.ProgramID, cost.ProgramElementID, cost.ProjectID, cost.TrendNumber, cost.ActivityID, cost.CostID, cost.StartDate, cost.EndDate, cost.Description, cost.TextBoxValue, cost.Base, cost.FTEHours, cost.FTECost, cost.Scale, cost.FTEIDList, cost.EmployeeID);

            //            //if (cost.CostType == "L")
            //            //    status = WebAPI.Models.CostLumpsum.updateCostLumpsum(cost.Operation, cost.ProgramID, cost.ProgramElementID, cost.ProjectID, cost.TrendNumber, cost.ActivityID, cost.CostID, cost.StartDate, cost.EndDate, cost.Description, cost.TextBoxValue, cost.Scale, cost.FTEIDList);

            //            //if (cost.CostType == "U")
            //            //    status = WebAPI.Models.CostUnit.updateCostUnit(cost.Operation, cost.ProgramID, cost.ProgramElementID, cost.ProjectID, cost.TrendNumber, cost.ActivityID, cost.CostID, cost.StartDate, cost.EndDate, cost.Description, cost.TextBoxValue, cost.Base, cost.Scale, cost.UnitType, cost.FTEIDList);
            //            ////ODC
            //            //if (cost.CostType == "O")
            //            //    status = WebAPI.Models.CostODC.updateCostODC(cost.Operation, cost.ProgramID, cost.ProgramElementID, cost.ProjectID, cost.TrendNumber, cost.ActivityID, cost.CostID, cost.StartDate, cost.EndDate, cost.Description, cost.TextBoxValue, cost.Base, cost.Scale, cost.UnitType, cost.FTEIDList);

            //        }

            //    }
            //    catch(Exception ex)
            //    {
            //        Console.WriteLine(ex.ToString());
            //    }     
   
            //}
          
            //save cost
            //if (cost.CostType == "F")
            //    status = WebAPI.Models.CostFTE.updateCostFTE(cost.Operation, cost.ProgramID, cost.ProgramElementID, cost.ProjectID, cost.TrendNumber, cost.ActivityID, cost.CostID, cost.StartDate, cost.EndDate, cost.Description, cost.TextBoxValue, cost.Base, cost.FTEHours, cost.FTECost, cost.Scale, cost.FTEIDList, cost.EmployeeID);

            //if (cost.CostType == "L")
            //    status = WebAPI.Models.CostLumpsum.updateCostLumpsum(cost.Operation, cost.ProgramID, cost.ProgramElementID, cost.ProjectID, cost.TrendNumber, cost.ActivityID, cost.CostID, cost.StartDate, cost.EndDate, cost.Description, cost.TextBoxValue, cost.Scale, cost.FTEIDList);

            //if (cost.CostType == "U")
            //    status = WebAPI.Models.CostUnit.updateCostUnit(cost.Operation, cost.ProgramID, cost.ProgramElementID, cost.ProjectID, cost.TrendNumber, cost.ActivityID, cost.CostID, cost.StartDate, cost.EndDate, cost.Description, cost.TextBoxValue, cost.Base, cost.Scale, cost.UnitType, cost.FTEIDList);
            ////ODC
            //if (cost.CostType == "O")
            //    status = WebAPI.Models.CostODC.updateCostODC(cost.Operation, cost.ProgramID, cost.ProgramElementID, cost.ProjectID, cost.TrendNumber, cost.ActivityID, cost.CostID, cost.StartDate, cost.EndDate, cost.Description, cost.TextBoxValue, cost.Base, cost.Scale, cost.UnitType, cost.FTEIDList);



            //mergedActivityList
            obj.Add(project);
            obj.Add(PhaseCodeList);
            obj.Add(mergedActivityList);
            obj.Add(mergedCostList);
            obj.Add(lastTrend);

            return obj;
        }

        public static List<Cost> mergeCostWeek()
        {
            List<Cost> mergedCostList = new List<Cost>();


            return mergedCostList;
        }
        public static Cost fixCostDate(Cost cost, Activity activity)
        {
            //just to push update
            Cost newCost = new Cost();
            newCost = cost;
            String result = "";
            String resultEndDate = "";
            String textBoxResult = "";
            String fteHours = "";
            String ftehour = "";
            if (newCost.CostType == "F")
            {
                ftehour = newCost.FTEHours.Split(',')[0];
            }
            String fteCosts = "";
            var activityStartDate = Convert.ToDateTime(activity.ActivityStartDate);
            var costStartDate = Convert.ToDateTime(newCost.StartDate.Split(',')[0]);

            if (costStartDate > activityStartDate)
            {
                if (cost.CostType == "F")
                {


                    var diff = costStartDate - activityStartDate;
                    var days = diff.Days;
                    while (days >= 7)
                    {
                        result += activityStartDate.Date.ToString("yyyy-MM-dd") + " ,";
                        resultEndDate += activityStartDate.AddDays(7).Date.ToString("yyyy-MM-dd") + ",";
                        textBoxResult += "0,";
                        fteHours += ftehour + ",";
                        fteCosts += "0,";
                        days = days - 7;
                        activityStartDate = activityStartDate.AddDays(7);

                    }
                }

            }
            newCost.StartDate = result + newCost.StartDate;
            newCost.EndDate = resultEndDate + newCost.EndDate;
            newCost.TextBoxValue = textBoxResult + newCost.TextBoxValue;
            if (newCost.CostType == "F")
            {
                newCost.FTEHours = fteHours + newCost.FTEHours;
                newCost.FTECost = fteCosts + newCost.FTECost;
            }

            List<String> textBoxId = new List<String>();
            var textBoxValues = newCost.TextBoxValue.Split(',');
            for (int i = 0; i < textBoxValues.Length; i++)
            {
                textBoxId.Add(i.ToString());
            }
            newCost.TextBoxID = String.Join(",", textBoxId).ToString();
            return newCost;
        }
        public static Activity getActivityByCost(Cost cost, List<Activity> activityList)
        {
            Activity activity = new Activity();
            bool isMatch = false;
            foreach (var act in activityList)
            {
                if (cost.ActivityID == act.ActivityID.ToString())
                {
                    activity = act;
                }
            }
            return activity;
        }
        public static bool isCostExisted(Cost cost, List<Cost> costList)
        {
            bool isMatch = false;
            foreach (var actCost in costList)
            {
                if (actCost.CostType == cost.CostType && actCost.Description == cost.Description)
                {
                    isMatch = true;
                }
            }

            return isMatch;
        }


        public static bool isEndOfMonth(DateTime date)
        {
            return date.AddDays(1).Day == 1;
        }
        public static bool isStartOfMonth(DateTime date)
        {
            return date.Day == 1;
        }
    }

   
}