using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models.ExportToMPP;
using System.Globalization;
using Microsoft.Office.Interop.MSProject;
using WebAPI.Helper;
using System.Configuration;
using System.Web;
using System.IO;
using System.Threading;
using System.Runtime.InteropServices;
using System.Text;
using System.Diagnostics;
using WebAPI.CustomHandler;

namespace WebAPI.Controllers
{
    public class ExportToMPPController : ApiController
    {
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private ExportProcess _ExportProcess;
        private readonly string workingFolder = HttpRuntime.AppDomainAppPath + @"Uploads\";

        [HttpGet]
        [Route("Request/Export/")]
        public HttpResponseMessage GetFile(int projectId, int trendNumber, string granularity, int phaseId)
        {
            int attempts = 5;   //reattempt

            //Thread.Sleep(Convert.ToInt32(ConfigurationManager.AppSettings["MPPAttemptWaitTime"]));
            Thread.Sleep(2000);

            for (int attemptNum = 0; attemptNum < attempts; attemptNum++)
            {
                try
                {
                    Application appclass = new Application();

                    Type officeType = Type.GetTypeFromProgID("MSProject.Application");



                    //logger.Info(" before ReleaseComObject - 1");

                    //Marshal.ReleaseComObject(appclass);

                    //logger.Info(" after ReleaseComObject - 1");

                    if (officeType == null)
                    {
                        var jsonNew = new
                        {
                            result = "Failed"
                        };
                        return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
                    }
                    else
                    {
                        //logger.Info("start of mpp export function");
                        MessageLogging.LogException("start of mpp export function");
                        string filePath = ConfigurationManager.AppSettings["LocalMppFilePath"].ToString();
                        _ExportProcess = new ExportProcess();
                        //var lstExportData = _ExportProcess.Export(projectId, trendNumber, granularity);
                        List<ExportData> lstExportData = _ExportProcess.Export(projectId, trendNumber, granularity);
                        if (phaseId != 0)
                        {
                            lstExportData = lstExportData.FindAll(d => d.Order == phaseId);
                        }
                        string proName = _ExportProcess.getProName(projectId);
                        string trend = "";
                        string query = "";
                        if (trendNumber.ToString() == "0")
                        {
                            trend = "Baseline";
                            query = "GetTotalCost";
                        }
                        else if (trendNumber.ToString() == "1000")
                        {
                            trend = "Current";
                            query = "GetTotalCost";
                        }
                        else if (trendNumber.ToString() == "2000")
                        {
                            trend = "Forecast";
                            query = "GetForecastTotalCost";
                        }
                        var totalCost = _ExportProcess.getTotalCost(projectId, trendNumber, granularity, query);
                        //string strTime = DateTime.Now.ToString("ddMMyyyyHHmmss", CultureInfo.InvariantCulture);
                        string strTime = DateTime.Now.ToString("MMddyyyy_HH-mm-ss");
                        //string fileName = "CPP_" + trend + "_" + granularity + "_" + strTime + ".mpp";
                        string fileName = proName + "_" + trend + "_" + granularity + "_" + strTime + ".mpp";

                        MessageLogging.LogException("Before open application");
                        //logger.Info("Before open application");

                        //Application appclass = new Application();

                        MessageLogging.LogException("Before filenew");
                        //logger.Info("Before filenew");

                        appclass.FileNew();

                        MessageLogging.LogException("After filenew");
                        //logger.Info("After filenew");

                        //foreach (Process proc in Process.GetProcessesByName("WINPROJ"))
                        //{
                        //    if (!proc.Responding)
                        //    {
                        //        //Thread.Sleep(10000);
                        //        logger.Info("Before Process Kill - appclass.FileNew()");
                        //        proc.Kill();
                        //        logger.Info("After Process Kill - appclass.FileNew()");
                        //    }
                        //}
                        //Thread.Sleep(5000);

                        checkProcess(attemptNum);

                        MessageLogging.LogException(workingFolder + fileName + " Started parallel checkProcess");
                        //logger.Info(workingFolder + fileName + " Started parallel checkProcess");

                        appclass.ViewApply("Resource Sheet"); //mark of the corrupt

                        appclass.BaselineSave(true, PjSaveBaselineFrom.pjCopyBaseline, PjSaveBaselineTo.pjIntoBaseline, true, true, true);
                        Project project1 = null;
                        project1 = appclass.ActiveProject;
                        //project1.LevelEntireProject = true;  //Manasi 14-10-2020
                        //appclass.LevelingOptions(Automatic: true, DelayInSlack: false, AutoClearLeveling: true, Order: PjLevelOrder.pjLevelID, LevelEntireProject: true);
                        //project1.Application.ShowAssignmentUnitsAs = PjAssignmentUnits.pjDecimalAssignmentUnits;
                        #region Resource
                        DateTime cutOffDate = DateUtility.getCutOffDate(granularity);
                        List<ResourceModel> resource = _ExportProcess.getResouces(projectId, trendNumber, granularity, cutOffDate);

                        Resources resources = null;
                        Resource resource1 = null;
                        resources = project1.Resources;
                        //resources.Application.ShowAssignmentUnitsAs=PjAssignmentUnits.pjDecimalAssignmentUnits;
                        Tables table = null;
                        table = project1.ResourceTables;
                        project1.Application.TableEditEx(Name: "&Entry", TaskTable: false, NewFieldName: "Cost1", Width: 10, WrapText: true, ShowAddNewColumn: true);
                        project1.Application.TableApply(Name: "&Entry");
                        int i = 1;
                        Int64 weeks = 0;
                        Int64 totalHours = 0;
                        foreach (var t in resource)
                        {
                            //resouce1 = resources.Add("1 2 3 4 5 6 7 8 9 0 a b c d e f g h i j k l m n o p q r s t u v w x y z ` ~ ! @ # $ % ^ & * ( ) - _ = + ? ; : \' \" \\ | < > . ", Convert.ToInt32(t.ResourceId));

                            //Replace comma
                            //String empNameReplaced = t.EmpName.TrimEnd('\r', '\n').Replace(",", "%<" + Encoding.Default.GetBytes(",")[0].ToString() + ">");
                            String empNameReplaced = t.EmpName.TrimEnd('\r', '\n').Replace(",", "").Replace(";", "").Replace("[", "").Replace("]", "");

                            resource1 = resources.Add(empNameReplaced, Convert.ToInt32(t.ResourceId));
                            resource1.SetField(FieldID: PjField.pjResourceCost1, Value: Convert.ToString(t.HourlyRate));
                            weeks = Convert.ToInt64(t.Duration / 7);
                            totalHours = Convert.ToInt64(weeks * 40);
                            t.MaxUnits = (t.Workinghours ) / totalHours;
                            if (granularity == "week")
                            {
                                //t.MaxUnits = t.Workinghours * 100 / 40;
                                t.MaxUnits = (t.Workinghours) * 100 / totalHours;
                            }
                            else if (granularity == "month")
                            {
                                t.MaxUnits = (t.Workinghours) * 100 / (40 * 4);
                            }
                            else if (granularity == "year")
                            {
                                t.MaxUnits = (t.Workinghours) * 100 / (40 * 52);
                            }
                            resource1.SetField(FieldID: PjField.pjResourceMaxUnits, Value: Convert.ToString(Math.Round(t.MaxUnits, 2)));
                            resource1.SetField(FieldID: PjField.pjResourceType, Value: t.CostType);
                            //resource1.SetField(FieldID: PjField.pjResourceAssignmentUnits, Value: Math.Round(t.MaxUnits, 0).ToString());
                            i++;
                        }
                        #endregion Resource

                        #region TaskSheet
                        appclass.ViewApply("Tracking Gantt");
                        project1 = null;
                        project1 = appclass.ActiveProject;
                        project1.CurrencyCode = "USD";
                        project1.CurrencySymbol = "$";
                        //project1.Application.ShowAssignmentUnitsAs = PjAssignmentUnits.pjDecimalAssignmentUnits;
                        //project1.AutoSplitTasks = false;
                        //project1.Application.GanttBarFormatEx();
                        //appclass.GanttBarFormatEx(RightText: PjField.pjTaskNumber1);
                        #region Timescale
                        if (granularity == "week")
                        {
                            project1.Application.TimescaleEdit(MajorUnits: PjTimescaleUnit.pjTimescaleMonths, MinorUnits: PjTimescaleUnit.pjTimescaleWeeks,
                                                            MajorLabel: PjMonthLabel.pjMonthLabelMonth_mmmm_yyyy, MinorLabel: PjWeekLabel.pjWeekLabelWeek_mmm_dd,
                                                            TopCount: 1, MinorCount: 1, Separator: true, TierCount: 2);
                        }
                        else if (granularity == "month")
                        {
                            project1.Application.TimescaleEdit(MajorUnits: PjTimescaleUnit.pjTimescaleYears, MinorUnits: PjTimescaleUnit.pjTimescaleMonths,
                                                              MinorLabel: PjMonthLabel.pjMonthLabelMonth_mmmm_yyyy,
                                                              TopCount: 1, MinorCount: 1, Separator: true, TierCount: 2);
                        }
                        else if (granularity == "year")
                        {
                            project1.Application.TimescaleEdit(MajorUnits: PjTimescaleUnit.pjTimescaleYears, TierCount: 1);
                        }
                        #endregion Timescale

                        #region Calender
                        //project1.Application.BaseCalendarCreate(Name: "Birdi", FromName: "Standard");
                        ////project.Application.BaseCalendarReset("Birdi");

                        //project1.Application.BaseCalendarEditDays(Name: "Birdi", WeekDay: PjWeekday.pjMonday, Working: true);
                        //project1.Application.BaseCalendarEditDays(Name: "Birdi", WeekDay: PjWeekday.pjTuesday, Working: true);
                        //project1.Application.BaseCalendarEditDays(Name: "Birdi", WeekDay: PjWeekday.pjWednesday, Working: true);
                        //project1.Application.BaseCalendarEditDays(Name: "Birdi", WeekDay: PjWeekday.pjThursday, Working: true);
                        //project1.Application.BaseCalendarEditDays(Name: "Birdi", WeekDay: PjWeekday.pjFriday, Working: true);
                        //project1.Application.BaseCalendarEditDays(Name: "Birdi", WeekDay: PjWeekday.pjSaturday, Working: true);
                        //project1.Application.BaseCalendarEditDays(Name: "Birdi", WeekDay: PjWeekday.pjSunday, Working: true);

                        ////project.Application.BaseCalendars(Index: "4", Locked: true);
                        #endregion Calender

                        appclass.ShowToolTips = true;

                        #region table
                        //project1.Application.TableEditEx(Name: "&Entry", TaskTable: true, FieldName: "PercentWorkComplete", Width: 10, WrapText: true, ColumnPosition: 3);
                        project1.Application.TableEditEx(Name: "&Entry", TaskTable: true, FieldName: "PercentWorkComplete", NewFieldName: "Number1", Title: "% Complete", Width: 10, WrapText: true, ColumnPosition: 3);
                        project1.Application.TableEditEx(Name: "&Entry", TaskTable: true, FieldName: "Duration1", Width: 10, WrapText: true, ColumnPosition: 4);
                        project1.Application.TableEditEx(Name: "&Entry", TaskTable: true, FieldName: "Start", NewFieldName: "Date1", Title: "Start", Width: 10, WrapText: true, ColumnPosition: 5);
                        project1.Application.TableEditEx(Name: "&Entry", TaskTable: true, FieldName: "Finish", NewFieldName: "Date2", Title: "Finish", Width: 10, WrapText: true, ColumnPosition: 6);
                        project1.Application.TableEditEx(Name: "&Entry", TaskTable: true, FieldName: "Cost1", Width: 10, WrapText: true, ColumnPosition: 7);
                        project1.Application.TableEditEx(Name: "&Entry", TaskTable: true, NewFieldName: "Work", Width: 10, WrapText: true, ColumnPosition: 8);
                        project1.Application.TableEditEx(Name: "&Entry", TaskTable: true, NewFieldName: "Resource Names", Width: 10, WrapText: true, ShowAddNewColumn: true);
                        project1.Application.TableEditEx(Name: "&Entry", TaskTable: true, FieldName: "Predecessors", Width: 0); //COMException 0x80010105

                        project1.Application.TableApply(Name: "&Entry");

                        PjField fieldId = appclass.FieldNameToFieldConstant(FieldName: "Work", FieldType: PjFieldType.pjProject);
                        #endregion table
                        if (phaseId == 0)
                        {
                            Task task = project1.Tasks.Add(proName);
                            foreach (var a in totalCost)
                            {
                                long duration = 0;
                                task.PercentComplete = Convert.ToString(a.PercentageCompletion);
                                task.Number1 = Convert.ToDouble(a.PercentageCompletion);
                                if (a.Duration >= 7)
                                {
                                    duration = a.Duration / 7;
                                    duration = duration * 5;
                                }
                                else
                                {
                                    duration = a.Duration;
                                }
                                task.Duration1 = Convert.ToInt32(duration);
                                task.Start = Convert.ToDateTime(a.ActivityStartDate);
                                task.Date1 = Convert.ToDateTime(a.ActivityStartDate);
                                task.ActualStart = Convert.ToDateTime(a.ActivityStartDate);
                                task.Finish = Convert.ToDateTime(a.ActivityEndDate);
                                task.Date2 = Convert.ToDateTime(a.ActivityEndDate);
                                task.ActualFinish = Convert.ToDateTime(a.ActivityEndDate);
                                task.Cost1 = Convert.ToDouble(a.Price);
                                task.Work = Convert.ToString(a.WorkingHours);
                                task.SetField(PjField.pjTaskActualStart, Convert.ToDateTime(a.ActivityStartDate));
                                task.SetField(PjField.pjTaskActualFinish, Convert.ToDateTime(a.ActivityEndDate));
                                task.SetField(PjField.pjTaskWork, a.WorkingHours);
                                task.SetField(PjField.pjTaskDuration1, task.Duration1);
                                task.SetField(PjField.pjTaskPercentWorkComplete, a.PercentageCompletion);
                            }
                            task.OutlineLevel = 1;

                            task.Milestone = false;    //Manasi 14-10-2020
                        }
                        project1.Application.SetTaskMode(Manual: false);
                        //project1.Application.SetTaskMode(Manual: true);

                        #region BaseLineCurrent
                        if (trendNumber.ToString() != "2000" && trendNumber.ToString() != "3000")
                        {
                            int j = 1;
                            foreach (var t in lstExportData)
                            {
                                Task task1;
                                long duration = 0;
                                if (t.BudgetCategory != null && t.BudgetCategory != "")
                                {
                                    task1 = project1.Tasks.Add(t.BudgetCategory + " - " + t.BudgetSubCategory);
                                    task1.Manual = false;
                                    //task1.Manual = true;

                                    //task1.ActualStart = Convert.ToDateTime(t.ActivityStartDate);
                                    //task1.ActualFinish = Convert.ToDateTime(t.ActivityStartDate);
                                    task1.PercentWorkComplete = Convert.ToString(t.PercentageCompletion);
                                    task1.Number1 = Convert.ToDouble(t.PercentageCompletion);
                                    //task1.Duration = Convert.ToInt32(t.Duration);
                                    if (t.Duration >= 7)
                                    {
                                        duration = t.Duration / 7;
                                        duration = duration * 5;
                                    }
                                    else
                                    {
                                        duration = t.Duration;
                                    }
                                    task1.Duration1 = Convert.ToInt32(duration);
                                    //task1.ActualDuration = Convert.ToInt32(t.Duration);
                                    if (t.ActivityStartDate != null && t.ActivityStartDate != "")
                                    {
                                        task1.Start = Convert.ToDateTime(t.ActivityStartDate);
                                        task1.Date1 = Convert.ToDateTime(t.ActivityStartDate);
                                        task1.ActualStart = Convert.ToDateTime(t.ActivityStartDate);
                                    }
                                    if (t.ActivityEndDate != null && t.ActivityEndDate != "")
                                    {
                                        task1.Finish = Convert.ToDateTime(t.ActivityEndDate);
                                        task1.Date2 = Convert.ToDateTime(t.ActivityEndDate);
                                        task1.ActualFinish = Convert.ToDateTime(t.ActivityEndDate);
                                    }

                                    task1.Cost1 = Convert.ToDouble(t.Price);
                                    //task1.Number1 = Convert.ToDouble(t.WorkingHours);
                                    task1.Work = Convert.ToString(t.WorkingHours);
                                    //task1.ActualWork = Convert.ToString(t.WorkingHours);
                                    //if (t.EmployeeID != null && Convert.ToInt64(t.EmployeeID) != 0)
                                    //{
                                    //    var ResId = (from p in resource
                                    //                 where p.EmployeeId == Convert.ToInt64(t.EmployeeID)
                                    //                 select p.ResourceId).Single();
                                    //    //resource.Where(p => p.EmployeeId == Convert.ToInt32(t.EmployeeID)).Select(p => p.ResourceId);
                                    //    task1.Assignments.Add(TaskID: task1.ID, ResourceID: Convert.ToInt32(ResId)); //, ResourceID: 1, Units: 1);

                                    //}

                                    if (t.EmployeeID != null)
                                    {
                                        //List<String> empId = new List<String>();

                                        string[] empId = t.EmployeeID.ToString().Split(',');

                                        for (int e = 0; e < empId.Length; e++)
                                        {
                                            if (Convert.ToInt64(empId[e]) != 0)
                                            {
                                                var ResId = (from p in resource
                                                             where p.EmployeeId == Convert.ToInt64(empId[e])
                                                             select p.ResourceId).Single();

                                                var maxUnits = (from p in resource
                                                                where p.EmployeeId == Convert.ToInt64(empId[e])
                                                                select p.MaxUnits).Single();
                                                //maxUnits = maxUnits / 100;
                                                //resource.Where(p => p.EmployeeId == Convert.ToInt32(t.EmployeeID)).Select(p => p.ResourceId);
                                                task1.Assignments.Add(TaskID: task1.ID, ResourceID: Convert.ToInt32(ResId)); //, ResourceID: 1, Units: 1)Convert.ToDecimal(maxUnits);
                                            }
                                        }
                                    }

                                    if (phaseId == 0)
                                    {
                                        task1.OutlineLevel = 3;
                                    }
                                    else
                                    {
                                        task1.OutlineLevel = 2;
                                    }

                                    if (t.ActivityStartDate != null && t.ActivityStartDate != "")
                                    {
                                        task1.SetField(PjField.pjTaskStart, Convert.ToDateTime(t.ActivityStartDate).ToShortDateString());
                                        task1.SetField(PjField.pjTaskActualStart, Convert.ToDateTime(t.ActivityStartDate).ToShortDateString());
                                    }
                                    if (t.ActivityEndDate != null && t.ActivityEndDate != "")
                                    {
                                        task1.SetField(PjField.pjTaskFinish, Convert.ToDateTime(t.ActivityEndDate).ToShortDateString());
                                        task1.SetField(PjField.pjTaskActualFinish, Convert.ToDateTime(t.ActivityEndDate).ToShortDateString());
                                    }
                                    //task1.SetField(PjField.pjTaskActualWork, t.WorkingHours);
                                    if (t.WorkingHours != 0)
                                        task1.SetField(PjField.pjTaskWork, t.WorkingHours.ToString());

                                    //project1.ProjectSummaryTask.SetField(FieldID: fieldId, Value: Convert.ToString(t.WorkingHours));
                                    task1.SetField(PjField.pjTaskDuration1, task1.Duration1);
                                    if (Convert.ToDouble(t.PercentageCompletion) != 0)
                                        task1.SetField(PjField.pjTaskPercentWorkComplete, t.PercentageCompletion);
                                    j++;
                                }

                                else
                                {
                                    task1 = project1.Tasks.Add(t.PhaseDescription.TrimEnd('\r', '\n'));
                                    task1.Manual = false;
                                    //task1.Manual = true;
                                    //task1.Application.Font32Ex(Bold: true);
                                    //task1.PercentComplete = 50;
                                    task1.PercentWorkComplete = Convert.ToString(t.PercentageCompletion);
                                    task1.Number1 = Convert.ToDouble(t.PercentageCompletion);
                                    //task1.Duration = Convert.ToInt32(t.Duration);
                                    if (t.Duration >= 7)
                                    {
                                        duration = t.Duration / 7;
                                        duration = duration * 5;
                                    }
                                    else
                                    {
                                        duration = t.Duration;
                                    }
                                    task1.Duration1 = Convert.ToInt32(duration);
                                    //task1.Duration1 = Convert.ToInt32(t.Duration);
                                    //task1.ActualDuration = Convert.ToInt32(t.Duration);
                                    if (t.ActivityStartDate != null && t.ActivityStartDate != "")
                                    {
                                        task1.Start = Convert.ToDateTime(t.ActivityStartDate);
                                        task1.Date1 = Convert.ToDateTime(t.ActivityStartDate);
                                        task1.ActualStart = Convert.ToDateTime(t.ActivityEndDate);
                                    }
                                    if (t.ActivityEndDate != null && t.ActivityEndDate != "")
                                    {
                                        task1.Finish = Convert.ToDateTime(t.ActivityEndDate);
                                        task1.Date2 = Convert.ToDateTime(t.ActivityEndDate);
                                        task1.ActualFinish = Convert.ToDateTime(t.ActivityEndDate);
                                    }
                                    task1.Cost1 = Convert.ToDouble(t.Price);
                                    //task1.Number1 = Convert.ToDouble(t.WorkingHours);
                                    task1.Work = Convert.ToString(t.WorkingHours);
                                    //task1.Number1 = Convert.ToDouble(t.WorkingHours);
                                    //task1.ActualWork = Convert.ToString(t.WorkingHours);
                                    //task1.BaselineBudgetCost = Convert.ToDouble(t.Price);

                                    if (phaseId == 0)
                                    {
                                        task1.OutlineLevel = 2;
                                    }
                                    else
                                    {
                                        task1.OutlineLevel = 1;
                                    }
                                    if (t.ActivityStartDate != null && t.ActivityStartDate != "")
                                    {
                                        task1.SetField(PjField.pjTaskStart, Convert.ToDateTime(t.ActivityStartDate).ToShortDateString());
                                        task1.SetField(PjField.pjTaskActualStart, Convert.ToDateTime(t.ActivityStartDate).ToShortDateString());
                                    }
                                    if (t.ActivityEndDate != null && t.ActivityEndDate != "")
                                    {
                                        task1.SetField(PjField.pjTaskFinish, Convert.ToDateTime(t.ActivityEndDate).ToShortDateString());
                                        task1.SetField(PjField.pjTaskActualFinish, Convert.ToDateTime(t.ActivityEndDate).ToShortDateString());
                                    }
                                    //task1.SetField(PjField.pjTaskActualWork, t.WorkingHours);
                                    task1.SetField(PjField.pjTaskWork, t.WorkingHours.ToString());
                                    //PjField fieldId = appclass.FieldNameToFieldConstant(FieldName: "Number1", FieldType: PjFieldType.pjProject);
                                    //project1.ProjectSummaryTask.SetField(FieldID: fieldId, Value: Convert.ToString(t.WorkingHours));
                                    task1.SetField(PjField.pjTaskDuration1, task1.Duration1);
                                    //task1.SetField(PjField.pjTaskActualWork, t.WorkingHours);
                                    task1.SetField(PjField.pjTaskPercentWorkComplete, task1.PercentComplete);
                                }



                                task1.Milestone = false;
                            }
                        }
                        #endregion BaseLineCurrent
                        else if (trendNumber.ToString() == "2000")
                        {
                            List<ExportForecastData> MainPhasedata = _ExportProcess.ExportForecastMainPhase(projectId, granularity);
                            List<ExportForecastData> SubPhaseData = _ExportProcess.ExportForecastSubPhase(projectId, granularity);

                            ExportData ex = new ExportData();
                            ex.lstExportForecastData = new List<ExportForecastData>();
                            ExportForecastData objExportForecastData;
                            foreach (var MPdata in MainPhasedata)
                            {
                                objExportForecastData = new ExportForecastData();
                                objExportForecastData.ActivityID = MPdata.ActivityID;
                                objExportForecastData.ProjectID = MPdata.ProjectID;
                                objExportForecastData.TrendNumber = MPdata.TrendNumber;
                                objExportForecastData.PhaseCode = MPdata.PhaseCode;
                                objExportForecastData.PhaseDescription = MPdata.PhaseDescription;
                                objExportForecastData.BudgetCategory = MPdata.BudgetCategory;
                                objExportForecastData.BudgetSubCategory = MPdata.BudgetSubCategory;
                                objExportForecastData.ActivityStartDate = MPdata.ActivityStartDate;
                                objExportForecastData.ActivityEndDate = MPdata.ActivityEndDate;
                                objExportForecastData.Price = MPdata.Price;
                                objExportForecastData.OriginalActivityStartDate = MPdata.OriginalActivityStartDate;
                                objExportForecastData.OriginalActivityEndDate = MPdata.OriginalActivityEndDate;
                                objExportForecastData.PercentageCompletion = MPdata.PercentageCompletion;
                                objExportForecastData.order = MPdata.order;
                                objExportForecastData.Workinghours = MPdata.Workinghours;
                                objExportForecastData.Granularity = MPdata.Granularity;
                                objExportForecastData.Duration = MPdata.Duration;

                                ex.lstExportForecastData.Add(objExportForecastData);

                                var Sdata = SubPhaseData.Where(x => x.PhaseCode == MPdata.PhaseCode).ToList();
                                foreach (var sp in Sdata)
                                {
                                    objExportForecastData = new ExportForecastData();
                                    objExportForecastData.ActivityID = sp.ActivityID;
                                    objExportForecastData.ProjectID = sp.ProjectID;
                                    objExportForecastData.TrendNumber = sp.TrendNumber;
                                    objExportForecastData.PhaseCode = sp.PhaseCode;
                                    objExportForecastData.PhaseDescription = sp.PhaseDescription;
                                    objExportForecastData.BudgetCategory = sp.BudgetCategory;
                                    objExportForecastData.BudgetSubCategory = sp.BudgetSubCategory;
                                    objExportForecastData.ActivityStartDate = sp.ActivityStartDate;
                                    objExportForecastData.ActivityEndDate = sp.ActivityEndDate;
                                    objExportForecastData.Price = sp.Price;
                                    objExportForecastData.OriginalActivityStartDate = sp.OriginalActivityStartDate;
                                    objExportForecastData.OriginalActivityEndDate = sp.OriginalActivityEndDate;
                                    objExportForecastData.PercentageCompletion = sp.PercentageCompletion;
                                    objExportForecastData.order = sp.order;
                                    objExportForecastData.Workinghours = sp.Workinghours;
                                    objExportForecastData.Granularity = sp.Granularity;
                                    objExportForecastData.Duration = sp.Duration;
                                    objExportForecastData.EmployeeID = sp.EmployeeID;
                                    ex.lstExportForecastData.Add(objExportForecastData);
                                }
                            }

                            foreach (var t in ex.lstExportForecastData)
                            {
                                Task task1;
                                long duration = 0;
                                if (t.BudgetCategory != null && t.BudgetCategory != "")
                                {
                                    //task1 = project1.Tasks.Add(t.BudgetCategory.TrimEnd('\r', '\n') + " - " + t.BudgetSubCategory.TrimEnd('\r', '\n'));
                                    task1 = project1.Tasks.Add(t.BudgetCategory.TrimEnd('\r', '\n'));
                                    task1.Manual = false;
                                    //task1.Manual = true;
                                    //task1.ActualStart = Convert.ToDateTime(t.ActivityStartDate);
                                    //task1.ActualFinish = Convert.ToDateTime(t.ActivityStartDate);
                                    task1.PercentWorkComplete = Convert.ToString(t.PercentageCompletion);
                                    task1.Number1 = Convert.ToDouble(t.PercentageCompletion);
                                    //task1.Duration = Convert.ToInt32(t.Duration);
                                    if (t.Duration >= 7)
                                    {
                                        duration = t.Duration / 7;
                                        duration = duration * 5;
                                    }
                                    else
                                    {
                                        duration = t.Duration;
                                    }
                                    task1.Duration1 = Convert.ToInt32(duration);
                                    //task1.Duration1 = Convert.ToInt32(t.Duration);
                                    //task1.ActualDuration = Convert.ToInt32(t.Duration);
                                    if (t.ActivityStartDate != null && t.ActivityStartDate != "")
                                    {
                                        task1.Start = Convert.ToDateTime(t.ActivityStartDate);
                                        task1.Date1 = Convert.ToDateTime(t.ActivityStartDate);
                                        task1.ActualStart = Convert.ToDateTime(t.ActivityStartDate);
                                    }
                                    if (t.ActivityEndDate != null && t.ActivityEndDate != "")
                                    {
                                        task1.Finish = Convert.ToDateTime(t.ActivityEndDate);
                                        task1.Date2 = Convert.ToDateTime(t.ActivityEndDate);
                                        task1.ActualFinish = Convert.ToDateTime(t.ActivityEndDate);
                                    }

                                    task1.Cost1 = Convert.ToDouble(t.Price);
                                    //task1.Number1 = Convert.ToDouble(t.WorkingHours);
                                    task1.Work = Convert.ToString(t.Workinghours);
                                    //task1.ActualWork = Convert.ToString(t.WorkingHours);

                                    //if (t.EmployeeID != null && Convert.ToInt64(t.EmployeeID) != 0)
                                    //{
                                    //    var ResId = (from p in resource
                                    //                 where p.EmployeeId == Convert.ToInt64(t.EmployeeID)
                                    //                 select p.ResourceId).Single();
                                    //    //resource.Where(p => p.EmployeeId == Convert.ToInt32(t.EmployeeID)).Select(p => p.ResourceId);
                                    //    task1.Assignments.Add(TaskID: task1.ID, ResourceID: Convert.ToInt32(ResId)); //, ResourceID: 1, Units: 1);

                                    //}

                                    if (t.EmployeeID != null)
                                    {
                                        //List<String> empId = new List<String>();

                                        string[] empId = t.EmployeeID.ToString().Split(',');

                                        for (int e = 0; e < empId.Length; e++)
                                        {
                                            if (Convert.ToInt64(empId[e]) != 0)
                                            {
                                                var ResId = (from p in resource
                                                             where p.EmployeeId == Convert.ToInt64(empId[e])
                                                             select p.ResourceId).Single();
                                                //resource.Where(p => p.EmployeeId == Convert.ToInt32(t.EmployeeID)).Select(p => p.ResourceId);
                                                var maxUnits = (from p in resource
                                                                where p.EmployeeId == Convert.ToInt64(empId[e])
                                                                select p.MaxUnits).Single();
                                                //maxUnits = maxUnits / 100;
                                                task1.Assignments.Add(TaskID: task1.ID, ResourceID: Convert.ToInt32(ResId)); //, ResourceID: 1, Units: 1 Convert.ToDecimal(maxUnits));
                                            }
                                        }
                                    }

                                    if (phaseId == 0)
                                    {
                                        task1.OutlineLevel = 3;
                                    }
                                    else
                                    {
                                        task1.OutlineLevel = 2;
                                    }


                                    if (t.ActivityStartDate != null && t.ActivityStartDate != "")
                                    {

                                        task1.SetField(PjField.pjTaskActualStart, task1.ActualStart);
                                    }
                                    if (t.ActivityEndDate != null && t.ActivityEndDate != "")
                                    {

                                        task1.SetField(PjField.pjTaskActualFinish, task1.ActualFinish);
                                    }
                                    //task1.SetField(PjField.pjTaskActualWork, t.WorkingHours);
                                    task1.SetField(PjField.pjTaskWork, t.Workinghours.ToString());

                                    //project1.ProjectSummaryTask.SetField(FieldID: fieldId, Value: Convert.ToString(t.WorkingHours));
                                    task1.SetField(PjField.pjTaskDuration1, task1.Duration1);
                                    task1.SetField(PjField.pjTaskPercentWorkComplete, task1.PercentComplete);
                                    //j++;
                                }

                                else
                                {
                                    task1 = project1.Tasks.Add(t.PhaseDescription.TrimEnd('\r', '\n'));
                                    task1.Manual = false;
                                    //task1.Manual = true;
                                    //task1.PercentComplete = 50;
                                    task1.PercentWorkComplete = Convert.ToString(t.PercentageCompletion);
                                    task1.Number1 = Convert.ToDouble(t.PercentageCompletion);
                                    //task1.Duration = Convert.ToInt32(t.Duration);
                                    if (t.Duration >= 7)
                                    {
                                        duration = t.Duration / 7;
                                        duration = duration * 5;
                                    }
                                    else
                                    {
                                        duration = t.Duration;
                                    }
                                    task1.Duration1 = Convert.ToInt32(duration);
                                    //task1.Duration1 = Convert.ToInt32(t.Duration);
                                    //task1.ActualDuration = Convert.ToInt32(t.Duration);
                                    if (t.ActivityStartDate != null && t.ActivityStartDate != "")
                                    {
                                        task1.Start = Convert.ToDateTime(t.ActivityStartDate);
                                        task1.Date1 = Convert.ToDateTime(t.ActivityStartDate);
                                        task1.ActualStart = Convert.ToDateTime(t.ActivityEndDate);
                                    }
                                    if (t.ActivityEndDate != null && t.ActivityEndDate != "")
                                    {
                                        task1.Finish = Convert.ToDateTime(t.ActivityEndDate);
                                        task1.Date2 = Convert.ToDateTime(t.ActivityEndDate);
                                        task1.ActualFinish = Convert.ToDateTime(t.ActivityEndDate);
                                    }
                                    task1.Cost1 = Convert.ToDouble(t.Price);
                                    //task1.Number1 = Convert.ToDouble(t.WorkingHours);
                                    task1.Work = Convert.ToString(t.Workinghours);
                                    //task1.Number1 = Convert.ToDouble(t.WorkingHours);
                                    //task1.ActualWork = Convert.ToString(t.WorkingHours);
                                    //task1.BaselineBudgetCost = Convert.ToDouble(t.Price);

                                    if (phaseId == 0)
                                    {
                                        task1.OutlineLevel = 2;
                                    }
                                    else
                                    {
                                        task1.OutlineLevel = 1;
                                    }

                                    //task1.OutlineLevel = 2;

                                    if (t.ActivityStartDate != null && t.ActivityStartDate != "")
                                    {
                                        //task1.SetField(PjField.pjTaskStartText, Convert.ToDateTime(t.ActivityStartDate));
                                        task1.SetField(PjField.pjTaskActualStart, task1.ActualStart);
                                    }
                                    if (t.ActivityEndDate != null && t.ActivityEndDate != "")
                                    {
                                        //task1.SetField(PjField.pjTaskFinishText, Convert.ToDateTime(t.ActivityEndDate));
                                        task1.SetField(PjField.pjTaskActualFinish, task1.ActualFinish);
                                    }
                                    //task1.SetField(PjField.pjTaskActualWork, t.WorkingHours);
                                    task1.SetField(PjField.pjTaskWork, t.Workinghours.ToString());
                                    //task1.SetField(PjField.pjTaskActualWork, task1.ActualWork);
                                    //PjField fieldId = appclass.FieldNameToFieldConstant(FieldName: "Number1", FieldType: PjFieldType.pjProject);
                                    //project1.ProjectSummaryTask.SetField(FieldID: fieldId, Value: Convert.ToString(t.WorkingHours));
                                    task1.SetField(PjField.pjTaskDuration1, task1.Duration1);
                                    //task1.SetField(PjField.pjTaskActualWork, t.WorkingHours);
                                    task1.SetField(PjField.pjTaskPercentWorkComplete, task1.PercentComplete);
                                }



                                task1.Milestone = false;
                            }

                        }

                        else if (trendNumber.ToString() == "3000")
                        {
                            List<ExportCurrentData> MainPhasedata = _ExportProcess.ExportCurrentMainPhase(projectId, granularity);
                            List<ExportCurrentData> SubPhaseData = _ExportProcess.ExportCurrentSubPhase(projectId, granularity);

                            ExportData ex = new ExportData();
                            ex.lstExportCurrentData = new List<ExportCurrentData>();
                            ExportCurrentData objExportCurrentData;
                            foreach (var MPdata in MainPhasedata)
                            {
                                objExportCurrentData = new ExportCurrentData();
                                objExportCurrentData.ActivityID = MPdata.ActivityID;
                                objExportCurrentData.ProjectID = MPdata.ProjectID;
                                objExportCurrentData.TrendNumber = MPdata.TrendNumber;
                                objExportCurrentData.PhaseCode = MPdata.PhaseCode;
                                objExportCurrentData.PhaseDescription = MPdata.PhaseDescription;
                                objExportCurrentData.BudgetCategory = MPdata.BudgetCategory;
                                objExportCurrentData.BudgetSubCategory = MPdata.BudgetSubCategory;
                                objExportCurrentData.ActivityStartDate = MPdata.ActivityStartDate;
                                objExportCurrentData.ActivityEndDate = MPdata.ActivityEndDate;
                                objExportCurrentData.Price = MPdata.Price;
                                objExportCurrentData.OriginalActivityStartDate = MPdata.OriginalActivityStartDate;
                                objExportCurrentData.OriginalActivityEndDate = MPdata.OriginalActivityEndDate;
                                objExportCurrentData.PercentageCompletion = MPdata.PercentageCompletion;
                                objExportCurrentData.order = MPdata.order;
                                objExportCurrentData.Workinghours = MPdata.Workinghours;
                                objExportCurrentData.Granularity = MPdata.Granularity;
                                objExportCurrentData.Duration = MPdata.Duration;

                                ex.lstExportCurrentData.Add(objExportCurrentData);

                                var Sdata = SubPhaseData.Where(x => x.PhaseCode == MPdata.PhaseCode).ToList();
                                foreach (var sp in Sdata)
                                {
                                    objExportCurrentData = new ExportCurrentData();
                                    objExportCurrentData.ActivityID = sp.ActivityID;
                                    objExportCurrentData.ProjectID = sp.ProjectID;
                                    objExportCurrentData.TrendNumber = sp.TrendNumber;
                                    objExportCurrentData.PhaseCode = sp.PhaseCode;
                                    objExportCurrentData.PhaseDescription = sp.PhaseDescription;
                                    objExportCurrentData.BudgetCategory = sp.BudgetCategory;
                                    objExportCurrentData.BudgetSubCategory = sp.BudgetSubCategory;
                                    objExportCurrentData.ActivityStartDate = sp.ActivityStartDate;
                                    objExportCurrentData.ActivityEndDate = sp.ActivityEndDate;
                                    objExportCurrentData.Price = sp.Price;
                                    objExportCurrentData.OriginalActivityStartDate = sp.OriginalActivityStartDate;
                                    objExportCurrentData.OriginalActivityEndDate = sp.OriginalActivityEndDate;
                                    objExportCurrentData.PercentageCompletion = sp.PercentageCompletion;
                                    objExportCurrentData.order = sp.order;
                                    objExportCurrentData.Workinghours = sp.Workinghours;
                                    objExportCurrentData.Granularity = sp.Granularity;
                                    objExportCurrentData.Duration = sp.Duration;
                                    objExportCurrentData.EmployeeID = sp.EmployeeID;
                                    ex.lstExportCurrentData.Add(objExportCurrentData);
                                }
                            }

                            foreach (var t in ex.lstExportCurrentData)
                            {
                                Task task1;
                                long duration = 0;
                                if (t.BudgetCategory != null && t.BudgetCategory != "")
                                {
                                    //task1 = project1.Tasks.Add(t.BudgetCategory.TrimEnd('\r', '\n') + " - " + t.BudgetSubCategory.TrimEnd('\r', '\n'));
                                    task1 = project1.Tasks.Add(t.BudgetCategory.TrimEnd('\r', '\n'));
                                    task1.Manual = false;
                                    //task1.Manual = true;
                                    //task1.ActualStart = Convert.ToDateTime(t.ActivityStartDate);
                                    //task1.ActualFinish = Convert.ToDateTime(t.ActivityStartDate);
                                    task1.PercentWorkComplete = Convert.ToString(t.PercentageCompletion);
                                    task1.Number1 = Convert.ToDouble(t.PercentageCompletion);
                                    //task1.Duration = Convert.ToInt32(t.Duration);
                                    if (t.Duration >= 7)
                                    {
                                        duration = t.Duration / 7;
                                        duration = duration * 5;
                                    }
                                    else
                                    {
                                        duration = t.Duration;
                                    }
                                    task1.Duration1 = Convert.ToInt32(duration);
                                    //task1.Duration1 = Convert.ToInt32(t.Duration);
                                    //task1.ActualDuration = Convert.ToInt32(t.Duration);
                                    if (t.ActivityStartDate != null && t.ActivityStartDate != "")
                                    {
                                        task1.Start = Convert.ToDateTime(t.ActivityStartDate);
                                        task1.Date1 = Convert.ToDateTime(t.ActivityStartDate);
                                        task1.ActualStart = Convert.ToDateTime(t.ActivityStartDate);
                                    }
                                    if (t.ActivityEndDate != null && t.ActivityEndDate != "")
                                    {
                                        task1.Finish = Convert.ToDateTime(t.ActivityEndDate);
                                        task1.Date2 = Convert.ToDateTime(t.ActivityEndDate);
                                        task1.ActualFinish = Convert.ToDateTime(t.ActivityEndDate);
                                    }

                                    task1.Cost1 = Convert.ToDouble(t.Price);
                                    //task1.Number1 = Convert.ToDouble(t.WorkingHours);
                                    task1.Work = Convert.ToString(t.Workinghours);
                                    //task1.ActualWork = Convert.ToString(t.WorkingHours);

                                    //if (t.EmployeeID != null && Convert.ToInt64(t.EmployeeID) != 0)
                                    //{
                                    //    var ResId = (from p in resource
                                    //                 where p.EmployeeId == Convert.ToInt64(t.EmployeeID)
                                    //                 select p.ResourceId).Single();
                                    //    //resource.Where(p => p.EmployeeId == Convert.ToInt32(t.EmployeeID)).Select(p => p.ResourceId);
                                    //    task1.Assignments.Add(TaskID: task1.ID, ResourceID: Convert.ToInt32(ResId)); //, ResourceID: 1, Units: 1);

                                    //}

                                    if (t.EmployeeID != null)
                                    {
                                        //List<String> empId = new List<String>();

                                        string[] empId = t.EmployeeID.ToString().Split(',');

                                        for (int e = 0; e < empId.Length; e++)
                                        {
                                            if (Convert.ToInt64(empId[e]) != 0)
                                            {
                                                var ResId = (from p in resource
                                                             where p.EmployeeId == Convert.ToInt64(empId[e])
                                                             select p.ResourceId).Single();
                                                //resource.Where(p => p.EmployeeId == Convert.ToInt32(t.EmployeeID)).Select(p => p.ResourceId);
                                                var maxUnits = (from p in resource
                                                                where p.EmployeeId == Convert.ToInt64(empId[e])
                                                                select p.MaxUnits).Single();
                                                //axUnits = maxUnits / 100;
                                                task1.Assignments.Add(TaskID: task1.ID, ResourceID: Convert.ToInt32(ResId)); //, ResourceID: 1, Units: 1)Convert.ToDecimal(maxUnits);
                                            }
                                        }
                                    }

                                    if (phaseId == 0)
                                    {
                                        task1.OutlineLevel = 3;
                                    }
                                    else
                                    {
                                        task1.OutlineLevel = 2;
                                    }


                                    if (t.ActivityStartDate != null && t.ActivityStartDate != "")
                                    {

                                        task1.SetField(PjField.pjTaskActualStart, task1.ActualStart);
                                    }
                                    if (t.ActivityEndDate != null && t.ActivityEndDate != "")
                                    {

                                        task1.SetField(PjField.pjTaskActualFinish, task1.ActualFinish);
                                    }
                                    //task1.SetField(PjField.pjTaskActualWork, t.WorkingHours);
                                    task1.SetField(PjField.pjTaskWork, t.Workinghours.ToString());

                                    //project1.ProjectSummaryTask.SetField(FieldID: fieldId, Value: Convert.ToString(t.WorkingHours));
                                    task1.SetField(PjField.pjTaskDuration1, task1.Duration1);
                                    task1.SetField(PjField.pjTaskPercentWorkComplete, task1.PercentComplete);
                                    //j++;
                                }

                                else
                                {
                                    task1 = project1.Tasks.Add(t.PhaseDescription.TrimEnd('\r', '\n'));
                                    task1.Manual = false;
                                    //task1.Manual = true;
                                    //task1.PercentComplete = 50;
                                    task1.PercentWorkComplete = Convert.ToString(t.PercentageCompletion);
                                    task1.Number1 = Convert.ToDouble(t.PercentageCompletion);
                                    //task1.Duration = Convert.ToInt32(t.Duration);
                                    if (t.Duration >= 7)
                                    {
                                        duration = t.Duration / 7;
                                        duration = duration * 5;
                                    }
                                    else
                                    {
                                        duration = t.Duration;
                                    }
                                    task1.Duration1 = Convert.ToInt32(duration);
                                    //task1.Duration1 = Convert.ToInt32(t.Duration);
                                    //task1.ActualDuration = Convert.ToInt32(t.Duration);
                                    if (t.ActivityStartDate != null && t.ActivityStartDate != "")
                                    {
                                        task1.Start = Convert.ToDateTime(t.ActivityStartDate);
                                        task1.Date1 = Convert.ToDateTime(t.ActivityStartDate);
                                        task1.ActualStart = Convert.ToDateTime(t.ActivityEndDate);
                                    }
                                    if (t.ActivityEndDate != null && t.ActivityEndDate != "")
                                    {
                                        task1.Finish = Convert.ToDateTime(t.ActivityEndDate);
                                        task1.Date2 = Convert.ToDateTime(t.ActivityEndDate);
                                        task1.ActualFinish = Convert.ToDateTime(t.ActivityEndDate);
                                    }
                                    task1.Cost1 = Convert.ToDouble(t.Price);
                                    //task1.Number1 = Convert.ToDouble(t.WorkingHours);
                                    task1.Work = Convert.ToString(t.Workinghours);
                                    //task1.Number1 = Convert.ToDouble(t.WorkingHours);
                                    //task1.ActualWork = Convert.ToString(t.WorkingHours);
                                    //task1.BaselineBudgetCost = Convert.ToDouble(t.Price);

                                    if (phaseId == 0)
                                    {
                                        task1.OutlineLevel = 2;
                                    }
                                    else
                                    {
                                        task1.OutlineLevel = 1;
                                    }

                                    //task1.OutlineLevel = 2;

                                    if (t.ActivityStartDate != null && t.ActivityStartDate != "")
                                    {
                                        //task1.SetField(PjField.pjTaskStartText, Convert.ToDateTime(t.ActivityStartDate));
                                        task1.SetField(PjField.pjTaskActualStart, task1.ActualStart);
                                    }
                                    if (t.ActivityEndDate != null && t.ActivityEndDate != "")
                                    {
                                        //task1.SetField(PjField.pjTaskFinishText, Convert.ToDateTime(t.ActivityEndDate));
                                        task1.SetField(PjField.pjTaskActualFinish, task1.ActualFinish);
                                    }
                                    //task1.SetField(PjField.pjTaskActualWork, t.WorkingHours);
                                    task1.SetField(PjField.pjTaskWork, t.Workinghours.ToString());
                                    //task1.SetField(PjField.pjTaskActualWork, task1.ActualWork);
                                    //PjField fieldId = appclass.FieldNameToFieldConstant(FieldName: "Number1", FieldType: PjFieldType.pjProject);
                                    //project1.ProjectSummaryTask.SetField(FieldID: fieldId, Value: Convert.ToString(t.WorkingHours));
                                    task1.SetField(PjField.pjTaskDuration1, task1.Duration1);
                                    //task1.SetField(PjField.pjTaskActualWork, t.WorkingHours);
                                    task1.SetField(PjField.pjTaskPercentWorkComplete, task1.PercentComplete);
                                }



                                task1.Milestone = false;
                            }

                        }

                        #endregion TaskSheet

                        string currentFilePath = filePath + fileName;

                        //logger.Info(workingFolder + fileName + " before saving");

                        appclass.FileSaveAs(workingFolder + fileName, PjFileFormat.pjMPP, Type.Missing, false, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, true, true, true, true, Type.Missing, true);

                        MessageLogging.LogException(workingFolder + fileName + " Saved successfully");
                        //logger.Info(workingFolder + fileName + " Saved successfully");

                        //System.Threading.Tasks.Parallel.Invoke(() => checkProcess());

                        //logger.Info(workingFolder + fileName + " Started parallel checkProcess");

                        ////if (isCorrupt)
                        ////{
                        //    //Execute this after 3 seconds
                        //    foreach (Process proc in Process.GetProcessesByName("WINPROJ"))
                        //    {
                        //        //if (!proc.Responding)
                        //        //{
                        //            //Thread.Sleep(10000);
                        //            logger.Info("Before Process Kill - filesaveas1");
                        //            proc.Kill();
                        //            logger.Info("After Process Kill - filesaveas1");
                        //        //}
                        //    }
                        ////}

                        //Thread.Sleep(1000);
                        //appclass.FileExit(PjSaveType.pjDoNotSave);  //mark of the corrupt
                        appclass.FileCloseEx(PjSaveType.pjDoNotSave);

                        //logger.Info(workingFolder + fileName + " before ReleaseComObject");

                        //Marshal.ReleaseComObject(appclass);

                        //logger.Info(workingFolder + fileName + " after ReleaseComObject");

                        foreach (Process proc in Process.GetProcessesByName("WINPROJ"))
                        {
                            logger.Info("Before Process Kill");
                            proc.Kill();
                            logger.Info("After Process Kill");
                        }

                        logger.Info(workingFolder + fileName + " before GetBinaryFile");

                        byte[] luantest = GetBinaryFile(workingFolder + fileName);

                        logger.Info(workingFolder + fileName + " after GetBinaryFile");

                        var luanresult = Convert.ToBase64String(luantest);

                        logger.Info(workingFolder + fileName + " before DeleteFile");

                        DeleteFile(fileName);

                        logger.Info(workingFolder + fileName + " after DeleteFile");


                        var jsonNew = new
                        {
                            //result = Path.Combine(filePath, fileName)
                            result = luanresult,
                            fileName = fileName
                        };

                        //Cleaup
                        //appclass.FileExit(PjSaveType.pjDoNotSave);


                        return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
                    }
                }
                catch (System.Exception ex)
                {
                    //logger.Info("ATTEMPT: " + attemptNum);
                    //logger.Info(ex.ToString());
                    MessageLogging.LogException(ex.ToString());
                    if (ex.ToString().Contains("0x80010105") || ex.ToString().Contains("0x80029C83"))
                    {
                        foreach (Process proc in Process.GetProcessesByName("WINPROJ"))
                        {
                            //logger.Info("Before Process Kill - EX 1");
                            //proc.Kill();
                            //logger.Info("After Process Kill - EX 1");
                            //Thread.Sleep(3000);

                            attemptNum -= 2;
                            if (attemptNum < -10) attemptNum = 3;
                        }
                    }

                    if (ex.ToString().Contains("0x80029C84"))
                    {
                        foreach (Process proc in Process.GetProcessesByName("WINPROJ"))
                        {
                            //logger.Info("Before Process Kill - EX 2");
                            MessageLogging.LogException("Before Process Kill - EX 2");
                            proc.Kill();
                            //logger.Info("After Process Kill - EX 2");
                            MessageLogging.LogException("After Process Kill - EX 2");
                            //Thread.Sleep(3000);
                        }
                    }
                    //Thread.Sleep(3000);

                    //appclass.FileExit(PjSaveType.pjDoNotSave);
                    //Marshal.ReleaseComObject(appclass);
                    //return Request.CreateResponse(HttpStatusCode.BadRequest, "Error - go check log file");
                }

                //Thread.Sleep(Convert.ToInt32(ConfigurationManager.AppSettings["MPPReattemptTimeOut"]));

            }
            return Request.CreateResponse(HttpStatusCode.BadRequest, "Error - go check log file");
        }



        private byte[] GetBinaryFile(String filename)
        {
            byte[] bytes;
            using (FileStream file = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                bytes = new byte[file.Length];
                file.Read(bytes, 0, (int)file.Length);
            }
            return bytes;
        }

        [HttpGet]
        [Route("Request/Delete/")]
        public HttpResponseMessage DeleteFile(string fileName)
        {

            //string fileName = Path.GetFileName(path);
            File.Delete(Path.Combine(workingFolder, fileName));

            var jsonNew = new
            {
                result = "File Generated Successfully."
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }

        public void checkProcess(int attemptNum)
        {
            if (attemptNum != 0) return;

            System.Threading.Tasks.Task task = new System.Threading.Tasks.Task(() =>
            {
                logger.Info("Inside parallel task - before sleep");

                Thread.Sleep(5000);

                logger.Info("Inside parallel task - after sleep");

                try
                {
                    logger.Info("Number of DWWIN " + Process.GetProcessesByName("DWWIN").Count());
                    logger.Info("Number of WerFault " + Process.GetProcessesByName("WerFault").Count());
                    logger.Info("Number of WINPROJ " + Process.GetProcessesByName("WINPROJ").Count());

                    foreach (Process proc in Process.GetProcessesByName("DWWIN"))
                    {
                        //if (!proc.Responding)
                        //{
                        //Thread.Sleep(10000);
                        logger.Info("Before DWWIN - auto kill after 5000 ms");
                        proc.Kill();
                        logger.Info("After DWWIN - auto kill after 5000 ms");
                        //}
                    }

                    foreach (Process proc in Process.GetProcessesByName("WerFault"))
                    {
                        //if (!proc.Responding)
                        //{
                        //Thread.Sleep(10000);
                        logger.Info("Before DWWIN - auto kill after 5000 ms");
                        proc.Kill();
                        logger.Info("After DWWIN - auto kill after 5000 ms");
                        //}
                    }

                    foreach (Process proc in Process.GetProcessesByName("WINPROJ"))
                    {
                        //if (!proc.Responding)
                        //{
                        //Thread.Sleep(10000);
                        logger.Info("Before WINPROJ - auto kill after 5000 ms");
                        proc.Kill();
                        logger.Info("After WINPROJ - auto kill after 5000 ms");
                        //}
                    }
                }
                catch (System.Exception ex)
                {
                    logger.Info("EXCEPTION: " + ex);
                }
            });
            task.Start();
        }
    }
}
