using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Web;
using WebAPI.Helper;

namespace WebAPI.Models
{
    [Table("cost_overhead")]
    public class CostOverhead : Audit
    {

    
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [NotMapped]
        public int Operation;

        public int CostTypeID { get; set; }
        public int CostRateTypeID { get; set; } //Billable Rate
        public Double Markup { get; set; } //or aka Multiplier
        public String Description { get; set; }
        public String Note { get; set; }
        public Boolean isActive { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

      

        //Mapping
        [ForeignKey("CostTypeID")]
        public virtual CostType CostType{ get; set; }

        [ForeignKey("CostRateTypeID")]
        public virtual CostRateType CostRateType{ get; set; }

        public static List<CostOverhead> GetCostOverhead()
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);

            List<CostOverhead> costOverheadList = new List<CostOverhead>();
            try
            {

                using (var ctx = new CPPDbContext())
                {
                    //query everything except the custom
                    String query = "select * from cost_overhead  co "
                                    + " inner join cost_rate_type crt on crt.id = co.CostRateTypeID"
                                    + " where crt.RateType != 'Custom' "
                                    + " order by co.ID; ";
                    costOverheadList = ctx.Database.SqlQuery<CostOverhead>(query).ToList();
                        
                        //ctx.CostOverhead.OrderBy(a => a.ID).ToList();
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

            return costOverheadList;
        }


        public static List<CostOverhead> getCustomCostOverhead()
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);

            List<CostOverhead> costOverheadList = new List<CostOverhead>();
            try
            {

                using (var ctx = new CPPDbContext())
                {
                    //query everything except the custom
                    String query = "select * from cost_overhead  co "
                                    + " inner join cost_rate_type crt on crt.id = co.CostRateTypeID"
                                    + " where crt.RateType = 'Custom' "
                                    + " order by co.ID; ";
                    costOverheadList = ctx.Database.SqlQuery<CostOverhead>(query).ToList();

                    //ctx.CostOverhead.OrderBy(a => a.ID).ToList();
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

            return costOverheadList;
        }

        public static List<CostOverhead> getActiveCostOverhead()
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);

            List<CostOverhead> costOverheadList = new List<CostOverhead>();
            try
            {

                using (var ctx = new CPPDbContext())
                {
                    //query everything except the custom
                    String query = "select * from cost_overhead  co "
                                    + " inner join cost_rate_type crt on crt.id = co.CostRateTypeID"
                                    + " where crt.RateType != 'Custom' "
                                    + " and StartDate < @CurrentDate "
                                    + " and EndDate > @CurrentDate"
                                    + " order by co.ID; ";
                    costOverheadList = ctx.Database.SqlQuery<CostOverhead>(query,
                                                new MySql.Data.MySqlClient.MySqlParameter("@CurrentDate", DateTime.UtcNow)).ToList();

                    //ctx.CostOverhead.OrderBy(a => a.ID).ToList();
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

            return costOverheadList;
        }



        public static String registerCostOverhead(CostOverhead costOverhead)
        {

            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String result = "";
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    CostOverhead retrievedCostOverhead = new CostOverhead();
                    retrievedCostOverhead = ctx.CostOverhead.Where(u => u.CostTypeID == costOverhead.CostTypeID
                                                                        && u.CostRateTypeID == costOverhead.CostRateTypeID
                                                                        && (DbFunctions.TruncateTime(u.StartDate) <= DbFunctions.TruncateTime(costOverhead.EndDate)
                                                                        && DbFunctions.TruncateTime(u.EndDate) >= DbFunctions.TruncateTime(costOverhead.StartDate))).FirstOrDefault();

                    bool validCostOverheadRateType = false;

                    List<CostTypeRateType> costTypeRateTypeList = ctx.CostTypeRateType.Where(b => b.CostTypeID == costOverhead.CostTypeID).ToList();

                    for(int x = 0; x < costTypeRateTypeList.Count; x++)
                    {
                        if(costOverhead.CostRateTypeID == costTypeRateTypeList[x].RateTypeID)
                        {
                            validCostOverheadRateType = true;
                            break;
                        }
                    }

                    if (!validCostOverheadRateType)
                    {
                        CostType costType = ctx.CostType.Where(p => p.ID == costOverhead.CostTypeID).FirstOrDefault();
                        CostRateType costRateType = ctx.CostRateType.Where(p => p.ID == costOverhead.CostRateTypeID).FirstOrDefault();

                        result += costType.Type + " - " + costRateType.RateType + " failed to be created, invalid rate type.\n";
                    }
                    else if(retrievedCostOverhead == null)
                    {
                        //register
                        ctx.CostOverhead.Add(costOverhead);
                        ctx.SaveChanges();
                        //result = "Success";

                        CostType costType = ctx.CostType.Where(p => p.ID == costOverhead.CostTypeID).FirstOrDefault();
                        CostRateType costRateType = ctx.CostRateType.Where(p => p.ID == costOverhead.CostRateTypeID).FirstOrDefault();

                        result += costType.Type + " - " + costRateType.RateType + " has been created successfully.\n";
                    }
                    else
                    {
                        CostType costType = ctx.CostType.Where(p => p.ID == costOverhead.CostTypeID).FirstOrDefault();
                        CostRateType costRateType = ctx.CostRateType.Where(p => p.ID == costOverhead.CostRateTypeID).FirstOrDefault();

                        result += costType.Type + " - " + costRateType.RateType + " failed to be created, duplicate for date range found.\n";
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
            }

            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Exit Point", Logger.logLevel.Debug);

            return result;
        }
        public static String updateCostOverhead(CostOverhead costOverhead)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String result = "";

            try
            {
                using (var ctx = new CPPDbContext())
                {
                    CostOverhead retrievedCostOverhead = new CostOverhead();
                    CostOverhead duplicateCostOverhead = new CostOverhead();

                    retrievedCostOverhead = ctx.CostOverhead.Where(u => u.ID == costOverhead.ID).FirstOrDefault();

                    duplicateCostOverhead = ctx.CostOverhead.Where(u => u.ID != costOverhead.ID
                                                                        && u.CostTypeID == costOverhead.CostTypeID
                                                                        && u.CostRateTypeID == costOverhead.CostRateTypeID
                                                                        && (DbFunctions.TruncateTime(u.StartDate) <= DbFunctions.TruncateTime(costOverhead.EndDate)
                                                                        && DbFunctions.TruncateTime(u.EndDate) >= DbFunctions.TruncateTime(costOverhead.StartDate))).FirstOrDefault();

                    bool validCostOverheadRateType = false;

                    List<CostTypeRateType> costTypeRateTypeList = ctx.CostTypeRateType.Where(b => b.CostTypeID == costOverhead.CostTypeID).ToList();

                    for (int x = 0; x < costTypeRateTypeList.Count; x++)
                    {
                        if (costOverhead.CostRateTypeID == costTypeRateTypeList[x].RateTypeID)
                        {
                            validCostOverheadRateType = true;
                            break;
                        }
                    }

                    if (!validCostOverheadRateType)
                    {
                        CostType costType = ctx.CostType.Where(p => p.ID == costOverhead.CostTypeID).FirstOrDefault();
                        CostRateType costRateType = ctx.CostRateType.Where(p => p.ID == costOverhead.CostRateTypeID).FirstOrDefault();

                        result += costType.Type + " - " + costRateType.RateType + " failed to be updated, invalid rate type.\n";
                    }
                    else if (duplicateCostOverhead != null)
                    {
                        CostType costType = ctx.CostType.Where(p => p.ID == costOverhead.CostTypeID).FirstOrDefault();
                        CostRateType costRateType = ctx.CostRateType.Where(p => p.ID == costOverhead.CostRateTypeID).FirstOrDefault();

                        result += costType.Type + " - " + costRateType.RateType + " failed to be updated, duplicate entry for the date range will be created.\n";
                    }
                    else if (retrievedCostOverhead != null)
                    {
                        CopyUtil.CopyFields<CostOverhead>(costOverhead,retrievedCostOverhead);
                        ctx.Entry(retrievedCostOverhead).State = System.Data.Entity.EntityState.Modified;
                        ctx.SaveChanges();

                        CostType costType = ctx.CostType.Where(p => p.ID == costOverhead.CostTypeID).FirstOrDefault();
                        CostRateType costRateType = ctx.CostRateType.Where(p => p.ID == costOverhead.CostRateTypeID).FirstOrDefault();

                        result += costType.Type + " - " + costRateType.RateType + " has been updated successfully.\n";
                    }
                    else
                    {
                        CostType costType = ctx.CostType.Where(p => p.ID == costOverhead.CostTypeID).FirstOrDefault();
                        CostRateType costRateType = ctx.CostRateType.Where(p => p.ID == costOverhead.CostRateTypeID).FirstOrDefault();

                        result += costType.Type + " - " + costRateType.RateType + " failed to be updated, it does not exist.\n";
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

            }
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Exit Point", Logger.logLevel.Debug);

            return result;

        }
        public static String deleteCostOverhead(CostOverhead costOverhead)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String result = "";
            String entryName = "";
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    CostOverhead retrievedCostOverhead = new CostOverhead();
                    retrievedCostOverhead = ctx.CostOverhead.Where(u => u.ID == costOverhead.ID).FirstOrDefault();

                    CostType costType = ctx.CostType.Where(p => p.ID == costOverhead.CostTypeID).FirstOrDefault();
                    CostRateType costRateType = ctx.CostRateType.Where(p => p.ID == costOverhead.CostRateTypeID).FirstOrDefault();

                    entryName = costType.Type + " - " + costRateType.RateType;

                    if (retrievedCostOverhead != null)
                    {
                        entryName = retrievedCostOverhead.Description;
                        ctx.CostOverhead.Remove(retrievedCostOverhead);
                        ctx.SaveChanges();

                        result = costType.Type + " - " + costRateType.RateType + " has been deleted successfully.\n";
                    }
                    else
                    {
                        result = costType.Type + " - " + costRateType.RateType + " failed to be deleted, it does not exist.\n";
                    }
                }

            }
            catch (Exception ex)
            {
                result = entryName + " failed to be updated due to dependencies.\n";
                var stackTrace = new StackTrace(ex, true);
                var line = stackTrace.GetFrame(0).GetFileLineNumber();
                Logger.LogExceptions(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, ex.Message, line.ToString(), Logger.logLevel.Exception);
            }
            finally
            {
            }
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Exit Point", Logger.logLevel.Debug);

            return result;
        }
    }
}