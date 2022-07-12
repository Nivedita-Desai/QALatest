using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Net.Http.Formatting;
using System.Web.Http.Cors;
using System.Web.Http.ExceptionHandling;
using WebDemoAPI.CustomHandler;
using WebAPI.CustomHandler;

namespace WebAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);


            // Web API configuration and services
            //Registering GlobalExceptionHandler
            config.Services.Replace(typeof(IExceptionHandler), new GlobalExceptionHandler());
            //Registering UnhandledExceptionLogger
            config.Services.Replace(typeof(IExceptionLogger), new UnhandledExceptionLogger());
            // Web API routes
            //config.MapHttpAttributeRoutes();
            //Registering RequestResponseHandler
            //config.MessageHandlers.Add(new RequestResponseHandler());

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            config.Routes.MapHttpRoute(
              name: "Values",
              routeTemplate: "Request/values",
              //Returns: User Object
              defaults: new { controller = "Values" }
          );

            config.Routes.MapHttpRoute(
                     name: "All Trends",
                     routeTemplate: "Request/AllTrend/{ProjectID}",
                     //Returns: User Object
                     defaults: new { controller = "RequestAllTrend", ProjectID = RouteParameter.Optional }
                 );

            config.Routes.MapHttpRoute(
           name: "Is Trend Approved Code",
           routeTemplate: "Request/ApprovalTimerCountDown/{ProjectID}/{TrendNumber}/{UserID}/{CurrentThreshold}",
           //Returns: User Object
           defaults: new { controller = "RequestApprovalTimerCountDown", ProjectID = RouteParameter.Optional, TrendNumber = RouteParameter.Optional, UserID = RouteParameter.Optional, CurrentThreshold = RouteParameter.Optional }
       );

            config.Routes.MapHttpRoute(
     name: "Resend Trend Approved Link",
     routeTemplate: "Request/ResendTrendApprovalLink/{ProjectID}/{TrendId}/{TrendNumber}/{UserID}",
     //Returns: User Object
     defaults: new { controller = "RegisterApprovalResendLinkinMatrix", ProjectID = RouteParameter.Optional, TrendId = RouteParameter.Optional, TrendNumber = RouteParameter.Optional, UserID = RouteParameter.Optional }
 );
            config.Routes.MapHttpRoute(
          name: "Is Code matched for Approval",
          routeTemplate: "Request/TrendCodeSubmitted/{ProjectID}/{TrendNumber}/{UserID}/{UniqueCode}/{CurrentThreshold}",
          //Returns: User Object
          defaults: new { controller = "RequestApprovedCodeSubmit", ProjectID = RouteParameter.Optional, TrendNumber = RouteParameter.Optional, UserID = RouteParameter.Optional, UniqueCode = RouteParameter.Optional, CurrentThreshold = RouteParameter.Optional }
      );
            config.Routes.MapHttpRoute(
   name: "Resend Unique Code For pproval",
   routeTemplate: "Request/ResendUniqueCode/{ProjectID}/{TrendId}/{UserID}/{UniqueCode}",
   //Returns: User Object
   defaults: new { controller = "RequestResendUniqueCode", ProjectID = RouteParameter.Optional, TrendId = RouteParameter.Optional, UserID = RouteParameter.Optional, UniqueCode = RouteParameter.Optional }
);

            config.Routes.MapHttpRoute(
            name: "GetUserByID",
            routeTemplate: "Request/GetUser/{userID}",
            //Returns: User Object
            defaults: new { controller = "RequestUser" }
        );
            config.Routes.MapHttpRoute(
              name: "Request Trend Cost Overhead",
              routeTemplate: "Request/TrendCostOverhead/{ProjectID}/{TrendNumber}",
              defaults: new { controller = "RequestTrendCostOverhead", ProjectID = RouteParameter.Optional, TrendNumber = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
            name: "GetUserByEmployeeListID",
            routeTemplate: "Request/GetUserByEmployeeListID/{EmployeeListID}/{Dummy}",
            //Returns: User Object
            defaults: new { controller = "RequestUser" }
        );

            config.Routes.MapHttpRoute(
             name: "RequestUser",
             routeTemplate: "Request/User/{UserID}/{FirstName}/{LastName}/{Role}",
             //Returns: User Object
             defaults: new { controller = "RequestUser", UserID = RouteParameter.Optional, FirstName = RouteParameter.Optional, LastName = RouteParameter.Optional, Role = RouteParameter.Optional }
         );



            config.Routes.MapHttpRoute(
                name: "RequestFundTypeByOrgID",
                routeTemplate: "Request/FundTypeByOrgID/{OrganizationID}",
                defaults: new
                {
                    controller = "RequestFundTypeByOrgId",
                    OrganizationID = RouteParameter.Optional
                }
        );
            config.Routes.MapHttpRoute(
                name: "RequestFundType",
                routeTemplate: "Request/FundType/{FundType}/{Value}",
                defaults: new
                {
                    controller = "RequestFundType",
                    FundType = RouteParameter.Optional,
                    Value = RouteParameter.Optional
                }
        );
            config.Routes.MapHttpRoute(
                name: "RegisterFunType",
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

            config.Routes.MapHttpRoute(
                name: "RequestTrendById",
                routeTemplate: "Request/trend/{trendId}/{projectId}",
                defaults: new { controller = "RequestTrendById", trendId = RouteParameter.Optional, projectId = RouteParameter.Optional }

                );

            config.Routes.MapHttpRoute(
                name: "requesttest",
                routeTemplate: "Request/test",
                defaults: new { controller = "RequestTest" }

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
                //Returns: DT_RowId, ProgramID, ProgramName, ProgramManager, ProgramSponsor		                 //Returns: DT_RowId, ProgramID, ProgramName, ProgramManager, ProgramSponsor
                defaults: new { controller = "RequestProgram", OrganizationID = RouteParameter.Optional, ProgramID = RouteParameter.Optional, KeyStroke = RouteParameter.Optional }
              );
            config.Routes.MapHttpRoute(
                name: "RequestUnitType",
                routeTemplate: "Request/UnitType",
                defaults: new { controller = "RequestUnitType" }
                );
            config.Routes.MapHttpRoute(
                name: "RegisterUnitType",
                routeTemplate: "Response/UnitType",
                defaults: new { controller = "RegisterUnitType" }
                );
            config.Routes.MapHttpRoute(
                name: "RequestApprovalMatrix",
                routeTemplate: "Request/approvalMatrix",
                defaults: new { controller = "RequestApprovalMatrix" }
                );
            config.Routes.MapHttpRoute(
                name: "registerApprovalMatrix",
                routeTemplate: "response/approvalMatrix",
                defaults: new
                {
                    controller = "RegisterApprovalMatrix"
                });


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
            defaults: new { controller = "RequestOrganization", OrganizationID = RouteParameter.Optional, KeyStroke = RouteParameter.Optional }
        );

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
                name: "RequestProjectByOid",
                        routeTemplate: "Request/ProjectByOid/{OrganizationID}",
                defaults: new { controller = "RequestProjectByOid" }
            );

            config.Routes.MapHttpRoute(
                name: "RequestProjectLocation",
                routeTemplate: "Request/ProjectLocation/{OrganizationID}", // /{ProgramID}/{ProgramElementID}/{ProjectID}", 
                defaults: new { controller = "RequestProjectLocation", OrganizationID = RouteParameter.Optional } // ProgramID = RouteParameter.Optional, ProgramElementID = RouteParameter.Optional, ProjectID = RouteParameter.Optional }
             );

            config.Routes.MapHttpRoute(
                name: "RequestLocations",
                routeTemplate: "Request/Locations",
                defaults: new { controller = "RequestLocations" }
                );

            config.Routes.MapHttpRoute(
                name: "RequestProLocationModule",
                routeTemplate: "Request/ProjLocation/{OrganizationID}",
                defaults: new { controller = "RequestProLocation" }
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
             name: "RequestService",
             routeTemplate: "Request/Service/RequestApproval/{UserID}/{Role}/{TrendId}/{ProjectId}",
             //Returns: DT_RowId, ProgramName, ProgramElementName, ProjectID, ProjectName, TrendNumber, TrendDescription and TrendStatus
             //TODO: Add attributes
             defaults: new { controller = "RequestService", action = "RequestApproval", UserID = RouteParameter.Optional, Role = RouteParameter.Optional, TrendId = RouteParameter.Optional, ProjectId = RouteParameter.Optional }
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

            //get Current project 
            config.Routes.MapHttpRoute(
                name: "RequestCurrentTrend",
                routeTemplate: "Request/currentProject/{ProjectID}/{Granularity}",
                defaults: new { controller = "RequestCurrentTrend", ProjectID = RouteParameter.Optional, Granularity = RouteParameter.Optional }

                );

            config.Routes.MapHttpRoute(
                name: "FutureProject",
                routeTemplate: "Request/FutureProject/{ProjectID}/{Granularity}",
                defaults: new { controller = "RequestFutureTrend", ProjectID = RouteParameter.Optional, Granularity = RouteParameter.Optional }
                );
            config.Routes.MapHttpRoute(
                name: "RequestTrendCurrentDate",
                routeTemplate: "Request/currentTrend/{ProjectID}",
                defaults: new { controller = "RequestTrendCurrentDate" }
                );

            config.Routes.MapHttpRoute(
            name: "RegisterPhase",
            routeTemplate: "Response/Phase/{Operation}/{ProjectID}/{TrendNumber}/{PhaseCode}/{PhaseStartDate}/{PhaseEndDate}",
            //Returns: Phase 'CON' already exists for this Trend | Success
            //TODO: Add attributes
            defaults: new { controller = "RegisterPhase", Operation = RouteParameter.Optional, ProjectID = RouteParameter.Optional, TrendNumber = RouteParameter.Optional, PhaseCode = RouteParameter.Optional, PhaseStartDate = RouteParameter.Optional, PhaseEndDate = RouteParameter.Optional }
        );
            config.Routes.MapHttpRoute(
                name: "RequestMaxFutureDate",
                routeTemplate: "Request/MaxFutureDate/{ProjectID}",
                defaults: new { controller = "RequestMaxFutureDate", ProjectID = RouteParameter.Optional }
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
                name: "RequestActivityByID",
                routeTemplate: "Request/RequestActivityByID/{ActivityID}",
                defaults: new { controller = "RequestActivityID", ActivityID = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(                                              //Tanmay - 15/12/2021
                name: "RequestClientDetailsByID",
                routeTemplate: "Request/RequestClientDetailsByID/{ClientId}",
                defaults: new { controller = "RequestClientController"}
            );
            config.Routes.MapHttpRoute(
               name: "Request Purchase Order Number",
               routeTemplate: "Request/RequestNewPurchaseOrderNumber/{ProjectID}",
               defaults: new { controller = "RequestNewPurchaseOrderNumber", ProjectID = RouteParameter.Optional }
           );

            // swapnil 09/08/2021 Get PO List API

            config.Routes.MapHttpRoute(
               name: "Request Purchase Order List",
               routeTemplate: "Request/RequestPOList/{ProjectID}",
               defaults: new { controller = "RequestPOList", ProjectID = RouteParameter.Optional }
           );

            config.Routes.MapHttpRoute(
                name: "Process QB files",
                routeTemplate: "Request/ProcessQuickbookFile",
                defaults: new { controller = "TranslateQuickbooksBill" }
            );

            config.Routes.MapHttpRoute(
            name: "Request Purchase Order Detail",
            routeTemplate: "Request/RequestPurchaseOrderDetail/{ProjectID}",
            defaults: new { controller = "RequestPurchaseOrderDetail", ProjectID = RouteParameter.Optional }
        );
            config.Routes.MapHttpRoute(
           name: "Request Purchase Order ID Detail",
           routeTemplate: "Request/RequestPurchaseOrderIDDetail/{PurchaseOrderID}",
           defaults: new { controller = "RequestPurchaseOrderIDDetail", PurchaseOrderID = RouteParameter.Optional }
       );

            config.Routes.MapHttpRoute(
                name: "Request Next Unique Identity Number",
                routeTemplate: "Request/NextUniqueIdentityNumber/{NumberType}/{OrganizationID}/{PhaseID}/{CategoryID}",
                defaults: new { controller = "RequestNextUniqueIdentityNumber", NumberType = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
           name: "Register Purchase Order Detail",
           routeTemplate: "Response/PurchaseOrderDetail",
           defaults: new { controller = "RegisterPurchaseOrderDetail" }
       );

            config.Routes.MapHttpRoute(
            name: "RequestWBS",
            routeTemplate: "Request/WBS/{uId}/{OrganizationID}/{ProgramID}/{ProgramElementID}/{ProjectID}/{TrendNumber}/{PhaseCode}/{ActivityID}/{BudgetCategory}/{BudgetSubCategory}/{SearchText}/{AllData}/{DeptID}",

            //Returns: Complex/Nested JSON Object. NOTE: use string.replace("\\","") before parsing the returned JSON
            defaults: new { controller = "RequestWBS", OrganizationID = RouteParameter.Optional, ProgramID = RouteParameter.Optional, ProgramElementID = RouteParameter.Optional, ProjectID = RouteParameter.Optional,
                TrendNumber = RouteParameter.Optional, PhaseCode = RouteParameter.Optional, ActivityID = RouteParameter.Optional,
                BudgetCategory = RouteParameter.Optional, BudgetSubCategory = RouteParameter.Optional, SearchText = RouteParameter.Optional,
                AllData = RouteParameter.Optional,
                DeptID = RouteParameter.Optional
            }
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
                name: "Requst Cost Row",
                routeTemplate: "Request/CostRow/{ActivityID}/{Granularity}/{LineID}/{CostType}",
                defaults: new
                {
                    controller = "RequestCostRow",
                    ActivityID = RouteParameter.Optional,
                    Granularity = RouteParameter.Optional,
                    LineID = RouteParameter.Optional,
                    CostType = RouteParameter.Optional
                }
            );
            config.Routes.MapHttpRoute(
               name: "Register Cost Row",
               routeTemplate: "Response/CostRow",
               defaults: new
               {
                   controller = "RegisterCostRow"
               }
           );
            config.Routes.MapHttpRoute(
            name: "RequestCost",
            routeTemplate: "Request/Cost/{ProjectID}/{TrendNumber}/{PhaseCode}/{ActivityID}/{Granularity}/{ViewLabor}/{BudgetID}/{BudgetCategory}/{BudgetSubCategory}",
            //Returns: 

            defaults: new
            {
                controller = "RequestCost",
                ProjectID = RouteParameter.Optional,
                TrendNumber = RouteParameter.Optional,
                PhaseCode = RouteParameter.Optional,
                ActivityID = RouteParameter.Optional,
                Granularity = RouteParameter.Optional,
                ViewLabor = RouteParameter.Optional,
                BudgetID = RouteParameter.Optional,
                BudgetCategory = RouteParameter.Optional,
                BudgetSubCategory = RouteParameter.Optional

            }
            );
            config.Routes.MapHttpRoute(
            name: "RequestGanttViewCost",
            routeTemplate: "Request/GanttViewCost/{ContractID}/{ProjectID}/{ElementID}/{TrendNumber}/{PhaseCode}/{ActivityID}/{Granularity}/{ViewLabor}/{BudgetID}/{BudgetCategory}/{BudgetSubCategory}",
            //Returns: 

            defaults: new
            {
                controller = "RequestGanttViewCost",
                ContractID = RouteParameter.Optional,
                ProjectID = RouteParameter.Optional,
                ElementID = RouteParameter.Optional,
                TrendNumber = RouteParameter.Optional,
                PhaseCode = RouteParameter.Optional,
                ActivityID = RouteParameter.Optional,
                Granularity = RouteParameter.Optional,
                ViewLabor = RouteParameter.Optional,
                BudgetID = RouteParameter.Optional,
                BudgetCategory = RouteParameter.Optional,
                BudgetSubCategory = RouteParameter.Optional

            }
            );
            //    }	
            //        //ProjectID = RouteParameter.Optional, TrendNumber = RouteParameter.Optional,
            //        //                        PhaseCode = RouteParameter.Optional, ActivityID = RouteParameter.Optional, Granularity = RouteParameter.Optional,
            //        //                        BudgetID = RouteParameter.Optional, BudgetCategory = RouteParameter.Optional, BudgetSubCategory = RouteParameter.Optional,
            //        //Role = RouteParameter.Optional }
            //);

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
            name: "RequestODCCost",
            routeTemplate: "Request/ODCCost/{ActivityID}/",
            //Returns:  
            defaults: new { controller = "RequestODCCost", ActivityID = RouteParameter.Optional }
        );

            config.Routes.MapHttpRoute(
            name: "RegisterODCCost",
            routeTemplate: "Response/ODCCost",
            //Returns: Succesfully deleted | Succesfuly inserted

            defaults: new { controller = "RegisterODCCost" }
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
           name: "RequestLookupActivityCategoryByOrganizationID",
           routeTemplate: "Request/ActivityCategoryByOrgID/{OrganizationID}/{VersionID}",
           //Returns:  DT_RowId, CategoryID, CategoryDescription, SubCategoryID, SubCategoryDescription
           defaults: new { controller = "RequestLookupActivityCategory" }
       );

            config.Routes.MapHttpRoute(
           name: "RequestLookupActivityCategory",
           routeTemplate: "Request/ActivityCategory/{CategoryID}/{CategoryDescription}/{SubCategoryID}/{SubCategoryDescription}",
           //Returns:  DT_RowId, CategoryID, CategoryDescription, SubCategoryID, SubCategoryDescription
           defaults: new { controller = "RequestLookupActivityCategory", CategoryID = RouteParameter.Optional, CategoryDescription = RouteParameter.Optional, SubCategoryID = RouteParameter.Optional, SubCategoryDescription = RouteParameter.Optional }
       );


            //getProjectFund
            config.Routes.MapHttpRoute(
                name: "RequestProjectFund",
                routeTemplate: "Request/ProjectFund/{ProjectID}",
                defaults: new { controller = "RequestProjectFund", ProjectID = RouteParameter.Optional }

                );

            config.Routes.MapHttpRoute(
                name: "RegisterProjectFund",
                routeTemplate: "Response/ProjectFund",
                defaults: new { controller = "RegisterProjectFund" }
                );
            config.Routes.MapHttpRoute(
                name: "RequestProgramFund",
                routeTemplate: "Request/ProgramFund/{ProgramID}",
                defaults: new { controller = "RequestProgramFund", ProgramID = RouteParameter.Optional }

                );
            config.Routes.MapHttpRoute(
                name: "RegisterProgramFund",
                routeTemplate: "response/ProgramFund",
                defaults: new { controller = "RegisterProgramFund" }
                );
            config.Routes.MapHttpRoute(
                name: "getTrendFund",
                routeTemplate: "Request/TrendFund/{TrendID}/{ProjectID}",
                defaults: new { controller = "RequestTrendFund", TrenID = RouteParameter.Optional, ProjectID = RouteParameter.Optional }
                );
            config.Routes.MapHttpRoute(
                name: "RegisterTrendFund",
                routeTemplate: "response/TrendFund",
                defaults: new { controller = "RegisterTrendFund" }
                );
            config.Routes.MapHttpRoute(
                name: "MainCategory",
                routeTemplate: "Request/MainActivityCategory/{Phase}/{OrganizationID}/{ProjectId}",
                defaults: new { controller = "RequestMainAC" }
                );
            config.Routes.MapHttpRoute(
                name: "SubCategory",
                routeTemplate: "Request/SubActivityCategory/{OrganizationID}/{Phase}/{CategoryID}/{VersionId}",
                defaults: new { controller = "RequestSubAC", OrganizationID = RouteParameter.Optional, Phase = RouteParameter.Optional, CategoryID = RouteParameter.Optional, VersionId = RouteParameter.Optional }

                );
            //get activity category by program
            config.Routes.MapHttpRoute(
                name: "MainActivityCategoryProgram",
                routeTemplate: "Request/MainActivityCategoryProgram/{ProgramID}/{Phase}",
                defaults: new { controller = "RequestMainActivityCategoryProgram", ProgramID = RouteParameter.Optional, Phase = RouteParameter.Optional }
                );
            config.Routes.MapHttpRoute(
                name: "SubActivityCategoryProgram",
                routeTemplate: "Request/SubActivityCategoryProgram/{ProgramID}/{Phase}/{CategoryID}",
                defaults: new { controller = "RequestSubActivityCategoryProgram", ProgramID = RouteParameter.Optional, Phase = RouteParameter.Optional, Category = RouteParameter.Optional }
                );
            config.Routes.MapHttpRoute(
                name: "ReportModule",
                routeTemplate: "Response/Report/{param1}/{param2}/{ReportType}",
                defaults: new { controller = "RequestReport" }
                );
            config.Routes.MapHttpRoute(
                name: "ReportTableauChart",
                routeTemplate: "Response/Report/{projectID}/{reportType}",
                defaults: new { controller = "RequestReport" }
                );


            #region Reports
            config.Routes.MapHttpRoute( //luan here - Testing SSRS Report
                name: "TestingSSRS",
                routeTemplate: "Request/TestingSSRS",
                defaults: new { controller = "RequestTestingSSRS" }
                );
            config.Routes.MapHttpRoute( //luan here - Summary Report
                name: "SummaryReport",
                routeTemplate: "Request/SummaryReport",
                defaults: new { controller = "RequestSummaryReport" }
                );
            config.Routes.MapHttpRoute( //luan here - Budget Planning Report
                name: "BudgetPlanningReport",
                routeTemplate: "Request/BudgetPlanningReport",
                defaults: new { controller = "RequestBudgetPlanningReport" }
                );
            config.Routes.MapHttpRoute( //luan here - Budget Summary CPP1 Report
                name: "BudgetSummaryCPP1Report",
                routeTemplate: "Request/BudgetSummaryCPP1Report",
                defaults: new { controller = "RequestBudgetSummaryCPP1Report" }
                );
            config.Routes.MapHttpRoute( //luan here - Material Report
                name: "MaterialReport",
                routeTemplate: "Request/MaterialReport",
                defaults: new { controller = "RequestMaterialReport" }
                );
            config.Routes.MapHttpRoute( //luan here - Subcontractor Report
                name: "SubcontractorReport",
                routeTemplate: "Request/SubcontractorReport",
                defaults: new { controller = "RequestSubcontractorReport" }
                );
            config.Routes.MapHttpRoute( //luan here - ODC Report
                name: "ODCReport",
                routeTemplate: "Request/ODCReport",
                defaults: new { controller = "RequestODCReport" }
                );
            config.Routes.MapHttpRoute( //luan here - Budget Summary Report
                name: "BudgetSummaryReport",
                routeTemplate: "Request/BudgetSummaryReport",
                defaults: new { controller = "RequestBudgetSummaryReport" }
                );
            config.Routes.MapHttpRoute( //luan here - FTE Report 
                name: "FTEReport",
                routeTemplate: "Request/FTEReport",
                defaults: new { controller = "RequestFTEReport" }
                );
            config.Routes.MapHttpRoute( //luan here - Purchase Order Detail Report 
                name: "PurchaseOrderDetailReport",
                routeTemplate: "Request/PurchaseOrderDetailReport",
                defaults: new { controller = "RequestPurchaseOrderDetailReport" }
                );
            #endregion

            #region Accounting Reports
            config.Routes.MapHttpRoute( //luan here - Customer Report 
                name: "CustomerReport",
                routeTemplate: "Request/CustomerReport",
                defaults: new { controller = "RequestCustomerReport" }
                );
            config.Routes.MapHttpRoute( //luan here - Sales Order Report 
                name: "SalesOrderReport",
                routeTemplate: "Request/SalesOrderReport",
                defaults: new { controller = "RequestSalesOrderReport" }
                );
            config.Routes.MapHttpRoute( //luan here - Purchase Order Report
                name: "PurchaseOrderReport",
                routeTemplate: "Request/PurchaseORderReport",
                defaults: new { controller = "RequestPurchaseOrderReport" }
                );
            config.Routes.MapHttpRoute( //luan here - QuickBooks Material Report
                name: "QuickBooksMaterialReport",
                routeTemplate: "Request/QuickBooksMaterialReport",
                defaults: new { controller = "RequestQuickBooksMaterialReport" }
                );
            config.Routes.MapHttpRoute( //luan here - QuickBooks Subcontractor Report
                name: "QuickBooksSubcontractorReport",
                routeTemplate: "Request/QuickBooksSubcontractorReport",
                defaults: new { controller = "RequestQuickBooksSubcontractorReport" }
                );
            #endregion

            #region Jonas Reports
            config.Routes.MapHttpRoute( //luan here - Project
                name: "ProjectExportFromCPPIntoJonasReport",
                routeTemplate: "Request/ProjectExportFromCPPIntoJonasReport",
                defaults: new { controller = "RequestProjectExportFromCPPIntoJonasReport" }
                );
            config.Routes.MapHttpRoute( //luan here - Baseline
                name: "BaselineExportFromCPPIntoJonasReport",
                routeTemplate: "Request/BaselineExportFromCPPIntoJonasReport",
                defaults: new { controller = "RequestBaselineExportFromCPPIntoJonasReport" }
                );
            config.Routes.MapHttpRoute( //luan here - Client
                name: "ClientExportFromCPPIntoJonasReport",
                routeTemplate: "Request/ClientExportFromCPPIntoJonasReport",
                defaults: new { controller = "RequestClientExportFromCPPIntoJonasReport" }
                );
            config.Routes.MapHttpRoute( //luan here - Master Inventory
                name: "MasterInventoryExportFromCPPIntoJonasReport",
                routeTemplate: "Request/MasterInventoryExportFromCPPIntoJonasReport",
                defaults: new { controller = "RequestMasterInventoryExportFromCPPIntoJonasReport" }
                );
            #endregion

            #region Admin Reports
            config.Routes.MapHttpRoute( //luan here - Application Security Admin Report
                name: "ApplicationSecurityAdminReport",
                routeTemplate: "Request/ApplicationSecurityAdminReport",
                defaults: new { controller = "RequestApplicationSecurityAdminReport" }
                );
            config.Routes.MapHttpRoute( //luan here - Client Admin Report
                name: "ClientAdminReport",
                routeTemplate: "Request/ClientAdminReport",
                defaults: new { controller = "RequestClientAdminReport" }
                );
            config.Routes.MapHttpRoute( //luan here - Cost Overhead Admin Report
                name: "CostOverheadAdminReport",
                routeTemplate: "Request/CostOverheadAdminReport",
                defaults: new { controller = "RequestCostOverheadAdminReport" }
                );
            config.Routes.MapHttpRoute( //luan here - Document Type Admin Report
                name: "DocumentTypeAdminReport",
                routeTemplate: "Request/DocumentTypeAdminReport",
                defaults: new { controller = "RequestDocumentTypeAdminReport" }
                );
            config.Routes.MapHttpRoute( //luan here - Employee Admin Report
                name: "EmployeeAdminReport",
                routeTemplate: "Request/EmployeeAdminReport",
                defaults: new { controller = "RequestEmployeeAdminReport" }
                );
            config.Routes.MapHttpRoute( //luan here - Material Admin Report
                name: "MaterialAdminReport",
                routeTemplate: "Request/MaterialAdminReport",
                defaults: new { controller = "RequestMaterialAdminReport" }
                );
            config.Routes.MapHttpRoute( //luan here - Material Category Admin Report
                name: "MaterialCategoryAdminReport",
                routeTemplate: "Request/MaterialCategoryAdminReport",
                defaults: new { controller = "RequestMaterialCategoryAdminReport" }
                );
            config.Routes.MapHttpRoute( //luan here - ODC Type Admin Report
                name: "ODCTypeAdminReport",
                routeTemplate: "Request/ODCTypeAdminReport",
                defaults: new { controller = "RequestODCTypeAdminReport" }
                );
            config.Routes.MapHttpRoute( //luan here - Organization Admin Report
                name: "OrganizationAdminReport",
                routeTemplate: "Request/OrganizationAdminReport",
                defaults: new { controller = "RequestOrganizationAdminReport" }
                );
            config.Routes.MapHttpRoute( //luan here - Position Title Admin Report
                name: "PositionTitleAdminReport",
                routeTemplate: "Request/PositionTitleAdminReport",
                defaults: new { controller = "RequestPositionTitleAdminReport" }
                );
            config.Routes.MapHttpRoute( //luan here - Project Approval Requirement Admin Report
                name: "ProjectApprovalRequirementAdminReport",
                routeTemplate: "Request/ProjectApprovalRequirementAdminReport",
                defaults: new { controller = "RequestProjectApprovalRequirementAdminReport" }
                );
            config.Routes.MapHttpRoute( //luan here - Project Element List Admin Report
                name: "ProjectElementListAdminReport",
                routeTemplate: "Request/ProjectElementListAdminReport",
                defaults: new { controller = "RequestProjectElementListAdminReport" }
                );
            config.Routes.MapHttpRoute( //luan here - Project Class Admin Report
                name: "ProjectClassAdminReport",
                routeTemplate: "Request/ProjectClassAdminReport",
                defaults: new { controller = "RequestProjectClassAdminReport" }
                );
            config.Routes.MapHttpRoute( //luan here - Project Class to phase Admin Report
                name: "ProjectClassToPhaseMappingAdminReport",
                routeTemplate: "Request/ProjectClassToPhaseMappingAdminReport",
                defaults: new { controller = "RequestProjectClassToPhaseMappingAdminReport" }
                );
            config.Routes.MapHttpRoute( //luan here - Project Phase Admin Report
                name: "ProjectPhaseAdminReport",
                routeTemplate: "Request/ProjectPhaseAdminReport",
                defaults: new { controller = "RequestProjectPhaseAdminReport" }
                );
            config.Routes.MapHttpRoute( //luan here - Project Type Admin Report
                name: "ProjectTypeAdminReport",
                routeTemplate: "Request/ProjectTypeAdminReport",
                defaults: new { controller = "RequestProjectTypeAdminReport" }
                );
            config.Routes.MapHttpRoute( //luan here - Subcontractor Admin Report
                name: "SubcontractorAdminReport",
                routeTemplate: "Request/SubcontractorAdminReport",
                defaults: new { controller = "RequestSubcontractorAdminReport" }
                );
            config.Routes.MapHttpRoute( //luan here - Subcontractor Type Admin Report
                name: "SubcontractorTypeAdminReport",
                routeTemplate: "Request/SubcontractorTypeAdminReport",
                defaults: new { controller = "RequestSubcontractorTypeAdminReport" }
                );
            config.Routes.MapHttpRoute( //luan here - Trend Status Code Admin Report
                name: "TrendStatusCodeAdminReport",
                routeTemplate: "Request/TrendStatusCodeAdminReport",
                defaults: new { controller = "RequestTrendStatusCodeAdminReport" }
                );
            config.Routes.MapHttpRoute( //luan here - Unit Type Admin Report
                name: "UnitTypeAdminReport",
                routeTemplate: "Request/UnitTypeAdminReport",
                defaults: new { controller = "RequestUnitTypeAdminReport" }
                );
            config.Routes.MapHttpRoute( //luan here - User Management Admin Report
                name: "UserManagementAdminReport",
                routeTemplate: "Request/UserManagementAdminReport",
                defaults: new { controller = "RequestUserManagementAdminReport" }
                );
            config.Routes.MapHttpRoute( //luan here - Work Breakdown Structure Admin Report
                name: "WorkBreakdownStructureAdminReport",
                routeTemplate: "Request/WorkBreakdownStructureAdminReport",
                defaults: new { controller = "RequestWorkBreakdownStructureAdminReport" }
                );
            #endregion

            config.Routes.MapHttpRoute(
                name: "RequestProjectDurationAndCost",
                routeTemplate: "Request/ProjectDurationAndCost/{ProjectID}/{TrendID}",
                defaults: new { controller = "RequestProjectDurationAndCost" }
                );
            /* config.Routes.MapHttpRoute(
             name: "RegisterLookupActivityCategory",
             routeTemplate: "Response/ActivityCategory/{Operation}/{CategoryID}/{CategoryDescription}/{SubCategoryID}/{SubCategoryDescription}",
                 //Returns:  SubCategory 'Land Bidding' already exists in system | Success
             defaults: new { controller = "RegisterLookupActivityCategory", Operation = RouteParameter.Optional, CategoryID = RouteParameter.Optional, CategoryDescription = RouteParameter.Optional, SubCategoryID = RouteParameter.Optional, SubCategoryDescription = RouteParameter.Optional }
         );
             */
            config.Routes.MapHttpRoute(
                name: "CheckUsedProgramCategory",
                routeTemplate: "Request/checkExistingActivity/{ProgramID}/{ActivityCategoryID}",
                defaults: new { controller = "CheckUsedProgramCategory" }
                );
            config.Routes.MapHttpRoute(
         name: "RegisterLookupActivityCategory",
         routeTemplate: "Response/ActivityCategory",
         //Returns:  SubCategory 'Land Bidding' already exists in system | Success
         defaults: new { controller = "RegisterLookupActivityCategory" }
     );

            config.Routes.MapHttpRoute(
         name: "RegisterSingleLookupActivityCategory",
         routeTemplate: "Response/SingleActivityCategory/",
         defaults: new { controller = "RegisterSingleLookupActivityCategory" }
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
           routeTemplate: "Request/FTEPosition/{Id}/{PositionDescription}",
           //Returns:  DT_RowId, PositionID, PositionDescription, MinHourlyRate, MaxHourlyRate, CurrentHourlyRate
           defaults: new { controller = "RequestLookupFTEPosition", Id = RouteParameter.Optional, PositionDescription = RouteParameter.Optional }
       );
            config.Routes.MapHttpRoute(
            name: "RequestLaborRate",
            routeTemplate: "Request/LaborRate/{TrendID}/{PositionID}/{EmployeeID}",
            //Returns:  DT_RowId, PositionID, PositionDescription, MinHourlyRate, MaxHourlyRate, CurrentHourlyRate
            defaults: new { controller = "RequestLaborRate", TrendID = RouteParameter.Optional, PositionID = RouteParameter.Optional, EmployeeID = RouteParameter.Optional }
        );

            config.Routes.MapHttpRoute(
               name: "Request Custom Cost Overhead",
               routeTemplate: "Request/CustomCostOverhead/{TrendID}",
               //Returns:  DT_RowId, PositionID, PositionDescription, MinHourlyRate, MaxHourlyRate, CurrentHourlyRate
               defaults: new { controller = "RequestCustomCostOverhead", TrendID = RouteParameter.Optional }
           );

            config.Routes.MapHttpRoute(
             name: "Apply Custom Cost Overhead",
             routeTemplate: "Response/CustomCostOverhead",
             //Returns:  DT_RowId, PositionID, PositionDescription, MinHourlyRate, MaxHourlyRate, CurrentHourlyRate
             defaults: new { controller = "RegisterCustomCostOverhead" }
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


            config.Routes.MapHttpRoute(
                name: "RequestAnother",
                routeTemplate: "Request/Another/{ProjectID}/{TrendID}",
                defaults: new { controller = "RequestAnother", ProjectID = RouteParameter.Optional, TrendID = RouteParameter.Optional }
                );

            config.Routes.MapHttpRoute(
                name: "RequestProjectScope",
                routeTemplate: "Request/projectscope/{ProjectID}",
                defaults: new { controller = "RequestProjectScope" }
                );

            config.Routes.MapHttpRoute(
                name: "RegisterProjectScope",
                routeTemplate: "Response/ProjectScope",
                defaults: new { controller = "RegisterProjectScope" }

                );

            config.Routes.MapHttpRoute(
                name: "RequestForgotPassowrd",
                routeTemplate: "Response/forgotPassword",
                defaults: new { controller = "RequestForgotPassword" }
                );
            config.Routes.MapHttpRoute(
              name: "RequestUserByToken",
              routeTemplate: "Request/getUserByToken",
              defaults: new { controller = "RequestUserByToken" }
              );
            config.Routes.MapHttpRoute(
                name: "RequestResetPassword",
                routeTemplate: "Response/resetPassword",
                defaults: new { controller = "RequestResetPassword" }
                );
            config.Routes.MapHttpRoute(
                name: "RequestValidateToken",
                routeTemplate: "Response/validateToken",
                defaults: new { controller = "RequestValidateToken" }
                );
            config.Routes.MapHttpRoute(
                name: "RequestGetUser",
                routeTemplate: "Response/getUser",
                defaults: new { controller = "RequestGetUser" }
                );

            //APi for extending a session
            config.Routes.MapHttpRoute(

                name: "RequestSession",
                routeTemplate: "Request/ExtendSession",
                defaults: new { controller = "RequestSessionExtension" }
                );


            // Asset Manager
            config.Routes.MapHttpRoute(
                name: "RequestFacility",
                routeTemplate: "Request/Facility/{FacilityID}",
                defaults: new { controller = "RequestFacilityByID" }
                );
            config.Routes.MapHttpRoute(
                name: "RequestAsset",
                routeTemplate: "Request/Asset/{AssetID}",
                defaults: new { controller = "RequestAssetByID" }
                );
            config.Routes.MapHttpRoute(
                name: "RequestAssetComponent",
                routeTemplate: "Request/Component/{AssetComponentID}",
                defaults: new { controller = "RequestAssetComponentByID" }
                );
            config.Routes.MapHttpRoute(
                name: "RequestAssetHistory",
                routeTemplate: "Request/AssetHistory/{AssetHistoryID}",
                defaults: new { controller = "RequestAssetHistoryByID" }
                );
            config.Routes.MapHttpRoute(
                name: "RequestAssetComponentHistory",
                routeTemplate: "Request/ComponentHistory/{AssetComponentHistoryID}",
                defaults: new { controller = "RequestAssetComponentHistoryByID" }
                );


            config.Routes.MapHttpRoute(
                name: "RegisterAsset",
                routeTemplate: "Response/Asset",
                defaults: new { controller = "RegisterAsset" }
                );
            config.Routes.MapHttpRoute(
                name: "RegisterAssetComponent",
                routeTemplate: "Response/Component",
                defaults: new { controller = "RegisterAssetComponent" }
                );
            config.Routes.MapHttpRoute(
                name: "RegisterAssetHistory",
                routeTemplate: "Response/AssetHistory",
                defaults: new { controller = "RegisterAssetHistory" }
                );
            config.Routes.MapHttpRoute(
                name: "RegisterAssetComponentHistory",
                routeTemplate: "Response/ComponentHistory",
                defaults: new { controller = "RegisterAssetComponentHistory" }
                );

            config.Routes.MapHttpRoute(
                name: "RequestDashboard",
                routeTemplate: "Request/Dashboard/{Command}/{ID}",
               defaults: new { controller = "RequestDashboard" }
               );

            // Dashboard reports
            config.Routes.MapHttpRoute(
                name: "RequestBVAReport",
                routeTemplate: "Request/DashboardReport/BVR/{ProjectID}",
                defaults: new { controller = "RequestBVAReport" }
                );

            config.Routes.MapHttpRoute(
              name: "RequestAllEmployee",
              routeTemplate: "Request/AllEmployee",
              //Returns:  Position 'Architect' already exists in system | Success
              defaults: new { controller = "RequestEmployee" }
          );

            config.Routes.MapHttpRoute(
              name: "RequestEmployeeByOrgID",
              routeTemplate: "Request/Employee/{OrganizationID}",
              //Returns:  Position 'Architect' already exists in system | Success
              defaults: new { controller = "RequestEmployee" }
          );

            config.Routes.MapHttpRoute(
            name: "RegisterEmployee",
            routeTemplate: "Response/Employee",
            defaults: new { controller = "RegisterEmployee" }
            );

            //Materials
            config.Routes.MapHttpRoute(
                  name: "RequestMaterial",
                  routeTemplate: "Request/Material",
                 defaults: new { controller = "RequestMaterial" }
             );

            config.Routes.MapHttpRoute(
                 name: "RegisterMaterial",
                 routeTemplate: "Response/Material",
                 defaults: new { controller = "RegisterMaterial" }
             );

            //ODC Type
            config.Routes.MapHttpRoute(
                  name: "RequestODCType",
                  routeTemplate: "Request/ODCType",
                 defaults: new { controller = "RequestODCType" }
             );

            config.Routes.MapHttpRoute(
                 name: "RegisterODCType",
                 routeTemplate: "Response/ODCType",
                 defaults: new { controller = "RegisterODCType" }
             );

            //Material Category
            config.Routes.MapHttpRoute(
                  name: "RequestMaterialCategory",
                  routeTemplate: "Request/MaterialCategory",
                 defaults: new { controller = "RequestMaterialCategory" }
             );

            config.Routes.MapHttpRoute(
                 name: "RegisterMaterialCategory",
                 routeTemplate: "Response/MaterialCategory",
                 defaults: new { controller = "RegisterMaterialCategory" }
             );

            //Subcontractor Type
            config.Routes.MapHttpRoute(
                  name: "RequestSubcontractorType",
                  routeTemplate: "Request/SubcontractorType",
                 defaults: new { controller = "RequestSubcontractorType" }
             );

            config.Routes.MapHttpRoute(
                 name: "RegisterSubcontractorType",
                 routeTemplate: "Response/SubcontractorType",
                 defaults: new { controller = "RegisterSubcontractorType" }
             );

            //Subcontractor
            config.Routes.MapHttpRoute(
                  name: "RequestSubcontractor",
                  routeTemplate: "Request/Subcontractor",
                 defaults: new { controller = "RequestSubcontractor" }
             );

            config.Routes.MapHttpRoute(
                 name: "RegisterSubcontractor",
                 routeTemplate: "Response/Subcontractor",
                 defaults: new { controller = "RegisterSubcontractor" }
             );

            //Cost Type
            config.Routes.MapHttpRoute(
                  name: "RequestCostType",
                  routeTemplate: "Request/CostType",
                 defaults: new { controller = "RequestCostType" }
             );
            config.Routes.MapHttpRoute(
                  name: "RegisterCostType",
                  routeTemplate: "Response/CostType",
                 defaults: new { controller = "RegisterCostType" }
             );

            //Cost Rate Type
            config.Routes.MapHttpRoute(
                  name: "RequestCostRateType",
                  routeTemplate: "Request/CostRateType",
                 defaults: new { controller = "RequestCostRateType" }
             );
            config.Routes.MapHttpRoute(
                  name: "RegisterCostRateType",
                  routeTemplate: "Response/CostRateType",
                 defaults: new { controller = "RegisterCostRateType" }
             );

            //Cost Type Rate Type
            config.Routes.MapHttpRoute(
                  name: "RequestCostTypeRateType",
                  routeTemplate: "Request/CostTypeRateType",
                 defaults: new { controller = "RequestCostTypeRateType" }
             );
            config.Routes.MapHttpRoute(
                  name: "RegisterCostTypeRateType",
                  routeTemplate: "Response/CostTypeRateType",
                 defaults: new { controller = "RegisterCostTypeRateType" }
             );

            //Cost overhead
            config.Routes.MapHttpRoute(
                  name: "RequestCostOverhead",
                  routeTemplate: "Request/CostOverhead",
                 defaults: new { controller = "RequestCostOverhead" }
             );
            config.Routes.MapHttpRoute(
                  name: "RegisterCostOverhead",
                  routeTemplate: "Response/CostOverhead",
                 defaults: new { controller = "RegisterCostOverhead" }
             );

            // Request estimate
            config.Routes.MapHttpRoute(
                name: "RequestEstimate",
                routeTemplate: "Request/Estimate/{ProjectID}/{TrendNumber}/",
                defaults: new { controller = "RequestEstimate", ProjectID = RouteParameter.Optional, TrendNumber = RouteParameter.Optional }
            );

            // Request estimate
            config.Routes.MapHttpRoute(
                name: "RequestInvoice",
                routeTemplate: "Request/Invoice/{ProjectID}/",
                defaults: new { controller = "RequestInvoice", ProjectID = RouteParameter.Optional }
            );


            //Project Type
            config.Routes.MapHttpRoute(
                  name: "RequestProjectType",
                  routeTemplate: "Request/ProjectType",
                 defaults: new { controller = "RequestProjectType" }
             );

            config.Routes.MapHttpRoute(
                 name: "RegisterProjectType",
                 routeTemplate: "Response/ProjectType",
                 defaults: new { controller = "RegisterProjectType" }
             );

            //Contract
            config.Routes.MapHttpRoute(
                  name: "RequestContract",
                  routeTemplate: "Request/Contract",
                 defaults: new { controller = "RequestContract" }
             );

            config.Routes.MapHttpRoute(
                 name: "RegisterContract",
                 routeTemplate: "Response/Contract",
                 defaults: new { controller = "RegisterContract" }
             );

            //Milestone
            config.Routes.MapHttpRoute(
                  name: "RequestMilestone",
                  routeTemplate: "Request/Milestone",
                 defaults: new { controller = "RequestMilestone" }
             );

            config.Routes.MapHttpRoute(
                 name: "RegisterMilestone",
                 routeTemplate: "Response/Milestone",
                 defaults: new { controller = "RegisterMilestone" }
             );

            //ChangeOrder
            config.Routes.MapHttpRoute(
                  name: "RequestChangeOrder",
                  routeTemplate: "Request/ChangeOrder",
                 defaults: new { controller = "RequestChangeOrder" }
             );

            config.Routes.MapHttpRoute(
                 name: "RegisterChangeOrder",
                 routeTemplate: "Response/ChangeOrder",
                 defaults: new { controller = "RegisterChangeOrder" }
             );

            //Program Contract
            config.Routes.MapHttpRoute(
                  name: "RequestProgramContract",
                  routeTemplate: "Request/ProgramContract",
                 defaults: new { controller = "RequestProgramContract" }
             );

            config.Routes.MapHttpRoute(
                 name: "RegisterProgramContract",
                 routeTemplate: "Response/ProgramContract",
                 defaults: new { controller = "RegisterProgramContract" }
             );

            //Project Class
            config.Routes.MapHttpRoute(
                  name: "RequestProjectClass",
                  routeTemplate: "Request/ProjectClass",
                 defaults: new { controller = "RequestProjectClass" }
             );

            config.Routes.MapHttpRoute(
                  name: "RequestProjectClassByProgramId",
                  routeTemplate: "Request/ProjectClassByProgramId/{programID}",
                 defaults: new { controller = "RequestProjectClassByProgramId" }
             );

            config.Routes.MapHttpRoute(
                  name: "RequestProjectClassByProgramElemId",
                  routeTemplate: "Request/ProjectClassByProgramElemId/{programElemID}",
                 defaults: new { controller = "RequestProjectClassByProgramElemId" }
             );

            config.Routes.MapHttpRoute(
                 name: "RegisterProjectClass",
                 routeTemplate: "Response/ProjectClass",
                 defaults: new { controller = "RegisterProjectClass" }
             );

            //Services Class
            config.Routes.MapHttpRoute(
                  name: "RequestServiceClass",
                  routeTemplate: "Request/ServiceClass",
                 defaults: new { controller = "RequestServiceClass" }
             );
            config.Routes.MapHttpRoute(
                name: "RegisterServiceClass",
                routeTemplate: "Response/ServiceClass",
                defaults: new { controller = "RegisterServiceClass" }
            );

            //Project Class Phase
            config.Routes.MapHttpRoute(
                  name: "RequestProjectClassPhase",
                  routeTemplate: "Request/ProjectClassPhase",
                 defaults: new { controller = "RequestProjectClassPhase" }
             );

            config.Routes.MapHttpRoute(
                 name: "RegisterProjectClassPhase",
                 routeTemplate: "Response/ProjectClassPhase",
                 defaults: new { controller = "RegisterProjectClassPhase" }
             );

            //service to subservice mapping
            config.Routes.MapHttpRoute(
                  name: "RequestServiceToSubservice",
                  routeTemplate: "Request/ServiceToSubserviceMapping",
                 defaults: new { controller = "RequestServiceToSubservice" }
             );

            config.Routes.MapHttpRoute(
                 name: "RegisterServiceToSubsevice",
                 routeTemplate: "Response/ServiceToSubserviceMapping",
                 defaults: new { controller = "RegisterServiceToSubsevice" }
             );
            //Client
            config.Routes.MapHttpRoute(
                  name: "RequestClient",
                  routeTemplate: "Request/Client",
                 defaults: new { controller = "RequestClient" }
             );

            config.Routes.MapHttpRoute(
                 name: "RegisterClient",
                 routeTemplate: "Response/Client",
                 defaults: new { controller = "RegisterClient" }
             );
            config.Routes.MapHttpRoute(                                     //Tanmay - 15/12/2021
                  name: "RequestClientPOC",
                  routeTemplate: "Request/ClientPOC",
                 defaults: new { controller = "RequestClientPOC" }
             );
            config.Routes.MapHttpRoute(
                 name: "RegisterClientPOC",
                 routeTemplate: "Response/ClientPOC",
                 defaults: new { controller = "RegisterClientPOC" }
             );

            //Location
            config.Routes.MapHttpRoute(
                  name: "RequestLocation",
                  routeTemplate: "Request/Location",
                 defaults: new { controller = "RequestLocation" }
             );

            config.Routes.MapHttpRoute(
                 name: "RegisterLocation",
                 routeTemplate: "Response/Location",
                 defaults: new { controller = "RegisterLocation" }
             );

            //Project White List
            config.Routes.MapHttpRoute(
                  name: "RequestProjectWhiteList",
                  routeTemplate: "Request/ProjectWhiteList",
                 defaults: new { controller = "RequestProjectWhiteList" }
             );

            config.Routes.MapHttpRoute(
                 name: "RegisterProjectWhiteList",
                 routeTemplate: "Response/ProjectWhiteList",
                 defaults: new { controller = "RegisterProjectWhiteList" }
             );

            //Document
            config.Routes.MapHttpRoute(
                  name: "RequestDocument",
                  routeTemplate: "Request/Document/{DocumentSet}/{projectID}",
                 defaults: new { controller = "RequestDocument" }
             );
            //config.Routes.MapHttpRoute(
            //      name: "DelDocumentByDocIDs",
            //      routeTemplate: "Request/DelByDocIDs/{docIDs}",
            //     defaults: new { controller = "RequestDocument" }
            // );

            //Document
            config.Routes.MapHttpRoute(
                  name: "RequestDocumentStream",
                  routeTemplate: "Request/DocumentByDocID/{documentID}",
                 defaults: new { controller = "RequestDocument" }
             );

            config.Routes.MapHttpRoute(
                 name: "RegisterDocument",
                 routeTemplate: "Register/Document/{docIDs}",
                 defaults: new { controller = "RegisterDocument" }
             );

            //Document Type
            config.Routes.MapHttpRoute(
                  name: "RequestDocumentType",
                  routeTemplate: "Request/DocumentType",
                 defaults: new { controller = "RequestDocumentType" }
             );

            config.Routes.MapHttpRoute(
                 name: "RegisterDocumentType",
                 routeTemplate: "Response/DocumentType",
                 defaults: new { controller = "RegisterDocumentType" }
             );

            //File Upload
            //config.Routes.MapHttpRoute(
            //     name: "FilesUpload",
            //     routeTemplate: "uploadFiles/Post/{projectID}/{docTypeID}",
            //     defaults: new { controller = "FilesUpload" }
            // );
            //Large File Upload
            config.Routes.MapHttpRoute(
                 name: "LargeFilesUpload",
                 routeTemplate: "uploadFiles/Post/{DocumentSet}/{projectID}/{ProgramElementID}/{ProgramID}/{ContractID}/{ChangeOrderID}/{docTypeID}",
                 defaults: new { controller = "FilesUpload" }
             );

            config.Routes.MapHttpRoute(
               name: "LargeFilesUploadNew",
               routeTemplate: "uploadFilesnew/Postnew/{DocumentSet}/{projectID}/{ProgramElementID}/{ProgramID}/{ContractID}/{ChangeOrderID}/{docTypeID}",
               defaults: new { controller = "FileUploadNew" }
           );
            config.Routes.MapHttpRoute(
                 name: "ServerIPs",
                 routeTemplate: "uploadFiles/GetServerIP",
                 defaults: new { controller = "FilesUpload" }
             );
            // Actuals Upload
            config.Routes.MapHttpRoute(
                 name: "ActualsUpload",
                 routeTemplate: "actualsUploadFiles/Post",
                 defaults: new { controller = "ActualsUpload", action = "Post" }
             );
            //Line of Business lob
            config.Routes.MapHttpRoute(
                    name: "Request Line of Business",
                    routeTemplate: "Request/LineOfBusiness",
                    defaults: new { controller = "RequestLineOfBusiness" }

                );
            config.Routes.MapHttpRoute(
                    name: "Request Phases by Line of Business",
                    routeTemplate: "Request/lobphase/{lobID}",
                    defaults: new { controller = "RequestLobPhase", lobID = RouteParameter.Optional }
                );
            //test adp hours
            config.Routes.MapHttpRoute(
                name: "TEst ADP Hours",
                routeTemplate: "Request/ADPHour",
                defaults: new { controller = "RequestADPHour" }
                );

            // Timesheet Upload
            config.Routes.MapHttpRoute(
                 name: "TimesheetUpload",
                 routeTemplate: "timesheetUploadFiles/Post",
                 defaults: new { controller = "TimesheetUpload" }
             );


            //Project Number
            config.Routes.MapHttpRoute(
                 name: "RequestProjectNumber",
                 routeTemplate: "Request/ProjectNumber/{OrganizationID}",
                 defaults: new { controller = "RequestProjectNumber" }
             );

            //Project Number
            config.Routes.MapHttpRoute(
                 name: "RequestProjectElementNumber",
                 routeTemplate: "Request/ProjectElementNumber/{ProjectNumber}/{OrganizationID}",
                 defaults: new { controller = "RequestProjectElementNumber" }
             );

            //Trend Status Code
            config.Routes.MapHttpRoute(
                  name: "RequestTrendStatusCode",
                  routeTemplate: "Request/TrendStatusCode",
                 defaults: new { controller = "RequestTrendStatusCode" }
             );
            config.Routes.MapHttpRoute(
                 name: "RegisterTrendStatusCode",
                 routeTemplate: "Response/TrendStatusCode",
                 defaults: new { controller = "RegisterTrendStatusCode" }
             );

            //Vendor
            config.Routes.MapHttpRoute(
                  name: "RequestVendor",
                  routeTemplate: "Request/Vendor",
                 defaults: new { controller = "RequestVendor" }
             );

            config.Routes.MapHttpRoute(
                 name: "RegisterVendor",
                 routeTemplate: "Response/Vendor",
                 defaults: new { controller = "RegisterVendor" }
             );

            //BMRequest
            config.Routes.MapHttpRoute(
                  name: "RequestBOMRequest",
                  routeTemplate: "Request/BOMRequest",
                 defaults: new { controller = "RequestBOMRequest" }
             );

            config.Routes.MapHttpRoute(
                 name: "RegisterBOMRequest",
                 routeTemplate: "Response/BOMRequest",
                 defaults: new { controller = "RegisterBOMRequest" }
             );

            //Manufacturer
            config.Routes.MapHttpRoute(
                  name: "RequestManufacturer",
                  routeTemplate: "Request/Manufacturer",
                 defaults: new { controller = "RequestManufacturer" }
             );

            config.Routes.MapHttpRoute(
                 name: "RegisterManufacturer",
                 routeTemplate: "Response/Manufacturer",
                 defaults: new { controller = "RegisterManufacturer" }
             );

            //Inventory
            config.Routes.MapHttpRoute(
                  name: "RequestInventory",
                  routeTemplate: "Request/Inventory",
                 defaults: new { controller = "RequestInventory" }
             );

            config.Routes.MapHttpRoute(
                 name: "RegisterInventory",
                 routeTemplate: "Response/Inventory",
                 defaults: new { controller = "RegisterInventory" }
             );

            //Manasi
            config.Routes.MapHttpRoute(
             name: "ExportToMPP",
             routeTemplate: "Request/Export/{projectId}/{trendNumber}/{granularity}/{phaseId}",
             defaults: new { controller = "ExportToMPP", projectId = RouteParameter.Optional, trendNumber = RouteParameter.Optional, granularity = RouteParameter.Optional, phaseId = RouteParameter.Optional }
            );
            //Manasi
            config.Routes.MapHttpRoute(
             name: "DeleteMPP",
             routeTemplate: "Request/Delete/{fileName}",
             defaults: new { controller = "ExportToMPP", fileName = RouteParameter.Optional }
            );

            //Pritesh Graph
            config.Routes.MapHttpRoute(
           name: "GetGraph",
           routeTemplate: "Request/GetGraph/{ProjectId}",
           //routeTemplate: "Request/GetGraph",
           defaults: new { controller = "Graph", projectId = RouteParameter.Optional }
          );

            //Manasi
            config.Routes.MapHttpRoute(
            name: "GetDocument",
            routeTemplate: "Request/getDocument/{documentId}",
            defaults: new { controller = "FileUploadNew", documentId = RouteParameter.Optional }
           );

            //Swapnil 16-09-2020
            config.Routes.MapHttpRoute(
             name: "RequestMFAConfiguration",
             routeTemplate: "Request/MFAConfiguration",
             defaults: new { controller = "RequestMFAConfiguration" }
         );

            //Swapnil 16-09-2020
            config.Routes.MapHttpRoute(
           name: "RegisterMFAConfiguration",
           routeTemplate: "Response/MFAConfiguration",
           defaults: new { controller = "RegisterMFAConfiguration" }
            );

            //Swapnil 18-09-2020
            config.Routes.MapHttpRoute(
           name: "RequestTrendApprovalReminderMail",
           routeTemplate: "Request/TrendApprovalReminderMail",
           defaults: new { controller = "RequestTrendApprovalReminderMail" }
            );

            ////Swapnil 20-10-2020
            config.Routes.MapHttpRoute(
           name: "RequestExportCostCodeToExcel",
           routeTemplate: "Request/ExportCostCodeToExcel",
           defaults: new { controller = "RequestExportCostCodeToExcel" }
            );

            config.Routes.MapHttpRoute(
    name: "RequestExportInsperityCostCodeToExcel",
    routeTemplate: "Request/ExportInsperityCostCodeToExcel",
    defaults: new { controller = "RequestExportInsperityCostCodeToExcel" }
     );

            ////Swapnil 20-10-2020
            config.Routes.MapHttpRoute(
           name: "RegisterImportCoAdData",
           routeTemplate: "Register/ImportCoAdData",
           defaults: new { controller = "RegisterImportCoAdData" }
            );

            //Jignesh 01-10-2020
            config.Routes.MapHttpRoute(
                 name: "ImportExcel",
                 routeTemplate: "importExcelFiles/Post",
                 defaults: new { controller = "ImportExcel", action = "Post" }
             );
            //Jignesh 01-10-2020
            config.Routes.MapHttpRoute(
                 name: "BillOfMaterialsList",
                 routeTemplate: "billOfMaterials/Get",
                 defaults: new { controller = "BillOfMaterials", action = "Get" }
             );
            //Jignesh 28-10-2020
            config.Routes.MapHttpRoute(
                 name: "ContractModification",
                 routeTemplate: "contractModification/saveContractModification",
                 defaults: new { controller = "ContractModification", action = "Post" }
             );
            //Jignesh 28-10-2020
            config.Routes.MapHttpRoute(
                 name: "GetContractModification",
                 routeTemplate: "contractModification/getContractModificationData/{programId}/{modId}",
                 defaults: new { controller = "ContractModification", action = "Get", programId = RouteParameter.Optional, modId = RouteParameter.Optional }
             );
            //Jignesh 05-11-2020
            config.Routes.MapHttpRoute(
                name: "All Trends with out project id",
                routeTemplate: "Request/GetAllTrendWOProjectId",
                defaults: new { controller = "RequestAllTrendWithoutId" }
            );
            //Jignesh 10-11-2020 For Co-Ad
            config.Routes.MapHttpRoute(
                name: "Co-Ad Import",
                routeTemplate: "Request/ImportCoAdFile",
                defaults: new { controller = "CoAdImport", action = "Post" }
            );

            config.Routes.MapHttpRoute(
              name: "DataMigration",
              routeTemplate: "Request/Migration",
              defaults: new { controller = "DataMigration" }
             );
            //Jignesh 24-11-2020 To Get All Trends For ChangeOrder List 
            config.Routes.MapHttpRoute(
                name: "All Trends For ChangeOrder List",
                routeTemplate: "Request/AllTrendForChangeOrderList/{ProjectID}",
                defaults: new { controller = "RequestTrendsForChangeOrder" }
            );
            // Swapnil 24-11-2020
            config.Routes.MapHttpRoute(
               name: "QuickBooksODCReport",
               routeTemplate: "Request/QuickBooksODCReport",
               defaults: new { controller = "RequestQuickBooksODCReport" }
               );

            //Jignesh 21-12-2020 - Cost Type Admin Report
            config.Routes.MapHttpRoute(
                name: "CostTypeAdminReport",
                routeTemplate: "Request/CostTypeAdminReport",
                defaults: new { controller = "RequestCostTypeAdminReport" }
                );
            //Jignesh 31-12-2020 - Trend Status Admin Report
            config.Routes.MapHttpRoute(
                name: "TrendStatusAdminReport",
                routeTemplate: "Request/TrendStatusAdminReport",
                defaults: new { controller = "RequestTrendStatusReport" }
                );
            //Manasi 05-01-2021 - View gantt at Project level
            config.Routes.MapHttpRoute(
            name: "ViewProgramElementGanttPhase",
            routeTemplate: "Request/Phases/{PhaseDescription}/{Code}/{ProgramElementID}",
            //Returns:  DT_RowId, PhaseID, PhaseDescription, PhaseCode
            defaults: new { controller = "RequestPhaseForGanttAtProjectLevel", PhaseDescription = RouteParameter.Optional, Code = RouteParameter.Optional, ProgramElementID = RouteParameter.Optional }
        );
            //Manasi 05-01-2021 - View gantt at Project level
            config.Routes.MapHttpRoute(
             name: "ViewProgramElementGanttActivity",
             routeTemplate: "Request/Activities/{CostBreakdown}/{ProgramID}/{ProgramElementID}/{ProjectID}/{TrendNumber}/{PhaseCode}/{ActivityID}/{BudgetCategory}/{BudgetSubCategory}",
             defaults: new { controller = "RequestActivitiesForGanttAtProjectLevel", ProgramID = RouteParameter.Optional, ProgramElementID = RouteParameter.Optional, ProjectID = RouteParameter.Optional, TrendNumber = RouteParameter.Optional, PhaseCode = RouteParameter.Optional, ActivityID = RouteParameter.Optional, BudgetCategory = RouteParameter.Optional, BudgetSubCategory = RouteParameter.Optional }
         );
            //Manasi 12-01-2021 - View gantt at Contract level
            config.Routes.MapHttpRoute(
            name: "ViewContractGanttPhase",
            routeTemplate: "Request/ContractPhases/{PhaseDescription}/{Code}/{ProgramID}",
            //Returns:  DT_RowId, PhaseID, PhaseDescription, PhaseCode
            defaults: new { controller = "RequestPhaseForGanttAtContractLevel", PhaseDescription = RouteParameter.Optional, Code = RouteParameter.Optional, ProgramID = RouteParameter.Optional }
        );
            //Manasi 12-01-2021 - View gantt at Contract level
            config.Routes.MapHttpRoute(
             name: "ViewContractGanttActivity",
             routeTemplate: "Request/ContractActivities/{CostBreakdown}/{ProgramID}/{ProgramElementID}/{ProjectID}/{TrendNumber}/{PhaseCode}/{ActivityID}/{BudgetCategory}/{BudgetSubCategory}",
             defaults: new { controller = "RequestActivitiesForGanttAtContractLevel", ProgramID = RouteParameter.Optional, ProgramElementID = RouteParameter.Optional, ProjectID = RouteParameter.Optional, TrendNumber = RouteParameter.Optional, PhaseCode = RouteParameter.Optional, ActivityID = RouteParameter.Optional, BudgetCategory = RouteParameter.Optional, BudgetSubCategory = RouteParameter.Optional }
         );
            // Jignesh-TDM-06-01-2020

            config.Routes.MapHttpRoute(
                name: "GetModificationTypes",
                routeTemplate: "Request/GetModificationTypes",
                defaults: new { controller = "RequestModificationType" }
                );

           // Jignesh - ModificationPopUpChanges
            config.Routes.MapHttpRoute(
               name: "UploadModificationDocument",
               routeTemplate: "uploadModificationDoc/Post/{programID}/{ModId}/{docTypeID}",
               defaults: new { controller = "FileUploadModification" }
           );

            config.Routes.MapHttpRoute(
               name: "UploadTrendDocument",
               routeTemplate: "uploadTrendFile/Post/{projectID}/{TrendNumber}/{docTypeID}",
               defaults: new { controller = "FileUploadTrend" }
           );

            //Manasi 14-01-2021
            config.Routes.MapHttpRoute(
            name: "RequestCostForGanttAtProjectLevel",
            routeTemplate: "Request/ProjectCost/{ProjectID}/{TrendNumber}/{PhaseCode}/{ActivityID}/{Granularity}/{BudgetID}/{ViewLabor}/{BudgetCategory}/{BudgetSubCategory}/{ProgramElementID}",
            //Returns: 

            defaults: new
            {
                controller = "RequestCostForGanttAtProjectLevel",
                ProjectID = RouteParameter.Optional,
                TrendNumber = RouteParameter.Optional,
                PhaseCode = RouteParameter.Optional,
                ActivityID = RouteParameter.Optional,
                Granularity = RouteParameter.Optional,
                BudgetID = RouteParameter.Optional,
                ViewLabor = RouteParameter.Optional,
                BudgetCategory = RouteParameter.Optional,
                BudgetSubCategory = RouteParameter.Optional,
                ProgramElementID = RouteParameter.Optional
            }
            );

            //Swapnil 17-06-2021
            config.Routes.MapHttpRoute(
                name: "RequestVersionDetails",
                routeTemplate: "Request/VersionDetails/{operation}/{programElementID}/{organizationID}/",
                defaults: new { controller = "RequestVersionDetails", operation = RouteParameter.Optional, programElementID = RouteParameter.Optional,
                    organizationID = RouteParameter.Optional
                }
            );

            //User License mapping -- Nivedita 03-11-2021
           /* config.Routes.MapHttpRoute(
                  name: "RequestUserLicenseMapping",
                  routeTemplate: "Request/UserLicenseMapping",
                 defaults: new { controller = "RequestUserLicenseMapping" }
             );*/
            config.Routes.MapHttpRoute(
                name: "RequestUserLicenseMapping",
                routeTemplate: "Request/RequestUserLicenseMapping/{userName}",
               defaults: new { controller = "RequestUserLicenseMapping", userName = RouteParameter.Optional }
           );
            config.Routes.MapHttpRoute(
                 name: "RegisterUserLicenseMapping",
                 routeTemplate: "Response/RegisterUserLicenseMapping",
                 defaults: new { controller = "RegisterUserLicenseMapping" }
             );

            //Poject Element changes by Amruta

            config.Routes.MapHttpRoute(
                 name: "RegisterProgramElementOnChangeOrder",
                 routeTemplate: "Request/ProjectEndDateChangeOrder/{projectId}/{ProjectEndDate}",
                 defaults: new { controller = "RequestProgramElementOnChangeOrder", projectId = RouteParameter.Optional, ProjectEndDate = RouteParameter.Optional }
             );

        }
    }
}