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
    [Table("program_warranty")]
    public class ProgramWarranty:Audit
    {
        readonly static log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        [NotMapped]
        public int Operation;

        public int Id { get; set; }

        public String WarrantyType { get; set; }

        public int ProgramID { get; set; }
        //public String WarrantyDescription { get; set; }

        public String Description { get; set; }
        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public static string registerProgramWarranty(ProgramWarranty programWarranty)
        {
            var status = "";
            List<ProgramFund> programFundList = new List<ProgramFund>();
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    //ProgramWarranty laborWarranty = new ProgramWarranty();
                    //ProgramWarranty materialsWarranty = new ProgramWarranty();
                    //ProgramWarranty otherWarranty = new ProgramWarranty();
                    
                    //laborWarranty.WarrantyType = pgm.LaborWarrantyList.WarrantyType;
                    //laborWarranty.Description = pgm.LaborWarrantyList.Description;
                    //laborWarranty.ProgramID = pgm.ProgramID;
                    //laborWarranty.WarrantyDescription = pgm.LaborWarrantyList.WarrantyDescription;
                    //laborWarranty.StartDate = pgm.LaborWarrantyList.StartDate;
                    //laborWarranty.EndDate = pgm.LaborWarrantyList.EndDate;

                    //ctx.ProgramWarranty.Add(laborWarranty);
                    //    ctx.SaveChanges();

                    //materialsWarranty.WarrantyType = pgm.MaterialsWarrantyList.WarrantyType;
                    //materialsWarranty.Description = pgm.MaterialsWarrantyList.Description;
                    //materialsWarranty.ProgramID = pgm.ProgramID;
                    //materialsWarranty.WarrantyDescription = pgm.MaterialsWarrantyList.WarrantyDescription;
                    //materialsWarranty.StartDate = pgm.MaterialsWarrantyList.StartDate;
                    //materialsWarranty.EndDate = pgm.MaterialsWarrantyList.EndDate;

                    //ctx.ProgramWarranty.Add(materialsWarranty);
                    //ctx.SaveChanges();

                    //otherWarranty.WarrantyType = pgm.OtherWarrantyList.WarrantyType;
                    //otherWarranty.Description = pgm.OtherWarrantyList.Description;
                    //otherWarranty.ProgramID = pgm.ProgramID;
                    //otherWarranty.WarrantyDescription = pgm.OtherWarrantyList.WarrantyDescription;
                    //otherWarranty.StartDate = pgm.OtherWarrantyList.StartDate;
                    //otherWarranty.EndDate = pgm.OtherWarrantyList.EndDate;

                    //ctx.ProgramWarranty.Add(otherWarranty);
                    //ctx.SaveChanges(); 
                    
                    ctx.ProgramWarranty.Add(programWarranty);
                    ctx.SaveChanges();


                    status = "Success";
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

        public static string updateProgramWarranty(ProgramWarranty programWarranty)
        {
            var status = "";
            List<ProgramFund> programFundList = new List<ProgramFund>();
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String result = "";

            try
            {
                using (var ctx = new CPPDbContext())
                {
                    ProgramWarranty retreivedWarranty = new ProgramWarranty();
                    retreivedWarranty = ctx.ProgramWarranty.Where(p => p.Id == programWarranty.Id).FirstOrDefault();

                    if (retreivedWarranty != null)
                    {
                        CopyUtil.CopyFields<ProgramWarranty>(programWarranty, retreivedWarranty);
                        ctx.Entry(retreivedWarranty).State = System.Data.Entity.EntityState.Modified;
                        ctx.SaveChanges();
                        result = programWarranty.WarrantyType + " has been updated successfully.\n";
                    }
                    else
                    {
                        result += programWarranty.WarrantyType + " failed to be updated, it does not exist.\n";
                    }

                    //ProgramWarranty laborWarranty = new ProgramWarranty();
                    //ProgramWarranty materialsWarranty = new ProgramWarranty();
                    //ProgramWarranty otherWarranty = new ProgramWarranty();

                    //ProgramWarranty retrievedLaborWarranty = ctx.ProgramWarranty.Where(l => l.ProgramID == program.ProgramID && l.WarrantyType == "Labor Warranty").FirstOrDefault();
                    //ProgramWarranty retrievedMaterialsWarranty = ctx.ProgramWarranty.Where(l => l.ProgramID == program.ProgramID && l.WarrantyType == "Materials Warranty").FirstOrDefault();
                    //ProgramWarranty retrievedOthersWarranty = ctx.ProgramWarranty.Where(l => l.ProgramID == program.ProgramID && l.WarrantyType == "Other Warranty").FirstOrDefault();
                    //if (retrievedLaborWarranty != null)
                    //{
                    //    retrievedLaborWarranty.WarrantyType = program.LaborWarrantyList.WarrantyType;
                    //    retrievedLaborWarranty.Description = program.LaborWarrantyList.Description;
                    //    retrievedLaborWarranty.ProgramID = program.ProgramID;
                    //    retrievedLaborWarranty.WarrantyDescription = program.LaborWarrantyList.WarrantyDescription;
                    //    retrievedLaborWarranty.StartDate = program.LaborWarrantyList.StartDate;
                    //    retrievedLaborWarranty.EndDate = program.LaborWarrantyList.EndDate;
                    //    ctx.SaveChanges();

                    //}
                    //else
                    //{
                    //    laborWarranty.WarrantyType = program.LaborWarrantyList.WarrantyType;
                    //    laborWarranty.Description = program.LaborWarrantyList.Description;
                    //    laborWarranty.ProgramID = program.ProgramID;
                    //    laborWarranty.WarrantyDescription = program.LaborWarrantyList.WarrantyDescription;
                    //    laborWarranty.StartDate = program.LaborWarrantyList.StartDate;
                    //    laborWarranty.EndDate = program.LaborWarrantyList.EndDate;

                    //    ctx.ProgramWarranty.Add(laborWarranty);
                    //    ctx.SaveChanges();
                    //}

                    //if (retrievedMaterialsWarranty != null)
                    //{

                    //    retrievedMaterialsWarranty.WarrantyType = program.MaterialsWarrantyList.WarrantyType;
                    //    retrievedMaterialsWarranty.Description = program.MaterialsWarrantyList.Description;
                    //    retrievedMaterialsWarranty.ProgramID = program.ProgramID;
                    //    retrievedMaterialsWarranty.WarrantyDescription = program.MaterialsWarrantyList.WarrantyDescription;
                    //    retrievedMaterialsWarranty.StartDate = program.MaterialsWarrantyList.StartDate;
                    //    retrievedMaterialsWarranty.EndDate = program.MaterialsWarrantyList.EndDate;

                    //    ctx.SaveChanges();
                    //}
                    //else
                    //{
                    //    materialsWarranty.WarrantyType = program.MaterialsWarrantyList.WarrantyType;
                    //    materialsWarranty.Description = program.MaterialsWarrantyList.Description;
                    //    materialsWarranty.ProgramID = program.ProgramID;
                    //    materialsWarranty.WarrantyDescription = program.MaterialsWarrantyList.WarrantyDescription;
                    //    materialsWarranty.StartDate = program.MaterialsWarrantyList.StartDate;
                    //    materialsWarranty.EndDate = program.MaterialsWarrantyList.EndDate;

                    //    ctx.ProgramWarranty.Add(materialsWarranty);
                    //    ctx.SaveChanges();

                    //}

                    //if (retrievedOthersWarranty != null)
                    //{
                    //    retrievedOthersWarranty.WarrantyType = program.OtherWarrantyList.WarrantyType;
                    //    retrievedOthersWarranty.Description = program.OtherWarrantyList.Description;
                    //    retrievedOthersWarranty.ProgramID = program.ProgramID;
                    //    retrievedOthersWarranty.WarrantyDescription = program.OtherWarrantyList.WarrantyDescription;
                    //    retrievedOthersWarranty.StartDate = program.OtherWarrantyList.StartDate;
                    //    retrievedOthersWarranty.EndDate = program.OtherWarrantyList.EndDate;

                    //    ctx.SaveChanges();

                    //}
                    //else
                    //{
                    //    otherWarranty.WarrantyType = program.OtherWarrantyList.WarrantyType;
                    //    otherWarranty.Description = program.OtherWarrantyList.Description;
                    //    otherWarranty.ProgramID = program.ProgramID;
                    //    otherWarranty.WarrantyDescription = program.OtherWarrantyList.WarrantyDescription;
                    //    otherWarranty.StartDate = program.OtherWarrantyList.StartDate;
                    //    otherWarranty.EndDate = program.OtherWarrantyList.EndDate;

                    //    ctx.ProgramWarranty.Add(otherWarranty);
                    //    ctx.SaveChanges();
                    //}

                    //status = "Success";
                }
            }
            catch (Exception ex)
            {
                var stackTrace = new StackTrace(ex, true);
                var line = stackTrace.GetFrame(0).GetFileLineNumber();
                Logger.LogExceptions(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, ex.Message, line.ToString(), Logger.logLevel.Exception);

            }
            return result;
            //return status;
        }

        public static List<ProgramWarranty> GetProgramWarrantyList(int programId)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            List<ProgramWarranty> programWarrantyList = new List<ProgramWarranty>();

            Program programid = new Program();
            try
            {
                using (var ctx = new CPPDbContext())
                {

                    programWarrantyList = ctx.ProgramWarranty.Where(u => u.ProgramID == programId).ToList();


                    return programWarrantyList;

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
            return programWarrantyList;
        }
    }

}