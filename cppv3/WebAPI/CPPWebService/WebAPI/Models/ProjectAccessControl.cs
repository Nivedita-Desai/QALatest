using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Web;

namespace WebAPI.Models
{
    [Table("project_access_control")]
    public class ProjectAccessControl
    {
        public int Id { get; set; }
        public int UserId { get; set; }
		public int ProgramId { get; set; }
		public bool IsProgramCreator { get; set; }
		public int ProgramElementID { get; set; }
		public bool IsProgramEleCreator { get; set; }
		public bool IsPrgmEleApprover { get; set; }
		public int ProjectID { get; set; }
		public bool IsProjectCreator { get; set; }
		public bool IsProjectApprover { get; set; }
		public bool IsAllowedUser { get; set; }


		public static List<ProjectAccessControl> GetContractModificationList(int Id)
		{
			Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
			Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
			List<ProjectAccessControl> projectAccessControlList = new List<ProjectAccessControl>();
			
			try
			{
				using (var ctx = new CPPDbContext())
				{
					projectAccessControlList = ctx.ProjectAccessControl.Where(x => x.UserId == Id).ToList();

					return projectAccessControlList;
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
			return projectAccessControlList;
		}


		public static List<Int32> GetAllowedUserList(int ProjectID)
		{
			Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
			Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
			List<Int32> projectAccessControlList = new List<Int32>();

			try
			{
				using (var ctx = new CPPDbContext())
				{
					projectAccessControlList = ctx.ProjectAccessControl.Where(x => x.ProjectID == ProjectID && x.IsAllowedUser == true).Select(p => p.UserId).ToList();

					//return projectAccessControlList;
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
			return projectAccessControlList;
		}


	}

}