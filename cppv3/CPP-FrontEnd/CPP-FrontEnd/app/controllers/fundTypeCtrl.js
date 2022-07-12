angular.module('cpp.controllers').
    //FundType controller
    controller('FundTypeCtrl', ['$state', '$scope', '$rootScope', 'FundType', 'Organization', '$uibModal', '$http', '$state', 'Page', 'ProjectTitle', 'TrendStatus', '$location',
        function($state,$scope, $rootScope,FundType, Organization, $uibModal,$http,$state,Page,ProjectTitle,TrendStatus,$location){

            $scope.selectedOrg;
            $scope.selectedOrgID = null;
            Organization.lookup().get({}, function (organizationData) {
                $scope.organizationList = organizationData.result;
                $scope.setOrganization(($scope.organizationList[0]));
                //console.log("Get Org List");
                //console.log($scope.organizationList);
                $scope.filterOrg = $scope.organizationList[0].OrganizationID;
                $scope.selectedOrgID = $scope.organizationList[0].OrganizationID;

                FundType.getFundTypeByOrgID().get({ OrganizationID: $scope.filterOrg }, function (fundData) {
                    $scope.abcd = [];
                    $scope.fundCollection = fundData.result;
                    $scope.orgFundCollection = angular.copy(fundData.result);
                    addIndex($scope.fundCollection);
                    angular.forEach($scope.fundCollection, function (item, index) {
                        item.checkbox = false;
                        $scope.abcd[index + 1] = false;
                    });
                    $scope.gridOptions.data = $scope.fundCollection
                    console.log($scope.abcd);
                    console.log($scope.fundCollection);

                });
            });

            $scope.setOrganization = function (org) {

                $scope.selectedOrg = org;
                console.log("Here setOrganization");
                console.log(org);
                console.log("End here setOrganization");
                //var infoWindow;
                //google.maps.Polygon.prototype.getBounds = function () {
                //    var bounds = new google.maps.LatLngBounds();
                //    var paths = this.getPaths();
                //    var path;

                //    for (var i = 0; i < paths.getLength() ; i++) {
                //        path = paths.getAt(i);
                //        for (var ii = 0; ii < path.getLength() ; ii++) {
                //            bounds.extend(path.getAt(ii));
                //        }
                //    }
                //    return bounds;
                //}

                //initialize();
            };

            $scope.filterChangeOrg = function (inputOrgID) {
                var orgId = null;
                if (inputOrgID) {
                    orgId = inputOrgID;
                } else {
                    orgId = $("#selectOrg").val();
                }
                $scope.selectedOrgID = orgId;
                angular.forEach($scope.organizationList, function (org) {
                    if (orgId == org.OrganizationID) {
                        $scope.setOrganization(org);
                    }
                });

                

                console.log('filterChangeOrg');
                console.log(orgId);

                FundType.getFundTypeByOrgID().get({ OrganizationID: orgId }, function (fundData) {
                    $scope.abcd = [];
                    $scope.fundCollection = fundData.result;

                    $scope.orgFundCollection = angular.copy(fundData.result);
                    addIndex($scope.fundCollection);
                    angular.forEach($scope.fundCollection, function (item, index) {
                        item.checkbox = false;
                        $scope.abcd[index + 1] = false;
                    });

                    $scope.gridOptions.data = $scope.fundCollection
                    console.log($scope.abcd);
                    console.log($scope.fundCollection);

                });
            };

            var url = serviceBasePath + '/response/fundtype';
            ProjectTitle.setTitle('');
            TrendStatus.setStatus('');
            //console.log('$scope.selectedOrg');
            //console.log($scope.selectedOrg);
            
            Page.setTitle("Funding Source");
            var addIndex = function(data){
                var i = 1;
                angular.forEach(data, function(value, key, obj){
                    value.displayId =  i;
                    i = i + 1;
                    if(value.Schedule === "0001-01-01T00:00:00"){
                        value.Schedule = "";
                    }
                });
            }

            $scope.cellInputEditableTemplate = '<input ng-class="\'colt\' + col.index" ng-input="COL_FIELD" ng-model="COL_FIELD" ng-blur="updateEntity(row)" />';
            $scope.cellSelectEditableTemplate = '<select ng-class="\'colt\' + col.index" ng-input="COL_FIELD" ng-model="COL_FIELD" ng-options="id.PositionDescription for id in positionCollection track by id.PositionID" ng-blur="updateEntity(row)" />';
            $scope.cellCheckEditTableTemplate =  '<input class="c-approval-matrix-check"  type="checkbox" ng-input="COL-FIELD" ng-model="COL_FIELD"/>';
            $scope.addRow = function(){
                var x =  Math.max.apply(Math,$scope.fundCollection.map(function(o){
                    return o.displayId;
                }))

                console.log(x);
                if (x < 0) {
                    console.log(x);
                    x = 0;
                }

                $scope.abcd[++x] = false;
                $scope.fundCollection .splice(0,0,{
                    displayId: x,
                    Fund:'',
                    Amount : '',
                    Availability : '',
                    BalanceRemaining: '',
                    OrganizationID: '',
                    checkbox : false,
                    new : true
                });
                console.log($scope.fundCollection);
            }
            $scope.gridOptions = {
                enableColumnMenus :false,
                enableCellEditOnFocus: true,
               /* data: 'fundCollection',
                enableRowSelection:false,
                enableCellSelection: true,
                selectedItems: $scope.mySelections,
                multiSelect: false,*/
                rowHeight:40,
                width:800,
                /*afterSelectionChange: function (rowItem, event) {
                    console.log($scope.mySelections);
                    $scope.selectedIDs = [];
                    console.log(rowItem);
                    angular.forEach($scope.mySelections, function ( item ) {
                        $scope.selectedIDs.push( item.id )
                    });

                },*/
                columnDefs: [{
                    field:'displayId',
                    name:'ID',
                    enableCellEdit:false,
                    cellClass:'c-col-id',
                    width: 50
                    /*cellTemplate:'<div ng-class="c-col-id" style="margin-top:15%;" ng-click="clicked(row,col)">{{row.getProperty(col.field)}}</div>'*/

                },{
                    field: 'Fund',
                    name: 'Fund Name',
                    /*editableCellTemplate: $scope.cellInputEditableTemplate,
                    cellFilter : 'mapStatus'*/

                },{
                    field: 'Amount',
                    name: 'Amount',
                    /*enableCellEditOnFocus: true,
                    editableCellTemplate: $scope.cellInputEditableTemplate,*/
                    cellFilter :'currency:$:0'


                },{
                    field:'Availability',
                    name:'Availability',
                    /*editableCellTemplate:$scope.cellInputEditableTemplate,*/
                    cellFilter: 'currency:$:0'
                    /*enableCellEditOnFocus: true*/
                },{
                    field:'BalanceRemaining',
                    name:'BalanceRemaining',
                    /*editableCellTemplate:$scope.cellInputEditableTemplate,*/
                    cellFilter: 'currency:$:0'
                    /*enableCellEditOnFocus: true*/
                },
                    {
                        field:'checkBox',
                        displayName: '',
                        enableCellEdit: false,
                        width:35,
                        cellTemplate:'<input type="checkbox" class = "c-col-check" ng-model="abcd[row.entity.displayId]"   ng-click="grid.appScope.check(row,col)" style="text-align: center;vertical-align: middle;">'

                    }
                ]
            }
            $scope.check = function(row,col){
                console.log(row);
                if(row.entity.checkbox==false){
                    row.entity.checkbox = true;
                    $scope.abcd[row.entity.displayId] = true;
                    row.config.enableRowSelection = true;
                }else{
                    $scope.abcd[row.entity.displayId] = false;

                    row.entity.checkbox = false;
                }
                console.log($scope.abcd);
            }
            $scope.clicked = function(row,col){
                console.log(row);
                $scope.orgRow = row;
                $scope.col = col;
                $scope.row = row.entity;
                console.log(col);
            }
            $scope.save = function(){
                var isReload = false;
                var isChanged = true;
                var isFilled  = true;
                var listToSave = [];
                angular.forEach($scope.fundCollection, function(value, key,obj){
                    console.log(value);
                    if(isFilled == false){
                        return;
                    }
                    if(value.new === true) {
                        if(value.Fund === "") {
                            dhtmlx.alert({text:"Please fill data to all required fields before save",
                                            width:"300px"});
                            isFilled = false;
                            return;
                        }
                        isReload = true;
                        var dataObj = {
                            Operation: '1',
                            Fund: value.Fund,
                            Amount: value.Amount,
                            Availability: value.Availability,
                            BalanceRemaining: value.BalanceRemaining,
                            OrganizationID: $("#selectOrg").val()
                        };
                        console.log(dataObj);
                        listToSave.push(dataObj);
                    }
                    else {
                        console.log(value);
                        //Detech change
                        isChanged=true;
                        angular.forEach($scope.orgFundCollection, function (orgItem) {
                            if (value.FundTypeId === orgItem.FundTypeId &&
                                value.Fund === orgItem.Fund &&
                                value.Amount === orgItem.Amount &&
                                value.Availability === orgItem.Availability &&
                                value.BalanceRemaining === orgItem.BalanceRemaining) {
                                isChanged = false;
                            }
                        });
                        if (isChanged) {
                            var dataObj = {
                                Operation: '2',
                                Fund: value.Fund,
                                Amount: value.Amount,
                                Availability: value.Availability,
                                BalanceRemaining: value.BalanceRemaining,
                                FundTypeId: value.FundTypeId,
                                OrganizationID: $("#selectOrg").val()
                            }
                            listToSave.push(dataObj);
                            isChanged = true;
                        }
                       else{
                            var dataObj = {
                                Operation: '4',
                                Fund: value.Fund,
                                Amount: value.Amount,
                                Availability: value.Availability,
                                BalanceRemaining: value.BalanceRemaining,
                                FundTypeId : value.FundTypeId
                            }
                        }
                    }

                });
                angular.forEach($scope.listToDelete, function(item){

                        listToSave.push(item);

                });
                console.log(listToSave);
                if(isFilled == false){
                    return;
                }else {
                    console.log(listToSave);
                    $http({
                        url: url,
                        //url: 'http://localhost:29986/api/response/phasecode',
                        method: "POST",
                        data: JSON.stringify(listToSave),
                        headers: {'Content-Type': 'application/json'}
                    }).then(function success(response) {
                        //$state.reload();    //luan here
                        console.log($scope.selectedOrgID);
                        $scope.filterChangeOrg($scope.selectedOrgID);

                        response.data.result.replace(/[\r]/g, '\n');

                        if (response.data.result) {
                            dhtmlx.alert(response.data.result);
                        } else {
                            dhtmlx.alert('No changes to be saved.');
                        }

                    },function error(response){
                            dhtmlx.alert("Failed to save. Please contact your Administrator.");
                    });
                }
            }
            $scope.delete = function() {
                var isChecked = false;
                var unSavedChanges = false;
                var listToSave = [];
                $scope.listToDelete = [];
                var newList = [];
                angular.forEach($scope.fundCollection,function(item){
                    console.log(item);
                   if(item.checkbox == true){
                       if(item.new === true) {
                           unSavedChanges = true;
                           newList.push(item);
                       } else{
                           isChecked = true;
                           var dataObj = {
                               Operation: '3',
                               Fund: item.Fund,
                               Amount: item.Amount,
                               Availability: item.Availability,
                               BalanceRemaining: item.BalanceRemaining,
                               displayId : item.displayId,
                               FundTypeId: item.FundTypeId,
                               OrganizationID: item.OrganizationID
                           };
                           $scope.listToDelete.push(dataObj);
                           listToSave.push(dataObj);
                           //dhtmlx.alert("Record Deleted.");
                       }
                   }
                });

                //console.log($('.ngCanvas .colt5'));
                //var checkbox = $('.ngCanvas .colt5');
                //$.each(checkbox,function(index){
                //   console.log($(($(this).select('input[type=textbox]').children())[1]).children().attr('checked', false));
                //});
                if(newList.length != 0){
                    for(var i = 0; i < newList.length; i++) {
                        var ind = -1;
                        angular.forEach($scope.fundCollection, function (item, index) {
                            if (item.displayId == newList[i].displayId) {
                                item.checkbox = false;

                                ind = index;
                            }
                        });
                        if(ind != -1){

                            $scope.fundCollection.splice(ind,1);
                            $scope.abcd.splice(newList[i].displayId,1);
                        }
                        console.log($scope.fundCollection);
                        console.log($scope.abcd);
                    }

                }
                    console.log(listToSave);
                    if(listToSave.length != 0){

                        for(var i = 0; i < listToSave.length; i++) {
                            var ind = -1;
                            angular.forEach($scope.fundCollection, function (item, index) {
                                if (item.displayId == listToSave[i].displayId) {
                                    item.checkbox = false;

                                    ind = index;
                                }
                            });

                            if(ind != -1){
                                console.log(ind);
                                $scope.fundCollection.splice(ind,1);
                                $scope.abcd.splice(listToSave[i].displayId,1);
                            }
                            console.log($scope.fundCollection);
                            console.log($scope.abcd);
                        }
                    }
                //if(listToSave.length != 0) {
                //    $http({
                //        url: url,
                //        //url: 'http://localhost:29986/api/response/phasecode',
                //        method: "POST",
                //        data: JSON.stringify(listToSave),
                //        headers: {'Content-Type': 'application/json'}
                //    }).then(function (response) {
                //        if (response.data.result === 'Success') {
                //
                //            $scope.orgFundCollection = angular.copy($scope.fundCollection);
                //            console.log($scope.fundCollection);
                //           // $state.reload();        //Temporary Solution
                //        }
                //        else {
                //            alert("failed to delete");
                //        }
                //    });
                //}


            }
            $scope.checkForChanges = function(){
                var unSavedChanges = false;
                var originalCollection = $scope.orgFundCollection;
                var currentCollection = $scope.fundCollection;
                if (currentCollection.length != originalCollection.length){
                    unSavedChanges = true;
                    return unSavedChanges;
                } else {
                    angular.forEach(currentCollection,function(currentObject){
                        for(var i = 0, len = originalCollection.length; i < len; i++) {
                            if(unSavedChanges){
                                return unSavedChanges; // no need to look through the rest of the original array
                            }
                            if(originalCollection[i].FundTypeId == currentObject.FundTypeId) {
                                var originalObject = originalCollection[i];
                                // compare relevant data
                                if(originalObject.Fund !== currentObject.Fund ||
                                    originalObject.Amount !== currentObject.Amount ||
                                    originalObject.Availability !== currentObject.Availability ||
                                    originalObject.BalanceRemaining !== currentObject.BalanceRemaining)
                                {
                                    // alert if a change has not been saved
                                    //alert("unsaved change on " + originalObject.Fund);
                                    unSavedChanges = true;
                                    return unSavedChanges;
                                }
                                break; //no need to check any further, go to next object in new collection
                            }
                        }
                    });
                }
                return unSavedChanges;
            };
            onRouteChangeOff = $scope.$on('$locationChangeStart', function(event){
                var newUrl = $location.path();
                if(!$scope.checkForChanges()) return;
                $scope.confirm = "";
                var scope = $rootScope.$new();
                scope.params = {confirm:$scope.confirm};
                $rootScope.modalInstance = $uibModal.open({
                    scope: scope,
                    templateUrl: 'app/views/Modal/exit_confirmation_modal.html',
                    controller: 'exitConfirmation',
                    size: 'md',
                    backdrop: true
                });
                $rootScope.modalInstance.result.then(function(data){
                    console.log(scope.params.confirm);
                    if(scope.params.confirm === "exit"){
                        onRouteChangeOff();
                        $location.path(newUrl);
                    }
                    else if(scope.params.confirm === "save"){
                        $scope.save();
                        onRouteChangeOff();
                        $location.path(newUrl);
                    }
                    else if(scope.params.confirm === "back"){
                        //do nothing
                    }
                });
                event.preventDefault();
            });

            //$scope.setFund = function(item){
        //    $scope.fundItem = item;
        //}
        //var newOrEdit = "";
        //$scope.newFund = function(){
        //    var scope = $scope.$new();
        //    $scope.fundItem = null;
        //    newOrEdit = 'new';
        //    scope.params={
        //
        //        fundItem:$scope.fundItem,
        //        newOrEdit :newOrEdit
        //    }
        //    $rootScope.modalInstance = $uibModal.open({
        //        backdrop: 'static',
        //        keyboard: false,
        //        scope:scope,
        //        templateUrl:"app/views/Modal/fund_type_modal.html",
        //        size : "md",
        //        controller : "fundModalCtrl"
        //    });
        //
        //    $rootScope.modalInstance.result.then(function(response){
        //       $state.reload();
        //    });
        //}
        //$scope.editFund = function(){
        //    var scope = $scope.$new();
        //    newOrEdit = 'edit';
        //    scope.params = {
        //        fundItem:$scope.fundItem,
        //        newOrEdit :newOrEdit
        //    }
        //    $rootScope.modalInstance = $uibModal.open({
        //        backdrop: 'static',
        //        keyboard: false,
        //        scope:scope,
        //        templateUrl:"app/views/Modal/fund_type_modal.html",
        //        size : "md",
        //        controller : "fundModalCtrl"
        //    });
        //
        //    $rootScope.modalInstance.result.then(function(response){
        //        $state.reload();
        //    });
        //}
        //$scope.deleteFund = function(){
        //    $scope.confirm = "";
        //    var scope = $rootScope.$new();
        //    scope.params = {confirm:$scope.confirm};
        //
        //    $rootScope.modalInstance = $uibModal.open({
        //        backdrop:'static',
        //        templateUrl: 'app/views/modal/confirmation_dialog.html',
        //        scope : scope,
        //        controller : "ConfirmationCtrl",
        //        size : 'sm'
        //
        //    });
        //
        //    $rootScope.modalInstance.result.then(function(data){
        //
        //        if(scope.params.confirm === 'yes'){
        //
        //            var dataObj = {
        //
        //                'Operation':'3',
        //                'FundTypeId': $scope.fundItem.FundTypeId,
        //                'Fund':$scope.fundItem.Fund,
        //                'Amount':$scope.fundItem.Amount,
        //                'Availability':$scope.fundItem.Availability,
        //                'BalanceRemaining':$scope.fundItem.BalanceRemaining
        //
        //            }
        //            $http({
        //                //url: 'http://192.168.0.19:1832/response/fundtype',
        //                url: 'http://localhost:29986/api/response/fundtype',
        //                //  url: 'http://localhost/api/response/phasecode',
        //                method: "POST",
        //                data: JSON.stringify(dataObj),
        //                headers: {'Content-Type': 'application/json'}
        //            }).then(function(response){
        //                if(response.data.result ==='Success'){
        //                    var index = $scope.fundCollection.indexOf($scope.fundItem);
        //                    $scope.fundCollection.splice(index,1);
        //                }
        //                else{
        //                    alert("Delete failed");
        //                }
        //            });
        //        }
        //
        //    });
        //}
        }]);
