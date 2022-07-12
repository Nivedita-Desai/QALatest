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
using System.Collections;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WebAPI.Helper;
using Newtonsoft.Json;

namespace WebAPI.Models
{
    [Table("organization")]
    public class Organization : Audit
    {

        readonly static log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        [NotMapped]
        public int Operation;
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int OrganizationID { get; set; }
        public String OrganizationName { get; set; }
        public String ShapeType { get; set; }
        public String OrganizationAddress { get; set; }
        public String GeocodedLocation { get; set; }
        public String RGBValue { get; set; }
        public String LatLong { get; set; }
        public String Note { get; set; }


        public static void copy(Organization orgOrganization, Organization newOrganization)
        {
            orgOrganization.OrganizationName = newOrganization.OrganizationName;
            orgOrganization.ShapeType = newOrganization.ShapeType;
            orgOrganization.OrganizationAddress = newOrganization.OrganizationAddress;
            orgOrganization.GeocodedLocation = newOrganization.GeocodedLocation;
            orgOrganization.RGBValue = newOrganization.RGBValue;
            orgOrganization.LatLong = newOrganization.LatLong;
            orgOrganization.Note = newOrganization.Note;
        }
        //Audit fields
        //public String CreatedBy { get; set; }
        //public String UpdatedBy { get; set; }
        //public DateTime CreatedDate { get; set; }
        //public DateTime UpdatedDate { get; set; }

        //remove for now
        //Organization(String org_id, String org_name, String shapeType, String org_addr, String geocode, String rbgVal, String latLong, String note )
        //{ OrganizationID = org_id; OrganizationName = org_name; ShapeType = shapeType; OrganizationAddress = org_addr; GeocodedLocation = geocode; RGBValue = rbgVal; LatLong = latLong; Note = note; }
        //Organization() { }

        //From RequestProjectController
        public static List<Organization> getOrganization(String OrganizationID, String KeyStroke )
        {

            List<Organization> MatchedOrganizationList = new List<Organization>();
            String guid = Guid.NewGuid().ToString();
            try
            {    
                using (var ctx = new CPPDbContext())
                {
                    Organization RetrievedOrangization = new Organization();
                    if (OrganizationID != "null")
                    {
                        //Debug
                        logger.Debug(
                            string.Format("Getting organization details for OrganizationID {0}",
                            JsonConvert.SerializeObject(new{
                               SessionID = guid,
                               Params = new { OrganizationID = OrganizationID, KeyStroke = KeyStroke },
                               RequestedUser = UserUtil.getCurrentUserName(),
                               Result = ""
                            })
                        ));

                        var id = Convert.ToInt16(OrganizationID);
                        MatchedOrganizationList = ctx.Organization.Where(o => o.OrganizationID == id).ToList();
                    }
                    else
                    {
                        //Debug
                        logger.Debug(
                            string.Format("Getting all organizations. {0}",
                            JsonConvert.SerializeObject(new
                            {
                                SessionID = guid,
                                Params = new { OrganizationID = OrganizationID, KeyStroke = KeyStroke },
                                RequestedUser = UserUtil.getCurrentUserName(),
                                Result = ""
                            })
                        ));
                        MatchedOrganizationList = ctx.Organization.OrderBy(or => or.OrganizationID).ToList();
                    }

                    //Debug
                    logger.Debug(string.Format("Retreived Organization List {0}",
                            JsonConvert.SerializeObject(new
                            {
                                SessionID = guid,
                                Params = new { OrganizationID = OrganizationID, KeyStroke = KeyStroke },
                                RequestedUser = UserUtil.getCurrentUserName(),
                                Result = MatchedOrganizationList
                            })
                        ));
                }
            }
            catch (Exception ex)
            {
                logger.Error(
                           string.Format("Unable to retrieve Organization[s] . {0}",
                           JsonConvert.SerializeObject(new
                           {
                               SessionID = guid,
                               Params = new { OrganizationID = OrganizationID, KeyStroke = KeyStroke },
                               RequestedUser = UserUtil.getCurrentUserName(),
                               Result = "",
                               Exception = ex.ToString()
                           })
                       ));
            }
          
            return MatchedOrganizationList;

        }


        public static String registerOrganization(Organization organization, String userName)
        {
            String register_result = "";
            String guid = Guid.NewGuid().ToString();
            try
            {
                // create and open a connection object
                using (var ctx = new CPPDbContext())
                {
                    Organization org = new Organization();
                    organization.LatLong = organization.LatLong.Trim();
                    org = ctx.Organization.Where(o => o.OrganizationName == organization.OrganizationName).FirstOrDefault();

                    if (org != null)
                    {
                        register_result += "Organization '" + organization.OrganizationName + "' already exists in system";
                        logger.Info(string.Format("Organization already exists in the system  {0}",
                                                    JsonConvert.SerializeObject(new
                                                    {
                                                        SessionID = guid,
                                                        Params = new { organization = organization, userName = userName },
                                                        RequestedUser = UserUtil.getCurrentUserName(),
                                                        Result = organization
                                                    })));
                    }
                    else
                    {
                        //register
                        ctx.Organization.Add(organization);
                        ctx.SaveChanges();

                        org = ctx.Organization.Where(o => o.OrganizationName == organization.OrganizationName).FirstOrDefault();
                        Versionmaster CreatedNewVersion = new Versionmaster();
                        CreatedNewVersion.CreatedDate = DateTime.Now;
                        CreatedNewVersion.UpdatedDate = DateTime.Now;
                        CreatedNewVersion.description = "V1";
                        CreatedNewVersion.OrganizationID = org.OrganizationID;
                        ctx.VersionMaster.Add(CreatedNewVersion);
                        ctx.SaveChanges();

                        logger.Info(string.Format("New Organization Created {0}",
                                                       JsonConvert.SerializeObject(new
                                                       {
                                                           SessionID = guid,
                                                           Params = new { organization = organization, userName = userName },
                                                           RequestedUser = UserUtil.getCurrentUserName(),
                                                           Result = organization
                                                       })));

                        register_result = organization.OrganizationName + " has been created successfully.\n";
                    }
                }          

            }
            catch (Exception ex)
            {
                logger.Error(string.Format("Unable to create new organization {0}",
                                                   JsonConvert.SerializeObject(new
                                                   {
                                                       SessionID = guid,
                                                       Params = new { organization = organization, userName = userName },
                                                       RequestedUser = UserUtil.getCurrentUserName(),
                                                       data = organization,
                                                       Exception = ex.ToString()
                                                   })));
                register_result = string.Format("Unable to create organization {0}", organization.OrganizationName);
            }
        

            return register_result;
        }


        public static String updateOrganization(Organization organization,String userName)
        {

            String update_result = "";
            String guid = Guid.NewGuid().ToString();
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    Organization org = new Organization();
                    organization.LatLong = organization.LatLong.Trim();
                    org = ctx.Organization.Where(o => o.OrganizationID == organization.OrganizationID).FirstOrDefault();

                    Organization duplicateOrg = new Organization();
                    duplicateOrg = ctx.Organization.Where(u => u.OrganizationName == organization.OrganizationName
                                                                        && u.OrganizationID != organization.OrganizationID).FirstOrDefault();

                    if (duplicateOrg != null)
                    {
                        update_result = organization.OrganizationName + " failed to be updated, duplicate will be created.\n";
                        logger.Info(string.Format(update_result + "{0}",
                                                    JsonConvert.SerializeObject(new
                                                    {
                                                        SessionID = guid,
                                                        Params = new { organization = organization },
                                                        RequestedUser = UserUtil.getCurrentUserName(),
                                                        Result = ""
                                                    })));
                    }
                    else if (org != null)
                    {
                        //update
                        using (var dbCtx = new CPPDbContext())
                        {
                            //Organization.copy(org, organization);
                            CopyUtil.CopyFields<Organization>( organization, org);

                            dbCtx.Entry(org).State = System.Data.Entity.EntityState.Modified;
                            dbCtx.SaveChanges();

                            logger.Info(string.Format("Organization updated successfully {0}",
                                                     JsonConvert.SerializeObject(new
                                                     {
                                                         SessionID = guid,
                                                         Params = new { organization = organization },
                                                         RequestedUser = UserUtil.getCurrentUserName(),
                                                         Result = organization
                                                     })));

                            update_result = organization.OrganizationName + " has been updated successfully.\n";
                        }

                    }
                    else
                    {
                        update_result += "Organization ID '" + organization.OrganizationID + "' does not exist in system";
                        logger.Info(string.Format("Organization does not exist in the system {0}",
                                                   JsonConvert.SerializeObject(new
                                                   {
                                                       SessionID = guid,
                                                       Params = new { organization = organization },
                                                       RequestedUser = UserUtil.getCurrentUserName(),
                                                       Result = ""
                                                   })));

                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("Unable to update organization {0}",
                                                  JsonConvert.SerializeObject(new
                                                  {
                                                      SessionID = guid,
                                                      Params = new { organization = organization },
                                                      RequestedUser = UserUtil.getCurrentUserName(),
                                                      Result = organization,
                                                      Exception = ex.ToString()
                                                  })));

                update_result = string.Format("Unable to update organization {0}", organization.OrganizationName);
            }


            return update_result;
        }



        public static String deleteOrganization(Organization organization, String userName)
        {

            String delete_result = "";
            String guid = Guid.NewGuid().ToString();
            bool OKForDelete = false;
            try
            {
                using(var ctx = new CPPDbContext()){
                    Organization org = new Organization();
                    org = ctx.Organization.Where(o=>o.OrganizationID == organization.OrganizationID).FirstOrDefault();
                    
                    if(org != null){
                        //delete
                        //delete descendants first
                       // org.UpdatedBy = userName;
                        var orgId = org.OrganizationID.ToString();
                        //delete organization
                        ctx.Organization.Remove(org);
                        ctx.SaveChanges();
                        delete_result = organization.OrganizationName + " has been deleted successfully.\n";
                        logger.Info(string.Format("Organization deleted successfully {0}",
                                                    JsonConvert.SerializeObject(new
                                                    {
                                                        SessionID = guid,
                                                        Params = new { organization = organization },
                                                        RequestedUser = UserUtil.getCurrentUserName(),
                                                        Result = organization
                                                    })));
                    }
                    else{
                        delete_result += organization.OrganizationName + "' does not exist in system";
                        logger.Info(string.Format("Organization does not exist in the system {0}",
                                                JsonConvert.SerializeObject(new
                                                {
                                                    SessionID = guid,
                                                    Params = new { organization = organization },
                                                    RequestedUser = UserUtil.getCurrentUserName(),
                                                    Result = ""
                                                })));
                    }
                }

            }
            catch (Exception ex)
            {
                logger.Error(string.Format("Unable to delete organization {0}",
                                                     JsonConvert.SerializeObject(new
                                                     {
                                                         SessionID = guid,
                                                         Params = new { organization = organization },
                                                         RequestedUser = UserUtil.getCurrentUserName(),
                                                         Result = "",
                                                         Exception = ex.ToString()
                                                     })));
                delete_result += string.Format("Unable to delete organization {0}", organization.OrganizationName);
            }
            
          
            return delete_result;
        }
    }
}