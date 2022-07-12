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
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    [Table("project")]
    public class Project : Audit
    {
        [NotMapped]
        public int Operation;
        public bool isModified;
        public List<ProjectScope> scopeToBeDeleted;
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProjectID { get; set; }
        [NotMapped]
        
        public List<TrendApproversDetails> ApproversDetails { get; set; }
        public string ProjectName { get; set; }
        public string ProjectManager { get; set; }
        public string ProjectSponsor { get; set; }
        public string ApprovedTrendNumber { get; set; }
        public DateTime? CurrentStartDate { get; set; }
        public DateTime? CurrentEndDate { get; set; }
        public DateTime? ForecastStartDate { get; set; }
        public DateTime? ForecastEndDate { get; set; }
        public string CurrentCost { get; set; }
        public string ForecastCost { get; set; }
        public string Director { get; set; }
        public string Scheduler { get; set; }
        public string ExecSteeringComm { get; set; }
        public string VicePresident { get; set; }
        public string FinancialAnalyst { get; set; }
        public string CapitalProjectAssistant { get; set; }
        public string OrganizationID { get; set; }
        public int ProgramID { get; set; }

		//From the point of turning into Project Element - START
		public string ClientPONumber { get; set; }
        public string Amount { get; set; }
        public string QuickbookJobNumber { get; set; }
		public string LocationName { get; set; }
		//From the point of turning into Project Element - END

		public int ProjectTypeID { get; set; }
        public int ProjectClassID { get; set; }
        public string ProjectNumber { get; set; }
        public string ProjectElementNumber { get; set; }
        public string ProjectDescription { get; set; }

        public string ContractNumber { get; set; }
        public string ProjectStartDate { get; set; }
        public string ContractStartDate { get; set; }
      
        public int ClientID { get; set; }

        public int LocationID { get; set; }

        public int ProjectManagerID { get; set; }
        public int DirectorID { get; set; }
        public int SchedulerID { get; set; }
        public int VicePresidentID { get; set; }
        public int FinancialAnalystID { get; set; }
        public int CapitalProjectAssistantID { get; set; }

        public string CostDescription { get; set; }
        public string ScheduleDescription { get; set; }
        public string ScopeQualityDescription { get; set; }

        public int LineOfBusinessID { get; set; }

        public string BillingPOC { get; set; }
        public string BillingPOCPhone1 { get; set; }
        public string BillingPOCPhone2 { get; set; }
        public string BillingPOCAddress { get; set; }
        //====== Jignesh-AddAddressField-21-01-2021 =======
        public string BillingPOCAddressLine1 { get; set; }
        public string BillingPOCAddressLine2 { get; set; }
        public string BillingPOCCity { get; set; }
        public string BillingPOCState { get; set; }
        public string BillingPOCPONo { get; set; }
        //=================================================
        public string BillingPOCEmail { get; set; }
        public string BillingPOCSpecialInstruction { get; set; }
        public byte TMBilling { get; set; }
        public byte SOVBilling { get; set; }
        public byte MonthlyBilling { get; set; }
        public byte CertifiedPayroll { get; set; }
        public byte Lumpsum { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string DeletedBy { get; set; }
        public string Status { get; set; }   //----Vaishnavi 30-03-2022----//

        [ForeignKey("ProjectTypeID")]
        public virtual ProjectType ProjectType { get; set; }

        [ForeignKey("ProjectClassID")]
        public virtual ProjectClass ProjectClass { get; set; }


        [ForeignKey("ClientID")]
        public virtual Client Client { get; set; }

        [ForeignKey("LocationID")]
        public virtual Location Location { get; set; }

        [ForeignKey("ProjectManagerID")]
        public virtual Employee ProjectManagerObj { get; set; }

        [ForeignKey("DirectorID")]
        public virtual Employee DirectorObj { get; set; }

        [ForeignKey("SchedulerID")]
        public virtual Employee SchedulerObj { get; set; }

        [ForeignKey("VicePresidentID")]
        public virtual Employee VicePresidentObj { get; set; }

        [ForeignKey("FinancialAnalystID")]
        public virtual Employee FinancialAnalystObj { get; set; }

        [ForeignKey("CapitalProjectAssistantID")]
        public virtual Employee CapitalProjectAssistantObj { get; set; }


        //   public int ProjectNumber { get; set; }
        [ForeignKey("ProgramID")]
        //[IgnoreDataMember]
        public Program Program { get; set; }

        public int ProgramElementID { get; set; }
        [ForeignKey("ProgramElementID")]
        //[IgnoreDataMember]
        public ProgramElement ProgramElement { get; set; }
        public String LatLong { get; set; }

        public string VersionId { get; set; }

        [NotMapped]
        public virtual ICollection<ProjectLocation> ProjectLocations { get; set; }
      
        public virtual ICollection<ProjectScope> projectScopes { get; set; }

        [NotMapped]
        public string TotalUnapprovedTrends;

        // FOr projectAccessCOntrol change
        //[NotMapped]
        public List<Int32> employeeAllowedList { get; set; }

        //From RequestProjectController
        public static List<Project> getProject(String ProgramID, String ProgramElementID, String ProjectID, String KeyStroke)     
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            List<Project> MatchedProjectList = new List<Project>();
 
            try
            {

                using (var ctx = new CPPDbContext())
                {
                    ctx.Database.Log = msg => Trace.WriteLine(msg);

                    if (ProjectID != "null" && ProjectID != null)
                    {
                        int projectId = int.Parse(ProjectID);
                        IQueryable<Project> projects = ctx.Project.Include("Program").Include("ProgramElement").Where(p => p.ProjectID == projectId && p.IsDeleted == false);
      
                        MatchedProjectList = projects.ToList<Project>();
                        for (int i = 0; i < MatchedProjectList.Count; i++)
                        {
                            Project prj = MatchedProjectList[i];
                            prj.ApproversDetails = ctx.TrendApproversDetails.Where(p => p.ProjectElementId == projectId).ToList<TrendApproversDetails>();
                        }

                    }
                    else if (ProgramElementID != "null" && ProgramElementID != null)
                    {
                        int pgmEltId = int.Parse(ProgramElementID);
                        IQueryable<Project> projects = ctx.Project.Include("Program").Include("ProgramElement").Where(p => p.ProgramElement.ProgramElementID == pgmEltId && p.ProgramElement.IsDeleted == false);
                        MatchedProjectList = projects.ToList<Project>();
                      
                    }
                    else if (ProgramID != "null" && ProgramID != null)
                    {
                        int pgmId = int.Parse(ProgramID);
                        IQueryable<Project> projects = ctx.Project.Include("Program").Include("ProgramElement").Where(p => p.Program.ProgramID == pgmId && p.Program.IsDeleted == false);
                        MatchedProjectList = projects.ToList<Project>();
                      
                    }   
                    else if (KeyStroke != "null" && KeyStroke != null)
                    {
                        IQueryable<Project> projects = ctx.Project.Include("Program").Include("ProgramElement").Where(p => p.ProjectName.Contains(KeyStroke) && p.IsDeleted == false);
                        MatchedProjectList = projects.ToList<Project>();
                        
                    }
                    else 
                    {
                        IQueryable<Project> projects = ctx.Project.Include("Program").Include("ProgramElement").Where(p => p.IsDeleted == false);
                        MatchedProjectList = projects.ToList<Project>();
                       
                    }

                    for (var i = 0; i < MatchedProjectList.Count; i++)
                    {
                        List<ProjectScope> projectScopes = ProjectScope.getProjectScope(MatchedProjectList[i].ProjectID);
                        MatchedProjectList[i].projectScopes = projectScopes;

                        //For Projet access control
                        List<Int32> employeeAllowedList = ProjectAccessControl.GetAllowedUserList(MatchedProjectList[i].ProjectID);
                        MatchedProjectList[i].employeeAllowedList = employeeAllowedList;
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
            return MatchedProjectList;

        }

        //For Reporting
        public static List<Project> projectByOid(String OrganizationID)
        {
            List<Project> MatchedProjectList = new List<Project>();
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    ctx.Database.Log = msg => Trace.WriteLine(msg);
                    if (OrganizationID != "null")
                    {
                        IQueryable<Project> projects = ctx.Project.Include("Program").Include("ProgramElement").Where(p => p.OrganizationID == OrganizationID && p.IsDeleted == false);
                        MatchedProjectList = projects.ToList<Project>();
                    }
                }
            }
            catch (Exception ex)
            {
                var stackTrace = new StackTrace(ex.InnerException, true);
                var line = stackTrace.GetFrame(0).GetFileLineNumber();
            }
            return MatchedProjectList;
        }

        public static String registerProject(Project project)
        {
            String result = "";
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            MySqlConnection conn = null;
            MySqlDataReader reader = null;
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    IQueryable<Project> projects = ctx.Project.Where(p => p.ProgramElementID == project.ProgramElementID && p.ProjectName == project.ProjectName && p.IsDeleted == false);

                    //    Project retreivedProject = ctx.Project.Where(f => f.ProjectNumber == project.ProjectNumber && f.ProjectElementNumber == project.ProjectElementNumber).FirstOrDefault();
                    ////  ProjectNumber retreivedProject = ctx.ProjectNumber.Where(f => f.projectNumber == project.ProjectNumber).FirstOrDefault();

                    //  if (retreivedProject != null)   //duplicate project element number found - luan here
                    //  {
                    //      result = "Duplicate";
                    //  }
                    string maxProjectNumber = "00";
                    if (projects.ToList().Count == 0)
                    {

                        int count = ctx.Project.Where(b => b.ProgramElementID == project.ProgramElementID).Count();

                        if (count > 0)
                        {
                            maxProjectNumber = ctx.Project.Where(a => a.ProgramElementID == project.ProgramElementID).Max(b => b.ProjectElementNumber);
                            if (maxProjectNumber != null && maxProjectNumber != "")
                            {
                                maxProjectNumber = (Convert.ToInt32(maxProjectNumber) + 1).ToString();
                            }
                            else
                                maxProjectNumber = "01";
                        }

                        else
                        {
                            maxProjectNumber = "01";
                        }
                        if (maxProjectNumber.Length < 2)
                        {
                            int diff = 2 - maxProjectNumber.Length;
                            for (int i = 0; i < diff; i++)
                            {
                                maxProjectNumber = "0" + maxProjectNumber;
                            }
                        }
                        project.ProjectElementNumber = maxProjectNumber.ToString();

                        //Adding Project number to project_number table
                        ProjectNumber projectNumber = new ProjectNumber();
                        projectNumber.projectNumber = project.ProjectNumber;
                        projectNumber.projectElementNumber = project.ProjectElementNumber;
                        projectNumber.isUSed = true;
                        projectNumber.CreatedDate = DateTime.Now;
                        projectNumber.UpdatedDate = DateTime.Now;
                        projectNumber.OrganizationID = project.OrganizationID;
                        ctx.ProjectNumber.Add(projectNumber);
                        ctx.SaveChanges();

                        ctx.Database.Log = msg => Trace.WriteLine(msg);
                        int pgmId = project.ProgramID;
                        Program pgm = ctx.Program.First(p => p.ProgramID == pgmId);
                        project.Program = pgm;
                        int pgmEltId = project.ProgramElementID;
                        ProgramElement pgmElt = ctx.ProgramElement.First(p => p.ProgramElementID == pgmEltId);
                        project.ProgramElement = pgmElt;
                        //List<ProjectFund> projectFundList = project.projectFunds.ToList();
                        //project.projectFunds = null;

                        //register projectScope
                        //List<ProjectScope> projectScopeList = new List<ProjectScope>();
                        //projectScopeList = project.projectScopes.ToList();
                        //  project.projectScopes = null;

                        //Luan here - tbd 10000
                        if (project.ProjectManagerID <= 0) project.ProjectManagerID = 10000;
                        if (project.DirectorID <= 0) project.DirectorID = 10000;
                        if (project.SchedulerID <= 0) project.SchedulerID = 10000;
                        if (project.VicePresidentID <= 0) project.VicePresidentID = 10000;
                        if (project.FinancialAnalystID <= 0) project.FinancialAnalystID = 10000;
                        if (project.CapitalProjectAssistantID <= 0) project.CapitalProjectAssistantID = 10000;

                        int orgId = Convert.ToInt32(project.OrganizationID);

                        Versionmaster latestVersion = ctx.VersionMaster.Where(s => s.OrganizationID == orgId)
                                                    .OrderByDescending(a => a.CreatedDate).FirstOrDefault();


                        project.VersionId = latestVersion.Id.ToString();
                        project.Status= "Active";    //----Vaishnavi 30-03-2022----//
                        ctx.Project.Add(project);
                        ctx.SaveChanges();

                        var proj = ctx.Project.OrderByDescending(pj => pj.ProjectID).FirstOrDefault();

                        for (int i = 0; i < project.ApproversDetails.Count; i++)
                        {
                            project.ApproversDetails[i].ProjectElementId = proj.ProjectID;
                        }

                        ctx.TrendApproversDetails.AddRange(project.ApproversDetails);
                        ctx.SaveChanges();


                        //for (var i = 0; i < projectFundList.Count; i++)
                        //{
                        //    projectFundList[i].ProjectID = proj.ProjectID;
                        //    projectFundList[i].Operation = 1;
                        //    ProjectFund.registerProjectFund(projectFundList[i]);
                        //    FundType fund = FundType.getFundTypeByName(projectFundList[i].FundName);
                        //    fund.Availability = Convert.ToString(Convert.ToInt64(fund.Availability) - projectFundList[i].FundAmount);
                        //    fund.BalanceRemaining = Convert.ToString(Convert.ToInt64(fund.BalanceRemaining) - projectFundList[i].FundAmount);
                        //    FundType.updateFundType(fund.Fund, fund.Amount, fund.Availability, fund.BalanceRemaining);
                        //}
                        //foreach (var projScope in projectScopeList)
                        //{
                        //    projScope.ProjectID = proj.ProjectID;
                        //    projScope.Operation = 1;
                        //    ProjectScope.registerProjectScope(projScope);
                        //}


                        //ProjectAccess control -- Project Creator
                        User user = ctx.User.Where(u => u.UserID == project.CreatedBy).FirstOrDefault();
                        ProjectAccessControl projectAccessControl = new ProjectAccessControl();
                        projectAccessControl.UserId = user.Id;


                        ProjectAccessControl checkDataExist1 = ctx.ProjectAccessControl.
                           Where(p => projectAccessControl.UserId == p.UserId &&
                           project.ProgramElementID == p.ProgramElementID
                           && project.ProgramID == p.ProgramId
                           && project.ProjectID == p.ProjectID)
                           .FirstOrDefault();

                        ProjectAccessControl checkDataExist2 = ctx.ProjectAccessControl.
                           Where(p => projectAccessControl.UserId == p.UserId &&
                           project.ProgramElementID == p.ProgramElementID
                           && project.ProgramID == p.ProgramId
                           && p.ProjectID == 0)
                           .FirstOrDefault();

                        if (checkDataExist1 != null)
                        {
                            projectAccessControl.ProjectID = project.ProjectID;
                            checkDataExist1.IsProjectCreator = true;
                            projectAccessControl.Id = checkDataExist1.Id;
                            projectAccessControl.IsProgramEleCreator = checkDataExist1.IsProgramEleCreator;
                            projectAccessControl.IsProjectCreator = checkDataExist1.IsProjectCreator;
                            projectAccessControl.IsProgramCreator = checkDataExist1.IsProgramCreator;
                            ctx.SaveChanges();

                        }
                        else if (checkDataExist2 != null)
                        {
                            checkDataExist2.ProjectID = project.ProjectID;
                            checkDataExist2.IsProjectCreator = true;
                            projectAccessControl.Id = checkDataExist2.Id;
                            projectAccessControl.IsProgramEleCreator = checkDataExist2.IsProgramEleCreator;
                            projectAccessControl.IsProjectCreator = checkDataExist2.IsProjectCreator;
                            projectAccessControl.IsProgramCreator = checkDataExist2.IsProgramCreator;
                            ctx.SaveChanges();

                        }

                        else
                        {
                            projectAccessControl.ProjectID = project.ProjectID;
                            projectAccessControl.ProgramElementID = project.ProgramElementID;
                            projectAccessControl.IsProjectCreator = true;
                            projectAccessControl.ProgramId = project.ProgramID;
                            ctx.ProjectAccessControl.Add(projectAccessControl);
                            ctx.SaveChanges();
                        }

                        // Project Element Approvers
                        int projectId = project.ProjectID;
                        // List<TrendApproversDetails> ApproversDetails = ctx.TrendApproversDetails.Where(p => p.ProjectElementId == projectId).ToList<TrendApproversDetails>();
                        List<TrendApproversDetails> ApproversDetails = project.ApproversDetails;
                        List<int> approverDetailsList = new List<int>();

                        if (ApproversDetails.Count > 0)
                        {

                            for (var x = 0; x < ApproversDetails.Count; x++)
                            {
                                int approverUserID = ApproversDetails[x].EmpId;
                                if (approverUserID != 10000)
                                {

                                    //User userID = ctx.User.Where(u => u.EmployeeID == approverUserID).FirstOrDefault();

                                    ProjectAccessControl projectAccessControl1 = new ProjectAccessControl();
                                    projectAccessControl1.UserId = ApproversDetails[x].UserId;
                                    projectAccessControl1.ProgramElementID = ApproversDetails[x].ProjectElementId;

                                    projectAccessControl1.ProgramId = project.ProgramID;

                                    ProjectAccessControl checkDataExist11 = ctx.ProjectAccessControl.
                                      Where(p => projectAccessControl1.UserId == p.UserId && project.ProgramElementID == p.ProgramElementID
                                      && project.ProgramID == p.ProgramId && project.ProjectID == p.ProjectID)
                                      .FirstOrDefault();

                                    if (checkDataExist11 != null)
                                    {
                                        projectAccessControl1.Id = checkDataExist11.Id;
                                        checkDataExist11.IsProjectApprover = true;
                                        projectAccessControl1.ProjectID = checkDataExist11.ProjectID;
                                        projectAccessControl1.IsProgramEleCreator = checkDataExist11.IsProgramEleCreator;
                                        projectAccessControl1.IsProjectCreator = checkDataExist11.IsProjectCreator;
                                        projectAccessControl1.IsProgramCreator = checkDataExist11.IsProgramCreator;
                                        //ctx.projectAccessControl1.Add(projectAccessControl1);
                                        ctx.SaveChanges();
                                    }
                                    else
                                    {
                                        projectAccessControl1.ProjectID = project.ProjectID;
                                        projectAccessControl1.ProgramElementID = project.ProgramElementID;
                                        projectAccessControl1.IsProjectApprover = true;
                                        projectAccessControl1.ProgramId = project.ProgramID;
                                        ctx.ProjectAccessControl.Add(projectAccessControl1);
                                        ctx.SaveChanges();
                                    }
                                    approverDetailsList.Add(projectAccessControl1.UserId);

                                }


                            }
                        }


                        result = "Success" + "," + proj.ProgramID + "," + proj.ProgramElementID + "," + proj.ProjectID + "," + proj.ProjectElementNumber;
                    }
                    else
                    {
                        //result = "Unable to add new Project. Project " + project.ProjectName + " already existed."; 
                        result = "Unable to add new project element. Project element " + project.ProjectName + " already exists.";
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

        public static String updateProject(Project prj)
        {
            String result = "";
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    ctx.Database.Log = msg => Trace.WriteLine(msg);
                    int projectId = prj.ProjectID;
                    //var scopeToBeDeleted = prj.scopeToBeDeleted;
                    //prj.scopeToBeDeleted = null;
                    //foreach (var p in scopeToBeDeleted)
                    //{
                    //    ProjectScope.deleteProjectScope(p);
                    //}
                    if (prj.isModified == true)
                    {
                        IQueryable<Project> projects = ctx.Project.Where(p => p.ProjectID == prj.ProjectID);
                        
                        if (projects.ToList().Count == 1)
                        {
                            Project project = ctx.Project.First(p => p.ProjectID == projectId);
                            List<TrendApproversDetails> ApproversDetails = ctx.TrendApproversDetails.Where(p => p.ProjectElementId == projectId).ToList<TrendApproversDetails>();

                            if (project != null)
                            {
                                //Adding Project number to project_number table
                                ProjectNumber projectNumber = new ProjectNumber();
                                projectNumber.projectNumber = project.ProjectNumber;
                                projectNumber.projectElementNumber = project.ProjectElementNumber;
                                projectNumber.isUSed = true;
                                projectNumber.CreatedDate = DateTime.Now;
                                projectNumber.UpdatedDate = DateTime.Now;
                                projectNumber.OrganizationID = project.OrganizationID;
                                ctx.ProjectNumber.Add(projectNumber);
                                ctx.SaveChanges();

                                project.ProjectName = prj.ProjectName;
                                project.ProjectManager = prj.ProjectManager;
                                project.ProjectSponsor = prj.ProjectSponsor;
                                project.LatLong = prj.LatLong;
                                project.Director = prj.Director;
                                project.Scheduler = prj.Scheduler;
                                project.ExecSteeringComm = prj.ExecSteeringComm;
                                project.VicePresident = prj.VicePresident;
                                project.FinancialAnalyst = prj.FinancialAnalyst;
                                project.CapitalProjectAssistant = prj.CapitalProjectAssistant;
                                project.ProjectClassID = prj.ProjectClassID;
                                project.ProjectTypeID = prj.ProjectTypeID;
                                project.ProjectNumber = prj.ProjectNumber;
                                project.ContractNumber = prj.ContractNumber;
                                project.ProjectDescription = prj.ProjectDescription;

                                project.BillingPOC = prj.BillingPOC;
                                project.BillingPOCPhone1 = prj.BillingPOCPhone1;
                                project.BillingPOCPhone2 = prj.BillingPOCPhone2;
                                //====== Jignesh-AddAddressField-21-01-2021 =======
                                //project.BillingPOCAddress = prj.BillingPOCAddress;
                                project.BillingPOCAddressLine1 = prj.BillingPOCAddressLine1;
                                project.BillingPOCAddressLine2 = prj.BillingPOCAddressLine2;
                                project.BillingPOCCity = prj.BillingPOCCity;
                                project.BillingPOCState = prj.BillingPOCState;
                                project.BillingPOCPONo = prj.BillingPOCPONo;
                                //=================================================
                                project.BillingPOCEmail = prj.BillingPOCEmail;
                                project.BillingPOCSpecialInstruction = prj.BillingPOCSpecialInstruction;
                                project.TMBilling = prj.TMBilling;
                                project.SOVBilling = prj.SOVBilling;
                                project.MonthlyBilling = prj.MonthlyBilling;
                                project.Lumpsum = prj.Lumpsum;


                                project.ScopeQualityDescription = prj.ScopeQualityDescription;

								project.ProjectElementNumber = prj.ProjectElementNumber;
                                project.ClientPONumber = prj.ClientPONumber;
                                project.Amount = prj.Amount;
                                project.QuickbookJobNumber = prj.QuickbookJobNumber;
								project.LocationName = prj.LocationName;

								project.ClientID = prj.ClientID;
                                project.LocationID = prj.LocationID;
                                project.ProjectManagerID = prj.ProjectManagerID;
                                project.DirectorID = prj.DirectorID;
                                project.SchedulerID = prj.SchedulerID;
                                project.VicePresidentID = prj.VicePresidentID;
                                project.FinancialAnalystID = prj.FinancialAnalystID;
                                project.CapitalProjectAssistantID = prj.CapitalProjectAssistantID;

                                project.ProjectStartDate = prj.ProjectStartDate;
                                project.ContractStartDate = prj.ContractStartDate;
                                project.LatLong = prj.LatLong;
                                ctx.SaveChanges();

                                ctx.TrendApproversDetails.RemoveRange(ApproversDetails);
                                ctx.TrendApproversDetails.AddRange(prj.ApproversDetails);
                                ctx.SaveChanges();

                                // For Project Access Control change
                                project.employeeAllowedList = prj.employeeAllowedList;

                                if (project.employeeAllowedList != null)
                                {
                                    if (project.employeeAllowedList.Count > 0)
                                    {
                                        for (var x = 0; x < project.employeeAllowedList.Count; x++)
                                        {
                                            

                                            ProjectAccessControl projectAccessControl = new ProjectAccessControl();
                                            projectAccessControl.UserId = project.employeeAllowedList[x];


                                            ProjectAccessControl checkDataExist1 = ctx.ProjectAccessControl.
                                               Where(p => projectAccessControl.UserId == p.UserId &&
                                               project.ProgramElementID == p.ProgramElementID
                                               && project.ProgramID == p.ProgramId && project.ProjectID == p.ProjectID)
                                               .FirstOrDefault();

                                            if (checkDataExist1 != null)
                                            {

                                                checkDataExist1.IsAllowedUser = true;
                                                projectAccessControl.Id = checkDataExist1.Id;
                                                projectAccessControl.ProjectID = checkDataExist1.ProjectID;
                                                projectAccessControl.IsProgramEleCreator = checkDataExist1.IsProgramEleCreator;
                                                projectAccessControl.IsProjectCreator= checkDataExist1.IsProjectCreator;
                                                projectAccessControl.IsProgramCreator = checkDataExist1.IsProgramCreator;
                                                // ctx.Entry(projectAccessControl).State = System.Data.Entity.EntityState.Modified;
                                                ctx.SaveChanges();
                                                /* ctx.ProjectAccessControl.Add(projectAccessControl);
                                                 ctx.SaveChanges();*/
                                            }
                                            else
                                            {
                                                projectAccessControl.ProjectID = project.ProjectID;
                                                projectAccessControl.ProgramElementID = project.ProgramElementID;
                                                projectAccessControl.IsAllowedUser = true;
                                                projectAccessControl.ProgramId = project.ProgramID;
                                                ctx.ProjectAccessControl.Add(projectAccessControl);
                                                ctx.SaveChanges();
                                            }

                                        }
                                    }
                                }

                                // Project Element Approvers
                                List<int> approverDetailsList = new List<int>();

                                if (prj.ApproversDetails.Count > 0)
                                {

                                    for (var x = 0; x < prj.ApproversDetails.Count; x++)
                                    {
                                        int approverUserID = prj.ApproversDetails[x].EmpId;
                                        if (approverUserID != 10000) {

                                            //User userID = ctx.User.Where(u => u.EmployeeID == approverUserID).FirstOrDefault();
                                            //User userID = prj.ApproversDetails[x].UserId;

                                            ProjectAccessControl projectAccessControl = new ProjectAccessControl();
                                            projectAccessControl.UserId = prj.ApproversDetails[x].UserId;
                                            projectAccessControl.ProgramElementID = prj.ApproversDetails[x].ProjectElementId;

                                            projectAccessControl.ProgramId = project.ProgramID;

                                            ProjectAccessControl checkDataExist1 = ctx.ProjectAccessControl.
                                              Where(p => projectAccessControl.UserId == p.UserId && project.ProgramElementID == p.ProgramElementID
                                              && project.ProgramID == p.ProgramId && project.ProjectID == p.ProjectID)
                                              .FirstOrDefault();

                                            if (checkDataExist1 != null)
                                            {
                                                projectAccessControl.Id = checkDataExist1.Id;
                                                checkDataExist1.IsProjectApprover = true;
                                                projectAccessControl.ProjectID = checkDataExist1.ProjectID;
                                                projectAccessControl.IsProgramEleCreator = checkDataExist1.IsProgramEleCreator;
                                                projectAccessControl.IsProjectCreator = checkDataExist1.IsProjectCreator;
                                                projectAccessControl.IsProgramCreator = checkDataExist1.IsProgramCreator;
                                                //ctx.ProjectAccessControl.Add(projectAccessControl);
                                                ctx.SaveChanges();
                                            }
                                            else
                                            {
                                                projectAccessControl.ProjectID = project.ProjectID;
                                                projectAccessControl.ProgramElementID = project.ProgramElementID;
                                                projectAccessControl.IsProjectApprover = true;
                                                projectAccessControl.ProgramId = project.ProgramID;
                                                ctx.ProjectAccessControl.Add(projectAccessControl);
                                                ctx.SaveChanges();
                                            }
                                            approverDetailsList.Add(projectAccessControl.UserId);

                                        }
                                        

                                    }
                                }

                                //Update the status for creator and approver

                                List<ProjectAccessControl> createApprovertEmpList = ctx.ProjectAccessControl.Where(
                                    p => project.ProgramElementID == p.ProgramElementID
                                          && project.ProgramID == p.ProgramId && project.ProjectID == p.ProjectID && (p.IsProjectApprover == true && p.IsAllowedUser == true)
                                    )
                                    .ToList();
                                for(var i=0;i< createApprovertEmpList.Count; i++)
                                {
                                    if (approverDetailsList!=null && (!approverDetailsList.Contains(createApprovertEmpList[i].UserId) &&
                                        (createApprovertEmpList[i].IsProgramEleCreator || createApprovertEmpList[i].IsProjectCreator
                                        || createApprovertEmpList[i].IsAllowedUser)))
                                    {
                                        ProjectAccessControl projectAccessControl = new ProjectAccessControl();
                                        projectAccessControl.Id = createApprovertEmpList[i].Id;
                                        createApprovertEmpList[i].IsProjectApprover = false;
                                        projectAccessControl.ProjectID = createApprovertEmpList[i].ProjectID;
                                        projectAccessControl.IsProgramEleCreator = createApprovertEmpList[i].IsProgramEleCreator;
                                        projectAccessControl.IsProjectCreator = createApprovertEmpList[i].IsProjectCreator;
                                        projectAccessControl.IsProgramCreator = createApprovertEmpList[i].IsProgramCreator;
                                        //ctx.ProjectAccessControl.Add(projectAccessControl);
                                        ctx.SaveChanges();

                                    }
                                    else if (project.employeeAllowedList!=null && (!project.employeeAllowedList.Contains(createApprovertEmpList[i].UserId) &&
                                      (createApprovertEmpList[i].IsProgramEleCreator || createApprovertEmpList[i].IsProjectCreator
                                      || createApprovertEmpList[i].IsAllowedUser)))
                                    {
                                        ProjectAccessControl projectAccessControl = new ProjectAccessControl();
                                        projectAccessControl.Id = createApprovertEmpList[i].Id;
                                        createApprovertEmpList[i].IsAllowedUser = false;
                                        projectAccessControl.ProjectID = createApprovertEmpList[i].ProjectID;
                                        projectAccessControl.IsProgramEleCreator = createApprovertEmpList[i].IsProgramEleCreator;
                                        projectAccessControl.IsProjectCreator = createApprovertEmpList[i].IsProjectCreator;
                                        projectAccessControl.IsProgramCreator = createApprovertEmpList[i].IsProgramCreator;
                                        //ctx.ProjectAccessControl.Add(projectAccessControl);
                                        ctx.SaveChanges();

                                    }
                                }



                                // Remove Nonexiting ID for projectelement
                                List<int> recordExistsInDb = ctx.ProjectAccessControl.
                                    Where(p => project.ProgramElementID == p.ProgramElementID
                                          && project.ProgramID == p.ProgramId && project.ProjectID == p.ProjectID && (p.IsProjectCreator == false && p.IsProgramEleCreator == false))
                                    .Select(q => q.UserId).ToList();
                                List<Int32> checkNonExistAllowedRecords = new List<Int32>();

                                if (project.employeeAllowedList == null && approverDetailsList !=null)
                                {
                                    checkNonExistAllowedRecords = recordExistsInDb.Except(approverDetailsList).ToList();
                                }
                                else if(project.employeeAllowedList != null && approverDetailsList == null)
                                {
                                    checkNonExistAllowedRecords = recordExistsInDb.Except(project.employeeAllowedList).ToList();
                                }else if (project.employeeAllowedList != null && approverDetailsList != null)
                                {
                                    checkNonExistAllowedRecords = recordExistsInDb.Except(project.employeeAllowedList).Except(approverDetailsList).ToList();
                                }
                                    Console.WriteLine(checkNonExistAllowedRecords);

                                if (checkNonExistAllowedRecords.Count > 0)
                                {
                                    List<ProjectAccessControl> nonExistEmpList = ctx.ProjectAccessControl.Where(
                                        p => checkNonExistAllowedRecords.Contains(p.UserId)
                                        && project.ProgramElementID == p.ProgramElementID
                                              && project.ProgramID == p.ProgramId && project.ProjectID == p.ProjectID && (p.IsProjectCreator == false && p.IsProgramEleCreator == false)
                                        )
                                        .ToList();
                                    ctx.ProjectAccessControl.RemoveRange(nonExistEmpList);
                                    ctx.SaveChanges();
                                }
                                    approverDetailsList = new List<int>();
                                
                                result = "Success";

                            }
                            else
                            {
                                result = "Update failed because project " + prj.ProjectName + " is not exist!";
                            }
                        }
                        else
                        {
                            result = "Update failed because project " + prj.ProjectName + " already exist. Please make sure that program name is unique!";
                        }
                    }
                    else
                    {
                        Project project = ctx.Project.First(p => p.ProjectID == projectId);

                        //TODO check for duplicate project #
                        Project duplicateProjectNumber = ctx.Project.Where(f => f.ProjectElementNumber == prj.ProjectElementNumber
                                                                                && f.ProjectNumber == prj.ProjectNumber 
                                                                                && f.ProjectID != prj.ProjectID).FirstOrDefault();
                        //Project duplicateProjectNumber = null;
                        if (duplicateProjectNumber != null)   //duplicate project number found - luan here
                        {
                            result = "Duplicate";
                        } else if (project != null)
                        {
                            //Adding Project number to project_number table
                            ProjectNumber projectNumber = new ProjectNumber();
                            projectNumber.projectNumber = prj.ProjectNumber;
                            projectNumber.projectElementNumber = prj.ProjectElementNumber;
                            projectNumber.isUSed = true;
                            projectNumber.CreatedDate = DateTime.Now;
                            projectNumber.UpdatedDate = DateTime.Now;
                            projectNumber.OrganizationID = project.OrganizationID;
                            ctx.ProjectNumber.Add(projectNumber);
                            ctx.SaveChanges();

                            project.ProjectName = prj.ProjectName;
                            project.ProjectManager = prj.ProjectManager;
                            project.ProjectSponsor = prj.ProjectSponsor;
                            project.LatLong = prj.LatLong;
                            project.Director = prj.Director;
                            project.Scheduler = prj.Scheduler;
                            project.ExecSteeringComm = prj.ExecSteeringComm;
                            project.VicePresident = prj.VicePresident;
                            project.FinancialAnalyst = prj.FinancialAnalyst;
                            project.CapitalProjectAssistant = prj.CapitalProjectAssistant;
                            project.ProjectClassID = prj.ProjectClassID;
                            project.ProjectTypeID = prj.ProjectTypeID;
                            project.ProjectNumber = prj.ProjectNumber;
                            project.ContractNumber = prj.ContractNumber;
                            project.ProjectDescription = prj.ProjectDescription;


                            project.BillingPOC = prj.BillingPOC;
                            project.BillingPOCPhone1 = prj.BillingPOCPhone1;
                            project.BillingPOCPhone2 = prj.BillingPOCPhone2;
                            //====== Jignesh-AddAddressField-21-01-2021 =======
                            //project.BillingPOCAddress = prj.BillingPOCAddress;
                            project.BillingPOCAddressLine1 = prj.BillingPOCAddressLine1;
                            project.BillingPOCAddressLine2 = prj.BillingPOCAddressLine2;
                            project.BillingPOCCity = prj.BillingPOCCity;
                            project.BillingPOCState = prj.BillingPOCState;
                            project.BillingPOCPONo = prj.BillingPOCPONo;
                            //=================================================
                            project.BillingPOCEmail = prj.BillingPOCEmail;
                            project.BillingPOCSpecialInstruction = prj.BillingPOCSpecialInstruction;
                            project.TMBilling = prj.TMBilling;
                            project.SOVBilling = prj.SOVBilling;
                            project.MonthlyBilling = prj.MonthlyBilling;
                            project.Lumpsum = prj.Lumpsum;

                            project.ClientID = prj.ClientID;
                            project.LocationID = prj.LocationID;
                            project.ProjectManagerID = prj.ProjectManagerID;
                            project.DirectorID = prj.DirectorID;
                            project.SchedulerID = prj.SchedulerID;
                            project.VicePresidentID = prj.VicePresidentID;
                            project.FinancialAnalystID = prj.FinancialAnalystID;
                            project.CapitalProjectAssistantID = prj.CapitalProjectAssistantID;

                            project.CostDescription = prj.CostDescription;
                            project.ScheduleDescription = prj.ScheduleDescription;
                            project.ScopeQualityDescription = prj.ScopeQualityDescription;

                            project.ProjectStartDate = prj.ProjectStartDate;
                            project.ContractStartDate = prj.ContractStartDate;
                            project.LatLong = prj.LatLong;
                            ctx.SaveChanges();
                            result = "Success";

                        }
                        else
                        {
                            result = "Update failed because project " + prj.ProjectName + " is not exist!";
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
        
        public static String deleteProject(Project proj)
        {
            int ProjectID = proj.ProjectID;
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            MySqlConnection conn = null;
            MySqlDataReader reader = null;
            ProgramElement programElement = new ProgramElement();
            String result = "";
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    ctx.Database.Log = msg => Trace.WriteLine(msg);
                    int projectId = ProjectID;
                    Project project = ctx.Project.First(p => p.ProjectID == projectId);
                    List<ProjectScope> projectScopes = new List<ProjectScope>(); 
                    projectScopes = ctx.ProjectScope.Where(ps => ps.ProjectID == ProjectID).ToList();
                    programElement = ctx.ProgramElement.Where(p => p.ProgramElementID == project.ProgramElementID).FirstOrDefault();
                    List<Trend> trendList = ctx.Trend.Where(tr => tr.ProjectID == ProjectID).Select(trendItem => trendItem).ToList();
                    List<Document> documents = ctx.Document.Where(a => a.ProjectID == ProjectID).ToList();

                    //Nivedita 02 - 12 - 2021
                    if (trendList.Count > 0)
                    {
                        foreach (var trend in trendList)
                        {
                            trend.DeletedBy = proj.DeletedBy;
                            Trend.deleteTrend(trend); //this will also delete trend Funds assigned to the trend
                        }
                    }
                    foreach (var scope in projectScopes)
                    {
                        //Nivedita 10022022
                        //ctx.ProjectScope.Remove(scope);
                        scope.IsDeleted = true;
                        scope.DeletedDate = DateTime.Now;
                        scope.DeletedBy = proj.DeletedBy;
                        ctx.SaveChanges();
                    }
                    //Remove all documents
                    foreach(Document doc in documents)
                    {
                        //Nivedita 10022022
                        //ctx.Entry(doc).State = System.Data.Entity.EntityState.Deleted;
                        doc.IsDeleted = true;
                        doc.DeletedDate = DateTime.Now;
                        doc.DeletedBy = proj.DeletedBy;
                        ctx.SaveChanges();
                    }
                    ctx.SaveChanges();

                    //using different context to delete Project
                    using (var DbCtx = new CPPDbContext())
                    {
                        //Delete documents

                        //Nivedita 02-12-2021
                        Project projectToBeDeleted = DbCtx.Project.First(p => p.ProjectID == ProjectID);
                        projectToBeDeleted.IsDeleted = true;
                        projectToBeDeleted.DeletedDate= DateTime.Now;
                        projectToBeDeleted.DeletedBy = proj.DeletedBy;
                        projectToBeDeleted.Status = "Archived";    //----Vaishnavi 30-03-2022----//
                        //DbCtx.Project.Remove(projectToBeDeleted); 
                        DbCtx.SaveChanges();
                        updateCostOnProjectDelete(programElement.ProgramElementID);
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
         public static void updateCostOnProjectDelete(int programElementID){
             Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
             Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
             using (var ctx = new CPPDbContext())
            {
                //Project project = ctx.Project.Where(p => p.ProjectID == ProjectID).FirstOrDefault();
                ProgramElement programElement = ctx.ProgramElement.Where(p => p.ProgramElementID == programElementID).FirstOrDefault();
                Program program = ctx.Program.Where(p => p.ProgramID == programElement.ProgramID).FirstOrDefault();
                var pmID = programElement.ProgramElementID;
                var programID = program.ProgramID;
              
                MySqlCommand command = null;
                MySqlConnection conn = null;
                MySqlDataReader reader = null;

                try
                {
                    var query = "update_cost_on_project_delete"; //Stored Procedure
                    conn = ConnectionManager.getConnection();
                    conn.Open();
                    command = new MySqlCommand(query, conn);
                    command.CommandType = CommandType.StoredProcedure;

                    //For Create New
               
                    //command.Parameters.Add(new MySqlParameter("_ProjectID", ProjectID));
                    command.Parameters.Add(new MySqlParameter("_ProgramElementID", pmID));
                    command.Parameters.Add(new MySqlParameter("_ProgramID", programID));
                    //command.Parameters.Add(new MySqlParameter("_TrendID", 0));
                   
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
        //----Vaishnavi 30-03-2022----//
        public static String closeProject(Project proj)
        {
            int ProjectID = proj.ProjectID;
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            MySqlConnection conn = null;
            MySqlDataReader reader = null;
            ProgramElement programElement = new ProgramElement();
            String result = "";
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    ctx.Database.Log = msg => Trace.WriteLine(msg);
                    int projectId = ProjectID;
                    Project project = ctx.Project.First(p => p.ProjectID == projectId && p.IsDeleted == false);
                    //List<ProjectScope> projectScopes = new List<ProjectScope>();
                    //projectScopes = ctx.ProjectScope.Where(ps => ps.ProjectID == ProjectID).ToList();
                    //programElement = ctx.ProgramElement.Where(p => p.ProgramElementID == project.ProgramElementID).FirstOrDefault();
                    List<Trend> trendList = ctx.Trend.Where(tr => tr.ProjectID == ProjectID && tr.IsDeleted == false).Select(trendItem => trendItem).ToList();
                    //List<Document> documents = ctx.Document.Where(a => a.ProjectID == ProjectID).ToList();

                    //Nivedita 02 - 12 - 2021
                    if (trendList.Count > 0)
                    {
                        foreach (var trend in trendList)
                        {
                           // trend.DeletedBy = proj.DeletedBy;
                            Trend.closeTrend(trend); //this will also delete trend Funds assigned to the trend
                        }
                    }
                    //foreach (var scope in projectScopes)
                    //{
                    //    //Nivedita 10022022
                    //    //ctx.ProjectScope.Remove(scope);
                    //    scope.IsDeleted = true;
                    //    scope.DeletedDate = DateTime.Now;
                    //    scope.DeletedBy = proj.DeletedBy;
                    //    ctx.SaveChanges();
                    //}
                    ////Remove all documents
                    //foreach (Document doc in documents)
                    //{
                    //    //Nivedita 10022022
                    //    //ctx.Entry(doc).State = System.Data.Entity.EntityState.Deleted;
                    //    doc.IsDeleted = true;
                    //    doc.DeletedDate = DateTime.Now;
                    //    doc.DeletedBy = proj.DeletedBy;
                    //    ctx.SaveChanges();
                    //}
                    //ctx.SaveChanges();

                    //using different context to delete Project
                    using (var DbCtx = new CPPDbContext())
                    {
                        //Delete documents

                        //Nivedita 02-12-2021
                        Project projectToBeDeleted = DbCtx.Project.First(p => p.ProjectID == ProjectID);
                        //projectToBeDeleted.IsDeleted = true;
                        //projectToBeDeleted.DeletedDate = DateTime.Now;
                        //projectToBeDeleted.DeletedBy = proj.DeletedBy;
                        projectToBeDeleted.Status = "Closed";
                        //DbCtx.Project.Remove(projectToBeDeleted); 
                        DbCtx.SaveChanges();
                        //updateCostOnProjectDelete(programElement.ProgramElementID);
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