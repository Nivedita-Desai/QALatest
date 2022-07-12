angular.module('cpp.controllers').
    controller('AdminReportManagerCtrl', ['$scope', '$timeout', '$uibModal', '$rootScope', '$http', 'Program', 'ProgramElement', 'Page', 'Project', 'Trend', 'MaterialCategory', 'SubcontractorType', 'fteposition', 'Organization', 'PhaseCode', 'ProjectClass', 'ProjectClassPhase',
        function ($scope, $timeout, $uibModal, $rootScope, $http, Program, ProgramElement, Page, Project, Trend, MaterialCategory, SubcontractorType, fteposition, Organization, PhaseCode, ProjectClass, ProjectClassPhase) {

            Page.setTitle('Admin Report Manager');

            $('.modal-backdrop').hide();

            (function () {
                if (window.history && window.history.pushState) {
                    $(window).on('popstate', function () {
                        $scope.$close('close');
                    });
                }
            })();

            //initializations
            $scope.selectedProgram = {};
            $scope.selectedProgramElement = {};
            $scope.selectedProject = {};
            $scope.selectedTrend = {};
            $scope.selectedTrendStatus = null; // Jignesh 31-12-2020
            $scope.selectedMaterialCategory = {};
            $scope.selectedSubcontractorType = {};
            $scope.selectedPosition = {};

            $scope.allOrganizationList = [];
            $scope.allProgramList = [];
            $scope.allProgramElementList = [];
            $scope.allProjectList = [];
            $scope.allTrendList = [];
            $scope.allMaterialCategoryList = [];
            $scope.allSubcontractorTypeList = [];
            $scope.allPositionList = [];
            $scope.allProjectClassList = [];
            $scope.allPhaseList = [];
            $scope.allProjectClassPhaseList = [];

            $scope.currentProjectList = [];
            $scope.currentTrendList = [];

            //List of selectable report types with relating info



            $scope.filedateformat = "DDMMYY_hhmm";
            $scope.filedateformat = moment().format($scope.filedateformat);

            //Jignesh 31-12-2020
            $scope.trendStatusList = [
                { filterName: 'Approved' },
                { filterName: 'Not Approved' },
                { filterName: 'Acceptance' }
            ]

            $scope.reportTypeList = [
                //Commented by Manasi
                //{ filterName: 'Project Export From CPP Into Jonas',						reportPathName: 'ProjectExportFromCPPIntoJonas',				fileName: 'Jonas Import Project',						reportGroup: 'Jonas Premier',	filterLess: false },
                //{ filterName: 'Baseline Export From CPP Into Jonas',					reportPathName: 'BaselineExportFromCPPIntoJonas',				fileName: 'Jonas Import Baseline',						reportGroup: 'Jonas Premier',	filterLess: false,	reportVersion: '1' },
                //{ filterName: 'Baseline Pair Revenue Export From CPP Into Jonas',		reportPathName: 'BaselineExportFromCPPIntoJonas',				fileName: 'Jonas Import Baseline',						reportGroup: 'Jonas Premier',	filterLess: false,	reportVersion: '2' },
                //{ filterName: 'Baseline One Total Revenue Export From CPP Into Jonas',	reportPathName: 'BaselineExportFromCPPIntoJonas',				fileName: 'Jonas Import Baseline',						reportGroup: 'Jonas Premier',	filterLess: false,	reportVersion: '3' },
                //{ filterName: 'Customer Export From CPP Into Jonas',					reportPathName: 'ClientExportFromCPPIntoJonasReport',			fileName: 'Client Uploads',								reportGroup: 'Jonas Premier',	filterLess: true },
                //{ filterName: 'Inventory Export From CPP Into Jonas',					reportPathName: 'MasterInventoryExportFromCPPIntoJonasReport',	fileName: 'Master Inventory Uploads',					reportGroup: 'Jonas Premier',	filterLess: false },

                { filterName: 'Customer Report', reportPathName: 'CustomerReport', fileName: 'Customer Report' + '_' + + $scope.filedateformat, reportGroup: 'Accounting', filterLess: false },
                { filterName: 'Sales Order Report', reportPathName: 'SalesOrderReport', fileName: 'Sales Order Report' + '_' + + $scope.filedateformat, reportGroup: 'Accounting', filterLess: false },
                { filterName: 'Purchase Order Report', reportPathName: 'PurchaseOrderReport', fileName: 'Purchase Order Report' + '_' + + $scope.filedateformat, reportGroup: 'Accounting', filterLess: false },
                { filterName: 'QuickBooks Material Report', reportPathName: 'QuickBooksMaterialReport', fileName: 'QuickBooks Material Report' + '_' + + $scope.filedateformat, reportGroup: 'Accounting', filterLess: true },
                { filterName: 'QuickBooks Subcontractor Report', reportPathName: 'QuickBooksSubcontractorReport', fileName: 'QuickBooks Subcontractor Report' + '_' + + $scope.filedateformat, reportGroup: 'Accounting', filterLess: true },

                // Swapnil 24-11-2020
                { filterName: 'QuickBooks ODC Report', reportPathName: 'QuickBooksODCReport', fileName: 'QuickBooks ODC Report' + '_' + + $scope.filedateformat, reportGroup: 'Accounting', filterLess: true },
                

                { filterName: 'Application Security Admin Report', reportPathName: 'ApplicationSecurityAdminReport', fileName: 'Application Security Admin Report' + '_' + + $scope.filedateformat, reportGroup: 'Administration', filterLess: true },
                { filterName: 'Client Admin Report', reportPathName: 'ClientAdminReport', fileName: 'Client Admin Report' + '_' + + $scope.filedateformat, reportGroup: 'Administration', filterLess: true },
                { filterName: 'Cost Overhead Admin Report', reportPathName: 'CostOverheadAdminReport', fileName: 'Cost Overhead Admin Report' + '_' + + $scope.filedateformat, reportGroup: 'Administration', filterLess: true },
                { filterName: 'Department Admin Report', reportPathName: 'ProjectClassAdminReport', fileName: 'Division Admin Report' + '_' + + $scope.filedateformat, reportGroup: 'Administration', filterLess: true },
                { filterName: 'Department To Phase Mapping Admin Report', reportPathName: 'ProjectClassToPhaseMappingAdminReport', fileName: 'Division To Phase Mapping Admin Report' + '_' + + $scope.filedateformat, reportGroup: 'Administration', filterLess: true },
                { filterName: 'Document Type Admin Report', reportPathName: 'DocumentTypeAdminReport', fileName: 'Document Type Admin Report' + '_' + + $scope.filedateformat, reportGroup: 'Administration', filterLess: true },
                { filterName: 'Organization Admin Report', reportPathName: 'OrganizationAdminReport', fileName: 'Organization Admin Report' + '_' + + $scope.filedateformat, reportGroup: 'Administration', filterLess: true },
                // { filterName: 'Project Type Admin Report',								reportPathName: 'ProjectTypeAdminReport',						fileName: 'Project Type Admin Report',                  reportGroup: 'Administration',  filterLess: true}, Commneted By Manasi
                { filterName: 'Project Approval Requirement Admin Report', reportPathName: 'ProjectApprovalRequirementAdminReport', fileName: 'Project Approval Requirement Admin Report' + '_' + + $scope.filedateformat, reportGroup: 'Administration', filterLess: true },
                { filterName: 'Project Element List Admin Report', reportPathName: 'ProjectElementListAdminReport', fileName: 'Project Element List Admin Report' + '_' + + $scope.filedateformat, reportGroup: 'Administration', filterLess: false },
                { filterName: 'Project Phase Admin Report', reportPathName: 'ProjectPhaseAdminReport', fileName: 'Project Phase Admin Report' + '_' + + $scope.filedateformat, reportGroup: 'Administration', filterLess: true },
                { filterName: 'Trend Status Code Admin Report', reportPathName: 'TrendStatusCodeAdminReport', fileName: 'Trend Status Code Admin Report' + '_' + + $scope.filedateformat, reportGroup: 'Administration', filterLess: true },
                { filterName: 'Unit Type Admin Report', reportPathName: 'UnitTypeAdminReport', fileName: 'Unit Type Admin Report' + '_' + + $scope.filedateformat, reportGroup: 'Administration', filterLess: true },
                { filterName: 'User Management Admin Report', reportPathName: 'UserManagementAdminReport', fileName: 'User Management Admin Report' + '_' + + $scope.filedateformat, reportGroup: 'Administration', filterLess: true },
                { filterName: 'Work Breakdown Structure Admin Report', reportPathName: 'WorkBreakdownStructureAdminReport', fileName: 'Work Breakdown Structure Admin Report' + '_' + + $scope.filedateformat, reportGroup: 'Administration', filterLess: false },
                { filterName: 'Position Title Admin Report', reportPathName: 'PositionTitleAdminReport', fileName: 'Position Title Admin Report' + '_' + + $scope.filedateformat, reportGroup: 'Administration', filterLess: true },
                { filterName: 'Employee Admin Report', reportPathName: 'EmployeeAdminReport', fileName: 'Employee Admin Report' + '_' + + $scope.filedateformat, reportGroup: 'Administration', filterLess: false },
                { filterName: 'Subcontractor Type Admin Report', reportPathName: 'SubcontractorTypeAdminReport', fileName: 'Subcontractor Type Admin Report' + '_' + + $scope.filedateformat, reportGroup: 'Administration', filterLess: true },
                { filterName: 'Subcontractor Admin Report', reportPathName: 'SubcontractorAdminReport', fileName: 'Subcontractor Admin Report' + '_' + + $scope.filedateformat, reportGroup: 'Administration', filterLess: false },
                { filterName: 'Material Category Admin Report', reportPathName: 'MaterialCategoryAdminReport', fileName: 'Material Category Admin Report' + '_' + + $scope.filedateformat, reportGroup: 'Administration', filterLess: true },
                { filterName: 'Material Admin Report', reportPathName: 'MaterialAdminReport', fileName: 'Material Admin Report' + '_' + + $scope.filedateformat, reportGroup: 'Administration', filterLess: false },
                { filterName: 'ODC Type Admin Report', reportPathName: 'ODCTypeAdminReport', fileName: 'ODC Type Admin Report' + '_' + + $scope.filedateformat, reportGroup: 'Administration', filterLess: true },
                // Jignesh 21-12-2020
                { filterName: 'Cost Type Report', reportPathName: 'CostTypeReport', fileName: 'Cost Type Report' + '_' + + $scope.filedateformat, reportGroup: 'Administration', filterLess: false },
                // Jignesh 31-12-2020
                { filterName: 'Trend Status Report', reportPathName: 'TrendStatusReport', fileName: 'Trend Status Report' + '_' + + $scope.filedateformat, reportGroup: 'Administration', filterLess: false },
            ]

            // Jignesh 21-12-2020
            //Start Date Report
            $("#_start_date").datepicker();

            //Default select of report type
            $scope.reportTypeFilter = $scope.reportTypeList[0];

            //Run to populate data for all filter lists
            initializeFilterLists();

            //When a report type is selected
            $scope.selectReportType = function (reportType) {

                if (reportType.filterName != 'Customer Report') {
                    $scope.currentProgramElementList = $scope.allProgramElementList;
                    console.log($scope.currentProgramElementList);
                }

                $scope.reportTypeFilter = reportType;
                return true;
            }


            //When clicked on run button
            $scope.generateReport = function () {

                console.log($scope.selectedOrganization, $scope.selectedProgramElement, $scope.selectedProject, $scope.selectedTrend);

                var baseUrl = '';

                var allFilters = findFilterValues();
                console.log(allFilters);

                if ($scope.reportTypeFilter.filterLess) {   //Filterless Reports Report - MySQL

                    filterlessReport($scope.reportTypeFilter.reportPathName, $scope.reportTypeFilter.fileName);

                }
                else if ($scope.reportTypeFilter.filterName == 'Inventory Export From CPP Into Jonas') {            //Master Inventory Export From CPP Into Jonas - MySQL
                    baseUrl = serviceBasePath + 'Request/MasterInventoryExportFromCPPIntoJonasReport';

                    var pdfUrl = baseUrl
                        + '?CostType=' + allFilters.CostTypeID
                        + '&FileType=' + 'PDF';

                    var excelUrl = baseUrl
                        + '?CostType=' + allFilters.CostTypeID
                        + '&FileType=' + 'excel';

                    openReportViewer(baseUrl, pdfUrl, excelUrl, $scope.reportTypeFilter.fileName);
                }
                else if ($scope.reportTypeFilter.filterName == 'Project Export From CPP Into Jonas') {            //Project Export From CPP Into Jonas - MySQL
                    baseUrl = serviceBasePath + 'Request/ProjectExportFromCPPIntoJonasReport';

                    var pe_id = 0;

                    if (allFilters.programElementID > 0) {
                        pe_id = allFilters.programElementID;
                    }

                    var pdfUrl = baseUrl
                        + '?ProgramElementID=' + pe_id
                        + '&FileType=' + 'PDF';

                    var excelUrl = baseUrl
                        + '?ProgramElementID=' + pe_id
                        + '&FileType=' + 'excel';

                    openReportViewer(baseUrl, pdfUrl, excelUrl, $scope.reportTypeFilter.fileName);
                }
                else if ($scope.reportTypeFilter.filterName == 'Baseline Export From CPP Into Jonas'
                    || $scope.reportTypeFilter.filterName == 'Baseline Pair Revenue Export From CPP Into Jonas'
                    || $scope.reportTypeFilter.filterName == 'Baseline One Total Revenue Export From CPP Into Jonas') {            //Baseline Export From CPP Into Jonas - MySQL
                    if (allFilters.programElementID > 0) {	//must select a project

                        baseUrl = serviceBasePath + 'Request/BaselineExportFromCPPIntoJonasReport';

                        var pdfUrl = baseUrl
                            + '?ProgramElementID=' + allFilters.programElementID
                            + '&ProjectID=' + allFilters.projectID
                            + '&ReportVersion=' + $scope.reportTypeFilter.reportVersion
                            + '&FileType=' + 'PDF';

                        var excelUrl = baseUrl
                            + '?ProgramElementID=' + allFilters.programElementID
                            + '&ProjectID=' + allFilters.projectID
                            + '&ReportVersion=' + $scope.reportTypeFilter.reportVersion
                            + '&FileType=' + 'excel';

                        openReportViewer(baseUrl, pdfUrl, excelUrl, $scope.reportTypeFilter.fileName);
                    }
                    else {
                        dhtmlx.alert('This report requires a project to be selected');
                        return;
                    }
                }
                else if ($scope.reportTypeFilter.filterName == 'Customer Report') {            //Customer Report - MySQL
                    if (!allFilters.organizationID) {
                        dhtmlx.alert('Must select an organization for Customer Report');
                        return;
                    }

                    baseUrl = serviceBasePath + 'Request/CustomerReport';

                    var pdfUrl = baseUrl
                        + '?OrganizationID=' + allFilters.organizationID
                        + '&ProgramElementID=' + allFilters.programElementID
                        + '&ProjectID=' + allFilters.projectID
                        + '&FileType=' + 'PDF';

                    var excelUrl = baseUrl
                        + '?OrganizationID=' + allFilters.organizationID
                        + '&ProgramElementID=' + allFilters.programElementID
                        + '&ProjectID=' + allFilters.projectID
                        + '&FileType=' + 'excel';

                    openReportViewer(baseUrl, pdfUrl, excelUrl, $scope.reportTypeFilter.fileName);
                }
                else if ($scope.reportTypeFilter.filterName == 'Sales Order Report') {            //Sales Order Report - MySQL
                    baseUrl = serviceBasePath + 'Request/SalesOrderReport';

                    var pdfUrl = baseUrl
                        + '?ProgramElementID=' + allFilters.programElementID
                        + '&ProjectID=' + allFilters.projectID
                        + '&TrendID=' + allFilters.trendID
                        + '&FileType=' + 'PDF';

                    var excelUrl = baseUrl
                        + '?ProgramElementID=' + allFilters.programElementID
                        + '&ProjectID=' + allFilters.projectID
                        + '&TrendID=' + allFilters.trendID
                        + '&FileType=' + 'excel';

                    openReportViewer(baseUrl, pdfUrl, excelUrl, $scope.reportTypeFilter.fileName);
                }
                else if ($scope.reportTypeFilter.filterName == 'Purchase Order Report') {            //Purchase Order Report - MySQL
                    baseUrl = serviceBasePath + 'Request/PurchaseOrderReport';

                    var pdfUrl = baseUrl
                        + '?ProgramElementID=' + allFilters.programElementID
                        + '&ProjectID=' + allFilters.projectID
                        + '&TrendID=' + allFilters.trendID
                        + '&FileType=' + 'PDF';

                    var excelUrl = baseUrl
                        + '?ProgramElementID=' + allFilters.programElementID
                        + '&ProjectID=' + allFilters.projectID
                        + '&TrendID=' + allFilters.trendID
                        + '&FileType=' + 'excel';

                    openReportViewer(baseUrl, pdfUrl, excelUrl, $scope.reportTypeFilter.fileName);
                }
                else if ($scope.reportTypeFilter.filterName == 'Material Admin Report') {            //Material Admin Report - MySQL
                    baseUrl = serviceBasePath + 'Request/MaterialAdminReport';

                    var pdfUrl = baseUrl
                        + '?MaterialCategoryID=' + allFilters.materialCategoryID
                        + '&FileType=' + 'PDF';

                    var excelUrl = baseUrl
                        + '?MaterialCategoryID=' + allFilters.materialCategoryID
                        + '&FileType=' + 'excel';

                    openReportViewer(baseUrl, pdfUrl, excelUrl, $scope.reportTypeFilter.fileName);
                }
                else if ($scope.reportTypeFilter.filterName == 'Subcontractor Admin Report') {            //Subcontractor Admin Report - MySQL
                    baseUrl = serviceBasePath + 'Request/SubcontractorAdminReport';

                    var pdfUrl = baseUrl
                        + '?SubcontractorTypeID=' + allFilters.subcontractorTypeID
                        + '&FileType=' + 'PDF';

                    var excelUrl = baseUrl
                        + '?SubcontractorTypeID=' + allFilters.subcontractorTypeID
                        + '&FileType=' + 'excel';

                    openReportViewer(baseUrl, pdfUrl, excelUrl, $scope.reportTypeFilter.fileName);
                }
                else if ($scope.reportTypeFilter.filterName == 'Employee Admin Report') {            //Employee Admin Report - MySQL
                    baseUrl = serviceBasePath + 'Request/EmployeeAdminReport';

                    var pdfUrl = baseUrl
                        + '?OrganizationID=' + allFilters.organizationID
                        + '&PositionID=' + allFilters.positionID
                        + '&FileType=' + 'PDF';

                    var excelUrl = baseUrl
                        + '?OrganizationID=' + allFilters.organizationID
                        + '&PositionID=' + allFilters.positionID
                        + '&FileType=' + 'excel';

                    openReportViewer(baseUrl, pdfUrl, excelUrl, $scope.reportTypeFilter.fileName);
                }
                else if ($scope.reportTypeFilter.filterName == 'Work Breakdown Structure Admin Report') {            //Work Breakdown Structure Admin Report - MySQL
                    baseUrl = serviceBasePath + 'Request/WorkBreakdownStructureAdminReport';

                    var pdfUrl = baseUrl
                        + '?OrganizationID=' + allFilters.organizationID
                        + '&ProjectClassID=' + allFilters.projectClassID
                        + '&PhaseCode=' + allFilters.phaseCode
                        + '&FileType=' + 'PDF';

                    var excelUrl = baseUrl
                        + '?OrganizationID=' + allFilters.organizationID
                        + '&ProjectClassID=' + allFilters.projectClassID
                        + '&PhaseCode=' + allFilters.phaseCode
                        + '&FileType=' + 'excel';

                    openReportViewer(baseUrl, pdfUrl, excelUrl, $scope.reportTypeFilter.fileName);
                }
                else if ($scope.reportTypeFilter.filterName == 'Project Element List Admin Report') {            //Project Element List Admin Report - MySQL
                    baseUrl = serviceBasePath + 'Request/ProjectElementListAdminReport';

                    var pdfUrl = baseUrl
                        + '?ProgramID=' + allFilters.programID
                        + '&ProgramElementID=' + allFilters.programElementID
                        + '&ProjectID=' + allFilters.projectID
                        + '&FileType=' + 'PDF';

                    var excelUrl = baseUrl
                        + '?ProgramID=' + allFilters.programID
                        + '&ProgramElementID=' + allFilters.programElementID
                        + '&ProjectID=' + allFilters.projectID
                        + '&FileType=' + 'excel';

                    openReportViewer(baseUrl, pdfUrl, excelUrl, $scope.reportTypeFilter.fileName);
                }
                // Jignesh 22-12-2020
                else if ($scope.reportTypeFilter.filterName == 'Cost Type Report') {
                    var startDate = $('#_start_date').val();
                    if (!allFilters.organizationID) {
                        dhtmlx.alert('Must select an organization for Cost Type Report');
                        return;
                    }
                    if (!allFilters.programElementID) {
                        dhtmlx.alert('Must select project for Cost Type Report');
                        return;
                    }
                    if (!allFilters.projectID) {
                        dhtmlx.alert('Must select project element for Cost Type Report');
                        return;
                    }
                    baseUrl = serviceBasePath + 'Request/CostTypeAdminReport';

                    startDate = dateFormatter(startDate);
                    var pdfUrl = baseUrl
                        + '?ProjectID=' + allFilters.projectID
                        + '&FileType=' + 'PDF';

                    var excelUrl = baseUrl
                        + '?ProjectID=' + allFilters.projectID
                        + '&FileType=' + 'excel';

                    openReportViewer(baseUrl, pdfUrl, excelUrl, $scope.reportTypeFilter.fileName);
                }
                //Jignesh 31-12-2020
                else if ($scope.reportTypeFilter.filterName == 'Trend Status Report') {
                    if (!$scope.selectedTrendStatus) {
                        dhtmlx.alert('Must select trend status for Trend Status Report');
                        return;
                    }
                    baseUrl = serviceBasePath + 'Request/TrendStatusAdminReport';

                    var pdfUrl = baseUrl
                        + '?ProgramID=' + allFilters.programID
                        + '&ProgramElementID=' + allFilters.programElementID
                        + '&ProjectID=' + allFilters.projectID
                        + '&Status=' + $scope.selectedTrendStatus
                        + '&FileType=' + 'PDF';

                    var excelUrl = baseUrl
                        + '?ProgramID=' + allFilters.programID
                        + '&ProgramElementID=' + allFilters.programElementID
                        + '&ProjectID=' + allFilters.projectID
                        + '&Status=' + $scope.selectedTrendStatus
                        + '&FileType=' + 'excel';

                    openReportViewer(baseUrl, pdfUrl, excelUrl, $scope.reportTypeFilter.fileName);
                }
            }

            //Select cost type
            $scope.selectCostType = function (costType) {
                $scope.selectedCostType = costType;
            }

            //Select organizationid
            $scope.selectOrganization = function (organization) {
                $scope.selectedOrganization = organization;

                //reset children filters
                $scope.selectedProgram = undefined;
                $scope.selectedProgramElement = undefined;
                $scope.selectedProject = undefined;
                $scope.selectedTrend = undefined;

                //get new list of program elements
                $scope.currentProgramElementList = filterProgramElementsByOrganizationID(organization.OrganizationID);
                console.log($scope.currentProgramElementList);
            }

            //Select programid
            $scope.selectProgram = function (program) {
                $scope.selectedProgram = program;
                console.log($scope.selectedProgram);

                //reset children filters
                $scope.selectedProgramElement = undefined;
                $scope.selectedProject = undefined;
                $scope.selectedTrend = undefined;

                //get new list of program elements
                $scope.currentProgramElementList = filterProgramElementsByProgramID(program.ProgramID);
                console.log($scope.currentProgramElementList);
            }

            //Select programelementid
            $scope.selectProgramElement = function (programElement) {
                $scope.selectedProgramElement = programElement;
                console.log($scope.selectedProgramElement);

                //reset children filters
                $scope.selectedProject = undefined;
                $scope.selectedTrend = undefined;

                //get new list of projects
                $scope.currentProjectList = filterProjectsByProgramElementID(programElement.ProgramElementID);
                console.log($scope.currentProjectList);
            }

            //Select projectid
            $scope.selectProject = function (project) {
                $scope.selectedProject = project;
                console.log($scope.selectedProject);

                //reset children filters
                $scope.selectedTrend = undefined;

                //get new list of trends
                $scope.currentTrendList = filterTrendsByProjectID(project.ProjectID);
            }

            //Select trendid
            $scope.selectTrend = function (trend) {
                $scope.selectedTrend = trend;
                console.log($scope.selectedTrend);
            }

            //Jignesh 31-12-2020
            $scope.selectTrendStatus = function (trendStatus) {
                $scope.selectedTrendStatus = trendStatus.filterName;
            }

            //Select materialcategoryid
            $scope.selectMaterialCategory = function (materialCategory) {
                $scope.selectedMaterialCategory = materialCategory;
                console.log($scope.selectedMaterialCategory);
            }

            //Select subcontractortypeid
            $scope.selectSubcontractorType = function (subcontractorType) {
                $scope.selectedSubcontractorType = subcontractorType;
                console.log($scope.selectedSubcontractorType);
            }

            //Select positionid
            $scope.selectPosition = function (position) {
                $scope.selectedPosition = position;
                console.log($scope.selectedPosition);
            }

            //Select projectclassid
            $scope.selectProjectClass = function (projectClass) {
                $scope.selectedProjectClass = projectClass;
                $scope.currentPhaseList = [];
                console.log($scope.selectedProjectClass);
                var currentProjectPhaseList = filterPhaseCodeByProjectClassID(projectClass.ProjectClassID);

                for (var i = 0; i < $scope.allPhaseList.length; i++) {
                    for (var j = 0; j < currentProjectPhaseList.length; j++) {
                        if ($scope.allPhaseList[i].PhaseID == currentProjectPhaseList[j].PhaseID) {
                            $scope.currentPhaseList.push($scope.allPhaseList[i]);
                        }
                    }
                }
            }

            //Select phasecode
            $scope.selectPhase = function (phase) {
                $scope.selectedPhase = phase;
                console.log($scope.selectedPhase);
            }

            $scope.$watch('selectedProgram', function () {
                console.log($scope.selectedProgram)
                if ($scope.selectedProgram == undefined || $scope.selectedProgram == null || angular.equals($scope.selectedProgram, {})) {
                    $scope.selectedProgramElementList = [];
                    $scope.currentProjectList = [];
                    $scope.currentTrendList = [];

                    $scope.selectedProgram = undefined;
                    $scope.selectedTrend = undefined;
                }
            });

            $scope.$watch('selectedProgramElement', function () {
                console.log($scope.selectedProgramElement)
                if ($scope.selectedProgramElement == undefined || $scope.selectedProgramElement == null || angular.equals($scope.selectedProgramElement, {})) {
                    $scope.currentProjectList = [];
                    $scope.currentTrendList = [];

                    $scope.selectedProgramElement = undefined;
                    $scope.selectedTrend = undefined;
                }
            });

            $scope.$watch('selectedProject', function () {
                if ($scope.selectedProject == undefined || $scope.selectedProject == null || angular.equals($scope.selectedProject, {})) {
                    $scope.currentTrendList = [];

                    $scope.selectedProject = undefined;
                    $scope.selectedTrend = undefined;
                }
            });

            //When clicked on back button or X
            $scope.goBack = function (param) {
                $scope.$close(param);
            }

            //Initialize all filter lists with data from backend
            function findFilterValues() {
                var allFilters = {
                    organizationID: 0,
                    programID: 0,
                    programElementID: 0,
                    projectID: 0,
                    trendID: 0,
                    materialCategoryID: 0,
                    subcontractorTypeID: 0,
                    positionID: 0,
                    CostTypeID: 'A',
                    projectClassID: 0,
                    phaseCode: 'All'
                }

                //Process cost type
                if ($scope.selectedCostType != undefined && $scope.selectedCostType != null && $scope.selectedCostType.CostTypeID) {
                    allFilters.CostTypeID = $scope.selectedCostType.CostTypeID;
                } else {
                    allFilters.CostTypeID = 'A';
                }

                //Process organization
                if ($scope.selectedOrganization != undefined && $scope.selectedOrganization != null && $scope.selectedOrganization.OrganizationID) {
                    allFilters.organizationID = $scope.selectedOrganization.OrganizationID;
                } else {
                    allFilters.organizationID = 0;
                }

                //Process program filter 
                if ($scope.selectedProgram != undefined && $scope.selectedProgram != null && $scope.selectedProgram.ProgramID) {
                    allFilters.programID = $scope.selectedProgram.ProgramID;
                } else {
                    allFilters.programID = 0;
                    allFilters.programElementID = 0;
                    allFilters.projectID = 0;
                    allFilters.trendID = 0;
                }

                //Process program element filter aka project
                if ($scope.selectedProgramElement != undefined && $scope.selectedProgramElement != null && $scope.selectedProgramElement.ProgramElementID) {
                    allFilters.programElementID = $scope.selectedProgramElement.ProgramElementID;
                } else {
                    allFilters.programElementID = 0;
                    allFilters.projectID = 0;
                    allFilters.trendID = 0;
                }

                //Process project filter aka project elements
                if ($scope.selectedProject != undefined && $scope.selectedProject != null && $scope.selectedProject.ProjectID) {
                    allFilters.projectID = $scope.selectedProject.ProjectID;
                } else {
                    allFilters.projectID = 0;
                    allFilters.trendID = 0;
                }

                //Process trend filter
                if ($scope.selectedTrend != undefined && $scope.selectedTrend != null && $scope.selectedTrend.TrendID) {
                    allFilters.trendID = $scope.selectedTrend.TrendID;
                } else {
                    allFilters.trendID = 0;
                }

                //Process material category filter
                if ($scope.selectedMaterialCategory != undefined && $scope.selectedMaterialCategory != null && $scope.selectedMaterialCategory.ID) {
                    allFilters.materialCategoryID = $scope.selectedMaterialCategory.ID;
                } else {
                    allFilters.materialCategoryID = 0;
                }

                //Process subcontractor type filter
                if ($scope.selectedSubcontractorType != undefined && $scope.selectedSubcontractorType != null && $scope.selectedSubcontractorType.SubcontractorTypeID) {
                    allFilters.subcontractorTypeID = $scope.selectedSubcontractorType.SubcontractorTypeID;
                } else {
                    allFilters.subcontractorTypeID = 0;
                }

                //Process position filter
                if ($scope.selectedPosition != undefined && $scope.selectedPosition != null && $scope.selectedPosition.Id) {
                    allFilters.positionID = $scope.selectedPosition.Id;
                } else {
                    allFilters.positionID = 0;
                }

                //Process project class filter
                if ($scope.selectedProjectClass != undefined && $scope.selectedProjectClass != null && $scope.selectedProjectClass.ProjectClassID) {
                    allFilters.projectClassID = $scope.selectedProjectClass.ProjectClassID;
                } else {
                    allFilters.projectClassID = 0;
                }

                //Process phase code filter
                if ($scope.selectedPhase != undefined && $scope.selectedPhase != null && $scope.selectedPhase.PhaseID) {
                    allFilters.phaseCode = $scope.selectedPhase.Code;
                } else {
                    allFilters.phaseCode = 'All';
                }

                return allFilters
            }

            //Get new list of program based on organization
            function filterProgramElementsByProgramID(ProgramID) {
                var tempList = [];
                for (var x = 0; x < $scope.allProgramElementList.length; x++) {
                    if ($scope.allProgramElementList[x].ProgramID == ProgramID) {
                        tempList.push(angular.copy($scope.allProgramElementList[x]));
                    }
                }

                return tempList;
            }

            //Get new list of program element based on organization
            function filterProgramElementsByOrganizationID(organizationID) {
                var tempList = [];
                for (var x = 0; x < $scope.allProgramElementList.length; x++) {
                    if ($scope.allProgramElementList[x].OrganizationID == organizationID) {
                        tempList.push(angular.copy($scope.allProgramElementList[x]));
                    }
                }

                return tempList;
            }

            //Get new list of projects based on program element id
            function filterProjectsByProgramElementID(programElementID) {
                var tempList = [];
                for (var x = 0; x < $scope.allProjectList.length; x++) {
                    if ($scope.allProjectList[x].ProgramElementID == programElementID) {
                        tempList.push(angular.copy($scope.allProjectList[x]));
                    }
                }

                return tempList;
            }

            //Get new list of trends based on project id
            function filterTrendsByProjectID(projectID) {
                var tempList = [];
                for (var x = 0; x < $scope.allTrendList.length; x++) {
                    if ($scope.allTrendList[x].ProjectID == projectID) {
                        tempList.push(angular.copy($scope.allTrendList[x]));
                    }
                }

                return tempList;
            }

            // Get new list of phase codes based on projectclass id
            function filterPhaseCodeByProjectClassID(ProjectClassID) {
                var tempList = [];
                for (var x = 0; x < $scope.allProjectClassPhaseList.length; x++) {
                    console.log($scope.allProjectClassPhaseList[x]);
                    if ($scope.allProjectClassPhaseList[x].ProjectClassID == ProjectClassID) {
                        tempList.push(angular.copy($scope.allProjectClassPhaseList[x]));
                    }
                }

                console.log($scope.allProjectClassPhaseList);
                console.log(tempList);
                return tempList;
            }

            //Reports with filter
            function openReportViewer(baseUrl, pdfUrl, excelUrl, fileName) {
                $http.get(pdfUrl).then(function success(response) {
                    console.log(response);

                    var scope = $rootScope.$new();

                    //Declare parameters for pdf viewer modal
                    scope.params = {
                        content: response.data,
                        excelUrl: excelUrl,
                        baseUrl: baseUrl,
                        fileName: fileName,
                        contentType: 'base64',
                        excelDownloadable: true,
                        pdfDownloadable: true
                    };

                    //Open pdf viewer modal
                    openPDFViewerModal(scope);

                }, function error(response) {
                    console.log(response);
                });
            }

            // Jignesh 21-12-2020
            // Change Date Format
            function dateFormatter(dateObject) {
                var d = new Date(dateObject);
                var day = d.getDate();
                var month = d.getMonth() + 1;
                var year = d.getFullYear();
                if (day < 10) {
                    day = "0" + day;
                }
                if (month < 10) {
                    month = "0" + month;
                }
                var date = year + "-" + month + "-" + day;

                return date;
            }

            //Open the pdf viewer modal to view the content
            function openPDFViewerModal(scope) {
                $rootScope.modalInstance = $uibModal.open({
                    backdrop: 'static',
                    keyboard: false,
                    scope: scope,
                    templateUrl: "app/views/modal/PDFViewerModal.html",
                    size: "md",
                    controller: "PDFViewerCtrl"
                });
            }

            //Filterless reports function
            function filterlessReport(reportPathName, fileNameInput) {
                var baseUrl = null;

                baseUrl = serviceBasePath + 'Request/' + reportPathName;

                var pdfUrl = baseUrl
                    + '?FileType=' + 'PDF';

                var excelUrl = baseUrl
                    + '?FileType=' + 'excel';

                var fileName = generateFileName(fileNameInput);

                openReportViewer(baseUrl, pdfUrl, excelUrl, fileName);
            }

            //Generate file name for download based on the report type
            function generateFileName(reportType) {
                var fileName = reportType;

                if (reportType == 'Testing SSRS') {
                    if ($scope.ReportParameter1) {
                        fileName += '_' + $scope.ReportParameter1;
                    }

                    if (!$scope.ReportParameter1) {
                        fileName += '_No Filter';
                    }
                }

                return fileName;
            }

            //Call to initialize all lists for the filters, making backend calls
            function initializeFilterLists() {
                //initialize cost type list
                $scope.costTypeList = [{ CostTypeID: 'A', CostTypeName: 'All' },
                { CostTypeID: 'L', CostTypeName: 'Employees' },
                { CostTypeID: 'S', CostTypeName: 'Subcontractors' },
                { CostTypeID: 'M', CostTypeName: 'Materials' },
                { CostTypeID: 'O', CostTypeName: 'ODC Types' }
                ]

                //initialize list of material categories
                MaterialCategory.get({}, function (response) {
                    console.log(response);
                    $scope.allMaterialCategoryList = response.result;
                });

                //initialize list of subcontractor types
                SubcontractorType.get({}, function (response) {
                    console.log(response);
                    $scope.allSubcontractorTypeList = response.result;
                });

                //initialize list of positions
                fteposition.get({}, function (response) {
                    console.log(response);
                    $scope.allPositionList = response.result;
                });

                //initialize list of organizations
                Organization.lookup().get({}, function (response) {
                    console.log(response);
                    $scope.allOrganizationList = response.result;
                });

                //initialize list of programs
                Program.lookup().get({}, function (response) {
                    console.log(response);
                    $scope.allProgramList = response.result;
                    $scope.currentProgramList = response.result;
                });

                //initialize list of program elements
                ProgramElement.lookup().get({}, function (response) {
                    console.log(response);
                    $scope.allProgramElementList = response.result;
                    $scope.currentProgramElementList = response.result;
                });

                //initialize list of projects
                Project.lookup().get({}, function (response) {
                    console.log(response);
                    $scope.allProjectList = response.result;
                });

                //Initialize list of project classes
                ProjectClass.get({}, function (response) {
                    console.log(response);
                    $scope.allProjectClassList = response.result;
                });

                //Initialize list of phases
                PhaseCode.get({}, function (response) {
                    console.log(response);
                    $scope.allPhaseList = response.result;
                });

                //Initialize list of project class phase
                ProjectClassPhase.get({}, function (response) {
                    console.log(response);
                    $scope.allProjectClassPhaseList = response.result;
                });

                //initialize list of trends
                Trend.lookup().get({
                    ProgramID: 'null', ProgramElementID: 'null', ProjectID: 'null', TrendNumber: 'null', KeyStroke: 'null'
                }, function (response) {
                    console.log(response);
                    $scope.allTrendList = response.result;

                    for (var x = 0; x < $scope.allTrendList.length; x++) {
                        $scope.allTrendList[x].TrendNumberDescription = '';
                        if ($scope.allTrendList[x].TrendNumber != 0 && $scope.allTrendList[x].TrendNumber != 1000) {
                            if ($scope.allTrendList[x].TrendDescription != null || $scope.allTrendList[x].TrendDescription != '') {
                                $scope.allTrendList[x].TrendNumberDescription = $scope.allTrendList[x].TrendNumber + ' - ' + $scope.allTrendList[x].TrendDescription;
                            } else {
                                $scope.allTrendList[x].TrendNumberDescription = $scope.allTrendList[x].TrendNumber;
                            }
                        } else {
                            $scope.allTrendList[x].TrendNumberDescription = $scope.allTrendList[x].TrendDescription;
                        }
                    }
                });
            }


        }
    ]);