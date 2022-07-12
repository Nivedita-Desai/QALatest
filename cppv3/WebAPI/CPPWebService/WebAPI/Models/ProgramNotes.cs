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
    [Table("program_notes")]
    public class ProgramNotes:Audit
    {
        readonly static log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int notes_id { get; set; }
        public string notes_desc { get; set; }
        public int programID { get; set; }

        public static List<ProgramNotes> getProgramNotes(int ProgramID)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            List<ProgramNotes> MatchedProgramNotesList = new List<ProgramNotes>();
            try
            {
                using (var ctx = new CPPDbContext())
                {
                     MatchedProgramNotesList = ctx.ProgramNotes.Where(a => a.programID == ProgramID).ToList();
                    //MatchedProgramList = test.Select(a => new Program { ProgramID = a.ProgramID, ProgramName = a.ProgramName }).ToList();
                }

            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return MatchedProgramNotesList;
        }
    }
    
}