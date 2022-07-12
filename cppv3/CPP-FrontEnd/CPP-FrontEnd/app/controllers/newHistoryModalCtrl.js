// New History (Log) Modal Controller

angular.module('cpp.controllers')
    .controller('newHistoryModalCtrl',
    ['$scope','$rootScope','$uibModal','$http',
        function($scope,$rootScope,$uibModal,$http){
            console.log("Creating a new history log");
            console.log($scope.params);

            // get the parent asset
            $scope.asset = $scope.params.asset;
            $scope.isNew = $scope.params.isNew;

            // new log to be added
            $scope.newLog = {
                Operation: 1,
                ID: 0,
                Description: "", //user input
                Note: "", //user input
                StartDate: "", //user input
                EndDate: "", //user input
                Cost: 0, //user input
                Asset_Component_ID: $scope.asset.ID,
                AssetID: $scope.asset.ID
            };

            // save
            $scope.save = function() {
                // if no changes, do not allow to save empty comp
                if(!changes()){
                    alert("no changes have been detected");
                    return;
                }

                //send new log to other controller or to backend to add
                if($scope.isNew) {
                    $scope.params.confirm = "Success";
                    $scope.params.newItem = $scope.newLog;
                }
                else{

                    var url = serviceBasePath+"response/";
                    url += ($scope.asset.isComponent) ? "ComponentHistory/" : "AssetHistory/";
                    $http({
                        url: url,
                        method: "POST",
                        data: $scope.newLog,
                        headers: {'Content-Type':'application/json'}
                    }).then(function(response){
                        console.log(response);
                    });

                    $scope.params.newItem = $scope.newLog;
                }

                $scope.exit();
            };

            // cancel
            $scope.cancel = function() {
                //if there are changes, make user aware
                if(changes()){
                    var scope = $rootScope.$new();
                    scope.params = {confirm: ""};
                    $rootScope.modalInstance = $uibModal.open({
                        scope: scope,
                        templateUrl: 'app/views/Modal/exit_confirmation_modal.html',
                        controller: 'exitConfirmation',
                        size: 'md',
                        backdrop: true
                    });
                    $rootScope.modalInstance.result.then(function() {
                        console.log(scope.params.confirm);
                        // exit without saving
                        if (scope.params.confirm === "exit") { $scope.exit(); }
                        // save then exit
                        else if (scope.params.confirm === "save") { $scope.save(); }
                        // go back
                        else {  }
                    });
                }
                // if no changes were made, then exit
                else {
                    $scope.exit();
                }
            };

            // return true if any changes have been made
            var changes = function(){
                return($scope.newLog.Description != ""
                        && $scope.newLog.StartDate != ""
                        && $scope.newLog.EndDate != "")
            };

            // close the modal
            $scope.exit = function(){ $scope.$close(); };

            $scope.test = function(){
                console.log($scope.newLog);
            }
        }]);
