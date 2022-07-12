using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Web;
using WebAPI.Helper;

namespace WebAPI.Models
{
	[Table("milestone")]
	public class Milestone : Audit
	{
		[NotMapped]
		public int Operation;

		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int MilestoneID { get; set; }
		public String MilestoneName { get; set; }
		public String MilestoneDescription { get; set; }
		public String MilestoneDate { get; set; }
		public int? ProgramElementID { get; set; }
		public int? ProjectID { get; set; }

		public static List<Milestone> getMilestone()
		{
			Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
			Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);

			List<Milestone> MilestoneList = new List<Milestone>();
			try
			{

				using (var ctx = new CPPDbContext())
				{
					MilestoneList = ctx.Milestone.OrderBy(a => a.MilestoneDate).ToList();
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

			return MilestoneList;
		}


		public static String registerMilestone(Milestone Milestone)
		{

			Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
			Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
			String result = "";
			try
			{
				using (var ctx = new CPPDbContext())
				{
					using (var dbContextTransaction = ctx.Database.BeginTransaction())
					{
						try
						{
							Milestone retreivedMilestone = new Milestone();
							
							retreivedMilestone = ctx.Milestone.Where(u => u.MilestoneName == Milestone.MilestoneName && u.MilestoneID == Milestone.MilestoneID).FirstOrDefault();

							if (retreivedMilestone == null)
							{
								//register
								if(Milestone.ProgramElementID == 0) 
								{
									Milestone.ProgramElementID = null;
								}
								if (Milestone.ProjectID == 0)
								{
									Milestone.ProjectID = null;
								}
								ctx.Milestone.Add(Milestone);
								ctx.SaveChanges();

								result = "Success," + Milestone.MilestoneID;
							}
							else
							{
								result += Milestone.MilestoneName + "' failed to be created, it already exist.\n";
							}

							dbContextTransaction.Commit();
						}
						catch (Exception ex)
						{
							//Log, handle or absorbe I don't care ^_^
						}
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
		public static String updateMilestone(Milestone Milestone)
		{
			Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
			Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
			String result = "";

			try
			{
				using (var ctx = new CPPDbContext())
				{
					Milestone retreivedMilestone = new Milestone();
					retreivedMilestone = ctx.Milestone.Where(u => u.MilestoneID == Milestone.MilestoneID).FirstOrDefault();

					Milestone duplicateMilestone = new Milestone();
					duplicateMilestone = ctx.Milestone.Where(u => u.MilestoneName == Milestone.MilestoneName
																		&& u.MilestoneID != Milestone.MilestoneID).FirstOrDefault();

					if (false)   //duplicateMilestone != null
					{
						result = Milestone.MilestoneName + " failed to be updated, duplicate will be created.\n";
					}
					else if (retreivedMilestone != null)
					{
						CopyUtil.CopyFields<Milestone>(Milestone, retreivedMilestone);
						if (retreivedMilestone.ProgramElementID == 0)
						{
							retreivedMilestone.ProgramElementID = null;
						}
						if (retreivedMilestone.ProjectID == 0)
						{
							retreivedMilestone.ProjectID = null;
						}
						ctx.Entry(retreivedMilestone).State = System.Data.Entity.EntityState.Modified;
						ctx.SaveChanges();
						result = Milestone.MilestoneName + " has been updated successfully.\n";
					}
					else
					{
						result += Milestone.MilestoneName + " failed to be updated, it does not exist.\n";
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
		public static String deleteMilestone(Milestone Milestone)
		{
			Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
			Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
			String result = "";
			try
			{
				using (var ctx = new CPPDbContext())
				{
					Milestone retreivedMilestone = new Milestone();
					retreivedMilestone = ctx.Milestone.Where(u => u.MilestoneID == Milestone.MilestoneID).FirstOrDefault();

					if (retreivedMilestone != null)
					{
						ctx.Milestone.Remove(retreivedMilestone);
						ctx.SaveChanges();

						result = Milestone.MilestoneName + " has been deleted successfully.\n";
					}
					else
					{
						result = Milestone.MilestoneName + " failed to be deleted, it does not exist.\n";
					}
				}

			}
			catch (Exception ex)
			{
				result += Milestone.MilestoneName + " failed to be deleted due to dependencies.\n";
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