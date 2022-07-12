using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebAPI.Helper;
using WebAPI.Models.StoredProcedure;

namespace WebAPI.Models
{
    public class DataMigration
    {
        public int ProjectId { get; set; }

        private static String SQL_DATE_FORMAT = "yyyy-MM-dd HH:mm:ss";
        public static void mergeTrends(int projectID)
        {
            var ctx = new CPPDbContext();
            //1. Merge Trends
            //Current Trend and baseline trend are created when project is created
            //Current trend will have trend number of 1000
            //I. Delete existing trend

            //II. Create new
            Trend currentTrend = ctx.Trend.Where(a => a.ProjectID == projectID && a.TrendNumber == "1000").FirstOrDefault();
            Project project = ctx.Project.Where(a => a.ProjectID == projectID).FirstOrDefault();



            //2. Merge Activities
            try
            {
                //Store the current state of actual cost and etc cost to temporary table
                StoreCostToTempTable(projectID);

                //Delete All activities and costs for the project 
                ctx.Database.ExecuteSqlCommand("call remove_current_project_cost(@ProjectID)",
                    new MySql.Data.MySqlClient.MySqlParameter("@ProjectID", projectID));


                List<Activity> activities = ctx.Database.SqlQuery<Activity>("call get_activities_to_be_merged(@ProjectID)",
                                                    new MySql.Data.MySqlClient.MySqlParameter("@ProjectID", projectID)).ToList();


                foreach (Activity act in activities)
                {

                    PhaseCode phase = ctx.PhaseCode.Where(a => a.PhaseID == act.PhaseCode).FirstOrDefault();

                    act.TrendID = currentTrend.TrendID;
                    if (project.OrganizationID != null)
                        act.OrganizationID = Int32.Parse(project.OrganizationID);
                    if (project.ProgramID != 0)
                        act.ProgramID = project.ProgramID.ToString();
                    if (project.ProgramElementID != 0)
                        act.ProgramElementID = project.ProgramElementID.ToString();
                    act.TrendID = currentTrend.TrendID;


                    var lineId = 1;
                    var existingAct = findExistingActivity(act);
                    if (existingAct != null)
                    {
                        //update
                        var oldPercentageCompletion = existingAct.PercentageCompletion;
                        CopyUtil.CopyFields<Activity>(act, existingAct);
                        existingAct.PercentageCompletion = oldPercentageCompletion;
                        ctx.Entry<Activity>(existingAct).State = System.Data.Entity.EntityState.Modified;
                        ctx.SaveChanges();
                        act.ActivityID = existingAct.ActivityID;

                    }
                    else
                    {
                        ctx.Activity.Add(act);
                        ctx.SaveChanges();
                    }

                    //Save and merge cost

                    //1. FTE
                    //lineId = mergeFteCost(projectID, act, lineId);
                    lineId = mergeFteCostForRollUp(projectID, act, lineId);
                    //2. Lumpsum
                    //lineId = mergeLumpsumCost(projectID, act, lineId);
                    lineId = mergeLumpsumCostForRollUp(projectID, act, lineId);
                    //3. Unit Cost
                    //lineId = mergeMaterialCost(projectID, act, lineId);
                    lineId = mergeMaterialCostForRollUp(projectID, act, lineId);
                    //4. ODC Cost
                    //lineId = mergeODCCost(projectID, act, lineId);
                    lineId = mergeODCCostForRollUp(projectID, act, lineId);

                    var phaseCode = phase.PhaseID;
                    // var phaseCode = phase.ActivityPhaseCode;

                    //Update actual,etc from temporary back to the correct table
                    //updateCostFromTempTable(projectID, phaseCode.ToString(), act.BudgetCategory, act.BudgetSubCategory);
                    updateCostFromTempTableForRollup(projectID, phaseCode.ToString(), act.BudgetCategory, act.BudgetSubCategory);
                    //Scale All Actual-Etc Cost
                    scaleActualEtcCost("", projectID.ToString(), act.ActivityID.ToString());
                    //Remove cost from temporary table
                    //RemoveCostsFromTempTable(projectID,phaseCode.ToString(),act.BudgetCategory, act.BudgetSubCategory);

                    RemoveCostsFromTempTableForRollup(projectID, phaseCode.ToString(), act.BudgetCategory, act.BudgetSubCategory);

                }






            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }

        public static void StoreCostToTempTable(int projectID)
        {
            try
            {
                var ctx = new CPPDbContext();
                ctx.Database.ExecuteSqlCommand("call store_actual_etc_to_temporary_Migration(@ProjectID)",
                          new MySql.Data.MySqlClient.MySqlParameter("@ProjectID", projectID));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Activity findExistingActivity(Activity act)
        {
            Activity result = null;
            using (var ctx = new CPPDbContext())
            {
                var existingActivity = ctx.Activity.Where(
                                                            a => a.BudgetID == act.BudgetID
                                                                && a.BudgetCategory == act.BudgetCategory
                                                                && a.BudgetSubCategory == act.BudgetSubCategory
                                                                && a.PhaseCode == act.PhaseCode
                                                                && a.ProjectID == act.ProjectID
                                                                && a.TrendID == act.TrendID
                                                        )
                                                        .FirstOrDefault();
                if (existingActivity != null)
                    result = existingActivity;
            }

            return result;
        }

        public static int mergeFteCostForRollUp(int projectID, Activity act, int lineId)
        {
            var ctx = new CPPDbContext();
            PhaseCode phase = ctx.PhaseCode.Where(a => a.PhaseID == act.PhaseCode).FirstOrDefault();
            IEnumerable<List<FTE>> fteCost = ctx.Database.SqlQuery<FTE>("call getCostForRollup(@ProjectID, @BudgetCategory, @BudgetSubCategory, @PhaseCode,@CostType,@TrendNumber)",
                                                          new MySql.Data.MySqlClient.MySqlParameter("@ProjectID", projectID),
                                                          new MySql.Data.MySqlClient.MySqlParameter("@BudgetCategory", act.BudgetCategory),
                                                          new MySql.Data.MySqlClient.MySqlParameter("@BudgetSubCategory", act.BudgetSubCategory),
                                                          new MySql.Data.MySqlClient.MySqlParameter("@PhaseCode", phase.ActivityPhaseCode),
                                                          new MySql.Data.MySqlClient.MySqlParameter("@CostType", "F"),
                                                          new MySql.Data.MySqlClient.MySqlParameter("@TrendNumber", act.TrendNumber))
                                                            .GroupBy(a => new { a.FTEPosition, a.EmployeeID }).Select(a => a.ToList());


            var costId = lineId;
            var actualCostId = lineId;
            var etcCostId = lineId;
            Project project = ctx.Project.Where(a => a.ProjectID == projectID).FirstOrDefault();
            var projectClass = ctx.ServiceClass.Where(a => a.ID == project.ProjectClassID).FirstOrDefault();
            ProgramElement programElement = ctx.ProgramElement.Include("ProjectClass").Where(a => a.ProgramElementID == project.ProgramElementID).FirstOrDefault();
            var programElementClass = ctx.ProjectClass.Where(a => a.ProjectClassID == programElement.ProjectClassID).FirstOrDefault();  //Manasi 27-10-2020
            ActivityCategory category = ctx.ActivityCategory.Where(a => a.ID == act.BudgetID).FirstOrDefault();
            foreach (List<FTE> fteList in fteCost)
            {


                //Padding left Cost
                var startDate = fteList.FirstOrDefault().FTEStartDate;
                var activityStart = act.ActivityStartDate;
                var diff = (DateTime.Parse(startDate) - DateTime.Parse(activityStart)).TotalDays;
                actualCostId = costId + 1;
                etcCostId = actualCostId + 1;
                var textboxIndex = 0;
                CostLineItemResult lineItem = CostLineItemResult.getCostLineItem(projectClass.Code.ToString(), project.ProjectNumber, project.ProjectElementNumber, act.TrendNumber, phase.ActivityPhaseCode.ToString(), category.CategoryID,
                                                            category.SubCategoryID, "F", fteList[0].FTEPositionID, fteList[0].EmployeeID, null, null, null, null, null);

                //Manasi 07-11-2020
                CostLineItemResult newLineItem = CostLineItemResult.getNewCostLineItem(projectClass.Code.ToString(), project.ProjectNumber, project.ProjectElementNumber, act.TrendNumber, phase.ActivityPhaseCode.ToString(), category.CategoryID,
                                                            category.SubCategoryID, "F", fteList[0].FTEPositionID, fteList[0].EmployeeID, null, null, null, null, null, projectID.ToString());

                String LineNumber = "";
                if (lineItem.LineNumber.ToString().Length <= 1)
                {
                    LineNumber = "0" + lineItem.LineNumber;
                }

                //Manasi 07-11-2020
                if (newLineItem.LineNumber.ToString().Length <= 1)
                {
                    LineNumber = "0" + newLineItem.LineNumber;
                }
                else
                {
                    LineNumber = newLineItem.LineNumber.ToString();
                }

                //--------------------------Manasi 27-10-2020------------------------------
                string year = programElement.ProjectStartDate.Split('/')[2].ToString();
                year = year.Substring(2, 2);
                string costLineItem = Cost.getLineItem(projectClass.Code.ToString(), programElement.ProjectNumber, project.ProjectElementNumber,
                                            phase.ActivityPhaseCode.ToString(),
                                      category.CategoryID, category.SubCategoryID, LineNumber, year, "1", programElementClass.ProjectClassLineItemID);

                ///TODO -- Handling multiple costs
                //List<TemporaryCost> additionalCost = ctx.Database.SqlQuery<TemporaryCost>("call get_additional_actual_etc_cost(@CostLineItem, @ProjectID, @CostType)",
                //                                      new MySql.Data.MySqlClient.MySqlParameter("@CostLineItem", costLineItem + "." + "02"),
                //                                      new MySql.Data.MySqlClient.MySqlParameter("@ProjectID", projectID),
                //                                      new MySql.Data.MySqlClient.MySqlParameter("@CostType", "FTE")).ToList();

                List<TemporaryCost> additionalCost = ctx.Database.SqlQuery<TemporaryCost>("call get_additional_actual_etc_cost(@CostLineItem, @ProjectID, @CostType)",
                                                      new MySql.Data.MySqlClient.MySqlParameter("@CostLineItem", costLineItem + "2"),
                                                      new MySql.Data.MySqlClient.MySqlParameter("@ProjectID", projectID),
                                                      new MySql.Data.MySqlClient.MySqlParameter("@CostType", "FTE")).ToList();

                String dt = DateTime.Parse(activityStart).ToString(SQL_DATE_FORMAT);
                List<int> lineItemsToScale = new List<int>();

                //IF The start date of the first cost cell is after the activity start date
                //Add '0' cost cell.
                while (diff > 0)
                {

                    CostFTE costFte = new CostFTE();
                    String fteCostID = act.ActivityID.ToString() + "_" + costId + "_" + textboxIndex;
                    costFte.FTECostID = fteCostID;
                    costFte.ActivityID = act.ActivityID;
                    costFte.FTEStartDate = activityStart;
                    costFte.FTEEndDate = DateTime.Parse(activityStart).AddDays(6).ToString(SQL_DATE_FORMAT);
                    costFte.FTEPosition = fteList.FirstOrDefault().FTEPosition;
                    costFte.FTEPositionID = Int32.Parse(fteList.FirstOrDefault().FTEPositionID);
                    costFte.FTECost = "0";
                    costFte.FTEValue = "0";
                    costFte.FTEHourlyRate = fteList.FirstOrDefault().FTEHourlyRate;
                    costFte.FTEHours = "0";
                    costFte.RawFTEHourlyRate = fteList.FirstOrDefault().RawFTEHourlyRate;
                    costFte.Granularity = "week";
                    costFte.CostTrackTypeID = 2;//--todo
                    costFte.EstimatedCostID = Int32.Parse("0");
                    costFte.EmployeeID = Int32.Parse(fteList.FirstOrDefault().EmployeeID);
                    costFte.CostLineItemID = costLineItem;
                    costFte.OriginalCost = "0";
                    ctx.CostFte.Add(costFte);
                    ctx.SaveChanges();

                    CostFTE actualCostFte = ctx.CostFte.AsNoTracking().FirstOrDefault(a => a.ID == costFte.ID);
                    actualCostFte.FTECostID = act.ActivityID.ToString() + "_" + actualCostId + "_" + textboxIndex;
                    actualCostFte.CostTrackTypeID = 3;
                    actualCostFte.EstimatedCostID = costFte.ID;
                    actualCostFte.FTEHours = "0";
                    actualCostFte.FTECost = "0";
                    actualCostFte.FTEValue = "0";
                    actualCostFte.FTEHourlyRate = fteList.FirstOrDefault().FTEHourlyRate;
                    actualCostFte.RawFTEHourlyRate = fteList.FirstOrDefault().RawFTEHourlyRate;
                    actualCostFte.FTEStartDate = DateTime.Parse(actualCostFte.FTEStartDate).ToString(SQL_DATE_FORMAT);
                    actualCostFte.FTEEndDate = DateTime.Parse(actualCostFte.FTEEndDate).ToString(SQL_DATE_FORMAT);
                    actualCostFte.ID = 0;
                    actualCostFte.CostLineItemID = costLineItem + "1";//Actual Cost   //Manasi 30-10-2020
                    actualCostFte.OriginalCost = "0";
                    ctx.CostFte.Add(actualCostFte);
                    ctx.SaveChanges();

                    CostFTE etcCostFte = ctx.CostFte.AsNoTracking().FirstOrDefault(a => a.ID == costFte.ID);
                    etcCostFte.FTECostID = act.ActivityID.ToString() + "_" + etcCostId + "_" + textboxIndex;
                    etcCostFte.CostTrackTypeID = 4;
                    etcCostFte.EstimatedCostID = costFte.ID;
                    etcCostFte.FTEHours = "0";
                    etcCostFte.FTECost = "0";
                    etcCostFte.FTEValue = "0";
                    etcCostFte.FTEHourlyRate = fteList.FirstOrDefault().FTEHourlyRate;
                    etcCostFte.RawFTEHourlyRate = fteList.FirstOrDefault().RawFTEHourlyRate;
                    etcCostFte.FTEStartDate = DateTime.Parse(etcCostFte.FTEStartDate).ToString(SQL_DATE_FORMAT);
                    etcCostFte.FTEEndDate = DateTime.Parse(etcCostFte.FTEEndDate).ToString(SQL_DATE_FORMAT);
                    etcCostFte.ID = 0;
                    etcCostFte.OriginalCost = "0";
                    etcCostFte.CostLineItemID = costLineItem + "2";    //Manasi 30-10-2020
                    ctx.CostFte.Add(etcCostFte);
                    ctx.SaveChanges();




                    textboxIndex += 1;
                    activityStart = DateTime.Parse(activityStart).AddDays(7).ToString(SQL_DATE_FORMAT); ;
                    diff -= 7;
                }

                //Padding Right Cost
                var endDate = fteList.LastOrDefault().FTEEndDate;
                var activityStartRight = act.ActivityStartDate;
                var endDiff = (DateTime.Parse(activityStartRight) - DateTime.Parse(endDate)).TotalDays;


                while (endDiff > 0)
                {
                    CostFTE costFte = new CostFTE();
                    String fteCostID = act.ActivityID.ToString() + "_" + costId + "_" + textboxIndex;
                    costFte.FTECostID = fteCostID;
                    costFte.Granularity = "week";
                    costFte.ActivityID = act.ActivityID;
                    costFte.FTEStartDate = DateTime.Parse(endDate).AddDays(1).ToString(SQL_DATE_FORMAT);
                    costFte.FTEEndDate = DateTime.Parse(costFte.FTEStartDate).AddDays(6).ToString(SQL_DATE_FORMAT);
                    costFte.FTEPosition = fteList.FirstOrDefault().FTEPosition;
                    costFte.FTEValue = "0";
                    costFte.FTEHourlyRate = fteList.FirstOrDefault().FTEHourlyRate;
                    costFte.RawFTEHourlyRate = fteList.FirstOrDefault().RawFTEHourlyRate;
                    costFte.FTEHours = "0";
                    costFte.FTECost = "0";
                    costFte.OriginalCost = "0";
                    costFte.FTEPositionID = Int32.Parse(fteList.FirstOrDefault().FTEPositionID);
                    costFte.EmployeeID = Int32.Parse(fteList.FirstOrDefault().EmployeeID);
                    costFte.CostTrackTypeID = 2;//--todo
                    costFte.CostLineItemID = costLineItem;
                    costFte.EstimatedCostID = 0;

                    ctx.CostFte.Add(costFte);
                    ctx.SaveChanges();

                    CostFTE actualCostFte = ctx.CostFte.AsNoTracking().FirstOrDefault(a => a.ID == costFte.ID);
                    actualCostFte.FTECostID = act.ActivityID.ToString() + "_" + actualCostId + "_" + textboxIndex;
                    actualCostFte.CostTrackTypeID = 3;
                    actualCostFte.EstimatedCostID = costFte.ID;
                    actualCostFte.FTEHours = "0";
                    actualCostFte.FTECost = "0";
                    actualCostFte.FTEValue = "0";
                    actualCostFte.OriginalCost = "0";
                    actualCostFte.RawFTEHourlyRate = fteList.FirstOrDefault().RawFTEHourlyRate;
                    actualCostFte.FTEHourlyRate = fteList.FirstOrDefault().FTEHourlyRate;
                    actualCostFte.FTEStartDate = DateTime.Parse(actualCostFte.FTEStartDate).ToString(SQL_DATE_FORMAT);
                    actualCostFte.FTEEndDate = DateTime.Parse(actualCostFte.FTEEndDate).ToString(SQL_DATE_FORMAT);
                    actualCostFte.ID = 0;
                    actualCostFte.CostLineItemID = costLineItem + "1";//Actual Cost  Manasi 30-10-2020
                    ctx.CostFte.Add(actualCostFte);
                    ctx.SaveChanges();

                    CostFTE etcCostFte = ctx.CostFte.AsNoTracking().FirstOrDefault(a => a.ID == costFte.ID);
                    etcCostFte.FTECostID = act.ActivityID.ToString() + "_" + etcCostId + "_" + textboxIndex;
                    etcCostFte.CostTrackTypeID = 4;
                    etcCostFte.EstimatedCostID = costFte.ID;
                    etcCostFte.FTEHours = "0";
                    etcCostFte.FTECost = "0";
                    etcCostFte.FTEValue = "0";
                    etcCostFte.OriginalCost = "0";
                    etcCostFte.RawFTEHourlyRate = fteList.FirstOrDefault().RawFTEHourlyRate;
                    etcCostFte.FTEHourlyRate = fteList.FirstOrDefault().FTEHourlyRate;
                    etcCostFte.FTEStartDate = DateTime.Parse(etcCostFte.FTEStartDate).ToString(SQL_DATE_FORMAT);
                    etcCostFte.FTEEndDate = DateTime.Parse(etcCostFte.FTEEndDate).ToString(SQL_DATE_FORMAT);
                    etcCostFte.ID = 0;
                    etcCostFte.CostLineItemID = costLineItem + "2";  //Manasi 30-10-2020
                    ctx.CostFte.Add(etcCostFte);
                    ctx.SaveChanges();

                    textboxIndex += 1;
                    activityStart = DateTime.Parse(activityStart).AddDays(6).ToString(SQL_DATE_FORMAT); ;
                    endDiff -= 7;

                }


                for (int i = 0; i < fteList.Count; i++)
                {
                    var fteDate = DateTime.Parse(fteList.ElementAt(i).FTEStartDate).ToString(SQL_DATE_FORMAT);

                    var cost = fteList.ElementAt(i);
                    CostFTE costFte = new CostFTE();
                    String fteCostID = act.ActivityID.ToString() + "_" + costId + "_" + textboxIndex;
                    costFte.FTECostID = fteCostID;
                    costFte.Granularity = cost.Granularity;
                    costFte.ActivityID = act.ActivityID;
                    costFte.FTEStartDate = DateTime.Parse(cost.FTEStartDate).ToString(SQL_DATE_FORMAT);
                    costFte.FTEEndDate = DateTime.Parse(cost.FTEEndDate).ToString(SQL_DATE_FORMAT);
                    costFte.FTEPosition = cost.FTEPosition;
                    costFte.FTEValue = cost.FTEValue;
                    costFte.FTEHourlyRate = cost.FTEHourlyRate;
                    costFte.RawFTEHourlyRate = cost.RawFTEHourlyRate;
                    costFte.FTEHours = cost.FTEHours;
                    costFte.FTECost = cost.FTECost;
                    costFte.FTEPositionID = Int32.Parse(cost.FTEPositionID);
                    costFte.OriginalCost = cost.OriginalCost;
                    costFte.CostTrackTypeID = 2;//--todo
                    costFte.EstimatedCostID = 0;
                    costFte.CostLineItemID = costLineItem;
                    costFte.EmployeeID = Int32.Parse(cost.EmployeeID);
                    ctx.CostFte.Add(costFte);
                    ctx.SaveChanges();

                    CostFTE actualCostFte = ctx.CostFte.AsNoTracking().FirstOrDefault(a => a.ID == costFte.ID);
                    actualCostFte.FTECostID = act.ActivityID.ToString() + "_" + actualCostId + "_" + textboxIndex;
                    actualCostFte.CostTrackTypeID = 3;
                    actualCostFte.EstimatedCostID = costFte.ID;
                    actualCostFte.FTEHours = "0";
                    actualCostFte.FTECost = "0";
                    actualCostFte.FTEValue = "0";
                    actualCostFte.OriginalCost = "0";
                    actualCostFte.FTEHourlyRate = cost.FTEHourlyRate;
                    actualCostFte.RawFTEHourlyRate = cost.RawFTEHourlyRate;
                    actualCostFte.FTEStartDate = DateTime.Parse(actualCostFte.FTEStartDate).ToString(SQL_DATE_FORMAT);
                    actualCostFte.FTEEndDate = DateTime.Parse(actualCostFte.FTEEndDate).ToString(SQL_DATE_FORMAT);
                    actualCostFte.ID = 0;
                    actualCostFte.CostLineItemID = costLineItem + "1";//Actual Cost   Manasi 30-10-2020
                    ctx.CostFte.Add(actualCostFte);
                    ctx.SaveChanges();

                    CostFTE etcCostFte = ctx.CostFte.AsNoTracking().FirstOrDefault(a => a.ID == costFte.ID);
                    etcCostFte.FTECostID = act.ActivityID.ToString() + "_" + etcCostId + "_" + textboxIndex;
                    etcCostFte.CostTrackTypeID = 4;
                    etcCostFte.EstimatedCostID = costFte.ID;
                    etcCostFte.FTEHours = "0";
                    etcCostFte.FTECost = "0";
                    etcCostFte.FTEValue = "0";
                    etcCostFte.OriginalCost = "0";
                    etcCostFte.FTEHourlyRate = cost.FTEHourlyRate;
                    etcCostFte.RawFTEHourlyRate = cost.RawFTEHourlyRate;
                    etcCostFte.FTEStartDate = DateTime.Parse(etcCostFte.FTEStartDate).ToString(SQL_DATE_FORMAT);
                    etcCostFte.FTEEndDate = DateTime.Parse(etcCostFte.FTEEndDate).ToString(SQL_DATE_FORMAT);
                    etcCostFte.ID = 0;
                    etcCostFte.CostLineItemID = costLineItem + "2";//etc Cost    Manasi 30-10-2020
                    ctx.CostFte.Add(etcCostFte);
                    ctx.SaveChanges();
                    //TODO
                    List<TemporaryCost> tempCost = additionalCost.Where(a => (a.CostTrackTypeID == "3") && a.StartDate.Equals(fteDate)).ToList();
                    int additionalActualID = etcCostId;
                    int additionalETCID = additionalActualID;
                    foreach (TemporaryCost costs in tempCost)
                    {


                        additionalActualID += 1;
                        additionalETCID = additionalActualID + 1;
                        actualCostFte = ctx.CostFte.AsNoTracking().FirstOrDefault(a => a.ID == costFte.ID);
                        actualCostFte.FTECostID = act.ActivityID.ToString() + "_" + additionalActualID + "_" + textboxIndex;
                        actualCostFte.CostTrackTypeID = 3;
                        actualCostFte.EstimatedCostID = costFte.ID;
                        actualCostFte.FTEHours = "0";
                        actualCostFte.FTECost = "0";
                        actualCostFte.FTEValue = "0";
                        actualCostFte.OriginalCost = "0";
                        actualCostFte.FTEHourlyRate = cost.FTEHourlyRate;
                        actualCostFte.RawFTEHourlyRate = cost.RawFTEHourlyRate;
                        actualCostFte.FTEStartDate = DateTime.Parse(actualCostFte.FTEStartDate).ToString(SQL_DATE_FORMAT);
                        actualCostFte.FTEEndDate = DateTime.Parse(actualCostFte.FTEEndDate).ToString(SQL_DATE_FORMAT);
                        actualCostFte.ID = 0;
                        actualCostFte.EmployeeID = Convert.ToInt16(costs.Name);
                        actualCostFte.FTEPositionID = Convert.ToInt16(costs.Type);
                        actualCostFte.CostLineItemID = costLineItem + (additionalActualID - 1).ToString();//Actual Cost  Manasi 30-10-2020

                        ctx.CostFte.Add(actualCostFte);
                        ctx.SaveChanges();

                        etcCostFte = ctx.CostFte.AsNoTracking().FirstOrDefault(a => a.ID == costFte.ID);
                        etcCostFte.FTECostID = act.ActivityID.ToString() + "_" + additionalETCID + "_" + textboxIndex;
                        etcCostFte.CostTrackTypeID = 4;
                        etcCostFte.EstimatedCostID = costFte.ID;
                        etcCostFte.FTEHours = "0";
                        etcCostFte.FTECost = "0";
                        etcCostFte.FTEValue = "0";
                        etcCostFte.OriginalCost = "0";
                        etcCostFte.FTEHourlyRate = cost.FTEHourlyRate;
                        etcCostFte.RawFTEHourlyRate = cost.RawFTEHourlyRate;
                        etcCostFte.FTEStartDate = DateTime.Parse(etcCostFte.FTEStartDate).ToString(SQL_DATE_FORMAT);
                        etcCostFte.FTEEndDate = DateTime.Parse(etcCostFte.FTEEndDate).ToString(SQL_DATE_FORMAT);
                        etcCostFte.ID = 0;
                        etcCostFte.EmployeeID = Convert.ToInt16(costs.Name);
                        etcCostFte.FTEPositionID = Convert.ToInt16(costs.Type);
                        etcCostFte.CostLineItemID = costLineItem + (additionalETCID - 1).ToString();//etc Cost   Manasi 30-10-2020

                        ctx.CostFte.Add(etcCostFte);
                        ctx.SaveChanges();
                        lineItemsToScale.Add(additionalActualID);
                        lineItemsToScale.Add(additionalETCID);
                    }

                    textboxIndex += 1;

                    //Padding differenc in the middle
                    if (i + 1 != fteList.Count)
                    {
                        var nextCost = fteList.ElementAt(i + 1);
                        var diffDate = (DateTime.Parse(nextCost.FTEStartDate) - DateTime.Parse(cost.FTEEndDate)).TotalDays;
                        var eDate = cost.FTEEndDate;
                        while (diffDate > 1)
                        {
                            costFte = new CostFTE();
                            fteCostID = act.ActivityID.ToString() + "_" + costId + "_" + textboxIndex;
                            costFte.FTECostID = fteCostID;
                            costFte.ActivityID = act.ActivityID;
                            costFte.Granularity = "week";
                            costFte.FTEStartDate = DateTime.Parse(eDate).AddDays(1).ToString(SQL_DATE_FORMAT);
                            costFte.FTEEndDate = DateTime.Parse(costFte.FTEStartDate).AddDays(6).ToString(SQL_DATE_FORMAT);
                            costFte.FTEPosition = cost.FTEPosition;
                            costFte.FTEValue = "0";
                            costFte.OriginalCost = "0";
                            costFte.FTEHourlyRate = cost.FTEHourlyRate;
                            costFte.RawFTEHourlyRate = cost.RawFTEHourlyRate;
                            costFte.FTEHours = "0";
                            costFte.FTECost = "0";
                            costFte.FTEPositionID = Int32.Parse(cost.FTEPositionID);
                            costFte.EmployeeID = Int32.Parse(cost.EmployeeID);
                            costFte.CostTrackTypeID = 2;//--todo
                            costFte.EstimatedCostID = 0;
                            costFte.CostLineItemID = costLineItem;

                            ctx.CostFte.Add(costFte);
                            ctx.SaveChanges();

                            actualCostFte = ctx.CostFte.AsNoTracking().FirstOrDefault(a => a.ID == costFte.ID);
                            actualCostFte.FTECostID = act.ActivityID.ToString() + "_" + actualCostId + "_" + textboxIndex;
                            actualCostFte.CostTrackTypeID = 3;
                            actualCostFte.EstimatedCostID = costFte.ID;
                            actualCostFte.FTEHours = "0";
                            actualCostFte.FTECost = "0";
                            actualCostFte.OriginalCost = "0";
                            actualCostFte.FTEHourlyRate = cost.FTEHourlyRate;
                            actualCostFte.RawFTEHourlyRate = cost.RawFTEHourlyRate;
                            actualCostFte.FTEStartDate = DateTime.Parse(actualCostFte.FTEStartDate).ToString(SQL_DATE_FORMAT);
                            actualCostFte.FTEEndDate = DateTime.Parse(actualCostFte.FTEEndDate).ToString(SQL_DATE_FORMAT);
                            actualCostFte.ID = 0;
                            actualCostFte.CostLineItemID = costLineItem + "1";//Actual Cost   Manasi 30-10-2020
                            ctx.CostFte.Add(actualCostFte);
                            ctx.SaveChanges();

                            etcCostFte = ctx.CostFte.AsNoTracking().FirstOrDefault(a => a.ID == costFte.ID);
                            etcCostFte.FTECostID = act.ActivityID.ToString() + "_" + etcCostId + "_" + textboxIndex;
                            etcCostFte.CostTrackTypeID = 4;
                            etcCostFte.EstimatedCostID = costFte.ID;
                            etcCostFte.FTEHours = "0";
                            etcCostFte.FTECost = "0";
                            etcCostFte.OriginalCost = "0";
                            etcCostFte.FTEHourlyRate = cost.FTEHourlyRate;
                            etcCostFte.RawFTEHourlyRate = cost.RawFTEHourlyRate;
                            etcCostFte.FTEStartDate = DateTime.Parse(etcCostFte.FTEStartDate).ToString(SQL_DATE_FORMAT);
                            etcCostFte.FTEEndDate = DateTime.Parse(etcCostFte.FTEEndDate).ToString(SQL_DATE_FORMAT);
                            etcCostFte.ID = 0;
                            etcCostFte.CostLineItemID = costLineItem + "2";//Actual Cost   Manasi 30-10-2020
                            ctx.CostFte.Add(etcCostFte);
                            ctx.SaveChanges();



                            textboxIndex += 1;
                            diffDate -= 7;
                            eDate = DateTime.Parse(eDate).AddDays(7).ToString(SQL_DATE_FORMAT); ;
                        }
                    }

                    //}
                }


                //If Activity End Date is greater than the cost end date
                var activityEndDate = act.ActivityEndDate;
                var rightDiff = (DateTime.Parse(activityEndDate) - DateTime.Parse(endDate)).TotalDays;
                while (rightDiff > 0)
                {
                    CostFTE costFte = new CostFTE();
                    String fteCostID = act.ActivityID.ToString() + "_" + costId + "_" + textboxIndex;
                    costFte.FTECostID = fteCostID;
                    costFte.Granularity = "week";
                    costFte.ActivityID = act.ActivityID;
                    costFte.FTEStartDate = DateTime.Parse(endDate).AddDays(1).ToString(SQL_DATE_FORMAT);
                    costFte.FTEEndDate = DateTime.Parse(costFte.FTEStartDate).AddDays(6).ToString(SQL_DATE_FORMAT);
                    costFte.FTEPosition = fteList.FirstOrDefault().FTEPosition;
                    costFte.FTEValue = "0";
                    costFte.FTEHourlyRate = fteList.FirstOrDefault().FTEHourlyRate;
                    costFte.RawFTEHourlyRate = fteList.FirstOrDefault().RawFTEHourlyRate;
                    costFte.FTEHours = "0";
                    costFte.FTECost = "0";
                    costFte.OriginalCost = "0";
                    costFte.FTEPositionID = Int32.Parse(fteList.FirstOrDefault().FTEPositionID);
                    costFte.EmployeeID = Int32.Parse(fteList.FirstOrDefault().EmployeeID);
                    costFte.CostTrackTypeID = 2;//--todo
                    costFte.CostLineItemID = costLineItem;
                    costFte.EstimatedCostID = 0;

                    ctx.CostFte.Add(costFte);
                    ctx.SaveChanges();

                    CostFTE actualCostFte = ctx.CostFte.AsNoTracking().FirstOrDefault(a => a.ID == costFte.ID);
                    actualCostFte.FTECostID = act.ActivityID.ToString() + "_" + actualCostId + "_" + textboxIndex;
                    actualCostFte.CostTrackTypeID = 3;
                    actualCostFte.EstimatedCostID = costFte.ID;
                    actualCostFte.FTEHours = "0";
                    actualCostFte.FTECost = "0";
                    actualCostFte.FTEValue = "0";
                    actualCostFte.OriginalCost = "0";
                    actualCostFte.FTEHourlyRate = fteList.FirstOrDefault().FTEHourlyRate;
                    actualCostFte.RawFTEHourlyRate = fteList.FirstOrDefault().RawFTEHourlyRate;
                    actualCostFte.FTEStartDate = DateTime.Parse(actualCostFte.FTEStartDate).ToString(SQL_DATE_FORMAT);
                    actualCostFte.FTEEndDate = DateTime.Parse(actualCostFte.FTEEndDate).ToString(SQL_DATE_FORMAT);
                    actualCostFte.ID = 0;
                    actualCostFte.CostLineItemID = costLineItem + "1";//Actual Cost  Manasi 30-10-2020
                    ctx.CostFte.Add(actualCostFte);
                    ctx.SaveChanges();

                    CostFTE etcCostFte = ctx.CostFte.AsNoTracking().FirstOrDefault(a => a.ID == costFte.ID);
                    etcCostFte.FTECostID = act.ActivityID.ToString() + "_" + etcCostId + "_" + textboxIndex;
                    etcCostFte.CostTrackTypeID = 4;
                    etcCostFte.EstimatedCostID = costFte.ID;
                    etcCostFte.FTEHours = "0";
                    etcCostFte.FTECost = "0";
                    etcCostFte.FTEValue = "0";
                    etcCostFte.OriginalCost = "0";
                    etcCostFte.FTEHourlyRate = fteList.FirstOrDefault().FTEHourlyRate;
                    etcCostFte.RawFTEHourlyRate = fteList.FirstOrDefault().RawFTEHourlyRate;
                    etcCostFte.FTEStartDate = DateTime.Parse(etcCostFte.FTEStartDate).ToString(SQL_DATE_FORMAT);
                    etcCostFte.FTEEndDate = DateTime.Parse(etcCostFte.FTEEndDate).ToString(SQL_DATE_FORMAT);
                    etcCostFte.ID = 0;
                    etcCostFte.CostLineItemID = costLineItem + "2";//Actual Cost   Manasi 30-10-2020
                    ctx.CostFte.Add(etcCostFte);
                    ctx.SaveChanges();


                    textboxIndex += 1;
                    endDate = DateTime.Parse(endDate).AddDays(7).ToString(SQL_DATE_FORMAT);

                    rightDiff -= 7;
                }


                //Scaling
                Scaling.scaling(act.ActivityID, costId, "week", "F");

                lineItemsToScale.Add(actualCostId);
                lineItemsToScale.Add(etcCostId);
                var maxId = Int32.Parse(lineItemsToScale.Max(a => a).ToString());
                costId = maxId + 1;

            }

            return costId;
        }

        public static int mergeLumpsumCostForRollUp(int projectID, Activity act, int lineId)
        {
            var ctx = new CPPDbContext();
            var phase = ctx.PhaseCode.Where(a => a.PhaseID == act.PhaseCode).FirstOrDefault();
            IEnumerable<List<Lumpsum>> lumpsumCost = ctx.Database.SqlQuery<Lumpsum>("call getCostForRollup(@ProjectID, @BudgetCategory, @BudgetSubCategory, @PhaseCode,@CostType,@TrendNumber)",
                                                          new MySql.Data.MySqlClient.MySqlParameter("@ProjectID", projectID),
                                                          new MySql.Data.MySqlClient.MySqlParameter("@BudgetCategory", act.BudgetCategory),
                                                          new MySql.Data.MySqlClient.MySqlParameter("@BudgetSubCategory", act.BudgetSubCategory),
                                                          new MySql.Data.MySqlClient.MySqlParameter("@PhaseCode", phase.ActivityPhaseCode),
                                                          new MySql.Data.MySqlClient.MySqlParameter("@CostType", "L"),
                                                          new MySql.Data.MySqlClient.MySqlParameter("@TrendNumber", act.TrendNumber))
                                                            .GroupBy(a => a.LumpsumDescription).Select(a => a.ToList());
            var costId = lineId;
            var actualCostId = lineId;
            var etcCostId = lineId;
            Project project = ctx.Project.Where(a => a.ProjectID == projectID).FirstOrDefault();
            var projectClass = ctx.ServiceClass.Where(a => a.ID == project.ProjectClassID).FirstOrDefault();
            ProgramElement programElement = ctx.ProgramElement.Include("ProjectClass").Where(a => a.ProgramElementID == project.ProgramElementID).FirstOrDefault();
            var programElementClass = ctx.ProjectClass.Where(a => a.ProjectClassID == programElement.ProjectClassID).FirstOrDefault();  //Manasi 27-10-2020
            ActivityCategory category = ctx.ActivityCategory.Where(a => a.ID == act.BudgetID).FirstOrDefault();

            foreach (List<Lumpsum> lumpsumList in lumpsumCost)
            {
                //Padding left Cost
                var startDate = lumpsumList.FirstOrDefault().LumpsumCostStartDate;
                var activityStart = act.ActivityStartDate;
                var diff = (DateTime.Parse(startDate) - DateTime.Parse(activityStart)).TotalDays;
                actualCostId = costId + 1;
                etcCostId = actualCostId + 1;
                var textboxIndex = 0;  //getNewCostLineItem
                CostLineItemResult lineItem = CostLineItemResult.getCostLineItem(projectClass.Code.ToString(), project.ProjectNumber, project.ProjectElementNumber, act.TrendNumber, phase.ActivityPhaseCode.ToString(), category.CategoryID,
                                                           category.SubCategoryID, "L", null, null, null, null, null, lumpsumList[0].SubcontractorTypeID, lumpsumList[0].SubcontractorID);

                //Manasi 07-11-2020
                CostLineItemResult newLineItem = CostLineItemResult.getNewCostLineItem(projectClass.Code.ToString(), project.ProjectNumber, project.ProjectElementNumber, act.TrendNumber, phase.ActivityPhaseCode.ToString(), category.CategoryID,
                                                           category.SubCategoryID, "L", null, null, null, null, null, lumpsumList[0].SubcontractorTypeID, lumpsumList[0].SubcontractorID, projectID.ToString());

                String LineNumber = "";
                //if (lineItem.LineNumber.ToString().Length <= 1)
                //{
                //    LineNumber = "0" + lineItem.LineNumber;
                //}

                //Manasi 07-11-2020
                if (newLineItem.LineNumber.ToString().Length <= 1)
                {
                    LineNumber = "0" + newLineItem.LineNumber;
                }
                else
                {
                    LineNumber = newLineItem.LineNumber.ToString();
                }
                //string costLineItem = Cost.getLineItem(projectClass.ProjectClassLineItemID.ToString(), programElement.ProjectNumber, project.ProjectElementNumber,
                //                            phase.ActivityPhaseCode.ToString(),
                //                      category.CategoryID, category.SubCategoryID, LineNumber);

                string year = programElement.ProjectStartDate.Split('/')[2].ToString();
                year = year.Substring(2, 2);

                string costLineItem = Cost.getLineItem(projectClass.Code.ToString(), programElement.ProjectNumber, project.ProjectElementNumber,
                                      phase.ActivityPhaseCode.ToString(),
                                category.CategoryID, category.SubCategoryID, LineNumber, year, "4", programElementClass.ProjectClassLineItemID);

                ///TODO -- Handling multiple costs

                //List<TemporaryCost> additionalCost = ctx.Database.SqlQuery<TemporaryCost>("call get_additional_actual_etc_cost(@CostLineItem, @ProjectID, @CostType)",
                //                                      new MySql.Data.MySqlClient.MySqlParameter("@CostLineItem", costLineItem + "." + "02"),
                //                                      new MySql.Data.MySqlClient.MySqlParameter("@ProjectID", projectID),
                //                                      new MySql.Data.MySqlClient.MySqlParameter("@CostType", "L")).ToList();

                //Manasi 07-11-2020
                List<TemporaryCost> additionalCost = ctx.Database.SqlQuery<TemporaryCost>("call get_additional_actual_etc_cost(@CostLineItem, @ProjectID, @CostType)",
                                                      new MySql.Data.MySqlClient.MySqlParameter("@CostLineItem", costLineItem + "2"),
                                                      new MySql.Data.MySqlClient.MySqlParameter("@ProjectID", projectID),
                                                      new MySql.Data.MySqlClient.MySqlParameter("@CostType", "L")).ToList();
                // String dt = DateTime.Parse(activityStart).ToString(SQL_DATE_FORMAT);
                String dt = DateTime.Parse(activityStart).ToString(SQL_DATE_FORMAT);
                List<int> lineItemsToScale = new List<int>();

                while (diff > 0)
                {

                    CostLumpsum costLumpsum = new CostLumpsum();
                    String lumpsumCostID = act.ActivityID.ToString() + "_" + costId + "_" + textboxIndex;
                    costLumpsum.LumpsumCostID = lumpsumCostID;
                    costLumpsum.ActivityID = act.ActivityID;
                    costLumpsum.LumpsumCostStartDate = activityStart;
                    costLumpsum.LumpsumCostEndDate = DateTime.Parse(activityStart).AddDays(6).ToString(SQL_DATE_FORMAT);
                    costLumpsum.LumpsumDescription = lumpsumList.FirstOrDefault().LumpsumDescription;
                    costLumpsum.LumpsumCost = "0";
                    costLumpsum.OriginalCost = "0";
                    costLumpsum.Granularity = "week";
                    costLumpsum.CostTrackTypeID = 2;//--todo
                    costLumpsum.EstimatedCostID = 0;
                    costLumpsum.SubcontractorTypeID = Int32.Parse(lumpsumList.FirstOrDefault().SubcontractorTypeID);
                    costLumpsum.SubcontractorID = Int32.Parse(lumpsumList.FirstOrDefault().SubcontractorID);
                    costLumpsum.CostLineItemID = costLineItem;
                    ctx.CostLumpsum.Add(costLumpsum);
                    ctx.SaveChanges();

                    //Creating actual cost
                    CostLumpsum actualCostLumpsum = ctx.CostLumpsum.AsNoTracking().FirstOrDefault(a => a.ID == costLumpsum.ID);
                    actualCostLumpsum.LumpsumCostID = act.ActivityID.ToString() + "_" + actualCostId + "_" + textboxIndex;
                    actualCostLumpsum.CostTrackTypeID = 3;
                    actualCostLumpsum.EstimatedCostID = costLumpsum.ID;
                    actualCostLumpsum.LumpsumCost = "0";
                    actualCostLumpsum.OriginalCost = "0";
                    actualCostLumpsum.LumpsumCostStartDate = DateTime.Parse(actualCostLumpsum.LumpsumCostStartDate).ToString(SQL_DATE_FORMAT);
                    actualCostLumpsum.LumpsumCostEndDate = DateTime.Parse(actualCostLumpsum.LumpsumCostEndDate).ToString(SQL_DATE_FORMAT);
                    actualCostLumpsum.SubcontractorTypeID = Int32.Parse(lumpsumList.FirstOrDefault().SubcontractorTypeID);
                    actualCostLumpsum.SubcontractorID = Int32.Parse(lumpsumList.FirstOrDefault().SubcontractorID);
                    actualCostLumpsum.ID = 0;
                    actualCostLumpsum.CostLineItemID = costLineItem + "1";   //Manasi 07-11-2020
                    ctx.CostLumpsum.Add(actualCostLumpsum);
                    ctx.SaveChanges();

                    //Creating Estimate-to-completion cost
                    CostLumpsum etcCostLumpsum = ctx.CostLumpsum.AsNoTracking().FirstOrDefault(a => a.ID == costLumpsum.ID);
                    etcCostLumpsum.LumpsumCostID = act.ActivityID.ToString() + "_" + etcCostId + "_" + textboxIndex;
                    etcCostLumpsum.CostTrackTypeID = 4;
                    etcCostLumpsum.EstimatedCostID = costLumpsum.ID;
                    etcCostLumpsum.LumpsumCost = "0";
                    etcCostLumpsum.OriginalCost = "0";
                    etcCostLumpsum.LumpsumCostStartDate = DateTime.Parse(etcCostLumpsum.LumpsumCostStartDate).ToString(SQL_DATE_FORMAT);
                    etcCostLumpsum.LumpsumCostEndDate = DateTime.Parse(etcCostLumpsum.LumpsumCostEndDate).ToString(SQL_DATE_FORMAT);
                    etcCostLumpsum.SubcontractorTypeID = Int32.Parse(lumpsumList.FirstOrDefault().SubcontractorTypeID);
                    etcCostLumpsum.SubcontractorID = Int32.Parse(lumpsumList.FirstOrDefault().SubcontractorID);
                    etcCostLumpsum.ID = 0;
                    etcCostLumpsum.CostLineItemID = costLineItem + "2";   //Manasi 07-11-2020
                    ctx.CostLumpsum.Add(etcCostLumpsum);
                    ctx.SaveChanges();

                    textboxIndex += 1;
                    activityStart = DateTime.Parse(activityStart).AddDays(7).ToString(SQL_DATE_FORMAT); ;
                    diff -= 7;
                }

                //Padding Right Cost
                var endDate = lumpsumList.LastOrDefault().LumpsumCostEndDate;
                var activityStartRight = act.ActivityStartDate;
                var endDiff = (DateTime.Parse(activityStartRight) - DateTime.Parse(endDate)).TotalDays;
                while (endDiff > 0)
                {
                    CostLumpsum costLumpsum = new CostLumpsum();
                    String lumpsumCostID = act.ActivityID.ToString() + "_" + costId + "_" + textboxIndex;
                    costLumpsum.LumpsumCostID = lumpsumCostID;
                    costLumpsum.ActivityID = act.ActivityID;
                    costLumpsum.LumpsumCostStartDate = DateTime.Parse(endDate).AddDays(1).ToString(SQL_DATE_FORMAT);
                    costLumpsum.LumpsumCostEndDate = DateTime.Parse(costLumpsum.LumpsumCostStartDate).AddDays(6).ToString(SQL_DATE_FORMAT);
                    costLumpsum.SubcontractorTypeID = Int32.Parse(lumpsumList.FirstOrDefault().SubcontractorTypeID);
                    costLumpsum.SubcontractorID = Int32.Parse(lumpsumList.FirstOrDefault().SubcontractorID);
                    costLumpsum.LumpsumDescription = lumpsumList.FirstOrDefault().LumpsumDescription;
                    costLumpsum.LumpsumCost = "0";
                    costLumpsum.OriginalCost = "0";
                    costLumpsum.Granularity = "week";
                    costLumpsum.CostTrackTypeID = Int32.Parse("2");//--todo
                    costLumpsum.EstimatedCostID = Int32.Parse("0");
                    costLumpsum.CostLineItemID = costLineItem;
                    ctx.CostLumpsum.Add(costLumpsum);
                    ctx.SaveChanges();

                    //Creating actual cost
                    CostLumpsum actualCostLumpsum = ctx.CostLumpsum.AsNoTracking().FirstOrDefault(a => a.ID == costLumpsum.ID);
                    actualCostLumpsum.LumpsumCostID = act.ActivityID.ToString() + "_" + actualCostId + "_" + textboxIndex;
                    actualCostLumpsum.CostTrackTypeID = 3;
                    actualCostLumpsum.EstimatedCostID = costLumpsum.ID;
                    actualCostLumpsum.LumpsumCost = "0";
                    actualCostLumpsum.OriginalCost = "0";
                    actualCostLumpsum.LumpsumCostStartDate = DateTime.Parse(actualCostLumpsum.LumpsumCostStartDate).ToString(SQL_DATE_FORMAT);
                    actualCostLumpsum.LumpsumCostEndDate = DateTime.Parse(actualCostLumpsum.LumpsumCostEndDate).ToString(SQL_DATE_FORMAT);
                    actualCostLumpsum.SubcontractorTypeID = Int32.Parse(lumpsumList.FirstOrDefault().SubcontractorTypeID);
                    actualCostLumpsum.SubcontractorID = Int32.Parse(lumpsumList.FirstOrDefault().SubcontractorID);
                    actualCostLumpsum.ID = 0;
                    actualCostLumpsum.CostLineItemID = costLineItem + "1";   //Manasi 07-11-2020
                    ctx.CostLumpsum.Add(actualCostLumpsum);
                    ctx.SaveChanges();

                    //Creating Estimate-to-completion cost
                    CostLumpsum etcCostLumpsum = ctx.CostLumpsum.AsNoTracking().FirstOrDefault(a => a.ID == costLumpsum.ID);
                    etcCostLumpsum.LumpsumCostID = act.ActivityID.ToString() + "_" + etcCostId + "_" + textboxIndex;
                    etcCostLumpsum.CostTrackTypeID = 4;
                    etcCostLumpsum.EstimatedCostID = costLumpsum.ID;
                    etcCostLumpsum.LumpsumCost = "0";
                    etcCostLumpsum.OriginalCost = "0";
                    etcCostLumpsum.LumpsumCostStartDate = DateTime.Parse(etcCostLumpsum.LumpsumCostStartDate).ToString(SQL_DATE_FORMAT);
                    etcCostLumpsum.LumpsumCostEndDate = DateTime.Parse(etcCostLumpsum.LumpsumCostEndDate).ToString(SQL_DATE_FORMAT);
                    etcCostLumpsum.SubcontractorTypeID = Int32.Parse(lumpsumList.FirstOrDefault().SubcontractorTypeID);
                    etcCostLumpsum.SubcontractorID = Int32.Parse(lumpsumList.FirstOrDefault().SubcontractorID);
                    etcCostLumpsum.ID = 0;
                    etcCostLumpsum.CostLineItemID = costLineItem + "2";  //Manasi 07-11-2020
                    ctx.CostLumpsum.Add(etcCostLumpsum);
                    ctx.SaveChanges();

                    textboxIndex += 1;
                    activityStart = DateTime.Parse(activityStart).AddDays(6).ToString(SQL_DATE_FORMAT); ;
                    endDiff -= 7;
                }


                for (int i = 0; i < lumpsumList.Count; i++)
                {
                    var lumpsumDate = DateTime.Parse(lumpsumList.ElementAt(i).LumpsumCostStartDate).ToString(SQL_DATE_FORMAT);

                    var cost = lumpsumList.ElementAt(i);
                    CostLumpsum costLumpsum = new CostLumpsum();
                    String lumpsumCostID = act.ActivityID.ToString() + "_" + costId + "_" + textboxIndex;
                    costLumpsum.LumpsumCostID = lumpsumCostID;
                    costLumpsum.ActivityID = act.ActivityID;
                    costLumpsum.LumpsumCostStartDate = DateTime.Parse(cost.LumpsumCostStartDate).ToString(SQL_DATE_FORMAT);
                    costLumpsum.LumpsumCostEndDate = DateTime.Parse(cost.LumpsumCostEndDate).ToString(SQL_DATE_FORMAT);
                    costLumpsum.SubcontractorTypeID = Int32.Parse(lumpsumList.FirstOrDefault().SubcontractorTypeID);
                    costLumpsum.SubcontractorID = Int32.Parse(lumpsumList.FirstOrDefault().SubcontractorID);
                    costLumpsum.LumpsumDescription = cost.LumpsumDescription;
                    costLumpsum.LumpsumCost = cost.LumpsumCost;
                    costLumpsum.OriginalCost = cost.OriginalCost;
                    costLumpsum.Granularity = cost.Granularity;
                    costLumpsum.CostTrackTypeID = 2;//--todo
                    costLumpsum.CostLineItemID = costLineItem;
                    costLumpsum.EstimatedCostID = Int32.Parse(cost.EstimatedCostID);

                    ctx.CostLumpsum.Add(costLumpsum);
                    ctx.SaveChanges();

                    //Creating actual cost
                    CostLumpsum actualCostLumpsum = ctx.CostLumpsum.AsNoTracking().FirstOrDefault(a => a.ID == costLumpsum.ID);
                    actualCostLumpsum.LumpsumCostID = act.ActivityID.ToString() + "_" + actualCostId + "_" + textboxIndex;
                    actualCostLumpsum.CostTrackTypeID = 3;
                    actualCostLumpsum.EstimatedCostID = costLumpsum.ID;
                    actualCostLumpsum.LumpsumCost = "0";
                    actualCostLumpsum.OriginalCost = "0";
                    actualCostLumpsum.LumpsumCostStartDate = DateTime.Parse(actualCostLumpsum.LumpsumCostStartDate).ToString(SQL_DATE_FORMAT);
                    actualCostLumpsum.LumpsumCostEndDate = DateTime.Parse(actualCostLumpsum.LumpsumCostEndDate).ToString(SQL_DATE_FORMAT);
                    actualCostLumpsum.SubcontractorTypeID = Int32.Parse(lumpsumList.FirstOrDefault().SubcontractorTypeID);
                    actualCostLumpsum.SubcontractorID = Int32.Parse(lumpsumList.FirstOrDefault().SubcontractorID);
                    actualCostLumpsum.ID = 0;
                    actualCostLumpsum.CostLineItemID = costLineItem + "1";  //Manasi 07-11-2020
                    ctx.CostLumpsum.Add(actualCostLumpsum);
                    ctx.SaveChanges();

                    //Creating Estimate-to-completion cost
                    CostLumpsum etcCostLumpsum = ctx.CostLumpsum.AsNoTracking().FirstOrDefault(a => a.ID == costLumpsum.ID);
                    etcCostLumpsum.LumpsumCostID = act.ActivityID.ToString() + "_" + etcCostId + "_" + textboxIndex;
                    etcCostLumpsum.CostTrackTypeID = 4;
                    etcCostLumpsum.EstimatedCostID = costLumpsum.ID;
                    etcCostLumpsum.LumpsumCost = "0";
                    etcCostLumpsum.OriginalCost = "0";
                    etcCostLumpsum.LumpsumCostStartDate = DateTime.Parse(etcCostLumpsum.LumpsumCostStartDate).ToString(SQL_DATE_FORMAT);
                    etcCostLumpsum.LumpsumCostEndDate = DateTime.Parse(etcCostLumpsum.LumpsumCostEndDate).ToString(SQL_DATE_FORMAT);
                    etcCostLumpsum.SubcontractorTypeID = Int32.Parse(lumpsumList.FirstOrDefault().SubcontractorTypeID);
                    etcCostLumpsum.SubcontractorID = Int32.Parse(lumpsumList.FirstOrDefault().SubcontractorID);
                    etcCostLumpsum.ID = 0;
                    etcCostLumpsum.CostLineItemID = costLineItem + "2";   //Manasi 07-11-2020
                    ctx.CostLumpsum.Add(etcCostLumpsum);
                    ctx.SaveChanges();

                    //TODO
                    List<TemporaryCost> tempCost = additionalCost.Where(a => (a.CostTrackTypeID == "3") && a.StartDate.Equals(lumpsumDate)).ToList();
                    int additionalActualID = etcCostId;
                    int additionalETCID = additionalActualID;
                    foreach (TemporaryCost costs in tempCost)
                    {
                        additionalActualID += 1;
                        additionalETCID = additionalActualID + 1;
                        actualCostLumpsum = ctx.CostLumpsum.AsNoTracking().FirstOrDefault(a => a.ID == costLumpsum.ID);
                        actualCostLumpsum.LumpsumCostID = act.ActivityID.ToString() + "_" + additionalActualID + "_" + textboxIndex;
                        actualCostLumpsum.CostTrackTypeID = 3;
                        actualCostLumpsum.EstimatedCostID = costLumpsum.ID;
                        actualCostLumpsum.LumpsumCost = "0";
                        actualCostLumpsum.OriginalCost = "0";
                        actualCostLumpsum.LumpsumCostStartDate = DateTime.Parse(actualCostLumpsum.LumpsumCostStartDate).ToString(SQL_DATE_FORMAT);
                        actualCostLumpsum.LumpsumCostEndDate = DateTime.Parse(actualCostLumpsum.LumpsumCostEndDate).ToString(SQL_DATE_FORMAT);
                        actualCostLumpsum.SubcontractorTypeID = Int32.Parse(costs.Type);
                        actualCostLumpsum.SubcontractorID = Int32.Parse(costs.Name);
                        actualCostLumpsum.ID = 0;
                        actualCostLumpsum.CostLineItemID = costLineItem + (additionalActualID - 1).ToString();//Actual Cost  Manasi 07-11-2020
                        ctx.CostLumpsum.Add(actualCostLumpsum);
                        ctx.SaveChanges();


                        etcCostLumpsum = ctx.CostLumpsum.AsNoTracking().FirstOrDefault(a => a.ID == costLumpsum.ID);
                        etcCostLumpsum.LumpsumCostID = act.ActivityID.ToString() + "_" + additionalETCID + "_" + textboxIndex;
                        etcCostLumpsum.CostTrackTypeID = 4;
                        etcCostLumpsum.EstimatedCostID = costLumpsum.ID;
                        etcCostLumpsum.LumpsumCost = "0";
                        etcCostLumpsum.OriginalCost = "0";
                        etcCostLumpsum.LumpsumCostStartDate = DateTime.Parse(etcCostLumpsum.LumpsumCostStartDate).ToString(SQL_DATE_FORMAT);
                        etcCostLumpsum.LumpsumCostEndDate = DateTime.Parse(etcCostLumpsum.LumpsumCostEndDate).ToString(SQL_DATE_FORMAT);
                        etcCostLumpsum.SubcontractorTypeID = Int32.Parse(costs.Type);
                        etcCostLumpsum.SubcontractorID = Int32.Parse(costs.Name);
                        etcCostLumpsum.ID = 0;
                        etcCostLumpsum.CostLineItemID = costLineItem + (additionalETCID - 1).ToString();//etc Cost  Manasi 07-11-2020
                        ctx.CostLumpsum.Add(etcCostLumpsum);
                        ctx.SaveChanges();



                        lineItemsToScale.Add(additionalActualID);
                        lineItemsToScale.Add(additionalETCID);
                    }

                    textboxIndex += 1;

                    //Padding differenc in the middle
                    if (i + 1 != lumpsumList.Count)
                    {
                        var nextCost = lumpsumList.ElementAt(i + 1);
                        var diffDate = (DateTime.Parse(nextCost.LumpsumCostStartDate) - DateTime.Parse(cost.LumpsumCostEndDate)).TotalDays;
                        while (diffDate > 1)
                        {
                            costLumpsum = new CostLumpsum();
                            lumpsumCostID = act.ActivityID.ToString() + "_" + costId + "_" + textboxIndex;
                            var eDate = cost.LumpsumCostEndDate;
                            costLumpsum.LumpsumCostID = lumpsumCostID;
                            costLumpsum.ActivityID = act.ActivityID;
                            costLumpsum.LumpsumCostStartDate = DateTime.Parse(eDate).AddDays(1).ToString(SQL_DATE_FORMAT);
                            costLumpsum.LumpsumCostEndDate = DateTime.Parse(costLumpsum.LumpsumCostStartDate).AddDays(6).ToString(SQL_DATE_FORMAT);
                            costLumpsum.SubcontractorTypeID = Int32.Parse(lumpsumList.FirstOrDefault().SubcontractorTypeID);
                            costLumpsum.SubcontractorID = Int32.Parse(lumpsumList.FirstOrDefault().SubcontractorID);
                            costLumpsum.LumpsumDescription = cost.LumpsumDescription;
                            costLumpsum.LumpsumCost = "0";
                            costLumpsum.OriginalCost = "0";
                            costLumpsum.Granularity = "week";
                            costLumpsum.CostLineItemID = costLineItem;
                            costLumpsum.CostTrackTypeID = Int32.Parse(cost.CostTrackTypeID);//--todo
                            costLumpsum.EstimatedCostID = 0;

                            ctx.CostLumpsum.Add(costLumpsum);
                            ctx.SaveChanges();

                            //Creating actual cost
                            actualCostLumpsum = ctx.CostLumpsum.AsNoTracking().FirstOrDefault(a => a.ID == costLumpsum.ID);
                            actualCostLumpsum.LumpsumCostID = act.ActivityID.ToString() + "_" + actualCostId + "_" + textboxIndex;
                            actualCostLumpsum.CostTrackTypeID = 3;
                            actualCostLumpsum.EstimatedCostID = costLumpsum.ID;
                            actualCostLumpsum.LumpsumCost = "0";
                            actualCostLumpsum.OriginalCost = "0";
                            actualCostLumpsum.LumpsumCostStartDate = DateTime.Parse(actualCostLumpsum.LumpsumCostStartDate).ToString(SQL_DATE_FORMAT);
                            actualCostLumpsum.LumpsumCostEndDate = DateTime.Parse(actualCostLumpsum.LumpsumCostEndDate).ToString(SQL_DATE_FORMAT);
                            actualCostLumpsum.SubcontractorTypeID = Int32.Parse(lumpsumList.FirstOrDefault().SubcontractorTypeID);
                            actualCostLumpsum.SubcontractorID = Int32.Parse(lumpsumList.FirstOrDefault().SubcontractorID);
                            actualCostLumpsum.ID = 0;
                            actualCostLumpsum.CostLineItemID = costLineItem + "1";    //Manasi 07-11-2020
                            ctx.CostLumpsum.Add(actualCostLumpsum);
                            ctx.SaveChanges();
                            //Creating Estimate-to-completion cost
                            etcCostLumpsum = ctx.CostLumpsum.AsNoTracking().FirstOrDefault(a => a.ID == costLumpsum.ID);
                            etcCostLumpsum.LumpsumCostID = act.ActivityID.ToString() + "_" + etcCostId + "_" + textboxIndex;
                            etcCostLumpsum.CostTrackTypeID = 4;
                            etcCostLumpsum.EstimatedCostID = costLumpsum.ID;
                            etcCostLumpsum.LumpsumCost = "0";
                            etcCostLumpsum.OriginalCost = "0";
                            etcCostLumpsum.LumpsumCostStartDate = DateTime.Parse(etcCostLumpsum.LumpsumCostStartDate).ToString(SQL_DATE_FORMAT);
                            etcCostLumpsum.LumpsumCostEndDate = DateTime.Parse(etcCostLumpsum.LumpsumCostEndDate).ToString(SQL_DATE_FORMAT);
                            etcCostLumpsum.SubcontractorTypeID = Int32.Parse(lumpsumList.FirstOrDefault().SubcontractorTypeID);
                            etcCostLumpsum.SubcontractorID = Int32.Parse(lumpsumList.FirstOrDefault().SubcontractorID);
                            etcCostLumpsum.ID = 0;
                            etcCostLumpsum.CostLineItemID = costLineItem + "2";   //Manasi 07-11-2020
                            ctx.CostLumpsum.Add(etcCostLumpsum);
                            ctx.SaveChanges();

                            textboxIndex += 1;
                            diffDate -= 7;
                            eDate = DateTime.Parse(eDate).AddDays(7).ToString(SQL_DATE_FORMAT);
                        }
                    }

                    //}
                }


                var activityEndDate = act.ActivityEndDate;
                var rightDiff = (DateTime.Parse(activityEndDate) - DateTime.Parse(endDate)).TotalDays;
                while (rightDiff > 0)
                {
                    CostLumpsum costLumpsum = new CostLumpsum();
                    String lumpsumCostID = act.ActivityID.ToString() + "_" + costId + "_" + textboxIndex;
                    costLumpsum.LumpsumCostID = lumpsumCostID;
                    costLumpsum.ActivityID = act.ActivityID;
                    costLumpsum.LumpsumCostStartDate = DateTime.Parse(endDate).AddDays(1).ToString(SQL_DATE_FORMAT);
                    costLumpsum.LumpsumCostEndDate = DateTime.Parse(costLumpsum.LumpsumCostStartDate).AddDays(6).ToString(SQL_DATE_FORMAT);
                    costLumpsum.SubcontractorTypeID = Int32.Parse(lumpsumList.FirstOrDefault().SubcontractorTypeID);
                    costLumpsum.SubcontractorID = Int32.Parse(lumpsumList.FirstOrDefault().SubcontractorID);
                    costLumpsum.LumpsumDescription = lumpsumList.FirstOrDefault().LumpsumDescription;
                    costLumpsum.LumpsumCost = "0";
                    costLumpsum.OriginalCost = "0";
                    costLumpsum.CostLineItemID = costLineItem;
                    costLumpsum.Granularity = "week";
                    costLumpsum.CostTrackTypeID = Int32.Parse("2");//--todo
                    costLumpsum.EstimatedCostID = Int32.Parse("0");

                    ctx.CostLumpsum.Add(costLumpsum);
                    ctx.SaveChanges();

                    //Creating actual cost
                    CostLumpsum actualCostLumpsum = ctx.CostLumpsum.AsNoTracking().FirstOrDefault(a => a.ID == costLumpsum.ID);
                    actualCostLumpsum.LumpsumCostID = act.ActivityID.ToString() + "_" + actualCostId + "_" + textboxIndex;
                    actualCostLumpsum.CostTrackTypeID = 3;
                    actualCostLumpsum.EstimatedCostID = costLumpsum.ID;
                    actualCostLumpsum.LumpsumCost = "0";
                    actualCostLumpsum.OriginalCost = "0";
                    actualCostLumpsum.LumpsumCostStartDate = DateTime.Parse(actualCostLumpsum.LumpsumCostStartDate).ToString(SQL_DATE_FORMAT);
                    actualCostLumpsum.LumpsumCostEndDate = DateTime.Parse(actualCostLumpsum.LumpsumCostEndDate).ToString(SQL_DATE_FORMAT);
                    actualCostLumpsum.SubcontractorTypeID = Int32.Parse(lumpsumList.FirstOrDefault().SubcontractorTypeID);
                    actualCostLumpsum.SubcontractorID = Int32.Parse(lumpsumList.FirstOrDefault().SubcontractorID);
                    actualCostLumpsum.ID = 0;
                    actualCostLumpsum.CostLineItemID = costLineItem + "1";   //Manasi 07-11-2020
                    ctx.CostLumpsum.Add(actualCostLumpsum);
                    ctx.SaveChanges();
                    //Creating Estimate-to-completion cost
                    CostLumpsum etcCostLumpsum = ctx.CostLumpsum.AsNoTracking().FirstOrDefault(a => a.ID == costLumpsum.ID);
                    etcCostLumpsum.LumpsumCostID = act.ActivityID.ToString() + "_" + etcCostId + "_" + textboxIndex;
                    etcCostLumpsum.CostTrackTypeID = 4;
                    etcCostLumpsum.EstimatedCostID = costLumpsum.ID;
                    etcCostLumpsum.LumpsumCost = "0";
                    etcCostLumpsum.OriginalCost = "0";
                    etcCostLumpsum.LumpsumCostStartDate = DateTime.Parse(etcCostLumpsum.LumpsumCostStartDate).ToString(SQL_DATE_FORMAT);
                    etcCostLumpsum.LumpsumCostEndDate = DateTime.Parse(etcCostLumpsum.LumpsumCostEndDate).ToString(SQL_DATE_FORMAT);
                    etcCostLumpsum.SubcontractorTypeID = Int32.Parse(lumpsumList.FirstOrDefault().SubcontractorTypeID);
                    etcCostLumpsum.SubcontractorID = Int32.Parse(lumpsumList.FirstOrDefault().SubcontractorID);
                    etcCostLumpsum.ID = 0;
                    etcCostLumpsum.CostLineItemID = costLineItem + "2";    //Manasi 07-11-2020
                    ctx.CostLumpsum.Add(etcCostLumpsum);
                    ctx.SaveChanges();

                    textboxIndex += 1;
                    endDate = DateTime.Parse(endDate).AddDays(7).ToString(SQL_DATE_FORMAT);
                    rightDiff -= 7;
                }


                //3. Unit
                //4. ODC
                //Scaling
                Scaling.scaling(act.ActivityID, costId, "week", "L");
                lineItemsToScale.Add(actualCostId);
                lineItemsToScale.Add(etcCostId);
                var maxId = Int32.Parse(lineItemsToScale.Max(a => a).ToString());
                costId = maxId + 1;

            }

            return costId;
        }

        public static int mergeMaterialCostForRollUp(int projectID, Activity act, int lineId)
        {
            var ctx = new CPPDbContext();
            var phase = ctx.PhaseCode.Where(a => a.PhaseID == act.PhaseCode).FirstOrDefault();

            IEnumerable<List<Unit>> unitCost = ctx.Database.SqlQuery<Unit>("call getCostForRollup(@ProjectID, @BudgetCategory, @BudgetSubCategory, @PhaseCode,@CostType,@TrendNumber)",
                                                          new MySql.Data.MySqlClient.MySqlParameter("@ProjectID", projectID),
                                                          new MySql.Data.MySqlClient.MySqlParameter("@BudgetCategory", act.BudgetCategory),
                                                          new MySql.Data.MySqlClient.MySqlParameter("@BudgetSubCategory", act.BudgetSubCategory),
                                                          new MySql.Data.MySqlClient.MySqlParameter("@PhaseCode", phase.ActivityPhaseCode),
                                                          new MySql.Data.MySqlClient.MySqlParameter("@CostType", "U"),
                                                          new MySql.Data.MySqlClient.MySqlParameter("@TrendNumber", act.TrendNumber))
                                                            .GroupBy(a => new { a.UnitDescription, a.MaterialID }).Select(a => a.ToList());
            var costId = lineId;
            var actualCostId = lineId;
            var etcCostId = lineId;
            var project = ctx.Project.Where(a => a.ProjectID == projectID).FirstOrDefault();
            var projectClass = ctx.ServiceClass.Where(a => a.ID == project.ProjectClassID).FirstOrDefault();
            var programElement = ctx.ProgramElement.Include("ProjectClass").Where(a => a.ProgramElementID == project.ProgramElementID).FirstOrDefault();
            var programElementClass = ctx.ProjectClass.Where(a => a.ProjectClassID == programElement.ProjectClassID).FirstOrDefault();  //Manasi 27-10-2020
            var category = ctx.ActivityCategory.Where(a => a.ID == act.BudgetID).FirstOrDefault();
            foreach (List<Unit> unitList in unitCost)
            {
                //Padding left Cost
                var startDate = unitList.FirstOrDefault().UnitCostStartDate;
                var activityStart = act.ActivityStartDate;
                var diff = (DateTime.Parse(startDate) - DateTime.Parse(activityStart)).TotalDays;
                actualCostId = costId + 1;
                etcCostId = actualCostId + 1;
                var textboxIndex = 0;

                CostLineItemResult lineItem = CostLineItemResult.getCostLineItem(projectClass.Code.ToString(), project.ProjectNumber, project.ProjectElementNumber, act.TrendNumber, phase.ActivityPhaseCode.ToString(), category.CategoryID,
                                                        category.SubCategoryID, "U", null, null, null, unitList[0].MaterialCategoryID, unitList[0].MaterialID, null, null);

                //Manasi 07-11-2020
                CostLineItemResult newLineItem = CostLineItemResult.getNewCostLineItem(projectClass.Code.ToString(), project.ProjectNumber, project.ProjectElementNumber, act.TrendNumber, phase.ActivityPhaseCode.ToString(), category.CategoryID,
                                                        category.SubCategoryID, "U", null, null, null, unitList[0].MaterialCategoryID, unitList[0].MaterialID, null, null, projectID.ToString());

                String LineNumber = "";

                if (lineItem.LineNumber.ToString().Length <= 1)
                {
                    LineNumber = "0" + lineItem.LineNumber;
                }

                //Manasi 07-11-2020
                if (newLineItem.LineNumber.ToString().Length <= 1)
                {
                    LineNumber = "0" + newLineItem.LineNumber;
                }
                else
                {
                    LineNumber = newLineItem.LineNumber.ToString();
                }
                //string costLineItem = Cost.getLineItem(projectClass.ProjectClassLineItemID.ToString(), programElement.ProjectNumber, project.ProjectElementNumber,
                //                              phase.ActivityPhaseCode.ToString(),
                //                        category.CategoryID, category.SubCategoryID, LineNumber);

                //----------Manasi 26-10-2020-------------------------------------------
                string year = programElement.ProjectStartDate.Split('/')[2].ToString();
                year = year.Substring(2, 2);

                string costLineItem = Cost.getLineItem(projectClass.Code.ToString(), programElement.ProjectNumber, project.ProjectElementNumber,
                                              phase.ActivityPhaseCode.ToString(),
                                        category.CategoryID, category.SubCategoryID, LineNumber, year, "3", programElementClass.ProjectClassLineItemID);
                //-------------------------------------------------------------------------

                ///TODO -- Handling multiple costs

                //List<TemporaryCost> additionalCost = ctx.Database.SqlQuery<TemporaryCost>("call get_additional_actual_etc_cost(@CostLineItem, @ProjectID, @CostType)",
                //                                      new MySql.Data.MySqlClient.MySqlParameter("@CostLineItem", costLineItem + "." + "02"),
                //                                      new MySql.Data.MySqlClient.MySqlParameter("@ProjectID", projectID),
                //                                      new MySql.Data.MySqlClient.MySqlParameter("@CostType", "U")).ToList();

                //Manasi 07-11-2020
                List<TemporaryCost> additionalCost = ctx.Database.SqlQuery<TemporaryCost>("call get_additional_actual_etc_cost(@CostLineItem, @ProjectID, @CostType)",
                                                      new MySql.Data.MySqlClient.MySqlParameter("@CostLineItem", costLineItem + "2"),
                                                      new MySql.Data.MySqlClient.MySqlParameter("@ProjectID", projectID),
                                                      new MySql.Data.MySqlClient.MySqlParameter("@CostType", "U")).ToList();

                // String dt = DateTime.Parse(activityStart).ToString(SQL_DATE_FORMAT);
                String dt = DateTime.Parse(activityStart).ToString(SQL_DATE_FORMAT);
                List<int> lineItemsToScale = new List<int>();

                while (diff > 0)
                {

                    CostUnit costUnit = new CostUnit();
                    String unitCostID = act.ActivityID.ToString() + "_" + costId + "_" + textboxIndex;
                    costUnit.UnitCostID = unitCostID;
                    costUnit.Granularity = "week";
                    costUnit.ActivityID = act.ActivityID;
                    costUnit.UnitCostStartDate = activityStart;
                    costUnit.UnitCostEndDate = DateTime.Parse(activityStart).AddDays(6).ToString(SQL_DATE_FORMAT);
                    costUnit.UnitDescription = unitList.ElementAt(0).UnitDescription;
                    costUnit.UnitQuantity = "0";
                    costUnit.UnitPrice = unitList.ElementAt(0).UnitPrice;
                    costUnit.RawUnitPrice = unitList.ElementAt(0).RawUnitPrice;
                    costUnit.UnitCost = "0";
                    costUnit.OriginalCost = "0";
                    costUnit.CostLineItemID = costLineItem;
                    costUnit.UnitType = unitList.ElementAt(0).UnitType;
                    costUnit.UnitType_ID = Int32.Parse(unitList.ElementAt(0).UnitType_ID);
                    costUnit.MaterialID = Int32.Parse(unitList.ElementAt(0).MaterialID);
                    costUnit.MaterialCategoryID = Int32.Parse(unitList.ElementAt(0).MaterialCategoryID);
                    costUnit.CostTrackTypeID = 2;//--todo
                    costUnit.EstimatedCostID = Int32.Parse("0");

                    ctx.CostUnit.Add(costUnit);
                    ctx.SaveChanges();


                    CostUnit actualCostUnit = ctx.CostUnit.AsNoTracking().FirstOrDefault(a => a.ID == costUnit.ID);
                    actualCostUnit.UnitCostID = act.ActivityID.ToString() + "_" + actualCostId + "_" + textboxIndex;
                    actualCostUnit.CostTrackTypeID = 3;
                    actualCostUnit.EstimatedCostID = costUnit.ID;
                    actualCostUnit.UnitQuantity = "0";
                    actualCostUnit.UnitCost = "0";
                    actualCostUnit.UnitPrice = unitList.ElementAt(0).UnitPrice;
                    actualCostUnit.RawUnitPrice = unitList.ElementAt(0).RawUnitPrice;
                    actualCostUnit.OriginalCost = "0";
                    actualCostUnit.ID = 0;
                    //actualCostUnit.CostLineItemID = costLineItem + "." + "01";
                    actualCostUnit.CostLineItemID = costLineItem + "1";   //Manasi 07-11-2020
                    ctx.CostUnit.Add(actualCostUnit);
                    ctx.SaveChanges();

                    CostUnit etcCostUnit = ctx.CostUnit.AsNoTracking().FirstOrDefault(a => a.ID == costUnit.ID);
                    etcCostUnit.UnitCostID = act.ActivityID.ToString() + "_" + etcCostId + "_" + textboxIndex;
                    etcCostUnit.CostTrackTypeID = 4;
                    etcCostUnit.EstimatedCostID = costUnit.ID;
                    etcCostUnit.UnitQuantity = "0";
                    etcCostUnit.UnitCost = "0";
                    etcCostUnit.OriginalCost = "0";
                    actualCostUnit.UnitPrice = unitList.ElementAt(0).UnitPrice;
                    actualCostUnit.RawUnitPrice = unitList.ElementAt(0).RawUnitPrice;
                    etcCostUnit.ID = 0;
                    //etcCostUnit.CostLineItemID = costLineItem + "." + "02";
                    etcCostUnit.CostLineItemID = costLineItem + "2";   //Manasi 07-11-2020
                    ctx.CostUnit.Add(etcCostUnit);
                    ctx.SaveChanges();

                    textboxIndex += 1;
                    activityStart = DateTime.Parse(activityStart).AddDays(7).ToString(SQL_DATE_FORMAT); ;
                    diff -= 7;
                }

                //Padding Right Cost
                var endDate = unitList.LastOrDefault().UnitCostEndDate;
                var activityStartRight = act.ActivityStartDate;
                var endDiff = (DateTime.Parse(activityStartRight) - DateTime.Parse(endDate)).TotalDays;
                while (endDiff > 0)
                {
                    CostUnit costUnit = new CostUnit();
                    String unitCostID = act.ActivityID.ToString() + "_" + costId + "_" + textboxIndex;
                    costUnit.UnitCostID = unitCostID;
                    costUnit.Granularity = "week";
                    costUnit.ActivityID = act.ActivityID;
                    costUnit.UnitCostStartDate = DateTime.Parse(endDate).AddDays(1).ToString(SQL_DATE_FORMAT);
                    costUnit.UnitCostEndDate = DateTime.Parse(costUnit.UnitCostStartDate).AddDays(6).ToString(SQL_DATE_FORMAT);

                    costUnit.UnitDescription = unitList.ElementAt(0).UnitDescription;
                    costUnit.UnitQuantity = "0";
                    costUnit.UnitPrice = unitList.ElementAt(0).UnitPrice;
                    costUnit.UnitPrice = unitList.ElementAt(0).RawUnitPrice;
                    costUnit.UnitCost = "0";
                    costUnit.OriginalCost = "0";
                    costUnit.UnitType = unitList.ElementAt(0).UnitType;
                    costUnit.UnitType_ID = Int32.Parse(unitList.ElementAt(0).UnitType_ID);
                    costUnit.MaterialID = Int32.Parse(unitList.ElementAt(0).MaterialID);
                    costUnit.MaterialCategoryID = Int32.Parse(unitList.ElementAt(0).MaterialCategoryID);
                    costUnit.CostTrackTypeID = 2;//--todo
                    costUnit.EstimatedCostID = Int32.Parse("0");
                    costUnit.CostLineItemID = costLineItem;


                    ctx.CostUnit.Add(costUnit);
                    ctx.SaveChanges();


                    CostUnit actualCostUnit = ctx.CostUnit.AsNoTracking().FirstOrDefault(a => a.ID == costUnit.ID);
                    actualCostUnit.UnitCostID = act.ActivityID.ToString() + "_" + actualCostId + "_" + textboxIndex;
                    actualCostUnit.CostTrackTypeID = 3;
                    actualCostUnit.EstimatedCostID = costUnit.ID;
                    actualCostUnit.UnitQuantity = "0";
                    actualCostUnit.UnitCost = "0";
                    actualCostUnit.OriginalCost = "0";
                    actualCostUnit.UnitPrice = unitList.ElementAt(0).UnitPrice;
                    actualCostUnit.RawUnitPrice = unitList.ElementAt(0).RawUnitPrice;
                    actualCostUnit.ID = 0;
                    //actualCostUnit.CostLineItemID = costLineItem + "." + "01";
                    actualCostUnit.CostLineItemID = costLineItem + "1";     //Manasi 07-11-2020
                    ctx.CostUnit.Add(actualCostUnit);
                    ctx.SaveChanges();

                    CostUnit etcCostUnit = ctx.CostUnit.AsNoTracking().FirstOrDefault(a => a.ID == costUnit.ID);
                    etcCostUnit.UnitCostID = act.ActivityID.ToString() + "_" + etcCostId + "_" + textboxIndex;
                    etcCostUnit.CostTrackTypeID = 4;
                    etcCostUnit.EstimatedCostID = costUnit.ID;
                    etcCostUnit.UnitQuantity = "0";
                    etcCostUnit.UnitCost = "0";
                    etcCostUnit.OriginalCost = "0";
                    etcCostUnit.UnitPrice = unitList.ElementAt(0).UnitPrice;
                    etcCostUnit.RawUnitPrice = unitList.ElementAt(0).RawUnitPrice;
                    etcCostUnit.ID = 0;
                    //etcCostUnit.CostLineItemID = costLineItem + "." + "02";
                    etcCostUnit.CostLineItemID = costLineItem + "2";    //Manasi 07-11-2020
                    ctx.CostUnit.Add(etcCostUnit);
                    ctx.SaveChanges();

                    textboxIndex += 1;
                    activityStart = DateTime.Parse(activityStart).AddDays(6).ToString(SQL_DATE_FORMAT); ;
                    endDiff -= 7;
                }


                for (int i = 0; i < unitList.Count; i++)
                {
                    var unitDate = DateTime.Parse(unitList.ElementAt(i).UnitCostStartDate).ToString(SQL_DATE_FORMAT);

                    var cost = unitList.ElementAt(i);
                    CostUnit costUnit = new CostUnit();
                    String unitCostID = act.ActivityID.ToString() + "_" + costId + "_" + textboxIndex;
                    costUnit.UnitCostID = unitCostID;
                    costUnit.Granularity = cost.Granularity;
                    costUnit.ActivityID = act.ActivityID;
                    costUnit.UnitCostStartDate = DateTime.Parse(cost.UnitCostStartDate).ToString(SQL_DATE_FORMAT);
                    costUnit.UnitCostEndDate = DateTime.Parse(cost.UnitCostEndDate).ToString(SQL_DATE_FORMAT);

                    costUnit.UnitDescription = cost.UnitDescription;
                    costUnit.UnitQuantity = cost.UnitQuantity;
                    costUnit.UnitPrice = unitList.ElementAt(0).UnitPrice;
                    costUnit.RawUnitPrice = unitList.ElementAt(0).RawUnitPrice;
                    costUnit.UnitCost = cost.UnitCost;
                    costUnit.OriginalCost = cost.OriginalCost;
                    costUnit.UnitType = cost.UnitType;
                    costUnit.UnitType_ID = Int32.Parse(cost.UnitType_ID);

                    costUnit.CostTrackTypeID = 2;//--todo
                    costUnit.EstimatedCostID = Int32.Parse(cost.EstimatedCostID);
                    costUnit.MaterialID = Int32.Parse(cost.MaterialID);
                    costUnit.MaterialCategoryID = Int32.Parse(unitList.ElementAt(0).MaterialCategoryID);
                    costUnit.CostLineItemID = costLineItem;
                    ctx.CostUnit.Add(costUnit);
                    ctx.SaveChanges();

                    CostUnit actualCostUnit = ctx.CostUnit.AsNoTracking().FirstOrDefault(a => a.ID == costUnit.ID);
                    actualCostUnit.UnitCostID = act.ActivityID.ToString() + "_" + actualCostId + "_" + textboxIndex;
                    actualCostUnit.CostTrackTypeID = 3;
                    actualCostUnit.EstimatedCostID = costUnit.ID;
                    actualCostUnit.UnitQuantity = "0";
                    actualCostUnit.UnitCost = "0";
                    actualCostUnit.OriginalCost = "0";
                    actualCostUnit.ID = 0;
                    actualCostUnit.UnitPrice = unitList.ElementAt(0).UnitPrice;
                    actualCostUnit.RawUnitPrice = unitList.ElementAt(0).RawUnitPrice;
                    //actualCostUnit.CostLineItemID = costLineItem + "." + "01";
                    actualCostUnit.CostLineItemID = costLineItem + "1";    //Manasi 07-11-2020
                    ctx.CostUnit.Add(actualCostUnit);
                    ctx.SaveChanges();

                    CostUnit etcCostUnit = ctx.CostUnit.AsNoTracking().FirstOrDefault(a => a.ID == costUnit.ID);
                    etcCostUnit.UnitCostID = act.ActivityID.ToString() + "_" + etcCostId + "_" + textboxIndex;
                    etcCostUnit.CostTrackTypeID = 4;
                    etcCostUnit.EstimatedCostID = costUnit.ID;
                    etcCostUnit.UnitQuantity = "0";
                    etcCostUnit.OriginalCost = "0";
                    etcCostUnit.UnitCost = "0";
                    etcCostUnit.UnitPrice = unitList.ElementAt(0).UnitPrice;
                    etcCostUnit.RawUnitPrice = unitList.ElementAt(0).RawUnitPrice;
                    etcCostUnit.ID = 0;
                    //etcCostUnit.CostLineItemID = costLineItem + "." + "02";
                    etcCostUnit.CostLineItemID = costLineItem + "2";    //Manasi 07-11-2020
                    ctx.CostUnit.Add(etcCostUnit);
                    ctx.SaveChanges();

                    //TODO
                    List<TemporaryCost> tempCost = additionalCost.Where(a => (a.CostTrackTypeID == "3") && a.StartDate.Equals(unitDate)).ToList();
                    int additionalActualID = etcCostId;
                    int additionalETCID = additionalActualID;
                    foreach (TemporaryCost costs in tempCost)
                    {
                        additionalActualID += 1;
                        additionalETCID = additionalActualID + 1;
                        actualCostUnit = ctx.CostUnit.AsNoTracking().FirstOrDefault(a => a.ID == costUnit.ID);
                        actualCostUnit.UnitCostID = act.ActivityID.ToString() + "_" + additionalActualID + "_" + textboxIndex;
                        actualCostUnit.CostTrackTypeID = 3;
                        actualCostUnit.EstimatedCostID = costUnit.ID;
                        actualCostUnit.UnitPrice = unitList.ElementAt(0).UnitPrice;
                        actualCostUnit.RawUnitPrice = unitList.ElementAt(0).RawUnitPrice;
                        actualCostUnit.UnitQuantity = "0";
                        actualCostUnit.UnitCost = "0";
                        actualCostUnit.OriginalCost = "0";
                        actualCostUnit.ID = 0;
                        //actualCostUnit.CostLineItemID = costLineItem + "." + ((additionalActualID.ToString().Length == 1) ? "0" + (additionalActualID - 1).ToString() : (additionalActualID - 1).ToString());//Actual Cost
                        actualCostUnit.CostLineItemID = costLineItem + (additionalActualID - 1).ToString();//Actual Cost   Manasi 07-11-2020

                        ctx.CostUnit.Add(actualCostUnit);
                        ctx.SaveChanges();



                        etcCostUnit = ctx.CostUnit.AsNoTracking().FirstOrDefault(a => a.ID == costUnit.ID);
                        etcCostUnit = ctx.CostUnit.AsNoTracking().FirstOrDefault(a => a.ID == costUnit.ID);
                        etcCostUnit.UnitCostID = act.ActivityID.ToString() + "_" + additionalActualID + "_" + textboxIndex;
                        etcCostUnit.CostTrackTypeID = 3;
                        etcCostUnit.EstimatedCostID = costUnit.ID;
                        etcCostUnit.UnitPrice = unitList.ElementAt(0).UnitPrice;
                        etcCostUnit.RawUnitPrice = unitList.ElementAt(0).RawUnitPrice;
                        etcCostUnit.UnitQuantity = "0";
                        etcCostUnit.UnitCost = "0";
                        etcCostUnit.OriginalCost = "0";
                        etcCostUnit.ID = 0;
                        //etcCostUnit.CostLineItemID = costLineItem + "." + ((additionalETCID.ToString().Length == 1) ? "0" + (additionalETCID - 1).ToString() : (additionalETCID - 1).ToString());//ETC Cost
                        etcCostUnit.CostLineItemID = costLineItem + (additionalETCID - 1).ToString();//ETC Cost  Manasi 07-11-2020

                        ctx.CostUnit.Add(etcCostUnit);
                        ctx.SaveChanges();



                        lineItemsToScale.Add(additionalActualID);
                        lineItemsToScale.Add(additionalETCID);
                    }


                    textboxIndex += 1;

                    //Padding differenc in the middle
                    if (i + 1 != unitList.Count)
                    {
                        var nextCost = unitList.ElementAt(i + 1);
                        var diffDate = (DateTime.Parse(nextCost.UnitCostStartDate) - DateTime.Parse(cost.UnitCostEndDate)).TotalDays;
                        var eDate = cost.UnitCostEndDate;
                        while (diffDate > 1)
                        {
                            costUnit = new CostUnit();
                            unitCostID = act.ActivityID.ToString() + "_" + costId + "_" + textboxIndex;
                            costUnit.UnitCostID = unitCostID;
                            costUnit.ActivityID = act.ActivityID;
                            costUnit.Granularity = "week";
                            costUnit.UnitCostStartDate = DateTime.Parse(eDate).AddDays(1).ToString(SQL_DATE_FORMAT);
                            costUnit.UnitCostEndDate = DateTime.Parse(costUnit.UnitCostStartDate).AddDays(6).ToString(SQL_DATE_FORMAT);

                            costUnit.UnitDescription = cost.UnitDescription;
                            costUnit.UnitQuantity = "0";
                            costUnit.UnitPrice = cost.UnitPrice;
                            costUnit.RawUnitPrice = cost.RawUnitPrice;
                            costUnit.UnitCost = "0";
                            costUnit.OriginalCost = "0";
                            costUnit.UnitType = cost.UnitType;
                            costUnit.CostLineItemID = costLineItem;
                            costUnit.UnitType_ID = Int32.Parse(cost.UnitType_ID);
                            costUnit.CostTrackTypeID = Int32.Parse(cost.CostTrackTypeID);//--todo
                            costUnit.EstimatedCostID = Int32.Parse(cost.EstimatedCostID);
                            costUnit.MaterialID = Int32.Parse(cost.MaterialID);
                            costUnit.MaterialCategoryID = Int32.Parse(unitList.ElementAt(0).MaterialCategoryID);

                            ctx.CostUnit.Add(costUnit);
                            ctx.SaveChanges();


                            actualCostUnit = ctx.CostUnit.AsNoTracking().FirstOrDefault(a => a.ID == costUnit.ID);
                            actualCostUnit.UnitCostID = act.ActivityID.ToString() + "_" + actualCostId + "_" + textboxIndex;
                            actualCostUnit.CostTrackTypeID = 3;
                            actualCostUnit.EstimatedCostID = costUnit.ID;
                            actualCostUnit.UnitQuantity = "0";
                            actualCostUnit.UnitCost = "0";
                            actualCostUnit.OriginalCost = "0";
                            actualCostUnit.UnitPrice = cost.UnitPrice;
                            actualCostUnit.RawUnitPrice = cost.RawUnitPrice;
                            actualCostUnit.ID = 0;
                            //actualCostUnit.CostLineItemID = costLineItem + "." + "01";
                            actualCostUnit.CostLineItemID = costLineItem + "1";  //Manasi 07-11-2020
                            ctx.CostUnit.Add(actualCostUnit);
                            ctx.SaveChanges();

                            etcCostUnit = ctx.CostUnit.AsNoTracking().FirstOrDefault(a => a.ID == costUnit.ID);
                            etcCostUnit.UnitCostID = act.ActivityID.ToString() + "_" + etcCostId + "_" + textboxIndex;
                            etcCostUnit.CostTrackTypeID = 4;
                            etcCostUnit.EstimatedCostID = costUnit.ID;
                            etcCostUnit.UnitQuantity = "0";
                            etcCostUnit.UnitCost = "0";
                            etcCostUnit.OriginalCost = "0";
                            etcCostUnit.UnitPrice = cost.UnitPrice;
                            etcCostUnit.RawUnitPrice = cost.RawUnitPrice;
                            etcCostUnit.ID = 0;
                            //etcCostUnit.CostLineItemID = costLineItem + "." + "02";
                            etcCostUnit.CostLineItemID = costLineItem + "2";   //Manasi 07-11-2020
                            ctx.CostUnit.Add(etcCostUnit);
                            ctx.SaveChanges();

                            textboxIndex += 1;
                            diffDate -= 7;
                            eDate = DateTime.Parse(eDate).AddDays(7).ToString(SQL_DATE_FORMAT); ;
                        }
                    }

                    //}
                }

                var activityEndDate = act.ActivityEndDate;
                var rightDiff = (DateTime.Parse(activityEndDate) - DateTime.Parse(endDate)).TotalDays;
                while (rightDiff > 0)
                {
                    CostUnit costUnit = new CostUnit();
                    String unitCostID = act.ActivityID.ToString() + "_" + costId + "_" + textboxIndex;
                    costUnit.UnitCostID = unitCostID;
                    costUnit.Granularity = "week";
                    costUnit.ActivityID = act.ActivityID;
                    costUnit.UnitCostStartDate = DateTime.Parse(endDate).AddDays(1).ToString(SQL_DATE_FORMAT);
                    costUnit.UnitCostEndDate = DateTime.Parse(costUnit.UnitCostStartDate).AddDays(6).ToString(SQL_DATE_FORMAT);

                    costUnit.UnitDescription = unitList.ElementAt(0).UnitDescription;
                    costUnit.UnitQuantity = "0";
                    costUnit.UnitPrice = unitList.ElementAt(0).UnitPrice;
                    costUnit.RawUnitPrice = unitList.ElementAt(0).RawUnitPrice;
                    costUnit.UnitCost = "0";
                    costUnit.OriginalCost = "0";
                    costUnit.UnitType = unitList.ElementAt(0).UnitType;
                    costUnit.UnitType_ID = Int32.Parse(unitList.ElementAt(0).UnitType_ID);
                    costUnit.MaterialID = Int32.Parse(unitList.ElementAt(0).MaterialID);
                    costUnit.MaterialCategoryID = Int32.Parse(unitList.ElementAt(0).MaterialCategoryID);
                    costUnit.CostTrackTypeID = 2;//--todo
                    costUnit.CostLineItemID = costLineItem;
                    costUnit.EstimatedCostID = Int32.Parse("0");


                    ctx.CostUnit.Add(costUnit);
                    ctx.SaveChanges();


                    CostUnit actualCostUnit = ctx.CostUnit.AsNoTracking().FirstOrDefault(a => a.ID == costUnit.ID);
                    actualCostUnit.UnitCostID = act.ActivityID.ToString() + "_" + actualCostId + "_" + textboxIndex;
                    actualCostUnit.CostTrackTypeID = 3;
                    actualCostUnit.EstimatedCostID = costUnit.ID;
                    actualCostUnit.UnitPrice = unitList.ElementAt(0).UnitPrice;
                    actualCostUnit.RawUnitPrice = unitList.ElementAt(0).RawUnitPrice;
                    actualCostUnit.UnitQuantity = "0";
                    actualCostUnit.UnitCost = "0";
                    actualCostUnit.OriginalCost = "0";
                    actualCostUnit.ID = 0;
                    //actualCostUnit.CostLineItemID = costLineItem + "." + "01";
                    actualCostUnit.CostLineItemID = costLineItem + "1";   //Manasi 07-11-2020
                    ctx.CostUnit.Add(actualCostUnit);
                    ctx.SaveChanges();

                    CostUnit etcCostUnit = ctx.CostUnit.AsNoTracking().FirstOrDefault(a => a.ID == costUnit.ID);
                    etcCostUnit.UnitCostID = act.ActivityID.ToString() + "_" + etcCostId + "_" + textboxIndex;
                    etcCostUnit.CostTrackTypeID = 4;
                    etcCostUnit.EstimatedCostID = costUnit.ID;
                    etcCostUnit.UnitPrice = unitList.ElementAt(0).UnitPrice;
                    etcCostUnit.RawUnitPrice = unitList.ElementAt(0).RawUnitPrice;
                    etcCostUnit.UnitQuantity = "0";
                    etcCostUnit.UnitCost = "0";
                    etcCostUnit.OriginalCost = "0";
                    etcCostUnit.ID = 0;
                    //etcCostUnit.CostLineItemID = costLineItem + "." + "02";
                    etcCostUnit.CostLineItemID = costLineItem + "2";   //Manasi 07-11-2020
                    ctx.CostUnit.Add(etcCostUnit);
                    ctx.SaveChanges();

                    textboxIndex += 1;
                    endDate = DateTime.Parse(endDate).AddDays(7).ToString(SQL_DATE_FORMAT);
                    rightDiff -= 7;
                }

                //3. Unit
                //4. ODC
                //Scaling
                Scaling.scaling(act.ActivityID, costId, "week", "U");
                //Scaling.scaling(act.ActivityID, actualCostId, "week", "F");
                //Scaling.scaling(act.ActivityID, etcCostId, "week", "F");
                ////Scaling additional fte cost
                //lineItemsToScale = lineItemsToScale.Distinct().ToList();
                //foreach (int id in lineItemsToScale)
                //{
                //    Scaling.scaling(act.ActivityID, id, "week", "F");
                //}
                lineItemsToScale.Add(actualCostId);
                lineItemsToScale.Add(etcCostId);
                var maxId = Int32.Parse(lineItemsToScale.Max(a => a).ToString());
                costId = maxId + 1;

            }

            return costId;
        }

        public static int mergeODCCostForRollUp(int projectID, Activity act, int lineId)
        {

            var ctx = new CPPDbContext();
            var costId = lineId;
            var actualCostId = lineId;
            var etcCostId = lineId;
            try
            {
                var phase = ctx.PhaseCode.Where(a => a.PhaseID == act.PhaseCode).FirstOrDefault(); //phasecode is fk

                IEnumerable<List<ODC>> odcCost = ctx.Database.SqlQuery<ODC>("call getCostForRollup(@ProjectID, @BudgetCategory, @BudgetSubCategory, @PhaseCode,@CostType,@TrendNumber)",
                                                              new MySql.Data.MySqlClient.MySqlParameter("@ProjectID", projectID),
                                                              new MySql.Data.MySqlClient.MySqlParameter("@BudgetCategory", act.BudgetCategory),
                                                              new MySql.Data.MySqlClient.MySqlParameter("@BudgetSubCategory", act.BudgetSubCategory),
                                                              new MySql.Data.MySqlClient.MySqlParameter("@PhaseCode", phase.ActivityPhaseCode),
                                                              new MySql.Data.MySqlClient.MySqlParameter("@CostType", "ODC"),
                                                              new MySql.Data.MySqlClient.MySqlParameter("@TrendNumber", act.TrendNumber))
                                                                .GroupBy(a => a.ODCTypeID).Select(a => a.ToList());

                Project project = ctx.Project.Where(a => a.ProjectID == projectID).FirstOrDefault();
                var projectClass = ctx.ServiceClass.Where(a => a.ID == project.ProjectClassID).FirstOrDefault();
                ProgramElement programElement = ctx.ProgramElement.Include("ProjectClass").Where(a => a.ProgramElementID == project.ProgramElementID).FirstOrDefault();
                var programElementClass = ctx.ProjectClass.Where(a => a.ProjectClassID == programElement.ProjectClassID).FirstOrDefault();  //Manasi 27-10-2020
                ActivityCategory category = ctx.ActivityCategory.Where(a => a.ID == act.BudgetID).FirstOrDefault();
                foreach (List<ODC> odcList in odcCost)
                {
                    //Padding left Cost
                    var startDate = odcList.FirstOrDefault().ODCStartDate;
                    var activityStart = act.ActivityStartDate;
                    var diff = (DateTime.Parse(startDate) - DateTime.Parse(activityStart)).TotalDays;
                    actualCostId = costId + 1;
                    etcCostId = actualCostId + 1;
                    var textboxIndex = 0;
                    CostLineItemResult lineItem = CostLineItemResult.getCostLineItem(projectClass.Code.ToString(), project.ProjectNumber, project.ProjectElementNumber, act.TrendNumber, phase.ActivityPhaseCode.ToString(), category.CategoryID,
                                                             category.SubCategoryID, "ODC", null, null, odcList[0].ODCTypeID, null, null, null, null);

                    //Manasi 07-11-2020
                    CostLineItemResult newLineItem = CostLineItemResult.getNewCostLineItem(projectClass.Code.ToString(), project.ProjectNumber, project.ProjectElementNumber, act.TrendNumber, phase.ActivityPhaseCode.ToString(), category.CategoryID,
                                                             category.SubCategoryID, "ODC", null, null, odcList[0].ODCTypeID, null, null, null, null, projectID.ToString());

                    String LineNumber = "";
                    if (lineItem.LineNumber.ToString().Length <= 1)
                    {
                        LineNumber = "0" + lineItem.LineNumber;
                    }

                    //Manasi 07-11-2020
                    if (newLineItem.LineNumber.ToString().Length <= 1)
                    {
                        LineNumber = "0" + newLineItem.LineNumber;
                    }
                    else
                    {
                        LineNumber = newLineItem.LineNumber.ToString();
                    }
                    //string costLineItem = Cost.getLineItem(projectClass.ProjectClassLineItemID.ToString(), programElement.ProjectNumber, project.ProjectElementNumber,
                    //                         phase.ActivityPhaseCode.ToString(),
                    //                   category.CategoryID, category.SubCategoryID, LineNumber);


                    //----------Manasi 26-10-2020-------------------------------------------
                    string year = programElement.ProjectStartDate.Split('/')[2].ToString();
                    year = year.Substring(2, 2);

                    string costLineItem = Cost.getLineItem(projectClass.Code.ToString(), programElement.ProjectNumber, project.ProjectElementNumber,
                                             phase.ActivityPhaseCode.ToString(),
                                       category.CategoryID, category.SubCategoryID, LineNumber, year, "2", programElementClass.ProjectClassLineItemID);
                    //-------------------------------------------------------------------------
                    ///TODO -- Handling multiple costs

                    //List<TemporaryCost> additionalCost = ctx.Database.SqlQuery<TemporaryCost>("call get_additional_actual_etc_cost(@CostLineItem, @ProjectID, @CostType)",
                    //                                      new MySql.Data.MySqlClient.MySqlParameter("@CostLineItem", costLineItem + "." + "02"),
                    //                                      new MySql.Data.MySqlClient.MySqlParameter("@ProjectID", projectID),
                    //                                      new MySql.Data.MySqlClient.MySqlParameter("@CostType", "ODC")).ToList();

                    List<TemporaryCost> additionalCost = ctx.Database.SqlQuery<TemporaryCost>("call get_additional_actual_etc_cost(@CostLineItem, @ProjectID, @CostType)",
                                                          new MySql.Data.MySqlClient.MySqlParameter("@CostLineItem", costLineItem + "2"),
                                                          new MySql.Data.MySqlClient.MySqlParameter("@ProjectID", projectID),
                                                          new MySql.Data.MySqlClient.MySqlParameter("@CostType", "ODC")).ToList();

                    // String dt = DateTime.Parse(activityStart).ToString(SQL_DATE_FORMAT);
                    String dt = DateTime.Parse(activityStart).ToString(SQL_DATE_FORMAT);
                    List<int> lineItemsToScale = new List<int>();

                    while (diff > 0)
                    {

                        CostODC costODC = new CostODC();
                        String odcCostID = act.ActivityID.ToString() + "_" + costId + "_" + textboxIndex;

                        costODC.ActivityID = act.ActivityID;
                        costODC.ODCStartDate = activityStart;
                        costODC.ODCEndDate = DateTime.Parse(activityStart).AddDays(6).ToString(SQL_DATE_FORMAT);
                        costODC.ODCQuantity = "0";
                        costODC.ODCPrice = odcList.ElementAt(0).ODCPrice;
                        costODC.ODCCost = "0";
                        costODC.OriginalCost = "0";
                        costODC.Granularity = "week";
                        costODC.EstimatedCostID = 0;
                        costODC.CostTrackTypeID = 2;
                        costODC.CreatedBy = odcList.ElementAt(0).CreatedBy;
                        costODC.UpdatedBy = odcList.ElementAt(0).UpdatedBy;
                        costODC.CreatedDate = odcList.ElementAt(0).CreatedDate;
                        costODC.UpdatedDate = odcList.ElementAt(0).UpdatedDate;
                        costODC.ODCCostID = odcCostID;
                        costODC.CostLineItemID = costLineItem;
                        costODC.ODCTypeID = Int32.Parse(odcList.ElementAt(0).ODCTypeID);


                        ctx.CostODC.Add(costODC);
                        ctx.SaveChanges();

                        CostODC actualCostODC = ctx.CostODC.AsNoTracking().FirstOrDefault(a => a.ID == costODC.ID);
                        actualCostODC.ODCCostID = act.ActivityID.ToString() + "_" + actualCostId + "_" + textboxIndex;
                        actualCostODC.CostTrackTypeID = 3;
                        actualCostODC.EstimatedCostID = costODC.ID;
                        actualCostODC.ODCCost = "0";
                        actualCostODC.OriginalCost = "0";
                        actualCostODC.ODCQuantity = "0";
                        actualCostODC.ID = 0;
                        //actualCostODC.CostLineItemID = costLineItem + "." + "01";
                        actualCostODC.CostLineItemID = costLineItem + "1";   //Manasi 07-11-2020
                        ctx.CostODC.Add(actualCostODC);
                        ctx.SaveChanges();

                        CostODC etcCostODC = ctx.CostODC.AsNoTracking().FirstOrDefault(a => a.ID == costODC.ID);
                        etcCostODC.ODCCostID = act.ActivityID.ToString() + "_" + etcCostId + "_" + textboxIndex;
                        etcCostODC.CostTrackTypeID = 4; actualCostODC.EstimatedCostID = costODC.ID;
                        etcCostODC.EstimatedCostID = costODC.ID;
                        etcCostODC.ODCCost = "0";
                        etcCostODC.OriginalCost = "0";
                        etcCostODC.ODCQuantity = "0";
                        etcCostODC.ID = 0;
                        //etcCostODC.CostLineItemID = costLineItem + "." + "02";
                        etcCostODC.CostLineItemID = costLineItem + "2";   //Manasi 07-11-2020
                        ctx.CostODC.Add(etcCostODC);
                        ctx.SaveChanges();

                        textboxIndex += 1;
                        activityStart = DateTime.Parse(activityStart).AddDays(7).ToString(SQL_DATE_FORMAT); ;
                        diff -= 7;
                    }

                    //Padding Right Cost
                    var endDate = odcList.LastOrDefault().ODCEndDate;
                    var activityStartRight = act.ActivityStartDate;
                    var endDiff = (DateTime.Parse(activityStartRight) - DateTime.Parse(endDate)).TotalDays;
                    while (endDiff > 0)
                    {
                        CostODC costODC = new CostODC();
                        String odcCostID = act.ActivityID.ToString() + "_" + costId + "_" + textboxIndex;

                        costODC.ActivityID = act.ActivityID;
                        costODC.ODCStartDate = DateTime.Parse(endDate).AddDays(1).ToString(SQL_DATE_FORMAT);
                        costODC.ODCEndDate = DateTime.Parse(costODC.ODCStartDate).AddDays(6).ToString(SQL_DATE_FORMAT);

                        costODC.ODCQuantity = "0";
                        costODC.ODCPrice = odcList.ElementAt(0).ODCPrice;
                        costODC.ODCCost = "0";
                        costODC.OriginalCost = "0";
                        costODC.Granularity = "week";
                        costODC.EstimatedCostID = 0;
                        costODC.CostTrackTypeID = 2;
                        costODC.CreatedBy = odcList.ElementAt(0).CreatedBy;
                        costODC.UpdatedBy = odcList.ElementAt(0).UpdatedBy;
                        costODC.CreatedDate = odcList.ElementAt(0).CreatedDate;
                        costODC.UpdatedDate = odcList.ElementAt(0).UpdatedDate;
                        costODC.ODCCostID = odcCostID;
                        costODC.CostLineItemID = costLineItem;
                        costODC.ODCTypeID = Int32.Parse(odcList.ElementAt(0).ODCTypeID);

                        ctx.CostODC.Add(costODC);
                        ctx.SaveChanges();

                        CostODC actualCostODC = ctx.CostODC.AsNoTracking().FirstOrDefault(a => a.ID == costODC.ID);
                        actualCostODC.ODCCostID = act.ActivityID.ToString() + "_" + actualCostId + "_" + textboxIndex;
                        actualCostODC.CostTrackTypeID = 3;
                        actualCostODC.EstimatedCostID = costODC.ID;
                        actualCostODC.ODCCost = "0";
                        actualCostODC.OriginalCost = "0";
                        actualCostODC.ODCQuantity = "0";
                        actualCostODC.ID = 0;
                        //actualCostODC.CostLineItemID = costLineItem + "." + "01";
                        actualCostODC.CostLineItemID = costLineItem + "1";   //Manasi 07-11-2020
                        ctx.CostODC.Add(actualCostODC);
                        ctx.SaveChanges();

                        CostODC etcCostODC = ctx.CostODC.AsNoTracking().FirstOrDefault(a => a.ID == costODC.ID);
                        etcCostODC.ODCCostID = act.ActivityID.ToString() + "_" + etcCostId + "_" + textboxIndex;
                        etcCostODC.CostTrackTypeID = 4; actualCostODC.EstimatedCostID = costODC.ID;
                        etcCostODC.EstimatedCostID = costODC.ID;
                        etcCostODC.ODCCost = "0";
                        etcCostODC.OriginalCost = "0";
                        etcCostODC.ODCQuantity = "0";
                        etcCostODC.ID = 0;
                        //etcCostODC.CostLineItemID = costLineItem + "." + "02";
                        etcCostODC.CostLineItemID = costLineItem + "2";    //Manasi 07-11-2020
                        ctx.CostODC.Add(etcCostODC);
                        ctx.SaveChanges();


                        textboxIndex += 1;
                        activityStart = DateTime.Parse(activityStart).AddDays(7).ToString(SQL_DATE_FORMAT); ;
                        endDiff -= 7;
                    }


                    for (int i = 0; i < odcList.Count; i++)
                    {
                        var odcDate = DateTime.Parse(odcList.ElementAt(i).ODCStartDate).ToString(SQL_DATE_FORMAT);

                        var cost = odcList.ElementAt(i);
                        CostODC costODC = new CostODC();
                        String odcCostID = act.ActivityID.ToString() + "_" + costId + "_" + textboxIndex;

                        costODC.ActivityID = act.ActivityID;
                        costODC.ODCStartDate = DateTime.Parse(cost.ODCStartDate).ToString(SQL_DATE_FORMAT);
                        costODC.ODCEndDate = DateTime.Parse(cost.ODCEndDate).ToString(SQL_DATE_FORMAT);

                        costODC.ODCQuantity = cost.ODCQuantity;
                        costODC.ODCPrice = odcList.ElementAt(0).ODCPrice;
                        costODC.ODCCost = cost.ODCCost;
                        costODC.OriginalCost = cost.OriginalCost;
                        costODC.Granularity = "week";
                        costODC.EstimatedCostID = 0;
                        costODC.CostTrackTypeID = 2;
                        costODC.CreatedBy = cost.CreatedBy;
                        costODC.UpdatedBy = cost.UpdatedBy;
                        costODC.CreatedDate = cost.CreatedDate;
                        costODC.UpdatedDate = cost.UpdatedDate;
                        costODC.ODCCostID = odcCostID;
                        costODC.CostLineItemID = costLineItem;
                        costODC.ODCTypeID = Int32.Parse(cost.ODCTypeID);

                        ctx.CostODC.Add(costODC);
                        ctx.SaveChanges();

                        CostODC actualCostODC = ctx.CostODC.AsNoTracking().FirstOrDefault(a => a.ID == costODC.ID);
                        actualCostODC.ODCCostID = act.ActivityID.ToString() + "_" + actualCostId + "_" + textboxIndex;
                        actualCostODC.CostTrackTypeID = 3;
                        actualCostODC.EstimatedCostID = costODC.ID;
                        actualCostODC.ODCCost = "0";
                        actualCostODC.OriginalCost = "0";
                        actualCostODC.ODCQuantity = "0";
                        actualCostODC.ID = 0;
                        //actualCostODC.CostLineItemID = costLineItem + "." + "01";
                        actualCostODC.CostLineItemID = costLineItem + "1";   //Manasi 07-11-2020
                        ctx.CostODC.Add(actualCostODC);
                        ctx.SaveChanges();

                        CostODC etcCostODC = ctx.CostODC.AsNoTracking().FirstOrDefault(a => a.ID == costODC.ID);
                        etcCostODC.ODCCostID = act.ActivityID.ToString() + "_" + etcCostId + "_" + textboxIndex;
                        etcCostODC.CostTrackTypeID = 4;
                        etcCostODC.EstimatedCostID = costODC.ID;
                        etcCostODC.ODCCost = "0";
                        etcCostODC.OriginalCost = "0";
                        etcCostODC.ODCQuantity = "0";
                        etcCostODC.ID = 0;
                        //etcCostODC.CostLineItemID = costLineItem + "." + "02";
                        etcCostODC.CostLineItemID = costLineItem + "2";    //Manasi 07-11-2020
                        ctx.CostODC.Add(etcCostODC);
                        ctx.SaveChanges();

                        //TODO
                        List<TemporaryCost> tempCost = additionalCost.Where(a => (a.CostTrackTypeID == "3") && a.StartDate.Equals(odcDate)).ToList();
                        int additionalActualID = etcCostId;
                        int additionalETCID = additionalActualID;
                        foreach (TemporaryCost costs in tempCost)
                        {
                            additionalActualID += 1;
                            additionalETCID = additionalActualID + 1;
                            actualCostODC = ctx.CostODC.AsNoTracking().FirstOrDefault(a => a.ID == costODC.ID);
                            actualCostODC.ODCCostID = act.ActivityID.ToString() + "_" + additionalActualID + "_" + textboxIndex;
                            actualCostODC.CostTrackTypeID = 3;
                            actualCostODC.EstimatedCostID = costODC.ID;
                            actualCostODC.ODCCost = "0";
                            actualCostODC.OriginalCost = "0";
                            actualCostODC.ODCQuantity = "0";
                            actualCostODC.ID = 0;
                            actualCostODC.ODCTypeID = Int32.Parse(costs.Type);
                            //actualCostODC.CostLineItemID = costLineItem + "." + ((additionalActualID.ToString().Length == 1) ? "0" + (additionalActualID - 1).ToString() : (additionalActualID - 1).ToString());//Actual Cost
                            actualCostODC.CostLineItemID = costLineItem + (additionalActualID - 1).ToString();//Actual Cost Manasi 07-11-2020
                            ctx.CostODC.Add(actualCostODC);
                            ctx.SaveChanges();



                            etcCostODC = ctx.CostODC.AsNoTracking().FirstOrDefault(a => a.ID == costODC.ID);
                            etcCostODC.ODCCostID = act.ActivityID.ToString() + "_" + additionalETCID + "_" + textboxIndex;
                            etcCostODC.CostTrackTypeID = 4;
                            etcCostODC.EstimatedCostID = costODC.ID;
                            etcCostODC.ODCCost = "0";
                            etcCostODC.OriginalCost = "0";
                            etcCostODC.ODCQuantity = "0";
                            etcCostODC.ID = 0;
                            etcCostODC.ODCTypeID = Int32.Parse(costs.Type);
                            //etcCostODC.CostLineItemID = costLineItem + "." + ((additionalETCID.ToString().Length == 1) ? "0" + (additionalETCID - 1).ToString() : (additionalETCID - 1).ToString());//Actual Cost
                            etcCostODC.CostLineItemID = costLineItem + (additionalETCID - 1).ToString();//Actual Cost   Manasi 07-11-2020
                            ctx.CostODC.Add(etcCostODC);
                            ctx.SaveChanges();



                            lineItemsToScale.Add(additionalActualID);
                            lineItemsToScale.Add(additionalETCID);
                        }

                        textboxIndex += 1;

                        //Padding differenc in the middle
                        if (i + 1 != odcList.Count)
                        {
                            var nextCost = odcList.ElementAt(i + 1);
                            var diffDate = (DateTime.Parse(nextCost.ODCStartDate) - DateTime.Parse(cost.ODCEndDate)).TotalDays;
                            var eDate = cost.ODCEndDate;
                            while (diffDate > 1)
                            {
                                costODC = new CostODC();
                                odcCostID = act.ActivityID.ToString() + "_" + costId + "_" + textboxIndex;

                                costODC.ActivityID = act.ActivityID;
                                costODC.ODCStartDate = DateTime.Parse(eDate).AddDays(1).ToString(SQL_DATE_FORMAT);
                                costODC.ODCEndDate = DateTime.Parse(costODC.ODCStartDate).AddDays(6).ToString(SQL_DATE_FORMAT);

                                costODC.ODCQuantity = "0";
                                costODC.ODCPrice = odcList.ElementAt(0).ODCPrice;
                                costODC.ODCCost = "0";
                                costODC.OriginalCost = "0";
                                costODC.Granularity = "week";
                                costODC.EstimatedCostID = 0;
                                costODC.CostTrackTypeID = 2;
                                costODC.CreatedBy = odcList.ElementAt(0).CreatedBy;
                                costODC.UpdatedBy = odcList.ElementAt(0).UpdatedBy;
                                costODC.CreatedDate = odcList.ElementAt(0).CreatedDate;
                                costODC.UpdatedDate = odcList.ElementAt(0).UpdatedDate;
                                costODC.ODCCostID = odcCostID;
                                costODC.CostLineItemID = costLineItem;
                                costODC.ODCTypeID = Int32.Parse(odcList.ElementAt(0).ODCTypeID);

                                ctx.CostODC.Add(costODC);
                                ctx.SaveChanges();

                                actualCostODC = ctx.CostODC.AsNoTracking().FirstOrDefault(a => a.ID == costODC.ID);
                                actualCostODC.ODCCostID = act.ActivityID.ToString() + "_" + actualCostId + "_" + textboxIndex;
                                actualCostODC.CostTrackTypeID = 3;
                                actualCostODC.EstimatedCostID = costODC.ID;
                                actualCostODC.ODCCost = "0";
                                actualCostODC.OriginalCost = "0";
                                actualCostODC.ODCQuantity = "0";
                                actualCostODC.ID = 0;
                                //actualCostODC.CostLineItemID = costLineItem + "." + "01";
                                actualCostODC.CostLineItemID = costLineItem + "1";      //Manasi 07-11-2020
                                ctx.CostODC.Add(actualCostODC);
                                ctx.SaveChanges();

                                etcCostODC = ctx.CostODC.AsNoTracking().FirstOrDefault(a => a.ID == costODC.ID);
                                etcCostODC.ODCCostID = act.ActivityID.ToString() + "_" + etcCostId + "_" + textboxIndex;
                                etcCostODC.CostTrackTypeID = 4; actualCostODC.EstimatedCostID = costODC.ID;
                                etcCostODC.EstimatedCostID = costODC.ID;
                                etcCostODC.ODCCost = "0";
                                etcCostODC.OriginalCost = "0";
                                etcCostODC.ODCQuantity = "0";
                                etcCostODC.ID = 0;
                                //etcCostODC.CostLineItemID = costLineItem + "." + "02";
                                etcCostODC.CostLineItemID = costLineItem + "2";     //Manasi 07-11-2020
                                ctx.CostODC.Add(etcCostODC);
                                ctx.SaveChanges();

                                tempCost = additionalCost.Where(a => (a.CostTrackTypeID == "3") && a.StartDate.Equals(costODC.ODCStartDate)).ToList();
                                additionalActualID = etcCostId;
                                additionalETCID = additionalActualID;
                                foreach (TemporaryCost costs in tempCost)
                                {
                                    additionalActualID += 1;
                                    additionalETCID = additionalActualID + 1;
                                    actualCostODC = ctx.CostODC.AsNoTracking().FirstOrDefault(a => a.ID == costODC.ID);
                                    actualCostODC.ODCCostID = act.ActivityID.ToString() + "_" + additionalActualID + "_" + textboxIndex;
                                    actualCostODC.CostTrackTypeID = 3;
                                    actualCostODC.EstimatedCostID = costODC.ID;
                                    actualCostODC.ODCCost = "0";
                                    actualCostODC.OriginalCost = "0";
                                    actualCostODC.ODCQuantity = "0";
                                    actualCostODC.ID = 0;
                                    actualCostODC.ODCTypeID = Int32.Parse(costs.Type);
                                    //actualCostODC.CostLineItemID = costLineItem + "." + ((additionalActualID.ToString().Length == 1) ? "0" + (additionalActualID - 1).ToString() : (additionalActualID - 1).ToString());//Actual Cost
                                    actualCostODC.CostLineItemID = costLineItem + (additionalActualID - 1).ToString();//Actual Cost    Manasi 07-11-2020
                                    ctx.CostODC.Add(actualCostODC);
                                    ctx.SaveChanges();



                                    etcCostODC = ctx.CostODC.AsNoTracking().FirstOrDefault(a => a.ID == costODC.ID);
                                    etcCostODC.ODCCostID = act.ActivityID.ToString() + "_" + additionalETCID + "_" + textboxIndex;
                                    etcCostODC.CostTrackTypeID = 4;
                                    etcCostODC.EstimatedCostID = costODC.ID;
                                    etcCostODC.ODCCost = "0";
                                    etcCostODC.OriginalCost = "0";
                                    etcCostODC.ODCQuantity = "0";
                                    etcCostODC.ID = 0;
                                    etcCostODC.ODCTypeID = Int32.Parse(costs.Type);
                                    //etcCostODC.CostLineItemID = costLineItem + "." + ((additionalETCID.ToString().Length == 1) ? "0" + (additionalETCID - 1).ToString() : (additionalETCID - 1).ToString());//Actual Cost
                                    etcCostODC.CostLineItemID = costLineItem + (additionalETCID - 1).ToString();//Actual Cost    Manasi 07-11-2020
                                    ctx.CostODC.Add(etcCostODC);
                                    ctx.SaveChanges();



                                    lineItemsToScale.Add(additionalActualID);
                                    lineItemsToScale.Add(additionalETCID);
                                }






                                textboxIndex += 1;
                                diffDate -= 7;
                                eDate = DateTime.Parse(eDate).AddDays(7).ToString(SQL_DATE_FORMAT); ;
                            }
                        }


                    }

                    var activityEndDate = act.ActivityEndDate;
                    var rightDiff = (DateTime.Parse(activityEndDate) - DateTime.Parse(endDate)).TotalDays;
                    while (rightDiff > 0)
                    {
                        CostODC costODC = new CostODC();
                        String odcCostID = act.ActivityID.ToString() + "_" + costId + "_" + textboxIndex;

                        costODC.ActivityID = act.ActivityID;
                        costODC.ODCStartDate = DateTime.Parse(endDate).AddDays(1).ToString(SQL_DATE_FORMAT);
                        costODC.ODCEndDate = DateTime.Parse(costODC.ODCStartDate).AddDays(6).ToString(SQL_DATE_FORMAT);

                        costODC.ODCQuantity = "0";
                        costODC.ODCPrice = odcList.ElementAt(0).ODCPrice;
                        costODC.ODCCost = "0";
                        costODC.OriginalCost = "0";
                        costODC.Granularity = "week";
                        costODC.EstimatedCostID = 0;
                        costODC.CostTrackTypeID = 2;
                        costODC.CreatedBy = odcList.ElementAt(0).CreatedBy;
                        costODC.UpdatedBy = odcList.ElementAt(0).UpdatedBy;
                        costODC.CreatedDate = odcList.ElementAt(0).CreatedDate;
                        costODC.UpdatedDate = odcList.ElementAt(0).UpdatedDate;
                        costODC.ODCCostID = odcCostID;
                        costODC.CostLineItemID = costLineItem;
                        costODC.ODCTypeID = Int32.Parse(odcList.ElementAt(0).ODCTypeID);

                        ctx.CostODC.Add(costODC);
                        ctx.SaveChanges();

                        CostODC actualCostODC = ctx.CostODC.AsNoTracking().FirstOrDefault(a => a.ID == costODC.ID);
                        actualCostODC.ODCCostID = act.ActivityID.ToString() + "_" + actualCostId + "_" + textboxIndex;
                        actualCostODC.CostTrackTypeID = 3;
                        actualCostODC.EstimatedCostID = costODC.ID;
                        actualCostODC.ODCCost = "0";
                        actualCostODC.OriginalCost = "0";
                        actualCostODC.ODCQuantity = "0";
                        actualCostODC.ID = 0;
                        //actualCostODC.CostLineItemID = costLineItem + "." + "01";
                        actualCostODC.CostLineItemID = costLineItem + "1";     //Manasi 07-11-2020
                        ctx.CostODC.Add(actualCostODC);
                        ctx.SaveChanges();

                        CostODC etcCostODC = ctx.CostODC.AsNoTracking().FirstOrDefault(a => a.ID == costODC.ID);
                        etcCostODC.ODCCostID = act.ActivityID.ToString() + "_" + etcCostId + "_" + textboxIndex;
                        etcCostODC.CostTrackTypeID = 4; actualCostODC.EstimatedCostID = costODC.ID;
                        etcCostODC.EstimatedCostID = costODC.ID;
                        etcCostODC.ODCCost = "0";
                        etcCostODC.OriginalCost = "0";
                        etcCostODC.ODCQuantity = "0";
                        etcCostODC.ID = 0;
                        //etcCostODC.CostLineItemID = costLineItem + "." + "02";
                        etcCostODC.CostLineItemID = costLineItem + "2";    //Manasi 07-11-2020
                        ctx.CostODC.Add(etcCostODC);
                        ctx.SaveChanges();

                        List<TemporaryCost> tempCost = additionalCost.Where(a => (a.CostTrackTypeID == "3") && a.StartDate.Equals(costODC.ODCStartDate)).ToList();
                        int additionalActualID = etcCostId;
                        int additionalETCID = additionalActualID;
                        foreach (TemporaryCost costs in tempCost)
                        {
                            additionalActualID += 1;
                            additionalETCID = additionalActualID + 1;
                            actualCostODC = ctx.CostODC.AsNoTracking().FirstOrDefault(a => a.ID == costODC.ID);
                            actualCostODC.ODCCostID = act.ActivityID.ToString() + "_" + additionalActualID + "_" + textboxIndex;
                            actualCostODC.CostTrackTypeID = 3;
                            actualCostODC.EstimatedCostID = costODC.ID;
                            actualCostODC.ODCCost = "0";
                            actualCostODC.OriginalCost = "0";
                            actualCostODC.ODCQuantity = "0";
                            actualCostODC.ID = 0;
                            actualCostODC.ODCTypeID = Int32.Parse(costs.Type);
                            //actualCostODC.CostLineItemID = costLineItem + "." + ((additionalActualID.ToString().Length == 1) ? "0" + (additionalActualID - 1).ToString() : (additionalActualID - 1).ToString());//Actual Cost
                            actualCostODC.CostLineItemID = costLineItem + (additionalActualID - 1).ToString();//Actual Cost   Manasi 07-11-2020
                            ctx.CostODC.Add(actualCostODC);
                            ctx.SaveChanges();



                            etcCostODC = ctx.CostODC.AsNoTracking().FirstOrDefault(a => a.ID == costODC.ID);
                            etcCostODC.ODCCostID = act.ActivityID.ToString() + "_" + additionalETCID + "_" + textboxIndex;
                            etcCostODC.CostTrackTypeID = 4;
                            etcCostODC.EstimatedCostID = costODC.ID;
                            etcCostODC.ODCCost = "0";
                            etcCostODC.OriginalCost = "0";
                            etcCostODC.ODCQuantity = "0";
                            etcCostODC.ID = 0;
                            etcCostODC.ODCTypeID = Int32.Parse(costs.Type);
                            //etcCostODC.CostLineItemID = costLineItem + "." + ((additionalETCID.ToString().Length == 1) ? "0" + (additionalETCID - 1).ToString() : (additionalETCID - 1).ToString());//Actual Cost
                            etcCostODC.CostLineItemID = costLineItem + (additionalETCID - 1).ToString();//Actual Cost   Manasi 07-11-2020
                            ctx.CostODC.Add(etcCostODC);
                            ctx.SaveChanges();



                            lineItemsToScale.Add(additionalActualID);
                            lineItemsToScale.Add(additionalETCID);
                        }


                        textboxIndex += 1;
                        endDate = DateTime.Parse(endDate).AddDays(7).ToString(SQL_DATE_FORMAT); ;
                        rightDiff -= 7;
                    }


                    //3. Unit
                    //4. ODC
                    //Scaling
                    Scaling.scaling(act.ActivityID, costId, "week", "ODC");
                    //Scaling.scaling(act.ActivityID, actualCostId, "week", "F");
                    //Scaling.scaling(act.ActivityID, etcCostId, "week", "F");
                    ////Scaling additional fte cost
                    //lineItemsToScale = lineItemsToScale.Distinct().ToList();
                    //foreach (int id in lineItemsToScale)
                    //{
                    //    Scaling.scaling(act.ActivityID, id, "week", "F");
                    //}
                    lineItemsToScale.Add(actualCostId);
                    lineItemsToScale.Add(etcCostId);
                    var maxId = Int32.Parse(lineItemsToScale.Max(a => a).ToString());
                    costId = maxId + 1;

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return costId;
        }

        public static void updateCostFromTempTableForRollup(int projectID, String PhaseCode, String MainCategory, String SubCategory)
        {
            try
            {
                var ctx = new CPPDbContext();
                ctx.Database.ExecuteSqlCommand("call update_actual_etc_cost_Migration(@ProjectID,@PhaseCode,@MainCategory)",
                          new MySql.Data.MySqlClient.MySqlParameter("@ProjectID", projectID),
                          new MySql.Data.MySqlClient.MySqlParameter("@PhaseCode", PhaseCode),
                          new MySql.Data.MySqlClient.MySqlParameter("@MainCategory", MainCategory)
                          //new MySql.Data.MySqlClient.MySqlParameter("@SubCategory", SubCategory)
                          );
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void RemoveCostsFromTempTableForRollup(int projectID, String phaseCode, String MainCategory, String SubCategory)
        {
            try
            {
                var ctx = new CPPDbContext();
                String pID = projectID.ToString();
                ctx.TemporaryCostMigration.RemoveRange(ctx.TemporaryCostMigration.Where(a => a.ProjectID == pID && a.PhaseCode == phaseCode && a.MainCategory == MainCategory));
                ctx.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public static void scaleActualEtcCost(String CostType, String ProjectID, String ActivityID)
        {
            try
            {
                var ctx = new CPPDbContext();
                ctx.Database.ExecuteSqlCommand("call scale_actual_etc_cost(@ProjectID,@ActivityID,@CostType)",
                          new MySql.Data.MySqlClient.MySqlParameter("@ProjectID", ProjectID),
                          new MySql.Data.MySqlClient.MySqlParameter("@ActivityID", ActivityID),
                          new MySql.Data.MySqlClient.MySqlParameter("@CostType", CostType)
                          );
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }


}