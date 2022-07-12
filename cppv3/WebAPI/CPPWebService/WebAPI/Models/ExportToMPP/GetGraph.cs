using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;

namespace WebAPI.Models.ExportToMPP
{
    public class GetGraph
    {
        public Data GetDetails(int ProjectId) //, string granularity
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);

            Data d = new Data();
            //GoogleChart innerArr;
            //GoogleChart outerArr = new GoogleChart();
            //outerArr.chartData = new List<object>();
            yData yd;
            string FTC = "0.00";
            string BTD = "0.00";
            DateTime date = DateTime.Now;
            DayOfWeek fdow = CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
            int offset = fdow - date.DayOfWeek;
            DateTime fdowDate = date.AddDays(offset + 1);

            DayOfWeek firstDay = CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
            DateTime firstDayInWeek = DateTime.Now.Date;
            while (firstDayInWeek.DayOfWeek != firstDay)
                firstDayInWeek = firstDayInWeek.AddDays(-1);

            //DateTime LastDayOfWeek = firstDayInWeek.AddDays(-1);  //For local
             
            //while (LastDayOfWeek.DayOfWeek != firstDay)
            //    LastDayOfWeek = LastDayOfWeek.AddDays(-1);

            try
            {
                using (var ctx = new CPPDbContext())
                {
                    var graph = ctx.Database.SqlQuery<GraphDetails>("call GetGraphDetails(@_ProjectID)",
                                                       new MySql.Data.MySqlClient.MySqlParameter("@_ProjectID", ProjectId)
                                                       )
                                                       .ToList();

                    #region chart.js
                    d.labels = new List<string>();
                    DateTime firstDate = graph.Min(a => a.StartDate);
                    DateTime lastDate = graph.Max(a => a.StartDate);
                    //d.labels.Add(firstDate.AddDays(-7).ToShortDateString());
                    d.labels.Add(firstDate.ToShortDateString());
                    foreach (var g in graph)
                    {
                       
                        d.labels.Add(g.EndDate.ToShortDateString());
                        //yd.data.Add(Convert.ToDouble(g.TotalBudget));
                        //yd.name = "Total Budget";
                    }

                    d.datasets = new List<yData>();
                    //yd = new yData();
                    //yd.data = new List<double>();
                    //foreach (var g in graph)
                    //{
                    //    yd.data.Add(Convert.ToDouble(g.TotalCost));
                    //    yd.label = "Total Budget";
                    //    yd.fill = false;
                    //    yd.borderColor = "blue";
                    //}
                    //d.datasets.Add(yd);

                    yd = new yData();
                    yd.data = new List<string>();
                    yd.data.Add("0.00");
                    
                    foreach (var g in graph)
                    {
                        if (Convert.ToDateTime(g.EndDate) < DateTime.Now)
                        {
                            FTC = Math.Round(Convert.ToDouble(FTC) + Convert.ToDouble(g.ATD), 2).ToString();
                            //yd.data.Add(Convert.ToDouble(g.ATD));
                            yd.data.Add(FTC);
                            yd.borderColor = "green";
                            yd.spanGaps = true;
                        }
                        else
                        {
                            yd.data.Add(null);
                        }
                        yd.label = "ATD";
                        yd.fill = false;
                    }
                    d.datasets.Add(yd);


                    yd = new yData();
                    yd.data = new List<string>();
                    yd.data.Add("0.00");

                    foreach (var g in graph)
                    {
                        if (Convert.ToDateTime(g.EndDate) < DateTime.Now)
                        {
                            BTD = Math.Round(Convert.ToDouble(BTD) + Convert.ToDouble(g.BTD), 2).ToString();
                            //yd.data.Add(Convert.ToDouble(g.ATD));
                            yd.data.Add(BTD);
                            yd.borderColor = "blue";
                            yd.spanGaps = true;
                        }
                        else
                        {
                            yd.data.Add(null);
                        }
                        yd.label = "BTD";
                        yd.fill = false;
                    }
                    d.datasets.Add(yd);

                    //yd = new yData();
                    //yd.data = new List<double>();
                    //yd.data.Add(0.00);
                    //double totalCost = 0.00;
                    //foreach (var g in graph)
                    //{
                    //    if (Convert.ToDateTime(g.EndDate) < DateTime.Now)
                    //    {
                    //        totalCost = Math.Round(totalCost + Convert.ToDouble(g.BTD), 2);
                    //        //yd.data.Add(Convert.ToDouble(g.BTD));
                    //        yd.data.Add(totalCost);
                    //        yd.borderColor = "blue";
                    //    }
                    //    else
                    //    {
                    //        totalCost = Math.Round(totalCost + Convert.ToDouble(g.BTC), 2);
                    //        //yd.data.Add(Convert.ToDouble(g.BTC));
                    //        yd.data.Add(totalCost);
                    //        yd.borderColor = "blue";
                    //    }
                    //    //yd.data.Add(Convert.ToDouble(g.BTD));
                    //    yd.label = "BTD";
                    //    yd.fill = false;

                    //    //yd.borderColor = "green";
                    //}
                    //d.datasets.Add(yd);

                    //yd = new yData();
                    //yd.data = new List<double>();
                    //foreach (var g in graph)
                    //{
                    //    yd.data.Add(Convert.ToDouble(g.BTC));
                    //    yd.label = "BTC";
                    //    yd.fill = false;
                    //    yd.borderColor = "purple";
                    //}
                    //d.datasets.Add(yd);

                    yd = new yData();
                    yd.data = new List<string>();
                    //yd.data.Add("0.00");
                    string totalEAC = FTC.ToString();
                    yd.data.Add(null);
                    foreach (var g in graph)
                    {
                        //if(Convert.ToDateTime(g.EndDate)== LastDayOfWeek)  //For local
                        if (Convert.ToDateTime(g.EndDate) == firstDayInWeek) //For server
                        {
                            yd.data.Add(totalEAC);
                        }
                        else if (Convert.ToDateTime(g.EndDate) > DateTime.Now)
                        {
                            totalEAC = Math.Round(Convert.ToDouble(totalEAC) + Convert.ToDouble(g.EAC), 2).ToString();
                            yd.data.Add(totalEAC);
                            yd.label = "ETC"; // Manasi 29-04-2021
                            yd.fill = false;
                            yd.borderColor = "orange";
                        }
                        else
                        {
                            yd.data.Add(null);
                            //-- Narayan 19-05-2022 --
                            yd.label = "ETC"; 
                            yd.fill = false;
                            yd.borderColor = "orange";
                            //-- -- -- -- -- -- -- -- -- --
                            yd.spanGaps = true;
                        }
                    }
                    d.datasets.Add(yd);


                    yd = new yData();
                    yd.data = new List<string>();
                    //yd.data.Add("0.00");
                    string totalBudget = BTD.ToString();
                    yd.data.Add(null);
                    foreach (var g in graph)
                    {
                        //if (Convert.ToDateTime(g.EndDate) == LastDayOfWeek)  //For local
                        if (Convert.ToDateTime(g.EndDate) == firstDayInWeek)   //For Server
                        {
                            yd.data.Add(totalBudget);
                        }
                        else if (Convert.ToDateTime(g.EndDate) > DateTime.Now)
                        {
                            totalBudget = Math.Round(Convert.ToDouble(totalBudget) + Convert.ToDouble(g.BTC), 2).ToString();
                            yd.data.Add(totalBudget);
                            yd.label = "BTC";
                            yd.fill = false;
                            yd.borderColor = "red";
                        }
                        else
                        {
                            yd.data.Add(null);
                            //-- Narayan 19-05-2022 --
                            yd.label = "BTC";
                            yd.fill = false;
                            yd.borderColor = "red";
                            //-- -- -- -- -- -- -- -- -- --
                            yd.spanGaps = true;
                        }
                    }
                    d.datasets.Add(yd);


                    //yd = new yData();
                    //yd.data = new List<double>();
                    //yd.data.Add(0.00);
                    //foreach (var g in graph)
                    //{
                    //    yd.data.Add(Convert.ToDouble(g.FTC));
                    //    yd.label = "FTC";
                    //    yd.fill = false;
                    //    yd.borderColor = "black";
                    //}
                    //d.datasets.Add(yd);
                    #endregion chart.js

                    //foreach (var g in graph)
                    //{
                    //    innerArr = new GoogleChart();
                    //    innerArr.chartData = new List<object>();
                    //    innerArr.chartData.Add(g.StartDate);
                    //    innerArr.chartData.Add(g.TotalCost);
                    //    innerArr.chartData.Add(g.ATD);
                    //    innerArr.chartData.Add(g.BTD);
                    //    innerArr.chartData.Add(g.BTC);
                    //    innerArr.chartData.Add(g.EAC);
                    //    innerArr.chartData.Add(g.FTC);
                    //    outerArr.chartData.Add(innerArr.chartData);
                    //}
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
            return d;

        }
    }
}