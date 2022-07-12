using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Web;
using WebAPI.Controllers;

namespace WebAPI.Models
{
    [Table("phase_lookup")]
    public class ViewPrgmElementGantt:Audit
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


        public static List<ViewPrgmElementGantt> getPhaseCode(String PhaseDescription, String PhaseCode, String ProgramElementID)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            List<ViewPrgmElementGantt> MatchedPhaseCodeList = new List<ViewPrgmElementGantt>();

            MySqlConnection conn = null;
            MySqlDataReader reader = null;
            try
            {
                //Luan experimental
                if (ProgramElementID != "null")
                {
                    conn = ConnectionManager.getConnection();
                    conn.Open();

                    var query = String.Format("call get_phases_by_programElement_id({0})", ProgramElementID);// phase mapping by division
                                                                                               // var query = String.Format("Call get_phase_by_lob({0})", ProjectID);
                    MySqlCommand command = new MySqlCommand(query, conn);

                    using (reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ViewPrgmElementGantt pc = new ViewPrgmElementGantt();
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
    public class ViewProgramElementGanttActivities
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
                    var id = Convert.ToInt16(ProgramElementID);
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
                        MatchedActivityList = ctx.Database.SqlQuery<Activity>("call get_forecast_activities_ForRollUp_ProjectGantt(@ProgramElementID)",
                                                 new MySql.Data.MySqlClient.MySqlParameter("@ProgramElementID", ProgramElementID)).ToList();


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

                        MatchedActivityList = ctx.Database.SqlQuery<Activity>("call get_current_activities_ForRollUp_ProjectGantt(@ProgramElementID)",
                                                 new MySql.Data.MySqlClient.MySqlParameter("@ProgramElementID", ProgramElementID)).ToList();

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
                            var aID = Convert.ToInt16(ProgramElementID);
                            var trendId = Convert.ToInt16(TrendNumber);

                            //MatchedActivityList = ctx.Activity.Where(a => a.ProgramElementID == ProgramElementID && a.TrendNumber == TrendNumber).ToList();
                            MatchedActivityList = ctx.Database.SqlQuery<Activity>("call get_baseline_activities_ProjectGantt(@ProgramElementID,@TrendNumber)",
                                                 new MySql.Data.MySqlClient.MySqlParameter("@ProgramElementID", ProgramElementID),
                                                 new MySql.Data.MySqlClient.MySqlParameter("@TrendNumber", TrendNumber)).ToList();

                            if (TrendNumber == "1000")
                                MatchedActivityList = MatchedActivityList.Where(a => a.TrendNumber == "1000").ToList();

                            MatchedActivityList = processCost(MatchedActivityList, TrendNumber, ProjectID, PhaseCode, BudgetCategory, BudgetSubCategory);
                            
                        }
                        else
                        if (ActivityID != "null")
                        {
                            var aID = Convert.ToInt16(ActivityID);
                            MatchedActivityList = ctx.Activity.Where(a => a.ProjectID == id && a.TrendNumber == TrendNumber && a.ActivityID == aID).ToList();
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

    public class ViewProgramElementGanttCost
    {
        //Max Cost ID
        public static int MaxFCostID = 0;
        public static int MaxLCostID = 0;
        public static int MaxUCostID = 0;
        public static int MaxODCCostID = 0;
        public static int MaxPCostID = 0;

        //Common
        [DataMember]
        public int DT_RowID; //Iterative ID for display in table in Front end
        [DataMember]
        public int Operation;
        [DataMember]
        public String CostType; //FTECost || UnitCost || PercentageBasisCost || LumpsumCost
        [DataMember]
        public String ProgramID;
        [DataMember]
        public String ProgramElementID;
        [DataMember]
        public String ProjectID;
        [DataMember]
        public String TrendNumber;
        [DataMember]
        public String PhaseCode;
        [DataMember]
        public String ActivityID;
        [DataMember]
        public String Scale;
        [DataMember]
        public String TextBoxID;
        [DataMember]
        public String UnitType;
        //Uncommon single valued
        [DataMember]
        public String CostID;
        [DataMember]
        public String Description; //FTEPosition || LumpsumDescription || UnitDescription || PercentageBasisDescription 
        [DataMember]
        public String StartDate;
        [DataMember]
        public String EndDate;
        [DataMember]
        public String TextBoxValue; //FTEValue || LumpsumCost || UnitQuantity || PercentageBasisPercentageValue
        [DataMember]
        public String CostWithOverhead; //FTEValue || LumpsumCost || UnitQuantity || PercentageBasisPercentageValue
        [DataMember]
        public String Base; //FTEHourlyRate || UnitPrice || PercentageBasisBaseAmount
        [DataMember]
        public String Drag_Direction;
        [DataMember]
        public String NumberOfTextboxToBeRemoved;
        [DataMember]
        public String CostTrackTypes;
        //Only FTE
        [DataMember]
        public String FTEIDList;          //List of all the textbox ID of each cost
        [DataMember]
        public String FTECost;
        [DataMember]
        public String FTEHours;
        [DataMember]
        public int? FTEPositionID;
        [DataMember]
        public int EstimatedCostID;
        [DataMember]
        public int CostTrackTypeID;
        [DataMember]
        public int ODCTypeID;
        [DataMember]
        public int EmployeeID;

        //Only 
        [DataMember]
        public string RawFTEHourlyRate;
        [DataMember]
        public string RawUnitPrice;

        //Only Material
        [DataMember]
        public int MaterialID;
        [DataMember]
        public int MaterialCategoryID;


        //Only Subcontractor
        [DataMember]
        public int SubcontractorID;
        [DataMember]
        public int SubcontractorTypeID;
        [DataMember]
        public String CostLineItemID;
        [DataMember]
        public String CreatedBy;
        [DataMember]
        public String UpdatedBy;
        [DataMember]
        public DateTime CreatedDate;
        [DataMember]
        public DateTime UpdatedDate;

        [DataMember]
        public string ActualRate; // swapnil 24-11-2020

        [DataMember]
        public string TotalActualCost; // swapnil 24-11-2020


        private static String SQL_DATE_FORMAT = "yyyy-MM-dd HH:mm:ss";

        public static List<Cost> getCosts(String ProjectID, String TrendNumber, String PhaseCode, String ActivityID, String Granularity, String BudgetID, String BudgetCategory, String BudgetSubCategory, String ViewLabor, String ProgramElementID)
        {
            int CostNumber = 0;
            List<Cost> matchedCostList = new List<Cost>();

            List<Activity> matchedActivityList = new List<Activity>();

            if (TrendNumber == "2000")
            {
                var activity = new Activity();
                activity.ProjectID = int.Parse(ProjectID);
                activity.TrendNumber = TrendNumber;
                //activity.PhaseCode = int.Parse(PhaseCode);
                //activity.PhaseCode = PhaseCode != null || PhaseCode != "null" ? int.Parse(PhaseCode) : 0; //Manasi 29-07-2020
                if (PhaseCode != "null")
                {
                    activity.PhaseCode = int.Parse(PhaseCode);
                }
                activity.ActivityID = int.Parse("0");//ActivityID is not required
                activity.BudgetCategory = BudgetCategory;
                activity.BudgetSubCategory = BudgetSubCategory;

                //matchedActivityList.Add(activity);
                matchedActivityList = WebAPI.Models.ViewProgramElementGanttActivities.getActivityDetails("null", "null", ProjectID, TrendNumber, PhaseCode, ActivityID, "null", "null");    //Manasi 29-07-2020
            }
            //--------------------------Manasi 10-11-2020--------------------------------
            else if (TrendNumber == "3000")
            {
                var activity = new Activity();
                activity.ProjectID = int.Parse(ProjectID);
                activity.TrendNumber = TrendNumber;
                //activity.PhaseCode = int.Parse(PhaseCode);
                //activity.PhaseCode = PhaseCode != null || PhaseCode != "null" ? int.Parse(PhaseCode) : 0; //Manasi 29-07-2020
                if (PhaseCode != "null")
                {
                    activity.PhaseCode = int.Parse(PhaseCode);
                }
                activity.ActivityID = int.Parse("0");//ActivityID is not required
                activity.BudgetCategory = BudgetCategory;
                activity.BudgetSubCategory = BudgetSubCategory;

                //matchedActivityList.Add(activity);
                matchedActivityList = WebAPI.Models.ViewProgramElementGanttActivities.getActivityDetails("null", "null", ProjectID, TrendNumber, PhaseCode, ActivityID, "null", "null");    //Manasi 29-07-2020
            }
            //-------------------------------------------------------------------------------
            else
                matchedActivityList = WebAPI.Models.ViewProgramElementGanttActivities.getActivityDetails("null", "null", ProjectID, TrendNumber, PhaseCode, ActivityID, "null", "null");



            foreach (Activity ReturnedActivity in matchedActivityList)
            {
                List<CostFTE> FTECostList = new List<CostFTE>();
                List<CostLumpsum> LumpsumCostList = new List<CostLumpsum>();
                List<CostUnit> UnitCostList = new List<CostUnit>();
                List<CostODC> ODCCostList = new List<CostODC>();
                List<CostPercentage> PercentageBasisCostList = new List<CostPercentage>();
                FTECostList = WebAPI.Models.CostFTE.getCostFTE(ReturnedActivity.ActivityID.ToString(), Granularity, TrendNumber, ReturnedActivity.ProjectID.ToString(), PhaseCode, BudgetID, BudgetCategory, BudgetSubCategory);
                LumpsumCostList = WebAPI.Models.CostLumpsum.getCostLumpsum(ReturnedActivity.ActivityID.ToString(), Granularity, TrendNumber, ReturnedActivity.ProjectID.ToString(), PhaseCode, BudgetID, BudgetCategory, BudgetSubCategory);
                UnitCostList = WebAPI.Models.CostUnit.getCostUnit(ReturnedActivity.ActivityID.ToString(), Granularity, TrendNumber, ReturnedActivity.ProjectID.ToString(), PhaseCode, BudgetID, BudgetCategory, BudgetSubCategory);
                ODCCostList = WebAPI.Models.CostODC.getCostODC(ReturnedActivity.ActivityID.ToString(), Granularity, TrendNumber, ReturnedActivity.ProjectID.ToString(), PhaseCode, BudgetID, BudgetCategory, BudgetSubCategory);
                //List<CostPercentage> PercentageBasisCostList = WebAPI.Models.CostPercentage.getCostPercentage(ReturnedActivity.ActivityID);

                if (ViewLabor.Equals("1"))
                {
                    foreach (CostFTE ReturnedFTECost in FTECostList)
                    {
                        Cost tempCost = new Cost();
                        tempCost.CostType = "F";
                        tempCost.ActivityID = ReturnedFTECost.ActivityID.ToString();
                        tempCost.CostID = ReturnedFTECost.FTECostID;
                        tempCost.TextBoxID = ReturnedFTECost.TextBoxID;
                        tempCost.StartDate = ReturnedFTECost.FTEStartDate;
                        tempCost.EndDate = ReturnedFTECost.FTEEndDate;
                        tempCost.Description = ReturnedFTECost.FTEPosition;
                        tempCost.TextBoxValue = ReturnedFTECost.FTEValue.ToString();
                        tempCost.Base = ReturnedFTECost.FTEHourlyRate.ToString();
                        tempCost.Scale = ReturnedFTECost.Granularity;

                        tempCost.FTECost = ReturnedFTECost.FTECost.ToString();
                        tempCost.FTEHours = ReturnedFTECost.FTEHours.ToString();
                        tempCost.FTEPositionID = ReturnedFTECost.FTEPositionID;
                        tempCost.EstimatedCostID = ReturnedFTECost.EstimatedCostID;
                        tempCost.CostTrackTypeID = ReturnedFTECost.CostTrackTypeID;
                        tempCost.EmployeeID = ReturnedFTECost.EmployeeID;
                        tempCost.DT_RowID = CostNumber;
                        tempCost.CostTrackTypes = ReturnedFTECost.CostTrackTypes;
                        tempCost.CostLineItemID = ReturnedFTECost.CostLineItemID;
                        tempCost.RawFTEHourlyRate = (ReturnedFTECost.RawFTEHourlyRate == null) ? "0" : ReturnedFTECost.RawFTEHourlyRate;
                        tempCost.RawUnitPrice = tempCost.RawFTEHourlyRate;
                        CostNumber += 1;
                        //swapnil 24-11-2020
                        tempCost.ActualRate = ReturnedFTECost.ActualFTEHourlyRate;
                        tempCost.TotalActualCost = ReturnedFTECost.ActualBudget;
                        tempCost.CreatedDate = DateTime.Now; tempCost.UpdatedDate = DateTime.Now; // Manasi
                        matchedCostList.Add(tempCost);

                        if (MaxFCostID < int.Parse(tempCost.CostID))
                            MaxFCostID = int.Parse(tempCost.CostID);

                    }
                }

                foreach (CostLumpsum ReturnedLumpsumCost in LumpsumCostList)
                {
                    Cost tempCost = new Cost();
                    tempCost.CostType = "L";
                    tempCost.ActivityID = ReturnedLumpsumCost.ActivityID.ToString();
                    tempCost.CostID = ReturnedLumpsumCost.LumpsumCostID;
                    tempCost.TextBoxID = ReturnedLumpsumCost.TextBoxID;
                    tempCost.StartDate = ReturnedLumpsumCost.LumpsumCostStartDate;
                    tempCost.EndDate = ReturnedLumpsumCost.LumpsumCostEndDate;
                    tempCost.Description = ReturnedLumpsumCost.LumpsumDescription;
                    tempCost.TextBoxValue = ReturnedLumpsumCost.LumpsumCost;//Original Cost
                    tempCost.CostWithOverhead = ReturnedLumpsumCost.LumpsumCost;
                    tempCost.Scale = ReturnedLumpsumCost.Granularity;
                    tempCost.EstimatedCostID = ReturnedLumpsumCost.EstimatedCostID;
                    tempCost.CostTrackTypeID = ReturnedLumpsumCost.CostTrackTypeID;
                    tempCost.SubcontractorTypeID = ReturnedLumpsumCost.SubcontractorTypeID;
                    tempCost.SubcontractorID = ReturnedLumpsumCost.SubcontractorID;
                    tempCost.CostTrackTypes = ReturnedLumpsumCost.CostTrackTypes;
                    tempCost.CostLineItemID = ReturnedLumpsumCost.CostLineItemID;
                    tempCost.RawUnitPrice = ReturnedLumpsumCost.OriginalCost;
                    tempCost.DT_RowID = CostNumber;
                    CostNumber += 1;
                    //swapnil 24-11-2020
                    tempCost.ActualRate = "0";
                    tempCost.TotalActualCost = ReturnedLumpsumCost.ActualBudget;
                    tempCost.CreatedDate = DateTime.Now; tempCost.UpdatedDate = DateTime.Now; // Manasi
                    matchedCostList.Add(tempCost);

                    if (MaxLCostID < int.Parse(tempCost.CostID))
                        MaxLCostID = int.Parse(tempCost.CostID);
                }


                foreach (CostUnit ReturnedUnitCost in UnitCostList)
                {
                    Cost tempCost = new Cost();
                    tempCost.CostType = "U";
                    tempCost.ActivityID = ReturnedUnitCost.ActivityID.ToString();
                    tempCost.CostID = ReturnedUnitCost.UnitCostID;
                    tempCost.TextBoxID = ReturnedUnitCost.TextBoxID;
                    tempCost.StartDate = ReturnedUnitCost.UnitCostStartDate;
                    tempCost.EndDate = ReturnedUnitCost.UnitCostEndDate;
                    tempCost.Description = ReturnedUnitCost.UnitDescription;
                    tempCost.CostWithOverhead = ReturnedUnitCost.UnitCost;
                    tempCost.TextBoxValue = ReturnedUnitCost.UnitQuantity;
                    tempCost.Base = ReturnedUnitCost.UnitPrice;
                    tempCost.UnitType = ReturnedUnitCost.UnitType;
                    tempCost.Scale = ReturnedUnitCost.Granularity;
                    tempCost.EstimatedCostID = ReturnedUnitCost.EstimatedCostID;
                    tempCost.CostTrackTypeID = ReturnedUnitCost.CostTrackTypeID;
                    tempCost.MaterialID = ReturnedUnitCost.MaterialID;
                    tempCost.MaterialCategoryID = ReturnedUnitCost.MaterialCategoryID;
                    tempCost.CostTrackTypes = ReturnedUnitCost.CostTrackTypes;
                    tempCost.CostLineItemID = ReturnedUnitCost.CostLineItemID;
                    tempCost.RawUnitPrice = ReturnedUnitCost.RawUnitPrice;
                    tempCost.DT_RowID = CostNumber;
                    CostNumber += 1;
                    //swapnil 24-11-2020
                    tempCost.ActualRate = ReturnedUnitCost.ActualRate; ;
                    tempCost.TotalActualCost = ReturnedUnitCost.ActualBudget;
                    tempCost.CreatedDate = DateTime.Now; tempCost.UpdatedDate = DateTime.Now; // Manasi
                    matchedCostList.Add(tempCost);

                    if (MaxUCostID < int.Parse(tempCost.CostID))
                        MaxUCostID = int.Parse(tempCost.CostID);
                }

                foreach (CostODC ReturnedODCCost in ODCCostList)
                {
                    Cost tempCost = new Cost();
                    List<ODCType> ODCTypeList = new List<ODCType>();
                    ODCTypeList = WebAPI.Models.ODCType.GetODCType();
                    String ODCTypeDescription = "";
                    for (int x = 0; x < ODCTypeList.Count; x++)
                    {
                        if (ReturnedODCCost.ODCTypeID == ODCTypeList[x].ODCTypeID)
                        {
                            ODCTypeDescription = ODCTypeList[x].ODCTypeName;
                        }
                    }

                    tempCost.CostType = "ODC";
                    tempCost.ActivityID = ReturnedODCCost.ActivityID.ToString();
                    tempCost.CostID = ReturnedODCCost.ODCCostID;
                    tempCost.TextBoxID = ReturnedODCCost.TextBoxID;
                    tempCost.StartDate = ReturnedODCCost.ODCStartDate;
                    tempCost.EndDate = ReturnedODCCost.ODCEndDate;
                    tempCost.Description = ODCTypeDescription;
                    tempCost.TextBoxValue = ReturnedODCCost.ODCCost; // The cost entered by the user
                    tempCost.CostWithOverhead = ReturnedODCCost.ODCCost;
                    tempCost.Base = ReturnedODCCost.ODCCost;
                    tempCost.UnitType = "dummy"; //ReturnedODCCost.ODCType.ODCDescription;
                    tempCost.Scale = ReturnedODCCost.Granularity;
                    tempCost.EstimatedCostID = ReturnedODCCost.EstimatedCostID;
                    tempCost.CostTrackTypeID = ReturnedODCCost.CostTrackTypeID;
                    tempCost.ODCTypeID = ReturnedODCCost.ODCTypeID;
                    tempCost.CostTrackTypes = ReturnedODCCost.CostTrackTypes;
                    tempCost.DT_RowID = CostNumber;
                    tempCost.CostLineItemID = ReturnedODCCost.CostLineItemID;
                    tempCost.RawUnitPrice = ReturnedODCCost.OriginalCost;
                    CostNumber += 1;
                    //swapnil 24-11-2020
                    tempCost.ActualRate = "0";
                    tempCost.TotalActualCost = ReturnedODCCost.ActualBudget;
                    tempCost.CreatedDate = DateTime.Now; tempCost.UpdatedDate = DateTime.Now; // Manasi
                    matchedCostList.Add(tempCost);

                    if (MaxODCCostID < int.Parse(tempCost.CostID))
                        MaxODCCostID = int.Parse(tempCost.CostID);
                }

                foreach (CostPercentage ReturnedPercentageBasisCost in PercentageBasisCostList)
                {
                    Cost tempCost = new Cost();
                    tempCost.CostType = "P";
                    // tempCost.ActivityID = ReturnedPercentageBasisCost.ActivityID;
                    tempCost.CostID = ReturnedPercentageBasisCost.PercentageBasisCostID;
                    tempCost.TextBoxID = ReturnedPercentageBasisCost.TextBoxID;
                    tempCost.StartDate = ReturnedPercentageBasisCost.PercentageBasisCostStartDate;
                    tempCost.EndDate = ReturnedPercentageBasisCost.PercentageBasisCostEndDate;
                    tempCost.Description = ReturnedPercentageBasisCost.PercentageBasisDescription;
                    tempCost.TextBoxValue = ReturnedPercentageBasisCost.PercentageBasisPercentageValue;
                    tempCost.Base = ReturnedPercentageBasisCost.PercentageBasisBaseAmount;
                    tempCost.Scale = ReturnedPercentageBasisCost.Granularity;
                    tempCost.DT_RowID = CostNumber;
                    CostNumber += 1;
                    //swapnil 24-11-2020
                    tempCost.ActualRate = "0";
                    tempCost.TotalActualCost = "0";
                    tempCost.CreatedDate = DateTime.Now; tempCost.UpdatedDate = DateTime.Now; // Manasi
                    matchedCostList.Add(tempCost);

                    if (MaxPCostID > int.Parse(tempCost.CostID))
                        MaxPCostID = int.Parse(tempCost.CostID);
                }
            }
            if (matchedCostList.Count > 0)
            {
                int maxNoBox = 0;
                DateTime minStartDate;
                DateTime maxEndDate;
                List<DateTime> dtList = new List<DateTime>();
                foreach (Cost cost in matchedCostList)
                {
                    List<String> boxes = cost.StartDate.Split(',').ToList();
                    foreach (String dt in boxes)
                    {
                        dtList.Add(Convert.ToDateTime(dt));
                    }

                    boxes = cost.EndDate.Split(',').ToList();
                    foreach (String dt in boxes)
                    {
                        dtList.Add(Convert.ToDateTime(dt));
                    }
                }
                minStartDate = dtList.Min();
                maxEndDate = dtList.Max();
                System.TimeSpan diffResult = maxEndDate.Subtract(minStartDate);
                maxNoBox = (diffResult.Days / 7) + 1;


                foreach (Cost cost in matchedCostList)
                {
                    List<String> boxes = cost.CostTrackTypes.Split(',').ToList();
                    String firstBox = boxes.First();
                    var diff = maxNoBox - boxes.Count;

                    while (diff > 0)
                    {
                        boxes.Add(firstBox);
                        diff--;
                    };

                    cost.CostTrackTypes = String.Join(", ", boxes.ToArray());

                }
            }

            return matchedCostList;
        }

        public static String getLineItem(String ProjectClassLineItemID, String ProjectNumber, String SubProjectNumber, String PhaseCode, String CategoryID,
                                            String SubCategoryID, String LineNumber, String Year, String CostType, String ProgramElementClassLineItemID)
        {
            String costLineItem = null;
            using (var ctx = new CPPDbContext())
            {
                String pCode = PhaseCode;
                int projectLength = ProjectNumber.Length - 3;
                var pNumber = ProjectNumber.Substring(projectLength);
                //Project subProject = ctx.Project.Where(a => a.ProjectNumber == pNumber).FirstOrDefault();
                //String ProjectID = subProject.ProgramElementID.ToString();
                // ProgramElement project = ctx.ProgramElement.Include("ProjectClass").Where(a => a.ProjectNumber == ProjectID).FirstOrDefault();
                PhaseCode phase = ctx.PhaseCode.Where(a => a.ActivityPhaseCode == pCode).FirstOrDefault();
                String phaseCode = "";
                if (ProjectClassLineItemID != null && ProjectClassLineItemID.ToString().Length < 2)
                    ProjectClassLineItemID = "0" + ProjectClassLineItemID;
                if (phase != null && phase.ActivityPhaseCode != null && phase.ActivityPhaseCode.ToString().Length < 2)
                    phaseCode = "0" + phase.ActivityPhaseCode.ToString();
                else
                    phaseCode = phase.ActivityPhaseCode.ToString();
                //LineId LineNo = ctx.Database.SqlQuery<LineId>("call GetMaxLineNo(@vProjectID, @vPhaseCode, @vCategoryId)",
                //                                      new MySql.Data.MySqlClient.MySqlParameter("@vProjectID", ProjectID),
                //                                      new MySql.Data.MySqlClient.MySqlParameter("@vPhaseCode", phaseCode),
                //                                      new MySql.Data.MySqlClient.MySqlParameter("@vCategoryId", CategoryID)).FirstOrDefault();

                //int maxLineId = 0;
                //if (LineNo != null)
                //    maxLineId = LineNo.MaxLineID;
                //LineNumber = (maxLineId + 1).ToString().Length < 2 ? "0" + (maxLineId + 1).ToString() : (maxLineId + 1).ToString();
                //foreach (var item in LineNo)
                //{
                //    maxLineId = item.MaxLineID;
                //}
                //costLineItem = ProjectClassLineItemID + "."
                //                    + ProjectNumber + "."
                //                    + SubProjectNumber + "."
                //                    + phaseCode + "."
                //                    + CategoryID + "."
                //                    + SubCategoryID + "."
                //                    + LineNumber;



                costLineItem = ProgramElementClassLineItemID
                                        + Year
                                        + ProjectNumber.Substring(ProjectNumber.Length - 3, 3)
                                        //+ ProjectClassLineItemID.Substring(0, 1)
                                        + ProjectClassLineItemID
                                        + SubProjectNumber.Substring(SubProjectNumber.Length - 2, 2)
                                        + phaseCode
                                        + CategoryID
                                        + CostType
                                        + LineNumber;

            }

            return costLineItem;
        }
    }
}