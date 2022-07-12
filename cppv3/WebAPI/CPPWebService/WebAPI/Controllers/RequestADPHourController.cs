using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models.ADP;
using WebAPI.Models;
using WebAPI.Models.StoredProcedure;
using WebAPI.Helper;

namespace WebAPI.Controllers
{
    public class RequestADPHourController : ApiController
    {

        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        [HttpGet]
        public String GET()
        {
            Console.WriteLine("Hello");

            using (var ctx = new CPPDbContext())
            {
                List<Project> projects = ctx.Project.ToList();
                foreach (var cppProject in projects)
                {
                    IEnumerable<List<ADPHour>> lines = ctx.Database.SqlQuery<ADPHour>("call QB_get_hours(@Projectid)",
                            new MySql.Data.MySqlClient.MySqlParameter("@ProjectId", cppProject.ProjectID))
                                 .GroupBy(a => a.Name).Select(a => a.ToList());


                    foreach (var line in lines)
                    {

                        Project project = ctx.Database.SqlQuery<Project>("CALL qb_get_project(@Ref)",
                                                                    new MySql.Data.MySqlClient.MySqlParameter("@Ref", line.FirstOrDefault().WorkJobs)
                                                                    ).FirstOrDefault();
                        if (project == null)
                        {
                            logger.Error(String.Format("Unable to find line {0} , {1}", line.FirstOrDefault().WorkJobs, line.FirstOrDefault().Name));
                            continue;
                        }

                        logger.Info(String.Format("Processing costs for projectID {0}, project name {1}, project number {2}", project.ProjectID.ToString(), project.ProjectName, project.ProjectNumber));

                        var programElement = ctx.ProgramElement.Include("ProjectClass").Where(pm => pm.ProgramElementID == project.ProgramElementID).FirstOrDefault();
                        Trend trend = ctx.Trend.Where(a => a.ProjectID == project.ProjectID && a.TrendNumber == "1000")
                                            .FirstOrDefault();


                        ActivityCategory cat = ctx.ActivityCategory.Where(a => a.CategoryID == "9999"
                                                            && a.SubCategoryID == "9999").FirstOrDefault();
                        Activity act = ctx.Activity.Where(a => a.BudgetID == 6014 && a.TrendID == trend.TrendID).FirstOrDefault();
                        var phase = ctx.PhaseCode.Where(a => a.PhaseID == act.PhaseCode).FirstOrDefault();//phaseCode in Activity object is the fk points to phaseID
                        var projectClass = ctx.ProjectClass.Where(a => a.ProjectClassID == project.ProjectClassID).FirstOrDefault();//project class line item for the costcode
                        var programElementClass = ctx.ProjectClass.Where(a => a.ProjectClassID == programElement.ProjectClassID).FirstOrDefault();  //Manasi 27-10-2020
                        var empName = line.FirstOrDefault().Name;
                        Employee emp = ctx.Employee.Where(a => a.Name.Trim() == empName).FirstOrDefault();

                        var costTrackTypeID = 2;
                        var estimatedCostID = 0;
                        var ftePositionID = 24;

                        CostLineItemResult costLineItem = CostLineItemResult.getCostLineItem(projectClass.ProjectClassLineItemID.ToString(), project.ProjectNumber, project.ProjectElementNumber, act.TrendNumber, phase.ActivityPhaseCode.ToString(),
                                                                           cat.CategoryID, cat.SubCategoryID, "F", ftePositionID.ToString(), emp.ID.ToString(), null, null, null, null, null);

                        //is LineItemexist

                        bool isExist = false;

                        String lineNumber = "";
                        if (costLineItem != null)
                        {
                            if (costLineItem.LineNumber != 0 && costLineItem.LineNumber.ToString().Length == 1)
                                lineNumber = "0" + costLineItem.LineNumber.ToString();
                            else lineNumber = costLineItem.LineNumber.ToString();
                            isExist = costLineItem.IsExist;
                        }
                        //string lineItem = Cost.getLineItem(project.ProjectClassID.ToString(), project.ProjectNumber, activity.PhaseCode.ToString(),
                        //                                       category.CategoryID, category.SubCategoryID, lineNumber);

                        //string lineItem = Cost.getLineItem(projectClass.ProjectClassLineItemID.ToString(), programElement.ProjectNumber, project.ProjectElementNumber,
                        //                       phase.ActivityPhaseCode.ToString(),
                        //                 cat.CategoryID, cat.SubCategoryID, lineNumber);

                        //----------Manasi 26-10-2020-------------------------------------------
                        string year = programElement.ProjectStartDate.Split('/')[2].ToString();
                        year = year.Substring(2, 2);

                        string lineItem = Cost.getLineItem(projectClass.ProjectClassLineItemID.ToString(), programElement.ProjectNumber, project.ProjectElementNumber,
                                              phase.ActivityPhaseCode.ToString(),
                                        cat.CategoryID, cat.SubCategoryID, lineNumber, year, "1", programElementClass.ProjectClassLineItemID);
                        //-------------------------------------------------------------------------

                        String CostID = ctx.Database.SqlQuery<String>("CALL QB_get_cost_id(@ProjectID, @TrendID, @ActivityID, @CostLineItemID,@CostType)",
                                                        new MySql.Data.MySqlClient.MySqlParameter("@ProjectID", project.ProjectID),
                                                        new MySql.Data.MySqlClient.MySqlParameter("@TrendID", trend.TrendID),
                                                        new MySql.Data.MySqlClient.MySqlParameter("@ActivityID", act.ActivityID),
                                                        new MySql.Data.MySqlClient.MySqlParameter("@CostLineItemID", lineItem),
                                                        new MySql.Data.MySqlClient.MySqlParameter("@CostType", "F")
                                                    ).FirstOrDefault();

                        int costId = Convert.ToInt16(CostID.Split('_')[1]);
                        int actualCostId = costId + 1;
                        int etcCostId = actualCostId + 1;

                        //Padding left Cost
                        var startDate = line.FirstOrDefault().StartDate;
                        var activityStart = act.ActivityStartDate;
                        var diff = (DateTime.Parse(startDate) - DateTime.Parse(activityStart)).TotalDays;
                        int textboxIndex = 0;
                        while (diff > 0)
                        {

                            CostFTE costFte = new CostFTE();
                            String fteCostID = act.ActivityID.ToString() + "_" + costId + "_" + textboxIndex;
                            costFte.FTECostID = fteCostID;
                            costFte.ActivityID = act.ActivityID;
                            costFte.FTEStartDate = activityStart;
                            costFte.FTEEndDate = DateTime.Parse(activityStart).AddDays(6).ToString(DateUtility.getSqlDateFormat());
                            costFte.FTEPosition = ftePositionID.ToString();
                            costFte.FTEPositionID = ftePositionID;
                            costFte.FTECost = "0";
                            costFte.FTEValue = "0";
                            costFte.FTEHourlyRate = line.FirstOrDefault().SalaryRate;
                            costFte.FTEHours = "0";
                            costFte.RawFTEHourlyRate = line.FirstOrDefault().SalaryRate;
                            costFte.Granularity = "week";
                            costFte.CostTrackTypeID = 2;//--todo
                            costFte.EstimatedCostID = Int32.Parse("0");
                            costFte.EmployeeID = emp.ID;
                            costFte.CostLineItemID = lineItem;
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
                            actualCostFte.FTEHourlyRate = line.FirstOrDefault().SalaryRate;
                            actualCostFte.RawFTEHourlyRate = line.FirstOrDefault().SalaryRate;
                            actualCostFte.FTEStartDate = DateTime.Parse(actualCostFte.FTEStartDate).ToString(DateUtility.getSqlDateFormat());
                            actualCostFte.FTEEndDate = DateTime.Parse(actualCostFte.FTEEndDate).ToString(DateUtility.getSqlDateFormat());
                            actualCostFte.ID = 0;
                            actualCostFte.CostLineItemID = lineItem + "." + "01";//Actual Cost
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
                            etcCostFte.FTEHourlyRate = line.FirstOrDefault().SalaryRate;
                            etcCostFte.RawFTEHourlyRate = line.FirstOrDefault().SalaryRate;
                            etcCostFte.FTEStartDate = DateTime.Parse(etcCostFte.FTEStartDate).ToString(DateUtility.getSqlDateFormat());
                            etcCostFte.FTEEndDate = DateTime.Parse(etcCostFte.FTEEndDate).ToString(DateUtility.getSqlDateFormat());
                            etcCostFte.ID = 0;
                            etcCostFte.OriginalCost = "0";
                            etcCostFte.CostLineItemID = lineItem + "." + "02";
                            ctx.CostFte.Add(etcCostFte);
                            ctx.SaveChanges();




                            textboxIndex += 1;
                            activityStart = DateTime.Parse(activityStart).AddDays(7).ToString(DateUtility.getSqlDateFormat()); ;
                            diff -= 7;
                        }

                        //
                        for (int i = 0; i < line.Count; i++)
                        {
                            var fteDate = DateTime.Parse(line.ElementAt(i).StartDate).ToString(DateUtility.getSqlDateFormat());

                            var cost = line.ElementAt(i);
                            var fteValue = double.Parse(cost.Hours) / 40;
                            var hours = (double.Parse(cost.Hours) / 8).ToString();
                            CostFTE costFte = new CostFTE();
                            String fteCostID = act.ActivityID.ToString() + "_" + costId + "_" + textboxIndex;
                            costFte.FTECostID = fteCostID;
                            costFte.Granularity = "week";
                            costFte.ActivityID = act.ActivityID;
                            costFte.FTEStartDate = DateTime.Parse(cost.StartDate).ToString(DateUtility.getSqlDateFormat());
                            costFte.FTEEndDate = DateTime.Parse(cost.EndDate).ToString(DateUtility.getSqlDateFormat());
                            costFte.FTEPosition = ftePositionID.ToString();
                            costFte.FTEValue = "0";
                            costFte.FTEHourlyRate = cost.SalaryRate;
                            costFte.RawFTEHourlyRate = cost.SalaryRate;
                            costFte.FTEHours = "0";
                            costFte.FTECost = "0";
                            costFte.FTEPositionID = ftePositionID;
                            costFte.OriginalCost = cost.Total;
                            costFte.CostTrackTypeID = 2;//--todo
                            costFte.EstimatedCostID = 0;
                            costFte.CostLineItemID = lineItem;
                            costFte.EmployeeID = emp.ID;
                            ctx.CostFte.Add(costFte);
                            ctx.SaveChanges();

                            CostFTE actualCostFte = ctx.CostFte.AsNoTracking().FirstOrDefault(a => a.ID == costFte.ID);
                            actualCostFte.FTECostID = act.ActivityID.ToString() + "_" + actualCostId + "_" + textboxIndex;
                            actualCostFte.CostTrackTypeID = 3;
                            actualCostFte.EstimatedCostID = costFte.ID;
                            actualCostFte.FTEHours = hours;
                            actualCostFte.FTECost = cost.Total;
                            actualCostFte.FTEValue = fteValue.ToString();
                            actualCostFte.OriginalCost = cost.Total;
                            actualCostFte.FTEHourlyRate = cost.SalaryRate;
                            actualCostFte.RawFTEHourlyRate = cost.SalaryRate;
                            actualCostFte.FTEStartDate = DateTime.Parse(actualCostFte.FTEStartDate).ToString(DateUtility.getSqlDateFormat());
                            actualCostFte.FTEEndDate = DateTime.Parse(actualCostFte.FTEEndDate).ToString(DateUtility.getSqlDateFormat());
                            actualCostFte.ID = 0;
                            actualCostFte.CostLineItemID = lineItem + "." + "01";//Actual Cost
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
                            etcCostFte.FTEHourlyRate = cost.SalaryRate;
                            etcCostFte.RawFTEHourlyRate = cost.SalaryRate;
                            etcCostFte.FTEStartDate = DateTime.Parse(etcCostFte.FTEStartDate).ToString(DateUtility.getSqlDateFormat());
                            etcCostFte.FTEEndDate = DateTime.Parse(etcCostFte.FTEEndDate).ToString(DateUtility.getSqlDateFormat());
                            etcCostFte.ID = 0;
                            etcCostFte.CostLineItemID = lineItem + "." + "02";//etc Cost
                            ctx.CostFte.Add(etcCostFte);
                            ctx.SaveChanges();


                            textboxIndex += 1;

                            //Padding differenc in the middle
                            if (i + 1 != line.Count)
                            {
                                var nextCost = line.ElementAt(i + 1);
                                var diffDate = (DateTime.Parse(nextCost.StartDate) - DateTime.Parse(cost.EndDate)).TotalDays;
                                var eDate = cost.EndDate;
                                while (diffDate > 1)
                                {
                                    costFte = new CostFTE();
                                    fteCostID = act.ActivityID.ToString() + "_" + costId + "_" + textboxIndex;
                                    costFte.FTECostID = fteCostID;
                                    costFte.ActivityID = act.ActivityID;
                                    costFte.Granularity = "week";
                                    costFte.FTEStartDate = DateTime.Parse(eDate).AddDays(1).ToString(DateUtility.getSqlDateFormat());
                                    costFte.FTEEndDate = DateTime.Parse(costFte.FTEStartDate).AddDays(6).ToString(DateUtility.getSqlDateFormat());
                                    costFte.FTEPosition = ftePositionID.ToString();
                                    costFte.FTEValue = "0";
                                    costFte.OriginalCost = "0";
                                    costFte.FTEHourlyRate = cost.SalaryRate;
                                    costFte.RawFTEHourlyRate = cost.SalaryRate;
                                    costFte.FTEHours = "0";
                                    costFte.FTECost = "0";
                                    costFte.FTEPositionID = ftePositionID;
                                    costFte.EmployeeID = emp.ID;
                                    costFte.CostTrackTypeID = 2;//--todo
                                    costFte.EstimatedCostID = 0;
                                    costFte.CostLineItemID = lineItem;

                                    ctx.CostFte.Add(costFte);
                                    ctx.SaveChanges();

                                    actualCostFte = ctx.CostFte.AsNoTracking().FirstOrDefault(a => a.ID == costFte.ID);
                                    actualCostFte.FTECostID = act.ActivityID.ToString() + "_" + actualCostId + "_" + textboxIndex;
                                    actualCostFte.CostTrackTypeID = 3;
                                    actualCostFte.EstimatedCostID = costFte.ID;
                                    actualCostFte.FTEHours = "0";
                                    actualCostFte.FTECost = "0";
                                    actualCostFte.OriginalCost = "0";
                                    actualCostFte.FTEHourlyRate = cost.SalaryRate;
                                    actualCostFte.RawFTEHourlyRate = cost.SalaryRate;
                                    actualCostFte.FTEStartDate = DateTime.Parse(actualCostFte.FTEStartDate).ToString(DateUtility.getSqlDateFormat());
                                    actualCostFte.FTEEndDate = DateTime.Parse(actualCostFte.FTEEndDate).ToString(DateUtility.getSqlDateFormat());
                                    actualCostFte.ID = 0;
                                    actualCostFte.CostLineItemID = lineItem + "." + "01";//Actual Cost
                                    ctx.CostFte.Add(actualCostFte);
                                    ctx.SaveChanges();

                                    etcCostFte = ctx.CostFte.AsNoTracking().FirstOrDefault(a => a.ID == costFte.ID);
                                    etcCostFte.FTECostID = act.ActivityID.ToString() + "_" + etcCostId + "_" + textboxIndex;
                                    etcCostFte.CostTrackTypeID = 4;
                                    etcCostFte.EstimatedCostID = costFte.ID;
                                    etcCostFte.FTEHours = "0";
                                    etcCostFte.FTECost = "0";
                                    etcCostFte.OriginalCost = "0";
                                    etcCostFte.FTEHourlyRate = cost.SalaryRate;
                                    etcCostFte.RawFTEHourlyRate = cost.SalaryRate;
                                    etcCostFte.FTEStartDate = DateTime.Parse(etcCostFte.FTEStartDate).ToString(DateUtility.getSqlDateFormat());
                                    etcCostFte.FTEEndDate = DateTime.Parse(etcCostFte.FTEEndDate).ToString(DateUtility.getSqlDateFormat());
                                    etcCostFte.ID = 0;
                                    etcCostFte.CostLineItemID = lineItem + "." + "02";//Actual Cost
                                    ctx.CostFte.Add(etcCostFte);
                                    ctx.SaveChanges();


                                    textboxIndex += 1;
                                    diffDate -= 7;
                                    eDate = DateTime.Parse(eDate).AddDays(7).ToString(DateUtility.getSqlDateFormat()); ;
                                }
                            }

                            //}
                        }
                        //If Activity End Date is greater than the cost end date
                        var endDate = line.LastOrDefault().EndDate;
                        var activityEndDate = act.ActivityEndDate;
                        var rightDiff = (DateTime.Parse(activityEndDate) - DateTime.Parse(endDate)).TotalDays;
                        while (rightDiff > 0)
                        {
                            CostFTE costFte = new CostFTE();
                            String fteCostID = act.ActivityID.ToString() + "_" + costId + "_" + textboxIndex;
                            costFte.FTECostID = fteCostID;
                            costFte.Granularity = "week";
                            costFte.ActivityID = act.ActivityID;
                            costFte.FTEStartDate = DateTime.Parse(endDate).AddDays(1).ToString(DateUtility.getSqlDateFormat());
                            costFte.FTEEndDate = DateTime.Parse(costFte.FTEStartDate).AddDays(6).ToString(DateUtility.getSqlDateFormat());
                            costFte.FTEPosition = ftePositionID.ToString();
                            costFte.FTEValue = "0";
                            costFte.FTEHourlyRate = line.FirstOrDefault().SalaryRate;
                            costFte.RawFTEHourlyRate = line.FirstOrDefault().SalaryRate;
                            costFte.FTEHours = "0";
                            costFte.FTECost = "0";
                            costFte.OriginalCost = "0";
                            costFte.FTEPositionID = ftePositionID;
                            costFte.EmployeeID = emp.ID;
                            costFte.CostTrackTypeID = 2;//--todo
                            costFte.CostLineItemID = lineItem;
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
                            actualCostFte.FTEHourlyRate = line.FirstOrDefault().SalaryRate;
                            actualCostFte.RawFTEHourlyRate = line.FirstOrDefault().SalaryRate;
                            actualCostFte.FTEStartDate = DateTime.Parse(actualCostFte.FTEStartDate).ToString(DateUtility.getSqlDateFormat());
                            actualCostFte.FTEEndDate = DateTime.Parse(actualCostFte.FTEEndDate).ToString(DateUtility.getSqlDateFormat());
                            actualCostFte.ID = 0;
                            actualCostFte.CostLineItemID = lineItem + "." + "01";//Actual Cost
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
                            etcCostFte.FTEHourlyRate = line.FirstOrDefault().SalaryRate;
                            etcCostFte.RawFTEHourlyRate = line.FirstOrDefault().SalaryRate;
                            etcCostFte.FTEStartDate = DateTime.Parse(etcCostFte.FTEStartDate).ToString(DateUtility.getSqlDateFormat());
                            etcCostFte.FTEEndDate = DateTime.Parse(etcCostFte.FTEEndDate).ToString(DateUtility.getSqlDateFormat());
                            etcCostFte.ID = 0;
                            etcCostFte.CostLineItemID = lineItem + "." + "02";//Actual Cost
                            ctx.CostFte.Add(etcCostFte);
                            ctx.SaveChanges();


                            textboxIndex += 1;
                            endDate = DateTime.Parse(endDate).AddDays(7).ToString(DateUtility.getSqlDateFormat());

                            rightDiff -= 7;
                        }





                        //Create line Item
                        if (costLineItem.IsExist == false)
                        {
                            CostLineItemTracker.save(projectClass.ProjectClassLineItemID.ToString(), project.ProjectID.ToString(), project.ProjectNumber, project.ProjectElementNumber, act.TrendNumber, phase.ActivityPhaseCode.ToString(), cat.CategoryID,
                                                   cat.SubCategoryID, "F", ftePositionID.ToString(), emp.ID.ToString(), null, null, null, null, null, lineNumber);

                        }

                        //ctx.Database.ExecuteSqlCommand("CALL QB_Rollup_Update(@ActivityID)",
                        //                        new MySql.Data.MySqlClient.MySqlParameter("@ActivityID", act.ActivityID));

                    }
                }





            }

            return "Hello World!";
        }
    }
}
