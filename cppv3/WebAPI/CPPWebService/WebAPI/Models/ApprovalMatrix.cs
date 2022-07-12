using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;
using WebAPI.Controllers;
using WebAPI.Helper;

namespace WebAPI.Models
{
    [Table("approval_matrix")]
    public class ApprovalMatrix
    {
        [NotMapped]
        public int Operation;
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public String Role { get; set; }
        public decimal Cost { get; set; }
        public String Schedule { get; set; }

        ////ON DELETE RESTRICT
        //[ForeignKey("Role")]
        //public virtual UserRole UserRole { get; set; }

        //ApprovalMatrix(String position, String cost, DateTime schedule)
        //{
        //    Position = position;
        //    Cost = cost;
        //    Schedule = schedule;
        //}

        public static List<ApprovalMatrix> getApprovalMatrix()
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            List<ApprovalMatrix> approvalMatrixList = new List<ApprovalMatrix>();

            try
            {
                using (var ctx = new CPPDbContext())
                {
                    approvalMatrixList = ctx.ApprovalMatrix.OrderBy(a => a.Cost).ToList();
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
            return approvalMatrixList;
        }

        public static String registerApprovalMatrix(ApprovalMatrix approvalMatrix)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String result = "";
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    ApprovalMatrix retreivedApprovalMatrix = new ApprovalMatrix();
                    retreivedApprovalMatrix = ctx.ApprovalMatrix.Where(u => u.Role == approvalMatrix.Role).FirstOrDefault();

                    if (retreivedApprovalMatrix == null)
                    {
                        //register
                        ctx.ApprovalMatrix.Add(approvalMatrix);
                        ctx.SaveChanges();
                        result = approvalMatrix.Role + " has been created successfully.\n";
                    }
                    else
                    {
                        result += approvalMatrix.Role + "' failed to be created, it already exist.\n";
                    }
                }
            }
            catch (Exception ex)
            {
                var stackTrace = new StackTrace(ex, true);
                var line = stackTrace.GetFrame(0).GetFileLineNumber();
                Logger.LogExceptions(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, ex.Message, line.ToString(), Logger.logLevel.Exception);            }
            finally
            {
            }
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Exit Point", Logger.logLevel.Debug);

            return result;
        }

        public static String updateApprovalMatrix(ApprovalMatrix approvalMatrix)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String result = "";

            try
            {
                using (var ctx = new CPPDbContext())
                {
                    ApprovalMatrix retreivedApprovalMatrix = new ApprovalMatrix();
                    retreivedApprovalMatrix = ctx.ApprovalMatrix.Where(u => u.Id == approvalMatrix.Id).FirstOrDefault();

                    ApprovalMatrix duplicateRetreivedMatrix = new ApprovalMatrix();
                    duplicateRetreivedMatrix = ctx.ApprovalMatrix.Where(u => u.Role == approvalMatrix.Role
                                                            && u.Id != approvalMatrix.Id).FirstOrDefault();

                    if (duplicateRetreivedMatrix != null)
                    {
                        result += approvalMatrix.Role + " failed to be updated, duplicate will be created.\n";
                    }
                    else if (retreivedApprovalMatrix != null)
                    {
                        CopyUtil.CopyFields<ApprovalMatrix>(approvalMatrix, retreivedApprovalMatrix);
                        ctx.Entry(retreivedApprovalMatrix).State = System.Data.Entity.EntityState.Modified;
                        ctx.SaveChanges();
                        result = approvalMatrix.Role + " has been updated successfully.\n";
                    }
                    else
                    {
                        result += approvalMatrix.Role + " failed to be updated, it does not exist.\n";
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

        public static String deleteApprovalMatrix(ApprovalMatrix approvalMatrix)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String result = "";
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    ApprovalMatrix retreivedApprovalMatrix = new ApprovalMatrix();
                    retreivedApprovalMatrix = ctx.ApprovalMatrix.Where(u => u.Id == approvalMatrix.Id).FirstOrDefault();

                    if (retreivedApprovalMatrix != null)
                    {
                        ctx.ApprovalMatrix.Remove(retreivedApprovalMatrix);
                        ctx.SaveChanges();
                        result = approvalMatrix.Role + " has been deleted successfully.\n";
                    }
                    else
                    {
                        result = approvalMatrix.Role + " failed to be deleted, it does not exist.\n";
                    }
                }

            }
            catch (Exception ex)
            {
                result = approvalMatrix.Role + " failed to be deleted due to dependencies.\n";
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