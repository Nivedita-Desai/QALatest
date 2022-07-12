using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    [Table("cost_rate_type")]
    public class TrendCostOverhead
    {

       /* public TrendCostOverhead()//This will create a default value for audit date time when insert to db
        {
            CreatedDate = DateTime.Now;
            UpdatedDate = DateTime.Now;
        }*/

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }


        public int CostOverheadID { get; set; }
        public int TrendID { get; set; }
        public String Description { get; set; }
        public Double CurrentMarkup { get; set; }
        public Boolean isActive { get; set; } //
        public String Justification { get; set; } 

        //Custom Trend fields
        public double OriginalCost { get; set; }
        public double CurrentCost { get; set; }
        public double NewCost { get; set; }

        //Audit Fields
        public String CreatedBy { get; set; }
        public String UpdatedBy { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedDate { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedDate { get; set; }

        //Mapping
        [ForeignKey("TrendID")]
        public virtual Trend Trend { get; set; }

        //ON DELETE RESTRICT
        [ForeignKey("CostOverheadID")]
        public virtual CostOverhead CostOverhead{ get; set; }


        //Nivedita 10022022
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string DeletedBy { get; set; }

        public static List<TrendCostOverhead> getTrendCustomCostOverhead(int trendID)
        {
            List<TrendCostOverhead> trendCostOverheads = new List<TrendCostOverhead>();
            List<TrendCostOverhead> results = new List<TrendCostOverhead>();
            using(var ctx = new CPPDbContext())
            {
                #region SqlQuery method
                    /*
                        The method ctx.Database.SqlQuery cannot be used to include navigation properties
                     */

                    //String query = "select tco.* from trend tr" +
                    //                       " inner join trend_cost_overhead tco on tr.trendid = tco.trendid " +
                    //                       " inner join cost_overhead co on co.id = tco.CostOverheadID" +
                    //                       " inner join cost_type ct on ct.id = co.CostTypeID" +
                    //                       " inner join cost_rate_type crt on crt.id = co.CostRateTypeID" +
                    //                       " where tr.trendid = @TrendID " +
                    //                       "  AND crt.RateType = 'Custom'";

                    //trendCostOverheads = ctx.Database.SqlQuery<TrendCostOverhead>(query,
                    //                                    new MySql.Data.MySqlClient.MySqlParameter("@TrendID", trendID))
                    //                                    .ToList();

                    //foreach(var tco in trendCostOverheads)
                    //{
                    //    TrendCostOverhead newTco = ctx.TrendCostOverhead
                    //                                .Include("CostOverhead")
                    //                                .Include("CostOverhead.CostRateType")
                    //                                .Include("CostOverhead.CostType")
                    //                            .Where(a => a.ID == tco.ID)
                    //                            .FirstOrDefault();

                    //    results.Add(newTco);
                    //}
                #endregion

                try
                {
                    /*Lamda expression method
                        The join method in LINQ changes the shape of the query. It will ignores the Include if used within the query.
                            e.g join tco in ctx.TrendCostOverhead.Include("CostOverhead").Include("CostOverhead.CostRateType").Include("CostOverhead.CostType")
                        The result will returns the list of entity without the navigation properties

                        To Include the navigation properties. Have to convert to DbQuery so that i can use the Include method
                    */
                         results = ((from tr in ctx.Trend
                                          join tco in ctx.TrendCostOverhead
                                                on tr.TrendID equals tco.TrendID
                                          join co in ctx.CostOverhead
                                                on tco.CostOverheadID equals co.ID
                                          join ct in ctx.CostType
                                                on co.CostTypeID equals ct.ID
                                          join crt in ctx.CostRateType
                                                on co.CostRateTypeID equals crt.ID

                                where tr.TrendID == trendID
                                    && crt.RateType == "Custom" && tr.IsDeleted==false

                                select tco
                            )  as DbQuery<TrendCostOverhead>) //Convert to DBQuery so that it can use the Include method
                               .Include("CostOverhead") //Loading Navigation Properties
                               .Include("CostOverhead.CostRateType")
                               .Include("CostOverhead.CostType")
                            .ToList();
                    ;
          
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
             
            }

            return results;
        }


        public static String applyNewMarkup(int trendId, int trendCostOverheadID, String costType, Double newMarkup, String Justification)
        {
            String query = "Call apply_custom_cost_overhead(@TrendID, @TrendCostOverheadID, @CostType, @newMarkup, @justification)";
            String result = "";
            using(var ctx = new CPPDbContext())
            {
                try
                {
                    //Execute Sql command does not returns a value
                    ctx.Database.ExecuteSqlCommand(query,
                                        new MySql.Data.MySqlClient.MySqlParameter("@TrendID", trendId),
                                        new MySql.Data.MySqlClient.MySqlParameter("@TrendCostOverheadID", trendCostOverheadID),
                                        new MySql.Data.MySqlClient.MySqlParameter("@CostType", costType),
                                        new MySql.Data.MySqlClient.MySqlParameter("@newMarkup", newMarkup),
                                        new MySql.Data.MySqlClient.MySqlParameter("@justification", Justification)
                                        );
                    result = "success";
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
         
            }
            return result;
        }
    }
}