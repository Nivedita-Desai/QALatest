
angular.module('xenon-app').config(function($stateProvider, $httpProvider, $urlRouterProvider, $ocLazyLoadProvider, ASSETS){

    $stateProvider.

        //Added by Rohit Mani
        state('app.cost-gantt', {
            url: '/cost-gantt/:ProjectID/:TrendNumber/:OrganizationID',
            templateUrl: appHelper.templatePath('cost-gantt'),

            controller: 'CostGanttCtrl',
            resolve: {
                resources: function($ocLazyLoad){
                    return $ocLazyLoad.load([
                        //ASSETS.gantt.js,
                        //ASSETS.gantt.gant_export,
                        //ASSETS.gantt.css,
                        ASSETS.chartjs.lib,
                        ASSETS.jspdf
                        //"http://export.dhtmlx.com/gantt/api.js"
                    ]);
                },
                delayedData: function($stateParams, $q, Activity, PhaseCode, Project, Material, Employee, TrendCostOverhead) { // Inject resources named 'Activity' and 'Phases'
                    console.log($stateParams);
                    // Set up our resource calls
                    var ActivityData = Activity.get({ ProjectID: $stateParams.ProjectID, TrendNumber: $stateParams.TrendNumber, ProgramID: 'null', ProgramElementID: 'null' });
                    var PhaseData = PhaseCode.get({ PhaseDescription: 'null', Code: 'null', ProjectID: $stateParams.ProjectID });   //luan here
                    var Project = Project.lookup().get({ ProgramID: 'null', ProgramElementID: 'null', ProjectID: $stateParams.ProjectID });
                    var Materials = Material.get({});
                    var Employees = Employee.get({ OrganizationID: $stateParams.OrganizationID });
                    var CostOverhead = TrendCostOverhead.get( {ProjectID : $stateParams.ProjectID , TrendNumber :  $stateParams.TrendNumber });
                    
                    if($stateParams.TrendNumber === "0"){
                        isBaseline = true;
                    }else{
                        isBaseline= false;
                    }

                    return $q.all([ActivityData.$promise, PhaseData.$promise, Project.$promise, $stateParams.TrendNumber, isBaseline, $stateParams.OrganizationID, Materials.$promise, Employees.$promise,CostOverhead.$promise]);
                }
            }
        }).
        //Manasi 04-01-2021
        state('app.view-gantt-Project', {
            url: '/view-gantt-Project/:ProgramElementID/:TrendNumber/:OrganizationID',
            templateUrl: appHelper.templatePath('view-gantt-Project'),

            controller: 'ViewProgramElementGanttCtrl',
            resolve: {
                resources: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        //ASSETS.gantt.js,
                        //ASSETS.gantt.gant_export,
                        //ASSETS.gantt.css,
                        ASSETS.chartjs.lib,
                        ASSETS.jspdf
                        //"http://export.dhtmlx.com/gantt/api.js"
                    ]);
                },
                delayedData: function ($stateParams, $q, Project, Material, Employee, ViewProjectGanttActivities, ViewProjectGanttPhase, ProgramElement) { // Inject resources named 'Activity' and 'Phases'
                    console.log($stateParams);
                    // Set up our resource calls
                    var ActivityData = ViewProjectGanttActivities.get({ ProjectID: 'null', TrendNumber: $stateParams.TrendNumber, ProgramID: 'null', ProgramElementID: $stateParams.ProgramElementID });
                    var PhaseData = ViewProjectGanttPhase.get({ PhaseDescription: 'null', Code: 'null', ProgramElementID: $stateParams.ProgramElementID });   //luan here
                    var Project = Project.lookup().get({ ProgramID: 'null', ProgramElementID: $stateParams.ProgramElementID, ProjectID: 'null' });
                    var Materials = Material.get({});
                    var Employees = Employee.get({ OrganizationID: $stateParams.OrganizationID });
                    //var CostOverhead = TrendCostOverhead.get({ ProjectID: $stateParams.ProjectID, TrendNumber: $stateParams.TrendNumber });
                    var ProgramElement = ProgramElement.lookup().get({ ProgramID: 'null', ProgramElementID: $stateParams.ProgramElementID });

                    if ($stateParams.TrendNumber === "0") {
                        isBaseline = true;
                    } else {
                        isBaseline = false;
                    }

                    return $q.all([ActivityData.$promise, PhaseData.$promise, Project.$promise, $stateParams.TrendNumber, isBaseline, $stateParams.OrganizationID, Materials.$promise, Employees.$promise, ProgramElement.$promise]);  //, CostOverhead.$promise
                }
            }
        }).
        //Manasi 11-01-2021
        state('app.view-gantt-Contract', {
            url: '/view-gantt-Contract/:ProgramID/:TrendNumber/:OrganizationID',
            templateUrl: appHelper.templatePath('view-gantt-Contract'),

            controller: 'ViewContractGanttCtrl',
            resolve: {
                resources: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        //ASSETS.gantt.js,
                        //ASSETS.gantt.gant_export,
                        //ASSETS.gantt.css,
                        ASSETS.chartjs.lib,
                        ASSETS.jspdf
                        //"http://export.dhtmlx.com/gantt/api.js"
                    ]);
                },
                delayedData: function ($stateParams, $q, Project, Material, Employee, ViewContractGanttActivities, ViewContractGanttPhase, ProgramElement, Program) { // Inject resources named 'Activity' and 'Phases'
                    console.log($stateParams);
                    // Set up our resource calls
                    var ActivityData = ViewContractGanttActivities.get({ ProjectID: 'null', TrendNumber: $stateParams.TrendNumber, ProgramID: $stateParams.ProgramID, ProgramElementID: 'null' });
                    var PhaseData = ViewContractGanttPhase.get({ PhaseDescription: 'null', Code: 'null', ProgramID: $stateParams.ProgramID });   //luan here
                    var Project = Project.lookup().get({ ProgramID: $stateParams.ProgramID, ProgramElementID: 'null', ProjectID: 'null' });
                    var Materials = Material.get({});
                    var Employees = Employee.get({ OrganizationID: $stateParams.OrganizationID });
                    //var CostOverhead = TrendCostOverhead.get({ ProjectID: $stateParams.ProjectID, TrendNumber: $stateParams.TrendNumber });
                    var ProgramElement = ProgramElement.lookup().get({ ProgramID: $stateParams.ProgramID, ProgramElementID: 'null' });
                    var Program = Program.lookup().get({ ProgramID: $stateParams.ProgramID, OrganizationID: $stateParams.OrganizationID });

                    if ($stateParams.TrendNumber === "0") {
                        isBaseline = true;
                    } else {
                        isBaseline = false;
                    }

                    return $q.all([ActivityData.$promise, PhaseData.$promise, Project.$promise, $stateParams.TrendNumber, isBaseline, $stateParams.OrganizationID, Materials.$promise, Employees.$promise, ProgramElement.$promise, Program.$promise, $stateParams.ProgramID]);  //, CostOverhead.$promise
                }
            }
        }).
        state('app.dashboard',{
           url: '/dashboard',
            templateUrl : appHelper.templatePath('dashboard'),
            controller: 'DashboardCtrl',
            resolve: {
                delayedData: function(){

                }
            }
        }).
        state('app.baseline-project',{
            url: '/baseline-project/:ProjectID/:TrendNumber/:OrganizationID',
            templateUrl: appHelper.templatePath('baseline-project'),

            controller: 'BaselineCtrl',
            resolve: {
                resources: function($ocLazyLoad){
                    return $ocLazyLoad.load([

                        //ASSETS.gantt.css,
                        //ASSETS.gantt.js,
                        //ASSETS.chartjs.lib
                    ]);
                },
                delayedData: function($stateParams, $q, Activity, PhaseCode, Project) { // Inject resources named 'Activity' and 'Phases'
                    console.log($stateParams);
                    // Set up our resource calls
                    alert();
                    var ActivityData = Activity.get({ProjectID:$stateParams.ProjectID, TrendNumber:$stateParams.TrendNumber, ProgramID: 'null', ProgramElementID: 'null'});
                    var PhaseData = PhaseCode.get({});  //TODO ? Custom phases?

                    var Project = Project.get({ProgramID:'null',ProgramElementID:'null',ProjectID:$stateParams.ProjectID});


                    return $q.all([ActivityData.$promise, PhaseData.$promise, Project.$promise,$stateParams.TrendNumber, $stateParams.OrganizationID]);
                }
            }
        }).
        //Current Project
        state('app.current-project',{
            url: '/current-project/:ProjectID/:OrgaaasfsanizationID/:Granularity',
            templateUrl: appHelper.templatePath('baseline-project'),

            controller: 'BaselineCtrl',
            resolve: {
                resources: function($ocLazyLoad){
                    return $ocLazyLoad.load([
                        //ASSETS.gantt.css,
                        //ASSETS.gantt.js,
                        //ASSETS.chartjs.lib
                    ]);
                },
                delayedData: function($stateParams, $q, currentProject) { // Inject resources named 'Activity' and 'Phases'
                    console.log($stateParams);
                    // Set up our resource calls
                    var current = currentProject.get({ProjectID:$stateParams.ProjectID, Granularity: 'week'});
                    console.log(current);

                    return $q.all([current.$promise, $stateParams.TrendNumber, $stateParams.OrganizationID]);
                }
            }
        }).
        //Futture Project
        state('app.future-project',{
            url: '/future-project/:ProjectID/:OrganizationID/:Granularity',
            templateUrl: appHelper.templatePath('future-project'),

            controller: 'FutureProjectCtrl',
            resolve: {
                resources: function($ocLazyLoad){
                    return $ocLazyLoad.load([
                        //ASSETS.gantt.css,
                        //ASSETS.gantt.js,
                        //ASSETS.chartjs.lib
                    ]);
                },
                delayedData: function($stateParams, $q, futureProject) { // Inject resources named 'Activity' and 'Phases'
                    console.log($stateParams);
                    // Set up our resource calls
                    var future = futureProject.get({ProjectID:$stateParams.ProjectID, Granularity : 'week'});
                    console.log(future);
                    return $q.all([future.$promise, $stateParams.TrendNumber, $stateParams.OrganizationID]);
                }
            }
        }).
        state('app.wbs', {
            url: '/wbs',
            templateUrl: appHelper.templatePath('wbs'),
            controller: 'WBSCtrl',
            resolve: {
                resources: function($ocLazyLoad){
                    return $ocLazyLoad.load([{
                        files: [
                            ASSETS.wbs.dagre,
                            ASSETS.wbs.mxn,
                            ASSETS.wbs.mxnCore,
                            ASSETS.wbs.mxnGoogleCore,
                            ASSETS.wbs.timeline,
                            //ASSETS.wbs.timemap,
                            //ASSETS.projectgis.customecolorpicker,
                            //ASSETS.projectgis.customColorPicker,
                            ASSETS.projectgis.project_gis
                        ]
                    },{
                        files: [
                            ASSETS.wbs.jsonLoader,
                            ASSETS.wbs.css,
                            ASSETS.wbs.example,
                            ASSETS.chartjs.lib]


                    }]);
                }
            }
        }).
         state('app.bom', {
                    url: '/bom',
                    templateUrl: appHelper.templatePath('bom'),
                    controller: 'BOMCtrl',
                    resolve: {
                        resources: function ($ocLazyLoad) {
                            return $ocLazyLoad.load([{
                                files: [
                                    //ASSETS.wbs.dagre,
                                    //ASSETS.wbs.mxn,
                                    //ASSETS.wbs.mxnCore,
                                    //ASSETS.wbs.mxnGoogleCore,
                                    //ASSETS.wbs.timeline,
                                    ////ASSETS.wbs.timemap,
                                    ////ASSETS.projectgis.customecolorpicker,
                                    ////ASSETS.projectgis.customColorPicker,
                                    //ASSETS.projectgis.project_gis
                                ]
                            }, {
                                files: [
                                    //ASSETS.wbs.jsonLoader,
                                    //ASSETS.wbs.css,
                                    //ASSETS.wbs.example,
                                    //ASSETS.chartjs.lib
                                ]


                            }]);
                        }
                    }
                }).
        state('app.admin-access-control', {
            url: '/admin-access-control',
            templateUrl: appHelper.templatePath('admin/access-control'),
            controller: 'AccessControlCtrl'
        }).
        state('app.admin-budget-categories', {
            url: '/admin-budget-categories',
            templateUrl: appHelper.templatePath('admin/budget-categories'),
            controller: 'BudgetCategoryCtrl'
        }).
        state('app.admin-positions', {
            url: '/admin-positions',
            templateUrl: appHelper.templatePath('admin/positions'),
            controller: 'PositionCtrl'
        }).
        state('app.admin-approval-matrix',{
            url:'/admin-approval-matrix',
            templateUrl: appHelper.templatePath('admin/approval-matrix'),
            controller:'ApprovalMatrixCtrl'
        }).
        state('app.admin-phase-code',{
            url:'/admin-phase-code',
            templateUrl: appHelper.templatePath('admin/phase-code'),
            controller:'PhaseCodeCtrl'
        }).
        state('app.admin-fund-type',{
            url:'/admin-fund-type',
            templateUrl: appHelper.templatePath('admin/fund-type'),
            controller : 'FundTypeCtrl'
        }).
        state('app.admin-asset-manager',{
            url:'/admin-asset-manager',
            templateUrl: appHelper.templatePath('admin/asset-manager'),
            controller: 'AssetManagerCtrl'
        }).
        state('app.admin-users',{
            url:'/admin-users',
            templateUrl: appHelper.templatePath('admin/Users'),
            controller:'UserCtrl'
        }).
        state('app.admin-unit-type',{
            url:'/admin-unit-type',
            templateUrl: appHelper.templatePath('admin/unit-type'),
            controller : 'UnitTypeCtrl'
        }).
        state('app.admin-organization', {
            url: '/admin-organization',
            templateUrl: appHelper.templatePath('admin/organization'),
            controller: 'OrganizationCtrl'
        }).
        state('app.po-Approval', {
            url: '/po-Approval/:poid',
            templateUrl: appHelper.templatePath('po-approval'),
            controller: 'POApprovalCtrl'
        }).
        state('app.admin-employee', {
            url: '/admin-employee',
            templateUrl: appHelper.templatePath('admin/employee'),
            controller: 'EmployeeCtrl'
        }).
        state('app.admin-report', {
            url: '/admin-report',
            templateUrl: appHelper.templatePath('admin/report'),
            controller: 'AdminReportManagerCtrl'
        }).
        state('app.admin-odc-type', {
            url: '/admin-odc-type',
            templateUrl: appHelper.templatePath('admin/odc-type'),
            controller: 'ODCTypeCtrl'
        }).
        state('app.admin-material-category', {
            url: '/admin-material-category',
            templateUrl: appHelper.templatePath('admin/material-category'),
            controller: 'MaterialCategoryCtrl'
        }).
        state('app.admin-material', {
            url: '/admin-material',
            templateUrl: appHelper.templatePath('admin/material'),
            controller: 'MaterialCtrl'
        }).
        state('app.admin-subcontractor-type', {
            url: '/admin-subcontractor-type',
            templateUrl: appHelper.templatePath('admin/subcontractor-type'),
            controller: 'SubcontractorTypeCtrl'
        }).
        state('app.admin-subcontractor', {
            url: '/admin-subcontractor',
            templateUrl: appHelper.templatePath('admin/subcontractor'),
            controller: 'SubcontractorCtrl'
        }).
        state('app.admin-cost-overhead', {
            url: '/admin-cost-overhead',
            templateUrl: appHelper.templatePath('admin/cost-overhead'),
            controller: 'CostOverheadCtrl'
        }).
        state('app.admin-project-type', {
            url: '/admin-project-type',
            templateUrl: appHelper.templatePath('admin/project-type'),
            controller: 'ProjectTypeCtrl'
        }).
        state('app.admin-project-class', {
            url: '/admin-project-class',
            templateUrl: appHelper.templatePath('admin/project-class'),
            controller: 'ProjectClassCtrl'
        }).
        state('app.admin-service-class', {
            url: '/admin-service-class',
            templateUrl: appHelper.templatePath('admin/service-class'),
            controller: 'ServiceClassCtrl'
        }).
        state('app.admin-project-class-phase', {
            url: '/admin-project-class-phase',
            templateUrl: appHelper.templatePath('admin/project-class-phase'),
            controller: 'ProjectClassPhaseCtrl'
        }).
        state('app.admin-service-to-subservice-mapping', {
            url: '/admin-service-to-subservice-mapping',
            templateUrl: appHelper.templatePath('admin/service-to-subservice-mapping'),
            controller: 'ServiceToSubserviceCtrl'
        }).

        state('app.admin-client', {
            url: '/admin-client',
            templateUrl: appHelper.templatePath('admin/client'),
            controller: 'ClientCtrl'
        }).
        //  Tanmay - 07/12/2021
        state('app.admin-clientPOC', {
            url: '/admin-clientPOC',
            templateUrl: appHelper.templatePath('admin/clientPOC'),
            controller: 'ClientPOCCtrl'
        }).
        state('app.admin-document-type', {
            url: '/admin-document-type',
            templateUrl: appHelper.templatePath('admin/document-type'),
            controller: 'DocumentTypeCtrl'
        }).
        state('app.admin-actuals-upload', {
            url: '/admin-actuals-upload',
            templateUrl: appHelper.templatePath('admin/actuals-upload'),
            controller: 'ActualsUploadCtrl'
        }).
        state('app.admin-trend-status-code', {
            url: '/admin-trend-status-code',
            templateUrl: appHelper.templatePath('admin/trend-status-code'),
            controller: 'TrendStatusCodeCtrl'
        }).
        state('app.admin-vendor', {
            url: '/admin-vendor',
            templateUrl: appHelper.templatePath('admin/vendor'),
            controller: 'VendorCtrl'
        }).
        state('app.admin-manufacturer', {
            url: '/admin-manufacturer',
            templateUrl: appHelper.templatePath('admin/manufacturer'),
            controller: 'ManufacturerCtrl'
        }).
        state('app.admin-inventory', {
            url: '/admin-inventory',
            templateUrl: appHelper.templatePath('admin/inventory'),
            controller: 'InventoryCtrl'
        }).
         state('app.admin-location', {
             url: '/admin-location',
             templateUrl: appHelper.templatePath('admin/territory'),
             controller: 'TerritoryCtrl'
         }).

         state('app.admin-agilegrid', {
             url: '/admin-agilegrid',
             templateUrl: appHelper.templatePath('admin/agilegrid'),
             controller: 'AgilegridCtrl'
         }).
                 state('app.admin-filedownload', {
                     url: '/admin-filedownload',
                     templateUrl: appHelper.templatePath('admin/filedownload'),
                     controller: 'FiledownloadCtrl'
                 }).

        state('app.admin-whitelist', {
            url: '/admin-whitelist',
            templateUrl: appHelper.templatePath('admin/whitelist'),
            controller: 'WhitelistCtrl'
        }).

        // Logins and Lockscreen
        state('login', {
            url: '/login',
            templateUrl: appHelper.templatePath('login'),
            controller: 'LoginCtrl',
            resolve: {
                resources: function($ocLazyLoad){
                    return $ocLazyLoad.load([
                        ASSETS.forms.jQueryValidate,
                        ASSETS.extra.toastr,
                    ]);
                },
            }
        }).
        state('login-light', {
            url: '/login-light',
            templateUrl: appHelper.templatePath('login-light'),
            controller: 'LoginLightCtrl',
            resolve: {
                resources: function($ocLazyLoad){
                    return $ocLazyLoad.load([
                        ASSETS.forms.jQueryValidate,
                    ]);
                },
            }
        }).
        state('lockscreen', {
            url: '/lockscreen',
            templateUrl: appHelper.templatePath('lockscreen'),
            controller: 'LockscreenCtrl',
            resolve: {
                resources: function($ocLazyLoad){
                    return $ocLazyLoad.load([
                        ASSETS.forms.jQueryValidate,
                        ASSETS.extra.toastr,
                    ]);
                },
            }
        }).
        state('signup', {
            url: '/signup',
            templateUrl: appHelper.templatePath('signup'),
            controller: 'SignupCtrl',
            resolve: {
                resources: function($ocLazyLoad){
                    return $ocLazyLoad.load([
                        ASSETS.forms.jQueryValidate,
                        ASSETS.extra.toastr,
                    ]);
                },
            }
        }).
        state('password-recovery',{
            url:'/password-recovery',
            templateUrl : appHelper.templatePath('forgot-password'),
            controller : "PasswordRecoveryCtrl",
            resolve:{
                resources: function($ocLazyLoad){
                    return $ocLazyLoad.load([
                        ASSETS.forms.jQueryValidate,
                        ASSETS.extra.toastr,
                    ]);
                },
            }
        }).
        state('change-password', {
            url: '/change-password',
            templateUrl: appHelper.templatePath('change-password'),
            controller: "ChangePasswordOnLoginCtrl",
            resolve: {
                resources: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        ASSETS.forms.jQueryValidate,
                        ASSETS.extra.toastr,
                    ]);
                },
            }
        }).
        state('password-update', {
            url: '/update-password/?token',
            templateUrl: appHelper.templatePath('password-update'),
            controller: "PasswordUpdateCtrl",
            resolve: {
                resources: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        ASSETS.forms.jQueryValidate,
                        ASSETS.extra.toastr,
                    ]);
                },
            }
        }).
        //testing authentication
        state('app.test-login',{
            url:'/test-login',
            templateUrl:appHelper.templatePath('test/login'),
            controller :'loginController'

        }).
        //testing authentication
        state('app.test-signup',{
            url:'/test-signup',
            templateUrl:appHelper.templatePath('test/signup'),
            controller :'signupController'

        }).
        state('app.order',{
            url:'/order',
            templateUrl:appHelper.templatePath('/order'),
            controller:'orderController'

        }).
        state('app.admin-statuses', {
            url: '/admin-statuses',
            templateUrl: appHelper.templatePath('admin/statuses'),
            controller: 'TrendStatusCtrl'
        }).
        state('app.const-timeline', {
            url: '/const-timeline',
            templateUrl: appHelper.templatePath('const-timeline'),
            controller: 'ConstTimelineCtrl',
            resolve: {
                resources: function($ocLazyLoad){

                    return $ocLazyLoad.load([{
                        files: [
                            //ASSETS.timemap.mxn,
                            //ASSETS.timemap.example,
                            //ASSETS.timemap.jsonLoader,
                            ASSETS.timemap.timeline
                        ]
                    }, {
                        files: [
                            ASSETS.timemap.mxnCore,
                            ASSETS.timemap.timemap
                        ]
                    },{
                        files: [

                            ASSETS.timemap.timeline,
                        ]
                    }
                        ,{
                            files: [

                                ASSETS.timemap.mxnGoogleCore
                            ]
                        }]);
                }
            }
        }).
        state('app.project-location', {
            url: '/project-location',
            templateUrl: appHelper.templatePath('project-location'),
            controller: 'LocationCtrl',
            resolve: {
                resources: function($ocLazyLoad){
                    return $ocLazyLoad.load([{
                        files: [
                            ASSETS.projectgis.customecolorpicker,
                            ASSETS.projectgis.customColorPicker,
                            ASSETS.projectgis.project_gis
                        ]
                    }]);
                }
            }
        }).
        state('app.admin-Chart', {  //Manasi 21-09-2020
            url: '/admin-Chart',
            templateUrl: appHelper.templatePath('admin/Chart'),
            controller: 'ChartCtrl'
        });


});
