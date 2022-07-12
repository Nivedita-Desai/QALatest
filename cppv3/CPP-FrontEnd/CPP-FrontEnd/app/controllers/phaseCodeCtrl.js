angular.module('cpp.controllers').
    //Phase Code Controll
    controller('PhaseCodeCtrl',['$state','$compile','$scope','$rootScope','PhaseCode','$uibModal','$http','Page','ProjectTitle','TrendStatus','$location', '$timeout',
        function($state,$compile,$scope,$rootScope,PhaseCode,$uibModal,$http, Page,ProjectTitle,TrendStatus,$location, $timeout){
            Page.setTitle('Project Phases');
            TrendStatus.setStatus('');
            ProjectTitle.setTitle('');
            PhaseCode.get({},function(phaseCodeData){
                $scope.checkList = [];
                $scope.phaseCodeCollection = phaseCodeData.result;
                console.log(phaseCodeData);
                $scope.orgPhaseCodeCollection = angular.copy(phaseCodeData.result);
                addIndex($scope.phaseCodeCollection);
                angular.forEach($scope.phaseCodeCollection,function(item,index){
                    item.checkbox = false;
                    $scope.checkList[index + 1] = false;
                });
                $scope.gridOptions.data= $scope.phaseCodeCollection
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
            $scope.cellInputEditableTemplate = '<input ng-class="\'colt\' + col.index" ng-input="COL_FIELD" ng-model="COL_FIELD" ng-blur="updateEntity(row)" />';
            $scope.cellInputEditableTemplate2 = '<input ng-class="\'colt\' + col.index" ng-input="COL_FIELD" ng-model="COL_FIELD" ng-blur="updateEntity(row)" />';
            $scope.cellSelectEditableTemplate = '<input ng-class="\'colt\' + col.index" ng-input="COL_FIELD" ng-model="COL_FIELD" ng-model="COL_FIELD" />';
            $scope.cellCheckEditTableTemplate =  '<input class="c-approval-matrix-check"  type="checkbox" ng-input="COL-FIELD" ng-model="COL_FIELD"/>';
            $scope.addRow = function(){
                var x =  Math.max.apply(Math,$scope.phaseCodeCollection.map(function(o){

                    return o.displayId;
                }))

                if (x < 0) {
                    console.log(x);
                    x = 0;
                }

                $scope.checkList[++x] = false;
                $scope.phaseCodeCollection .splice(x,0,{
                    displayId: x,
                    PhaseDescription:'',
                    Code : '',
                    checkbox : false,
                    new : true
                });
                console.log($scope.phaseCodeCollection);
                $scope.gridApi.core.clearAllFilters();//Nivedita-T on 17/11/2021
                $timeout(function () {
                    console.log($scope.gridOptions.data[$scope.gridOptions.data.length - 1], $scope.gridOptions.columnDefs[0]);
                    
                    $scope.gridApi.core.scrollTo($scope.gridOptions.data[$scope.gridOptions.data.length - 1], $scope.gridOptions.columnDefs[0]);
                }, 1);
            }

            $scope.save = function () {
                $("#save_Phase_Code").attr("disabled", true);
                var isReload = false;
                var isChanged = true;
                var isFilled  = true;
                var listToSave = [];
                angular.forEach($scope.phaseCodeCollection, function(value, key,obj) {

                    console.log(value);
                    if (value.PhaseDescription == "" || value.ActivityPhaseCode == "") {
                        $("#save_Phase_Code").attr("disabled", false);
                        dhtmlx.alert({
                            text: "Please fill data to all required fields before save (Row " + value.displayId + ")",
                            width: "300px"
                        });
                        isFilled = false;
                        return;
                    }

                    if (isFilled == false) {
                        return;
                    }

                    //New Item
                    if (value.new === true) {

                        isReload = true;
                        var dataObj = {
                            Operation: '1',
                            PhaseDescription: value.PhaseDescription,
                            Code: value.Code,
                            ActivityPhaseCode: value.ActivityPhaseCode
                        }
                        console.log(dataObj);
                        listToSave.push(dataObj);
                    }
                    else {
                        isChanged = true;
                        angular.forEach($scope.orgPhaseCodeCollection, function (orgItem) {
                            if (value.PhaseID === orgItem.PhaseID &&
                                value.PhaseDescription === orgItem.PhaseDescription &&
                                value.ActivityPhaseCode === orgItem.ActivityPhaseCode) {  // value.Code === orgItem.Code
                                //Do nothing on unchanged Item
                                isChanged = false;
                            }

                        });
                        if (isChanged == true) {
                            var temp = '';
                            console.log(value);
                            var dataObj = {
                                Operation: '2',
                                PhaseID: value.PhaseID,
                                PhaseDescription: value.PhaseDescription,
                                Code: value.Code,
                                ActivityPhaseCode: value.ActivityPhaseCode
                            }
                        }
                        else {
                            var dataObj = {
                                Operation: '4',
                                PhaseID: value.PhaseID,
                                PhaseDescription: value.PhaseDescription,
                                Code: value.Code
                            }
                        }
                        listToSave.push(dataObj);
                    }
                });
                angular.forEach($scope.listToDelete,function(item){
                    listToSave.push(item);
                })
                console.log(listToSave);
                if(isFilled == false){
                    return;
                }else {
                    $http({
                        url: serviceBasePath + 'response/phasecode',
                        //url: 'http://localhost:29986/api/response/phasecode',
                        method: "POST",
                        data: JSON.stringify(listToSave),
                        headers: {'Content-Type': 'application/json'}
                    }).then(function success(response) {
                        $("#save_Phase_Code").attr("disabled", false);
                        response.data.result.replace(/[\r]/g, '\n');

                        if (response.data.result) {
                            $("#save_Phase_Code").attr("disabled", false);
                            dhtmlx.alert(response.data.result);
                        } else {
                            $("#save_Phase_Code").attr("disabled", false);
                            dhtmlx.alert('No changes to be saved.');
                        }

                        $scope.orgPhaseCodeCollection = angular.copy($scope.phaseCodeCollection);
                        $state.reload();

                    }, function error(response) {
                        $("#save_Phase_Code").attr("disabled", false);
                        dhtmlx.alert("Failed to save. Please contact your Administrator.");
                    });
                }
            }
            $scope.check = function(row,col){
                if(row.entity.checkbox==false){
                    row.entity.checkbox = true;
                    $scope.checkList[row.entity.displayId] = true;
                   // row.config.enableRowSelection = true;
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
            $scope.delete = function() {
                var isChecked = false;
                var unSavedChanges = false;
                $scope.listToDelete = [];
                var listToSave = [];
                var newList = [];
                var selectedRow = false;
                var success = false;
                angular.forEach($scope.phaseCodeCollection,function(item){
                    isChecked = false;
                    if (item.checkbox == true) {
                        selectedRow = true;
                        if(item.new === true) {
                            unSavedChanges = true;
                           newList.push(item);
                        } else {
                            isChecked = true;
                            var dataObj = {
                                Operation: '3',
                                PhaseID: item.PhaseID,
                                PhaseDescription: item.PhaseDescription,
                                Code: item.PhaseCode,
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
                    for(var i = 0; i < newList.length; i++) {
                        var ind = -1;
                        angular.forEach($scope.phaseCodeCollection, function (item, index) {
                            if (item.displayId == newList[i].displayId) {
                                item.checkbox = false;

                                ind = index;
                            }
                        });
                        if(ind != -1){
                            $scope.checkList.splice(newList[i].displayId,1);
                            $scope.phaseCodeCollection.splice(ind,1);
                        }
                    }

                }

                if(listToSave.length != 0) {

                    for(var i = 0; i < listToSave.length; i++) {
                        var ind = -1;
                        angular.forEach($scope.phaseCodeCollection, function (item, index) {
                            if (item.displayId == listToSave[i].displayId) {
                                item.checkbox = false;

                                ind = index;
                            }
                        });
                        if(ind != -1){
                            $scope.checkList.splice(listToSave[i].displayId,1);
                            $scope.phaseCodeCollection.splice(ind,1);
                        }
                    }

                }

            }
            $scope.gridOptions = {
                enableColumnMenus :false,
                enableCellEditOnFocus: true,
                enableFiltering: true,
                /*enableRowSelection:false,
                 data: 'phaseCodeCollection',
                enableRowSelection:false,
                enableCellSelection: true,
                selectedItems: $scope.mySelections,
                multiSelect: false,*/
                rowHeight:40,
               /* afterSelectionChange: function (rowItem, event) {
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
                    width: 50,
                    cellClass: 'c-col-Num' //Manasi

                    /*cellTemplate:'<div ng-class="c-col-id" style="margin-top:15%;" ng-click="clicked(row,col)">{{row.getProperty(col.field)}}</div>'*/

                },{
                    field: 'PhaseDescription',
                    name: 'Subservice',
                    /*editableCellTemplate: $scope.cellInputEditableTemplate,
                    cellFilter : 'mapStatus'*/

                },{
                    field: 'ActivityPhaseCode',
                    name: 'Code',
                    /*editableCellTemplate: $scope.cellInputEditableTemplate2,
                    cellFilter : 'mapStatus'*/

                },
                    {
                        field:'checkBox',
                        displayName: '',
                        enableCellEdit: false,
                        enableFiltering: false,
                        width:35,
                        cellTemplate:'<input type="checkbox" ng-model="checkList[row.entity.displayId]" class = "c-col-check" ng-click="grid.appScope.check(row,col)" style="text-align: center;vertical-align: middle;">'

                    }
                ]
            };

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

            $scope.checkForChanges = function(){
                var unSavedChanges = false;
                var originalCollection = $scope.orgPhaseCodeCollection;
                var currentCollection = $scope.phaseCodeCollection;
                if (currentCollection.length != originalCollection.length){
                    unSavedChanges = true;
                    return unSavedChanges;
                } else {
                    angular.forEach(currentCollection,function(currentObject){
                        for(var i = 0, len = originalCollection.length; i < len; i++) {
                            if(unSavedChanges){
                                return unSavedChanges; // no need to look through the rest of the original array
                            }
                            if(originalCollection[i].PhaseID == currentObject.PhaseID) {
                                var originalObject = originalCollection[i];
                                // compare relevant data
                                if(originalObject.Code !== currentObject.Code ||
                                    originalObject.PhaseDescription !== currentObject.PhaseDescription)
                                {
                                    // alert if a change has not been saved
                                    //alert("unsaved change on " + originalObject.PhaseDescription);
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
                    else if (scope.params.confirm === "save") {
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

        }]);