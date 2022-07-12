using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Reflection;
using System.Diagnostics;
using System.Data.OleDb;
using WebAPI.Controllers;
using MySql.Data.MySqlClient;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    [Table("program")]
    public class Program : Audit
    {
        readonly static log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [NotMapped]
        public int Operation;
        public bool isModified;
        public bool isNotesModified;

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProgramID { get; set; }
        public string ProgramName { get; set; }
        public string ProgramManager { get; set; }
        public string ProgramSponsor { get; set; }

        //Added to store client ID
        public int ClientID { get; set; }
        public string ClientPOC { get; set; }
        public string ClientPhone { get; set; }
        public string ClientEmail { get; set; }
        public string ClientAddress { get; set; }
        //====== Jignesh-AddAddressField-21-01-2021 =======
        public string ClientAddressLine1 { get; set; }
        public string ClientAddressLine2 { get; set; }
        public string ClientCity { get; set; }
        public string ClientState { get; set; }
        public string ClientPONo { get; set; }
        //=================================================
        public string ProjectManager { get; set; }
        public string ProjectManagerPhone { get; set; }
        public string ProjectManagerEmail { get; set; }
        public string ContractName { get; set; }
        public string ContractNumber { get; set; }
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

        public String ContractValue { get; set; }   //Manasi 14-07-2020
        public String JobNumber { get; set; }    //Manasi 04-08-2020

        public List<int> CertifiedPayrollIDS { get; set; }   //Vaishnavi 12-04-2022

        public String IsCertifiedPayrollChecked { get; set; }

        public List<String> ProgramPrevailingWagesList { get; set; }

        public String IsPrevailingWageChecked { get; set; }

        public List<int> WrapIDS { get; set; }

        public String IsWrapChecked { get; set; }     //Vaishnavi 12-04-2022

        public DateTime? CurrentStartDate { get; set; }
        public DateTime? CurrentEndDate { get; set; }
        public DateTime? originalEndDate { get; set; } //Aditya 17-02-2021
        public DateTime? ForecastStartDate { get; set; }
        public DateTime? ForecastEndDate { get; set; }
        public string CurrentCost { get; set; }
        public string ForecastCost { get; set; }
        public string OrganizationID { get; set; }
        public bool IsDeleted { get; set; }

        public string DeletedBy { get; set; }
        public string Status { get; set; }     //----Vaishnavi 30-03-2022----//
        public int ProgramManagerID { get; set; }
        public int ProgramSponsorID { get; set; }

        public int ProjectClassID { get; set; }

        public string IsPPBond { get; set; }

        public string PrimeSubPrime { get; set; }

        public string PrimeParent { get; set; }

        public List<int> PManagerIDS { get; set; } //Aditya PMDD 05052022

        public string IsCostPartOfContract { get; set; }

        public string PPBondNotes { get; set; }

        [NotMapped]
        public DateTime? LaborStartDate { get; set; }    //Vaishnavi 12-04-2022
        [NotMapped]
        public DateTime? LaborEndDate { get; set; }
        [NotMapped]
        public DateTime? MaterialsStartDate { get; set; }
        [NotMapped]
        public DateTime? MaterialsEndDate { get; set; }
        [NotMapped]
        public DateTime? OtherStartDate { get; set; }
        [NotMapped]
        public DateTime? OtherEndDate { get; set; }
        [NotMapped]
        public string LaborWarranty { get; set; }    //Vaishnavi 12-04-2022
        [NotMapped]
        public string MaterialsWarranty { get; set; }
        [NotMapped]
        public string OtherWarranty { get; set; }
        [NotMapped]
        public string LaborDescription { get; set; }
        [NotMapped]
        public string MaterialsDescription { get; set; }
        [NotMapped]
        public string OtherDescription { get; set; }

        public string ReportingTo { get; set; }    //Vaishnavi 12-04-2022

        [ForeignKey("ProgramManagerID")]
        public virtual Employee ProgramManagerObj { get; set; }

        [ForeignKey("ProgramSponsorID")]
        public virtual Employee ProgramSponsorObj { get; set; }

        [ForeignKey("ProjectClassID")]
        public virtual ProjectClass ProjectClass { get; set; }

        [NotMapped]
        public ProgramWarranty LaborWarrantyList { get; set; }     //Vaishnavi 12-04-2022
        [NotMapped]
        public ProgramWarranty MaterialsWarrantyList { get; set; }
        [NotMapped]
        public ProgramWarranty OtherWarrantyList { get; set; }       //Vaishnavi 12-04-2022

        [NotMapped]
        public string programNote { get; set; }

        [NotMapped]
        public PrelimnaryNotice prelimnaryNotice { get; set; }

        [NotMapped]
        public virtual ICollection<ProgramCategory> programCategories { get; set; }
        public virtual ICollection<ProgramCategory> categoryToBeDeleted { get; set; }
        [NotMapped]
        public virtual ICollection<ProgramFund> programFunds { get; set; }
        public virtual ICollection<ProgramFund> fundToBeDeleted { get; set; }

        public static List<Program> getProgramLookup(String OrganizationID, String ProgramID, String KeyStroke)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            List<Program> MatchedProgramList = new List<Program>();

            try
            {
                using (var ctx = new CPPDbContext())
                {
                    var test = ctx.Program.Where(a => a.OrganizationID == OrganizationID && a.IsDeleted == false)
                         .Select(c => new { c.ProgramID, c.ProgramName })
                        .ToList();
                    MatchedProgramList = test.Select(a => new Program { ProgramID = a.ProgramID, ProgramName = a.ProgramName }).ToList();
                }

            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }

            return MatchedProgramList;
        }
        //From RequestProgramController
        public static List<Program> getProgram(String OrganizationID, String ProgramID, String KeyStroke)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            List<Program> MatchedProgramList = new List<Program>();

            try
            {
                using (var ctx = new CPPDbContext())
                {
                    ctx.Database.Log = msg => Trace.WriteLine(msg);
                    ctx.Configuration.ProxyCreationEnabled = false;
                    if (OrganizationID != "null" && ProgramID != "null")
                    {
                        string orgId = OrganizationID;
                        int pgmId = int.Parse(ProgramID);
                        IQueryable<Program> programs = ctx.Program.Where(p => p.OrganizationID == orgId && p.ProgramID == pgmId && p.IsDeleted == false);
                        MatchedProgramList = programs.ToList<Program>();
                        for (var i = 0; i < MatchedProgramList.Count; i++)
                        {
                            var proId = MatchedProgramList[i].ProgramID;
                            
                            
                            // List<ProgramFund> programFunds = ProgramFund.getProgramFund(MatchedProgramList[i].ProgramID);
                            //MatchedProgramList[i].programCategories = ctx.ProgramCategory.Where(a => a.ProgramID == MatchedProgramList[i].ProgramID).ToList();
                            //List<ProgramCategory> programCategories = ctx.ProgramCategory.Where(a => a.ProgramID == proId).ToList();
                            //foreach (var category in programCategories)
                            //{

                            //    category.ActivityCategory = ctx.ActivityCategory.Where(a => a.ID == category.ActivityCategoryID).FirstOrDefault();
                            //    category.Program = null;
                            //}
                            //MatchedProgramList[i].programCategories = programCategories;
                            //MatchedProgramList[i].programFunds = programFunds;
                        }
                    }
                    else if (OrganizationID != "null")
                    {
                        //MatchedProgramList = getProgramLookup(OrganizationID,ProgramID,KeyStroke);
                        string orgId = OrganizationID;
                        IQueryable<Program> programs = ctx.Program.Where(p => p.OrganizationID == orgId && p.IsDeleted == false);
                          
                        MatchedProgramList = programs.ToList<Program>();
                        for (var i = 0; i < MatchedProgramList.Count; i++)
                        {
                            var proId = MatchedProgramList[i].ProgramID;
                            // List<ProgramFund> programFunds = ProgramFund.getProgramFund(MatchedProgramList[i].ProgramID);
                            //MatchedProgramList[i].programCategories = ctx.ProgramCategory.Where(a => a.ProgramID == MatchedProgramList[i].ProgramID).ToList();
                            // List<ProgramCategory> programCategories = ctx.ProgramCategory.Where(a => a.ProgramID == proId).ToList();
                            //foreach (var category in programCategories)
                            //{

                            //    category.ActivityCategory = ctx.ActivityCategory.Where(a => a.ID == category.ActivityCategoryID).FirstOrDefault();
                            //    category.Program = null;
                            //}
                            //MatchedProgramList[i].programCategories = programCategories;
                            //MatchedProgramList[i].programFunds = programFunds;
                        }
                    }
                    else if (ProgramID != "null")
                    {
                        int pgmId = int.Parse(ProgramID);
                        IQueryable<Program> programs = ctx.Program.Where(p => p.ProgramID == pgmId && p.IsDeleted == false);
                            
                        MatchedProgramList = programs.ToList<Program>();
                        for (var i = 0; i < MatchedProgramList.Count; i++)
                        {
                            var proId = MatchedProgramList[i].ProgramID;

                          
                            //List<ProgramFund> programFunds = ProgramFund.getProgramFund(MatchedProgramList[i].ProgramID);
                            ////MatchedProgramList[i].programCategories = ctx.ProgramCategory.Where(a => a.ProgramID == MatchedProgramList[i].ProgramID).ToList();
                            //List<ProgramCategory> programCategories = ctx.ProgramCategory.Where(a => a.ProgramID == proId).ToList();
                            //foreach (var category in programCategories)
                            //{

                            //    category.ActivityCategory = ctx.ActivityCategory.Where(a => a.ID == category.ActivityCategoryID).FirstOrDefault();
                            //    category.Program = null;
                            //}
                            //MatchedProgramList[i].programCategories = programCategories;
                            //MatchedProgramList[i].programFunds = programFunds;
                        }
                    }
                    else if (KeyStroke != "null")
                    {
                        IQueryable<Program> programs = ctx.Program.Where(p => p.ProgramName.Contains(KeyStroke) && p.IsDeleted == false);

                           
                        MatchedProgramList = programs.ToList<Program>();
                        for (var i = 0; i < MatchedProgramList.Count; i++)
                        {
                            var proId = MatchedProgramList[i].ProgramID;
                            //List<ProgramFund> programFunds = ProgramFund.getProgramFund(MatchedProgramList[i].ProgramID);
                            ////MatchedProgramList[i].programCategories = ctx.ProgramCategory.Where(a => a.ProgramID == MatchedProgramList[i].ProgramID).ToList();
                            //List<ProgramCategory> programCategories = ctx.ProgramCategory.Where(a => a.ProgramID == proId).ToList();
                            //foreach (var category in programCategories)
                            //{

                            //    category.ActivityCategory = ctx.ActivityCategory.Where(a => a.ID == category.ActivityCategoryID).FirstOrDefault();
                            //    category.Program = null;
                            //}
                            //MatchedProgramList[i].programCategories = programCategories;
                            //MatchedProgramList[i].programFunds = programFunds;
                        }
                    }
                    else
                    {
                        IQueryable<Program> programs = ctx.Program.Where(p => p.IsDeleted == false);
                            
                        MatchedProgramList = programs.ToList<Program>();
                        for (var i = 0; i < MatchedProgramList.Count; i++)
                        {
                            var proId = MatchedProgramList[i].ProgramID;
                            //List<ProgramFund> programFunds = ProgramFund.getProgramFund(MatchedProgramList[i].ProgramID);
                            ////MatchedProgramList[i].programCategories = ctx.ProgramCategory.Where(a => a.ProgramID == MatchedProgramList[i].ProgramID).ToList();
                            //List<ProgramCategory> programCategories = ctx.ProgramCategory.Where(a => a.ProgramID == proId).ToList();
                            //foreach (var category in programCategories)
                            //{

                            //    category.ActivityCategory = ctx.ActivityCategory.Where(a => a.ID == category.ActivityCategoryID).FirstOrDefault();
                            //    category.Program = null;
                            //}
                            //MatchedProgramList[i].programCategories = programCategories;
                            //MatchedProgramList[i].programFunds = programFunds;
                        }
                    }

                }


            }
            catch (Exception ex)
            {
               
                var stackTrace = new StackTrace(ex, true);
                var line = stackTrace.GetFrame(0).GetFileLineNumber();
                Logger.LogExceptions(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, ex.Message, line.ToString(), Logger.logLevel.Exception);
                string a = ex.InnerException.ToString();
            }
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Exit Point", Logger.logLevel.Debug);
            return MatchedProgramList;

        }

        public static String registerProgram(Program pgm)
        {

            String result = "";
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    IQueryable<Program> programs = ctx.Program.Where(p => p.OrganizationID == pgm.OrganizationID && p.ProgramName == pgm.ProgramName && p.IsDeleted == false);
                    if (programs.ToList().Count == 0)
                    {
                        ctx.Database.Log = msg => Trace.WriteLine(msg);


                        List<ProgramFund> programFundList = pgm.programFunds.ToList();
                        List<ProgramCategory> programCategoryList = pgm.programCategories.ToList();
                        pgm.programFunds = null;
                        pgm.programCategories = null;
                        pgm.IsDeleted = false;
                        pgm.Status = "Active";   //----Vaishnavi 30-03-2022----//
                        //pgm.IsCertifiedPayrollChecked = pgm.IsCertifiedPayrollChecked;    //Vaishnavi 12-04-2022
                        //pgm.IsPrevailingWageChecked = pgm.IsPrevailingWageChecked;
                        //pgm.IsWrapChecked = pgm.IsWrapChecked;
                        //pgm.ReportingTo = pgm.ReportingTo;    //Vaishnavi 12-04-2022
                        ctx.Program.Add(pgm);
                        ctx.SaveChanges();
                        var prog = ctx.Program.OrderByDescending(p => p.ProgramID).FirstOrDefault();
                        Console.WriteLine(prog);
                        for (var i = 0; i < programFundList.Count; i++)
                        {
                            programFundList[i].ProgramID = prog.ProgramID;
                            programFundList[i].Operation = 1;
                            ProgramFund.registerProgramFund(programFundList[i]);

                        }

                        for (var i = 0; i < programCategoryList.Count; i++)
                        {
                            programCategoryList[i].ProgramID = prog.ProgramID;
                            programCategoryList[i].Operation = 1;
                            ProgramCategory.registerProgramCategory(programCategoryList[i]);

                        }
                        result = "Success" + "," + prog.ProgramID;
                        List<Program> programList = new List<Program>();
                        programList = ctx.Program.Where(s => s.ProgramID == pgm.ProgramID).ToList();
                        ContractModification contractModificationsList = new ContractModification();
                        foreach (var plist in programList)
                        {
                            var cvalue = plist.ContractValue.Trim('$');
                           
                            contractModificationsList.ModificationNo = "0";
                            contractModificationsList.Title = "Original Contract Value";
                            contractModificationsList.ModificationType = 0;
                            contractModificationsList.Value = cvalue;
                            contractModificationsList.ScheduleImpact = 0;
                            contractModificationsList.Date = plist.CurrentStartDate ?? DateTime.Now;
                            contractModificationsList.ProgramID = plist.ProgramID;
                            contractModificationsList.CreatedDate = plist.CreatedDate;
                            contractModificationsList.CreatedBy = plist.CreatedBy;

                        }
                        ctx.ContractModification.Add(contractModificationsList);
                        ctx.SaveChanges();

                        ContractProjectManager.SavePMList(pgm); //Aditya PMDD 05052022

                        //if (pgm.IsCertifiedPayrollChecked == "Yes")     //Vaishnavi 12-04-2022
                        //{
                        //    if (pgm.CertifiedPayrollIDS.Count != 0)
                        //    {
                        //        for (int i = 0; i < pgm.CertifiedPayrollIDS.Count; i++)
                        //        {
                        //            ProgramCertifiedPayroll programCertifiedPayroll = new ProgramCertifiedPayroll();
                        //            programCertifiedPayroll.ProgramID = pgm.ProgramID;
                        //            programCertifiedPayroll.CertifiedPayrollID = pgm.CertifiedPayrollIDS[i];
                        //            ctx.ProgramCertifiedPayroll.Add(programCertifiedPayroll);
                        //            ctx.SaveChanges();
                        //        }
                        //    }
                        //}
                        //if (pgm.IsPrevailingWageChecked == "Yes")
                        //{
                        //    if (pgm.ProgramPrevailingWagesList.Count != 0)
                        //    {
                        //        for (int i = 0; i < pgm.ProgramPrevailingWagesList.Count; i++)
                        //        {
                        //            ProgramPrevailingWage programPrevailingWage = new ProgramPrevailingWage();
                        //            programPrevailingWage.ProgramID = pgm.ProgramID;
                        //            programPrevailingWage.Description = pgm.ProgramPrevailingWagesList[i];
                        //            ctx.ProgramPrevailingWage.Add(programPrevailingWage);
                        //            ctx.SaveChanges();
                        //        }
                        //    }
                        //}
                        //if (pgm.IsWrapChecked == "Yes")
                        //{
                        //    if (pgm.WrapIDS.Count != 0)
                        //    {
                        //        for (int i = 0; i < pgm.WrapIDS.Count; i++)
                        //        {
                        //            ProgramWrap programWrap = new ProgramWrap();
                        //            programWrap.ProgramID = pgm.ProgramID;
                        //            programWrap.WrapID = pgm.WrapIDS[i];
                        //            ctx.ProgramWrap.Add(programWrap);
                        //            ctx.SaveChanges();
                        //        }
                        //    }
                        //}
                        //ProgramWarranty.registerProgramWarranty(pgm);

                        if (pgm.programNote != "")
                        {
                            ProgramNotes pNotes = new ProgramNotes();
                            pNotes.notes_desc = pgm.programNote;
                            pNotes.programID = pgm.ProgramID;
                            ctx.ProgramNotes.Add(pNotes);
                            ctx.SaveChanges();
                        }
                    }     //Vaishnavi 12-04-2022
                    else
                    {
                        //result = "Failed to add new Program. Program " + pgm.ProgramName + " Already Exist in the system";
                        result = "Failed to add new contract. Contract " + pgm.ProgramName + " already existed.";  //Manasi 13-07-2020
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

        public static String updateProgram(Program program)
        {

            String result = "";
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    ctx.Database.Log = msg => Trace.WriteLine(msg);
                    int pgmId = program.ProgramID;
                    IQueryable<Program> programs;
                    if (program.isModified == true)
                    {
                        programs = ctx.Program.Where(p => p.OrganizationID == program.OrganizationID && p.ProgramName == program.ProgramName);
                        if (programs.ToList().Count == 0)
                        {
                            Program pgm = ctx.Program.First(p => p.ProgramID == pgmId);
                            if (pgm != null)
                            {
                                pgm.ProgramName = program.ProgramName;
                                pgm.ProgramManager = program.ProgramManager;
                                pgm.ProgramSponsor = program.ProgramSponsor;
                                pgm.ClientPOC = program.ClientPOC;
                                pgm.ClientID = program.ClientID;
                                pgm.ClientPhone = program.ClientPhone;
                                pgm.ClientEmail = program.ClientEmail;
                                //====== Jignesh-AddAddressField-21-01-2021 =======
                                //pgm.ClientAddress = program.ClientAddress;
                                pgm.ClientAddressLine1 = program.ClientAddressLine1;
                                pgm.ClientAddressLine2 = program.ClientAddressLine2;
                                pgm.ClientCity = program.ClientCity;
                                pgm.ClientState = program.ClientState;
                                pgm.ClientPONo = program.ClientPONo;
                                //=================================================
                                pgm.ProjectManager = program.ProjectManager;
                                pgm.ProjectManagerPhone = program.ProjectManagerPhone;
                                pgm.ProjectManagerEmail = program.ProjectManagerEmail;
                                pgm.ContractName = program.ContractName;
                                pgm.ContractNumber = program.ContractNumber;
                                pgm.ContractValue = program.ContractValue;       //Manasi 14-07-2020
                                pgm.JobNumber = program.JobNumber;   //Manasi 04-08-2020
                                pgm.CurrentStartDate = program.CurrentStartDate;
                                pgm.CurrentEndDate = program.CurrentEndDate;
                                pgm.originalEndDate = program.originalEndDate; //Aditya 17-02-2022
                                pgm.BillingPOC = program.BillingPOC;
                                pgm.BillingPOCPhone1 = program.BillingPOCPhone1;
                                pgm.BillingPOCPhone2 = program.BillingPOCPhone2;
                                //====== Jignesh-AddAddressField-21-01-2021 =======
                                //pgm.BillingPOCAddress = program.BillingPOCAddress;
                                pgm.BillingPOCAddressLine1 = program.BillingPOCAddressLine1;
                                pgm.BillingPOCAddressLine2 = program.BillingPOCAddressLine2;
                                pgm.BillingPOCCity = program.BillingPOCCity;
                                pgm.BillingPOCState = program.BillingPOCState;
                                pgm.BillingPOCPONo = program.BillingPOCPONo;
                                //=================================================
                                pgm.BillingPOCEmail = program.BillingPOCEmail;
                                pgm.BillingPOCSpecialInstruction = program.BillingPOCSpecialInstruction;
                                pgm.TMBilling = program.TMBilling;
                                pgm.SOVBilling = program.SOVBilling;
                                pgm.MonthlyBilling = program.MonthlyBilling;
                                pgm.CertifiedPayroll = program.CertifiedPayroll;
                                pgm.Lumpsum = program.Lumpsum;


                                pgm.ProgramManagerID = program.ProgramManagerID;
                                pgm.ProgramSponsorID = program.ProgramSponsorID;
                                pgm.ProjectClassID = program.ProjectClassID;
                                List<ProgramFund> programFundList = program.programFunds.ToList();
                                pgm.programFunds = null;
                                List<ProgramFund> fundToBeDeletedList = program.fundToBeDeleted.ToList();
                                pgm.fundToBeDeleted = null;

                                //program Categories
                                List<ProgramCategory> programCategoryList = program.programCategories.ToList();
                                pgm.programCategories = null;
                                List<ProgramCategory> categoryToBeDeleted = program.categoryToBeDeleted.ToList();
                                pgm.categoryToBeDeleted = null;
                                //pgm.IsPPBond = program.IsPPBond;
                                //pgm.IsCostPartOfContract = program.IsCostPartOfContract;
                                //pgm.PPBondNotes = program.PPBondNotes;
                                pgm.PrimeSubPrime = program.PrimeSubPrime;
                                pgm.PrimeParent = program.PrimeParent;
                                ctx.SaveChanges();
                                ContractProjectManager.SavePMList(program); //Aditya PMDD 05052022

                                for (var i = 0; i < fundToBeDeletedList.Count; i++)
                                {
                                    ProgramFund.deleteProgramFund(fundToBeDeletedList[i]);
                                }
                                foreach (var pgmf in programFundList)
                                {
                                    var fund = ctx.ProgramFund.Where(p => p.FundName == pgmf.FundName && p.ProgramID == pgmId).FirstOrDefault();
                                    if (fund == null)
                                    {

                                        pgmf.ProgramID = program.ProgramID;
                                        ProgramFund.registerProgramFund(pgmf);
                                    }
                                    else
                                    {

                                        ProgramFund.updateProgramFund(pgmf);
                                    }

                                }
                                //delete category first before adding new one
                                foreach (var cat in categoryToBeDeleted)
                                {
                                    ProgramCategory.deleteProgramCategory(cat);
                                }
                                foreach (var item in programCategoryList)
                                {
                                    var category = ctx.ProgramCategory.Where(a => a.ActivityCategoryID == item.ActivityCategoryID && a.ProgramID == pgmId).FirstOrDefault();
                                    if (category == null)
                                    {
                                        //register
                                        ProgramCategory.registerProgramCategory(category);
                                    }
                                }
                                if (program.isNotesModified == true)
                                {
                                    if (program.programNote != "")
                                    {
                                        ProgramNotes pNotes = new ProgramNotes();
                                        pNotes.notes_desc = program.programNote;
                                        pNotes.programID = program.ProgramID;
                                        ctx.ProgramNotes.Add(pNotes);
                                        ctx.SaveChanges();
                                    }
                                }
                                result = "Success";
                            }
                            else
                            {
                                result = "Update failed because Program " + program.ProgramName + " is not exist!";
                            }
                        }
                        else
                        {
                            result = "Update failed because Contract " + program.ProgramName + " already exist. Please make sure that contract name is unique!";
                        }
                    }
                    else
                    {

                        Program pgm = ctx.Program.First(p => p.ProgramID == pgmId);
                        
                        if (pgm != null)
                        {
                            pgm.ProgramName = program.ProgramName;
                            pgm.ProgramManager = program.ProgramManager;
                            pgm.ProgramSponsor = program.ProgramSponsor;
                            pgm.ClientPOC = program.ClientPOC;
                            pgm.ClientID = program.ClientID;
                            pgm.ClientPhone = program.ClientPhone;
                            pgm.ClientEmail = program.ClientEmail;

                            //====== Jignesh-AddAddressField-21-01-2021 =======
                            //pgm.ClientAddress = program.ClientAddress;
                            pgm.ClientAddressLine1 = program.ClientAddressLine1;
                            pgm.ClientAddressLine2 = program.ClientAddressLine2;
                            pgm.ClientCity = program.ClientCity;
                            pgm.ClientState = program.ClientState;
                            pgm.ClientPONo = program.ClientPONo;
                            //=================================================

                            pgm.ProjectManager = program.ProjectManager;
                            pgm.ProjectManagerPhone = program.ProjectManagerPhone;
                            pgm.ProjectManagerEmail = program.ProjectManagerEmail;
                            pgm.ContractName = program.ContractName;
                            pgm.ContractNumber = program.ContractNumber;
                            pgm.ContractValue = program.ContractValue;       //Manasi 14-07-2020
                            pgm.JobNumber = program.JobNumber;             //Manasi 04-08-2020
                            pgm.CurrentStartDate = program.CurrentStartDate;
                            pgm.CurrentEndDate = program.CurrentEndDate;
                            pgm.originalEndDate = program.originalEndDate; //Aditya 17-02-2022
                            pgm.BillingPOC = program.BillingPOC;
                            pgm.BillingPOCPhone1 = program.BillingPOCPhone1;
                            pgm.BillingPOCPhone2 = program.BillingPOCPhone2;

                            //====== Jignesh-AddAddressField-21-01-2021 =======
                            //pgm.BillingPOCAddress = program.BillingPOCAddress;
                            pgm.BillingPOCAddressLine1 = program.BillingPOCAddressLine1;
                            pgm.BillingPOCAddressLine2 = program.BillingPOCAddressLine2;
                            pgm.BillingPOCCity = program.BillingPOCCity;
                            pgm.BillingPOCState = program.BillingPOCState;
                            pgm.BillingPOCPONo = program.BillingPOCPONo;
                            //=================================================
                            pgm.BillingPOCEmail = program.BillingPOCEmail;
                            pgm.BillingPOCSpecialInstruction = program.BillingPOCSpecialInstruction;
                            pgm.TMBilling = program.TMBilling;
                            pgm.SOVBilling = program.SOVBilling;
                            pgm.MonthlyBilling = program.MonthlyBilling;
                            pgm.CertifiedPayroll = program.CertifiedPayroll;
                            pgm.Lumpsum = program.Lumpsum;


                            pgm.ProgramManagerID = program.ProgramManagerID;
                            pgm.ProgramSponsorID = program.ProgramSponsorID;
                            pgm.ProjectClassID = program.ProjectClassID;
                            List<ProgramFund> programFundList = program.programFunds.ToList();
                            pgm.programFunds = null;
                            List<ProgramFund> fundToBeDeletedList = program.fundToBeDeleted.ToList();
                            pgm.fundToBeDeleted = null;
                           // pgm.IsPPBond = program.IsPPBond;
                            //pgm.IsCostPartOfContract = program.IsCostPartOfContract;
                            //pgm.PPBondNotes = program.PPBondNotes;
                            //program Categories
                            List<ProgramCategory> programCategoryList = program.programCategories.ToList();
                            pgm.programCategories = null;
                            List<ProgramCategory> categoryToBeDeleted = program.categoryToBeDeleted.ToList();
                            pgm.categoryToBeDeleted = null;
                            pgm.PrimeSubPrime = program.PrimeSubPrime;
                            pgm.PrimeParent = program.PrimeParent;

                            ctx.SaveChanges();
                            ContractProjectManager.SavePMList(program); //Aditya PMDD 05052022
                            //pgm.IsCertifiedPayrollChecked = program.IsCertifiedPayrollChecked;    //Vaishnavi 12-04-2022
                            //pgm.IsPrevailingWageChecked = program.IsPrevailingWageChecked;
                            //pgm.IsWrapChecked = program.IsWrapChecked;
                            //pgm.ReportingTo = program.ReportingTo;
                            //ctx.SaveChanges();
                            //if (program.IsCertifiedPayrollChecked == "Yes")
                            //{
                            //    if (program.CertifiedPayrollIDS.Count != 0)
                            //    {
                            //        List<ProgramCertifiedPayroll> programids = ctx.ProgramCertifiedPayroll.Where(p => p.ProgramID == program.ProgramID).ToList();
                            //        foreach (var k in programids)
                            //        {
                            //            ctx.ProgramCertifiedPayroll.Remove(k);
                            //            ctx.SaveChanges();
                            //        }
                            //        for (int i = 0; i < program.CertifiedPayrollIDS.Count; i++)
                            //        {
                            //            ProgramCertifiedPayroll programCertifiedPayroll = new ProgramCertifiedPayroll();
                            //            programCertifiedPayroll.ProgramID = program.ProgramID;
                            //            programCertifiedPayroll.CertifiedPayrollID = program.CertifiedPayrollIDS[i];
                            //            ctx.ProgramCertifiedPayroll.Add(programCertifiedPayroll);
                            //            ctx.SaveChanges();
                            //        }
                            //    }
                            //}
                            //if (program.IsCertifiedPayrollChecked == "No")
                            //{

                            //    List<ProgramCertifiedPayroll> programids = ctx.ProgramCertifiedPayroll.Where(p => p.ProgramID == program.ProgramID).ToList();
                            //    foreach (var x in programids)
                            //    {
                            //        ctx.ProgramCertifiedPayroll.Remove(x);
                            //        ctx.SaveChanges();
                            //    }

                            //}
                            //if (program.IsPrevailingWageChecked == "Yes")
                            //{
                            //    if (program.ProgramPrevailingWagesList.Count != 0)
                            //    {
                            //        List<ProgramPrevailingWage> programids = ctx.ProgramPrevailingWage.Where(p => p.ProgramID == program.ProgramID).ToList();
                            //        foreach (var k in programids)
                            //        {
                            //            ctx.ProgramPrevailingWage.Remove(k);
                            //            ctx.SaveChanges();
                            //        }
                            //        for (int i = 0; i < program.ProgramPrevailingWagesList.Count; i++)
                            //        {
                            //            ProgramPrevailingWage programPrevailingWage = new ProgramPrevailingWage();
                            //            programPrevailingWage.ProgramID = program.ProgramID;
                            //            programPrevailingWage.Description = program.ProgramPrevailingWagesList[i];
                            //            ctx.ProgramPrevailingWage.Add(programPrevailingWage);
                            //            ctx.SaveChanges();
                            //        }
                            //    }
                            //}
                            //if (program.IsPrevailingWageChecked == "No")
                            //{

                            //    List<ProgramPrevailingWage> programids = ctx.ProgramPrevailingWage.Where(p => p.ProgramID == program.ProgramID).ToList();
                            //    foreach (var x in programids)
                            //    {
                            //        ctx.ProgramPrevailingWage.Remove(x);
                            //        ctx.SaveChanges();
                            //    }

                            //}
                            //if (program.IsWrapChecked == "Yes")
                            //{
                            //    if (program.WrapIDS.Count != 0)
                            //    {
                            //        List<ProgramWrap> programids = ctx.ProgramWrap.Where(p => p.ProgramID == program.ProgramID).ToList();
                            //        foreach (var k in programids)
                            //        {
                            //            ctx.ProgramWrap.Remove(k);
                            //            ctx.SaveChanges();
                            //        }
                            //        for (int i = 0; i < program.WrapIDS.Count; i++)
                            //        {
                            //            ProgramWrap programWrap = new ProgramWrap();
                            //            programWrap.ProgramID = program.ProgramID;
                            //            programWrap.WrapID = program.WrapIDS[i];
                            //            ctx.ProgramWrap.Add(programWrap);
                            //            ctx.SaveChanges();
                            //        }
                            //    }
                            //}
                            //if (program.IsWrapChecked == "No")
                            //{

                            //    List<ProgramWrap> programids = ctx.ProgramWrap.Where(p => p.ProgramID == program.ProgramID).ToList();
                            //    foreach (var x in programids)
                            //    {
                            //        ctx.ProgramWrap.Remove(x);
                            //        ctx.SaveChanges();
                            //    }

                            //}

                            /*ProgramWarranty.updateProgramWarranty(program);*/    //Vaishnavi 12-04-2022

                            for (var i = 0; i < fundToBeDeletedList.Count; i++)
                            {
                                ProgramFund.deleteProgramFund(fundToBeDeletedList[i]);
                            }
                            foreach (var pgmf in programFundList)
                            {

                                var fund = ctx.ProgramFund.Where(p => p.FundName == pgmf.FundName && p.ProgramID == pgmf.ProgramID).ToList();
                                if (fund.Count == 0)
                                {

                                    pgmf.ProgramID = program.ProgramID;
                                    ProgramFund.registerProgramFund(pgmf);
                                }
                                else
                                {

                                    ProgramFund.updateProgramFund(pgmf);
                                }

                            }
                            //delete category first before adding new one
                            foreach (var cat in categoryToBeDeleted)
                            {
                                ProgramCategory.deleteProgramCategory(cat);
                            }
                            foreach (var item in programCategoryList)
                            {
                                var category = ctx.ProgramCategory.Where(a => a.ActivityCategoryID == item.ActivityCategoryID && a.ProgramID == pgmId).FirstOrDefault();
                                if (category == null)
                                {
                                    //register
                                    ProgramCategory.registerProgramCategory(item);
                                }
                            }
                            if (program.isNotesModified == true)
                            {
                                if (program.programNote != "")
                                {
                                    ProgramNotes pNotes = new ProgramNotes();
                                    pNotes.notes_desc = program.programNote;
                                    pNotes.programID = program.ProgramID;
                                    ctx.ProgramNotes.Add(pNotes);
                                    ctx.SaveChanges();
                                }
                            }

                            result = "Success";
                        }
                        else
                        {
                            result = "Update failed because Program " + program.ProgramName + " is not exist!";
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

        public static void deleteCost(List<Activity> activityList, string DeletedBy)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            MySqlConnection conn = null;
            MySqlDataReader reader = null;
            foreach (var act in activityList)
            {
                List<CostFTE> costFTEList = new List<CostFTE>();
                Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
                Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
                conn = null;
                reader = null;

                try
                {
                    conn = ConnectionManager.getConnection();
                    conn.Open();
                    var query = "Select * from cost_fte where ActivityID = @ActivityID";
                    MySqlCommand command = new MySqlCommand(query, conn);
                    command.Parameters.AddWithValue("@ActivityID", act.ActivityID);
                    using (reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CostFTE RetreivedCostFTE = new CostFTE();
                            RetreivedCostFTE.ID = Convert.ToInt16(reader.GetValue(0).ToString().Trim());
                            RetreivedCostFTE.FTECostID = reader.GetValue(1).ToString().Trim();
                            RetreivedCostFTE.Granularity = reader.GetValue(2).ToString().Trim();
                            RetreivedCostFTE.ActivityID = Convert.ToInt16(reader.GetValue(3).ToString().Trim());
                            RetreivedCostFTE.FTEStartDate = reader.GetValue(4).ToString().Trim();

                            RetreivedCostFTE.FTEEndDate = reader.GetValue(5).ToString().Trim();
                            RetreivedCostFTE.FTEPosition = reader.GetValue(6).ToString().Trim();
                            RetreivedCostFTE.FTEValue = reader.GetValue(7).ToString().Trim();

                            RetreivedCostFTE.FTEHourlyRate = reader.GetValue(8).ToString().Trim();
                            RetreivedCostFTE.FTEHours = reader.GetValue(9).ToString().Trim();
                            RetreivedCostFTE.FTECost = reader.GetValue(10).ToString().Trim();


                            costFTEList.Add(RetreivedCostFTE);
                        }
                    }
                }
                catch (Exception ex)
                {
                    var stackTrace = new StackTrace(ex, true);
                    var line = stackTrace.GetFrame(0).GetFileLineNumber();
                    Logger.LogExceptions(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, ex.Message, line.ToString(), Logger.logLevel.Exception);
                }

                List<CostLumpsum> costLumpsumList = new List<CostLumpsum>();
                Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
                Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
                conn = null;
                reader = null;
                try
                {
                    conn = ConnectionManager.getConnection();
                    conn.Open();
                    var query = "Select * from cost_lumpsum where ActivityID = @ActivityID";
                    MySqlCommand command = new MySqlCommand(query, conn);
                    command.Parameters.AddWithValue("@ActivityID", act.ActivityID);
                    using (reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CostLumpsum RetreivedCostLumpsum = new CostLumpsum();
                            RetreivedCostLumpsum.ID = Convert.ToInt16(reader.GetValue(0).ToString().Trim());
                            RetreivedCostLumpsum.LumpsumCostID = reader.GetValue(1).ToString().Trim();
                            RetreivedCostLumpsum.ActivityID = Convert.ToInt16(reader.GetValue(2).ToString().Trim());
                            RetreivedCostLumpsum.LumpsumCostStartDate = reader.GetValue(3).ToString().Trim();
                            RetreivedCostLumpsum.LumpsumCostEndDate = reader.GetValue(4).ToString().Trim();

                            RetreivedCostLumpsum.LumpsumDescription = reader.GetValue(5).ToString().Trim();
                            RetreivedCostLumpsum.LumpsumCost = reader.GetValue(6).ToString().Trim();
                            RetreivedCostLumpsum.Granularity = reader.GetValue(7).ToString().Trim();


                            costLumpsumList.Add(RetreivedCostLumpsum);
                        }
                    }
                }
                catch (Exception ex)
                {
                    var stackTrace = new StackTrace(ex, true);
                    var line = stackTrace.GetFrame(0).GetFileLineNumber();
                    Logger.LogExceptions(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, ex.Message, line.ToString(), Logger.logLevel.Exception);
                }

                List<CostUnit> costUnitList = new List<CostUnit>();
                Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
                Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
                conn = null;
                reader = null;

                try
                {
                    conn = ConnectionManager.getConnection();
                    conn.Open();
                    var query = "Select * from cost_unitcost where ActivityID = @ActivityID" ;
                    MySqlCommand command = new MySqlCommand(query, conn);
                    command.Parameters.AddWithValue("@ActivityID", act.ActivityID);
                    using (reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CostUnit RetreivedCostUnit = new CostUnit();
                            RetreivedCostUnit.ID = Convert.ToInt16(reader.GetValue(0).ToString().Trim());
                            RetreivedCostUnit.UnitCostID = reader.GetValue(1).ToString().Trim();
                            RetreivedCostUnit.Granularity = reader.GetValue(2).ToString().Trim();

                            RetreivedCostUnit.ActivityID = Convert.ToInt16(reader.GetValue(3).ToString().Trim());
                            RetreivedCostUnit.UnitCostStartDate = reader.GetValue(4).ToString().Trim();
                            RetreivedCostUnit.UnitCostEndDate = reader.GetValue(5).ToString().Trim();
                            RetreivedCostUnit.UnitDescription = reader.GetValue(6).ToString().Trim();

                            RetreivedCostUnit.UnitQuantity = reader.GetValue(7).ToString().Trim();
                            RetreivedCostUnit.UnitPrice = reader.GetValue(8).ToString().Trim();
                            RetreivedCostUnit.UnitCost = reader.GetValue(9).ToString().Trim();
                            RetreivedCostUnit.UnitType = reader.GetValue(10).ToString().Trim();



                            costUnitList.Add(RetreivedCostUnit);
                        }
                    }
                }
                catch (Exception ex)
                {
                    var stackTrace = new StackTrace(ex, true);
                    var line = stackTrace.GetFrame(0).GetFileLineNumber();
                    Logger.LogExceptions(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, ex.Message, line.ToString(), Logger.logLevel.Exception);
                }

                if (costFTEList.Count > 0)
                {
                    foreach (var fte in costFTEList)
                    {
                        //Nivedita 10022022
                        //var query = "delete from cost_fte where 1=1 and FTECostID = @FTECostID";
                        var query = "update cost_fte set IsDeleted=true, DeletedDate= NOW(), DeletedBy=@DeletedBy where 1=1 and FTECostID = @FTECostID";
                        MySqlCommand command = new MySqlCommand(query, conn);
                        command.Parameters.AddWithValue("@FTECostID", fte.FTECostID);
                        command.Parameters.AddWithValue("@DeletedBy", DeletedBy);
                        command.ExecuteNonQuery();
                        // WebAPI.Models.CostFTE.updateCostFTE(fte.Operation, fte.ProgramID, fte.ProgramElementID, fte.ProjectID, fte.TrendNumber, fte.ActivityID, fte.CostID, fte.StartDate, fte.EndDate, fte.Description, fte.TextBoxValue, fte.Base, fte.FTEHours, fte.FTECost, fte.Scale);
                    }
                }
                if (costLumpsumList.Count > 0)
                {
                    foreach (var lumpsum in costLumpsumList)
                    {
                        //Nivedita 10022022
                        //var query = "delete from cost_lumpsum where 1=1 and LumpsumCostID = @LumpsumCostID";
                        var query = "update cost_lumpsum set IsDeleted=true, DeletedDate= NOW(), DeletedBy=@DeletedBy where 1=1 and LumpsumCostID = @LumpsumCostID";
                        MySqlCommand command = new MySqlCommand(query, conn);
                        command.Parameters.AddWithValue("@LumpsumCostID", lumpsum.LumpsumCostID);
                        command.Parameters.AddWithValue("@DeletedBy", DeletedBy);
                        command.ExecuteNonQuery();
                        // WebAPI.Models.CostFTE.updateCostFTE(fte.Operation, fte.ProgramID, fte.ProgramElementID, fte.ProjectID, fte.TrendNumber, fte.ActivityID, fte.CostID, fte.StartDate, fte.EndDate, fte.Description, fte.TextBoxValue, fte.Base, fte.FTEHours, fte.FTECost, fte.Scale);
                    }
                }
                if (costUnitList.Count > 0)
                {
                    foreach (var unitcost in costUnitList)
                    {
                        //Nivedita 10022022
                        //var query = "delete from cost_unitcost where 1=1 and UnitCostID = @UnitCostID";
                        var query = "update cost_unitcost set IsDeleted=true, DeletedDate= NOW(), DeletedBy=@DeletedBy where 1=1 and UnitCostID = @UnitCostID";
                        MySqlCommand command = new MySqlCommand(query, conn);
                        command.Parameters.AddWithValue("@UnitCostID", unitcost.UnitCostID);
                        command.Parameters.AddWithValue("@DeletedBy", DeletedBy);
                        command.ExecuteNonQuery();
                        // WebAPI.Models.CostFTE.updateCostFTE(fte.Operation, fte.ProgramID, fte.ProgramElementID, fte.ProjectID, fte.TrendNumber, fte.ActivityID, fte.CostID, fte.StartDate, fte.EndDate, fte.Description, fte.TextBoxValue, fte.Base, fte.FTEHours, fte.FTECost, fte.Scale);
                    }
                }

            }
        }

        public static String deleteProgram(Program program)
        {
            int ProgramID = program.ProgramID;
            String result = "";
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            MySqlConnection conn = null;
            MySqlDataReader reader = null;
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    ctx.Database.Log = msg => Trace.WriteLine(msg);
                    int pgmId = ProgramID;
                    Program pgm = ctx.Program.First(p => p.ProgramID == pgmId && p.IsDeleted==false);
                    List<ProgramElement> programElementList = ctx.ProgramElement.Where(pe => pe.ProgramID == ProgramID && pe.IsDeleted==false).Select(row => row).ToList();
                    //IQueryable<Project> projectList = ctx.Project.Where()
                    foreach (var peItem in programElementList)
                    {
                        List<Project> projectList = ctx.Project.Where(p => p.ProgramElementID == peItem.ProgramElementID && p.IsDeleted == false).Select(proj => proj).ToList();
                        foreach (var project in projectList)
                        {
                            List<Trend> trendList = ctx.Trend.Where(tr => tr.ProjectID == project.ProjectID && tr.IsDeleted==false).Select(trendItem => trendItem).ToList();
                            foreach (var trend in trendList)
                            {
                                List<Activity> activityList = new List<Activity>();

                                String delete_result = "";
                                try
                                {
                                    if (conn == null)
                                    {
                                        conn = ConnectionManager.getConnection();
                                        conn.Open();
                                    }
                                    activityList = ctx.Activity.Where(a => a.TrendNumber == trend.TrendNumber && a.ProjectID == trend.ProjectID && a.IsDeleted==false).ToList();

                                }
                                catch (Exception ex)
                                {
                                    var stackTrace = new StackTrace(ex, true);
                                    var line = stackTrace.GetFrame(0).GetFileLineNumber();
                                    Logger.LogExceptions(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, ex.Message, line.ToString(), Logger.logLevel.Exception);
                                }
                                //Nivedita 02-12-2021
                                deleteCost(activityList, program.DeletedBy);
                                if (activityList.Count > 0)
                                {
                                    foreach (var act in activityList)
                                    {
                                        act.DeletedBy = program.DeletedBy;
                                        Activity.deleteActivity(act); 

                                    }
                                }
                            }
                            //Nivedita 02 - 12 - 2021
                            if (trendList.Count > 0)
                            {

                                foreach (var tr in trendList)
                                {
                                    tr.DeletedBy = program.DeletedBy;
                                    Trend.deleteTrend(tr); 
                                }
                            }
                        }
                        if (projectList.Count > 0)
                        {

                            foreach (var p in projectList)
                            {
                                p.DeletedBy = program.DeletedBy;
                                Project.deleteProject(p);
                            }
                        }

                    }



                    if (programElementList.Count > 0)
                    {
                        foreach (var pe in programElementList)
                        {
                            //ProgramElement.deleteProgramElement(pe.ProgramElementID);
                            pe.DeletedBy = program.DeletedBy;
                            ProgramElement.deleteProgramElement(pe);
                        }
                    }
                    //delete program fund
                    if (conn == null)
                    {
                        conn = ConnectionManager.getConnection();
                        conn.Open();
                    }
                    //var query1 = "delete from program_fund where 1=1 and ProgramId = @ProgramID" ;
                    //MySqlCommand command1 = new MySqlCommand(query1, conn);
                    //command1.Parameters.AddWithValue("@ProgramID", ProgramID);
                    //command1.ExecuteNonQuery();
                   
                    //Delete Program Category
                    if (conn == null)
                    {
                        conn = ConnectionManager.getConnection();
                        conn.Open();
                    }
                    //query1 = "delete from program_category where 1=1 and ProgramID = @ProgramID";
                    //MySqlCommand command3 = new MySqlCommand(query1, conn);
                    //command3.Parameters.AddWithValue("@ProgramID", ProgramID);
                    //command3.ExecuteNonQuery();
                    //finally delete program
                    if (conn == null)
                    {
                        conn = ConnectionManager.getConnection();
                        conn.Open();
                    }
                    //var query2 = "delete from program where 1=1 and ProgramID = @ProgramID";
                    var query2 = "update program set IsDeleted=1, DeletedDate=@DeletedDate, DeletedBy=@DeletedBy, Status='Archived' where  ProgramID = @ProgramID";   //----Vaishnavi 30-03-2022----//
                    MySqlCommand command2 = new MySqlCommand(query2, conn);
                    command2.Parameters.AddWithValue("@ProgramID", pgm.ProgramID);
                    command2.Parameters.AddWithValue("@DeletedBy", program.DeletedBy);
                    command2.Parameters.AddWithValue("@DeletedDate", DateTime.Now);
                    command2.ExecuteNonQuery();
                    //ctx.Program.Remove(pgm);
                    // ctx.SaveChanges();
                    result = "Success";
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
            return result;
        }
        //----Vaishnavi 30-03-2022----//
        public static String closeProgram(Program program)
        {
            int ProgramID = program.ProgramID;
            String result = "";
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            MySqlConnection conn = null;
            MySqlDataReader reader = null;
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    ctx.Database.Log = msg => Trace.WriteLine(msg);
                    int pgmId = ProgramID;
                    Program pgm = ctx.Program.First(p => p.ProgramID == pgmId && p.IsDeleted == false);
                    List<ProgramElement> programElementList = ctx.ProgramElement.Where(pe => pe.ProgramID == ProgramID && pe.IsDeleted == false).Select(row => row).ToList();
                    //IQueryable<Project> projectList = ctx.Project.Where()
                    //foreach (var peItem in programElementList)
                    //{
                    //    List<Project> projectList = ctx.Project.Where(p => p.ProgramElementID == peItem.ProgramElementID && p.IsDeleted == false).Select(proj => proj).ToList();
                    //    foreach (var project in projectList)
                    //    {
                    //        List<Trend> trendList = ctx.Trend.Where(tr => tr.ProjectID == project.ProjectID && tr.IsDeleted == false).Select(trendItem => trendItem).ToList();
                    //        foreach (var trend in trendList)
                    //        {
                    //            List<Activity> activityList = new List<Activity>();

                    //            String delete_result = "";
                    //            try
                    //            {
                    //                if (conn == null)
                    //                {
                    //                    conn = ConnectionManager.getConnection();
                    //                    conn.Open();
                    //                }
                    //                activityList = ctx.Activity.Where(a => a.TrendNumber == trend.TrendNumber && a.ProjectID == trend.ProjectID && a.IsDeleted == false).ToList();

                    //            }
                    //            catch (Exception ex)
                    //            {
                    //                var stackTrace = new StackTrace(ex, true);
                    //                var line = stackTrace.GetFrame(0).GetFileLineNumber();
                    //                Logger.LogExceptions(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, ex.Message, line.ToString(), Logger.logLevel.Exception);
                    //            }
                    //            //Nivedita 02-12-2021
                    //            deleteCost(activityList, program.DeletedBy);
                    //            if (activityList.Count > 0)
                    //            {
                    //                foreach (var act in activityList)
                    //                {
                    //                    act.DeletedBy = program.DeletedBy;
                    //                    Activity.deleteActivity(act);

                    //                }
                    //            }
                    //        }
                    //        //Nivedita 02 - 12 - 2021
                    //        if (trendList.Count > 0)
                    //        {

                    //            foreach (var tr in trendList)
                    //            {
                    //                tr.DeletedBy = program.DeletedBy;
                    //                Trend.deleteTrend(tr);
                    //            }
                    //        }
                    //    }
                    //    if (projectList.Count > 0)
                    //    {

                    //        foreach (var p in projectList)
                    //        {
                    //            p.DeletedBy = program.DeletedBy;
                    //            Project.deleteProject(p);
                    //        }
                    //    }

                    //}



                    if (programElementList.Count > 0)
                    {
                        foreach (var pe in programElementList)
                        {
                            //ProgramElement.deleteProgramElement(pe.ProgramElementID);
                            //pe.DeletedBy = program.DeletedBy;
                            ProgramElement.closeProgramElement(pe);
                        }
                    }
                    ////delete program fund
                    //if (conn == null)
                    //{
                    //    conn = ConnectionManager.getConnection();
                    //    conn.Open();
                    //}
                    //var query1 = "delete from program_fund where 1=1 and ProgramId = @ProgramID" ;
                    //MySqlCommand command1 = new MySqlCommand(query1, conn);
                    //command1.Parameters.AddWithValue("@ProgramID", ProgramID);
                    //command1.ExecuteNonQuery();

                    //Delete Program Category
                    //if (conn == null)
                    //{
                    //    conn = ConnectionManager.getConnection();
                    //    conn.Open();
                    //}
                    //query1 = "delete from program_category where 1=1 and ProgramID = @ProgramID";
                    //MySqlCommand command3 = new MySqlCommand(query1, conn);
                    //command3.Parameters.AddWithValue("@ProgramID", ProgramID);
                    //command3.ExecuteNonQuery();
                    //finally delete program
                    if (conn == null)
                    {
                        conn = ConnectionManager.getConnection();
                        conn.Open();
                    }
                    //var query2 = "delete from program where 1=1 and ProgramID = @ProgramID";
                    var query2 = "update program set Status='Closed' where  ProgramID = @ProgramID";
                    MySqlCommand command2 = new MySqlCommand(query2, conn);
                    command2.Parameters.AddWithValue("@ProgramID", pgm.ProgramID);
                    //command2.Parameters.AddWithValue("@DeletedBy", program.DeletedBy);
                    //command2.Parameters.AddWithValue("@DeletedDate", DateTime.Now);
                    command2.ExecuteNonQuery();
                    //ctx.Program.Remove(pgm);
                    // ctx.SaveChanges();
                    result = "Success";
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
            return result;
        }

        public static String UpdateAdditionalProgramDetails(Program program)
        {
            String result = "";
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    Program pgm = ctx.Program.First(p => p.ProgramID == program.ProgramID);
                    pgm.IsPPBond = program.IsPPBond;
                    pgm.IsCostPartOfContract = program.IsCostPartOfContract;
                    pgm.PPBondNotes = program.PPBondNotes;
                    pgm.IsCertifiedPayrollChecked = program.IsCertifiedPayrollChecked;    //Vaishnavi 12-04-2022
                    pgm.IsPrevailingWageChecked = program.IsPrevailingWageChecked;
                    pgm.IsWrapChecked = program.IsWrapChecked;
                    pgm.ReportingTo = program.ReportingTo;
                    if (program.IsCertifiedPayrollChecked == "Yes")
                    {
                        if (program.CertifiedPayrollIDS.Count != 0)
                        {
                            List<ProgramCertifiedPayroll> programids = ctx.ProgramCertifiedPayroll.Where(p => p.ProgramID == program.ProgramID).ToList();
                            foreach (var k in programids)
                            {
                                ctx.ProgramCertifiedPayroll.Remove(k);
                                ctx.SaveChanges();
                            }
                            for (int i = 0; i < program.CertifiedPayrollIDS.Count; i++)
                            {
                                ProgramCertifiedPayroll programCertifiedPayroll = new ProgramCertifiedPayroll();
                                programCertifiedPayroll.ProgramID = program.ProgramID;
                                programCertifiedPayroll.CertifiedPayrollID = program.CertifiedPayrollIDS[i];
                                ctx.ProgramCertifiedPayroll.Add(programCertifiedPayroll);
                                ctx.SaveChanges();
                            }
                        }
                    }
                    if (program.IsCertifiedPayrollChecked == "No")
                    {

                        List<ProgramCertifiedPayroll> programids = ctx.ProgramCertifiedPayroll.Where(p => p.ProgramID == program.ProgramID).ToList();
                        foreach (var x in programids)
                        {
                            ctx.ProgramCertifiedPayroll.Remove(x);
                            ctx.SaveChanges();
                        }

                    }

                    if (program.IsPrevailingWageChecked == "Yes")
                    {
                        if (program.ProgramPrevailingWagesList.Count != 0)
                        {
                            List<ProgramPrevailingWage> programids = ctx.ProgramPrevailingWage.Where(p => p.ProgramID == program.ProgramID).ToList();
                            foreach (var k in programids)
                            {
                                ctx.ProgramPrevailingWage.Remove(k);
                                ctx.SaveChanges();
                            }
                            for (int i = 0; i < program.ProgramPrevailingWagesList.Count; i++)
                            {
                                ProgramPrevailingWage programPrevailingWage = new ProgramPrevailingWage();
                                programPrevailingWage.ProgramID = program.ProgramID;
                                programPrevailingWage.Description = program.ProgramPrevailingWagesList[i];
                                ctx.ProgramPrevailingWage.Add(programPrevailingWage);
                                ctx.SaveChanges();
                            }
                        }
                    }
                    if (program.IsPrevailingWageChecked == "No")
                    {

                        List<ProgramPrevailingWage> programids = ctx.ProgramPrevailingWage.Where(p => p.ProgramID == program.ProgramID).ToList();
                        foreach (var x in programids)
                        {
                            ctx.ProgramPrevailingWage.Remove(x);
                            ctx.SaveChanges();
                        }

                    }
                    if (program.IsWrapChecked == "Yes")
                    {
                        if (program.WrapIDS.Count != 0)
                        {
                            List<ProgramWrap> programids = ctx.ProgramWrap.Where(p => p.ProgramID == program.ProgramID).ToList();
                            foreach (var k in programids)
                            {
                                ctx.ProgramWrap.Remove(k);
                                ctx.SaveChanges();
                            }
                            for (int i = 0; i < program.WrapIDS.Count; i++)
                            {
                                ProgramWrap programWrap = new ProgramWrap();
                                programWrap.ProgramID = program.ProgramID;
                                programWrap.WrapID = program.WrapIDS[i];
                                ctx.ProgramWrap.Add(programWrap);
                                ctx.SaveChanges();
                            }
                        }
                    }
                    if (program.IsWrapChecked == "No")
                    {

                        List<ProgramWrap> programids = ctx.ProgramWrap.Where(p => p.ProgramID == program.ProgramID).ToList();
                        foreach (var x in programids)
                        {
                            ctx.ProgramWrap.Remove(x);
                            ctx.SaveChanges();
                        }

                    }
                    ctx.SaveChanges();

                    //ProgramWarranty.updateProgramWarranty(program);
                    //PrelimnaryNotice prelimnaryNotice = new PrelimnaryNotice();
                    //prelimnaryNotice.CreatedDate = DateTime.Now;
                    //prelimnaryNotice.ProgramID = program.ProgramID;
                    //prelimnaryNotice.CreatedBy = program.CreatedBy;
                    //prelimnaryNotice.Date = program.prelimnaryNotice.Date;
                    //prelimnaryNotice.Reason = program.prelimnaryNotice.Reason; 
                    //ctx.PrelimnaryNotices.Add(prelimnaryNotice);
                    //ctx.SaveChanges();
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

        public static Program GetProgramAdditionalInfo(int ProgramID)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            List<Program> MatchedProgramList = new List<Program>();
            Program additionalinfodata = new Program();
            try
            {
                using (var ctx = new CPPDbContext())
                {

                    additionalinfodata.IsCertifiedPayrollChecked = ctx.Program.Where(x => x.ProgramID == ProgramID).Select(x => x.IsCertifiedPayrollChecked).FirstOrDefault();
                    additionalinfodata.IsPrevailingWageChecked = ctx.Program.Where(x => x.ProgramID == ProgramID).Select(x => x.IsPrevailingWageChecked).FirstOrDefault();
                    additionalinfodata.IsWrapChecked = ctx.Program.Where(x => x.ProgramID == ProgramID).Select(x => x.IsWrapChecked).FirstOrDefault();

                    additionalinfodata.CertifiedPayrollIDS = ctx.ProgramCertifiedPayroll.Where(c => c.ProgramID == ProgramID).Select(x => x.CertifiedPayrollID).ToList();
                    additionalinfodata.ProgramPrevailingWagesList = ctx.ProgramPrevailingWage.Where(c => c.ProgramID ==ProgramID).Select(x => x.Description).ToList();
                    additionalinfodata.WrapIDS = ctx.ProgramWrap.Where(c => c.ProgramID == ProgramID).Select(x => x.WrapID).ToList();
                    additionalinfodata.ReportingTo = ctx.Program.Where(w => w.ProgramID == ProgramID).Select(p => p.ReportingTo).FirstOrDefault();

                    //additionalinfodata.LaborWarranty = ctx.ProgramWarranty.Where(w => w.ProgramID == ProgramID && w.WarrantyType == "Labor Warranty").Select(p => p.WarrantyDescription).FirstOrDefault();    //Vaishnavi 12-04-2022
                    //additionalinfodata.MaterialsWarranty = ctx.ProgramWarranty.Where(w => w.ProgramID == ProgramID && w.WarrantyType == "Materials Warranty").Select(p => p.WarrantyDescription).FirstOrDefault();
                    //additionalinfodata.OtherWarranty = ctx.ProgramWarranty.Where(w => w.ProgramID == ProgramID && w.WarrantyType == "Other Warranty").Select(p => p.WarrantyDescription).FirstOrDefault();

                    //additionalinfodata.LaborStartDate = ctx.ProgramWarranty.Where(w => w.ProgramID ==ProgramID && w.WarrantyType == "Labor Warranty").Select(p => p.StartDate).FirstOrDefault();
                    //additionalinfodata.MaterialsStartDate = ctx.ProgramWarranty.Where(w => w.ProgramID ==ProgramID && w.WarrantyType == "Materials Warranty").Select(p => p.StartDate).FirstOrDefault();
                    //additionalinfodata.OtherStartDate = ctx.ProgramWarranty.Where(w => w.ProgramID ==ProgramID && w.WarrantyType == "Other Warranty").Select(p => p.StartDate).FirstOrDefault();
                    //additionalinfodata.LaborEndDate = ctx.ProgramWarranty.Where(w => w.ProgramID == ProgramID && w.WarrantyType == "Labor Warranty").Select(p => p.EndDate).FirstOrDefault();
                    //additionalinfodata.MaterialsEndDate = ctx.ProgramWarranty.Where(w => w.ProgramID == ProgramID && w.WarrantyType == "Materials Warranty").Select(p => p.EndDate).FirstOrDefault();
                    //additionalinfodata.OtherEndDate = ctx.ProgramWarranty.Where(w => w.ProgramID == ProgramID && w.WarrantyType == "Other Warranty").Select(p => p.EndDate).FirstOrDefault();

                    //additionalinfodata.LaborStartDate = (wbsprg.LaborStartDate != null ? wbsprg.LaborStartDate.Value.ToString("yyyy-MM-dd") : "");
                    //additionalinfodata.MaterialsStartDate = (wbsprg.MaterialsStartDate != null ? wbsprg.MaterialsStartDate.Value.ToString("yyyy-MM-dd") : "");
                    //additionalinfodata.OtherStartDate = (wbsprg.OtherStartDate != null ? wbsprg.OtherStartDate.Value.ToString("yyyy-MM-dd") : "");
                    //additionalinfodata.LaborEndDate = (wbsprg.LaborEndDate != null ? wbsprg.LaborEndDate.Value.ToString("yyyy-MM-dd") : "");
                    //additionalinfodata.MaterialsEndDate = (wbsprg.MaterialsEndDate != null ? wbsprg.MaterialsEndDate.Value.ToString("yyyy-MM-dd") : "");
                    //additionalinfodata.OtherEndDate = (wbsprg.OtherEndDate != null ? wbsprg.OtherEndDate.Value.ToString("yyyy-MM-dd") : "");

                    //additionalinfodata.LaborDescription = ctx.ProgramWarranty.Where(w => w.ProgramID == ProgramID && w.WarrantyType == "Labor Warranty").Select(p => p.Description).FirstOrDefault();
                    //additionalinfodata.MaterialsDescription = ctx.ProgramWarranty.Where(w => w.ProgramID ==ProgramID && w.WarrantyType == "Materials Warranty").Select(p => p.Description).FirstOrDefault();
                    //additionalinfodata.OtherDescription = ctx.ProgramWarranty.Where(w => w.ProgramID ==ProgramID && w.WarrantyType == "Other Warranty").Select(p => p.Description).FirstOrDefault();     //Vaishnavi 12-04-2022
                    additionalinfodata.IsPPBond = ctx.Program.Where(w => w.ProgramID == ProgramID).Select(p => p.IsPPBond).FirstOrDefault();
                    additionalinfodata.IsCostPartOfContract = ctx.Program.Where(w => w.ProgramID == ProgramID).Select(p => p.IsCostPartOfContract).FirstOrDefault();
                    additionalinfodata.PPBondNotes = ctx.Program.Where(w => w.ProgramID == ProgramID).Select(p => p.PPBondNotes).FirstOrDefault(); 

                }
            }
            catch (Exception ex)
            {

                var stackTrace = new StackTrace(ex, true);
                var line = stackTrace.GetFrame(0).GetFileLineNumber();
                Logger.LogExceptions(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, ex.Message, line.ToString(), Logger.logLevel.Exception);
                string a = ex.InnerException.ToString();
            }
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Exit Point", Logger.logLevel.Debug);
            return additionalinfodata;

        }

      
       
    }
}