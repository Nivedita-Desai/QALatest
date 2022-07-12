angular.module('cpp.controllers').
    //Confirmation Controller
    controller('ReportDetailModalCtrl',['$scope','$uibModal','$rootScope','$sce','$http','usSpinnerService','Organization','Program','ProgramElement','Project','Trend','$timeout',
        function($scope,$uibModal,$rootScope,$sce,$http,usSpinnerService,Organization,Program,ProgramElement,Project,Trend,$timeout){

            //get/set parameters
            var reportType = $scope.params.ReportType;
            $scope.reportLabel = $scope.params.reportLabel;
            $scope.ProList = $scope.params.ProList;
            $scope.sn = $scope.params.SelectedNode;
            $scope.trend = 0;
            $scope.param1 = "";
        

            // filters
            $scope.filtPgm = "";
            $scope.filtPro = "";
            $scope.filtElm = "";
            $scope.filtPjt = "";
            $scope.filtTrd = "";

            //show correct prompt
            $scope.pgmrpt = $scope.reportLabel == "Program";
            $scope.pjtrpt = $scope.reportLabel == "Project";
            $scope.trdrpt = $scope.reportLabel == "Trend";

            //exit
            $scope.goBack = function(){
                $scope.$close();
                $scope.params.confirm = 'no';
            };

            //program report
            $scope.selectPro = function(){
                $scope.param1 = $("#proSelect").val();
            };

            //project report
            $scope.selectPgm = function(pgm){
                if(pgm != ""){
                    $scope.filtPgm = pgm;
                    ProgramElement.lookup().get({ProgramID:pgm},function(programElementData){
                        $scope.ElmList = programElementData.result;
                        if(!$scope.sn)
                            setNext("pgm");
                    });
                    $scope.PjtList = [];
                    $scope.TrdList = [];
                    $scope.filtPjt = "";
                    $scope.filtTrd = "";
                    $scope.param1 = "";
                } else {
                    $scope.ElmList = [];
                    $scope.PjtList = [];
                    $scope.TrdList = [];
                    $scope.filtElm = "";
                    $scope.filtPjt = "";
                    $scope.filtTrd = "";
                    $scope.param1 = "";
                }
            };
            $scope.selectElm = function(elm){
                var pgm = $("#pgmSelect").val();
                if(elm != ""){
                    $scope.filtElm = elm;
                    Project.lookup().get({ProgramID:pgm, ProgramElementID:elm},function(projectData){
                        $scope.PjtList = projectData.result;
                        if(!$scope.sn)
                            setNext("elm");
                    });
                    $scope.TrdList = [];
                    $scope.filtPjt = "";
                    $scope.filtTrd = "";
                    $scope.param1 = "";
                } else {
                    $scope.PjtList = [];
                    $scope.TrdList = [];
                    $scope.filtPjt = "";
                    $scope.filtTrd = "";
                    $scope.param1 = "";
                }
            };
            $scope.selectPjt = function(pjt){
                if($scope.trdrpt){
                    var pgm = $("#pgmSelect").val();
                    var elm = $("#elmSelect").val();
                    if(pjt != ""){
                        $scope.filtPjt = pjt;
                        $http.get(serviceBasePath+"Request/Trend/"+pgm+"/"+elm+"/"+pjt)
                            .then(function successfulCallback(result){
                                $scope.TrdList = result.data.result;
                            });
                        $scope.filtTrd = "";
                        $scope.param1 = pjt;
                    } else {
                        $scope.TrdList = [];
                        $scope.filtTrd = "";
                        $scope.param1 = "";
                    }
                }
                else {
                    $scope.param1 = pjt;
                }
            };

            //trend report
            $scope.selectTrd = function(trd){
                $scope.trend = trd;
            };

            //generate report
            $scope.openReportModal = function(){
                if($scope.param1 == ""){
                    dhtmlx.alert(
                        {
                            type:"position",
                            text:"Please select a Program or Project",
                            width:"300px",
                            position:"top"
                        });
                    return;
                }
                var scope = $scope.$new();
                var newOrEdit = false;
                scope.params = {
                    newOrEdit: newOrEdit,
                    ReportType : reportType,
                    ProjectID : $scope.param1,
                    TrendNumber: $scope.trend
                };
                $rootScope.modalInstance = $uibModal.open({
                    backdrop: 'static',
                    keyboard: false,
                    scope: scope,
                    templateUrl: "app/views/Modal/PdfModal.html",
                    size: "lg",
                    controller: "PdfModalCtrl"
                });
                $rootScope.modalInstance.result.then(function(response){
                    console.log(response);
                    $scope.goBack();
                });
            };

            // if user selected a node, traverse from the root to the leaf
            $timeout(function(){
                if($scope.sn){
                    console.log($scope.sn);
                    switch($scope.sn.depth){
                        case 0:
                            $scope.sn = null;
                            break;
                        case 1:
                            $scope.selectPgm($scope.sn.ProgramID);
                            $scope.sn = null;
                            break;
                        case 2:
                            $scope.selectPgm($scope.sn.ProgramID);
                            $timeout(function(){
                                $scope.selectElm($scope.sn.ProgramElementID);
                                $scope.sn = null;
                            },50);
                            break;
                        case 3:
                            $scope.selectPgm($scope.sn.ProgramID);
                            $timeout(function(){
                                $scope.selectElm($scope.sn.ProgramElementID);
                            },50);
                            $timeout(function(){
                                $scope.selectPjt($scope.sn.ProjectID);
                                $scope.sn = null;
                            },100);
                            break;
                    }
                }
                else if($scope.ProList.length==1){
                    $scope.selectPgm($scope.ProList[0].ProgramID);
                }
            },100);

            // set next item in selectors if the next item has no siblings
            var setNext = function(pl){
                if(pl=="pgm" && $scope.ElmList.length==1){
                    $timeout(function(){
                        $scope.selectElm($scope.ElmList[0].ProgramElementID);
                    },100);
                }
                else if(pl=="elm" && $scope.PjtList.length==1){
                    $timeout(function(){
                        $scope.selectPjt($scope.PjtList[0].ProjectID);
                    },100);
                }
            }

        }]);
