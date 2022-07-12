angular.module('cpp.controllers').
    //Confirmation Controller
    controller('PdfModalCtrl',['$scope','$uibModal','$rootScope','$sce','$http','usSpinnerService',
        function($scope,$uibModal,$rootScope,$sce,$http,usSpinnerService){
            var ProjectId = $scope.params.ProjectID;
            var TrendNumber = $scope.params.TrendNumber;
            var uss = $scope.params.ss;
           $scope.showSpinner2 = true;
           usSpinnerService.spin('spinner-6');
           $('.modal-dialog').draggable({
               handle: ".modal-header"
           });

            var reportType = $scope.params.ReportType;
            if (reportType === 'FTERT') {
                $http.get(serviceBasePath + "response/report/" + ProjectId + "/" + reportType)
                    .then(function success (response) {
                        console.log(response);
                        
                        $scope.pdfDiv = false;
                        $scope.chartDiv = true;
                        $scope.currentUrl = $sce.trustAsResourceUrl(response.data.result);
                        usSpinnerService.stop('spinner-6');
                        }, function error(response) {
                            dhtmlx.alert("Unable to render report!");
                            usSpinnerService.stop('spinner-6');
                        });
            }
            else {
                if(reportType=="Project")
                    reportType = "ForecastTrend";
                $http.get(serviceBasePath + "response/report/" + ProjectId + "/" + TrendNumber
                    + "/" + reportType, { responseType: 'arraybuffer' }).then(function success(response) {
                        console.log(response);

                        var blob = new Blob([response.data], {
                            type: 'application/pdf'
                        });

                        console.log(blob);

                        $scope.pdfDiv = true;
                        $scope.chartDiv = false;
                        var url = window.URL.createObjectURL(blob);
                        // var a = document.createElement("a");

                        //window.URL.revokeObjectURL(url);
                        $scope.content = $sce.trustAsResourceUrl(url);
                    }, function error(response) {
                        dhtmlx.alert("Unable to render report!");
                    });

               
            }
            //
            //$scope.ganttBg = '{background-color:red}';
            console.log($scope.params);
            $scope.goBack = function () {
                //    $scope.$close();
                $scope.params.confirm = 'no';

            };
            if($scope.params.message){
                $scope.message = $scope.params.message;
            }else{
                $scope.message = "Are you sure you want to delete?";
            }

            $("#confirmation").on('show.bs.modal',function(event){
            });
            $scope.delete = function () {

                $scope.$close();
                $scope.params.confirm = 'yes';

            };

        }]);