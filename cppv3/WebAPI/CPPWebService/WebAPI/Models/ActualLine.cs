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
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.SqlServer;
using System.Text.RegularExpressions;

namespace WebAPI.Models
{
    public class ActualLine
    {
        // Response types
        public int success = 0;
        public int duplicate = 1;
        public int formatError = 4;
        public int nonExistentCost = 5;
        public int Response { get; set; }                   // Response for creating actual line (success, duplicate, error)
        public String ResponseText { get; set; }
        
        public String CostLineItem { get; set; }
        public String PONumber { get; set; }
        public int ProjectDivision { get; set; }
        public int ProjectNumber { get; set; }
        public int Phase { get; set; }
        public int MainCategory { get; set; }
        public int SubCategory { get; set; }
        public int CostLineID { get; set; }
        public String CostType { get; set; }
        public float Amount { get; set; }
        public float Quantity { get; set; }
        public DateTime Date { get; set; }
        public int RowNumberFromExcel { get; set; }

        public ActualLine(string CostLineItem, string PONumber,
            string cost, string quantity, string date, int rowNumber)
        {
            try
            {
                this.Response = 0;
                this.RowNumberFromExcel = rowNumber;
                this.PONumber = PONumber;

                //String[] lineItem = CostLineItem.Split('.');    // Swapnil 24-11-2020

                this.CostLineItem = CostLineItem;
                //-----------------------Swapnil 24-11-2020--------------------------------------------
                //this.ProjectDivision = Int32.Parse(lineItem[0]);
                //this.ProjectNumber = Int32.Parse(lineItem[1]);
                //this.Phase = Int32.Parse(lineItem[2]);
                //this.MainCategory = Int32.Parse(lineItem[3]);
                //this.SubCategory = Int32.Parse(lineItem[4]);

                this.ProjectDivision = Int32.Parse("0");
                this.ProjectNumber = Int32.Parse("0");
                this.Phase = Int32.Parse("0");
                this.MainCategory = Int32.Parse("0");
                this.SubCategory = Int32.Parse("0");
                //------------------------------------------------------------------------------------------
                this.CostType = Cost.getCostTypeByLineItem(CostLineItem);
                this.Amount = float.Parse(cost, System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
                this.Quantity = float.Parse(quantity);
                try
                {
                    this.Date = DateTime.FromOADate(Int32.Parse(date));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    this.Date = DateTime.Parse(date);
                }

                // Check if the actual line is a duplicate in historical_actual_cost.
                if (this.checkHistoricalActual())
                {
                    this.setDuplicate();
                }

                // Check for non existent parallel cost for actual line item.
                if (!this.doesCostExist())
                {
                    this.setNonExistentCostError();
                }

                // Save this in historical_actual_cost.
                bool dup = this.isDuplicate();
                bool e1 = this.isFormatError();
                bool e2 = this.isNonExistentCostError();
                // Should check if not all these things, or just insert no matter what
                if (!(this.isDuplicate() || this.isFormatError() | this.isNonExistentCostError()))
                {
                    this.saveHistoricalActual();
                }
            }
            catch (Exception e)
            {
                Console.Write(e);
                this.setFormatError();
            }
        }
        
        // Save the actual line to the database and return true if it worked.
        public bool saveActualLine()
        {
            if (this.isFormatError() | this.isNonExistentCostError() | this.isDuplicate())
            {
                return false;
            }
            bool noError = true;
            MySqlConnection conn = null;
            MySqlCommand command = null;
            try
            {
                conn = ConnectionManager.getConnection();
                conn.Open();
                String query = "update_actual";
                command = new MySqlCommand(query, conn);
                command.CommandType = CommandType.StoredProcedure;
                
                command.Parameters.Add(new MySqlParameter("_actualDateString", this.Date.ToString("MM/dd/yyyy")));
                command.Parameters.Add(new MySqlParameter("_costType", this.CostType));
                command.Parameters.Add(new MySqlParameter("_lineItem", this.CostLineItem));
                command.Parameters.Add(new MySqlParameter("_increaseQuantity", this.Quantity));
                command.Parameters.Add(new MySqlParameter("_increaseAmount", this.Amount));

                int numberOfRecords = command.ExecuteNonQuery();

                // If no records were updated, there is something wrong with the query.
                if (numberOfRecords == 0)
                {
                    noError = false;
                }
            }
            catch (Exception ex)
            {
                var stackTrace = new StackTrace(ex, true);
                var line = stackTrace.GetFrame(0).GetFileLineNumber();
                Logger.LogExceptions(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, ex.Message, line.ToString(), Logger.logLevel.Exception);
                noError = false;
            }
            finally
            {
                if (conn != null) conn.Close();
            }
            
            return noError;
        }

        // Returns true if the actual line item contains this response.
        public bool isDuplicate()
        {
            return this.Response == this.duplicate;
        }
        public bool isFormatError()
        {
            return this.Response == this.formatError;
        }
        public bool isNonExistentCostError()
        {
            return this.Response == this.nonExistentCost;
        }

        // Set response for actual line item.
        public void setDuplicate()
        {
            this.Response = this.duplicate;
            this.ResponseText = String.Format(WebAPI.Models.UploadResponse.duplicateResponse, this.RowNumberFromExcel, this.CostLineItem);
        }
        public void setFormatError()
        {
            this.Response = this.formatError;
            this.ResponseText = String.Format(WebAPI.Models.UploadResponse.formatErrorResponse, this.RowNumberFromExcel);
        }
        public void setNonExistentCostError()
        {
            this.Response = this.nonExistentCost;
            this.ResponseText = String.Format(WebAPI.Models.UploadResponse.nonExistentCostErrorResponse, this.RowNumberFromExcel, this.CostLineItem);
        }

        // Save this actual line item to see if it was already submitted later.
        public void saveHistoricalActual()
        {
            MySqlConnection conn = null;
            MySqlCommand command = null;
            try
            {
                conn = ConnectionManager.getConnection();
                conn.Open();
                String query = "insert_historical_cost";
                command = new MySqlCommand(query, conn);
                command.CommandType = CommandType.StoredProcedure;
                
                command.Parameters.Add(new MySqlParameter("_lineItem", this.CostLineItem));
                command.Parameters.Add(new MySqlParameter("_ponumber", this.PONumber));
                command.Parameters.Add(new MySqlParameter("_date", this.Date.ToString()));
                command.Parameters.Add(new MySqlParameter("_amount", this.Amount));
                command.Parameters.Add(new MySqlParameter("_quantity", this.Quantity));

                command.ExecuteNonQuery();
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
        }
        // Returns true if the actual line item is a duplicate.
        public bool checkHistoricalActual()
        {
            bool hasRows = false;
            MySqlConnection conn = null;
            MySqlDataReader reader = null;
            try
            {
                conn = ConnectionManager.getConnection();
                conn.Open();
                String query = string.Format(
                    "select * from historical_actual_cost where lineItem = '{0}' and ponumber = '{1}' and date = '{2}';",
                    this.CostLineItem, this.PONumber, this.Date.ToString());
                MySqlCommand command = new MySqlCommand(query, conn);

                using (reader = command.ExecuteReader())
                {
                    hasRows = reader.HasRows;
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                var stackTrace = new StackTrace(ex, true);
                var line = stackTrace.GetFrame(0).GetFileLineNumber();
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
                if (reader != null)
                {
                    reader.Close();
                }
            }
            return hasRows;
        }


        // Return true if this cost exists in the cost tables.
        public bool doesCostExist()
        {
            bool costExists = false;
            MySqlConnection conn = null;
            MySqlCommand command = null;
            MySqlDataReader reader = null;
            try
            {
                conn = ConnectionManager.getConnection();
                conn.Open();
                String query = "get_cost_by_line_item";
                command = new MySqlCommand(query, conn);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new MySqlParameter("_lineItem", this.CostLineItem));

                using (reader = command.ExecuteReader())
                {
                    costExists = reader.HasRows;
                    reader.Close();
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
            }

            return costExists;
        }
    }
}