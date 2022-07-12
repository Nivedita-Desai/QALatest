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
    [Table("costtype_ratetype")]
    public class CostTypeRateType
    {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [NotMapped]
        public int Operation;

        public CostTypeRateType() //This will create a default value for audit date time when insert to db
        {
            CreatedDate = DateTime.Now;
            UpdatedDate = DateTime.Now;
        }

        public int CostTypeID { get; set; }
        public int RateTypeID { get; set; }
        public String Description { get; set; }


        //Audti Fields
        public String CreatedBy { get; set; }
        public String UpdatedBy { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedDate { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedDate { get; set; }

        //Mapping
        //Mapping
        [ForeignKey("CostTypeID")]
        public virtual CostType CostType { get; set; }
        //Mapping
        [ForeignKey("RateTypeID")]
        public virtual CostRateType CostRateType{ get; set; }

        public static List<CostTypeRateType> GetCostTypeRateType()
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);

            List<CostTypeRateType> costTypeRateTypeList = new List<CostTypeRateType>();
            try
            {

                using (var ctx = new CPPDbContext())
                {
                    costTypeRateTypeList = ctx.CostTypeRateType.OrderBy(a => a.ID).ToList();
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

            return costTypeRateTypeList;
        }
    }
}