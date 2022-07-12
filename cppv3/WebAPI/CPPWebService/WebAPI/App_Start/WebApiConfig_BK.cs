using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Net.Http.Formatting;
using System.Web.Http.Cors;

namespace WebAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);
            // Web API configuration and services

            // Web API routes
            //config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );



            config.Routes.MapHttpRoute(
             name: "RequestUser",
             routeTemplate: "Request/User/{UserID}/{FirstName}/{LastName}/{Role}",
                //Returns: User Object
             defaults: new { controller = "RequestUser", UserID = RouteParameter.Optional, FirstName = RouteParameter.Optional, LastName = RouteParameter.Optional, Role = RouteParameter.Optional }
         );
            
            config.Routes.MapHttpRoute(
                name:"RequestFundType",
                routeTemplate: "Request/FundType/{FundType}/{Value}",
                defaults: new
                {
                    controller = "RequestFundType",
                    FundType =RouteParameter.Optional,
                    Value = RouteParameter.Optional
                }
                );
            config.Routes.MapHttpRoute(
                name:"RegisterFunType",
                routeTemplate: "response/FundType/",
                defaults: new
                {
                    controller = "RegisterFundType"
                }
            
        );
            config.Routes.MapHttpRoute(
             name: "RequestUserLogin",
             routeTemplate: "Request/UserLogin/{UserID}/{Password}",
                //Returns: FullName, ACL Bit Vecotr and AuthLevel of User if validated, else returns N/A and Not Authourized Respectively
             defaults: new { controller = "RequestUserLogin", UserID = RouteParameter.Optional, Password = RouteParameter.Optional }
         );

        //    config.Routes.MapHttpRoute(
        //    name: "RegisterUser",
        //    routeTemplate: "Response/User/{Operation}/{FirstName}/{MiddleName}/{LastName}/{DOB}/{UserID}/{Password}/{Role}",
        //        //Returns: User ID jsmith already exists in system | User John Smith already exists in system | Success
        //        //TODO: Add attributes
        //    defaults: new { controller = "RegisterUser", Operation = RouteParameter.Optional, FirstName = RouteParameter.Optional, LastName = RouteParameter.Optional, DOB = RouteParameter.Optional, UserID = RouteParameter.Optional, Password = RouteParameter.Optional, Role = RouteParameter.Optional }
        //);
         

            config.Routes.MapHttpRoute(
            name: "RegisterUsers",
            routeTemplate: "Response/User",
                //Returns: User ID jsmith already exists in system | User John Smith already exists in system | Success
                //TODO: Add attributes
            //defaults: new { controller = "RegisterUser", Operation = RouteParameter.Optional, FirstName = RouteParameter.Optional, LastName = RouteParameter.Optional, DOB = RouteParameter.Optional, UserID = RouteParameter.Optional, Password = RouteParameter.Optional, Role = RouteParameter.Optional }
            defaults: new { controller = "RegisterUser" }
            );
            config.Routes.MapHttpRoute(
             name: "RequestProgram",
             routeTemplate: "Request/Program/{OrganizationID}/{ProgramID}/{KeyStroke}",
                //Returns: DT_RowId, ProgramID, ProgramName, ProgramManager, ProgramSponsor
             defaults: new { controller = "RequestProgram", OrganizationID = RouteParameter.Optional,ProgramID = RouteParameter.Optional, KeyStroke = RouteParameter.Optional }
         );
           

            /*config.Routes.MapHttpRoute(
             name: "RegisterProgram",
             routeTemplate: "Response/Program/{Operation}/{ProgramID}/{ProgramName}/{ProgramManager}/{ProgramSponsor}",
                //Returns: Program 'BrdProgram' already exists in the system | Success

             defaults: new { controller = "RegisterProgram", Operation = RouteParameter.Optional, ProgramID = RouteParameter.Optional, ProgramName = RouteParameter.Optional, ProgramManager = RouteParameter.Optional, ProgramSponsor = RouteParameter.Optional }
         );*/

            config.Routes.MapHttpRoute(
            name: "RegisterProgram",
            routeTemplate: "Response/Program",
                //Returns: Program 'BrdProgram' already exists in the system | Success
            defaults: new { controller = "RegisterProgram" }
        );
            config.Routes.MapHttpRoute(
    name: "RequestOrganization",
    routeTemplate: "Request/Organization/{OrganizationID}/{KeyStroke}",
                //Returns: DT_RowId, ProgramID, ProgramName, ProgramManager, ProgramSponsor
    defaults: new { controller = "RequestOrganization", OrganizationID = RouteParameter.Optional, KeyStroke = RouteParameter.Optional });
            
            config.Routes.MapHttpRoute(
         name: "RegisterOrganization",
         routeTemplate: "Response/Organization",
                //Returns: Program 'BrdOrganization' already exists in the system | Success
         defaults: new { controller = "RegisterOrganization" }
     );

            config.Routes.MapHttpRoute(
             name: "RequestProgramElement",
             routeTemplate: "Request/ProgramElement/{ProgramID}/{ProgramElementID}/{KeyStroke}",
                //Returns: DT_RowId, ProgramName, ProgramElementID, ProgramElementName, ProgramManager, ProgramSponsor, ProgramElementManager, ProgramElementSponsor
             defaults: new { controller = "RequestProgramElement", ProgramID = RouteParameter.Optional, ProgramElementID = RouteParameter.Optional, KeyStroke = RouteParameter.Optional }
         );


            /*config.Routes.MapHttpRoute(
             name: "RegisterProgramElement",
             routeTemplate: "Response/ProgramElement/{Operation}/{ProgramID}/{ProgramElementID}/{ProgramElementName}/{ProgramElementManager}/{ProgramElementSponsor}",
                //Returns: Program Element 'BrdProgramLAX' already exists in the system | Success
                //TODO: Add attributes
             defaults: new { controller = "RegisterProgramElement", Operation = RouteParameter.Optional, ProgramID = RouteParameter.Optional, ProgramElementID = RouteParameter.Optional, ProgramElementName = RouteParameter.Optional, ProgramElementManager = RouteParameter.Optional, ProgramElementSponsor = RouteParameter.Optional }
         );*/

            config.Routes.MapHttpRoute(
            name: "RegisterProgramElement",
            routeTemplate: "Response/ProgramElement",
                //Returns: Program Element 'BrdProgramLAX' already exists in the system | Success
            defaults: new { controller = "RegisterProgramElement" }
        );


            config.Routes.MapHttpRoute(
             name: "RequestProject",
             routeTemplate: "Request/Project/{ProgramID}/{ProgramElementID}/{ProjectID}/{KeyStroke}",
                //Returns: DT_RowId, ProgramName, ProgramElementName, ProjectID, ProjectName, ProgramManager, ProgramSponsor, ProgramElementManager, ProgramElementSponsor, ProjectManager, ProjectSponsor, ApprovedTrendNumber
                //TODO: Add geolocation
             defaults: new { controller = "RequestProject", ProgramID = RouteParameter.Optional, ProgramElementID = RouteParameter.Optional, ProjectID = RouteParameter.Optional, KeyStroke = RouteParameter.Optional }
         );


            config.Routes.MapHttpRoute(
             name: "RequestProjectLocation",
             routeTemplate: "Request/ProjectLocation/{ProgramID}/{ProgramElementID}/{ProjectID}",
                //Returns: 

             defaults: new { controller = "RequestProjectLocation", ProgramID = RouteParameter.Optional, ProgramElementID = RouteParameter.Optional, ProjectID = RouteParameter.Optional }
         );

            /*config.Routes.MapHttpRoute(
             name: "RegisterProject",
             routeTemplate: "Response/Project/{Operation}/{ProgramID}/{ProgramElementID}/{ProjectID}/{ProjectName}/{ProjectManager}/{ProjectSponsor}",
                //Returns: Project 'LAX_ReportingServer' already exists in the system | Success
                //TODO: Add geolocation
             defaults: new { controller = "RegisterProject", Operation = RouteParameter.Optional, ProgramID = RouteParameter.Optional, ProgramElementID = RouteParameter.Optional, ProjectID = RouteParameter.Optional, ProjectName = RouteParameter.Optional, ProjectManager = RouteParameter.Optional, ProjectSponsor = RouteParameter.Optional }
         );*/

            config.Routes.MapHttpRoute(
           name: "RegisterProject",
           routeTemplate: "Response/Project",
                //Returns: Project 'LAX_ReportingServer' already exists in the system | Success
           defaults: new { controller = "RegisterProject" }
       );

            /*config.Routes.MapHttpRoute(
             name: "RegisterProjectLocation",
             routeTemplate: "Response/ProjectLocation/{Operation}/{GeoLocationID}/{ProjectID}/{ShapeType}/{GeocodedLocation}/{UserDefinedLocation}/{RGBValue}",
                //Returns: Project 'LAX_ReportingServer' already exists in the system | Success

             defaults: new { controller = "RegisterProjectLocation", Operation = RouteParameter.Optional, GeoLocationID = RouteParameter.Optional, ProjectID = RouteParameter.Optional, ShapeType = RouteParameter.Optional, GeocodedLocation = RouteParameter.Optional, UserDefinedLocation = RouteParameter.Optional, RGBValue = RouteParameter.Optional }
         );*/

            config.Routes.MapHttpRoute(
           name: "RegisterProjectLocation",
           routeTemplate: "Response/ProjectLocation",
                //Returns: Project 'LAX_ReportingServer' already exists in the system | Success
           defaults: new { controller = "RegisterProjectLocation" }
       );


            config.Routes.MapHttpRoute(
             name: "RequestTrend",
             routeTemplate: "Request/Trend/{ProgramID}/{ProgramElementID}/{ProjectID}/{TrendNumber}/{KeyStroke}",
                //Returns: DT_RowId, ProgramName, ProgramElementName, ProjectID, ProjectName, TrendNumber, TrendDescription and TrendStatus
                //TODO: Add attributes
             defaults: new { controller = "RequestTrend", ProgramID = RouteParameter.Optional, ProgramElementID = RouteParameter.Optional, ProjectID = RouteParameter.Optional, TrendNumber = RouteParameter.Optional, KeyStroke = RouteParameter.Optional }
         );


            config.Routes.MapHttpRoute(
            name: "RegisterTrend",
            routeTemplate: "Response/Trend",
                //Returns: Project ID 'PRJ001' does not exist in the system | Success
                //TODO: Add attributes
            defaults: new { controller = "RegisterTrend" }
        );

            config.Routes.MapHttpRoute(
             name: "RegisterTrendApproval",
             routeTemplate: "Response/TrendApproval/{ProjectID}/{TrendNumber}/{TrendStatus}/{ApprovedBy}/{ApprovedDate}",
                //Returns: 
                defaults: new { controller = "RegisterTrendApproval", ProjectID = RouteParameter.Optional, TrendNumber = RouteParameter.Optional, TrendStatus = RouteParameter.Optional, ApprovedBy = RouteParameter.Optional, ApprovedDate = RouteParameter.Optional }
         );


            config.Routes.MapHttpRoute(
             name: "RequestPhase",
             routeTemplate: "Request/Phase/{ProjectID}/{TrendNumber}/{PhaseCode}",
                //Returns:  DT_RowId, ProjectID, TrendNumber, PhaseCode, PhaseStartDate, PhaseEndDate
                //TODO: Add costing attributes
             defaults: new { controller = "RequestPhase", ProjectID = RouteParameter.Optional, TrendNumber = RouteParameter.Optional, PhaseCode = RouteParameter.Optional }
         );




            config.Routes.MapHttpRoute(
            name: "RegisterPhase",
            routeTemplate: "Response/Phase/{Operation}/{ProjectID}/{TrendNumber}/{PhaseCode}/{PhaseStartDate}/{PhaseEndDate}",
                //Returns: Phase 'CON' already exists for this Trend | Success
                //TODO: Add attributes
            defaults: new { controller = "RegisterPhase", Operation = RouteParameter.Optional, ProjectID = RouteParameter.Optional, TrendNumber = RouteParameter.Optional, PhaseCode = RouteParameter.Optional, PhaseStartDate = RouteParameter.Optional, PhaseEndDate = RouteParameter.Optional }
        );


            config.Routes.MapHttpRoute(
            name: "RequestActivity",
            routeTemplate: "Request/Activity/{CostBreakdown}/{ProgramID}/{ProgramElementID}/{ProjectID}/{TrendNumber}/{PhaseCode}/{ActivityID}/{BudgetCategory}/{BudgetSubCategory}",
                //CostBreakdown: 0 for just Activity Details; 1 for Cost Breakdowns also
                //Returns: Complex/Nested JSON Object. NOTE: use string.replace("\\","") before parsing the returned JSON
            defaults: new { controller = "RequestActivity", ProgramID = RouteParameter.Optional, ProgramElementID = RouteParameter.Optional, ProjectID = RouteParameter.Optional, TrendNumber = RouteParameter.Optional, PhaseCode = RouteParameter.Optional, ActivityID = RouteParameter.Optional, BudgetCategory = RouteParameter.Optional, BudgetSubCategory = RouteParameter.Optional }
        );


            config.Routes.MapHttpRoute(
            name: "RegisterActivity",
            routeTemplate: "Response/Activity",
                //Returns: 
                //TODO: Add attributes
            defaults: new { controller = "RegisterActivity" }
        );


            config.Routes.MapHttpRoute(
            name: "RequestWBS",
            routeTemplate: "Request/WBS/{ProgramID}/{ProgramElementID}/{ProjectID}/{TrendNumber}/{PhaseCode}/{ActivityID}/{BudgetCategory}/{BudgetSubCategory}",

                //Returns: Complex/Nested JSON Object. NOTE: use string.replace("\\","") before parsing the returned JSON
            defaults: new { controller = "RequestWBS", ProgramID = RouteParameter.Optional, ProgramElementID = RouteParameter.Optional, ProjectID = RouteParameter.Optional, TrendNumber = RouteParameter.Optional, PhaseCode = RouteParameter.Optional, ActivityID = RouteParameter.Optional, BudgetCategory = RouteParameter.Optional, BudgetSubCategory = RouteParameter.Optional }
        );

            config.Routes.MapHttpRoute(
            name: "RequestTrendGraph",
            routeTemplate: "Request/TrendGraph/{ProjectID}",
                //CostBreakdown: 0 for just Activity Details; 1 for Cost Breakdowns also
                //Returns: Complex/Nested JSON Object. NOTE: use string.replace("\\","") before parsing the returned JSON
            defaults: new { controller = "RequestTrendGraph", ProjectID = RouteParameter.Optional }
        );

            config.Routes.MapHttpRoute(
            name: "RegisterCost",
            routeTemplate: "Response/Cost",
                //Returns: Succesfully deleted | Succesfuly inserted

            defaults: new { controller = "RegisterCost" }
        );

            config.Routes.MapHttpRoute(
            name: "RequestCost",
            routeTemplate: "Request/Cost/{ProjectID}/{TrendNumber}/{PhaseCode}/{ActivityID}/",
                //Returns: 

            defaults: new { controller = "RequestCost", ProjectID = RouteParameter.Optional, TrendNumber = RouteParameter.Optional, PhaseCode = RouteParameter.Optional, ActivityID = RouteParameter.Optional }
        );

            config.Routes.MapHttpRoute(
            name: "RequestFTECost",
            routeTemplate: "Request/FTECost/{ActivityID}/",
                //Returns:  
            defaults: new { controller = "RequestFTECost", ActivityID = RouteParameter.Optional }
        );

            config.Routes.MapHttpRoute(
            name: "RegisterFTECost",
            routeTemplate: "Response/FTECost",
                //Returns: Succesfully deleted | Succesfuly inserted

            defaults: new { controller = "RegisterFTECost" }
        );

            config.Routes.MapHttpRoute(
            name: "RequestUnitCost",
            routeTemplate: "Request/UnitCost/{ActivityID}/",
                //Returns:  
            defaults: new { controller = "RequestUnitCost", ActivityID = RouteParameter.Optional }
        );

            config.Routes.MapHttpRoute(
            name: "RegisterUnitCost",
            routeTemplate: "Response/UnitCost",
                //Returns: Succesfully deleted | Succesfuly inserted

            defaults: new { controller = "RegisterUnitCost" }
        );


            config.Routes.MapHttpRoute(
            name: "RequestPercentageBasisCost",
            routeTemplate: "Request/PercentageBasisCost/{ActivityID}/",
                //Returns:  
            defaults: new { controller = "RequestPercentageBasisCost", ActivityID = RouteParameter.Optional }
        );


            config.Routes.MapHttpRoute(
            name: "RegisterPercentageBasisCost",
            routeTemplate: "Response/PercentageBasisCost",
                //Returns: Succesfully deleted | Succesfuly inserted

            defaults: new { controller = "RegisterPercentageBasisCost" }
        );

            config.Routes.MapHttpRoute(
            name: "RequestLumpsumCost",
            routeTemplate: "Request/LumpsumCost/{ActivityID}/",
                //Returns:  
            defaults: new { controller = "RequestLumpsumCost", ActivityID = RouteParameter.Optional }
        );

            config.Routes.MapHttpRoute(
            name: "RegisterLumpsumCost",
            routeTemplate: "Response/LumpsumCost",
                //Returns: Succesfully deleted | Succesfuly inserted

            defaults: new { controller = "RegisterLumpsumCost" }
        );

            config.Routes.MapHttpRoute(
            name: "RequestLookupTrendStatus",
            routeTemplate: "Request/TrendStatus/{StatusID}/{StatusDescription}",
                //Returns:  DT_RowId, StatusID, StatusDescription
            defaults: new { controller = "RequestLookupTrendStatus", StatusID = RouteParameter.Optional, StatusDescription = RouteParameter.Optional }
        );


            config.Routes.MapHttpRoute(
           name: "RegisterLookupTrendStatus",
           routeTemplate: "Response/TrendStatus/{Operation}/{StatusID}/{StatusDescription}",
                //Returns: Status 'Approved' already exists in the system | Success
           defaults: new { controller = "RegisterLookupTrendStatus", Operation = RouteParameter.Optional, StatusID = RouteParameter.Optional, StatusDescription = RouteParameter.Optional }
       );

            config.Routes.MapHttpRoute(
            name: "RequestLookupPhaseCode",
            routeTemplate: "Request/PhaseCode/{PhaseID}/{PhaseDescription}/{PhaseCode}",
                //Returns:  DT_RowId, PhaseID, PhaseDescription, PhaseCode
            defaults: new { controller = "RequestLookupPhaseCode", PhaseID = RouteParameter.Optional, PhaseDescription = RouteParameter.Optional, PhaseCode = RouteParameter.Optional }
        );


            /*config.Routes.MapHttpRoute(
            name: "RegisterLookupPhaseCode",
            routeTemplate: "Response/PhaseCode/{Operation}/{PhaseID}/{PhaseDescription}/{PhaseCode}",
                //Returns:  PhaseCode 'CON' already exists in the system | Success
            defaults: new { controller = "RegisterLookupPhaseCode", Operation = RouteParameter.Optional, PhaseID = RouteParameter.Optional, PhaseDescription = RouteParameter.Optional, PhaseCode = RouteParameter.Optional }
        );*/

            config.Routes.MapHttpRoute(
          name: "RegisterLookupPhaseCode",
          routeTemplate: "Response/PhaseCode",
                //Returns: Project 'LAX_ReportingServer' already exists in the system | Success
          defaults: new { controller = "RegisterLookupPhaseCode" }
      );

            config.Routes.MapHttpRoute(
           name: "RequestLookupActivityCategory",
           routeTemplate: "Request/ActivityCategory/{CategoryID}/{CategoryDescription}/{SubCategoryID}/{SubCategoryDescription}",
                //Returns:  DT_RowId, CategoryID, CategoryDescription, SubCategoryID, SubCategoryDescription
           defaults: new { controller = "RequestLookupActivityCategory", CategoryID = RouteParameter.Optional, CategoryDescription = RouteParameter.Optional, SubCategoryID = RouteParameter.Optional, SubCategoryDescription = RouteParameter.Optional }
       );


            /* config.Routes.MapHttpRoute(
             name: "RegisterLookupActivityCategory",
             routeTemplate: "Response/ActivityCategory/{Operation}/{CategoryID}/{CategoryDescription}/{SubCategoryID}/{SubCategoryDescription}",
                 //Returns:  SubCategory 'Land Bidding' already exists in system | Success
             defaults: new { controller = "RegisterLookupActivityCategory", Operation = RouteParameter.Optional, CategoryID = RouteParameter.Optional, CategoryDescription = RouteParameter.Optional, SubCategoryID = RouteParameter.Optional, SubCategoryDescription = RouteParameter.Optional }
         );
             */
            config.Routes.MapHttpRoute(
         name: "RegisterLookupActivityCategory",
         routeTemplate: "Response/ActivityCategory",
                //Returns:  SubCategory 'Land Bidding' already exists in system | Success
         defaults: new { controller = "RegisterLookupActivityCategory" }
     );

            config.Routes.MapHttpRoute(
           name: "RequestLookupRole",
           routeTemplate: "Request/Role/{Role}",
                //Returns:  DT_RowId, Role, Access Control List
           defaults: new { controller = "RequestLookupRole", Role = RouteParameter.Optional }
       );


            /* config.Routes.MapHttpRoute(
             name: "RegisterLookupRole",
             routeTemplate: "Response/Role/{Operation}/{Role}/{AccessControlList}",
                 //Returns:  Success | Role "Admin" already exists/does not exist in system
             defaults: new { controller = "RegisterLookupRole", Operation = RouteParameter.Optional, Role = RouteParameter.Optional, AccessControlList = RouteParameter.Optional }
         );*/

            config.Routes.MapHttpRoute(
        name: "RegisterLookupRole",
        routeTemplate: "Response/Role",
                //Returns:  Success | Role "Admin" already exists/does not exist in system
        defaults: new { controller = "RegisterLookupRole" }
    );


            config.Routes.MapHttpRoute(
           name: "RequestLookupFTEPosition",
           routeTemplate: "Request/FTEPosition/{PositionID}/{PositionDescription}",
                //Returns:  DT_RowId, PositionID, PositionDescription, MinHourlyRate, MaxHourlyRate, CurrentHourlyRate
           defaults: new { controller = "RequestLookupFTEPosition", PositionID = RouteParameter.Optional, PositionDescription = RouteParameter.Optional }
       );


            /*config.Routes.MapHttpRoute(
            name: "RegisterLookupFTEPosition",
            routeTemplate: "Response/FTEPosition/{Operation}/{PositionID}/{PositionDescription}/{MinHourlyRate}/{MaxHourlyRate}/{CurrentHourlyRate}",
                //Returns:  Position 'Architect' already exists in system | Success
            defaults: new { controller = "RegisterLookupFTEPosition", Operation = RouteParameter.Optional, PositionID = RouteParameter.Optional, PositionDescription = RouteParameter.Optional, MinHourlyRate = RouteParameter.Optional, MaxHourlyRate = RouteParameter.Optional, CurrentHourlyRate = RouteParameter.Optional }
        );*/

            config.Routes.MapHttpRoute(
          name: "RegisterLookupFTEPosition",
          routeTemplate: "Response/FTEPosition",
                //Returns:  Position 'Architect' already exists in system | Success
          defaults: new { controller = "RegisterLookupFTEPosition" }
      );
           




        }
    }
}
