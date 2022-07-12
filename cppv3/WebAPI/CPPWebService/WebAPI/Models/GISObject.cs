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

namespace WebAPI.Models
{
    [DataContract]
    public class Point
    {
        [DataMember]
        public String lat;
        [DataMember]
        public String lon;
    }
    [DataContract]
    public class Polygon
    {
        [DataMember]
        public String lat;
        [DataMember]
        public String lon;
    }
    [DataContract]
    public class Options
    {
        [DataMember]
        public String ProgramID;
        [DataMember]
        public String ProgramName;
        [DataMember]
        public String ProgramElementID;
        [DataMember]
        public String ProgramElementName;
        [DataMember]
        public String ProjectID;
        [DataMember]
        public String ProjectName;
        [DataMember]
        public String ProjectManager;
        [DataMember]
        public String ProjectSponsor;
        //[DataMember]
        //public String GeocodedLocation;
        //[DataMember]
        //public String UserDefinedLocation;
        [DataMember]
        public String OrganizationAddress;
        [DataMember]
        public String Note;
        [DataMember]
        public String AStartDate;
        [DataMember]
        public String AEndDate;
        [DataMember]
        public String Construction;
        [DataMember]
        public String CStartDate;
        [DataMember]
        public String CEndDate;
        [DataMember]
        public String theme; //RGB Value
        [DataMember]
        public String tags;
    }
    [DataContract]
    public class GISObject
    {
        //[DataMember] public String DT_RowId;
        [DataMember]
        public String title; //Project Name
        [DataMember]
        public String start; //Start Date
        [DataMember]
        public String end; //End Date
        [DataMember]
        public String ShapeType;
        [DataMember]
        public Options options;
        [DataMember]
        public Point point;
        [DataMember]
        public List<Polygon> polygon;

        public static List<GISObject> getLocation(String OrganizationID)//(String ProgramID, String ProgramElementID, String ProjectID)
        {
            //QUICKTEST
            log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

            logger.Error("OrganizationID" + OrganizationID);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            List<GISObject> MatchedGISObject = new List<GISObject>();
            MySqlConnection conn = null;
            MySqlDataReader reader = null;
            try
            {
                // create and open a connection object
                conn = ConnectionManager.getConnection();
                conn.Open();
                //Query to get GIS details
                //String query = "SELECT prg.ProgramID, prg.ProgramName, prge.ProgramElementID, prge.ProgramElementName, prj.ProjectID, prj.ProjectName, prj.ProjectManager, prj.ProjectSponsor, prj.CurrentStartDate, prj.CurrentEndDate, prj_loc.GeoLocationID, prj_loc.ShapeType, prj_loc.GeocodedLocation, prj_loc.UserDefinedLocation, prj_loc.RGBValue, prj_loc.LatLong";
                String q =  "SELECT"
                 + " prg.ProgramID"             //0
                 + ", prg.ProgramName"          //1
                 + ", prge.ProgramElementID"    //2
                 + ", prge.ProgramElementName"  //3
                 + ", prj.ProjectID"            //4
                 + ", prj.ProjectName"          //5
                 + ", prj.ProjectManager"       //6
                 + ", prj.ProjectSponsor"       //7
                 + ", ifnull(prj.CurrentStartDate,now())"     //8
                 + ", ifnull(prj.CurrentEndDate,addDate(now(),1))"       //9
                 //+ ", null as GeoLocationID, \"Polygon\" as ShapeType, null as GeocodedLocation, null as UserDefinedLocation, null as RGBValue, prj.LatLong"
                 + ", prj.LatLong"              //10
                 + ", org.OrganizationName"     //11
                 + ", org.OrganizationAddress"  //12
                 + ", org.Note"                 //13
                 + ", org.LatLong AS OLatLong"  //14
                 + ", CASE WHEN (SELECT PhaseCode FROM activity a WHERE a.ProjectID = prj.ProjectID AND a.PhaseCode = 4 GROUP BY a.ProjectID) = 4 THEN TRUE ELSE \"\" END"
                 + " AS PhaseCode"              //15
                 + ", IFNULL((SELECT MIN(ActivityStartDate) FROM activity a WHERE a.ProjectID = prj.ProjectID AND a.PhaseCode = 4 GROUP BY a.ProjectID),\"\")"
                 + " AS CSD"                    //16
                 + ", IFNULL((SELECT MAX(ActivityEndDate) FROM activity a WHERE a.ProjectID = prj.ProjectID AND a.PhaseCode = 4 GROUP BY a.ProjectID),\"\")"
                 + " AS CED"                    //17
                 + " FROM"
                 + " program prg"
                 + ", program_element prge"
                 + ", project prj"
                 + ", organization org"
                 + " WHERE 1=1"
                 + " AND prg.ProgramID = prge.ProgramID"
                 + " AND prge.ProgramElementID = prj.ProgramElementID"
                 + " AND prg.OrganizationID = org.OrganizationID"
                 + " AND org.OrganizationID = @OrganizationID";
                //query += " AND prj.ProjectID = prj_loc.ProjectID";
                //if (ProgramID != "null")
                //    query += " AND prg.ProgramID = '" + ProgramID + "'";
                //if (ProgramElementID != "null")
                //    query += " AND prge.ProgramElementID = '" + ProgramElementID + "'";
                //if (ProjectID != "null")
                //    query += " AND prj.ProjectID = '" + ProjectID + "'";
             
                MySqlCommand command = new MySqlCommand(q, conn);
                command.Parameters.AddWithValue("@OrganizationID", OrganizationID);
                logger.Error("Pass Query");

                using (reader = command.ExecuteReader())
                {
                    int first = 1;
                    while (reader.Read())
                    {
                        logger.Error("AfterQueyr");
                        if (first == 1)
                        {
                            logger.Error("IF EQUAL 1");
                            GISObject orgGISObject = new GISObject();
                            //get organization first
                            orgGISObject.title = reader.GetValue(11).ToString().Trim();
                            orgGISObject.ShapeType = "Polygon";
                            logger.Error("SHAPE TYPE");
                            //set the start and end date to that of the first project temporarily
                            orgGISObject.start = reader.GetDateTime(8).ToString("yyyy-MM-dd").Trim();
                            logger.Error(orgGISObject.start);
                            orgGISObject.end = reader.GetDateTime(9).ToString("yyyy-MM-dd").Trim();
                            logger.Error(orgGISObject.end);

                            //set options
                            orgGISObject.options = new Options();
                            orgGISObject.options.OrganizationAddress = reader.GetValue(12).ToString().Trim();
                            logger.Error("GIS OBJECT Options ORganiation Address");
                            orgGISObject.options.Note = reader.GetValue(13).ToString().Trim();
                            logger.Error("GIS OBJECT Options NOte");
                            orgGISObject.options.theme = "blue";


                            //get organization polygon
                            orgGISObject.polygon = new List<Polygon>();
                            String[] OLatLong = reader.GetValue(14).ToString().Trim().Split(' ');
                            logger.Error("GIS O LAT LONG" + OLatLong.Length);
                            for (int i = 0; i < OLatLong.Length; i += 1)
                            {
                                Polygon GISPolygon = new Polygon();
                                String[] coor = OLatLong[i].Trim().Split(',');
                                GISPolygon.lat = coor[0];
                                GISPolygon.lon = coor[1];
                                orgGISObject.polygon.Add(GISPolygon);
                            }
                            logger.Error("AFTER OLATLONG LOOP");
                            MatchedGISObject.Add(orgGISObject);
                            logger.Error("ADD TO LIST");
                            first = 0; // got all data for first
                        }
                        logger.Error("ELSE NOT EQUAL 11");
                        GISObject returnedGISObject = new GISObject();
                        returnedGISObject.title = reader.GetValue(5).ToString().Trim();
                        returnedGISObject.start = reader.GetDateTime(8).ToString("yyyy-MM-dd").Trim();
                        returnedGISObject.end = reader.GetDateTime(9).ToString("yyyy-MM-dd").Trim();
                        returnedGISObject.ShapeType = "Polygon";
                        returnedGISObject.options = new Options();
                        returnedGISObject.options.ProgramID = reader.GetValue(0).ToString().Trim();
                        returnedGISObject.options.ProgramName = reader.GetValue(1).ToString().Trim();
                        returnedGISObject.options.ProgramElementID = reader.GetValue(2).ToString().Trim();
                        returnedGISObject.options.ProgramElementName = reader.GetValue(3).ToString().Trim();
                        returnedGISObject.options.ProjectID = reader.GetValue(4).ToString().Trim();
                        returnedGISObject.options.ProjectName = reader.GetValue(5).ToString().Trim();
                        returnedGISObject.options.ProjectManager = reader.GetValue(6).ToString().Trim();
                        returnedGISObject.options.ProjectSponsor = reader.GetValue(7).ToString().Trim();
                        //returnedGISObject.options.GeocodedLocation = reader.GetValue(12).ToString().Trim();
                        //returnedGISObject.options.UserDefinedLocation = reader.GetValue(13).ToString().Trim();
                        returnedGISObject.options.theme = "red";
                        returnedGISObject.options.AStartDate = reader.GetDateTime(8).ToString("yyyy-MM-dd").Trim();
                        returnedGISObject.options.AEndDate = reader.GetDateTime(9).ToString("yyyy-MM-dd").Trim();
                        returnedGISObject.options.Construction = reader.GetValue(15).ToString().Trim();
                        returnedGISObject.options.CStartDate = reader.GetValue(16).ToString().Trim();  //GetDateTime(16).ToString("yyyy-MM-dd").Trim();
                        returnedGISObject.options.CEndDate = reader.GetValue(17).ToString().Trim();  //GetDateTime(17).ToString("yyyy-MM-dd").Trim();

                        if (returnedGISObject.ShapeType == "Point")
                        {
                            String[] LatLong = reader.GetValue(15).ToString().Trim().Split(',');
                            returnedGISObject.point = new Point();
                            returnedGISObject.point.lat = LatLong[0];
                            returnedGISObject.point.lon = LatLong[1];
                        }

                        //ProjectLocation RetreivedProjectLocation = new ProjectLocation(reader.GetValue(0).ToString().Trim(), reader.GetValue(1).ToString().Trim(), reader.GetValue(2).ToString().Trim(), reader.GetValue(3).ToString().Trim(), reader.GetValue(4).ToString().Trim(), reader.GetValue(5).ToString().Trim(), reader.GetValue(6).ToString().Trim(), reader.GetValue(7).ToString().Trim(), reader.GetValue(8).ToString().Trim(), reader.GetValue(9).ToString().Trim(), reader.GetValue(10).ToString().Trim(), reader.GetValue(11).ToString().Trim(), reader.GetValue(12).ToString().Trim());
                        //MatchedProjectLocationList.Add(RetreivedProjectLocation);

                        if (returnedGISObject.ShapeType == "Polygon")
                        {
                            /* String[] LatLong = reader.GetValue(15).ToString().Trim().Split(',');
                             returnedGISObject.Point.lat = LatLong[0];
                             returnedGISObject.Point.lon = LatLong[1];*/
                            
                            returnedGISObject.polygon = new List<Polygon>();
                            //returnedGISObject.Polygon.lon = new List<String>();
                            String[] LatLong = reader.GetValue(10).ToString().Trim().Split(' ');
                            if (LatLong.Length > 1) // there is a polygon
                            {
                                for (int i = 0; i < LatLong.Length; i += 1)
                                {
                                    Polygon GISPolygon = new Polygon();
                                    String[] coor = LatLong[i].Trim().Split(',');
                                    GISPolygon.lat = coor[0];
                                    GISPolygon.lon = coor[1];
                                    //GISPolygon.lat = LatLong[i];
                                    //GISPolygon.lon = LatLong[i + 1];
                                    returnedGISObject.polygon.Add(GISPolygon);
                                }
                            }
                            else
                            {
                                //returnedGISObject.polygon.Add(null);
                            }
                            

                        }
                        logger.Debug(returnedGISObject);
                        MatchedGISObject.Add(returnedGISObject);

                    }

                    //set date of Organization to first program
                    String start = MatchedGISObject[1].start;
                    String end = MatchedGISObject[1].end;
                    //go through each item to find earliest start and latest end date for organization
                    logger.Debug("COUNT IS " + MatchedGISObject.Count);
                    foreach (var obj in MatchedGISObject)
                    {
                        //string.Compare returns -1 if 1st parameter is smaller
                        if (obj.start != null && string.Compare(obj.start, start) == -1)
                        {
                            start = obj.start;
                        }
                        //string.Compare returns 1 is 1st parameter is larger
                        if (obj.end != null && string.Compare(obj.end, end) == 1)
                        {
                            end = obj.end;
                        }
                    }
                    MatchedGISObject[0].start = start;
                    MatchedGISObject[0].end = end;
                }
                // test


            }
            catch (Exception ex)
            {
                var stackTrace = new StackTrace(ex, true);
                var line = stackTrace.GetFrame(0).GetFileLineNumber();
                Logger.LogExceptions(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, ex.Message, line.ToString(), Logger.logLevel.Exception);
            }
            finally
            {
                if (conn != null) conn.Close();
                if (reader != null) reader.Close();

            }
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Exit Point", Logger.logLevel.Debug);

            logger.Debug("RETURN OBJECT");
            logger.Debug(MatchedGISObject);
            return MatchedGISObject;
        }



    }
}