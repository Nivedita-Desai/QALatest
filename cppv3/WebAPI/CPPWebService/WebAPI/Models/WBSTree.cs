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
using System.Threading.Tasks;

namespace WebAPI.Models
{

    [DataContract]
    public class FutureTrendWBSTree
    {
        //[DataMember]
        //public Trend WBSTrend;
        [DataMember]
        public int counter;
        [DataMember]
        public String name;
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
        public String TrendImpactSchedule;   //Manasi 13-07-2020
        [DataMember]
        public String TrendImpactCostSchedule;   //Manasi 13-07-2020
        [DataMember]
        public String Status;    //----Vaishnavi 30-03-2022----//

        //[DataMember] 
        //public List<PhaseWBS> children = new List<PhaseWBS>();

        public FutureTrendWBSTree(Trend wbst)
        {
            name = "Trend " + wbst.TrendNumber + " - " + wbst.TrendDescription; TrendDescription = wbst.TrendDescription;
            TrendStatus = wbst.TrendStatus.StatusDescription; TrendJustification = wbst.TrendJustification; TrendImpact = wbst.TrendImpact; CreatedOn = wbst.CreatedOn; ApprovalFrom = wbst.ApprovalFrom; ApprovalDate = (wbst.ApprovalDate != null ? wbst.ApprovalDate.Value.ToString("yyyy-MM-dd") : "");
            PostTrendStartDate = (wbst.PostTrendStartDate != null ? wbst.PostTrendStartDate.Value.ToString("yyyy-MM-dd") : "");
            PostTrendEndDate = (wbst.PostTrendEndDate != null ? wbst.PostTrendEndDate.Value.ToString("yyyy-MM-dd") : "");
            PreTrendStartDate = (wbst.PreTrendStartDate != null ? wbst.PreTrendStartDate.Value.ToString("yyyy-MM-dd") : "");
            PreTrendEndDate = (wbst.PreTrendStartDate != null ? wbst.PreTrendEndDate.Value.ToString("yyyy-MM-dd") : "");
            PreTrendCost = int.Parse(wbst.PreTrendCost);
            PostTrendCost = int.Parse(wbst.PostTrendCost);
            TrendStatusCodeID = wbst.TrendStatusCodeID; IsInternal = wbst.IsInternal;
            CostOverheadTypeID = wbst.CostOverheadTypeID;
            TrendImpactSchedule = wbst.TrendImpactSchedule;  //Manasi 13-07-2020
            TrendImpactCostSchedule = wbst.TrendImpactCostSchedule;  //Manasi 13-07-2020
            TrendImpactCostSchedule = wbst.Status;   //----Vaishnavi 30-03-2022----//
        }
    }
    [DataContract]
    public class CurrentTrendWBSTree
    {
        //[DataMember]
        //public Trend WBSTrend;
        [DataMember]
        public int counter;
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
        public String level = "CurrentTrend";

        [DataMember]
        public List<FutureTrendWBSTree> children = new List<FutureTrendWBSTree>();

        [DataMember]
        public String TrendImpactSchedule;   //Manasi 13-07-2020
        [DataMember]
        public String TrendImpactCostSchedule;   //Manasi 13-07-2020
        [DataMember]
        public String Status;   //----Vaishnavi 30-03-2022----//

        public CurrentTrendWBSTree(Trend wbst, List<FutureTrendWBSTree> phs)
        {
            name = "Trend " + wbst.TrendNumber + " - " + wbst.TrendDescription; TrendDescription = wbst.TrendDescription;
            TrendStatus = wbst.TrendStatus.StatusDescription; TrendJustification = wbst.TrendJustification; TrendImpact = wbst.TrendImpact; CreatedOn = wbst.CreatedOn; ApprovalFrom = wbst.ApprovalFrom; ApprovalDate = (wbst.ApprovalDate != null ? wbst.ApprovalDate.Value.ToString("yyyy-MM-dd") : "");
            PostTrendStartDate = (wbst.PostTrendStartDate != null ? wbst.PostTrendStartDate.Value.ToString("yyyy-MM-dd") : "");
            PostTrendEndDate = (wbst.PostTrendEndDate != null ? wbst.PostTrendEndDate.Value.ToString("yyyy-MM-dd") : "");
            PreTrendStartDate = (wbst.PreTrendStartDate != null ? wbst.PreTrendStartDate.Value.ToString("yyyy-MM-dd") : "");
            PreTrendEndDate = (wbst.PreTrendStartDate != null ? wbst.PreTrendEndDate.Value.ToString("yyyy-MM-dd") : "");
            PreTrendCost = int.Parse(wbst.PreTrendCost);
            PostTrendCost = int.Parse(wbst.PostTrendCost);
            TrendStatusCodeID = wbst.TrendStatusCodeID; IsInternal = wbst.IsInternal;
            CostOverheadTypeID = wbst.CostOverheadTypeID;
            children = phs;
            TrendImpactSchedule = wbst.TrendImpactSchedule;  //Manasi 13-07-2020
            TrendImpactCostSchedule = wbst.TrendImpactCostSchedule;  //Manasi 13-07-2020
            Status= wbst.Status;   //----Vaishnavi 30-03-2022----//
        }
    }
    [DataContract]
    public class PastTrendWBSTree
    {
        //[DataMember]
        //public Trend WBSTrend;
        [DataMember]
        public int counter;
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
        public String level = "PastTrend";

        [DataMember]
        public List<CurrentTrendWBSTree> children = new List<CurrentTrendWBSTree>();

        [DataMember]
        public String TrendImpactSchedule;   //Manasi 13-07-2020
        [DataMember]
        public String TrendImpactCostSchedule;  //Manasi 13-07-2020
        [DataMember]
        public String Status;   //----Vaishnavi 30-03-2022----//

        public PastTrendWBSTree(Trend wbst, List<CurrentTrendWBSTree> phs)
        {
            name = "Trend " + wbst.TrendNumber + " - " + wbst.TrendDescription; TrendDescription = wbst.TrendDescription;
            TrendStatus = wbst.TrendStatus.StatusDescription; TrendJustification = wbst.TrendJustification; TrendImpact = wbst.TrendImpact; CreatedOn = wbst.CreatedOn; ApprovalFrom = wbst.ApprovalFrom; ApprovalDate = (wbst.ApprovalDate != null ? wbst.ApprovalDate.Value.ToString("yyyy-MM-dd") : "");
            PostTrendStartDate = (wbst.PostTrendStartDate != null ? wbst.PostTrendStartDate.Value.ToString("yyyy-MM-dd") : "");
            PostTrendEndDate = (wbst.PostTrendEndDate != null ? wbst.PostTrendEndDate.Value.ToString("yyyy-MM-dd") : "");
            PreTrendStartDate = (wbst.PreTrendStartDate != null ? wbst.PreTrendStartDate.Value.ToString("yyyy-MM-dd") : "");
            PreTrendEndDate = (wbst.PreTrendStartDate != null ? wbst.PreTrendEndDate.Value.ToString("yyyy-MM-dd") : "");
            PreTrendCost = int.Parse(wbst.PreTrendCost);
            PostTrendCost = int.Parse(wbst.PostTrendCost);
            TrendStatusCodeID = wbst.TrendStatusCodeID; IsInternal = wbst.IsInternal;
            CostOverheadTypeID = wbst.CostOverheadTypeID;
            children = phs;
            TrendImpactSchedule = wbst.TrendImpactSchedule;  //Manasi 13-07-2020
            TrendImpactCostSchedule = wbst.TrendImpactCostSchedule;  //Manasi 13-07-2020
            Status = wbst.Status;   //----Vaishnavi 30-03-2022----//
        }
    }

    [DataContract]
    public class ProjectWBSTree
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
        public String LatLong;
        [DataMember]
        public String level = "Project";
        [DataMember]
        public String Director;
        [DataMember]
        public String Scheduler;
        [DataMember]
        public String ExecSteeringComm;
        [DataMember]
        public String VicePresident;
        [DataMember]
        public String FinancialAnalyst;
        [DataMember]
        public String CapitalProjectAssistant;
        [DataMember]
        public double FundTotal;
        [DataMember]
        public double FundUsed;
        [DataMember]
        public double FundRemained;

        [DataMember]
        public int ProjectTypeID;
        [DataMember]
        public int ProjectClassID; //ServiceID
        [DataMember]
        public string ServiceName;
        [DataMember]
        public String ProjectNumber;
        [DataMember]
        public String ContractNumber;
        [DataMember]
        public String ProjectDescription;

        [DataMember]
        public int ClientID;
        [DataMember]
        public int LocationID;
        [DataMember]
        public int ProjectManagerID;
        [DataMember]
        public int DirectorID;
        [DataMember]
        public int SchedulerID;
        [DataMember]
        public int VicePresidentID;
        [DataMember]
        public int FinancialAnalystID;
        [DataMember]
        public int CapitalProjectAssistantID;

        [DataMember]
        public String ProjectElementNumber;
        [DataMember]
        public String ClientPONumber;
        [DataMember]
        public String Amount;
        [DataMember]
        public String QuickbookJobNumber;
        [DataMember]
        public String LocationName;
        [DataMember]
        public int LineOfBusinessID;
        [DataMember]
        public String CostDescription;
        [DataMember]
        public String ScheduleDescription;
        [DataMember]
        public String ScopeQualityDescription;

        [DataMember]
        public String ProjectStartDate;
        [DataMember]
        public String ContractStartDate;


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
        public List<PastTrendWBSTree> children = new List<PastTrendWBSTree>();
        [DataMember]
        public List<ProjectScope> projectScopes;

        [DataMember]
        public string TotalUnapprovedTrends;

        [DataMember]
        public List<Int32> employeeAllowedList;

        [DataMember]
        public String Status;   //----Vaishnavi 30-03-2022----//
        public ProjectWBSTree(Project wbspj, List<PastTrendWBSTree> trn)
        {
            ProgramID = wbspj.Program.ProgramID.ToString(); ProgramElementID = wbspj.ProgramElement.ProgramElementID.ToString();
            ProjectID = wbspj.ProjectID.ToString(); name = wbspj.ProjectName; ProjectManager = wbspj.ProjectManager;
            ProjectSponsor = wbspj.ProjectSponsor; ApprovedTrendNumber = wbspj.ApprovedTrendNumber;
            CurrentStartDate = (wbspj.CurrentStartDate != null ? wbspj.CurrentStartDate.Value.ToString("yyyy-MM-dd") : "");
            CurrentEndDate = (wbspj.CurrentEndDate != null ? wbspj.CurrentEndDate.Value.ToString("yyyy-MM-dd") : "");
            ForecastStartDate = (wbspj.ForecastStartDate != null ? wbspj.ForecastStartDate.Value.ToString("yyyy-MM-dd") : "");
            ForecastEndDate = (wbspj.ForecastEndDate != null ? wbspj.ForecastEndDate.Value.ToString("yyyy-MM-dd") : "");
            CurrentCost = wbspj.CurrentCost;
            ForecastCost = wbspj.ForecastCost;
            LatLong = wbspj.LatLong;
            Director = wbspj.Director;
            Scheduler = wbspj.Scheduler;
            ExecSteeringComm = wbspj.ExecSteeringComm;
            VicePresident = wbspj.VicePresident;
            FinancialAnalyst = wbspj.FinancialAnalyst;
            CapitalProjectAssistant = wbspj.CapitalProjectAssistant;
            projectScopes = wbspj.projectScopes.ToList();
            children = trn;
            ProjectClassID = wbspj.ProjectClassID;
            ServiceName = ServiceClass.getServiceById(wbspj.ProjectClassID).Description.ToString();
            ProjectTypeID = wbspj.ProjectTypeID;
            ProjectNumber = wbspj.ProjectNumber;
            name = wbspj.ProjectName;
            ProjectName = wbspj.ProjectName;
            ContractNumber = wbspj.ContractNumber;
            ProjectDescription = wbspj.ProjectDescription;

            BillingPOC = wbspj.BillingPOC;
            BillingPOCPhone1 = wbspj.BillingPOCPhone1;
            BillingPOCPhone2 = wbspj.BillingPOCPhone2;
            BillingPOCAddress = wbspj.BillingPOCAddress;
            //====== Jignesh-AddAddressField-21-01-2021 =======
            BillingPOCAddressLine1 = wbspj.BillingPOCAddressLine1;
            BillingPOCAddressLine2 = wbspj.BillingPOCAddressLine2;
            BillingPOCCity = wbspj.BillingPOCCity;
            BillingPOCState = wbspj.BillingPOCState;
            BillingPOCPONo = wbspj.BillingPOCPONo;
            //=================================================
            BillingPOCEmail = wbspj.BillingPOCEmail;
            BillingPOCSpecialInstruction = wbspj.BillingPOCSpecialInstruction;
            TMBilling = wbspj.TMBilling;
            SOVBilling = wbspj.SOVBilling;
            MonthlyBilling = wbspj.MonthlyBilling;
            CertifiedPayroll = wbspj.CertifiedPayroll;
            Lumpsum = wbspj.Lumpsum;


            ClientID = wbspj.ClientID;
            LocationID = wbspj.LocationID;
            ProjectManagerID = wbspj.ProjectManagerID;
            DirectorID = wbspj.DirectorID;
            SchedulerID = wbspj.SchedulerID;
            VicePresidentID = wbspj.VicePresidentID;
            FinancialAnalystID = wbspj.FinancialAnalystID;
            CapitalProjectAssistantID = wbspj.CapitalProjectAssistantID;

            ProjectElementNumber = wbspj.ProjectElementNumber;
            ClientPONumber = wbspj.ClientPONumber;
            Amount = wbspj.Amount;
            QuickbookJobNumber = wbspj.QuickbookJobNumber;
            LocationName = wbspj.LocationName;

            CostDescription = wbspj.CostDescription;
            ScheduleDescription = wbspj.ScheduleDescription;
            ScopeQualityDescription = wbspj.ScopeQualityDescription;
            employeeAllowedList = wbspj.employeeAllowedList.ToList();

            ProjectStartDate = wbspj.ProjectStartDate;
            ContractStartDate = wbspj.ContractStartDate;
            LineOfBusinessID = wbspj.LineOfBusinessID;

            TotalUnapprovedTrends = wbspj.TotalUnapprovedTrends;
            Status = wbspj.Status;   //----Vaishnavi 30-03-2022----//
        }
    }
    [DataContract]
    public class ProgramElementWBSTree
    {
        //[DataMember]
        //public ProgramElement WBSProgramElement;

        [DataMember]
        public String ProgramElementID;
        [DataMember]
        public String name;
        [DataMember]
        public String ProgramElementName;
        [DataMember]
        public string ProgramElementNumber;
        [DataMember]
        public String ProgramID;
        [DataMember]
        public String ProgramElementManager;
        [DataMember]
        public String ProgramElementSponsor;
        [DataMember]
        public int ProgramElementManagerID;
        [DataMember]
        public int ProgramElementSponsorID;
      
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
        public String level = "ProgramElement";

        [DataMember]
        public String ClientProjectManager;
        [DataMember]
        public String ClientPhoneNumber;

        [DataMember]
        public int ProjectTypeID;
        [DataMember]
        public int ProjectClassID;
        [DataMember]
        public string ProjectClassName;
        [DataMember]
        public String ProjectNumber;
        [DataMember]
        public String ContractNumber;

        [DataMember]
        public int ClientID;
        [DataMember]
        public int LocationID;
        [DataMember]
        public int ProjectManagerID;
        [DataMember]
        public int DirectorID;
        [DataMember]
        public int SchedulerID;
        [DataMember]
        public int VicePresidentID;
        [DataMember]
        public int FinancialAnalystID;
        [DataMember]
        public int CapitalProjectAssistantID;

        [DataMember]
        public String CostDescription;
        [DataMember]
        public String ScheduleDescription;
        [DataMember]
        public String ScopeQualityDescription;

        [DataMember]
        public String ProjectStartDate;

        //==== Jignesh-25-02-2021 ==========
        [DataMember]
        public String ProjectPStartDate;
        [DataMember]
        public String ProjectPODate;
        [DataMember]
        public String ProjectPEndDate;

        //================================

        [DataMember]
        public String ContractStartDate;


        [DataMember]
        public String LocationName;
        [DataMember]
        public String ContractEndDate;
        [DataMember]
        public int ContractID;
        [DataMember]
        public String ProjectValueContract;
        [DataMember]
        public String ProjectValueTotal;

        [DataMember]
        public string BillingPOC;
        [DataMember]
        public string BillingPOCPhone1;
        [DataMember]
        public string BillingPOCPhone2;
        [DataMember]
        public string BillingPOCAddress;
       
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
        public byte CertifiedPayroll;
        [DataMember]
        public byte Lumpsum;
        //=================================================
        //Nivedita 04-01-2021
        [DataMember]
        public String ClientPONumber;
        [DataMember]
        public List<ProjectWBSTree> children = new List<ProjectWBSTree>();
        [DataMember]
        public String Status;    //----Vaishnavi 30-03-2022----//
        public ProgramElementWBSTree(ProgramElement wbspe, List<ProjectWBSTree> prj, int uId)
        {
            ProgramElementID = wbspe.ProgramElementID.ToString(); name = wbspe.ProgramElementName; ProgramID = wbspe.Program.ProgramID.ToString();
            ProgramElementManager = wbspe.ProgramElementManager; ProgramElementSponsor = wbspe.ProgramElementSponsor;
            ProgramElementManagerID = wbspe.ProgramElementManagerID; ProgramElementSponsorID = wbspe.ProgramElementSponsorID;
           
            ClientID = wbspe.ClientID; LocationID = wbspe.LocationID; ProjectManagerID = wbspe.ProjectManagerID; DirectorID = wbspe.DirectorID; SchedulerID = wbspe.SchedulerID;
            VicePresidentID = wbspe.VicePresidentID; FinancialAnalystID = wbspe.FinancialAnalystID; CapitalProjectAssistantID = wbspe.CapitalProjectAssistantID;
            CostDescription = wbspe.CostDescription; ScheduleDescription = wbspe.ScheduleDescription; ScopeQualityDescription = wbspe.ScopeQualityDescription;
            ProjectStartDate = wbspe.ProjectStartDate; ContractStartDate = wbspe.ContractStartDate;
            ProjectNumber = wbspe.ProjectNumber; ProjectTypeID = wbspe.ProjectTypeID; ProjectClassID = wbspe.ProjectClassID; ContractNumber = wbspe.ContractNumber;
            LocationName = wbspe.LocationName; ContractEndDate = wbspe.ContractEndDate; ContractID = wbspe.ContractID; ProjectValueContract = wbspe.ProjectValueContract; ProjectValueTotal = wbspe.ProjectValueTotal;
            name = wbspe.ProgramElementName;//Drew
            ProgramElementName = wbspe.ProgramElementName;
            ClientProjectManager = wbspe.ClientProjectManager;
			ProjectClassName = ProjectClass.getProjectClassById(wbspe.ProjectClassID).ProjectClassName.ToString();
            ProgramElementNumber = wbspe.ProgramElementNumber;

            // Jignesh-25-02-2021
            ProjectPStartDate = (wbspe.ProjectPStartDate != null ? wbspe.ProjectPStartDate.Value.ToString("yyyy-MM-dd") : "");
            ProjectPEndDate = (wbspe.ProjectPEndDate != null ? wbspe.ProjectPEndDate.Value.ToString("yyyy-MM-dd") : "");
            ProjectPODate = (wbspe.ProjectPODate != null ? wbspe.ProjectPODate.Value.ToString("yyyy-MM-dd") : "");

            ClientPhoneNumber = wbspe.ClientPhoneNumber;
            CurrentStartDate = (wbspe.CurrentStartDate != null ? wbspe.CurrentStartDate.Value.ToString("yyyy-MM-dd") : "");
            CurrentEndDate = (wbspe.CurrentEndDate != null ? wbspe.CurrentEndDate.Value.ToString("yyyy-MM-dd") : "");
            ForecastStartDate = (wbspe.ForecastStartDate != null ? wbspe.ForecastStartDate.Value.ToString("yyyy-MM-dd") : "");
            ForecastEndDate = (wbspe.ForecastEndDate != null ? wbspe.ForecastEndDate.Value.ToString("yyyy-MM-dd") : "");
            CurrentCost = wbspe.CurrentCost; ForecastCost = wbspe.ForecastCost;
            BillingPOC = wbspe.BillingPOC;
            BillingPOCPhone1 = wbspe.BillingPOCPhone1;
            BillingPOCPhone2 = wbspe.BillingPOCPhone2;
            
            BillingPOCAddressLine1 = wbspe.BillingPOCAddressLine1;
            BillingPOCAddressLine2 = wbspe.BillingPOCAddressLine2;
            BillingPOCCity = wbspe.BillingPOCCity;
            BillingPOCState = wbspe.BillingPOCState;
            BillingPOCPONo = wbspe.BillingPOCPONo;
            
            BillingPOCEmail = wbspe.BillingPOCEmail;
            BillingPOCSpecialInstruction = wbspe.BillingPOCSpecialInstruction;
            TMBilling = wbspe.TMBilling;
            SOVBilling = wbspe.SOVBilling;
            MonthlyBilling = wbspe.MonthlyBilling;
            CertifiedPayroll = wbspe.CertifiedPayroll;
            Lumpsum = wbspe.Lumpsum;

            ClientPONumber = wbspe.ClientPONumber;
            Status = wbspe.Status;   //----Vaishnavi 30-03-2022----//

            // Jignesh-21-10-2021
            List<ProjectAccessControl> projectAccessControlsList = ProjectAccessControl.GetContractModificationList(uId);
            foreach (ProjectAccessControl item in projectAccessControlsList)
            {
                if (item.ProgramElementID == wbspe.ProgramElementID)
                {
                    if (item.IsProgramEleCreator == true)
                    {
                        children = prj;
                        break;
                    }
                    else
                    {
                        foreach (var project in prj)
                        {
                            if (item.IsProjectCreator == true || item.IsProjectApprover == true || item.IsAllowedUser == true)
                            {
                                if (Convert.ToInt32(project.ProjectID) == item.ProjectID)
                                {
                                    children.Add(project);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
    [DataContract]
    public class ProgramWBSTree
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
        public int ProgramManagerID;
        [DataMember]
        public int ProgramSponsorID;
        [DataMember]
        public int ProjectClassID;
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
        //Aditya 21022022
        [DataMember]
        public String originalEndDate;
        [DataMember]
        public String level = "Program";
        [DataMember]
        public List<ProgramFund> programFunds;
        [DataMember]
        public List<ProgramCategory> programCategories;

        [DataMember]
        public String ContractValue;   //Manasi 14-07-2020

        [DataMember]
        public String JobNumber;  //Manasi  04-08-2020

        [DataMember]       
        public List<int> CertifiedPayrollIDS;     //Vaishnavi 12-04-2022

        [DataMember]
        public String IsCertifiedPayrollChecked;

        [DataMember]
        public List<String> ProgramPrevailingWagesList;

        [DataMember]
        public String IsPrevailingWageChecked;

        [DataMember]
        public List<int> WrapIDS;
        [DataMember]
        public String IsWrapChecked;    //Vaishnavi 12-04-2022

        [DataMember]
        public String IsPPBond;
        [DataMember]
        public String PrimeSubPrime;
        [DataMember]
        public String PrimeParent;
        [DataMember]
        public String IsCostPartOfContract;
        [DataMember]
        public String PPBondNotes;
        [DataMember]
        public String ProgramNote;

        [DataMember]
        public String preliminaryNoticeDate;

        [DataMember]
        public String preliminaryNoticeReason;

        [DataMember]
        public  List<ProgramNotes> programnotesList;

        [DataMember]
        public List<PrelimnaryNotice> prelimnaryNoticeList;

        [DataMember]
        public String LaborWarranty;     //Vaishnavi 12-04-2022
        [DataMember]
        public String MaterialsWarranty;
        [DataMember]
        public String OtherWarranty;
        [DataMember]
        public String LaborStartDate;
        [DataMember]
        public String LaborEndDate;
        [DataMember]
        public String MaterialsStartDate;
        [DataMember]
        public String MaterialsEndDate;
        [DataMember]
        public String OtherStartDate;
        [DataMember]
        public String OtherEndDate;
        [DataMember]
        public String LaborDescription;
        [DataMember]
        public String MaterialsDescription;
        [DataMember]
        public String OtherDescription;

        [DataMember]
        public ProgramWarranty LaborWarrantyList;
        [DataMember]
        public ProgramWarranty MaterialsWarrantyList;
        [DataMember]
        public ProgramWarranty OtherWarrantyList;    //Vaishnavi 12-04-2022

        [DataMember]
        public List<ProgramElementWBSTree> children = new List<ProgramElementWBSTree>();

        [DataMember]
        public String Status;   //----Vaishnavi 30-03-2022----//

        //Aditya PMDD 05052022
        //[DataMember]
        //public List<ContractProjectManager> ContractProjectManager;
        //Aditya PMDD 05052022
        [DataMember]
        public List<ContractProjectManager> PManagerIDS;


        [DataMember]
        public String ReportingTo;    //Vaishnavi 12-04-2022
        public ProgramWBSTree(Project proj, Program wbsprg, List<ProgramElementWBSTree> prge)
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
            ContractValue = wbsprg.ContractValue;   //Manasi 14-07-2020
            JobNumber = wbsprg.JobNumber;   //Manasi  04-08-2020
            CPPDbContext ctx = new CPPDbContext();      //Vaishnavi 12-04-2022
            CertifiedPayrollIDS = ctx.ProgramCertifiedPayroll.Where(c => c.ProgramID == wbsprg.ProgramID).Select(x => x.CertifiedPayrollID).ToList();
            IsCertifiedPayrollChecked = wbsprg.IsCertifiedPayrollChecked;
            ProgramPrevailingWagesList = ctx.ProgramPrevailingWage.Where(c => c.ProgramID == wbsprg.ProgramID).Select(x => x.Description).ToList();
            IsPrevailingWageChecked = wbsprg.IsPrevailingWageChecked;
            WrapIDS = ctx.ProgramWrap.Where(c => c.ProgramID == wbsprg.ProgramID).Select(x => x.WrapID).ToList();
            IsWrapChecked = wbsprg.IsWrapChecked;
            ReportingTo = wbsprg.ReportingTo;      //Vaishnavi 12-04-2022
            IsPPBond = wbsprg.IsPPBond;
            IsCostPartOfContract = wbsprg.IsCostPartOfContract;
            PPBondNotes = wbsprg.PPBondNotes;
            //List<ProgramNotes> ProgramNotesList = new List<ProgramNotes>();
            programnotesList = ProgramNotes.getProgramNotes(Convert.ToInt32(ProgramID));
            ProgramNote = programnotesList.Max(m => m.notes_desc);
            //ProgramNote = LatestNote.ToString(;

            //prelimnaryNoticeList = PrelimnaryNotice.GetPrelimnaryNoticeList(Convert.ToInt32(ProgramID));
            
            //if (prelimnaryNoticeList.Count > 0)
            //{
            //    preliminaryNoticeDate = prelimnaryNoticeList.Max(m => m.Date).ToString("yyyy-MM-dd");
            //    preliminaryNoticeReason = prelimnaryNoticeList.Max(m => m.Reason);
            //}
            //else
            //{
            //    preliminaryNoticeDate = "";
            //    preliminaryNoticeReason = "";
            //}
             


            PrimeSubPrime = wbsprg.PrimeSubPrime;
            PrimeParent = wbsprg.PrimeParent;
            PManagerIDS = ctx.ContractProjectManagers.Where(w => w.ProgramId == wbsprg.ProgramID).ToList();  //Aditya PMDD 05052022
            //ContractProjectManager = ctx.ContractProjectManagers.Where(w => w.ProgramId == wbsprg.ProgramID).ToList();

            //LaborWarranty = ctx.ProgramWarranty.Where(w => w.ProgramID == wbsprg.ProgramID && w.WarrantyType == "Labor Warranty").Select(p => p.WarrantyDescription).FirstOrDefault();    //Vaishnavi 12-04-2022
            //MaterialsWarranty = ctx.ProgramWarranty.Where(w => w.ProgramID == wbsprg.ProgramID && w.WarrantyType == "Materials Warranty").Select(p => p.WarrantyDescription).FirstOrDefault();
            //OtherWarranty = ctx.ProgramWarranty.Where(w => w.ProgramID == wbsprg.ProgramID && w.WarrantyType == "Other Warranty").Select(p => p.WarrantyDescription).FirstOrDefault();

            //wbsprg.LaborStartDate = ctx.ProgramWarranty.Where(w => w.ProgramID == wbsprg.ProgramID && w.WarrantyType == "Labor Warranty").Select(p => p.StartDate).FirstOrDefault();
            //wbsprg.MaterialsStartDate = ctx.ProgramWarranty.Where(w => w.ProgramID == wbsprg.ProgramID && w.WarrantyType == "Materials Warranty").Select(p => p.StartDate).FirstOrDefault();
            //wbsprg.OtherStartDate = ctx.ProgramWarranty.Where(w => w.ProgramID == wbsprg.ProgramID && w.WarrantyType == "Other Warranty").Select(p => p.StartDate).FirstOrDefault();
            //wbsprg.LaborEndDate = ctx.ProgramWarranty.Where(w => w.ProgramID == wbsprg.ProgramID && w.WarrantyType == "Labor Warranty").Select(p => p.EndDate).FirstOrDefault();
            //wbsprg.MaterialsEndDate = ctx.ProgramWarranty.Where(w => w.ProgramID == wbsprg.ProgramID && w.WarrantyType == "Materials Warranty").Select(p => p.EndDate).FirstOrDefault();
            //wbsprg.OtherEndDate = ctx.ProgramWarranty.Where(w => w.ProgramID == wbsprg.ProgramID && w.WarrantyType == "Other Warranty").Select(p => p.EndDate).FirstOrDefault();

            //LaborStartDate = (wbsprg.LaborStartDate != null ? wbsprg.LaborStartDate.Value.ToString("yyyy-MM-dd") : "");
            //MaterialsStartDate = (wbsprg.MaterialsStartDate != null ? wbsprg.MaterialsStartDate.Value.ToString("yyyy-MM-dd") : "");
            //OtherStartDate = (wbsprg.OtherStartDate != null ? wbsprg.OtherStartDate.Value.ToString("yyyy-MM-dd") : "");
            //LaborEndDate = (wbsprg.LaborEndDate != null ? wbsprg.LaborEndDate.Value.ToString("yyyy-MM-dd") : "");
            //MaterialsEndDate = (wbsprg.MaterialsEndDate != null ? wbsprg.MaterialsEndDate.Value.ToString("yyyy-MM-dd") : "");
            //OtherEndDate = (wbsprg.OtherEndDate != null ? wbsprg.OtherEndDate.Value.ToString("yyyy-MM-dd") : "");

            //LaborDescription = ctx.ProgramWarranty.Where(w => w.ProgramID == wbsprg.ProgramID && w.WarrantyType == "Labor Warranty").Select(p => p.Description).FirstOrDefault();
            //MaterialsDescription = ctx.ProgramWarranty.Where(w => w.ProgramID == wbsprg.ProgramID && w.WarrantyType == "Materials Warranty").Select(p => p.Description).FirstOrDefault();
            //OtherDescription = ctx.ProgramWarranty.Where(w => w.ProgramID == wbsprg.ProgramID && w.WarrantyType == "Other Warranty").Select(p => p.Description).FirstOrDefault();     //Vaishnavi 12-04-2022



            originalEndDate = (wbsprg.originalEndDate != null ? wbsprg.originalEndDate.Value.ToString("yyyy-MM-dd") : ""); // Aditya 21022022
            ProgramManagerID = wbsprg.ProgramManagerID; ProgramSponsorID = wbsprg.ProgramSponsorID; ProjectClassID = wbsprg.ProjectClassID;
            CurrentStartDate = (wbsprg.CurrentStartDate != null ? wbsprg.CurrentStartDate.Value.ToString("yyyy-MM-dd") : "");
            CurrentEndDate = (wbsprg.CurrentEndDate != null ? wbsprg.CurrentEndDate.Value.ToString("yyyy-MM-dd") : "");
            ForecastStartDate = (wbsprg.ForecastStartDate != null ? wbsprg.ForecastStartDate.Value.ToString("yyyy-MM-dd") : "");
            ForecastEndDate = (wbsprg.ForecastEndDate != null ? wbsprg.ForecastEndDate.Value.ToString("yyyy-MM-dd") : "");
            CurrentCost = String.Format("{0:#,###0}", wbsprg.CurrentCost);
            ForecastCost = String.Format("{0:#,###0}", wbsprg.ForecastCost);
            //programFunds = wbsprg.programFunds.ToList(); programCategories = wbsprg.programCategories.ToList();
            children = prge;
            Status= wbsprg.Status;   //----Vaishnavi 30-03-2022----//

        }

        public static List<ProgramWBSTree> getWBSTreeDetails(int uId, String OrganizationID, String ProgramID, String ProgramElementID, String ProjectID,
            String TrendNumber, String PhaseCode, String ActivityID, String BudgetCategory, String BudgetSubCategory,string AllData = "null", String DeptID = "null")
        {
            List<ProgramWBSTree> matchedWBSTree = new List<ProgramWBSTree>();
            List<ProjectWBSTree> ProjectList = new List<ProjectWBSTree>();
            List<ProgramElementWBSTree> ProgramElementList = new List<ProgramElementWBSTree>();
            
            //Single Project
            if (ProjectID != "null")
            {
                ProjectList = getProjectWbSTree("null", ProjectID, TrendNumber, PhaseCode, ActivityID
                    , BudgetCategory, BudgetSubCategory, AllData);
                //Get ProgramID and ProgramElementID
                List<Program> returnedPrograms = Program.getProgram(OrganizationID, ProjectList[0].ProgramID, "null");
                //List<Project> returnProjects = Project.getProject( ProjectList[0].ProgramID,  ProjectList[0].ProgramElementID, ProjectList[0].ProjectID, "null");
                List<ProgramElement> returnedProgramElements = ProgramElement.getProgramElement("null", ProjectList[0].ProgramElementID, "null");
                ProgramElementList.Add(new ProgramElementWBSTree(returnedProgramElements[0], ProjectList, uId));
                matchedWBSTree.Add(new ProgramWBSTree(null, returnedPrograms[0], ProgramElementList));
            }
            //Single Program Element 
            else if (ProgramElementID != "null")
            {
                ProjectList = getProjectWbSTree(ProgramElementID, ProjectID, TrendNumber, PhaseCode, ActivityID
                    , BudgetCategory, BudgetSubCategory, AllData);
                //   List<Project> returnProjects = Project.getProject(ProjectList[0].ProgramID, ProjectList[0].ProgramElementID, ProjectList[0].ProjectID, "null");
                List<ProgramElement> returnedProgramElements = ProgramElement.getProgramElement("null", ProgramElementID, "null");
                List<Program> returnedPrograms = Program.getProgram(OrganizationID, returnedProgramElements[0].Program.ProgramID.ToString(), "null");
                ProgramElementList.Add(new ProgramElementWBSTree(returnedProgramElements[0], ProjectList, uId));
                matchedWBSTree.Add(new ProgramWBSTree(null, returnedPrograms[0], ProgramElementList));
            }
            //Multiple or Single Program
            else
            {


                List<Program> returnedPrograms = Program.getProgram(OrganizationID, ProgramID, "null");
                List<ProgramElement> allProgramElements =  ProgramElement.getProgramElement(null, null, null);
                List<Project> allProjects = Project.getProject(null, null, null, null);

                //Parallel.Invoke(()=> 
                //{
                //    returnedPrograms = Program.getProgram(OrganizationID, ProgramID, "null");
                //},()=>
                //{
                //    allProgramElements = ProgramElement.getProgramElement(null, null, null);
                //},()=>
                //{
                //    allProjects = Project.getProject(null, null, null, null);
                //});

                foreach (Program tempProgram in returnedPrograms)
                {
                    List<ProgramElement> returnedProgramElements = new List<ProgramElement>();//ProgramElement.getProgramElement(tempProgram.ProgramID.ToString(), "null", "null");
                    foreach(ProgramElement pe in allProgramElements)
                    {
                        if (DeptID != "null" && DeptID != "undefined")
                        {
                            if (pe.ProgramID == tempProgram.ProgramID && pe.ProjectClassID == Convert.ToInt32(DeptID))
                            {
                                returnedProgramElements.Add(pe);
                            }
                        }
                        else
                        {
                            if (pe.ProgramID == tempProgram.ProgramID)
                            {
                                returnedProgramElements.Add(pe);
                            }
                        }
                    }
                    // List<Project> returnProjects = Project.getProject(ProjectList[0].ProgramID, ProjectList[0].ProgramElementID, ProjectList[0].ProjectID, "null");
                    ProgramElementList = new List<ProgramElementWBSTree>();
                    
                    foreach (ProgramElement tempProgramElement in returnedProgramElements)
                    {
                        //ProjectList = getProjectWbSTree(tempProgramElement.ProgramElementID.ToString(), ProjectID, TrendNumber, PhaseCode, ActivityID
                        //    , BudgetCategory, BudgetSubCategory, AllData);
                        ProjectList = new List<ProjectWBSTree>();
                        foreach (Project tempProject in allProjects)
                        {
                            if (tempProject.ProgramElementID == tempProgramElement.ProgramElementID)
                            {
                                tempProject.CurrentCost = Math.Round(Convert.ToDouble(tempProject.CurrentCost), 2).ToString("F");
                                var tNumber = new List<string> { "1000", "2000", "3000" };

                                List<Trend> returnedTrends = Trend.getTrend("null", "null", tempProject.ProjectID.ToString(), TrendNumber, "null")
                                    .Where(x => !tNumber.Contains(x.TrendNumber) && x.TrendStatus.StatusDescription.Equals("Pending")).ToList();

                                tempProject.TotalUnapprovedTrends = returnedTrends.Count().ToString();

                                if (AllData == "0")
                                {
                                    if (tempProject.TotalUnapprovedTrends != "0")
                                        ProjectList.Add(new ProjectWBSTree(tempProject, null));
                                }
                                else
                                {
                                    ProjectList.Add(new ProjectWBSTree(tempProject, null));
                                }
                            }
                        }

                        tempProgramElement.CurrentCost = Math.Round(Convert.ToDouble(tempProgramElement.CurrentCost), 2).ToString("F"); ; //luan here fix floating point

                        ProgramElementList.Add(new ProgramElementWBSTree(tempProgramElement, ProjectList, uId));
                    }

                    tempProgram.CurrentCost = Math.Round(Convert.ToDouble(tempProgram.CurrentCost), 2).ToString("F"); //luan here fix floating point
                    matchedWBSTree.Add(new ProgramWBSTree(null, tempProgram, ProgramElementList));
                    //ProgramElementList.Clear();

                }


                //foreach (Program tempProgram in returnedPrograms)
                //{
                //    List<ProgramElement> returnedProgramElements = ProgramElement.getProgramElement(tempProgram.ProgramID.ToString(), "null", "null");
                //    // List<Project> returnProjects = Project.getProject(ProjectList[0].ProgramID, ProjectList[0].ProgramElementID, ProjectList[0].ProjectID, "null");
                //    ProgramElementList = new List<ProgramElementWBSTree>();
                //    ProjectList = new List<ProjectWBSTree>();
                //    foreach (ProgramElement tempProgramElement in returnedProgramElements)
                //    {
                //        ProjectList = getProjectWbSTree(tempProgramElement.ProgramElementID.ToString(), ProjectID, TrendNumber, PhaseCode, ActivityID
                //            , BudgetCategory, BudgetSubCategory, AllData);

                //        tempProgramElement.CurrentCost = Math.Round(Convert.ToDouble(tempProgramElement.CurrentCost), 2).ToString("F"); ; //luan here fix floating point

                //        ProgramElementList.Add(new ProgramElementWBSTree(tempProgramElement, ProjectList, uId));
                //    }

                //    tempProgram.CurrentCost = Math.Round(Convert.ToDouble(tempProgram.CurrentCost), 2).ToString("F"); //luan here fix floating point

                //    matchedWBSTree.Add(new ProgramWBSTree(null, tempProgram, ProgramElementList));
                //    //ProgramElementList.Clear();

                //}
            }



            return matchedWBSTree;
        }



        public static List<ProjectWBSTree> getProjectWbSTree(String ProgramElementID, String ProjectID, String TrendNumber,
            String PhaseCode, String ActivityID, String BudgetCategory, String BudgetSubCategory, string AllData = "null")
        {
            List<ProjectWBSTree> ProjectList = new List<ProjectWBSTree>();
            List<PastTrendWBSTree> PastTrendList = new List<PastTrendWBSTree>();
            int PastTrendCount, FutureTrendCount;
            List<CurrentTrendWBSTree> CurrentTrendList = new List<CurrentTrendWBSTree>();
            List<FutureTrendWBSTree> FutureTrendList = new List<FutureTrendWBSTree>();
            List<Project> returnedProjects = Project.getProject("null", ProgramElementID, ProjectID, "null");
            Trend CurrentTrend = null;

            foreach (Project tempProject in returnedProjects)
            {
                /*List<Trend> returnedTrends = Trend.getTrend("null", "null", tempProject.ProjectID, TrendNumber, "null");
                PastTrendCount = 0;
                FutureTrendCount = 0;
                foreach (Trend tempTrend in returnedTrends)
                {


                    if (tempTrend.TrendStatus.Equals("Pending"))
                    {
                        FutureTrendCount += 1;
                        FutureTrendWBSTree tempFutureTrend = new FutureTrendWBSTree(tempTrend);
                        tempFutureTrend.counter = FutureTrendCount;
                        FutureTrendList.Add(tempFutureTrend);
                    }
                    else if (tempProject.ApprovedTrendNumber == tempTrend.TrendNumber)
                    {
                        CurrentTrend = tempTrend;
                    }
                    else
                    {
                        PastTrendCount += 1;
                        PastTrendWBSTree tempPastTrend = new PastTrendWBSTree(tempTrend, null);
                        tempPastTrend.counter = PastTrendCount;
                        PastTrendList.Add(tempPastTrend);
                    }
                }//End for each Trend
                Trend PlaceHolderTrend = new Trend();

                /*ADD DT ROW ID. FOR PLACEHOLDER TREND, 0 + MAX ROW ID / 2 

                PlaceHolderTrend.TrendDescription = "Pending Trends";
                FutureTrendWBSTree FuturePlaceHolder = new FutureTrendWBSTree(PlaceHolderTrend);
                FuturePlaceHolder.level = "Trend";
                FuturePlaceHolder.counter = FutureTrendCount / 2;
                FutureTrendList.Add(FuturePlaceHolder);
                FutureTrendList = FutureTrendList.OrderBy(a => a.counter).ToList<FutureTrendWBSTree>(); 
                
                PlaceHolderTrend.TrendDescription = "Current Trend";
                CurrentTrendWBSTree CurrentPlaceHolder = new CurrentTrendWBSTree(PlaceHolderTrend, FutureTrendList);
                CurrentPlaceHolder.level = "Trend";
                CurrentTrendList.Add(new CurrentTrendWBSTree(CurrentTrend, null));
                CurrentTrendList.Add(CurrentPlaceHolder);

                PlaceHolderTrend.TrendDescription = "Past Trends";
                PastTrendWBSTree PastPlaceHolder = new PastTrendWBSTree(PlaceHolderTrend, CurrentTrendList);
                PastPlaceHolder.level = "Trend";
                PastPlaceHolder.counter = PastTrendCount / 2;
                PastTrendList.Add(PastPlaceHolder);
                PastTrendList = PastTrendList.OrderBy(a => a.counter).ToList<PastTrendWBSTree>();*/

                tempProject.CurrentCost = Math.Round(Convert.ToDouble(tempProject.CurrentCost), 2).ToString("F"); ;	//luan here fix floating point

                //ProjectList.Add(new ProjectWBSTree(tempProject, PastTrendList));
                //

                var tNumber = new List<string> { "1000", "2000", "3000" };

                List<Trend> returnedTrends = Trend.getTrend("null", "null", tempProject.ProjectID.ToString(), TrendNumber, "null")
                    .Where(x=> !tNumber.Contains(x.TrendNumber) && x.TrendStatus.StatusDescription.Equals("Pending")).ToList();

                tempProject.TotalUnapprovedTrends = returnedTrends.Count().ToString();

                if (AllData == "0")
                {
                    if(tempProject.TotalUnapprovedTrends != "0")
                    ProjectList.Add(new ProjectWBSTree(tempProject, null));
                }
                else { 
                ProjectList.Add(new ProjectWBSTree(tempProject, null));
                }

            }
            return ProjectList;

        }


    }

}