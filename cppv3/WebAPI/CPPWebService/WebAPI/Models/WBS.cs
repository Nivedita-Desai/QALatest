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
//NOT USED
namespace WebAPI.Models
{
    [DataContract]
    public class ActivityWBS
    {
        //[DataMember]    
        //public Activity WBSActivity;

        [DataMember]
        public String ActivityID;
        [DataMember]
        public String BudgetCategory;
        [DataMember]
        public String name;
        [DataMember]
        public String ActivityStartDate;
        [DataMember]
        public String ActivityEndDate;
        [DataMember]
        public String FTECost;
        [DataMember]
        public String LumpsumCost;
        [DataMember]
        public String UnitCost;
        [DataMember]
        public String PercentageBasisCost;
        [DataMember]
        public String level = "Activity";


    }
    [DataContract]
    public class PhaseWBS
    {
        //[DataMember]
        //public Phase WBSPhase;

        [DataMember]
        public String name;
        [DataMember]
        public String PostTrendPhaseStartDate;
        [DataMember]
        public String PostTrendPhaseEndDate;
        [DataMember]
        public String PreTrendPhaseStartDate;
        [DataMember]
        public String PreTrendPhaseEndDate;
        [DataMember]
        public int PreTrendPhaseCost;
        [DataMember]
        public int PostTrendPhaseCost;
        [DataMember]
        public String level = "Phase";
        [DataMember]
        public List<ActivityWBS> children = new List<ActivityWBS>();

        public PhaseWBS(Phase wbsp, List<ActivityWBS> act)
        { name = wbsp.PhaseCode; PostTrendPhaseStartDate = wbsp.PostTrendPhaseStartDate; PostTrendPhaseEndDate = wbsp.PostTrendPhaseEndDate; PreTrendPhaseStartDate = wbsp.PreTrendPhaseStartDate; PreTrendPhaseEndDate = wbsp.PreTrendPhaseEndDate; PreTrendPhaseCost = wbsp.PreTrendPhaseCost; PostTrendPhaseCost = wbsp.PostTrendPhaseCost; children = act; }
    }
    [DataContract]
    public class TrendWBS
    {
        //[DataMember]
        //public Trend WBSTrend;

        [DataMember]
        public String name;
        [DataMember]
        public String TrendDescription;
        [DataMember]
        public String TrendStatus;
        [DataMember]
        public int? TrendStatusCodeID;
        [DataMember]
        public int? IsInternal;
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
        public String level = "Trend";

        [DataMember]
        public List<PhaseWBS> children = new List<PhaseWBS>();

        [DataMember]
        public String TrendImpactSchedule;   //Manasi 13-07-2020
        [DataMember]
        public String TrendImpactCostSchedule;   //Manasi 13-07-2020

        public TrendWBS(Trend wbst, List<PhaseWBS> phs)
        {
            name = "Trend " + wbst.TrendNumber + " - " + wbst.TrendDescription; TrendDescription = wbst.TrendDescription; TrendStatus = wbst.TrendStatus.StatusDescription; TrendJustification = wbst.TrendJustification; TrendImpact = wbst.TrendImpact; CreatedOn = wbst.CreatedOn; ApprovalFrom = wbst.ApprovalFrom; ApprovalDate = (wbst.ApprovalDate != null ? wbst.ApprovalDate.Value.ToString("yyyy-MM-dd") : "");
            PostTrendStartDate = (wbst.PostTrendStartDate != null ? wbst.PostTrendStartDate.Value.ToString("yyyy-MM-dd") : "");
            PostTrendEndDate = (wbst.PostTrendEndDate != null ? wbst.PostTrendEndDate.Value.ToString("yyyy-MM-dd") : "");
            PreTrendStartDate = (wbst.PreTrendStartDate != null ? wbst.PreTrendStartDate.Value.ToString("yyyy-MM-dd") : "");
            PreTrendEndDate = (wbst.PreTrendStartDate != null ? wbst.PreTrendEndDate.Value.ToString("yyyy-MM-dd") : "");
            PreTrendCost = int.Parse(wbst.PreTrendCost);
            PostTrendCost = int.Parse(wbst.PostTrendCost);
            TrendStatusCodeID = wbst.TrendStatusCodeID; IsInternal = wbst.IsInternal;
            children = phs;
            TrendImpactSchedule = wbst.TrendImpactSchedule;  //Manasi 13-07-2020
            TrendImpactCostSchedule = wbst.TrendImpactCostSchedule;   //Manasi 13-07-2020
        }
    }
    [DataContract]
    public class ProjectWBS
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
        public String ProjectManager;

        [DataMember]
        public String ScopeQualityDescription;

        [DataMember]
        public String ProjectSponsor;
        [DataMember]
        public String ApprovedTrendNumber;
        [DataMember]
        public String CurrentStartDate;
        [DataMember]
        public String CurrentEndDate;
        [DataMember]
        public String originalEndDate;// Aditya
        [DataMember]
        public String ForecastStartDate;
        [DataMember]
        public String ForecastEndDate;
        [DataMember]
        public String CurrentCost;
        [DataMember]
        public String ForecastCost;


        [DataMember]
        public string BillingPOC;
        [DataMember]
        public string BillingPOCPhone1;
        [DataMember]
        public string BillingPOCPhone2;
        [DataMember]
        public string BillingPOCAddress;
        //====== Jignesh-AddAddressField-21-01-2021 =======
        [DataMember]
        public string BillingPOCAddressLine1;
        [DataMember]
        public string BillingPOCAddressLine2;
        [DataMember]
        public string BillingPOCCity;
        [DataMember]
        public string BillingPOCState;
        [DataMember]
        public string BillingPOCPONo;
        //=================================================
        [DataMember]
        public string BillingPOCEmail;
        [DataMember]
        public string BillingPOCSpecialInstruction;
        [DataMember]
        public byte TMBilling;
        [DataMember]
        public byte SOVBilling;
        [DataMember]
        public byte MonthlyBilling;
        [DataMember]
        public byte CertifiedPayroll;
        [DataMember]
        public byte Lumpsum;


        [DataMember]
        public String level = "Project";
        //[DataMember]
        //public String ProjectNumber;
        [DataMember]
        public List<TrendWBS> children = new List<TrendWBS>();

        public ProjectWBS(Project wbspj, List<TrendWBS> trn)
        {
            ProgramID = wbspj.Program.ProgramID.ToString(); ProgramElementID = wbspj.ProgramElement.ProgramElementID.ToString();
            ProjectID = wbspj.ProjectID.ToString(); name = wbspj.ProjectName; ProjectManager = wbspj.ProjectManager;
            ScopeQualityDescription = wbspj.ScopeQualityDescription;
            ProjectSponsor = wbspj.ProjectSponsor; ApprovedTrendNumber = wbspj.ApprovedTrendNumber;
            CurrentStartDate = (wbspj.CurrentStartDate != null ? wbspj.CurrentStartDate.Value.ToString("yyyy-MM-dd") : "");
            CurrentEndDate = (wbspj.CurrentEndDate != null ? wbspj.CurrentEndDate.Value.ToString("yyyy-MM-dd") : "");
            ForecastStartDate = (wbspj.ForecastStartDate != null ? wbspj.ForecastStartDate.Value.ToString("yyyy-MM-dd") : "");
            ForecastEndDate = (wbspj.ForecastEndDate != null ? wbspj.ForecastEndDate.Value.ToString("yyyy-MM-dd") : "");
            CurrentCost = wbspj.CurrentCost; ForecastCost = wbspj.ForecastCost; children = trn;

            //====== Jignesh-AddAddressField-21-01-2021 =======
            BillingPOCAddressLine1 = wbspj.BillingPOCAddressLine1;
            BillingPOCAddressLine2 = wbspj.BillingPOCAddressLine2;
            BillingPOCCity = wbspj.BillingPOCCity;
            BillingPOCState = wbspj.BillingPOCState;
            BillingPOCPONo = wbspj.BillingPOCPONo;
            //=================================================
        }
    }
    [DataContract]
    public class ProgramElementWBS
    {
        //[DataMember]
        //public ProgramElement WBSProgramElement;

        [DataMember]
        public String ProgramElementID;
        [DataMember]
        public String name;
        [DataMember]
        public String ProgramID;
        [DataMember]
        public String ProgramElementManager;
        [DataMember]
        public String ProgramElementSponsor;
        [DataMember]
        public String ClientProjectManager;
        [DataMember]
        public String ClientPhoneNumber;
        [DataMember]
        public String ProjectManager;
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

        //====== Nivedita-30-12-2021 =======
        [DataMember]
        public string BillingPOC;
        [DataMember]
        public string BillingPOCPhone1;
        [DataMember]
        public string BillingPOCPhone2;
        [DataMember]
        public string BillingPOCAddressLine1;
        [DataMember]
        public string BillingPOCAddressLine2;
        [DataMember]
        public string BillingPOCCity;
        [DataMember]
        public string BillingPOCState;
        [DataMember]
        public string BillingPOCPONo;
        [DataMember]
        public string BillingPOCEmail;
        [DataMember]
        public string BillingPOCSpecialInstruction;
        [DataMember]
        public byte TMBilling;
        [DataMember]
        public byte SOVBilling;
        [DataMember]
        public byte MonthlyBilling;
        [DataMember]
        public byte Lumpsum;

        //=================================================

        [DataMember]
        public String level = "ProgramElement";



        [DataMember]
        public List<ProjectWBS> children = new List<ProjectWBS>();

        public ProgramElementWBS(ProgramElement wbspe, List<ProjectWBS> prj)
        {
            ProgramElementID = wbspe.ProgramElementID.ToString(); name = wbspe.ProgramElementName; ProgramID = wbspe.Program.ProgramID.ToString(); ProgramElementManager = wbspe.ProgramElementManager; ProgramElementSponsor = wbspe.ProgramElementSponsor;
            ClientProjectManager = wbspe.ClientProjectManager; ClientPhoneNumber = wbspe.ClientPhoneNumber;
            CurrentStartDate = (wbspe.CurrentStartDate != null ? wbspe.CurrentStartDate.Value.ToString("yyyy-MM-dd") : "");
            CurrentEndDate = (wbspe.CurrentEndDate != null ? wbspe.CurrentEndDate.Value.ToString("yyyy-MM-dd") : "");
            ForecastStartDate = (wbspe.ForecastStartDate != null ? wbspe.ForecastStartDate.Value.ToString("yyyy-MM-dd") : "");
            ForecastEndDate = (wbspe.ForecastEndDate != null ? wbspe.ForecastEndDate.Value.ToString("yyyy-MM-dd") : "");
            CurrentCost = wbspe.CurrentCost; ForecastCost = wbspe.ForecastCost; children = prj;

            //====== Nivedita-30-12-2021 =======
            BillingPOC = wbspe.BillingPOC;
            BillingPOCPhone1 = wbspe.BillingPOCPhone1;
            BillingPOCPhone2 = wbspe.BillingPOCPhone2;
            BillingPOCEmail = wbspe.BillingPOCEmail;
            BillingPOCAddressLine1 = wbspe.BillingPOCAddressLine1;
            BillingPOCAddressLine2 = wbspe.BillingPOCAddressLine2;
            BillingPOCCity = wbspe.BillingPOCCity;
            BillingPOCState = wbspe.BillingPOCState;
            BillingPOCPONo = wbspe.BillingPOCPONo;
            BillingPOCSpecialInstruction = wbspe.BillingPOCSpecialInstruction;
            TMBilling = wbspe.TMBilling;
            SOVBilling = wbspe.SOVBilling;
            MonthlyBilling = wbspe.MonthlyBilling;
            Lumpsum = wbspe.Lumpsum;
            //=================================================
        }
    }
    [DataContract]
    public class ProgramWBS
    {
        //[DataMember]
        //public Program WBSProgram;

        [DataMember]
        public String ProgramID;
        [DataMember]
        public String name;
        [DataMember]
        public String ProgramManager;
        [DataMember]
        public String ProgramSponsor;

        [DataMember]
        public String ClientPOC;

        [DataMember]
        public String ClientID;

        [DataMember]
        public String ClientPhone;
        [DataMember]
        public String ClientEmail;
        [DataMember]
        public String ClientAddress;
        //====== Jignesh-AddAddressField-21-01-2021 =======
        [DataMember]
        public String ClientAddressLine1;
        [DataMember]
        public String ClientAddressLine2;
        [DataMember]
        public String ClientCity;
        [DataMember]
        public String ClientState;
        [DataMember]
        public String ClientPONo;
        //=================================================
        [DataMember]
        public string ProjectManager;
        [DataMember]
        public string ProjectManagerPhone;
        [DataMember]
        public string ProjectManagerEmail;
        [DataMember]
        public string ContractName;
        [DataMember]
        public string ContractNumber;
        [DataMember]
        public string BillingPOC;
        [DataMember]
        public string BillingPOCPhone1;
        [DataMember]
        public string BillingPOCPhone2;
        [DataMember]
        public string BillingPOCAddress;
        //====== Jignesh-AddAddressField-21-01-2021 =======
        [DataMember]
        public string BillingPOCAddressLine1;
        [DataMember]
        public string BillingPOCAddressLine2;
        [DataMember]
        public string BillingPOCCity;
        [DataMember]
        public string BillingPOCState;
        [DataMember]
        public string BillingPOCPONo;
        //=================================================
        [DataMember]
        public string BillingPOCEmail;
        [DataMember]
        public string BillingPOCSpecialInstruction;
        [DataMember]
        public byte TMBilling;
        [DataMember]
        public byte SOVBilling;
        [DataMember]
        public byte MonthlyBilling;
        [DataMember]
        public byte CertifiedPayroll;
        [DataMember]
        public byte Lumpsum;

        [DataMember]
        public String CurrentStartDate;
        [DataMember]
        public String CurrentEndDate;
        [DataMember]
        public String originalEndDate;// Aditya
        [DataMember]
        public String ForecastStartDate;
        [DataMember]
        public String ForecastEndDate;
        [DataMember]
        public String CurrentCost;
        [DataMember]
        public String ForecastCost;
        [DataMember]
        public String level = "Program";

        [DataMember]
        public List<ProgramElementWBS> children = new List<ProgramElementWBS>();
        public ProgramWBS(Program wbsprg, List<ProgramElementWBS> prge)
        {
            ProgramID = wbsprg.ProgramID.ToString();
            name = wbsprg.ProgramName;
            ProgramManager = wbsprg.ProgramManager;
            ProgramSponsor = wbsprg.ProgramSponsor;
            ClientPOC = wbsprg.ClientPOC;
            ClientID = wbsprg.ClientID.ToString();
            ClientPhone = wbsprg.ClientPhone;
            ClientEmail = wbsprg.ClientEmail;
            ClientAddress = wbsprg.ClientAddress;
            //====== Jignesh-AddAddressField-21-01-2021 =======
            ClientAddressLine1 = wbsprg.ClientAddressLine1;
            ClientAddressLine2 = wbsprg.ClientAddressLine2;
            ClientCity = wbsprg.ClientCity;
            ClientState = wbsprg.ClientState;
            ClientPONo = wbsprg.ClientPONo;
            //=================================================
            ProjectManager = wbsprg.ProjectManager;
            ProjectManagerPhone = wbsprg.ProjectManagerPhone;
            ProjectManagerEmail = wbsprg.ProjectManagerEmail;
            ContractName = wbsprg.ContractName;
            ContractNumber = wbsprg.ContractNumber;
            BillingPOC = wbsprg.BillingPOC;
            BillingPOCPhone1 = wbsprg.BillingPOCPhone1;
            BillingPOCPhone2 = wbsprg.BillingPOCPhone2;
            BillingPOCAddress = wbsprg.BillingPOCAddress;
            //====== Jignesh-AddAddressField-21-01-2021 =======
            BillingPOCAddressLine1 = wbsprg.BillingPOCAddressLine1;
            BillingPOCAddressLine2 = wbsprg.BillingPOCAddressLine2;
            BillingPOCCity = wbsprg.BillingPOCCity;
            BillingPOCState = wbsprg.BillingPOCState;
            BillingPOCPONo = wbsprg.BillingPOCPONo;
            //=================================================
            BillingPOCEmail = wbsprg.BillingPOCEmail;
            BillingPOCSpecialInstruction = wbsprg.BillingPOCSpecialInstruction;
            TMBilling = wbsprg.TMBilling;
            SOVBilling = wbsprg.SOVBilling;
            MonthlyBilling = wbsprg.MonthlyBilling;
            CertifiedPayroll = wbsprg.CertifiedPayroll;
            Lumpsum = wbsprg.Lumpsum;

            CurrentStartDate = wbsprg.CurrentStartDate.Value.ToString("yyyy-MM-dd");
            CurrentEndDate = wbsprg.CurrentEndDate.Value.ToString("yyyy-MM-dd");
            originalEndDate = wbsprg.originalEndDate.Value.ToString("yyyy-MM-dd"); //Aditya
            ForecastStartDate = wbsprg.ForecastStartDate.Value.ToString("yyyy-MM-dd");
            ForecastEndDate = wbsprg.ForecastEndDate.Value.ToString("yyyy-MM-dd");
            CurrentCost = String.Format("{0:#,###0}", wbsprg.CurrentCost);
            ForecastCost = String.Format("{0:#,###0}", wbsprg.ForecastCost);
            children = prge;
        }

        public static List<ProgramWBS> getWBSDetails(String ProgramID, String ProgramElementID, String ProjectID, String TrendNumber, String PhaseCode, String ActivityID, String BudgetCategory, String BudgetSubCategory)
        {
            List<ProgramWBS> matchedWBS = new List<ProgramWBS>();
            List<ProjectWBS> ProjectList = new List<ProjectWBS>();
            List<ProgramElementWBS> ProgramElementList = new List<ProgramElementWBS>();

            //Single Project
            if (ProjectID != "null")
            {
                ProjectList = getProjectWbS("null", ProjectID, TrendNumber, PhaseCode, ActivityID, BudgetCategory, BudgetSubCategory);
                //Get ProgramID and ProgramElementID
                List<Program> returnedPrograms = Program.getProgram("null", ProjectList[0].ProgramID, "null");
                List<ProgramElement> returnedProgramElements = ProgramElement.getProgramElement("null", ProjectList[0].ProgramElementID, "null");
                ProgramElementList.Add(new ProgramElementWBS(returnedProgramElements[0], ProjectList));
                matchedWBS.Add(new ProgramWBS(returnedPrograms[0], ProgramElementList));
            }
            //Single Program Element 
            else if (ProgramElementID != "null")
            {
                ProjectList = getProjectWbS(ProgramElementID, ProjectID, TrendNumber, PhaseCode, ActivityID, BudgetCategory, BudgetSubCategory);
                List<ProgramElement> returnedProgramElements = ProgramElement.getProgramElement("null", ProgramElementID, "null");
                List<Program> returnedPrograms = Program.getProgram("null", returnedProgramElements[0].Program.ProgramID.ToString(), "null");
                ProgramElementList.Add(new ProgramElementWBS(returnedProgramElements[0], ProjectList));
                matchedWBS.Add(new ProgramWBS(returnedPrograms[0], ProgramElementList));
            }
            //Multiple or Single Program
            else
            {
                List<Program> returnedPrograms = Program.getProgram("null", ProgramID, "null");
                foreach (Program tempProgram in returnedPrograms)
                {
                    List<ProgramElement> returnedProgramElements = ProgramElement.getProgramElement(tempProgram.ProgramID.ToString(), "null", "null");
                    ProgramElementList = new List<ProgramElementWBS>();
                    foreach (ProgramElement tempProgramElement in returnedProgramElements)
                    {
                        ProjectList = getProjectWbS(tempProgramElement.ProgramElementID.ToString(), ProjectID, TrendNumber, PhaseCode, ActivityID, BudgetCategory, BudgetSubCategory);
                        ProgramElementList.Add(new ProgramElementWBS(tempProgramElement, ProjectList));
                    }
                    matchedWBS.Add(new ProgramWBS(tempProgram, ProgramElementList));
                    //ProgramElementList.Clear();

                }
            }



            return matchedWBS;
        }

        public static List<ProjectWBS> getProjectWbS(String ProgramElementID, String ProjectID, String TrendNumber, String PhaseCode, String ActivityID, String BudgetCategory, String BudgetSubCategory)
        {
            List<ProjectWBS> ProjectList = new List<ProjectWBS>();
            List<TrendWBS> TrendList = new List<TrendWBS>();
            List<Project> returnedProjects = Project.getProject("null", ProgramElementID, ProjectID, "null");

            foreach (Project tempProject in returnedProjects)
            {
                List<Trend> returnedTrends = Trend.getTrend("null", "null", tempProject.ProjectID.ToString(), TrendNumber, "null");

                foreach (Trend tempTrend in returnedTrends)
                {
                    List<PhaseWBS> PhaseList = new List<PhaseWBS>();
                    List<Phase> returnedPhases = new List<Phase>();

                    if (TrendNumber == "null") //All trends
                        returnedPhases = Phase.getPhase(tempProject.ProjectID.ToString(), tempTrend.TrendNumber, PhaseCode, "null", "null");
                    else //Passed trend
                        returnedPhases = Phase.getPhase(tempProject.ProjectID.ToString(), TrendNumber, PhaseCode, "null", "null");

                    foreach (Phase tempPhase in returnedPhases)
                    {
                        List<ActivityWBS> ActivityList = new List<ActivityWBS>();
                        List<Activity> returnedActivities = new List<Activity>();

                        if (PhaseCode == "null") //All phases
                            returnedActivities = Activity.getActivityDetails("null", "null", tempProject.ProjectID.ToString(), tempTrend.TrendNumber, tempPhase.PhaseCode, ActivityID, BudgetCategory, BudgetSubCategory);
                        else //Passed phase
                            returnedActivities = Activity.getActivityDetails("null", "null", tempProject.ProjectID.ToString(), tempTrend.TrendNumber, PhaseCode, ActivityID, BudgetCategory, BudgetSubCategory);

                        //Add returned Activities for this Phase
                        foreach (Activity tempActivity in returnedActivities)
                        {
                            ActivityWBS temp = new ActivityWBS();
                            //temp.WBSActivity = tempActivity;

                            temp.ActivityID = tempActivity.ActivityID.ToString();
                            temp.BudgetCategory = tempActivity.BudgetCategory;
                            temp.name = tempActivity.BudgetSubCategory;
                            temp.ActivityStartDate = tempActivity.ActivityStartDate;
                            temp.ActivityEndDate = tempActivity.ActivityEndDate;
                            temp.FTECost = tempActivity.FTECost.ToString();
                            temp.LumpsumCost = tempActivity.LumpsumCost.ToString();
                            temp.UnitCost = tempActivity.UnitCost.ToString();
                            temp.PercentageBasisCost = tempActivity.PercentageBasisCost.ToString();

                            ActivityList.Add(temp);
                        }//End for each Activity

                        //Add phase to PhaseWBS
                        PhaseList.Add(new PhaseWBS(tempPhase, ActivityList));
                    }//End for each Phase

                    //Add returned Phases to this Trend

                    TrendList.Add(new TrendWBS(tempTrend, PhaseList));


                }//End for each Trend

                ProjectList.Add(new ProjectWBS(tempProject, TrendList));

            }
            return ProjectList;

        }


    }

}