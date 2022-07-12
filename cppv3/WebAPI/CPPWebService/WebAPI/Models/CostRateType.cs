using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Web;

namespace WebAPI.Models
{
    [Table("cost_rate_type")]
    public class CostRateType
    {
        public CostRateType() //This will create a default value for audit date time when insert to db
        {
            CreatedDate = DateTime.Now;
            UpdatedDate = DateTime.Now;
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [NotMapped]
        public int Operation;

        public String RateType { get; set; } //Birdi Rate, Union Rate, G&A
        public String Description { get; set; }
        public String Note { get; set; }
        
        //Audit Fields
        public String CreatedBy { get; set; }
        public String UpdatedBy { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedDate { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedDate { get; set; }

        public static List<CostRateType> GetCostRateType()
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);

            List<CostRateType> costRateTypeList = new List<CostRateType>();
            try
            {

                using (var ctx = new CPPDbContext())
                {
                    costRateTypeList = ctx.CostRateType.OrderBy(a => a.RateType).ToList();
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

            return costRateTypeList;
        }

    }
}