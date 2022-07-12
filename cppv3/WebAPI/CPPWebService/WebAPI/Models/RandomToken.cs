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
    [Table("random_token")]
    public class RandomToken
    {
        [NotMapped]
        public int Operation;
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public String Token { get; set; }
        public int UserID { get; set; }
        public DateTime ExpiredDate { get; set; }
        public DateTime DateCreate { get; set; }
        public int Active { get; set; }

        //Foreign Key

        //Navigation Property-enable lazy loading
        [ForeignKey("UserID")]
        public virtual User User{ get; set; }

        public RandomToken()
        {
            ID = 0;
            Token = "";
            UserID = 0;
            Active = 0;
        }


        public static RandomToken getTokenByUserID(int Id)
        {
            RandomToken token = new RandomToken();

            using (var ctx = new CPPDbContext())
            {
                try
                {
                    token = ctx.RandomToken.Where(r => r.UserID == Id).FirstOrDefault();

                }
                catch (Exception ex)
                {
                    var stackTrace = new StackTrace(ex, true);
                    var line = stackTrace.GetFrame(0).GetFileLineNumber();
                    Logger.LogExceptions(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, ex.Message, line.ToString(), Logger.logLevel.Exception);
  
                }
            }


            return token;
        }

        public static String updateToken(RandomToken token)
        {
            var status = "";
            var OkForUpdate = false;
            using (var ctx = new CPPDbContext())
            {
                try
                {
                    RandomToken tokenToBeUpdate = ctx.RandomToken.Where(r => r.UserID == token.ID).FirstOrDefault();
                    if (token != null)
                    {
                        OkForUpdate = true;
                    }
                    else
                    {
                        status = "Item not found";
                    }

                    if (OkForUpdate == true)
                    {
                        using (var dbCtx = new CPPDbContext())
                        {
                            try
                            {
                                tokenToBeUpdate = token;
                                dbCtx.Entry(tokenToBeUpdate).State = System.Data.Entity.EntityState.Modified;
                                dbCtx.SaveChanges();
                                status = "Success";
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
                catch (Exception ex)
                {
                    var stackTrace = new StackTrace(ex, true);
                    var line = stackTrace.GetFrame(0).GetFileLineNumber();
                    Logger.LogExceptions(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, ex.Message, line.ToString(), Logger.logLevel.Exception);

                }
            }


            return status;
        }

        public static String registerToken(RandomToken token)
        {
            var status = "";
            using (var ctx = new CPPDbContext())
            {
                try
                {
                    ctx.RandomToken.Add(token);
                    ctx.SaveChanges();
                    status = "Success";

                }
                catch (Exception ex)
                {
                    var stackTrace = new StackTrace(ex, true);
                    var line = stackTrace.GetFrame(0).GetFileLineNumber();
                    Logger.LogExceptions(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, ex.Message, line.ToString(), Logger.logLevel.Exception);

                }
            }


            return status;
        }

        public static String deleteToken(RandomToken token)
        {

            String delete_result = "";
            bool OkForDelete = false;
            using (var ctx = new CPPDbContext())
            {
                try
                {
                    // query to check existence of object in database
                    RandomToken tokenToBeDeleted = ctx.RandomToken.Where(f => f.ID == token.ID).FirstOrDefault();
                    if (tokenToBeDeleted != null)
                    {
                        OkForDelete = true;
                    }
                    else
                    {
                        delete_result += "Token " + tokenToBeDeleted.ID+ " does not exist in system";

                    }


                    // delete from database
                    if (OkForDelete)
                    {
                        ctx.RandomToken.Remove(tokenToBeDeleted);
                        ctx.SaveChanges();

                        delete_result = "Success";
                    }

                }
                catch (Exception ex)
                {
                    var stackTrace = new StackTrace(ex, true);
                    var line = stackTrace.GetFrame(0).GetFileLineNumber();
                    Logger.LogExceptions(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, ex.Message, line.ToString(), Logger.logLevel.Exception);
                    delete_result = ex.Message;
                }
                finally
                {

                }
            }
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Exit Point", Logger.logLevel.Debug);

            return delete_result;
        }

        public static User validateToken(RandomToken token)
        {
            User user = new User();
            RandomToken tokenResult = new RandomToken();

            using (var ctx = new CPPDbContext())
            {
                try
                {
                    tokenResult = ctx.RandomToken.Where(t => t.Token == token.Token).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    var stackTrace = new StackTrace(ex, true);
                    var line = stackTrace.GetFrame(0).GetFileLineNumber();
                    Logger.LogExceptions(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, ex.Message, line.ToString(), Logger.logLevel.Exception);
                }

                if (tokenResult != null && tokenResult.Active != 0 && DateTime.Compare(DateTime.Now, tokenResult.ExpiredDate) < 0)
                {
                    try
                    {
                        user = ctx.User.Where(u => u.Id == tokenResult.UserID).FirstOrDefault();
                    }
                    catch (Exception ex)
                    {
                        var stackTrace = new StackTrace(ex, true);
                        var line = stackTrace.GetFrame(0).GetFileLineNumber();
                        Logger.LogExceptions(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, ex.Message, line.ToString(), Logger.logLevel.Exception);
                    }
                }
            }

            return user;
        }
    }
}