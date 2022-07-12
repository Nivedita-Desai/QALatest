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
using WebAPI.Helper;

namespace WebAPI.Models
{
    [Table("fte_position")]
    public class FTEPosition : Audit
    {
        readonly static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        [NotMapped]
        public int Operation;

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public String PositionID { get; set; }
        public String PositionDescription { get; set; }
        public String MinHourlyRate { get; set; }
        public String MaxHourlyRate { get; set; }
        public String CurrentHourlyRate { get; set; }
        public String UniqueIdentityNumber { get; set; }

        //FTEPosition() { }
        //FTEPosition(String id, String desc, String min_rate, String max_rate, String current_rate, int primaryKey)
        //{ PositionID = DT_RowId = id; PositionDescription = desc; MinHourlyRate = min_rate; MaxHourlyRate = max_rate; CurrentHourlyRate = current_rate; Id = primaryKey; }

        ////From RequestLookupFTEPositionController
        public static List<FTEPosition> getFTEPosition(int Id, String PositionDescription)
        {
            List<FTEPosition> MatchedFTEPositionList = new List<FTEPosition>();
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
          
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    if (Id != -1 )
                    {
                        MatchedFTEPositionList = ctx.FtePosition.Where(f =>f.Id == Id ).ToList();
                    }
                    else
                    {
                        MatchedFTEPositionList = ctx.FtePosition.OrderBy(a=>a.PositionDescription).ToList();
                    }
                }
                // create and open a connection object
                //conn = ConnectionManager.getConnection();
                //conn.Open();

                //String query = "SELECT PositionID, PositionDescription, MinHourlyRate, MaxHourlyRate, CurrentHourlyRate, Id FROM fte_position";
                //query += " WHERE 1=1";
                //if (Id != -1)
                //    query += " AND Id = " + Id;
                //if (PositionDescription != "null")
                //    query += " AND PositionDescription like '%" + PositionDescription + "%'";
                //MySqlCommand command = new MySqlCommand(query, conn);


                //using (reader = command.ExecuteReader())
                //{
                //    while (reader.Read())
                //    {
                //        FTEPosition RetreivedFTEPosition = new FTEPosition(reader.GetValue(0).ToString().Trim(), reader.GetValue(1).ToString().Trim(),
                //            reader.GetValue(2).ToString().Trim(), reader.GetValue(3).ToString().Trim(),
                //            reader.GetValue(4).ToString().Trim(), Convert.ToInt16(reader.GetValue(5).ToString().Trim()));
                //        MatchedFTEPositionList.Add(RetreivedFTEPosition);
                //    }
                //}



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
            return MatchedFTEPositionList;

        }

        ////From RegisterLookupFTEPositionController
        public static String registerFTEPosition(FTEPosition fte_position)
        {
            log4net.Config.XmlConfigurator.Configure();
            log.Debug("registerFTEPosition start here");
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug); 
            String register_result = "";
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    FTEPosition retrievedFtePosition = new FTEPosition();
                    retrievedFtePosition = ctx.FtePosition.Where(f => f.PositionDescription == fte_position.PositionDescription).FirstOrDefault();
                    if (retrievedFtePosition == null)
                    {
                        //register
                        ctx.FtePosition.Add(fte_position);
                        ctx.SaveChanges();
                        register_result = fte_position.PositionDescription + " has been created successfully.\n";
                    }
                    else
                    {
                        register_result = fte_position.PositionDescription + " failed to be created, it already exist.\n";
                    }
                }
              
            }
            catch (Exception ex)
            {
                var stackTrace = new StackTrace(ex, true);
                var line = stackTrace.GetFrame(0).GetFileLineNumber();
                log.Debug(MethodBase.GetCurrentMethod().DeclaringType.ToString());
                log.Debug(MethodBase.GetCurrentMethod().Name);
                log.Debug(ex.Message);
                log.Debug(line.ToString());
                log.Debug(Logger.logLevel.Exception);
                log.Debug("registerFTEPosition end here");
                Logger.LogExceptions(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, ex.Message, line.ToString(), Logger.logLevel.Exception);
            }
            finally
            {            
            }
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Exit Point", Logger.logLevel.Debug);

            return register_result;
        }

        public static String updateFTEPosition(FTEPosition fte_position)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
          
            String update_result = "";
         
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    FTEPosition retrievedFtePosition = new FTEPosition();
                    retrievedFtePosition = ctx.FtePosition.Where(f => f.Id == fte_position.Id).FirstOrDefault();

                    FTEPosition duplicateFtePosition = new FTEPosition();
                    duplicateFtePosition = ctx.FtePosition.Where(u => u.PositionDescription == fte_position.PositionDescription
                                                                        && u.Id != fte_position.Id).FirstOrDefault();

                    if (duplicateFtePosition != null)
                    {
                        update_result = fte_position.PositionDescription + " failed to be updated, duplicate will be created.\n";
                    }
                    else if (retrievedFtePosition != null)
                    {
                        CopyUtil.CopyFields<FTEPosition>(fte_position, retrievedFtePosition);
                        ctx.Entry(retrievedFtePosition).State = System.Data.Entity.EntityState.Modified;
                        ctx.SaveChanges();
                        update_result = fte_position.PositionDescription + " has been updated successfully.\n";
                    }
                    else
                    {
                        update_result = fte_position.PositionDescription + " failed to be updated, it does not exist.\n";
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
            return update_result;
        }

        public static String deleteFTEPosition(FTEPosition fte_position)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String result = "";
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    FTEPosition retrievedFtePosition = new FTEPosition();
                    retrievedFtePosition = ctx.FtePosition.Where(f => f.Id == fte_position.Id && f.PositionDescription == fte_position.PositionDescription).FirstOrDefault();
                    if(retrievedFtePosition != null)
                    {
                        //Delete
                        ctx.FtePosition.Remove(retrievedFtePosition);
                        ctx.SaveChanges();
                        result = fte_position.PositionDescription + " has been deleted successfully.\n";
                    }
                    else
                    {
                        result = fte_position.PositionDescription + " failed to be deleted, it does not exist.\n";
                    }
                }
            }
            catch (Exception ex)
            {
                result = fte_position.PositionDescription + " failed to be deleted due to dependencies.\n";
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

		public static String nextUniqueIdentityNumber()
		{
			Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
			Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
			String nextUniqueIdentityNumber = "";

			try
			{
				using (var ctx = new CPPDbContext())
				{
					nextUniqueIdentityNumber = ctx.FtePosition.Max(u => u.UniqueIdentityNumber);
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

			return nextUniqueIdentityNumber;
		}
	}
}