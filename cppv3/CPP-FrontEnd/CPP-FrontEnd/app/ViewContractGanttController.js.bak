/*
 * Created by ikhong on 6/10/2015.
 */
angular.module('xenon.ViewContractGanttController', []).
    controller('ViewContractGanttCtrl', ['MainActivityCategory', 'currentTrend', 'TrendId', 'ProjectTitle', 'UnitType', 'Vendor', 'SubActivityCategory', 'Category', 'GetActivity', '$http', '$q', '$state', '$scope', '$compile', 'Program', 'ProgramElement',
        'Project', 'Trend', 'Activity', 'Cost', 'InsertCost', 'fteposition', 'FTEPositionCost', 'delayedData', 'Page', 'UpdateActivity', 'localStorageService', 'RequestApproval', 'TrendStatus',
        '$rootScope', '$uibModal', 'ProgramFund', 'TrendFund', '$timeout', 'usSpinnerService', '$filter', '$location', 'alertBox', 'usSpinnerConfig', 'GanttCategory', 'ProgramCategory', 'MaterialCategory', 'Material', 'ODCType', 'SubcontractorType', 'Subcontractor', 'PhaseCode', 'UserByEmployeeListID', 'AllEmployee', 'TrendStatusCode', '$stateParams', 'Employee',
        'LaborRate', 'PurchaseOrder', 'PurchaseOrderDetail', 'MFAConfiguration', 'ViewProjectGanttActivities',
        function (MainActivityCategory, currentTrend, TrendId, ProjectTitle, UnitType, Vendor, SubActivityCategory, Category, GetActivity, $http, $q, $state, $scope, $compile, Program, ProgramElement,
            Project, Trend, Activity, Cost, InsertCost, FTEPositions, FTEPositionCost, delayedData, Page, UpdateActivity, localStorageSrevice, RequestApproval, TrendStatus,
            $rootScope, $uibModal, ProgramFund, TrendFund, $timeout, usSpinnerService, $filter, $location, alertBox, usSpinnerConfig, GanttCategory, ProgramCategory, MaterialCategory, Material, ODCType, SubcontractorType, Subcontractor, PhaseCode, UserByEmployeeListID, AllEmployee, TrendStatusCode, $stateParams, Employee,
            LaborRate, PurchaseOrder, PurchaseOrderDetail, MFAConfiguration, ViewProjectGanttActivities) {

            function roundToTwo(num) {
                return +(Math.round(num + "e+2") + "e-2");
            }
            //luan here 4/24
            var htmlDelete =
                ' <div class="dropdown">'
                + ' <button class="btn btn-xs btn-primary dropdown-toggle" type="button" data-toggle="dropdown">'
                + '<span class="caret"></span></button>'
                + ' <ul class="dropdown-menu">'
                + '<li><a ng-click="costDetails()">Cost Details</a></li>'
                + '<li><i class=" fa fa-clipboard"><a ng-click="copyToClipboard()">Copy cost code to clipboard</a></i></li>'
                + '<li><a ng-click="cancelCostDetail()">Cancel</a></li>'
                + '<li><a id="cost-delete-id" ng-click="deleteCost()">Delete</a></li>'
                + '  </ul>'
                + ' </div>'

            $compile(htmlDelete)($scope);

            $scope.luantest = function () {
                alert('luan test');
            }


            var refreshCounter = 0;
            function refreshHtmlDelete() {
                //luan here 4/24
                setTimeout(function () {
                    //alert('ready');
                    console.log('attempting refresh', $scope.costs.data);
                    for (var x = 0; x < $scope.costs.data.length; x++) {
                        var id = x + 1;
                        var isCurrentTrend = ($scope.trend.TrendNumber == 1000);

                        var isTrue = ($scope.trend.TrendNumber == 3000); // Swapnil 24-11-2020

                        var row = $("#cost-gantt .gantt_row[task_id='" + (x + 1) + "']");
                        console.log(row);
                        var cells = row.find('.gantt_cell');

                        var dropdown = ' <div class="dropdown" style="position:unset !important;">';
                        dropdown += ' <button class="btn btn-xs btn-primary dropdown-toggle" type="button" data-toggle="dropdown">';
                        dropdown += '<span class="caret"></span></button>';
                        dropdown += ' <ul class="dropdown-menu">';
                        if ($scope.isNewCost[id] == true) { //new cost disable it
                            dropdown += '<li class="cost-dropdown-disable" disabled=true ng-click="costDetails()"><i class=" fa fa-info-circle"><a style="margin-left:0.5em;" >Cost Details</a></i></li>';
                            dropdown += '<li class="cost-dropdown-disable" disabled=true ng-click="copyToClipboard()" ><i class=" fa fa-clipboard"><a style="margin-left:0.5em;" >Copy cost code to clipboard</a></i></li>';
                        }
                        else {
                            dropdown += '<li class="cost-dropdown" ng-click="costDetails()" id="costDetailUnique_' + refreshCounter + '_' + id + '" ><i class=" fa fa-info-circle"><a style="margin-left:0.5em;" >Cost Details</a></i></li>';
                            dropdown += '<li class="cost-dropdown" ng-click="copyToClipboard()" id="copyToClipboardUnique_' + refreshCounter + '_' + id + '" ><i class=" fa fa-clipboard"><a style="margin-left:0.5em;" >Copy cost code to clipboard</a></i></li>';
                        }
                        dropdown += ' <li class="cost-dropdown" ng-click="cancelCostDetail()" id="cancelUnique_' + refreshCounter + '_' + id + '"  ><i class=" fa fa-close"><a style="margin-left:0.5em;" >Cancel</a></i></li>';
                        if (isCurrentTrend) //Disable it
                            dropdown += ' <li class="cost-dropdown-disable" ng-click="deleteCost(' + id + ')" ><i class=" fa fa-trash"><a  style="margin-left:0.5em;" >Delete</a></i></li>';
                        else
                            dropdown += ' <li class="cost-dropdown" ng-click="deleteCost()" id="deleteCostUnique_' + refreshCounter + '_' + id + '" ><i class=" fa fa-trash"><a  style="margin-left:0.5em;" >Delete</a></i></li>';
                        dropdown += '  </ul>';
                        dropdown += ' </div>';
                        // alert(dropdown);
                        $scope.costs.data[x].delete = dropdown;

                        console.log($scope.costs.data[x].delete);
                        $(cells[0]).html($compile($scope.costs.data[x].delete)($scope));



                        //$("#costDetailUnique_" + refreshCounter + "_" + id).click(function (e) {
                        //	costDetailsCaller();
                        //});

                        //$("#copyToClipboardUnique_" + refreshCounter + "_" + id).click(function (e) {
                        //	copyToClipboardCaller();
                        //});

                        //$("#cancelUnique_" + refreshCounter + "_" + id).click(function (e) {
                        //	cancelCaller();
                        //});

                        //$("#deleteCostUnique_" + refreshCounter + "_" + id).click(function (e) {
                        //	deleteCostCaller(id);
                        //});
                    }

                    refreshCounter++;
                }, 1000);	//for after the rendering
            }

            function costDetailsCaller() {
                $scope.costDetails();
                //refreshHtmlDelete();
            }

            function copyToClipboardCaller() {
                alert('copyToClipboardCaller');
                $scope.copyToClipboard();
                //refreshHtmlDelete();
            }

            function cancelCaller() {
                alert('cancelCaller');
                $scope.cancelCostDetail();
                //refreshHtmlDelete();
            }

            function deleteCostCaller(id) {
                alert('deleteCostCaller ' + id);
                $scope.deleteCost(id);
                //refreshHtmlDelete();
            }

            var BILLABLE_RATE = "Billable";
            var BASE_RATE_WITH_MULTIPLIER = "Base";
            var LABOR_RATE = 1;
            var SUBCONTRACTOR_RATE = 1;
            var ODC_RATE = 1;
            var MATERIAL_RATE = 1;
            var CUSTOM_LABOR_RATE = 1;
            var CUSTOM_SUBCONTRACTOR_RATE = 1;
            var CUSTOM_ODC_RATE = 1;
            var CUSTOM_MATERIAL_RATE = 1;

            var PREVIOUSMAIN = '';
            var PREVIOUSSUB = '';

            var rateType = {
                company: "Company",
                union: "Union",
                GA: "G&A",
                Other: ""
            }

            console.debug(SUBCONTRACTOR_RATE, MATERIAL_RATE, ODC_RATE);
            console.debug(CUSTOM_SUBCONTRACTOR_RATE, CUSTOM_MATERIAL_RATE, CUSTOM_ODC_RATE);
            //Jquery
            setTimeout(function () {
                $("<h6 style='color: black; font-weight: 1000; opacity: .75; padding-bottom: 5px'>WBS</h5>").insertBefore($("#schedule-gantt"));
                $("<h6 style='margin-top:15px; color: black; font-weight: 1000; opacity: .75; padding-bottom: 5px'>Cost Line Items</h5>").insertBefore($("#cost-gantt"));  // Jignesh-22-03-2021
                $('div:contains("Category"):not(:has(div))').css('margin-left', '2%');
                $("div[style='width: 313px;']").css('margin-left', '2%');
            }, 100);

            //========================Choose file=============================================
            var formdata = new FormData();
            $('#uploadBtnProject').prop('disabled', true);

            console.log(delayedData);


            var mfaDetails = "";
            var projectTitle = "";
            $scope.trend;   //store the trend information
            $scope.isDeleteFromLightbox;  // bool - check to see whether the user delete an activity from the lightbox
            $scope.currentProjectDuration;  //The total number of days to complete the current project (current project = baseline + approved trends)
            $scope.baselineTrend;           //Store baseline project information
            $scope.FTEPositions = [];      //An array to store all FTE postitions
            $scope.ODCTypes = [];           //An array to store all ODC types
            $scope.subcontractorTypes = []; //An array to store all subcontractor types
            $scope.materialCategories = []; //An array to store all material categories
            $scope.unitTypes = [];         //Array to store unitTypes
            $scope.materials = [];          //Array to store materials
            $scope.isScaleChanged = false;  //a variable to keep track of the scale change
            $scope.isUpdateTaskFromLightbox = true;  //A variable to keep track if an activity is updated from the lightbox
            $scope.schedule = { data: [] };   //store all the activity for schedule Gantt
            $scope.costs = { data: [] };     //Store all the cost for the cost Gantt
            $scope.isMonthSchedule = false;  //a variable to keep track if it is a monthly scale
            $scope.MainCategory = [];        // An array to store the Main Category according to its phase
            $scope.subCategory = [];        //An array to store all the subCategory
            $scope.firstTime = true;        //use on firstTime opening a lightbox to update the Budget Main Category
            $scope.screenLoad = true;
            $scope.percentages = [{ key: '0%', label: '0%' }, { key: '5%', label: '5%' }, { key: '10%', label: '10%' }, { key: '15%', label: '15%' }, { key: '20%', label: '20%' }, { key: '25%', label: '25%' },
            { key: '30%', label: '30%' }, { key: '35%', label: '35%' }, { key: '40%', label: '40%' }, { key: '45%', label: '45%' }, { key: '50%', label: '50%' },
            { key: '55%', label: '55%' }, { key: '60%', label: '60%' }, { key: '65%', label: '65%' }, { key: '70%', label: '70%' }, { key: '75%', label: '75%' },
            { key: '80%', label: '80%' }, { key: '85%', label: '85%' }, { key: '90%', label: '90%' }, { key: '95%', label: '95%' }, { key: '100%', label: '100%' }]
            $scope.isNewCost = [];
            $scope.employeeID = [];
            $scope.isTaskUpdate = false;
            $scope.showSpinner12 = false;
            $scope.buffer = [];// store object in memory
            $scope.newCostBuffer = [];
            var MainCategory;          //Variable to store the BudgetCategory
            $scope.isBaseline = delayedData[4];
            paddingLabel();
            $scope.selectedPhase = "";
            $scope.OrganizationID = delayedData[5];
            $scope.localStorageSrevice = localStorageSrevice;

            var dateFormat = "DD-MM-YYYY";
            var sqlDateFormat = "YYYY-MM-DD";
            var isCostSaving = false;//To bypass the rounding issues in costcalculation -- HELP ME to fix it
            $scope.filterPhase = function (phase) {
            }

            $scope.costTrackType = {
                'ESTIMATED': 1,
                'BUDGET': 2,
                'ACTUAL': 3,
                'ESTIMATE_TO_COMPLETION': 4
            }

            //On resize of window, adjust the location of TotalCost
            $(window).unbind('resize').resize(function () {
                paddingLabel();
            });

            $scope.isTrendApproved = false;





            //Set Title for the project
            //ProjectTitle.setTitle(delayedData[2].result[0].ProjectName);    //07-01-2021

            //Called everytime a cost method is changed



            // On changes end here 
            $scope.newEmployees = [];   //here
            $scope.newSubcontractors = [];  //here
            $scope.newMaterials = [];   //here





            //changedCost
            $scope.textBoxStyles = [];


            //initialize cost method - FTE, Lumpsum, and unit
            $scope.methods = [{
                name: 'Labor',
                value: 'F'
            }, {
                //name: 'Contractor',
                name: 'Subcontractor ',  //Manasi 17-07-2020
                value: 'L'
            }, {
                name: 'Material',
                value: 'U'
            }, {
                name: "ODC",
                value: "ODC"
            }];

            var projectMaxId = "";
            //(Math.max.apply(Math, delayedData[1].result.map(function (a) { return a.PhaseID })) + 1) * 1000;
            var programElementMaxId = "";
            //(Math.max.apply(Math, delayedData[2].result.map(function (a) { return a.ProjectID })) + 1) * 1000;
            var contractMaxId = (Math.max.apply(Math, delayedData[8].result.map(function (a) { return a.ProgramElementID })) + 1) * 1000;
            //API call to get a list of unitypes



            console.log(delayedData);
            $scope.employees = [];
            $scope.subcontractors = [];
            $scope.subcontractorTypes = [];
            $scope.materials = [];
            $scope.materialCategories = [];

            function refreshEmployees() {
                angular.forEach(delayedData[7].result, function (item) {
                    var temp = {};
                    temp.name = item.Name;
                    temp.value = item.ID;
                    $scope.employees.push(temp);
                });
            }
            refreshEmployees();


            $scope.orgMaterials = delayedData[6].result;
            function refreshMaterials() {
                angular.forEach($scope.orgMaterials, function (item) {
                    var temp = {};
                    temp.name = item.Name;
                    temp.value = item.ID;
                    $scope.materials.push(temp);
                });
            }
            refreshMaterials();
            console.log($scope.materials);



            $("#schedule-gantt").mouseleave(function () {
                $scope.scheduleGanttInstance.ext.tooltips.tooltip.hide();
                $timeout(function () {
                    if (!$('#schedule-gantt').is(":hover")) {
                        $scope.scheduleGanttInstance.ext.tooltips.tooltip.hide();
                    }
                }, 100);
            });
            $("#cost-gantt").mouseleave(function () {
                console.log('mouse leave');
                $scope.costGanttInstance.ext.tooltips.tooltip.hide();
                $timeout(function () {
                    if (!$('#cost-gantt').is(":hover")) {
                        $scope.costGanttInstance.ext.tooltips.tooltip.hide();
                    }
                }, 100);
            });



            $scope.exit = function () {
                window.location.href = "#/app/wbs";
            }

            //assign dollar value for all the phases on page load
            /* $scope.planning_value = $filter('currency')(phaseArray[0].totalCost,'$',formatCurrency(phaseArray[0].totalCost) );      //Total activities cost for phase planning
             $scope.schematic_design_value =  $filter('currency')(phaseArray[1].totalCost,'$',formatCurrency(phaseArray[1].totalCost) ); //total activities cost for schematic design phase
             $scope.design_bidding_value = $filter('currency')(phaseArray[2].totalCost,'$',formatCurrency(phaseArray[2].totalCost));  //total activity cost for design bidding phase
             $scope.construction_value = $filter('currency')(phaseArray[3].totalCost,'$',formatCurrency(phaseArray[3].totalCost));   //total activity cost for construction phase
             $scope.closeout_value = $filter('currency')(phaseArray[4].totalCost,'$',formatCurrency(phaseArray[4].totalCost));     //total activity cost for the closeout phse*/

            //initialize activity object for the schedule Gantt


          
            var trendTotalValue = 0;
            var trendTotalValueActual = 0;
            var trendTotalBudget = 0;
            var activities = delayedData[0].result;   //List of activites
            var projects = delayedData[2].result;
            var programElements = delayedData[8].result;
            //var contract = {};
            //contract["id"] = contractMaxId;
            //contract["text"] = delayedData[9].result[0].ProgramName;
            //contract["type"] = gantt.config.types.project;
            //contract["open"] = true;
            //contract["duration"] = 0;
            //contract["totalCost"] = "" + "";
            //console.log(delayedData);
            ////luan mark - added for original start/end dates to show on columns 
            //contract["originalStartDate"] = getProjectOriginalStartEndDate(programElements).originalStartDate;
            //contract["originalEndDate"] = getProjectOriginalStartEndDate(programElements).originalEndDate;

            //contract["percentage_completion"] = getProjectPercentageCompletion(programElements);
            //contract["totalBudget"] = trendTotalBudget;
            //$scope.schedule.data.push(contract);

            programElements = initializeProgramElements(programElements);
            projects = initializeProjects(projects);
            //activities = initializeActivities(activities);
            var viewPhase = delayedData[1].result;   //List of Phases
            viewPhase = initializePhases(viewPhase);

            //var programElements = delayedData[8].result;

            
            var contract = {};
            contract["id"] = contractMaxId;
            contract["text"] = delayedData[9].result[0].ProgramName;
            contract["type"] = gantt.config.types.project;
            contract["open"] = true;
            contract["duration"] = 0;
            contract["totalCost"] = "" + "";
            console.log(delayedData);
            //luan mark - added for original start/end dates to show on columns 
            contract["originalStartDate"] = getProjectOriginalStartEndDate(programElements).originalStartDate;
            contract["originalEndDate"] = getProjectOriginalStartEndDate(programElements).originalEndDate;

            contract["percentage_completion"] = getProjectPercentageCompletion(programElements);
            contract["totalBudget"] = trendTotalBudget;
            $scope.schedule.data.push(contract);

           
            //push all phases as children of project


            $scope.phases = delayedData[1].result;;
            $timeout(function () {
                $scope.schedulePhase = null;
            }, 100);

            //var MainCategory = delayedData[4].result;  //Main category

            //project["totalCost"] = trendTotalValue;
            //project["totalBudget"] = trendTotalBudget;
            ////pritesh1
            //// alert("First : " + trendTotalValue + " ---Second : " + trendTotalBudget + "-- Third : " + trendTotalValueActual);
            ////luan quest 3
            //project["totalCostActualForecast"] = trendTotalValueActual;



            //var threshold = authData.threshold;
            //$scope.threshold = parseFloat(threshold);

            //luan here - No hiding approve btn 
            //if (parseFloat(threshold) < trendTotalValue) {
            //    $('#approveBtn').hide();
            //}

            //Add phases


            activities = initializeActivities(activities);

            setProjectStartEndDate(activities, projects);
            setPhaseOriginalStartEndDate(viewPhase, activities);
            setProgramElementOriginalStartEndDate(activities, programElements);
            setContractOriginalStartEndDate(activities);
            //Manasi 13-01-2021
            function setProjectStartEndDate(activities, projects) {
                var maxDate = new Date(8640000000000000);
                var minDate = new Date(-8640000000000000);

                for (var i = 0; i < projects.length; i++) {
                    var originalStartDate = maxDate;
                    var originalEndDate = minDate;
                    var hasActivity = false;
                    for (var x = 0; x < activities.length; x++) {
                        if (projects[i].project == activities[x].project) {
                            var d1 = new Date(activities[x].originalStartDate);
                            var d2 = new Date(activities[x].originalEndDate);

                            if (d1 == 'Invalid Date' || d2 == 'Invalid Date') {
                                continue;
                            }

                            //d1.setDate(d1.getDate() + 1);
                            //d2.setDate(d2.getDate() + 1);

                            originalStartDate = new Date(Math.min(originalStartDate.getTime(), d1.getTime()));
                            originalEndDate = new Date(Math.max(originalEndDate.getTime(), d2.getTime()));

                            hasActivity = true;
                        }
                    }

                    if (!hasActivity) {
                        originalStartDate = 'N/A';
                        originalEndDate = 'N/A';
                    }

                    if (originalStartDate == 'Invalid Date') {
                        originalStartDate = 'N/A';
                    }
                    if (originalEndDate == 'Invalid Date') {
                        originalEndDate = 'N/A';
                    }



                    projects[i]["originalStartDate"] = originalStartDate;
                    projects[i]["originalEndDate"] = originalEndDate;
                }

            }

            function setPhaseOriginalStartEndDate(viewPhase, activities) {
                var maxDate = new Date(8640000000000000);
                var minDate = new Date(-8640000000000000);

                console.log(activities);
                for (var i = 0; i < viewPhase.length; i++) {
                    var originalStartDate = maxDate;
                    var originalEndDate = minDate;
                    var hasActivity = false;
                    for (var x = 0; x < activities.length; x++) {
                        if (activities[x].originalStartDate == "Invalid date" || activities[x].original_end_date == "Invalid date") {
                            continue;
                        }
                        var d1 = null;
                        var d2 = null;

                        d1 = new Date(activities[x].originalStartDate);
                        d2 = new Date(activities[x].originalEndDate);

                        if (d1 == 'Invalid Date' || d2 == 'Invalid Date') {
                            continue;
                        }

                        if (activities[x].project == viewPhase[i].ProjectID) {
                            if (activities[x].PhaseCode == viewPhase[i].PhaseID) {
                                console.log(originalStartDate, d1);
                                originalStartDate = new Date(Math.min(originalStartDate.getTime(), d1.getTime()));
                                originalEndDate = new Date(Math.max(originalEndDate.getTime(), d2.getTime()));

                                hasActivity = true;
                            }
                        }

                    }
                    if (!hasActivity) {
                        originalStartDate = 'N/A';
                        originalEndDate = 'N/A';
                    }

                    if (originalStartDate == 'Invalid Date') {
                        originalStartDate = 'N/A';
                    }
                    if (originalEndDate == 'Invalid Date') {
                        originalEndDate = 'N/A';
                    }

                    viewPhase[i]["originalStartDate"] = originalStartDate;
                    viewPhase[i]["originalEndDate"] = originalEndDate;
                }



            }

            function setProgramElementOriginalStartEndDate(activities, programElements) {
                var maxDate = new Date(8640000000000000);
                var minDate = new Date(-8640000000000000);
                
                for (var i = 0; i < programElements.length; i++) {
                    var originalStartDate = maxDate;
                    var originalEndDate = minDate;
                    var hasActivity = false;
                    for (var x = 0; x < activities.length; x++) {
                        if (programElements[i].program_element == activities[x].program_element) {
                            var d1 = new Date(activities[x].originalStartDate);
                            var d2 = new Date(activities[x].originalEndDate);

                            if (d1 == 'Invalid Date' || d2 == 'Invalid Date') {
                                continue;
                            }

                            //d1.setDate(d1.getDate() + 1);
                            //d2.setDate(d2.getDate() + 1);

                            originalStartDate = new Date(Math.min(originalStartDate.getTime(), d1.getTime()));
                            originalEndDate = new Date(Math.max(originalEndDate.getTime(), d2.getTime()));

                            hasActivity = true;
                        }
                    }

                    if (!hasActivity) {
                        originalStartDate = 'N/A';
                        originalEndDate = 'N/A';
                    }

                    if (originalStartDate == 'Invalid Date') {
                        originalStartDate = 'N/A';
                    }
                    if (originalEndDate == 'Invalid Date') {
                        originalEndDate = 'N/A';
                    }

                    //return {
                    //    originalStartDate: originalStartDate,
                    //    originalEndDate: originalEndDate
                    //}
                    programElements[i]["originalStartDate"] = originalStartDate;
                    programElements[i]["originalEndDate"] = originalEndDate;
                }
                
            }

            function setContractOriginalStartEndDate(activities) {
                var maxDate = new Date(8640000000000000);
                var minDate = new Date(-8640000000000000);
                var originalStartDate = maxDate;
                var originalEndDate = minDate;
                var hasActivity = false;
                for (var x = 0; x < activities.length; x++) {
                    var d1 = new Date(activities[x].originalStartDate);
                    var d2 = new Date(activities[x].originalEndDate);

                    if (d1 == 'Invalid Date' || d2 == 'Invalid Date') {
                        continue;
                    }

                    //d1.setDate(d1.getDate() + 1);
                    //d2.setDate(d2.getDate() + 1);

                    originalStartDate = new Date(Math.min(originalStartDate.getTime(), d1.getTime()));
                    originalEndDate = new Date(Math.max(originalEndDate.getTime(), d2.getTime()));

                    hasActivity = true;
                }

                if (!hasActivity) {
                    originalStartDate = 'N/A';
                    originalEndDate = 'N/A';
                }

                if (originalStartDate == 'Invalid Date') {
                    originalStartDate = 'N/A';
                }
                if (originalEndDate == 'Invalid Date') {
                    originalEndDate = 'N/A';
                }

                //return {
                //    originalStartDate: originalStartDate,
                //    originalEndDate: originalEndDate
                //}
                contract["originalStartDate"] = originalStartDate;
                contract["originalEndDate"] = originalEndDate;
            }

            // Get two instances of the Gantt object
            $scope.scheduleGanttInstance = Gantt.getGanttInstance();
            $scope.costGanttInstance = Gantt.getGanttInstance();
            //console.log('getting $scope.actualBudgetGanttInstance Gantt_Controller');
            //$scope.actualBudgetGanttInstance = Gantt.getGanttInstance();


            $scope.$watch($scope.description, function () {
                console.log($scope.description);
            }, true);

            //Save Cost


            // Schedule Gantt Chart Configuration
            ConfigScheduleGantt();

            //Schedule Gantt Event handler
            $scope.cancel = false; //use to check if the use click on the cancel button from lightbox

            //onBeforeLightbox - load all the Budget main category according to the selected phase
            $scope.scheduleGanttInstance.attachEvent("onBeforeLightBox", function (id) {    //collapsing star
                var isEdited = false;
                console.log(id);
                //If Tren is Approved => lightbox cannot be opened
                var auth = $scope.localStorageSrevice.get('authorizationData');
                if (auth.acl[9] == 0) { // Pritesh modified acl[5] to acl [9] as per database entry on 4 Aug 2020
                    dhtmlx.alert({
                        text: "You do not have access to add Activity.",
                        width: "400px"
                    });
                    $scope.isDeleteFromLightbox = false;
                    $scope.scheduleGanttInstance.deleteTask(id);
                    $scope.costGanttInstance.clearAll();
                    return false;
                }

                console.log(isCurrentTrend);
                setTimeout(function () {    //luan quest 3/6
                    $scope.scheduleGanttInstance.getLightboxSection("mainphase").node.children[0].classList.add('form-control');
                    $scope.scheduleGanttInstance.getLightboxSection("subphase").node.children[0].classList.add('form-control');

                    var widthAr = ['25%', '50%', '40%'];

                    //Luan here - For loop too slow?
                    for (var x = 0; x < 3; x++) {
                        $scope.scheduleGanttInstance.getLightboxSection("start_date").node.children[0].children[x].classList.add('form-control');
                        $scope.scheduleGanttInstance.getLightboxSection("start_date").node.children[0].children[x].style.display = 'inline';
                        $scope.scheduleGanttInstance.getLightboxSection("start_date").node.children[0].children[x].style.width = widthAr[x];

                        $scope.scheduleGanttInstance.getLightboxSection("end_date").node.children[0].children[x].classList.add('form-control');
                        $scope.scheduleGanttInstance.getLightboxSection("end_date").node.children[0].children[x].style.display = 'inline';
                        $scope.scheduleGanttInstance.getLightboxSection("end_date").node.children[0].children[x].style.width = widthAr[x];
                    }

                    ////Day
                    //$scope.scheduleGanttInstance.getLightboxSection("start_date").node.children[0].children[0].classList.add('form-control');
                    //$scope.scheduleGanttInstance.getLightboxSection("start_date").node.children[0].children[0].style.display = 'inline';
                    //$scope.scheduleGanttInstance.getLightboxSection("start_date").node.children[0].children[0].style.width = '25%';

                    //$scope.scheduleGanttInstance.getLightboxSection("end_date").node.children[0].children[0].classList.add('form-control');
                    //$scope.scheduleGanttInstance.getLightboxSection("end_date").node.children[0].children[0].style.display = 'inline';
                    //$scope.scheduleGanttInstance.getLightboxSection("end_date").node.children[0].children[0].style.width = '25%';

                    ////Month
                    //$scope.scheduleGanttInstance.getLightboxSection("start_date").node.children[0].children[1].classList.add('form-control');
                    //$scope.scheduleGanttInstance.getLightboxSection("start_date").node.children[0].children[1].style.display = 'inline';
                    //$scope.scheduleGanttInstance.getLightboxSection("start_date").node.children[0].children[1].style.width = '50%';

                    //$scope.scheduleGanttInstance.getLightboxSection("end_date").node.children[0].children[1].classList.add('form-control');
                    //$scope.scheduleGanttInstance.getLightboxSection("end_date").node.children[0].children[1].style.display = 'inline';
                    //$scope.scheduleGanttInstance.getLightboxSection("end_date").node.children[0].children[1].style.width = '50%';

                    ////Year
                    //$scope.scheduleGanttInstance.getLightboxSection("start_date").node.children[0].children[2].classList.add('form-control');
                    //$scope.scheduleGanttInstance.getLightboxSection("start_date").node.children[0].children[2].style.display = 'inline';
                    //$scope.scheduleGanttInstance.getLightboxSection("start_date").node.children[0].children[2].style.width = '40%';

                    //$scope.scheduleGanttInstance.getLightboxSection("end_date").node.children[0].children[2].classList.add('form-control');
                    //$scope.scheduleGanttInstance.getLightboxSection("end_date").node.children[0].children[2].style.display = 'inline';
                    //$scope.scheduleGanttInstance.getLightboxSection("end_date").node.children[0].children[2].style.width = '40%';

                    $scope.scheduleGanttInstance.getLightboxSection("percentage_completion").node.children[0].classList.add('form-control');
                    $('label:contains("Percentage Completion")').parent().css('padding-bottom', '0');
                    $('label:contains("Percentage Completion")').parent().css('padding-top', '10px');
                }, 1);


                var task = $scope.scheduleGanttInstance.getTask(id);

                var denyOpenLightbox = false;
                var denyMessage = "";

                //  alert("Status Id : " + $scope.trend.TrendStatusID + " ---- Trend  number : " + $scope.trend.TrendNumber);
                //luan 3/6 - ghetto fix
                $('div.gantt_modal_box.dhtmlx_modal_box.gantt-confirm.dhtmlx-confirm').remove();

                if ($scope.trend.TrendNumber == 2000) {
                    denyMessage = "Activity cannot be added or edited in forecast view";
                    denyOpenLightbox = true;
                } else if (isCurrentTrend && task.text == "Add") {
                    denyMessage = "Activity cannot be added or edited in current view once it is approved";
                    denyOpenLightbox = true;
                } else if ($scope.trend.TrendStatusID === 1 && $scope.trend.TrendNumber == 0) { // && $scope.trend.TrendNumber == 0  Pritesh on 23jul2020 as this should also be considered for trend 
                    denyMessage = "Activity cannot be added or edited in baseline view once it is approved";
                    denyOpenLightbox = true;
                }
                else if ($scope.trend.TrendStatusID === 1 && ($scope.trend.TrendNumber > 0 && $scope.trend.TrendNumber < 1000)) { //   Pritesh on 10Aug2020 as this should also be considered for trend 
                    denyMessage = "Activity cannot be added or edited in Trend view once it is approved";
                    denyOpenLightbox = true;

                }
                debugger
                // swapnil 02-10-2020
                if ($scope.trend.TrendStatusID === 3 && $scope.trend.approvedList_EmployeeID != "" && $scope.trend.approvedList_EmployeeID != null && $scope.trend.approvedList_EmployeeID != "0") {
                    denyMessage = "Activity cannot be added or edited in Trend view once it is approved";
                    denyOpenLightbox = true;
                }
                //-------------------------------------------------
                //luan quest 3/6
                //if ($scope.trend.TrendStatusID === 1 || $scope.trend.TrendNumber == 2000) {
                if (denyOpenLightbox) {

                    dhtmlx.alert({
                        //text: "Activity cannot be added if the trend is approved",
                        text: denyMessage,
                        width: "400px"
                    });

                    if (task.text == "Add") {
                        $scope.isDeleteFromLightbox = false;
                        $scope.scheduleGanttInstance.deleteTask(id);
                        $scope.costGanttInstance.clearAll();
                    }
                    return false;
                }

                if ($scope.isCostEdited || $scope.isNewCost) {
                    angular.forEach($scope.isCostEdited, function (item, key) {
                        if (item === true) {
                            isEdited = true;
                        }
                    });
                }
                //if(isEdited == true){
                //    if(!$scope.changeTask){
                //        $scope.changeTask={
                //            promise:null,
                //            pending:false
                //        }
                //    }
                //    if(!$scope.changeTask.pending){
                //        $scope.changeTask.pending = true;
                //        $scope.changeTask.promise = $timeout(function(){
                //            dhtmlx.alert({text:"A cost is being modified. Please save it first!",
                //                width:'400px'});
                //
                //            $scope.changeTask.pending = false;
                //            console.log(task);
                //            if(task){
                //                if(task.text == "Add"){
                //                    $scope.scheduleGanttInstance.deleteTask(id);
                //                }
                //            }
                //            //  $scope.scheduleGanttInstance.deleteTask(id);
                //
                //        }, 500);
                //    }
                //    return;
                //}
                var task = $scope.scheduleGanttInstance.getTask(id);
                if ($scope.temp) {
                    //  main.setValue($scope.temp);
                    if ($scope.lightBoxStartDate) {
                        console.log($scope.lightBoxStartDate, $scope.lightBoxEndDate);
                        task.start_date = $scope.lightBoxStartDate.start_date;
                        task.end_date = $scope.lightBoxEndDate.start_date;
                    }
                }
                if (task.text === "Add") {
                    //escape html tags
                    //for (var x = 0; x < $scope.subCategory.length; x++) {
                    //    $scope.subCategory[x].key = $scope.subCategory[x].key.replace(/&/g, '&#38').replace(/</g, '&#60').replace(/>/g, '&#62');
                    //    $scope.subCategory[x].label = $scope.subCategory[x].label.replace(/&/g, '&#38').replace(/</g, '&#60').replace(/>/g, '&#62');
                    //}

                    $scope.scheduleGanttInstance.updateCollection('sub', $scope.subCategory);

                }
                else {
                    //luan quest 3/6
                    if (isCurrentTrend) {
                        setTimeout(function () {
                            $scope.scheduleGanttInstance.getLightboxSection("mainphase").node.children[0].disabled = true;
                            $scope.scheduleGanttInstance.getLightboxSection("subphase").node.children[0].disabled = true;

                            $scope.scheduleGanttInstance.getLightboxSection("start_date").node.children[0].children[0].disabled = true;
                            $scope.scheduleGanttInstance.getLightboxSection("start_date").node.children[0].children[1].disabled = true;
                            $scope.scheduleGanttInstance.getLightboxSection("start_date").node.children[0].children[2].disabled = true;

                            $scope.scheduleGanttInstance.getLightboxSection("end_date").node.children[0].children[0].disabled = true;
                            $scope.scheduleGanttInstance.getLightboxSection("end_date").node.children[0].children[1].disabled = true;
                            $scope.scheduleGanttInstance.getLightboxSection("end_date").node.children[0].children[2].disabled = true;

                            //vamos
                            $('div.gantt_btn_set.gantt_left_btn_set.gantt_delete_btn_set').fadeTo(1, .4);
                            $('div.gantt_btn_set.gantt_left_btn_set.gantt_delete_btn_set').css({ pointerEvents: "none" })
                        }, 110);
                    } else if ($scope.trend.TrendNumber == 0) {
                    }
                }

                if ($scope.trend.TrendNumber == 0) {
                    setTimeout(function () {
                        $scope.scheduleGanttInstance.getLightboxSection("percentage_completion").node.children[0].disabled = true;
                    }), 110;
                }

                //  GanttCategory.getMainCategory().get({"ProgramID": delayedData[2].result[0].ProgramID,"Phase": task.parent}, function (response) {
                console.log(task.parent, $scope.OrganizationID);
                var phaseid = $scope.scheduleGanttInstance.getTask(task.parent).PhaseID;
                ProgramCategory.getMainActivityCategoryProgram().get({ "Phase": phaseid, "OrganizationID": $scope.OrganizationID }, function (response) {
                    MainCategory = response.result;
                    console.log(MainCategory);
                    if (MainCategory.length == 0) {
                        //if(task){
                        //    if(task.text == "Add"){
                        //        $scope.scheduleGanttInstance.deleteTask(id);
                        //    }
                        //}

                        //dhtmlx.alert({ text: "There are no activities assigned to this phase. Please contact your Project Manager!", width: "500px" });
                        //return;
                    }

                    //luan 3/28
                    var temp = {};
                    temp.key = 'Add New';
                    temp.label = 'Add New';
                    $scope.MainCategory.push(temp);

                    //luan 3/28 - unnecessary?
                    angular.forEach(MainCategory, function (value, key) {
                        var obj = {};
                        obj.key = value.CategoryDescription;
                        obj.label = value.CategoryDescription;
                        $scope.MainCategory.push(obj);
                    });

                    console.log($scope.MainCategory);

                    //Update main Category - use only once the first time lightbox open
                    console.log($scope.firstTime);
                    if ($scope.firstTime === true) {
                        $scope.firstTime = false;

                        //escape html tags
                        for (var x = 0; x < $scope.MainCategory.length; x++) {
                            $scope.MainCategory[x].key = $scope.MainCategory[x].key.replace(/&/g, '&#38').replace(/</g, '&#60').replace(/>/g, '&#62');
                            $scope.MainCategory[x].label = $scope.MainCategory[x].label.replace(/&/g, '&#38').replace(/</g, '&#60').replace(/>/g, '&#62');
                        }

                        $scope.scheduleGanttInstance.updateCollection("main", $scope.MainCategory);

                        $scope.scheduleGanttInstance.updateCollection("options", $scope.percentages);
                        $scope.scheduleGanttInstance.showLightbox($scope.selectedId);
                    }
                });
                return true;
            });

            //OnLightbox - get activity task (MainCategory-SubCategory) and display them as selection
            $scope.scheduleGanttInstance.attachEvent("onLightBox", function (id, task, is_new) {
                console.log('luan test');
                $scope.lightBoxTask = $scope.scheduleGanttInstance.getTask(id);

                $scope.oneTime = true;
                $scope.isUpdateTaskFromLightbox = true;
                var task = $scope.scheduleGanttInstance.getTask(id);

                var taskList = task.text.split(' - ');

                var main = $scope.scheduleGanttInstance.getLightboxSection('mainphase');

                if ($scope.temp) {
                    main.setValue($scope.temp);
                }
                var main = $scope.scheduleGanttInstance.getLightboxSection("mainphase");
                var sub = $scope.scheduleGanttInstance.getLightboxSection("subphase");
                $scope.orgActivityStartDate = $scope.scheduleGanttInstance.getLightboxSection('start_date').getValue().start_date;
                $scope.orgActivityEndDate = $scope.scheduleGanttInstance.getLightboxSection('end_date').getValue().end_date;

                if (task.text === "Add") {
                    $scope.cancel = true;
                }
                else {
                    $scope.cancel = false;
                }
                $scope.id = main.section.id;
                var taskList = task.text.split(' - ');
                if ($scope.temp !== "") {

                    $scope.temp = "";
                }
                else if ($scope.cancel === true) {
                    //sub.setValue("");
                    $scope.cancel = false;
                    main.setValue("");
                }
                else {
                    //luan 3/29
                    //PREVIOUSMAIN = taskList[0];
                    //PREVIOUSSUB = taskList[1];
                    PREVIOUSMAIN = task.BudgetCategory;
                    PREVIOUSSUB = task.BudgetSubCategory;


                    //main.setValue(taskList[0]);
                    //sub.setValue(taskList[1]);
                    main.setValue(task.BudgetCategory);
                    sub.setValue(task.BudgetSubCategory);
                }
                return true;
            });

            //onLightboxCancel - empty the MainCategory and SubCategory
            $scope.scheduleGanttInstance.attachEvent("onLightboxCancel", function (id) {
                console.log('cancel', id);

                var obj = $scope.scheduleGanttInstance.getTask(id);

                if (obj.text == 'Add') {
                    $scope.costGanttInstance.clearAll();
                    //return;
                }
                $scope.subCategory = [];
                $scope.scheduleGanttInstance.updateCollection('sub', $scope.subCategory);
                $scope.MainCategory = [];
                $scope.scheduleGanttInstance.updateCollection('main', $scope.MainCategory);

                var sub = $scope.scheduleGanttInstance.getLightboxSection('subphase');
                sub.setValue("");

                $scope.firstTime = true;
                $scope.cancel = true;

            });

            //OnlightboxSave - Add new activity when the user click on the save button in lightbox
            $scope.scheduleGanttInstance.attachEvent("onLightboxSave", function (id, task, is_new) {

                console.log(id, task, is_new);

                if (task.mainCategory.includes('\'') || task.mainCategory.includes('\"') || task.mainCategory.includes('\\')
                    || task.subCategory.includes('\'') || task.subCategory.includes('\"') || task.subCategory.includes('\\')) {
                    dhtmlx.alert('Special characters of single quote, double quote, and backslash are not allowed');
                    return;
                }

                // date validation bug fix - 9/24/15
                var task1 = $scope.scheduleGanttInstance.getTask(id);
                var phase = task1.parent;
                var mainCategory = $scope.scheduleGanttInstance.getLightboxSection('mainphase').getValue();
                var subCateogry = $scope.scheduleGanttInstance.getLightboxSection('subphase').getValue();
                var isTaskExist = false;

                //Main and Sub category must be selected
                if (mainCategory == undefined || mainCategory == null || mainCategory == ''
                    || subCateogry == undefined || subCateogry == null || subCateogry == '') {
                    //dhtmlx.alert('Main and Sub categories cannot be empty');
                    dhtmlx.alert('Main and Sub tasks cannot be empty'); //Manasi
                    return;
                }

                $scope.scheduleGanttInstance.eachTask(function (item) {
                    if (item.parent == phase) {
                        var category = item.text.split(' - ');
                        if (mainCategory == category[0] && subCateogry == category[1]) {
                            isTaskExist = true;
                            return;
                        }
                    }
                });

                if (isTaskExist && (is_new == true)) {
                    //alertBox.showAlert("Task is already exist");
                    dhtmlx.alert("Task is already exist");
                    return false;
                }

                console.log($scope.scheduleGanttInstance.getLightboxSection('start_date').getValue(), $scope.scheduleGanttInstance.getLightboxSection('end_date').getValue());

                $scope.sDateObject = $scope.scheduleGanttInstance.getLightboxSection('start_date').getValue();
                $scope.eDateObject = $scope.scheduleGanttInstance.getLightboxSection('end_date').getValue();
                //$scope.originalSDateObject = $scope.scheduleGanttInstance.getLightboxSection('originalStartDate').getValue();
                //$scope.originalEDateObject = $scope.scheduleGanttInstance.getLightboxSection('originalEndDate').getValue();
                $scope.originalSDateObject = $scope.scheduleGanttInstance.getLightboxSection('start_date').getValue();
                $scope.originalEDateObject = $scope.scheduleGanttInstance.getLightboxSection('end_date').getValue();
                console.log($scope.originalSDateObject, $scope.originalEDateObject);

                $scope.sDate = $scope.sDateObject.start_date;
                $scope.eDate = $scope.eDateObject.end_date;
                $scope.originalSDate = $scope.originalSDateObject.start_date;   //luan mark
                $scope.originalEDate = $scope.originalEDateObject.end_date;

                $scope.taskDuration = gantt.calculateDuration($scope.sDate, $scope.eDate);
                if ($scope.taskDuration <= 0) {
                    dhtmlx.alert("Start date must come before end date.");
                    return false;
                }

                if (is_new) {
                    $scope.is_new = is_new;
                    var s = $scope.scheduleGanttInstance.getLightboxSection('mainphase').getValue();
                    if (!s) {
                        dhtmlx.alert("The Activity Category cannot be empty.");
                        return false;
                    }
                    $scope.saveFromLightbox = true;
                    $scope.scheduleGanttInstance.refreshData();
                    $scope.scheduleGanttInstance.render();
                    //

                    console.log('special test 2/12', task);
                    var onTaskSelected = $scope.scheduleGanttInstance.callEvent("customAdd", [id, task]);
                } else {

                    if (moment($scope.sDate).format(sqlDateFormat) != moment($scope.orgActivityStartDate).format(sqlDateFormat)
                        && moment($scope.eDate).format(sqlDateFormat) != moment($scope.orgActivityEndDate).format(sqlDateFormat)) {
                        dhtmlx.alert({
                            text: "Modified end date and start date at the same time is not yet supported, please modify either the start date or end date!",
                            width: "650px",
                            callback: function (result) {
                                //$scope.sDateObject.start_date = $scope.orgActivityStartDate;
                                $scope.scheduleGanttInstance.showLightbox(id);
                            }
                        });

                        return false;
                    }
                    $scope.duc = false;
                    $scope.isUpdateTaskFromLightbox = true;

                    $scope.MainCategoryName = $scope.scheduleGanttInstance.getLightboxSection('mainphase').getValue();
                    $scope.SubCategoryName = $scope.scheduleGanttInstance.getLightboxSection('subphase').getValue();

                    $scope.isScaleChanged = false;
                    //  $scope.selectedActivity = $scope.scheduleGanttInstance.getTask(id);
                }

                //Empty Main and Sub Category
                $scope.subCategory = [];
                $scope.scheduleGanttInstance.updateCollection('sub', $scope.subCategory);
                $scope.firstTime = true;
                $scope.MainCategory = [];
                $scope.scheduleGanttInstance.updateCollection('main', $scope.MainCategory);
                return true;
            });

            //Fire when user click on the delete button in lightbox
            $scope.scheduleGanttInstance.attachEvent("onLightboxDelete", function (id) {
                //Empty the Main and Sub Budget Category
                var task = $scope.scheduleGanttInstance.getTask(id);
                $scope.isDeleteFromLightbox = true;
                $scope.duc = false;

                return true;
            });

            //Do Nothing if selected outside of gantt grid
            $scope.scheduleGanttInstance.attachEvent("onEmptyClick", function () {
            });

            // Schedule Gantt Chart Templates
            $scope.scheduleGanttInstance.templates.grid_indent = function (task) {
                return "<div style='width:10px; float:left; height:100%;'> </div>"
            };

            $scope.scheduleGanttInstance.templates.grid_folder = function (item) {
                return "<div class='gantt_tree_icon gantt_folder_" +
                    (item.$open ? "open" : "closed") + "'></div>";
            };

            $scope.scheduleGanttInstance.templates.grid_row_class = function (start, end, task) {
                if (task.$level != 1) {
                    if (task.id == projectMaxId) {
                        return "hideAdd phase-bold"
                    }
                    return "hideAdd ";
                }

                if (task.id >= 1000) {

                    return "phase-bold";
                }

                return "";
            }

            $scope.scheduleGanttInstance.templates.grid_header_class = function (column, config) {
            }

            $scope.scheduleGanttInstance.locale = {
                date: {
                    month_full: ["January", "February", "March", "April", "May", "June", "July",
                        "August", "September", "October", "November", "December"],
                    month_short: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep",
                        "Oct", "Nov", "Dec"],
                    day_full: ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday",
                        "Saturday"],
                    day_short: ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"]
                },
                labels: {
                    new_task: "Add",
                    icon_save: "Save",
                    icon_cancel: "Cancel",
                    icon_details: "Details",
                    icon_edit: "Edit",
                    icon_delete: "Delete",
                    confirm_closing: "",//Your changes will be lost, are you sure ?
                    confirm_deleting: "The selected item will be deleted permanently, are you sure?",
                    main: "Main activity",

                    section_mainphase: "WBS Task",
                    section_subphase: "WBS Sub Task",
                    section_start_date: "Start Date",
                    section_end_date: "End Date",
                    section_originalStartDate: "Original Start Date",
                    section_originalEndDate: "Original End Date",
                    section_percentage_completion: "Percentage Completion",
                    section_scale: "Scale",
                    section_time: "Time period",

                    /* link confirmation */

                    confirm_link_deleting: "Dependency will be deleted permanently, are you sure?",
                    link_from: "From",
                    link_to: "To",
                    link_start: "Start",
                    link_end: "End",

                    minutes: "Minutes",
                    hours: "Hours",
                    days: "Days",
                    weeks: "Week",
                    months: "Months",
                    years: "Years"
                }
            };

            // Schedule Gantt Chart Events
            $scope.scheduleGanttInstance.attachEvent("onBeforeTaskDisplay", function (id, task) {
                // $scope.isScaleChanged = true;
                var found = false;
                //console.log(task, $scope.schedulePhase);
                angular.forEach(phases, function (phase) {
                    //if ($scope.schedulePhase === phase.Code &&
                    //    (task.parent === (Number(phase.PhaseID) * 1000) ||
                    //    task.id === (Number(phase.PhaseID) * 1000))) {
                    //    found = true;
                    //}

                    //Luan here - fix phase filter
                    if ((task.id === (Number($scope.schedulePhase) * 1000) && !found) || (task.id % 1000 != 0 && task.parent === (Number($scope.schedulePhase) * 1000))) {
                        console.log(task, phase);
                        found = true;
                    }

                    if (!$scope.schedulePhase)
                        found = true;
                });
                if (found == true)
                    return true;
                return false;

            });

            $scope.scheduleGanttInstance.attachEvent("onBeforeTaskAdd", function (id, task) {
            });

            $scope.scheduleGanttInstance.attachEvent("onParse", function () {

                if ($scope.duc == true) {

                    $scope.scheduleGanttInstance.selectTask($scope.retrievedActivityID);
                }
            });

            $scope.scheduleGanttInstance.attachEvent('onTaskUnselected', function () {
                currentId = -1;
            });

            $scope.scheduleGanttInstance.attachEvent("onTaskDblClick", function (id) {
                var task = $scope.scheduleGanttInstance.getTask(id);
                console.log(task);
                //if ((id >= 1000 && id <= projectMaxId) || $scope.trend.TrendStatusID === 1) return;

                //luan quest 3/6
                //if ((task.Phasecode && id <= projectMaxId) || $scope.trend.TrendStatusID === 1 || $scope.trend.TrendNumber == 2000) return;

                var taskExist = $scope.scheduleGanttInstance.isTaskExists(id);
                if (taskExist == false) {
                    return;
                }

                $scope.selectedActivity = task;

                if (task) {
                    var CategoryID;
                    var taskList = $scope.selectedActivity.text.split(' - ');
                    // GanttCategory.getMainCategory().get({"ProgramID": delayedData[2].result[0].ProgramID,"Phase": task.parent}, function (response) {

                    console.log($scope.OrganizationID);
                    //luan quest 2/21
                    ProgramCategory.getMainActivityCategoryProgram().get({ "Phase": task.phase, "OrganizationID": $scope.OrganizationID }, function (response) {
                        MainCategory = response.result;

                        //luan 3/28
                        var temp = {};
                        temp.key = 'Add New';
                        temp.label = 'Add New';
                        $scope.MainCategory.push(temp);

                        angular.forEach(MainCategory, function (value, key) {
                            var obj = {};
                            obj.key = value.CategoryDescription;
                            obj.label = value.CategoryDescription;
                            $scope.MainCategory.push(obj);
                        });

                        console.log($scope.MainCategory);

                        if ($scope.firstTime === true) {
                            $scope.firstTime = false;

                            //escape html tags
                            for (var x = 0; x < $scope.MainCategory.length; x++) {
                                $scope.MainCategory[x].key = $scope.MainCategory[x].key.replace(/&/g, '&#38').replace(/</g, '&#60').replace(/>/g, '&#62');
                                $scope.MainCategory[x].label = $scope.MainCategory[x].label.replace(/&/g, '&#38').replace(/</g, '&#60').replace(/>/g, '&#62');
                            }

                            $scope.scheduleGanttInstance.updateCollection("main", $scope.MainCategory);
                            $scope.scheduleGanttInstance.updateCollection("options", $scope.percentages);
                            $scope.scheduleGanttInstance.showLightbox($scope.selectedId);
                        }
                        for (var i = 0; i < MainCategory.length; i++) {
                            //if (MainCategory[i].CategoryDescription === taskList[0]) {
                            if (MainCategory[i].CategoryDescription === task.BudgetCategory) {
                                CategoryID = MainCategory[i].CategoryID;
                            }
                        }
                        console.log('calling SubActivityCategory Gantt_ctrl 1');
                        console.log(task.parent);
                        var phaseid = $scope.scheduleGanttInstance.getTask(task.parent).PhaseID;
                        $http.get(serviceBasePath + "Request/SubActivityCategory/" + $scope.OrganizationID + "/" + phaseid + "/" + CategoryID).then(function (response) {
                            //  GanttCategory.getSubCategory().get({ProgramID:delayedData[2].result[0].ProgramID, Phase:task.parent , CategoryID: CategoryID },function(response){
                            // ProgramCategory.getSubActivityCategoryProgram().get({Phase: task.parent, CateogryID : CategoryID},function(response){
                            var subCategory = response.data.result;

                            console.log(response);

                            //luan 3/28
                            var temp = {};
                            temp.key = 'Add New';
                            temp.label = 'Add New';
                            $scope.subCategory.push(temp);

                            for (var i = 0; i < subCategory.length; i++) {
                                var obj = {};
                                obj.key = subCategory[i].SubCategoryDescription;
                                obj.label = subCategory[i].SubCategoryDescription;
                                $scope.subCategory.push(obj);

                            }

                            //escape html tags
                            for (var x = 0; x < $scope.subCategory.length; x++) {
                                $scope.subCategory[x].key = $scope.subCategory[x].key.replace(/&/g, '&#38').replace(/</g, '&#60').replace(/>/g, '&#62');
                                $scope.subCategory[x].label = $scope.subCategory[x].label.replace(/&/g, '&#38').replace(/</g, '&#60').replace(/>/g, '&#62');
                            }

                            $scope.scheduleGanttInstance.updateCollection("sub", $scope.subCategory);
                            $scope.scheduleGanttInstance.showLightbox($scope.selectedActivity.id);

                        });
                    });
                }

            });

            //Initialize FTE positions
            var positionsList;
            initializeFTEPositions();

            function initializeFTEPositions() {
                FTEPositions.get({}, function (FTEPositionsData) {
                    $scope.FTEPositions = [];
                    var position = {};
                    positionsList = FTEPositionsData.result;

                    //Jignesh-26-03-2021
                    var authRole = $scope.localStorageSrevice.get('authorizationData').role;
                    if (authRole === "Admin") {
                        //luan quest 3/26
                        position["name"] = '----------Add New----------';
                        position["value"] = 0;
                        $scope.FTEPositions.push(position);
                    }
                    

                    for (var i = 0; i < positionsList.length; i++) {
                        position = {};
                        position["name"] = positionsList[i].PositionDescription;
                        position["value"] = positionsList[i].Id;
                        $scope.FTEPositions.push(position);
                    }
                });
            }

            //Initialize material categories
            var materialCategoryList;
            MaterialCategory.get({}, function (materialCategoryData) {
                console.log(materialCategoryData);
                materialCategoryList = materialCategoryData.result;

                var temp = { name: '', value: '' };
                //Jignesh-26-03-2021
                var authRole = $scope.localStorageSrevice.get('authorizationData').role;
                if (authRole === "Admin") {
                    //luan quest 3/26
                    temp["name"] = '----------Add New----------';
                    temp["value"] = 0;
                    $scope.materialCategories.push(temp);
                }
                

                for (var x = 0; x < materialCategoryData.result.length; x++) {
                    temp = {};
                    temp.name = materialCategoryData.result[x].Name;
                    temp.value = materialCategoryData.result[x].ID;
                    $scope.materialCategories.push(temp);
                }

                console.log($scope.materialCategories);
            });

            //Initialize subcontractor types
            $scope.orgSubcontractorTypes = null;
            var subcontractorTypeList = [];
            SubcontractorType.get({}, function (subcontractorTypeData) {
                console.log(subcontractorTypeData);
                $scope.orgSubcontractorTypes = subcontractorTypeData.result;
                subcontractorTypeList = subcontractorTypeData.result;

                //luan quest 3/26
                var temp = { name: '', value: '' };
                //Jignesh-26-03-2021
                var authRole = $scope.localStorageSrevice.get('authorizationData').role;
                if (authRole === "Admin") {
                    temp["name"] = '----------Add New----------';
                    temp["value"] = 0;
                    $scope.subcontractorTypes.push(temp);
                }
                
                for (var x = 0; x < subcontractorTypeData.result.length; x++) {
                    temp = {};
                    temp.name = subcontractorTypeData.result[x].SubcontractorTypeName;
                    temp.value = subcontractorTypeData.result[x].SubcontractorTypeID;
                    $scope.subcontractorTypes.push(temp);
                }

                console.log($scope.subcontractorTypes);
            });

            //Initialize subcontractors
            $scope.orgSubcontractors = null;
            Subcontractor.get({}, function (subcontractorData) {
                console.log(subcontractorData);
                $scope.orgSubcontractors = subcontractorData.result;

                for (var x = 0; x < subcontractorData.result.length; x++) {
                    var temp = {};
                    temp.name = subcontractorData.result[x].SubcontractorName;
                    temp.value = subcontractorData.result[x].SubcontractorID;
                    $scope.subcontractors.push(temp);
                }
                console.log($scope.subcontractors);
            });

            function refreshSubcontractors() {
                angular.forEach($scope.orgSubcontractors, function (item) {
                    var temp = {};
                    temp.name = item.SubcontractorName;
                    temp.value = item.SubcontractorID;
                    $scope.subcontractors.push(temp);
                });
            }
            refreshSubcontractors();

            //Initialize ODC types
            var ODCTypeList;
            ODCType.get({}, function (ODCTypeData) {
                ODCTypeList = ODCTypeData.result;
                $scope.ODCTypes = [];

                //luan quest 3/26
                var temp = { name: '', value: '' };
                // Jignesh-26-03-2021
                var authRole = $scope.localStorageSrevice.get('authorizationData').role;
                if (authRole === "Admin") {
                    temp["name"] = '----------Add New----------';
                    temp["value"] = 0;
                    $scope.ODCTypes.push(temp);
                }
                

                for (var ODCi = 0; ODCi < ODCTypeList.length; ODCi++) {
                    temp = {};
                    temp["name"] = ODCTypeList[ODCi].ODCTypeName;
                    temp["value"] = ODCTypeList[ODCi].ODCTypeID;
                    $scope.ODCTypes.push(temp);
                }
                console.log($scope.ODCTypes);
            });

            //Custom select a task
            //luan quest 2/20
            $scope.scheduleGanttInstance.attachEvent("customSelect", function (id, phaseID, isPhase) {
                console.log('horizontal testing', angular.copy($scope.description));
                console.log('function mark', id);
                if ($scope.scheduleGanttInstance.isTaskExists(id) == false && !isPhase) {
                    console.log('ultimate test');
                    return false;
                }
                $scope.selectedActivity = $scope.scheduleGanttInstance.getTask(id);

                console.log($scope.selectedActivity);
                if ($scope.selectedActivity) {
                    $scope.selectedPhase = $scope.scheduleGanttInstance.getTask($scope.selectedActivity.parent);
                    //$scope.allCostTotal = $scope.selectedActivity.totalCost;
                    $scope.allCostTotal = $scope.selectedActivity.totalBudget;   //Manasi 15-07-2020
                    var amountInput = delayedData[2].result[0].Amount;
                    $scope.amount = amountInput - $scope.allCostTotal;
                    var task = $scope.scheduleGanttInstance.getTask(0);

                    paddingLabel();
                    var firstChild = null;
                    var childTask = null;
                    var testGetParent = $scope.scheduleGanttInstance.getParent(id);
                    var numberOfChildren = $scope.scheduleGanttInstance.hasChild(id);
                    if (numberOfChildren > 0) {
                        firstChild = $scope.scheduleGanttInstance.getChildren(id);
                        childTask = $scope.scheduleGanttInstance.getTask(firstChild[0]);
                    }

                    if (testGetParent === projectMaxId && numberOfChildren === false) {
                        $scope.costGanttInstance.clearAll();
                        $('#total_label').hide();
                        $('#total_cost').hide();
                    } else {
                        $scope.selectedId = id;
                        $scope.MaxFTECostID = 0;
                        $scope.MaxLumpSumCostID = 0;
                        $scope.MaxUnitCostID = 0;
                        $scope.MaxODCCostID = 0;
                        $scope.workingDays = [];
                        $scope.MaxPercentageCostID = 0;
                        $scope.isCostEdited = [];
                        $scope.method = [];
                        $scope.description = [];
                        $scope.unit_type = [];
                        $scope.employee_id = [];
                        $scope.subcontractor_id = [];
                        $scope.material_id = [];
                        $scope.unitType = [];
                        $scope.totalUnits = [];
                        $scope.unitCost = [];
                        $scope.unitBudget = [];
                        $scope.totalCost = [];
                        $scope.totalBudget = [];
                        $scope.isNewCost = [];
                        $scope.textBoxIds = [];
                        $scope.textBoxValues = [];
                        $scope.tempTBValues = [];
                        $scope.fteCosts = [];
                        $scope.unitCosts = [];
                        $scope.fteHours = [];
                        $scope.isRequestCost = true;
                        $scope.scale = [];
                        $scope.numberOfBoxes = [];
                        $scope.subCategory = [];
                        $scope.CostTrackTypes = [];
                        var bufferObj = {};

                        //var testBufferObj = {}; //holding costs
                        //testBufferObj.data =  [];
                        bufferObj.costs = {};
                        bufferObj.costs.data = [];

                        bufferObj.method = []; bufferObj.unitType = []; bufferObj.unitCost = [];
                        bufferObj.unitBudget = [];
                        bufferObj.scale = []; bufferObj.isCostEdited = []; bufferObj.isNewCost = [];
                        bufferObj.description = []; bufferObj.totalCost = []; bufferObj.totalBudget = [];
                        bufferObj.totalUnits = [];
                        bufferObj.unit_type = []; bufferObj.textBoxValues = []; bufferObj.textBoxIds = [];
                        bufferObj.employee_id = []; bufferObj.material_id = []; bufferObj.subcontractor_id = [];
                        $scope.taskBeingEdited = false;
                        var childProject = (childTask) ? Number(childTask.project) : null;
                        $scope.selectedProject = isNaN(Number($scope.selectedActivity.project)) ? (childTask) ? Number(childTask.project) : null : Number($scope.selectedActivity.project);
                        $scope.selectedTrend = isNaN(Number($scope.selectedActivity.trend)) ? (childTask) ? Number(childTask.trend) : null : Number($scope.selectedActivity.trend);
                        var workingDaysInMonth = getWorkingDays($scope.selectedActivity.start_date, $scope.selectedActivity.end_date);
                        $scope.workingDays = workingDaysInMonth.split(",");


                        if (Number(id) == projectMaxId) {
                            var phase = 'null';
                            var activity = 'null';
                        }
                        else if (Number(id) >= 1000) {
                            //luan here - replace Number(id) with Number(phaseID)
                            var phase = Number(phaseID) / 1000;
                            var activity = "null";
                        }
                        else {
                            var phase = "null";
                            var activity = Number(id);
                        }
                        //$scope.v_phase = phase;
                        // alert("isPhase: " + isPhase + " ---phaseID : " + phaseID + " -- Number ID :" + Number(id) + " -- Project MAxId : " + projectMaxId);
                        //priteshs1
                        if (!isPhase && phaseID == 0 && Number(id) != projectMaxId) {
                            //  alert(id);
                            $scope.v_activity = id;   //05-08-2020
                        }
                        else {
                            $scope.v_activity = null;
                        }
                        console.log($scope.selectedActivity);
                        if ($scope.selectedActivity.trend == 2000 || $scope.selectedActivity.trend == "2000") {
                            phase = $scope.selectedActivity.PhaseCode;
                        }

                        $scope.x = {
                            ProjectID: delayedData[2].result[0].ProjectID,
                            TrendNumber: $scope.selectedTrend,
                            Phasecode: phase,
                            ActivityID: activity
                        }
                        if ($scope.buffer.length > 0) {
                            angular.forEach($scope.buffer, function (item) {
                                if (item.activityId == $scope.selectedActivity.id && item.currentScale == $scope.scheduleScale)
                                    $scope.isRequestCost = false;
                            });
                        }
                        //what to do here...
                        if ($scope.isRequestCost == false && false) {   //luan here temporarily - switching

                            console.log($scope.buffer);
                            angular.forEach($scope.buffer, function (item) {
                                if (item.activityId == $scope.selectedActivity.id && item.currentScale == $scope.scheduleScale) {
                                    var copyItem = angular.copy(item);
                                    console.log(item);
                                    $scope.costs = angular.copy(item.costs);
                                    $scope.MaxFTECostID = angular.copy(item.MaxFTECostID);
                                    $scope.MaxLumpSumCostID = angular.copy(item.MaxLumpSumCostID);
                                    $scope.MaxUnitCostID = angular.copy(item.MaxUnitCostID);
                                    $scope.MaxODCCostID = angular.copy(item.MaxODCCostID);
                                    $scope.MaxPercentageCostID = angular.copy(item.MaxPercentageCostID);
                                    $scope.method = angular.copy(item.method);
                                    $scope.description = angular.copy(item.description);
                                    $scope.unitType = angular.copy(item.unitType);
                                    $scope.unitCost = angular.copy(item.unitCost);
                                    $scope.unitBudget = angular.copy(item.unitBudget);
                                    $scope.currentCostIndex = angular.copy(item.currentCostIndex);
                                    $scope.scale = angular.copy(item.scale);
                                    $scope.isCostEdited = angular.copy(item.isCostEdited);
                                    $scope.employee_id = angular.copy(item.employee_id);
                                    $scope.subcontractor_id = angular.copy(item.subcontractor_id);
                                    console.log($scope.subcontractor_id);
                                    $scope.material_id = angular.copy(item.material_id);
                                    $scope.isNewCost = angular.copy(item.isNewCost);
                                    $scope.totalCost = angular.copy(item.totalCost);
                                    $scope.totalBudget = angular.copy(item.totalBudget);
                                    $scope.totalUnits = angular.copy(item.totalUnits);
                                    console.log('total here');
                                    $scope.textBoxValues = angular.copy(item.textBoxValues);
                                    $scope.textBoxIds = angular.copy(item.textBoxIds);
                                    $scope.unit_type = angular.copy(item.unit_type);
                                    syncCostType();
                                    appendNewCostFromBuffer(item, true);

                                    $scope.costGanttInstance.clearAll();
                                    $scope.costGanttInstance.config.start_date = $scope.scheduleGanttInstance.getState().min_date;
                                    $scope.costGanttInstance.config.end_date = $scope.scheduleGanttInstance.getState().max_date;
                                    $scope.costGanttInstance.parse($scope.costs, "json");
                                    $scope.costGanttInstance.render();
                                    //setTimeout(function () { refreshHtmlDelete(); }, 1000);
                                    return true;
                                }
                            });

                        } else {
                            console.log(delayedData);
                            console.log($scope.selectedTrend);
                            console.log(activity);
                            console.log($scope.selectedActivity);
                            if (!activity || activity == "null")
                                activity = $scope.selectedActivity.id;
                            if (!$scope.selectedActivity.PhaseCode)
                                activity = "null";
                            var obj = {
                                ProjectID: delayedData[2].result[0].ProjectID,
                                TrendNumber: delayedData[3],
                                PhaseCode: phase,
                                ActivityID: activity,
                                Granularity: $scope.scheduleScale,
                                BudgetID: $scope.selectedActivity.BudgetID,
                                BudgetCategory: $scope.selectedActivity.BudgetCategory,
                                BudgetSubCategory: $scope.selectedActivity.BudgetSubCategory,
                                ViewLabor: $scope.localStorageSrevice.get('authorizationData').acl[15]
                            };
                            console.log(obj);

                            Cost.get(obj, function (CostData) {
                                //luan quest 2/14
                                console.log(CostData);
                                //luan here 4/23

                                $scope.CostData = CostData.CostRow;

                                //angular.forEach(CostData.CostRow, function (item) {
                                //    item.StartDate = moment(item.StartDate).format(sqlDateFormat);
                                //    item.EndDate = moment(item.EndDate).format(sqlDateFormat);
                                //});
                                $scope.currentCostIndex = 1;
                                $scope.MaxFTECostID = CostData.MaxFTECostID;
                                $scope.MaxLumpSumCostID = CostData.MaxLumpsumCostID;
                                $scope.MaxUnitCostID = CostData.MaxUnitCostID;
                                $scope.MaxODCCostID = CostData.MaxODCCostID;
                                $scope.MaxPercentageCostID = CostData.MaxPercentageCostID;

                                console.log(CostData);
                                var costs = CostData.CostRow;
                                $scope.originalCostData = angular.copy(costs);
                                $scope.costs.data = [];
                                console.log(costs);
                                if (costs.length > 0) {
                                    for (var i = 0; i < costs.length; i++) {
                                        var UT;


                                        if (costs[i].CostType === "U") {
                                            for (var j = 0; j < $scope.unitTypes.length; j++) {
                                                if ($scope.unitTypes[j].name === costs[i].UnitType) {
                                                    UT = j;
                                                }
                                            }
                                        }
                                        var cost = {};

                                        //luan 4/24
                                        var expandCloseClass = '';
                                        var taskID = (i + 1) + "_cost_line_id";

                                        if ($scope.trend.TrendNumber == 1000 && costs[i].CostTrackTypeID == 2) {    //Add plus icon for budget lines only   Manasi 10-11-2020
                                            console.log('1000 in current');
                                            expandCloseClass = 'gantt_tree_icon gantt_close';
                                        }

                                        cost["estimated_cost_id"] = costs[i].EstimatedCostID;
                                        cost["cost_track_type_id"] = costs[i].CostTrackTypeID;
                                        cost["id"] = $scope.currentCostIndex;
                                        cost["cost_id"] = costs[i].CostID;
                                        cost["text"] =
                                            (costs[i].CostType === "F") ? "<div id='" + taskID + "' class='" + expandCloseClass + "'></div> Labor" :  //luan quest
                                                //(costs[i].CostType === "L") ? "<div id='" + taskID + "' class='" + expandCloseClass + "'></div> Contractor" :  //luan quest
                                                (costs[i].CostType === "L") ? "<div id='" + taskID + "' class='" + expandCloseClass + "'></div> Subcontractor" :    //Manasi 17-07-2020
                                                    (costs[i].CostType === "U") ? "<div id='" + taskID + "' class='" + expandCloseClass + "'></div> Material" :  //luan quest
                                                        (costs[i].CostType === "ODC") ? "<div id='" + taskID + "' class='" + expandCloseClass + "'></div> ODC" :  //luan quest
                                                            (costs[i].CostType === "P") ? "% Basis" : "Error";
                                        cost["cost_type"] = costs[i].CostType;
                                        cost["description"] = costs[i].Description;
                                        cost["material_id"] = costs[i].MaterialID;
                                        cost["unit_type"] =
                                            (costs[i].CostType === "F") ? "FTE" :
                                                //(costs[i].CostType === "L") ? "Contractor" :
                                                //(costs[i].CostType === "L") ? "Subcontractor" :    //Manasi 17-07-2020
                                                (costs[i].CostType === "L") ? "USD" :    //Manasi 05-08-2020
                                                    (costs[i].CostType === "U") ? costs[i].UnitType :
                                                        (costs[i].CostType === "ODC") ? "USD" :
                                                            (costs[i].CostType === "P") ? "Percentage" : "Error";
                                        // Swapnil 24-11-2020-----------------------------------------------------
                                        cost["actual_rate"] = (costs[i].ActualRate == '' || costs[i].ActualRate == null) ? '0' : costs[i].ActualRate;
                                        cost["total_actual_cost"] = (costs[i].TotalActualCost == '' || costs[i].TotalActualCost == null) ? '0' : costs[i].TotalActualCost;
                                        //-------------------------------------------------------------------
                                        //luan here 4/23
                                        var CostTrackTypeArray = [];
                                        var FTECostArray = [];
                                        var FTEHoursArray = [];
                                        var MaterialCostWithOverheadArray = [];
                                        var MaterialQuantityArray = [];
                                        var averageRate = 0;
                                        console.log(costs[i]);

                                        if (costs[i].CostTrackTypes) {
                                            CostTrackTypeArray = costs[i].CostTrackTypes.split(',').map(function (item) {
                                                return item.trim();
                                            });;
                                        }

                                        if (costs[i].FTECost) {
                                            FTECostArray = costs[i].FTECost.split(',').map(function (item) {
                                                return item.trim();
                                            });;
                                        }
                                        if (costs[i].FTEHours) {
                                            FTEHoursArray = costs[i].FTEHours.split(',').map(function (item) {
                                                return item.trim();
                                            });;
                                        }

                                        if (costs[i].CostWithOverhead) {
                                            MaterialCostWithOverheadArray = costs[i].CostWithOverhead.split(',').map(function (item) {
                                                return item.trim();
                                            });;
                                        }
                                        if (costs[i].TextBoxValue) {
                                            MaterialQuantityArray = costs[i].TextBoxValue.split(',').map(function (item) {
                                                return item.trim();
                                            });;
                                        }

                                        if (costs[i].CostTrackTypeID == 3 && costs[i].CostType == 'F') {   //Actual and ETC
                                            //var tempTotalRates = 0;
                                            //var cellCount = 0;
                                            //for (var x = 0; x < FTECostArray.length; x++) {
                                            //    //if (CostTrackTypeArray[x] == '3') {
                                            //    cellCount++;
                                            //    tempTotalRates += (parseFloat(FTECostArray[x]) / (parseFloat(FTEHoursArray[x]) * 8));
                                            //    //}
                                            //}
                                            //console.log(tempTotalRates);
                                            //averageRate = tempTotalRates / cellCount;
                                            //costs[i].Base = roundToTwo(averageRate);
                                        } else if (costs[i].CostTrackTypeID == 1 && costs[i].CostType == 'F') {
                                            costs[i].RawUnitPrice = costs[i].RawUnitPrice
                                        }

                                        if (costs[i].CostTrackTypeID == 3 && costs[i].CostType == 'U') {   //Actual and ETC
                                            //var tempTotalRates = 0;
                                            //var cellCount = 0;
                                            //for (var x = 0; x < MaterialCostWithOverheadArray.length; x++) {
                                            //    //if (CostTrackTypeArray[x] == '3') {
                                            //    cellCount++;
                                            //    tempTotalRates += (parseFloat(MaterialCostWithOverheadArray[x]) / parseFloat(MaterialQuantityArray[x]));
                                            //    //}
                                            //}
                                            //console.log(tempTotalRates);
                                            //averageRate = tempTotalRates / cellCount;
                                            //costs[i].Base = roundToTwo(averageRate);
                                        }



                                        console.log(FTECostArray, CostTrackTypeArray, FTEHoursArray);
                                        cost["unit_cost"] = costs[i].Base;
                                        if (delayedData[3] != 2000 && delayedData[3] != 3000) {
                                            if (costs[i].CostType === "F")  //Manasi 29-07-2020
                                                cost["unit_budget"] = $scope.getCostWithOverhead(costs[i].Base, 'F');
                                            else if (costs[i].CostType == "U")
                                                cost["unit_budget"] = $scope.getCostWithOverhead(costs[i].Base, 'U');
                                            else if (costs[i].CostType == "ODC")
                                                cost["unit_budget"] = $scope.getCostWithOverhead(costs[i].Base, 'ODC');
                                            else if (costs[i].CostType == "L")
                                                cost["unit_budget"] = $scope.getCostWithOverhead(costs[i].Base, 'L');
                                        }
                                        else if (delayedData[3] == 2000) {
                                            if (costs[i].CostType === "F")  //Manasi 29-07-2020
                                                cost["unit_budget"] = costs[i].RawUnitPrice;
                                            else if (costs[i].CostType == "U")
                                                cost["unit_budget"] = costs[i].RawUnitPrice;
                                            else if (costs[i].CostType == "ODC")
                                                cost["unit_budget"] = costs[i].RawUnitPrice;
                                            else if (costs[i].CostType == "L")
                                                cost["unit_budget"] = costs[i].RawUnitPrice;
                                        }

                                        else if (delayedData[3] == 3000) {
                                            if (costs[i].CostType === "F")  //Manasi 29-07-2020
                                                cost["unit_budget"] = costs[i].RawUnitPrice;
                                            else if (costs[i].CostType == "U")
                                                cost["unit_budget"] = costs[i].RawUnitPrice;
                                            else if (costs[i].CostType == "ODC")
                                                cost["unit_budget"] = costs[i].RawUnitPrice;
                                            else if (costs[i].CostType == "L")
                                                cost["unit_budget"] = costs[i].RawUnitPrice;
                                        }

                                        cost["unit_budget"] = parseFloat(cost["unit_budget"]); // PRiteshcc on 24 jul 2020 

                                        //cost["unit_budget"] = $scope.getCostWithOverhead(costs[i].Base);
                                        // cost["unit_budget"] = costs[i].RawUnitPrice;
                                        //if (costs[i].CostType == 'F') {

                                        //}else(costs[i].CostType == "U"){

                                        //}else if(costs[i].CostType == )

                                        cost["CostLineItemID"] = costs[i].CostLineItemID;
                                        var totalCost = 0;
                                        var totalBudget = 0;
                                        var EAC = 0;
                                        if (costs[i].FTECost) {
                                            var individualCosts = costs[i].FTECost.split(",");
                                            if (costs[i].CostTrackTypeID == $scope.costTrackType.ACTUAL || costs[i].CostTrackTypeID == $scope.costTrackType.ESTIMATE_TO_COMPLETION) {
                                                $.each(individualCosts, function (index) {
                                                    totalCost += parseFloat(individualCosts[index]);
                                                });

                                                EAC = totalCost * parseFloat(cost["unit_budget"]) / parseFloat(costs[i].Base);
                                                //  alert(EAC);
                                                totalCost = EAC;
                                                totalBudget = 0;
                                            } else {

                                                $.each(individualCosts, function (index) {
                                                    totalCost += parseFloat(individualCosts[index]);
                                                });

                                                totalBudget = totalCost * parseFloat(cost["unit_budget"]) / parseFloat(costs[i].Base);
                                                //  alert(totalBudget);
                                            }

                                        }
                                        else {
                                            var individualCosts = costs[i].CostWithOverhead.split(",");
                                            if (costs[i].CostTrackTypeID == $scope.costTrackType.ACTUAL || costs[i].CostTrackTypeID == $scope.costTrackType.ESTIMATE_TO_COMPLETION) {
                                                $.each(individualCosts, function (index) {
                                                    totalCost += parseFloat(individualCosts[index]);
                                                });
                                                var individualPrices = costs[i].RawUnitPrice.split(",");
                                                $.each(individualPrices, function (index) {
                                                    totalBudget += parseFloat(individualPrices[index]);
                                                });
                                                totalCost = totalBudget;
                                                totalBudget = 0;
                                            } else {
                                                var individualPrices = costs[i].RawUnitPrice.split(",");
                                                $.each(individualPrices, function (index) {
                                                    totalBudget += parseFloat(individualPrices[index]);
                                                });
                                            }


                                        }

                                        var totalUnits = 0;
                                        if (costs[i].CostType === "F") {
                                            var hours = costs[i].FTEHours.split(',');
                                            var total = 0.0;
                                            for (var j = 0; j < hours.length; j++) {
                                                total += parseFloat(hours[j]);
                                            }
                                            cost["total_units"] = roundToTwo(total * 8);//Math.round(total * 8);
                                            angular.forEach($scope.employees, function (item) {

                                                if (item.value == costs[i].EmployeeID) {
                                                    console.log(item);
                                                    cost["employee_id"] = item.name; //display view
                                                    $scope.employee_id[$scope.currentCostIndex] = item;
                                                    bufferObj.employee_id[$scope.currentCostIndex] = item;
                                                }
                                            });
                                        }
                                        else if (costs[i].CostType === "L") {
                                            cost["total_units"] = "N/A";
                                            cost["unit_cost"] = 'N/A';
                                            cost["unit_budget"] = 'N/A';
                                            cost['employee_id'] = 'N/A';

                                            console.log($scope.subcontractors);

                                            angular.forEach($scope.subcontractors, function (item) {
                                                console.log(item, costs[i].SubcontractorID);
                                                if (item.value == costs[i].SubcontractorID) {
                                                    console.log(item);
                                                    cost["employee_id"] = item.name; //display view
                                                    $scope.subcontractor_id[$scope.currentCostIndex] = item;
                                                    bufferObj.subcontractor_id[$scope.currentCostIndex] = item;
                                                }
                                            });
                                        }
                                        else if (costs[i].CostType === "U") {
                                            var individualUnits = costs[i].TextBoxValue.split(",");

                                            $scope.unit_type[$scope.currentCostIndex] = {
                                                name: costs[i].UnitType,
                                                value: $scope.unitTypes[UT].value
                                            };
                                            bufferObj.unit_type[$scope.currentCostIndex] = {
                                                name: costs[i].UnitType,
                                                value: $scope.unitTypes[UT].value
                                            }
                                            $.each(individualUnits, function (index) {
                                                totalUnits += parseFloat(individualUnits[index]);
                                            });
                                            cost["total_units"] = roundToTwo(totalUnits);
                                            if (costs[i].CostTrackTypeID == $scope.costTrackType.ACTUAL || costs[i].CostTrackTypeID == $scope.costTrackType.ESTIMATE_TO_COMPLETION) {

                                                totalCost = totalUnits * costs[i].Base;
                                                totalBudget = totalUnits * cost["unit_budget"];
                                                totalCost = totalBudget;
                                                totalBudget = 0;
                                            } else {
                                                totalCost = totalUnits * costs[i].Base;
                                                totalBudget = totalUnits * cost["unit_budget"];

                                                totalCost = 0;
                                                console.log(MaterialCostWithOverheadArray);
                                                for (var x = 0; x < MaterialCostWithOverheadArray.length; x++) {
                                                    totalCost += parseFloat(MaterialCostWithOverheadArray[x]);
                                                }
                                            }




                                            console.log($scope.materials);
                                            angular.forEach($scope.materials, function (item) {

                                                if (item.value == costs[i].MaterialID) {
                                                    console.log(item);
                                                    cost["employee_id"] = item.name;
                                                    $scope.material_id[$scope.currentCostIndex] = item;
                                                    bufferObj.material_id[$scope.currentCostIndex] = item;
                                                }
                                            });
                                        } else if (costs[i].CostType === "ODC") {
                                            cost["total_units"] = "N/A";
                                            cost["unit_cost"] = 'N/A';
                                            cost["unit_budget"] = 'N/A';
                                            cost['employee_id'] = 'N/A';
                                        } else if (costs[i].CostType === "P") {
                                            var individualUnits = costs[i].TextBoxValue.split(",");

                                            $.each(individualUnits, function (index) {
                                                totalUnits += parseFloat(individualUnits[index]);
                                            });
                                            cost["total_units"] = totalUnits;
                                            totalCost = totalUnits * costs[i].Base / 100;
                                        }
                                        console.log(totalCost);
                                        $scope.totalCost[$scope.currentCostIndex] = roundToTwo(totalCost);
                                        console.log($scope.currentCostIndex);
                                        bufferObj.totalCost[$scope.currentCostIndex] = roundToTwo(totalCost);

                                        $scope.totalBudget[$scope.currentCostIndex] = roundToTwo(totalBudget);
                                        bufferObj.totalBudget[$scope.currentCostIndex] = roundToTwo(totalBudget);
                                        if (costs[i].CostTrackTypeID == $scope.costTrackType.ACTUAL || costs[i].CostTrackTypeID == $scope.costTrackType.ESTIMATE_TO_COMPLETION)
                                            cost["total_cost"] = (Number(EAC) % 1 > 0) ? roundToTwo(totalCost) : roundToTwo(totalCost);
                                        else
                                            cost["total_cost"] = 0;
                                        cost["total_budget"] = (Number(totalBudget) % 1 > 0) ? roundToTwo(totalBudget) : roundToTwo(totalBudget);

                                        cost["scale"] = costs[i].Scale;
                                        cost["open"] = false;

                                        cost["individual_start_date"] = costs[i].StartDate;
                                        cost["individual_end_date"] = costs[i].EndDate;

                                        if ($scope.scheduleScale === 'week') {
                                            var start = moment($scope.selectedActivity.start_date).clone().startOf('isoWeek');
                                            if (moment($scope.selectedActivity.end_date).isoWeekday() == 1) {
                                                var end = moment($scope.selectedActivity.end_date);
                                            } else {
                                                var end = moment($scope.selectedActivity.end_date).clone().endOf('isoWeek');
                                            }
                                            //$scope.activity_start_of_week = start;
                                            //$scope.activity_end_of_week = end;
                                            cost["original_start_date"] = start;
                                            cost["original_end_date"] = end;
                                            cost["start_date"] = start;
                                            cost["end_date"] = end;
                                        }
                                        if ($scope.scheduleScale === "month") {
                                            cost["original_start_date"] = moment($scope.selectedActivity.original_start_date).clone().startOf('month');
                                            cost["original_end_date"] = moment($scope.selectedActivity.original_end_date).clone().endOf('month');
                                            cost["start_date"] = moment($scope.selectedActivity.start_date).clone().startOf('month');
                                            cost["end_date"] = moment($scope.selectedActivity.end_date).clone().endOf('month');
                                        }
                                        if ($scope.scheduleScale === "year") {
                                            cost["original_start_date"] = moment($scope.selectedActivity.original_start_date).clone().startOf('year');
                                            cost["original_end_date"] = moment($scope.selectedActivity.original_end_date).clone().endOf('year');
                                            cost["start_date"] = moment($scope.selectedActivity.start_date).clone().startOf('year');
                                            cost["end_date"] = moment($scope.selectedActivity.end_date).clone().endOf('year');
                                        }
                                        cost["phase"] = $scope.selectedActivity.parent;
                                        cost["save"] = "<span class='notClickableFont'><i class='fa fa-save'></i></span>";
                                        console.log(costs[i]);
                                        if (costs[i].CostTrackTypeID == $scope.costTrackType.ACTUAL || costs[i].CostTrackTypeID == $scope.costTrackType.ESTIMATE_TO_COMPLETION)
                                            //cost["cost_track_type"] = "A-E";
                                            cost["cost_track_type"] = "A/F";
                                        else if (costs[i].CostTrackTypeID == $scope.costTrackType.BUDGET)
                                            cost["cost_track_type"] = "B"
                                        // else
                                        //cost["delete"] = "<span class='notClickableFont'><i class='fa fa-trash'></i></span>";
                                        //cost["costLineItemIdNew"] = costs[i].CostLineItemID; // Pritesh
                                        cost["costLineItemIdNew"] = costs[i].CostLineItemID;  //Manasi 02-12-2020
                                        cost["newCostLineItemId"] = costs[i].CostLineItemID.substring(18) == 'undefined' ? '' : costs[i].CostLineItemID.substring(18);    //Manasi 06-11-2020
                                        //cost["newCostLineItemId"] = costs[i].CostLineItemID == 'undefined' ? '' : costs[i].CostLineItemID;    //Manasi 06-11-2020
                                        cost["delete"] = htmlDelete;

                                        cost["info"] = "<span class='notClickableFont'><i class='fa fa-info'></i></span>";
                                        cost["project"] = $scope.selectedProject;
                                        cost["trend"] = $scope.selectedTrend;
                                        cost["program_element"] = $scope.selectedActivity.ProgramElementID;
                                        cost["program"] = $scope.selectedActivity.ProgramID;
                                        cost["activity"] = $scope.selectedActivity.id;
                                        cost['isSelected'] = false;
                                        cost['totalDays'] = 0;
                                        $scope.method[$scope.currentCostIndex] = costs[i].CostType;
                                        bufferObj.method[$scope.currentCostIndex] = costs[i].CostType;
                                        //luan quest 2/27
                                        $scope.description[$scope.currentCostIndex] = {
                                            name: costs[i].Description,
                                            value: ""
                                        };

                                        bufferObj.description[$scope.currentCostIndex] = {
                                            name: costs[i].Description,
                                            value: ""
                                        };

                                        console.log(costs[i]);
                                        if (costs[i].CostType === "F") {
                                            $scope.description[$scope.currentCostIndex].value = costs[i].FTEPositionID;
                                            bufferObj.description[$scope.currentCostIndex].value = costs[i].FTEPositionID;
                                        }
                                        else if (costs[i].CostType === "L") {
                                            $scope.description[$scope.currentCostIndex].value = costs[i].SubcontractorTypeID;
                                            bufferObj.description[$scope.currentCostIndex].value = costs[i].SubcontractorTypeID;
                                        }
                                        else if (costs[i].CostType === "U") {
                                            $scope.description[$scope.currentCostIndex].value = costs[i].MaterialCategoryID;
                                            bufferObj.description[$scope.currentCostIndex].value = costs[i].MaterialCategoryID;
                                        }
                                        else if (costs[i].CostType === "ODC") {
                                            $scope.description[$scope.currentCostIndex].value = costs[i].ODCTypeID;
                                            bufferObj.description[$scope.currentCostIndex].value = costs[i].ODCTypeID;

                                        }
                                        console.log(angular.copy($scope.description[$scope.currentCostIndex]));


                                        $scope.unitType[$scope.currentCostIndex] = costs[i].Description;
                                        bufferObj.unitType[$scope.currentCostIndex] = costs[i].Description;

                                        if (costs[i].CostType === "F") {
                                            $scope.totalCost[$scope.currentCostIndex] = 0;
                                            bufferObj.totalCost[$scope.currentCostIndex] = 0;
                                            $scope.totalBudget[$scope.currentCostIndex] = 0;
                                            bufferObj.totalBudget[$scope.currentCostIndex] = 0;
                                            var hoursArray = costs[i].FTEHours.split(",");
                                            var costArray = costs[i].FTECost.split(",");

                                            $.each(hoursArray, function (index) {
                                                $scope.totalUnits[$scope.currentCostIndex] += parseFloat(this);
                                                bufferObj.totalUnits[$scope.currentCostIndex] += parseFloat(this);
                                                console.log('total here');
                                            });

                                            $.each(costArray, function (index) {
                                                $scope.totalCost[$scope.currentCostIndex] += parseFloat(this);
                                                bufferObj.totalUnits[$scope.currentCostIndex] += parseFloat(this);

                                                // $scope.totalBudget[$scope.currentCostIndex] += ($scope.isBillableRate) ?  parseFloat(this) * LABOR_RATE  : parseFloat(this); 
                                                $scope.totalBudget[$scope.currentCostIndex] += roundToTwo($scope.getCostWithOverhead(parseFloat(this), 'F'));
                                                cost['totalDays'] += parseFloat(this);
                                                console.log('total here');
                                            });
                                        }
                                        console.log(costs[i]);

                                        //luan here 4/23
                                        $scope.unitCost[$scope.currentCostIndex] = costs[i].Base;
                                        // $scope.unitBudget[$scope.currentCostIndex] = costs[i].RawUnitPrice;
                                        if (costs[i].CostType === "F")
                                            $scope.unitBudget[$scope.currentCostIndex] = $scope.getCostWithOverhead(costs[i].Base, 'F');
                                        else if (costs[i].CostType == "U")
                                            $scope.unitBudget[$scope.currentCostIndex] = $scope.getCostWithOverhead(costs[i].Base, 'U');
                                        else if (costs[i].CostType == "ODC")
                                            $scope.unitBudget[$scope.currentCostIndex] = $scope.getCostWithOverhead(costs[i].Base, 'ODC');
                                        else if (costs[i].CostType == "L")
                                            $scope.unitBudget[$scope.currentCostIndex] = $scope.getCostWithOverhead(costs[i].Base, 'L');

                                        bufferObj.unitCost[$scope.currentCostIndex] = costs[i].Base;
                                        bufferObj.unitBudget[$scope.currentCostIndex] = $scope.unitBudget[$scope.currentCostIndex];
                                        $scope.textBoxIds[$scope.currentCostIndex] = costs[i].TextBoxID.split(",");
                                        //cost['TextBoxID'] = costs[i].TextBoxID.split(",");
                                        //cost['TextBoxValues'] = costs[i].TextBoxValue.split(",");
                                        bufferObj.textBoxIds[$scope.currentCostIndex] = costs[i].TextBoxID.split(',');
                                        $scope.textBoxValues[$scope.currentCostIndex] = costs[i].TextBoxValue.split(",");
                                        bufferObj.textBoxValues[$scope.currentCostIndex] = costs[i].TextBoxValue.split(',');
                                        $scope.CostTrackTypes[$scope.currentCostIndex] = costs[i].CostTrackTypes.split(",");
                                        console.log($scope.CostTrackTypes, costs[i].CostTrackTypes);
                                        var len = $scope.textBoxValues[$scope.currentCostIndex].length;
                                        var tempTextBox = angular.copy($scope.textBoxValues[$scope.currentCostIndex]);
                                        for (var k = 0; k < len; k++) {
                                            //$scope.textBoxValues[$scope.currentCostIndex][k] = parseFloat(tempTextBox[k]).toFixed(5);
                                            $scope.textBoxValues[$scope.currentCostIndex][k] = parseFloat(tempTextBox[k]).toFixed(2);  //Pritesh 11-08-2020
                                        }
                                        console.log($scope.textBoxValues[$scope.currentCostIndex]);
                                        console.log(len);
                                        console.log($scope.currentCostIndex);
                                        console.log($scope.textBoxValues);

                                        $scope.scale[$scope.currentCostIndex] = costs[i].Granularity;
                                        bufferObj.scale[$scope.currentCostIndex] = costs[i].Granularity;
                                        $scope.isCostEdited[$scope.currentCostIndex] = false;
                                        bufferObj.isCostEdited[$scope.currentCostIndex] = false;
                                        $scope.isNewCost[$scope.currentCostIndex] = false;
                                        bufferObj.isNewCost[$scope.currentCostIndex] = false;

                                        $scope.currentCostIndex++;

                                        $scope.costs.data.push(cost);
                                        bufferObj.costs.data.push(cost);

                                    }
                                    console.log($scope.description);

                                    applyExpandables();
                                }



                                console.log('luan test');
                                //
                                // Last empty row
                                if ($scope.isRequestCost == false)
                                    //return true;  //luan quest 2 - was here to comment it out

                                    if ($scope.currentScale != $scope.scheduleScale) {
                                        appendNewCostFromBuffer(bufferObj, false);
                                    }
                                cost = {};
                                cost["id"] = $scope.currentCostIndex;
                                cost["estimated_cost_id"] = "";
                                cost["cost_track_type_id"] = 1;
                                cost["costLineItemIdNew"] = "";
                                cost["newCostLineItemId"] = "";   //Manasi 06-11-2020
                                cost["cost_id"] = "";
                                cost["text"] = "";
                                cost["description"] = "";
                                cost["unit_type"] = "";
                                cost["UnitType"] = "";
                                cost["unit_cost"] = "";
                                cost["unit_budget"] = "";
                                cost["total_units"] = "";
                                cost["total_cost"] = "";
                                cost["total_budget"] = "";
                                cost["scale"] = "";
                                cost["material_id"] = "";
                                cost["employee_id"] = "";
                                cost["subcontractor_id"] = "";
                                cost["open"] = false;
                                if ($scope.scheduleScale === "week") {
                                    var start = moment($scope.selectedActivity.start_date).clone().startOf('isoWeek');
                                    if (moment($scope.selectedActivity.end_date).isoWeekday() == 1) {
                                        var end = moment($scope.selectedActivity.end_date);
                                    } else {
                                        var end = moment($scope.selectedActivity.end_date).clone().endOf('isoWeek');
                                    }
                                    //$scope.activity_start_of_week = start;
                                    //$scope.activity_end_of_week = end;
                                    cost["original_start_date"] = moment(start);
                                    cost["original_end_date"] = moment(end);
                                    cost["start_date"] = moment(start);
                                    cost["end_date"] = moment(end);
                                }
                                if ($scope.scheduleScale === "month") {

                                    var start = moment($scope.selectedActivity.start_date).clone().startOf('month');
                                    var end = moment($scope.selectedActivity.end_date).clone().endOf('month');

                                    cost["original_start_date"] = start;
                                    cost["original_end_date"] = end;
                                    cost["start_date"] = start;
                                    cost["end_date"] = end;
                                }
                                if ($scope.scheduleScale === "year") {
                                    var start = moment($scope.selectedActivity.start_date).clone().startOf('year');
                                    var end = moment($scope.selectedActivity.end_date).clone().endOf('year');
                                    cost["original_start_date"] = start;
                                    cost["original_end_date"] = end;
                                    cost["start_date"] = start;
                                    cost["end_date"] = end;
                                }
                                cost['isSelected'] = false;
                                cost["phase"] = $scope.selectedActivity.parent;
                                cost["save"] = "<span class='notClickableFont'><i class='fa fa-save'></i></span>";
                                cost["delete"] = htmlDelete;
                                $compile(cost["delete"])($scope);

                                cost["project"] = $scope.selectedProject;
                                cost["trend"] = $scope.selectedTrend;
                                cost["program_element"] = $scope.selectedActivity.ProgramElementID;
                                cost["program"] = $scope.selectedActivity.ProgramID;
                                cost["activity"] = $scope.selectedActivity.id;
                                cost["CostLineItemID"] = "";

                                //luan quest 2/21 - Add the blank row if you're in a pending trend AND you're selecting an activity
                                if ($scope.trend.TrendStatusID == 3 && !$scope.selectedActivity.PhaseID && $scope.selectedActivity.parent != 0) {   //Manasi 10-11-2020
                                    $scope.costs.data.push(cost);
                                    bufferObj.costs.data.push(cost);
                                }

                                $scope.isCostEdited[$scope.currentCostIndex] = false;
                                bufferObj.isCostEdited[$scope.currentCostIndex] = false;
                                $scope.isNewCost[$scope.currentCostIndex] = true;
                                bufferObj.isNewCost[$scope.currentCostIndex] = true;


                                if ($scope.costs.data.length > 0) {
                                    if ($scope.isRequestCost == true) {

                                        if ($scope.scheduleScale === "week")
                                            bufferObj.currentScale = 'week';
                                        else if ($scope.scheduleScale === "month")
                                            bufferObj.currentScale = 'month';
                                        else
                                            bufferObj.currentScale = 'year';
                                        bufferObj.activityId = $scope.selectedActivity.id;
                                        bufferObj.MaxFTECostID = CostData.MaxFTECostID;
                                        bufferObj.MaxLumpSumCostID = CostData.MaxLumpsumCostID;
                                        bufferObj.MaxUnitCostID = CostData.MaxUnitCostID;
                                        bufferObj.MaxODCCostID = CostData.MaxODCCostID;
                                        bufferObj.MaxPercentageCostID = CostData.MaxPercentageCostID;
                                        bufferObj.currentCostIndex = $scope.currentCostIndex;
                                        $scope.buffer.push(bufferObj);
                                    }
                                }


                                if ($scope.currentScale != $scope.scheduleScale) {
                                    syncNewCost(bufferObj);

                                }

                                $scope.CopyTextBoxValues = angular.copy($scope.textBoxValues);
                                $scope.costGanttInstance.clearAll();
                                $scope.costGanttInstance.config.start_date = $scope.scheduleGanttInstance.getState().min_date;
                                $scope.costGanttInstance.config.end_date = $scope.scheduleGanttInstance.getState().max_date;
                                console.log($scope.description);

                                console.log('what', angular.copy($scope.costs));

                                //Escape html tags
                                for (var x = 0; x < $scope.costs.data.length; x++) {
                                    $scope.costs.data[x].description = $scope.costs.data[x].description.replace(/&/g, '&#38').replace(/</g, '&#60').replace(/>/g, '&#62');
                                    $scope.costs.data[x].employee_id = $scope.costs.data[x].employee_id.replace(/&/g, '&#38').replace(/</g, '&#60').replace(/>/g, '&#62');
                                }
                                $scope.costGanttInstance.parse({ data: $scope.costs.data, links: [] });
                                $scope.costGanttInstance.render();
                            });

                        }
                    }
                }

                console.log('horizontal testing', angular.copy($scope.description));
                return true;
            });

            function applyExpandables() {
                //luan quest - apply jquery BEGIN
                if ($scope.trends.TrendNumber == 1000) {   //Manasi 10-11-2020
                    setTimeout(function () {
                        var listOfDivs = $('#cost-gantt').find('div.gantt_grid_data').children();
                        console.log(listOfDivs);
                        listOfDivs.each(function (index) {
                            var test = listOfDivs.eq(index).find('div.gantt_cell').eq(1);
                            var typeOfCostLine = test.find('div.gantt_tree_content')[0].innerHTML;
                            var taskID = listOfDivs.eq(index).attr('task_id');

                            //padding for cost type text
                            var costTypeDiv = listOfDivs.eq(index).find('div.gantt_cell').eq(2);
                            var costTypeTextDiv = costTypeDiv.find('div.gantt_tree_content')[0].innerText;
                            if (costTypeTextDiv == 'Labor')
                                costTypeDiv.css('padding-left', '28px');
                            //else if (costTypeTextDiv == 'Contractor')
                            else if (costTypeTextDiv == 'Subcontractor')    //Manasi 17-07-2020
                                costTypeDiv.css('padding-left', '28px');
                            else if (costTypeTextDiv == 'Material')
                                costTypeDiv.css('padding-left', '28px');
                            else if (costTypeTextDiv == 'ODC')
                                costTypeDiv.css('padding-left', '28px');

                            //Hide the last row with the trash can
                            //if (taskID == listOfDivs.length) {
                            //    $('div[task_id="' + taskID + '"]').hide();
                            //}

                            if (typeOfCostLine == 'B') {

                                var clickedExpandableSelector = $('#' + taskID + '_cost_line_id');
                                console.log(clickedExpandableSelector);
                                clickedExpandableSelector.click(function () {   //Apply clicks to all cost line item with the expandable icon
                                    var clickedTaskID = clickedExpandableSelector.attr('id');
                                    var listOfTaskAE = [];
                                    var listOfOnesBelow = [];
                                    var isHiding = null;

                                    clickedTaskID = clickedTaskID.split('_')[0];

                                    if (clickedExpandableSelector.hasClass('gantt_open')) {
                                        clickedExpandableSelector.addClass('gantt_close').removeClass('gantt_open'); //Make it close
                                        isHiding = false;
                                    } else {
                                        clickedExpandableSelector.addClass('gantt_open').removeClass('gantt_close'); //Make it open
                                        isHiding = true;
                                    }

                                    //We gotta find out all the task_id that has A-E that is in between and all the ones below after that
                                    var isDoneTaskInBetween = false;
                                    var moveMultiplier = 0;
                                    for (var x = index + 1; x < listOfDivs.length; x++) {
                                        var typeOfCost = listOfDivs.eq(x).find('div.gantt_cell').eq(1).find('div.gantt_tree_content')[0].innerHTML;

                                        if (typeOfCost == 'A-E' && !isDoneTaskInBetween) {
                                            listOfTaskAE.push(listOfDivs.eq(x));
                                            moveMultiplier += 1;
                                        } else {
                                            isDoneTaskInBetween = true;
                                            listOfOnesBelow.push(listOfDivs.eq(x));
                                        }
                                    }

                                    //Loop through listOfTaskAE to hide/display them
                                    for (var x = 0; x < listOfTaskAE.length; x++) {
                                        var taskID = listOfTaskAE[x].attr('task_id');

                                        if (!isHiding) {
                                            console.log('showing', 2);
                                            $('div[task_id="' + taskID + '"]').show();
                                        } else {
                                            console.log('hiding', 2);
                                            $('div[task_id="' + taskID + '"]').hide();
                                        }
                                    }

                                    //Loop through the ones below to move them up/down - applicable to gantt_task_line costbar
                                    for (var x = 0; x < listOfOnesBelow.length; x++) {
                                        var taskID = listOfOnesBelow[x].attr('task_id');
                                        var cssTopValue = parseInt($('div[task_id="' + taskID + '"].gantt_task_line').css('top'));

                                        if (!isHiding) {
                                            //Everything below has to move down 25px * multiplier
                                            cssTopValue += 25 * moveMultiplier;
                                            $('div[task_id="' + taskID + '"].gantt_task_line').css('top', cssTopValue);
                                        } else {
                                            //Everything below has to move up 25px * multiplier
                                            cssTopValue -= 25 * moveMultiplier;
                                            $('div[task_id="' + taskID + '"].gantt_task_line').css('top', cssTopValue);
                                        }
                                    }
                                });
                            }
                        })

                    }, 1);
                }
                //luan quest - apply jquery END
            }


            $scope.removeScheduleGanttHighlight = function () {
                var rows = $('#schedule-gantt .gantt_row');
                $.each(rows, function (index, value) {
                    if ($(this).hasClass('gantt_selected'))
                        $(this).removeClass('gantt_selected');
                });

                var taskRows = $('#schedule-gantt .gantt_task_row');
                $.each(taskRows, function (index, value) {
                    if ($(this).hasClass('gantt_selected'))
                        $(this).removeClass('gantt_selected');
                });
            }
            $scope.scheduleGanttInstance.attachEvent('onBeforetaskSelected', function (id, item) {

                $scope.removeScheduleGanttHighlight();
                console.log('function mark');
                var isEdited = false;
                var task = $scope.scheduleGanttInstance.getTask(id);
                if ($scope.isCostEdited || $scope.isNewCost) {
                    angular.forEach($scope.isCostEdited, function (item, key) {
                        if (item === true) {
                            isEdited = true;
                        }
                    });
                }

                //if (isEdited === true) {
                //
                //    if(task.text == "Add"){
                //        return false;
                //    }
                //    //s.css("background","gray");
                //    dhtmlx.confirm({
                //        title : "Important!",
                //        type: "confirm-error",
                //        text : "A cost is being modified. Data will be lost if proceeding. Do you want to proceed without saving?",
                //        width: "600px",
                //        ok : "Yes",
                //        cancel : "No",
                //        callback : function(result){
                //            if(result== true){
                //                $scope.isNewCost = [];
                //                $scope.isCostEdited = [];
                //                $scope.scheduleGanttInstance.selectTask(id);
                //            }
                //        }
                //    });
                //    return false;
                //}
                return true;
            });

            $scope.scheduleGanttInstance.attachEvent("onTaskSelected", function (id) {

                //Manasi 26-08-2020
                if ($scope.first_task_id != id) {
                    if ($scope.oldId == id || $scope.oldId == undefined || $scope.oldId == null) {
                        $scope.oldId = id;
                        $scope.cancelButton = true;
                    }
                    else {
                        //$scope.NewId = id;
                        $scope.cancelButton = false;
                    }
                }
                else {
                    $scope.cancelButton = false;
                }
                //Manasi 26-08-2020----------------------------------------------------
                console.log('function mark', id);
                var isCostEdited = isCostsEdited();
                // alert(isCostsEdited());
                console.log(isCostEdited);
                //if (false) {   //isCostEdited
                if (isCostEdited && !$scope.cancelButton) {  //Manasi 26-08-2020
                    dhtmlx.confirm("Unsaved data will be lost. Want to Continue?", function (result) {
                        //$scope.oldId = id;
                        if (result) {
                            var task = $scope.scheduleGanttInstance.getTask(id);
                            $scope.selectedActivity = task;
                            console.log(task);
                            if (!task) {
                                return;
                            }
                            if (task) {

                                var taskPosition = $scope.scheduleGanttInstance.getTaskPosition(task, task.original_start_date, task.original_end_date);
                                var secondTask = $scope.scheduleGanttInstance.getTaskPosition(task, task.start_date, task.end_date);
                                $scope.scheduleGanttInstance.scrollTo(secondTask.left - 150);
                            }
                            //if (id == projectMaxId) {
                            //
                            //    $scope.costGanttInstance.clearAll();
                            //    return;
                            //}

                            $('#total_label').show();
                            $('#total_cost').show();

                            if (task.PhaseID) {
                                var phaseID = Number(task.PhaseID) * 1000;
                                var isPhase = true;
                                // alert("PB1");
                                var s = $scope.scheduleGanttInstance.callEvent("customSelect", [id, phaseID, isPhase]);

                            } else {
                                var isPhase = false;
                                //  alert("PB2");
                                var s = $scope.scheduleGanttInstance.callEvent("customSelect", [id, 0, isPhase]);
                            }
                            $scope.cancelButton = false;   //Manasi 26-08-2020
                            $scope.oldId = id;
                        }
                        //Manasi 26-08-2020
                        else {
                            var id1 = $scope.oldId;
                            $scope.oldId = null;
                            //$scope.isCostEdited[$scope.selectedCost.id] = false;
                            $scope.cancelButton = true;
                            $scope.scheduleGanttInstance.selectTask(id1);

                        }
                        //Manasi 26-08-2020------------------------------------
                    });
                }
                else {
                    if (!isCostEdited) {  //Manasi 26-08-2020
                        var task = $scope.scheduleGanttInstance.getTask(id);
                        $scope.selectedActivity = task;
                        console.log(task);
                        if (!task) {
                            return;
                        }
                        if (task) {

                            var taskPosition = $scope.scheduleGanttInstance.getTaskPosition(task, task.original_start_date, task.original_end_date);
                            var secondTask = $scope.scheduleGanttInstance.getTaskPosition(task, task.start_date, task.end_date);
                            $scope.scheduleGanttInstance.scrollTo(secondTask.left - 150);
                        }
                        //if (id == projectMaxId) {
                        //
                        //    $scope.costGanttInstance.clearAll();
                        //    return;
                        //}

                        $('#total_label').show();
                        $('#total_cost').show();

                        if (task.PhaseID) {
                            var phaseID = Number(task.PhaseID) * 1000;
                            var isPhase = true;
                            // alert("PB1");
                            var s = $scope.scheduleGanttInstance.callEvent("customSelect", [id, phaseID, isPhase]);

                        } else {
                            var isPhase = false;
                            //  alert("PB2");
                            var s = $scope.scheduleGanttInstance.callEvent("customSelect", [id, 0, isPhase]);
                        }
                    }
                }
            });

            //Variable initilization for schedule Gantt
            $scope.temp = "";                //use to update and track if an activity description is empty
            //$scope.schedulePhase = "all";  //Initializae scale to 'all' on page load
            $scope.oneTime = false;         //use only when user first open up a lightbox
            $scope.retrievedActivityID = 0;  //store the selected activity
            $scope.saveFromLightbox = false; //a variable to keep track when the user click on the save button in lightbox

            // A custom Gantt event handller used to add new Activity
            $scope.scheduleGanttInstance.attachEvent("customAdd", function (id, item) {
                console.log('function mark');
                var tempActivity;
                console.log(item);
                var startDate = item.start_date.toISOString().substring(0, 10);
                var endDate = item.end_date.toISOString().substring(0, 10);

                if ($scope.scheduleScale === "week") {
                    if (moment(startDate).isoWeekday() === 7) {
                        startDate = moment(startDate).add(1, 'days').format(sqlDateFormat);
                        endDate = moment(endDate).add(1, 'days').format(sqlDateFormat);
                    }

                    var startOfWeek = (moment(startDate).isoWeekday() === 1) ? startDate : moment(startDate).startOf('isoWeek').format(sqlDateFormat);
                    var endOfWeek = (moment(endDate).isoWeekday() === 7) ? endDate : moment(endDate).endOf('isoWeek').format(sqlDateFormat);

                } else if ($scope.scheduleScale === "month") {

                    var startOfWeek = (moment(startDate).isoWeekday() === 1) ? startDate : moment(startDate).startOf('isoWeek').format(sqlDateFormat);
                    var endOfWeek = (moment(endDate).isoWeekday() === 7) ? endDate : moment(endDate).endOf('isoWeek').format(sqlDateFormat);
                } else if ($scope.scheduleScale === "year") {

                    var startOfWeek = (moment(startDate).isoWeekday() === 1) ? startDate : moment(startDate).startOf('isoWeek').format(sqlDateFormat);
                    var endOfWeek = (moment(endDate).isoWeekday() === 7) ? endDate : moment(endDate).endOf('isoWeek').format(sqlDateFormat);
                }

                var phaseCode = 0;

                //luan finally here - map order(parentid) with phaseid
                for (var x = 0; x < $scope.phases.length; x++) {
                    if ($scope.phases[x].Order == (parseInt(item.parent) / 1000)) {
                        phaseCode = $scope.phases[x].PhaseID;
                    }
                }

                //luan here 3/16
                item.percentage_completion = '0%';

                console.log(phaseCode);
                UpdateActivity.save({
                    "Operation": 1,
                    "ProjectID": delayedData[2].result[0].ProjectID,
                    "TrendNumber": delayedData[3],
                    "PhaseCode": phaseCode, //parseInt(item.parent) / 1000, //parseInt(item.parent) / 1000,
                    "BudgetCategory": item.mainCategory,
                    "BudgetSubCategory": item.subCategory,
                    "ActivityStartDate": startOfWeek,
                    "ActivityEndDate": endOfWeek,
                    "OriginalActivityStartDate": startOfWeek,
                    "OriginalActivityEndDate": endOfWeek,
                    "TrendID": $scope.trend.TrendID,
                    "PercentageCompletion": item.percentage_completion.substr(0, item.percentage_completion.length - 1)


                }).$promise.then(function (response) {
                    //TODO
                    $scope.retrievedActivityID = parseInt(response.result.split(",")[1]);

                    if (response.result.split(",")[0] === 'Success') {

                        //getProjectDurationAndCost();
                        var activityID = parseInt(response.result.split(",")[1]);
                        GetActivity.get({ ActivityID: activityID }, function (response) {

                            tempActivity = response.result;
                            console.log(tempActivity);
                            var activity = {};
                            activity["id"] = Number(tempActivity.ActivityID);
                            activity["update_id"] = Number(tempActivity.ActivityID);
                            //escape html tags
                            activity["text"] = tempActivity.BudgetCategory.replace(/&/g, '&#38').replace(/</g, '&#60').replace(/>/g, '&#62') + " - " + tempActivity.BudgetSubCategory.replace(/&/g, '&#38').replace(/</g, '&#60').replace(/>/g, '&#62');
                            var testDateFormat = "DD MMM YYYY";
                            activity["originalStartDate"] = moment(tempActivity.OriginalActivityStartDate).format(testDateFormat);
                            activity["originalEndDate"] = moment(tempActivity.OriginalActivityEndDate).format(testDateFormat);
                            activity["start_date"] = moment(tempActivity.ActivityStartDate).format(dateFormat);
                            activity["end_date"] = moment(tempActivity.ActivityEndDate).format(dateFormat);
                            activity["original_start_date"] = moment(tempActivity.ActivityStartDate).format(dateFormat);
                            activity["original_end_date"] = moment(tempActivity.ActivityEndDate).format(dateFormat);
                            activity["ActivityStartDate"] = tempActivity.ActivityStartDate;
                            activity["ActivityEndDate"] = tempActivity.ActivityEndDate;
                            activity["BudgetCategory"] = tempActivity.BudgetCategory;
                            activity["BudgetSubCategory"] = tempActivity.BudgetSubCategory;
                            activity["percentage_completion"] = tempActivity.PercentageCompletion + '%';
                            activity["parent"] = parseInt(item.parent); //Number(tempActivity.Order) * 1000; //Number(tempActivity.PhaseCode) * 1000;

                            activity["program_element"] = tempActivity.ProgramElementID;
                            activity["program"] = tempActivity.ProgramID;
                            activity["project"] = tempActivity.ProjectID;
                            activity["trend"] = tempActivity.TrendNumber;
                            activity["phase"] = tempActivity.PhaseCode;
                            activity["PhaseCode"] = tempActivity.PhaseCode;
                            activity["totalCost"] = "0";
                            activity["totalBudget"] = "0";
                            $scope.duc = true;

                            console.log(activity);

                            $scope.schedule.data.push(activity);

                            //luan quest 2/19
                            activities.push(activity);

                            console.log($scope.schedule);

                            updatePhaseAndProjectDisplay();

                            $scope.scheduleGanttInstance.parse({ data: $scope.schedule.data, links: [] });
                            // var task = $scope.scheduleGanttInstance.getTask(tempActivity.ActivityID);
                            //$scope.selectedActivity = task;
                            //updatePhase();
                            updateTrendDate();

                        });
                    } else {
                        console.log('Failed to add new Activity');
                    }

                })
                return true;
            });

            $scope.scheduleGanttInstance.attachEvent("onAfterTaskUpdate", function (id, item) {
                updatePhaseAndProjectDisplay();
                var count = 0;
                //$scope.selectedActivity = $scope.scheduleGanttInstance.getTask(id);
                if ($scope.isUpdateTaskFromLightbox === true && $scope.isScaleChanged === false) {
                    $scope.isTaskUpdate = true;
                    $scope.retrievedActivityID = id;
                    $scope.duc = true;
                    //$scope.scheduleGanttInstance.select_task(id);

                    var mainCategory = $scope.MainCategoryName;
                    var subCategory = $scope.SubCategoryName;
                    var selectedTask = $scope.scheduleGanttInstance.getTask(id);
                    //update task with appropriate end date and start date

                    console.log(selectedTask);

                    var start_date = selectedTask.start_date.toISOString().substring(0, 10);
                    var end_date = selectedTask.end_date.toISOString().substring(0, 10);

                    // end_date = moment(end_date).subtract(1, 'days');
                    var startOfWeek = (moment(start_date).isoWeekday() === 1) ? start_date : moment(start_date).startOf('isoWeek').format(sqlDateFormat);
                    var endOfWeek = (moment(end_date).isoWeekday() === 7) ? end_date : moment(end_date).endOf('isoWeek').format(sqlDateFormat);

                    //Original End Date
                    console.log(selectedTask);
                    var original_end_date = selectedTask.original_end_date.substring(6, 10);//yyyy
                    original_end_date += selectedTask.original_end_date.substring(2, 5);//-mm
                    original_end_date += "-" + selectedTask.original_end_date.substring(0, 2);//dd

                    //Original Start Date
                    var original_start_date = selectedTask.original_start_date.substring(6, 10);//yyyy
                    original_start_date += selectedTask.original_start_date.substring(2, 5);//-mm
                    original_start_date += "-" + selectedTask.original_start_date.substring(0, 2);//dd

                    var id = 5;//use to compare with methods value
                    var weekDifference = "";
                    var costArray = [];
                    var original_endDate;


                    var drag_direction = "";


                    if (original_end_date != endOfWeek || original_start_date != startOfWeek) {
                        //activateSpinner();
                        //if (moment(original_end_date) > moment(endOfWeek)) {                         //Drag right-left
                        if (moment(original_end_date) > moment(end_date)) {                         //Drag right-left
                            //weekDifference = moment(original_end_date).format('ww') - moment(endOfWeek).format('ww');
                            //weekDifference = getWeekDifferences(original_end_date, endOfWeek);
                            if ($scope.scheduleScale === "week")
                                weekDifference = getWeekDifferences(original_end_date, endOfWeek);
                            else if ($scope.scheduleScale === "month") {

                                var abStart = getAbsoulteMonths(endOfWeek);
                                var abEnd = getAbsoulteMonths(original_end_date);
                                weekDifference = abEnd - abStart;
                                //weekDifference = getMonthDifferences(original_end_date, endOfWeek);
                            } else if ($scope.scheduleScale === "year") {
                                weekDifference = Math.abs(moment(original_end_date).format('YYYY') - moment(endOfWeek).format('YYYY'));
                            }
                            angular.forEach($scope.costs.data, function (cost, id, objCost) {
                                drag_direction = "right-left";
                                $scope.direction = "right-left";
                                if (cost.text !== "") {
                                    for (i = 0; i < weekDifference; i++) {
                                        $scope.textBoxValues[cost.id][$scope.textBoxValues[cost.id].length - (i + 1)] = 0;
                                    }
                                    //for (var j = 0; j < $scope.methods.length; j++) {
                                    //    if ($scope.methods[j].name === cost.text) {
                                    //        id = j + 1;
                                    //    }
                                    //}
                                    id += 1;
                                    console.log(id);
                                    costCalculation(id, cost);  //id = index of item methods, cost = object cost
                                    var costTemp = $scope.updateCost(cost, drag_direction, id);
                                    if (costTemp.CostType == "F") {
                                        var tempHours = costTemp.FTEHours.split(',');
                                        var newFTEHours = [];
                                        angular.forEach(tempHours, function (item) {
                                            if (item != 0) {
                                                item = item / 8;
                                            }
                                            newFTEHours.push(item);
                                        });
                                        if (typeof cost.employee_id == 'object') {
                                            costTemp["EmployeeID"] = cost.employee_id;
                                        } else {
                                            angular.forEach($scope.employees, function (item) {
                                                if (item.name == cost.employee_id) {
                                                    costTemp["EmployeeID"] = item.value;
                                                }
                                            });

                                        }
                                        costTemp.FTEHours = newFTEHours.join(',');
                                    }
                                    else if (costTemp.CostType == "L" && $scope.scheduleScale == "week") {

                                    }
                                    costArray.push(costTemp);
                                }
                            });
                        }
                        //else if (moment(original_start_date) > moment(startOfWeek))
                        else if (moment(original_start_date) > moment(start_date))
                        //Drag left-left
                        {

                            drag_direction = "left-left";
                            $scope.direction = "left-left";
                            //var weeksInYear = (moment(original_start_date).format('YYYY') - moment(startOfWeek).format('YYYY')) * 52;
                            //weekDifference = weeksInYear + (moment(original_start_date).format('ww') - moment(startOfWeek).format('ww'));
                            if ($scope.scheduleScale === "week")
                                weekDifference = getWeekDifferences(original_start_date, startOfWeek);
                            else if ($scope.scheduleScale === "month") {
                                var abStart = getAbsoulteMonths(original_start_date);
                                var abEnd = getAbsoulteMonths(startOfWeek);
                                weekDifference = Math.abs(abEnd - abStart);
                            } else if ($scope.scheduleScale === "year") {
                                weekDifference = Math.abs(moment(original_start_date).format("YYYY") - moment(startOfWeek).format("YYYY"));
                            }
                            angular.forEach($scope.costs.data, function (cost, id, objCost) {
                                console.log(cost);
                                if (cost.text !== "") {
                                    //1. update Textboxvalues
                                    for (i = 0; i < weekDifference; i++) {
                                        $scope.textBoxValues[cost.id].splice(0, 0, 0);
                                    }
                                    //2. update wokring days


                                    //3. update cost
                                    //for (var j = 0; j < $scope.methods.length; j++) {
                                    //    if ($scope.methods[j].name === cost.text) {
                                    //        id = j + 1;
                                    //    }
                                    //}
                                    id += 1;
                                    costCalculation(id, cost);//calculate cost type, description, #ofunits, unittype, unitcost, cost
                                    var costTemp = $scope.updateCost(cost, drag_direction, id);
                                    if (costTemp.CostType == "F") {
                                        var tempHours = costTemp.FTEHours.split(',');
                                        var newFTEHours = [];
                                        angular.forEach(tempHours, function (item) {
                                            if (item != 0) {
                                                item = item / 8;
                                            }
                                            newFTEHours.push(item);
                                        });

                                        if (typeof cost.employee_id == 'object') {
                                            costTemp["EmployeeID"] = cost.employee_id;
                                        } else {
                                            angular.forEach($scope.employees, function (item) {
                                                if (item.name == cost.employee_id) {
                                                    costTemp["EmployeeID"] = item.value;
                                                }
                                            });

                                        }
                                        costTemp.FTEHours = newFTEHours.join(",");
                                    }
                                    costArray.push(costTemp);
                                }
                            });
                        }
                        //else if (moment(original_start_date) <= moment(startOfWeek)) {                 //Drag left-right
                        else if (moment(original_start_date) < moment(start_date)) {                 //Drag left-right
                            drag_direction = "left-right";
                            $scope.direction = "left-right";
                            if ($scope.scheduleScale === "week")
                                weekDifference = getWeekDifferences(original_start_date, startOfWeek);
                            else if ($scope.scheduleScale === "month") {
                                var abStart = getAbsoulteMonths(original_start_date);
                                var abEnd = getAbsoulteMonths(startOfWeek);
                                weekDifference = Math.abs(abEnd - abStart);
                            }
                            else if ($scope.scheduleScale === "year") {
                                weekDifference = Math.abs(moment(original_start_date).format("YYYY") - moment(startOfWeek).format("YYYY"));
                            }

                            angular.forEach($scope.costs.data, function (cost, id) {
                                if (cost.text !== "") {
                                    for (i = 0; i < weekDifference; i++) {
                                        $scope.textBoxValues[cost.id][i] = 0;
                                    }
                                    //for (var j = 0; j < $scope.methods.length; j++) {
                                    //    if ($scope.methods[j].name === cost.text) {
                                    //        id = j + 1 ;
                                    //    }
                                    //}
                                    id += 1;
                                    costCalculation(id, cost);
                                    var costTemp = $scope.updateCost(cost, drag_direction, id);
                                    //costCalculation(id, costTemp);
                                    if (costTemp.CostType == "F") {
                                        var tempHours = costTemp.FTEHours.split(',');
                                        var newFTEHours = [];
                                        angular.forEach(tempHours, function (item) {
                                            if (item != 0) {
                                                item = (parseFloat(item) / 8).toString();
                                            }
                                            newFTEHours.push(item);
                                        });
                                        if (typeof cost.employee_id == 'object') {
                                            costTemp["EmployeeID"] = cost.employee_id;
                                        } else {
                                            angular.forEach($scope.employees, function (item) {
                                                if (item.name == cost.employee_id) {
                                                    costTemp["EmployeeID"] = item.value;
                                                }
                                            });

                                        }
                                        costTemp.FTEHours = newFTEHours.join(",");
                                    }
                                    costArray.push(costTemp);
                                }
                            });
                        }
                        else if (moment(original_end_date) < moment(end_date)) {
                            //drag right-right
                            drag_direction = "right-right";
                            $scope.direction = "right-right";
                            if ($scope.scheduleScale === "week")
                                weekDifference = getWeekDifferences(original_end_date, endOfWeek);
                            else if ($scope.scheduleScale === "month") {
                                //weekDifference = getMonthDifferences(original_end_date, endOfWeek);
                                var abStart = getAbsoulteMonths(original_end_date);
                                var abEnd = getAbsoulteMonths(endOfWeek);
                                weekDifference = (abEnd - abStart);
                            } else if ($scope.scheduleScale === "year") {
                                weekDifference = Math.abs(moment(original_end_date).format("YYYY") - moment(endOfWeek).format("YYYY"));
                            }
                            angular.forEach($scope.costs.data, function (cost, id, objCost) {
                                if (cost.text !== "") {
                                    //1. update Textboxvalues
                                    for (i = 0; i < weekDifference; i++) {
                                        $scope.textBoxValues[cost.id].push(0);
                                    }
                                    // 2. update wokring days

                                    ////3. update cost
                                    //for (var j = 0; j < $scope.methods.length; j++) {
                                    //    if ($scope.methods[j].name === cost.text) {
                                    //        id = j  + 1;
                                    //    }
                                    //}
                                    id += 1;
                                    costCalculation(id, cost);//calculate cost type, description, #ofunits, unittype, unitcost, cost
                                    var costTemp = $scope.updateCost(cost, drag_direction, id);
                                    if (costTemp.CostType == "F") {
                                        var tempHours = costTemp.FTEHours.split(',');
                                        var newFTEHours = [];
                                        angular.forEach(tempHours, function (item) {
                                            if (item != 0) {
                                                item = item / 8;
                                            }
                                            newFTEHours.push(item);
                                        });
                                        if (typeof cost.employee_id == 'object') {
                                            costTemp["EmployeeID"] = cost.employee_id;
                                        } else {
                                            angular.forEach($scope.employees, function (item) {
                                                if (item.name == cost.employee_id) {
                                                    costTemp["EmployeeID"] = item.value;
                                                }
                                            });

                                        }

                                        costTemp.FTEHours = newFTEHours.join(",");
                                    }
                                    costArray.push(costTemp);
                                }
                            });
                        }

                        if (costArray.length != 0) {
                            //if($scope.scheduleScale != "week")
                            // endOfWeek = moment(endOfWeek).add(-1,'days').format(sqlDateFormat);
                            var obj = {
                                /*
                                    "Operation": 1,
                                    "ProjectID": delayedData[2].result[0].ProjectID,
                                    "TrendNumber": delayedData[3],
                                    "PhaseCode": parseInt(item.parent) / 1000,
                                    "BudgetCategory": item.mainCategory,
                                    "BudgetSubCategory": item.subCategory,
                                    "ActivityStartDate": startOfWeek,
                                    "ActivityEndDate": endOfWeek,
                                    "OriginalActivityStartDate": originalStartOfWeek,
                                    "OriginalActivityEndDate": originalEndOfWeek,
                                    "TrendID": $scope.trend.TrendID,
                                    "PercentageCompletion": item.percentage_completion.substr(0, item.percentage_completion.length - 1)
                                */
                                "Operation": 4,
                                "ActivityEndDate": moment(endOfWeek).format(sqlDateFormat),
                                "ActivityID": selectedTask.id,
                                "BudgetCategory": mainCategory,
                                "BudgetSubCategory": subCategory,
                                "ActivityStartDate": moment(startOfWeek).format(sqlDateFormat),
                                "TrendNumber": $scope.trend.TrendNumber,
                                "ProjectID": $scope.trend.ProjectID,
                                "PhaseCode": $scope.selectedActivity.phase,
                                "TrendID": $scope.trend.TrendID,
                                "OriginalActivityStartDate": moment(startOfWeek).format(sqlDateFormat),
                                "OriginalActivityEndDate": moment(endOfWeek).format(sqlDateFormat),
                                "PercentageCompletion": item.percentage_completion.substr(0, item.percentage_completion.length - 1)
                            };
                            console.log('before update', obj);
                            UpdateActivity.save(obj, function (response) {
                                //luan here - trying to show new original start/end dates
                                var testDateFormat = "DD MMM YYYY";
                                $scope.selectedActivity.originalStartDate = moment(startOfWeek, sqlDateFormat).format(testDateFormat);
                                $scope.selectedActivity.originalEndDate = moment(endOfWeek, sqlDateFormat).format(testDateFormat);

                                //updatePhase();
                                updateTrendDate();
                                item.text = mainCategory.replace(/&/g, '&#38').replace(/</g, '&#60').replace(/>/g, '&#62') + " - " + subCategory.replace(/&/g, '&#38').replace(/</g, '&#60').replace(/>/g, '&#62'); //escape html tags
                                item.BudgetCategory = mainCategory;
                                item.BudgetSubCategory = subCategory;
                                $scope.selectedActivity.original_end_date = moment(endOfWeek).format(dateFormat);//original_endDate.substring(8, 10) + original_endDate.substring(4, 7) + "-" + original_endDate.substring(0, 4);
                                if (drag_direction == "right-left" || drag_direction == "right-right") {
                                    $scope.selectedActivity.end_date = moment(endOfWeek).format(dateFormat);
                                    $scope.selectedActivity.start_date = moment($scope.selectedActivity.start_date).format(dateFormat);
                                } else {
                                    $scope.selectedActivity.start_date = moment(startOfWeek).format(dateFormat);
                                    $scope.selectedActivity.end_date = moment($scope.selectedActivity.end_date).format(dateFormat);
                                }
                                $scope.selectedActivity.original_start_date = moment(startOfWeek).format(dateFormat);//startOfWeek;original_startDate.substring(8, 10) + original_startDate.substring(4, 7) + "-" + original_startDate.substring(0, 4);
                                // $scope.scheduleGanttInstance.refreshData();
                                //updatePhaseAndProjectDisplay();
                                InsertCost.save(costArray, function (response) {
                                    $scope.buffer = [];
                                    console.log('special test 2/12', selectedTask);

                                    //Manasi 01-09-2020
                                    GetActivity.get({ ActivityID: $scope.selectedActivity.id }, function (response) {
                                        console.log(response);
                                        var activityResponse = response.result;
                                        console.log(activityResponse);
                                        console.log($scope.schedule.data);
                                        var updatedActivityTotal = 0;
                                        var updatedActivityTotalActualForecast = 0;
                                        var updatedActivityBudget = 0;
                                        for (var x = 0; x < $scope.schedule.data.length; x++) { //loop through schedule.data to update the one activity
                                            if ($scope.schedule.data[x].id == $scope.selectedActivity.id) {
                                                updatedActivityTotal = calculateActivityCostTotal(activityResponse).toString();
                                                updatedActivityTotalActualForecast = calculateActivityForecastTotal(activityResponse).toString();
                                                updateActivityTotalBudget = calculateActivityBudgetTotal(activityResponse).toString();
                                                // alert(updateActivityTotalBudget);
                                                $scope.schedule.data[x].totalCost = updatedActivityTotal;
                                                $scope.schedule.data[x].totalCostActualForecast = updatedActivityTotalActualForecast;
                                                $scope.schedule.data[x].totalBudget = updateActivityTotalBudget;
                                                console.log(activityResponse, $scope.schedule.data[x]);
                                            }
                                        }

                                        var trendTotalValue = 0;
                                        var trendTotalValueActual = 0;
                                        var trendTotalBudget = 0;
                                        console.log(activities);

                                        for (var x = 0; x < activities.length; x++) { //loop through activities to update the activities list.
                                            if (activities[x].id == activityResponse.ActivityID) {
                                                activities[x].totalCost = updatedActivityTotal;
                                                activities[x].totalBudget = updateActivityTotalBudget;
                                                activities[x].totalCostActualForecast = updatedActivityTotalActualForecast;
                                            }
                                        }

                                        for (var x = 0; x < $scope.schedule.data.length; x++) { //loop through schedule.data to update all the phases
                                            if ($scope.schedule.data[x].PhaseID) {
                                                var phaseTotalCost = CalculatePhaseTotal(activities, $scope.schedule.data[x].PhaseID);
                                                $scope.schedule.data[x].totalCost = phaseTotalCost;
                                                console.log($scope.schedule.data[x].totalCost);
                                                trendTotalValue += phaseTotalCost;

                                                var phaseTotalCostActualForecast = CalculatePhaseTotalActualForecast(activities, $scope.schedule.data[x].PhaseID);
                                                $scope.schedule.data[x].totalCostActualForecast = phaseTotalCostActualForecast;
                                                console.log($scope.schedule.data[x].totalCostActualForecast);
                                                trendTotalValueActual += phaseTotalCostActualForecast;

                                                var phaseBudgetCost = CalculatePhaseTotalBudget(activities, $scope.schedule.data[x].PhaseID);
                                                $scope.schedule.data[x].totalBudget = phaseBudgetCost;
                                                trendTotalBudget += parseFloat(phaseBudgetCost); // PRitesh for Proejct Budget format on 22jul2020

                                                console.log(trendTotalValue, trendTotalValueActual);
                                                console.log(trendTotalValue, phaseTotalCostActualForecast);
                                                console.log(trendTotalBudget, phaseBudgetCost);
                                            }
                                        }
                                        //  alert(trendTotalBudget);
                                        for (var x = 0; x < $scope.schedule.data.length; x++) { //loop through schedule.data to update the project
                                            if ($scope.schedule.data[x].parent == 0) {
                                                $scope.schedule.data[x].totalCost = trendTotalValue;
                                                $scope.schedule.data[x].totalCostActualForecast = trendTotalValueActual;
                                                $scope.schedule.data[x].totalBudget = trendTotalBudget;
                                                console.log($scope.schedule.data[x]);
                                            }
                                        }

                                        $timeout(function () {
                                            currentId = -1;
                                            // alert("Check");
                                            $scope.scheduleGanttInstance.callEvent('customSelect', [$scope.selectedActivity.id, 0, false]);
                                            $scope.scheduleGanttInstance.selectTask($scope.selectedActivity.id);
                                            $scope.showSpinner12 = false;
                                        }, 1000);

                                    });
                                    //Manasi 01-09-2020

                                    $scope.scheduleGanttInstance.callEvent('onTaskSelected', [selectedTask.id]);

                                    $timeout(function () {
                                        //getProjectDurationAndCost();
                                        updateTrendValue($scope.selectedActivity.id);
                                        calculateTrendValue();
                                        //$scope.allCostTotal = $scope.selectedActivity.totalCost;
                                        $scope.allCostTotal = $scope.selectedActivity.totalBudget;   //Manasi 15-07-2020
                                        var amountInput = delayedData[2].result[0].Amount;
                                        $scope.amount = amountInput - $scope.allCostTotal;

                                        //deactivateSpinner();
                                        $scope.direction = '';
                                    }, 1000);


                                });
                            });

                        } else {
                            var obj = {
                                "Operation": 4,
                                "ActivityEndDate": endOfWeek,
                                "ActivityID": $scope.selectedActivity.id,
                                "BudgetCategory": mainCategory,
                                "BudgetSubCategory": subCategory,
                                "ActivityStartDate": startOfWeek,
                                "TrendNumber": $scope.trend.TrendNumber,
                                "ProjectID": $scope.trend.ProjectID,
                                "PhaseCode": $scope.selectedActivity.phase,
                                "TrendID": $scope.trend.TrendID,
                                "OriginalActivityStartDate": startOfWeek,
                                "OriginalActivityEndDate": endOfWeek,
                                "PercentageCompletion": item.percentage_completion.substr(0, item.percentage_completion.length - 1)
                            };
                            console.log('before update');
                            UpdateActivity.save(obj, function (response) {
                                //updatePhase();
                                console.log('test');
                                updateTrendDate();
                                item.text = mainCategory.replace(/&/g, '&#38').replace(/</g, '&#60').replace(/>/g, '&#62') + " - " + subCategory.replace(/&/g, '&#38').replace(/</g, '&#60').replace(/>/g, '&#62'); //escape html tags
                                item.BudgetCategory = mainCategory;
                                item.BudgetSubCategory = subCategory;
                                original_endDate = $scope.selectedActivity.end_date.toISOString().substring(0, 10);
                                $scope.selectedActivity.original_end_date = moment(endOfWeek).format(dateFormat);// original_endDate.substring(8, 10) + original_endDate.substring(4, 7) + "-" + original_endDate.substring(0, 4);
                                $scope.selectedActivity.end_date = moment(endOfWeek).format(dateFormat);
                                $scope.selectedActivity.original_start_date = moment(startOfWeek).format(dateFormat);
                                $scope.selectedActivity.start_date = moment(startOfWeek).format(dateFormat);

                                //rightnow
                                var testDateFormat = "DD MMM YYYY";
                                $scope.selectedActivity.originalStartDate = moment(startOfWeek, sqlDateFormat).format(testDateFormat);
                                $scope.selectedActivity.originalEndDate = moment(endOfWeek, sqlDateFormat).format(testDateFormat);
                                //updatePhaseAndProjectDisplay();

                                $scope.buffer = [];
                                console.log('special test 2/12', selectedTask);
                                $scope.scheduleGanttInstance.callEvent('onTaskSelected', [selectedTask.id]);
                                //deactivateSpinner();
                            });
                        }
                    }


                    else {//Update Main Category and Sub Category--start date and end date remained unchanged
                        console.log('before update', item);
                        UpdateActivity.save({
                            "Operation": 2,
                            "ActivityID": item.update_id,
                            "ProjectID": delayedData[2].result[0].ProjectID,
                            "TrendNumber": delayedData[3],
                            "TrendID": $scope.trend.TrendID,
                            "PhaseCode": parseInt(item.phase),
                            "BudgetCategory": mainCategory,
                            "BudgetSubCategory": subCategory,
                            "ActivityStartDate": startOfWeek,//item.start_date.toISOString().substring(0, 10),
                            "ActivityEndDate": endOfWeek,//item.end_date.toISOString().substring(0, 10)
                            "OriginalActivityStartDate": startOfWeek,
                            "OriginalActivityEndDate": endOfWeek,
                            "PercentageCompletion": item.percentage_completion.substr(0, item.percentage_completion.length - 1),
                            //"OdcCost": item.totalCost,  //unofficial
                        }).$promise.then(function (response) {
                            //updatePhase();
                            item.text = mainCategory.replace(/&/g, '&#38').replace(/</g, '&#60').replace(/>/g, '&#62') + " - " + subCategory.replace(/&/g, '&#38').replace(/</g, '&#60').replace(/>/g, '&#62'); //escape html tags
                            item.BudgetCategory = mainCategory;
                            item.BudgetSubCategory = subCategory;
                            console.log('special test 2/12', selectedTask);
                            $scope.selectedActivity.original_end_date = moment(endOfWeek).format(dateFormat);// original_endDate.substring(8, 10) + original_endDate.substring(4, 7) + "-" + original_endDate.substring(0, 4);
                            $scope.selectedActivity.end_date = moment(endOfWeek).format(dateFormat);
                            $scope.selectedActivity.original_start_date = moment(startOfWeek).format(dateFormat);
                            $scope.selectedActivity.start_date = moment(startOfWeek).format(dateFormat);

                            //rightnow
                            var testDateFormat = "DD MMM YYYY";
                            $scope.selectedActivity.originalStartDate = moment(startOfWeek, sqlDateFormat).format(testDateFormat);
                            $scope.selectedActivity.originalEndDate = moment(endOfWeek, sqlDateFormat).format(testDateFormat);
                            $scope.scheduleGanttInstance.callEvent('onTaskSelected', [selectedTask.id]);
                            //updatePhaseAndProjectDisplay();
                            //deactivateSpinner();
                        })
                    }
                }
                if ($scope.isScaleChanged == true) {
                    $scope.isScaleChanged = false;
                }
                $scope.isUpdateTaskFromLightbox = false;
                $scope.isTaskUpdate = false;
                // $scope.scheduleGanttInstance.selectTask($scope.selectedActivity.id);
                //Update phase duration for baseline or trend
                if ($scope.saveFromLightbox == true) {
                    //updatePhase();

                }
            });

            // Get the estimate for the current project.
            $scope.createEstimate = function () {
                var projectID = delayedData[2].result[0].ProjectID;
                var trendNumber = $scope.trend.TrendNumber;
                var path = serviceBasePath + 'Request/Estimate/' + projectID + '/' + trendNumber;
                $http.get(path).then(function success(response) {
                    console.log(response);
                    var blob = new Blob([s2ab(atob(response.data))], {
                        type: ''
                    });
                    var url = window.URL.createObjectURL(blob);
                    var a = document.createElement("a");
                    document.body.appendChild(a);
                    a.style = "display: none";
                    href = URL.createObjectURL(blob);
                    a.href = href;
                    a.download = 'Estimate.xlsx';
                    a.click();
                    window.URL.revokeObjectURL(url);
                }, function error(response) {
                    console.log(response);
                    dhtmlx.alert("An estimate couldn't be created for this project.");
                });


            };

            $scope.createActual = function () {
                var projectID = delayedData[2].result[0].ProjectID;
                //TODO use $scope.trend.TrendID in path, currently just using the current trend
                var path = serviceBasePath + 'Request/Invoice/' + projectID;
                $http.get(path).then(function success(response) {
                    var blob = new Blob([s2ab(atob(response.data))], {
                        type: ''
                    });
                    var url = window.URL.createObjectURL(blob);
                    var a = document.createElement("a");
                    document.body.appendChild(a);
                    a.style = "display: none";
                    href = URL.createObjectURL(blob);
                    a.href = href;
                    a.download = 'Actual.xlsx';
                    a.click();
                    window.URL.revokeObjectURL(url);
                }, function error(response) {
                    console.log(response);
                    dhtmlx.alert("An actual couldn't be created for this project.");
                });
            };

            $scope.scheduleGanttInstance.attachEvent('onAfterTaskDelete', function (id, item) {
                console.log('function mark');
                if ($scope.isDeleteFromLightbox == true) {
                    $('#total_label').hide();
                    $('#total_cost').hide();

                    $scope.isCostEdited = [];
                    $scope.isNewCost = [];
                    $scope.duc = false;
                    console.log(item);
                    var phaseTask = $scope.scheduleGanttInstance.getTask(Number(item.phase) * 1000);
                    var phaseTotalCost = 0;

                    $scope.scheduleGanttInstance.eachTask(function (task) {
                        if (task.phase === item.phase) {
                            //   var c = parseInt(task.totalCost.substring(1, task.totalCost.length));
                            phaseTotalCost += parseFloat(task.totalCost);
                        }
                    })
                    if (phaseTask) {
                        phaseTask["totalCost"] = roundToTwo(phaseTotalCost).toString();//Math.round(phaseTotalCost).toString(); //error here
                    }
                    $scope.allCostTotal = "$0";
                    var amountInput = delayedData[2].result[0].Amount;
                    $scope.amount = amountInput - $scope.allCostTotal;

                    paddingLabel();
                    /*      if (1000 === phaseTask.id) {
                              $scope.planning_value = $filter('currency')( phaseTotalCost,'$',formatCurrency(phaseTotalCost));
                          } else if (2000 === phaseTask.id) {
                              $scope.schematic_design_value = $filter('currency')(phaseTotalCost,'$',formatCurrency(phaseTotalCost));
                          }
                          else if (3000 === phaseTask.id) {
                              $scope.design_bidding_value = $filter('currency')(phaseTotalCost,'$',formatCurrency(phaseTotalCost));
                          }
                          else if (4000 === phaseTask.id) {
                              $scope.construction_value =  $filter('currency')(phaseTotalCost,'$',formatCurrency(phaseTotalCost));
                          }
                          else if (5000 === phaseTask.id) {
                              $scope.closeout_value =  $filter('currency')(phaseTotalCost,'$',formatCurrency(phaseTotalCost));
      
                          }*/
                    calculateTrendValue();
                    $scope.isDeleteFromLightbox = false;
                    $scope.costGanttInstance.clearAll();
                }
            });

            $scope.scheduleGanttInstance.attachEvent("onBeforeTaskDelete", function (id, item) {
                // $scope.isDeleteFromLightbox = true;
                var task = $scope.scheduleGanttInstance.getTask(id);
                if (task) {
                    if (task.text == "Add")
                        return;
                }
                $scope.subCategory = [];
                $scope.scheduleGanttInstance.updateCollection('sub', $scope.subCategory);

                $scope.MainCategory = [];
                $scope.scheduleGanttInstance.updateCollection('main', $scope.MainCategory);
                $scope.firstTime = true;
                var index = -1;
                for (var i = 0; i < activities.length; i++) {
                    if (parseInt(activities[i].ActivityID) === item.id) {
                        index = i;
                    }
                }
                console.log('before update', item);
                UpdateActivity.save({
                    "Operation": 3,
                    "ActivityID": item.id,
                    "ProjectID": delayedData[2].result[0].ProjectID,
                    "TrendID": $scope.trend.TrendID,
                    "TrendNumber": delayedData[3],
                    "PhaseCode": parseInt(item.phase),
                    "BudgetCategory": '',
                    "BudgetSubCategory": item.text,
                    "ActivityStartDate": item.start_date.toISOString().substring(0, 10),
                    "ActivityEndDate": item.end_date.toISOString().substring(0, 10)


                }).$promise.then(function (response) {
                    if (response.result === 'Success') {

                        //updatePhase();
                        updateTrendDate();

                        console.log('delete applied');
                        activities.splice(index, 1);
                        for (var i = 0; i < $scope.schedule.data.length; i++) {
                            if ($scope.schedule.data[i].id == item.id) {
                                $scope.schedule.data.splice(i, 1);
                                $scope.costGanttInstance.clearAll();
                            }
                        }
                        //updatePhaseAndProjectDisplay();
                    } else {
                        console.log(' Failed to delete  Activity');
                    }
                })

            });

            $scope.scheduleGanttInstance.attachEvent('onAfterLightbox', function () {
            });

            $scope.scheduleGanttInstance.attachEvent('onAfterTaskAdd', function (id, item) {
                updateTrendDate();
                $scope.subCategory = [];

                //escape html tags
                for (var x = 0; x < $scope.subCategory.length; x++) {
                    $scope.subCategory[x].key = $scope.subCategory[x].key.replace(/&/g, '&#38').replace(/</g, '&#60').replace(/>/g, '&#62');
                    $scope.subCategory[x].label = $scope.subCategory[x].label.replace(/&/g, '&#38').replace(/</g, '&#60').replace(/>/g, '&#62');
                }

                $scope.scheduleGanttInstance.updateCollection('sub', $scope.subCategory);
            });

            $timeout(function () {
                if ($scope.screenLoad && $http.pendingRequests.length == 0) {
                    var first = false;
                    $scope.scheduleGanttInstance.eachTask(function (task) {
                        if (!task.update_id && !first) {
                            $scope.scheduleGanttInstance.selectTask($scope.first_task_id);
                            first = true;

                        }
                    });
                    $scope.screenLoad = false;
                }
            }, 2000);

            $scope.scheduleGanttInstance.attachEvent('onGanttRender', function () {
                $('div.gantt_grid_head_cell.gantt_grid_head_add').css('opacity', '0');   //Manasi 10-08-2020
                $('div.gantt_grid_head_cell.gantt_grid_head_add').css('pointer-events', 'none');   //Manasi 13-08-2020
                //$scope.scheduleGanttInstance.selectTask($scope.oldId);

            });

            $scope.costGanttInstance.attachEvent('onDataRender', function () {
                setTimeout(function () {
                    $('div.gantt_grid_head_cell.gantt_grid_head_add').css('opacity', '0');
                    $('div.gantt_grid_head_cell.gantt_grid_head_add').css('pointer-events', 'none');
                    $('div.gantt_layout_cell.gantt_resizer.gantt_resizer_x.gantt_layout_cell_border_right').remove();
                    $('div.gantt_layout_cell.grid_cell.gantt_layout_outer_scroll.gantt_layout_outer_scroll_vertical.gantt_layout_outer_scroll.gantt_layout_outer_scroll_horizontal.gantt_layout_cell_border_right').css('border-left', '1px solid #cecece');

                    var listOfHeaders = $(".gantt_grid_scale").children('div.gantt_grid_head_cell');

                    listOfHeaders.each(function (index) {
                        if (!$(this).hasClass("gantt_grid_head_add")) {
                            var oldStyle = $(this).attr("style");
                            $(this).attr("style", oldStyle + "border-right: 1px solid #cecece !important");
                        }
                    });
                }, 150);

            });

            //The Task Drag is not supported at the moment
            $scope.scheduleGanttInstance.attachEvent("onAfterTaskDrag", function (id, mode, e) {
                //When drag right - update new dates ; drag-lef t : remove dates
                var item = $scope.scheduleGanttInstance.getTask(id);
                var main = $scope.scheduleGanttInstance.getLightboxSection("mainphase").getValue();
                var sub = $scope.scheduleGanttInstance.getLightboxSection("subphase").getValue();
                var categoryList = item.text.split('-');
                var mainCategory = (main) ? main : categoryList[0];
                var subCategory = (sub) ? sub : categoryList[1];


                var selectedTask = $scope.scheduleGanttInstance.getTask(id);
                var start_date = $scope.selectedActivity.start_date.toISOString().substring(0, 10);
                var end_date = $scope.selectedActivity.end_date.toISOString().substring(0, 10);
                //Original End Date
                var original_end_date = $scope.selectedActivity.original_end_date.substring(6, 10);//yyyy
                original_end_date += $scope.selectedActivity.original_end_date.substring(2, 5);//-mm
                original_end_date += "-" + $scope.selectedActivity.original_end_date.substring(0, 2);//dd
                //Original Start Date
                var original_start_date = $scope.selectedActivity.original_start_date.substring(6, 10);//yyyy
                original_start_date += $scope.selectedActivity.original_start_date.substring(2, 5);//-mm
                original_start_date += "-" + $scope.selectedActivity.original_start_date.substring(0, 2);//dd
                var id = 5;//use to compare with methods value
                var weekDifference = "";
                var costArray = [];
                var original_endDate;
                var original_startDate;
                var url = serviceBasePath + 'response/cost';
                var drag_direction = "";
                if (original_end_date > end_date) {                         //Drag right-left
                    //weekDifference = moment(original_end_date).format('ww') - moment(end_date).format('ww');
                    weekDifference = getWeekDifferences(original_end_date, end_date);

                    angular.forEach($scope.costs.data, function (cost, id, objCost) {
                        drag_direction = "right-left";
                        if (cost.text !== "") {
                            for (i = 0; i < weekDifference; i++) {
                                $scope.textBoxValues[cost.id][$scope.textBoxValues[cost.id].length - (i + 1)] = 0;
                            }
                            for (var j = 0; j < $scope.methods; j++) {
                                if ($scope.methods[j] === cost.text) {
                                    id = j;
                                }
                            }
                            costCalculation(id, cost);  //id = index of item methods, cost = object cost
                            var costTemp = $scope.updateCost(cost, drag_direction, id);
                            costArray.push(costTemp);
                        }
                    });
                } else if (original_start_date > start_date)                 //Drag left-left
                {
                    drag_direction = "left-left";
                    weekDifference = moment(original_start_date).format('ww') - moment(start_date).format('ww');

                    angular.forEach($scope.costs.data, function (cost, id, objCost) {
                        if (cost.text !== "") {
                            for (i = 0; i < weekDifference; i++) {
                                $scope.textBoxValues[cost.id].splice(0, 0, 0);
                            }
                            for (var j = 0; j < $scope.methods; j++) {
                                if ($scope.methods[j] === cost.text) {
                                    id = j;
                                }
                            }
                            costCalculation(id, cost);

                            var costTemp = $scope.updateCost(cost, drag_direction, id);
                            costArray.push(costTemp);
                        }
                    });
                }
                else if (original_start_date < start_date) {                 //Drag left-right
                    drag_direction = "left-right";
                    weekDifference = moment(start_date).format('ww') - moment(original_start_date).format('ww');
                    angular.forEach($scope.costs.data, function (cost, id) {
                        if (cost.text !== "") {
                            for (i = 0; i < weekDifference; i++) {
                                $scope.textBoxValues[cost.id][i] = 0;
                            }
                            for (var j = 0; j < $scope.methods; j++) {
                                if ($scope.methods[j] === cost.text) {
                                    id = j;
                                }
                            }
                            costCalculation(id, cost);
                            var costTemp = $scope.updateCost(cost, drag_direction, id);
                            costArray.push(costTemp);
                        }
                    });
                }
                if (costArray.length != 0) {

                    var j = 0;
                    var temp;
                    for (i = 0; i < costArray.length; i++) {
                        $http.post(url, costArray[i]).then(function (response) {
                            if ((j + 1) == costArray.length) {
                                var obj = {
                                    "Operation": 4,
                                    "ActivityEndDate": end_date,
                                    "ActivityID": $scope.selectedActivity.id,
                                    "BudgetCategory": mainCategory,
                                    "BudgetSubCategory": subCategory,
                                    "ActivityStartDate": start_date
                                };
                                console.log('before update');
                                UpdateActivity.save(obj, function (response) {
                                    $scope.reloadCost(selectedTask.id);
                                    //updatePhase();
                                    updateTrendDate();
                                    //updatePhaseAndProjectDisplay();
                                    original_endDate = $scope.selectedActivity.end_date.toISOString().substring(0, 10);
                                    $scope.selectedActivity.original_end_date = original_endDate.substring(8, 10) + original_endDate.substring(4, 7) + "-" + original_endDate.substring(0, 4);
                                    original_startDate = $scope.selectedActivity.start_date.toISOString().substring(0, 10);
                                    $scope.selectedActivity.original_start_date = original_startDate.substring(8, 10) + original_startDate.substring(4, 7) + "-" + original_startDate.substring(0, 4);
                                });
                            }
                            j++;
                        })
                    }

                } else {   //Drag right-right
                    var obj = {
                        "Operation": 4,
                        "ActivityEndDate": end_date,
                        "ActivityID": $scope.selectedActivity.id,
                        "BudgetCategory": mainCategory,
                        "BudgetSubCategory": subCategory,
                        "ActivityStartDate": start_date
                    };
                    console.log('before update');
                    UpdateActivity.save(obj, function (response) {
                        //updatePhase();
                        updateTrendDate();
                        //updatePhaseAndProjectDisplay();
                        original_endDate = $scope.selectedActivity.end_date.toISOString().substring(0, 10);
                        $scope.selectedActivity.original_end_date = original_endDate.substring(8, 10) + original_endDate.substring(4, 7) + "-" + original_endDate.substring(0, 4);
                        $scope.reloadCost(selectedTask.id);
                    });
                }
                $scope.isUpdateTaskFromLightbox = false;
                //   $scope.scheduleGanttInstance.parse({data:$scope.schedule.data,links:[]})
                // $scope.scheduleGanttInstance.selectTask(id);

            });

            $scope.updateCost = function (costObj, drag_direction) {
                console.log('here');
                $scope.updateStatus = "";
                $scope.selectedCost = costObj;
                var original_end_date; var original_start_date;
                var start_array = []; var end_array = [];
                var operation; var start_date; var costID = costObj.id;
                var numberOfTextboxToBeRemoved;
                //right-left - the new ending date is smaller than the current ending date
                //left-right - the new starting date is bigger than the current starting date
                //left-left - the new starting date is smaller than the current starting date
                //right-right - the new ending date is bigger than the current ednign date
                if (drag_direction === "right-left" || drag_direction === "left-right") {
                    operation = "4";
                    original_end_date = getActivityOriginalStartDate($scope.selectedActivity.original_end_date);
                    original_start_date = getActivityOriginalStartDate($scope.selectedActivity.original_start_date);
                    if (drag_direction === "left-right") {
                        start_date = $scope.selectedActivity.start_date.toISOString().substring(0, 10);
                        start_date = (moment(start_date).isoWeekday() === 1) ? start_date
                            : moment(start_date).startOf('isoWeek').format(sqlDateFormat);
                        if ($scope.scheduleScale === "week")
                            numberOfTextboxToBeRemoved = getWeekDifferences(start_date, original_start_date);
                        else if ($scope.scheduleScale === "month") {
                            var abStart = getAbsoulteMonths(start_date);
                            var abEnd = getAbsoulteMonths(original_start_date);
                            numberOfTextboxToBeRemoved = Math.abs(abEnd - abStart);
                        } else if ($scope.scheduleScale === "year") {
                            numberOfTextboxToBeRemoved = Math.abs(moment(start_date).format('YYYY') - moment(original_start_date).format('YYYY'));
                        }
                    }

                }
                else if (drag_direction === "left-left") {
                    operation = "5";
                    original_end_date = getActivityOriginalStartDate($scope.selectedActivity.original_end_date);
                    original_start_date = moment($scope.selectedActivity.start_date).startOf('isoWeek').toISOString().substring(0, 10);
                } else if (drag_direction == "right-right") {
                    operation = "5";
                    original_start_date = getActivityOriginalStartDate($scope.selectedActivity.original_start_date);
                    original_end_date = moment($scope.selectedActivity.end_date).endOf('isoWeek').toISOString().substring(0, 10);
                }
                if ($scope.scheduleScale === "week") {
                    var difference = getWeekDifferences(original_start_date, original_end_date);
                    for (var i = 0; i < difference; i++) {
                        start_array.push(original_start_date);
                        original_end_date = moment(original_start_date).add(6, 'days').format(sqlDateFormat);
                        end_array.push(original_end_date);
                        original_start_date = moment(original_start_date).add(1, 'w').format(sqlDateFormat);
                    }
                } else if ($scope.scheduleScale === "month") {
                    var difference = $scope.textBoxValues[costObj.id].length;
                    if (drag_direction === "right-left")
                        difference += 1;

                    if (difference == 0) {
                        start_array.push(moment($scope.selectedActivity.start_date).format(sqlDateFormat));
                        end_array.push(moment($scope.selectedActivity.end_date).format(sqlDateFormat));
                    } else {
                        var start_date = moment($scope.selectedActivity.start_date).format(sqlDateFormat);
                        start_date = (moment(start_date).isoWeekday() === 1) ? start_date : moment(start_date).startOf('isoWeek').format(sqlDateFormat);
                        var end_date = moment($scope.selectedActivity.end_date).format(sqlDateFormat);
                        end_date = (moment(end_date).isoWeekday() === 7) ? end_date : moment(end_date).endOf('isoWeek').format(sqlDateFormat);
                        end_date = moment(end_date).add(-1, 'days').format(sqlDateFormat);
                        var activityStartDate = original_start_date;
                        var activityEndDate = original_end_date;

                        for (var i = 0; i < difference; i++) {
                            start_array.push(activityStartDate);
                            end = moment(activityStartDate).clone().endOf('month').format(sqlDateFormat);

                            var diff = moment(end_date).diff(moment(end), 'days');
                            if (diff >= 0) {
                                end_array.push(end);
                                activityStartDate = moment(end).add(1, 'days').format(sqlDateFormat);
                            }
                            else {

                                end_array.push(end_date);
                                activityStartDate = moment(end).add(1, 'days').format(sqlDateFormat);
                            }
                        }
                        if (drag_direction === "left-right")
                            start_array[numberOfTextboxToBeRemoved] = start_date;
                    }
                } else if ($scope.scheduleScale === "year") {
                    var difference = $scope.textBoxValues[costObj.id].length;

                    if (difference == 0) {
                        start_array.push(moment($scope.selectedActivity.start_date).format(sqlDateFormat));
                        end_array.push(moment($scope.selectedActivity.end_date).format(sqlDateFormat));
                    } else {
                        var start_date = moment($scope.selectedActivity.start_date).format(sqlDateFormat);
                        start_date = (moment(start_date).isoWeekday() === 1) ? start_date : moment(start_date).startOf('isoWeek').format(sqlDateFormat);
                        var end_date = moment($scope.selectedActivity.end_date).format(sqlDateFormat);
                        end_date = (moment(end_date).isoWeekday() === 7) ? end_date : moment(end_date).endOf('isoWeek').format(sqlDateFormat);
                        end_date = moment(end_date).add(-1, 'days').format(sqlDateFormat);
                        var activityStartDate = original_start_date;
                        var activityEndDate = original_end_date;

                        for (var i = 0; i < difference; i++) {
                            start_array.push(activityStartDate);
                            end = moment(activityStartDate).clone().endOf('year').format(sqlDateFormat);

                            var diff = moment(end_date).diff(moment(end), 'days');
                            if (diff >= 0) {
                                end_array.push(end);
                                activityStartDate = moment(end).add(1, 'days').format(sqlDateFormat);
                            }
                            else {
                                end_array.push(end_date);
                                activityStartDate = moment(end).add(1, 'days').format(sqlDateFormat);
                            }
                        }
                        if (drag_direction === "left-right")
                            start_array[numberOfTextboxToBeRemoved] = start_date;
                    }
                }
                //var index = 0;
                //for (var i = 0; i < $scope.methods.length; i++) {
                //    if ($scope.methods[i].name === $scope.selectedCost.text) {
                //        index = i;
                //    }
                //}
                var costID = $scope.selectedCost.id;
                $scope.FTECostID = [];
                for (var j = 0; j < $scope.textBoxValues[costID].length; j++) {
                    $scope.FTECostID.push($scope.selectedCost.activity + "_" + $scope.selectedCost.cost_id + "_" + j);
                }
                var cost = {
                    "Operation": operation,
                    "ProgramID": $scope.selectedCost.program,
                    "ProgramElementID": $scope.selectedCost.program_element,
                    "ProjectID": $scope.selectedCost.project,
                    "TrendNumber": $scope.selectedCost.trend,
                    "ActivityID": $scope.selectedCost.activity,
                    "CostID": costID,
                    "CostType": $scope.selectedCost.cost_type,
                    "Description": $scope.selectedCost.description,
                    "Scale": $scope.scheduleScale,
                    "StartDate": start_array.join(","),
                    "EndDate": end_array.join(","),
                    "TextBoxValue": $scope.textBoxValues[costID].join(","),
                    "Base": $scope.unitCost[costID],
                    "UnitBudget": $scope.unitBudget[costID],
                    "Drag_Direction": drag_direction,
                    "NumberOfTextboxToBeRemoved": numberOfTextboxToBeRemoved,
                    "FTEIDList": $scope.FTECostID.join(","),
                    "CostLineItemID": $scope.selectedCost.CostLineItemID,
                    "UnitType": ""
                };
                //if (cost.CostType == "U") {
                //    cost["UnitType"] = $scope.unit_type[costID].name;
                //}
                if (cost.CostType == "F") {
                    //  cost["FTEHours"] = fteCostArray.join(",");
                    cost["FTEHours"] = $scope.fteHours[costID].join(",");
                    cost["FTECost"] = $scope.fteCosts[costID].join(",")
                    cost["EmployeeID"] = $scope.employee_id[costID].value;
                }

                if (cost.CostType == "U") {
                    cost["UnitType"] = $scope.unit_type[costID].name;
                }

                //ODC
                if (cost.CostType == "ODC") {

                    cost.UnitType = $scope.description[cost.CostID].name;
                    //find odc type id
                    cost["ODCTypeID"] = 1; // dummy
                    for (var x = 0; x < $scope.ODCTypes.length; x++) {
                        if ($scope.ODCTypes[x].name == cost.Description) {
                            cost["ODCTypeID"] = $scope.ODCTypes[x].value;
                            break;
                        }
                    }

                    console.log(cost["ODCTypeID"]);
                }

                //Subcontractors
                if (cost.CostType == "L") {
                    cost.UnitType = $scope.description[cost.CostID].name;
                    //find subcontractor type id
                    cost["SubcontractorTypeID"] = 1; // dummy
                    for (var x = 0; x < $scope.subcontractorTypes.length; x++) {
                        if ($scope.subcontractorTypes[x].name == cost.Description) {
                            cost["SubcontractorTypeID"] = $scope.subcontractorTypes[x].value;
                            break;
                        }
                    }
                    cost["SubcontractorID"] = $scope.subcontractor_id[costID].value;
                }

                //Materials
                if (cost.CostType == "U") {

                    console.log($scope.unit_type);
                    cost.UnitType = $scope.description[cost.CostID].name;
                    //find material type id
                    cost["MaterialCategoryID"] = 1; // dummy
                    for (var x = 0; x < $scope.materialCategories.length; x++) {
                        if ($scope.materialCategories[x].name == cost.Description) {
                            cost["MaterialCategoryID"] = $scope.materialCategories[x].value;
                            break;
                        }
                    }
                    cost["MaterialID"] = $scope.material_id[costID].value;
                    cost["UnitType"] = $scope.unit_type[costID].name;
                    console.log(cost["MaterialCategoryID"]);
                }

                // Only for update

                cost["CostID"] = $scope.selectedCost.cost_id;
                return cost;
            }

            //luan quest 3
            var isCurrentTrend = false;
            var isTrue = false;   //Swapnil  24-11-2020
            var grid_width = 900;
            if (delayedData[3] == 1000) {
                isCurrentTrend = true;
                grid_width = 1000;
            }

            //Swapnil  24-11-2020
            if (delayedData[3] == 3000) {
                grid_width = 1000;
                isTrue = true;
            }

            $scope.costGanttInstance.config.drag_links = false;
            $scope.costGanttInstance.config.drag_resize = false;
            $scope.costGanttInstance.config.drag_progress = false;
            $scope.costGanttInstance.config.fit_tasks = false;
            $scope.costGanttInstance.config.scroll_on_click = false;
            $scope.costGanttInstance.config.min_column_width = 120;
            $scope.costGanttInstance.config.initial_scroll = false;
            $scope.costGanttInstance.config.drag_move = false;
            $scope.costGanttInstance.config.prevent_default_scroll = true;
            $scope.costGanttInstance.config.task_height = 20;
            $scope.costGanttInstance.config.row_height = 25;
            $scope.costGanttInstance.config.select_task = false;
            $scope.costGanttInstance.config.grid_width = grid_width;
            $scope.costGanttInstance.config.hide_tooltip = true;

            //Cofig columns for Cost Chart
            if (isCurrentTrend) {
                // ------------Swapnil 24-11-2020--------------------------------
                var auth = $scope.localStorageSrevice.get('authorizationData');
                if (auth.role == "Executive Manager") {
                    $scope.costGanttInstance.config.columns = [
                        { name: "delete", label: "", width: 30, align: "center" },
                        //{ name: "costLineItemIdNew", label: "#", width: 200, align: "left", resize: true }, 
                        //{ name: "newCostLineItemId", label: "#", width: 200, align: "left", resize: true },
                        // Swapnil 30/11/2020
                        { name: "newCostLineItemId", label: "#", width: 40, align: "left", resize: true },
                        { name: "cost_track_type", label: "", width: 30, hide: false, align: "center" },
                        { name: "text", label: " Cost Type", width: 103, tree: false, align: "left", resize: true },
                        { name: "description", label: "Cost Type Category", width: 220, align: "left", resize: true },  //Replace "Type with category"  Manasi 24-07-2020
                        { name: "employee_id", label: "Name", width: 220, align: "left", resize: true }, //align: "center" Manasi
                        { name: "unit_type", label: "Unit Type", width: 100, align: "left", resize: true },  //align: "center" Manasi
                        //{ name: "unit_cost", label: "Unit Cost", width: 100, align: "right", resize: true }, //align: "center" Manasi
                        //{ name: "unit_budget", label: "Unit Budget", width: 100, align: "right", resize: true }, // align: "center" Manasi
                        { name: "unit_budget", label: "Unit Cost", width: 100, align: "right", resize: true }, // align: "center" Manasi
                        {
                            //name: "total_units", label: "# Of Units", width: 100, align: "center", resize: true, template: function (obj) {
                            name: "total_units", label: "# Of Units", width: 100, align: "right", resize: true, template: function (obj) {
                                if (obj.text.indexOf("Material") >= 0 || obj.text.indexOf("Labor") >= 0) {
                                    console.log(obj.total_units);
                                    return roundToTwo(Number(obj.total_units));
                                    if (Number(obj.total_units) % 1 > 0) {
                                        //2
                                        return roundToTwo(Number(obj.total_units));
                                    } else {
                                        return '';
                                    }
                                    //} else if (obj.text.indexOf('Contractor') >= 0 || obj.text.indexOf('ODC') >= 0) {
                                } else if (obj.text.indexOf('Subcontractor') >= 0 || obj.text.indexOf('ODC') >= 0) {   //Manasi 17-07-2020
                                    return 'N/A';
                                } else {
                                    return '';
                                }
                            }  // Manasi
                        },
                        //-------------------Manasi 16-07-2020-----------------------------------------
                        {
                            name: "total_budget", label: "Budget", width: 100, align: "right", resize: true, template: function (obj) {
                                if (obj.total_budget === "") {
                                    return '';
                                }

                                //return $filter('currency')(obj.total_budget, '$', formatCurrency(obj.total_budget)); // Pritesh Commented added below to make consistent in Current view on 25jul2020
                                return $filter('currency')(obj.total_budget, '$', 2);
                            }
                        },
                        {
                            name: "total_cost", label: "EAC", width: 100, align: "right", resize: true, template: function (obj) {     //Manasi 16-07-2020
                                if (obj.total_cost === "") {
                                    return '';
                                }
                                return $filter('currency')(obj.total_cost, '$', 2);  // Pritesh 25Jul2020 to make it consistent
                            }
                        },

                        {
                            name: "actual_rate", label: "Actual rate", width: 100, align: "right", resize: true, template: function (obj) {     //Manasi 16-07-2020
                                if (obj.actual_rate === "") {
                                    return '';
                                }
                                return $filter('currency')(obj.actual_rate, '$', 2);  // Pritesh 25Jul2020 to make it consistent
                            }
                        },
                        {
                            name: "total_actual_cost", label: "Actual Budget", width: 100, align: "right", resize: true, template: function (obj) {     //Manasi 16-07-2020
                                if (obj.total_actual_cost === "") {
                                    return '';
                                }
                                return $filter('currency')(obj.total_actual_cost, '$', 2);
                            }
                        },

                    ];
                } else {
                    $scope.costGanttInstance.config.columns = [
                        { name: "delete", label: "", width: 30, align: "center" },
                        //{ name: "costLineItemIdNew", label: "#", width: 200, align: "left", resize: true }, 
                        //{ name: "newCostLineItemId", label: "#", width: 200, align: "left", resize: true },
                        // Swapnil 30/11/2020
                        { name: "newCostLineItemId", label: "#", width: 40, align: "left", resize: true },
                        { name: "cost_track_type", label: "", width: 30, hide: false, align: "center" },
                        { name: "text", label: " Cost Type", width: 103, tree: false, align: "left", resize: true },
                        { name: "description", label: "Cost Type Category", width: 220, align: "left", resize: true },  //Replace "Type with category"  Manasi 24-07-2020
                        { name: "employee_id", label: "Name", width: 220, align: "left", resize: true }, //align: "center" Manasi
                        { name: "unit_type", label: "Unit Type", width: 100, align: "left", resize: true },  //align: "center" Manasi
                        //{ name: "unit_cost", label: "Unit Cost", width: 100, align: "right", resize: true }, //align: "center" Manasi
                        //{ name: "unit_budget", label: "Unit Budget", width: 100, align: "right", resize: true }, // align: "center" Manasi
                        { name: "unit_budget", label: "Unit Cost", width: 100, align: "right", resize: true }, // align: "center" Manasi
                        {
                            //name: "total_units", label: "# Of Units", width: 100, align: "center", resize: true, template: function (obj) {
                            name: "total_units", label: "# Of Units", width: 100, align: "right", resize: true, template: function (obj) {
                                if (obj.text.indexOf("Material") >= 0 || obj.text.indexOf("Labor") >= 0) {
                                    console.log(obj.total_units);
                                    return roundToTwo(Number(obj.total_units));
                                    if (Number(obj.total_units) % 1 > 0) {
                                        //2
                                        return roundToTwo(Number(obj.total_units));
                                    } else {
                                        return '';
                                    }
                                    //} else if (obj.text.indexOf('Contractor') >= 0 || obj.text.indexOf('ODC') >= 0) {
                                } else if (obj.text.indexOf('Subcontractor') >= 0 || obj.text.indexOf('ODC') >= 0) {   //Manasi 17-07-2020
                                    return 'N/A';
                                } else {
                                    return '';
                                }
                            }  // Manasi
                        },
                        //-------------------Manasi 16-07-2020-----------------------------------------
                        {
                            name: "total_budget", label: "Budget", width: 100, align: "right", resize: true, template: function (obj) {
                                if (obj.total_budget === "") {
                                    return '';
                                }

                                //return $filter('currency')(obj.total_budget, '$', formatCurrency(obj.total_budget)); // Pritesh Commented added below to make consistent in Current view on 25jul2020
                                return $filter('currency')(obj.total_budget, '$', 2);
                            }
                        },
                        {
                            name: "total_cost", label: "EAC", width: 100, align: "right", resize: true, template: function (obj) {     //Manasi 16-07-2020
                                if (obj.total_cost === "") {
                                    return '';
                                }
                                return $filter('currency')(obj.total_cost, '$', 2);  // Pritesh 25Jul2020 to make it consistent
                            }
                        }
                    ];
                }

                // --------------------------------------------
            }
            else if (isTrue) {  //isTrue == currentTrend
                // ------------Swapnil 24-11-2020--------------------------------
                var auth = $scope.localStorageSrevice.get('authorizationData');
                if (auth.role == "Executive Manager") {
                    $scope.costGanttInstance.config.columns = [
                        { name: "delete", label: "", width: 30, align: "center" },
                        //{ name: "costLineItemIdNew", label: "#", width: 200, align: "left", resize: true }, 
                        //{ name: "newCostLineItemId", label: "#", width: 200, align: "left", resize: true },
                        // Swapnil 30/11/2020
                        { name: "newCostLineItemId", label: "#", width: 40, align: "left", resize: true },
                        //{ name: "cost_track_type", label: "", width: 30, hide: false, align: "center" },
                        { name: "text", label: " Cost Type", width: 103, tree: false, align: "left", resize: true },
                        { name: "description", label: "Cost Type Category", width: 220, align: "left", resize: true },  //Replace "Type with category"  Manasi 24-07-2020
                        { name: "employee_id", label: "Name", width: 220, align: "left", resize: true }, //align: "center" Manasi
                        { name: "unit_type", label: "Unit Type", width: 100, align: "left", resize: true },  //align: "center" Manasi
                        //{ name: "unit_cost", label: "Unit Cost", width: 100, align: "right", resize: true }, //align: "center" Manasi
                        //{ name: "unit_budget", label: "Unit Budget", width: 100, align: "right", resize: true }, // align: "center" Manasi
                        { name: "unit_budget", label: "Unit Cost", width: 100, align: "right", resize: true }, // align: "center" Manasi
                        {
                            //name: "total_units", label: "# Of Units", width: 100, align: "center", resize: true, template: function (obj) {
                            name: "total_units", label: "# Of Units", width: 100, align: "right", resize: true, template: function (obj) {
                                if (obj.text.indexOf("Material") >= 0 || obj.text.indexOf("Labor") >= 0) {
                                    console.log(obj.total_units);
                                    return roundToTwo(Number(obj.total_units));
                                    if (Number(obj.total_units) % 1 > 0) {
                                        //2
                                        return roundToTwo(Number(obj.total_units));
                                    } else {
                                        return '';
                                    }
                                    //} else if (obj.text.indexOf('Contractor') >= 0 || obj.text.indexOf('ODC') >= 0) {
                                } else if (obj.text.indexOf('Subcontractor') >= 0 || obj.text.indexOf('ODC') >= 0) {   //Manasi 17-07-2020
                                    return 'N/A';
                                } else {
                                    return '';
                                }
                            }  // Manasi
                        },
                        //-------------------Manasi 16-07-2020-----------------------------------------
                        {
                            name: "total_budget", label: "Budget", width: 100, align: "right", resize: true, template: function (obj) {
                                if (obj.total_budget === "") {
                                    return '';
                                }

                                //return $filter('currency')(obj.total_budget, '$', formatCurrency(obj.total_budget)); // Pritesh Commented added below to make consistent in Current view on 25jul2020
                                return $filter('currency')(obj.total_budget, '$', 2);
                            }
                        },
                        {
                            name: "total_cost", label: "EAC", width: 100, align: "right", resize: true, template: function (obj) {     //Manasi 16-07-2020
                                if (obj.total_cost === "") {
                                    return '';
                                }
                                return $filter('currency')(obj.total_cost, '$', 2);  // Pritesh 25Jul2020 to make it consistent
                            }
                        },

                        {
                            name: "actual_rate", label: "Actual rate", width: 100, align: "right", resize: true, template: function (obj) {     //Manasi 16-07-2020
                                if (obj.actual_rate === "") {
                                    return '';
                                }
                                return $filter('currency')(obj.actual_rate, '$', 2);  // Pritesh 25Jul2020 to make it consistent
                            }
                        },
                        {
                            name: "total_actual_cost", label: "Actual Budget", width: 100, align: "right", resize: true, template: function (obj) {     //Manasi 16-07-2020
                                if (obj.total_actual_cost === "") {
                                    return '';
                                }
                                return $filter('currency')(obj.total_actual_cost, '$', 2);
                            }
                        },

                    ];
                }
                else {
                    $scope.costGanttInstance.config.columns = [
                        { name: "delete", label: "", width: 30, align: "center" },
                        //{ name: "costLineItemIdNew", label: "#", width: 200, align: "left", resize: true }, 
                        //{ name: "newCostLineItemId", label: "#", width: 200, align: "left", resize: true },
                        // Swapnil 30/11/2020
                        { name: "newCostLineItemId", label: "#", width: 40, align: "left", resize: true },
                        //{ name: "cost_track_type", label: "", width: 30, hide: false, align: "center" },
                        { name: "text", label: " Cost Type", width: 103, tree: false, align: "left", resize: true },
                        { name: "description", label: "Cost Type Category", width: 220, align: "left", resize: true },  //Replace "Type with category"  Manasi 24-07-2020
                        { name: "employee_id", label: "Name", width: 220, align: "left", resize: true }, //align: "center" Manasi
                        { name: "unit_type", label: "Unit Type", width: 100, align: "left", resize: true },  //align: "center" Manasi
                        //{ name: "unit_cost", label: "Unit Cost", width: 100, align: "right", resize: true }, //align: "center" Manasi
                        //{ name: "unit_budget", label: "Unit Budget", width: 100, align: "right", resize: true }, // align: "center" Manasi
                        { name: "unit_budget", label: "Unit Cost", width: 100, align: "right", resize: true }, // align: "center" Manasi
                        {
                            //name: "total_units", label: "# Of Units", width: 100, align: "center", resize: true, template: function (obj) {
                            name: "total_units", label: "# Of Units", width: 100, align: "right", resize: true, template: function (obj) {
                                if (obj.text.indexOf("Material") >= 0 || obj.text.indexOf("Labor") >= 0) {
                                    console.log(obj.total_units);
                                    return roundToTwo(Number(obj.total_units));
                                    if (Number(obj.total_units) % 1 > 0) {
                                        //2
                                        return roundToTwo(Number(obj.total_units));
                                    } else {
                                        return '';
                                    }
                                    //} else if (obj.text.indexOf('Contractor') >= 0 || obj.text.indexOf('ODC') >= 0) {
                                } else if (obj.text.indexOf('Subcontractor') >= 0 || obj.text.indexOf('ODC') >= 0) {   //Manasi 17-07-2020
                                    return 'N/A';
                                } else {
                                    return '';
                                }
                            }  // Manasi
                        },
                        //-------------------Manasi 16-07-2020-----------------------------------------
                        {
                            name: "total_budget", label: "Budget", width: 100, align: "right", resize: true, template: function (obj) {
                                if (obj.total_budget === "") {
                                    return '';
                                }

                                //return $filter('currency')(obj.total_budget, '$', formatCurrency(obj.total_budget)); // Pritesh Commented added below to make consistent in Current view on 25jul2020
                                return $filter('currency')(obj.total_budget, '$', 2);
                            }
                        },
                        {
                            name: "total_cost", label: "EAC", width: 100, align: "right", resize: true, template: function (obj) {     //Manasi 16-07-2020
                                if (obj.total_cost === "") {
                                    return '';
                                }
                                return $filter('currency')(obj.total_cost, '$', 2);  // Pritesh 25Jul2020 to make it consistent
                            }
                        }

                        //,{
                        //    name: "actual_rate", label: "Actual rate", width: 100, align: "right", resize: true, template: function (obj) {     //Manasi 16-07-2020
                        //        if (obj.actual_rate === "") {
                        //            return '';
                        //        }
                        //        return $filter('currency')(obj.actual_rate, '$', 2);  // Pritesh 25Jul2020 to make it consistent
                        //    }
                        //},
                        //{
                        //    name: "total_actual_cost", label: "Actual Budget", width: 100, align: "right", resize: true, template: function (obj) {     //Manasi 16-07-2020
                        //        if (obj.total_actual_cost === "") {
                        //            return '';
                        //        }
                        //        return $filter('currency')(obj.total_actual_cost, '$', 2);
                        //    }
                        //}
                    ];
                }

                // --------------------------------------------
            } else {
                $scope.costGanttInstance.config.columns = [
                    { name: "delete", label: "", width: 30, align: "center" },
                    //{ name: "costLineItemIdNew", label: "#", width: 200, align: "left", resize: true },
                    //{ name: "newCostLineItemId", label: "#", width: 200, align: "left", resize: true },
                    // Swapnil 30/11/2020
                    { name: "newCostLineItemId", label: "#", width: 40, align: "left", resize: true },
                    { name: "ABCD", label: "", width: 30, hide: true },
                    { name: "text", label: " Cost Type", width: 102, tree: false, align: "left", resize: true },
                    { name: "description", label: "Cost Type Category", width: 185, align: "left", resize: true },    //Replace "Type with category"  Manasi 24-07-2020
                    { name: "employee_id", label: "Name", width: 185, align: "left", resize: true },  //align: "center" Manasi
                    { name: "unit_type", label: "Unit Type", width: 100, align: "left", resize: true },  //align: "center"  Manasi
                    //{ name: "unit_cost", label: "Unit Cost", width: 100, align: "right", resize: true }, //align: "center" Manasi
                    //{ name: "unit_budget", label: "Unit Budget", width: 100, align: "right", resize: true }, //align: "center" Manasi
                    { name: "unit_budget", label: "Unit Cost", width: 100, align: "right", resize: true }, //align: "center" Manasi
                    {
                        //name: "total_units", label: "# Of Units", width: 100, align: "center", resize: true, template: function (obj) {
                        name: "total_units", label: "# Of Units", width: 100, align: "right", resize: true, template: function (obj) { //Manasi
                            if (obj.text.indexOf("Material") >= 0 || obj.text.indexOf("Labor") >= 0) {
                                console.log(obj.total_units);
                                return roundToTwo(Number(obj.total_units));
                                if (Number(obj.total_units) % 1 > 0) {
                                    //2
                                    return roundToTwo(Number(obj.total_units));
                                } else {
                                    return '';
                                }
                                //} else if (obj.text.indexOf('Contractor') >= 0 || obj.text.indexOf('ODC') >= 0) {
                            } else if (obj.text.indexOf('Subcontractor') >= 0 || obj.text.indexOf('ODC') >= 0) {   //Manasi 17-07-2020
                                return 'N/A';
                            } else {
                                return '';
                            }
                        }
                    },
                    //{
                    //    name: "total_cost", label: "Price", width: 110, align: "right", resize: true, template: function (obj) {
                    //        if (obj.total_cost === "") {
                    //            return '';
                    //        }

                    //        return $filter('currency')(obj.total_cost, '$', formatCurrency(obj.total_cost));

                    //    }
                    //},
                    {
                        name: "total_budget", label: "Budget", width: 110, align: "right", resize: true, template: function (obj) {
                            if (obj.total_budget === "") {
                                return '';
                            }
                            // return $filter('currency')(obj.total_budget, '$', formatCurrency(obj.total_budget));  Pritesh Commented on 25Jul2020 as to keept the format consistent
                            return $filter('currency')(obj.total_budget, '$', 2);
                        }
                    }

                ];
            }

            $scope.costGanttInstance.config.lightbox.sections = [
                {
                    name: "method",
                    height: 38,
                    map_to: "text",
                    type: "select",

                    options: [
                        { key: 'FTECost', label: 'FTE' },
                        //{ key: 'LumpsumCost', label: 'Contractor' },
                        { key: 'LumpsumCost', label: 'Subcontractor' },  //Manasi 17-07-2020
                        { key: 'UnitCost', label: 'Unit' },
                        //{ key: 'ODC', label: 'ODC' },
                        { key: 'PercentBasisCost', label: 'Percent' }],
                    focus: true
                },
                { name: "description", height: 28, map_to: "description", type: "textarea", focus: true },
                { name: "base", height: 28, map_to: "base", type: "textarea", focus: true },
                {
                    name: "scale",
                    height: 38,
                    map_to: "scale",
                    type: "select",
                    options: [{ key: 'W', label: 'W' }, { key: 'M', label: 'M' }, { key: 'Y', label: 'Y' }],
                    focus: true
                }
            ];


            $scope.costGanttInstance.templates.tooltip_text = function (start, end, task) {

                $scope.scheduleGanttInstance.ext.tooltips.tooltip.hide();
                var row = $("#cost-gantt .gantt_row[task_id='" + task.id + "']");
                var cells = row.find('.gantt_cell');

                var bbb = cells.get(0);
                if (isCurrentTrend)
                    bbb = cells.get(1);
                //activate tooltip on first mouseover

                $(bbb).on('mouseover', function () {
                    $(this).addClass('hover');
                    if ($scope.trend.TrendNumber == "1000") {
                        if (task.cost_track_type == "B") {
                            $(bbb).tooltip({ title: "Budget", placement: "right" }).tooltip('show');
                        }
                        //else if (task.cost_track_type == "A-E") {
                        //    $(bbb).tooltip({ title: "Actual - Estimate to Completion", placement: "right" }).tooltip('show');
                        //}
                        //--------------Manasi-------------------
                        else if (task.cost_track_type == "A/F") {
                            $(bbb).tooltip({ title: "Actual/Forecast", placement: "right" }).tooltip('show');
                        }
                        //-----------------------------------------
                        else {
                            $(bbb).tooltip().tooltip('hide');
                        }

                    } else {
                        //if (typeof $(bbb).attr("data-original-title") == 'undefined') {
                        //    $(bbb).tooltip({ title: "Delete", placement: "right" }).tooltip('show');
                        //}
                        //$(bbb).tooltip({ title: "Delete", placement: "right" }).tooltip('show');
                    }

                });
                $(bbb).on('mouseleave', function () {
                    $(this).removeClass('hover');
                });
                //show dhtmlx tooltip on mouseover on any other columns
                if ($(bbb).hasClass('hover')) {
                } else {


                    var costLineItemID = task.CostLineItemID;

                    if (task.CostLineItemID && (task.cost_track_type_id == 3 || task.cost_track_type_id == 4)) {
                        //costLineItemID = costLineItemID.slice(0, -3);
                        costLineItemID = costLineItemID;  //Manasi 02-12-2020
                    }

                    //return "<b>Cost Total : </b>" + $filter('currency')(task.total_cost, '$', formatCurrency(task.totalCost));
                    if (costLineItemID)
                        return "<b>Line Item: </b>" + costLineItemID;
                }

            }

            $scope.costGanttInstance.templates.task_class = function (start, end, task) {
                return "costBar";
            };

            $scope.costGanttInstance.templates.grid_row_class = function (start, end, task) {
                return "";
            }

            $scope.costGanttInstance.templates.scale_cell_class = function (date) {
                return "";
            }

            $scope.costGanttInstance.templates.task_cell_class = function (item, date) {
                return "";
            }

            $scope.costGanttInstance.attachEvent("onEmptyClick", function (date, e) {
                return;
                var target = null;
                if (typeof $scope.costGanttInstance.getActionData == 'function') {
                    target = $scope.costGanttInstance.getActionData(e).section;
                }
                if (target != selected_row) {
                    selected_row = target;
                    $scope.costGanttInstance.setCurrentView();
                }
            });

            $scope.costGanttInstance.locale = {
                date: {
                    month_full: ["January", "February", "March", "April", "May", "June", "July",
                        "August", "September", "October", "November", "December"],
                    month_short: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep",
                        "Oct", "Nov", "Dec"],
                    day_full: ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday",
                        "Saturday"],
                    day_short: ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"]
                },
                labels: {
                    new_task: "New task",
                    icon_save: "Save",
                    icon_cancel: "Cancel",
                    icon_details: "Details",
                    icon_edit: "Edit",
                    icon_delete: "Delete",
                    confirm_closing: "",//Your changes will be lost, are you sure ?
                    confirm_deleting: "Task will be deleted permanently, are you sure?",

                    section_method: "Method",
                    section_description: "Description",
                    section_base: "Base",
                    section_scale: "Scale",
                    section_time: "Time period",

                    /* link confirmation */

                    confirm_link_deleting: "Dependency will be deleted permanently, are you sure?",
                    link_from: "From",
                    link_to: "To",
                    link_start: "Start",
                    link_end: "End",

                    minutes: "Minutes",
                    hours: "Hours",
                    days: "Days",
                    weeks: "Week",
                    months: "Months",
                    years: "Years"
                }
            };

            $scope.test = function (a) {
                $timeout(function () {
                    costCalculation(a);
                });
            }

            $scope.costGanttInstance.templates.task_text = function (start, end, task) {
                console.log(start);
                console.log(end);
                console.log(task);
                var s = getFirstDayOfMonth($scope.selectedActivity.start_date);
                var e = getLastDayOfMonth($scope.selectedActivity.end_date);
                if ($scope.scheduleScale === "week") {
                    start = moment($scope.selectedActivity.start_date);
                    var activityEndDate = moment($scope.selectedActivity.end_date).add(1, 'days');
                    end = moment(activityEndDate);
                    var tempTask = task;
                    tempTask.start_date = start;
                    tempTask.end_date = end;
                    tempTask.original_start_date = start;
                    tempTask.original_end_date = end;
                    var sizes = $scope.costGanttInstance.getTaskPosition(tempTask);

                } else if ($scope.scheduleScale === "month") {
                    task.start_date = moment(s);
                    task.end_date = moment(e);
                    task.original_start_date = moment(s);
                    task.original_end_date = moment(e);
                    var sizes = $scope.costGanttInstance.getTaskPosition(task, moment(getFirstDayOfMonth(start)), moment(getLastDayOfMonth(end)).add(1, 'days'));
                } else if ($scope.scheduleScale === "year") {
                    //var sizes = $scope.costGanttInstance.getTaskPosition(task, $scope.activity_start_of_year, $scope.activity_end_of_year);
                    var s = getFirstDayOfYear($scope.selectedActivity.start_date);
                    var e = getLastDayOfYear($scope.selectedActivity.end_date);
                    task.start_date = moment(s);
                    task.end_date = moment(e);
                    task.original_start_date = moment(s);
                    task.original_end_date = moment(e);
                    var sizes = $scope.costGanttInstance.getTaskPosition(task);
                }
                var numberOfBoxes = 0;

                var year1 = moment($scope.selectedActivity.start_date).format('YYYY');
                var year2 = moment($scope.selectedActivity.end_date).format('YYYY');

                var month1 = moment($scope.selectedActivity.start_date).format('MM');
                var month2 = moment($scope.selectedActivity.end_date).format('MM');

                var week1 = moment($scope.selectedActivity.start_date).format('ww');
                var week2 = moment($scope.selectedActivity.end_date).format('ww');
                if ($scope.scheduleScale === "week") {
                    var startDate = (moment($scope.selectedActivity.start_date).isoWeekday() === 1) ? moment($scope.selectedActivity.start_date).format(sqlDateFormat)
                        : moment($scope.selectedActivity.start_date).startOf('isoWeek').format(sqlDateFormat);
                    var endDate = (moment($scope.selectedActivity.end_date).isoWeekday() === 7) ? moment($scope.selectedActivity.end_date).format(sqlDateFormat)
                        : moment($scope.selectedActivity.end_date).endOf('isoWeek').format(sqlDateFormat);



                    var st = moment(startDate).format(sqlDateFormat);
                    var ed = moment(endDate).format(sqlDateFormat);
                    year1 = moment(startDate).format('YYYY');
                    year2 = moment(endDate).format('YYYY');
                    var ONE_WEEK = 1000 * 60 * 60 * 24 * 7;  //# of seconds in a week
                    // Convert both dates to milliseconds
                    var date1_ms = moment(ed).format(sqlDateFormat);
                    var date2_ms = moment(st).format(sqlDateFormat);
                    var d1 = new Date(date1_ms);
                    var d2 = new Date(date2_ms);
                    // Calculate the difference in milliseconds
                    var difference_ms = Math.abs(d1.getTime() - d2.getTime());
                    // Convert back to weeks and return hole weeks
                    if (year2 == year1) {
                        numberOfBoxes = Math.ceil(difference_ms / ONE_WEEK);
                    } else {
                        numberOfBoxes = Math.ceil(difference_ms / ONE_WEEK);
                    }

                }
                else if ($scope.scheduleScale === "month") {
                    numberOfBoxes = (year2 - year1) * 12 + (month2 - month1) + 1;

                }
                else if ($scope.scheduleScale === "year")
                    numberOfBoxes = moment(e).format('YYYY') - moment(s).format('YYYY') + 1;
                var widthOfTextBox = sizes.width / numberOfBoxes;
                return renderCostBoxes(widthOfTextBox, numberOfBoxes, task.id);
            };

            $scope.costGanttInstance.attachEvent("onBeforeTaskDisplay", function (id, task) {
                console.log(task);
                $scope.isScaleChanged = true;
                var found = false;
                angular.forEach(phases, function (phase) {
                    if ($scope.schedulePhase === Number(phase.Order) && task.phase === (Number(phase.Order) * 1000)) {
                        //if (task.cost_track_type_id == 1)
                        found = true;
                        return true;
                        //else if(task.cost_track_type_id == 2 && $scope.trend.TrendStatusID == 1)
                        //    found = true;
                    }

                    if (!$scope.schedulePhase)
                        found = true;
                    //else if (task.cost_track_type_id == 2 && $scope.trend.TrendStatusID == 1)
                    //    found = true;
                });
                if (found == true)
                    return true;
                return found;
                //return true;
            });

            //$scope.scheduleGanttInstance.ext.tooltips.attach('onmouseleave', function () {
            //    alert();
            //});

            $scope.scheduleGanttInstance.attachEvent('onMouseMove', function (id, e) {

            });

            $scope.scheduleGanttInstance.attachEvent('onGanttRender', function () {
                //luan 3/7
                //$('.gantt_grid_head_originalStartDate').css("margin-left", "3%"); //Centering the label

                //var isTrue = false;
                //var id = 0;

                //$scope.scheduleGanttInstance.eachTask(function(task){
                //    if(task.$level == 2 && isTrue == false){
                //        id=task.id;
                //        alert(id);
                //        $scope.selectedActivity = $scope.scheduleGanttInstance.getTask(id);
                //        isTrue = true;
                //    }
                //    if(isTrue && id != 0)
                //    {
                //        $scope.scheduleGanttInstance.selectTask(id);
                //        return;
                //    }
                //})
            });

            $scope.costGanttInstance.attachEvent("onBeforeParse", function () {
                angular.forEach($scope.costs.data, function (item) {
                    item.start_date = moment(item.start_date).format(dateFormat);
                    item.end_date = moment(item.end_date).format(dateFormat);
                })
            });


            ///// -------------------------------- Pritesh New Logic added on 2 July 2020-------------------------------------------------------

            var OnlyOneTaskIdInstance = 1;
            $scope.costGanttInstance.attachEvent("onGanttRender", function () {
                $scope.isScaleChanged = true;

                $scope.costGanttInstance.eachTask(function (task) {
                    if ($scope.scheduleScale == 'week') {
                        //  alert("Before TaskID  : " + task.id + " -- Text Value : " + $scope.textBoxValues[task.id] + " Instance Value : " + OnlyOneTaskIdInstance);
                        if (OnlyOneTaskIdInstance == task.id) { //PRitesh: This condition is used since the task id are repeating it get click twice
                            OnlyOneTaskIdInstance++;
                            if ($scope.currentCostIndex == (task.id - 1)) {
                                $scope.textBoxIds[task.id] = [];
                            }
                            var div = $("." + task.id + "_cost");
                            var div = $("." + task.id + "_cost");
                            $(div).html(
                                $compile(
                                    $(div).html()
                                )($scope)
                            );
                            // Pritesh New Logic Start added on 2 july 2020
                            if ($scope.textBoxIds[task.id]) {

                                var dateFormatForLoop = "MM/DD/YYYY";

                                var StartFormatLoop = moment(task.start_date).format(dateFormatForLoop);
                                var EndFormatLoop = moment(task.end_date).format(dateFormatForLoop);
                                // alert(StartFormatLoop);

                                var start = new Date(StartFormatLoop);  //  new Date("05/25/2020"); // Month date year
                                var end = new Date(EndFormatLoop); // new Date("06/21/2020");
                                // alert(start);
                                var StartDateOfEachWeek = [];
                                //  daysOfYear.push(start);
                                var loop = new Date(start);

                                while (loop < end) {

                                    var d = new Date(loop),
                                        month = '' + (d.getMonth() + 1),
                                        day = '' + d.getDate(),
                                        year = d.getFullYear();

                                    if (month.length < 2)
                                        month = '0' + month;
                                    if (day.length < 2)
                                        day = '0' + day;

                                    var FormateLoop = [year, month, day].join('-');

                                    StartDateOfEachWeek.push(FormateLoop);
                                    var newDate = loop.setDate(loop.getDate() + 7);
                                    loop = new Date(newDate);
                                }
                                var TaskStartDate = [];
                                var SD = [];
                                var FormatTaskStart = task.individual_start_date;
                                if (FormatTaskStart)
                                    SD = FormatTaskStart.split(",");
                                var StringStr = "";

                                for (var k = 0; k < SD.length; k++) {
                                    TaskStartDate.push(SD[k].replace(" 00:00:00", ""));
                                }
                                var TxtValCount = 0;
                                var TextBoxValuesArray = [];
                                var TxtValCountWhenIndividualStartISNull = 0;
                                // alert("Before : " + $scope.textBoxValues[task.id]);
                                for (var i = 0; i < StartDateOfEachWeek.length; i++) { // Start Week Dates For Main Phase with comma Seperated Array
                                    var InnerLoopRecordNotMatch = 0;
                                    for (var k = 0; k < TaskStartDate.length; k++) {
                                        var d1 = new Date(StartDateOfEachWeek[i].trim());
                                        var d2 = new Date(TaskStartDate[k].trim());

                                        if (d1.getTime() === d2.getTime()) {//(StartDateOfEachWeek[i].trim() == TaskStartDate[k].trim()) {
                                            var numFormat = parseFloat($scope.textBoxValues[task.id][TxtValCount]);
                                            TextBoxValuesArray.push(numFormat.toFixed(2));
                                            //TextBoxValuesArray.push(parseFloat($scope.textBoxValues[task.id][TxtValCount]));
                                            TxtValCount++;
                                            InnerLoopRecordNotMatch++;
                                            break;
                                        } else {
                                            InnerLoopRecordNotMatch = 0;

                                        }
                                    }
                                    if (InnerLoopRecordNotMatch == 0) {
                                        if (TaskStartDate.length == 0) {
                                            var numFormat = parseFloat($scope.textBoxValues[task.id][TxtValCountWhenIndividualStartISNull]);
                                            TextBoxValuesArray.push(numFormat.toFixed(2));
                                            // TextBoxValuesArray.push($scope.textBoxValues[task.id][TxtValCountWhenIndividualStartISNull]);
                                            TxtValCountWhenIndividualStartISNull++;
                                        }
                                        else {
                                            TextBoxValuesArray.push('0.00');
                                        }
                                        //  TextBoxValuesArray.push('0');                                  
                                    }
                                }
                                $scope.textBoxValues[task.id] = TextBoxValuesArray;
                                //  alert(TextBoxValuesArray);
                                // alert("After : " + $scope.textBoxValues[task.id]);
                                // Pritesh New Logic Ended.

                                //    //Previous logic commented on 2 july 2020 by pritesh
                                //if ($scope.textBoxIds[task.id]) {
                                //    var textBoxVals = Array.apply(null, new Array($scope.numberOfBoxes[task.id])).map(Number.prototype.valueOf, 0);
                                //    for (var i = 0; i < $scope.textBoxIds[task.id].length; i++) {

                                //        var textBoxid = parseInt($scope.textBoxIds[task.id][i]);
                                //        textBoxVals[textBoxid] = parseFloat($scope.textBoxValues[task.id][i]);
                                //    }
                                //    $scope.textBoxValues[task.id] = textBoxVals;

                                //}
                                //else {
                                //    console.log('test1');
                                //    $scope.textBoxIds[task.id] = Array.apply(null, new Array($scope.numberOfBoxes[task.id])).map(Number.prototype.valueOf, 0);
                                //    $scope.textBoxValues[task.id] = Array.apply(null, new Array($scope.numberOfBoxes[task.id])).map(Number.prototype.valueOf, 0);
                                //    console.log('test2');
                                //    //for(var i = 0; i < $scope.textBoxValues[task.id].length - 1; i++){
                                //    //    $scope.textBoxIds[task.id][i] = i;
                                //    //}
                                //}
                                ////Previous Logic end here
                            }
                            else {
                                console.log('test1');
                                $scope.textBoxIds[task.id] = Array.apply(null, new Array($scope.numberOfBoxes[task.id])).map(Number.prototype.valueOf, 0);
                                $scope.textBoxValues[task.id] = Array.apply(null, new Array($scope.numberOfBoxes[task.id])).map(Number.prototype.valueOf, 0);
                                console.log('test2');
                                //for(var i = 0; i < $scope.textBoxValues[task.id].length - 1; i++){
                                //    $scope.textBoxIds[task.id][i] = i;
                                //}

                            }


                            $scope.fteCosts[task.id] = Array.apply(null, new Array($scope.numberOfBoxes[task.id])).map(Number.prototype.valueOf, 0);
                            $scope.unitCosts[task.id] = Array.apply(null, new Array($scope.numberOfBoxes[task.id])).map(Number.prototype.valueOf, 0);
                            // $scope.unitBudget[task.id] = Array.apply(null, new Array($scope.numberOfBoxes[task.id])).map(Number.prototype.valueOf, 0);
                            $scope.fteHours[task.id] = Array.apply(null, new Array($scope.numberOfBoxes[task.id])).map(Number.prototype.valueOf, 0);
                            if ($scope.isCostEdited[task.id] == true) {
                                $scope.costGanttInstance.callEvent('customClick', [task.id]);
                            }

                        }
                        else {
                            if ($scope.textBoxIds[task.id]) {
                                if ($scope.currentCostIndex == (task.id - 1)) {
                                    $scope.textBoxIds[task.id] = [];
                                }
                                var div = $("." + task.id + "_cost");
                                var div = $("." + task.id + "_cost");
                                $(div).html(
                                    $compile(
                                        $(div).html()
                                    )($scope)
                                );
                                $scope.textBoxValues[task.id] = $scope.textBoxValues[task.id];

                            }
                            else {
                                console.log('test1');
                                $scope.textBoxIds[task.id] = Array.apply(null, new Array($scope.numberOfBoxes[task.id])).map(Number.prototype.valueOf, 0);
                                $scope.textBoxValues[task.id] = Array.apply(null, new Array($scope.numberOfBoxes[task.id])).map(Number.prototype.valueOf, 0);
                                console.log('test2');
                                //for(var i = 0; i < $scope.textBoxValues[task.id].length - 1; i++){
                                //    $scope.textBoxIds[task.id][i] = i;
                                //}

                            }

                            $scope.fteCosts[task.id] = Array.apply(null, new Array($scope.numberOfBoxes[task.id])).map(Number.prototype.valueOf, 0);
                            $scope.unitCosts[task.id] = Array.apply(null, new Array($scope.numberOfBoxes[task.id])).map(Number.prototype.valueOf, 0);
                            // $scope.unitBudget[task.id] = Array.apply(null, new Array($scope.numberOfBoxes[task.id])).map(Number.prototype.valueOf, 0);
                            $scope.fteHours[task.id] = Array.apply(null, new Array($scope.numberOfBoxes[task.id])).map(Number.prototype.valueOf, 0);
                            if ($scope.isCostEdited[task.id] == true) {
                                $scope.costGanttInstance.callEvent('customClick', [task.id]);
                            }
                        }
                        //  alert("After TaskID  : " + task.id + " -- Text Value : " + $scope.textBoxValues[task.id] + " Instance Value : " + OnlyOneTaskIdInstance);
                    }
                    else if ($scope.scheduleScale == 'month') {

                        if (OnlyOneTaskIdInstance == task.id) { //PRitesh: This condition is used since the task id are repeating it get click twice
                            OnlyOneTaskIdInstance++;
                            if ($scope.currentCostIndex == (task.id - 1)) {
                                $scope.textBoxIds[task.id] = [];
                            }
                            var div = $("." + task.id + "_cost");
                            var div = $("." + task.id + "_cost");
                            $(div).html(
                                $compile(
                                    $(div).html()
                                )($scope)
                            );

                            // Pritesh New Logic Start added on 2 july 2020
                            if ($scope.textBoxIds[task.id]) {
                                var dateFormatForLoop = "MM/DD/YYYY";

                                var StartFormatLoop = moment(task.start_date).format(dateFormatForLoop);
                                var EndFormatLoop = moment(task.end_date).format(dateFormatForLoop);
                                // alert(StartFormatLoop);

                                var start = new Date(StartFormatLoop);  //  new Date("05/25/2020"); // Month date year
                                var end = new Date(EndFormatLoop); // new Date("06/21/2020");
                                // alert(start);
                                var StartDateOfEachWeek = [];
                                //  daysOfYear.push(start);
                                var loop = new Date(start);

                                while (loop < end) {

                                    var d = new Date(loop),
                                        month = '' + (d.getMonth() + 1),
                                        day = '' + d.getDate(),
                                        year = d.getFullYear();

                                    if (month.length < 2)
                                        month = '0' + month;
                                    if (day.length < 2)
                                        day = '0' + day;

                                    // var FormateLoop = [year, month, day].join('-');
                                    var FormateLoop = [year, month].join('-');
                                    StartDateOfEachWeek.push(FormateLoop);
                                    var newDate = loop.setMonth(loop.getMonth() + 1);
                                    loop = new Date(newDate);
                                }
                                //  alert(StartDateOfEachWeek);
                                var TaskStartDate = [];
                                var SD = [];
                                var FormatTaskStart = task.individual_start_date;
                                if (FormatTaskStart)
                                    SD = FormatTaskStart.split(",");
                                var StringStr = "";

                                for (var k = 0; k < SD.length; k++) {
                                    var dateFrom = SD[k].replace(" 00:00:00", "");
                                    var partsFrom = dateFrom.split('-');
                                    TaskStartDate.push(partsFrom[0] + "-" + partsFrom[1]);
                                    // TaskStartDate.push(SD[k].replace(" 00:00:00", ""));
                                }
                                var TxtValCount = 0;
                                var TextBoxValuesArray = [];
                                var TxtValCountWhenIndividualStartISNull = 0;
                                for (var i = 0; i < StartDateOfEachWeek.length; i++) { // Start Week Dates For Main Phase with comma Seperated Array
                                    var InnerLoopRecordNotMatch = 0;
                                    for (var k = 0; k < TaskStartDate.length; k++) {
                                        var d1 = new Date(StartDateOfEachWeek[i].trim());
                                        var d2 = new Date(TaskStartDate[k].trim());

                                        if (StartDateOfEachWeek[i].trim() === TaskStartDate[k].trim()) { //  (d1.getTime() === d2.getTime())
                                            var numFormat = parseFloat($scope.textBoxValues[task.id][TxtValCount]);
                                            TextBoxValuesArray.push(numFormat.toFixed(2));
                                            // TextBoxValuesArray.push(parseFloat($scope.textBoxValues[task.id][TxtValCount]));
                                            TxtValCount++;
                                            InnerLoopRecordNotMatch++;
                                            break;
                                        } else {
                                            InnerLoopRecordNotMatch = 0;

                                        }
                                    }
                                    if (InnerLoopRecordNotMatch == 0) {
                                        if (TaskStartDate.length == 0) {
                                            var numFormat = parseFloat($scope.textBoxValues[task.id][TxtValCountWhenIndividualStartISNull]);
                                            TextBoxValuesArray.push(numFormat.toFixed(2));
                                            //  TextBoxValuesArray.push($scope.textBoxValues[task.id][TxtValCountWhenIndividualStartISNull]);
                                            TxtValCountWhenIndividualStartISNull++;
                                        }
                                        else {
                                            TextBoxValuesArray.push('0.00');
                                        }
                                        //  TextBoxValuesArray.push('0');
                                    }
                                }
                                $scope.textBoxValues[task.id] = TextBoxValuesArray;


                                $scope.fteCosts[task.id] = Array.apply(null, new Array($scope.numberOfBoxes[task.id])).map(Number.prototype.valueOf, 0);
                                $scope.unitCosts[task.id] = Array.apply(null, new Array($scope.numberOfBoxes[task.id])).map(Number.prototype.valueOf, 0);
                                // $scope.unitBudget[task.id] = Array.apply(null, new Array($scope.numberOfBoxes[task.id])).map(Number.prototype.valueOf, 0);
                                $scope.fteHours[task.id] = Array.apply(null, new Array($scope.numberOfBoxes[task.id])).map(Number.prototype.valueOf, 0);
                                if ($scope.isCostEdited[task.id] == true) {
                                    $scope.costGanttInstance.callEvent('customClick', [task.id]);
                                }
                            }
                            else {
                                console.log('test1');
                                $scope.textBoxIds[task.id] = Array.apply(null, new Array($scope.numberOfBoxes[task.id])).map(Number.prototype.valueOf, 0);
                                $scope.textBoxValues[task.id] = Array.apply(null, new Array($scope.numberOfBoxes[task.id])).map(Number.prototype.valueOf, 0);
                                console.log('test2');
                                //for(var i = 0; i < $scope.textBoxValues[task.id].length - 1; i++){
                                //    $scope.textBoxIds[task.id][i] = i;
                                //}
                                $scope.fteCosts[task.id] = Array.apply(null, new Array($scope.numberOfBoxes[task.id])).map(Number.prototype.valueOf, 0);
                                $scope.unitCosts[task.id] = Array.apply(null, new Array($scope.numberOfBoxes[task.id])).map(Number.prototype.valueOf, 0);
                                // $scope.unitBudget[task.id] = Array.apply(null, new Array($scope.numberOfBoxes[task.id])).map(Number.prototype.valueOf, 0);
                                $scope.fteHours[task.id] = Array.apply(null, new Array($scope.numberOfBoxes[task.id])).map(Number.prototype.valueOf, 0);
                                if ($scope.isCostEdited[task.id] == true) {
                                    $scope.costGanttInstance.callEvent('customClick', [task.id]);
                                }
                            }
                        }
                        else {
                            if ($scope.textBoxIds[task.id]) {
                                if ($scope.currentCostIndex == (task.id - 1)) {
                                    $scope.textBoxIds[task.id] = [];
                                }
                                var div = $("." + task.id + "_cost");
                                var div = $("." + task.id + "_cost");
                                $(div).html(
                                    $compile(
                                        $(div).html()
                                    )($scope)
                                );
                                $scope.textBoxValues[task.id] = $scope.textBoxValues[task.id];
                                $scope.fteCosts[task.id] = Array.apply(null, new Array($scope.numberOfBoxes[task.id])).map(Number.prototype.valueOf, 0);
                                $scope.unitCosts[task.id] = Array.apply(null, new Array($scope.numberOfBoxes[task.id])).map(Number.prototype.valueOf, 0);
                                // $scope.unitBudget[task.id] = Array.apply(null, new Array($scope.numberOfBoxes[task.id])).map(Number.prototype.valueOf, 0);
                                $scope.fteHours[task.id] = Array.apply(null, new Array($scope.numberOfBoxes[task.id])).map(Number.prototype.valueOf, 0);
                                if ($scope.isCostEdited[task.id] == true) {
                                    $scope.costGanttInstance.callEvent('customClick', [task.id]);
                                }
                            }
                            else {
                                console.log('test1');
                                $scope.textBoxIds[task.id] = Array.apply(null, new Array($scope.numberOfBoxes[task.id])).map(Number.prototype.valueOf, 0);
                                $scope.textBoxValues[task.id] = Array.apply(null, new Array($scope.numberOfBoxes[task.id])).map(Number.prototype.valueOf, 0);
                                console.log('test2');
                                //for(var i = 0; i < $scope.textBoxValues[task.id].length - 1; i++){
                                //    $scope.textBoxIds[task.id][i] = i;
                                //}
                                $scope.fteCosts[task.id] = Array.apply(null, new Array($scope.numberOfBoxes[task.id])).map(Number.prototype.valueOf, 0);
                                $scope.unitCosts[task.id] = Array.apply(null, new Array($scope.numberOfBoxes[task.id])).map(Number.prototype.valueOf, 0);
                                // $scope.unitBudget[task.id] = Array.apply(null, new Array($scope.numberOfBoxes[task.id])).map(Number.prototype.valueOf, 0);
                                $scope.fteHours[task.id] = Array.apply(null, new Array($scope.numberOfBoxes[task.id])).map(Number.prototype.valueOf, 0);
                                if ($scope.isCostEdited[task.id] == true) {
                                    $scope.costGanttInstance.callEvent('customClick', [task.id]);
                                }
                            }
                        }

                    }
                    else if ($scope.scheduleScale == 'year') {

                        if (OnlyOneTaskIdInstance == task.id) { //PRitesh: This condition is used since the task id are repeating it get click twice
                            OnlyOneTaskIdInstance++;
                            if ($scope.currentCostIndex == (task.id - 1)) {
                                $scope.textBoxIds[task.id] = [];
                            }
                            var div = $("." + task.id + "_cost");
                            var div = $("." + task.id + "_cost");
                            $(div).html(
                                $compile(
                                    $(div).html()
                                )($scope)
                            );

                            // Pritesh New Logic Start added on 2 july 2020
                            if ($scope.textBoxIds[task.id]) {
                                var dateFormatForLoop = "MM/DD/YYYY";

                                var StartFormatLoop = moment(task.start_date).format(dateFormatForLoop);
                                var EndFormatLoop = moment(task.end_date).format(dateFormatForLoop);
                                // alert(StartFormatLoop);

                                var start = new Date(StartFormatLoop);  //  new Date("05/25/2020"); // Month date year
                                var end = new Date(EndFormatLoop); // new Date("06/21/2020");
                                // alert(start);
                                var StartDateOfEachWeek = [];
                                //  daysOfYear.push(start);
                                var loop = new Date(start);

                                while (loop < end) {

                                    var d = new Date(loop),
                                        month = '' + (d.getMonth() + 1),
                                        day = '' + d.getDate(),
                                        year = d.getFullYear();

                                    if (month.length < 2)
                                        month = '0' + month;
                                    if (day.length < 2)
                                        day = '0' + day;

                                    // var FormateLoop = [year, month, day].join('-');
                                    var FormateLoop = [year].join('-');
                                    StartDateOfEachWeek.push(FormateLoop);
                                    var newDate = loop.setFullYear(loop.getFullYear() + 1); // loop.setMonth(loop.getMonth() + 1);
                                    loop = new Date(newDate);
                                }
                                //  alert(StartDateOfEachWeek);
                                var TaskStartDate = [];
                                var SD = [];
                                var FormatTaskStart = task.individual_start_date;
                                if (FormatTaskStart)
                                    SD = FormatTaskStart.split(",");
                                var StringStr = "";

                                for (var k = 0; k < SD.length; k++) {
                                    var dateFrom = SD[k].replace(" 00:00:00", "");
                                    var partsFrom = dateFrom.split('-');
                                    TaskStartDate.push(partsFrom[0]);
                                    // TaskStartDate.push(SD[k].replace(" 00:00:00", ""));
                                }
                                // alert("Phase year : " + StartDateOfEachWeek + "  ---- Task YEar : " + TaskStartDate);
                                var TxtValCount = 0;
                                var TextBoxValuesArray = [];
                                var TxtValCountWhenIndividualStartISNull = 0;
                                for (var i = 0; i < StartDateOfEachWeek.length; i++) { // Start Week Dates For Main Phase with comma Seperated Array
                                    var InnerLoopRecordNotMatch = 0;
                                    for (var k = 0; k < TaskStartDate.length; k++) {
                                        var d1 = new Date(StartDateOfEachWeek[i].trim());
                                        var d2 = new Date(TaskStartDate[k].trim());

                                        if (StartDateOfEachWeek[i].trim() === TaskStartDate[k].trim()) { //  (d1.getTime() === d2.getTime())
                                            var numFormat = parseFloat($scope.textBoxValues[task.id][TxtValCount]);
                                            TextBoxValuesArray.push(numFormat.toFixed(2));
                                            // TextBoxValuesArray.push(parseFloat($scope.textBoxValues[task.id][TxtValCount]));
                                            TxtValCount++;
                                            InnerLoopRecordNotMatch++;
                                            break;
                                        } else {
                                            InnerLoopRecordNotMatch = 0;

                                        }
                                    }
                                    if (InnerLoopRecordNotMatch == 0) {
                                        if (TaskStartDate.length == 0) {
                                            var numFormat = parseFloat($scope.textBoxValues[task.id][TxtValCountWhenIndividualStartISNull]);
                                            TextBoxValuesArray.push(numFormat.toFixed(2));
                                            // TextBoxValuesArray.push($scope.textBoxValues[task.id][TxtValCountWhenIndividualStartISNull]);
                                            TxtValCountWhenIndividualStartISNull++;
                                        }
                                        else {
                                            TextBoxValuesArray.push('0.00');
                                        }
                                        // TextBoxValuesArray.push('0');
                                    }
                                }
                                $scope.textBoxValues[task.id] = TextBoxValuesArray;


                                $scope.fteCosts[task.id] = Array.apply(null, new Array($scope.numberOfBoxes[task.id])).map(Number.prototype.valueOf, 0);
                                $scope.unitCosts[task.id] = Array.apply(null, new Array($scope.numberOfBoxes[task.id])).map(Number.prototype.valueOf, 0);
                                // $scope.unitBudget[task.id] = Array.apply(null, new Array($scope.numberOfBoxes[task.id])).map(Number.prototype.valueOf, 0);
                                $scope.fteHours[task.id] = Array.apply(null, new Array($scope.numberOfBoxes[task.id])).map(Number.prototype.valueOf, 0);
                                if ($scope.isCostEdited[task.id] == true) {
                                    $scope.costGanttInstance.callEvent('customClick', [task.id]);
                                }
                            }
                            else {
                                console.log('test1');
                                $scope.textBoxIds[task.id] = Array.apply(null, new Array($scope.numberOfBoxes[task.id])).map(Number.prototype.valueOf, 0);
                                $scope.textBoxValues[task.id] = Array.apply(null, new Array($scope.numberOfBoxes[task.id])).map(Number.prototype.valueOf, 0);
                                console.log('test2');
                                //for(var i = 0; i < $scope.textBoxValues[task.id].length - 1; i++){
                                //    $scope.textBoxIds[task.id][i] = i;
                                //}
                                $scope.fteCosts[task.id] = Array.apply(null, new Array($scope.numberOfBoxes[task.id])).map(Number.prototype.valueOf, 0);
                                $scope.unitCosts[task.id] = Array.apply(null, new Array($scope.numberOfBoxes[task.id])).map(Number.prototype.valueOf, 0);
                                // $scope.unitBudget[task.id] = Array.apply(null, new Array($scope.numberOfBoxes[task.id])).map(Number.prototype.valueOf, 0);
                                $scope.fteHours[task.id] = Array.apply(null, new Array($scope.numberOfBoxes[task.id])).map(Number.prototype.valueOf, 0);
                                if ($scope.isCostEdited[task.id] == true) {
                                    $scope.costGanttInstance.callEvent('customClick', [task.id]);
                                }
                            }
                        }
                        else {
                            if ($scope.textBoxIds[task.id]) {
                                if ($scope.currentCostIndex == (task.id - 1)) {
                                    $scope.textBoxIds[task.id] = [];
                                }
                                var div = $("." + task.id + "_cost");
                                var div = $("." + task.id + "_cost");
                                $(div).html(
                                    $compile(
                                        $(div).html()
                                    )($scope)
                                );
                                $scope.textBoxValues[task.id] = $scope.textBoxValues[task.id];
                                $scope.fteCosts[task.id] = Array.apply(null, new Array($scope.numberOfBoxes[task.id])).map(Number.prototype.valueOf, 0);
                                $scope.unitCosts[task.id] = Array.apply(null, new Array($scope.numberOfBoxes[task.id])).map(Number.prototype.valueOf, 0);
                                // $scope.unitBudget[task.id] = Array.apply(null, new Array($scope.numberOfBoxes[task.id])).map(Number.prototype.valueOf, 0);
                                $scope.fteHours[task.id] = Array.apply(null, new Array($scope.numberOfBoxes[task.id])).map(Number.prototype.valueOf, 0);
                                if ($scope.isCostEdited[task.id] == true) {
                                    $scope.costGanttInstance.callEvent('customClick', [task.id]);
                                }
                            }
                            else {
                                console.log('test1');
                                $scope.textBoxIds[task.id] = Array.apply(null, new Array($scope.numberOfBoxes[task.id])).map(Number.prototype.valueOf, 0);
                                $scope.textBoxValues[task.id] = Array.apply(null, new Array($scope.numberOfBoxes[task.id])).map(Number.prototype.valueOf, 0);
                                console.log('test2');
                                //for(var i = 0; i < $scope.textBoxValues[task.id].length - 1; i++){
                                //    $scope.textBoxIds[task.id][i] = i;
                                //}
                                $scope.fteCosts[task.id] = Array.apply(null, new Array($scope.numberOfBoxes[task.id])).map(Number.prototype.valueOf, 0);
                                $scope.unitCosts[task.id] = Array.apply(null, new Array($scope.numberOfBoxes[task.id])).map(Number.prototype.valueOf, 0);
                                // $scope.unitBudget[task.id] = Array.apply(null, new Array($scope.numberOfBoxes[task.id])).map(Number.prototype.valueOf, 0);
                                $scope.fteHours[task.id] = Array.apply(null, new Array($scope.numberOfBoxes[task.id])).map(Number.prototype.valueOf, 0);
                                if ($scope.isCostEdited[task.id] == true) {
                                    $scope.costGanttInstance.callEvent('customClick', [task.id]);
                                }
                            }
                        }

                    }



                    setTimeout(function () { OnlyOneTaskIdInstance = 1; }, 500);
                });

                //$scope.deleteFromNew = false;
                //  OnlyOneTaskIdInstance = 1;
                return true;

            });
            ////////------------------------ Pritesh New Logic  End added on 2 Jul 2020------------------------------------------


            /////// -------------------Pritesh commented old logic on 2 july 2020 only kept for reference if it get affected anywhere ------------------------
            //$scope.costGanttInstance.attachEvent("onGanttRender", function () {
            //    $scope.isScaleChanged = true;

            //    $scope.costGanttInstance.eachTask(function (task) {
            //        if ($scope.currentCostIndex == (task.id - 1)) {
            //            $scope.textBoxIds[task.id] = [];
            //        }
            //        var div = $("." + task.id + "_cost");
            //        var div = $("." + task.id + "_cost");
            //        $(div).html(
            //            $compile(
            //                $(div).html()
            //            )($scope)
            //        );
            //        if ($scope.textBoxIds[task.id]) {
            //            // alert("Task ID : "+task.id+ " ----  Number Of Boxes : " + $scope.numberOfBoxes[task.id]+" ---  Prottype No Val : " + Number.prototype.valueOf);
            //            var textBoxVals = Array.apply(null, new Array($scope.numberOfBoxes[task.id])).map(Number.prototype.valueOf, 0);
            //            // alert("Array Apply : " + textBoxVals);
            //            for (var i = 0; i < $scope.textBoxIds[task.id].length; i++) {
            //                var textBoxid = parseInt($scope.textBoxIds[task.id][i]);

            //                textBoxVals[textBoxid] = parseFloat($scope.textBoxValues[task.id][i]);
            //            }
            //            $scope.textBoxValues[task.id] = textBoxVals;
            //        }
            //        else {
            //            console.log('test1');
            //            $scope.textBoxIds[task.id] = Array.apply(null, new Array($scope.numberOfBoxes[task.id])).map(Number.prototype.valueOf, 0);
            //            $scope.textBoxValues[task.id] = Array.apply(null, new Array($scope.numberOfBoxes[task.id])).map(Number.prototype.valueOf, 0);
            //            console.log('test2');
            //            //for(var i = 0; i < $scope.textBoxValues[task.id].length - 1; i++){
            //            //    $scope.textBoxIds[task.id][i] = i;
            //            //}
            //        }

            //        $scope.fteCosts[task.id] = Array.apply(null, new Array($scope.numberOfBoxes[task.id])).map(Number.prototype.valueOf, 0);
            //        $scope.unitCosts[task.id] = Array.apply(null, new Array($scope.numberOfBoxes[task.id])).map(Number.prototype.valueOf, 0);
            //        // $scope.unitBudget[task.id] = Array.apply(null, new Array($scope.numberOfBoxes[task.id])).map(Number.prototype.valueOf, 0);
            //        $scope.fteHours[task.id] = Array.apply(null, new Array($scope.numberOfBoxes[task.id])).map(Number.prototype.valueOf, 0);
            //        if ($scope.isCostEdited[task.id] == true) {
            //            $scope.costGanttInstance.callEvent('customClick', [task.id]);
            //        }
            //    });

            //    //$scope.deleteFromNew = false;
            //    return true;
            //});

            //// -------------------- Pritesh old logic End commented on 2 july 2020

            $scope.currentScale = 'week';

            $scope.setWeek = function () {
                currentId = -1;
                $scope.currentScale = angular.copy($scope.scheduleScale);
                $scope.scheduleScale = 'week';
                $('#weekBtn').tooltip({ title: "Week", placement: "bottom" }).tooltip('hide');  //Manasi
                $('#weekBtn').removeClass('hover'); //Manasi
            }

            $scope.setMonth = function () {
                currentId = -1;
                $scope.currentScale = angular.copy($scope.scheduleScale);
                $scope.scheduleScale = 'month';
                $('#monthBtn').tooltip({ title: "Week", placement: "bottom" }).tooltip('hide');  //Manasi
                $('#monthBtn').removeClass('hover'); //Manasi

            }

            $scope.setYear = function () {
                currentId = -1;
                $scope.currentScale = angular.copy($scope.scheduleScale);
                $scope.scheduleScale = 'year';
                $('#yearBtn').tooltip({ title: "Week", placement: "bottom" }).tooltip('hide');  //Manasi
                $('#yearBtn').removeClass('hover');  //Manasi
            }

            $scope.costGanttInstance.attachEvent("onAfterTaskUpdate", function (id, item) {
                if ($scope.isMonthSchedule === false) {
                    console.log($scope.isMonthSchedule);

                    $scope.textBoxIds[id] = [];
                    var count = $scope.textBoxValues[id].length - 1;
                    for (var i = $scope.textBoxValues[id].length - 1; i >= 0; i--) {
                        if ($scope.textBoxValues[id][i] == 0) {
                            $scope.textBoxValues[id].splice(i, 1);
                        }
                        else {
                            $scope.textBoxIds[id].push(count);
                        }
                        count--;
                    }

                    $scope.textBoxIds[id] = $scope.textBoxIds[id].reverse();

                    $scope.costGanttInstance.eachTask(function (task) {
                        var div = $("." + task.id + "_cost");

                        $(div).html(
                            $compile(
                                $(div).html()
                            )($scope)
                        );

                        //var textBoxes = $(div).children();

                        var textBoxVals = Array.apply(null, new Array($scope.numberOfBoxes[task.id])).map(Number.prototype.valueOf, 0);
                        // alert(textBoxVals);
                        //HACK
                        if ($scope.textBoxIds[task.id].length != $scope.textBoxValues[task.id].length) {
                            var counter = $scope.textBoxIds[task.id][0];
                            if (counter != undefined && counter != null) { //luan here - an infinite loop was here - crash here
                                for (i = counter - 1; i != -1; i--) {
                                    $scope.textBoxIds[task.id].splice(0, 0, i);
                                }
                            }
                        }
                        for (var i = 0; i < $scope.textBoxIds[task.id].length; i++) {
                            var textBoxid = parseInt($scope.textBoxIds[task.id][i]);
                            textBoxVals[textBoxid] = parseFloat($scope.textBoxValues[task.id][i]);
                            //$scope.textBoxValues[task.id][i] = parseFloat($scope.textBoxValues[task.id][i]);    //luan here
                        }

                        $scope.textBoxValues[task.id] = textBoxVals.slice();    //luan here
                        $scope.fteCosts[task.id] = Array.apply(null, new Array($scope.numberOfBoxes[task.id])).map(Number.prototype.valueOf, 0);
                        $scope.fteHours[task.id] = Array.apply(null, new Array($scope.numberOfBoxes[task.id])).map(Number.prototype.valueOf, 0);

                    });
                }
                return true;
            });

            $scope.fromScale = "";

            $scope.$watch('schedulePhase', function () {
                console.log($scope.schedulePhase);


                $scope.allCostTotal = 0; //original total before any project is selected;
                var amountInput = delayedData[2].result[0].Amount;
                $scope.amount = amountInput - $scope.allCostTotal;

                paddingLabel();
                $scope.scheduleGanttInstance.render();
                //$scope.costGanttInstance.render();
            });

            var orgScale = angular.copy($scope.scheduleScale);

            $scope.$watch('scheduleScale', function (newValue, oldValue) {
                console.log('vertical testing', angular.copy($scope.description));
                //keep old scale if cost data is edited
                switch ($scope.scheduleScale) {
                    case "week":
                        $scope.costGanttInstance.clearAll();
                        $("#weekBtn").css("background-color", "gray");
                        $("#monthBtn").css("background-color", "black");
                        $("#yearBtn").css("background-color", "black");

                        if ($scope.selectedActivity) {
                            if ($scope.selectedActivity.text != "Add") {
                                $scope.scheduleGanttInstance.callEvent('onTaskSelected', [$scope.selectedActivity.id]);
                                $scope.scheduleGanttInstance.eachTask(function (task) {
                                    if (task.type !== "project") {
                                        task.start_date = moment(task.original_start_date, dateFormat).toDate();
                                        task.end_date = moment(task.original_end_date, dateFormat).toDate();
                                        $scope.scheduleGanttInstance.updateTask(Number(task.id));
                                    }
                                });
                                $scope.costGanttInstance.eachTask(function (task) {
                                    if (task.type !== "project") {
                                        task.start_date = moment(task.original_start_date, dateFormat).toDate();
                                        task.end_date = moment(task.original_end_date, dateFormat).toDate();
                                        $scope.costGanttInstance.updateTask(Number(task.id));
                                    }
                                });
                            }
                        }
                        var weekScaleTemplate = function (date) {
                            var dateToStr = $scope.scheduleGanttInstance.date.date_to_str("%d %M");
                            var endDate = $scope.scheduleGanttInstance.date.add($scope.scheduleGanttInstance.date.add(date, 1, "week"), -1, "day");
                            return dateToStr(date) + " - " + dateToStr(endDate);
                        };

                        $scope.scheduleGanttInstance.config.scale_unit = "month";
                        $scope.scheduleGanttInstance.config.step = 1;
                        $scope.scheduleGanttInstance.config.date_scale = "%F %Y";

                        $scope.scheduleGanttInstance.config.subscales = [
                            { unit: "week", step: 1, template: weekScaleTemplate }
                        ];
                        $scope.scheduleGanttInstance.config.scale_height = 50;
                        $scope.scheduleGanttInstance.render();
                        $scope.costGanttInstance.config.scale_unit = "month";
                        $scope.costGanttInstance.config.step = 1;
                        $scope.costGanttInstance.config.date_scale = "%F %Y";
                        $scope.costGanttInstance.config.subscales = [
                            { unit: "week", step: 1, template: weekScaleTemplate }
                        ];
                        $scope.costGanttInstance.config.scale_height = 50;
                        $scope.costGanttInstance.config.start_date = $scope.scheduleGanttInstance.getState().min_date;
                        $scope.costGanttInstance.config.end_date = $scope.scheduleGanttInstance.getState().max_date;
                        $scope.costGanttInstance.render();
                        //setTimeout(function () { refreshHtmlDelete(); }, 1000);
                        $scope.isScaleChanged = true;
                        break;
                    case "month":
                        $scope.costGanttInstance.clearAll();
                        $("#weekBtn").css("background-color", "black");
                        $("#monthBtn").css("background-color", "gray");
                        $("#yearBtn").css("background-color", "black");

                        //$scope.costGanttInstance.clearAll();
                        if ($scope.selectedActivity) {
                            if ($scope.selectedActivity.text != "Add") {
                                console.log('special test 2/12', $scope.selectedActivity);
                                $scope.scheduleGanttInstance.callEvent('onTaskSelected', [$scope.selectedActivity.id]);
                                $scope.scheduleGanttInstance.eachTask(function (task) {
                                    if (task.type !== "project") {
                                        task.start_date = moment(task.original_start_date, dateFormat).toDate();
                                        task.end_date = moment(task.original_end_date, dateFormat).toDate();
                                        $scope.scheduleGanttInstance.updateTask(Number(task.id));
                                    }
                                });
                                $scope.costGanttInstance.eachTask(function (task) {
                                    if (task.type !== "project") {
                                        task.start_date = moment(task.original_start_date, dateFormat).toDate();
                                        task.end_date = moment(task.original_end_date, dateFormat).toDate();
                                        $scope.costGanttInstance.updateTask(Number(task.id));
                                    }
                                });
                            }
                        }
                        $scope.scheduleGanttInstance.config.scale_unit = "year";
                        $scope.scheduleGanttInstance.config.step = 1;
                        $scope.scheduleGanttInstance.config.date_scale = "%Y";
                        $scope.scheduleGanttInstance.config.scale_height = 50;
                        $scope.scheduleGanttInstance.templates.date_scale = null;


                        $scope.scheduleGanttInstance.config.subscales = [
                            { unit: "month", step: 1, date: "%M" }
                        ];
                        $scope.scheduleGanttInstance.render();


                        $scope.costGanttInstance.config.scale_unit = "year";
                        $scope.costGanttInstance.config.step = 1;
                        $scope.costGanttInstance.config.date_scale = "%Y";
                        $scope.costGanttInstance.config.scale_height = 50;
                        $scope.costGanttInstance.templates.date_scale = null;


                        $scope.costGanttInstance.config.subscales = [
                            { unit: "month", step: 1, date: "%F" }
                        ];
                        $scope.costGanttInstance.config.start_date = $scope.scheduleGanttInstance.getState().min_date;
                        $scope.costGanttInstance.config.end_date = $scope.scheduleGanttInstance.getState().max_date;
                        $scope.costGanttInstance.render();
                        //setTimeout(function () { refreshHtmlDelete(); }, 1000);
                        $scope.isScaleChanged = true;


                        break;
                    case "year":
                        $scope.costGanttInstance.clearAll();
                        $("#weekBtn").css("background-color", "black");
                        $("#monthBtn").css("background-color", "black");
                        $("#yearBtn").css("background-color", "gray");
                        if ($scope.selectedActivity) {
                            if ($scope.selectedActivity.text != "Add") {
                                console.log('special test 2/12', $scope.selectedActivity);
                                $scope.scheduleGanttInstance.callEvent('onTaskSelected', [$scope.selectedActivity.id]);
                                $scope.scheduleGanttInstance.eachTask(function (task) {
                                    if (task.type !== "project") {
                                        task.start_date = moment(task.original_start_date, dateFormat).toDate();
                                        task.end_date = moment(task.original_end_date, dateFormat).toDate();
                                        $scope.scheduleGanttInstance.updateTask(Number(task.id));
                                    }
                                });
                                $scope.costGanttInstance.eachTask(function (task) {
                                    if (task.type !== "project") {
                                        task.start_date = moment(task.original_start_date, dateFormat).toDate();
                                        task.end_date = moment(task.original_end_date, dateFormat).toDate();
                                        $scope.costGanttInstance.updateTask(Number(task.id));
                                    }
                                });
                            }
                        }
                        $scope.scheduleGanttInstance.config.scale_unit = "year";
                        $scope.scheduleGanttInstance.config.step = 1;
                        $scope.scheduleGanttInstance.config.date_scale = "%Y";
                        $scope.scheduleGanttInstance.config.subscales = [];
                        $scope.scheduleGanttInstance.config.scale_height = 25;

                        $scope.scheduleGanttInstance.render();
                        $scope.costGanttInstance.config.scale_unit = "year";
                        $scope.costGanttInstance.config.step = 1;
                        $scope.costGanttInstance.config.date_scale = "%Y";
                        $scope.costGanttInstance.config.subscales = [];
                        $scope.costGanttInstance.config.scale_height = 25;

                        $scope.costGanttInstance.config.start_date = $scope.scheduleGanttInstance.getState().min_date;
                        $scope.costGanttInstance.config.end_date = $scope.scheduleGanttInstance.getState().max_date;
                        $scope.costGanttInstance.render();
                        //setTimeout(function () { refreshHtmlDelete(); }, 1000);
                        $scope.isScaleChanged = true;

                        break;
                }
                console.log('vertical testing', angular.copy($scope.description));
            });

            //init gantt
            $scope.scheduleGanttInstance.init("schedule-gantt");
            $scope.scheduleScale = "week";
            $scope.scheduleGanttInstance.parse({ data: $scope.schedule.data, links: [] });

            //updatePhase();
            $scope.costGanttInstance.init("cost-gantt");   //06-01-2021
            var currentId;
            $scope.currentCostActivityID;

            //watch data collection, reload on changes
            $scope.$watch("schedule", function (collection) {
                $scope.scheduleGanttInstance.clearAll();
                setTimeout(function () {
                    $scope.scheduleGanttInstance.parse(collection, "json");
                }, 0);
            }, true);

            $(document).ready(function () {
                $($scope.scheduleGanttInstance.$task).on('scroll', function () {
                    $scope.costGanttInstance.scrollTo($($scope.scheduleGanttInstance.$task).scrollLeft(), null);
                });
                $($scope.costGanttInstance.$task).on('scroll', function () {
                    console.log('cost-gantt scroll');
                    $scope.scheduleGanttInstance.scrollTo($($scope.costGanttInstance.$task).scrollLeft(), null);
                });

            });

            $scope.taskBeingEdited = false;

            // getCurrentDate();
            $scope.costGanttInstance.attachEvent("onTaskDblClick", function (id) {
            });

            $scope.costGanttInstance.attachEvent('onTaskClick', function (id, e) {
                //App Security ModifyProject: 5
                console.log(e);
                //luan here 4/24
                $scope.selectedCost = $scope.costGanttInstance.getTask(id);
                var auth = $scope.localStorageSrevice.get('authorizationData');
                if (auth.acl[9] == 0) { // Pritesh on 4 Aug 2020 Change the acl[5] to acl[9] as per DB entry
                    dhtmlx.alert("You don't have access to edit activity.");
                    return;
                }

                //App Security ModifyTrend: 7
                if (auth.acl[7] == 0 && ($scope.trend.TrendDescription != 'Baseline' && $scope.trend.TrendDescription != 'Current' && $scope.trend.TrendDescription != 'Forecast')) {
                    dhtmlx.alert("You don't have edit trend permission.");
                    return;
                }

                var currentSelectedCost = $scope.costGanttInstance.getTask(id);
                if ($scope.trend.TrendStatusID == 1 && currentSelectedCost.cost_track_type_id != $scope.costTrackType.ACTUAL && currentSelectedCost.cost_track_type_id != $scope.costTrackType.ESTIMATE_TO_COMPLETION) {
                    return;
                }

                if ($scope.trend.TrendStatusID == 1 && currentSelectedCost.cost_track_type_id != $scope.costTrackType.ACTUAL && currentSelectedCost.cost_track_type_id != $scope.costTrackType.ESTIMATE_TO_COMPLETION) {
                    return;
                }
                // swapnil 02-10-2020
                if ($scope.trend.TrendStatusID === 3 && $scope.trend.approvedList_EmployeeID != "" && $scope.trend.approvedList_EmployeeID != null && $scope.trend.approvedList_EmployeeID != "0") {
                    return;
                }
                //-----------------------------------
                if ($scope.trend.TrendNumber == 2000) {
                    return;
                }

                if ($scope.deleteFromNew === true || $scope.isCostEdited[id] === true) {
                    $scope.isCostCancel = false;
                    if ($scope.deleteFromNew === true) {
                        $scope.deleteFromNew = false;
                    }
                    currentId = -1;
                }
                else {
                    debugger
                    $scope.isCostEdited[id] = true;
                    var clickResult = $scope.costGanttInstance.callEvent('customClick', [id]);
                    if (clickResult == true) {
                        $scope.isCostEdited[id] = true;
                        var cost = $scope.costGanttInstance.getTask(id);
                        updateBuffer(cost);
                    }
                }
            });

            $scope.costGanttInstance.attachEvent("onAftertTaskDelete", function () {
            });
            $scope.deleteNewCost = function (id) {

                $scope.deleteFromNew = true;
                $scope.testt = true;
                currentId = -1;
                if (id == $scope.costs.data.length) {
                    $scope.tempTextBoxValues = angular.copy($scope.textBoxValues);
                    $scope.isCostEdited.splice(id, 1);
                    $scope.costGanttInstance.clearAll();
                    $scope.costGanttInstance.parse({ data: $scope.costs.data, links: [] });
                    //$scope.allCostTotal = $scope.selectedActivity.totalCost;
                    $scope.allCostTotal = $scope.selectedActivity.totalBudget;     //Manasi 15-07-2020
                    var amountInput = delayedData[2].result[0].Amount;
                    $scope.amount = amountInput - $scope.allCostTotal;

                    paddingLabel();

                    updateBuffer($scope.selectedCost);

                } else {
                    //if method is selected
                    $scope.costs.data.splice(id - 1, 1);//1
                    spliceCost(id);
                    $scope.tempTextBoxValues = angular.copy($scope.textBoxValues);
                    adjustCostId(id, $scope.costs.data);
                    removeSavedCostFromBuffer(id);
                    $scope.currentCostIndex--;

                    updateBuffer($scope.selectedCost);
                    $scope.costGanttInstance.clearAll();
                    //angular.forEach($scope.costs.data, function (item) {
                    //    item.start_date = moment(item.start_date).format(dateFormat);
                    //    item.end_date = moment(item.end_date).format(dateFormat);
                    //})
                    $scope.costGanttInstance.parse({ data: $scope.costs.data, links: [] });
                    $scope.costGanttInstance.render();
                    //setTimeout(function () { refreshHtmlDelete(); }, 1000);
                    //$scope.allCostTotal = $scope.selectedActivity.totalCost;
                    $scope.allCostTotal = $scope.selectedActivity.totalBudget;      //Manasi 15-07-2020
                    var amountInput = delayedData[2].result[0].Amount;
                    $scope.amount = amountInput - $scope.allCostTotal;
                    paddingLabel();
                }
            }
            $scope.deleteCost = function (id) {
                var costToDelete = [];
                if ($scope.duc = true) {
                    $scope.retrievedActivityID = $scope.selectedCost.activity;
                }
                var span = $(this).children(0).children(0);
                span.css('color', 'red');
                span.removeClass('notClickableFont').addClass('clickableFont');





                dhtmlx.confirm({
                    title: "Delete Confirmation",
                    type: "confirm-error",
                    text: "Are you sure you want to delete?",
                    callback: function (response) {
                        if (response) { //confirmed to delete
                            var FTECostID = [];
                            for (var j = 0; j < $scope.textBoxValues[$scope.selectedCost.id].length; j++) {
                                FTECostID.push($scope.selectedCost.activity + "_" + $scope.selectedCost.cost_id + "_" + j);

                            }
                            if ($scope.isSave === true && $scope.isNewCost[id] === false) {  //Delete Saved tasks

                                //activateSpinner();
                                var cost = {
                                    "Operation": "3",
                                    "ProgramID": $scope.selectedCost.program,
                                    "ProgramElementID": $scope.selectedCost.program_element,
                                    "ProjectID": $scope.selectedCost.project,
                                    "TrendNumber": $scope.selectedCost.trend,
                                    "ActivityID": $scope.selectedCost.activity,
                                    "CostID": $scope.selectedCost.cost_id,
                                    "CostType": $scope.method[$scope.selectedCost.id],
                                    "Description": "",
                                    "Scale": $scope.scheduleScale,
                                    "StartDate": "",
                                    "EndDate": "",
                                    "TextBoxValue": "",
                                    "Base": "",
                                    "FTEHours": "",
                                    "FTECost": "",
                                    "FTEIDList": FTECostID.join(',')
                                };
                                costToDelete.push(cost);
                                InsertCost.save(costToDelete, function (response) {
                                    $scope.costs.data.splice(id - 1, 1);
                                    currentId = -1;
                                    spliceCost(id);
                                    adjustCostId(id, $scope.costs.data);
                                    $scope.tempTextBoxValues = angular.copy($scope.textBoxValues);
                                    $scope.currentCostIndex--;
                                    $scope.test = false;
                                    $scope.deleteFromNew = false;
                                    removeSavedCostFromBuffer(id);
                                    $scope.costGanttInstance.clearAll();
                                    //angular.forEach($scope.costs.data, function (item) {
                                    //    item.start_date = moment(item.start_date).format(dateFormat);
                                    //    item.end_date = moment(item.end_date).format(dateFormat);
                                    //})
                                    $scope.costGanttInstance.parse({ data: $scope.costs.data, links: [] });
                                    $scope.costGanttInstance.render();
                                    //setTimeout(function () { refreshHtmlDelete(); }, 1000);
                                    //getProjectDurationAndCost();
                                    updateTrendValue($scope.selectedActivity.id); //update Trend value
                                    calculateTrendValue();//update Trend Value)
                                    //deactivateSpinner();

                                });
                            }
                            else {  //Delete unsaved tasks
                                $scope.deleteNewCost(id);
                            }


                            $scope.taskBeingEdited = false;
                        } else {
                            return;
                        }
                    }
                });



                if ($scope.duc != true) {
                    $scope.scheduleGanttInstance.selectTask($scope.selectedActivity.id);
                }
            }
            function isFloat(n) {
                return Number(n) === n && n % 1 !== 0;
            }
            $scope.getCostWithOverhead = function (costAmount, costType) {
                // priteshcc1 used for Cost over head unit cost calculation
                // alert("Labor : " + CUSTOM_LABOR_RATE + " -- SubContract : " + CUSTOM_SUBCONTRACTOR_RATE + "--ODC : " + CUSTOM_ODC_RATE + "-- Material : " + CUSTOM_MATERIAL_RATE);
                var costWithOverhead = 1;
                if (costAmount == "NaN") {
                    costAmount = 0;
                }
                else {
                    costAmount = parseFloat(costAmount);
                }
                switch (costType) {
                    case 'F':
                        //costWithOverhead = ($scope.isBillableRate) ? costAmount   : costAmount * CUSTOM_LABOR_RATE;
                        costWithOverhead = costAmount * CUSTOM_LABOR_RATE;
                        break;
                    case 'L':
                        costWithOverhead = (SUBCONTRACTOR_RATE != 0 && CUSTOM_SUBCONTRACTOR_RATE != 0)
                            ? costAmount * CUSTOM_SUBCONTRACTOR_RATE : costAmount;
                        break;
                    case 'ODC':
                        costWithOverhead = (ODC_RATE != 0 && CUSTOM_ODC_RATE != 0)
                            ? costAmount * CUSTOM_ODC_RATE : costAmount;
                        break;
                    case 'U':
                        costWithOverhead = (MATERIAL_RATE != 0 && CUSTOM_MATERIAL_RATE != 0)
                            ? costAmount * CUSTOM_MATERIAL_RATE : costAmount;
                        break;
                    default:
                        costWithOverhead = costAmount;
                }

                return (isFloat(costWithOverhead)) ? roundToTwo(costWithOverhead) : costWithOverhead; //return 2 decimals if float
            }
            $scope.costDetails = function () {
                //  cost_id
                var scope = $rootScope.$new();
                scope.params = {}
                scope.params = {
                    ActivityID: $scope.selectedCost.activity,
                    CostType: $scope.selectedCost.cost_type,
                    Granularity: $scope.scheduleScale,
                    SelectedCost: $scope.selectedCost,
                    //LineID: $scope.selectedCost.cost_track_type_id,
                    LineID: $scope.selectedCost.CostLineItemID,
                    isBillableRate: ($scope.trend.CostOverheadTypeID == 1) || false,
                    Type: $scope.description[$scope.selectedCost.id],
                    Name: $scope.employee_id[$scope.selectedCost.id]
                        || $scope.material_id[$scope.selectedCost.id]
                        || $scope.subcontractor_id[$scope.selectedCost.id],
                    selectedActivity: $scope.selectedActivity,
                    trend: $scope.trend

                }
                if ($scope.selectedCost.cost_type == "F") {
                    scope.params.CUSTOM_OVERHEAD_RATE = CUSTOM_LABOR_RATE;
                    scope.params.OVERHEAD_RATE = LABOR_RATE;
                } else if ($scope.selectedCost.cost_type == "L") {
                    scope.params.CUSTOM_OVERHEAD_RATE = CUSTOM_SUBCONTRACTOR_RATE;
                    scope.params.OVERHEAD_RATE = SUBCONTRACTOR_RATE;
                } else if ($scope.selectedCost.cost_type == "ODC") {
                    scope.params.CUSTOM_OVERHEAD_RATE = CUSTOM_ODC_RATE;
                    scope.params.OVERHEAD_RATE = ODC_RATE;
                } else if ($scope.selectedCost.cost_type = "U") {
                    scope.params.CUSTOM_OVERHEAD_RATE = CUSTOM_MATERIAL_RATE;
                    scope.params.OVERHEAD_RATE = MATERIAL_RATE;
                }
                console.log(scope.params);
                console.log($scope.employee_id[$scope.selectedCost.id]);
                $rootScope.modalInstance = $uibModal.open({
                    backdrop: 'static',
                    keyboard: false,
                    scope: scope,
                    templateUrl: "app/views/modal/cost_row_detail_modal.html",
                    size: "lg",
                    controller: "CostRowDetailCtrl"
                });
                $rootScope.modalInstance.result.then(function (response) {
                    console.log(response);
                    setTimeout(function () {
                        applyExpandables();
                    }, 1000);
                    if (response == 'close') {

                    } else {
                        //Update cost
                        $scope.totalCost[$scope.selectedCost.id] = 0;
                        $scope.totalBudget[$scope.selectedCost.id] = 0;
                        $scope.totalUnits[$scope.selectedCost.id] = 0;
                        angular.forEach(response, function (item, index) {
                            $scope.textBoxValues[$scope.selectedCost.id][index] = item.CellValue;
                            $scope.totalCost[$scope.selectedCost.id] += parseFloat(item.TotalCost);

                            $scope.totalUnits[$scope.selectedCost.id] += parseFloat(item.FTEHours * 8);
                        });
                        $scope.selectedCost.total_cost = $scope.totalCost[$scope.selectedCost.id];
                        $scope.selectedCost.total_units = $scope.totalUnits[$scope.selectedCost.id];


                        //$scope.cancelCost();
                        $scope.saveCost();   //Manasi 24-07-2020
                        updateTrendValue($scope.selectedActivity.id);

                        calculateTrendValue();
                        $timeout(function () {
                            var s = $('.gantt_row[task_id="1644"]');
                            $($('.gantt_row[task_id="' + $scope.selectedActivity.id + '"]')[0]).addClass('gantt_selected');
                            $($('.gantt_task_row[task_id="' + $scope.selectedActivity.id + '"]')[0]).addClass('gantt_selected');

                        }, 1000);

                    }
                }, function error(response) {
                    console.log(response);
                });

                //setTimeout(function () { refreshHtmlDelete(); }, 1000);

            }


            $scope.copyToClipboard = function () {
                copyToClipboard($scope.selectedCost.CostLineItemID);
            }
            function copyToClipboard(text) {
                var dummy = document.createElement("input");
                document.body.appendChild(dummy);
                dummy.setAttribute('value', text);
                dummy.select();
                document.execCommand("copy");
                document.body.removeChild(dummy);
            }
            $scope.cancelCostDetail = function (id) {
                // alert($scope.selectedCost.id);
                dhtmlx.confirm({
                    title: "Cancel Confirmation",
                    type: "confirm-error",
                    width: "300px",
                    text: "Are you sure you want to cancel your changes?",
                    callback: function (response) {
                        if (response) { //confirmed to delete
                            if ($scope.isNewCost[$scope.selectedCost.id] === false) {
                                $scope.cancelCost();
                            } else {
                                $scope.deleteNewCost($scope.selectedCost.id);
                            }
                        } else {
                            return;
                        }
                    }
                });


            }
            $scope.cancelCost = function () {
                $scope.isCostCancel = true;
                $scope.isCostEdited[$scope.selectedCost.id] = false;

                for (var i = 0; i < $scope.textBoxValues[$scope.selectedCost.id].length; i++) {
                    $scope.textBoxValues[$scope.selectedCost.id][i] = $scope.CopyTextBoxValues[$scope.selectedCost.id][i];
                }

                $scope.costGanttInstance.templates.task_text($scope.selectedCost.start_date, $scope.selectedCost.end_date, $scope.selectedCost);
                $scope.costGanttInstance.refreshTask($scope.selectedCost.id);
                $scope.costGanttInstance.render();
                //setTimeout(function () { refreshHtmlDelete(); }, 1000);

            }
            //customClick - BEGIN
            $scope.costGanttInstance.attachEvent("customClick", function (id) {
                // id = id + 1;
                debugger
                $scope.newEmployees[id] = [];
                $scope.newSubcontractors[id] = [];
                $scope.newMaterials[id] = [];
                $scope.newEmployees[id] = getNewEmployees(id);
                $scope.newSubcontractors[id] = getNewSubcontractors(id);
                $scope.newMaterials[id] = getNewMaterials(id);
                // $scope.CopyTextBoxValues = angular.copy($scope.TextBoxValue);
                var s = isCostsEdited();
                console.log($scope.description);
                console.log($scope.newSubcontractors, 2);
                console.log($scope.selectedActivity);
                var scaleChanged = ($scope.scheduleScale == $scope.fromScale) ? false : true;
                //luan quest 2/27
                if (s === true && $scope.testt !== true) {
                    currentId = -1;
                }

                if ($scope.selectedActivity && $scope.selectedActivity.$level < 2) {
                    //currentId = -1; //do not process if the selected activity is not a task
                    dhtmlx.alert("Please select a task to edit costs.")
                    $scope.isCostEdited = null;   //Manasi 21-08-2020
                    return;
                }
                //alert(id);
                //    $scope.selectedCost = $scope.costGanttInstance.getTask(id);

                if (currentId != id) {
                    if (scaleChanged && $scope.fromScale != "")
                        currentId = id;
                    if (id) {
                        $scope.taskBeingEdited = true;
                        // $scope.selectedCost = $scope.costGanttInstance.getTask(id);

                        if ($scope.scheduleScale == 'week') {
                            // $scope.totalUnits[id] = parseFloat($scope.totalUnits[id]) * 8; Pritesh on 15 Jul 2020 for making unit fixed
                            var UNitCalP = parseFloat($scope.totalUnits[id]) * 8;
                            $scope.totalUnits[id] = roundToTwo(UNitCalP);
                            console.log('total here');
                        }
                        console.log('totalunits test');

                        var activityId = $scope.selectedCost.activity;
                        var costActivity = $scope.scheduleGanttInstance.getTask(activityId);
                        var phaseTask = $scope.scheduleGanttInstance.getTask(Number(costActivity.phase) * 1000);
                        if ($scope.selectedCost) {
                            if ($scope.selectedCost.description === "") {
                                $scope.isSave = false;
                            } else {
                                $scope.isSave = true;
                            }
                        }
                        var count = 0;
                        costCalculation(id);
                        var rowBox = $("#cost-gantt .costBar[task_id='" + id + "']");

                        var box = rowBox.find('.' + id + '_costText');
                        $(box).each(function (index) {
                            var test = $(this);
                            console.log(index);
                            //if(index == 2 || index == 4)
                            //$(this).removeAttr('disabled');
                            //  $(this).
                            console.log($scope.CostTrackTypes);
                            console.log(id);
                            //for (var t = 0; t < $scope.CostTrackTypes[id].length; t++) {
                            //    console.log(parseInt($scope.CostTrackTypes[id][t]));
                            if ($scope.CostTrackTypes && $scope.CostTrackTypes[id] && ($scope.CostTrackTypes[id][index]) != 3) {

                                console.log($scope.CostTrackTypes[id][index]);
                                test.removeAttr('disabled');

                            }
                            //  }


                        });

                        console.log(id);
                        var row = $("#cost-gantt .gantt_row[task_id='" + id + "']");
                        console.log(row);
                        var cells = row.find('.gantt_cell');
                        // var span = cells.childNodes(0).prop('disabled');

                        console.log(cells);

                        //luan here 4/24 coup de grace
                        $(cells).each(function (index) {
                            var zero = (isCurrentTrend) ? 1 : 0;

                            var value = $(this).children(0).text();
                            console.log(index);
                            console.log($scope.description);
                            switch (index) {

                                case (0):
                                    var dropdown = ' <div class="dropdown" style="position:unset !important;">';
                                    //dropdown += ' <button class="btn btn-xs btn-primary dropdown-toggle" type="button" data-toggle="dropdown">';
                                    // alert($scope.v_activity);
                                    if (!$scope.v_activity) {
                                        dropdown += ' <button class="btn btn-xs btn-primary dropdown-toggle" type="button" data-toggle="dropdown" disabled>';
                                    }
                                    else {
                                        dropdown += ' <button class="btn btn-xs btn-primary dropdown-toggle" type="button" data-toggle="dropdown">';
                                    }
                                    dropdown += '<span class="caret"></span></button>';
                                    dropdown += ' <ul class="dropdown-menu">';
                                    if ($scope.isNewCost[id] == true) { //new cost disable it
                                        dropdown += '<li class="cost-dropdown-disable" disabled=true ng-click="costDetails()"><i class=" fa fa-info-circle"><a style="margin-left:0.5em;" >Cost Details</a></i></li>';
                                        dropdown += '<li class="cost-dropdown-disable" disabled=true ng-click="copyToClipboard()" ><i class=" fa fa-clipboard"><a style="margin-left:0.5em;" >Copy cost code to clipboard</a></i></li>';
                                    }
                                    else {
                                        dropdown += '<li class="cost-dropdown" ng-click="costDetails()"><i class=" fa fa-info-circle"><a style="margin-left:0.5em;" >Cost Details</a></i></li>';
                                        dropdown += '<li class="cost-dropdown" ng-click="copyToClipboard()" ><i class=" fa fa-clipboard"><a style="margin-left:0.5em;" >Copy cost code to clipboard</a></i></li>';
                                    }
                                    dropdown += ' <li class="cost-dropdown" ng-click="cancelCostDetail()" ><i class=" fa fa-close"><a style="margin-left:0.5em;" >Cancel</a></i></li>';
                                    if (isCurrentTrend) //Disable it
                                        dropdown += ' <li class="cost-dropdown-disable" ng-click="deleteCost(' + id + ')" ><i class=" fa fa-trash"><a  style="margin-left:0.5em;" >Delete</a></i></li>';
                                    else
                                        dropdown += ' <li class="cost-dropdown" ng-click="deleteCost(' + id + ')" ><i class=" fa fa-trash"><a  style="margin-left:0.5em;" >Delete</a></i></li>';
                                    dropdown += '  </ul>';
                                    dropdown += ' </div>';
                                    $(this).html(
                                        $compile(
                                            dropdown

                                        )($scope)
                                    );
                                    break;
                                //case (1 + zero): //luan_test - luan 07/29/2020
                                //    $scope.tempTextBoxValues = angular.copy($scope.textBoxValues);
                                //    var span = $(this).children();

                                //    if ($scope.isNewCost[id] === true) {
                                //        $(this).html(
                                //            $compile(
                                //                "<input  disabled='true' value=" + 'placeholder' + " style='width:100%;height:100%; text-align:left; vertical-align: top;'>a</input>"
                                //            )($scope)
                                //        );
                                //    }
                                //    else {
                                //        console.log('here');
                                //        $(this).html(
                                //            $compile(
                                //                "<input  disabled='true' value=" + 'placeholder' + " style='width:100%;height:100%; text-align:left; vertical-align: top;'>a</input>"
                                //            )($scope)
                                //        );
                                //    }
                                //    break;
                                case (2 + zero): //Type
                                    $scope.tempTextBoxValues = angular.copy($scope.textBoxValues);
                                    var span = $(this).children();

                                    if ($scope.isNewCost[id] === true) {
                                        $(this).html(
                                            $compile(
                                                "<select required ng-model='method[" + id + "]' style='width:100%;height:100%; text-align:left; vertical-align: top;' ng-options='option.value as option.name for option in methods' ng-change='changedMethod(" + id + ")'></select>"
                                            )($scope)
                                        );
                                    }
                                    else {
                                        console.log('here');
                                        $(this).html(
                                            $compile(
                                                "<input  disabled='true' value=" + value + " style='width:100%;height:100%; text-align:left; vertical-align: top;'></input>"
                                            )($scope)
                                        );
                                    }
                                    break;
                                case (3 + zero): //Title/Description
                                    var index = id + '00';
                                    index = parseInt(index);
                                    console.log($scope.method);
                                    if ($scope.method[id] == "F") { //LABOR
                                        console.log($scope.description);
                                        console.log(id);
                                        if ($scope.trend.TrendStatusID == 1 &&
                                            ($scope.selectedCost.cost_track_type_id == $scope.costTrackType.ACTUAL || $scope.selectedCost.cost_track_type_id == $scope.costTrackType.ESTIMATE_TO_COMPLETION))//approved and actual cost
                                        {

                                            $(this).html(
                                                $compile(
                                                    "<select class='select-disabled' disabled='true' tabindex=" + index + " ng-model='description[" + id + "]' style='width:100%;height:100%; text-align:center; vertical-align: top;' ng-options='option.name for option in FTEPositions track by option.value' ng-change='changedDescription(" + id + ")'></select>"
                                                )($scope)
                                            );
                                        } else {
                                            $(this).html(
                                                $compile(   //luan quest 3/26
                                                    "<select tabindex=" + index + " ng-model='description[" + id + "]' style='width:100%;height:100%; text-align:center; vertical-align: top;' ng-options='option.name for option in FTEPositions track by option.value' ng-change='changedDescription(" + id + ")'></select>"
                                                )($scope)
                                            );
                                        }
                                    }
                                    else if ($scope.method[id] == "L") {    //SUBCONTRACTORS

                                        if ($scope.trend.TrendStatusID == 1 &&
                                            ($scope.selectedCost.cost_track_type_id == $scope.costTrackType.ACTUAL || $scope.selectedCost.cost_track_type_id == $scope.costTrackType.ESTIMATE_TO_COMPLETION))//approved and actual cost
                                        {

                                            $(this).html(
                                                $compile(
                                                    //"<input disabled='true' tabindex=" + index + " ng-model='description[" + id + "].name'  style='width:100%;height:100%; text-align:center; vertical-align: top;' ng-keyup='changeDescription(" + id + ")' ng-change='changedDescription(" + id + ")'></input>"
                                                    "<select class='select-disabled' disabled='true' tabindex=" + index + " ng-model='description[" + id + "]' style='width:100%;height:100%; text-align:center; vertical-align: top;' ng-options='option.name for option in subcontractorTypes track by option.value' ng-change='changedDescription(" + id + ")'></select>"
                                                )($scope)
                                            );
                                        } else {
                                            console.log($scope.description, $scope.subcontractorTypes);
                                            $(this).html(
                                                $compile(
                                                    //"<input tabindex=" + index + " ng-model='description[" + id + "].name'  style='width:100%;height:100%; text-align:center; vertical-align: top;' ng-keyup='changeDescription(" + id + ")' ng-change='changedDescription(" + id + ")'></input>"
                                                    "<select tabindex=" + index + " ng-model='description[" + id + "]' style='width:100%;height:100%; text-align:center; vertical-align: top;' ng-options='option.name for option in subcontractorTypes track by option.value' ng-change='changedDescription(" + id + ")'></select>"
                                                )($scope)
                                            );
                                        }
                                    }
                                    else if ($scope.method[id] == "ODC") {  //ODC
                                        console.log("woo hoo");
                                        console.log($scope.description);
                                        console.log(id);
                                        if ($scope.trend.TrendStatusID == 1 &&
                                            ($scope.selectedCost.cost_track_type_id == $scope.costTrackType.ACTUAL || $scope.selectedCost.cost_track_type_id == $scope.costTrackType.ESTIMATE_TO_COMPLETION))//approved and actual cost
                                        {
                                            $(this).html(
                                                $compile(
                                                    "<select class='select-disabled' disabled='true' tabindex=" + index + " ng-model='description[" + id + "]' style='width:100%;height:100%; text-align:center; vertical-align: top;' ng-options='option.name for option in ODCTypes track by option.value' ng-change='changedDescription(" + id + ")'></select>"
                                                )($scope)
                                            );
                                        } else {
                                            $(this).html(
                                                $compile(
                                                    "<select tabindex=" + index + " ng-model='description[" + id + "]' style='width:100%;height:100%; text-align:center; vertical-align: top;' ng-options='option.name for option in ODCTypes track by option.value' ng-change='changedDescription(" + id + ")'></select>"
                                                )($scope)
                                            );
                                        }
                                    }
                                    else if ($scope.method[id] == "U") {    //MATERIALS
                                        console.log($scope.description);
                                        if ($scope.trend.TrendStatusID == 1 &&
                                            ($scope.selectedCost.cost_track_type_id == $scope.costTrackType.ACTUAL || $scope.selectedCost.cost_track_type_id == $scope.costTrackType.ESTIMATE_TO_COMPLETION))//approved and actual cost
                                        {

                                            $(this).html(
                                                $compile(

                                                    "<select class='select-disabled' disabled='true'   tabindex=" + index + " ng-model='description[" + id + "]' style='width:100%;height:100%; text-align:center; vertical-align: top;' ng-options='option.name for option in materialCategories track by option.value' ng-change='changedDescription(" + id + ")'></select>"
                                                )($scope)
                                            );
                                        } else {
                                            $(this).html(
                                                $compile(
                                                    // "<input tabindex=" + index + " ng-model='description[" + id + "].name'  style='width:100%;height:100%; text-align:center; vertical-align: top;' ng-keyup='changeDescription(" + id + ")' ng-change='changedDescription(" + id + ")'></input>"
                                                    "<select  tabindex=" + index + " ng-model='description[" + id + "]' style='width:100%;height:100%; text-align:center; vertical-align: top;' ng-options='option.name for option in materialCategories track by option.value' ng-change='changedDescription(" + id + ")'></select>"
                                                )($scope)
                                            );
                                        }
                                    }
                                    else {
                                        console.log("Error");
                                    }
                                    break;
                                case (7 + zero): //Unit Budget
                                    var index = id + '02';
                                    index = parseInt(index);
                                    if ($scope.method[id] == "F") {
                                        $(this).html(
                                            $compile(
                                                //"<input ng-model='unitBudget[" + id + "]' disabled=true style='width:100%;height:100%; text-align:center; vertical-align: top;' ng-keyup='changedUnitCost(" + id + ")'></input>"
                                                "<input ng-model='totalUnits[" + id + "]' disabled=true style='width:100%;height:100%; text-align:center; vertical-align: top;' ng-keyup='changedTotalUnits(" + id + ")'></input>"  //Manasi 10-07-2020
                                            )($scope)
                                        );
                                    }
                                    else if ($scope.method[id] == "L" || $scope.method[id] == "ODC") {
                                        $(this).html(
                                            $compile(
                                                "<input disabled='true' value='N/A' style='width:100%;height:100%; text-align:center; vertical-align: top;'></input>"
                                            )($scope))
                                    } else if ($scope.method[id] == "U") {
                                        if ($scope.trend.TrendStatusID == 1 && $scope.selectedCost.cost_track_type_id == 2)//approved and actual cost
                                        {
                                            $(this).html(
                                                $compile(
                                                    //"<input disabled='true' tabindex=" + index + " ng-model='unitBudget[" + id + "]' ng-keyup='changedUnitCost(" + id + ")'   style='width:100%;height:100%; text-align:center; vertical-align: top;' ng-keyup='changedUnitCost(" + id + ")'></input>"
                                                    "<input disabled='true' tabindex=" + index + " ng-model='totalUnits[" + id + "]' ng-keyup='changedTotalUnits(" + id + ")'   style='width:100%;height:100%; text-align:center; vertical-align: top;' ng-keyup='changedUnitCost(" + id + ")'></input>"  //Manasi 10-07-2020
                                                )($scope)
                                            );
                                        } else {
                                            $(this).html(
                                                $compile(
                                                    //"<input disabled='true' tabindex=" + index + " ng-model='unitBudget[" + id + "]' ng-keyup='changedUnitCost(" + id + ")'   style='width:100%;height:100%; text-align:center; vertical-align: top;' ng-keyup='changedUnitCost(" + id + ")'></input>"
                                                    "<input disabled='true' tabindex=" + index + " ng-model='totalUnits[" + id + "]' ng-keyup='changedUnitCost(" + id + ")'   style='width:100%;height:100%; text-align:center; vertical-align: top;' ng-keyup='changedUnitCost(" + id + ")'></input>"
                                                )($scope)
                                            );
                                        }
                                    }
                                    break;
                                case (8 + zero): //# of units
                                    console.log('here');
                                    if ($scope.method[id] == "F" || $scope.method[id] == "U") {
                                        $(this).html(
                                            $compile(
                                                //"<input ng-model='totalUnits[" + id + "]' disabled='true'  style='width:100%;height:100%; text-align:center; vertical-align: top;' ng-keyup='changedTotalUnits(" + id + ")'></input>"
                                                "<input ng-model='totalBudget[" + id + "]' disabled='true'  style='width:100%;height:100%; text-align:center; vertical-align: top;' ng-keyup='changedUnitCost(" + id + ")'></input>"  //Manasi 10-07-2020
                                            )($scope)
                                        );
                                    } else if ($scope.method[id] == "L" || $scope.method[id] == "ODC") {
                                        $(this).html(
                                            $compile(
                                                //"<input disabled='true' value='N/A' style='width:100%;height:100%; text-align:center; vertical-align: top;'></input>"  
                                                "<input ng-model='totalBudget[" + id + "]' disabled='true'  style='width:100%;height:100%; text-align:center; vertical-align: top;' ng-keyup='changedUnitCost(" + id + ")'></input>"  // Manasi 10-07-2020
                                            )($scope)
                                        )
                                    } else {
                                        console.log('do nothing');

                                    }
                                    break;

                                case (9 + zero): //# of units
                                    console.log('here');
                                    if ($scope.method[id] == "F" || $scope.method[id] == "U") {
                                        $(this).html(
                                            $compile(
                                                //"<input ng-model='totalUnits[" + id + "]' disabled='true'  style='width:100%;height:100%; text-align:center; vertical-align: top;' ng-keyup='changedTotalUnits(" + id + ")'></input>"
                                                "<input ng-model='totalCost[" + id + "]' disabled='true'  style='width:100%;height:100%; text-align:center; vertical-align: top;' ng-keyup='changedUnitCost(" + id + ")'></input>"  //Manasi 10-07-2020
                                            )($scope)
                                        );
                                    } else if ($scope.method[id] == "L" || $scope.method[id] == "ODC") {
                                        $(this).html(
                                            $compile(
                                                //"<input disabled='true' value='N/A' style='width:100%;height:100%; text-align:center; vertical-align: top;'></input>"  
                                                "<input ng-model='totalCost[" + id + "]' disabled='true'  style='width:100%;height:100%; text-align:center; vertical-align: top;' ng-keyup='changedUnitCost(" + id + ")'></input>"  // Manasi 10-07-2020
                                            )($scope)
                                        )
                                    } else {
                                        console.log('do nothing');

                                    }
                                    break;
                                case (5 + zero): //Unit type
                                    var index = id + '01';
                                    index = parseInt(index);
                                    if ($scope.method[id] == "U") {
                                        //$scope.unit_type = {name:$scope.unit_type};
                                        if ($scope.trend.TrendStatusID == 1 && $scope.selectedCost.cost_track_type_id == 2)//approved and actual cost
                                        {
                                            $(this).html(
                                                $compile(
                                                    //"<input disabled=true  value='Each' style='width:100%;height:100%; text-align:center; vertical-align: top;'></input>"
                                                    " <select class='select-disabled' disabled='true' ng-model= 'unit_type[" + id + "]' style='width:100%;height:100%; text-align:center; vertical-align: top; text-align-last: center; -moz-appearance: none; -webkit-appearance: none;'" +
                                                    " ng-options='type.name for type in unitTypes track by type.value ' ng-change='changedUnitType(" + id + ")'></select>"
                                                )($scope)
                                            );
                                        } else {
                                            $(this).html(
                                                $compile(
                                                    //"<input disabled=true  value='Each' style='width:100%;height:100%; text-align:center; vertical-align: top;'></input>"
                                                    " <select class='select-disabled' disabled='true' ng-model= 'unit_type[" + id + "]' style='width:100%;height:100%; text-align:center; vertical-align: top; text-align-last: center; -moz-appearance: none; -webkit-appearance: none;'" +
                                                    " ng-options='type.name for type in unitTypes track by type.value ' ng-change='changedUnitType(" + id + ")'></select>"
                                                )($scope)
                                            );
                                        }
                                    }
                                    else if ($scope.method[id] == "L") {
                                        $(this).html(
                                            $compile(
                                                //"<input disabled=true  value='Contractor' style='width:100%;height:100%; text-align:center; vertical-align: top;'></input>"
                                                //"<input disabled=true  value='Subcontractor' style='width:100%;height:100%; text-align:center; vertical-align: top;'></input>"  //Manasi 17-07-2020
                                                "<input disabled=true  value='USD' style='width:100%;height:100%; text-align:center; vertical-align: top;'></input>"  //Manasi 05-08-2020
                                            )($scope)
                                        )

                                    }
                                    else if ($scope.method[id] == "F") {
                                        $(this).html(
                                            $compile(
                                                "<input disabled='true' value='FTE' style='width:100%;height:100%; text-align:center; vertical-align: top;'></input>"
                                            )($scope)
                                        )
                                    } else if ($scope.method[id] == "ODC") {
                                        $(this).html(
                                            $compile(
                                                "<input disabled='true' value='USD' style='width:100%;height:100%; text-align:center; vertical-align: top;'></input>"
                                            )($scope)
                                        )
                                    }

                                    break;
                                case (6 + zero): //Unit cost
                                    var index = id + '02';
                                    index = parseInt(index);
                                    if ($scope.method[id] == "F") {
                                        $(this).html(
                                            $compile(
                                                //"<input ng-model='unitCost[" + id + "]'  style='width:100%;height:100%; text-align:center; vertical-align: top;' ng-keyup='changedUnitCost(" + id + ")'></input>"
                                                "<input ng-model='unitBudget[" + id + "]'  style='width:100%;height:100%; text-align:center; vertical-align: top;' ng-keyup='changedUnitCost(" + id + ")'></input>"  //Manasi 10-07-2020

                                            )($scope)
                                        );
                                    }
                                    else if ($scope.method[id] == "L" || $scope.method[id] == "ODC") {
                                        $(this).html(
                                            $compile(
                                                "<input disabled='true' value='N/A' style='width:100%;height:100%; text-align:center; vertical-align: top;'></input>"
                                            )($scope))
                                    } else if ($scope.method[id] == "U") {
                                        if ($scope.trend.TrendStatusID == 1 && $scope.selectedCost.cost_track_type_id == 2)//approved and actual cost
                                        {
                                            $(this).html(
                                                $compile(
                                                    //"<input disabled='true' tabindex=" + index + " ng-model='unitCost[" + id + "]' ng-keyup='changedUnitCost(" + id + ")'   style='width:100%;height:100%; text-align:center; vertical-align: top;' ng-keyup='changedUnitCost(" + id + ")'></input>"
                                                    "<input disabled='true' tabindex=" + index + " ng-model='unitBudget[" + id + "]' ng-keyup='changedUnitCost(" + id + ")'   style='width:100%;height:100%; text-align:center; vertical-align: top;' ng-keyup='changedUnitCost(" + id + ")'></input>"  //Manasi 10-07-2020
                                                )($scope)
                                            );
                                        } else {
                                            $(this).html(
                                                $compile(
                                                    //"<input  tabindex=" + index + " ng-model='unitCost[" + id + "]' ng-keyup='changedUnitCost(" + id + ")'   style='width:100%;height:100%; text-align:center; vertical-align: top;' ng-keyup='changedUnitCost(" + id + ")'></input>"
                                                    "<input  tabindex=" + index + " ng-model='unitBudget[" + id + "]' ng-keyup='changedUnitCost(" + id + ")'   style='width:100%;height:100%; text-align:center; vertical-align: top;' ng-keyup='changedUnitCost(" + id + ")'></input>"  //Manasi 10-07-2020
                                                )($scope)
                                            );
                                        }
                                    }
                                    break;
                                case (9 + zero):   //Cost
                                    console.log(id, $scope.totalCost);
                                    if ($scope.method[id] == "F" || $scope.method[id] == "U" || $scope.method[id] == "L" || $scope.method[id] == "ODC") {
                                        //$(this).html(
                                        //    $compile(
                                        //        "<input ng-model='totalCost[" + id + "]' disabled=true style='width:100%;height:100%; text-align:center; vertical-align: top;' ng-change='changedUnitCost(" + id + ")'></input>"
                                        //    )($scope)
                                        //);  //Manasi10-07-2020
                                    } else {
                                        console.log('do nothing');
                                    }

                                    break;
                                case (10 + zero):   //Cost
                                    console.log(id, $scope.totalCost);
                                    if ($scope.method[id] == "F" || $scope.method[id] == "U" || $scope.method[id] == "L" || $scope.method[id] == "ODC") {
                                        $(this).html(
                                            $compile(
                                                "<input ng-model='totalBudget[" + id + "]' disabled=true style='width:100%;height:100%; text-align:center; vertical-align: top;' ng-change='changedUnitCost(" + id + ")'></input>"
                                            )($scope)
                                        );
                                    } else {
                                        console.log('do nothing');
                                    }

                                    break;
                                case (4 + zero): //Employee
                                    console.log($scope.method[id]);
                                    var index = id + '00';
                                    index = parseInt(index);
                                    console.log($scope.method[id]);

                                    if ($scope.method[id] == "F") { //LABOR
                                        console.log($scope.description);
                                        console.log(id);
                                        console.log($scope.newEmployees);
                                        if (typeof $scope.employee_id[id] == 'object') {
                                            console.log('in here');
                                        }
                                        else {
                                            console.log('in here', $scope.employees);
                                            angular.forEach($scope.employees, function (item) {
                                                if (item.name == $scope.employee_id[id]) {
                                                    $scope.employee_id[id] = item;

                                                }
                                            });
                                        }

                                        if ($scope.trend.TrendStatusID == 1 &&
                                            ($scope.selectedCost.cost_track_type_id == $scope.costTrackType.ACTUAL || $scope.selectedCost.cost_track_type_id == $scope.costTrackType.ESTIMATE_TO_COMPLETION))//approved and actual cost
                                        {
                                            $(this).html(
                                                $compile(
                                                    "<select  class='select-disabled' disabled='true'  ng-model='employee_id[" + id + "]'  style='width:100%;height:100%; text-align:center; vertical-align: top;'" +
                                                    " ng-options='option.name for option in newEmployees[" + id + "] track by option.value'></select>"
                                                )($scope)
                                            );
                                        } else {
                                            $(this).html(   //luan quest 3/26
                                                $compile(
                                                    "<select ng-model='employee_id[" + id + "]'  style='width:100%;height:100%; text-align:center; vertical-align: top;'" +
                                                    " ng-options='option.name for option in newEmployees[" + id + "] track by option.value' ng-change='changedName(" + id + ")' ></select>"
                                                )($scope)
                                            );
                                        }
                                    }
                                    else if ($scope.method[id] == "L") {  //SUBCONTRACTORS
                                        console.log($scope.description);
                                        console.log(id);
                                        console.log($scope.subcontractor_id);
                                        if (typeof $scope.subcontractor_id[id] == 'object') {
                                            console.log('in here');
                                        } else {
                                            console.log('in here', $scope.subcontractors);
                                            angular.forEach($scope.subcontractors, function (item) {
                                                if (item.name == $scope.subcontractor_id[id]) {
                                                    $scope.subcontractor_id[id] = item;
                                                    console.log('in here');
                                                }
                                            });
                                        }

                                        console.log($scope.newSubcontractors);
                                        if ($scope.trend.TrendStatusID == 1 &&
                                            ($scope.selectedCost.cost_track_type_id == $scope.costTrackType.ACTUAL || $scope.selectedCost.cost_track_type_id == $scope.costTrackType.ESTIMATE_TO_COMPLETION))//approved and actual cost
                                        {

                                            $(this).html(
                                                $compile(
                                                    "<select  class='select-disabled' disabled='true' ng-model='subcontractor_id[" + id + "]'  style='width:100%;height:100%; text-align:center; vertical-align: top;'" +
                                                    " ng-options='option.name for option in newSubcontractors[" + id + "] track by option.value'></select>"
                                                )($scope)
                                            );
                                        } else {
                                            $(this).html(   //luan quest 3/26
                                                $compile(
                                                    "<select ng-model='subcontractor_id[" + id + "]'  style='width:100%;height:100%; text-align:center; vertical-align: top;'" +
                                                    " ng-options='option.name for option in newSubcontractors[" + id + "] track by option.value' ng-change='changedName(" + id + ")' ></select>"
                                                )($scope)
                                            );
                                        }
                                    }
                                    else if ($scope.method[id] == "U") {  //MATERIALS
                                        console.log($scope.description);
                                        console.log(id);
                                        if (typeof $scope.material_id[id] == 'object') {

                                        } else {
                                            angular.forEach($scope.materials, function (item) {
                                                if (item.name == $scope.material_id[id]) {
                                                    $scope.material_id[id] = item;

                                                }
                                            });
                                        }

                                        if ($scope.trend.TrendStatusID == 1 &&
                                            ($scope.selectedCost.cost_track_type_id == $scope.costTrackType.ACTUAL || $scope.selectedCost.cost_track_type_id == $scope.costTrackType.ESTIMATE_TO_COMPLETION))//approved and actual cost
                                        {
                                            $(this).html(
                                                $compile(
                                                    "<select  class='select-disabled' disabled='true' ng-model='material_id[" + id + "]'  style='width:100%;height:100%; text-align:center; vertical-align: top; -moz-appearance: none;'" +  // -webkit-appearance: none;  text-align-last: center;
                                                    " ng-options='option.name for option in newMaterials[" + id + "] track by option.value'></select>"
                                                )($scope)
                                            );
                                        } else {
                                            $(this).html(   //luan quest 3/26
                                                $compile(
                                                    "<select ng-model='material_id[" + id + "]'  style='width:100%;height:100%; text-align:center; vertical-align: top; -moz-appearance: none;'" +    // -webkit-appearance: none; text-align-last: center;
                                                    " ng-options='option.name for option in newMaterials[" + id + "] track by option.value' ng-change='changedName(" + id + ")' ></select>"
                                                )($scope)
                                            );
                                        }
                                    }
                                    else if ($scope.method[id] == "ODC") {
                                        console.log('here');


                                        $(this).html(
                                            $compile(
                                                "<input disabled='true' value='N/A' style='width:100%;height:100%; text-align:center; vertical-align: top;'></input>"
                                            )($scope)
                                        );
                                    } else {
                                        console.log('do nothing');
                                    }
                                    break;
                            }
                        });

                    }

                }
                //}

                if ($scope.testt === true) {
                    currentId = -1;
                    $scope.testt = false;
                }
                return true;

            });
            //customClick - END

            //#region init gantt
            var tasks = {
                data: [
                    {
                        id: 1, text: "Project #2", start_date: "01-04-2013", duration: 18, order: 10,
                        progress: 0.4, open: true
                    },
                    {
                        id: 2, text: "Task #1", start_date: "02-04-2013", duration: 8, order: 10,
                        progress: 0.6, parent: 1
                    },
                    {
                        id: 3, text: "Task #2", start_date: "11-04-2013", duration: 8, order: 20,
                        progress: 0.6, parent: 1
                    }
                ],
                links: [
                    { id: 1, source: 1, target: 2, type: "1" },
                    { id: 2, source: 2, target: 3, type: "0" },
                    { id: 3, source: 3, target: 4, type: "0" },
                    { id: 4, source: 2, target: 5, type: "2" },
                ]
            };


            //-----------------Manasi 26-08-2020------------------------------------------------
            onRouteChangeOff = $scope.$on('$locationChangeStart', function (event) {
                debugger
                console.log('we in there');
                var newUrl = $location.path();
                var isCostEdited = isCostsEdited();
                var res;
                $scope.costitems = [];
                // alert(isCostsEdited());
                console.log(isCostEdited);

                var a = $scope.selectedCost;

                $scope.costGanttInstance.eachTask(function (item) {
                    $scope.costitems[item.id - 1] = item;

                });

                for (var i = 0; i <= $scope.costitems.length - 1; i++) {
                    if ($scope.method[$scope.costitems[i].id] == "" || $scope.method[$scope.costitems[i].id] == undefined) {
                        res = false;
                    }
                    else {
                        if ($scope.isCostEdited[$scope.costitems[i].id]) {
                            if ($scope.selectedCost.description != $scope.description[$scope.costitems[i].id].name) {
                                res = true;
                                break;
                            }
                            else if ($scope.selectedCost.total_budget != $scope.totalBudget[$scope.costitems[i].id]) {
                                res = true;
                                break;
                            }
                            else if ($scope.employee_id[$scope.costitems[i].id]) {
                                if ($scope.selectedCost.employee_id != $scope.employee_id[$scope.costitems[i].id].name) {
                                    res = true;
                                    break;
                                }
                            }
                            else if ($scope.subcontractor_id[$scope.costitems[i].id]) {
                                if ($scope.selectedCost.employee_id != $scope.subcontractor_id[$scope.costitems[i].id].name) {
                                    res = true;
                                    break;
                                }
                            }
                            else if ($scope.material_id[$scope.costitems[i].id]) {
                                if ($scope.selectedCost.employee_id != $scope.material_id[$scope.costitems[i].id].name) {
                                    res = true;
                                    break;
                                }

                            }


                        }

                    }
                }
                if (res) {
                    //$scope.saveCost();
                    $scope.confirm = "";
                    var scope = $rootScope.$new();
                    scope.params = { confirm: $scope.confirm };
                    $rootScope.modalInstance = $uibModal.open({
                        scope: scope,
                        templateUrl: 'app/views/Modal/exit_confirmation_modal.html',
                        controller: 'exitConfirmation',
                        size: 'md',
                        backdrop: true
                    });
                    $rootScope.modalInstance.result.then(function (data) {
                        setTimeout(function () {
                            applyExpandables();
                        }, 1000);
                        if (scope.params.confirm === "exit") {
                            onRouteChangeOff();
                            $location.path(newUrl);
                        }
                        else if (scope.params.confirm === "save") {
                            $scope.costGanttInstance.eachTask(function (item) {
                                $scope.costitems[item.id - 1] = item;
                            });

                            for (var i = 0; i <= $scope.costitems.length - 1; i++) {
                                if ($scope.method[$scope.costitems[i].id] == "" || $scope.method[$scope.costitems[i].id] == undefined) {
                                    res = false;
                                }
                                else {
                                    res = true;
                                    break;
                                }
                            }
                            if (res)
                                $scope.saveCost();
                            else {
                                //onRouteChangeOff();
                                //$location.path(newUrl);
                                dhtmlx.alert("Please add cost details.");
                                return;
                            }
                        }
                        else if (scope.params.confirm === "back") {
                            //do nothing
                        }
                    });
                }
                else {
                    //onRouteChangeOff();
                    $location.path(newUrl);
                    return;
                    //dhtmlx.alert("Please add cost details.")
                }
                if (!isCostEdited) {
                    console.log(isCostEdited);
                    return;
                }
                console.log('we out there');
                // if(!$scope.checkForChanges()) return; priteshexit
                $scope.confirm = "";
                var scope = $rootScope.$new();
                scope.params = { confirm: $scope.confirm };
                $rootScope.modalInstance = $uibModal.open({
                    scope: scope,
                    templateUrl: 'app/views/Modal/exit_confirmation_modal.html',
                    controller: 'exitConfirmation',
                    size: 'md',
                    backdrop: true
                });
                $rootScope.modalInstance.result.then(function (data) {
                    setTimeout(function () {
                        applyExpandables();
                    }, 1000);
                    if (scope.params.confirm === "exit") {
                        onRouteChangeOff();
                        $location.path(newUrl);
                    }
                    else if (scope.params.confirm === "save") {

                        $scope.saveCost();
                    }
                    else if (scope.params.confirm === "back") {
                        //do nothing
                    }
                });
                event.preventDefault();
            });
            //-----------------------------------------------------------------------------------------------------------------------------

            //onRouteChangeOff = $scope.$on('$locationChangeStart', function (event) {
            //    console.log('we in there');
            //    var newUrl = $location.path();
            //    var isCostEdited = isCostsEdited();
            //    var res;
            //    $scope.costitems = [];
            //    // alert(isCostsEdited());
            //    console.log(isCostEdited);
            //    if (!isCostEdited) {
            //        console.log(isCostEdited);
            //        return;
            //    }
            //    console.log('we out there');
            //    // if(!$scope.checkForChanges()) return; priteshexit
            //    $scope.confirm = "";
            //    var scope = $rootScope.$new();
            //    scope.params = { confirm: $scope.confirm };
            //    $rootScope.modalInstance = $uibModal.open({
            //        scope: scope,
            //        templateUrl: 'app/views/Modal/exit_confirmation_modal.html',
            //        controller: 'exitConfirmation',
            //        size: 'md',
            //        backdrop: true
            //    });
            //    $rootScope.modalInstance.result.then(function (data) {
            //        setTimeout(function () {
            //            applyExpandables();
            //        }, 1000);
            //        if (scope.params.confirm === "exit") {
            //            onRouteChangeOff();
            //            $location.path(newUrl);
            //        }
            //        else if (scope.params.confirm === "save") {
            //            //-------------------------------------Manasi 21-08-2020--------------------------------------------------
            //            $scope.costGanttInstance.eachTask(function (item) {
            //                $scope.costitems[item.id - 1] = item;
            //            });

            //            for (var i = 0; i < $scope.costitems.length - 1; i++) {
            //                if ($scope.method[$scope.costitems[i].id] == "" || $scope.method[$scope.costitems[i].id] == undefined) {
            //                    res = false;
            //                }
            //                else {
            //                    res = true;
            //                    //break;
            //                }
            //            }
            //            if (res)
            //                $scope.saveCost();
            //            else {
            //                //onRouteChangeOff();
            //                //$location.path(newUrl);
            //                dhtmlx.alert("Please add cost details.")
            //            }

            //             //-------------------------------------------------------------------------------------------------------
            //        }
            //        else if (scope.params.confirm === "back") {
            //            //do nothing
            //        }
            //    });
            //    event.preventDefault();
            //});

            //Export to PDF
            $(document).ready(function () {
                $scope.export = function () {
                    $scope.scheduleGanttInstance.exportToPDF({
                        name: "mygantt.pdf",
                        header: '<style>.project-left, .project-right{top:6px;background-color:transparent;bordoer-style:solid;width:0px;height:0px;} .project-left{left:0px;border-width:0px 0px 8px;7px; border-top-color:transparent; border-right-color:transparent !important; border-bottom-color: transparent !important; border-left-color:#444444 !important;} .custom-project{position:absolute;height:6px;color:#ffffff;background-color:#444444;}</style>'

                    });
                }
            });

            $(window).keypress(function (event) {
                var key = event.keyCode;
            });

            //$scope.ExportToMPP = function () {
            //    //Export to MPP - Manasi 14-05-2020
            //    mppFile.Export().get({
            //        "projectId": delayedData[2].result[0].ProjectID,
            //        "trendNumber": $scope.trend.TrendNumber,
            //        "granularity": $scope.scheduleScale
            //    }, function (response) {
            //        console.log(response);
            //        var MPP = response.result;
            //    });

            //}

            var saveByteArray = (function () {
                var a = document.createElement("a");
                document.body.appendChild(a);
                a.style = "display: none";
                return function (data, name) {
                    var blob = new Blob([data], { type: "application/octet-stream;base64" }), //need the square bracket
                        url = window.URL.createObjectURL(blob);
                    a.href = url;
                    a.download = name;
                    a.click();
                    window.URL.revokeObjectURL(url);
                };
            }());


            $scope.ExportToMPP = function () {

                var cannotExportMPP = false;
                $scope.scheduleGanttInstance.eachTask(function (task) {
                    if (task.id == projectMaxId) {
                        if (task.totalCost == 0) {
                            dhtmlx.alert({
                                text: "Cannot export to MS Project because there are no costs associated with this trend",     //Manasi 15-07-2020
                                width: "500px"
                            });
                            cannotExportMPP = true;
                            return;
                        }
                    }
                });
                if (cannotExportMPP) return;

                $('#ExportToMPP').prop('disabled', true);
                document.getElementById("loading").style.display = "block";
                var projectId = delayedData[2].result[0].ProjectID;
                var trendNumber = $scope.trend.TrendNumber;
                var granularity = $scope.scheduleScale;
                var PhaseId = $scope.schedulePhase;

                if (!PhaseId) {
                    PhaseId = 0;
                }
                //alert(PhaseId);
                var request = {
                    method: 'GET',
                    url: serviceBasePath + 'Request/Export/' + projectId + '/' + trendNumber + '/' + granularity + '/' + PhaseId,
                    data: '', //fileUploadProject.files, //$scope.
                    ignore: true,
                    headers: {
                        'Content-Type': undefined
                    }
                };

                /* Needs $http */
                // SEND THE FILES.
                $http(request).then(function success(d) {
                    //alert(d.data.result);

                    if (d.data.result === "Failed") {
                        $('#ExportToMPP').prop('disabled', false);
                        document.getElementById("loading").style.display = "none";
                        dhtmlx.alert("Microsoft Project should be installed on your machine to generate MPP file.");
                        return;
                    }
                    else {
                        byteCharacters = atob(d.data.result);
                        byteNumbers = new Array(byteCharacters.length);
                        for (let i = 0; i < byteCharacters.length; i++) {
                            byteNumbers[i] = byteCharacters.charCodeAt(i);
                        }
                        byteArray = new Uint8Array(byteNumbers);

                        saveByteArray(byteArray, d.data.fileName);
                        // debugger;
                        $('#ExportToMPP').prop('disabled', false);
                        document.getElementById("loading").style.display = "none";
                        dhtmlx.alert("File generated successfully");
                        return;
                    }

                    window.open(d.data.result, '_blank');
                    var fileName = d.data.result.replace(/^.*[\\\/]/, '');
                    //alert(fileName);
                    $http.get(serviceBasePath + 'Request/Delete?fileName=' + fileName)
                        .then(function success(d) {
                            // alert(d.data.result);
                            //debugger;

                            dhtmlx.alert(d.data.result);

                        });
                }, function error(d) {
                    //dhtmlx.alert(d.ExceptionMessage);
                    dhtmlx.alert("File generation failed. Please try again.")
                }).finally(function () {
                    $('#ExportToMPP').prop('disabled', false);
                    document.getElementById("loading").style.display = "none";
                });
            }

            //$('#ExportToMPP').unbind('click').on('click', function () {
            //    var projectId = delayedData[2].result[0].ProjectID;
            //    var trendNumber = $scope.trend.TrendNumber;
            //    var granularity = $scope.scheduleScale;
            //    var PhaseId = $scope.schedulePhase;

            //    if (!PhaseId) {
            //        PhaseId = 0;
            //    }
            //    //alert(PhaseId);
            //    var request = {
            //        method: 'GET',
            //        url: serviceBasePath + 'Request/Export/' + projectId + '/' + trendNumber + '/' + granularity + '/' + PhaseId,
            //        data: '', //fileUploadProject.files, //$scope.
            //        ignore: true,
            //        headers: {
            //            'Content-Type': undefined
            //        }
            //    };

            //    /* Needs $http */
            //    // SEND THE FILES.
            //    $http(request).then(function success(d) {
            //        //alert(d.data.result);

            //        byteCharacters = atob(d.data.result);
            //        byteNumbers = new Array(byteCharacters.length);
            //        for (let i = 0; i < byteCharacters.length; i++) {
            //            byteNumbers[i] = byteCharacters.charCodeAt(i);
            //        }
            //        byteArray = new Uint8Array(byteNumbers);

            //        saveByteArray(byteArray, d.data.fileName);
            //        dhtmlx.alert("File Generated Successfully");
            //        return;

            //        window.open(d.data.result, '_blank');
            //        var fileName = d.data.result.replace(/^.*[\\\/]/, '');
            //        //alert(fileName);
            //        $http.get(serviceBasePath + 'Request/Delete?fileName=' + fileName)
            //            .then(function success(d) {
            //                // alert(d.data.result);
            //                dhtmlx.alert(d.data.result);
            //            });
            //    });
            //});


            //----------------------------------------------Manasi - 02/07/2020 ----------------------------------------------------
            $('#weekBtn').on('mouseover', function () {
                $(this).addClass('hover');
                $('#weekBtn').tooltip({ title: "Week", placement: "bottom" }).tooltip('show');
            });
            $('#weekBtn').on('mouseleave', function () {
                $(this).removeClass('hover');
                $('#weekBtn').tooltip({ title: "Week", placement: "bottom" }).tooltip('hide');
            });
            $('#monthBtn').on('mouseover', function () {
                $(this).addClass('hover');
                $('#monthBtn').tooltip({ title: "Month", placement: "bottom" }).tooltip('show');
            });
            $('#monthBtn').on('mouseleave', function () {
                $(this).removeClass('hover');
                $('#monthBtn').tooltip({ title: "Month", placement: "bottom" }).tooltip('hide');
            });
            $('#yearBtn').on('mouseover', function () {
                $(this).addClass('hover');
                $('#yearBtn').tooltip({ title: "Year", placement: "bottom" }).tooltip('show');
            });
            $('#yearBtn').on('mouseleave', function () {
                $(this).removeClass('hover');
                $('#yearBtn').tooltip({ title: "Year", placement: "bottom" }).tooltip('hide');
            });
            //---------------------------------------------------------------------------------------------------------

            /*----------------------------------- Function Definitions - BEGIN ---------------------------------------------*/


            function updatePhaseAndProjectDisplay() {
                var activities = [];

                for (var x = 0; x < $scope.schedule.data.length; x++) {
                    if (!$scope.schedule.data[x].type) {
                        activities.push($scope.schedule.data[x]);
                    }
                }

                //Loop through all phases and the project and change the original start/end dates and percentage completion
                console.log(activities);
                //luan quest 2/13
                for (var x = 0; x < $scope.schedule.data.length; x++) {     //phases
                    if ($scope.schedule.data[x].type == 'project' && $scope.schedule.data[x].PhaseID) {
                        $scope.schedule.data[x].originalStartDate = getPhaseOriginalStartEndDate($scope.schedule.data[x].PhaseID, activities).originalStartDate;
                        $scope.schedule.data[x].originalEndDate = getPhaseOriginalStartEndDate($scope.schedule.data[x].PhaseID, activities).originalEndDate;
                        $scope.schedule.data[x].percentage_completion = getPhasePercentageCompletion($scope.schedule.data[x].PhaseID, activities);
                    } else if ($scope.schedule.data[x].type == 'project' && !$scope.schedule.data[x].PhaseID) {      //project
                        $scope.schedule.data[x].originalStartDate = getProjectOriginalStartEndDate(activities).originalStartDate;
                        $scope.schedule.data[x].originalEndDate = getProjectOriginalStartEndDate(activities).originalEndDate;
                        $scope.schedule.data[x].percentage_completion = getProjectPercentageCompletion(activities);
                    }
                }
            }

            // Used to create a blob from an excel file byte array.
            function s2ab(s) {
                var buf = new ArrayBuffer(s.length);
                var view = new Uint8Array(buf);
                for (var i = 0; i != s.length; ++i) view[i] = s.charCodeAt(i) & 0xFF;
                return buf;
            }

            function renderCostBoxes(widthOfTextBox, numberOfBoxes, id) {
                console.log(numberOfBoxes);
                $scope.numberOfBoxes[id] = numberOfBoxes;

                var relWidth = 33;
                var cssClass = "costBox " + id.toString() + "_cost";
                var costBoxes = '';
                costBoxes = "<div class='" + cssClass + "'>";
                //if($scope.taskBeingEdited === true){
                console.log($scope.CostTrackTypes);

                //$scope.textBoxValues[id] = "555555555555555555555555";
                if ($scope.isCostEdited[id] === true) {
                    for (var i = 0; i < numberOfBoxes; i++) {
                        var s = id.toString() + '0' + i.toString();
                        s = parseInt(s) + 5;

                        if ($scope.CostTrackTypes[id] && $scope.CostTrackTypes[id][i] == $scope.costTrackType.ACTUAL) { //ACTUAL COST

                            costBoxes += "<input onClick='this.select();'  disabled = true  ng-style = '{color: textBoxStyles[" + id + "][" + i + "]}' ng-model='textBoxValues[" + id + "][" + i + "]' TABINDEX=" + s + " ng-keyup = 'changedCost(" + id + "," + i + ")' class='" + id.toString() + "_costText' type='text' style='width:" + widthOfTextBox + "px; text-align:center;background-color:#98FB98;' />"
                        } else if ($scope.CostTrackTypes[id] && $scope.CostTrackTypes[id][i] == $scope.costTrackType.ESTIMATE_TO_COMPLETION) {
                            costBoxes += "<input onClick='this.select();'  disabled = true  ng-style = '{color: textBoxStyles[" + id + "][" + i + "]}' ng-model='textBoxValues[" + id + "][" + i + "]' TABINDEX=" + s + " ng-keyup = 'changedCost(" + id + "," + i + ")' class='" + id.toString() + "_costText' type='text' style='width:" + widthOfTextBox + "px; text-align:center;background-color:#FFDAB9;' />"
                        } else {
                            costBoxes += "<input onClick='this.select();'    ng-style = '{color: textBoxStyles[" + id + "][" + i + "]}' ng-model='textBoxValues[" + id + "][" + i + "]' TABINDEX=" + s + " ng-keyup = 'changedCost(" + id + "," + i + ")' class='" + id.toString() + "_costText' type='text' style='width:" + widthOfTextBox + "px; text-align:center;' />"
                        }
                    }
                }
                else {

                    for (var i = 0; i < numberOfBoxes; i++) {
                        var s = id.toString() + '0' + i.toString();
                        s = parseInt(s) + 5;

                        if ($scope.CostTrackTypes[id] && $scope.CostTrackTypes[id][i] == $scope.costTrackType.ACTUAL) { // ACTUAL COST

                            costBoxes += "<input  onClick='this.select();'  disabled = true  ng-style = '{color: textBoxStyles[" + id + "][" + i + "]}' ng-model='textBoxValues[" + id + "][" + i + "]' TABINDEX=" + s + " ng-keyup = 'changedCost(" + id + "," + i + ")' class='" + id.toString() + "_costText' type='text' style='width:" + widthOfTextBox + "px; text-align:center;background-color:#98FB98;' />"
                        } else if ($scope.CostTrackTypes[id] &&
                            (($scope.CostTrackTypes[id][i] == $scope.costTrackType.ESTIMATE_TO_COMPLETION))
                        ) {
                            costBoxes += "<input onClick='this.select();'  disabled = true  ng-style = '{color: textBoxStyles[" + id + "][" + i + "]}' ng-model='textBoxValues[" + id + "][" + i + "]' TABINDEX=" + s + " ng-keyup = 'changedCost(" + id + "," + i + ")' class='" + id.toString() + "_costText' type='text' style='width:" + widthOfTextBox + "px; text-align:center;background-color:#FFDAB9;' />"
                        }
                        else
                            costBoxes += "<input onClick='this.select();'  disabled = true ng-style = '{color: textBoxStyles[" + id + "][" + i + "]}' TABINDEX=" + s + " ng-model='textBoxValues[" + id + "][" + i + "]' ng-keyup = 'changedCost(" + id + "," + i + ")' class='" + id.toString() + "_costText' type='text' style='width:" + widthOfTextBox + "px; text-align:center;' />"
                    }
                }


                costBoxes += "</div>";
                return costBoxes;
            }


            function formatCurrency(amount) {
                return (amount % 1 > 0) ? 2 : 0;
            }

            function getCurrentBufferObject() {
                var bufferItem;
                angular.forEach($scope.buffer, function (item) {
                    if (item.activityId == $scope.selectedActivity.id) {
                        bufferItem = item;
                    }
                });
                return bufferItem;
            }

            //luan quest 3/26
            function filterEmployeeByPosition(id, employeeList) {
                var allEmployees = employeeList;
                var resultAr = [];
                var allPositions = positionsList;
                console.log(allPositions);
                for (var x = 0; x < allEmployees.length; x++) {
                    var found = false;
                    for (var y = 0; y < allPositions.length; y++) {
                        if (allEmployees[x].FTEPositionID == allPositions[y].Id) {
                            allEmployees[x].FTEPositionName = allPositions[y].PositionDescription;
                            found = true;
                            break;
                        }
                    }
                    if (!found) {
                        allEmployees[x].FTEPositionName = 'N/A';
                    }
                }

                if ($scope.description && $scope.description[id]) {

                    console.log($scope.description[1], id);
                    //luan quest 3/26
                    if ($scope.description[id].name != '----------Add New----------') {
                        // Jignesh-26-03-2021
                        var authRole = $scope.localStorageSrevice.get('authorizationData').role;
                        if (authRole === "Admin") {
                            var temp = { name: '----------Add New----------', value: '0' };
                            resultAr.push(temp);
                        }
                        
                    }

                    for (var x = 0; x < allEmployees.length; x++) {
                        console.log(allEmployees[x], $scope.description[id].name);
                        if (allEmployees[x].FTEPositionName == $scope.description[id].name) {
                            var temp = { name: '', value: '' };
                            console.log(allEmployees[x]);
                            temp.name = allEmployees[x].Name;
                            temp.value = allEmployees[x].ID;
                            resultAr.push(temp);
                        }
                    }
                }
                return resultAr;
            }

            function getNewEmployees(id) {
                var allEmployees = delayedData[7].result;
                var resultAr = [];
                var allPositions = positionsList;
                console.log(allPositions);
                for (var x = 0; x < allEmployees.length; x++) {
                    var found = false;
                    for (var y = 0; y < allPositions.length; y++) {
                        if (allEmployees[x].FTEPositionID == allPositions[y].Id) {
                            allEmployees[x].FTEPositionName = allPositions[y].PositionDescription;
                            found = true;
                            console.log(allEmployees[x]);
                            break;
                        }
                    }
                    if (!found) {
                        allEmployees[x].FTEPositionName = 'N/A';
                    }
                }

                if ($scope.description && $scope.description[id]) {

                    console.log($scope.description[1], id);
                    //luan quest 3/26
                    if ($scope.description[id].name != '----------Add New----------') {
                        // Jignesh-26-03-2021
                        var authRole = $scope.localStorageSrevice.get('authorizationData').role;
                        if (authRole === "Admin") {
                            var temp = { name: '----------Add New----------', value: '0' };
                            resultAr.push(temp);
                        }
                        
                    }

                    for (var x = 0; x < allEmployees.length; x++) {
                        console.log(allEmployees[x], $scope.description[id].name);
                        if (allEmployees[x].FTEPositionName == $scope.description[id].name) {
                            var temp = { name: '', value: '' };
                            console.log(allEmployees[x]);
                            temp.name = allEmployees[x].Name;
                            temp.value = allEmployees[x].ID;
                            resultAr.push(temp);
                        }
                    }
                }
                return resultAr;
            }

            //luan quest 3/26
            function filterSubcontractorBySubcontractorType(id, subcontractorList) {
                var resultAr = [];
                var allSubcontractorTypes = $scope.orgSubcontractorTypes;
                var allSubcontractors = subcontractorList;
                console.log(allSubcontractorTypes);
                console.log(allSubcontractors);
                for (var x = 0; x < allSubcontractors.length; x++) {
                    var found = false;
                    for (var y = 0; y < allSubcontractorTypes.length; y++) {
                        if (allSubcontractors[x].SubcontractorTypeID == allSubcontractorTypes[y].SubcontractorTypeID) {
                            allSubcontractors[x].SubcontractorTypeName = allSubcontractorTypes[y].SubcontractorTypeName;
                            found = true;
                            console.log(allSubcontractors[x]);
                            break;
                        }
                    }
                    if (!found) {
                        allSubcontractors[x].SubcontractorTypeName = 'N/A';
                    }
                }
                console.log(allSubcontractors);

                if ($scope.description && $scope.description[id]) {

                    //luan quest 3/26
                    if ($scope.description[id].name != '----------Add New----------') {
                        // Jignesh-26-03-2021
                        var authRole = $scope.localStorageSrevice.get('authorizationData').role;
                        if (authRole === "Admin") {
                            var temp = { name: '----------Add New----------', value: '0' };
                            resultAr.push(temp);
                        }
                        
                    }

                    for (var x = 0; x < allSubcontractors.length; x++) {
                        console.log(allSubcontractors[x], $scope.description[id].name);
                        if (allSubcontractors[x].SubcontractorTypeName == $scope.description[id].name) {
                            var temp = { name: '', value: '' };
                            console.log(allSubcontractors[x]);
                            temp.name = allSubcontractors[x].SubcontractorName;
                            temp.value = allSubcontractors[x].SubcontractorID;
                            resultAr.push(temp);
                        }
                    }
                }

                console.log(resultAr);
                return resultAr;
            }

            function getNewSubcontractors(id) {
                var resultAr = [];
                var allSubcontractorTypes = $scope.orgSubcontractorTypes;
                var allSubcontractors = $scope.orgSubcontractors;
                console.log(allSubcontractorTypes);
                console.log(allSubcontractors);
                for (var x = 0; x < allSubcontractors.length; x++) {
                    var found = false;
                    for (var y = 0; y < allSubcontractorTypes.length; y++) {
                        if (allSubcontractors[x].SubcontractorTypeID == allSubcontractorTypes[y].SubcontractorTypeID) {
                            allSubcontractors[x].SubcontractorTypeName = allSubcontractorTypes[y].SubcontractorTypeName;
                            found = true;
                            console.log(allSubcontractors[x]);
                            break;
                        }
                    }
                    if (!found) {
                        allSubcontractors[x].SubcontractorTypeName = 'N/A';
                    }
                }
                console.log(allSubcontractors);

                if ($scope.description && $scope.description[id]) {

                    //luan quest 3/26
                    if ($scope.description[id].name != '----------Add New----------') {
                        // Jignesh-26-03-2021
                        var authRole = $scope.localStorageSrevice.get('authorizationData').role;
                        if (authRole === "Admin") {
                            var temp = { name: '----------Add New----------', value: '0' };
                            resultAr.push(temp);
                        }
                        
                    }

                    for (var x = 0; x < allSubcontractors.length; x++) {
                        console.log(allSubcontractors[x], $scope.description[id].name);
                        if (allSubcontractors[x].SubcontractorTypeName == $scope.description[id].name) {
                            var temp = { name: '', value: '' };
                            console.log(allSubcontractors[x]);
                            temp.name = allSubcontractors[x].SubcontractorName;
                            temp.value = allSubcontractors[x].SubcontractorID;
                            resultAr.push(temp);
                        }
                    }
                }

                console.log(resultAr);
                return resultAr;
            }

            //luan quest 3/26
            function filterMaterialByMaterialCategory(id, materialList) {
                var resultAr = [];
                var allMaterialCategories = materialCategoryList;
                var allMaterials = materialList;
                console.log(allMaterialCategories);
                console.log(allMaterials);
                for (var x = 0; x < allMaterials.length; x++) {
                    var found = false;
                    for (var y = 0; y < allMaterialCategories.length; y++) {
                        if (allMaterials[x].MaterialCategoryID == allMaterialCategories[y].ID) {
                            allMaterials[x].MaterialCategoryName = allMaterialCategories[y].Name;
                            found = true;
                            console.log(allMaterials[x]);
                            break;
                        }
                    }
                    if (!found) {
                        allMaterials[x].MaterialCategoryName = 'N/A';
                    }
                }

                if ($scope.description && $scope.description[id]) {

                    //luan quest 3/26
                    if ($scope.description[id].name != '----------Add New----------') {
                        // Jignesh-26-03-2021
                        var authRole = $scope.localStorageSrevice.get('authorizationData').role;
                        if (authRole === "Admin") {
                            var temp = { name: '----------Add New----------', value: '0' };
                            resultAr.push(temp);
                        }
                        
                    }

                    for (var x = 0; x < allMaterials.length; x++) {
                        console.log(allMaterials[x], $scope.description[id].name);
                        if (allMaterials[x].MaterialCategoryName == $scope.description[id].name) {
                            var temp = { name: '', value: '' };
                            console.log(allMaterials[x]);
                            temp.name = allMaterials[x].Name;
                            temp.value = allMaterials[x].ID;
                            resultAr.push(temp);
                        }
                    }
                }
                return resultAr;
            }

            function getNewMaterials(id) {
                var resultAr = [];
                var allMaterialCategories = materialCategoryList;
                var allMaterials = $scope.orgMaterials;
                console.log(allMaterialCategories);
                console.log(allMaterials);
                for (var x = 0; x < allMaterials.length; x++) {
                    var found = false;
                    for (var y = 0; y < allMaterialCategories.length; y++) {
                        if (allMaterials[x].MaterialCategoryID == allMaterialCategories[y].ID) {
                            allMaterials[x].MaterialCategoryName = allMaterialCategories[y].Name;
                            found = true;
                            console.log(allMaterials[x]);
                            break;
                        }
                    }
                    if (!found) {
                        allMaterials[x].MaterialCategoryName = 'N/A';
                    }
                }

                if ($scope.description && $scope.description[id]) {

                    //luan quest 3/26
                    if ($scope.description[id].name != '----------Add New----------') {
                        // Jignesh-26-03-2021
                        var authRole = $scope.localStorageSrevice.get('authorizationData').role;
                        if (authRole === "Admin") {
                            var temp = { name: '----------Add New----------', value: '0' };
                            resultAr.push(temp);
                        }
                        
                    }

                    for (var x = 0; x < allMaterials.length; x++) {
                        console.log(allMaterials[x], $scope.description[id].name);
                        if (allMaterials[x].MaterialCategoryName == $scope.description[id].name) {
                            var temp = { name: '', value: '' };
                            console.log(allMaterials[x]);
                            temp.name = allMaterials[x].Name;
                            temp.value = allMaterials[x].ID;
                            resultAr.push(temp);
                        }
                    }
                }
                return resultAr;
            }

            //Function to calculate dollar value at trend level
            function calculateTrendValue() {
                var total = 0;
                $scope.scheduleGanttInstance.eachTask(function (task) {
                    if (task.$level == 1) {
                        var cost = task.totalCost;
                        total += parseFloat(cost);
                    }
                });
                project["totalCost"] = roundToTwo(total);//Math.round(total);

                //luan here - temporarily
                //if (parseFloat(threshold) > total) {
                //    $("#approveBtn").show();
                //} else {
                //    $("#approveBtn").hide();
                //}
            }

            //Calculate the phase original start and end dates
            function getPhaseOriginalStartEndDate(phaseID, activities) {
                var hasActivity = false;
                var maxDate = new Date(8640000000000000);
                var minDate = new Date(-8640000000000000);
                var originalStartDate = maxDate;
                var originalEndDate = minDate;
                console.log(activities);
                for (var x = 0; x < activities.length; x++) {
                    if (activities[x].originalStartDate == "Invalid date" || activities[x].original_end_date == "Invalid date") {
                        continue;
                    }
                    var d1 = null;
                    var d2 = null;

                    d1 = new Date(activities[x].originalStartDate);
                    d2 = new Date(activities[x].originalEndDate);

                    if (d1 == 'Invalid Date' || d2 == 'Invalid Date') {
                        continue;
                    }

                    if (activities[x].PhaseCode == phaseID) {
                        console.log(originalStartDate, d1);
                        originalStartDate = new Date(Math.min(originalStartDate.getTime(), d1.getTime()));
                        originalEndDate = new Date(Math.max(originalEndDate.getTime(), d2.getTime()));

                        hasActivity = true;
                    }
                }

                if (!hasActivity) {
                    originalStartDate = 'N/A';
                    originalEndDate = 'N/A';
                }

                if (originalStartDate == 'Invalid Date') {
                    originalStartDate = 'N/A';
                }
                if (originalEndDate == 'Invalid Date') {
                    originalEndDate = 'N/A';
                }

                return {
                    originalStartDate: originalStartDate,
                    originalEndDate: originalEndDate
                }
            }

            //Calculate the project original start and end dates
            function getProjectOriginalStartEndDate(activities) {
                var maxDate = new Date(8640000000000000);
                var minDate = new Date(-8640000000000000);
                var originalStartDate = maxDate;
                var originalEndDate = minDate;
                var hasActivity = false;
                for (var x = 0; x < activities.length; x++) {
                    var d1 = new Date(activities[x].originalStartDate);
                    var d2 = new Date(activities[x].originalEndDate);

                    if (d1 == 'Invalid Date' || d2 == 'Invalid Date') {
                        continue;
                    }

                    //d1.setDate(d1.getDate() + 1);
                    //d2.setDate(d2.getDate() + 1);

                    originalStartDate = new Date(Math.min(originalStartDate.getTime(), d1.getTime()));
                    originalEndDate = new Date(Math.max(originalEndDate.getTime(), d2.getTime()));

                    hasActivity = true;
                }

                if (!hasActivity) {
                    originalStartDate = 'N/A';
                    originalEndDate = 'N/A';
                }

                if (originalStartDate == 'Invalid Date') {
                    originalStartDate = 'N/A';
                }
                if (originalEndDate == 'Invalid Date') {
                    originalEndDate = 'N/A';
                }

                return {
                    originalStartDate: originalStartDate,
                    originalEndDate: originalEndDate
                }
            }

            //Get average percentage for a phase
            function getPhasePercentageCompletion(phaseID, activities) {
                var activitiesPerPhaseCounter = 0;
                var numerator = 0;
                var denominator = 0;
                console.log(phaseID, activities);
                for (var x = 0; x < activities.length; x++) {
                    if (phaseID == activities[x].PhaseCode) {
                        numerator += parseInt(activities[x].percentage_completion, 10) * parseInt(activities[x].totalCost, 10);
                        denominator += parseInt(activities[x].totalCost, 10);
                        activitiesPerPhaseCounter++;
                    }
                }

                if (denominator == 0) {
                    return '0%';
                } else {
                    return roundToTwo(numerator / denominator) + '%';//Math.round(numerator / denominator) + '%';
                }
            }

            //Get average percentage for Project
            function getProjectPercentageCompletion(activities) {
                var phases = delayedData[1].result; //phases

                var denominator = 0;
                var numerator = 0;
                for (var x = 0; x < activities.length; x++) {
                    numerator += parseInt(activities[x].percentage_completion, 10) * parseInt(activities[x].totalCost, 10);
                    denominator += parseInt(activities[x].totalCost, 10);
                }
                if (denominator == 0) {
                    return '0%';
                } else {
                    //return Math.round(total / activities.length) + '%';
                    return roundToTwo(numerator / denominator) + '%';//Math.round(numerator / denominator) + '%';
                }
            }

            //Date formatter
            function toShortFormat(date) {
                var month_names = ["Jan", "Feb", "Mar",
                    "Apr", "May", "Jun",
                    "Jul", "Aug", "Sep",
                    "Oct", "Nov", "Dec"];
                console.log(date, date.getDate().toString().length);
                var day = '0'.repeat(2 - date.getDate().toString().length) + date.getDate();
                //var day = date.getDate();
                var month_index = date.getMonth();
                var year = date.getFullYear();

                return "" + day + " " + month_names[month_index] + " " + year;
            }

            //FUNCTION check valid input of textboxvalues
            function isValidInput() {
                var isValid = false;
                for (var i = 1; i < $scope.textBoxValues.length; i++) {
                    for (var j = 0; j < $scope.textBoxValues[i].length; j++) {
                        if (isNaN($scope.textBoxValues[i][j]) == true) {
                            isValid = true;
                        }
                    }
                }
                if (isValid == true) {
                    return false;
                }
                return true;
            }

            //UpdateTrendDate - used to update changes on scheduler date
            function updateTrendDate() {
                $scope.scheduleGanttInstance.eachTask(function (task) {
                    if (task.id === projectMaxId) {
                        //var start = moment(task.start_date).format(sqlDateFormat);
                        var start = task.start_date.toISOString().substring(0, 10);
                        //var end = moment(task.end_date).format(sqlDateFormat);
                        var end = task.end_date.toISOString().substring(0, 10);
                        var diff = moment(end).diff(moment(start), 'days');
                        if (diff > 1) {
                            start = (moment(start).isoWeekday() === 1) ? start
                                : moment(start).startOf('isoWeek').format(sqlDateFormat);
                            end = (moment(end).isoWeekday() === 7) ? end
                                : moment(end).endOf('isoWeek').format(sqlDateFormat);
                        }
                        var currentDate = new Date();
                        var createdOn = moment(currentDate).format(sqlDateFormat);
                        var obj = {
                            "Operation": 4,
                            "TrendDescription": $scope.trend.TrendDescription,
                            "ProjectID": delayedData[2].result[0].ProjectID,
                            "TrendNumber": delayedData[3],
                            "CreatedOn": createdOn,
                            "PostTrendStartDate": start,
                            "PostTrendEndDate": end
                        }
                        Trend.persist().save(obj, function (response) {
                            //getProjectDurationAndCost();
                            if ($scope.scheduleGanttInstance.isTaskExists($scope.selectedActivity.id) == true)
                                $scope.scheduleGanttInstance.selectTask($scope.selectedActivity.id);
                        });
                    }
                });
            }

            //Padding for TotalCost so that it will alwasy align with the cost
            function paddingLabel() {
                $timeout(function () {
                    var col2 = $('.col-sm-2').css('width');
                    var col1 = $('.col-sm-1').css('width');
                    var total_label = $("#total_label").css('width');
                    var total_cost = $("#total_cost").css('width');

                    col2 = col2.substring(0, col2.length - 2);
                    col1 = col1.substring(0, col1.length - 2);
                    total_label = total_label.substring(0, total_label.length - 2);
                    total_cost = total_cost.substring(0, total_cost.length - 2);
                    var padding = 700 - (Number(col2) + Number(col1) + Number(total_cost) + Number(total_label)) - 20;
                    $("#bfc-container").css('padding-left', padding.toString() + "px");
                }, 100);
            }

            //UpdatePhase - Update the duration for each phase everytime an activity start-end date changes
            function updatePhase() {
                if ($scope.isBaseline == true) {
                    $scope.scheduleGanttInstance.eachTask(function (task) {
                        if (task.id >= 1000 && task.id < projectMaxId) {
                            if (task.id === 1000) {
                                $scope.planning_start_date = moment(task.start_date).format("MM/DD/YY");
                                $scope.planning_end_date = moment(task.end_date).format("MM/DD/YY");
                            } else if (task.id === 2000) {
                                $scope.schematic_design_start_date = moment(task.start_date).format("MM/DD/YY");
                                $scope.schematic_design_end_date = moment(task.end_date).format("MM/DD/YY");
                            }
                            else if (task.id === 3000) {
                                $scope.design_bidding_start_date = moment(task.start_date).format("MM/DD/YY");
                                $scope.design_bidding_end_date = moment(task.end_date).format("MM/DD/YY");
                            }
                            else if (task.id === 4000) {
                                $scope.construction_start_date = moment(task.start_date).format("MM/DD/YY");
                                $scope.construction_end_date = moment(task.end_date).format("MM/DD/YY");
                            }
                            else if (task.id === 5000) {
                                $scope.closeout_start_date = moment(task.start_date).format("MM/DD/YY");
                                $scope.closeout_end_date = moment(task.end_date).format("MM/DD/YY");
                            }
                        }
                    });
                } else {
                    $scope.scheduleGanttInstance.eachTask(function (task) {
                        if (task.id >= 1000 && task.id < projectMaxId) {
                            var start = moment(task.start_date).format(sqlDateFormat);
                            var end = moment(task.end_date).format(sqlDateFormat);
                            var diff = moment(end).diff(moment(start), 'days');
                            if (diff > 1) {
                                start = (moment(start).isoWeekday() === 1) ? start
                                    : moment(start).startOf('isoWeek').format(sqlDateFormat);
                                end = (moment(end).isoWeekday() === 7) ? end
                                    : moment(end).endOf('isoWeek').format(sqlDateFormat);
                            }
                            //var difference = moment(end).format("days") - moment(start).format("days");
                            var difference = moment(end).diff(moment(start), 'days') + 1;
                            if (difference == 2) difference = 0;
                            if (task.id === 1000) {
                                $scope.planning_duration = difference.toString() + " days";
                            } else if (task.id === 2000) {
                                $scope.schematic_design_duration = difference.toString() + " days";
                            }
                            else if (task.id === 3000) {
                                $scope.design_bidding_duration = difference.toString() + " days";
                            }
                            else if (task.id === 4000) {
                                $scope.construction_duration = difference.toString() + " days";
                            }
                            else if (task.id === 5000) {
                                $scope.closeout_duration = difference.toString() + " days";
                            }
                        }
                    });
                }
            }

            //get Business days
            function getWorkingDaysOfWeek(start, end) {
                var difference = moment(end).diff(moment(start), 'weeks') + 1;
                var numberOfWorkingDays = [];
                var startOfWeek = start;
                var endOfWeek = moment(startOfWeek).clone().endOf('week').format(sqlDateFormat);
                for (var i = 0; i < difference; i++) {
                    var days = moment(endOfWeek).diff(moment(startOfWeek), 'days') + 1;
                    var countDays = 0;
                    for (var j = 0; j < days; j++) {
                        if (moment(startOfWeek).isoWeekday() != 6 && moment(startOfWeek).isoWeekday() != 7) {
                            countDays += 1;
                        }
                        startOfWeek = moment(startOfWeek).add(1, 'days').format(sqlDateFormat);
                    }
                    numberOfWorkingDays.push(countDays);
                    var w = moment(startOfWeek).clone().endOf('week').format(sqlDateFormat);
                    if (moment(w).diff(moment(end), 'days') > 0) {
                        endOfWeek = end;
                    } else {
                        endOfWeek = w;
                    }
                }
                return numberOfWorkingDays.toString();
            }

            function getWorkingDaysOfYear(start, end) {
                var difference = ((moment(end).format('YYYY') - moment(start).format('YYYY')));
                var numberOfWorkingDays = [];
                if (difference == 0) {
                    var countDays = 0;
                    difference = moment(end).diff(moment(start), 'days') + 1;
                    var startOfYear = start;
                    for (i = 0; i < difference; i++) {
                        if (moment(startOfYear).isoWeekday() != 6 && moment(startOfYear).isoWeekday() != 7) {
                            countDays += 1;
                        }
                        startOfYear = moment(startOfYear).add(1, 'days').format(sqlDateFormat);
                    }
                    numberOfWorkingDays.push(countDays);
                } else {
                    difference += 1;
                    var startOfYear = start;
                    var endOfYear = moment(startOfYear).clone().endOf('year').format(sqlDateFormat);

                    for (var i = 0; i < difference; i++) {
                        var days = moment(endOfYear).diff(moment(startOfYear), 'days') + 1;
                        var countDays = 0;
                        for (var j = 0; j < days; j++) {
                            if (moment(startOfYear).isoWeekday() != 6 && moment(startOfYear).isoWeekday() != 7) {
                                countDays += 1;
                            }
                            startOfYear = moment(startOfYear).add(1, 'days').format(sqlDateFormat);
                        }
                        numberOfWorkingDays.push(countDays);
                        var w = moment(startOfYear).clone().endOf('year').format(sqlDateFormat);
                        if (moment(w).diff(moment(end), 'days') > 0) {
                            endOfYear = end;
                        } else {
                            endOfYear = w;
                        }
                    }
                }
                return numberOfWorkingDays.toString();
            }

            function getFirstDayOfMonth(date) {
                return moment(date).startOf('month').format(sqlDateFormat);
            }

            function getLastDayOfMonth(date) {
                return moment(date).endOf('month').format(sqlDateFormat);
            }

            function getFirstDayOfYear(date) {
                return moment(date).startOf('year').format(sqlDateFormat);
            }

            function getLastDayOfYear(date) {
                return moment(date).endOf('year').format(sqlDateFormat);
            }

            function getWorkingDays(start, end, granularity) {
                var difference = ((moment(end).format('YYYY') - moment(start).format('YYYY')) * 12) + (moment(end).format('MM') - moment(start).format('MM') + 1);

                var startDate = moment(start).format(sqlDateFormat);
                var numberOfWorkingDays = [];
                var startOfMonth = start;
                var endOfMonth = moment(start).clone().endOf('month').format(sqlDateFormat);

                if (endOfMonth > moment(end).format(sqlDateFormat)) {
                    endOfMonth = moment(end).format(sqlDateFormat);
                }
                for (var i = 0; i < difference; i++) {

                    var daysDifference = moment(endOfMonth).diff(moment(startOfMonth), 'days') + 1;
                    var countDays = 0;
                    for (var j = 0; j < daysDifference; j++) {

                        if (moment(startOfMonth).isoWeekday() != 6 && moment(startOfMonth).isoWeekday() != 7) {
                            countDays += 1;
                        }
                        startOfMonth = moment(startOfMonth).add(1, 'days').format(sqlDateFormat);
                    }
                    //startOfMonth  = moment(startOfMonth).add(1,'days').format(sqlDateFormat);
                    numberOfWorkingDays.push(countDays);

                    var s = moment(startOfMonth).clone().endOf('month').format(sqlDateFormat);
                    if ((moment(s).diff(moment(end), 'days')) > 0) {
                        endOfMonth = end;
                    } else {
                        endOfMonth = s;
                    }

                }
                return numberOfWorkingDays.toString();

            }

            function isZeroArray(arr) {
                var isZero = true;

                for (var i = 0; i < arr.length - 1; i++) {
                    if (arr[i] != 0) {
                        isZero = false;
                        break;
                    }
                }
                return isZero;
            }

            function getNumbOfTextBox(taskStartDate, taskEndDate) {
                var diff = 0;
                if ($scope.scheduleScale == 'week') {
                    diff = getWeekDifferences(taskStartDate, taskEndDate);
                } else if ($scope.scheduleScale == 'month') {
                    diff = getMonthDifferences(moment(taskStartDate).startOf('month'), moment(taskEndDate).endOf('month'));
                    diff += 1;
                } else if ($scope.scheduleScale == 'year') {
                    diff = getYearDifference(moment(taskStartDate).startOf('year'), moment(taskEndDate).endOf('year'));
                    diff += 1;
                }

                return diff;
            }

            //luan here 4/23
            function costCalculation(id, obj, i) {//index) {

                console.log(obj);
                console.log(id);
                console.log($scope.textBoxIds);
                if ($scope.textBoxIds[id].length == 0 || isZeroArray($scope.textBoxIds[id])) {
                    //temporary code for
                    if ($scope.textBoxIds.length > 2) {
                        var textIds = [];
                        var diff = getNumbOfTextBox($scope.selectedActivity.start_date, $scope.selectedActivity.end_date);
                        for (var i = 0; i < diff; i++) {
                            textIds[i] = i;
                        }
                        $scope.textBoxIds[id] = textIds;
                    } else {
                        $scope.textBoxIds[id] = angular.copy($scope.textBoxIds[1]);
                    }
                }
                if (obj) {
                    //$scope.selectedCost = obj;
                    id = obj.id;
                } else {
                    obj = $scope.selectedCost;
                }
                console.log($scope.selectedCost);
                if (!$scope.selectedCost)
                    $scope.selectedCost = obj;
                if ($scope.selectedCost) {
                    var start = moment($scope.selectedCost.start_date).format(sqlDateFormat);
                    var end = moment($scope.selectedCost.end_date).format(sqlDateFormat);
                    // Pritesh2 Here value get change
                    var total = 0;
                    var EAC = 0;
                    if ($scope.method[id] === "F") {
                        var multiplier = 1;
                        var original_end_date = getActivityOriginalStartDate($scope.selectedActivity.original_end_date);
                        var original_start_date = getActivityOriginalStartDate($scope.selectedActivity.original_start_date);
                        var start_date = moment($scope.selectedActivity.start_date).format(sqlDateFormat);
                        start_date = (moment(start_date).isoWeekday() === 1) ? start_date
                            : moment(start_date).startOf('isoWeek').format(sqlDateFormat);
                        var activity_original_start_date = (moment(original_start_date).isoWeekday() === 1) ? original_start_date
                            : moment(original_start_date).startOf('isoWeek').format(sqlDateFormat);
                        if ($scope.direction == "left-left" || $scope.direction == "left-right") {
                            original_start_date = $scope.selectedActivity.start_date;
                            if (moment(original_start_date).isoWeekday() != 1) {
                                original_start_date = moment(original_start_date).clone().startOf('isoWeek');
                            }
                        }

                        if ($scope.direction === "right-right" || $scope.direction === "right-left") {
                            original_end_date = $scope.selectedActivity.end_date;
                            if (moment(original_end_date).isoWeekday() != 7) {
                                original_end_date = moment(original_end_date).clone().endOf('isoWeek');
                            }
                        }
                        if ($scope.scheduleScale === "week") {

                            var numb = getWorkingDaysOfWeek(original_start_date, original_end_date);
                            var workingDays = numb.split(',');
                            if ($scope.direction === "left-right") {
                                var diff = Math.abs(getWeekDifferences(activity_original_start_date, start_date));
                                for (var i = 0; i < diff; i++) {
                                    workingDays.splice(0, 0, 0);
                                }
                            }
                            //2. get working hours - # of units
                            multiplier = 0;
                            for (var j = 0; j < workingDays.length; j++) {
                                multiplier += parseInt(workingDays[j]);
                            }
                            if (!$scope.fteHours[id])
                                $scope.fteHours = [[]];
                            //$scope.fteHours[id] = [];
                            $.each($scope.textBoxValues[id], function (index) {

                                $scope.fteHours[id][index] = parseFloat(this) * workingDays[index] * 8;
                                total += $scope.fteHours[id][index];
                            });
                        }
                        else if ($scope.scheduleScale === "month") {
                            if ($scope.selectedCost.isSelected && $scope.isNewCost[$scope.selectedCost.id]) {
                                var firstDayOfMonth = original_start_date;
                                var lastDayOfMonth = original_end_date;
                            } else {
                                var firstDayOfMonth = getFirstDayOfMonth(original_start_date);
                                var lastDayOfMonth = getLastDayOfMonth(original_end_date);
                            }

                            var numberOfWorkingDays = getWorkingDays(original_start_date, original_end_date);
                            //var numberOfWorkingDays = getWorkingDays(firstDayOfMonth, lastDayOfMonth);
                            var days = numberOfWorkingDays.split(',');
                            if ($scope.direction === "left-right") {
                                var abStart = getAbsoulteMonths(activity_original_start_date);
                                var abEnd = getAbsoulteMonths(start_date);
                                var diff = Math.abs(abStart - abEnd);
                                for (var i = 0; i < diff; i++) {
                                    days.splice(0, 0, 0);
                                }
                            }
                            $.each($scope.textBoxValues[id], function (index) {
                                $scope.fteHours[id][index] = parseFloat(this) * (parseFloat(days[index]) * 8);
                                total += $scope.fteHours[id][index];
                            });
                        }
                        else if ($scope.scheduleScale === "year") {
                            if ($scope.selectedCost.isSelected && $scope.isNewCost[$scope.selectedCost.id]) {
                                firstDayOfYear = original_start_date;
                                lastDayOfYear = original_end_date;
                            } else {
                                var firstDayOfYear = getFirstDayOfYear(original_start_date);
                                var lastDayOfYear = getLastDayOfYear(original_start_date);
                            }
                            //var numberOfWorkingDays = getWorkingDaysOfYear(firstDayOfYear, lastDayOfYear); //New
                            var numberOfWorkingDays = getWorkingDaysOfYear(original_start_date, original_end_date);
                            var days = numberOfWorkingDays.split(',');
                            if ($scope.direction === "left-right") {
                                var diff = Math.abs(moment(start_date).format("YYYY") - moment(activity_original_start_date).format("YYYY"));

                                for (var i = 0; i < diff; i++) {
                                    days.splice(0, 0, 0);
                                }

                            }
                            $.each($scope.textBoxValues[id], function (index) {
                                $scope.fteHours[id][index] = parseFloat(this) * (parseFloat(days[index]) * 8);
                                total += $scope.fteHours[id][index];
                            });
                        }

                        $scope.totalUnits[id] = roundToTwo(total);//total.toFixed((total % 1 > 0) ? 2 : 0);//Math.round(total);

                        console.log('total here');
                        total = 0;
                        $.each($scope.fteHours[id], function (index) {
                            //luan here 4/23 - use budget unit costs if clicking on a-e to edit etc
                            var unitCost = $scope.unitCost[id];
                            if ($scope.isCostEdited[id] && !isCostSaving)
                                unitCost = $scope.getCostWithOverhead(unitCost, 'F');
                            //if ($scope.CostData[id - 1] && $scope.CostData[id - 1].CostType == 'F' && $scope.trend.TrendNumber == 1000) {
                            //    unitCost = ($scope.CostData[id - 2]) ? $scope.CostData[id - 2].Base : 1;
                            //}
                            console.log(unitCost);

                            $scope.fteCosts[id][index] = parseFloat(this) * unitCost;
                            total += $scope.fteCosts[id][index];
                        });
                        if (obj && obj.cost_track_type_id && //EAC
                            (obj.cost_track_type_id == $scope.costTrackType.ACTUAL || obj.cost_track_type_id == $scope.costTrackType.ESTIMATE_TO_COMPLETION)) {
                            if ($scope.isCostEdited[id] && !isCostSaving)
                                $scope.totalCost[id] = roundToTwo(total);
                            else
                                $scope.totalCost[id] = roundToTwo($scope.getCostWithOverhead(total, 'F'));//total.toFixed((total % 1 > 0) ? 2 : 0);//Math.round(total);
                            $scope.totalBudget[id] = 0;

                        } else {

                            $scope.totalCost[id] = roundToTwo(total);//total.toFixed((total % 1 > 0) ? 2 : 0);//Math.round(total);
                            if ($scope.isCostEdited[id] && !isCostSaving)
                                $scope.totalBudget[id] = roundToTwo(total);
                            else
                                $scope.totalBudget[id] = roundToTwo($scope.getCostWithOverhead($scope.totalCost[id], 'F'));
                        }
                        //$scope.totalBudget[id] = ($scope.isBillableRate) ? $scope.totalCost[id] * LABOR_RATE : $scope.totalCost[id];
                        //$scope.unitBudget[id] = ($scope.isBillableRate) ? $scope.unitCost[id] * LABOR_RATE : $scope.unitCost[id];

                        $scope.unitBudget[id] = $scope.getCostWithOverhead($scope.unitCost[id], 'F');
                        //luan here 4/23
                        console.log($scope.CostData);
                        if ($scope.CostData[id - 1] && $scope.CostData[id - 1].CostType == 'F' && $scope.trend.TrendNumber == 1000) {
                            var i = id - 1;
                            var costs = $scope.CostData;
                            var CostTrackTypeArray = [];
                            var FTECostArray = [];
                            var FTEHoursArray = [];
                            var averageRate = 0;
                            console.log(costs[i]);

                            if ($scope.CostData[i].CostTrackTypes) {
                                CostTrackTypeArray = costs[i].CostTrackTypes.split(',').map(function (item) {
                                    return item.trim();
                                });;
                            }

                            if (costs[i].FTECost) {
                                FTECostArray = costs[i].FTECost.split(',').map(function (item) {
                                    return item.trim();
                                });;
                            }
                            if (costs[i].FTEHours) {
                                FTEHoursArray = costs[i].FTEHours.split(',').map(function (item) {
                                    return item.trim();
                                });;
                            }

                            if (costs[i].CostTrackTypeID == 3 && costs[i].CostType == 'F') {   //Actual and ETC
                                var tempActualTotal = 0;
                                var cellCount = 0;
                                for (var x = 0; x < FTECostArray.length; x++) {
                                    if (CostTrackTypeArray[x] == '3') {
                                        cellCount++;
                                        tempActualTotal += parseFloat(FTECostArray[x]);
                                    }
                                }

                                var tempETCTotal = 0;
                                for (var y = cellCount; y < $scope.fteCosts[id].length; y++) {
                                    console.log(parseFloat($scope.fteCosts[id][y]));
                                    tempETCTotal += parseFloat($scope.fteCosts[id][y]);
                                }

                                $scope.totalCost[id] = tempActualTotal + tempETCTotal;
                                $scope.totalBudget[id] = 0; //??
                                // $scope.totalBudget[id] = ($scope.isBillableRate) ? $scope.totalCost[id] * LABOR_RATE : $scope.totalCost[id];
                            }
                        }
                    }
                    else if ($scope.method[id] === "L") {
                        total = 0; EAC = 0;
                        $.each($scope.textBoxValues[id], function (index) {
                            total += parseFloat(this);
                        });
                        total = total;//* SUBCONTRACTOR_RATE * CUSTOM_SUBCONTRACTOR_RATE; //IVAN 03-12
                        total = + roundToTwo(total); //total.toFixed((total % 1 > 0) ? 2 : 0);
                        if (obj && obj.cost_track_type_id && //EAC
                            (obj.cost_track_type_id == $scope.costTrackType.ACTUAL || obj.cost_track_type_id == $scope.costTrackType.ESTIMATE_TO_COMPLETION)) {
                            $scope.totalCost[id] = roundToTwo($scope.getCostWithOverhead(total, 'L'));
                            $scope.totalBudget[id] = 0;

                        }
                        else {
                            $scope.totalCost[id] = total;
                            // $scope.totalBudget[id] = $scope.totalCost[id] * SUBCONTRACTOR_RATE * CUSTOM_SUBCONTRACTOR_RATE;
                            $scope.totalBudget[id] = roundToTwo($scope.getCostWithOverhead($scope.totalCost[id], 'L'));
                        }

                    }
                    else if ($scope.method[id] === "U") {   //luan here 4/23
                        var total = 0;
                        var totalUnits = 0.0;

                        $.each($scope.textBoxValues[id], function (index) {
                            $scope.unitCosts[id] = parseFloat(this) + $scope.unitCost[id];	//luan here 5/28 - changed from + to *. Nvm it does nothing

                            totalUnits += parseFloat(this);
                        });

                        totalUnits = roundToTwo(Number(totalUnits));

                        $scope.totalUnits[id] = (totalUnits % 1 > 0) ? roundToTwo(totalUnits) : totalUnits.toFixed(0);
                        //$scope.unitBudget[id] = $scope.unitCost[id] * MATERIAL_RATE;
                        $scope.unitBudget[id] = roundToTwo($scope.getCostWithOverhead($scope.unitCost[id], 'U'));
                        //luan here 4/23
                        var unitCost = $scope.unitCost[id];
                        if ($scope.CostData[id - 1] && $scope.CostData[id - 1].CostType == 'U' && $scope.trend.TrendNumber == 1000) {
                            unitCost = ($scope.CostData[id - 2]) ? $scope.CostData[id - 2].Base : 1;
                        }
                        console.log(unitCost);

                        total = $scope.totalUnits[id] * unitCost;
                        if (obj && obj.cost_track_type_id && //EAC
                            (obj.cost_track_type_id == $scope.costTrackType.ACTUAL || obj.cost_track_type_id == $scope.costTrackType.ESTIMATE_TO_COMPLETION)) {
                            $scope.totalCost[id] = roundToTwo(parseFloat($scope.unitBudget[id] * $scope.totalUnits[id]));
                            $scope.totalBudget[id] = 0;
                        } else {
                            $scope.totalCost[id] = 0;//(total % 1 > 0) ? roundToTwo(total) : total.toFixed(0);
                            //$scope.totalBudget[id] = $scope.totalCost[id] * MATERIAL_RATE;
                            //$scope.totalBudget[id] = $scope.getCostWithOverhead($scope.totalCost[id], 'U');
                            $scope.totalBudget[id] = roundToTwo(parseFloat($scope.unitBudget[id] * $scope.totalUnits[id]));       //Manasi 26-07-2020

                        }

                        //luan here 4/23
                        console.log($scope.CostData);
                        if ($scope.CostData[id - 1] && $scope.CostData[id - 1].CostType == 'U' && $scope.trend.TrendNumber == 1000) {
                            var i = id - 1;
                            var costs = $scope.CostData;
                            var CostTrackTypeArray = [];
                            var MaterialCostWithOverheadArray = [];
                            var MaterialQuantityArray = [];
                            var averageRate = 0;
                            console.log(costs[i]);

                            if (costs[i].CostTrackTypes) {
                                CostTrackTypeArray = costs[i].CostTrackTypes.split(',').map(function (item) {
                                    return item.trim();
                                });;
                            }

                            if (costs[i].CostWithOverhead) {
                                MaterialCostWithOverheadArray = costs[i].CostWithOverhead.split(',').map(function (item) {
                                    return item.trim();
                                });;
                            }
                            if (costs[i].TextBoxValue) {
                                MaterialQuantityArray = costs[i].TextBoxValue.split(',').map(function (item) {
                                    return item.trim();
                                });;
                            }

                            if (costs[i].cost_track_type_id == 3 && costs[i].CostType == 'U') {   //Actual and ETC
                                var tempActualTotal = 0;
                                var tempETCTotal = 0;
                                var cellCount = 0;
                                var totalETCUnits = 0;

                                for (var x = 0; x < MaterialCostWithOverheadArray.length; x++) {
                                    if (CostTrackTypeArray[x] == '3') {
                                        cellCount++;
                                        tempActualTotal += parseFloat(MaterialCostWithOverheadArray[x]);
                                    }
                                }

                                for (var x = cellCount; x < $scope.textBoxValues[id].length; x++) {
                                    console.log(parseFloat($scope.textBoxValues[id][x]));
                                    totalETCUnits += parseFloat($scope.textBoxValues[id][x]);
                                }

                                tempETCTotal = totalETCUnits * unitCost;

                                console.log(tempActualTotal, tempETCTotal);

                                $scope.totalCost[id] = tempActualTotal + tempETCTotal;
                                $scope.totalBudget[id] = 0;
                                //  $scope.totalBudget[id] = $scope.totalCost[id] * MATERIAL_RATE;
                            }
                        }
                    }
                    else if ($scope.method[id] === "ODC") {
                        console.log($scope.textBoxValues);
                        //$scope.totalCost[id] = 'N/A';
                        var total = 0;
                        var totalUnits = 0.0;
                        $.each($scope.textBoxValues[id], function (index) {
                            console.log(this);
                            $scope.unitCosts[id] = parseFloat(this) + $scope.unitCost[id];
                            totalUnits += parseFloat(this);
                        });
                        totalUnits = totalUnits;// * ODC_RATE * CUSTOM_ODC_RATE; //IVAN 03-12
                        totalUnits = roundToTwo(Number(totalUnits));
                        $scope.totalUnits[id] = (totalUnits % 1 > 0) ? roundToTwo(totalUnits) : totalUnits.toFixed(0);
                        console.log('total here');
                        total = $scope.unitCost[id];
                        if (obj && obj.cost_track_type_id && //EAC
                            (obj.cost_track_type_id == $scope.costTrackType.ACTUAL || obj.cost_track_type_id == $scope.costTrackType.ESTIMATE_TO_COMPLETION)) {
                            $scope.totalCost[id] = $scope.getCostWithOverhead(totalUnits, 'ODC');
                            //$scope.totalBudget[id] = $scope.totalCost[id] * ODC_RATE * CUSTOM_ODC_RATE;
                            $scope.totalBudget[id] = 0;
                        } else {
                            $scope.totalCost[id] = totalUnits;
                            //$scope.totalBudget[id] = $scope.totalCost[id] * ODC_RATE * CUSTOM_ODC_RATE;
                            $scope.totalBudget[id] = roundToTwo($scope.getCostWithOverhead($scope.totalCost[id], 'ODC'));
                        }

                        console.log($scope.totalCost[id]);
                    }
                    else if ($scope.method[id] === "P") {
                        total = 0;
                        $.each($scope.textBoxValues[id], function (index) {
                            total += parseFloat(this);
                        });
                        // alert(total);
                        $scope.totalUnits[id] = roundToTwo(total); // Pritesh on 15 Jul 2020 for making unit fixed

                        $scope.totalCost[id] = $scope.totalUnits[id] * $scope.unitCost[id] / 100;

                        console.log('total here');
                    }
                    var total = 0;
                    var totalB = 0;
                    angular.forEach($scope.totalCost, function (item) {
                        if (isNaN(item) == false) {
                            total += parseFloat(item);
                        }
                    });

                    angular.forEach($scope.totalBudget, function (item) {
                        if (isNaN(item) == false) {
                            totalB += parseFloat(item);
                        }
                    });

                    //$scope.allCostTotal = total;//$filter('currency')(total,'$',0);1
                    $scope.allCostTotal = totalB;
                    var amountInput = delayedData[2].result[0].Amount;
                    $scope.amount = amountInput - $scope.allCostTotal;

                    paddingLabel();

                    updateBuffer($scope.selectedCost);
                    return $scope.selectedCost;
                }
            }

            //Function to calculate the activities total value for each phase
            function CalculatePhaseTotal(activities, phaseId) {
                var phaseTotalCost = 0;

                console.log(activities, phaseId);
                angular.forEach(activities, function (activity, value) {
                    console.log(activity);
                    if (parseInt(activity.PhaseCode) === phaseId) {
                        //var fte = (activity.FTECost) ? parseFloat(activity.FTECost) : 0;
                        //var lumpsum = (activity.LumpsumCost) ? parseFloat(activity.LumpsumCost) : 0;
                        //var unit = (activity.UnitCost) ? parseFloat(activity.UnitCost) : 0;
                        //var odc = (activity.OdcCost) ? parseFloat(activity.OdcCost) : 0;
                        //var activityCost = fte + lumpsum + unit + odc;
                        phaseTotalCost += parseFloat(activity.totalCost);
                    }
                });
                console.log(phaseTotalCost);
                return roundToTwo(phaseTotalCost);//Math.round(phaseTotalCost);
            }

            //Function to calculate the activities total value for each phase
            function CalculatePhaseTotalBudget(activities, phaseId, projectID) {
                var phaseTotalCost = 0;
                console.log(activities, phaseId);

                for (var i = 0; i < activities.length; i++) {
                    if (activities[i].ProjectID == projectID) {
                        if (activities[i].PhaseCode == phaseId) {
                            var fteCost = (activities[i].FTECost) ? parseFloat(activities[i].FTEBudget) : 0;
                            var lumpsumCost = (activities[i].LumpsumCost) ? parseFloat(activities[i].LumpsumBudget) : 0;
                            var unitCost = (activities[i].UnitCost) ? parseFloat(activities[i].UnitBudget) : 0;
                            var odcCost = (activities[i].OdcCost) ? parseFloat(activities[i].OdcBudget) : 0;
                            var totalCost = fteCost + lumpsumCost + unitCost + odcCost;
                            phaseTotalCost += totalCost;
                        }
                    }
                }
                console.log(phaseTotalCost);
                var ReturnVal = (Number(phaseTotalCost) % 1 > 0) ? roundToTwo(phaseTotalCost) : phaseTotalCost.toFixed(0);
                //alert(ReturnVal);
                return ReturnVal;
                // alert(phaseTotalCost);
                // return Math.round(phaseTotalCost);
            }

            //luan quest 3
            function CalculatePhaseTotalActualForecast(activities, phaseId) {
                var phaseTotalCostActualForecast = 0;
                angular.forEach(activities, function (activity, value) {
                    if (parseInt(activity.PhaseCode) === phaseId) {
                        phaseTotalCostActualForecast += parseFloat(activity.totalCostActualForecast);
                    }
                });
                var ReturnVal = roundToTwo(phaseTotalCostActualForecast);
                return ReturnVal;
                // return Math.round(phaseTotalCostActualForecast); Pritesh 25Jul2020 as we are keeping decimal aces in costline details so this will miss math
            }

            //Get AuthorizationData
            function getAuthorizationData() {
                return localStorageSrevice.get('authorizationData');
            }
            function CheckIfParentCall() {
                alert("Tested call");
            }
            function applyApprovalProgressBar(trend) {
                //Clear the progress bar

                //luan here - progress bar
                var approvedUserList = [];
                var originalApprovalUserList = [];
                var originalApprovalRoleList = [];

                console.log(trend);

                if (trend.original_approvalList_Role == null) trend.original_approvalList_Role = '';
                originalApprovalRoleList = trend.original_approvalList_Role.split('|');
                console.log(originalApprovalRoleList);

                var allEmployees = [];

                if (trend.original_approvalList_EmployeeID == null) {
                    trend.original_approvalList_EmployeeID = "0";
                }
                if (trend.approvedList_EmployeeID == null) {
                    trend.approvedList_EmployeeID = "0";
                }

                UserByEmployeeListID.get({
                    "EmployeeListID": trend.original_approvalList_EmployeeID,
                    "Dummy": "test"
                }, function (response) {
                    console.log(response.result);
                    originalApprovalUserList = response.result;

                    UserByEmployeeListID.get({
                        "EmployeeListID": trend.approvedList_EmployeeID,
                        "Dummy": "test"
                    }, function (response) {
                        console.log(response.result);
                        approvedUserList = response.result;

                        AllEmployee.get({}, function (response) {
                            $('#approvalProgressBar').find('span').remove()

                            allEmployeeList = response.result;
                            console.log(response.result);
                            var firstAdd = true;
                            for (var x = 0; x < originalApprovalUserList.length; x++) {
                                for (var y = 0; y < allEmployeeList.length; y++) {
                                    if (originalApprovalUserList[x].EmployeeID == allEmployeeList[y].ID) {
                                        //var style = "";  // Swapnil 04-09-2020
                                        var style = "'color: black; background-color:#f9e95c'"; // Swapnil 04-09-2020
                                        if (approvedUserList[x]) {
                                            if (approvedUserList[x].UserID == originalApprovalUserList[x].UserID) {
                                                style = "'color: white; background-color: #00ad00;'";
                                            }
                                        }

                                        if (!originalApprovalRoleList[x]) {
                                            originalApprovalRoleList[x] = "";
                                        }

                                        if (firstAdd) {
                                            $('#approvalProgressBar').append("<span style=" + style + ">" + originalApprovalRoleList[x] + '(' + allEmployeeList[y].Name + ')' + "</span>");
                                            firstAdd = false;
                                        } else {
                                            $('#approvalProgressBar').append("<span style=" + style + ">" + " &#10230; " + originalApprovalRoleList[x] + '(' + allEmployeeList[y].Name + ')' + "</span>");
                                        }
                                    }
                                }
                            }

                            var style = "";
                            if (trend.TrendStatusID == 1) {
                                style = "'color: white; background-color: #00ad00;'";
                            }
                            else {
                                style = "'color: black; background-color:#f9e95c'"; // Swapnil 04-09-2020
                            }

                            $('#approvalProgressBar').append("<span style=" + style + ">" + " &#10230; " + "APPROVED" + "</span>");
                        });
                    });
                });
            }

            function initializeActivities(activities) {
                var newActivities = [];
                var maxPhaseId = 0;

                for (var i = 0; i < activities.length; i++) {
                    var activity = {};
                    activity["id"] = Number(activities[i].ActivityID);
                    activity["update_id"] = Number(activities[i].ActivityID);
                    //activity['color'] = 'red';
                    //escape html tags

                    //Manasi 09-11-2020
                    if (activities[i].TrendNumber == "1000" || activities[i].TrendNumber == "2000" || activities[i].TrendNumber == "3000")
                        activity["text"] = activities[i].BudgetCategory.replace(/&/g, '&#38').replace(/</g, '&#60').replace(/>/g, '&#62');
                    else
                        activity["text"] = activities[i].BudgetCategory.replace(/&/g, '&#38').replace(/</g, '&#60').replace(/>/g, '&#62') + " - " + activities[i].BudgetSubCategory.replace(/&/g, '&#38').replace(/</g, '&#60').replace(/>/g, '&#62');
                    var testDateFormat = "DD MMM YYYY";
                    console.log(activities[i]);

                    var order = 0;
                    var phaseId = 0;
                    //luan finally here - map order(parentid) with phaseid
                    $scope.phases = delayedData[1].result;
                    for (var x = 0; x < $scope.phases.length; x++) {
                        if (x == 0)
                            console.log($scope.phases[x].PhaseID, Number(activities[i].PhaseCode));
                        if ($scope.phases[x].PhaseID == parseInt(Number(activities[i].PhaseCode))) {
                            order = $scope.phases[x].Order;
                            console.log(order);
                        }
                    }

                    activity["originalStartDate"] = moment(activities[i].OriginalActivityStartDate, sqlDateFormat).format(testDateFormat);
                    activity["originalEndDate"] = moment(activities[i].OriginalActivityEndDate, sqlDateFormat).format(testDateFormat);
                    activity["start_date"] = moment(activities[i].ActivityStartDate, sqlDateFormat).format(dateFormat);
                    activity["end_date"] = moment(activities[i].ActivityEndDate, sqlDateFormat).format(dateFormat);
                    activity["original_start_date"] = moment(activities[i].ActivityStartDate, sqlDateFormat).format(dateFormat);
                    activity["original_end_date"] = moment(activities[i].ActivityEndDate, sqlDateFormat).format(dateFormat);
                    for (var p = 0; p < viewPhase.length; p++) {
                        if (activities[i].ProjectID == viewPhase[p].ProjectID) {
                            if (activities[i].PhaseCode == viewPhase[p].PhaseID) {
                                //activity["parent"] = viewPhase[p].id;
                                phaseId = viewPhase[p].id;
                                break;
                            }
                        }
                    }

                    activity["parent"] = phaseId; //Number(activities[i].PhaseCode) * 1000;
                    activity["ActivityStartDate"] = activities[i].ActivityStartDate;
                    activity["ActivityEndDate"] = activities[i].ActivityEndDate;
                    activity["percentage_completion"] = activities[i].PercentageCompletion + '%';
                    activity["BudgetID"] = activities[i].BudgetID;
                    activity["BudgetCategory"] = activities[i].BudgetCategory;
                    activity["BudgetSubCategory"] = activities[i].BudgetSubCategory;
                    if (!activities[i].OriginalActivityStartDate) {
                        activity["originalStartDate"] = 'N/A';
                    }
                    if (!activities[i].OriginalActivityEndDate) {
                        activity["originalEndDate"] = 'N/A';
                    }

                    for (var y = 0; y < projects.length; y++) {
                        if (activities[i].ProjectID == projects[y].project) {
                            activity["program_element"] = projects[y].program_element;
                            activity["program"] = projects[y].program;
                        }
                    }

                    //activity["program_element"] = activities[i].ProgramElementID;
                    //activity["program"] = activities[i].ProgramID;
                    activity["project"] = activities[i].ProjectID;
                    activity["trend"] = activities[i].TrendNumber;
                    activity["phase"] = activities[i].PhaseCode
                    activity["PhaseCode"] = activities[i].PhaseCode;
                    console.log(activities);

                    activity["totalCost"] = calculateActivityCostTotal(activities[i]).toString();
                    activity["totalBudget"] = calculateActivityBudgetTotal(activities[i]).toString();// This needs to be calculated
                    //activity["totalCostActualForecast"] = calculateActivityForecastTotal(activities[i]).toString();
                    /// alert(roundToTwo(calculateActivityForecastTotalNew(activities[i])).toString());
                    activity["totalCostActualForecast"] = roundToTwo(calculateActivityForecastTotal(activities[i])).toString();  //Manasi 09-07-2020
                    // activity["totalCostActualForecast"] = roundToTwo(parseFloat('0'));  //Manasi 22-07-2020
                    $scope.schedule.data.push(activity);
                    newActivities.push(activity);
                }
                return newActivities;
            }
            function initializeProgramElements(programElements) {
                var newprogramElements = [];
                var allProjects = delayedData[2].result;

                var selectedProjects = [];
                for (var i = 0; i < programElements.length; i++) {
                    var programElement = {};
                    for (var j = 0; j < allProjects.length; j++) {
                        var selectedProject = {};
                        if (programElements[i].ProgramElementID == allProjects[j].ProgramElementID) {
                            selectedProject["projectId"] = allProjects[j].ProjectID;
                            //selectedProject["PhaseID"] = allProjects[j].PhaseID;
                            selectedProjects.push(selectedProject);
                        }
                    }
                    //programElement["id"] = (Math.max.apply(Math, selectedProjects.map(function (a) { return a.projectId })) + 1 + i) * 1000;
                    if (selectedProjects.length != 0) {
                        programElement["id"] = (Math.max.apply(Math, selectedProjects.map(function (a) { return a.projectId })) + 1) * 1000;
                        maxProjectId = (Math.max.apply(Math, selectedProjects.map(function (a) { return a.projectId })) + 1);
                    }
                    else {
                        programElement["id"] = (maxProjectId + i) * 1000;
                    }
                    programElement["text"] = programElements[i].ProgramElementName;
                    programElement["type"] = gantt.config.types.project;
                    programElement["open"] = true;
                    programElement["duration"] = 0;
                    programElement["totalCost"] = "" + "";
                    programElement["originalStartDate"] = "" + "";
                    programElement["originalEndDate"] = "" + "";
                    programElement["percentage_completion"] = getProjectPercentageCompletion(activities);
                    programElement["parent"] = contractMaxId;
                    programElement["program_element"] = programElements[i].ProgramElementID;
                    programElement["program"] = programElements[i].ProgramID;
                    var ProgramElementTotalBudget = CalculateProgramElementTotalBudget(activities, programElements[i].ProgramElementID);
                   
                    trendTotalBudget += parseFloat(ProgramElementTotalBudget);
                    programElement["totalBudget"] = ProgramElementTotalBudget
                    //programElement["project"] = projects[i].ProjectID;
                    $scope.schedule.data.push(programElement);
                    newprogramElements.push(programElement);
                    selectedProject = {};
                    selectedProjects = [];
                }
                return newprogramElements;
            }
            function initializeProjects(projects) {
                var newproject = [];
                var allPhases = delayedData[1].result;
                var maxPhaseId = 0;
                var selectedPhases = [];
                for (var i = 0; i < projects.length; i++) {
                    var project = {};
                    for (var j = 0; j < allPhases.length; j++) {
                        var selectedPhase = {};
                        if (projects[i].ProjectID == allPhases[j].ProjectID) {
                            selectedPhase["projectId"] = projects[i].ProjectID;
                            selectedPhase["PhaseID"] = allPhases[j].PhaseID;
                            selectedPhases.push(selectedPhase);
                        }
                    }
                    //project["id"] = (Math.max.apply(Math, selectedPhases.map(function (a) { return a.PhaseID })) + 1 + i) * 1000;
                    if (selectedPhases.length != 0) {
                        project["id"] = (Math.max.apply(Math, selectedPhases.map(function (a) { return a.PhaseID })) + 1 + i) * 1000;
                        maxPhaseId = (Math.max.apply(Math, selectedPhases.map(function (a) { return a.PhaseID })) + 1);
                    }
                    else {
                        project["id"] = (maxPhaseId + i) * 1000;
                    }
                    project["text"] = projects[i].ProjectName;
                    project["type"] = gantt.config.types.project;
                    project["open"] = true;
                    project["duration"] = 0;
                    project["totalCost"] = "" + "";
                    project["originalStartDate"] = "" + "";
                    project["originalEndDate"] = "" + "";
                    project["percentage_completion"] = getProjectPercentageCompletion(activities);
                    for (var p = 0; p < programElements.length; p++) {
                        if (projects[i].ProgramElementID == programElements[p].program_element) {
                            project["parent"] = programElements[p].id;
                            break;
                        }

                    }
                    //project["parent"] = programElementMaxId;
                    project["program_element"] = projects[i].ProgramElementID;
                    project["program"] = projects[i].ProgramID;
                    project["project"] = projects[i].ProjectID;
                    project["totalBudget"] = CalculateProjectTotalBudget(activities, projects[i].ProjectID);
                    $scope.schedule.data.push(project);
                    newproject.push(project);
                    selectedPhase = {};
                    selectedPhases = [];
                }
                return newproject;
            }

            var phases = delayedData[1].result;   //List of Phases

            function initializePhases(phases) {
                var phaseArray = [];
                var viewPhaseArray = [];

                var projectActivity = delayedData[0].result;
                var maxProjectActId = 0;

                for (var j = 0; j < phases.length; j++) {
                    var phase = {};
                    var projectActivities = [];
                    for (var k = 0; k < projectActivity.length; k++) {
                        var projectAct = {};
                        if (phases[j].ProjectID == projectActivity[k].ProjectID) {
                            if (phases[j].PhaseID == projectActivity[k].PhaseCode) {
                                projectAct["projectId"] = phases[j].ProjectID;
                                projectAct["PhaseID"] = phases[j].PhaseID;
                                projectAct["ActivityID"] = projectActivity[k].ActivityID;
                                projectActivities.push(projectAct);
                            }

                        }
                    }
                    //phase["id"] = Number(phases[j].Order) * 1000; //Number(phases[j].PhaseID) * 1000
                    if (projectActivities.length != 0) {
                        phase["id"] = (Math.max.apply(Math, projectActivities.map(function (a) { return a.ActivityID })) + 1 + j) * 1000;
                        maxProjectActId = (Math.max.apply(Math, projectActivities.map(function (a) { return a.ActivityID })) + 1 + j);
                    }
                    else {
                        phase["id"] = (maxProjectActId * 10 + Number(phases[j].Order)) * 1000;
                    }
                    phase["text"] = phases[j].PhaseDescription;
                    phase["type"] = gantt.config.types.project;
                    phase["open"] = true;
                    phase["duration"] = 0;
                    for (var p = 0; p < projects.length; p++) {
                        if (phases[j].ProjectID == projects[p].project) {
                            phase["parent"] = projects[p].id;
                        }

                    }
                    //phase["parent"] = projectMaxId;
                    phase["ActivityPhaseCode"] = phases[j].ActivityPhaseCode;
                    phase["PhaseID"] = phases[j].PhaseID;
                    phase["originalStartDate"] = getPhaseOriginalStartEndDate(phases[j].PhaseID, activities).originalStartDate;
                    phase["originalEndDate"] = getPhaseOriginalStartEndDate(phases[j].PhaseID, activities).originalEndDate;

                    console.log(getPhasePercentageCompletion(phases[j].PhaseID, activities));
                    phase["percentage_completion"] = getPhasePercentageCompletion(phases[j].PhaseID, activities);

                    var phaseTotalCost = CalculatePhaseTotal(activities, phases[j].PhaseID);
                    phase["totalCost"] = phaseTotalCost;
                    trendTotalValue += phaseTotalCost;


                    var phaseTotalBudget = CalculatePhaseTotalBudget(activities, phases[j].PhaseID, phases[j].ProjectID);
                    phase["totalBudget"] = phaseTotalBudget;
                    //trendTotalBudget += parseFloat(phaseTotalBudget);
                    // alert(trendTotalBudget);
                    //luan quest 3
                    var phaseTotalCostActualForecast = CalculatePhaseTotalActualForecast(activities, phases[j].PhaseID);
                    phase["totalCostActualForecast"] = phaseTotalCostActualForecast;
                    //trendTotalValueActual += phaseTotalCostActualForecast;

                    phase["ProjectID"] = phases[j].ProjectID;
                    $scope.schedule.data.push(phase);
                    phaseArray.push(phase);
                    viewPhaseArray.push(phase);
                }
                $scope.trendTotalValue = parseFloat(trendTotalValue);
                //while (phaseArray.length > 0) {
                //    var phasePush = phaseArray[0];
                //    phaseArray.splice(0, 1);
                //    $scope.schedule.data.push(phasePush);
                //}
                return viewPhaseArray;
            }

            function calculateActivityCostTotal(activity) {
                console.log(activity);
                var fteCost = (activity.FTECost) ? parseFloat(activity.FTECost) : 0;
                var lumpsumCost = (activity.LumpsumCost) ? parseFloat(activity.LumpsumCost) : 0;
                var unitCost = (activity.UnitCost) ? parseFloat(activity.UnitCost) : 0;
                var odcCost = (activity.OdcCost) ? parseFloat(activity.OdcCost) : 0;
                var totalCost = fteCost + lumpsumCost + unitCost + odcCost;

                return totalCost.toString();
            }
            function calculateActivityBudgetTotal(activity) {
                console.log(activity);
                var fteCost = (activity.FTECost) ? parseFloat(activity.FTEBudget) : 0;
                var lumpsumCost = (activity.LumpsumCost) ? parseFloat(activity.LumpsumBudget) : 0;
                var unitCost = (activity.UnitCost) ? parseFloat(activity.UnitBudget) : 0;
                var odcCost = (activity.OdcCost) ? parseFloat(activity.OdcBudget) : 0;
                var totalCost = fteCost + lumpsumCost + unitCost + odcCost;

                return totalCost.toString();
            }

            function CalculateProgramElementTotalBudget(activities, programElementID) {
                var phaseTotalCost = 0;
                
                for (var i = 0; i < activities.length; i++) {
                    for (var y = 0; y < projects.length; y++) {
                        if (activities[i].ProjectID === projects[y].ProjectID) {
                            if (projects[y].ProgramElementID == programElementID) {
                                var fteCost = (activities[i].FTECost) ? parseFloat(activities[i].FTEBudget) : 0;
                                var lumpsumCost = (activities[i].LumpsumCost) ? parseFloat(activities[i].LumpsumBudget) : 0;
                                var unitCost = (activities[i].UnitCost) ? parseFloat(activities[i].UnitBudget) : 0;
                                var odcCost = (activities[i].OdcCost) ? parseFloat(activities[i].OdcBudget) : 0;
                                var totalCost = fteCost + lumpsumCost + unitCost + odcCost;
                                phaseTotalCost += totalCost;
                            }
                        }
                    }
                }
                console.log(phaseTotalCost);
                var ReturnVal = (Number(phaseTotalCost) % 1 > 0) ? roundToTwo(phaseTotalCost) : phaseTotalCost.toFixed(0);
                //alert(ReturnVal);
                return ReturnVal;
                // alert(phaseTotalCost);
                // return Math.round(phaseTotalCost);
            }

            function CalculateProjectTotalBudget(activities, projectID) {
                var phaseTotalCost = 0;
                for (var i = 0; i < activities.length; i++) {
                    if (activities[i].ProjectID == projectID) {
                        var fteCost = (activities[i].FTECost) ? parseFloat(activities[i].FTEBudget) : 0;
                        var lumpsumCost = (activities[i].LumpsumCost) ? parseFloat(activities[i].LumpsumBudget) : 0;
                        var unitCost = (activities[i].UnitCost) ? parseFloat(activities[i].UnitBudget) : 0;
                        var odcCost = (activities[i].OdcCost) ? parseFloat(activities[i].OdcBudget) : 0;
                        var totalCost = fteCost + lumpsumCost + unitCost + odcCost;
                        phaseTotalCost += totalCost;
                    }
                }
                console.log(phaseTotalCost);
                var ReturnVal = (Number(phaseTotalCost) % 1 > 0) ? roundToTwo(phaseTotalCost) : phaseTotalCost.toFixed(0);
                //alert(ReturnVal);
                return ReturnVal;
                // alert(phaseTotalCost);
                // return Math.round(phaseTotalCost);
            }

            //-----------------------Manasi-------------------------------------------
            function calculateActivityForecastTotalNew(activity) {
                var fteCost = (activity.FTECost) ? parseFloat(activity.FTECost) : 0;
                var lumpsumCost = (activity.LumpsumCost) ? parseFloat(activity.LumpsumCost) : 0;
                var unitCost = (activity.UnitCost) ? parseFloat(activity.UnitCost) : 0;
                var odcCost = (activity.OdcCost) ? parseFloat(activity.OdcCost) : 0;
                //var totalCost = fteCost + lumpsumCost + unitCost + odcCost;
                var fteCostBudget = (activity.FTECost) ? parseFloat(activity.FTEBudget) : 0;
                var lumpsumCostBudget = (activity.LumpsumCost) ? parseFloat(activity.LumpsumBudget) : 0;
                var unitCostBuget = (activity.UnitCost) ? parseFloat(activity.UnitBudget) : 0;
                var odcCostBudget = (activity.OdcCost) ? parseFloat(activity.OdcBudget) : 0;
                var totalEAC = fteCost + lumpsumCost + unitCost + odcCost + fteCostBudget + lumpsumCostBudget + unitCostBuget + odcCostBudget;

                return totalEAC.toString();
            }
            //-----------------------------------------------------------------------------------

            function calculateActivityForecastTotal(activity) {
                //luan quest 3
                var fteCostActual = (activity.FTECostActual) ? parseFloat(activity.FTECostActual) : 0;
                var lumpsumCostActual = (activity.LumpsumCostActual) ? parseFloat(activity.LumpsumCostActual) : 0;
                var unitCostActual = (activity.UnitCostActual) ? parseFloat(activity.UnitCostActual) : 0;
                var odcCostActual = (activity.OdcCostActual) ? parseFloat(activity.OdcCostActual) : 0;
                var totalCostActual = fteCostActual + lumpsumCostActual + unitCostActual + odcCostActual;

                var fteCostForecast = (activity.FTECostForecast) ? parseFloat(activity.FTECostForecast) : 0;
                var lumpsumCostForecast = (activity.LumpsumCostForecast) ? parseFloat(activity.LumpsumCostForecast) : 0;
                var unitCostForecast = (activity.UnitCostForecast) ? parseFloat(activity.UnitCostForecast) : 0;
                var odcCostForecast = (activity.OdcCostForecast) ? parseFloat(activity.OdcCostForecast) : 0;
                var totalCostForecast = fteCostForecast + lumpsumCostForecast + unitCostForecast + odcCostForecast;

                var totalCostActualForecast = totalCostActual + totalCostForecast;

                return totalCostActualForecast.toString();
            }
            //function calculateActivityBudgetTotal(activity) {

            //}

            function updateTrendValue(costId) {
                var totalCost = 0;
                var totalBudget = 0;
                //  var costActivity = $scope.costGanttInstance.getTask(costId);
                //  var costParent = $scope.scheduleGanttInstance.getTask(costActivity.activity);
                var costParent = $scope.selectedActivity;
                //var costPhase = $scope.scheduleGanttInstance.getTask(costParent.parent); //phaseID
                var costPhase = $scope.selectedPhase;
                $scope.costGanttInstance.eachTask(function (cost) {
                    if (cost.total_cost && (cost.cost_track_type_id == 3 || cost.cost_track_type_id == 4)) {    //luan here - add only for budget for estimates
                        totalCost += parseFloat(cost.total_cost);
                    }
                });

                //Manasi 15-07-2020
                $scope.costGanttInstance.eachTask(function (cost) {
                    if (cost.total_cost && (cost.cost_track_type_id == 1 || cost.cost_track_type_id == 2)) {    //luan here - add only for budget for estimates
                        totalBudget += parseFloat(cost.total_budget);
                    }
                });

                //$scope.allCostTotal = $filter('currency')(totalCost, '$', formatCurrency(totalCost));
                $scope.allCostTotal = $filter('currency')(totalBudget, '$', formatCurrency(totalBudget));    //Manasi 15-07-2020
                var amountInput = delayedData[2].result[0].Amount;
                $scope.amount = amountInput - $scope.allCostTotal;

                paddingLabel();
                costParent["totalCost"] = roundToTwo(totalCost).toString();//Math.round(totalCost).toString();
                //var phaseTotal = CalculatePhaseTotal(activities, parseInt(costActivity.phase));

                var phaseTotalCost = 0;
                $scope.scheduleGanttInstance.eachTask(function (task) {
                    console.log(task);
                    if (task.parent === costPhase.id) {
                        var c = task.totalCost;
                        phaseTotalCost += parseFloat(c);

                    }
                });

                angular.forEach(phases, function (phase) {
                    if (costPhase.id == (phase.Order * 1000)) {   //luan here
                        costPhase["totalCost"] = roundToTwo(phaseTotalCost).toString();//Math.round(phaseTotalCost).toString();
                    }
                });
            }

            function isCostsEdited() {
                //$scope.isCostEdited is not defined yet, which means no activity has been clicked on before.
                if ($scope.isCostEdited == undefined || $scope.isCostEdited == null) {
                    console.log('returning false');
                    return false;
                }
                for (var i = 1; i < $scope.isCostEdited.length; i++) {
                    if (false !== $scope.isCostEdited[i]) {
                        return true;
                    }
                }
                return false;
            }

            function validateCostFields() {
                var isFieldEmptied = false;
                $scope.costGanttInstance.eachTask(function (item) {
                    if ($scope.method[item.id] == "F") {
                        if ($scope.description[item.id].name == "" || $scope.employee_id[item.id] == undefined || $scope.employee_id[item.id].name == "") {
                            isFieldEmptied = true;
                        }
                    } else if ($scope.method[item.id] == "L") {
                        if ($scope.description[item.id].name == "" || $scope.subcontractor_id[item.id] == undefined || $scope.subcontractor_id[item.id].name == "")
                            isFieldEmptied = true;
                    } else if ($scope.method[item.id] == "U") {
                        if ($scope.description[item.id].name == "" || $scope.material_id[item.id] == undefined || $scope.material_id[item.id].name == "" || $scope.unit_type[item.id] == "" || $scope.unitCost[item.id] == "") {//|| item.UnitType == "" || item.unitCost ==""){
                            isFieldEmptied = true;
                        }
                    } else if ($scope.method[item.id] == "ODC") {
                        if ($scope.description[item.id].name == "")
                            isFieldEmptied = true;
                    }
                    if (isFieldEmptied == true) {
                        return true;
                    }
                });

                if (isFieldEmptied == true)
                    return true;
                else
                    return false;

            }

            function getWeekDifferences(startDate, endDate) {
                var st = moment(startDate).format(sqlDateFormat);
                var ed = moment(endDate).format(sqlDateFormat);
                year1 = moment(startDate).format('YYYY');
                year2 = moment(endDate).format('YYYY');
                var ONE_WEEK = 1000 * 60 * 60 * 24 * 7;  //# of seconds in a week
                // Convert both dates to milliseconds
                var date1_ms = moment(ed).format(sqlDateFormat);
                var date2_ms = moment(st).format(sqlDateFormat);
                var d1 = new Date(date1_ms);
                var d2 = new Date(date2_ms);
                // Calculate the difference in milliseconds
                var difference_ms = Math.abs(d1.getTime() - d2.getTime());
                var weekDifference;
                // Convert back to weeks and return hole weeks

                weekDifference = Math.ceil(difference_ms / ONE_WEEK);

                return weekDifference;
            }

            function getMonthDifferences(startDate, endDate) {
                var month1 = moment(startDate).format('MM');
                var month2 = moment(endDate).format('MM');
                var year1 = moment(startDate).format("YYYY");
                var year2 = moment(endDate).format("YYYY");

                var diff = ((year2 - year1) * 12) + (month2 - month1);
                return diff;
            }

            function getYearDifference(startDate, endDate) {
                return Math.abs(moment(endDate).format("YYYY") - moment(startDate).format("YYYY"));
            }

            //Schedule Gantt configuration
            function ConfigScheduleGantt() {
                //luan quest 3 - conditional grid_width
                $scope.first_task_id = 0;   //Manasi 28-07-2020
                var isCurrentTrend = false;
                var isTrue = false;   //Swapnil 24-11-2020
                var grid_width = 900;
                console.log(delayedData);
                if (delayedData[3] == 1000) {
                    isCurrentTrend = true;
                    grid_width = 1000;
                }
                //Swapnil 24-11-2020
                if (delayedData[3] == 3000) {
                    grid_width = 1000;
                    isTrue = true;
                }

                $scope.scheduleGanttInstance.config.drag_links = false;
                //$scope.scheduleGanttInstance.config.show_grid = false;
                $scope.scheduleGanttInstance.config.drag_resize = false;
                $scope.scheduleGanttInstance.config.drag_progress = false;
                $scope.scheduleGanttInstance.config.fit_tasks = true;
                $scope.scheduleGanttInstance.config.scroll_on_click = false;
                $scope.scheduleGanttInstance.config.min_column_width = 120;
                $scope.scheduleGanttInstance.config.initial_scroll = false;
                $scope.scheduleGanttInstance.config.drag_move = false;
                $scope.scheduleGanttInstance.config.autoSize = 'xy';
                $scope.scheduleGanttInstance.config.prevent_default_scroll = true;
                $scope.scheduleGanttInstance.config.task_height = 15;
                $scope.scheduleGanttInstance.config.row_height = 22;
                $scope.scheduleGanttInstance.config.grid_width = grid_width;  //luan quest 3 - original 900
                $scope.scheduleGanttInstance.config.keyboard_navigation = true;
                $scope.scheduleGanttInstance.config.tooltip_timeout = 10;
                $scope.scheduleGanttInstance.config.tootltip_hide_timeout = 0;

                if (isCurrentTrend) {     //in current view
                    $scope.scheduleGanttInstance.config.columns = [
                        { name: "add", label: "", width: 30, class: "gantt_add" },
                        { name: "text", label: "Category", tree: true, width: 310, resize: true },
                        { name: "originalStartDate", label: "Orig. Start", align: "center", width: 102, resize: true },
                        { name: "originalEndDate", label: "Orig. End", align: "center", width: 102, resize: true },
                        { name: "start_date", label: "Cur. Start", align: "center", width: 102, resize: true },
                        { name: "end_date", label: "Cur. End", align: "center", width: 102, resize: true },
                        //------------------------------------Manasi--------------------------------------
                        //{
                        //    name: "totalCost", width: 100, label: "Price", align: "right", resize: true, template: function (obj) {
                        //        return $filter('currency')(obj.totalCost, '$', formatCurrency(obj.totalCost));
                        //    }
                        //},
                        //------------------------------------------------------------------------------------
                        {
                            name: "totalBudget", width: 100, label: "Budget", align: "right", resize: true, template: function (obj) {
                                // return $filter('currency')(obj.totalBudget, '$', formatCurrency(obj.totalBudget));
                                return $filter('currency')(obj.totalBudget, '$', 2);// Pritesh added for keeping the format of decimals 13aug202
                            }
                        },
                        {
                            //name: "totalCostActualForecast", width: 100, label: "Forecast", align: "right", resize: true, template: function (obj) {
                            name: "totalCostActualForecast", width: 100, label: "EAC", align: "right", resize: true, template: function (obj) {
                                // return $filter('currency')(obj.totalCostActualForecast, '$', formatCurrency(obj.totalCostActualForecast));// Pritesh Commented on 25Jul2020 as to keept the format consistent
                                return $filter('currency')(obj.totalCostActualForecast, '$', 2);// Pritesh added for keeping the format of decimals 13aug202
                                //return $filter('currency')(0, '$', 2);
                            }
                        },
                        { name: "percentage_completion", width: 155, label: "% Complete", align: "right", resize: true }, //align: "center" Manasi  24-07-2020

                    ];
                } else {    //not current view
                    $scope.scheduleGanttInstance.config.columns = [
                        { name: "add", label: "", width: 30, class: "gantt_add", resize: true },
                        { name: "text", label: "Category", tree: true, width: 313, resize: true },
                        { name: "originalStartDate", label: "Orig. Start", align: "center", width: 113, resize: true },
                        { name: "originalEndDate", label: "Orig. End", align: "center", width: 113, resize: true },
                        { name: "start_date", label: "Cur. Start", align: "center", width: 113, resize: true },
                        { name: "end_date", width: 113, label: "Cur. End", align: "center", resize: true },
                        //-------------Commented by Manasi----------------------
                        //{
                        //    name: "totalCost", width: 163, label: "Price", align: "right", resize: true, template: function (obj) {
                        //        return $filter('currency')(obj.totalCost, '$', formatCurrency(obj.totalCost));
                        //    }
                        //},
                        //----------------------------------------------------------
                        {
                            name: "totalBudget", width: 163, label: "Budget", align: "right", resize: true, template: function (obj) {
                                //  return $filter('currency')(obj.totalBudget, '$', formatCurrency(obj.totalBudget));
                                return $filter('currency')(obj.totalBudget, '$', 2);// Pritesh added for keeping the format of decimals 13aug202
                            }
                        },
                    ];
                }

                //luan 3/29 watcher
                $scope.$watch('subCategory', function () {
                    console.log($scope.subCategory);
                })

                $scope.scheduleGanttInstance.form_blocks["my_editor"] = {
                    render: function (sns) {
                        return "<div class='dhx_cal_ltext' style='height:60px;'>Text&nbsp;"
                            + "<select ng-model='subC' ng-options='s.label for s in subCategory  ' type='text'></div>";
                    },
                    set_value: function (node, value, task, section) {
                        //node.childNodes[1].value = value || "";
                        //node.childNodes[4].value = task.users || "";
                    },
                    get_value: function (node, task, section) {
                        //task.users = node.childNodes[4].value;
                        //return node.childNodes[1].value;
                    },
                    focus: function (node) {
                        //var a = node.childNodes[1];
                        //a.select();
                        //a.focus();
                    }
                };
                $scope.scheduleGanttInstance.config.type_renderers[gantt.config.types.project] = function (task, defaultRender) {

                    //Manasi 28-07-2020
                    if ($scope.first_task_id == 0) {
                        $scope.first_task_id = task.id;
                    }
                    var main_el = document.createElement("div");
                    main_el.setAttribute($scope.scheduleGanttInstance.config.task_attribute, task.id);
                    var size = $scope.scheduleGanttInstance.getTaskPosition(task);
                    main_el.innerHTML = [
                        "<div class='project-left'></div>",
                        "<div class='project-right'></div>"
                    ].join('');
                    main_el.className = "custom-project";

                    main_el.style.left = size.left + "px";
                    main_el.style.top = size.top + 7 + "px";
                    main_el.style.width = size.width + "px";

                    return main_el;
                };
                $scope.scheduleGanttInstance.config.date_grid = "%d %M %Y";

                $scope.names = ['test'];
                $scope.selectedName = 'test';

                //PurchaseORDER
                $scope.getPurchaseOrder = function () {
                    PurchaseOrder.getNewPurchaseOrderNumber()
                        .get({ ProjectID: delayedData[2].result[0].ProjectID }, function success(response) {
                            var purchaseOrder = response.result;
                            PurchaseOrderDetail.getPurchaseOrderDetail()
                                .get({ ProjectID: delayedData[2].result[0].ProjectID }, function success(response) {
                                    console.log(response);
                                    var purchaseOrderDetails = response.result;
                                    var scope = $rootScope.$new();

                                    scope.params = {
                                        purchaseOrder: purchaseOrder,
                                        purchaseOrderDetails: purchaseOrderDetails
                                    }

                                    console.log(scope.params);

                                    //4/25	
                                    $rootScope.modalInstance = $uibModal.open({
                                        backdrop: 'static',
                                        keyboard: false,
                                        scope: scope,
                                        templateUrl: "app/views/modal/purchase_order_detail_modal.html",
                                        size: "lg",
                                        controller: "PurchaseOrderDetailModalCtrl"
                                    });
                                    $rootScope.modalInstance.result.then(function (response) {
                                        setTimeout(function () {
                                            applyExpandables();
                                        }, 1000);
                                    });


                                }, function error(response) {

                                });

                        }, function error(response) {

                        });
                }




                //----END PURCHASE ORDER

                if (isCurrentTrend) {
                    $scope.scheduleGanttInstance.config.lightbox.sections = [
                        {
                            name: "mainphase",
                            height: 38,
                            map_to: "mainCategory",
                            type: "select",
                            options: $scope.scheduleGanttInstance.serverList("main", $scope.MainCategory),
                            focus: true,
                            onchange: function () {
                                var main = $scope.scheduleGanttInstance.getLightboxSection('mainphase');
                                var sub = $scope.scheduleGanttInstance.getLightboxSection('subphase');
                                var startDate = $scope.scheduleGanttInstance.getLightboxSection('start_date');
                                var endDate = $scope.scheduleGanttInstance.getLightboxSection('end_date');

                                $scope.temp = main.getValue();
                                $scope.tempSub = sub.getValue();
                                $scope.lightBoxStartDate = startDate.getValue();
                                $scope.lightBoxEndDate = endDate.getValue();

                                var index; //Category Id of parent
                                $scope.subCategory = [];
                                for (var i = 0; i < MainCategory.length; i++) {
                                    if (main.getValue() === MainCategory[i].CategoryDescription) {
                                        index = MainCategory[i].CategoryID;
                                    }
                                }

                                //   GanttCategory.getSubCategory().get({ProgramID:delayedData[2].result[0].ProgramID, Phase:$scope.lightBoxTask.parent , CategoryID: index },function(response){
                                //  ProgramCategory.getSubActivityCategoryProgram().get({Phase:$scope.lightBoxTask.parent, CateogryID: index},function(response){

                                var phaseid = $scope.scheduleGanttInstance.getTask($scope.lightBoxTask.parent).PhaseID;
                                $http.get(serviceBasePath + "Request/SubActivityCategory/" + $scope.OrganizationID + "/" + phaseid + "/" + index).then(function (response) {
                                    var subCategory = response.data.result;
                                    for (var i = 0; i < subCategory.length; i++) {
                                        var obj = {};
                                        obj.key = subCategory[i].SubCategoryDescription;
                                        obj.label = subCategory[i].SubCategoryDescription;
                                        $scope.subCategory.push(obj);

                                    }
                                    var sub = $scope.scheduleGanttInstance.getLightboxSection('subphase');

                                    //escape html tags
                                    for (var x = 0; x < $scope.subCategory.length; x++) {
                                        $scope.subCategory[x].key = $scope.subCategory[x].key.replace(/&/g, '&#38').replace(/</g, '&#60').replace(/>/g, '&#62');
                                        $scope.subCategory[x].label = $scope.subCategory[x].label.replace(/&/g, '&#38').replace(/</g, '&#60').replace(/>/g, '&#62');
                                    }

                                    $scope.scheduleGanttInstance.updateCollection("sub", $scope.subCategory);
                                    $scope.scheduleGanttInstance.showLightbox($scope.selectedId);

                                });
                            }
                        },
                        {
                            name: "subphase",
                            height: 38,
                            map_to: "subCategory",
                            type: "select",
                            options: $scope.scheduleGanttInstance.serverList("sub", $scope.subCategory),
                            focus: true,
                            onchange: function () {
                            }
                        },
                        { name: "start_date", single_date: true, height: 38, map_to: "start_date", type: "duration", year_range: 100 },
                        { name: "end_date", single_date: true, height: 38, map_to: "end_date", type: "duration", year_range: 100 },
                        {
                            name: "percentage_completion",
                            height: 38,
                            map_to: "percentage_completion",
                            type: "select",
                            options: $scope.scheduleGanttInstance.serverList("options", $scope.percentages),
                            focus: true,
                            onchange: function () {
                            }
                        },
                    ];
                } else {
                    $scope.scheduleGanttInstance.config.lightbox.sections = [
                        {
                            name: "mainphase",
                            height: 38,
                            map_to: "mainCategory",
                            type: "select",
                            options: $scope.scheduleGanttInstance.serverList("main", $scope.MainCategory),
                            focus: true,
                            onchange: function () { //luan 3/28
                                console.log($scope.selectedPhase);
                                var main = $scope.scheduleGanttInstance.getLightboxSection('mainphase');
                                var sub = $scope.scheduleGanttInstance.getLightboxSection('subphase');
                                var startDate = $scope.scheduleGanttInstance.getLightboxSection('start_date');
                                var endDate = $scope.scheduleGanttInstance.getLightboxSection('end_date');

                                $scope.temp = main.getValue();
                                $scope.tempSub = sub.getValue();
                                $scope.lightBoxStartDate = startDate.getValue();
                                $scope.lightBoxEndDate = endDate.getValue();

                                //luan 3/29
                                if (main.getValue() != 'Add New') {
                                    PREVIOUSMAIN = main.getValue();
                                    console.log(PREVIOUSMAIN, PREVIOUSSUB);
                                }
                                PREVIOUSSUB = sub.getValue();

                                console.log(main.getValue());
                                if (main.getValue() == 'Add New') {
                                    //luan 3/29
                                    //main.setValue('');   //reset
                                    //sub.setValue('');   //reset

                                    var scope = $rootScope.$new();

                                    scope.params = {
                                        phase: $scope.selectedPhase,
                                        phaseList: $scope.phases,
                                        organizationID: $scope.OrganizationID,
                                        mainCategory: MainCategory, // Jignesh 06-10-2020
                                    }

                                    console.log(scope.params);

                                    $rootScope.modalInstance = $uibModal.open({
                                        backdrop: 'static',
                                        keyboard: false,
                                        scope: scope,
                                        templateUrl: "app/views/modal/add_main_sub_category_modal.html",
                                        size: "md",
                                        controller: "AddMainSubCategoryModalCtrl"
                                    });
                                    $rootScope.modalInstance.result.then(function (response) {


                                        console.log(response);
                                        $('.gantt_cal_cover').show();
                                        $('.gantt_cal_light').css('z-index', '10001');  //restore

                                        //luan 3/29
                                        if (response.status == 'Cancel') {
                                            console.log(PREVIOUSMAIN, PREVIOUSSUB);

                                            main.setValue(PREVIOUSMAIN);   //revert history
                                            //sub.setValue(PREVIOUSSUB);   //revert history

                                            PREVIOUSMAIN = main.getValue();
                                            PREVIOUSSUB = sub.getValue();
                                            console.log(PREVIOUSMAIN, PREVIOUSSUB);
                                        }

                                        if (response.status == 'Success') {
                                            ProgramCategory.getMainActivityCategoryProgram().get({ "Phase": $scope.selectedPhase.PhaseID, "OrganizationID": $scope.OrganizationID }, function (CategoryListData) {
                                                console.log(CategoryListData.result);
                                                MainCategory = CategoryListData.result;
                                                var categoryList = CategoryListData.result;
                                                var selectedCategory = {};
                                                $scope.MainCategory = [];
                                                $scope.subCategory = [];
                                                //luan 3/28
                                                var temp = {};
                                                temp.key = 'Add New';
                                                temp.label = 'Add New';
                                                $scope.MainCategory.push(temp);
                                                $scope.subCategory.push(temp);

                                                /* Jignesh 06-10-2020 */
                                                angular.forEach(response.subCategoryData, function (value, key) {
                                                    var obj = {};
                                                    obj.key = value.CategoryDescription;
                                                    obj.label = value.CategoryDescription;
                                                    $scope.subCategory.push(obj);
                                                    console.log(value);
                                                });
                                                /* End */

                                                var newlyAddedSubcategory = {};
                                                newlyAddedSubcategory.key = response.objectSaved.SubCategoryDescription;
                                                newlyAddedSubcategory.label = response.objectSaved.SubCategoryDescription;
                                                $scope.subCategory.push(newlyAddedSubcategory);

                                                console.log($scope.subCategory);

                                                angular.forEach(categoryList, function (value, key) {
                                                    var obj = {};
                                                    obj.key = value.CategoryDescription;
                                                    obj.label = value.CategoryDescription;
                                                    $scope.MainCategory.push(obj);
                                                    console.log(value);
                                                    if (value.CategoryDescription == response.objectSaved.CategoryDescription) {
                                                        selectedCategory = obj;
                                                    }
                                                });

                                                console.log($scope.MainCategory, $scope.subCategory);

                                                //escape html tags
                                                for (var x = 0; x < $scope.MainCategory.length; x++) {
                                                    $scope.MainCategory[x].key = $scope.MainCategory[x].key.replace(/&/g, '&#38').replace(/</g, '&#60').replace(/>/g, '&#62');
                                                    $scope.MainCategory[x].label = $scope.MainCategory[x].label.replace(/&/g, '&#38').replace(/</g, '&#60').replace(/>/g, '&#62');
                                                }

                                                for (var x = 0; x < $scope.subCategory.length; x++) {
                                                    $scope.subCategory[x].key = $scope.subCategory[x].key.replace(/&/g, '&#38').replace(/</g, '&#60').replace(/>/g, '&#62');
                                                    $scope.subCategory[x].label = $scope.subCategory[x].label.replace(/&/g, '&#38').replace(/</g, '&#60').replace(/>/g, '&#62');
                                                }

                                                $scope.scheduleGanttInstance.updateCollection("main", $scope.MainCategory);
                                                $scope.scheduleGanttInstance.updateCollection("sub", $scope.subCategory);
                                                $scope.scheduleGanttInstance.showLightbox($scope.selectedId);
                                                var main = $scope.scheduleGanttInstance.getLightboxSection('mainphase');
                                                var sub = $scope.scheduleGanttInstance.getLightboxSection('subphase');

                                                if (response.message != undefined) {
                                                    dhtmlx.alert(response.message);
                                                    $('div.gantt_modal_box.dhtmlx_modal_box.gantt-confirm.dhtmlx-confirm').css('z-index', '100000001');
                                                }

                                                main.setValue(selectedCategory.label);
                                                sub.setValue(newlyAddedSubcategory.label);
                                                console.log($scope.MainCategory);
                                            });
                                        } else {
                                            if (response.message != undefined) {
                                                dhtmlx.alert(response.message);
                                                $('div.gantt_modal_box.dhtmlx_modal_box.gantt-confirm.dhtmlx-confirm').css('z-index', '100000001');
                                            }
                                        }
                                    }, function error(response) {
                                        console.log(response);
                                    });
                                    return;
                                }

                                var index; //Category Id of parent
                                $scope.subCategory = [];
                                for (var i = 0; i < MainCategory.length; i++) {
                                    if (main.getValue() === MainCategory[i].CategoryDescription) {
                                        index = MainCategory[i].CategoryID;
                                    }
                                }


                                console.log($scope.OrganizationID);
                                var phaseid = $scope.scheduleGanttInstance.getTask($scope.lightBoxTask.parent).PhaseID;
                                $http.get(serviceBasePath + "Request/SubActivityCategory/" + $scope.OrganizationID + "/" + phaseid + "/" + index).then(function (response) {
                                    var subCategory = response.data.result;
                                    var defaultOne = {};

                                    console.log('4319 test', response);

                                    //luan 3/28
                                    var temp = {};
                                    temp.key = 'Add New';
                                    temp.label = 'Add New';
                                    $scope.subCategory.push(temp);

                                    for (var i = 0; i < subCategory.length; i++) {
                                        var obj = {};
                                        obj.key = subCategory[i].SubCategoryDescription;
                                        obj.label = subCategory[i].SubCategoryDescription;
                                        $scope.subCategory.push(obj);

                                        if (i == 0) {
                                            defaultOne.key = subCategory[i].SubCategoryDescription;
                                            defaultOne.label = subCategory[i].SubCategoryDescription;
                                        }
                                    }

                                    console.log(defaultOne);

                                    //escape html tags
                                    for (var x = 0; x < $scope.subCategory.length; x++) {
                                        $scope.subCategory[x].key = $scope.subCategory[x].key.replace(/&/g, '&#38').replace(/</g, '&#60').replace(/>/g, '&#62');
                                        $scope.subCategory[x].label = $scope.subCategory[x].label.replace(/&/g, '&#38').replace(/</g, '&#60').replace(/>/g, '&#62');
                                    }

                                    $scope.scheduleGanttInstance.updateCollection("sub", $scope.subCategory);
                                    $scope.scheduleGanttInstance.showLightbox($scope.selectedId);

                                    var sub = $scope.scheduleGanttInstance.getLightboxSection('subphase');
                                    sub.setValue(defaultOne.label);


                                });
                            }
                        },
                        {
                            name: "subphase",
                            height: 38,
                            map_to: "subCategory",
                            type: "select",
                            options: $scope.scheduleGanttInstance.serverList("sub", $scope.subCategory),
                            focus: true,
                            onchange: function () { //luan 3/28
                                console.log($scope.selectedPhase);
                                var main = $scope.scheduleGanttInstance.getLightboxSection('mainphase');
                                var sub = $scope.scheduleGanttInstance.getLightboxSection('subphase');

                                //$scope.temp = main.getValue();
                                //$scope.tempSub = subs.getValue();
                                //alert($scope.tempSub);
                                //$scope.lightBoxStartDate = startDate.getValue();
                                //$scope.lightBoxEndDate = endDate.getValue();

                                //luan 3/29
                                if (sub.getValue() != 'Add New') {
                                    PREVIOUSSUB = sub.getValue();
                                    console.log(PREVIOUSMAIN, PREVIOUSSUB);
                                }

                                //luan 3/28
                                if (sub.getValue() == 'Add New') {
                                    //sub.setValue('');   //reset
                                    var scope = $rootScope.$new();

                                    //Prepare the main category and main category id
                                    var CategoryID = '';
                                    var main_category = '';

                                    main_category = main.getValue();
                                    for (var x = 0; x < MainCategory.length; x++) {
                                        if (main_category == MainCategory[x].CategoryDescription) {
                                            CategoryID = MainCategory[x].CategoryID;
                                        }
                                    }

                                    scope.params = {
                                        phase: $scope.selectedPhase,
                                        phaseList: $scope.phases,
                                        organizationID: $scope.OrganizationID,
                                        mainCategory: main_category,
                                        mainCategoryID: CategoryID,
                                        isSubCategory: true
                                    }

                                    console.log(scope.params);

                                    $rootScope.modalInstance = $uibModal.open({
                                        backdrop: 'static',
                                        keyboard: false,
                                        scope: scope,
                                        templateUrl: "app/views/modal/add_main_sub_category_modal.html",
                                        size: "md",
                                        controller: "AddMainSubCategoryModalCtrl"
                                    });
                                    $rootScope.modalInstance.result.then(function (response) {
                                        console.log(response);
                                        $('.gantt_cal_cover').show();
                                        $('.gantt_cal_light').css('z-index', '10001');  //restore

                                        //luan 3/29
                                        if (response.status == 'Cancel') {
                                            console.log(PREVIOUSMAIN, PREVIOUSSUB);

                                            //main.setValue(PREVIOUSMAIN);   //revert history
                                            sub.setValue(PREVIOUSSUB);   //revert history

                                            PREVIOUSMAIN = main.getValue();
                                            PREVIOUSSUB = sub.getValue();
                                            console.log(PREVIOUSMAIN, PREVIOUSSUB);
                                        }

                                        if (response.status == 'Success') {
                                            $http.get(serviceBasePath + "Request/SubActivityCategory/" + $scope.OrganizationID + "/" + $scope.selectedPhase.PhaseID + "/" + CategoryID).then(function (SubCategoryData) {
                                                var subCategory = SubCategoryData.data.result;
                                                console.log(subCategory);
                                                var selectedSubcategory = {};

                                                $scope.subCategory = [];

                                                console.log(PREVIOUSMAIN, PREVIOUSSUB);

                                                //luan 3/28
                                                var temp = {};
                                                temp.key = 'Add New';
                                                temp.label = 'Add New';
                                                $scope.subCategory.push(temp);

                                                for (var i = 0; i < subCategory.length; i++) {
                                                    var obj = {};
                                                    obj.key = subCategory[i].SubCategoryDescription;
                                                    obj.label = subCategory[i].SubCategoryDescription;
                                                    $scope.subCategory.push(obj);

                                                    console.log(obj, response.objectSaved);
                                                    if (obj.label == response.objectSaved.SubCategoryDescription) {
                                                        selectedSubcategory = obj;
                                                        console.log(selectedSubcategory);
                                                    }

                                                }

                                                //escape html tags
                                                for (var x = 0; x < $scope.subCategory.length; x++) {
                                                    $scope.subCategory[x].key = $scope.subCategory[x].key.replace(/&/g, '&#38').replace(/</g, '&#60').replace(/>/g, '&#62');
                                                    $scope.subCategory[x].label = $scope.subCategory[x].label.replace(/&/g, '&#38').replace(/</g, '&#60').replace(/>/g, '&#62');
                                                }

                                                $scope.scheduleGanttInstance.updateCollection("sub", $scope.subCategory);
                                                $scope.scheduleGanttInstance.showLightbox($scope.selectedActivity.id);
                                                var main = $scope.scheduleGanttInstance.getLightboxSection('mainphase');
                                                var sub = $scope.scheduleGanttInstance.getLightboxSection('subphase');

                                                if (response.message != undefined) {
                                                    dhtmlx.alert(response.message);
                                                    $('div.gantt_modal_box.dhtmlx_modal_box.gantt-confirm.dhtmlx-confirm').css('z-index', '100000001');
                                                }

                                                main.setValue(PREVIOUSMAIN);
                                                sub.setValue(selectedSubcategory.label);
                                                console.log($scope.subCategory);

                                            });
                                        } else {
                                            if (response.message != undefined) {
                                                dhtmlx.alert(response.message);
                                                $('div.gantt_modal_box.dhtmlx_modal_box.gantt-confirm.dhtmlx-confirm').css('z-index', '100000001');
                                            }
                                        }
                                    }, function error(response) {
                                        console.log(response);
                                    });
                                    return;
                                }

                            }
                        },
                        { name: "start_date", single_date: true, height: 38, map_to: "start_date", type: "duration", year_range: 100 },
                        { name: "end_date", single_date: true, height: 38, map_to: "end_date", type: "duration", year_range: 100 },
                    ];
                }
            }
            var s = $scope.scheduleGanttInstance.callEvent("onTaskSelected", [$scope.first_task_id]);   //Manasi 28-07-2020
            function adjustCostId(idToBeRemoved, costs) {
                angular.forEach(costs, function (item) {
                    if (parseInt(item.id) > parseInt(idToBeRemoved)) {
                        item.id = item.id - 1;
                    }
                })
            }

            function deleteCostFromBuffer(cost) {
                angular.forEach($scope.buffer, function (item) {
                    if (item.activity == cost.activityId) {
                        var ii = 0;
                        angular.forEach(item.costs.data, function (costItem) {
                            if (costItem.id == cost.id && costItem.activity == cost.activity) {
                                id = costItem.id;
                            }
                        });
                        item.costs.data.splice(id, 1);
                        //updateBuffer(cost);
                    }

                });
            }

            function updateBuffer(cost) {
                var textIds = $scope.textBoxValues[1];
                //if(textIds) {
                //    for (var i = 0; i < textIds.length - 1; i++) {
                //        textIds[i] = i;
                //    }
                //}
                // alert("Before : " + $scope.textBoxValues);
                angular.forEach($scope.buffer, function (item) {
                    if (item.activity === cost.activityId && item.currentScale == $scope.scheduleScale) {
                        angular.forEach(item.costs.data, function (costItem) {
                            if (costItem) {
                                if (costItem.id == cost.id && costItem.activity == cost.activity) {
                                    //if(costItem.currentScale != $scope.currentScale)
                                    syncCostType();
                                    item.isSelected = angular.copy(cost.isSelected);
                                    item.costs = angular.copy($scope.costs);
                                    item.MaxFTECostID = angular.copy($scope.MaxFTECostID);
                                    item.MaxLumpSumCostID = angular.copy($scope.MaxLumpSumCostID);
                                    item.MaxUnitCostID = angular.copy($scope.MaxUnitCostID);
                                    item.MaxODCCostID = angular.copy($scope.MaxODCCostID);
                                    item.MaxPercentageCostID = angular.copy($scope.MaxPercentageCostID);
                                    item.method = angular.copy($scope.method);
                                    item.description = angular.copy($scope.description);
                                    item.unitType = angular.copy($scope.unitType);
                                    item.currentCostIndex = angular.copy($scope.currentCostIndex);
                                    item.scale = angular.copy($scope.scale);
                                    item.isCostEdited = angular.copy($scope.isCostEdited);
                                    item.isNewCost = angular.copy($scope.isNewCost);
                                    item.totalCost = angular.copy($scope.totalCost);
                                    item.totalBudget = angular.copy($scope.totalBudget);
                                    item.unit_type = angular.copy($scope.unit_type);
                                    item.employee_id = angular.copy($scope.employee_id);
                                    item.subcontractor_id = angular.copy($scope.subcontractor_id);
                                    item.material_id = angular.copy($scope.material_id);
                                    item.textBoxIds = angular.copy($scope.textBoxIds);
                                    item.unitCost = angular.copy($scope.unitCost);
                                    item.unitBudget = angular.copy($scope.unitBudget);
                                    // alert($scope.textBoxValues);
                                    item.textBoxValues = angular.copy($scope.textBoxValues);
                                }
                            }
                        })

                    }
                });
                //  alert("After : "+$scope.textBoxValues);
            }

            function removeSavedCostFromBuffer(costId) {
                angular.forEach($scope.buffer, function (item) {

                    if (item.activityId == $scope.selectedActivity.id) {
                        item.costs.data.splice(costId - 1, 1);

                        item.isCostEdited.splice(costId, 1);
                        item.method.splice(costId, 1);
                        item.description.splice(costId, 1);
                        item.totalUnits.splice(costId, 1);
                        item.unitType.splice(costId, 1);
                        item.textBoxValues.splice(costId, 1);
                        item.textBoxIds.splice(costId, 1);
                        item.totalCost.splice(costId, 1);
                        item.isNewCost.splice(costId, 1);
                        item.unitCost.splice(costId, 1);
                        item.currentCostIndex = parseInt(item.currentCostIndex) - 1;
                        item.employee_id.splice(costId, 1);
                        adjustCostId(costId, item.costs.data);
                    }
                });
            }

            //Sync Cost from different scale if the cost in current scale changes
            function syncCostType() {
                angular.forEach($scope.buffer, function (item) {
                    if (item.activityId == $scope.selectedActivity.id && item.currentScale != $scope.scheduleScale) {
                        item.method = angular.copy($scope.method);
                        item.description = angular.copy($scope.description);
                        item.unitCost = angular.copy($scope.unitCost);
                        item.unitBudget = angular.copy($scope.unitBudget);
                        item.employee_id = angular.copy($scope.employee_id);
                        item.subcontractor_id = angular.copy($scope.subcontractor_id);
                        item.material_id - angular.copy($scope.material_id);
                    }
                });
            }

            //Sync Cost on the costs retrieved from db if the cost in currentscale changes
            function syncNewCost(bufferObj) {
                angular.forEach($scope.buffer, function (item) {
                    if (item.activityId == $scope.selectedActivity.id && item.currentScale == $scope.currentScale) {
                        $scope.method = angular.copy(item.method);
                        bufferObj.method = angular.copy(item.method);
                        $scope.description = angular.copy(item.description);
                        bufferObj.description = angular.copy(item.description);
                        $scope.unitCost = angular.copy(item.unitCost);
                        bufferObj.unitCost = angular.copy(item.unitCost);
                        $scope.unitBudget = angular.copy(item.unitBudget);
                        bufferObj.unitBudget = angular.copy(item.unitBudget);
                        $scope.employee_id = angular.copy(item.employee_id);
                        bufferObj.employee_id = angular.copy(item.employee_id);
                        $scope.subcontractor_id = angular.copy(item.subcontractor_id);
                        bufferObj.subcontractor_id = angular.copy(item.subcontractor_id);
                        $scope.material_id = angular.copy(item.material_id);
                        bufferObj.material_id = angular.copy(item.material_id)
                    }
                });
            }

            function appendNewCostFromBuffer(bufferObj, isUpdate) {
                var fromObj = {};
                var toObj = {};
                angular.forEach($scope.buffer, function (item) {
                    if (item.activityId == $scope.selectedActivity.id && item.currentScale == $scope.currentScale) {
                        fromObj = item;
                    }
                });
                if (angular.equals(fromObj, {})) {
                    return;
                }

                toObj = bufferObj;

                if (fromObj.costs.data.length > toObj.costs.data.length && isUpdate == true) {
                    //Update New Cost
                    //add an empty costRow for costs that are already on buffer
                    var diff = parseInt(fromObj.costs.data.length) - parseInt(toObj.costs.data.length);

                    $scope.costs.data[fromObj.costs.data.length - 1] = angular.copy($scope.costs.data[toObj.costs.data.length - 1]);
                    $scope.costs.data[fromObj.costs.data.length - 1].id += diff;
                    $scope.currentCostIndex += diff;

                }

                //
                //if(fromObj.costs.data.length > toObj.costs.data.length && isUpdate == false){
                //    //Update New Cost
                //    //add an empty costRow for costs that are already on buffer
                //    var diff = parseInt(fromObj.costs.data.length) - parseInt(toObj.costs.data.length);
                //    $scope.costs.data[fromObj.costs.data.length - 1] = angular.copy($scope.costs.data[toObj.costs.data.length - 1]);
                //    $scope.costs.data[fromObj.costs.data.length - 1].id += diff;
                //    $scope.currentCostIndex += diff;
                //
                //}

                if (parseInt(fromObj.isCostEdited.length) > parseInt(toObj.isCostEdited.length)) {
                    if (isUpdate == true) {
                        var index = parseInt(toObj.isCostEdited.length) - 1; //if the cost is already in buffer
                    } else {
                        var index = parseInt(toObj.isCostEdited.length); //if the cost is not in the buffer
                    }

                    while (index <= fromObj.isCostEdited.length) {
                        if (index < fromObj.isCostEdited.length - 1) {
                            toObj.costs.data[index - 1] = angular.copy(fromObj.costs.data[index - 1]);
                            $scope.costs.data[index - 1] = angular.copy(fromObj.costs.data[index - 1]);

                            toObj.scale = angular.copy(fromObj.scale);
                            toObj.unitType = angular.copy(fromObj.unitType);
                            $scope.unitType = angular.copy(fromObj.unitType);
                            toObj.totalCost = angular.copy(fromObj.totalCost);
                            $scope.totalCost = angular.copy(fromObj.totalCost);
                            toObj.totalBudget = angular.copy(fromObj.totalBudget);
                            $scope.totalBudget = angular.copy(fromObj.totalBudget);
                            toObj.totalUnits = angular.copy(fromObj.totalUnits);
                            $scope.totalUnits = angular.copy(fromObj.totalUnits);
                            console.log('total here');
                            toObj.unit_type = angular.copy(fromObj.unit_type);
                            $scope.unit_type = angular.copy(fromObj.unit_type);
                            toObj.employee_id = angular.copy(fromObj.employee_id);
                            $scope.employee_id = angular.copy(fromObj.employee_id);
                            toObj.subcontractor_id = angular.copy(fromObj.subcontractor_id);
                            $scope.subcontractor_id = angular.copy(fromObj.subcontractor_id);
                            $scope.currentCostIndex = angular.copy(fromObj.costs.data.length);
                        }

                        if (index < fromObj.isCostEdited.length) {
                            if (isUpdate == false) {
                                //    alert(index);
                                //if cost is not in buffer . fill 0 to $scope.textBoxValues
                                var diff = getNumbOfTextBox($scope.selectedActivity.start_date, $scope.selectedActivity.end_date);
                                var textValues = [];
                                var textIds = [];
                                for (var i = 0; i < diff; i++) {
                                    textValues[i] = 0;
                                    textIds[i] = i;
                                }
                                //toObj.textBoxValues[index] = angular.copy(Array.apply(null, Array(toObj.textBoxValues[index - 1].length)).map(Number.prototype.valueOf, 0));
                                //$scope.textBoxValues[index] = angular.copy(Array.apply(null, Array(toObj.textBoxValues[index - 1].length)).map(Number.prototype.valueOf, 0));
                                //toObj.textBoxIds[index] = angular.copy(toObj.textBoxIds[1]);
                                //$scope.textBoxIds[index] = angucopy(toObj.textBoxIds[1]);
                                toObj.textBoxValues[index] = textValues;
                                toObj.textBoxIds[index] = textIds;
                            }
                        }
                        //toObj.currentCostIndex = angular.copy(fromObj.currentCostIndex);
                        console.log(fromObj);
                        toObj.MaxFTECostID = angular.copy(fromObj.MaxFTECostID);
                        $scope.MaxFTECostID = angular.copy(fromObj.MaxFTECostID);
                        toObj.MaxLumpSumCostID = angular.copy(fromObj.MaxLumpSumCostID);
                        $scope.MaxLumpSumCostID = angular.copy(fromObj.MaxLumpSumCostID);
                        toObj.MaxUnitCostID = angular.copy(fromObj.MaxUnitCostID);
                        $scope.MaxUnitCostID = angular.copy(fromObj.MaxUnitCostID);
                        toObj.MaxODCCostID = angular.copy(fromObj.MaxODCCostID);
                        $scope.MaxODCCostID = angular.copy(fromObj.MaxODCCostID);
                        toObj.MaxPercentageCostID = angular.copy(fromObj.MaxPercentageCostID);
                        $scope.MaxPercentageCostID = angular.copy(fromObj.MaxPercentageCostID);
                        toObj.currentCostIndex = angular.copy(fromObj.currentCostIndex);

                        toObj.isNewCost = angular.copy(fromObj.isNewCost);
                        $scope.isNewCost = angular.copy(fromObj.isNewCost);
                        //toObj.currentCostIndex = parseInt(toObj.currentCostIndex) + 1;
                        index = index + 1;
                    }

                }
                toObj.isCostEdited = angular.copy(fromObj.isCostEdited);
                $scope.isCostEdited = angular.copy(fromObj.isCostEdited);

            }

            function spliceCost(id) {
                $scope.textBoxValues.splice(id, 1);
                $scope.isNewCost.splice(id, 1);
                $scope.isCostEdited.splice(id, 1);
                $scope.method.splice(id, 1);
                $scope.description.splice(id, 1);
                $scope.totalUnits.splice(id, 1);
                console.log('total here');
                $scope.textBoxIds.splice(id, 1);
                $scope.unitType.splice(id, 1);
                $scope.totalCost.splice(id, 1);
                $scope.totalBudget.splice(id, 1);
                $scope.unitCost.splice(id, 1);
                $scope.unitBudget.splice(id, 1);
                $scope.employee_id.splice(id, 1);
                $scope.subcontractor_id.splice(id, 1);
                $scope.material_id.splice(id, 1);
            }

            function getActivityOriginalStartDate(originalDate) {
                if (!originalDate)
                    return;
                var original_date = originalDate.substring(6, 10);//yyyy
                original_date += originalDate.substring(2, 5);//-mm
                original_date += "-" + originalDate.substring(0, 2);//dd
                //1. Get working days
                return original_date;
            }

            function activateSpinner() {
                $('#spin').addClass('fademe');
                if (usSpinnerConfig.config.color == "black")
                    usSpinnerConfig.config.color = "white";
                $scope.showSpinner12 = true;
                usSpinnerService.spin('spinner-14');
            }

            function deactivateSpinner() {
                $scope.showSpinner12 = false;
                usSpinnerService.stop('spinner-14');
                $("#spin").removeClass('fademe');
            }

            function getAbsoulteMonths(momentDate) {
                var months = Number(moment(momentDate).format("MM"));
                var years = Number(moment(momentDate).format("YYYY"));
                return months + (years * 12);
            }
            /*----------------------------------- Function Definitions - END ---------------------------------------------*/

        }]);
