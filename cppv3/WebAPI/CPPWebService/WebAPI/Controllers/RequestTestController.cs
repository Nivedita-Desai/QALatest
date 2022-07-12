using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{

    public class RequestTestController : System.Web.Http.ApiController
    {
        //
        // GET: /RequestFTECost/
        public HttpResponseMessage Get()
        {
            List<Object> returnList = new List<Object>();

         //   List<FundType> FundTypeList = new List<FundType>();
            var ctx = new CPPDbContext();
            List<CurrentProjectTrend> ActivityList = new List<CurrentProjectTrend>();

            int ProjectID = 78;
            String granularity = "week";
            ActivityList = ctx.CurrentProjectTrend.Where(c => c.ProjectID == ProjectID && c.Granularity == granularity).
                            ToList();
            List<int> activityIdList = ActivityList.Select(a => a.ActivityID).Distinct().ToList();

            //1. Project
            Project project = ctx.Project.Where(a => a.ProjectID == ProjectID).FirstOrDefault();
            returnList.Add(project);
            //2. Phase List
            List<PhaseCode> phaseList = ctx.PhaseCode.ToList();
            returnList.Add(phaseList);
            //3. Activity List
            List<Activity> actList = new List<Activity>();
            foreach (var id in activityIdList)
            {
                foreach (var act in ActivityList)
                {
                    if (id == act.ActivityID)
                    {
                        Activity activity = new Activity();
                        activity.ActivityID = act.ActivityID;
                        activity.ProjectID = (int) act.ProjectID;
                        activity.TrendNumber = Convert.ToString(act.TrendNumber);
                        activity.PhaseCode = (int) act.PhaseCode;
                        activity.BudgetCategory = act.BudgetCategory;
                        activity.BudgetSubCategory = act.BudgetSubCategory;
                        activity.ActivityStartDate = act.ActivityStartDate;
                        activity.ActivityEndDate = act.ActivityEndDate;
                        activity.FTECost = (Double) act.FTECost;
                        activity.LumpsumCost = (Double) act.LumpsumCost;
                        activity.UnitCost = (Double)act.UnitCost;
                        activity.PercentageBasisCost = (Double)act.PercentageBasisCost;
                        activity.OrganizationID = (int)act.OrganizationID;
                        activity.BudgetID = (int)act.BudgetID;
                        activity.TrendID = (int)act.TrendID;

                        actList.Add(activity);
                        break;

                    }
                }
            }
            returnList.Add(actList);

            //4. Costs List
            List<Cost> costList = new List<Cost>();
            foreach(var id in activityIdList){

         
                
                var intId = Convert.ToInt16(id);
              
                //CostList by Activity
                List<CurrentProjectTrend> currentActivityList = new List<CurrentProjectTrend>();
                currentActivityList = ActivityList.Where(a => a.ActivityID == intId).ToList(); 

                //Get FTECost
                List<CurrentProjectTrend> fteCostList = new List<CurrentProjectTrend>();
                fteCostList = currentActivityList.Where(a => a.CostType == "f").ToList();

                //Lumpsum Cost List
                List<CurrentProjectTrend> lumpsumCostList = new List<CurrentProjectTrend>();
                lumpsumCostList = currentActivityList.Where(a => a.CostType == "l").ToList();

                //Unit Cost List
                List<CurrentProjectTrend> unitCostList = new List<CurrentProjectTrend>();
                unitCostList = currentActivityList.Where(a => a.CostType == "u").ToList();

                //filter FTE Cost
                List<String> fteCostIdList = new List<String>();
                List<String> rawFteCostIDList = fteCostList.Select(a => a.CostRowID).ToList();


                foreach (var costId in rawFteCostIDList)
                {
                    fteCostIdList.Add(costId.Split('_')[1]);
                }
                List<String> uniqueFteCostIdList = fteCostIdList.Distinct().ToList();

                foreach (var uId in uniqueFteCostIdList)
                {
                    Cost cost = new Cost();
                    var index = 0;
                    var isFound = false;
                    //Init
                    List<String> endDates = new List<String>();
                    List<String> FTECost = new List<String>();
                    List<String> FTEHours = new List<String>();
                    List<String> startDates = new List<String>();
                    List<String> textBoxIds = new List<String>();
                    List<String> textBoxValues = new List<String>();
                    foreach (var fte in fteCostList )
                    {
                        if (fte.CostRowID.Split('_')[1] == uId && fte.CostType=="f")
                        {
                            cost.ActivityID = Convert.ToString(fte.ActivityID);
                            cost.Base = fte.CostMultiplier;
                           // cost.CostID
                            cost.CostType = fte.CostType;
                            cost.Description = fte.CostDescription;
                            endDates.Add(fte.CostEndDate);
                            //FTE POSITION ID
                            FTECost.Add(fte.CostTotal);
                            FTEHours.Add(fte.FTECostDays);
                            startDates.Add(fte.CostStartDate);
                            textBoxIds.Add(Convert.ToString(index));
                            textBoxValues.Add(Convert.ToString(fte.CostBoxValue));
                            isFound = true;


                        }
                    }
                    if (isFound == true)
                    {
                        cost.EndDate = String.Join(", ", endDates);
                        cost.FTECost = String.Join(", ", FTECost);
                        cost.FTEHours = String.Join(", ", FTEHours);
                        cost.StartDate = String.Join(", ", startDates);
                        cost.TextBoxID = String.Join(", ", textBoxIds);
                        cost.TextBoxValue = String.Join(", ", textBoxValues);
                        costList.Add(cost);
                    }

                }
            
        
                //CostID
      
            }
            returnList.Add(costList);

            var jsonNew = new
            {
                result = returnList
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}
