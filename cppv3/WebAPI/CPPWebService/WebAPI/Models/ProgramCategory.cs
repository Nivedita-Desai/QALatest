using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Web;

namespace WebAPI.Models
{
   
        [Table("program_category")]
        public class ProgramCategory
        {
            [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int Id { get; set; }
            public int ProgramID { get; set; }
            public int ActivityCategoryID { get; set; }

            [NotMapped]
            public int Operation { get; set; }
            public virtual ActivityCategory ActivityCategory{ get; set; }
            public virtual Program Program{ get; set; }

            public static string registerProgramCategory(ProgramCategory programCategory)
            {
                var status = "";
                List<ProgramFund> programFundList = new List<ProgramFund>();
                Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
                Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
                try
                {
                    using (var ctx = new CPPDbContext())
                    {

                        //Temporary fix for Getting FundTypeID, may be retrieve fundTypeID on front-end in the future
                        if (programCategory.ActivityCategory != null)
                            programCategory.ActivityCategory = null;
                        if (programCategory.Program != null)
                            programCategory.Program = null;

                        var retrievedProgramCategory = ctx.ProgramCategory.Where(c => c.Id == programCategory.Id).FirstOrDefault();
                        if (retrievedProgramCategory == null)
                        {
                            //add
                            //programCategory.Id = null;
                            ctx.ProgramCategory.Add(programCategory);
                            ctx.SaveChanges();
                            status = "Success";

                        }
                        else
                        {
                            status = "Item already Exist in the table";
                        }
                    }
                }
                catch (Exception ex)
                {
                    var stackTrace = new StackTrace(ex, true);
                    var line = stackTrace.GetFrame(0).GetFileLineNumber();
                    Logger.LogExceptions(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, ex.Message, line.ToString(), Logger.logLevel.Exception);

                }
                return status;
            }

            public static string updateProgramCategory(ProgramCategory programCategory)
            {
                var status = "";
                List<ProgramFund> programFundList = new List<ProgramFund>();
                Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
                Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
                try
                {
                    using (var ctx = new CPPDbContext())
                    {

                        //Temporary fix for Getting FundTypeID, may be retrieve fundTypeID on front-end in the future
                        if (programCategory.ActivityCategory != null)
                            programCategory.ActivityCategory = null;
                        if (programCategory.Program != null)
                            programCategory.Program = null;

                        var retrievedProgramCategory = ctx.ProgramCategory.Where(c => c.Id == programCategory.Id).FirstOrDefault();
                        if (retrievedProgramCategory != null)
                        {
                            //add
                            ctx.ProgramCategory.Add(programCategory);
                            ctx.SaveChanges();
                            status = "Success";

                        }
                        else
                        {
                            status = "Item does not Exist in the table";
                        }
                    }
                }
                catch (Exception ex)
                {
                    var stackTrace = new StackTrace(ex, true);
                    var line = stackTrace.GetFrame(0).GetFileLineNumber();
                    Logger.LogExceptions(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, ex.Message, line.ToString(), Logger.logLevel.Exception);

                }
                return status;
            }

            public static string deleteProgramCategory(ProgramCategory programCategory)
            {
                var status = "";
                List<ProgramFund> programFundList = new List<ProgramFund>();
                Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
                Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
                try
                {
                    using (var ctx = new CPPDbContext())
                    {

                        //Temporary fix for Getting FundTypeID, may be retrieve fundTypeID on front-end in the future
                        if (programCategory.ActivityCategory != null)
                            programCategory.ActivityCategory = null;
                        if (programCategory.Program != null)
                            programCategory.Program = null;

                        var retrievedProgramCategory = ctx.ProgramCategory.Where(c => c.Id == programCategory.Id).FirstOrDefault();
                        if (retrievedProgramCategory != null)
                        {
                            //add
                            ctx.ProgramCategory.Remove(retrievedProgramCategory);
                            ctx.SaveChanges();
                            status = "Success";

                        }
                        else
                        {
                            status = "Item does not Exist in the table";
                        }
                    }
                }
                catch (Exception ex)
                {
                    var stackTrace = new StackTrace(ex, true);
                    var line = stackTrace.GetFrame(0).GetFileLineNumber();
                    Logger.LogExceptions(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, ex.Message, line.ToString(), Logger.logLevel.Exception);

                }
                return status;
            }
        }
    
}