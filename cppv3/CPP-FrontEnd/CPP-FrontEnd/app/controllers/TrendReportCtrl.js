angular.module('cpp.controllers').
    //TrendReportController
controller('TrendReportCtrl',['$scope','$uibModal','$rootScope',
    function( $scope,$uibModal,$rootScope){

        console.log($scope.params);

        // capture parameters
        var ProjectID = $scope.params.ProjectID;
        var TrendNumber = $scope.params.TrendNumber;
        $scope.report = "";

        // select what kind of report to generate
        $scope.selectReport = function(){
            $scope.report = $("#reportSelect").val();
        };

        // exit
        $scope.goBack = function(){
            $scope.$close();
            $scope.params.confirm = 'no';
        };

        // generate report
        $scope.openReportModal = function(){
            console.log("openreportmodal");
            if($scope.report == "" || $scope.report == null){
                dhtmlx.alert({type:"position",text:"Please select a report",width:"300px",position:"top"});
                return;
            }
            var scope = $scope.$new();
            var newOrEdit = false;
            alert($scope.report);
            scope.params = {
                newOrEdit : newOrEdit,
                ReportType : $scope.report,
                ProjectID : ProjectID,
                TrendNumber : TrendNumber
            };
            $rootScope.modalInstance = $uibModal.open({
                backdrop : 'static',
                keyboard : false,
                scope : scope,
                templateUrl : "app/views/Modal/PdfModal.html",
                size : "lg",
                controller : "PdfModalCtrl"
            });
            $rootScope.modalInstance.result.then(function(response){
               // $scope.goBack();
            });
        }

    }]);