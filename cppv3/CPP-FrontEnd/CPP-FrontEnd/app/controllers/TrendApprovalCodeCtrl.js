//(function () {
//    'use strict';

//    angular
//        .module('app')
//        .controller('TrendApprovalCodeCtrl', TrendApprovalCodeCtrl);

//    TrendApprovalCodeCtrl.$inject = ['$location'];

//    function TrendApprovalCodeCtrl($location) {
//        /* jshint validthis:true */
//        var vm = this;
//        vm.title = 'TrendApprovalCodeCtrl';

//        activate();

//        function activate() { }
//    }
//})();

angular.module('cpp.controllers').
    controller('TrendApprovalCodeCtrl', ['$scope', '$timeout', '$interval', '$uibModal', '$rootScope', '$http', '$uibModalInstance',
        'Trend', 'TrendId', 'TrendApprovalTrackLog', '$state', 'TrendStatus',
        function ($scope, $timeout, $interval, $uibModal, $rootScope, $http, $uibModalInstance, Trend, TrendId, TrendApprovalTrackLog, $state, TrendStatus) {
            $("#TrendApprovalspin").show();
            $("#TrendResend").attr('disabled', 'disabled');
            $("#TrendClose").attr('disabled', 'disabled');
            $("#TrendSubmit").attr('disabled', 'disabled');
            // document.getElementById("TrendTimerCount").innerHTML = "";
            $('#TrendTimerCount').html('');
            $('#SpanExpireMessgae').html('');
            var xTimerInterval = null;
            var sqlDateFormat = "YYYY-MM-DD";
            // clearInterval(xTimer);
            TrendApprovalTrackLog.getAllApprovalTimer().get({
                ProjectID: $scope.params.ProjectID, TrendNumber: $scope.params.TrendNumber, UserID: $scope.params.UserID, CurrentThreshold: parseInt($scope.params.CurrentThreshold)}, function success(response) {
                console.log(response.result);
                $scope.TrendApprovalTrackLog = response.result;
               // alert($scope.params.UserID);
                // alert($scope.TrendApprovalTrackLog[0].ExpiredDate.toString().split('T')[1]);
                // var ExpireyDate = $scope.trends.ExpireyDate;
               // document.getElementById("SpanTxtMessage").innerHTML = "To Approve this Trend and 6 Digit Code is been Emailed to your registered EmailId: " + $scope.TrendApprovalTrackLog[0].UserEmailid + ".";
                    document.getElementById("SpanTxtMessage").innerHTML = "Enter the 6 Digit verification code send to your EmailId: " + $scope.TrendApprovalTrackLog[0].UserEmailid + ".";
                var OnlyDate = $scope.TrendApprovalTrackLog[0].ExpiredDate.toString().split('T')[0];
                var OnlyTime = $scope.TrendApprovalTrackLog[0].ExpiredDate.toString().split('T')[1];
                    var d = new Date(OnlyDate);
                    if (d.isDstObserved()) {
                        var formatedDate = moment(OnlyDate).add(1, 'days').format(sqlDateFormat);
                        d = new Date(formatedDate);
                    }
                var ye = new Intl.DateTimeFormat('en', { year: 'numeric' }).format(d)
                var mo = new Intl.DateTimeFormat('en', { month: 'short' }).format(d)
                var da = new Intl.DateTimeFormat('en', { day: '2-digit' }).format(d)
                //alert(mo + " " + da + ", " + ye);
                var ExpiredDateFormat = mo + " " + da + ", " + ye + " " + OnlyTime;
                var countDownDate = new Date(ExpiredDateFormat).getTime();

                // Get today's date and time
                var now = new Date().getTime();
                // Update the count down every 1 second
                if (countDownDate > now) {
                    $("#TrendApprovalspin").hide();
                    $("#TrendResend").removeAttr('disabled');
                    $("#TrendClose").removeAttr('disabled');
                    $("#TrendSubmit").removeAttr('disabled');
                    $("#hfUniqueCode").val($scope.TrendApprovalTrackLog[0].UniqueCode);
                    $('#TrendTimerCount').html('');
                    $("#SpanTxtRecreatedMessage").html('');
                    if ($scope.TrendApprovalTrackLog[0].IsRecreated) {
                        $("#SpanTxtRecreatedMessage").html('You have recreated the code. Kindly check latest Email.');
                    }
                    xTimerInterval = $interval(function () {
                        // Find the distance between now and the count down date
                        var CountExpiryDate = new Date(ExpiredDateFormat).getTime();
                        var CountNowDte = new Date().getTime();
                        var distance = CountExpiryDate - CountNowDte;

                        // Time calculations for days, hours, minutes and seconds
                        // var days = Math.floor(distance / (1000 * 60 * 60 * 24));
                        //  var hours = Math.floor((distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
                        var minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
                        var seconds = Math.floor((distance % (1000 * 60)) / 1000);
                        $('#TrendTimerCount').html(minutes + 'min ' + seconds + 'sec ');

                        // If the count down is over, write some text 
                        if (distance < 0) {
                            $interval.cancel(xTimerInterval);
                           // $('#TrendTimerCount').html('CODE EXPIRED! To regenrate the code Kindly approve the trend.');
                            $('#TrendTimerCount').html('00:00');
                            $('#SpanExpireMessgae').html('The Code has expired!! Click on Approval Button to generate a new code.');
                            $("#hfUniqueCode").val("");
                            $("#TrendResend").attr('disabled', 'disabled');
                            $("#TrendSubmit").attr('disabled', 'disabled');
                            $("#txtUniqueCode").attr('disabled', 'disabled');
                           // setTimeout($scope.$close(), 2000);
                            // $scope.$close();
                        }
                    }, 1000);
                }


            }, function error(response) {
                // console.log(response);
            });
            Date.prototype.stdTimezoneOffset = function () {
                var jan = new Date(this.getFullYear(), 0, 1);
                var jul = new Date(this.getFullYear(), 6, 1);
                return Math.max(jan.getTimezoneOffset(), jul.getTimezoneOffset());
            }

            Date.prototype.isDstObserved = function () {
                return this.getTimezoneOffset() < this.stdTimezoneOffset();
            }
            $scope.CodeResend = function () {
                if ($("#hfUniqueCode").val() == "") {
                    dhtmlx.alert('Code Expired.');
                    return false;
                }
                var UniqueCode = $("#hfUniqueCode").val();
                TrendApprovalTrackLog.ResendUnqiuecode().get({
                    ProjectID: $scope.params.ProjectID, TrendId: $scope.params.TrendID, UserID: $scope.params.UserID, UniqueCode: UniqueCode
                }, function success(response) {
                    dhtmlx.alert('Approval code resent to  EmailId: ' + $scope.TrendApprovalTrackLog[0].UserEmailid);
                    }, function error(response) {
                        // console.log(response);
                    });
                //dhtmlx.alert('Unqiue code Resend to your register EmailId.');
            }
            $scope.goBack = function () {
                // document.getElementById("TrendTimerCount").innerHTML = "";
                $('#TrendTimerCount').html('');
                $interval.cancel(xTimerInterval);
                $scope.$close();
            }
            $scope.save = function () {
                if ($("#txtUniqueCode").val().length != 6) {
                    dhtmlx.alert('Enter valid 6 digit code.'); // Jignesh-02-03-2021
                    return false;
                }
                if ($("#txtUniqueCode").val().trim() !== $("#hfUniqueCode").val().trim()) {
                    dhtmlx.alert('Enter valid 6 digit code.'); // Jignesh-02-03-2021
                    return false;
                }
                $("#TrendApprovalspin").show();
                var uniqueCode = $("#txtUniqueCode").val();
                TrendApprovalTrackLog.getApprovedCodeSubmitted().get({
                    ProjectID: $scope.params.ProjectID, TrendNumber: $scope.params.TrendNumber,
                    UserID: $scope.params.UserID, UniqueCode: uniqueCode, CurrentThreshold: parseInt($scope.params.CurrentThreshold)
                }, function success(response) {
                    console.log(response.result);
                    $scope.TrendApprovalTrackLog = response.result;
                    if ($scope.TrendApprovalTrackLog[0].IsApproved) {
                        dhtmlx.alert('Already been Approved by: ' + $scope.TrendApprovalTrackLog[0].UserID);
                        return false;
                    }

                    $scope.approve();
                    //                    // Working Commented by pritesh 25 aug 2020 Start
                    //                    var trendToBeApproved = {
                    //                        "Operation": $scope.params.Operation,
                    //                        "ProjectID": $scope.params.ProjectID,
                    //                        "TrendNumber": $scope.params.TrendNumber,
                    //                        "TrendDescription": $scope.params.TrendDescription,
                    //                        "TrendJustification": $scope.params.TrendJustification,
                    //                        "TrendImpact": $scope.params.TrendImpact,
                    //                        "TrendImpactSchedule": $scope.params.TrendImpactSchedule,
                    //                        "TrendImpactCostSchedule": $scope.params.TrendImpactCostSchedule,
                    //                        "ApprovalFrom": $scope.params.ApprovalFrom,
                    //                        "PostTrendStartDate": $scope.params.PostTrendStartDate,
                    //                        "PostTrendEndDate": $scope.params.PostTrendEndDate,
                    //                        "TrendStatusID": $scope.params.TrendStatusID, //submit for approval
                    //                        "ApprovalDate": $scope.params.ApprovalDate,
                    //                        "CreatedOn": $scope.params.CreatedOn,
                    //                        "approvalFlag": true,
                    //                        "UserID": $scope.params.UserID,
                    //                        "TrendID": $scope.params.TrendID,
                    //                        "TrendCost": $scope.params.TrendCost
                    //                        // "TrendCost": project.totalCost
                    //                    }

                    //                    Trend.persist().save(trendToBeApproved, function (response) {
                    //                        //dhtmlx.alert(response.result);
                    //                        if (response.result) {
                    //                            dhtmlx.alert(response.result);

                    //                            //approval progress bar
                    //                            TrendId.get({
                    //                                "trendId": $scope.params.delayedDataTrendId,
                    //                                "projectId": $scope.params.delayedDataProjectId
                    //                            }, function (response) {
                    //                                console.log(response);
                    //                                var trend = response.result;
                    //                                applyApprovalProgressBar(trend);
                   
                    //                            });

                    //                            $("#submitForApproval").prop('disabled', true);    //luan here
                    //                            //TrendStatus.setStatus(status + "Approved");  //Manasi
                    //                            TrendStatus.setStatus("Approved");
                    //                            $scope.isTrendApproved = true;
                    //                            //$scope.scheduleGanttInstance.callEvent('customSelect', [$scope.selectedActivity.id]);
                    //                            //$("#approveBtn").show();    //luan here
                    //                        } else {
                    //                            dhtmlx.alert("Failed to submit.");
                    //                        }
                    //                        $("#TrendApprovalspin").hide();
                    //                        $state.reload();
                    //                        $('#TrendTimerCount').html('');
                    //                        $interval.cancel(xTimerInterval);
                    //                        $scope.$close();
                    //                    });
                    //                       // Working Commented by pritesh 25 aug 2020 End
                }, function error(response) {
                    // console.log(response);
                });








            }

            $scope.approve = function () {

                //if cost has not saved
                var totalFundSource = 0.0;
                var projectTotal = $scope.params.projectTotal;
               

                var trendToBeApproved = {
                    "Operation": "2",
                    "ProjectID": $scope.params.ProjectID,
                    "TrendNumber": $scope.params.TrendNumberNew,
                    //"TrendCost": $scope.params.projectTotal,
                    "TrendCost": $scope.params.TrendCost, // Swapnil 03-09-2020
                    "CurrentThreshold": $scope.params.CurrentThreshold,
                    "TrendDescription": $scope.params.TrendDescription,
                    "TrendJustification": $scope.params.TrendJustification,
                    "TrendImpact": $scope.params.TrendImpact,
                    "TrendImpactSchedule": $scope.params.TrendImpactSchedule,   //Manasi 13-07-2020
                    "TrendImpactCostSchedule": $scope.params.TrendImpactCostSchedule,  //Manasi 13-07-2020
                    "ApprovalFrom": $scope.params.ApprovalFrom,
                    "PostTrendStartDate": $scope.params.PostTrendStartDate,
                    "PostTrendEndDate": $scope.params.PostTrendEndDate,
                    "TrendStatusID": 3, //Backend decides if it will be 1 or not.
                    "ApprovalDate": $scope.params.ApprovalDate,
                    "CreatedOn": $scope.params.CreatedOn,
                    "approvalFlag": true,
                    "UserID": $scope.params.UserID,
                    "TrendID": $scope.params.TrendID
                }
                Trend.persist().save(trendToBeApproved, function (response) {
                    if (response.result) {
                        dhtmlx.alert(response.result);

                        //approval progress bar
                        TrendId.get({
                            "trendId": $scope.params.delayedDataTrendId,  //delayedData[3],
                            "projectId": $scope.params.delayedDataProjectId // delayedData[2].result[0].ProjectID
                        }, function (response) {
                            var trend = response.result;
                            $scope.trend = response.result; 
                          //  applyApprovalProgressBar(trend);
                            });
                        $("#submitForApproval").prop('disabled', true);   
                        $("#submitForApproval").html('Approved');
                        
                     //  TrendStatus.setStatus("Approved");
                     //   $scope.isTrendApproved = true;
                    }
                    $("#TrendApprovalspin").hide();
                                            $state.reload();
                                            $('#TrendTimerCount').html('');
                                            $interval.cancel(xTimerInterval);
                                            $scope.$close();
                });



                ////Hide fund allocation
                //    } else {
                //        dhtmlx.alert({
                //            text: "Insufficient fund. Please assign fund sources for the trend before approve it!",
                //            width: "400px"
                //        });
                //    }
                //});
                ////Hide fund allocation
            }
        }
    ]);
