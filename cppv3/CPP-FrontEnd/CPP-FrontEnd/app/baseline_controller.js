/**
 * Created by ikhong on 6/10/2015.
 */
'use strict';

angular.module('xenon.baseline_controller', []).
    controller('BaselineCtrl', ['MainActivityCategory', 'currentTrend', 'TrendId', 'ProjectTitle', 'UnitType', 'SubActivityCategory', 'Category', 'GetActivity', '$http', '$q', '$state', '$scope', '$compile', 'Program', 'ProgramElement',
        'Project', 'Trend', 'Activity', 'Cost', 'InsertCost', 'fteposition',
        'FTEPositionCost', 'delayedData', 'Page', 'UpdateActivity', '$filter', 'currentProject', '$rootScope', '$uibModal', '$timeout',
        function (MainActivityCategory, currentTrend, TrendId, ProjectTitle, UnitType, SubActivityCategory, Category, GetActivity, $http, $q, $state, $scope, $compile, Program, ProgramElement,
                  Project, Trend, Activity, Cost, InsertCost, FTEPositions,
                  FTEPositionCost, delayedData, Page, UpdateActivity, $filter, currentProject, $rootScope, $uibModal, $timeout) {

            //Jquery
            setTimeout(function () {
                $("<h5 style='color: black; font-weight: 1000; opacity: .75; padding-bottom: 5px'>WBS</h5>").insertBefore($("#schedule-gantt"));
                $("<h5 style='margin-top:15px; color: black; font-weight: 1000; opacity: .75; padding-bottom: 5px'>Cost Details</h5>").insertBefore($("#cost-gantt"));
                //$("div, .gantt_grid_head_cell, .gantt_grid_head_text").css('margin-left', '2%');
                $('div:contains("Category"):not(:has(div))').css('margin-left', '2%');
                $("div[style='width: 313px;']").css('margin-left', '2%');
            }, 100);
            $scope.percentages = [{ key: '0%', label: '0%' }, { key: '5%', label: '5%' }, { key: '10%', label: '10%' }, { key: '15%', label: '15%' }, { key: '20%', label: '20%' }, { key: '25%', label: '25%' },
                      { key: '30%', label: '30%' }, { key: '35%', label: '35%' }, { key: '40%', label: '40%' }, { key: '45%', label: '45%' }, { key: '50%', label: '50%' },
                      { key: '55%', label: '55%' }, { key: '60%', label: '60%' }, { key: '65%', label: '65%' }, { key: '70%', label: '70%' }, { key: '75%', label: '75%' },
                      { key: '80%', label: '80%' }, { key: '85%', label: '85%' }, { key: '90%', label: '90%' }, { key: '95%', label: '95%' }, { key: '100%', label: '100%' }]
            var projectTitle = "";
            var currentId;
            var MainCategory;                            //Variable to store the BudgetCategory
            $scope.allCostTotal = 0;
            console.log(delayedData);
            var delayedProject = delayedData[0].result[0][0];
            var delayedPhase = delayedData[0].result[1];
            var delayedActivity = delayedData[0].result[2];
            console.log(delayedData);
            var delayedCost = delayedData[0].result[3];
            Page.setTitle('Current');
            $scope.isScaleChanged = false;
            $scope.isUpdateTaskFromLightbox = true;
            $scope.schedule = { data: [] };
            $scope.costs = { data: [] };
            $scope.isMonthSchedule = false;
            $scope.subCategory = [];
            $scope.FTEPositions = [];
            $scope.methods = [{
                name: 'FTE',
                value: 'F'
            }, {
                name: 'Lumpsum',
                value: 'L'
            }, {
                name: 'Unit',
                value: 'U'
            }];
            $scope.unitTypes = [];
            $scope.baselineDuration = "0 days";
            $scope.baselineTotalValue = 0;
            $scope.trend;                            //store the trend information
            $scope.isNewCost = [];
            $scope.isDeleteFromLightbox;            // bool - check to see whether the user delete an activity from the lightbox
            //Get Maximum start_date and end_date of the current Project and furture project
            function formatCurrency(amount) {
                return (amount % 1 > 0) ? 2 : 0;
            }

            $scope.filterPhase = function () {
                //console.log(phase);
                //if(phase == null) {
                //    $scope.schedulePhase = 'all';
                //    return;
                //}
                //$scope.schedulePhase = phase.toLowerCase();
                // $scope.schedulePhase = $scope.schedulePhase.toLowerCase();
                if (!$scope.schedulePhase)
                    $scope.schedulePhase = "ALL";

            }

            function getCurrentDate() {
                var path = serviceBasePath + "Request/MaxFutureDate/" + delayedProject.ProjectID;
                console.log(path);
                $http.get(serviceBasePath + "Request/MaxFutureDate/" + delayedProject.ProjectID).then(function (response) {
                    var maxFutureDate = (response.data.result) ? response.data.result : 0;
                    console.log(response);
                    $scope.maxFutureDate = maxFutureDate;
                    $http.get(serviceBasePath + "Request/currentTrend/" + delayedProject.ProjectID).then(function (response) {
                        var difference = 0;
                        var dateList = response.data.result;
                        var start = dateList[0];
                        var end = dateList[1];
                        var diff = moment(end).format('ww') - moment(start).format('ww');
                        if (dateList.length !== 0 && diff !== 0) {
                            $scope.currentStartDate = dateList[0];
                            $scope.currentEndDate = dateList[1];
                        } else {
                            $scope.currentNumberDate = 0;
                            $scope.currentProjectDuration = difference + ' days';
                            $scope.currentEndDate = 0;
                        }
                    });
                })
            }
            //Get baseline Project
            function getBaseline(futureTrendList, approvedTrendList) {
                var trend;
                angular.forEach(futureTrendList, function (futureTrend) {
                    console.log(futureTrend);
                    if (futureTrend.TrendNumber === "0") {
                        trend = futureTrend;
                    }
                });
                angular.forEach(approvedTrendList, function (approvedTrend) {
                    console.log(approvedTrend);
                    if (approvedTrend.TrendNumber === "0") {
                        trend = approvedTrend;
                    }
                });
                return trend;
            }
            getProjectDurationAndCost();
            function getProjectDurationAndCost() {
                $http.get(serviceBasePath + "Request/ProjectDurationAndCost/" + delayedProject.ProjectID + "/0").success(function (response) {
                    console.debug(response);
                    var baselineProject = response.result[0];
                    var currentProject = response.result[1];
                    var futureProject = response.result[2];
                    $scope.baselineTotalValue = (baselineProject[2]) ? baselineProject[2].toString() : "0";
                    $scope.currentTotalValue = (currentProject[2]) ? currentProject[2].toString() : "0";
                    $scope.futureTotalValue = (futureProject[2]) ? futureProject[2].toString() : "0";

                    var baselineDifference = moment(baselineProject[1]).diff(moment(baselineProject[0]), 'days');
                    var currentDifference = moment(currentProject[1]).diff(moment(currentProject[0]), 'days');
                    var forecastDifference = moment(futureProject[1]).diff(moment(futureProject[0]), 'days');

                    if (baselineDifference == 1) baselineDifference = 0;
                    if (currentDifference == 1) currentDifference = 0;
                    if (forecastDifference == 1) forecastDifference = 0;

                    $scope.baselineDuration = (baselineProject[1]) ? baselineDifference + " days " : "0 days";
                    $scope.currentDuration = (currentProject[1]) ? currentDifference + " days" : "0 days";
                    $scope.forecastDuration = (futureProject[1]) ? forecastDifference + " days" : "0 days";


                })
            }
            //GetProjectValue - calculate the duration and total value for baseline, current, and future projects
            $scope.exit = function () {
                //window.location.href="#/app/wbs";
                //console.log(window);
                window.history.back();
            }
            function getProjectValue() {
                //Get Duration date for baseline-current-future
                $http.get(serviceBasePath + "Request/TrendGraph/" + delayedProject.ProjectID).then(function (response) {
                    // var baselineTrend = response.data.result.FutureTrendList[0];
                    var futureTrendList = response.data.result.FutureTrendList;
                    var approvedTrendList = response.data.result.PastTrendList;
                    var baselineTrend = getBaseline(futureTrendList, approvedTrendList);
                    var startDateOfFutureTrend;

                    $scope.baselineTrend = baselineTrend;
                    var baselineStartDate = baselineTrend.PostTrendStartDate;
                    var baselineEndDate = baselineTrend.PostTrendEndDate;

                    console.log(baselineTrend);
                    console.log(futureTrendList.length);
                    //If there is not approved trend
                    if (baselineTrend.PostTrendStartDate == "" && approvedTrendList.length == 0) {
                        if (futureTrendList.length == 2) { //if there is one unapproved trend
                            startDateOfFutureTrend = futureTrendList[1].PostTrendStartDate;
                        } else if (futureTrendList.length == 1) { //If There is no unapproved trend
                            startDateOfFutureTrend = 0;
                        }
                        else {//if there are more than 1 unapproved trends
                            startDateOfFutureTrend = futureTrendList[1].PostTrendStartDate;
                            for (var i = 2; i < futureTrendList.length ; i++) {
                                var start = futureTrendList[i].PostTrendStartDate;
                                if ((moment(start).diff(moment(startDateOfFutureTrend), 'days')) <= 0) {
                                    startDateOfFutureTrend = start;
                                }
                            }
                        }
                    }

                    var diff = moment($scope.currentEndDate).diff(moment(baselineEndDate), 'days');
                    console.log($scope.maxFutureDate);
                    var baselineDifference = moment(baselineEndDate).diff(baselineStartDate, 'days');
                    if (isNaN(baselineDifference)) {  //If there is no baseline
                        $scope.baselineDuration = '0' + ' days';
                        if (isNaN(diff)) {  //if there is no current Trend
                            console.log($scope.currentEndDate);
                            if ($scope.currentEndDate == 0 && $scope.maxFutureDate == 0) { //if there is no future trend
                                $scope.currentProjectDuration = "0 days";
                                $scope.forecastDuration = "0 days";
                            } else {

                                $scope.currentProjectDuration = "0 days";
                                $scope.forecastDuration = ($scope.maxFutureDate == 0) ? "0 days" : (moment($scope.maxFutureDate).diff(moment(startDateOfFutureTrend), 'days') - 1).toString() + ' days';
                            }
                        } else {
                            if (diff >= 0)
                                $scope.currentProjectDuration = ($scope.currentEndDate == 0) ? "0 days" : (moment($scope.currentEndDate).diff(0, 'days') - 1).toString() + ' days';
                            else
                                $scope.currentProjectDuration = (baselineEndDate == 0) ? "0 days" : (moment(baselineEndDate).diff(0, 'days')).toString() + ' days';
                            $scope.forecastDuration = ($scope.maxFutureDate == 0) ? "0 days" : (moment($scope.maxFutureDate).diff(moment(startDateOfFutureTrend), 'days') - 1).toString() + ' days';
                            console.log($scope.forecastDuration);
                        }

                    } else {
                        $scope.forecastDuration = (moment($scope.maxFutureDate).diff(baselineStartDate, 'days') - 1).toString() + ' days';
                        if (diff >= 0)
                            $scope.currentProjectDuration = ($scope.currentEndDate == 0) ? baselineDifference + ' days' : (moment($scope.currentEndDate).diff(baselineStartDate, 'days') - 1).toString() + ' days';
                        else
                            $scope.currentProjectDuration = (baselineEndDate == 0) ? baselineDifference + ' days' : (moment(baselineEndDate).diff(baselineStartDate, 'days') - 1).toString() + ' days';
                        $scope.baselineDuration = baselineDifference + ' days';
                    }

                    //Calculate Cost for baseline-current-future
                    var approvedCost = 0;
                    var futureCost = 0;
                    if (baselineTrend) {
                        $scope.baselineTotalValue = baselineTrend.PostTrendCost.toString();
                    }
                    angular.forEach(approvedTrendList, function (trend) {
                        if (trend) {
                            approvedCost += trend.PostTrendCost;
                        }
                    });
                    if (baselineTrend) {
                        approvedCost += baselineTrend.PostTrendCost;
                        futureCost += approvedCost;
                    }
                    for (var i = 1; i < futureTrendList.length; i++) {
                        if (futureTrendList[i]) {
                            futureCost += futureTrendList[i].PostTrendCost;
                        }
                    }
                    $scope.futureTotalValue = futureCost.toString();
                    $scope.currentTotalValue = approvedCost.toString();
                });
            }

            //UpdateTrendDate - used to update changes on scheduler date
            function updateTrendDate() {
                $scope.scheduleGanttInstance.eachTask(function (task) {
                    console.log(task);
                    if (task.id === 8888) {
                        //var start = moment(task.start_date).format('YYYY-MM-DD');
                        var start = task.start_date.toISOString().substring(0, 10);
                        console.log(start);
                        //var end = moment(task.end_date).format('YYYY-MM-DD');
                        var end = task.end_date.toISOString().substring(0, 10);
                        console.log(end);
                        var currentDate = new Date();
                        var createdOn = moment(currentDate).format('YYYY-MM-DD');
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
                            console.log(response);
                            getProjectValue();
                        });
                    }
                });
            }

            function updateBaselineValue() {
                var total = 0;
                $scope.scheduleGanttInstance.eachTask(function (task) {
                    if (task.id >= 1000 && task.id < 8888) {
                        var value = task.totalCost.substring(1, task.totalCost.length);
                        total += parseInt(value);
                    }
                    if (task.id === 8888) {
                        console.log(task);
                        var start = moment(task.start_date).format("YYYY-MM-DD");
                        var end = moment(task.end_date).format("YYYY-MM-DD");
                        var difference = moment(end).diff(moment(start), 'days');
                        $scope.baselineDuration = difference + " days";
                    }

                });

                $scope.baselineTotalValue = total.toString();

            }
            //UpdatePhase - Update the duration for each phase everytime an activity start-end date changes
            function updatePhase() {
                $scope.scheduleGanttInstance.eachTask(function (tas) {

                    if (tas.id >= 1000 && tas.id < 8888) {
                        console.log(tas);
                        var start = moment(tas.start_date).format("YYYY-MM-DD");
                        var end = moment(tas.end_date).format("YYYY-MM-DD");
                        //var difference = moment(end).format("days") - moment(start).format("days");
                        var difference = moment(end).diff(moment(start), 'days') - 1;
                        if (tas.id === 1000) {

                            $scope.planning_start_date = moment(tas.start_date).format("MM/DD/YY");
                            $scope.planning_end_date = moment(tas.end_date).format("MM/DD/YY");
                        } else
                            if (tas.id === 2000) {
                                $scope.schematic_design_start_date = moment(tas.start_date).format("MM/DD/YY");
                                $scope.schematic_design_end_date = moment(tas.end_date).format("MM/DD/YY");
                            }
                            else if (tas.id === 3000) {
                                $scope.design_bidding_start_date = moment(tas.start_date).format("MM/DD/YY");
                                $scope.design_bidding_end_date = moment(tas.end_date).format("MM/DD/YY");
                            }
                            else if (tas.id === 4000) {
                                $scope.construction_start_date = moment(tas.start_date).format("MM/DD/YY");
                                $scope.construction_end_date = moment(tas.end_date).format("MM/DD/YY");
                            }
                            else if (tas.id === 5000) {
                                $scope.closeout_start_date = moment(tas.start_date).format("MM/DD/YY");
                                $scope.closeout_end_date = moment(tas.end_date).format("MM/DD/YY");

                            }
                    }


                });
            }

            //costCalculation - Calculate the cost
            function costCalculation(id, obj) {
                if (obj) {
                    $scope.selectedCost = obj;
                    id = obj.id;
                }
                if ($scope.selectedCost) {

                    //console.log(obj);
                    //console.log($scope.selectedCost);
                    var start = moment($scope.selectedCost.start_date).format("YYYY-MM-DD");
                    var end = moment($scope.selectedCost.end_date).format("YYYY-MM-DD");

                    //var numberOfWeeks = moment(end).format("ww") - moment(start).format("ww");

                    if ($scope.method[id] === "F") {
                        var multiplier = 1;

                        if ($scope.scheduleScale === "week") {
                            multiplier = 40;
                        } else if ($scope.scheduleScale === "month") {
                            multiplier = 40 * 4;
                        } else if ($scope.scheduleScale === "year") {
                            multiplier = 40 * 4 * 52;
                        }
                        var total = 0;
                        $.each($scope.textBoxValues[id], function (index) {
                            $scope.fteHours[id][index] = parseFloat(this) * multiplier;
                            total += $scope.fteHours[id][index];
                        });
                        $scope.totalUnits[id] = total;

                        total = 0;
                        $.each($scope.fteHours[id], function (index) {
                            $scope.fteCosts[id][index] = parseFloat(this) * $scope.unitCost[id];
                            total += $scope.fteCosts[id][index];
                        });
                        $scope.totalCost[id] = total;
                    }
                    else if ($scope.method[id] === "L") {
                        total = 0;
                        $.each($scope.textBoxValues[id], function (index) {
                            total += parseFloat(this);
                        });
                        $scope.totalCost[id] = total;
                    }
                    else if ($scope.method[id] === "U") {
                        var total = 0;
                        var totalUnits = 0;
                        $.each($scope.textBoxValues[id], function (index) {
                            $scope.unitCosts[id][index] = parseFloat(this) * $scope.unitCost[id];
                            totalUnits += parseFloat(this);
                            total += $scope.unitCosts[id][index];
                        });
                        $scope.totalCost[id] = total;
                        $scope.totalUnits[id] = totalUnits;
                    }
                    else if ($scope.method[id] === "P") {
                        total = 0;
                        $.each($scope.textBoxValues[id], function (index) {
                            total += parseFloat(this);
                        });
                        $scope.totalUnits[id] = total;

                        $scope.totalCost[id] = $scope.totalUnits[id] * $scope.unitCost[id] / 100;
                    }
                    return $scope.selectedCost;
                }
            }

            //API call to get the selected trend information
            //    TrendId.get({
            //        "trendId" : delayedData[3],
            //        "projectId": delayedData[2].result[0].ProjectID
            //
            //    },function(response){
            //        console.log(response);
            //        $scope.trend = response.result;
            //        projectTitle = response.result.TrendDescription;
            //        Page.setTitle('Baseline'  );
            //    });

            //Set Title for the project
            ProjectTitle.setTitle(delayedProject.ProjectName);

            //changeMethod - called everytime a cost method is changed
            $scope.changedMethod = function (id) {
                console.log($scope.costs.data);
                var s = $scope.costGanttInstance.getTask(id);
                //var test = $scope.costGanttInstance.callEvent('onAfterTaskUpdate',[id,s]);
                console.log($scope.textBoxValues);
                if (id > $scope.currentCostIndex) {
                    //  var task = $scope.costGanttInstance.
                    $scope.currentCostIndex = id;
                }
                var row = $("#cost-gantt .gantt_row[task_id='" + id + "']");
                var cells = row.find('.gantt_cell');
                if ($scope.method[id] === "F") {
                    $scope.unitCost[id] = "";
                    $scope.description[id] = { name: "", value: "" };

                    $(cells[3]).html(
                        $compile("<select ng-model='description[" + id + "]'  style='width:100%;height:100%; text-align:center; vertical-align: top;'" +
                            " ng-options='option.name for option in FTEPositions track by option.value' ng-change='changedDescription(" + id + ")'></input>"
                        )($scope)
                    );
                    $(cells[5]).html(
                        $compile(
                            "<input disabled='true' value='Hours' style='width:100%;height:100%; text-align:center; vertical-align: top;'></input>"
                        )($scope)
                    );
                    $(cells[4]).html(
                        $compile(
                            "<input ng-model='totalUnits[" + id + "]' disabled=true style='width:100%;height:100%; text-align:center; vertical-align: top;'></input>"
                        )($scope)
                    );

                    $(cells[6]).html(
                        $compile(
                            "<input ng-model='unitCost[" + id + "]' disabled=true ng-change='changedUnitCost[" + id + "] style='width:100%;height:100%; text-align:center; vertical-align: top;'></input>"
                        )($scope)
                    );
                    $(cells[7]).html(
                        $compile(
                            "<input ng-model='totalCost[" + id + "]' disabled=true style='width:100%;height:100%; text-align:center; vertical-align: top;'></input>"
                        )($scope)
                    );
                }
                else if ($scope.method[id] === "L") {

                    $scope.description[id] = { name: "", value: "" };
                    $scope.unitCost[id] = "";
                    $(cells[3]).html(
                        $compile("<input ng-model='description[" + id + "].name'  style='width:100%;height:100%; text-align:center; vertical-align: top;' ng-change='changedDescription(" + id + ")'></input>"
                        )($scope)
                    );
                    $(cells[5]).html(
                        $compile(
                            "<input disabled='true' value='Lumpsum' style='width:100%;height:100%; text-align:center; vertical-align: top;'></input>"
                        )($scope)
                    );

                    $(cells[4]).html(
                        $compile(
                            "<input disabled='true' value='N/A' style='width:100%;height:100%; text-align:center; vertical-align: top;'></input>"
                        )($scope)
                    );

                    $(cells[6]).html(
                        $compile(
                            "<input disabled='true' value='N/A' style='width:100%;height:100%; text-align:center; vertical-align: top;'></input>"
                        )($scope)
                    );

                    $(cells[7]).html(
                        $compile(
                            "<input disabled=true ng-model='totalCost[" + id + "]' style='width:100%;height:100%; text-align:center; vertical-align: top;'></input>"
                        )($scope)
                    );
                }
                else if ($scope.method[id] === "U") {
                    $scope.description[id] = { name: "", value: "" };
                    $scope.unitCost[id] = "";
                    $scope.unitType[id] = "";
                    //$scope.unit_type = {name: ""};
                    $(cells[3]).html(
                        $compile("<input ng-model='description[" + id + "].name'  style='width:100%;height:100%; text-align:center; vertical-align: top;' ng-change='changedDescription(" + id + ")'></input>"
                        )($scope)
                    );
                    $(cells[5]).html(
                        $compile(
                            "<select  ng-model= 'unit_type[" + id + "]' style='width:100%;height:100%; text-align:center; vertical-align: top;'" +
                            " ng-options='type.name for type in unitTypes track by type.value ' ng-change='changedUnitType(" + id + ")'></input>"
                        )($scope)
                    );

                    $(cells[4]).html(
                        $compile(
                            "<input ng-model='totalUnits[" + id + "]' disabled = 'true' style='width:100%;height:100%; text-align:center; vertical-align: top;'></input>"
                        )($scope)
                    );

                    $(cells[6]).html(
                        $compile(
                            "<input ng-model='unitCost[" + id + "]' style='width:100%;height:100%; text-align:center; vertical-align: top;' ng-blur='changedUnitCost(" + id + ")'></input>"
                        )($scope)
                    );

                    $(cells[7]).html(
                        $compile(
                            "<input ng-model='totalCost[" + id + "]' disabled =true style='width:100%;height:100%; text-align:center; vertical-align: top;'></input>"
                        )($scope)
                    );
                }
                else if ($scope.method[id] === "P") {

                    $scope.description[id] = { name: "", value: "" };
                    $scope.unitCost[id] = "";
                    $(cells[3]).html(
                        $compile("<input ng-model='description[" + id + "].name'  style='width:100%;height:100%; text-align:center; vertical-align: top;' ng-change='changedDescription(" + id + ")'></input>"
                        )($scope)
                    );
                    $(cells[5]).html(
                        $compile(
                            "<input disabled='true' value='Percentage' style='width:100%;height:100%; text-align:center; vertical-align: top;'></input>"
                        )($scope)
                    );

                    $(cells[4]).html(
                        $compile(
                            "<input ng-model='totalUnits[" + id + "]' style='width:100%;height:100%; text-align:center; vertical-align: top;'></input>"
                        )($scope)
                    );

                    $(cells[6]).html(
                        $compile(
                            "<input ng-model='unitCost[" + id + "]' style='width:100%;height:100%; text-align:center; vertical-align: top;'></input>"
                        )($scope)
                    );

                    $(cells[7]).html(
                        $compile(
                            "<input ng-model='totalCost[" + id + "]' style='width:100%;height:100%; text-align:center; vertical-align: top;'></input>"
                        )($scope)
                    );
                }
                if (id == $scope.currentCostIndex);
                {
                    currentId = -1;
                    var d = [0, 0, 0, 0];
                    var activity = $scope.scheduleGanttInstance.getTask($scope.selectedCost.activity);
                    console.log(activity);
                    var newID = $scope.currentCostIndex + 1;
                    var cost = {};
                    cost["id"] = newID;
                    cost["cost_id"] = "";
                    cost["text"] = "";
                    cost["description"] = "";
                    cost["unit_type"] = "";
                    cost["unit_cost"] = "";
                    cost["total_units"] = "";
                    cost["total_cost"] = "";
                    cost["scale"] = "";
                    cost["open"] = false;
                    cost["original_start_date"] = $scope.selectedCost.original_start_date;
                    cost["original_end_date"] = $scope.selectedCost.original_end_date;
                    cost["start_date"] = $scope.selectedCost.start_date;
                    cost["end_date"] = $scope.selectedCost.end_date;
                    cost["phase"] = activity.parent;
                    cost["save"] = "<span class='notClickableFont'><i class='fa fa-save'></i></span>";
                    cost["delete"] = "<span class='notClickableFont'><i class='fa fa-trash'></i></span>";
                    cost["project"] = $scope.selectedProject;
                    cost["trend"] = $scope.selectedTrend;
                    cost["activity"] = $scope.selectedCost.activity;
                    // if($scope.retrievedActivityID != 0){
                    //    cost["activity"] = $scope.retrievedActivityID;
                    //    $scope.retrieveActivity = 0;
                    //}
                    //
                    $scope.costs.data.push(cost);
                    //
                    $scope.textBoxValues.push(d);
                    $scope.isNewCost.push(true);
                    $scope.isCostEdited.push(false);
                    // $scope.costGanttInstance.clearAll();
                    $scope.costGanttInstance.config.start_date = $scope.scheduleGanttInstance.getState().min_date;
                    $scope.costGanttInstance.config.end_date = $scope.scheduleGanttInstance.getState().max_date;
                    // $scope.costGanttInstance.addTask(cost,"",newID);
                    //  $scope.costGanttInstance.parse($scope.costs, "json");

                    $scope.costGanttInstance.parse({ data: $scope.costs.data, links: [] });
                    $scope.textBoxValues = angular.copy($scope.tempTextBoxValues);

                    $scope.costGanttInstance.eachTask(function (task) {
                    });

                    $scope.textBoxValues.push(d);
                    //var s =  $scope.costGanttInstance.callEvent("customClick", [$scope.currentCostIndex  ]);
                    $scope.costGanttInstance.eachTask(function (task) {
                        if ($scope.isCostEdited[task.id] === true) {
                            $scope.costGanttInstance.callEvent("customClick", [task.id]);

                        }
                    });

                }
            };

            //changeDescription - called when the cost description is changed (only for FTE)
            $scope.changedDescription = function (id) {

                if ($scope.method[id] === "F") {
                    var row = $("#cost-gantt .gantt_row[task_id='" + id + "']");
                    var cells = row.find('.gantt_cell');
                    FTEPositionCost.get({ PositionID: $scope.description[id].value }, function (FTEPositionCostData) {
                        var positionDetails = FTEPositionCostData.result;
                        $scope.unitCost[id] = positionDetails[0].CurrentHourlyRate;
                    });

                    $(cells[6]).html(
                        $compile("<input ng-model='unitCost[" + id + "]' disabled=true  style='width:100%;height:100%; text-align:center; vertical-align: top;' ng-change='changedUnitCost(" + id + ")'></input>"
                        )($scope)
                    );
                    costCalculation(id);
                }
            };


            $scope.changedUnitType = function (id) {
                var row = $("#cost-gantt .gantt_row[task_id='" + id + "']");
                var cells = row.find('.gantt_cell');
                console.log(row);
                console.log(cells);
                ///     $scope.unit_Type = {name:"", value:""};
                //     if($scope.method[id]==="U"){
                //
                //         $(cells[5]).html(
                //             $compile(
                //                 "<select  ng-model= 'unit_Type[" + id + "]'   style='width:100%;height:100%; text-align:center; vertical-align: top;'" +
                //                 " ng-options='type.name for type in unitTypes track by type.value' ng-change='changedUnitType("+id+")'></input>"
                //             )($scope)
                //         );
                //     }
            };
            $scope.changedUnits = function (id) {
                console.log($scope.method[id]);
                console.log($scope.description[id]);
                console.log($scope.unitType[id]);
                console.log($scope.unitCost[id]);
            };
            $scope.changedUnitCost = function (id) {
                costCalculation(id);
            };
            $scope.changedTotalCost = function (id) {
                console.log($scope.method[id]);
                console.log($scope.description[id]);
                console.log($scope.unitType[id]);
                console.log($scope.unitCost[id]);
            };
            $scope.changeTotalUnit = function (id) {
                console.log($scope.method[id]);
                console.log($scope.description[id]);
                console.log($scope.unitType[id]);
                console.log($scope.unitCost[id]);
            }
            $scope.changedCost = function (id, i) {
                if ($scope.textBoxValues[id][i] == "") {
                    $scope.textBoxValues[id][i] = 0;
                }
                console.log('changedCost ' + id + " " + i);
                var div = $("." + id + "_cost");
                var textBoxes = $(div).children();
                console.log($scope.textBoxValues[id][i]);
                costCalculation(id);
            };
            $scope.reloadCost = function (id, costId) {
                currentId = -1;
                $scope.MaxFTECostID = 0;
                $scope.MaxLumpSumCostID = 0;
                $scope.MaxUnitCostID = 0;

                $scope.MaxPercentageCostID = 0;

                $scope.method = [];
                $scope.description = [];
                $scope.unitType = [];
                $scope.unit_type = [];
                $scope.totalUnits = [];
                $scope.unitCost = [];
                $scope.totalCost = [];

                $scope.textBoxIds = [];
                $scope.textBoxValues = [];
                $scope.tempTBValues = [];
                $scope.fteCosts = [];
                $scope.unitCosts = [];
                $scope.fteHours = [];

                $scope.scale = [];
                $scope.numberOfBoxes = [];

                $scope.taskBeingEdited = false;
                $scope.selectedActivity = $scope.scheduleGanttInstance.getTask(id);
                $scope.selectedProject = isNaN(Number($scope.selectedActivity.project)) ? "null" : Number($scope.selectedActivity.project);
                $scope.selectedTrend = isNaN(Number($scope.selectedActivity.trend)) ? "null" : Number($scope.selectedActivity.trend);

                if (Number(id) >= 1000) {
                    var phase = Number(id) / 1000;
                    var activity = "null";
                }
                else {
                    var phase = "null";
                    var activity = Number(id);
                }

                var positionsList;
                FTEPositions.get({}, function (FTEPositionsData) {
                    positionsList = FTEPositionsData.result;
                    for (var i = 0; i < positionsList.length; i++) {
                        var position = {};
                        position["name"] = positionsList[i].PositionDescription;
                        position["value"] = positionsList[i].PositionID;
                        $scope.FTEPositions.push(position);
                    }
                });
                $scope.x = {
                    ProjectID: $scope.selectedProject,
                    TrendNumber: $scope.selectedTrend,
                    Phasecode: phase,
                    ActivityID: activity
                }

                Cost.get({ ProjectID: $scope.selectedProject, TrendNumber: $scope.selectedTrend, Phasecode: phase, ActivityID: activity }, function (CostData) {

                    $scope.currentCostIndex = 1;
                    $scope.MaxFTECostID = CostData.MaxFTECostID;
                    $scope.MaxLumpSumCostID = CostData.MaxLumpsumCostID;
                    $scope.MaxUnitCostID = CostData.MaxUnitCostID;
                    $scope.MaxPercentageCostID = CostData.MaxPercentageCostID;

                    var costs = CostData.CostRow;
                    $scope.costs.data = [];

                    if (costs.length > 0) {
                        for (var i = 0; i < costs.length; i++) {
                            var cost = {};
                            var UT;
                            if (costs[i].CostType === "U") {
                                for (var j = 0; j < $scope.unitTypes.length; j++) {
                                    if ($scope.unitTypes[j].name === costs[i].UnitType) {
                                        UT = j;
                                    }
                                }
                            }
                            cost["id"] = $scope.currentCostIndex;
                            cost["cost_id"] = costs[i].CostID;
                            cost["text"] =
                                (costs[i].CostType === "F") ? "FTE" :
                                    (costs[i].CostType === "L") ? "Lumpsum" :
                                        (costs[i].CostType === "U") ? "Unit" :
                                            (costs[i].CostType === "P") ? "% Basis" : "Error";
                            cost["description"] = costs[i].Description;
                            cost["unit_type"] =
                                (costs[i].CostType === "F") ? "Hours" :
                                    (costs[i].CostType === "L") ? "Lumpsum" :
                                        (costs[i].CostType === "U") ? costs[i].UnitType :
                                            (costs[i].CostType === "P") ? "Percentage" : "Error";
                            cost["unit_cost"] = costs[i].Base;
                            var totalCost = 0;

                            if (costs[i].FTECost) {
                                var individualCosts = costs[i].FTECost.split(",");

                                $.each(individualCosts, function (index) {
                                    totalCost += parseFloat(individualCosts[index]);
                                });
                            }
                            else {
                                var individualCosts = costs[i].TextBoxValue.split(",");

                                $.each(individualCosts, function (index) {
                                    totalCost += parseFloat(individualCosts[index]);
                                });
                            }

                            var totalUnits = 0;
                            if (costs[i].CostType === "F") {
                                cost["total_units"] = costs[i].FTEHours.split(",")[0];
                            }
                            else if (costs[i].CostType === "L") {
                                cost["total_units"] = "N/A";
                                cost["unit_cost"] = "N/A";
                            }
                            else if (costs[i].CostType === "U") {
                                var individualUnits = costs[i].TextBoxValue.split(",");
                                $scope.unit_type[$scope.currentCostIndex] = { name: costs[i].UnitType, value: $scope.unitTypes[UT].value };
                                $.each(individualUnits, function (index) {
                                    totalUnits += parseFloat(individualUnits[index]);
                                });
                                cost["total_units"] = totalUnits;
                                totalCost = totalUnits * costs[i].Base;
                            } else if (costs[i].CostType === "P") {
                                var individualUnits = costs[i].TextBoxValue.split(",");

                                $.each(individualUnits, function (index) {
                                    totalUnits += parseFloat(individualUnits[index]);
                                });
                                cost["total_units"] = totalUnits;
                                totalCost = totalUnits * costs[i].Base / 100;
                            }
                            cost["total_cost"] = totalCost;
                            cost["scale"] = costs[i].Scale;
                            cost["open"] = false;
                            cost["original_start_date"] = $scope.selectedActivity.original_start_date;
                            cost["original_end_date"] = $scope.selectedActivity.original_end_date;
                            cost["start_date"] = $scope.selectedActivity.start_date;
                            cost["end_date"] = $scope.selectedActivity.end_date;
                            cost["phase"] = $scope.selectedActivity.parent;
                            cost["save"] = "<span class='notClickableFont'><i class='fa fa-save'></i></span>";
                            cost["delete"] = "<span class='notClickableFont'><i class='fa fa-trash'></i></span>";
                            cost["project"] = $scope.selectedProject;
                            cost["trend"] = $scope.selectedTrend;
                            cost["program_element"] = $scope.selectedActivity.ProgramElementID;
                            cost["program"] = $scope.selectedActivity.ProgramID;
                            cost["activity"] = $scope.selectedActivity.id;

                            $scope.method[$scope.currentCostIndex] = costs[i].CostType;

                            $scope.description[$scope.currentCostIndex] = { name: costs[i].Description, value: costs[i].FTEPositionID };
                            $scope.unitType[$scope.currentCostIndex] = costs[i].Description;

                            if (costs[i].CostType === "F") {
                                var hoursArray = costs[i].FTEHours.split(",");
                                var costArray = costs[i].FTECost.split(",");
                                //$.each(hoursArray, function(index) {
                                //    console.log(this);
                                //    $scope.totalUnits[$scope.currentCostIndex] += parseFloat(this);
                                //});
                                angular.forEach(hoursArray, function (hours) {
                                    $scope.totalUnits[$scope.currentCostIndex] += parseFloat(hours);
                                });
                                $.each(costArray, function (index) {
                                    $scope.totalCost[$scope.currentCostIndex] += parseFloat(this);
                                });
                            }



                            $scope.unitCost[$scope.currentCostIndex] = costs[i].Base;

                            $scope.textBoxIds[$scope.currentCostIndex] = costs[i].TextBoxID.split(",");
                            $scope.textBoxValues[$scope.currentCostIndex] = costs[i].TextBoxValue.split(",");

                            $scope.scale[$scope.currentCostIndex] = costs[i].Granularity;
                            $scope.currentCostIndex++;
                            $scope.costs.data.push(cost);
                            //updateTrendValue(cost.id);
                        }

                    }

                    // Last empty row

                    cost = {};
                    cost["id"] = $scope.currentCostIndex;
                    cost["cost_id"] = "";
                    cost["text"] = "";
                    cost["description"] = "";
                    cost["unit_type"] = "";
                    cost["unit_cost"] = "";
                    cost["total_units"] = "";
                    cost["total_cost"] = "";
                    cost["scale"] = "";
                    cost["open"] = false;
                    cost["original_start_date"] = $scope.selectedActivity.original_start_date;
                    cost["original_end_date"] = $scope.selectedActivity.original_end_date;
                    cost["start_date"] = $scope.selectedActivity.start_date;
                    cost["end_date"] = $scope.selectedActivity.end_date;
                    cost["phase"] = $scope.selectedActivity.parent;
                    cost["save"] = "<span class='notClickableFont'><i class='fa fa-save'></i></span>";
                    cost["delete"] = "<span class='notClickableFont'><i class='fa fa-trash'></i></span>";
                    cost["project"] = $scope.selectedProject;
                    cost["trend"] = $scope.selectedTrend;
                    cost["program_element"] = $scope.selectedActivity.ProgramElementID;
                    cost["program"] = $scope.selectedActivity.ProgramID;
                    cost["activity"] = $scope.selectedActivity.id;
                    $scope.costs.data.push(cost);

                    // $scope.costGanttInstance.clearAll();
                    $scope.costGanttInstance.config.start_date = $scope.scheduleGanttInstance.getState().min_date;
                    $scope.costGanttInstance.config.end_date = $scope.scheduleGanttInstance.getState().max_date;
                    $scope.costGanttInstance.parse($scope.costs, "json");
                });

            };


            UnitType.get({}, function (response) {
                var unitTypeList = response.result;
                angular.forEach(unitTypeList, function (item, key) {
                    console.log(item);
                    var temp = {};
                    temp.name = item.UnitName;
                    temp.value = item.UnitAbbr;

                    $scope.unitTypes.push(temp);
                });
                console.log($scope.unitTypes);
            })

            var activities = delayedActivity;   //List of activites
            var projectCost = 0;
            var project = {};
            project["id"] = 8888;
            project["text"] = delayedProject.ProjectName;
            project["type"] = gantt.config.types.project;
            project["open"] = true;
            project["duration"] = 0;
            project["totalCost"] = "" + "";
            project["originalStartDate"] = getProjectOriginalStartEndDate(activities).originalStartDate;
            project["originalEndDate"] = getProjectOriginalStartEndDate(activities).originalEndDate;

            project["percentage_completion"] = getProjectPercentageCompletion(activities);
            $scope.schedule.data.push(project);
            function CalculatePhaseTotal(activities, phaseId) {
                var phaseTotalCost = 0;
                angular.forEach(activities, function (activity, value) {
                    if (parseInt(activity.PhaseCode) === phaseId) {
                        var fte = (activity.FTECost) ? parseFloat(activity.FTECost) : 0;
                        var lumpsum = (activity.LumpsumCost) ? parseFloat(activity.LumpsumCost) : 0;
                        var unit = (activity.UnitCost) ? parseFloat(activity.UnitCost) : 0;
                        var activityCost = fte + lumpsum + unit;
                        phaseTotalCost += activityCost;
                    }
                });
                return phaseTotalCost;
            }
            var phaseArray = [];
            var phases = delayedPhase;   //List of Phases
            $scope.phases = phases;
            console.log(phases);
            for (var j = 0; j < phases.length; j++) {
                var phase = {};
                phase["id"] = Number(phases[j].PhaseID) * 1000;
                phase["text"] = phases[j].PhaseDescription;
                phase["type"] = gantt.config.types.project;
                phase["open"] = true;
                phase["duration"] = 0;
                phase["parent"] = 8888;
                phase["originalStartDate"] = getPhaseOriginalStartEndDate(phases[j].PhaseID, activities).originalStartDate;
                phase["originalEndDate"] = getPhaseOriginalStartEndDate(phases[j].PhaseID, activities).originalEndDate;
                phase["percentage_completion"] = getPhasePercentageCompletion(phases[j].PhaseID, activities);

                var phaseTotalCost = CalculatePhaseTotal(activities, phases[j].PhaseID);
                phase["totalCost"] = phaseTotalCost;
                projectCost += parseFloat(phaseTotalCost);

                phaseArray.push(phase);
            }
            project['totalCost'] = projectCost;
            //  console.log(phases);
            //duration


            //$scope.planning_value= $filter('currency')(phaseArray[0].totalCost,'$',formatCurrency(phaseArray[0].totalCost));
            ////phase dollar value
            //$scope.schematic_design_value=$filter('currency')(phaseArray[1].totalCost,'$',formatCurrency(phaseArray[1].totalCost));
            //$scope.design_bidding_value= $filter('currency')(phaseArray[2].totalCost,'$',formatCurrency(phaseArray[2].totalCost)) ;
            //$scope.construction_value=$filter('currency')(phaseArray[3].totalCost,'$',formatCurrency(phaseArray[3].totalCost));
            //$scope.closeout_value= $filter('currency')(phaseArray[4].totalCost,'$',formatCurrency(phaseArray[4].totalCost));

            $scope.MainCategory = [];
            //set Main Category as key:value format

            console.log(phaseArray);
            for (var i = 0; i < activities.length; i++) {
                var activity = {};
                activity["id"] = Number(activities[i].ActivityID);
                activity["update_id"] = Number(activities[i].ActivityID);
                var testDateFormat = "DD MMM YYYY";
                console.log(activities[i]);
                activity["text"] = activities[i].BudgetCategory + " - " + activities[i].BudgetSubCategory;
                //activity["original_start_date"] = moment(activities[i].ActivityStartDate, "YYYY-MM-DD").format("DD-MM-YYYY");
                //activity["original_end_date"] = moment(activities[i].ActivityEndDate, "YYYY-MM-DD").format("DD-MM-YYYY");
                //activity["start_date"] = moment(activities[i].ActivityStartDate, "YYYY-MM-DD").format("DD-MM-YYYY");
                //activity["end_date"] = moment(activities[i].ActivityEndDate, "YYYY-MM-DD").format("DD-MM-YYYY");
                activity["originalStartDate"] = moment(activities[i].OriginalActivityStartDate).format(testDateFormat);
                activity["originalEndDate"] = moment(activities[i].OriginalActivityEndDate).format(testDateFormat);
                activity["original_start_date"] = moment(activities[i].ActivityStartDate).format("DD-MM-YYYY");
                activity["original_end_date"] = moment(activities[i].ActivityEndDate).format("DD-MM-YYYY");
                activity["start_date"] = moment(activities[i].ActivityStartDate).format("DD-MM-YYYY");
                activity["end_date"] = moment(activities[i].ActivityEndDate).format("DD-MM-YYYY");
                activity["parent"] = Number(activities[i].PhaseCode) * 1000;

                activity["percentage_completion"] = activities[i].PercentageCompletion + '%';
                if (!activities[i].OriginalActivityStartDate) {
                    activity["originalStartDate"] = 'N/A';
                }
                if (!activities[i].OriginalActivityEndDate) {
                    activity["originalEndDate"] = 'N/A';
                }

                activity["program_element"] = activities[i].ProgramElementID;
                activity["program"] = activities[i].ProgramID;
                activity["project"] = activities[i].ProjectID;
                activity["trend"] = activities[i].TrendNumber;
                activity["phase"] = activities[i].PhaseCode;
                var fteCost = (activities[i].FTECost) ? parseFloat(activities[i].FTECost) : 0;
                var lumpsumCost = (activities[i].LumpsumCost) ? parseFloat(activities[i].LumpsumCost) : 0;
                var unitCost = (activities[i].UnitCost) ? parseFloat(activities[i].UnitCost) : 0;
                var totalCost = fteCost + lumpsumCost + unitCost;

                activity["totalCost"] = totalCost.toString();
                /*
                 if (phaseArray.length > 0) {
                 var ind;
                 var phase = $.grep(phaseArray, function (e, index) {
                 if (e.id == activity.parent) {
                 ind = index
                 return e;
                 }
                 });

                 var phasePush = {};
                 phasePush = phase[0];

                 if (phase.length > 0) {
                 phaseArray.splice(ind, 1);
                 $scope.schedule.data.push(phasePush);
                 }
                 }
                 */
                $scope.schedule.data.push(activity);
            }

            while (phaseArray.length > 0) {
                var phasePush = phaseArray[0];
                phaseArray.splice(0, 1);
                $scope.schedule.data.push(phasePush);
            }

            // Get two instances of the Gantt object
            $scope.scheduleGanttInstance = Gantt.getGanttInstance();
            //$scope.scheduleGanttInstance.parse($scope.schedule.data);
            console.log($scope.schedule.data);
            $scope.costGanttInstance = Gantt.getGanttInstance();

            // Schedule Gantt Chart Configuration
            $scope.scheduleGanttInstance.config.drag_links = false;
            $scope.scheduleGanttInstance.config.drag_resize = false;
            $scope.scheduleGanttInstance.config.drag_progress = false;
            $scope.scheduleGanttInstance.config.fit_tasks = true;
            $scope.scheduleGanttInstance.config.scroll_on_click = false;
            $scope.scheduleGanttInstance.config.min_column_width = 120;
            $scope.scheduleGanttInstance.config.initial_scroll = false;
            $scope.scheduleGanttInstance.config.drag_move = false;
            $scope.scheduleGanttInstance.config.prevent_default_scroll = true;
            $scope.scheduleGanttInstance.config.task_height = 15;
            $scope.scheduleGanttInstance.config.row_height = 22;
            $scope.scheduleGanttInstance.config.grid_width = 900;
            $scope.scheduleGanttInstance.config.columns = [
                { name: "add", label: "", width: '30px', class: "" },
                { name: "text", label: "Category", tree: true, width: '300px' },
                { name: "originalStartDate", label: "Orig. Start", align: "center", width: '' },
                { name: "originalEndDate", label: "Orig. End", align: "center" },
                { name: "start_date", label: "Start", align: "center", width: '' },
                { name: "end_date", label: "End", align: "center" },
                {
                    name: "totalCost", label: "Cost", align: "right", template: function (obj) {
                        return $filter('currency')(obj.totalCost, '$', formatCurrency(obj.totalCost));
                    }
                },
            { name: "percentage_completion", width: '60px', label: "%", align: "center" },
            ];

            $scope.scheduleGanttInstance.config.type_renderers[gantt.config.types.project] = function (task, defaultRender) {
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
            //$scope.scheduleGanttInstance.attachEvent("onEmptyClick",function(e){
            //
            //    if($scope.taskBeingEdited == true){
            //        if(confirm("Cost data has not been saved, are you sure you want to cancel?") == true) {
            //            $scope.costGanttInstance.refreshData();
            //            $scope.reloadCost($scope.selectedId);
            //        }
            //    }
            //
            //});
            //$scope.costGanttInstance.attachEvent("onEmptyClick",function(e){
            //
            //    if ($scope.taskBeingEdited == true) {
            //        if(confirm("Cost data has not been saved, are you sure you want to cancel?") == true) {
            //            $scope.costGanttInstance.refreshData();
            //            $scope.reloadCost($scope.selectedId);
            //        }
            //    }
            //
            //});
            $scope.firstTime = true;
            $scope.testChange = function () {
            }
            $scope.s = [

                {
                    name: "mainphase", height: 38, map_to: "mainCategory", type: "select", options:
                       $scope.scheduleGanttInstance.serverList("main", $scope.MainCategory), focus: true, onchange: function (task) {
                           var main = $scope.scheduleGanttInstance.getLightboxSection('mainphase');
                           var sub = $scope.scheduleGanttInstance.getLightboxSection('subphase');
                           var mainValue = main.getValue();
                           $scope.temp = mainValue;
                           $scope.tempSub = sub.getValue();
                           //console.log(mainValue);
                           //console.log(MainCategory);
                           var index;
                           $scope.subCategory = [];
                           for (var i = 0; i < MainCategory.length; i++) {
                               if (mainValue === MainCategory[i].CategoryDescription) {
                                   index = MainCategory[i].CategoryID;
                               }
                           }
                           $http.get(serviceBasePath + "Request/SubActivityCategory/" + index + "/" + $scope.lightBoxTask.parent).then(function (response) {
                               var subCategory = response.data.result;
                               for (var i = 0; i < subCategory.length; i++) {
                                   var obj = {};
                                   obj.key = subCategory[i].SubCategoryDescription;
                                   obj.label = subCategory[i].SubCategoryDescription;
                                   $scope.subCategory.push(obj);

                               }
                               var sub = $scope.scheduleGanttInstance.getLightboxSection('subphase');
                               main = $scope.scheduleGanttInstance.getLightboxSection('mainphase');


                               $scope.scheduleGanttInstance.updateCollection("sub", $scope.subCategory);
                               // $scope.scheduleGanttInstance.config.lightbox.sections = s;
                               $scope.scheduleGanttInstance.showLightbox($scope.selectedId);
                           });
                       }
                },
                {
                    name: "subphase", height: 38, map_to: "subCategory", type: "select", options:
                       $scope.scheduleGanttInstance.serverList("sub", $scope.subCategory), focus: false, onchange: function () {
                           console.log($scope.subCategory);
                       }
                },
                { name: "start_date", single_date: true, height: 38, map_to: "start_date", type: "duration" },
                { name: "end_date", single_date: true, height: 38, map_to: "end_date", type: "duration" }
            ];
            $scope.scheduleGanttInstance.config.lightbox.sections = [

                {
                    name: "mainphase", height: 38, map_to: "mainCategory", type: "select", options:
                       $scope.scheduleGanttInstance.serverList("main", $scope.MainCategory), focus: true, onchange: function () {
                           var main = $scope.scheduleGanttInstance.getLightboxSection('mainphase');
                           var sub = $scope.scheduleGanttInstance.getLightboxSection('subphase');
                           var mainValue = main.getValue();
                           $scope.temp = mainValue;
                           $scope.tempSub = sub.getValue();
                           console.log(main);
                           console.log(sub);
                           console.log(mainValue);

                           var index;
                           $scope.subCategory = [];
                           for (var i = 0; i < MainCategory.length; i++) {
                               if (mainValue === MainCategory[i].CategoryDescription) {
                                   index = MainCategory[i].CategoryID;
                               }
                           }
                           $http.get(serviceBasePath + "Request/SubActivityCategory/" + index + "/" + $scope.lightBoxTask.parent).then(function (response) {
                               console.log(response);
                               var subCategory = response.data.result;
                               for (var i = 0; i < subCategory.length; i++) {
                                   var obj = {};
                                   obj.key = subCategory[i].SubCategoryDescription;
                                   obj.label = subCategory[i].SubCategoryDescription;
                                   $scope.subCategory.push(obj);

                               }
                               var sub = $scope.scheduleGanttInstance.getLightboxSection('subphase');
                               main = $scope.scheduleGanttInstance.getLightboxSection('mainphase');


                               $scope.scheduleGanttInstance.updateCollection("sub", $scope.subCategory);
                               // $scope.scheduleGanttInstance.config.lightbox.sections = s;
                               $scope.scheduleGanttInstance.showLightbox($scope.selectedId);
                           });
                           //SubActivityCategory.get({CategoryID: index, Phase : $scope.lightBoxTask.parent},function(response){
                           //    console.log(response);
                           //    var subCategory = response.result;
                           //    for(var i = 0; i < subCategory.length;i++){
                           //        var obj = {};
                           //        obj.key = subCategory[i].SubCategoryDescription;
                           //        obj.label = subCategory[i].SubCategoryDescription;
                           //        $scope.subCategory.push(obj);
                           //
                           //    }
                           //    var sub = $scope.scheduleGanttInstance.getLightboxSection('subphase');
                           //    main = $scope.scheduleGanttInstance.getLightboxSection('mainphase');
                           //
                           //
                           //    $scope.scheduleGanttInstance.updateCollection("sub",$scope.subCategory);
                           //   // $scope.scheduleGanttInstance.config.lightbox.sections = s;
                           //    $scope.scheduleGanttInstance.showLightbox($scope.selectedId);
                           //
                           //});
                       }
                },
                {
                    name: "subphase", height: 38, map_to: "subCategory", type: "select", options:
                       $scope.scheduleGanttInstance.serverList("sub", $scope.subCategory), focus: true, onchange: function () {
                           console.log($scope.subCategory);
                       }
                },
                { name: "start_date", single_date: true, height: 38, map_to: "start_date", type: "duration" },
                { name: "end_date", single_date: true, height: 38, map_to: "end_date", type: "duration" }
            ];

            //Calculate the phase original start and end dates
            function getPhaseOriginalStartEndDate(phaseID, activities) {
                var hasActivity = false;
                var maxDate = new Date(8640000000000000);
                var minDate = new Date(-8640000000000000);
                var originalStartDate = maxDate;
                var originalEndDate = minDate;
                console.log(activities);
                for (var x = 0; x < activities.length; x++) {
                    if (activities[x].OriginalActivityStartDate == "Invalid date" || activities[x].OriginalActivityEndDate == "Invalid date") {
                        continue;
                    }
                    var d1 = null;
                    var d2 = null;

                    d1 = new Date(activities[x].OriginalActivityStartDate);
                    d2 = new Date(activities[x].OriginalActivityEndDate);
                    
                    d1.setDate(d1.getDate() + 1);
                    d2.setDate(d2.getDate() + 1);

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
                    var d1 = new Date(activities[x].OriginalActivityStartDate);
                    var d2 = new Date(activities[x].OriginalActivityEndDate);

                    if (d1 == 'Invalid Date' || d2 == 'Invalid Date') {
                        continue;
                    }

                    d1.setDate(d1.getDate() + 1);
                    d2.setDate(d2.getDate() + 1);

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
                var total = 0;
                console.log(phaseID, activities);
                for (var x = 0 ; x < activities.length; x++) {
                    if (phaseID == activities[x].PhaseCode) {
                        activitiesPerPhaseCounter++;
                        total += parseInt(activities[x].PercentageCompletion, 10);
                    }
                }

                if (total == 0) {
                    return '0%';
                } else {
                    return Math.round(total / activitiesPerPhaseCounter) + '%';
                }
            }

            //Get average percentage for Project
            function getProjectPercentageCompletion(activities) {
                var total = 0;
                for (var x = 0; x < activities.length; x++) {
                    total += parseInt(activities[x].PercentageCompletion, 10);
                }
                console.log(total);
                if (total == 0) {
                    return '0%';
                } else {
                    return Math.round(total / activities.length) + '%';
                }
            }

            //injectCost(5);
            $scope.scheduleGanttInstance.attachEvent("onBeforeLightBox", function (id) {


                alert("Activity cannot be added on this trend.");
                $scope.isDeleteFromLightbox = false;
                $scope.scheduleGanttInstance.deleteTask(id);
                $scope.costGanttInstance.clearAll();
                return false;

                //var task = $scope.scheduleGanttInstance.getTask(id);
                //if(task.text === "Add"){
                //    $scope.subCategory= [];
                //}
                //MainActivityCategory.get({"Phase": task.parent},function(response) {
                //    MainCategory = response.result;
                //    angular.forEach(MainCategory, function (value, key) {
                //        var obj = {};
                //        obj.key = value.CategoryDescription;
                //        obj.label = value.CategoryDescription;
                //        $scope.MainCategory.push(obj);
                //    });
                //    console.log($scope.MainCategory);
                //
                //    if( $scope.firstTime === true){
                //        $scope.firstTime = false;
                //        $scope.scheduleGanttInstance.updateCollection("main",$scope.MainCategory);
                //
                //        $scope.scheduleGanttInstance.showLightbox($scope.selectedId);
                //
                //    }
                //
                //    //$scope.scheduleGanttInstance.showLightbox($scope.selectedId);
                //});
                //return true;
            });
            $scope.scheduleGanttInstance.attachEvent("onLightBox", function (id, task, is_new) {
                $scope.lightBoxTask = $scope.scheduleGanttInstance.getTask(id);
                $scope.oneTime = true;
                $scope.isUpdateTaskFromLightbox = true;
                var task = $scope.scheduleGanttInstance.getTask(id);
                var taskList = task.text.split(' - ');
                //var row = $("#schedule-gantt .gantt_grid .gantt_grid_data ");
                //var cells = row.find('.gantt_row');
                //row.click(function(){
                //});
                var main = $scope.scheduleGanttInstance.getLightboxSection('mainphase');
                //sub.section.options = $scope.subCategory;
                if ($scope.temp) {
                    main.setValue($scope.temp);
                }
                var main = $scope.scheduleGanttInstance.getLightboxSection("mainphase");
                var sub = $scope.scheduleGanttInstance.getLightboxSection("subphase");

                if (task.text === "Add") {
                    $scope.cancel = true;

                }
                else {
                    $scope.cancel = false;
                }
                $scope.id = main.section.id;
                var taskList = task.text.split(" - ");
                console.log(taskList);
                if ($scope.temp !== "") {
                    // main.setValue($scope.temp);
                    //   sub.setValue($scope.tempSub);
                    $scope.temp = "";
                }
                else if ($scope.cancel === true) {
                    //sub.setValue("");
                    $scope.cancel = false;
                    main.setValue("");
                }
                else {
                    main.setValue(taskList[0]);
                    sub.setValue(taskList[1]);
                }
                return true;
            });
            $scope.cancel = false;
            $scope.scheduleGanttInstance.attachEvent("onLightboxCancel", function (id) {
                //  $scope.subCategory = [];
                $scope.subCategory = [];
                $scope.scheduleGanttInstance.updateCollection('sub', $scope.subCategory);

                $scope.MainCategory = [];
                $scope.scheduleGanttInstance.updateCollection('main', $scope.MainCategory);
                var sub = $scope.scheduleGanttInstance.getLightboxSection('subphase');
                //sub.section.options = [];
                $scope.firstTime = true;
                $scope.cancel = true;
            });
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
                    if (task.id == 8888) {
                        return "hideAdd phase-bold"
                    }
                    return "hideAdd ";
                }

                if (task.id >= 1000) {

                    return "phase-bold";
                }


                return "";
            };

            $scope.scheduleGanttInstance.templates.grid_header_class = function (column, config) {
                if (column == "add") {
                    return "hideHead";
                }
                return "";
            };

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
                    confirm_deleting: "The selected item be deleted permanently, are you sure?",
                    main: "Main activity",

                    section_subphase: "Sub Budget Category",
                    section_mainphase: "Main Budget Category",
                    section_start_date: "Start Date",
                    section_end_date: "End Date",
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
                $scope.isScaleChanged = true;
                var found = false;
                angular.forEach(phases, function (phase) {
                    //  console.log(phase);
                    if ($scope.schedulePhase === phase.Code && (task.parent === (Number(phase.PhaseID) * 1000)
                        || task.id === (Number(phase.PhaseID) * 1000))) {
                        found = true;
                    }

                    if ($scope.schedulePhase === "ALL")
                        found = true;
                });
                if (found == true)
                    return true;
                return false;
            });

            //$scope.scheduleGanttInstance.attachEvent("onBeforeTaskAdd",function(id, task){
            //   console.log(task);
            //});
            $scope.scheduleGanttInstance.attachEvent("onTaskDblClick", function (id) {
                //Do Nothing
                //$scope.subCategory = [];
                //
                //var CategoryID ;
                //var taskList = $scope.selectedActivity.text.split(" - ");
                //var task = $scope.scheduleGanttInstance.getTask(id);
                //MainActivityCategory.get({"Phase": task.parent},function(response) {
                //    MainCategory = response.result;
                //    console.log(MainCategory);
                //    angular.forEach(MainCategory, function (value, key) {
                //        var obj = {};
                //        obj.key = value.CategoryDescription;
                //        obj.label = value.CategoryDescription;
                //        $scope.MainCategory.push(obj);
                //    });
                //    console.log($scope.MainCategory);
                //
                //    if( $scope.firstTime === true){
                //        $scope.firstTime = false;
                //        $scope.scheduleGanttInstance.updateCollection("main",$scope.MainCategory);
                //
                //        $scope.scheduleGanttInstance.showLightbox($scope.selectedId);
                //
                //    }
                //    for(var i = 0; i < MainCategory.length; i++){
                //        if(MainCategory[i].CategoryDescription === taskList[0]){
                //            CategoryID = MainCategory[i].CategoryID;
                //        }
                //    }
                //    // SubActivityCategory.get({CategoryID: CategoryID},function(response){
                //    $http.get(serviceBasePath + "Request/SubActivityCategory/"+ CategoryID + "/" + task.parent).then(function(response){
                //        console.log(response);
                //        var subCategory = response.data.result;
                //        console.log(subCategory);
                //        for(var i = 0; i < subCategory.length;i++){
                //            var obj = {};
                //            obj.key = subCategory[i].SubCategoryDescription;
                //            obj.label = subCategory[i].SubCategoryDescription;
                //            $scope.subCategory.push(obj);
                //
                //        }
                //        console.log($scope.subCategory);
                //        $scope.scheduleGanttInstance.updateCollection("sub", $scope.subCategory);
                //
                //        $scope.scheduleGanttInstance.showLightbox($scope.selectedActivity.id);
                //        //$scope.scheduleGanttInstance.resetLightbox();
                //        //$scope.scheduleGanttInstance.config.lightbox.sections = $scope.s;
                //
                //    });
                //    //$scope.scheduleGanttInstance.showLightbox($scope.selectedId);
                //});
            });
            $scope.scheduleGanttInstance.attachEvent("customSelect", function (id) {
                console.log(id);
                var s = $scope.scheduleGanttInstance.getParent(id);
                $scope.selectedActivity = $scope.scheduleGanttInstance.getTask(id);
                var selectedActivityCost = [];

                if (id >= 1000) {
                    if (id == 8888) {
                        selectedActivityCost = delayedCost;
                    } else {
                        angular.forEach(delayedActivity, function (activity) {
                            console.log(activity);
                            if (activity.PhaseCode == (id / 1000)) {
                                angular.forEach(delayedCost, function (cost, index) {

                                    if (cost.ActivityID == activity.ActivityID) {
                                        selectedActivityCost.push(cost);
                                    }
                                });
                            }
                        });
                    }
                } else {

                    angular.forEach(delayedCost, function (cost, index) {
                        console.log(cost);
                        if (cost.ActivityID == $scope.selectedActivity.id) {
                            selectedActivityCost.push(cost);
                        }
                    });
                }
                var MaxFTECostID = 0;
                var MaxLumpSumCostID = 0;
                var MaxUnitCostID = 0;
                var MaxPercentageCostID = 0;

                for (var i = 0; i < selectedActivityCost.length; i++) {
                    if (selectedActivityCost[i].CostType == "F") {
                        MaxFTECostID++;
                    } else if (selectedActivityCost[i].CostType == "L") {
                        MaxLumpSumCostID++;
                    } else if (selectedActivityCost[i].CostType == "U") {
                        MaxUnitCostID++;
                    }
                }

                var firstChild = null;
                var childTask = null;
                $scope.allCostTotal = $scope.selectedActivity.totalCost;
                var testGetParent = $scope.scheduleGanttInstance.getParent(id);
                var numberOfChildren = $scope.scheduleGanttInstance.hasChild(id);
                if (numberOfChildren > 0) {
                    firstChild = $scope.scheduleGanttInstance.getChildren(id);
                    childTask = $scope.scheduleGanttInstance.getTask(firstChild[0]);
                }

                if (testGetParent === 8888 && numberOfChildren === false) {
                    $scope.costGanttInstance.clearAll();
                } else {


                    $scope.selectedId = id;
                    $scope.MaxFTECostID = 0;
                    $scope.MaxLumpSumCostID = 0;
                    $scope.MaxUnitCostID = 0;

                    $scope.MaxPercentageCostID = 0;
                    $scope.isCostEdited = [];
                    $scope.method = [];
                    $scope.description = [];
                    $scope.unit_type = [];
                    $scope.unitType = [];
                    $scope.totalUnits = [];
                    $scope.unitCost = [];
                    $scope.totalCost = [];

                    $scope.textBoxIds = [];
                    $scope.textBoxValues = [];
                    $scope.tempTBValues = [];
                    $scope.fteCosts = [];
                    $scope.unitCosts = [];
                    $scope.fteHours = [];

                    $scope.scale = [];
                    $scope.numberOfBoxes = [];
                    $scope.subCategory = [];
                    $scope.taskBeingEdited = false;
                    var childProject = (childTask) ? Number(childTask.project) : null;
                    $scope.selectedProject = isNaN(Number($scope.selectedActivity.project)) ? (childTask) ? Number(childTask.project) : null : Number($scope.selectedActivity.project);
                    $scope.selectedTrend = isNaN(Number($scope.selectedActivity.trend)) ? (childTask) ? Number(childTask.trend) : null : Number($scope.selectedActivity.trend);

                    if (Number(id) >= 1000) {
                        var phase = Number(id) / 1000;
                        var activity = "null";
                    }
                    else {
                        var phase = "null";
                        var activity = Number(id);
                    }


                    var positionsList;
                    FTEPositions.get({}, function (FTEPositionsData) {
                        positionsList = FTEPositionsData.result;
                        for (var i = 0; i < positionsList.length; i++) {
                            var position = {};
                            position["name"] = positionsList[i].PositionDescription;
                            position["value"] = positionsList[i].PositionID;
                            $scope.FTEPositions.push(position);
                        }
                    });
                    $scope.x = {
                        ProjectID: $scope.selectedProject,
                        TrendNumber: $scope.selectedTrend,
                        Phasecode: phase,
                        ActivityID: activity
                    }

                    $scope.currentCostIndex = 1;
                    $scope.MaxFTECostID = MaxFTECostID;
                    $scope.MaxLumpSumCostID = MaxLumpSumCostID;
                    $scope.MaxUnitCostID = MaxUnitCostID;
                    $scope.MaxPercentageCostID = MaxPercentageCostID;

                    var costs = selectedActivityCost;
                    console.log(costs);
                    $scope.costs.data = [];

                    if (costs.length > 0) {
                        for (var i = 0; i < costs.length; i++) {
                            var UT;
                            var costStartDate = costs[i].StartDate.split(',');
                            var costEndDate = costs[i].EndDate.split(',');
                            if (costs[i].CostType === "U") {
                                for (var j = 0; j < $scope.unitTypes.length; j++) {
                                    if ($scope.unitTypes[j].name === costs[i].UnitType) {
                                        UT = j;
                                    }
                                }
                            }
                            var cost = {};
                            cost["id"] = $scope.currentCostIndex;
                            cost["cost_id"] = costs[i].CostID;
                            cost["text"] =
                                (costs[i].CostType === "F") ? "FTE" :
                                    (costs[i].CostType === "L") ? "Lumpsum" :
                                        (costs[i].CostType === "U") ? "Unit" :
                                            (costs[i].CostType === "P") ? "% Basis" : "Error";
                            cost["description"] = costs[i].Description;
                            cost["unit_type"] =
                                (costs[i].CostType === "F") ? "Hours" :
                                    (costs[i].CostType === "L") ? "Lumpsum" :
                                        (costs[i].CostType === "U") ? costs[i].UnitType :
                                            (costs[i].CostType === "P") ? "Percentage" : "Error";
                            cost["unit_cost"] = costs[i].Base;
                            var totalCost = 0;

                            if (costs[i].FTECost) {
                                var individualCosts = costs[i].FTECost.split(",");

                                $.each(individualCosts, function (index) {
                                    totalCost += parseFloat(individualCosts[index]);
                                });
                            }
                            else {
                                var individualCosts = costs[i].TextBoxValue.split(",");

                                $.each(individualCosts, function (index) {
                                    totalCost += parseFloat(individualCosts[index]);
                                });
                            }

                            var totalUnits = 0;
                            if (costs[i].CostType === "F") {
                                cost["total_units"] = costs[i].FTEHours.split(",")[0];
                            }
                            else if (costs[i].CostType === "L") {
                                cost["total_units"] = "N/A";
                                cost["unit_cost"] = 'N/A'
                            }
                            else if (costs[i].CostType === "U") {
                                var individualUnits = costs[i].TextBoxValue.split(",");
                                //$scope.unit_type[$scope.currentCostIndex] = {name:costs[i].UnitType,value:$scope.unitTypes[UT].value};
                                $scope.unit_type[$scope.currentCostIndex] = {
                                    name: costs[i].UnitType,
                                    value: ($scope.unitTypes[UT]) ? $scope.unitTypes[UT].value:""
                                };
                                $.each(individualUnits, function (index) {
                                    totalUnits += parseFloat(individualUnits[index]);
                                });
                                cost["total_units"] = totalUnits;
                                totalCost = totalUnits * costs[i].Base;
                            } else if (costs[i].CostType === "P") {
                                var individualUnits = costs[i].TextBoxValue.split(",");

                                $.each(individualUnits, function (index) {
                                    totalUnits += parseFloat(individualUnits[index]);
                                });
                                cost["total_units"] = totalUnits;
                                totalCost = totalUnits * costs[i].Base / 100;
                            }

                            cost["total_cost"] = totalCost;
                            cost["scale"] = costs[i].Scale;
                            cost["open"] = false;
                            if ($scope.scheduleScale === 'week') {
                                var start = moment($scope.selectedActivity.start_date).clone().startOf('isoWeek');
                                if (moment($scope.selectedActivity.end_date).isoWeekday() == 1) {
                                    var end = moment($scope.selectedActivity.end_date);
                                } else {
                                    var end = moment($scope.selectedActivity.end_date).clone().endOf('isoWeek');
                                }
                                $scope.activity_start_of_week = start;
                                $scope.activity_end_of_week = end;
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
                            cost["delete"] = "<span class='notClickableFont'><i class='fa fa-trash'></i></span>";
                            cost["project"] = $scope.selectedProject;
                            cost["trend"] = $scope.selectedTrend;
                            cost["program_element"] = $scope.selectedActivity.ProgramElementID;
                            cost["program"] = $scope.selectedActivity.ProgramID;
                            cost["activity"] = $scope.selectedActivity.id;

                            $scope.method[$scope.currentCostIndex] = costs[i].CostType;
                            $scope.description[$scope.currentCostIndex] = {
                                name: costs[i].Description,
                                value: costs[i].FTEPositionID
                            };
                            $scope.unitType[$scope.currentCostIndex] = costs[i].Description;

                            if (costs[i].CostType === "F") {
                                var hoursArray = costs[i].FTEHours.split(",");
                                var costArray = costs[i].FTECost.split(",");

                                $.each(hoursArray, function (index) {
                                    $scope.totalUnits[$scope.currentCostIndex] += parseFloat(this);
                                });

                                $.each(costArray, function (index) {
                                    $scope.totalCost[$scope.currentCostIndex] += parseFloat(this);
                                });
                            }

                            $scope.unitCost[$scope.currentCostIndex] = costs[i].Base;

                            $scope.textBoxIds[$scope.currentCostIndex] = costs[i].TextBoxID.split(",");
                            $scope.textBoxValues[$scope.currentCostIndex] = costs[i].TextBoxValue.split(",");
                            //fix
                            //    console.log(costStartDate[0]);
                            //    console.log($scope.selectedActivity.original_start_date);
                            //    var cStartDate = moment(costStartDate[0]).format('ww');
                            //
                            //    var aStartDate = moment($scope.selectedActivity.original_start_date,'DD-MM-YYYY').format('ww');
                            //    var diff = cStartDate - aStartDate;
                            //    console.log(cStartDate);
                            //    console.log(aStartDate);
                            //    if(diff != 0){
                            //        for(var i = 0 ; i < diff; i ++){
                            //            $scope.textBoxValues[$scope.currentCostIndex].splice(i,0,0);
                            //            $scope.textBoxIds[$scope.currentCostIndex].splice(i,0,i);
                            //        }
                            //        console.log($scope.textBoxValues);
                            //        console.log($scope.textBoxIds);
                            //    }
                            $scope.scale[$scope.currentCostIndex] = costs[i].Granularity;
                            $scope.isCostEdited[$scope.currentCostIndex] = false;
                            $scope.isNewCost[$scope.currentCostIndex] = false;

                            $scope.currentCostIndex++;
                            $scope.costs.data.push(cost);

                        }
                    }

                    // Last empty row

                    cost = {};
                    cost["id"] = $scope.currentCostIndex;
                    cost["cost_id"] = "";
                    cost["text"] = "";
                    cost["description"] = "";
                    cost["unit_type"] = "";
                    cost["UnitType"] = "";
                    cost["unit_cost"] = "";
                    cost["total_units"] = "";
                    cost["total_cost"] = "";
                    cost["scale"] = "";
                    cost["open"] = false;
                    if ($scope.scheduleScale === "week") {
                        var start = moment($scope.selectedActivity.start_date).clone().startOf('isoWeek');
                        if (moment($scope.selectedActivity.end_date).isoWeekday() == 1) {
                            var end = moment($scope.selectedActivity.end_date);
                        } else {
                            var end = moment($scope.selectedActivity.end_date).clone().endOf('isoWeek');
                        }
                        $scope.activity_start_of_week = start;
                        $scope.activity_end_of_week = end;
                        cost["original_start_date"] = start;
                        cost["original_end_date"] = end;
                        cost["start_date"] = start;
                        cost["end_date"] = end;
                    }
                    if ($scope.scheduleScale === "month") {

                        var start = moment($scope.selectedActivity.start_date).clone().startOf('month');
                        var end = moment($scope.selectedActivity.end_date).clone().endOf('month');
                        $scope.activity_start_of_month = start;
                        $scope.activity_end_of_month = end;

                        cost["original_start_date"] = start;
                        cost["original_end_date"] = end;
                        cost["start_date"] = start;
                        cost["end_date"] = end;
                    }
                    if ($scope.scheduleScale === "year") {
                        var start = moment($scope.selectedActivity.start_date).clone().startOf('year');
                        var end = moment($scope.selectedActivity.end_date).clone().endOf('year');
                        $scope.activity_start_of_year = start;
                        $scope.activity_end_of_year = end;
                        cost["original_start_date"] = start;
                        cost["original_end_date"] = end;
                        cost["start_date"] = start;
                        cost["end_date"] = end;
                    }

                    cost["phase"] = $scope.selectedActivity.parent;
                    cost["save"] = "<span class='notClickableFont'><i class='fa fa-save'></i></span>";
                    cost["delete"] = "<span class='notClickableFont'><i class='fa fa-trash'></i></span>";
                    cost["project"] = $scope.selectedProject;
                    cost["trend"] = $scope.selectedTrend;
                    cost["program_element"] = $scope.selectedActivity.ProgramElementID;
                    cost["program"] = $scope.selectedActivity.ProgramID;
                    cost["activity"] = $scope.selectedActivity.id;
                    $scope.costs.data.push(cost);
                    $scope.isCostEdited[$scope.currentCostIndex] = false;
                    $scope.isNewCost[$scope.currentCostIndex] = true;

                    $scope.costGanttInstance.clearAll();
                    $scope.costGanttInstance.config.start_date = $scope.scheduleGanttInstance.getState().min_date;
                    $scope.costGanttInstance.config.end_date = $scope.scheduleGanttInstance.getState().max_date;
                    $scope.costGanttInstance.parse($scope.costs, "json");


                }
                //}
                return true;
            });
            $scope.scheduleGanttInstance.attachEvent("onTaskSelected", function (id) {
                var s = $scope.scheduleGanttInstance.callEvent("customSelect", [id]);

            });
            $scope.temp = "";
            var st = $scope.scheduleGanttInstance.attachEvent("customEvent", function (id) {
                return true;
            });

            $scope.schedulePhase = "ALL";
            $scope.oneTime = false;

            $scope.retrievedActivityID = 0;
            //$scope.scheduleGanttInstance.attachEvent("onGanttRender",function(){
            //    $scope.scheduleGanttInstance.eachTask(function(task){
            //        var row = $("#schedule-gantt .gantt_row[task_id='" + task.id + "']");
            //    //    var cells = row.find('.gantt_cell');
            //    //    console.log(task);
            //    //    console.log(row);
            //    //    console.log(cells);
            //    //    $(cells[4]).html(
            //    //        $compile(
            //    //            "<input  value='null' ng-model='totalCost' style='width:100%;height:100%; text-align:center; vertical-align: middle;'></input>"
            //    //        )($scope)
            //    //    );
            //    //});
            //});
            $scope.saveFromLightbox = false;
            $scope.scheduleGanttInstance.attachEvent("onLightboxSave", function (id, task, is_new) {
                if (is_new) {
                    var s = $scope.scheduleGanttInstance.getLabel('text');
                    $scope.saveFromLightbox = true;

                    // $scope.scheduleGanttInstance.deleteTask(id);
                    $scope.scheduleGanttInstance.refreshData();
                    $scope.scheduleGanttInstance.render();

                    var onTaskSelected = $scope.scheduleGanttInstance.callEvent("customAdd", [id, task]);
                    if (onTaskSelected) {
                    }
                } else {
                    $scope.isUpdateTaskFromLightbox = true;
                    $scope.isScaleChanged = false;
                }

                //Empty Main and Sub Category
                $scope.subCategory = [];
                $scope.scheduleGanttInstance.updateCollection('sub', $scope.subCategory);
                $scope.firstTime = true;
                $scope.MainCategory = [];
                $scope.scheduleGanttInstance.updateCollection('main', $scope.MainCategory);
                return true;
            });

            $scope.scheduleGanttInstance.attachEvent("customAdd", function (id, item) {
                var tempActivity;
                console.log(item);
                UpdateActivity.save({
                    "Operation": 1,
                    "ProjectID": delayedData[2].result[0].ProjectID,
                    "TrendNumber": delayedData[3],
                    "PhaseCode": parseInt(item.parent) / 1000,
                    "BudgetCategory": item.mainCategory,
                    "BudgetSubCategory": item.subCategory,
                    "ActivityStartDate": item.start_date.toISOString().substring(0, 10),
                    "ActivityEndDate": item.end_date.toISOString().substring(0, 10)


                }).$promise.then(function (response) {
                    //TODO

                    $scope.retrievedActivityID = parseInt(response.result.split(",")[1]);
                    //var task = $scope.scheduleGanttInstance.getTask($scope.retrievedActivityID);
                    //  $scope.scheduleGanttInstance.refreshData();
                    //  $state.reload();
                    //$scope.scheduleGanttInstance.changeTaskId(task.id, $scope.retrievedActivityID);
                    if (response.result.split(",")[0] === 'Success') {
                        //$state.reload();  //Temporary solution when add new activity
                        getCurrentDate();
                        var activityID = parseInt(response.result.split(",")[1]);
                        GetActivity.get({ ActivityID: activityID }, function (response) {
                            tempActivity = response.result;
                            var activity = {};
                            activity["id"] = Number(tempActivity.ActivityID);
                            activity["update_id"] = Number(tempActivity.ActivityID);
                            activity["text"] = tempActivity.BudgetCategory + " - " + tempActivity.BudgetSubCategory;
                            activity["original_start_date"] = moment(tempActivity.ActivityStartDate, "MM-DD-YYYY").format("DD-MM-YYYY");
                            activity["original_end_date"] = moment(tempActivity.ActivityEndDate, "MM-DD-YYYY").format("DD-MM-YYYY");
                            activity["start_date"] = moment(tempActivity.ActivityStartDate, "MM-DD-YYYY").format("DD-MM-YYYY");
                            activity["end_date"] = moment(tempActivity.ActivityEndDate, "MM-DD-YYYY").format("DD-MM-YYYY");

                            activity["parent"] = Number(tempActivity.PhaseCode) * 1000;

                            activity["program_element"] = tempActivity.ProgramElementID;
                            activity["program"] = tempActivity.ProgramID;
                            activity["project"] = tempActivity.ProjectID;
                            activity["trend"] = tempActivity.TrendNumber;
                            activity["phase"] = tempActivity.PhaseCode;
                            activity["totalCost"] = "$0";
                            $scope.schedule.data.push(activity);
                            $scope.scheduleGanttInstance.parse({ data: $scope.schedule.data, links: [] })

                        });
                        // $scope.scheduleGanttInstance.refreshData();
                    } else {
                        console.log('Failed to add new Activity');
                    }
                    // $scope.scheduleGanttInstance.refreshData();

                })
                return true;
            });

            $scope.scheduleGanttInstance.attachEvent("onAfterTaskUpdate", function (id, item) {


                if ($scope.isUpdateTaskFromLightbox === true && $scope.isScaleChanged === false) {
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
                    if (original_end_date != end_date || original_start_date != start_date) {

                        if (original_end_date > end_date) {                         //Drag right-left
                            weekDifference = moment(original_end_date).format('ww') - moment(end_date).format('ww');
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
                            //$q.all(costArray).then(function(data){
                            //});
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
                                        UpdateActivity.save(obj, function (response) {
                                            updateTrendDate()();
                                            $scope.reloadCost(selectedTask.id);
                                            item.text = mainCategory + " - " + subCategory;
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
                                "BudgetCategory": mainCategory,
                                "BudgetSubCategory": subCategory,
                                "ActivityID": $scope.selectedActivity.id,
                                "ActivityStartDate": start_date
                            };
                            UpdateActivity.save(obj, function (response) {
                                updateTrendDate()();
                                item.text = mainCategory + " - " + subCategory;
                                original_endDate = $scope.selectedActivity.end_date.toISOString().substring(0, 10);
                                $scope.selectedActivity.original_end_date = original_endDate.substring(8, 10) + original_endDate.substring(4, 7) + "-" + original_endDate.substring(0, 4);
                                $scope.reloadCost(selectedTask.id);
                            });
                        }
                    }
                    else {

                        UpdateActivity.save({
                            "Operation": 2,
                            "ActivityID": item.update_id,
                            "ProjectID": delayedData[2].result[0].ProjectID,
                            "TrendNumber": delayedData[3],
                            "PhaseCode": parseInt(item.parent) / 1000,
                            "BudgetCategory": mainCategory,
                            "BudgetSubCategory": subCategory,
                            "ActivityStartDate": item.start_date.toISOString().substring(0, 10),
                            "ActivityEndDate": item.end_date.toISOString().substring(0, 10)
                        }).$promise.then(function (response) {
                            updateTrendDate()();
                            item.text = mainCategory + " - " + subCategory;
                        })
                    }
                }
                if ($scope.isScaleChanged == true) {
                    $scope.isScaleChanged = false;
                }
                if ($scope.saveFromLightbox == true) {
                    $scope.scheduleGanttInstance.eachTask(function (tas) {
                        if (tas.id >= 1000 && tas.id < 8888) {
                            var start = moment(tas.start_date).format("DD/MM/YY");
                            var end = moment(tas.end_date).format("DD/MM/YY");
                            //var difference = moment(end).format("days") - moment(start).format("days");
                            var difference = moment(end).diff(moment(start), 'days') - 1;
                            if (tas.id === 1000) {
                                $scope.planning_start_date = moment(tas.start_date).format("MM/DD/YY");
                                $scope.planning_end_date = moment(tas.end_date).format("MM/DD/YY");
                            } else
                                if (tas.id === 2000) {
                                    $scope.schematic_design_start_date = moment(tas.start_date).format("MM/DD/YY");
                                    $scope.schematic_design_end_date = moment(tas.end_date).format("MM/DD/YY");
                                }
                                else if (tas.id === 3000) {
                                    $scope.design_bidding_start_date = moment(tas.start_date).format("MM/DD/YY");
                                    $scope.design_bidding_end_date = moment(tas.end_date).format("MM/DD/YY");
                                }
                                else if (tas.id === 4000) {
                                    $scope.construction_start_date = moment(tas.start_date).format("MM/DD/YY");
                                    $scope.construction_end_date = moment(tas.end_date).format("MM/DD/YY");
                                }
                                else if (tas.id === 5000) {
                                    $scope.closeout_start_date = moment(tas.start_date).format("MM/DD/YY");
                                    $scope.closeout_end_date = moment(tas.end_date).format("MM/DD/YY");

                                }
                        }


                    });
                }
                //$scope.scheduleGanttInstance.detachAllEvents();
                // $state.reload();
            }
            );

            $scope.scheduleGanttInstance.attachEvent("onLightboxDelete", function (id) {
                $scope.isDeleteFromLightbox = true;
                $scope.subCategory = [];
                $scope.scheduleGanttInstance.updateCollection('sub', $scope.subCategory);

                $scope.MainCategory = [];
                $scope.scheduleGanttInstance.updateCollection('main', $scope.MainCategory);
                $scope.firstTime = true;
                return true;
            });
            $scope.scheduleGanttInstance.attachEvent('onAfterTaskDelete', function (id, item) {
                //$scope.scheduleGanttInstance.refreshData();
                if ($scope.isDeleteFromLightbox == true) {

                    var phaseTask = $scope.scheduleGanttInstance.getTask(Number(item.phase) * 1000);
                    console.log(phaseTask);
                    var phaseTotalCost = 0;


                    phaseTask["totalCost"] = phaseTotalCost.toString();
                    $scope.allCostTotal = "0";

                    //if (1000 === phaseTask.id) {
                    //    $scope.planning_value =  phaseTotalCost;
                    //} else if (2000 === phaseTask.id) {
                    //    $scope.schematic_design_value =  phaseTotalCost;
                    //}
                    //else if (3000 === phaseTask.id) {
                    //    $scope.design_bidding_value =phaseTotalCost;
                    //}
                    //else if (4000 === phaseTask.id) {
                    //    $scope.construction_value =phaseTotalCost;
                    //}
                    //else if (5000 === phaseTask.id) {
                    //    $scope.closeout_value = phaseTotalCost;
                    //
                    //}

                    //updateBaselineValue();
                    $scope.isDeleteFromLightbox = false;
                }


            });
            $scope.scheduleGanttInstance.attachEvent("onBeforeTaskDelete", function (id, item) {
                //console.log($scope.selectedActivity);
                // console.log(moment(item.start_date,'MMMM Do YYYY, h:mm:ss a').format("YYYY-MM-DD"));
                //var index = -1;
                //console.log(item);
                //for(var i = 0; i < activities.length; i++){
                //    console.log(activities[i]);
                //    if(parseInt(activities[i].ActivityID)    === item.id ){
                //        index = i;
                //    }
                //}
                //UpdateActivity.save({
                //    "Operation": 3,
                //    "ActivityID":  item.id,
                //    "ProjectID": delayedData[2].result[0].ProjectID,
                //    "TrendNumber":  delayedData[3],
                //    "PhaseCode": parseInt(item.parent)/1000,
                //    "BudgetCategory": '',
                //    "BudgetSubCategory": item.text,
                //    "ActivityStartDate":item.start_date.toISOString().substring(0, 10) ,
                //    "ActivityEndDate":item.end_date.toISOString().substring(0, 10)
                //
                //
                //}).$promise.then(function (response) {
                //        if (response.result === 'Success') {
                //            //console.log(' Activity deleted successfuly');
                //            //$scope.scheduleGanttInstance.refreshTask(item.id);
                //            //for(var i in  $scope.schedule.data)
                //            updatePhase();
                //            updateTrendDate();
                //
                //            activities.splice(index,1);
                //            for(var i = 0; i < $scope.schedule.data.length; i++)
                //            {
                //                if($scope.schedule.data[i].id == item.id) {
                //                    $scope.schedule.data.splice(i, 1);
                //                    $scope.costGanttInstance.clearAll();
                //                }
                //            }
                //        } else {
                //            console.log(' Failed to delete  Activity');
                //        }
                //    })

            });
            //$scope.scheduleGanttInstance.attachEvent("OnBeforeTaskDrag",function(id,mode,event){
            //    //After task updated, highlight the selected task
            //    $scope.scheduleGanttInstance.toggleTaskSelection(id);
            //    $scope.scheduleGanttInstance.render();
            //});
            //$scope.costGanttInstance.attachEvent("onTaskSelected",function(id){
            //});

            $scope.scheduleGanttInstance.attachEvent('onAfterTaskAdd', function (id, item) {
                updateTrendDate();
                //  updatePhase();
                //$scope.subCategory = [];
                //$scope.scheduleGanttInstance.updateCollection('sub',$scope.subCategory);
                //
                //$scope.MainCategory = [];
                //$scope.scheduleGanttInstance.updateCollection('main', $scope.MainCategory);
                //updateBaselineValue();
            });
            $scope.scheduleGanttInstance.attachEvent("onAfterTaskDrag", function (id, mode, e) {
                //When drag right - update new dates ; drag-left : remove dates
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
                    weekDifference = moment(original_end_date).format('ww') - moment(end_date).format('ww');
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
                    //$q.all(costArray).then(function(data){
                    //    $"done");
                    //     console.log(data);
                    //});
                    var j = 0;
                    var temp;
                    for (i = 0; i < costArray.length; i++) {
                        $http.post(url, costArray[i]).then(function (response) {
                            if ((j + 1) == costArray.length) {
                                var obj = {
                                    "Operation": 4,
                                    "ActivityEndDate": end_date,
                                    "BudgetCategory": mainCategory,
                                    "BudgetSubCategory": subCategory,
                                    "ActivityID": $scope.selectedActivity.id,
                                    "ActivityStartDate": start_date
                                };
                                UpdateActivity.save(obj, function (response) {
                                    $scope.reloadCost(selectedTask.id);
                                    updateTrendDate()();
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
                        "BudgetCategory": mainCategory,
                        "BudgetSubCategory": subCategory,
                        "ActivityID": $scope.selectedActivity.id,
                        "ActivityStartDate": start_date
                    };
                    UpdateActivity.save(obj, function (response) {
                        updateTrendDate();
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
                $scope.updateStatus = "";
                $scope.selectedCost = costObj;
                var original_end_date;
                var original_start_date;
                var start_array = [];
                var end_array = [];
                var operation;
                var start_date;
                var numberOfTextboxToBeRemoved;
                if (drag_direction === "right-left" || drag_direction === "left-right") {
                    operation = "4";
                    original_end_date = $scope.selectedActivity.original_end_date.substring(6, 10);//yyyy
                    original_end_date += $scope.selectedActivity.original_end_date.substring(2, 5);//-mm
                    original_end_date += "-" + $scope.selectedActivity.original_end_date.substring(0, 2);//dd
                    //Original Start Date

                    original_start_date = $scope.selectedActivity.original_start_date.substring(6, 10);//yyyy
                    original_start_date += $scope.selectedActivity.original_start_date.substring(2, 5);//-mm
                    original_start_date += "-" + $scope.selectedActivity.original_start_date.substring(0, 2);//dd
                    //var start = moment($scope.selectedActivity.original_start_date).format("YYYY-MM-DD");
                    //var end = moment($scope.selectedActivity.original_end_date).format("YYYY-MM-DD");
                    if (drag_direction === "left-right") {
                        start_date = $scope.selectedActivity.start_date.toISOString().substring(0, 10);
                        numberOfTextboxToBeRemoved = moment(start_date).format("ww") - moment(original_start_date).format("ww");
                    }

                } else if (drag_direction === "left-left") {
                    operation = "5";
                    original_end_date = $scope.selectedActivity.original_end_date.substring(6, 10);//yyyy
                    original_end_date += $scope.selectedActivity.original_end_date.substring(2, 5);//-mm
                    original_end_date += "-" + $scope.selectedActivity.original_end_date.substring(0, 2);//dd
                    //Original Start Date

                    original_start_date = $scope.selectedActivity.start_date.toISOString().substring(0, 10);

                }
                var difference = moment(original_end_date).format("ww") - moment(original_start_date).format("ww");

                for (var i = 0; i < difference; i++) {
                    start_array.push(original_start_date);
                    original_end_date = moment(original_start_date).add(6, 'days').format("YYYY-MM-DD");
                    end_array.push(original_end_date);
                    original_start_date = moment(original_start_date).add(1, 'w').format("YYYY-MM-DD");
                }
                var index = 0;
                for (var i = 0; i < $scope.methods.length; i++) {
                    if ($scope.methods[i].name === $scope.selectedCost.text) {
                        index = i;
                    }
                }
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
                    "CostType": $scope.methods[index].value,
                    "Description": $scope.selectedCost.description,
                    "Scale": $scope.scheduleScale,
                    "StartDate": start_array.join(","),
                    "EndDate": end_array.join(","),
                    "TextBoxValue": $scope.textBoxValues[costID].join(","),
                    "Base": $scope.unitCost[costID],
                    "Drag_Direction": drag_direction,
                    "NumberOfTextboxToBeRemoved": numberOfTextboxToBeRemoved,
                    "FTEIDList": $scope.FTECostID.join(",")
                };
                if (cost.CostType == "F") {
                    cost["FTEHours"] = $scope.totalUnits[costID];
                    cost["FTECost"] = $scope.fteCosts[costID].join(",")
                }
                // Only for update

                cost["CostID"] = $scope.selectedCost.cost_id;
                //  $scope.method[$scope.currentCostIndex] = cost.CostType;
                // $scope.description[$scope.currentCostIndex] = {name: cost.Description, value: null};
                //$scope.unitType[$scope.currentCostIndex] = "Hours";
                //$scope.selectedCost.text =
                //    (cost.CostType === "F")? "FTE" :
                //        (cost.CostType === "L")? "Lumpsum" :
                //            (cost.CostType === "U")? "Unit" :
                //                (cost.CostType === "P")? "% Basis" : "Error";
                //$scope.selectedCost.description = cost.Description;
                //$scope.selectedCost.unit_type = (cost.CostType === "F")? "Hours" :
                //    (cost.CostType === "L")? "Lumpsum" :
                //        (cost.CostType === "U")? "Unit" :
                //            (cost.CostType === "P")? "%" : "Error";
                //$scope.selectedCost.cost_id = cost.CostID;
                //$scope.selectedCost.total_units = (cost.CostType === "L")? "null" : $scope.totalUnits[$scope.currentCostIndex];
                //
                //$scope.selectedCost.unit_cost = (cost.CostType === "L")? "null" : $scope.unitCost[$scope.currentCostIndex];
                //$scope.selectedCost.total_cost = $scope.totalCost[$scope.currentCostIndex];
                //Saving Cost to the database
                //console.log(cost);
                return cost;
                //    $scope.costGanttInstance.updateTask($scope.selectedCost.id);

            }
            // Cost Gantt Chart Configuration
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
            $scope.costGanttInstance.config.grid_width = 900;
            $scope.costGanttInstance.config.hide_tooltip = true;

            $scope.costGanttInstance.config.columns = [
              //  {name:"save", label:"", width:'30px', align: "center"},
                { name: "delete", label: "", width: '30px', align: "center" },
                { name: "text", label: "Type", tree: false, width: '100px', align: "left" },
                { name: "description", label: "Title/Description", width: '200px', align: "left" },
                { name: "total_units", label: "# Of Units", align: "center" },
                { name: "unit_type", label: "Unit Type", width: '100px', align: "center" },
                { name: "unit_cost", label: "Unit Cost", align: "center" },
                {
                    name: "total_cost", label: "Cost", align: "right", template: function (obj) {
                        if (obj.total_cost != 0) return $filter('currency')(obj.total_cost, '$', formatCurrency(obj.total_cost));
                        else return '';
                    }
                }
            ];

            $scope.costGanttInstance.config.lightbox.sections = [
                { name: "method", height: 38, map_to: "text", type: "select", options: [{ key: 'FTECost', label: 'FTE' }, { key: 'LumpsumCost', label: 'Lumpsum' }, { key: 'UnitCost', label: 'Unit' }, { key: 'PercentBasisCost', label: 'Percent' }], focus: true },
                { name: "description", height: 28, map_to: "description", type: "textarea", focus: true },
                { name: "base", height: 28, map_to: "base", type: "textarea", focus: true },
                { name: "scale", height: 38, map_to: "scale", type: "select", options: [{ key: 'W', label: 'W' }, { key: 'M', label: 'M' }, { key: 'Y', label: 'Y' }], focus: true }
            ];

            $scope.costGanttInstance.templates.task_class = function (start, end, task) {
                return "costBar";
            };

            $scope.costGanttInstance.templates.grid_row_class = function (start, end, task) {
                return "hideAdd";
            };

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

            function renderCostBoxes(widthOfTextBox, numberOfBoxes, id) {
                $scope.numberOfBoxes[id] = numberOfBoxes;
                /*

                 if($scope.scheduleScale == "week") {
                 $scope.numberOfWeeks = numberOfBoxes;
                 } else if($scope.scheduleScale == "month") {
                 $scope.numberOfMonths = numberOfBoxes;
                 } else if ($scope.scheduleScale == "year"){
                 $scope.numberOfYears = numberOfBoxes;
                 }
                 */

                var relWidth = 33;
                var cssClass = "costBox " + id.toString() + "_cost";
                var costBoxes = '';
                costBoxes = "<div class='" + cssClass + "'>";
                if ($scope.isCostEdited[id] === true) {
                    for (var i = 0; i < numberOfBoxes; i++) {
                        costBoxes += "<input  ng-model='textBoxValues[" + id + "][" + i + "]' class='" + id.toString() + "_costText' ng-blur='changedCost(" + id + "," + i + ")'  type='text' style='width:" + widthOfTextBox + "px; text-align:center;' />"
                    }
                }
                else {

                    for (var i = 0; i < numberOfBoxes; i++) {
                        costBoxes += "<input disabled = true ng-model='textBoxValues[" + id + "][" + i + "]' class='" + id.toString() + "_costText' ng-blur='changedCost(" + id + "," + i + ")'  type='text' style='width:" + widthOfTextBox + "px; text-align:center;' />"
                    }
                }

                costBoxes += "</div>";
                return costBoxes;
            }

            $scope.costGanttInstance.templates.task_text = function (start, end, task) {
                var tempTask = task;
                if ($scope.scheduleScale === "week") {
                    var sizes = $scope.costGanttInstance.getTaskPosition(tempTask);
                } else if ($scope.scheduleScale === "month") {
                    var sizes = $scope.costGanttInstance.getTaskPosition(task, $scope.activity_start_of_month, $scope.activity_end_of_month);
                } else if ($scope.scheduleScale === "year") {
                    var sizes = $scope.costGanttInstance.getTaskPosition(task, $scope.activity_start_of_year, $scope.activity_end_of_year);
                }
                var numberOfBoxes = 0;

                var year1 = moment($scope.activity_start_of_month).format('YYYY');
                var year2 = moment($scope.activity_end_of_month).format('YYYY');

                var month1 = moment($scope.activity_start_of_month).format('MM');
                var month2 = moment($scope.activity_end_of_month).format('MM');

                var week1 = moment(task.start_date).format('ww');
                var week2 = moment(task.end_date).format('ww');

                if ($scope.scheduleScale === "week") {
                    var startDate = (moment($scope.activity_start_of_week).isoWeekday() === 1) ? $scope.activity_start_of_week
                        : moment($scope.activity_start_of_week).startOf('isoWeek').format("YYYY-MM-DD");
                    var endDate = (moment($scope.activity_end_of_week).isoWeekday() === 7) ? $scope.activity_end_of_week
                        : moment($scope.activity_end_of_week).endOf('isoWeek').format("YYYY-MM-DD");

                    var st = moment(startDate).format('YYYY-MM-DD');
                    var ed = moment(endDate).format('YYYY-MM-DD');
                    year1 = moment(startDate).format('YYYY');
                    year2 = moment(endDate).format('YYYY');
                    var ONE_WEEK = 1000 * 60 * 60 * 24 * 7;  //# of seconds in a week
                    // Convert both dates to milliseconds
                    var date1_ms = moment(ed).format('YYYY-MM-DD');
                    var date2_ms = moment(st).format('YYYY-MM-DD');
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
                    //$scope.isMonthSchedule = true;
                    //var copyTextBoxValues = angular.copy($scope.textBoxValues);
                    //$scope.textBoxValues = [];
                    //numberOfBoxes = (year2 - year1) * 12 + (month2 - month1) + 1;
                    //var difference = moment(task.original_end_date).format("ww") - moment(task.original_start_date).format("ww");
                    //var count = 0;
                    //var k = 0;
                    //for (var i = 0; i < difference; i++) {
                    //    start_array.push(start_date);
                    //    end_date = moment(start_date).add(6, 'days').format("YYYY-MM-DD");
                    //    end_array.push(end_date);
                    //    start_date = moment(start_date).add(1, 'w').format("YYYY-MM-DD");
                    //}
                    //for(var j = 1; j < start_array.length; j++){
                    //    if(moment(start_array[j]).format('MM') - moment(start_array[j - 1]).format('MM') === 0){
                    //        count++;
                    //    }
                    //    else{
                    //        count++;
                    //        $scope.textBoxValues[task.id] = [];
                    //        $scope.textBoxValues[task.id][k] =0;
                    //        for(var c = 0; c < count ; c++) {
                    //            $scope.textBoxValues[task.id][k] += copyTextBoxValues[task.id][c];
                    //        }
                    //        $scope.textBoxValues[task.id][k] /= count;
                    //        count = 0;
                    //        k++;
                    //        if(j + 1 === start_array.length){
                    //            var date = start_array[j];
                    //            var next_date = moment(date).add(6,'days').format("YYYY-MM-DD");
                    //            while(moment(next_date).format("MM") - moment(date).format("MM")===0){
                    //                count++;
                    //                date = next_date;
                    //                next_date = moment(next_date).add(6,'days').format("YYYY-MM-DD");
                    //            }
                    //            count++;
                    //            for(var d = 0; d < count; d++){
                    //                $scope.textBoxValues[task.id][k] /= count;
                    //            }
                    //            count = 0;
                    //            k++;
                    //        }
                    //    }
                    //}
                }
                else if ($scope.scheduleScale === "year")
                    numberOfBoxes = moment(end).format('YYYY') - moment(start).format('YYYY') + 1;
                var widthOfTextBox = sizes.width / numberOfBoxes;
                return renderCostBoxes(widthOfTextBox, numberOfBoxes, task.id);
            };

            $scope.costGanttInstance.attachEvent("onBeforeTaskDisplay", function (id, task) {
                $scope.isScaleChanged = true;
                var found = false;

                angular.forEach(phases, function (phase) {
                    if ($scope.schedulePhase === phase.Code && task.phase === (Number(phase.PhaseID) * 1000)) {

                        found = true;
                    }

                    if ($scope.schedulePhase === "ALL")
                        found = true;
                });
                if (found == true)
                    return true;
                return found;
                //return true;
            });
            $scope.costGanttInstance.attachEvent("onGanttRender", function () {
                $scope.isScaleChanged = true;

                $scope.costGanttInstance.eachTask(function (task) {
                    if ($scope.deleteFromNew === true) {
                        //if ($scope.tempTextBoxValues[task.id] || $scope.isNewCost[task.id] === true) {
                        $scope.textBoxIds[task.id] = ["0", "1", "2", "3"];
                        //}

                    }

                    var div = $("." + task.id + "_cost");

                    $(div).html(
                        $compile(
                            $(div).html()
                        )($scope)
                    );

                    //var textBoxes = $(div).children();

                    console.log($scope.textBoxIds);
                    if ($scope.textBoxIds[task.id]) {
                        var textBoxVals = Array.apply(null, new Array($scope.numberOfBoxes[task.id])).map(Number.prototype.valueOf, 0);
                        console.log(textBoxVals);
                        for (var i = 0; i < $scope.textBoxIds[task.id].length; i++) {
                            var textBoxid = parseInt($scope.textBoxIds[task.id][i]);
                            textBoxVals[textBoxid] = parseFloat($scope.textBoxValues[task.id][i]);
                        }
                        console.log(textBoxVals);

                        $scope.textBoxValues[task.id] = textBoxVals;
                    }
                    else {
                        $scope.textBoxIds[task.id] = Array.apply(null, new Array($scope.numberOfBoxes[task.id])).map(Number.prototype.valueOf, 0);
                        $scope.textBoxValues[task.id] = Array.apply(null, new Array($scope.numberOfBoxes[task.id])).map(Number.prototype.valueOf, 0);
                    }

                    $scope.fteCosts[task.id] = Array.apply(null, new Array($scope.numberOfBoxes[task.id])).map(Number.prototype.valueOf, 0);
                    $scope.unitCosts[task.id] = Array.apply(null, new Array($scope.numberOfBoxes[task.id])).map(Number.prototype.valueOf, 0);
                    $scope.fteHours[task.id] = Array.apply(null, new Array($scope.numberOfBoxes[task.id])).map(Number.prototype.valueOf, 0);
                });
                //$scope.deleteFromNew = false;
                return true;
            });

            $scope.costGanttInstance.attachEvent("onAfterTaskUpdate", function (id, item) {
                if ($scope.isMonthSchedule === false) {
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
                        for (var i = 0; i < $scope.textBoxIds[task.id].length; i++) {
                            var textBoxid = parseInt($scope.textBoxIds[task.id][i]);
                            textBoxVals[textBoxid] = parseFloat($scope.textBoxValues[task.id][i]);
                        }
                        $scope.textBoxValues[task.id] = textBoxVals;

                        $scope.fteCosts[task.id] = Array.apply(null, new Array($scope.numberOfBoxes[task.id])).map(Number.prototype.valueOf, 0);
                        $scope.fteHours[task.id] = Array.apply(null, new Array($scope.numberOfBoxes[task.id])).map(Number.prototype.valueOf, 0);
                    });
                }

            });

            $scope.$watch('schedulePhase', function () {
                $scope.scheduleGanttInstance.render();
                $scope.costGanttInstance.render();
            });

            $scope.$watch('scheduleScale', function () {
                $scope.costGanttInstance.clearAll();
                switch ($scope.scheduleScale) {
                    case "week":
                        var current = currentProject.get({ ProjectID: delayedProject.ProjectID, Granularity: 'week' }, function (result) {
                            console.log(result.result);
                            delayedCost = result.result[3];
                            if ($scope.selectedActivity) {
                                if ($scope.selectedActivity.text != "Add") {
                                    $scope.scheduleGanttInstance.callEvent('onTaskSelected', [$scope.selectedActivity.id]);
                                    //$scope.scheduleGanttInstance.selectTask($scope.selectedActivity.id);
                                    $scope.scheduleGanttInstance.eachTask(function (task) {
                                        if (task.type !== "project") {
                                            var startDate = moment(task.original_start_date, "DD-MM-YYYY");
                                            var endDate = moment(task.original_end_date, "DD-MM-YYYY");

                                            task.start_date = startDate.toDate();
                                            task.end_date = endDate.toDate();
                                            $scope.scheduleGanttInstance.updateTask(Number(task.id));
                                        }
                                    });
                                    $scope.costGanttInstance.eachTask(function (task) {
                                        if (task.type !== "project") {
                                            var startDate = moment(task.original_start_date, "DD-MM-YYYY");
                                            var endDate = moment(task.original_end_date, "DD-MM-YYYY");

                                            task.start_date = startDate.toDate();
                                            task.end_date = endDate.toDate();
                                            $scope.costGanttInstance.updateTask(Number(task.id));
                                        }
                                    });
                                }
                            }
                        });
                        $("#weekBtn").css("background-color", "gray");
                        $("#monthBtn").css("background-color", "black");
                        $("#yearBtn").css("background-color", "black");



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
                        $scope.isScaleChanged = true;
                        break;
                    case "month":
                        var current = currentProject.get({ ProjectID: delayedProject.ProjectID, Granularity: 'month' }, function (result) {
                            console.log(result.result);
                            delayedCost = result.result[3];
                            if ($scope.selectedActivity) {
                                if ($scope.selectedActivity.text != "Add") {
                                    $scope.scheduleGanttInstance.callEvent('onTaskSelected', [$scope.selectedActivity.id]);
                                    //$scope.scheduleGanttInstance.selectTask($scope.selectedActivity.id);

                                }
                            }
                        });

                        $scope.scheduleGanttInstance.eachTask(function (task) {
                            if (task.type !== "project") {
                                var startDate = moment(task.original_start_date, "DD-MM-YYYY");
                                var endDate = moment(task.original_end_date, "DD-MM-YYYY");

                                task.start_date = startDate.toDate();
                                task.end_date = endDate.toDate();
                                $scope.scheduleGanttInstance.updateTask(Number(task.id));
                            }
                        });
                        $scope.costGanttInstance.eachTask(function (task) {
                            if (task.type !== "project") {
                                var startDate = moment(task.original_start_date, "DD-MM-YYYY");
                                var endDate = moment(task.original_end_date, "DD-MM-YYYY");

                                task.start_date = startDate.toDate();
                                task.end_date = endDate.toDate();
                                $scope.costGanttInstance.updateTask(Number(task.id));
                            }
                        });
                        $("#weekBtn").css("background-color", "black");
                        $("#monthBtn").css("background-color", "gray");
                        $("#yearBtn").css("background-color", "black");


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
                        $scope.isScaleChanged = true;

                        break;
                    case "year":
                        var current = currentProject.get({ ProjectID: delayedProject.ProjectID, Granularity: 'year' }, function (result) {
                            console.log(result.result);
                            delayedCost = result.result[3];
                            if ($scope.selectedActivity) {
                                if ($scope.selectedActivity.text != "Add") {
                                    $scope.scheduleGanttInstance.callEvent('onTaskSelected', [$scope.selectedActivity.id]);

                                }
                            }
                        });
                        $("#weekBtn").css("background-color", "black");
                        $("#monthBtn").css("background-color", "black");
                        $("#yearBtn").css("background-color", "gray");
                        $scope.scheduleGanttInstance.eachTask(function (task) {
                            if (task.type !== "project") {
                                var startDate = moment(task.original_start_date, "DD-MM-YYYY");
                                var endDate = moment(task.original_end_date, "DD-MM-YYYY");

                                task.start_date = startDate.toDate();
                                task.end_date = endDate.toDate();
                                $scope.scheduleGanttInstance.updateTask(Number(task.id));
                            }
                        });
                        $scope.costGanttInstance.eachTask(function (task) {
                            if (task.type !== "project") {
                                var startDate = moment(task.original_start_date, "DD-MM-YYYY");
                                var endDate = moment(task.original_end_date, "DD-MM-YYYY");

                                task.start_date = startDate.toDate();
                                task.end_date = endDate.toDate();
                                $scope.costGanttInstance.updateTask(Number(task.id));
                            }
                        });


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
                        $scope.isScaleChanged = true;

                        break;
                }
            });


            $scope.scheduleGanttInstance.attachEvent('onTaskUnselected', function () {
                currentId = -1;
            });
            //init gantt
            $scope.scheduleGanttInstance.init("schedule-gantt");
            $scope.scheduleScale = "week";
            $scope.scheduleGanttInstance.parse({ data: $scope.schedule.data, links: [] });

            $scope.scheduleGanttInstance.attachEvent('onGanttRender', function () {
                var isTrue = false;
                var id = 0;

                $scope.scheduleGanttInstance.eachTask(function (task) {
                    // console.log(task);
                    if (task.$level == 2 && isTrue == false) {
                        id = task.id;
                        $scope.selectedActivity = $scope.scheduleGanttInstance.getTask(id);
                        isTrue = true;
                    }
                    if (isTrue && id != 0) {
                        $scope.scheduleGanttInstance.selectTask(id);
                        return;
                    }
                })
            });
            //$scope.scheduleGanttInstance.eachTask(function(task){
            //    if(task.id >= 1000 && task.id < 8888){
            //        var start = moment(task.start_date).format("YYYY-MM-DD");
            //        var end = moment(task.end_date).format("YYYY-MM-DD");
            //
            //        //var difference = moment(end).format("days") - moment(start).format("days");
            //        var difference = moment(end).diff(moment(start),'days') -1 ;
            //        if(task.id === 1000){
            //            $scope.planning_start_date= moment(task.start_date).format("MM/DD/YY");
            //            $scope.planning_end_date = moment(task.end_date).format("MM/DD/YY");
            //        }else
            //        if(task.id === 2000){
            //            $scope.schematic_design_start_date= moment(task.start_date).format("MM/DD/YY");
            //            $scope.schematic_design_end_date = moment(task.end_date).format("MM/DD/YY");
            //        }
            //        else if(task.id === 3000){
            //            $scope.design_bidding_start_date= moment(task.start_date).format("MM/DD/YY");
            //            $scope.design_bidding_end_date = moment(task.end_date).format("MM/DD/YY");
            //        }
            //        else if(task.id === 4000){
            //            $scope.construction_start_date= moment(task.start_date).format("MM/DD/YY");
            //            $scope.construction_end_date = moment(task.end_date).format("MM/DD/YY");
            //        }
            //        else if(task.id === 5000){
            //            $scope.closeout_start_date= moment(task.start_date).format("MM/DD/YY");
            //            $scope.closeout_end_date = moment(task.end_date).format("MM/DD/YY");
            //
            //        }
            //    }
            //
            //});
            //updateBaselineValue();
            var t = $(".gantt_add").get();
            for (var i = 0; i < t.length; i++) {
                //  console.log($(t[i]).toggleClass('gantt_add'));
            }

            $scope.costGanttInstance.init("cost-gantt");
            //getProjectValue();

            var test;
            var s = $scope.scheduleGanttInstance.callEvent('customEvent', [test]);
            if (s == true) {

            }
            //watch data collection, reload on changes
            $scope.$watch("schedule", function (collection) {
                $scope.scheduleGanttInstance.clearAll();
                setTimeout(function () {
                    $scope.scheduleGanttInstance.parse(collection, "json");
                }, 0);
            }, true);

            $($scope.scheduleGanttInstance.$task).scroll(function () {
                $($scope.costGanttInstance.$task).scrollLeft($($scope.scheduleGanttInstance.$task).scrollLeft());
            });
            $($scope.costGanttInstance.$task).scroll(function () {
                $($scope.scheduleGanttInstance.$task).scrollLeft($($scope.costGanttInstance.$task).scrollLeft());
            });

            $scope.taskBeingEdited = false;
            getCurrentDate();
            $scope.costGanttInstance.attachEvent('onTaskDblClick', function () {
                //Do Nothing
            });
            function updateTrendValue(costId) {
                var totalCost = 0;
                var costActivity = $scope.costGanttInstance.getTask(costId);
                var costParent = $scope.scheduleGanttInstance.getTask(costActivity.activity);
                var costPhase = $scope.scheduleGanttInstance.getTask(costParent.parent); //phaseID

                //console.log(costParent);
                $scope.costGanttInstance.eachTask(function (cost) {
                    if (cost.total_cost) {
                        totalCost += parseFloat(cost.total_cost);
                    }

                });
                $scope.allCostTotal = totalCost;
                costParent["totalCost"] = (totalCost).toString();
                //var phaseTotal = CalculatePhaseTotal(activities, parseInt(costActivity.phase));


                var phaseTotalCost = 0;
                $scope.scheduleGanttInstance.eachTask(function (task) {

                    if (task.parent === costPhase.id) {
                        console.log(task);
                        var c = parseInt(task.totalCost.substring(1, task.totalCost.length));
                        console.log(c);
                        phaseTotalCost += parseInt(c);

                    }
                });

                console.log(phaseTotalCost);
                if (1000 === costPhase.id) {
                    //$scope.planning_value =  phaseTotalCost.toString();
                    costPhase["totalCost"] = phaseTotalCost.toString();
                } else if (2000 === costPhase.id) {
                    //$scope.schematic_design_value =  phaseTotalCost.toString();
                    costPhase["totalCost"] = phaseTotalCost.toString();
                }
                else if (3000 === costPhase.id) {
                    //$scope.design_bidding_value =  phaseTotalCost.toString();
                    costPhase["totalCost"] = phaseTotalCost.toString();
                }
                else if (4000 === costPhase.id) {
                    //$scope.construction_value =  phaseTotalCost.toString();
                    costPhase["totalCost"] = phaseTotalCost.toString();
                }
                else if (5000 === costPhase.id) {
                    //$scope.closeout_value = phaseTotalCost.toString();
                    costPhase["totalCost"] = phaseTotalCost.toString();

                }
            }
            function isCostsEdited() {
                for (var i = 1; i < $scope.isCostEdited.length; i++) {
                    if (false !== $scope.isCostEdited[i]) {
                        return false;
                    }
                }
                return true;
            }
            //$scope.costGanttInstance.attachEvent("onTaskClick", function(id,e){
            //    if($scope.deleteFromNew === true || $scope.isCostEdited[id] === true){
            //        if($scope.deleteFromNew === true) {
            //            $scope.deleteFromNew = false;
            //        }
            //        currentId = -1;
            //    }
            //    else {
            //        var clickResult = $scope.costGanttInstance.callEvent('customClick', [id]);
            //        $scope.isCostEdited[id] = true;
            //    }
            //});

            //var approvalData = google.visualization.arrayToDataTable([
            //    ['Stage', 'Stage'],
            //    ['Project Manager', 2],
            //    ['Program Manager', 2],
            //    ['ESC', 2],
            //    ['Executive Director', 2]
            //]);
            //
            //var fundingData = google.visualization.arrayToDataTable(
            //    [
            //        ['Task', 'Hours per Day'],
            //        ['AIP',     8],
            //        ['PFC',      4],
            //        ['Bonds',  5],
            //        ['Grant', 4]
            //    ]);
            //            $scope.costGanttInstance.attachEvent("onTaskDblClick",function(id){
            //
            //            });
            //            $scope.costGanttInstance.attachEvent('customClick',function(id){
            //                var s = isCostsEdited();
            //
            //                if(s === true && $scope.testt !== true ){
            //                    currentId = -1;
            //
            //                }
            //                var temp = $scope.costGanttInstance.getTask(id);
            //                $scope.selectedCost = temp;
            //
            //                if(currentId != id){
            //                    currentId = id;
            //                    if(id) {
            //
            //                        //if ($scope.taskBeingEdited == false) {
            //                            $scope.taskBeingEdited = true;
            //                            //$scope.taskBeingEdited = false;
            //
            //                            $scope.selectedCost = $scope.costGanttInstance.getTask(id);
            //                            var activityId = $scope.selectedCost.activity;
            //                            var costActivity = $scope.scheduleGanttInstance.getTask(activityId);
            //                            var phaseTask = $scope.scheduleGanttInstance.getTask(Number(costActivity.phase) * 1000);
            //                            //var sizes =  $scope.costGanttInstance.getTaskPosition($scope.selectedCost,$scope.selectedCost.start_date,$scope.selectedCost.end_date);
            //                            // var widthOfTextBox = sizes.width/$scope.textBoxValues[id].length;
            //                            // var numberOfTextBox = $scope.textBoxValues[id].length;
            //                            // console.log(sizes);
            //                            // var s=   renderCostBoxes(widthOfTextBox,numberOfTextBox , id);
            //                            if ($scope.selectedCost) {
            //                                if ($scope.selectedCost.description === "") {
            //                                    $scope.isSave = false;
            //                                } else {
            //                                    $scope.isSave = true;
            //                                }
            //                            }
            //                            costCalculation(id);
            //                            var rowBox = $("#cost-gantt .costBar[task_id='" + id + "']");
            //                            var box = rowBox.find('.' + id + '_costText');
            //                            $(box).each(function (index) {
            //                                console.log(index);
            //                                $(this).removeAttr('disabled');
            //                            });
            //                            var row = $("#cost-gantt .gantt_row[task_id='" + id + "']");
            //                            var cells = row.find('.gantt_cell');
            //                            console.log(cells);
            //                            $(cells).each(function (index) {
            //                                var value = $(this).children(0).text();
            //                                switch (index) {
            //                                    case 0:
            //                                        var span = $(this).children(0).children(0);
            //                                        span.css('color', 'blue');
            //                                        span.removeClass('notClickableFont').addClass('clickableFont');
            //                                        span.off('click').on('click', function () {
            //                                            $scope.costGanttInstance.eachTask(function(costTask){
            //                                                costCalculation(costTask.id);
            //                                                costActivity = costTask;
            //                                                var FTECostID = [];
            //                                                var maxId = 0;
            //
            //                                                if ($scope.method[costActivity.id] == "F") {
            //                                                    maxId = Number($scope.MaxFTECostID) + 1;
            //
            //                                                }
            //                                                else if ($scope.method[costActivity.id] == "L") {
            //                                                    maxId = Number($scope.MaxLumpSumCostID) + 1;
            //
            //                                                }
            //                                                else if ($scope.method[costActivity.id] == "U") {
            //                                                    maxId = Number($scope.MaxUnitCostID) + 1;
            //
            //
            //                                                }
            //                                                else if ($scope.method[costActivity.id] == "P") {
            //                                                    maxId = Number($scope.MaxPercentageCostID) + 1;
            //                                                }
            //
            //                                                for(var j = 0; j < $scope.textBoxValues[costActivity.id].length; j++){
            //
            //                                                    //if(costActivity.id == $scope.currentCostIndex){
            //
            //                                                    if(costActivity.cost_id === ""){
            //
            //                                                        FTECostID.push(costActivity.activity + "_" + maxId + "_" + j);
            //                                                    }
            //                                                    else{
            //                                                        if(Number(costActivity.cost_id) !== costActivity.id) {
            //
            //                                                            FTECostID.push(costActivity.activity + "_" + costActivity.cost_id + "_" + j);
            //                                                        }
            //                                                        else {
            //                                                            FTECostID.push(costActivity.activity + "_" + costActivity.id + "_" + j);
            //                                                        }
            //                                                    }
            //
            //                                                }
            //                                                var start = moment(costActivity.start_date).format("YYYY-MM-DD");
            //                                                var end = moment(costActivity.end_date).format("YYYY-MM-DD");
            //
            //                                                var difference = moment(end).format("ww") - moment(start).format("ww");
            //                                                var start_array = [];
            //                                                var end_array = [];
            //
            //                                                for (var i = 0; i < difference; i++) {
            //                                                    start_array.push(start);
            //                                                    end = moment(start).add(6, 'days').format("YYYY-MM-DD");
            //                                                    end_array.push(end);
            //                                                    start = moment(start).add(1, 'w').format("YYYY-MM-DD");
            //                                                }
            //
            //                                                var costID = costActivity.id;
            //
            //
            //                                                var cost = {
            //                                                    "Operation": "2",
            //                                                    "ProgramID": costActivity.program,
            //                                                    "ProgramElementID": costActivity.program_element,
            //                                                    "ProjectID": costActivity.project,
            //                                                    "TrendNumber": costActivity.trend,
            //                                                    "ActivityID": costActivity.activity,
            //                                                    "CostID": "",
            //                                                    "CostType": $scope.method[costActivity.id],
            //                                                    "Description": $scope.description[costActivity.id].name,
            //                                                    "Scale": $scope.scheduleScale,
            //                                                    "StartDate": start_array.join(","),
            //                                                    "EndDate": end_array.join(","),
            //                                                    "TextBoxValue": $scope.textBoxValues[costActivity.id].join(","),
            //                                                    "Base": $scope.unitCost[costActivity.id],
            //                                                    "UnitType": "",
            //                                                    "FTEIDList" : FTECostID.join(',')
            //                                                };
            //                                                var numb = $scope.currentCostIndex;
            //                                                if (cost.CostType == "F") {
            //                                                    cost["FTEHours"] = $scope.totalUnits[costID];
            //                                                    cost["FTECost"] = $scope.fteCosts[costID].join(",")
            //                                                }
            //
            //
            //                                                // Only for insert
            //                                                if (costActivity.cost_id === "" && costActivity.id !== $scope.currentCostIndex) {
            //                                                    if ($scope.method[costActivity.id] == "F") {
            //                                                        cost["CostID"] = Number($scope.MaxFTECostID) + 1;
            //                                                        $scope.MaxFTECostID++;
            //                                                    }
            //                                                    else if ($scope.method[costActivity.id] == "L") {
            //                                                        cost["CostID"] = Number($scope.MaxLumpSumCostID) + 1;
            //                                                        $scope.MaxLumpSumCostID++;
            //                                                    }
            //                                                    else if ($scope.method[costActivity.id] == "U") {
            //                                                        cost["CostID"] = Number($scope.MaxUnitCostID) + 1;
            //                                                        cost["unit_type"] = $scope.unit_type.name;
            //
            //                                                        $scope.MaxUnitCostID++;
            //                                                    }
            //                                                    else if ($scope.method[costActivity.id] == "P") {
            //                                                        cost["CostID"] = Number($scope.MaxPercentageCostID) + 1;
            //                                                        $scope.MaxPercentageCostID++;
            //                                                    }
            //                                                }
            //                                                // For edit
            //                                                else {
            //                                                    numb--;
            //                                                    cost["CostID"] = costActivity.cost_id;
            //
            //                                                }
            //                                                if (cost.CostType == "U") {
            //
            //                                                    cost["UnitType"] = $scope.unit_type[costActivity.id].name;
            //                                                }
            //                                                $scope.method[costActivity.id] = cost.CostType;
            //                                                $scope.description[costActivity.id] = {
            //                                                    name: cost.Description,
            //                                                    value: null
            //                                                };
            //                                                $scope.unitType[costActivity.id] = "Hours";
            //
            //                                                costActivity.text =
            //                                                    (cost.CostType === "F") ? "FTE" :
            //                                                        (cost.CostType === "L") ? "Lumpsum" :
            //                                                            (cost.CostType === "U") ? "Unit" :
            //                                                                (cost.CostType === "P") ? "% Basis" : "Error";
            //                                                costActivity.description = cost.Description;
            //                                                costActivity.unit_type = (cost.CostType === "F") ? "Hours" :
            //                                                    (cost.CostType === "L") ? "Lumpsum" :
            //                                                        (cost.CostType === "U") ? $scope.unit_type.name :
            //                                                            (cost.CostType === "P") ? "%" : "Error";
            //                                                costActivity.cost_id = cost.CostID;
            //                                                costActivity.total_units = (cost.CostType === "L") ? "N/A" : $scope.totalUnits[costActivity.id];
            //
            //                                                costActivity.unit_cost = (cost.CostType === "L") ? "N/A" : $scope.unitCost[costActivity.id];
            //                                                costActivity.total_cost = $scope.totalCost[costActivity.id];
            //
            //
            //                                                //var currentActivityCost = parseFloat(costActivity.totalCost.substring(1,costActivity.totalCost.lenght));
            //
            //
            //                                                //Saving Cost to the database
            //                                                InsertCost.save(cost, function (response) {
            //
            //                                                    if (response.result === 'Success') {
            //                                                        //$scope.reloadCost($scope.selectedId);
            //                                                        //Update phase total and activity total
            //                                                        for(var i = 1; i < $scope.isCostEdited.length;i++){
            //                                                            $scope.isCostEdited[i]= false;
            //                                                        }
            //
            //                                                        getProjectValue(costID);
            //                                                        getCurrentDate();
            //                                                        $scope.reloadCost($scope.selectedId,$scope.selectedCost.id);
            //                                                        updateTrendValue(costID);
            //                                                        currentId = -1;
            //
            //
            //
            //
            //                                                    }
            //
            //
            //                                                })
            //
            //
            //                                                //  $scope.costGanttInstance.updateTask($scope.selectedCost.id);
            //                                                //Update activity Cost
            //                                                // Only after insert add empty row: begin
            //                                                //if (costActivity.id == $scope.currentCostIndex) {
            //                                                //    $scope.currentCostIndex++;
            //                                                //    var cost = {};
            //                                                //    cost["id"] = $scope.currentCostIndex;
            //                                                //    cost["cost_id"] = "";
            //                                                //    cost["text"] = "";
            //                                                //    cost["description"] = "";
            //                                                //    cost["unit_type"] = "";
            //                                                //    cost["unit_cost"] = "";
            //                                                //    cost["total_units"] = "";
            //                                                //    cost["total_cost"] = "";
            //                                                //    cost["scale"] = "";
            //                                                //    cost["open"] = false;
            //                                                //    cost["original_start_date"] = costActivity.original_start_date;
            //                                                //    cost["original_end_date"] = costActivity.original_end_date;
            //                                                //    cost["start_date"] = costActivity.start_date;
            //                                                //    cost["end_date"] = costActivity.end_date;
            //                                                //    cost["phase"] = costActivity.parent;
            //                                                //    cost["save"] = "<span class='notClickableFont'><i class='fa fa-save'></i></span>";
            //                                                //    cost["delete"] = "<span class='notClickableFont'><i class='fa fa-trash'></i></span>";
            //                                                //    cost["project"] = $scope.selectedProject;
            //                                                //    cost["trend"] = $scope.selectedTrend;
            //                                                //    cost["activity"] = costActivity.id;
            //                                                //    // if($scope.retrievedActivityID != 0){
            //                                                //    //    cost["activity"] = $scope.retrievedActivityID;
            //                                                //    //    $scope.retrieveActivity = 0;
            //                                                //    //}
            //                                                //
            //                                                //    $scope.costs.data.push(cost);
            //                                                //
            //                                                //    $scope.costGanttInstance.clearAll();
            //                                                //    $scope.costGanttInstance.config.start_date = $scope.scheduleGanttInstance.getState().min_date;
            //                                                //    $scope.costGanttInstance.config.end_date = $scope.scheduleGanttInstance.getState().max_date;
            //                                                //    $scope.costGanttInstance.parse($scope.costs, "json");
            //                                                //}
            //
            //// Only after insert add empty row: end
            //                                                span.css('color', '#454545');
            //                                                span.removeClass('clickableFont').addClass('notClickableFont');
            //                                                $scope.taskBeingEdited = false;
            //
            //                                            });
            //
            //                                        });
            //
            //
            //                                        break;
            //                                    case 1:
            //                                        var span = $(this).children(0).children(0);
            //                                        span.css('color', 'red');
            //                                        span.removeClass('notClickableFont').addClass('clickableFont');
            //                                        console.log($scope.selectedCost);
            //                                        span.on('click', function () {
            //                                            var FTECostID = [];
            //                                            for(var j = 0; j < $scope.textBoxValues[$scope.selectedCost.id].length; j++){
            //                                                FTECostID.push($scope.selectedCost.activity+"_"+$scope.selectedCost.cost_id+"_"+j);
            //
            //                                            }
            //                                            if ($scope.isSave === true && $scope.isNewCost[id] ===false) {  //Delete Saved tasks
            //                                                var cost = {
            //                                                    "Operation": "3",
            //                                                    "ProgramID": $scope.selectedCost.program,
            //                                                    "ProgramElementID": $scope.selectedCost.program_element,
            //                                                    "ProjectID": $scope.selectedCost.project,
            //                                                    "TrendNumber": $scope.selectedCost.trend,
            //                                                    "ActivityID": $scope.selectedCost.activity,
            //                                                    "CostID": $scope.selectedCost.cost_id,
            //                                                    "CostType": $scope.method[$scope.selectedCost.id],
            //                                                    "Description": "",
            //                                                    "Scale": "",
            //                                                    "StartDate": "",
            //                                                    "EndDate": "",
            //                                                    "TextBoxValue": "",
            //                                                    "Base": "",
            //                                                    "FTEHours": "",
            //                                                    "FTECost": "",
            //                                                    "FTEIDList": FTECostID.join(',')
            //                                                };
            //                                                var temp = $scope.selectedCost.id;
            //                                                InsertCost.save(cost, function (response) {
            //                                                    currentId = -1;
            //                                                    for(var i = 1; i < $scope.isCostEdited.length;i++){
            //                                                        $scope.isCostEdited[i]= false;
            //                                                    }
            //                                                    getProjectValue();
            //                                                    $scope.isNewCost.splice(id,1);
            //                                                    getCurrentDate();
            //                                                    $scope.reloadCost($scope.selectedId);
            //                                                    $scope.costGanttInstance.clearAll();
            //                                                });
            //                                            }
            //                                            else {  //Delete unsaved tasks
            //
            //                                                $scope.costs.data.splice(id,1);
            //                                                $scope.currentCostIndex--;
            //                                                if(id < $scope.costs.data.length ) {
            //                                                    for (var j = id ; j < $scope.costs.data.length; j++) {
            //                                                        $scope.costs.data[j].id = parseFloat($scope.costs.data[j].id) - 1;
            //                                                    }
            //                                                }
            //                                                console.log($scope.costs.data);
            //                                                $scope.textBoxValues.splice(id,1);
            //
            //                                                $scope.tempTextBoxValues = angular.copy($scope.textBoxValues);
            //                                                $scope.isNewCost.splice(id,1);
            //                                                $scope.isCostEdited.splice(id,1);
            //
            //                                                $scope.deleteFromNew = true;
            //                                                $scope.testt = true;
            //                                                $scope.costGanttInstance.clearAll();
            //                                                $scope.costGanttInstance.parse({data:$scope.costs.data, links:[]});
            //                                                $scope.method.splice(id,1);
            //                                                $scope.description.splice(id,1);
            //                                                $scope.totalUnits.splice(id,1);
            //                                                $scope.unitType.splice(id,1);
            //                                                //if(typeof $scope.UnitCost[id] !== 'undefined') {
            //                                                //    $scope.UnitCost.splice(id, 1);
            //                                                //}
            //
            //                                                $scope.totalCost.splice(id,1);
            //
            //                                                console.log($scope.costs.data);
            //                                                var s = $scope.costGanttInstance.callEvent('onGanttRender',[]);
            //                                                console.log($scope.isCostEdited);
            //
            //                                                for(var i = 1; i < $scope.isCostEdited.length ; i++){
            //                                                    if($scope.isCostEdited[i]===true){
            //                                                        $scope.costGanttInstance.callEvent('customClick',[i]);
            //                                                    }
            //                                                }
            //                                            }
            //                                            //Update Costs
            //                                            var totalCost =0;
            //                                            //costActivity["totalCost"] = (newCost + currentActivityCost).toString();
            //                                            $scope.costGanttInstance.eachTask(function(cost){
            //                                                if(cost.total_cost) {
            //                                                    if(cost.id !== $scope.selectedCost.id)
            //                                                        totalCost += parseFloat(cost.total_cost);
            //                                                }
            //
            //                                            });
            //                                            $scope.allCostTotal = "$" +totalCost;
            //                                            costActivity["totalCost"] = "$" + (totalCost).toString();
            //                                            //var phaseTotal = CalculatePhaseTotal(activities, parseInt(costActivity.phase));
            //                                            var phaseTask = $scope.scheduleGanttInstance.getTask(Number(costActivity.phase) * 1000);
            //
            //                                            var phaseTotalCost = 0;
            //                                            $scope.scheduleGanttInstance.eachTask(function(task){
            //                                                if(task.phase === costActivity.phase){
            //
            //                                                    var c = parseInt(task.totalCost.substring(1,task.totalCost.length));
            //                                                    phaseTotalCost += parseInt(c);
            //
            //                                                }
            //                                            })
            //
            //                                            phaseTask["totalCost"] = "$"+ phaseTotalCost.toString();
            //
            //                                            if(1000 === phaseTask.id){
            //                                                $scope.planning_value = "$" + phaseTotalCost;
            //                                            }else
            //                                            if(2000 === phaseTask.id){
            //                                                $scope.schematic_design_value = "$" + phaseTotalCost;
            //                                            }
            //                                            else if(3000 === phaseTask.id){
            //                                                $scope.design_bidding_value = "$" + phaseTotalCost;
            //                                            }
            //                                            else if(4000 === phaseTask.id){
            //                                                $scope.construction_value = "$" + phaseTotalCost;
            //                                            }
            //                                            else if(5000 === phaseTask.id){
            //                                                $scope.closeout_value = "$" + phaseTotalCost;
            //
            //                                            }
            //                                            phaseTask["totalCost"] = "$"+ phaseTotalCost.toString();
            //
            //
            //                                            $scope.taskBeingEdited = false;
            //                                        });
            //
            //                                        break;
            //                                    case 2:
            //                                        $scope.tempTextBoxValues = angular.copy($scope.textBoxValues);
            //                                        var span = $(this).children(0).children(0);
            //                                        //
            //                                        //if(id > $scope.currentCostIndex){
            //                                        //    $scope.currentCostIndex = id;
            //                                        //}
            //
            //                                        if ($scope.isNewCost[id] === true) {
            //                                            $(this).html(
            //                                                $compile(
            //                                                    "<select ng-model='method[" + id + "]' style='width:100%;height:100%; text-align:center; vertical-align: top;' ng-options='option.value as option.name for option in methods' ng-change='changedMethod(" + id + ")'></select>"
            //                                                )($scope)
            //                                            );
            //                                        }
            //                                        else {
            //                                            $(this).html(
            //                                                $compile(
            //                                                    "<input disabled='true' value=" + value + " style='width:100%;height:100%; text-align:center; vertical-align: top;'></input>"
            //                                                )($scope)
            //                                            );
            //
            //                                        }
            //
            //
            //                                        break;
            //                                    case 3:
            //                                        if ($scope.method[id] == "F") {
            //                                            $(this).html(
            //                                                $compile(
            //                                                    "<select ng-model='description[" + id + "]' style='width:100%;height:100%; text-align:center; vertical-align: top;' ng-options='option.name for option in FTEPositions track by option.value' ng-change='changedDescription(" + id + ")'></input>"
            //                                                )($scope)
            //                                            );
            //                                        }
            //                                        else {
            //
            //                                            $(this).html(
            //                                                $compile(
            //                                                    "<input ng-model='description[" + id + "].name'  style='width:100%;height:100%; text-align:center; vertical-align: top;' ng-change='changedDescription(" + id + ")'></input>"
            //                                                )($scope)
            //                                            );
            //                                        }
            //                                        break;
            //                                    case 5:
            //                                        if ($scope.method[id] == "U") {
            //                                            //console.log($scope.description[id]);
            //                                            //console.log($scope.unit_type);
            //                                            //$scope.unit_type = {name:$scope.unit_type};
            //                                            console.log($scope.unit_type[id]);
            //                                            $(this).html(
            //                                                $compile(
            //                                                    "<select  ng-model= 'unit_type[" + id + "]' style='width:100%;height:100%; text-align:center; vertical-align: top;'" +
            //                                                    " ng-options=' type.name for type in unitTypes track by type.value ' ng-change='changedUnitType(" + id + ")'></input>"
            //                                                )($scope)
            //                                            );
            //                                        }
            //                                        else if ($scope.method[id] == "L") {
            //                                            $(this).html(
            //                                                $compile(
            //                                                    "<input disabled=true  value='Lumpsum' style='width:100%;height:100%; text-align:center; vertical-align: top;'></input>"
            //                                                )($scope)
            //                                            )
            //
            //                                        }
            //                                        else if ($scope.method[id] == "F") {
            //                                            $(this).html(
            //                                                $compile(
            //                                                    "<input disabled='true' value='Hours' style='width:100%;height:100%; text-align:center; vertical-align: top;'></input>"
            //                                                )($scope)
            //                                            )
            //                                        }
            //
            //                                        break;
            //                                    case 4:
            //
            //                                       if ($scope.method[id] == "F" || $scope.method[id] == "U") {
            //                                            $(this).html(
            //                                                $compile(
            //                                                    "<input ng-model='totalUnits[" + id + "]' disabled='true'  style='width:100%;height:100%; text-align:center; vertical-align: top;' ng-blur='changedTotalUnits(" + id + ")'></input>"
            //                                                )($scope)
            //                                            );
            //                                        }
            //                                        else if ($scope.method[id] == "L") {
            //                                            $(this).html(
            //                                                $compile(
            //                                                    "<input disabled='true' value='N/A' style='width:100%;height:100%; text-align:center; vertical-align: top;'></input>"
            //                                                )($scope)
            //                                            )
            //                                        }
            //                                        break;
            //                                    case 6:
            //
            //                                       if ($scope.method[id] == "F") {
            //                                            $(this).html(
            //                                                $compile(
            //                                                    "<input ng-model='unitCost[" + id + "]' disabled=true style='width:100%;height:100%; text-align:center; vertical-align: top;' ng-blur='changedUnitCost(" + id + ")'></input>"
            //                                                )($scope)
            //                                            );
            //                                        }
            //                                        else if ($scope.method[id] == "L") {
            //                                            $(this).html(
            //                                                $compile(
            //                                                    "<input disabled='true' value='N/A' style='width:100%;height:100%; text-align:center; vertical-align: top;'></input>"
            //                                                )($scope))
            //                                        } else if ($scope.method[id] == "U") {
            //                                            $(this).html(
            //                                                $compile(
            //                                                    "<input ng-model='unitCost[" + id + "]'  style='width:100%;height:100%; text-align:center; vertical-align: top;' ng-blur='changedUnitCost(" + id + ")'></input>"
            //                                                )($scope)
            //                                            );
            //                                        }
            //                                        break;
            //                                    case 7:
            //
            //                                            $(this).html(
            //                                                $compile(
            //                                                    "<input ng-model='totalCost[" + id + "]' disabled=true style='width:100%;height:100%; text-align:center; vertical-align: top;' ng-change='changedTotalCost(" + id + ")'></input>"
            //                                                )($scope)
            //                                            );
            //
            //                                        break;
            //                                }
            //                            });
            //
            //
            //
            //                    }
            //                }
            //                if($scope.testt === true ){
            //                    currentId = -1;
            //                    $scope.testt = false;
            //                }
            //            return true;
            //            });

            //var approvalOptions = {
            //    legend : {position: 'labeled', textStyle:{fontSize:9}},
            //    colors:['green','red', 'red','red'],
            //    sliceVisibilityThreshold: 0,
            //    width: 269,
            //    height: 100,
            //    pieHole: 0.4,
            //    pieSliceText : 'none',
            //    chartArea: {'width': '70%', 'height': '70%'}
            //};
            //
            //var fundingOptions = {
            //    legend : {position: 'labeled', textStyle:{fontSize:10}},
            //    colors:['16a085','#2980b9', '#e74c3c','#95a5a6'],
            //    sliceVisibilityThreshold: 0,
            //    width: 269,
            //    height: 100,
            //    pieHole: 0.4,
            //    pieSliceText : 'none',
            //    chartArea: {'width': '60%', 'height': '70%'},
            //};
            //
            //var approvalDonut = new google.visualization.PieChart(document.getElementById('approval-donut'));
            //var fundingDonut = new google.visualization.PieChart(document.getElementById('funding-donut'));

            //approvalDonut.draw(approvalData, approvalOptions);
            //fundingDonut.draw(fundingData, fundingOptions);

            /* Code for Report */
            $scope.chooseReport = function () {
                console.log(delayedData);
                console.log('Mohit BaseLine Controller');
                var scope = $rootScope.$new();
                var newOrEdit = false;
                scope.params = {
                    newOrEdit: newOrEdit,
                    ProjectID: delayedData[0].result[0][0].ProjectID,
                    TrendNumber: delayedData[0].result[4].TrendNumber,
                    trendType: "current"
                };
                $rootScope.modalInstance = $uibModal.open({
                    backdrop: 'static',
                    keyboard: false,
                    scope: scope,
                    templateUrl: "app/views/Modal/TrendReportModal.html",
                    size: "md",
                    controller: "TrendReportCtrl"
                });
                $rootScope.modalInstance.result.then(function (response) { });
            };

            //Get Tableau Token
            $scope.generateToken = function () {
                console.log(delayedData);
                console.log('Mohit Current Controller');
                var scope = $rootScope.$new();
                var newOrEdit = false;
                scope.params = {
                    newOrEdit: newOrEdit,
                    ProjectID: delayedData[0].result[0][0].ProjectID,
                    TrendNumber: delayedData[0].result[4].TrendNumber,
                    trendType: "current"
                };
                $rootScope.modalInstance = $uibModal.open({
                    backdrop: 'static',
                    keyboard: false,
                    scope: scope,
                    templateUrl: "app/views/Modal/TrendReportModal.html",
                    size: "md",
                    controller: "TrendReportCtrl"
                });
                $rootScope.modalInstance.result.then(function (response) { });
            }



        }]);
