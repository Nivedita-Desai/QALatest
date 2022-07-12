using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using WebAPI.Helper;
using System.Diagnostics;

namespace WebAPI.Models
{
    [Table("prelimnary_notice")]
    public class PrelimnaryNotice : Audit
	{
		readonly static log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]

		[NotMapped]
        public int Operation;
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Reason { get; set; }
        public int ProgramID { get; set; }

		public static List<PrelimnaryNotice> GetPrelimnaryNoticeList(int programId)
		{
			Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
			Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
			List<PrelimnaryNotice> prelimnaryNoticesList = new List<PrelimnaryNotice>();

			Program programid = new Program();
			try
			{
				using (var ctx = new CPPDbContext())
				{

					prelimnaryNoticesList = ctx.PrelimnaryNotices.Where(u => u.ProgramID == programId).ToList();


					return prelimnaryNoticesList;

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
			return prelimnaryNoticesList;
		}

		public static void SaveNotice(PrelimnaryNotice prelimnaryNotice)
		{

			Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
			Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
			List<PrelimnaryNotice> PrelimnaryNotice = new List<PrelimnaryNotice>();

			try
			{
				using (var ctx = new CPPDbContext())
				{
					ctx.PrelimnaryNotices.Add(prelimnaryNotice);
					ctx.SaveChanges();
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
		}

		public static void UpdateNotice(PrelimnaryNotice prelimnaryNotice)
		{

			Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
			Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
			String result = "";

			try
			{
				using (var ctx = new CPPDbContext())
				{
					PrelimnaryNotice retreivedNotice = new PrelimnaryNotice();
					retreivedNotice = ctx.PrelimnaryNotices.Where(p => p.Id == prelimnaryNotice.Id).FirstOrDefault();

					if (retreivedNotice != null)
					{
						CopyUtil.CopyFields<PrelimnaryNotice>(prelimnaryNotice, retreivedNotice);
						ctx.Entry(retreivedNotice).State = System.Data.Entity.EntityState.Modified;
						ctx.SaveChanges();
						result = prelimnaryNotice.Date + " has been updated successfully.\n";
					}
					else
					{
						result += prelimnaryNotice.Date + " failed to be updated, it does not exist.\n";
					}
				}
			}
			catch (Exception ex)
			{
				var stackTrace = new StackTrace(ex, true);
				var line = stackTrace.GetFrame(0).GetFileLineNumber();
				Logger.LogExceptions(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, ex.Message, line.ToString(), Logger.logLevel.Exception);
			}
		}

		
    }
}