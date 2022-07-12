using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.Reflection;
using System.Diagnostics;
using System.Data.OleDb;
using WebAPI.Controllers;
using MySql.Data.MySqlClient;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace WebAPI.Models
{
    [Table("trend")]
    public class Trend : Audit
    {
        [NotMapped]
        public int Operation;

        [NotMapped]
        public string UserID;

        public bool approvalFlag;

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TrendID { get; set; }
        public int ProjectID { get; set; }

        public string TrendNumber { get; set; }
        public string TrendDescription { get; set; }
        public string TrendJustification { get; set; }
        public string TrendImpact { get; set; }
        public string CreatedOn { get; set; }
        public string ApprovalFrom { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public DateTime? ClientApprovedDate { get; set; } // Jignesh-09-02-2021
        public DateTime? PostTrendStartDate { get; set; }
        public DateTime? PostTrendEndDate { get; set; }
        public DateTime? PreTrendStartDate { get; set; }
        public DateTime? PreTrendEndDate { get; set; }
        public string PreTrendCost { get; set; }
        public string PostTrendCost { get; set; }
        public string TrendCost { get; set; }

        public int? IsChangeRequest { get; set; } // Jignesh 30-10-2020
        public int? ChangeOrderID { get; set; } // Jignesh 30-10-2020
        public int? IsApprovedByClient { get; set; } // Jignesh 31-12-2020

        public string TrendImpactSchedule { get; set; }  //Manasi 13-07-2020
        public string TrendImpactCostSchedule { get; set; }   //Manasi 13-07-2020
        //aproval process
        public string CurrentThreshold { get; set; }
        public int? CurrentApprover_EmployeeID { get; set; }
        [ForeignKey("CurrentApprover_EmployeeID")]
        public Employee CurrentApprover { get; set; }

        public string LastApprover_UserID { get; set; } // Swapnil 18-09-2020

        public String approvedList_EmployeeID { get; set; }
        public String original_approvalList_EmployeeID { get; set; }
        public String original_approvalList_Role { get; set; }

        public int? TrendStatusCodeID { get; set; }

        public int? IsInternal { get; set; }

        [ForeignKey("TrendStatusCodeID")]
        public TrendStatusCode TrendStatusCode { get; set; }

        public int TrendStatusID { get; set; }
        [ForeignKey("TrendStatusID")]
        public TrendStatus TrendStatus { get; set; }

        [ForeignKey("ProjectID")]
        public Project Project { get; set; }

        public int CostOverheadTypeID { get; set; }
        [ForeignKey("CostOverheadTypeID")]
        public CostOverheadType CostOverheadType { get; set; }

        public DateTime? NextApproverEmailDate { get; set; }

        //Nivedita 10022022
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string DeletedBy { get; set; }
        public string Status { get; set; }   //----Vaishnavi 30-03-2022----//

        //From RequestProgramElementController
        public static List<Trend> getTrend(String ProgramID, String ProgramElementID, String ProjectID, String TrendNumber, String KeyStroke)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            List<Trend> MatchedTrendList = new List<Trend>();

            try
            {

                using (var ctx = new CPPDbContext())
                {

                    ctx.Database.Log = msg => Trace.WriteLine(msg);
                    if (TrendNumber != "null")
                    {
                        string trendNum = TrendNumber;
                        IQueryable<Trend> trends = ctx.Trend.Include("Project").Include("TrendStatus").Where(p => p.TrendNumber == trendNum && p.IsDeleted == false);//Do not display Current Trend
                        MatchedTrendList = trends.ToList<Trend>();
                    }
                    else if (ProjectID != "null")
                    {
                        int projId = int.Parse(ProjectID);
                        IQueryable<Trend> trends = ctx.Trend.Include("Project").Include("TrendStatus").Where(p => p.Project.ProjectID == projId && p.TrendNumber != "1000" && p.IsDeleted == false);//Do not display Current Trend
                        MatchedTrendList = trends.ToList<Trend>();
                    }
                    else if (ProgramElementID != "null")
                    {
                        int pgmEltId = int.Parse(ProgramElementID);
                        IQueryable<Trend> trends = ctx.Trend.Include("Project").Include("TrendStatus").Where(p => p.Project.ProgramElement.ProgramElementID == pgmEltId && p.TrendNumber != "1000" && p.IsDeleted == false);//Do not display Current Trend
                        MatchedTrendList = trends.ToList<Trend>();
                    }
                    else if (ProgramID != "null")
                    {
                        int pgmId = int.Parse(ProgramID);
                        IQueryable<Trend> trends = ctx.Trend.Include("Project").Include("TrendStatus").Where(p => p.Project.Program.ProgramID == pgmId && p.TrendNumber != "1000" && p.IsDeleted == false);//Do not display Current Trend
                        MatchedTrendList = trends.ToList<Trend>();

                    }
                    else if (KeyStroke != "null")
                    {
                        IQueryable<Trend> trends = ctx.Trend.Where(p => p.TrendDescription.Contains(KeyStroke) && p.IsDeleted == false);
                        MatchedTrendList = trends.ToList<Trend>();
                    }
                    else
                    {
                        IQueryable<Trend> trends = ctx.Trend.Include("Project").Include("TrendStatus").Where(p => p.IsDeleted == false);
                        MatchedTrendList = trends.ToList<Trend>();
                    }

                }


            }
            catch (Exception ex)
            {
                var stackTrace = new StackTrace(ex, true);
                var line = stackTrace.GetFrame(0).GetFileLineNumber();
                Logger.LogExceptions(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, ex.Message, line.ToString(), Logger.logLevel.Exception);
            }
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Exit Point", Logger.logLevel.Debug);
            return MatchedTrendList;

        }

        //GetMaxFutureDate ---Not used
        public static String getMaxFutureDate(String projectId)
        {
            String maxDate = "";
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            Trend matchedTrend = new Trend();
            MySqlConnection conn = null;
            MySqlDataReader reader = null;
            try
            {

                //var pId = Convert.ToInt16(projectId);
                //using (var ctx = new CPPDbContext())
                //{
                //    List<Trend> trends = ctx.Trend.Where(t => t.ProjectID == pId).ToList();
                //    maxDate = trends.Max(d => d.PostTrendEndDate).ToString();
                //}
                // create and open a connection object
                conn = ConnectionManager.getConnection();
                conn.Open();

                String query = "SELECT Max(PostTrendEndDate) FROM trend";
                query += " WHERE 1=1";

                query += " And ProjectID = @projectId";


                MySqlCommand command = new MySqlCommand(query, conn);
                command.Parameters.AddWithValue("@projectId", projectId);

                using (reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var maximumEndDate = reader.GetValue(0).ToString().Trim();
                        if (maximumEndDate != "")
                        {
                            maxDate = maximumEndDate;
                        }
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
                if (conn != null)
                {
                    conn.Close();
                }
                if (reader != null)
                {
                    reader.Close();
                }
            }
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Exit Point", Logger.logLevel.Debug);

            return maxDate;
        }

        //Get startDate, EndDate, and totalCost of Forecast Trend
        public static List<String> getForeCostTrendDate(int projectID)
        {
            List<String> listDate = new List<String>();
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            Trend matchedTrend = new Trend();
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    List<Trend> trends = ctx.Trend.Where(t => t.ProjectID == projectID).ToList();
                    var minimumStartDate = trends.Min(m => m.PostTrendStartDate).ToString();
                    var maximumEndDate = trends.Max(m => m.PostTrendEndDate).ToString();
                    var totalCost = trends.Sum(m => Convert.ToDouble(m.PostTrendCost));

                    if (minimumStartDate != "")
                    {
                        listDate.Add(minimumStartDate);
                    }
                    if (maximumEndDate != "")
                    {
                        listDate.Add(maximumEndDate);
                    }
                    listDate.Add(totalCost.ToString().Trim());
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
            }
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Exit Point", Logger.logLevel.Debug);

            return listDate;
        }

        //Get startDate, EndDate, and totalCost of CurrentTrend
        public static List<String> getCurrentTrendDate(String projectId)
        {
            List<String> listDate = new List<String>();

            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            Trend matchedTrend = new Trend();
            try
            {
                var pId = Convert.ToInt16(projectId);
                using (var ctx = new CPPDbContext())
                {
                    List<Trend> trends = ctx.Trend.Where(t => t.ProjectID == pId && t.TrendStatusID == 1).ToList();
                    var start_date = trends.Min(m => m.PostTrendStartDate).ToString().Trim();
                    var end_date = trends.Max(m => m.PostTrendEndDate).ToString().Trim();
                    var total_cost = trends.Sum(m => Convert.ToDouble(m.PostTrendCost));

                    if (start_date != "")
                        listDate.Add(start_date);
                    if (end_date != "")
                        listDate.Add(end_date);
                    listDate.Add(total_cost.ToString());
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
            }
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Exit Point", Logger.logLevel.Debug);

            return listDate;
        }

        public static TrendCostOverhead crateCustomTrendCostOverhead(Trend trend)
        {
            TrendCostOverhead trendCostOverHead = new TrendCostOverhead();
            List<CostOverhead> costOverHeads = null;
            // costOverHeads = ctx.CostOverhead.Where(a => a.StartDate < currentDate && a.EndDate > currentDate).ToList();
            using (var ctx = new CPPDbContext())
            {
                costOverHeads = CostOverhead.getCustomCostOverhead();

                saveTrendCostOverhead(trend, costOverHeads);
            }


            return trendCostOverHead;
        }
        public static void saveTrendCostOverhead(Trend trend, List<CostOverhead> costOverheads)
        {
            using (var ctx = new CPPDbContext())
            {
                foreach (var co in costOverheads)
                {
                    TrendCostOverhead trendOverhead = new TrendCostOverhead()
                    {
                        TrendID = trend.TrendID,
                        CostOverheadID = co.ID,
                        Description = co.Description,
                        CurrentMarkup = co.Markup,
                        isActive = true,
                        Justification = null,
                        CreatedBy = "System",
                        UpdatedBy = "System",
                        CreatedDate = DateTime.Now,
                        UpdatedDate = DateTime.Now,
                        OriginalCost = 0,
                        CurrentCost = 0,
                        NewCost = 0
                    };
                    ctx.TrendCostOverhead.Add(trendOverhead);
                    ctx.SaveChanges();
                }
            }
        }

        //============================== Jignesh-09-04-2021 ================================
        public static void saveTrendCostOverheadFromBaseline(Trend trend, List<TrendCostOverhead> costOverheads)
        {
            using (var ctx = new CPPDbContext())
            {
                foreach (var co in costOverheads)
                {
                    TrendCostOverhead trendOverhead = new TrendCostOverhead()
                    {
                        TrendID = trend.TrendID,
                        CostOverheadID = co.CostOverheadID,
                        Description = co.Description,
                        CurrentMarkup = co.CurrentMarkup,
                        isActive = true,
                        Justification = co.Justification,
                        CreatedBy = "System",
                        UpdatedBy = "System",
                        CreatedDate = DateTime.Now,
                        UpdatedDate = DateTime.Now,
                        OriginalCost = 0,
                        CurrentCost = 0,
                        NewCost = 0
                    };
                    ctx.TrendCostOverhead.Add(trendOverhead);
                    ctx.SaveChanges();
                }
            }
        }
        //=====================================================================================
        public static String registerTrend(Trend trend)
        {
            String result = "";
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    Trend currentTrend = trend;
                    CostOverheadType overHeadType = ctx.CostOverheadType.Where(a => a.ID == trend.CostOverheadTypeID).FirstOrDefault();

                    var isBillableLabor = (overHeadType.CostOverHeadType == "Billable Rate") ? true : false;
                    ctx.Database.Log = msg => Trace.WriteLine(msg);
                    // trend.CostOverheadTypeID = 1;//IVAN 03-12
                    trend.Status = "Active";   //----Vaishnavi 30-03-2022----//
                    ctx.Trend.Add(trend);
                    ctx.SaveChanges();
                    currentTrend = ctx.Trend.AsNoTracking().Where(a => a.TrendID == trend.TrendID).FirstOrDefault();
                    currentTrend.CurrentThreshold = "0";
                    /*
                        Name : Jignesh
                        Date : 09-04-2021
                        Description : Below section is commented because now 
                                      requirement is that to use baseline's customcostoverhead for trend
                     */
                    //========================== Commented by Jignesh-09-04-2021 ==============================================
                    //Costoverhead
                    DateTime currentDate = DateTime.Now;
                    List<CostOverhead> costOverHeads = null;
                    //costOverHeads = ctx.CostOverhead.Where(a => a.StartDate < currentDate && a.EndDate > currentDate).ToList();
                    costOverHeads = CostOverhead.getActiveCostOverhead(); // Commented by Jignesh-09-04-2021
                    //================= Jignesh-09-04-2021 =======================
                    if (currentTrend.TrendNumber == "0")
                    {
                        saveTrendCostOverhead(trend, costOverHeads);
                    }
                    //============================================================
                    //Create Custom Cost Overhead
                    List<CostOverhead> customCostOverheads = CostOverhead.getCustomCostOverhead(); // Commented by Jignesh-09-04-2021
                    foreach (CostOverhead custom in customCostOverheads)
                    {
                        CostType costType = ctx.CostType.Where(a => a.ID == custom.CostTypeID).FirstOrDefault();
                        foreach (CostOverhead admin in costOverHeads)
                        {

                            if (custom.CostTypeID == admin.CostTypeID)
                            {
                                //if (costType.Type == "Labor")
                                //{
                                //    custom.Markup = (isBillableLabor) ? 1 : admin.Markup;
                                //}
                                //else
                                //{
                                //    custom.Markup = admin.Markup;
                                //}
                                custom.Markup = admin.Markup;    //Manasi 28-07-2020
                            }
                        }
                    }
                    //================= Jignesh-09-04-2021 =======================
                    if (currentTrend.TrendNumber == "0")
                    {
                        saveTrendCostOverhead(currentTrend, customCostOverheads);
                    }
                    //============================================================

                    //======================================================================================================================
                    //========================== Jignesh-09-04-2021 =====================================================
                    if (currentTrend.TrendNumber != "0")
                    {
                        List<TrendCostOverhead> baseLineCostOverheads = null;
                        Trend trendCO = ctx.Trend.Where(a => a.ProjectID == trend.ProjectID && a.TrendNumber == "0").FirstOrDefault();
                        if (trendCO != null)
                        {
                            baseLineCostOverheads = ctx.TrendCostOverhead
                                                    .Include("CostOverhead")
                                                    .Include("CostOverhead.CostRateType")
                                                    .Include("CostOverhead.CostType")
                                                    .Where(a => a.TrendID == trendCO.TrendID)
                                                    .ToList();
                        }
                        saveTrendCostOverheadFromBaseline(currentTrend, baseLineCostOverheads);
                    }
                    //===================================================================================================
                    // crateCustomTrendCostOverhead(trend);

                    //Create a current 
                    if (trend.TrendDescription.ToLower() == "baseline" && trend.TrendNumber == "0")
                    {

                        currentTrend.TrendNumber = "1000";
                        currentTrend.TrendDescription = "Current";
                        currentTrend.TrendStatusID = 1;
                        currentTrend.TrendID = 0;
                        currentTrend.CostOverheadTypeID = 1;//IVAN 03-12
                        ctx.Trend.Add(currentTrend);
                        ctx.SaveChanges();


                        saveTrendCostOverhead(currentTrend, costOverHeads);
                        customCostOverheads = CostOverhead.getCustomCostOverhead();
                        foreach (CostOverhead custom in customCostOverheads)
                        {
                            CostType costType = ctx.CostType.Where(a => a.ID == custom.CostTypeID).FirstOrDefault();
                            foreach (CostOverhead admin in costOverHeads)
                            {
                                if (custom.CostTypeID == admin.CostTypeID)
                                {
                                    //if (costType.Type == "Labor")
                                    //{
                                    //    custom.Markup = (isBillableLabor) ? 1 : admin.Markup;
                                    //}
                                    //else
                                    //{
                                    //    custom.Markup = admin.Markup;
                                    //}
                                    custom.Markup = admin.Markup;   //Manasi 28-07-2020
                                }
                            }
                        }

                        saveTrendCostOverhead(currentTrend, customCostOverheads);
                        //  crateCustomTrendCostOverhead(currentTrend);
                        //foreach (var co in costOverHeads)
                        //{
                        //    TrendCostOverhead trendOverhead = new TrendCostOverhead()
                        //    {
                        //        TrendID = currentTrend.TrendID,
                        //        CostOverheadID = co.ID,
                        //        Description = co.Description,
                        //        CurrentMarkup = co.Markup,
                        //        isActive = true,
                        //        Justification = null,
                        //        CreatedBy = "System",
                        //        UpdatedBy = "System",
                        //        CreatedDate = DateTime.Now,
                        //        UpdatedDate = DateTime.Now
                        //    };
                        //    ctx.TrendCostOverhead.Add(trendOverhead);
                        //    ctx.SaveChanges();
                        //}
                    }



                    result = "Success";
                }
            }
            catch (Exception ex)
            {
                var stackTrace = new StackTrace(ex, true);
                var line = stackTrace.GetFrame(0).GetFileLineNumber();
                Logger.LogExceptions(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, ex.Message, line.ToString(), Logger.logLevel.Exception);
            }
            return result;
        }

        public static String updateTrendSetup(Trend trend)
        {
            String result = "";
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    Trend tr = new Trend();
                    ctx.Database.Log = msg => Trace.WriteLine(msg);
                    tr = ctx.Trend.First(p => p.ProjectID == trend.ProjectID && p.TrendNumber == trend.TrendNumber);

                    if (tr != null)
                    {
                        tr.TrendStatusID = trend.TrendStatusID;
                        tr.TrendNumber = trend.TrendNumber;
                        tr.TrendDescription = trend.TrendDescription;
                        tr.TrendJustification = trend.TrendJustification;
                        tr.TrendImpact = trend.TrendImpact;
                        tr.TrendImpactSchedule = trend.TrendImpactSchedule;   //Manasi 13-07-2020
                        tr.TrendImpactCostSchedule = trend.TrendImpactCostSchedule;  //Manasi 13-07-2020
                        tr.ChangeOrderID = trend.ChangeOrderID;
                        tr.IsChangeRequest = trend.IsChangeRequest;
                        tr.CreatedOn = trend.CreatedOn;
                        tr.ApprovalFrom = trend.ApprovalFrom;
                        tr.ApprovalDate = trend.ApprovalDate;
                        tr.TrendStatusCodeID = trend.TrendStatusCodeID;
                        tr.IsInternal = trend.IsInternal;
                        tr.IsApprovedByClient = trend.IsApprovedByClient; // Jignesh 31-12-2020
                        tr.CostOverheadTypeID = trend.CostOverheadTypeID;
                        tr.ClientApprovedDate = trend.ClientApprovedDate; // Jignesh-09-02-2021
                        //tr.PostTrendStartDate = trend.PostTrendStartDate;
                        //tr.PostTrendEndDate = trend.PostTrendEndDate;

                        ctx.SaveChanges();

                        result = "Success";
                    }
                    else
                    {
                        result = "Unable to find trend.";
                    }
                }
            }
            catch (Exception ex)
            {
                var stackTrace = new StackTrace(ex, true);
                var line = stackTrace.GetFrame(0).GetFileLineNumber();
                Logger.LogExceptions(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, ex.Message, line.ToString(), Logger.logLevel.Exception);
            }
            return result;
        }

        public static String updateTrendDate(Trend trend)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);

            String update_result = "";
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    Trend retrievedTrend = ctx.Trend.Where(t => t.TrendNumber == trend.TrendNumber && t.ProjectID == trend.ProjectID).FirstOrDefault();
                    if (retrievedTrend != null)
                    {
                        //Update Date
                        retrievedTrend.PostTrendStartDate = trend.PostTrendStartDate;
                        retrievedTrend.PostTrendEndDate = trend.PostTrendEndDate;

                        ctx.Entry(retrievedTrend).State = System.Data.Entity.EntityState.Modified;
                        ctx.SaveChanges();
                    }
                    else
                    {
                        //Trend not found
                        update_result = "The Trend cannot be found";
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
            }
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Exit Point", Logger.logLevel.Debug);

            return update_result;
        }

        //Luan here - approval process when given statusID of 5 - Submit Button
        public static String submitForApproval(Trend trend)
        {
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    //User only applicable roles
                    //List<String> applicableRoleList = new List<string>(new string[] { "Project Manager", "Director", "Scheduler", "Vice President", "Financial Analyst", "Capital Project Assistant" });

                    List<String> applicableRoleList = new List<string>();
                    applicableRoleList.AddRange(ctx.ApprovalMatrix.Select(p => p.Role).ToList());

                    Trend tr = ctx.Trend.First(p => p.Project.ProjectID == trend.ProjectID && p.TrendNumber == trend.TrendNumber);  //Get trend from db?
                    Project pr = ctx.Project.First(p => p.ProjectID == trend.ProjectID);    //Get associated project
                    String original_approvalList_EmployeeID = "";
                    String original_approvalList_Role = "";
                    String tbd_approvers = "";  //Swapnil 09-09-2020
                    if (tr != null)
                    {
                        //Where we find the first approver
                        List<ApprovalMatrix> approvalMatrixList = ApprovalMatrix.getApprovalMatrix();

                        //We know the current threshold is 0
                        String lowestApproverCost = "99999999999";
                        String lowestApproverRole = "";
                        int lowestApprovalMatrixId = 0;
                        bool foundNextApprover = false; ///assume there is always one
                        int nextApproverEmployeeID = 10000;

                        lowestApproverCost = approvalMatrixList.Min(p => p.Cost).ToString();    //assume the lowest will work

                        //test
                        decimal trendCost = Convert.ToDecimal(trend.TrendCost);
                        approvalMatrixList = ctx.ApprovalMatrix.Where(p => p.Cost <= trendCost && applicableRoleList.Contains(p.Role)).OrderBy(p => p.Cost).ToList();
                        List<TrendApproversDetails> elementApproverDetails = ctx.TrendApproversDetails.Where(p=>p.ProjectElementId == trend.ProjectID).ToList();
                        
                        bool firstAppend = true;

                        for (int x = 0; x < approvalMatrixList.Count; x++)
                        {
                            if (Convert.ToDouble(approvalMatrixList[x].Cost) == Convert.ToDouble(lowestApproverCost))
                            {
                                lowestApproverRole = approvalMatrixList[x].Role;

                                lowestApprovalMatrixId = approvalMatrixList[x].Id;

                                foundNextApprover = true;

                                //break;
                            }

                            //test
                            int employeeId = 0;
                            //for (int i = 0; i < elementApproverDetails.Count; i++)
                            //{

                            //    if (approvalMatrixList[x].Id == elementApproverDetails[i].Id) {

                            //        employeeId = elementApproverDetails[i].EmpId;

                            //        break;
                            //    }

                            //}

                            employeeId = elementApproverDetails.Where(p => p.ApproverMatrixId == approvalMatrixList[x].Id).Select(p => p.EmpId).FirstOrDefault();



                                //if (approvalMatrixList[x].Role == "Project Manager")
                                //    employeeId = pr.ProjectManagerID;
                                //if (approvalMatrixList[x].Role == "Director")
                                //    employeeId = pr.DirectorID;
                                //if (approvalMatrixList[x].Role == "Scheduler")
                                //    employeeId = pr.SchedulerID;
                                //if (approvalMatrixList[x].Role == "Vice President")
                                //    employeeId = pr.VicePresidentID;
                                //if (approvalMatrixList[x].Role == "Financial Analyst")
                                //    employeeId = pr.FinancialAnalystID;
                                //if (approvalMatrixList[x].Role == "Capital Project Assistant")
                                //    employeeId = pr.CapitalProjectAssistantID;



                            if (employeeId != 0)
                            {
                                if (firstAppend)
                                {
                                    //-------------- //Swapnil 09-09-2020 ---------------------------------
                                    Employee tbdApproverDetails = ctx.Employee.First(p => p.ID == employeeId);
                                    if (tbdApproverDetails.Name.Contains("TBD"))
                                    {
                                        tbd_approvers += approvalMatrixList[x].Role;
                                    }
                                    //-----------------------------------------------------------------------
                                    original_approvalList_EmployeeID += employeeId.ToString();
                                    original_approvalList_Role += approvalMatrixList[x].Role;
                                    firstAppend = false;
                                }
                                else
                                {
                                    //-------------- //Swapnil 09-09-2020 ---------------------------------
                                    Employee tbdApproverDetails = ctx.Employee.First(p => p.ID == employeeId);
                                    if (tbdApproverDetails.Name.Contains("TBD"))
                                    {
                                        if (tbd_approvers != "")
                                        {

                                            tbd_approvers += "," + approvalMatrixList[x].Role;

                                        }
                                        else
                                        {
                                            tbd_approvers += approvalMatrixList[x].Role;
                                        }
                                        
                                    }
                                    //-----------------------------------------------------------------------
                                    original_approvalList_EmployeeID += " " + employeeId.ToString();
                                    original_approvalList_Role += "|" + approvalMatrixList[x].Role;
                                }
                            }

                        }
                        //-------------- //Swapnil 09-09-2020 ---------------------------------
                        if (tbd_approvers != "")
                        {
                            return tbd_approvers + " set to TBD. \n Please update the details in the Element Setup page and submit the trend again.";
                        }
                        //-----------------------------------------------------------------------
                        //for (int i = 0; i < elementApproverDetails.Count; i++)
                        //{

                        //    if (lowestApprovalMatrixId == elementApproverDetails[i].Id)
                        //    {

                        //        nextApproverEmployeeID = elementApproverDetails[i].EmpId;

                        //        break;
                        //    }

                        //}

                        
                        nextApproverEmployeeID = elementApproverDetails.Where(p => p.ApproverMatrixId == lowestApprovalMatrixId).Select(p => p.EmpId).FirstOrDefault();

                        if (nextApproverEmployeeID == 0)
                        {
                            string nextApproverName = approvalMatrixList.Where(a => a.Id == lowestApprovalMatrixId).Select(a => a.Role).FirstOrDefault();
                            return nextApproverName + " set to TBD. \n Please update the details in the Element Setup page and submit the trend again.";
                        }

                        //if (lowestApproverRole == "Project Manager")
                        //    nextApproverEmployeeID = pr.ProjectManagerID;
                        //if (lowestApproverRole == "Director")
                        //    nextApproverEmployeeID = pr.DirectorID;
                        //if (lowestApproverRole == "Scheduler")
                        //    nextApproverEmployeeID = pr.SchedulerID;
                        //if (lowestApproverRole == "Vice President")
                        //    nextApproverEmployeeID = pr.VicePresidentID;
                        //if (lowestApproverRole == "Financial Analyst")
                        //    nextApproverEmployeeID = pr.FinancialAnalystID;
                        //if (lowestApproverRole == "Capital Project Assistant")
                        //    nextApproverEmployeeID = pr.CapitalProjectAssistantID;

                        //We know the statusID is 5, then it will change to 3 for pending
                        tr.TrendStatusID = 3;

                        //Set the first approver employee id after we figure it out.
                        tr.CurrentApprover_EmployeeID = nextApproverEmployeeID;

                        tr.CurrentThreshold = "0";

                        tr.TrendCost = trend.TrendCost;

                        tr.original_approvalList_EmployeeID = original_approvalList_EmployeeID;

                        tr.original_approvalList_Role = original_approvalList_Role;

                        tr.NextApproverEmailDate = DateTime.Now.Date; // Swapnil 18/09/2020

                        //Get associated employee
                        User requestingUser = ctx.User.First(p => p.UserID == trend.UserID);  //Get associated requesting user
                        
                        User targetedUser = ctx.User.First(p => p.EmployeeID == nextApproverEmployeeID);  //Get associated targeted user
                        Employee targetedEmployee = ctx.Employee.First(p => p.ID == nextApproverEmployeeID);

                        tr.LastApprover_UserID = requestingUser.UserID; // Swapnil 18/09/2020

                        ctx.SaveChanges();

                        WebAPI.Services.MailServices.SendApprovalEmail(requestingUser.FirstName + " " + requestingUser.LastName,
                                                                        targetedUser.FirstName + " " + targetedUser.LastName,
                                                                        "Admin", trend.TrendNumber.ToString(), pr.ProjectID.ToString(), targetedUser.Email);

                        return "Submission for approval is successful. \n" + lowestApproverRole + ", " + targetedUser.FirstName + " " + targetedUser.LastName + " will be notified for approval.";
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.InnerException.ToString();
                var stackTrace = new StackTrace(ex, true);
                var line = stackTrace.GetFrame(0).GetFileLineNumber();
                Logger.LogExceptions(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, ex.Message, line.ToString(), Logger.logLevel.Exception);
            }

            return "";
        }

        //Luan here - approval process when given statusID of 3 - Approve Button
        public static String singleApprove(Trend trend)
        {
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    //User only applicable roles
                    //List<String> applicableRoleList = new List<string>(new string[] { "Project Manager", "Director", "Scheduler", "Vice President", "Financial Analyst", "Capital Project Assistant" });

                    List<String> applicableRoleList = new List<string>();
                    applicableRoleList.AddRange(ctx.ApprovalMatrix.Select(p => p.Role).ToList());
                    

                    Trend tr = ctx.Trend.First(p => p.Project.ProjectID == trend.ProjectID && p.TrendNumber == trend.TrendNumber);  //Get trend from db?
                    Project pr = ctx.Project.First(p => p.ProjectID == trend.ProjectID);    //Get associated project
                    User requestingUser = ctx.User.First(p => p.UserID == trend.UserID);  //Get associated requesting user
                    Employee er = ctx.Employee.First(p => p.ID == requestingUser.EmployeeID);   //Get associated employee

                    if (tr != null)
                    {
                        //Where we find the next approver
                        //If cannot find next -> Trend status will be 1 - officially approved
                        //If found next approver -> Keep the cycle going - still pending
                        List<ApprovalMatrix> approvalMatrixList = ApprovalMatrix.getApprovalMatrix();
                        List<TrendApproversDetails> elementApproverDetails = ctx.TrendApproversDetails.Where(p => p.ProjectElementId == trend.ProjectID).ToList();

                        //If this guy approve successfuly, then we change the threshold to what he is associated with.
                        String lowestApproverCost = "99999999999";
                        String lowestApproverRole = "";
                        int lowestApprovalMatrixId = 0;
                        bool foundNextApprover = false;
                        int nextApproverEmployeeID = 10000;

                        decimal trendCost = Convert.ToDecimal(trend.TrendCost);
                        decimal currentThreshold = Convert.ToDecimal(trend.CurrentThreshold);
                        approvalMatrixList = ctx.ApprovalMatrix.Where(p => p.Cost < trendCost &&
                                                                           p.Cost > currentThreshold &&
                                                                           applicableRoleList.Contains(p.Role)).OrderBy(p => p.Cost).ToList();


                        //To prevent limbo
                        if (approvalMatrixList.Count < 1)
                        {
                            tr.TrendStatusID = 1;
                            tr.NextApproverEmailDate = DateTime.Now.Date; // Swapnil 18/09/2020
                            tr.LastApprover_UserID = requestingUser.UserID; // Swapnil 18/09/2020
                            ctx.SaveChanges();
                            return "Official Approved";
                        }

                        lowestApproverCost = approvalMatrixList.Min(p => p.Cost).ToString();

                        for (int x = 0; x < approvalMatrixList.Count; x++)
                        {
                            if (Convert.ToDouble(approvalMatrixList[x].Cost) == Convert.ToDouble(lowestApproverCost))
                            {
                                lowestApproverRole = approvalMatrixList[x].Role;

                                lowestApprovalMatrixId = approvalMatrixList[x].Id;

                                foundNextApprover = true;

                                break;
                            }
                        }
                        nextApproverEmployeeID = elementApproverDetails.Where(p => p.ApproverMatrixId == lowestApprovalMatrixId).Select(p => p.EmpId).FirstOrDefault();

                        //if (lowestApproverRole == "Project Manager")
                        //    nextApproverEmployeeID = pr.ProjectManagerID;
                        //if (lowestApproverRole == "Director")
                        //    nextApproverEmployeeID = pr.DirectorID;
                        //if (lowestApproverRole == "Scheduler")
                        //    nextApproverEmployeeID = pr.SchedulerID;
                        //if (lowestApproverRole == "Vice President")
                        //    nextApproverEmployeeID = pr.VicePresidentID;
                        //if (lowestApproverRole == "Financial Analyst")
                        //    nextApproverEmployeeID = pr.FinancialAnalystID;
                        //if (lowestApproverRole == "Capital Project Assistant")
                        //    nextApproverEmployeeID = pr.CapitalProjectAssistantID;

                        //Invalid user trying to approve
                        if (er.ID != nextApproverEmployeeID)
                        {
                            return "Unauthorized. You are not the designated user to approve right now.";
                        }

                        //if (foundNextApprover && approvalMatrixList.Count > 1)
                        if (foundNextApprover && approvalMatrixList.Count >= 1)  //Manasi 31-07-2020
                        {
                            //Keep it in the cycle
                            tr.TrendStatusID = 3;

                            //Set the next approver employee id after we figure it out.
                            tr.CurrentApprover_EmployeeID = nextApproverEmployeeID;

                            //Set new trend threshold
                            tr.CurrentThreshold = lowestApproverCost;
                        }
                        else
                        {
                            //no one to approve - official approved
                            tr.TrendStatusID = 1;
                        }


                        if (tr.approvedList_EmployeeID == null)
                        {
                            tr.approvedList_EmployeeID = nextApproverEmployeeID.ToString();
                        }
                        else
                        {
                            tr.approvedList_EmployeeID += " " + nextApproverEmployeeID.ToString();
                        }

                        tr.NextApproverEmailDate = DateTime.Now.Date; // Swapnil 18/09/2020
                        tr.LastApprover_UserID = requestingUser.UserID; // Swapnil 18/09/2020
                        ctx.SaveChanges();

                        //It's over
                        if (tr.TrendStatusID == 1)
                        {
                            return "Official Approved";
                        }

                        //Need to find targeted user here
                        //**************************REDUNDANT CODE - TEMPORARY*******************************
                        decimal trendCostTargeted = Convert.ToDecimal(tr.TrendCost);
                        decimal currentThresholdTargeted = Convert.ToDecimal(tr.CurrentThreshold);
                        approvalMatrixList = ctx.ApprovalMatrix.Where(p => p.Cost < trendCostTargeted &&
                                                                           p.Cost > currentThresholdTargeted &&
                                                                           applicableRoleList.Contains(p.Role)).OrderBy(p => p.Cost).ToList();

                        //To prevent limbo
                        if (approvalMatrixList.Count < 1)
                        {
                            tr.TrendStatusID = 1;
                            tr.NextApproverEmailDate = DateTime.Now.Date; // Swapnil 18/09/2020
                            tr.LastApprover_UserID = requestingUser.UserID; // Swapnil 18/09/2020
                            ctx.SaveChanges();
                            return "Official Approved";
                        }

                        lowestApproverCost = approvalMatrixList.Min(p => p.Cost).ToString();

                        for (int x = 0; x < approvalMatrixList.Count; x++)
                        {
                            if (Convert.ToDouble(approvalMatrixList[x].Cost) == Convert.ToDouble(lowestApproverCost))
                            {
                                lowestApproverRole = approvalMatrixList[x].Role;

                                lowestApprovalMatrixId = approvalMatrixList[x].Id;

                                foundNextApprover = true;

                                break;
                            }
                        }

                        nextApproverEmployeeID = elementApproverDetails.Where(p => p.ApproverMatrixId == lowestApprovalMatrixId).Select(p => p.EmpId).FirstOrDefault();


                        //if (lowestApproverRole == "Project Manager")
                        //    nextApproverEmployeeID = pr.ProjectManagerID;
                        //if (lowestApproverRole == "Director")
                        //    nextApproverEmployeeID = pr.DirectorID;
                        //if (lowestApproverRole == "Scheduler")
                        //    nextApproverEmployeeID = pr.SchedulerID;
                        //if (lowestApproverRole == "Vice President")
                        //    nextApproverEmployeeID = pr.VicePresidentID;
                        //if (lowestApproverRole == "Financial Analyst")
                        //    nextApproverEmployeeID = pr.FinancialAnalystID;
                        //if (lowestApproverRole == "Capital Project Assistant")
                        //    nextApproverEmployeeID = pr.CapitalProjectAssistantID;

                        // As the current Empployee id was not getting updated pritesh 25 Aug 2020
                        Trend trUpd = ctx.Trend.First(p => p.Project.ProjectID == trend.ProjectID && p.TrendNumber == trend.TrendNumber);
                        trUpd.CurrentApprover_EmployeeID = nextApproverEmployeeID;
                        ctx.Entry(trUpd).State = System.Data.Entity.EntityState.Modified;
                        tr.NextApproverEmailDate = DateTime.Now.Date; // Swapnil 18/09/2020
                        tr.LastApprover_UserID = requestingUser.UserID; // Swapnil 18/09/2020

                        ctx.SaveChanges();

                        User targetedUser = ctx.User.First(p => p.EmployeeID == nextApproverEmployeeID);  //Get associated targeted user


                        //**************************REDUNDANT CODE - TEMPORARY*******************************
                        //WebAPI.Services.MailServices.SendApprovalEmail(requestingUser.FirstName + " " + requestingUser.LastName,
                        //                                                targetedUser.FirstName + " " + targetedUser.LastName,
                        //                                                "Admin", trend.TrendNumber.ToString(), pr.ProjectID.ToString(), requestingUser.Email);

                        // -----------------------------Swapnil 17-09-2020 ----------------------------------------------------------------------------------------------
                        WebAPI.Services.MailServices.SendApprovalEmail(requestingUser.FirstName + " " + requestingUser.LastName,
                                                                        targetedUser.FirstName + " " + targetedUser.LastName,
                                                                        "Admin", trend.TrendNumber.ToString(), pr.ProjectID.ToString(), targetedUser.Email);
                        //---------------------------------------------------------------------------------------------------------------------------------------

                        return "You have approved successfully. \n" + lowestApproverRole + ", " + targetedUser.FirstName + " " + targetedUser.LastName + " will be notified for approval.";

                    }
                }
            }
            catch (Exception ex)
            {
                var stackTrace = new StackTrace(ex, true);
                var line = stackTrace.GetFrame(0).GetFileLineNumber();
                Logger.LogExceptions(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, ex.Message, line.ToString(), Logger.logLevel.Exception);
            }

            return "";
        }

        public static String updateTrend(Trend trend)
        {
            String result = "";
            try
            {
                ////luan here remove temporarily
                //if(trend.TrendStatusID == 5)    //Submit button clicked on front-end
                //{
                //    result = submitForApproval(trend);
                //}
                //else if(trend.TrendStatusID == 3)   //Clicked on approve button
                //{
                //    result = singleApprove(trend);
                //}

                ////Manasi 30-07-2020 to enable trend approval process
                if (trend.TrendStatusID == 5)
                {
                    result = submitForApproval(trend);
                }
                else if (trend.TrendStatusID == 3)
                {
                    result = singleApprove(trend);
                }

                ////Manasi 30-07-2020 to enable trend approval process
                //trend.TrendStatusID = 1;  

                //result = "Official Approved";

                if (result == "Official Approved")
                {
                    using (var ctx = new CPPDbContext())
                    {
                        ctx.Database.Log = msg => Trace.WriteLine(msg);
                        List<Project> projects = Project.getProject("null", "null", trend.ProjectID.ToString(), "null");
                        var programId = projects[0].ProgramID;

                        List<TrendFund> trendFundList = TrendFund.getTrendFund(Convert.ToInt16(trend.TrendID), projects[0].ProjectID);
                        List<ProgramFund> programFundList = ProgramFund.getProgramFund(programId);

                        foreach (var programFund in programFundList)
                        {
                            foreach (var trendFund in trendFundList)
                            {
                                if (programFund.FundName == trendFund.FundName)
                                {
                                    programFund.FundRequest = programFund.FundRequest - trendFund.FundAssign;
                                    programFund.FundUsed = programFund.FundUsed + trendFund.FundAssign;
                                }
                            }
                        }

                        foreach (var programFund in programFundList)
                        {
                            var status = ProgramFund.updateProgramFund(programFund);
                        }

                        //Trend tr = ctx.Trend.First(p => p.Project.ProjectID == trend.ProjectID && p.TrendNumber == trend.TrendNumber);
                        Trend tr = ctx.Trend.First(p => p.TrendID == trend.TrendID);
                        if (tr != null)
                        {
                            //Condition here to continue the cycle of approval process.
                            //tr.TrendStatusID = trend.TrendStatusID;
                            tr.TrendStatusID = 1;
                            tr.TrendNumber = trend.TrendNumber;
                            tr.TrendDescription = trend.TrendDescription;
                            tr.TrendJustification = trend.TrendJustification;
                            tr.TrendImpact = trend.TrendImpact;
                            tr.TrendImpactSchedule = trend.TrendImpactSchedule;  //Manasi 13-07-2020
                            tr.TrendImpactCostSchedule = trend.TrendImpactCostSchedule;  //Manasi 13-07-2020
                            tr.CreatedOn = trend.CreatedOn;
                            tr.ApprovalFrom = trend.ApprovalFrom;
                            tr.ApprovalDate = trend.ApprovalDate;
                            //tr.TrendStatusCodeID = trend.TrendStatusCodeID; //Jignesh-31-08-2021 for approved trend view
                            tr.IsInternal = trend.IsInternal;
                            tr.PostTrendCost = trend.TrendCost;
                            //tr.PostTrendStartDate = trend.PostTrendStartDate;
                            //tr.PostTrendEndDate = trend.PostTrendEndDate;

                            ctx.SaveChanges();

                            updateCostOnApproval(projects[0].ProjectID);

                            //Update Current Trend
                            // CurrentTrend.GetCurrentProject(projects[0].ProjectID, "week");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var stackTrace = new StackTrace(ex, true);
                var line = stackTrace.GetFrame(0).GetFileLineNumber();
                // Logger.LogExceptions(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, ex.Message, line.ToString(), Logger.logLevel.Exception);
            }
            return result;
        }

        public static String updateTrendStatusCode(Trend trend)
        {
            String result = "";
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    Trend tr = ctx.Trend.Where(p => p.TrendID == trend.TrendID).FirstOrDefault();
                    if (tr == null)
                        tr = ctx.Trend.Where(p => p.TrendNumber == trend.TrendNumber && p.ProjectID == trend.ProjectID).FirstOrDefault();
                    if (tr != null)
                    {
                        tr.TrendStatusCodeID = trend.TrendStatusCodeID;



                        ctx.SaveChanges();

                        result = "Trend status code has been updated successfully";
                    }
                }
            }
            catch (Exception ex)
            {
                var stackTrace = new StackTrace(ex, true);
                var line = stackTrace.GetFrame(0).GetFileLineNumber();
                Logger.LogExceptions(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, ex.Message, line.ToString(), Logger.logLevel.Exception);
            }
            return result;
        }

        public static void deleteCost(List<Activity> activityList)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            MySqlConnection conn = null;
            MySqlDataReader reader = null;
            using (var ctx = new CPPDbContext())
            {
                try
                {
                    foreach (var act in activityList)
                    {
                        Activity.deleteActivity(act);

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

                }
            }
        }

        public static String deleteTrend(Trend trend)
        {
            MySqlConnection conn = null;
            MySqlDataReader reader = null;
            String result = "";
            try
            {

                using (var ctx = new CPPDbContext())
                {
                    List<Activity> activityList = new List<Activity>();
                    Project project = ctx.Project.Where(p => p.ProjectID == trend.ProjectID && p.IsDeleted == false).FirstOrDefault();

                    activityList = ctx.Activity.Where(a => a.TrendNumber == trend.TrendNumber && a.ProjectID == trend.ProjectID && a.IsDeleted==false).ToList();
                    List<TrendCostOverhead> trendCostOverheads = ctx.TrendCostOverhead.Where(a => a.TrendID == trend.TrendID).ToList();
                    if (activityList.Count > 0)
                    {
                        foreach (var act in activityList)
                        {
                            act.DeletedBy = trend.DeletedBy;
                            Activity.deleteActivity(act);
                        }
                    }
                    ctx.Database.Log = msg => Trace.WriteLine(msg);
                    List<TrendFund> trendFundList = TrendFund.getTrendFund(Convert.ToInt16(trend.TrendID), trend.ProjectID);
                    List<Project> projectList = Project.getProject("null", "null", trend.ProjectID.ToString(), "null");
                    var programID = projectList[0].ProgramID;
                    List<ProgramFund> programFundList = ProgramFund.getProgramFund(programID);
                    using (var DbCtx = new CPPDbContext())
                    {
                        Trend tr = DbCtx.Trend.First(p => p.ProjectID == trend.ProjectID && p.TrendNumber == trend.TrendNumber && p.IsDeleted==false);
                        foreach (var programFund in programFundList)
                        {
                            foreach (var trendFund in trendFundList)
                            {
                                if (programFund.FundName == trendFund.FundName)
                                {
                                    if (tr.TrendStatusID == 1)
                                    {
                                        programFund.FundUsed -= trendFund.FundAssign;
                                        programFund.FundRemaining += trendFund.FundAssign;

                                    }
                                    else if (tr.TrendStatusID == 3)
                                    {
                                        programFund.FundRequest -= trendFund.FundAssign;
                                        programFund.FundRemaining += trendFund.FundAssign;
                                    }
                                }
                            }
                        }

                        foreach (var programFund in programFundList)
                        {
                            ProgramFund.updateProgramFund(programFund);
                        }
                        foreach (var trendFund in trendFundList)
                        {
                            TrendFund.deleteTrendFund(trendFund);
                        }
                        //var trendCtx = new CPPDbContext();
                        //CostOverhead 
                        //Remove Trend_cost_overhead
                        foreach (var tco in trendCostOverheads)
                        {
                            //Nivedita 10022022
                            //ctx.CostLineItemTracker.Remove(costLineItem);
                            tco.IsDeleted = true;
                            tco.DeletedDate = DateTime.Now;
                            tco.DeletedBy = trend.DeletedBy;
                            //ctx.TrendCostOverhead.Remove(tco);
                            ctx.SaveChanges();
                        }
                        //Nivedita 10022022
                        //DbCtx.Trend.Remove(tr);
                        tr.IsDeleted = true;
                        tr.DeletedDate = DateTime.Now;
                        tr.DeletedBy = trend.DeletedBy;
                        tr.Status= "Archived";   //----Vaishnavi 30-03-2022----//
                        DbCtx.SaveChanges();
                        updateCostOnApproval(project.ProjectID);
                        result = "Success";
                    }
                }


            }
            catch (Exception ex)
            {
                var stackTrace = new StackTrace(ex, true);
                var line = stackTrace.GetFrame(0).GetFileLineNumber();
                Logger.LogExceptions(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, ex.Message, line.ToString(), Logger.logLevel.Exception);
            }
            return result;
        }

        public static Trend getTrendById(String trendNumber, int projectId)
        {
            //Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            // Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            Trend matchedTrend = new Trend();

            var id = Convert.ToInt16(trendNumber);

            try
            {
                using (var ctx = new CPPDbContext())
                {
                    matchedTrend = ctx.Trend.Include("CostOverheadType").Where(t => t.TrendNumber == trendNumber && t.ProjectID == projectId).FirstOrDefault();
                }


            }
            catch (Exception ex)
            {
                var stackTrace = new StackTrace(ex, true);
                var line = stackTrace.GetFrame(0).GetFileLineNumber();
                //  Logger.LogExceptions(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, ex.Message, line.ToString(), Logger.logLevel.Exception);
            }
            finally
            {

            }
            // Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Exit Point", Logger.logLevel.Debug);
            return matchedTrend;
        }
        //Stored Procedure
        public static void updateCostOnApproval(int ProjectID)
        {
            using (var ctx = new CPPDbContext())
            {
                Project project = ctx.Project.Where(p => p.ProjectID == ProjectID).FirstOrDefault();
                ProgramElement programElement = ctx.ProgramElement.Where(p => p.ProgramElementID == project.ProgramElementID).FirstOrDefault();
                Program program = ctx.Program.Where(p => p.ProgramID == programElement.ProgramID).FirstOrDefault();

                Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
                Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
                MySqlCommand command = null;
                MySqlConnection conn = null;
                MySqlDataReader reader = null;

                try
                {
                    var query = "updateCostOnApproval"; //Stored Procedure
                    conn = ConnectionManager.getConnection();
                    conn.Open();
                    command = new MySqlCommand(query, conn);
                    command.CommandType = CommandType.StoredProcedure;

                    //For Create New

                    command.Parameters.Add(new MySqlParameter("_ProjectID", ProjectID));
                    command.Parameters.Add(new MySqlParameter("_ProgramElementID", programElement.ProgramElementID));
                    command.Parameters.Add(new MySqlParameter("_ProgramID", program.ProgramID));
                    command.Parameters.Add(new MySqlParameter("_TrendID", 0));

                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    var stackTrace = new StackTrace(ex, true);
                    var line = stackTrace.GetFrame(0).GetFileLineNumber();
                    Logger.LogExceptions(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, ex.Message, line.ToString(), Logger.logLevel.Exception);

                }
                finally
                {
                    if (conn != null)
                    {
                        conn.Close();
                    }
                    if (reader != null)
                    {
                        reader.Close();
                    }
                }

            }
        }

        public static String submitForApproval2(int ProjectID, String TrendNumber, int TrendStatusID, String ApprovedBy, String ApprovalDate, double TrendTotalValue)
        {
            String result = "";
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    Trend tr = ctx.Trend.First(p => p.Project.ProjectID == ProjectID && p.TrendNumber == TrendNumber);

                    if (tr != null)
                    {
                        //Where we find the first approver
                        List<ApprovalMatrix> approvalMatrixList = ApprovalMatrix.getApprovalMatrix();

                        tr.CurrentApprover_EmployeeID = 10000;
                        ctx.SaveChanges();

                        result += "Success";
                    }
                    else
                    {
                        result += "Trend does not exist in system";
                    }
                }
            }
            catch (Exception ex)
            {
                var stackTrace = new StackTrace(ex, true);
                var line = stackTrace.GetFrame(0).GetFileLineNumber();
                Logger.LogExceptions(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, ex.Message, line.ToString(), Logger.logLevel.Exception);
            }
            return result;
        }


        public static String approveTrend(int ProjectID, String TrendNumber, int TrendStatusID, String ApprovedBy, String ApprovalDate, double TrendTotalValue)
        {
            String result = "";
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    ctx.Database.Log = msg => Trace.WriteLine(msg);


                    Trend tr = ctx.Trend.First(p => p.Project.ProjectID == ProjectID && p.TrendNumber == TrendNumber);
                    TrendStatus trendStatusObj = ctx.TrendStatus.First(t => t.StatusID == TrendStatusID);
                    if (trendStatusObj.StatusDescription.Trim() != "Pending")
                    {
                        result += "Trend cannot be approved. Trend current status is: " + trendStatusObj.StatusDescription.Trim();
                    }
                    else
                    {
                        if (tr != null)
                        {
                            tr.TrendStatus = trendStatusObj;
                            //tr.ApprovalFrom = ApprovedBy;
                            tr.ApprovalDate = Convert.ToDateTime(ApprovalDate);
                            ctx.SaveChanges();
                            result = "Success";

                            updateCostOnApproval(ProjectID);

                        }
                        else
                        {
                            result += "Trend does not exist in system";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var stackTrace = new StackTrace(ex, true);
                var line = stackTrace.GetFrame(0).GetFileLineNumber();
                Logger.LogExceptions(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, ex.Message, line.ToString(), Logger.logLevel.Exception);
            }
            return result;
        }

        public static String rejectTrend(int ProjectID, String TrendNumber, int TrendStatusID, String RejectedBy, String RejectedDate)
        {
            String result = "";
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    ctx.Database.Log = msg => Trace.WriteLine(msg);
                    Trend tr = ctx.Trend.First(p => p.Project.ProjectID == ProjectID && p.TrendNumber == TrendNumber);
                    TrendStatus trendStatusObj = ctx.TrendStatus.First(t => t.StatusID == TrendStatusID);
                    if (trendStatusObj.StatusDescription.Trim() != "Pending")
                    {
                        result += "Trend cannot be rejected. Trend current status is: " + trendStatusObj.StatusDescription.Trim();
                    }
                    else
                    {
                        if (tr != null)
                        {

                            tr.TrendStatus = trendStatusObj;
                            //tr.ApprovalFrom = ApprovedBy;
                            //tr.ApprovalDate = ApprovedDate;

                            ctx.SaveChanges();
                            result = "Success";
                        }
                        else
                        {
                            result += "Trend does not exist in system";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var stackTrace = new StackTrace(ex, true);
                var line = stackTrace.GetFrame(0).GetFileLineNumber();
                Logger.LogExceptions(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, ex.Message, line.ToString(), Logger.logLevel.Exception);
            }
            return result;
        }

        public static List<Trend> getAllTrendsByProjectID(string projectID)
        {
            List<Trend> trends = null;
            int pID = int.Parse(projectID);
            using (var ctx = new CPPDbContext())
            {
                trends = ctx.Trend.Where(a => a.ProjectID == pID && a.IsDeleted == false).OrderBy(a => new { a.TrendStatusID, a.TrendNumber }).ToList();
                //trends = ctx.Trend.OrderBy(a => new { a.TrendStatusID, a.TrendNumber }).ToList();    //Jignesh 05-01-2021
            }

            return trends;
        }

        //--------------------------Swapnil 18-09-2020-------------------------------------------------------

        public static void SendReminderApprovalEmail()
        {
            MFAConfiguration MFADetails = WebAPI.Models.MFAConfiguration.getMFADetails();
            DateTime date = DateTime.Now.Date.AddDays(-(MFADetails.ReminderMailDays));
            List<Trend> trends = null;
            using (var ctx = new CPPDbContext())
            {
                trends = ctx.Trend.Where(a => a.NextApproverEmailDate == date && (a.TrendStatusID == 3 || a.TrendStatusID == 5)).ToList();

                foreach (var item in trends)
                {

                    User targetedUser = ctx.User.First(p => p.EmployeeID == item.CurrentApprover_EmployeeID);

                    User requestingUser = ctx.User.First(p => p.UserID == item.LastApprover_UserID);

                    Project pr = ctx.Project.First(p => p.ProjectID == item.ProjectID);

                    WebAPI.Services.MailServices.SendApprovalEmail(requestingUser.FirstName + " " + requestingUser.LastName,
                                                                            targetedUser.FirstName + " " + targetedUser.LastName,
                                                                            "Admin", item.TrendNumber.ToString(), pr.ProjectID.ToString(), targetedUser.Email, "REMINDER : CPP - APPROVAL REQUEST NOTIFICATION");

                    Trend tr = ctx.Trend.First(p => p.Project.ProjectID == item.ProjectID && p.TrendNumber == item.TrendNumber);

                    tr.NextApproverEmailDate = DateTime.Now.Date; // Swapnil 18/09/2020
                    tr.LastApprover_UserID = requestingUser.UserID; // Swapnil 18/09/2020

                    ctx.SaveChanges();

                }
            }


        }

        //---------------------------------------------------------------------------------------------------
        // Jignesh-TDM-06-01-2020
        public static List<Trend> GetAllTrendsForChangeOrder(string projectID)
        {
            List<Trend> trends = null;
            int pID = int.Parse(projectID);
            using (var ctx = new CPPDbContext())
            {
                //trends = ctx.Trend.Where(a => a.ProjectID == pID).OrderBy(a => new { a.TrendStatusID, a.TrendNumber }).ToList();
                trends = ctx.Trend.OrderBy(a => new { a.TrendStatusID, a.TrendNumber }).ToList();
            }
            return trends;
        }
        //---------------------------------------------------------------------------------------------------
        //----Vaishnavi 30-03-2022----//
        public static String closeTrend(Trend trend)
        {
            MySqlConnection conn = null;
            MySqlDataReader reader = null;
            String result = "";
            try
            {

                using (var ctx = new CPPDbContext())
                {
                    //List<Activity> activityList = new List<Activity>();
                    //Project project = ctx.Project.Where(p => p.ProjectID == trend.ProjectID && p.IsDeleted == false).FirstOrDefault();

                    //activityList = ctx.Activity.Where(a => a.TrendNumber == trend.TrendNumber && a.ProjectID == trend.ProjectID && a.IsDeleted == false).ToList();
                    //List<TrendCostOverhead> trendCostOverheads = ctx.TrendCostOverhead.Where(a => a.TrendID == trend.TrendID).ToList();
                    //if (activityList.Count > 0)
                    //{
                    //    foreach (var act in activityList)
                    //    {
                    //        act.DeletedBy = trend.DeletedBy;
                    //        Activity.deleteActivity(act);
                    //    }
                    //}
                    //ctx.Database.Log = msg => Trace.WriteLine(msg);
                    //List<TrendFund> trendFundList = TrendFund.getTrendFund(Convert.ToInt16(trend.TrendID), trend.ProjectID);
                    //List<Project> projectList = Project.getProject("null", "null", trend.ProjectID.ToString(), "null");
                    //var programID = projectList[0].ProgramID;
                    //List<ProgramFund> programFundList = ProgramFund.getProgramFund(programID);
                    using (var DbCtx = new CPPDbContext())
                    {
                        Trend tr = DbCtx.Trend.First(p => p.ProjectID == trend.ProjectID && p.TrendNumber == trend.TrendNumber && p.IsDeleted == false);
                        //foreach (var programFund in programFundList)
                        //{
                        //    foreach (var trendFund in trendFundList)
                        //    {
                        //        if (programFund.FundName == trendFund.FundName)
                        //        {
                        //            if (tr.TrendStatusID == 1)
                        //            {
                        //                programFund.FundUsed -= trendFund.FundAssign;
                        //                programFund.FundRemaining += trendFund.FundAssign;

                        //            }
                        //            else if (tr.TrendStatusID == 3)
                        //            {
                        //                programFund.FundRequest -= trendFund.FundAssign;
                        //                programFund.FundRemaining += trendFund.FundAssign;
                        //            }
                        //        }
                        //    }
                        //}

                        //foreach (var programFund in programFundList)
                        //{
                        //    ProgramFund.updateProgramFund(programFund);
                        //}
                        //foreach (var trendFund in trendFundList)
                        //{
                        //    TrendFund.deleteTrendFund(trendFund);
                        //}
                        ////var trendCtx = new CPPDbContext();
                        ////CostOverhead 
                        ////Remove Trend_cost_overhead
                        //foreach (var tco in trendCostOverheads)
                        //{
                        //    //Nivedita 10022022
                        //    //ctx.CostLineItemTracker.Remove(costLineItem);
                        //    tco.IsDeleted = true;
                        //    tco.DeletedDate = DateTime.Now;
                        //    tco.DeletedBy = trend.DeletedBy;
                        //    //ctx.TrendCostOverhead.Remove(tco);
                        //    ctx.SaveChanges();
                        //}
                        //Nivedita 10022022
                        //DbCtx.Trend.Remove(tr);
                        //tr.IsDeleted = true;
                        //tr.DeletedDate = DateTime.Now;
                        //tr.DeletedBy = trend.DeletedBy;
                        tr.Status = "Closed";
                        DbCtx.SaveChanges();
                      //  updateCostOnApproval(project.ProjectID);
                        result = "Success";
                    }
                }


            }
            catch (Exception ex)
            {
                var stackTrace = new StackTrace(ex, true);
                var line = stackTrace.GetFrame(0).GetFileLineNumber();
                Logger.LogExceptions(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, ex.Message, line.ToString(), Logger.logLevel.Exception);
            }
            return result;
        }
    }
}