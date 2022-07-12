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
	[Table("contract")]
	public class Contract : Audit
	{
		[NotMapped]
		public int Operation;

		[NotMapped]
		public int ProgramID;

		[NotMapped]
		public int DocumentID;

		[NotMapped]
		public String DocumentName;

		[NotMapped]
		public Document DocumentDraft;

		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ContractID { get; set; }
		public String ContractName { get; set; }
		public String ContractNumber { get; set; }
		public String ContractStartDate { get; set; }
		public String ContractEndDate { get; set; }
		public int ProjectClassID { get; set; }

		[ForeignKey("ProjectClassID")]
		public virtual ProjectClass ProjectClass { get; set; }


		public static List<Contract> getContract()
		{
			Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
			Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);

			List<Contract> contractList = new List<Contract>();
			try
			{

				using (var ctx = new CPPDbContext())
				{
					contractList = ctx.Contract.OrderBy(a => a.ContractName).ToList();
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

			return contractList;
		}


		public static String registerContract(Contract contract)
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
							Contract retreivedContract = new Contract();
							ProgramContract newProgramContract = new ProgramContract();
							Document transferDocument = new Document();

							newProgramContract.ProgramID = contract.ProgramID;

							retreivedContract = ctx.Contract.Where(u => u.ContractName == contract.ContractName).FirstOrDefault();

							if (retreivedContract == null)
							{
								//register
								ctx.Contract.Add(contract);
								ctx.SaveChanges();

								newProgramContract.ContractID = contract.ContractID;
								ProgramContract.registerProgramContractList(newProgramContract);

								if (contract.DocumentID > 0)
								{
									transferDocument = ctx.Document.Where(u => u.DocumentID == contract.DocumentID).FirstOrDefault();
									transferDocument.ContractID = contract.ContractID;
									ctx.Entry(transferDocument).State = System.Data.Entity.EntityState.Modified;
								}
								else {
									Document.registerDocument(contract.DocumentDraft);
								}

								ctx.SaveChanges();

								result = "Success," + contract.ContractID;
							}
							else
							{
								result += contract.ContractName + "' failed to be created, it already exist.\n";
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
		public static String updateContract(Contract contract)
		{
			Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
			Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
			String result = "";

			try
			{
				using (var ctx = new CPPDbContext())
				{
					Contract retreivedContract = new Contract();
					retreivedContract = ctx.Contract.Where(u => u.ContractID == contract.ContractID).FirstOrDefault();

					Contract duplicateContract = new Contract();
					duplicateContract = ctx.Contract.Where(u => u.ContractName == contract.ContractName
																		&& u.ContractID != contract.ContractID).FirstOrDefault();

					if (duplicateContract != null)
					{
						result = contract.ContractName + " failed to be updated, duplicate will be created.\n";
					}
					else if (retreivedContract != null)
					{
						CopyUtil.CopyFields<Contract>(contract, retreivedContract);
						ctx.Entry(retreivedContract).State = System.Data.Entity.EntityState.Modified;
						ctx.SaveChanges();
						result = contract.ContractName + " has been updated successfully.\n";
					}
					else
					{
						result += contract.ContractName + " failed to be updated, it does not exist.\n";
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
		public static String deleteContract(Contract contract)
		{
			Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
			Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
			String result = "";
			try
			{
				using (var ctx = new CPPDbContext())
				{
					Contract retreivedContract = new Contract();
					retreivedContract = ctx.Contract.Where(u => u.ContractID == contract.ContractID).FirstOrDefault();
					ProgramContract retreivedProgramContract = new ProgramContract();
					retreivedProgramContract = ctx.ProgramContract.Where(u => u.ContractID == contract.ContractID && u.ProgramID == contract.ProgramID).FirstOrDefault();

					if (retreivedContract != null)
					{
						ctx.Contract.Remove(retreivedContract);
						ctx.ProgramContract.Remove(retreivedProgramContract);
						ctx.SaveChanges();

						result = contract.ContractName + " has been deleted successfully.\n";
					}
					else
					{
						result = contract.ContractName + " failed to be deleted, it does not exist.\n";
					}
				}

			}
			catch (Exception ex)
			{
				result += contract.ContractName + " failed to be deleted due to dependencies.\n";
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