using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    [Table("versionmaster")]
    public class Versionmaster : Audit
    {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int? OrganizationID { get; set; }

        public String description { get; set; }

        public static List<Versionmaster> getProjectVersionDetails(string ProgramElementID)
        {
            List<Versionmaster> versionDetails = new List<Versionmaster>();
            using (var ctx = new CPPDbContext())
            {
                int elementID = Convert.ToInt32(ProgramElementID);
                Project projectDetails = ctx.Project.Where(x => x.ProjectID == elementID).FirstOrDefault();
                int versionID = Convert.ToInt32(projectDetails.VersionId);
                versionDetails = ctx.VersionMaster.Where(x => x.Id == versionID).ToList();
            }

            return versionDetails;
        }

        public static List<Versionmaster> getVersionDetailsByOrgID(string organizationID)
        {
            List<Versionmaster> versionDetails = new List<Versionmaster>();
            using (var ctx = new CPPDbContext())
            {
                int orgID = Convert.ToInt32(organizationID);
                versionDetails = ctx.VersionMaster.Where(x => x.OrganizationID == orgID).OrderByDescending(x => x.Id).ToList();
            }
                return versionDetails;
        }

        public static List<Versionmaster> getVersionDetails()
        {
            List<Versionmaster> versionDetails = new List<Versionmaster>();
            using (var ctx = new CPPDbContext())
            {
                
                versionDetails = ctx.VersionMaster.OrderByDescending(x => x.Id).ToList();
            }
            return versionDetails;
        }

    }

    
}