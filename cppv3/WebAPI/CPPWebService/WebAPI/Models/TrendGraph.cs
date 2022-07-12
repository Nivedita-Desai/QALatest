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

namespace WebAPI.Models
{
    public class PastTrends
    {
        //[DataMember]
        //public Trend WBSTrend;

        [DataMember]
        public String name;
        [DataMember]
        public String TrendNumber;
        [DataMember]
        public String ProjectID;
        [DataMember]
        public String ProjectName;
        [DataMember]
        public String TrendDescription;
        [DataMember]
        public String TrendStatus;
        [DataMember]
        public int? TrendStatusCodeID;
		[DataMember]
		public int? IsInternal;
        [DataMember]
        public String Status;    //----Vaishnavi 30-03-2022----//
        [DataMember]
        public int? CostOverheadTypeID;
        [DataMember]
        public String TrendJustification;
        [DataMember]
        public String TrendImpact;
        [DataMember]
        public String CreatedOn;
        [DataMember]
        public String ApprovalFrom;
        [DataMember]
        public String ApprovalDate;
        [DataMember]
        public String PostTrendStartDate;
        [DataMember]
        public String PostTrendEndDate;
        [DataMember]
        public String PreTrendStartDate;
        [DataMember]
        public String PreTrendEndDate;
        [DataMember]
        public Double PreTrendCost;
        [DataMember]
        public Double PostTrendCost;
        [DataMember]
        public String level = "PastTrend";

        [DataMember]
        public CurrentProject children;

        [DataMember]
        public String TrendImpactSchedule;   //Manasi 13-07-2020

        [DataMember]
        public String TrendImpactCostSchedule;  //Manasi 13-07-2020

        //======================== Jignesh-02-04-2021 ================================================
        [DataMember]
        public int? IsChangeRequest;
        [DataMember]
        public int? IsApprovedByClient; 
        [DataMember]
        public String ClientApprovedDate;
        [DataMember]
        public int? ChangeOrderID;
        [DataMember]
        public string ProgramElementId;
        //============================================================================================

        
        //======================== Added by Swapnil to get program element id 02-11-2020 ==================================
        public static String getMaxProgramElementId(int projectId)
        {
            String programElementId = "";
            Trend matchedTrend = new Trend();
            MySqlConnection conn = null;
            MySqlDataReader reader = null;
            try
            {
                conn = ConnectionManager.getConnection();
                conn.Open();

                String query = "SELECT ProgramElementID FROM project";

                query += " WHERE ProjectID = @projectId";


                MySqlCommand command = new MySqlCommand(query, conn);
                command.Parameters.AddWithValue("@projectId", projectId);
                using (reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var Id = reader.GetValue(0).ToString().Trim();
                        if (Id != "")
                        {
                            programElementId = Id;
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

            return programElementId;
        }
        //=====================================================================================================================
        public PastTrends(Trend wbst)
        { name = "Trend " + wbst.TrendNumber; TrendNumber = wbst.TrendNumber; 
            ProjectID = wbst.Project.ProjectID.ToString(); ProjectName = wbst.Project.ProjectName; TrendDescription = wbst.TrendDescription;
            TrendStatus = wbst.TrendStatus.StatusDescription; TrendJustification = wbst.TrendJustification; TrendImpact = wbst.TrendImpact; CreatedOn = wbst.CreatedOn; ApprovalFrom = wbst.ApprovalFrom; ApprovalDate = (wbst.ApprovalDate != null ? wbst.ApprovalDate.Value.ToString("yyyy-MM-dd") : "");
            PostTrendStartDate = (wbst.PostTrendStartDate != null ? wbst.PostTrendStartDate.Value.ToString("yyyy-MM-dd") : "");
            PostTrendEndDate = (wbst.PostTrendEndDate != null ? wbst.PostTrendEndDate.Value.ToString("yyyy-MM-dd") : "");
            PreTrendStartDate = (wbst.PreTrendStartDate != null ? wbst.PreTrendStartDate.Value.ToString("yyyy-MM-dd") : "");
            PreTrendEndDate = (wbst.PreTrendStartDate != null ? wbst.PreTrendEndDate.Value.ToString("yyyy-MM-dd") : "");
            TrendStatusCodeID = wbst.TrendStatusCodeID; IsInternal = wbst.IsInternal;
            Status = wbst.Status;   //----Vaishnavi 30-03-2022----//
            CostOverheadTypeID = wbst.CostOverheadTypeID;
            //======================== Jignesh-02-04-2021 ================================================
            ProgramElementId = getMaxProgramElementId(wbst.ProjectID);
            IsChangeRequest = wbst.IsChangeRequest; ChangeOrderID = wbst.ChangeOrderID;
            IsApprovedByClient = wbst.IsApprovedByClient; 
            ClientApprovedDate = (wbst.ClientApprovedDate != null ? wbst.ClientApprovedDate.Value.ToString("yyyy-MM-dd") : "");
            //============================================================================================
            if (wbst.PreTrendCost != null)
            {
                PreTrendCost = Convert.ToDouble(wbst.PreTrendCost);
            }
            if (wbst.PostTrendCost != null)
            {
                PostTrendCost = Convert.ToDouble(wbst.PostTrendCost);
            }
            /*children = cur_prj;*/
            TrendImpactSchedule = wbst.TrendImpactSchedule;TrendImpactCostSchedule = wbst.TrendImpactCostSchedule;  //Manasi 13-07-2020
        }
    }

    public class FutureTrends
    {
        //[DataMember]
        //public Trend WBSTrend;

        [DataMember]
        public String name;
        [DataMember]
        public String TrendNumber;
        [DataMember]
        public String ProjectID;
        [DataMember]
        public String ProjectName;
        [DataMember]
        public String TrendDescription;
        [DataMember]
        public String TrendStatus;
        [DataMember]
        public int? TrendStatusCodeID;
        [DataMember]
        public int? IsInternal;
        [DataMember]
        public String Status;   //----Vaishnavi 30-03-2022----//
        [DataMember]
        public int? CostOverheadTypeID;
        [DataMember]
        public String TrendJustification;
        [DataMember]
        public String TrendImpact;
        [DataMember]
        public String CreatedOn;
        [DataMember]
        public String ApprovalFrom;
        [DataMember]
        public String ApprovalDate;
        [DataMember]
        public String PostTrendStartDate;
        [DataMember]
        public String PostTrendEndDate;
        [DataMember]
        public String PreTrendStartDate;
        [DataMember]
        public String PreTrendEndDate;
        [DataMember]
        public int PreTrendCost;
        [DataMember]
        public int PostTrendCost;
        [DataMember]
        public String level = "FutureTrend";

        [DataMember]
        public ForecastProject children;

        [DataMember]
        public String TrendImpactSchedule;   //Manasi 13-07-2020

        [DataMember]
        public String TrendImpactCostSchedule;  //Manasi 13-07-2020
        [DataMember]
        public int? IsChangeRequest;
        [DataMember]
        public int? IsApprovedByClient; // Jignesh 31-12-2020
        [DataMember]
        public String ClientApprovedDate; // Jignesh-09-02-2021
        [DataMember]
        public int? ChangeOrderID;
        [DataMember]
        public string ProgramElementId;

        //public FutureTrends(Trend wbst, ForecastProject fc_prj)


        //======================== Added by Swapnil to get program element id 02-11-2020 ==================================
        public static String getMaxProgramElementId(int projectId)
        {
            String programElementId = "";
            Trend matchedTrend = new Trend();
            MySqlConnection conn = null;
            MySqlDataReader reader = null;
            try
            {
                conn = ConnectionManager.getConnection();
                conn.Open();

                String query = "SELECT ProgramElementID FROM project";

                query += " WHERE ProjectID = @projectId";


                MySqlCommand command = new MySqlCommand(query, conn);
                command.Parameters.AddWithValue("@projectId", projectId);

                using (reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var Id = reader.GetValue(0).ToString().Trim();
                        if (Id != "")
                        {
                            programElementId = Id;
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

            return programElementId;
        }
        //=====================================================================================================================

        public FutureTrends(Trend wbst)
        { name = "Trend " + wbst.TrendNumber; TrendNumber = wbst.TrendNumber; ProjectID = wbst.Project.ProjectID.ToString(); ProjectName = wbst.Project.ProjectName; TrendDescription = wbst.TrendDescription;
        TrendStatus = wbst.TrendStatus.StatusDescription; TrendJustification = wbst.TrendJustification; TrendImpact = wbst.TrendImpact; CreatedOn = wbst.CreatedOn; ApprovalFrom = wbst.ApprovalFrom; ApprovalDate = (wbst.ApprovalDate != null ? wbst.ApprovalDate.Value.ToString("yyyy-MM-dd") : "");
            PostTrendStartDate = (wbst.PostTrendStartDate != null ? wbst.PostTrendStartDate.Value.ToString("yyyy-MM-dd") : "");
            PostTrendEndDate = (wbst.PostTrendEndDate != null ? wbst.PostTrendEndDate.Value.ToString("yyyy-MM-dd") : "");
            PreTrendStartDate = (wbst.PreTrendStartDate != null ? wbst.PreTrendStartDate.Value.ToString("yyyy-MM-dd") : "");
            PreTrendEndDate = (wbst.PreTrendStartDate != null ? wbst.PreTrendEndDate.Value.ToString("yyyy-MM-dd") : "");
            TrendStatusCodeID = wbst.TrendStatusCodeID; IsInternal = wbst.IsInternal;
            Status = wbst.Status;   //----Vaishnavi 30-03-2022----//

            CostOverheadTypeID = wbst.CostOverheadTypeID;
            ProgramElementId = getMaxProgramElementId(wbst.ProjectID); // Swapnil 02-11-2020
            IsChangeRequest = wbst.IsChangeRequest;ChangeOrderID = wbst.ChangeOrderID;// Swapnil 30-10-2020
            IsApprovedByClient = wbst.IsApprovedByClient; // Jignesh 31-12-2020
            ClientApprovedDate = (wbst.ClientApprovedDate != null ? wbst.ClientApprovedDate.Value.ToString("yyyy-MM-dd") : ""); // Jignesh-09-02-2021
            if (wbst.PreTrendCost != null)
            {
                PreTrendCost = Convert.ToInt32(Convert.ToDouble(wbst.PreTrendCost));
            }
            if (wbst.PostTrendCost != null)
            {
                PostTrendCost = Convert.ToInt32(Convert.ToDouble(wbst.PostTrendCost));
            }
            TrendImpactSchedule = wbst.TrendImpactSchedule;  //Manasi 13-07-2020
            TrendImpactCostSchedule = wbst.TrendImpactCostSchedule;  //Manasi 13-07-2020
            /*children = fc_prj;*/
        }
    }

    [DataContract]
    public class CurrentProject
    {
        /*[DataMember]
        public Project WBSProject;*/
        [DataMember]
        public String ProgramID;
        [DataMember]
        public String ProgramElementID;
        [DataMember]
        public String ProjectID;
        [DataMember]
        public String name;
        [DataMember]
        public String ProjectName;
        [DataMember]
        public String ProjectManager;
        [DataMember]
        public String ProjectSponsor;
        [DataMember]
        public String ApprovedTrendNumber;
        [DataMember]
        public String CurrentStartDate;
        [DataMember]
        public String CurrentEndDate;
        [DataMember]
        public String ForecastStartDate;
        [DataMember]
        public String ForecastEndDate;
        [DataMember]
        public String CurrentCost;
        [DataMember]
        public String ForecastCost;
        [DataMember]
        public String level = "CurrentProject";

        
        public CurrentProject()
        { }
        //public CurrentProject(Project wbspj, List<FutureTrends> ft_trn)
        public CurrentProject(Project wbspj)
        { ProgramID = wbspj.ProgramID.ToString(); ProgramElementID = wbspj.ProgramElementID.ToString(); ProjectID = wbspj.ProjectID.ToString(); 
            name = "CurrentProject"; ProjectName = wbspj.ProjectName; ProjectManager = wbspj.ProjectManager; ProjectSponsor = wbspj.ProjectSponsor; ApprovedTrendNumber = wbspj.ApprovedTrendNumber;
            CurrentStartDate = (wbspj.CurrentStartDate != null ? wbspj.CurrentStartDate.Value.ToString("yyyy-MM-dd") : "");
            CurrentEndDate = (wbspj.CurrentEndDate != null ? wbspj.CurrentEndDate.Value.ToString("yyyy-MM-dd") : "");
            ForecastStartDate = (wbspj.ForecastStartDate != null ? wbspj.ForecastStartDate.Value.ToString("yyyy-MM-dd") : "");
            ForecastEndDate = (wbspj.ForecastEndDate != null ? wbspj.ForecastEndDate.Value.ToString("yyyy-MM-dd") : ""); 
            CurrentCost = wbspj.CurrentCost; ForecastCost = wbspj.ForecastCost; /*children = ft_trn;*/ }
    }

    [DataContract]
    public class ForecastProject
    {
        /*[DataMember]
        public Project WBSProject;*/
        [DataMember]
        public String ProgramID;
        [DataMember]
        public String ProgramElementID;
        [DataMember]
        public String ProjectID;
        [DataMember]
        public String name;
        [DataMember]
        public String ProjectName;
        [DataMember]
        public String ProjectManager;
        [DataMember]
        public String ProjectSponsor;
        [DataMember]
        public String ApprovedTrendNumber;
        [DataMember]
        public String CurrentStartDate;
        [DataMember]
        public String CurrentEndDate;
        [DataMember]
        public String ForecastStartDate;
        [DataMember]
        public String ForecastEndDate;
        [DataMember]
        public String CurrentCost;
        [DataMember]
        public String ForecastCost;
        [DataMember]
        public String level = "ForecastProject";

       
        public ForecastProject()
        { }
        public ForecastProject(Project wbspj)
        { ProgramID = wbspj.ProgramID.ToString(); ProgramElementID = wbspj.ProgramElementID.ToString(); ProjectID = wbspj.ProjectID.ToString(); 
            name = "ForecastProject"; ProjectName = wbspj.ProjectName; ProjectManager = wbspj.ProjectManager; ProjectSponsor = wbspj.ProjectSponsor; ApprovedTrendNumber = wbspj.ApprovedTrendNumber;
            CurrentStartDate = (wbspj.CurrentStartDate != null ? wbspj.CurrentStartDate.Value.ToString("yyyy-MM-dd") : "");
            CurrentEndDate = (wbspj.CurrentEndDate != null ? wbspj.CurrentEndDate.Value.ToString("yyyy-MM-dd") : "");
            ForecastStartDate = (wbspj.ForecastStartDate != null ? wbspj.ForecastStartDate.Value.ToString("yyyy-MM-dd") : "");
            ForecastEndDate = (wbspj.ForecastEndDate != null ? wbspj.ForecastEndDate.Value.ToString("yyyy-MM-dd") : "");  
            CurrentCost = wbspj.CurrentCost; ForecastCost = wbspj.ForecastCost; }
    }




    [DataContract]
    public class TrendGraph
    {
        [DataMember]
        public List<PastTrends> PastTrendList = new List<PastTrends>();
        [DataMember]
        public List<FutureTrends> FutureTrendList = new List<FutureTrends>();
        [DataMember]
        public CurrentProject CurrentProjectList = new CurrentProject();
        [DataMember]
        public ForecastProject ForecastProjectList = new ForecastProject();

        public static TrendGraph getTrendGraph(String ProjectID)
        {
            TrendGraph returnedTrendGraph = new TrendGraph();
            List<Project> returnedProjects = Project.getProject("null", "null", ProjectID, "null");
            List<Trend> returnedTrends = Trend.getTrend("null", "null", ProjectID, "null", "null");
            foreach (Trend tempTrend in returnedTrends)
            {
                if (tempTrend.TrendStatus.StatusDescription == "Approved" || tempTrend.TrendStatus.StatusDescription == "Rejected")
                {
                    PastTrends matchedPastTrend = new PastTrends(tempTrend);
                    returnedTrendGraph.PastTrendList.Add(matchedPastTrend);
                }
                else if  (tempTrend.TrendStatus.StatusDescription == "Pending")
                {
                    FutureTrends matchedFutureTrend = new FutureTrends(tempTrend);
                    returnedTrendGraph.FutureTrendList.Add(matchedFutureTrend);

                }
            }
            if (returnedProjects.Count > 0) 
            {
                returnedTrendGraph.CurrentProjectList = new CurrentProject(returnedProjects[0]);
                returnedTrendGraph.ForecastProjectList = new ForecastProject(returnedProjects[0]);
            }
            return returnedTrendGraph;

        }

        

    }
}