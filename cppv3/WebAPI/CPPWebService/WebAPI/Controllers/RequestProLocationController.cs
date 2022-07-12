using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{

    public class RequestProLocationController : ApiController
    {
        //
        // GET: /Project/

        public HttpResponseMessage Get(String OrganizationID)
        {
            List<Project> ProjectList = PrjLocation.getProjectLocation(OrganizationID);

            var jsonNew = new
            {
                result = ProjectList
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }


    }

    public class PrjLocation
    {

        public static List<Project> getProjectLocation(String OrganizationID)
        {
            List<Project> projectList = new List<Project>();

            try
            {
                using (var ctx = new CPPDbContext())
                {
                    List<Program> programList = ctx.Program.Where(p => p.OrganizationID == OrganizationID).ToList();
                    List<ProgramElement> programElementList = new List<ProgramElement>();
                    //Get Program Element List
                    foreach (var program in programList)
                    {
                        //programElement for each program
                        List<ProgramElement> pmList = ctx.ProgramElement.Where(p => p.ProgramID == program.ProgramID).ToList();
                        foreach (var pm in pmList)
                        {
                            programElementList.Add(pm);
                        }
                    }

                    //Get Project List
                    foreach (var pm in programElementList)
                    {
                        List<Project> pList = ctx.Project.Where(proj => proj.ProgramElementID == pm.ProgramElementID).ToList();
                        foreach (var p in pList)
                        {
                            projectList.Add(p);

                        }
                    }

                    //find out the phase of the project
                    //foreach (var project in projectList)
                    //{
                    //    Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
                    //    Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
                    //    MySqlConnection conn = null;
                    //    MySqlDataReader reader = null;
                    //    String update_result = "";

                    //    try
                    //    {
                    //        conn = ConnectionManager.getConnection();
                    //        conn.Open();
                    //        //Check if program exists in system
                    //        String query = "SELECT ActivityID from activity";
                    //        query += " WHERE 1=1";
                    //        query += " AND ProjectID = " + project.ProjectID;//'" + FTECostIDList[i] + "'";
                    //        query += " And Granularity = week";
                    //        //query += " And"
                    //    }
                    //    catch(Exception ex)
                    //    {

                    //    }
                    //}


                }
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }

            return projectList;

        }
    }


}
