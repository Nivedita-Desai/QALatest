using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json.Linq;

using WebAPI.Models;
using System.Diagnostics;
//using System.Web.Script.Serialization;


namespace WebAPI.Controllers
{
    [Authorize]
    public class RequestWBSController : System.Web.Http.ApiController
    {
        //
        // GET: /RequestWBS/
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public HttpResponseMessage Get(int uId, String OrganizationID = "null", String ProgramID = "null", String ProgramElementID = "null", String ProjectID = "null", String TrendNumber = "null", String PhaseCode = "null", String ActivityID = "null", String BudgetCategory = "null", String BudgetSubCategory = "null",
               string SearchText="null", string AllData="null", string DeptID = "null")
        {
            /*
            List<ProgramWBS> WBSList = new List<ProgramWBS>();
            WBSList = WebAPI.Models.ProgramWBS.getWBSDetails(ProgramID, ProgramElementID, ProjectID, TrendNumber, PhaseCode, ActivityID, BudgetCategory, BudgetSubCategory);
            */
            List<ProjectAccessControl> projectAccessControlsList = ProjectAccessControl.GetContractModificationList(uId);
            List<ProgramWBSTree> WBSListToAdd = new List<ProgramWBSTree>();
            List<ProgramWBSTree> WBSList = new List<ProgramWBSTree>();
            string organizationName = "";
            int organizationID = 0;
            List<Organization> orgs = Organization.getOrganization(OrganizationID, null);
            var ids = projectAccessControlsList.Select(x => x.ProjectID);
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();
            if (orgs.Count > 0)
            {
                Organization org = orgs.First<Organization>();
                organizationName = org.OrganizationName;
                organizationID = org.OrganizationID;
                 WBSList = WebAPI.Models.ProgramWBSTree.getWBSTreeDetails(uId, OrganizationID, ProgramID, ProgramElementID, ProjectID, TrendNumber, PhaseCode, ActivityID, BudgetCategory, BudgetSubCategory, AllData, DeptID);

                if (AllData == "0")
                {
                    WBSList = WBSList.Where(x => x.children.Any(y => y.children.Any(z => z.TotalUnapprovedTrends != "0"))).ToList();
                }

                //---------- Swapnil 24-09-2020-------------------------------------
                for (int i = 0; i < WBSList.Count; i++)
                {
                    if (WBSList[i].BillingPOCSpecialInstruction != null)
                    {
                        WBSList[i].BillingPOCSpecialInstruction = WBSList[i].BillingPOCSpecialInstruction.Replace("\"", "u022");
                    }
                }
               
            }

            //*******************************************************************************
            //Jignesh-22-10 - 2021 Quick Search in Program Navigation
            try
            {
                if (SearchText != "null")
                {
                    List<ProgramWBSTree> tempWBSList = WBSList;
                    WBSList = tempWBSList.Where(x => x.name.ToLower().Contains(SearchText.ToLower()) || x.ContractNumber.Contains(SearchText)).ToList();
                    foreach (ProgramWBSTree item in WBSList)
                    {
                        tempWBSList.Remove(item);
                    }
                    
                    for (int i = 0; i < tempWBSList.Count; i++)
                    {
                        List<ProgramElementWBSTree> tempPrgEleList = new List<ProgramElementWBSTree>();
                        var data = tempWBSList[i].children.Where(x => x.name.ToLower().Contains(SearchText.ToLower()) || x.ProgramElementNumber.Contains(SearchText)).ToList();
                        foreach (var item in data)
                        {
                            tempPrgEleList.Add(item);
                        }
                        if (tempPrgEleList.Count > 0)
                        {
                            tempWBSList[i].children = tempPrgEleList;
                            WBSList.Add(tempWBSList[i]);
                        }
                        else
                        {
                            List<ProgramElementWBSTree> tempProgEleList = new List<ProgramElementWBSTree>();
                            for (int x = 0; x < tempWBSList[i].children.Count; x++)
                            {
                                tempProgEleList.Add(tempWBSList[i].children[x]);
                            }
                            for (int j = 0; j < tempProgEleList.Count; j++)
                            {
                                var tempProjectList = tempProgEleList[j].children.Where(x => x.name.ToLower().Contains(SearchText.ToLower())).ToList();
                                if (tempProjectList.Count > 0)
                                {
                                    for (int c = 0; c < tempProgEleList.Count; c++)
                                    {
                                        tempWBSList[i].children.Remove(tempProgEleList[c]);
                                    }
                                    tempWBSList[i].children.Add(tempProgEleList[j]);
                                    
                                    tempWBSList[i].children[0].children = tempProjectList;
                                    WBSList.Add(tempWBSList[i]);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
            //*******************************************************************************
            //To pass JSONObject instead of String
            //String json = SerializeJSon<List<ProgramWBSTree>>(WBSList);
            //json = json.ToString().Replace("\\", "");
            //JObject jsonN = JObject.Parse("{ \"name\": \"" + organizationName + "\", \"organizationID\":\""+ organizationID +"\", \"level\": \"Root\",  \"children\": " + json + "}");
            //return Request.CreateResponse(HttpStatusCode.OK, jsonN);

            //---------- Swapnil 24-09-2020-------------------------------------
            String json = SerializeJSon<List<ProgramWBSTree>>(WBSList);
            json = json.ToString().Replace("\\", "");
            json = json.ToString().Replace("u022", "\\\"");
            JObject jsonN = JObject.Parse("{ \"name\": \"" + organizationName + "\", \"organizationID\":\"" + organizationID + "\", \"level\": \"Root\",  \"children\": " + json + "}");

            stopwatch.Stop();
            logger.Debug("Time Elapsed for retrieving mindmap - " + stopwatch.ElapsedMilliseconds);
            return Request.CreateResponse(HttpStatusCode.OK, jsonN);
            //-------------------------------------------------------------------
        }
        public static string SerializeJSon<T>(T t)
        {
            var resultString = "";
            try
            {
                MemoryStream stream = new MemoryStream();
                DataContractJsonSerializer ds = new DataContractJsonSerializer(typeof(T));
                DataContractJsonSerializerSettings s = new DataContractJsonSerializerSettings();
                ds.WriteObject(stream, t);
                string jsonString = System.Text.Encoding.UTF8.GetString(stream.ToArray());
                stream.Close();
                //jsonString = jsonString.Replace("\\", "");
                resultString = jsonString;
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return resultString;
        }
	}
}