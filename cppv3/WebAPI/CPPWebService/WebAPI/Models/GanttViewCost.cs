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
using WebAPI.Helper;

namespace WebAPI.Models
{
    [DataContract]
    public class GanttViewCost
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
        public string RegularHours; //  Jignesh-23-03-2021 BudVSActual-UnitcostUpdate-for-OT-DT-Reg-Hrs 
        [DataMember]
        public string OTHours; //  Jignesh-23-03-2021 BudVSActual-UnitcostUpdate-for-OT-DT-Reg-Hrs 
        [DataMember]
        public string DTHours; //  Jignesh-23-03-2021 BudVSActual-UnitcostUpdate-for-OT-DT-Reg-Hrs 

        [DataMember]
        public string ActualRate; // swapnil 24-11-2020

        [DataMember]
        public string TotalActualCost; // swapnil 24-11-2020


        private static String SQL_DATE_FORMAT = "yyyy-MM-dd HH:mm:ss";

        public static List<GanttViewCost> getCosts(String ContractID, String ProjectID, String ElementID, String TrendNumber, String PhaseCode, String ActivityID, String Granularity, String BudgetID, String BudgetCategory, String BudgetSubCategory, String ViewLabor)
        {
            int CostNumber = 0;
            List<GanttViewCost> matchedCostList = new List<GanttViewCost>();

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
                matchedActivityList = WebAPI.Models.Activity.getActivityDetails("null", "null", ProjectID, TrendNumber, PhaseCode, ActivityID, "null", "null");    //Manasi 29-07-2020
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
                matchedActivityList = WebAPI.Models.Activity.getActivityDetails("null", "null", ProjectID, TrendNumber, PhaseCode, ActivityID, "null", "null");    //Manasi 29-07-2020
            }
            //-------------------------------------------------------------------------------
            else
                matchedActivityList = WebAPI.Models.Activity.getGanttViewActivityDetails(ContractID, ProjectID, ElementID, TrendNumber, PhaseCode, ActivityID, "null", "null");



            foreach (Activity ReturnedActivity in matchedActivityList)
            {
                List<CostFTE> FTECostList = new List<CostFTE>();
                List<CostLumpsum> LumpsumCostList = new List<CostLumpsum>();
                List<CostUnit> UnitCostList = new List<CostUnit>();
                List<CostODC> ODCCostList = new List<CostODC>();
                List<CostPercentage> PercentageBasisCostList = new List<CostPercentage>();
                FTECostList = WebAPI.Models.CostFTE.getCostFTE(ReturnedActivity.ActivityID.ToString(), Granularity, TrendNumber, ProjectID, PhaseCode, BudgetID, BudgetCategory, BudgetSubCategory);
                LumpsumCostList = WebAPI.Models.CostLumpsum.getCostLumpsum(ReturnedActivity.ActivityID.ToString(), Granularity, TrendNumber, ProjectID, PhaseCode, BudgetID, BudgetCategory, BudgetSubCategory);
                UnitCostList = WebAPI.Models.CostUnit.getCostUnit(ReturnedActivity.ActivityID.ToString(), Granularity, TrendNumber, ProjectID, PhaseCode, BudgetID, BudgetCategory, BudgetSubCategory);
                ODCCostList = WebAPI.Models.CostODC.getCostODC(ReturnedActivity.ActivityID.ToString(), Granularity, TrendNumber, ProjectID, PhaseCode, BudgetID, BudgetCategory, BudgetSubCategory);
                //List<CostPercentage> PercentageBasisCostList = WebAPI.Models.CostPercentage.getCostPercentage(ReturnedActivity.ActivityID);

                if (ViewLabor.Equals("1"))
                {
                    foreach (CostFTE ReturnedFTECost in FTECostList)
                    {
                        GanttViewCost tempCost = new GanttViewCost();
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
                        tempCost.RegularHours = ReturnedFTECost.RegularHours; //  Jignesh-23-03-2021 BudVSActual-UnitcostUpdate-for-OT-DT-Reg-Hrs 
                        tempCost.OTHours = ReturnedFTECost.OTHours; //  Jignesh-23-03-2021 BudVSActual-UnitcostUpdate-for-OT-DT-Reg-Hrs 
                        tempCost.DTHours = ReturnedFTECost.DTHours; //  Jignesh-23-03-2021 BudVSActual-UnitcostUpdate-for-OT-DT-Reg-Hrs 
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
                    GanttViewCost tempCost = new GanttViewCost();
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
                    GanttViewCost tempCost = new GanttViewCost();
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
                    GanttViewCost tempCost = new GanttViewCost();
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
                    GanttViewCost tempCost = new GanttViewCost();
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
                foreach (GanttViewCost cost in matchedCostList)
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


                foreach (GanttViewCost cost in matchedCostList)
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



        /// <summary>
        /// Save FTE Cost
        /// </summary>
        /// <param name="Operation">
        ///     1 - Save
        ///     2 - Update
        ///     3 - Delete
        /// </param>
        /// <param name="ProgramID"></param>
        /// <param name="ProgramElementID"></param>
        /// <param name="ProjectID"></param>
        /// <param name="TrendNumber"></param>
        /// <param name="ActivityID"></param>
        /// <param name="FTECostID"></param>
        /// <param name="FTEStartDate"></param>
        /// <param name="FTEEndDate"></param>
        /// <param name="FTEPosition"></param>
        /// <param name="FTEValue"></param>
        /// <param name="FTEHourlyRate"></param>
        /// <param name="FTEHour"></param>
        /// <param name="FTETotalCost"></param>
        /// <param name="Scale"></param>
        /// <param name="FTEPositionID"></param>
        /// <param name="CostTrackTypeID">
        /// 1 - Estimated Cost; 2 - Budget; 3 - Actual Cost; 4 - Estimated to completion;
        /// </param>
        /// <param name="EstimatedCostID"></param>
        /// <param name="EmployeeID"></param>
        /// <param name="LineNumber"></param>
        /// <returns></returns>
        public static int saveFTECost(int Operation, String ProgramID, String ProgramElementID, String ProjectID, String TrendNumber, String ActivityID, String FTECostID, String FTEStartDate,
                                        String FTEEndDate, String FTEPosition, String FTEValue, String FTEHourlyRate, String FTEHour, String FTETotalCost, String Scale,
                                        int FTEPositionID, int CostTrackTypeID, int EstimatedCostID, int EmployeeID, String LineNumber)
        {
            MySqlConnection conn = null;
            MySqlCommand command = null;
            int newCostID = 0;
            try
            {

                conn = ConnectionManager.getConnection();
                conn.Open();
                String query = "alter_cost_fte"; //Stored Procedure
                command = new MySqlCommand(query, conn);
                command.CommandType = CommandType.StoredProcedure;
                String currentUser = UserUtil.getCurrentUserName();
                //For Create New
                command.Parameters.Add(new MySqlParameter("_Operation", Operation));
                command.Parameters.Add(new MySqlParameter("_ProgramID", ProgramID));
                command.Parameters.Add(new MySqlParameter("_ProgramElementID", ProgramElementID));
                command.Parameters.Add(new MySqlParameter("_ProjectID", ProjectID));
                command.Parameters.Add(new MySqlParameter("_TrendNumber", TrendNumber));
                command.Parameters.Add(new MySqlParameter("_ActivityID", ActivityID));

                command.Parameters.Add(new MySqlParameter("_FTECostID", FTECostID));
                command.Parameters.Add(new MySqlParameter("_FTEStartDate", FTEStartDate));
                command.Parameters.Add(new MySqlParameter("_FTEEndDate", FTEEndDate));
                command.Parameters.Add(new MySqlParameter("_FTEPosition", FTEPosition));
                command.Parameters.Add(new MySqlParameter("_FTEValue", FTEValue));
                command.Parameters.Add(new MySqlParameter("_FTEHourlyRate", FTEHourlyRate));
                command.Parameters.Add(new MySqlParameter("_FTEHours", FTEHour));
                command.Parameters.Add(new MySqlParameter("_FTECost", FTETotalCost));
                command.Parameters.Add(new MySqlParameter("_Granularity", Scale));
                command.Parameters.Add(new MySqlParameter("_FTEPositionID", FTEPositionID));
                command.Parameters.Add(new MySqlParameter("_CostTrackTypeID", 1)); //1 - estimated (planned) cost

                command.Parameters.Add(new MySqlParameter("_EstimatedCostID", EstimatedCostID)); //0 
                command.Parameters.Add(new MySqlParameter("_EmployeeID", EmployeeID));
                //var lineItem = project.ProjectClassID + "." + project.ProjectNumber + "." +
                //                    activity.PhaseCode + "." + category.CategoryID + "." + category.SubCategoryID + "." + lineNumber;
                command.Parameters.Add(new MySqlParameter("_LineNumber", LineNumber));
                //Audit fields
                command.Parameters.Add(new MySqlParameter("_CreatedBy", currentUser));
                command.Parameters.Add(new MySqlParameter("_UpdatedBy", currentUser));
                command.Parameters.Add(new MySqlParameter("_CreatedDate", DateTime.UtcNow.ToString(DateUtility.getSqlDateFormat())));
                command.Parameters.Add(new MySqlParameter("_UpdatedDate", DateTime.UtcNow.ToString(DateUtility.getSqlDateFormat())));


                //command.ExecuteNonQuery();
                object tmp = command.ExecuteScalar();
                int.TryParse(tmp.ToString(), out newCostID);
                Console.WriteLine("TESTING");
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

            }


            return newCostID;
        }


        public static int saveLumpsumCost(String Operation, String ProgramID, String ProgramElementID, String ProjectID, String TrendNumber, String ActivityID, String LumpsumCostID,
                                        String StartDate, String EndDate, String Description, String LumpsumCost, String Granularity, int CostTrackTypeID, int EstimatedCostID,
                                        int SubcontractorTypeID, int SubcontractorID, String LineNumber)
        {
            int newCostID = 0;
            MySqlConnection conn = null;
            MySqlCommand command = null;
            try
            {
                conn = ConnectionManager.getConnection();
                conn.Open();
                String query = "alter_cost_lumpsum"; //Stored Procedure
                command = new MySqlCommand(query, conn);
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 0;
                String currentUser = UserUtil.getCurrentUserName();
                //For delete
                command.Parameters.Add(new MySqlParameter("_Operation", Operation));
                command.Parameters.Add(new MySqlParameter("_ProgramID", ProgramID));
                command.Parameters.Add(new MySqlParameter("_ProgramElementID", ProgramElementID));
                command.Parameters.Add(new MySqlParameter("_ProjectID", ProjectID));
                command.Parameters.Add(new MySqlParameter("_TrendNumber", TrendNumber));
                command.Parameters.Add(new MySqlParameter("_ActivityID", ActivityID));
                command.Parameters.Add(new MySqlParameter("_LumpsumCostID", LumpsumCostID));
                command.Parameters.Add(new MySqlParameter("_LumpsumCostStartDate", (StartDate != null && StartDate != "") ? DateTime.Parse(StartDate).ToString(SQL_DATE_FORMAT) : ""));
                command.Parameters.Add(new MySqlParameter("_LumpsumCostEndDate", (EndDate != null && EndDate != "") ? DateTime.Parse(EndDate).ToString(SQL_DATE_FORMAT) : ""));
                command.Parameters.Add(new MySqlParameter("_LumpsumDescription", Description));
                command.Parameters.Add(new MySqlParameter("_LumpsumCost", LumpsumCost));
                command.Parameters.Add(new MySqlParameter("_Granularity", Granularity));
                command.Parameters.Add(new MySqlParameter("_CostTrackTypeID", CostTrackTypeID));//Don't update this
                command.Parameters.Add(new MySqlParameter("_EstimatedCostID", EstimatedCostID));//Don't update this
                command.Parameters.Add(new MySqlParameter("_SubcontractorTypeID", SubcontractorTypeID));
                command.Parameters.Add(new MySqlParameter("_SubcontractorID", SubcontractorID));
                command.Parameters.Add(new MySqlParameter("_LineNumber", LineNumber));
                //Audit fields
                command.Parameters.Add(new MySqlParameter("_CreatedBy", currentUser));
                command.Parameters.Add(new MySqlParameter("_UpdatedBy", currentUser));
                command.Parameters.Add(new MySqlParameter("_CreatedDate", DateTime.UtcNow.ToString(DateUtility.getSqlDateFormat())));
                command.Parameters.Add(new MySqlParameter("_UpdatedDate", DateTime.UtcNow.ToString(DateUtility.getSqlDateFormat())));
                object tmp = command.ExecuteScalar();
                int.TryParse(tmp.ToString(), out newCostID);



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
            }

            return newCostID;
        }

        public static int saveODCCost(String Operation, String ProgramID, String ProgramElementID, String ProjectID, String TrendNumber, String ActivityID, String ODCCostID, String ODCStartDate,
                                        String ODCEndDate, String ODCQuantity, int UnitMaterialID, String ODCPrice, String ODCCost, String Granularity, int ODCTypeID, int CostTrackTypeID,
                                        int EstimatedCostID, String LineNumber)
        {
            int newCostID = 0;
            MySqlConnection conn = null;
            MySqlCommand command = null;
            try
            {
                conn = ConnectionManager.getConnection();
                conn.Open();
                String query = "alter_cost_odc"; //Stored Procedure
                command = new MySqlCommand(query, conn);
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 0;
                String currentUser = UserUtil.getCurrentUserName();
                //For delete
                command.Parameters.Add(new MySqlParameter("_Operation", Operation));
                command.Parameters.Add(new MySqlParameter("_ProgramID", ProgramID));
                command.Parameters.Add(new MySqlParameter("_ProgramElementID", ProgramElementID));
                command.Parameters.Add(new MySqlParameter("_ProjectID", ProjectID));
                command.Parameters.Add(new MySqlParameter("_TrendNumber", TrendNumber));
                command.Parameters.Add(new MySqlParameter("_ActivityID", ActivityID));

                command.Parameters.Add(new MySqlParameter("_ODCCostID", ODCCostID));
                command.Parameters.Add(new MySqlParameter("_ODCStartDate", (ODCStartDate != null && ODCStartDate != "") ? DateTime.Parse(ODCStartDate).ToString(SQL_DATE_FORMAT) : ""));
                command.Parameters.Add(new MySqlParameter("_ODCEndDate", (ODCEndDate != null && ODCEndDate != "") ? DateTime.Parse(ODCEndDate).ToString(SQL_DATE_FORMAT) : ""));
                //command.Parameters.Add(new MySqlParameter("_UnitDescription", ""));
                command.Parameters.Add(new MySqlParameter("_ODCQuantity", ODCQuantity));
                command.Parameters.Add(new MySqlParameter("_UnitMaterialID", UnitMaterialID));
                command.Parameters.Add(new MySqlParameter("_ODCPrice", ODCPrice));

                command.Parameters.Add(new MySqlParameter("_ODCCost", ODCCost));
                command.Parameters.Add(new MySqlParameter("_Granularity", Granularity));
                //command.Parameters.Add(new MySqlParameter("_UnitType", ""));
                command.Parameters.Add(new MySqlParameter("_ODCTypeID", ODCTypeID));
                command.Parameters.Add(new MySqlParameter("_CostTrackTypeID", CostTrackTypeID)); //1 - estimated (planned) cost
                command.Parameters.Add(new MySqlParameter("_EstimatedCostID", EstimatedCostID)); //0 
                command.Parameters.Add(new MySqlParameter("_LineNumber", LineNumber));
                //Audit fields
                command.Parameters.Add(new MySqlParameter("_CreatedBy", currentUser));
                command.Parameters.Add(new MySqlParameter("_UpdatedBy", currentUser));
                command.Parameters.Add(new MySqlParameter("_CreatedDate", DateTime.UtcNow.ToString(DateUtility.getSqlDateFormat())));
                command.Parameters.Add(new MySqlParameter("_UpdatedDate", DateTime.UtcNow.ToString(DateUtility.getSqlDateFormat())));
                object tmp = command.ExecuteScalar();
                int.TryParse(tmp.ToString(), out newCostID);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            return 0;
        }

        public static int saveUnitCost(String Operation, String ProgramID, String ProgramElementID, String ProjectID, String TrendNumber, String ActivityID, String UnitCostID, String StartDate, String EndDate,
                                        String UnitDescription, String UnitQuantity, String UnitMaterialCategoryID, String UnitMaterialID, String UnitPrice, String UnitCost, String Granularity,
                                        String UnitType, String UnitType_ID, int CostTrackTypeID, int EstimatedCostID, String LineNumber)
        {

            int newCostID = 0;
            MySqlConnection conn = null;
            MySqlCommand command = null;
            try
            {
                conn = ConnectionManager.getConnection();
                conn.Open();
                String query = "alter_cost_unitcost"; //Stored Procedure
                command = new MySqlCommand(query, conn);
                command.CommandType = CommandType.StoredProcedure;
                String currentUser = UserUtil.getCurrentUserName();
                //For delete
                command.Parameters.Add(new MySqlParameter("_Operation", Operation));
                command.Parameters.Add(new MySqlParameter("_ProgramID", ProgramID));
                command.Parameters.Add(new MySqlParameter("_ProgramElementID", ProgramElementID));
                command.Parameters.Add(new MySqlParameter("_ProjectID", ProjectID));
                command.Parameters.Add(new MySqlParameter("_TrendNumber", TrendNumber));
                command.Parameters.Add(new MySqlParameter("_ActivityID", ActivityID));

                command.Parameters.Add(new MySqlParameter("_UnitCostID", UnitCostID));
                command.Parameters.Add(new MySqlParameter("_UnitCostStartDate", (StartDate != null && StartDate != "") ? DateTime.Parse(StartDate).ToString(SQL_DATE_FORMAT) : ""));
                command.Parameters.Add(new MySqlParameter("_UnitCostEndDate", (EndDate != null && EndDate != "") ? DateTime.Parse(EndDate).ToString(SQL_DATE_FORMAT) : ""));
                command.Parameters.Add(new MySqlParameter("_UnitDescription", UnitDescription));
                command.Parameters.Add(new MySqlParameter("_UnitQuantity", UnitQuantity));
                command.Parameters.Add(new MySqlParameter("_UnitMaterialCategoryID", UnitMaterialCategoryID));
                command.Parameters.Add(new MySqlParameter("_UnitMaterialID", UnitMaterialID));
                command.Parameters.Add(new MySqlParameter("_UnitPrice", UnitPrice));

                command.Parameters.Add(new MySqlParameter("_UnitCost", UnitCost));
                command.Parameters.Add(new MySqlParameter("_Granularity", Granularity));
                command.Parameters.Add(new MySqlParameter("_UnitType", UnitType));
                command.Parameters.Add(new MySqlParameter("_UnitType_ID", UnitType_ID));
                command.Parameters.Add(new MySqlParameter("_CostTrackTypeID", CostTrackTypeID)); //1 - estimated (planned) cost
                command.Parameters.Add(new MySqlParameter("_EstimatedCostID", EstimatedCostID)); //0 
                command.Parameters.Add(new MySqlParameter("_LineNumber", LineNumber));
                //Audit fields
                command.Parameters.Add(new MySqlParameter("_CreatedBy", currentUser));
                command.Parameters.Add(new MySqlParameter("_UpdatedBy", currentUser));
                command.Parameters.Add(new MySqlParameter("_CreatedDate", DateTime.UtcNow.ToString(DateUtility.getSqlDateFormat())));
                command.Parameters.Add(new MySqlParameter("_UpdatedDate", DateTime.UtcNow.ToString(DateUtility.getSqlDateFormat())));
                object obj = command.ExecuteScalar();
                int.TryParse(obj.ToString(), out newCostID);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
            }


            return newCostID;
        }

        // Get the cost type by the line item.
        public static string getCostTypeByLineItem(String lineItem)
        {
            string type = "";

            MySqlConnection conn = null;
            MySqlCommand command = null;
            try
            {
                conn = ConnectionManager.getConnection();
                conn.Open();
                String query = "get_costtype_by_line_item"; // Stored Procedure
                command = new MySqlCommand(query, conn);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new MySqlParameter("_lineItem", lineItem));

                object obj = command.ExecuteScalar();
                type = Convert.ToString(obj);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
            }

            return type;
        }

    }
}