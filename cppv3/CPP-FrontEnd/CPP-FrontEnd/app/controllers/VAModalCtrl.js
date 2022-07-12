// View All Modal (Component/History Logs) Controller

angular.module('cpp.controllers')
    .controller('VAModalCtrl',
    ['$state','$scope','$rootScope','$uibModal','$http',
        function($state,$scope,$rootScope,$uibModal,$http){
            console.log($scope.params);

            // set data
            $scope.isNew = $scope.params.isNew;
            $scope.list = $scope.params.list;
            $scope.asset = $scope.params.asset;
            $scope.nm = $scope.params.nm;
            $scope.add = $scope.params.add;
            $scope.assetCollection = $scope.params.assetCollection;
            $scope.logs = $scope.add == "Log";

            $scope.hideAdd = (!$scope.isNew && $scope.add == "Log");

            var list = $scope.list;
            $scope.mySelections = [];

            // set grid
            $scope.grid = {
                data: 'list',
                enableRowSelection: true,
                enableCellSelection: false,
                selectedComps: $scope.mySelections,
                enableCellEditOnFocus: false,
                enableCellEdit: false,
                multiSelect: false,
                rowHeight: 35,
                afterSelectionChange: function (rowItem){
                    $scope.mySelections = rowItem;
                },
                columnDefs: $scope.params.col
            };

            // add

            $scope.addNew = function(){
                var tUrl,ctrl, sz;
                var scope = $rootScope.$new();

                if($scope.add == "Component"){
                    tUrl = 'app/views/Modal/new_comp_modal.html';
                    ctrl = "newCompModalCtrl";
                    sz = 'lg';

                    scope.params = {
                        asset: $scope.asset,
                        isNew: $scope.isNew
                    };
                }
                else if($scope.add == "Log"){
                    tUrl = 'app/views/Modal/new_history_log_modal.html';
                    ctrl = "newHistoryModalCtrl";
                    sz = 'md';

                    // set scope parameters
                    scope.params = {
                        asset: {Name: "New",asset: $scope.asset},
                        confirm: "",
                        newItem: {},
                        isNew: $scope.isNew
                    };
                }
                else {
                    alert("Some error has occurred.");
                    return;
                }



                // open modal
                $rootScope.modalInstance = $uibModal.open({
                    backdrop: 'static',
                    keyboard: false,
                    scope: scope,
                    templateUrl: tUrl,
                    size: sz,
                    controller: ctrl
                });
                $rootScope.modalInstance.result.then(function(){
                    console.log(scope.params);
                    if(scope.params.confirm === "Success"){
                        list.push(scope.params.newItem);
                        $scope.list = list;
                    }
                    else if(scope.params.confirm === "sent") {
                        console.log("New item sent to backend");
                    }
                    else {
                        alert("Error: An error occurred.");
                    }
                })
            };

            // delete
            $scope.removeHist = function() { remove("hist",$scope.mySelections.entity); };
            $scope.removeComp = function() { remove("comp",$scope.mySelections.entity); };
            var remove = function(type,item){
                var s = "Delete ";
                s += (type=="comp") ? "component" : "history log";
                console.log(s);

                if($scope.isNew){
                    $scope.list.splice($scope.list.indexOf(item),1);
                }
                else {
                    var url = serviceBasePath+"response/";
                    url += (type=="comp") ? "Component" : "Asset";
                    url += ($scope.add == "Component") ? "/" : "History/";
                    console.log(url);
                    $http({
                        url: url,
                        method: "POST",
                        data: {
                            "Operation":3,
                            "ID":$scope.mySelections.entity.ID
                        },
                        headers: {'Content-Type':'application/json'}
                    }).then(function(response){
                        if(response.data.result == "Success"){
                            $scope.list.splice($scope.list.indexOf(item),1);
                            $scope.assetCollection.splice($scope.assetCollection.indexOf(item),1);
                        }
                        else {
                            alert("Error: An error occurred.");
                        }
                    })

                }
            };

            // view details
            $scope.viewHistDetails = function (){ viewDetail("hist"); };
            $scope.viewCompDetails = function() { viewDetail("comp"); };
            var viewDetail = function(type){
                var s = "View ";

                // variables
                var tUrl, ctrl, p1;
                var scope = $rootScope.$new();

                // set variables according to type
                if(type == "comp"){
                    s+= "component";
                    tUrl = 'app/views/Modal/comp_detail_modal.html';
                    ctrl = "CompDetailModalCtrl";
                    p1 = $scope.mySelections;
                }
                else if(type == "hist"){
                    s+= "history log";
                    tUrl = 'app/views/Modal/log_detail_modal.html';
                    ctrl = "LogDetailModalCtrl";
                    p1 = $scope.mySelections;
                }
                else {
                    alert("Err: Detail type \" " + type + " \" does not exist.");
                    return;
                }

                s+= " details";
                console.log(s);

                // set scope parameters
                scope.params = {
                    dt : p1,
                    asset : $scope.asset,
                    isNew: $scope.isNew
                };

                // open modal
                $rootScope.modalInstance = $uibModal.open({
                    backdrop: 'static',
                    keyboard: false,
                    scope: scope,
                    templateUrl: tUrl,
                    size: 'md',
                    controller: ctrl
                });
            };

            $scope.updateComp = function() { update("comp"); };
            var update = function(type){
                var s = "Update ",
                    scope = $rootScope.$new(),
                    tUrl = 'app/views/Modal/editAssetModal.html',
                    ctrl = 'editAssetModalCtrl';
                scope.params = {
                    isComp: true,
                    isNew: $scope.isNew,
                    asset: $scope.mySelections.entity,
                    parentCollection: $scope.parentCollection
                };
                if(type=="comp"){
                    s+="Component";
                }
                else{
                    alert("An error has occurred.");
                    return;
                }
                console.log(s);

                //open modal
                $rootScope.modalInstance = $uibModal.open({
                    backdrop: 'static',
                    keyboard: false,
                    scope: scope,
                    templateUrl: tUrl,
                    size: 'md',
                    controller: ctrl
                });
                $rootScope.modalInstance.result.then(function(){
                    console.log(scope.params);
                });
            };

            // close the modal
            $scope.close = function(){ $scope.$close(); };

        }]);
