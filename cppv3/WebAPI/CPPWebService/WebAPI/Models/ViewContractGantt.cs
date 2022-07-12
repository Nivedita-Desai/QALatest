
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Web;
using WebAPI.Controllers;

namespace WebAPI.Models
{
    [Table("phase_lookup")]
    public class ViewContractGantt : Audit
    {
        [NotMapped]
        public int Operation;

        [NotMapped]
        public int Order { get; set; }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PhaseID { get; set; }

        public String PhaseDescription { get; set; }
        public String Code { get; set; }
        public String ActivityPhaseCode { get; set; }

        public String PhaseNote { get; set; }

        public int ProjectID { get; set; }


        public static List<ViewContractGantt> getPhaseCode(String PhaseDescription, String PhaseCode, String ProgramID)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            List<ViewContractGantt> MatchedPhaseCodeList = new List<ViewContractGantt>();

            MySqlConnection conn = null;
            MySqlDataReader reader = null;
            try
            {
                //Luan experimental
                if (ProgramID != "null")
                {
                    conn = ConnectionManager.getConnection();
                    conn.Open();

                    var query = String.Format("call get_phases_by_program_id({0})", ProgramID);// phase mapping by division
                                                                                                             // var query = String.Format("Call get_phase_by_lob({0})", ProjectID);
                    MySqlCommand command = new MySqlCommand(query, conn);

                    using (reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ViewContractGantt pc = new ViewContractGantt();
                            pc.PhaseID = Convert.ToInt32(reader.GetValue(0));
                            pc.PhaseDescription = reader.GetValue(1).ToString();
                            pc.Code = reader.GetValue(2).ToString();
                            pc.ActivityPhaseCode = reader.GetValue(3).ToString();
                            //reader.getValue(4) return: ProjectID
                            pc.Order = Convert.ToInt32(reader.GetValue(5));
                            pc.ProjectID = Convert.ToInt32(reader.GetValue(4));

                            MatchedPhaseCodeList.Add(pc);
                        }
                    }

                    //MatchedPhaseCodeList = ctx.Database.SqlQuery<PhaseCode>("call get_phases_by_project_id(@_ProjectID)",
                    //            new MySql.Data.MySqlClient.MySqlParameter("@_ProjectID", ProjectID)).ToList();

                }
                else
                {
                    using (var ctx = new CPPDbContext())
                    {
                        //MatchedPhaseCodeList = ctx.PhaseCode.ToList();
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
                if (conn != null) conn.Close();
                if (reader != null) reader.Close();
            }
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Exit Point", Logger.logLevel.Debug);
            return MatchedPhaseCodeList;

        }
    }

    [Table("activity")]
    public class ViewContractGanttActivities
    {
        [NotMapped]
        public int Operation { get; set; }
        [NotMapped]
        public String ProgramID { get; set; }
        [NotMapped]
        public String ProgramElementID { get; set; }

        //luan experimental
        [NotMapped]
        public Double FTECostForecast { get; set; }
        [NotMapped]
        public Double LumpsumCostForecast { get; set; }
        [NotMapped]
        public Double UnitCostForecast { get; set; }
        [NotMapped]
        public Double OdcCostForecast { get; set; }

        [NotMapped]
        public Double FTEBudget { get; set; }
        [NotMapped]
        public Double LumpsumBudget { get; set; }
        [NotMapped]
        public Double UnitBudget { get; set; }
        [NotMapped]
        public Double OdcBudget { get; set; }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ActivityID { get; set; }
        public int ProjectID { get; set; }
        public string TrendNumber { get; set; }
        public int PhaseCode { get; set; }
        public int TrendID { get; set; }
        public String BudgetCategory { get; set; }
        public String BudgetSubCategory { get; set; }
        public String ActivityStartDate { get; set; }
        public String ActivityEndDate { get; set; }
        public String OriginalActivityStartDate { get; set; }
        public String OriginalActivityEndDate { get; set; }
        public int PercentageCompletion { get; set; }

        public Double FTECost { get; set; }
        public Double LumpsumCost { get; set; }
        public Double UnitCost { get; set; }
        public Double OdcCost { get; set; }


        public Double FTECostActual { get; set; }
        public Double LumpsumCostActual { get; set; }
        public Double UnitCostActual { get; set; }
        public Double OdcCostActual { get; set; }

        public Double PercentageBasisCost { get; set; }
        public int OrganizationID { get; set; }
        public int BudgetID { get; set; }

        //Navigation properties
        [ForeignKey("BudgetID")]
        public virtual ActivityCategory ActivityCategory { get; set; }
        [ForeignKey("ProjectID")]
        public virtual Project Project { get; set; }
        [ForeignKey("TrendID")]
        public virtual Trend Trend { get; set; }
        [ForeignKey("PhaseCode")]
        public virtual PhaseCode Phase { get; set; }

        //1 to many
        public virtual ICollection<CostFTE> FTECosts { get; set; }
        public virtual ICollection<CostLumpsum> LumpsumCosts { get; set; }
        public virtual ICollection<CostUnit> UnitCosts { get; set; }
        public virtual ICollection<CostPercentage> PercentageCosts { get; set; }
        //ODC
        public virtual ICollection<CostODC> ODCCosts { get; set; }


        public static List<Activity> getActivityDetails(String ProgramID, String ProgramElementID, String ProjectID, String TrendNumber, String PhaseCode, String ActivityID, String BudgetCategory, String BudgetSubCategory)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            List<Activity> MatchedActivityList = new List<Activity>();
            MySqlConnection conn = null;
            MySqlDataReader reader = null;
            try
            {

                using (var ctx = new CPPDbContext())
                {
                    var id = Convert.ToInt16(ProgramID);
                    if (TrendNumber == "2000") //Forecast not stored in the database
                    {
                        //------------Manasi 29-07-2020-----------------------------
                        int aID1 = 0;
                        int pID1 = 0;
                        if (ActivityID != null && ActivityID != "null")
                            aID1 = Convert.ToInt16(ActivityID);
                        if (PhaseCode != null && PhaseCode != "null")
                            pID1 = Convert.ToInt16(PhaseCode);
                        // ---------------------------------------------------

                        //MatchedActivityList = ctx.Database.SqlQuery<Activity>("call get_forecast_activities(@ProjectID)",
                        //                         new MySql.Data.MySqlClient.MySqlParameter("@ProjectID", ProjectID)).ToList();  

                        //Manasi 10-11-2020
                        MatchedActivityList = ctx.Database.SqlQuery<Activity>("call get_forecast_activities_ForRollUp_ContractGantt(@ProgramID)",
                                                 new MySql.Data.MySqlClient.MySqlParameter("@ProgramID", ProgramID)).ToList();


                        //int i = 1;
                        //foreach (var act in MatchedActivityList)
                        //{
                        //    act.ActivityID = i;
                        //    i++;

                        //}

                        //------------Manasi 29-07-2020-----------------------------
                        if (ActivityID != "null")
                        {
                            MatchedActivityList = MatchedActivityList.Where(a => a.ActivityID == aID1).ToList();
                        }
                        else if (PhaseCode != "null")
                        {
                            MatchedActivityList = MatchedActivityList.Where(a => a.PhaseCode == pID1).ToList();
                        }
                        //MatchedActivityList = processCost(MatchedActivityList, TrendNumber, ProjectID, PhaseCode, BudgetCategory, BudgetSubCategory);
                        //-------------------Manasi 10-11-2020--------------------------
                        MatchedActivityList = processCostForRollUp(MatchedActivityList, TrendNumber, ProjectID, PhaseCode, BudgetCategory, BudgetSubCategory);

                    }

                    //-------------------Manasi 10-11-2020--------------------------
                    else if (TrendNumber == "3000")
                    {
                        //------------Manasi 29-07-2020-----------------------------
                        int aID1 = 0;
                        int pID1 = 0;
                        if (ActivityID != null && ActivityID != "null")
                            aID1 = Convert.ToInt16(ActivityID);
                        if (PhaseCode != null && PhaseCode != "null")
                            pID1 = Convert.ToInt16(PhaseCode);
                        // ---------------------------------------------------

                        MatchedActivityList = ctx.Database.SqlQuery<Activity>("call get_current_activities_ForRollUp_ContractGantt(@ProgramID)",
                                                 new MySql.Data.MySqlClient.MySqlParameter("@ProgramID", ProgramID)).ToList();

                        //------------Manasi 29-07-2020-----------------------------
                        if (ActivityID != "null")
                        {
                            MatchedActivityList = MatchedActivityList.Where(a => a.ActivityID == aID1).ToList();
                        }
                        else if (PhaseCode != "null")
                        {
                            MatchedActivityList = MatchedActivityList.Where(a => a.PhaseCode == pID1).ToList();
                        }
                        MatchedActivityList = processCostForRollUp(MatchedActivityList, TrendNumber, ProjectID, PhaseCode, BudgetCategory, BudgetSubCategory);

                    }
                    else
                    {
                        if (ActivityID == "null" && PhaseCode == "null")
                        {
                            //var aID = Convert.ToInt16(ProjectID);
                            var aID = Convert.ToInt16(ProgramID);
                            var trendId = Convert.ToInt16(TrendNumber);

                            //MatchedActivityList = ctx.Activity.Where(a => a.ProgramElementID == ProgramElementID && a.TrendNumber == TrendNumber).ToList();
                            MatchedActivityList = ctx.Database.SqlQuery<Activity>("call get_baseline_activities_ContractGantt(@ProgramID,@TrendNumber)",
                                                 new MySql.Data.MySqlClient.MySqlParameter("@ProgramID", ProgramID),
                                                 new MySql.Data.MySqlClient.MySqlParameter("@TrendNumber", TrendNumber)).ToList();

                            if (TrendNumber == "1000")
                                MatchedActivityList = MatchedActivityList.Where(a => a.TrendNumber == "1000").ToList();

                            MatchedActivityList = processCost(MatchedActivityList, TrendNumber, ProjectID, PhaseCode, BudgetCategory, BudgetSubCategory);

                        }
                        else
                        if (ActivityID != "null")
                        {
                            var aID = Convert.ToInt16(ActivityID);
                            MatchedActivityList = ctx.Activity.Where(a => a.TrendNumber == TrendNumber && a.ActivityID == aID).ToList();
                            if (TrendNumber == "1000")
                            {
                                MatchedActivityList = MatchedActivityList.Where(a => a.TrendNumber == "1000" && a.ActivityID == aID).ToList();
                                MatchedActivityList = processCost(MatchedActivityList, TrendNumber, ProjectID, PhaseCode, BudgetCategory, BudgetSubCategory);
                            }
                        }
                        else if (PhaseCode != "null")
                        {
                            var phaseCode = Convert.ToInt16(PhaseCode);
                            MatchedActivityList = ctx.Activity.Where(a => a.ProjectID == id && a.TrendNumber == TrendNumber && a.PhaseCode == phaseCode).ToList();
                            if (TrendNumber == "1000")
                            {
                                MatchedActivityList = MatchedActivityList.Where(a => a.TrendNumber == "1000" && a.PhaseCode == phaseCode).ToList();
                                MatchedActivityList = processCost(MatchedActivityList, TrendNumber, ProjectID, PhaseCode, BudgetCategory, BudgetSubCategory);
                            }
                        }
                        else
                        {
                            MatchedActivityList = ctx.Activity.Where(a => a.ProjectID == id && a.TrendNumber == TrendNumber).ToList();
                            if (TrendNumber == "1000")
                            {
                                MatchedActivityList = MatchedActivityList.Where(a => a.TrendNumber == "1000").ToList();
                                MatchedActivityList = processCost(MatchedActivityList, TrendNumber, ProjectID, PhaseCode, BudgetCategory, BudgetSubCategory);
                            }
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

                if (conn != null) conn.Close();
                if (reader != null) reader.Close();
            }
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Exit Point", Logger.logLevel.Debug);
            return MatchedActivityList;

        }

        public static List<Activity> processCostForRollUp(List<Activity> MatchedActivityList, string TrendNumber, String ProjectID, String PhaseCode, String BudgetCategory, String BudgetSubCategory)
        {
            List<Activity> result = new List<Activity>();

            for (int x = 0; x < MatchedActivityList.Count; x++)
            {
                List<CostFTE> FTECostList = new List<CostFTE>();
                List<CostLumpsum> LumpsumCostList = new List<CostLumpsum>();
                List<CostUnit> UnitCostList = new List<CostUnit>();
                List<CostODC> ODCCostList = new List<CostODC>();
                List<CostPercentage> PercentageBasisCostList = new List<CostPercentage>();
                FTECostList = WebAPI.Models.CostFTE.getCostFTE(MatchedActivityList[x].ActivityID.ToString(), "week", TrendNumber, MatchedActivityList[x].ProjectID.ToString(), PhaseCode, "", BudgetCategory, BudgetSubCategory);
                LumpsumCostList = WebAPI.Models.CostLumpsum.getCostLumpsum(MatchedActivityList[x].ActivityID.ToString(), "week", TrendNumber, MatchedActivityList[x].ProjectID.ToString(), PhaseCode, "", BudgetCategory, BudgetSubCategory);
                UnitCostList = WebAPI.Models.CostUnit.getCostUnit(MatchedActivityList[x].ActivityID.ToString(), "week", TrendNumber, MatchedActivityList[x].ProjectID.ToString(), PhaseCode, "", BudgetCategory, BudgetSubCategory);
                ODCCostList = WebAPI.Models.CostODC.getCostODC(MatchedActivityList[x].ActivityID.ToString(), "week", TrendNumber, MatchedActivityList[x].ProjectID.ToString(), PhaseCode, "", BudgetCategory, BudgetSubCategory);

                double totalFTEActual = 0, totalLumpsumActual = 0, totalUnitActual = 0, totalODCActual = 0;
                double totalFTEForecast = 0, totalLumpsumForecast = 0, totalUnitForecast = 0, totalODCForecast = 0;
                double totalFTEBudget = 0, totalLumpsumBudget = 0, totalUnitBudget = 0, totalODCBudget = 0;

                //FTE
                for (int y = 0; y < FTECostList.Count; y++)
                {
                    if (FTECostList[y].CostTrackTypeID == 3 || FTECostList[y].CostTrackTypeID == 4)
                    {
                        var costTrackTypeAr = FTECostList[y].CostTrackTypes.Split(',');
                        var costAr = FTECostList[y].OriginalCost.Split(',');
                        for (int z = 0; z < costTrackTypeAr.Length; z++)
                        {
                            if (costTrackTypeAr[z].Trim() == "3")
                            {
                                totalFTEActual += Convert.ToDouble(costAr[z]);
                            }
                            else if (costTrackTypeAr[z].Trim() == "4")
                            {
                                totalFTEForecast += Convert.ToDouble(costAr[z]);
                            }
                        }
                    }

                    if (true)
                    {
                        string[] costs = FTECostList[y].FTECost.Split(',');
                        double scale = float.Parse(FTECostList[y].RawFTEHourlyRate) / float.Parse(FTECostList[y].FTEHourlyRate);
                        foreach (string cost in costs)
                        {
                            if (FTECostList[y].CostTrackTypeID == 1 || FTECostList[y].CostTrackTypeID == 2)
                                totalFTEBudget += float.Parse(cost) * scale;
                        }
                    }
                }

                //Lumpsum
                for (int y = 0; y < LumpsumCostList.Count; y++)
                {
                    if (LumpsumCostList[y].CostTrackTypeID == 3 || LumpsumCostList[y].CostTrackTypeID == 4)
                    {
                        var costTrackTypeAr = LumpsumCostList[y].CostTrackTypes.Split(',');
                        var costAr = LumpsumCostList[y].OriginalCost.Split(',');
                        for (int z = 0; z < costTrackTypeAr.Length; z++)
                        {
                            if (costTrackTypeAr[z].Trim() == "3")
                            {
                                totalLumpsumActual += Convert.ToDouble(costAr[z]);
                            }
                            else if (costTrackTypeAr[z].Trim() == "4")
                            {
                                totalLumpsumForecast += Convert.ToDouble(costAr[z]);
                            }
                        }
                    }

                    if (true)
                    {
                        string[] costs = LumpsumCostList[y].OriginalCost.Split(',');
                        foreach (string cost in costs)
                        {
                            if (LumpsumCostList[y].CostTrackTypeID == 1 || LumpsumCostList[y].CostTrackTypeID == 2)
                                totalLumpsumBudget += float.Parse(cost);
                        }
                    }
                }

                //Unit
                for (int y = 0; y < UnitCostList.Count; y++)
                {
                    if (UnitCostList[y].CostTrackTypeID == 3 || UnitCostList[y].CostTrackTypeID == 4)
                    {
                        var costTrackTypeAr = UnitCostList[y].CostTrackTypes.Split(',');
                        // var costAr = UnitCostList[y].OriginalCost.Split(',');
                        string[] costs = UnitCostList[y].UnitQuantity.Split(',');
                        float price = float.Parse(UnitCostList[y].RawUnitPrice);
                        for (int z = 0; z < costTrackTypeAr.Length; z++)
                        {
                            if (costTrackTypeAr[z].Trim() == "3")
                            {
                                totalUnitActual += Convert.ToDouble(float.Parse(costs[z]) * price);
                            }
                            else if (costTrackTypeAr[z].Trim() == "4")
                            {
                                totalUnitForecast += Convert.ToDouble(float.Parse(costs[z]) * price);
                            }
                        }
                    }

                    if (true)
                    {
                        string[] costs = UnitCostList[y].UnitQuantity.Split(',');
                        float price = float.Parse(UnitCostList[y].RawUnitPrice);
                        foreach (string cost in costs)
                        {
                            if (UnitCostList[y].CostTrackTypeID == 1 || UnitCostList[y].CostTrackTypeID == 2)
                                totalUnitBudget += float.Parse(cost) * price;
                        }
                    }
                }

                //ODC
                for (int y = 0; y < ODCCostList.Count; y++)
                {
                    if (ODCCostList[y].CostTrackTypeID == 3 || ODCCostList[y].CostTrackTypeID == 4)
                    {
                        var costTrackTypeAr = ODCCostList[y].CostTrackTypes.Split(',');
                        var costAr = ODCCostList[y].OriginalCost.Split(',');
                        for (int z = 0; z < costTrackTypeAr.Length; z++)
                        {
                            if (costTrackTypeAr[z].Trim() == "3")
                            {
                                totalODCActual += Convert.ToDouble(costAr[z]);
                            }
                            else if (costTrackTypeAr[z].Trim() == "4")
                            {
                                totalODCForecast += Convert.ToDouble(costAr[z]);
                            }
                        }
                    }

                    if (true)
                    {
                        string[] costs = ODCCostList[y].OriginalCost.Split(',');
                        foreach (string cost in costs)
                        {
                            if (ODCCostList[y].CostTrackTypeID == 1 || ODCCostList[y].CostTrackTypeID == 2)
                                totalODCBudget += float.Parse(cost);
                        }
                    }
                }

                MatchedActivityList[x].FTECostActual = totalFTEActual;
                MatchedActivityList[x].FTECostForecast = totalFTEForecast;
                MatchedActivityList[x].LumpsumCostActual = totalLumpsumActual;
                MatchedActivityList[x].LumpsumCostForecast = totalLumpsumForecast;
                MatchedActivityList[x].UnitCostActual = totalUnitActual;
                MatchedActivityList[x].UnitCostForecast = totalUnitForecast;
                MatchedActivityList[x].OdcCostActual = totalODCActual;
                MatchedActivityList[x].OdcCostForecast = totalODCForecast;

                MatchedActivityList[x].FTEBudget = totalFTEBudget;
                MatchedActivityList[x].LumpsumBudget = totalLumpsumBudget;
                MatchedActivityList[x].UnitBudget = totalUnitBudget;
                MatchedActivityList[x].OdcBudget = totalODCBudget;
            }

            result = MatchedActivityList;
            return result;
        }

        public static List<Activity> processCost(List<Activity> MatchedActivityList, string TrendNumber, String ProjectID, String PhaseCode, String BudgetCategory, String BudgetSubCategory)
        {
            List<Activity> result = new List<Activity>();

            for (int x = 0; x < MatchedActivityList.Count; x++)
            {
                List<CostFTE> FTECostList = new List<CostFTE>();
                List<CostLumpsum> LumpsumCostList = new List<CostLumpsum>();
                List<CostUnit> UnitCostList = new List<CostUnit>();
                List<CostODC> ODCCostList = new List<CostODC>();
                List<CostPercentage> PercentageBasisCostList = new List<CostPercentage>();
                FTECostList = WebAPI.Models.CostFTE.getCostFTE(MatchedActivityList[x].ActivityID.ToString(), "week", TrendNumber, MatchedActivityList[x].ProjectID.ToString(), PhaseCode, "", BudgetCategory, BudgetSubCategory);
                LumpsumCostList = WebAPI.Models.CostLumpsum.getCostLumpsum(MatchedActivityList[x].ActivityID.ToString(), "week", TrendNumber, MatchedActivityList[x].ProjectID.ToString(), PhaseCode, "", BudgetCategory, BudgetSubCategory);
                UnitCostList = WebAPI.Models.CostUnit.getCostUnit(MatchedActivityList[x].ActivityID.ToString(), "week", TrendNumber, MatchedActivityList[x].ProjectID.ToString(), PhaseCode, "", BudgetCategory, BudgetSubCategory);
                ODCCostList = WebAPI.Models.CostODC.getCostODC(MatchedActivityList[x].ActivityID.ToString(), "week", TrendNumber, MatchedActivityList[x].ProjectID.ToString(), PhaseCode, "", BudgetCategory, BudgetSubCategory);

                double totalFTEActual = 0, totalLumpsumActual = 0, totalUnitActual = 0, totalODCActual = 0;
                double totalFTEForecast = 0, totalLumpsumForecast = 0, totalUnitForecast = 0, totalODCForecast = 0;
                double totalFTEBudget = 0, totalLumpsumBudget = 0, totalUnitBudget = 0, totalODCBudget = 0;

                //FTE
                for (int y = 0; y < FTECostList.Count; y++)
                {
                    if (FTECostList[y].CostTrackTypeID == 3 || FTECostList[y].CostTrackTypeID == 4)
                    {
                        var costTrackTypeAr = FTECostList[y].CostTrackTypes.Split(',');
                        var costAr = FTECostList[y].OriginalCost.Split(',');
                        for (int z = 0; z < costTrackTypeAr.Length; z++)
                        {
                            if (costTrackTypeAr[z].Trim() == "3")
                            {
                                totalFTEActual += Convert.ToDouble(costAr[z]);
                            }
                            else if (costTrackTypeAr[z].Trim() == "4")
                            {
                                totalFTEForecast += Convert.ToDouble(costAr[z]);
                            }
                        }
                    }

                    if (true)
                    {
                        string[] costs = FTECostList[y].FTECost.Split(',');
                        double scale = float.Parse(FTECostList[y].RawFTEHourlyRate) / float.Parse(FTECostList[y].FTEHourlyRate);
                        foreach (string cost in costs)
                        {
                            if (FTECostList[y].CostTrackTypeID == 1 || FTECostList[y].CostTrackTypeID == 2)
                                totalFTEBudget += float.Parse(cost) * scale;
                        }
                    }
                }

                //Lumpsum
                for (int y = 0; y < LumpsumCostList.Count; y++)
                {
                    if (LumpsumCostList[y].CostTrackTypeID == 3 || LumpsumCostList[y].CostTrackTypeID == 4)
                    {
                        var costTrackTypeAr = LumpsumCostList[y].CostTrackTypes.Split(',');
                        var costAr = LumpsumCostList[y].OriginalCost.Split(',');
                        for (int z = 0; z < costTrackTypeAr.Length; z++)
                        {
                            if (costTrackTypeAr[z].Trim() == "3")
                            {
                                totalLumpsumActual += Convert.ToDouble(costAr[z]);
                            }
                            else if (costTrackTypeAr[z].Trim() == "4")
                            {
                                totalLumpsumForecast += Convert.ToDouble(costAr[z]);
                            }
                        }
                    }

                    if (true)
                    {
                        string[] costs = LumpsumCostList[y].OriginalCost.Split(',');
                        foreach (string cost in costs)
                        {
                            if (LumpsumCostList[y].CostTrackTypeID == 1 || LumpsumCostList[y].CostTrackTypeID == 2)
                                totalLumpsumBudget += float.Parse(cost);
                        }
                    }
                }

                //Unit
                for (int y = 0; y < UnitCostList.Count; y++)
                {
                    if (UnitCostList[y].CostTrackTypeID == 3 || UnitCostList[y].CostTrackTypeID == 4)
                    {
                        var costTrackTypeAr = UnitCostList[y].CostTrackTypes.Split(',');
                        // var costAr = UnitCostList[y].OriginalCost.Split(',');
                        string[] costs = UnitCostList[y].UnitQuantity.Split(',');
                        float price = float.Parse(UnitCostList[y].RawUnitPrice);
                        for (int z = 0; z < costTrackTypeAr.Length; z++)
                        {
                            if (costTrackTypeAr[z].Trim() == "3")
                            {
                                totalUnitActual += Convert.ToDouble(float.Parse(costs[z]) * price);
                            }
                            else if (costTrackTypeAr[z].Trim() == "4")
                            {
                                totalUnitForecast += Convert.ToDouble(float.Parse(costs[z]) * price);
                            }
                        }
                    }

                    if (true)
                    {
                        string[] costs = UnitCostList[y].UnitQuantity.Split(',');
                        float price = float.Parse(UnitCostList[y].RawUnitPrice);
                        foreach (string cost in costs)
                        {
                            if (UnitCostList[y].CostTrackTypeID == 1 || UnitCostList[y].CostTrackTypeID == 2)
                                totalUnitBudget += float.Parse(cost) * price;
                        }
                    }
                }

                //ODC
                for (int y = 0; y < ODCCostList.Count; y++)
                {
                    if (ODCCostList[y].CostTrackTypeID == 3 || ODCCostList[y].CostTrackTypeID == 4)
                    {
                        var costTrackTypeAr = ODCCostList[y].CostTrackTypes.Split(',');
                        var costAr = ODCCostList[y].OriginalCost.Split(',');
                        for (int z = 0; z < costTrackTypeAr.Length; z++)
                        {
                            if (costTrackTypeAr[z].Trim() == "3")
                            {
                                totalODCActual += Convert.ToDouble(costAr[z]);
                            }
                            else if (costTrackTypeAr[z].Trim() == "4")
                            {
                                totalODCForecast += Convert.ToDouble(costAr[z]);
                            }
                        }
                    }

                    if (true)
                    {
                        string[] costs = ODCCostList[y].OriginalCost.Split(',');
                        foreach (string cost in costs)
                        {
                            if (ODCCostList[y].CostTrackTypeID == 1 || ODCCostList[y].CostTrackTypeID == 2)
                                totalODCBudget += float.Parse(cost);
                        }
                    }
                }

                MatchedActivityList[x].FTECostActual = totalFTEActual;
                MatchedActivityList[x].FTECostForecast = totalFTEForecast;
                MatchedActivityList[x].LumpsumCostActual = totalLumpsumActual;
                MatchedActivityList[x].LumpsumCostForecast = totalLumpsumForecast;
                MatchedActivityList[x].UnitCostActual = totalUnitActual;
                MatchedActivityList[x].UnitCostForecast = totalUnitForecast;
                MatchedActivityList[x].OdcCostActual = totalODCActual;
                MatchedActivityList[x].OdcCostForecast = totalODCForecast;

                MatchedActivityList[x].FTEBudget = totalFTEBudget;
                MatchedActivityList[x].LumpsumBudget = totalLumpsumBudget;
                MatchedActivityList[x].UnitBudget = totalUnitBudget;
                MatchedActivityList[x].OdcBudget = totalODCBudget;
            }

            result = MatchedActivityList;
            return result;
        }
    }
}