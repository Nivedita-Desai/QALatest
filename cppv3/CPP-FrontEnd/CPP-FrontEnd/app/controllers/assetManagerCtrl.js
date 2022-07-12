// Asset Manager Controller

angular.module('cpp.controllers')
    .controller('AssetManagerCtrl',
    ['$state','$scope', '$rootScope','FundType','$uibModal','$http','Page','ProjectTitle','TrendStatus','$location','myLocalStorage','localStorageService',
        function($state,$scope, $rootScope,FundType,$uibModal,$http,Page,ProjectTitle,TrendStatus,$location,myLocalStorage,localStorageService) {
            ProjectTitle.setTitle('');
            TrendStatus.setStatus('');
            Page.setTitle("Asset Manager");
            var user = localStorageService.get('authorizationData');
            console.log(user.userName);

            $scope.assetCollection = [];
            $scope.parentCollection = [];
            $scope.facilityCollection = [];
            $scope.statusCollection = [
                {ID: "01", Status: "Active"},
                {ID: "02", Status: "Inactive"}
            ];
            $scope.collections = {
                "assetC": $scope.assetCollection,
                "facilityC": $scope.facilityCollection,
                "statusC": $scope.statusCollection,
                "parentC": $scope.parentCollection
            };

            // get info from backend
            $http({
                method : "GET",
                url : serviceBasePath+'request/Facility/0'
            }).then(function(data){
                intitializeData(data.data);
            });

            // initialize collections
            var intitializeData = function(list){
                angular.forEach(list,function(item){
                    var facility = item;
                    $scope.facilityCollection.push({ ID: facility.ID, Name: facility.Name });
                    if(facility.facilityAssets.length > 0){
                        angular.forEach(facility.facilityAssets,function(item){

                            var asset = item.Asset;
                            asset.FacilityID = facility.ID;
                            asset.FacilityName = facility.Name;
                            asset.FacilityLocation = facility.Location;
                            asset.FacilityNote = facility.Note;
                            asset.isComponent = false;
                            asset.CompCount = item.Asset.assetComponents.length;

                            $scope.assetCollection.push(asset);
                            $scope.parentCollection.push(asset);

                            if(asset.assetComponents.length > 0){
                                angular.forEach(asset.assetComponents,function(item){

                                    var component = item;
                                    component.FacilityID = asset.FacilityID;
                                    component.FacilityName = asset.FacilityName;
                                    component.FacilityLocation = asset.FacilityLocation;
                                    component.FacilityNote = asset.FacilityNote;
                                    component.Status = asset.Status;
                                    component.isComponent = true;
                                    component.CompCount = 0;

                                    $scope.assetCollection.push(component);
                                });
                            }
                        });
                    }
                });
            };

            // grid
            $scope.selectedItem = [];
            $scope.cellInputEditableTemplate = '<input ng-class="\'colt\' + col.index" ng-input="COL_FIELD" ng-model="COL_FIELD" ng-blur="updateEntity(row)" />';
            $scope.cellSelectEditableTemplate = '<select ng-class="\'colt\' + col.index" ng-input="COL_FIELD" ng-model="COL_FIELD" ng-options="id.PositionDescription for id in positionCollection track by id.PositionID" ng-blur="updateEntity(row)" />';
            $scope.cellCheckEditTableTemplate =  '<input class="c-approval-matrix-check"  type="checkbox" ng-input="COL-FIELD" ng-model="COL_FIELD"/>';
            $scope.gridOptions = {
                data: 'assetCollection',
                enableRowSelection: true,
                enableCellSelection: false,
                selectedItems: $scope.selectedItem,
                enableCellEditOnFocus: false,
                enableCellEdit: false,
                multiSelect: false,
                rowHeight: 35,
                width: 2000,
                columnDefs: [
                    {
                        field: 'info',
                        displayName: '',
                        enableCellSelection: true,
                        width: 30,
                        cellTemplate: '<div class="icon-info" ng-click="assetDetails()" style="height:30px;"></div>'
                    },
                    {
                        field:'FacilityName',
                        displayName:'Facility',
                        enableCellEdit:false
                    },
                    {
                        field: 'Name',
                        displayName: 'Name'
                    },
                    {
                        field: 'Tag',
                        displayName: 'Tag'
                    },
                    {
                        field: 'AcquisitionDate',
                        displayName: 'Acquisition Date',
                        cellFilter: 'date:\'MM/dd/yyyy\''
                    },
                    {
                        field: 'FacilityLocation',
                        displayName: 'Location'
                    },
                    {
                        field: 'Cost',
                        displayName: 'Cost',
                        cellFilter: 'currency'
                    },
                    {
                        field: 'Status',
                        displayName: 'Status',
                        width: 100
                    },
                    {
                        field: 'CompCount',
                        displayName: 'Components',
                        width: 120
                    },
                    {
                        field: 'edit',
                        displayName: '',
                        enableCellSelection: true,
                        width: 30,
                        cellTemplate: '<div class="icon-edit" ng-click="update()" style="height:30px;"></div>'
                    },
                    {
                        field: 'delete',
                        displayName: '',
                        enableCellSelection: true,
                        width: 30,
                        cellTemplate: '<div class="icon-delete" ng-click="delete()" style="height:30px;"></div>'
                    }
                ]
            };

            // functions
            $scope.assetDetails = function(){
                console.log("View Asset Details");

                var scope = $rootScope.$new();
                scope.params = {
                    asset : $scope.selectedItem[0],
                    collections: $scope.collections
                };

                var lnk = ($scope.selectedItem[0].isComponent)
                    ? 'app/views/Modal/comp_modal.html'
                    : 'app/views/Modal/info_modal.html';

                console.log(scope.params);

                $rootScope.modalInstance = $uibModal.open({
                    backdrop: 'static',
                    keyboard: false,
                    scope: scope,
                    templateUrl: lnk,
                    size: 'lg',
                    controller: "infoModalCtrl"
                });
                $rootScope.modalInstance.result.then(function (selectedItem){
                    console.log('modal selected Row: ' + selectedItem);
                }, function(){
                    console.log('Modal dismissed at: ' + new Date());
                });
            };
            $scope.newAsset = function(){
                console.log("Add Asset");

                var scope = $rootScope.$new();
                scope.params = {
                    isComponent: false,
                    collections: $scope.collections
                };
                $rootScope.modalInstance = $uibModal.open({
                    backdrop: 'static',
                    keyboard: false,
                    scope: scope,
                    templateUrl: 'app/views/Modal/new_asset_modal.html',
                    size: 'lg',
                    controller: "newAssetModalCtrl"
                });
                $rootScope.modalInstance.result.then(function(result){
                    console.log(result);
                    console.log(scope.params);
                });
            };
            $scope.delete = function() {
                console.log("Delete Asset");

                // ADD warning about delete

                var url = serviceBasePath+'response/';
                $scope.gridOptions.selectedItems[0].isComponent
                    ? url += 'Component/'
                    : url += 'Asset/';

                var item = {
                    "Operation": 3,
                    "ID": $scope.gridOptions.selectedItems[0].ID
                };

                $http({
                    url: url,
                    method: "POST",
                    data: item,
                    headers: {'Content-Type': 'application/json'}
                }).then(function(response){
                    if(response.data.result==='Success'){
                        $scope.assetCollection.splice($scope.assetCollection.indexOf($scope.gridOptions.selectedItems[0]),1);
                    }
                });
            };
            $scope.update = function() {
                console.log("Update Asset");

                var scope = $rootScope.$new(),
                    tUrl = 'app/views/Modal/editAssetModal.html',
                    ctrl = "editAssetModalCtrl";
                scope.params =
                {
                    isComp: $scope.gridOptions.selectedItems[0].isComponent,
                    asset: $scope.gridOptions.selectedItems[0],
                    assetCollection: $scope.assetCollection,
                    parentCollection: $scope.parentCollection,
                    facilityCollection: $scope.facilityCollection,
                    statusCollection: $scope.statusCollection
                };
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
                })
            }
        }]);
