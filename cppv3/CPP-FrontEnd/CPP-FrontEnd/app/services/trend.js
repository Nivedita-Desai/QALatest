'use strict';

angular.module('cpp.services').

    factory("TrendId",function($resource){
        return $resource(serviceBasePath + "Request/trend/:trendId/:projectId");
    }).
    factory("TrendApprovalTrackLog", function ($resource) {
        return {
            getAllApprovalTimer: function () {
                return $resource(serviceBasePath + "Request/ApprovalTimerCountDown/:ProjectID/:TrendNumber/:UserID/:CurrentThreshold",
                    { ProjectID: "@ProjectID", TrendNumber: "@TrendNumber", UserID: "@UserID", CurrentThreshold: "@CurrentThreshold" });
            },
            getApprovedCodeSubmitted: function () {
                return $resource(serviceBasePath + "Request/TrendCodeSubmitted/:ProjectID/:TrendNumber/:UserID/:UniqueCode/:CurrentThreshold",
                    { ProjectID: "@ProjectID", TrendNumber: "@TrendNumber", UserID: "@UserID", UniqueCode: "@UniqueCode", CurrentThreshold: "@CurrentThreshold" });
            },
            ResendUnqiuecode: function () {
                return $resource(serviceBasePath + "Request/ResendUniqueCode/:ProjectID/:TrendId/:UserID/:UniqueCode",
                    { ProjectID: "@ProjectID", TrendId: "@TrendId", UserID: "@UserID", UniqueCode: "@UniqueCode" });
            }

        }
    }).
    factory('Trend', function ($resource) {
        return {
            lookup : function() {

				return $resource( serviceBasePath+"Request/Trend/:ProgramID/:ProgramElementID/:ProjectID/:TrendNumber/:KeyStroke",
                    {ProgramID:'@ProgramID',ProgramElementID:"@ProgramElementID",ProjectID:"@ProjectID",TrendNumber:"@TrendNumber", KeyStroke:"@KeyStroke"} );
            },
            persist : function() {
                return $resource(serviceBasePath+"Response/Trend");
            },
            getAllTrends: function () {
                return $resource(serviceBasePath + "Request/AllTrend/:ProjectID",
                    { ProjectID: "@ProjectID" });
            },
            ResendTrendApprovalLink: function () {
                return $resource(serviceBasePath + "Request/ResendTrendApprovalLink/:ProjectID/:TrendId/:TrendNumber/:UserID",
                    { ProjectID: "@ProjectID", TrendId: "@TrendId", TrendNumber: "@TrendNumber", UserID: "@UserID" });
            },
            getAllTrendsForChangeOrderList: function () { // Added by Jignesh 24-11-2020
                return $resource(serviceBasePath + "Request/AllTrendForChangeOrderList/:ProjectID",
                    { ProjectID: "@ProjectID" });
            },
        }
    }).
    factory('RequestApproval',function($resource){
        return $resource(serviceBasePath + "Request/Service/RequestApproval/:UserID/:Role/:TrendID/:ProjectID",{UserID:'@UserID',Role:'@Role',TrendID:'@TrendID',ProjectID:'@ProjectID'});
    }).
    factory('TrendFund',function($resource){
        return{
            lookup : function(){
                return $resource(serviceBasePath + "Request/TrendFund/:TrendID/:ProjectID");
            },
            persist : function(){
                return $resource(serviceBasePath + "Response/TrendFund");
            }

        }
    }).
    factory('currentTrend',function($resource){
       return $resource(serviceBasePath + "Request/currentTrend/:ProjectID");
    }).
    factory('futureProject',function($resource){
      return $resource(serviceBasePath + "Request/FutureProject/:ProjectID");
    }).
    factory('Status', function($resource){
        return $resource(serviceBasePath+"Request/trendstatus");
        //return $resource("http://localhost:29986/api/Request/trendstatus");
    }).
    
    service('LoadData', function LoadData() {
        return {
            getData: function (activityArray) {
                if (!angular.isUndefined(activityArray)) {
                    var returnThis = [];


                    var phase = {name: 'All activities', children: []};
                    var activities = [];

                    for (var i = 0; i < activityArray.length; i++) {

                        console.log(activityArray[i]);
                        var activity =
                        {
                            name: activityArray[i].ActivityID, tasks: [
                            {
                                name: '',
                                color: '#68B828',
                                from: moment(activityArray[i].ActivityStartDate, "YYYY-MM-DD"),
                                to: moment(activityArray[i].ActivityEndDate, "YYYY-MM-DD"),
                                data: {
                                    "ActivityEndDate": activityArray[i].ActivityEndDate,
                                    "ActivityID": activityArray[i].ActivityID,
                                    "ActivityStartDate": activityArray[i].ActivityStartDate,
                                    "BudgetCategory": activityArray[i].BudgetCategory,
                                    "BudgetSubCategory": activityArray[i].BudgetSubCategory,
                                    "FTECost": activityArray[i].FTECost,
                                    "FTECosts": activityArray[i].FTECosts,
                                    "LumpsumCost": activityArray[i].LumpsumCost,
                                    "LumpsumCosts": activityArray[i].LumpsumCosts,
                                    "Operation": activityArray[i].Operation,
                                    "PercentageBasisCost": activityArray[i].PercentageBasisCost,
                                    "PercentageCosts": activityArray[i].PercentageCosts,
                                    "ProgramID": activityArray[i].ProgramID,
                                    "ProgramElementID": activityArray[i].ProgramElementID,
                                    "PhaseCode": activityArray[i].PhaseCode,
                                    "ProjectID": activityArray[i].ProjectID,
                                    "TrendNumber": activityArray[i].TrendNumber,
                                    "UnitCost": activityArray[i].UnitCost,
                                    "UnitCosts": activityArray[i].UnitCosts
                                }
                            }
                        ]
                        };

                        phase.children.push(activityArray[i].ActivityID);
                        activities.push(activity);
                    }

                    returnThis.push(phase);
                    var returnArray = returnThis.concat(activities);

                    return returnArray;
                }
            },
            getCostData: function (costArray, activityArray) {
                if (!angular.isUndefined(costArray)) {
                    var returnThis = [];

                    var costs = [];

                    for (var i = 0; i < costArray.length; i++) {

                        var cost =
                        {
                            name: 'FTE', tasks: [
                            {
                                name: '',
                                color: '#68B828',
                                from: moment(activityArray.ActivityStartDate, "YYYY-MM-DD"),
                                to: moment(activityArray.ActivityEndDate, "YYYY-MM-DD"),
                                data: {
                                    "Operation":costArray[i].Operation,
                                    "CostType":costArray[i].CostType,
                                    "ActivityID":costArray[i].ActivityID,
                                    "Granularity":costArray[i].Granularity,
                                    "TextBoxID":costArray[i].TextBoxID,
                                    "CostID":costArray[i].CostID,
                                    "Description":costArray[i].Description,
                                    "StartDate":costArray[i].StartDate,
                                    "EndDate":costArray[i].EndDate,
                                    "TextBoxValue":costArray[i].TextBoxValue,
                                    "Base":costArray[i].Base,
                                    "FTECost":costArray[i].FTECost,
                                    "FTEHours":costArray[i].FTEHours
                                }
                            }
                        ]
                        };
                        costs.push(cost);
                    }

                    var returnArray = returnThis.concat(costs);

                    return returnArray;
                }
            }
        }
    });