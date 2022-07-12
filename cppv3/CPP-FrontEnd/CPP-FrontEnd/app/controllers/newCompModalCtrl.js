// New Component Controller

angular.module('cpp.controllers')
    .controller('newCompModalCtrl',
    ['$scope','$rootScope','$uibModal','$http',
        function($scope,$rootScope,$uibModal,$http){
            console.log("Create a new component");
            console.log($scope.params);

            // get parameters
            $scope.asset = $scope.params.asset;
            $scope.isNew = $scope.params.isNew;
            $scope.params.newItem = $scope.newComponent;

            // new component
            $scope.newComponent = {
                Operation: 1,
                assetComponentHistories: [], //user input
                AcquisitionDate: "", //user input
                AssetID: $scope.asset.ID,
                Cost: "", //user input
                ID: 0, //modify in backend
                Name: "", //user input
                Note: "", //user input
                Tag: "" //user input
            };


            // data list to populate grids
            $scope.histData = $scope.newComponent.assetComponentHistories;

            // set views
            $scope.hlistempty = $scope.histData.length == 0;

            // grid definitions... short for details page, full for view all page
            var histDef = [
                {
                    field: 'info',
                    displayName: '',
                    enableCellSelection: true,
                    width: 30,
                    cellTemplate: '<div class="icon-info" ng-dblclick="viewHistDetails()"></div>' //'<div style="text-align:center" class="ngCellText ui-grid-cell-contents ng-binding ng-scope"><img src="{{grid.appScope.getTemplate()}}" alt="" height="40" width=""35"/></br>{{COL_FIELD}}</div>'
                },
                {
                    field:'Description',
                    displayName: 'Description'
                },
                {
                    field:'Cost',
                    displayName: 'Cost',
                    cellFilter: 'currency'
                },
                {
                    field: 'edit',
                    displayName: '',
                    enableCellSelection: true,
                    width: 30,
                    cellTemplate: '<div class="icon-edit" ng-dblclick="updateHist()"></div>'
                },
                {
                    field: 'delete',
                    displayName: '',
                    enableCellSelection: true,
                    width: 30,
                    cellTemplate: '<div class="icon-delete" ng-dblclick="removeHist()"></div>'
                }
            ];
            var fullGrid = [
                {
                    field: 'info',
                    displayName: '',
                    enableCellSelection: true,
                    width: 30,
                    cellTemplate: '<div class="icon-info" ng-dblclick="viewHistDetails()"></div>'
                },
                {
                    field:'Description',
                    displayName: 'Description'
                },
                {
                    field:'StartDate',
                    displayName: 'Start',
                    cellFilter: 'date:\'MM/dd/yyyy\''

                },
                {
                    field:'EndDate',
                    displayName: 'End',
                    cellFilter: 'date:\'MM/dd/yyyy\''
                },
                {
                    field:'Cost',
                    displayName: 'Cost',
                    cellFilter: 'currency'
                },
                {
                    field: 'Note',
                    displayeName: 'Note'
                },
                {
                    field: 'edit',
                    displayName: '',
                    enableCellSelection: true,
                    width: 30,
                    cellTemplate: '<div class="icon-edit" ng-dblclick="updateHist()"></div>'
                },
                {
                    field: 'delete',
                    displayName: '',
                    enableCellSelection: true,
                    width: 30,
                    cellTemplate: '<div class="icon-delete" ng-dblclick="removeHist()"></div>'
                }
            ];

            // get selected items
            $scope.myHSelections = [];

            // grid
            $scope.histGrid = {
                data: 'histData',
                rowHeight: 35,
                columnDefs: histDef,
                enableRowSelection: true,
                enableCellSelection: false,
                enableCellEditOnFocus: false,
                enableCellEdit: false,
                multiSelect: false,
                selectedItems: $scope.myHSelections,
                afterSelectionChange: function(rowItem) {
                    $scope.myHSelections = rowItem;
                }
            };




            // history functions
            $scope.addHistory = function() {
                // print action
                console.log("Add a new history log");

                // variables
                var scope = $rootScope.$new();

                // set scope parameters
                scope.params = {
                    asset: {Name: "New",asset: $scope.asset},
                    confirm: "",
                    newItem: {},
                    isNew: true
                };

                // open modal
                $rootScope.modalInstance = $uibModal.open({
                    backdrop: 'static',
                    keyboard: false,
                    scope: scope,
                    templateUrl: 'app/views/Modal/new_history_log_modal.html',
                    size: 'md',
                    controller: "newHistoryModalCtrl"
                });
                $rootScope.modalInstance.result.then(function(){
                    console.log(scope.params);
                    if(scope.params.confirm === "Success"){
                        $scope.histData.push(scope.params.newItem);
                        $scope.hlistempty = $scope.histData.length == 0;
                    }
                    else{
                        alert("Error adding new log");
                    }
                })
            };
            $scope.viewHistDetails = function(){
                console.log("View history details");
                console.log($scope.myHSelections);
                var scope = $rootScope.$new();
                scope.params = {
                    dt: $scope.myHSelections,
                    asset: {Name: "New"}
                };
                $rootScope.modalInstance = $uibModal.open({
                    backdrop: 'static',
                    keyboard: false,
                    scope: scope,
                    templateUrl: 'app/views/Modal/log_detail_modal.html',
                    size: 'md',
                    controller: "LogDetailModalCtrl"
                });
            };
            $scope.viewAllHistory = function() {
                console.log("View all history logs");

                var scope = $rootScope.$new();
                scope.params = {
                    list: $scope.histData,
                    col: fullGrid,
                    nm: "History Log",
                    asset: {Name: "New"},
                    add: "Log",
                    isNew: true
                };
                $rootScope.modalInstance = $uibModal.open({
                    backdrop: 'static',
                    keyboard: false,
                    scope: scope,
                    templateUrl: 'app/views/Modal/va_modal.html',
                    size: 'lg',
                    controller: 'VAModalCtrl'
                });
            };
            $scope.updateHist = function() {
                console.log("Edit history log");

            };
            $scope.removeHist = function() {
                console.log("Remove history log");
                console.log($scope.myHSelections.entity);
                $scope.histData.splice($scope.histData.indexOf($scope.myHSelections.entity),1);
                $scope.hlistempty = $scope.histData.length == 0;
            };

            // save
            $scope.save = function() {
                // if no changes, do not allow to save empty comp
                if(!changes()){
                    alert("no changes have been detected");
                    return;
                }
                console.log($scope.isNew);
                //send new component to other controller or to backend to add
                if($scope.isNew) {
                    $scope.params.confirm = "Success";
                    $scope.params.newItem = $scope.newComponent;
                }
                else{
                    var url = serviceBasePath+"response/Component";
                    $http({
                        url: url,
                        method: "POST",
                        data: $scope.newComponent,
                        headers: {'Content-Type': 'application/json'}
                    }).then(function(response){
                        if(response.data.result==="Success"){
                            $scope.params.confirm = "Success";
                        }
                    });
                }
                $scope.exit();
                console.debug("NEW COMPONENT: ",$scope.newComponent);

            };

            // cancel
            $scope.cancel = function(){
                // if there are changes, make user aware
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

            // returns true if there are any changes
            var changes = function(){
                return(!$scope.hlistempty
                    || $scope.newComponent.Name != ""
                    || $scope.newComponent.Tag != ""
                    || $scope.newComponent.AcquisitionDate != ""
                )
            };

            // close the modal
            $scope.exit = function() { $scope.$close(); };

            $scope.test = function(){
                console.log($scope.newComponent);
            }

        }]);
