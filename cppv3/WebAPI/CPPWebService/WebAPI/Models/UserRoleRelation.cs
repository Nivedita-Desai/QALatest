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
using System.Security.Cryptography;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WebAPI.Helper;
namespace WebAPI.Models
{
    [Table("user_role_relation")]
    public class UserRoleRelation : Audit
    {
        readonly static log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int UserId { get; set; }
        public int UserRoleId { get; set; }

        public UserRoleRelation() { }

        //[NotMapped]
        //public bool isSelected { get; set; }

        //public static List<UserRoleRelation> getUserRoleRelation()
        //{
        //    Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
        //    Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
        //    List<UserRoleRelation> userRoleRelation = new List<UserRoleRelation>();

        //    try
        //    {
        //        using (var ctx = new CPPDbContext())
        //        {
        //            userRoleRelation = ctx.UserRoleRelation.ToList();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var stackTrace = new StackTrace(ex, true);
        //        var line = stackTrace.GetFrame(0).GetFileLineNumber();
        //        Logger.LogExceptions(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, ex.Message, line.ToString(), Logger.logLevel.Exception);
        //    }
        //    finally
        //    {
        //    }
        //    return userRoleRelation;
        //}



    }
}