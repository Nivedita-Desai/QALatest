using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Web;
using System.Web.Http;

using WebAPI.Models;
//using System.Web.Script.Serialization;
namespace WebAPI.Controllers
{
    [Authorize]
    public class RegisterLookupActivityCategoryController : System.Web.Http.ApiController
    {
        //
        // GET: /RegisterLookupActivityCategory/
        //public HttpResponseMessage Get(int Operation, String CategoryID, String CategoryDescription, String SubCategoryID, String SubCategoryDescription)
        public HttpResponseMessage Post([FromBody] List<ActivityCategory> act_category_list)
        {
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    int? orgId = act_category_list[0].OrganizationID;

                    Versionmaster oldVersion = ctx.VersionMaster.Where(s => s.OrganizationID == orgId)
                                                    .OrderByDescending(a => a.CreatedDate).FirstOrDefault();

                    Versionmaster CreatedNewVersion = new Versionmaster();
                    CreatedNewVersion.CreatedDate = DateTime.Now;
                    CreatedNewVersion.UpdatedDate = DateTime.Now;
                    int count = ctx.VersionMaster.Where(a => a.OrganizationID == orgId).ToList().Count();
                    CreatedNewVersion.description = "V" + (count + 1).ToString();
                    CreatedNewVersion.OrganizationID = orgId == null ? 0 : orgId;
                    ctx.VersionMaster.Add(CreatedNewVersion);
                    ctx.SaveChanges();

                    Versionmaster latestVersion = ctx.VersionMaster.Where(s => s.OrganizationID == orgId)
                                                    .OrderByDescending(a => a.CreatedDate).FirstOrDefault();

                    ctx.Database.ExecuteSqlCommand("call SpCreateNewWBSWithExistingData(@OrganizationID, @OldVesrionID, @NewVersionID)",
                                                    new MySql.Data.MySqlClient.MySqlParameter("@OrganizationID", orgId),
                                                    new MySql.Data.MySqlClient.MySqlParameter("@OldVesrionID", oldVersion.Id),
                                                    new MySql.Data.MySqlClient.MySqlParameter("@NewVersionID", latestVersion.Id));

                    //Versionmaster latestVersion = ctx.VersionMaster.Where(s => s.Id == 76).FirstOrDefault();


                    String status = "";

                    act_category_list = act_category_list.Where(s => s.Operation != 4).ToList();

                    int Success = 0;

                    foreach (var act_category in act_category_list)
                    {
                        if (act_category.Operation == 1)
                        {
                            status += WebAPI.Models.ActivityCategory.registerActivityCategory(act_category, latestVersion);
                            Success += act_category.SuccessTask; ; // Narayan - incriment success task - 14/06/2022
                        }
                        if (act_category.Operation == 2)
                        {
                            if (act_category.CategoryID == "null" && act_category.SubCategoryID == "null")  //CategoryID and SubcategoryID cannot be null
                                status += "Must pass CategoryID and SubcategoryID";
                            else //Update both category and subcategory
                                status += WebAPI.Models.ActivityCategory.updateActivityCategorySubCategory(act_category, latestVersion);
                            Success += act_category.SuccessTask; ; // Narayan - incriment success task - 14/06/2022
                            //status += WebAPI.Models.ActivityCategory.registerActivityCategory(act_category);

                        }

                        if (act_category.Operation == 3)
                        {
                            status += WebAPI.Models.ActivityCategory.deleteActivityCategory(act_category, latestVersion);
                            Success += act_category.SuccessTask; ; // Narayan - incriment success task - 14/06/2022
                        }
                        if (act_category.Operation == 4)
                            status += "";
                    }

                    Console.WriteLine(Success);

                    // Narayan - if none of multiple or single task Succeed then delete new version - 15/06/2022
                    if(Success == 0)
                    {
                        ctx.VersionMaster.Remove(latestVersion);
                        //List<ActivityCategory> deleteActivities = new List<ActivityCategory>();
                        //deleteActivities = ctx.ActivityCategory.Where(a => a.VersionId == latestVersion.Id).ToList();
                        //foreach (var activity in deleteActivities)
                        //    ctx.ActivityCategory.Remove(activity);
                        ctx.SaveChanges();
                        ctx.Database.ExecuteSqlCommand("call SpRemoveWBSByVersionId(@VersionID)",
                                                    new MySql.Data.MySqlClient.MySqlParameter("@VersionID", latestVersion.Id));
                        status += "Unable to create new version due to fail task.";
                    }

                    var jsonNew = new
                    {
                        result = status
                        //result = "New WBS has been saved successfully."
                    };

                    return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
                }
            }
            catch (Exception ex)
            {

                throw ex;

            }




        }
    }
}