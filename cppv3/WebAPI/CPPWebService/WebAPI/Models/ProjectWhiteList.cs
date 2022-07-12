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
	[Table("project_white_list")]
	public class ProjectWhiteList : Audit
	{
		[NotMapped]
		public int Operation;

		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ID { get; set; }
		public String UserID { get; set; }
		public String ProjectID { get; set; }
		public String EmployeeID { get; set; }


		public static List<ProjectWhiteList> getProjectWhiteList()
		{
			Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
			Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);

			List<ProjectWhiteList> projectWhiteList = new List<ProjectWhiteList>();
			try
			{

				using (var ctx = new CPPDbContext())
				{
					projectWhiteList = ctx.ProjectWhiteList.OrderBy(a => a.ID).ToList();
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

			return projectWhiteList;
		}


		public static String registerProjectWhiteList(ProjectWhiteList projectWhiteListSingle)
		{

			Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
			Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
			String result = "";
			try
			{
				using (var ctx = new CPPDbContext())
				{
					ctx.ProjectWhiteList.Add(projectWhiteListSingle);
					ctx.SaveChanges();
					result += projectWhiteListSingle.ID + " has been created successfully.\n";
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

		#region Don't need to update
		//public static String updateMaterialCategory(MaterialCategory materialCategory)
		//{
		//	Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
		//	Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
		//	String result = "";

		//	try
		//	{
		//		using (var ctx = new CPPDbContext())
		//		{
		//			MaterialCategory retrievedMaterialCategory = new MaterialCategory();
		//			retrievedMaterialCategory = ctx.MaterialCategory.Where(u => u.ID == materialCategory.ID).FirstOrDefault();

		//			MaterialCategory duplicateMaterialCategory = new MaterialCategory();
		//			duplicateMaterialCategory = ctx.MaterialCategory.Where(u => u.Name == materialCategory.Name
		//																		&& u.ID != materialCategory.ID).FirstOrDefault();

		//			if (duplicateMaterialCategory != null)
		//			{
		//				result += materialCategory.Name + " failed to be updated, duplicate will be created.\n";
		//			}
		//			else if (retrievedMaterialCategory != null)
		//			{
		//				CopyUtil.CopyFields<MaterialCategory>(materialCategory, retrievedMaterialCategory);
		//				ctx.Entry(retrievedMaterialCategory).State = System.Data.Entity.EntityState.Modified;
		//				ctx.SaveChanges();
		//				result = materialCategory.Name + " has been updated successfully.\n";
		//			}
		//			else
		//			{
		//				result = materialCategory.Name + " failed to be updated, it does not exist.\n";
		//			}
		//		}

		//	}
		//	catch (Exception ex)
		//	{
		//		var stackTrace = new StackTrace(ex, true);
		//		var line = stackTrace.GetFrame(0).GetFileLineNumber();
		//		Logger.LogExceptions(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, ex.Message, line.ToString(), Logger.logLevel.Exception);
		//	}
		//	finally
		//	{

		//	}
		//	Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Exit Point", Logger.logLevel.Debug);

		//	return result;

		//}
		#endregion

		public static String deleteProjectWhiteList(String ProjectID)
		{
			Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
			Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
			String result = "";
			try
			{

				using (var ctx = new CPPDbContext())
				{
					List<ProjectWhiteList> retrievedProjectWhiteList = new List<ProjectWhiteList>();
                    retrievedProjectWhiteList = ctx.ProjectWhiteList.Where(u => u.ProjectID == ProjectID).ToList();

					if (retrievedProjectWhiteList != null)
					{
                        for (int x = 0; x < retrievedProjectWhiteList.Count; x++)
                        {
                            ctx.ProjectWhiteList.Remove(retrievedProjectWhiteList[x]);
                        }
						ctx.SaveChanges();
						result = "successful";
					}
					else
					{
						result = " failed to be deleted, it does not exist.\n";
					}
				}

			}
			catch (Exception ex)
			{
				result = " failed to be deleted due to dependencies.\n";
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
					nextUniqueIdentityNumber = ctx.MaterialCategory.Max(u => u.UniqueIdentityNumber);
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