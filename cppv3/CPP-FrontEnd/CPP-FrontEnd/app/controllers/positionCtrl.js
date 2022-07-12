angular.module('cpp.controllers'). 
    //Position Controll
    controller('PositionCtrl',['$state','$scope','$rootScope','fteposition','$uibModal','UpdatePosition','$http','Page','ProjectTitle','TrendStatus','$location', '$timeout', 'UniqueIdentityNumber',
        function ($state, $scope, $rootScope, fteposition, $uibModal, UpdatePosition, $http, Page, ProjectTitle, TrendStatus, $location, $timeout, UniqueIdentityNumber) {
            Page.setTitle('Position Titles');
            ProjectTitle.setTitle('');
            TrendStatus.setStatus('');
            var url = serviceBasePath+'response/FTEPosition/';
            var roleCollection;
            var url = serviceBasePath+'response/FTEPosition/';
         //   var url = 'http://localhost:29986/api/response/FTEPosition/';
            var newOrEdit = "";
            fteposition.get({}, function (PosistionsData) {
                $scope.checkList = [];
                $scope.positionCollection = PosistionsData.result;
                $scope.orgPositionCollection = angular.copy(PosistionsData.result);
                addIndex($scope.positionCollection);
                angular.forEach($scope.positionCollection,function(item,index){
                    item.checkbox = false;
                    $scope.checkList[index  + 1] = false;
                });
                $scope.gridOptions.data= $scope.positionCollection;
            });
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

            var isFresh = true;
            var currentPoint = "";

            //fteposition.getFtePosition().then(function(result){
            //    roleCollection = result.data.result;
            //    console.log(roleCollection);
            //});
            $scope.cellInputEditableTemplate = '<input ng-class="\'colt\' + col.index" ng-input="COL_FIELD" ng-model="COL_FIELD" ng-blur="updateEntity(row)" />';
            //$scope.cellSelectEditableTemplate = '<select ng-class="\'colt\' + col.index" ng-input="COL_FIELD" ng-model="COL_FIELD" ng-options="id.PositionDescription for id in positionCollection track by id.PositionID" ng-blur="updateEntity(row)" />';
            $scope.cellSelectEditableTemplate = '<select ng-class="\'colt\' + col.index" ng-input="COL_FIELD" ng-model="COL_FIELD" ng-options="id.PositionDescription for id in positionCollection track by id.Id" ng-blur="updateEntity(row)" />';
            $scope.cellCheckEditTableTemplate =  '<input class="c-approval-matrix-check"  type="checkbox" ng-input="COL-FIELD" ng-model="COL_FIELD"/>';
            $scope.addRow = function(){
                var x =  Math.max.apply(Math,$scope.positionCollection.map(function(o){

                    return o.displayId;
                }))

                if (x < 0) {
                    console.log(x);
                    x = 0;
                }

                $scope.checkList[++x];
                $scope.positionCollection .splice(x,0,{
                    displayId: x,
                    //PositionID:'',
                    PositionDescription : '',
                    MinHourlyRate: '',
                    MaxHourlyRate : '',
                    CurrentHourlyRate: '',
                    UniqueIdentityNumber: '',
                    Id: '',
                    checkbox:false,
                    new : true
                });
                console.log($scope.positionCollection);

                if (isFresh) {
                	UniqueIdentityNumber.get({
                		NumberType: 'FTEPosition',
                		'OrganizationID': 0,
                		'PhaseID': 0,
                		'CategoryID': 0
                	}, function (response) {
                		$scope.positionCollection[$scope.positionCollection.length - 1].UniqueIdentityNumber = response.result;
                		isFresh = false;
                		currentPoint = response.result;
                	});
                } else {
                	currentPoint = "BEP" + ((parseInt(currentPoint.substr(3)) + 1)).toString().padStart(5, '0');

                	$scope.positionCollection[$scope.positionCollection.length - 1].UniqueIdentityNumber = currentPoint;
                }
                $scope.gridApi.core.clearAllFilters();//Nivedita-T on 17/11/2021
                $timeout(function () {
                    console.log($scope.gridOptions.data[$scope.gridOptions.data.length - 1], $scope.gridOptions.columnDefs[0]);
                    
                    $scope.gridApi.core.scrollTo($scope.gridOptions.data[$scope.gridOptions.data.length - 1], $scope.gridOptions.columnDefs[0]);
                }, 1);
            }

            $scope.gridOptions = {
                 enableColumnMenus :false,
                 enableCellEditOnFocus: true,
                 enableFiltering: true,
                /* data: 'positionCollection',
                enableRowSelection:false,
                enableCellSelection: true,
                selectedItems: $scope.mySelections,
                enableCellEditOnFocus: true,
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
                    /*displayName:'ID',*/
                    name:'ID',
                    enableCellEdit:false,
                    //cellClass:'c-col-id',
                    width: 50,
                    cellClass: 'c-col-Num' //Manasi

                    /*cellTemplate:'<div ng-class="c-col-id" style="margin-top:15%;" ng-click="clicked(row,col)">{{row.getProperty(col.field)}}</div>'*/

                },
                //    {
                //    field: 'PositionID',
                //    displayName: 'Position ID',
                //    enableCellEditOnFocus: true,
                //    editableCellTemplate: $scope.cellInputEditableTemplate,
                //    cellFilter : 'mapStatus'
                //
                //},
                    {
                    field: 'PositionDescription',
                    /*displayName: 'Position Description',*/
                        name: 'Position Description*',
                        enableCellEdit: true,
                        width: 350
                        /*editableCellTemplate: $scope.cellInputEditableTemplate,
                    cellFilter :'mapStatus'*/


                    },
                //{
                //    field:'MinHourlyRate',
                //    name:'Minimum Hourly Rate',
                //        enableCellEdit: true,
                //        cellFilter: 'currency:$:0',
                //   /* editableCellTemplate:$scope.cellInputEditableTemplate,
                //    enableCellEditOnFocus: true*/
                //},
                //{
                //    field:'MaxHourlyRate',
                //        /*displayName:'Max Hourly Rate',*/
                //    name:'Max Hourly Rate',
                //        enableCellEdit: true,
                //        cellFilter: 'currency:$:0',
                //   /* editableCellTemplate:$scope.cellInputEditableTemplate,
                //    enableCellEditOnFocus: true*/
                //},
                {
                    field:'CurrentHourlyRate',
                    name:'Current Hourly Rate*',
                    enableCellEdit: true,
                    cellFilter: 'currency',
                    cellClass: 'c-col-Num' //Manasi

                    /*editableCellTemplate:$scope.cellInputEditableTemplate,
                    enableCellEditOnFocus: true*/
                },
                {
                    field: 'UniqueIdentityNumber',
                    name: 'Unique Identifier',
                    width: 200,
                    enableCellEdit: false,
                    //  editableCellTemplate: $scope.cellInputEditableTemplate,
                    //cellFilter :'mapStatus'


                },
                {
                    field:'checkBox',
                    name: '',
                    enableCellEdit: false,
                    enableFiltering: false,
                    width:35,
                    cellTemplate:'<input type="checkbox" ng-model="checkList[row.entity.displayId]" class = "c-col-check" ng-click="grid.appScope.check(row,col)" style="text-align: center;vertical-align: middle;">'

                }
                ]
            }

            $scope.gridOptions.onRegisterApi = function (gridApi) {
                $scope.gridApi = gridApi;

                $scope.gridApi.edit.on.beginCellEdit($scope, function (rowEntity, colDef) {
                    $('div.ui-grid-cell form').find('input').css('height', '40px');
                    $('div.ui-grid-cell form').find('select').css('height', '40px');
                    $('div.ui-grid-cell form').css('margin', '0px');
                    $('div.ui-grid-cell form').find('input').putCursorAtEnd();
                    $('div.ui-grid-cell form').find('select').putCursorAtEnd();
                    $('div.ui-grid-cell form').find('select').focus();
                    //    .on("focus", function () { // could be on any event
                    //    //$('div.ui-grid-cell form').find('input').putCursorAtEnd();
                    //    //console.log($('div.ui-grid-cell form').find('input').putCursorAtEnd());
                    //});
                });
            };

            $scope.cellClicked = function (row, col) {
                // do with that row and col what you want
                alert('hey!');
            }

            jQuery.fn.putCursorAtEnd = function () {
                console.log(this);
                return this.each(function () {
                    //alert('we in there');
                    // Cache references
                    var $el = $(this),
                        el = this;

                    // Only focus if input isn't already
                    if (!$el.is(":focus")) {
                        $el.focus();
                    }

                    // If this function exists... (IE 9+)
                    if (el.setSelectionRange) {

                        // Double the length because Opera is inconsistent about whether a carriage return is one character or two.
                        var ghettoLengthFix = 9999; //luan custom
                        var len = ghettoLengthFix * 2;

                        // Timeout seems to be required for Blink
                        setTimeout(function () {
                            el.setSelectionRange(len, len);
                            console.log('set to end');
                        }, 1);

                    } else {
                        $el.focus();
                        // As a fallback, replace the contents with itself
                        // Doesn't work in Chrome, but Chrome supports setSelectionRange
                        $el.val($el.val());

                    }

                    // Scroll to the bottom, in case we're in a tall textarea
                    // (Necessary for Firefox and Chrome)
                    this.scrollTop = 999999;

                });

            };

            $scope.check = function(row,col){
                if(row.entity.checkbox==false){
                    row.entity.checkbox = true;
                    $scope.checkList[row.entity.displayId] = true;
                    row.config.enableRowSelection = true;
                }else{
                    $scope.checkList[row.entity.displayId] = true;
                    row.entity.checkbox = false;
                }
            }
            $scope.clicked = function(row,col){
                console.log(row);
                $scope.orgRow = row;
                $scope.col = col;
                $scope.row = row.entity;
                console.log(col);
            }
            $scope.save = function () {
                $("#save_Positions").attr("disabled", true);
                var isChanged = true;
                var isFilled = true;
                var isReload = false;
                var listToSave = [];

                angular.forEach($scope.positionCollection, function(value, key,obj){

                    if (value.CurrentHourlyRate == "") value.CurrentHourlyRate = "0";

                    if (value.PositionDescription == "" ||
                        //value.MinHourlyRate == "" ||
                        //    value.MaxHourlyRate == "" ||
                        value.CurrentHourlyRate == ""
                        //|| value.UniqueIdentityNumber == ""
                    ) {
                        $("#save_Positions").attr("disabled", false);
                        dhtmlx.alert({
                            text: "Please fill data to all required fields before save (Row " + value.displayId + ")",
                            width: "300px"
                        });
                        isFilled = false;
                        return;
                    }

                    if (!(/(BEP[0-9]{5})/.test(value.UniqueIdentityNumber) && value.UniqueIdentityNumber.length == 8)) {
                        $("#save_Positions").attr("disabled", false);
                    	dhtmlx.alert({
                    		text: "Unique identifier must be in the format of BEPxxxxx (Row " + value.displayId + ")",
                    		width: "400px"
                    	});
                    	isFilled = false;
                    	return;
                    }

                    if(isFilled == false){
                        return;
                    }
                    if(value.new === true) {
                        //luan here - no need since we don't have max or min hourly rate anymore
                        //if((parseInt(value.MinHourlyRate) > parseInt(value.MaxHourlyRate)) || (parseInt(value.CurrentHourlyRate) < parseInt(value.MinHourlyRate)) || (parseInt(value.CurrentHourlyRate) > parseInt(value.MaxHourlyRate))) {
                        //    dhtmlx.alert("Please enter valid values");
                        //    return false;
                        //}

                        isReload = true;
                        var dataObj = {
                            Operation: '1',
                            PositionID: value.PositionID,
                            PositionDescription: value.PositionDescription,
                            MinHourlyRate: value.MinHourlyRate,
                            MaxHourlyRate: value.MaxHourlyRate,
                            CurrentHourlyRate: value.CurrentHourlyRate,
                            UniqueIdentityNumber: value.UniqueIdentityNumber
                        }
                        listToSave.push(dataObj);
                    }
                    else {
                        isChanged=true;
                        angular.forEach($scope.orgPositionCollection,function(orgItem){
                            if (value.Id === orgItem.Id &&
                                value.PositionDescription === orgItem.PositionDescription &&
                                //&& value.MinHourlyRate === orgItem.MinHourlyRate
                                //&& value.MaxHourlyRate === orgItem.MaxHourlyRate
                                value.CurrentHourlyRate === orgItem.CurrentHourlyRate &&
                                value.UniqueIdentityNumber === orgItem.UniqueIdentityNumber) {
                                    isChanged = false;
                                }
                        });
                        if(isChanged == true) {
                            isReload = true;
                            //if((parseInt(value.MinHourlyRate) > parseInt(value.MaxHourlyRate)) || (parseInt(value.CurrentHourlyRate) < parseInt(value.MinHourlyRate)) || (parseInt(value.CurrentHourlyRate) > parseInt(value.MaxHourlyRate))) {
                            //    dhtmlx.alert("Please enter valid values");
                            //    return false;
                            //}
                            var dataObj = {
                                Operation: '2',
                                PositionID: value.PositionID,
                                PositionDescription: value.PositionDescription,
                                MinHourlyRate: value.MinHourlyRate,
                                MaxHourlyRate: value.MaxHourlyRate,
                                CurrentHourlyRate: value.CurrentHourlyRate,
                                UniqueIdentityNumber: value.UniqueIdentityNumber,
                                Id : value.Id
                            }
                        }else{
                            var dataObj = {
                                Operation: '4',
                                PositionID: value.PositionID,
                                PositionDescription: value.PositionDescription,
                                MinHourlyRate: value.MinHourlyRate,
                                MaxHourlyRate: value.MaxHourlyRate,
                                CurrentHourlyRate: value.CurrentHourlyRate,
                                UniqueIdentityNumber: value.UniqueIdentityNumber,
                                Id: value.Id
                            }
                        }
                        listToSave.push(dataObj);
                    }

                });

                angular.forEach($scope.listToDelete,function(item){
                   listToSave.push(item);
                });

                if(isFilled == false){
                    return;
                } else {
                    console.log("Calling " + url);
                    $http({
                        url: url,
                        //url: 'http://localhost:29986/api/response/phasecode',
                        method: "POST",
                        data: JSON.stringify(listToSave),
                        headers: {'Content-Type': 'application/json'}
                    }).then(function success(response) {
                        $("#save_Positions").attr("disabled", false);

                    	isFresh = true;

                        response.data.result.replace(/[\r]/g, '\n');

                        if (response.data.result) {
                            $("#save_Positions").attr("disabled", false);
                            dhtmlx.alert(response.data.result);
                        } else {
                            $("#save_Positions").attr("disabled", false);
                            dhtmlx.alert('No changes to be saved.');
                        }

                        $scope.orgPositionCollection = angular.copy($scope.positionCollection);
                        $state.reload();

                    }, function error(response) {
                        $("#save_Positions").attr("disabled", false);
                        dhtmlx.alert("Failed to save. Please contact your Administrator.");
                    });
                }

            }
            $scope.delete = function() {
                var isChecked = false;
                var unSavedChanges = false;
                var listToSave = [];
                var selectedRow = false;
                var newList = [];
                $scope.listToDelete = [];
                angular.forEach($scope.positionCollection,function(item){
                    console.log(item);
                    isChecked = false;
                    if (item.checkbox == true) {
                        selectedRow = true;
                        if (item.new === true) {
                            unSavedChanges = true;
                            newList.push(item);
                           // $scope.positionCollection.splice(item.displayId, 1);
                        } else {
                            isChecked = true;
                            var dataObj = {
                                Operation: '3',
                                PositionID: item.PositionID,
                                PositionDescription: item.PositionDescription,
                                MinHourlyRate: item.MinHourlyRate,
                                MaxHourlyRate: item.MaxHourlyRate,
                                CurrentHourlyRate: item.CurrentHourlyRate,
                                UniqueIdentityNumber: item.UniqueIdentityNumber,
                                Id: item.Id,
                                displayId : item.displayId
                            }
                            listToSave.push(dataObj);
                            $scope.listToDelete.push(dataObj);
                            //dhtmlx.alert("Record Deleted.");
                        }
                    }
                });

                if (!selectedRow) {
                    dhtmlx.alert("Please select a record to delete.");
                }

                if(newList.length != 0){
                    for(var i = 0; i < newList.length; i++){
                        var ind = -1;
                        angular.forEach($scope.positionCollection, function(item, index){
                           if(item.displayId == newList[i].displayId){
                               item.checkbox = false;
                               ind = index;
                           }
                        });
                        if(ind != -1){
                            $scope.checkList.splice(newList[i].displayId, 1);
                            $scope.positionCollection.splice(ind,1);
                        }
                    }

                }
                if(listToSave.length != 0){
                    for(var i = 0; i < listToSave.length; i++){
                        var ind = -1;
                        angular.forEach($scope.positionCollection, function(item, index){
                            if(item.displayId == listToSave[i].displayId){
                                item.checkbox = false;
                                ind = index;
                            }
                        });
                        if(ind != -1){
                            $scope.checkList.splice(listToSave[i].displayId, 1);
                            $scope.positionCollection.splice(ind,1);
                        }
                    }
                }

            }
            $scope.checkForChanges = function(){
                var unSavedChanges = false;
                var originalCollection = $scope.orgPositionCollection;
                var currentCollection = $scope.positionCollection;
                if (currentCollection.length != originalCollection.length){
                    unSavedChanges = true;
                    return unSavedChanges;
                } else {
                    angular.forEach(currentCollection,function(currentObject){
                        for(var i = 0, len = originalCollection.length; i < len; i++) {
                            if(unSavedChanges){
                                return unSavedChanges; // no need to look through the rest of the original array
                            }
                            if(originalCollection[i].Id == currentObject.Id) {
                                var originalObject = originalCollection[i];
                                // compare relevant data
                                if(originalObject.MaxHourlyRate !== currentObject.MaxHourlyRate ||
                                    originalObject.MinHourlyRate !== currentObject.MinHourlyRate ||
                                    originalObject.CurrentHourlyRate !== currentObject.CurrentHourlyRate ||
                                    originalObject.PositionDescription !== currentObject.PositionDescription ||
                                    originalObject.UniqueIdentityNumber !== currentObject.UniqueIdentityNumber)
                                {
                                    // alert if a change has not been saved
                                    //alert("unsaved change on " + originalObject.PositionDescription);
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
                        isTest = true;
                        $scope.save();
                    //    onRouteChangeOff();
                    //    $location.path(newUrl);
                    }
                    else if(scope.params.confirm === "back"){
                        //do nothing
                    }
                });
                event.preventDefault();
            });

            //$scope.setPosition = function(position) {
            //    $scope.positionItem = position;
            //};
            //
            ////Add new position and update database
            //$scope.newPosition= function(){
            //    var scope = $rootScope.$new();
            //    newOrEdit = "new";
            //    scope.params = {positionItem:null,newOrEdit:newOrEdit};
            //    $rootScope.modalInstance = $uibModal.open({
            //        scope: scope,
            //        backdrop: 'static',
            //        keyboard: false,
            //        templateUrl: 'app/views/modal/FTE_position_modal.html',
            //        controller:'PositionModalCtrl',
            //        size: 'md'
            //    });
            //
            //    // after close the popup modal
            //    $rootScope.modalInstance.result.then(function () {
            //        fteposition.get({}, function (PosistionsData) {
            //            roleCollection = PosistionsData.result;
            //            $scope.positionCollection = roleCollection;
            //        });
            //    });
            //}
            //$scope.deletePosition = function(){
            //    var scope = $rootScope.$new();
            //    $scope.confirm = "";
            //    scope.params={confirm:$scope.confirm};
            //    $rootScope.modalInstance = $uibModal.open({
            //        scope:scope,
            //        templateUrl :'app/views/Modal/confirmation_dialog.html',
            //        controller : 'ConfirmationCtrl',
            //        size : 'sm'
            //    });
            //    $rootScope.modalInstance.result.then(function(data){
            //        if(scope.params.confirm ==='yes'){
            //            var dataObj = { Operation:"3",
            //                PositionID: $scope.positionItem.PositionID,
            //                PositionDescription:$scope.positionItem.PositionDescription,
            //                MinHourlyRate:$scope.positionItem.MinHourlyRate,
            //                MaxHourlyRate:$scope.positionItem.MaxHourlyRate,
            //                CurrentHourlyRate:$scope.positionItem.CurrentHourlyRate}
            //            var index = $scope.positionCollection.indexOf($scope.positionItem);
            //            $http.post(url,dataObj).then(function(response){
            //                if(response.data.result==="Success"){
            //                    //refresh on delete
            //                    fteposition.get({}, function (PosistionsData) {
            //                        roleCollection = PosistionsData.result;
            //                        $scope.positionCollection = roleCollection;
            //                    });
            //                    if(index !== -1){
            //                        $scope.positionCollection.splice(index,1);
            //                        $scope.positionItem = null;
            //                    }
            //
            //                }
            //                else{
            //                    alert("Delete Failed");
            //                }
            //
            //            });
            //        }
            //    });
            //
            //}
            //
            ////open a modal to edit data
            //$scope.editPosition = function(){
            //    var scope = $rootScope.$new();
            //    newOrEdit = "edit";
            //    console.log($scope.positionCollection);
            //    scope.params = {positionItem:$scope.positionItem,newOrEdit:newOrEdit};
            //    $rootScope.modalInstance = $uibModal.open({
            //        scope: scope,
            //        backdrop: 'static',
            //        keyboard: false,
            //        templateUrl: 'app/views/modal/FTE_position_modal.html',
            //        controller:'PositionModalCtrl',
            //        size: 'md'
            //    });
            //
            //    // after close the popup modal
            //    $rootScope.modalInstance.result.then(function () {
            //        fteposition.get({}, function (PosistionsData) {
            //            roleCollection = PosistionsData.result;
            //            $scope.positionCollection = roleCollection;
            //        });
            //        $scope.positionItem = null;
            //    });
            //}

        }]);