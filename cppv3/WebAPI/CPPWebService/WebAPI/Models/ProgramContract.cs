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
	[Table("program_contract")]
	public class ProgramContract : Audit
	{
		[NotMapped]
		public int Operation;

		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ProgramContractID { get; set; }
		public int ProgramID { get; set; }
		public int ContractID { get; set; }

		public static List<ProgramContract> getProgramContract()
		{
			Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
			Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);

			List<ProgramContract> programContractList = new List<ProgramContract>();
			try
			{

				using (var ctx = new CPPDbContext())
				{
					programContractList = ctx.ProgramContract.OrderBy(a => a.ProgramID).ThenBy(b => b.ContractID).ToList();
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

			return programContractList;
		}


		public static String registerProgramContractList(ProgramContract programContract)
		{

			Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
			Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
			String result = "";
			try
			{
				using (var ctx = new CPPDbContext())
				{
					ProgramContract retreivedProgramContract = new ProgramContract();
					retreivedProgramContract = ctx.ProgramContract.Where(a => (a.ContractID == programContract.ContractID
																				  && a.ProgramID == programContract.ProgramID)).FirstOrDefault();

					if (retreivedProgramContract == null)
					{
						//register
						ctx.ProgramContract.Add(programContract);
						ctx.SaveChanges();

						Program program = ctx.Program.Where(p => p.ProgramID == programContract.ProgramID).FirstOrDefault();
						Contract contract = ctx.Contract.Where(p => p.ContractID == programContract.ContractID).FirstOrDefault();

						result += "Success," + programContract.ContractID;
					}
					else
					{
						Program program = ctx.Program.Where(p => p.ProgramID == programContract.ProgramID).FirstOrDefault();
						Contract contract = ctx.Contract.Where(p => p.ContractID == programContract.ContractID).FirstOrDefault();

						result += program.ProgramName + " - " + contract.ContractName + " failed to be created, entry must be unique. \n";
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
		public static String updateProgramContract(ProgramContract programContract)
		{
			Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
			Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
			String result = "";



			try
			{
				using (var ctx = new CPPDbContext())
				{
					ProgramContract retreivedProgramContract = new ProgramContract();
					retreivedProgramContract = ctx.ProgramContract.Where(u => u.ProgramContractID == programContract.ProgramContractID).FirstOrDefault();

					ProgramContract duplicateProgramContract = ctx.ProgramContract.Where(a => (a.ProgramContractID == programContract.ProgramContractID
																				  && a.ContractID == programContract.ContractID
																				  && a.ProgramID == programContract.ProgramID)).FirstOrDefault();

					Program program = ctx.Program.Where(p => p.ProgramID == programContract.ProgramID).FirstOrDefault();
					Contract contract = ctx.Contract.Where(p => p.ContractID == programContract.ContractID).FirstOrDefault();

					if (duplicateProgramContract != null)
					{
						result += program.ProgramName + " - " + contract.ContractName + " failed to be updated, non-unique entry will be created.\n";
					}
					else if (retreivedProgramContract != null)
					{
						CopyUtil.CopyFields<ProgramContract>(programContract, retreivedProgramContract);
						ctx.Entry(retreivedProgramContract).State = System.Data.Entity.EntityState.Modified;
						ctx.SaveChanges();

						result += program.ProgramName + " - " + contract.ContractName + " has been updated successfully.\n";
					}
					else
					{
						result += program.ProgramName + " - " + contract.ContractName + " failed to be updated, it does not exist.\n";
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
		public static String deleteProgramContract(ProgramContract programContract)
		{
			Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
			Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
			String result = "";
			String entryName = "";
			try
			{
				using (var ctx = new CPPDbContext())
				{
					ProgramContract retreivedProgramContract = new ProgramContract();
					retreivedProgramContract = ctx.ProgramContract.Where(u => u.ProgramContractID == programContract.ProgramContractID).FirstOrDefault();

					Program program = ctx.Program.Where(p => p.ProgramID == programContract.ProgramID).FirstOrDefault();
					Contract contract = ctx.Contract.Where(p => p.ContractID == programContract.ContractID).FirstOrDefault();

					entryName = program.ProgramName + " - " + contract.ContractName;

					if (retreivedProgramContract != null)
					{
						ctx.ProgramContract.Remove(retreivedProgramContract);
						ctx.SaveChanges();
						result = entryName + " has been deleted successfully.\n";
					}
					else
					{
						result = entryName + " failed to be updated, it does not exist.\n";
					}
				}

			}
			catch (Exception ex)
			{
				result = entryName + " failed to be deleted due to dependencies.\n";
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