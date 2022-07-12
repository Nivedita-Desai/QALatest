using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models.StoredProcedure
{
    public class CostLineItemResult
    {
        public int LineNumber { get; set; }
        public Boolean IsExist { get; set; }


        public static CostLineItemResult getCostLineItem(String ProjectClassID, String ProjectNumber,String ProjectElementNumber, String TrendNumber, String PhaseCode, String CategoryID, String SubCategoryID,
                                            String CostType, String FtePositionID, String EmployeeID, String ODCTypeID, String MaterialCategoryID, String MaterialID,
                                            String SubcontractorTypeID, String SubcontractorID)
        {
            CostLineItemResult results = null;
            using (var ctx = new CPPDbContext())
            {
                 results = ctx.Database.SqlQuery<CostLineItemResult>("call get_cost_line_item(@ProjectDivisionID, @ProjectNumber,@ProjectElementNumber, @TrendNumber,@PhaseCode,@CategoryID,"
                                                                      + "@SubCategoryID,@CostType, @PositionID, @EmployeeID, @ODCTypeID,"
                                                                      + "@MaterialCategoryID,@MaterialID,@SubcontractorTypeID,@SubcontractorID)",
                                                new MySql.Data.MySqlClient.MySqlParameter("@ProjectDivisionID", ProjectClassID),
                                                new MySql.Data.MySqlClient.MySqlParameter("@ProjectNumber", ProjectNumber),
                                                new MySql.Data.MySqlClient.MySqlParameter("@ProjectElementNumber", ProjectElementNumber),
                                                new MySql.Data.MySqlClient.MySqlParameter("@TrendNumber", TrendNumber),
                                                new MySql.Data.MySqlClient.MySqlParameter("@PhaseCode", PhaseCode),
                                                new MySql.Data.MySqlClient.MySqlParameter("@CategoryID", CategoryID),
                                                new MySql.Data.MySqlClient.MySqlParameter("@SubCategoryID", SubCategoryID),
                                                new MySql.Data.MySqlClient.MySqlParameter("@CostType", CostType),
                                                new MySql.Data.MySqlClient.MySqlParameter("@PositionID", FtePositionID),
                                                new MySql.Data.MySqlClient.MySqlParameter("@EmployeeID", EmployeeID),
                                                new MySql.Data.MySqlClient.MySqlParameter("@ODCTypeID", ODCTypeID),
                                                new MySql.Data.MySqlClient.MySqlParameter("@MaterialCategoryID", MaterialCategoryID),
                                                new MySql.Data.MySqlClient.MySqlParameter("@MaterialID", MaterialID),
                                                new MySql.Data.MySqlClient.MySqlParameter("@SubcontractorTypeID", SubcontractorTypeID),
                                                new MySql.Data.MySqlClient.MySqlParameter("@SubcontractorID", SubcontractorID)).FirstOrDefault();

            }

            return results;
        
        }




        ///<summary>
        ///  CheckIFCostExistInOtherTrend - Check to see if there is already a cost with same category  exist in other trend
        /// <returns>
        ///     The number of cost with same category across trends
        /// </returns>
        ///</summary>
        public static int checkIfCostExistInOtherTrend(String ProjectID,String TrendNumber, String PhaseCode, String CategoryDescription, String SubCategoryDescription, String CostType,
                                                                    String FTEPositionID, String EmployeeID, String ODCTypeID, String SubcontractorTypeID, String SubcontractorID, String MaterialCategoryID, String MaterialID)
        {
            int result = 0;
            using(var ctx = new CPPDbContext())
            {
                try
                {

                    result = ctx.Database.SqlQuery<int>("call check_if_cost_exist_in_other_trend(@ProjectID,@TrendNumber, @PhaseCode, @Category,@SubCategory,@CostType,"
                                                                                  + "@FTEPositionID,@EmployeeID, @ODCTypeID, @SubcontractorTypeID, @SubcontractorID,"
                                                                                  + "@MaterialCategoryID,@MaterialID)",
                                                            new MySql.Data.MySqlClient.MySqlParameter("@ProjectID", ProjectID),
                                                            new MySql.Data.MySqlClient.MySqlParameter("@TrendNumber", TrendNumber),
                                                            new MySql.Data.MySqlClient.MySqlParameter("@PhaseCode", PhaseCode),
                                                            new MySql.Data.MySqlClient.MySqlParameter("@Category", CategoryDescription),
                                                            new MySql.Data.MySqlClient.MySqlParameter("@SubCategory", SubCategoryDescription),
                                                            new MySql.Data.MySqlClient.MySqlParameter("@CostType", CostType),
                                                            new MySql.Data.MySqlClient.MySqlParameter("@FTEPositionID", FTEPositionID),
                                                            new MySql.Data.MySqlClient.MySqlParameter("@EmployeeID", EmployeeID),
                                                            new MySql.Data.MySqlClient.MySqlParameter("@ODCTypeID", ODCTypeID),
                                                            new MySql.Data.MySqlClient.MySqlParameter("@SubcontractorTypeID", SubcontractorTypeID),
                                                            new MySql.Data.MySqlClient.MySqlParameter("@SubcontractorID", SubcontractorID),
                                                            new MySql.Data.MySqlClient.MySqlParameter("@MaterialCategoryID", MaterialCategoryID),
                                                            new MySql.Data.MySqlClient.MySqlParameter("@MaterialID", MaterialID)).FirstOrDefault();
                }catch(Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }


            return result; 
        }


        //Manasi 04-11-2020
        public static CostLineItemResult getNewCostLineItem(String ProjectClassID, String ProjectNumber, String ProjectElementNumber, String TrendNumber, String PhaseCode, String CategoryID, String SubCategoryID,
                                        String CostType, String FtePositionID, String EmployeeID, String ODCTypeID, String MaterialCategoryID, String MaterialID,
                                        String SubcontractorTypeID, String SubcontractorID, String projectID)
        {
            CostLineItemResult results = null;
            using (var ctx = new CPPDbContext())
            {
                results = ctx.Database.SqlQuery<CostLineItemResult>("call GetMaxLineNo(@ProjectDivisionID, @ProjectNumber,@ProjectElementNumber,@PhaseCode,@CategoryID,"
                                                                     + "@CostType, @PositionID, @EmployeeID, @ODCTypeID,"
                                                                     + "@MaterialCategoryID,@MaterialID,@SubcontractorTypeID,@SubcontractorID, @SubCategoryID, @TrendNumber, @ProjectID)",
                                               new MySql.Data.MySqlClient.MySqlParameter("@ProjectDivisionID", ProjectClassID),
                                               new MySql.Data.MySqlClient.MySqlParameter("@ProjectNumber", ProjectNumber),
                                               new MySql.Data.MySqlClient.MySqlParameter("@ProjectElementNumber", ProjectElementNumber),
                                               new MySql.Data.MySqlClient.MySqlParameter("@PhaseCode", PhaseCode),
                                               new MySql.Data.MySqlClient.MySqlParameter("@CategoryID", CategoryID),
                                               new MySql.Data.MySqlClient.MySqlParameter("@CostType", CostType),
                                               new MySql.Data.MySqlClient.MySqlParameter("@PositionID", FtePositionID),
                                               new MySql.Data.MySqlClient.MySqlParameter("@EmployeeID", EmployeeID),
                                               new MySql.Data.MySqlClient.MySqlParameter("@ODCTypeID", ODCTypeID),
                                               new MySql.Data.MySqlClient.MySqlParameter("@MaterialCategoryID", MaterialCategoryID),
                                               new MySql.Data.MySqlClient.MySqlParameter("@MaterialID", MaterialID),
                                               new MySql.Data.MySqlClient.MySqlParameter("@SubcontractorTypeID", SubcontractorTypeID),
                                               new MySql.Data.MySqlClient.MySqlParameter("@SubcontractorID", SubcontractorID),
                                               new MySql.Data.MySqlClient.MySqlParameter("@SubCategoryID", SubCategoryID),
                                               new MySql.Data.MySqlClient.MySqlParameter("@TrendNumber", TrendNumber),
                                               new MySql.Data.MySqlClient.MySqlParameter("@ProjectID", projectID)).FirstOrDefault();

            }

            return results;

        }




    }
}