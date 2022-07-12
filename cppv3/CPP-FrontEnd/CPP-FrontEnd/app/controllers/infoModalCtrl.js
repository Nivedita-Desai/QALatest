// Info Modal Controller

angular.module('cpp.controllers')
    .controller('infoModalCtrl',
    ['$state','$scope','$rootScope','$uibModal',
        function($state,$scope,$rootScope,$uibModal) {
            console.log($scope.params);

            // get paramaters
            $scope.asset = $scope.params.asset;
            $scope.collections = $scope.params.collections;

            // variables
            $scope.hasComp = false;
            $scope.hasDocs = false;
            $scope.hasHist = false;

            // grid definitions... short for details page, full for view all page
            var compGridDef, fullCGridDef,
                histGridDef, fullHGridDef;

            // data lists to populate grids
            $scope.CompList = [];
            $scope.HistList = [];

            // set history and components
            if($scope.asset.isComponent){
                $scope.HistList = $scope.asset.assetComponentHistories;

                // set history grid
                histGridDef = [
                    {
                        field: 'info',
                        displayName: '',
                        enableCellSelection: true,
                        width: 30,
                        cellTemplate: '<div class="icon-info" ng-dblclick="viewHistDetails()"></div>'
                    },
                    {
                        field: 'ID',
                        displayName: 'ID',
                        width: 50
                    },
                    {
                        field:'Description',
                        displayName: 'Description'
                    },
                    {
                        field:'Cost',
                        displayName: 'Cost',
                        cellFilter: 'currency'
                    }
                ];
            }
            else {
                $scope.CompList = $scope.asset.assetComponents;
                $scope.HistList = $scope.asset.assetHistories;

                // set grids
                histGridDef = [
                    {
                        field: 'info',
                        displayName: '',
                        enableCellSelection: true,
                        width: 30,
                        cellTemplate: '<div class="icon-info" ng-dblclick="viewHistDetails()"></div>'
                    },
                    {
                        field: 'ID',
                        displayName: 'ID',
                        width: 50
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
                    }
                ];
                compGridDef = [
                    {
                        field: 'info',
                        displayName: '',
                        enableCellSelection: true,
                        width: 30,
                        cellTemplate: '<div class="icon-info" ng-dblclick="viewCompDetails()"></div>'
                    },
                    {
                        field: 'Name',
                        displayName: 'Name'
                    },
                    {
                        field: 'Cost',
                        displayName: 'Cost',
                        cellFilter: 'currency'
                    }
                    //,
                    //{
                    //    field: 'edit',
                    //    displayName: '',
                    //    enableCellSelection: true,
                    //    width: 30,
                    //    cellTemplate: '<div class="icon-edit" ng-dblclick="editComp()"></div>'
                    //},
                    //{
                    //    field: 'delete',
                    //    displayName: '',
                    //    enableCellSelection: true,
                    //    width: 30,
                    //    cellTemplate: '<div class="icon-delete" ng-dblclick="removeComp()"></div>'
                    //}
                ];
                fullCGridDef = [
                    {
                        field: 'info',
                        displayName: '',
                        enableCellSelection: true,
                        width: 30,
                        cellTemplate: '<div class="icon-info" ng-dblclick="viewCompDetails()"></div>'
                    },
                    {
                        field: 'ID',
                        displayName: 'ID',
                        width: 50
                    },
                    {
                        field: 'Tag',
                        displayName: 'Tag'
                    },
                    {
                        field: 'Name',
                        displayName: 'Name'
                    },
                    {
                        field: 'AcquisitionDate',
                        displayName: 'Acquisition Date',
                        cellFilter: 'date:\'MM/dd/yyyy\''
                    },
                    {
                        field: 'Cost',
                        displayName: 'Cost',
                        cellFilter: 'currency'
                    },
                    {
                        field: 'Note',
                        displayName: 'Note'
                    },
                    {
                        field: 'edit',
                        displayName: '',
                        enableCellSelection: true,
                        width: 30,
                        cellTemplate: '<div class="icon-edit" ng-dblclick="updateComp()"></div>'
                    },
                    {
                        field: 'delete',
                        displayName: '',
                        enableCellSelection: true,
                        width: 30,
                        cellTemplate: '<div class="icon-delete" ng-dblclick="removeComp()"></div>'
                    }
                ];
            }

            // full grid to go to view all
            fullHGridDef = [
                {
                    field: 'info',
                    displayName: '',
                    enableCellSelection: true,
                    width: 30,
                    cellTemplate: '<div class="icon-info" ng-dblclick="viewHistDetails()"></div>'
                },
                {
                    field: 'ID',
                    displayName: 'ID',
                    width: 50
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
                }
            ];

            // set views
            $scope.hasComp = $scope.CompList.length != 0;
            $scope.hasHist = $scope.HistList.length != 0;

            // format dates and cost for view
            $scope.adate = moment($scope.asset.AcquisitionDate).format("MM/DD/YYYY");
            $scope.cost = "$" + parseFloat($scope.asset.Cost).toFixed(2).replace(/(\d)(?=(\d{3})+(?!\d))/g,"$1,");

            // used to organize selected items
            $scope.myHistSelections = [];
            $scope.myCompSelections = [];

            // grids - component, history, data (uploads)
            $scope.compGrid = {
                data: 'CompList',
                enableRowSelection: true,
                enableCellSelection: false,
                selectedComps: $scope.myCompSelections,
                enableCellEditOnFocus: false,
                enableCellEdit: false,
                multiSelect: false,
                rowHeight: 35,
                afterSelectionChange: function (rowItem){
                    $scope.myCompSelections = rowItem;
                },
                columnDefs: compGridDef
            };
            $scope.histGrid = {
                data: 'HistList',
                enableRowSelection: true,
                enableCellSelection: false,
                selectedItems: $scope.mySelections,
                enableCellEditOnFocus: false,
                enableCellEdit: false,
                multiSelect: false,
                rowHeight: 35,
                afterSelectionChange: function (rowItem) {
                    $scope.myHistSelections = rowItem;
                },
                columnDefs: histGridDef
            };

            // create
            $scope.addHist = function() { addNew("hist"); };
            $scope.addComp = function() { addNew("comp"); };
            var addNew = function(type){
                var params = {
                    asset: $scope.asset,
                    isNew: false
                };
                openModal(
                    (type=="hist")?'app/views/Modal/new_history_log_modal.html':'app/views/Modal/new_comp_modal.html',
                    (type=="hist")?"newHistoryModalCtrl":"newCompModalCtrl",
                    (type=="hist")?'md':'lg',
                    params
                );
            };

            // view details
            $scope.viewHistDetails = function() { viewDetails("hist"); };
            $scope.viewCompDetails = function() { viewDetails("comp"); };
            var viewDetails = function(type){
                var params = {
                    dt: (type=="hist")?$scope.myHistSelections:$scope.myCompSelections,
                    asset: $scope.asset
                };
                openModal(
                    (type=="hist")?'app/views/Modal/log_detail_modal.html':'app/views/Modal/comp_detail_modal.html',
                    (type=="hist")?"LogDetailModalCtrl":"CompDetailModalCtrl",
                    'md',
                    params
                );
            };

            // update
            $scope.updateComp = function() { update("comp"); };
            $scope.updateSelf = function() { update("self"); };
            var update = function(type){
                var params = {
                    isComp: (type=="self")?$scope.asset.isComponent:true,
                    asset: (type=="comp")?$scope.myCompSelections.entity:$scope.asset,
                    assetCollection: $scope.assetCollection,
                    facilityCollection: $scope.facilityCollection,
                    statusCollection: $scope.statusCollection,
                    parentCollection: $scope.parentCollection
                };
                openModal(
                    'app/views/Modal/editAssetModal.html',
                    "editAssetModalCtrl",
                    'md',
                    params
                );
            };

            // view all
            $scope.viewAllHist = function() { viewAll("hist"); };
            $scope.viewAllComp = function() { viewAll("comp"); };
            var viewAll = function(type) {
                var params = {
                    list: (type=="hist")?$scope.HistList:$scope.CompList,
                    col: (type=="hist")?fullHGridDef:fullCGridDef,
                    nm: (type=="hist")?"History Log":"Component List",
                    asset: $scope.asset,
                    add: (type=="hist")?"Component":"Log",
                    assetCollection: $scope.assetCollection,
                    isNew: false
                };
                openModal(
                    'app/views/Modal/va_modal.html',
                    'VAModalCtrl',
                    'lg',
                    params
                );
            };

            var openModal = function(template,controller,size,params) {
                var scope = $rootScope.$new();
                scope.params = params;

                $rootScope.modalInstance = $uibModal.open({
                    backdrop: 'static',
                    keyboard: false,
                    scope: scope,
                    templateUrl: template,
                    size: size,
                    controller: controller
                });
                $rootScope.modalInstance.result.then(function(){
                    console.log(scope.params);
                });
            };

            // delete
            $scope.removeComp = function() { remove("comp"); };
            var remove = function(type) {
                // variables
                var s = "Removing ",
                    url = serviceBasePath+'response/',
                    item = { "Operation": 3, "ID": 0};

                // set variables
                if(type == "comp"){
                    s += "component";
                    url += 'Component/';
                    item.ID = $scope.myCompSelections.entity.ID;
                }
                else {
                    alert("An error has occurred.");
                    return;
                }

                // print action
                console.log(s);

                // send back end request
                $http({
                    url:url,
                    method:"POST",
                    data:item,
                    headers:{'Content-Type':'application/json'}
                }).then(function(response){
                    console.log(response);
                });
            };


            // close modal
            $scope.cancel = function() { $scope.$close(); };

        }]);
