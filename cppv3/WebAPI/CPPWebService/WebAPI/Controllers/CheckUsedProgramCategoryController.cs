using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class CheckUsedProgramCategoryController : ApiController
    {
        public HttpResponseMessage Get(int ProgramID, int ActivityCategoryID)
        {

            //List<ActivityCategory> MainActivityCategoryList = WebAPI.Models.ActivityCategory.getMainCategoryByProgram(ProgramID, Phase);

            String result = checkUsedCategory(ProgramID, ActivityCategoryID);
            var jsonNew = new
            {
                result = result
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }

        private string checkUsedCategory(int ProgramID, int ActivityCategoryID)
        {
            var status = "false";

            using (var ctx = new CPPDbContext())
            {
                try
                {
                    Program program = ctx.Program.Where(a => a.ProgramID == ProgramID).FirstOrDefault();
                    List<Project> projectList = ctx.Project.Where(p => p.ProgramID == ProgramID).ToList();
                    var activityCategory = ctx.ActivityCategory.Where(a=>a.ID == ActivityCategoryID).FirstOrDefault();
                   var budgetCat = activityCategory.CategoryDescription;
                    var subBudgetCat = activityCategory.SubCategoryDescription;
                    
                    foreach (var project in projectList)
                    {
                     var projectId = project.ProjectID;
                     List<Activity> activityList = ctx.Activity.Where(a => a.ProjectID == projectId &&
                                                         a.BudgetCategory == budgetCat && a.BudgetSubCategory == subBudgetCat).ToList();
                     if (activityList.Count != 0)
                     {
                         status = "true";
                         return status;
                     }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
            return status;
        }
    }
}
