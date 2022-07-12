angular.module('cpp.controllers').
    //Approval matrix Controller
    controller('ApprovalMatrixCtrl',['$state','UpdateApprovalMatrix','$timeout','$scope', '$rootScope','fteposition','Page','$http',
        'ApprovalMatrix', 'localStorageService', 'ProjectTitle', 'TrendStatus', '$uibModal', '$location', '$filter', 'MFAConfiguration',
        function($state,UpdateApprovalMatrix,$timeout,$scope, $rootScope,fteposition,Page,$http,
            ApprovalMatrix, localStorageService, ProjectTitle, TrendStatus, $uibModal, $location, $filter, MFAConfiguration){
            TrendStatus.setStatus('');
            var url = serviceBasePath+"response/approvalMatrix";
            ProjectTitle.setTitle('');
            var mfaDetailsObj, mfaDetails;
            $http.get(serviceBasePath+'request/role').then(function(response){
                $scope.RoleCollection = response.data.result;
                $scope.gridOptions.columnDefs[1].editDropdownOptionsArray = response.data.result;
                console.log($scope.RoleCollection)
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
            ApprovalMatrix.get({}, function (approvalMatrixData) {
                //debugger;
                var i = 1;
                $scope.checkList = [];
                $scope.approvalMatrixCollection = approvalMatrixData.result;
                console.log(approvalMatrixData.result);
                $scope.originalApprovalMatrix = angular.copy(approvalMatrixData.result);
                addIndex($scope.approvalMatrixCollection);
                angular.forEach($scope.approvalMatrixCollection, function (item, index) {
                    //item.Cost = toString(item.Cost);
                    item.checkbox = false;
                    $scope.checkList[index+1] = false;
                });
                console.log($scope.approvalMatrixCollection);
                $scope.gridOptions.data= $scope.approvalMatrixCollection
            })
            //------------------------------Swapnil 17-09-2020------------------------------------------------------------------------------
            MFAConfiguration.lookup().get(function (response) {
                //debugger
                console.log(response);
                mfaDetails = response.result;

                if (mfaDetails.IsMFAEnabled == "0") {
                    $('#chkEnableMFA').prop('checked', false);
                }
                else if (mfaDetails.IsMFAEnabled == "1") {
                    $('#chkEnableMFA').prop('checked', true);
                }
                $('#txtNoDaysForReminderMail').val(mfaDetails.ReminderMailDays);
                $('#txtNoOfMinForValidityCode').val(mfaDetails.ApprovalCodeValidity);
            });
            //------------------------------------------------------------------------------------------------------------------------------
            //Page.setTitle("Project Approval Requirements"); Manasi
            Page.setTitle("Trend Approval Requirements");
            function checkForDuplicateRole(){
                isExist = false;
                for(var i = 0; i < $scope.approvalMatrixCollection.length ; i++){
                    var role = $scope.approvalMatrixCollection[i];
                    for(var j = 0; j < $scope.approvalMatrixCollection.length; j++){
                        if(i == j){
                            //do nothing
                        }else{
                            if((typeof $scope.approvalMatrixCollection[j].Role) == "string"){
                                if(role.Role === $scope.approvalMatrixCollection[j].Role){
                                    isExist = true;
                                }
                            }
                            else{
                                console.log(typeof role);
                               //if((typeof role) !== "string"){
                               //    role = role.Role;
                               //}
                                console.debug(role.Role);
                                console.debug($scope.approvalMatrixCollection[j].Role.Role);
                                if(role.Role === $scope.approvalMatrixCollection[j].Role.Role) {
                                    isExist = true;
                                }
                            }
                        }

                    }

                }
                return isExist;
            }
            $scope.checkRole = function(role){
                isExist = false;
                angular.forEach($scope.approvalMatrixCollection,function(item){

                    if(role.Role === item.Role){
                        isExist = true;
                        dhtmlx.alert({text:"The role " + role.Role + " already exist. Please select another role!",
                                    width: '400px'});
                    }
                   // if(isExist) return;
                });
                return isExist;
            }
            $scope.cellInputEditableTemplate = '<input ng-class="\'colt\' + col.index" ng-input="COL_FIELD" ng-model="COL_FIELD" ng-blur="updateEntity(row)" />';
           /* $scope.cellSelectEditableTemplate = '<select ng-class="\'colt\' + col.index" ui-grid-edit-dropdown ng-input="COL_FIELD" ng-change="checkRole(COL_FIELD)" ng-model="COL_FIELD" ng-options="role.Role for role in RoleCollection" />';*/
            $scope.cellSelectEditableTemplate = '<select ng-class=\"\'colt\' + col.uid\"  ui-grid-edit-dropdown ng-model=\"MODEL_COL_FIELD\" ng-options=\"field[editDropdownIdLabel] as field[editDropdownValueLabel] CUSTOM_FILTERS for field in editDropdownOptionsArray\"></select>';
            $scope.cellCheckEditTableTemplate =  '<input class="c-approval-matrix-check"  type="checkbox" ng-input="COL-FIELD" ng-model="COL_FIELD"/>';
            $scope.addRow = function(){
                var x =  Math.max.apply(Math,$scope.approvalMatrixCollection.map(function(o){

                    return o.displayId;
                }))

                if (x < 0) {
                    console.log(x);
                    x = 0;
                }

                $scope.checkList[++x] = false;
                $scope.approvalMatrixCollection .splice(x,0,{
                    displayId: x,
                    Role:'Click to select',
                    Cost : 0,
                    Schedule: '',
                    checkbox:false,
                    new : true
                });
                $scope.gridApi.core.clearAllFilters();//Nivedita-T on 17/11/2021
                $timeout(function () {
                    console.log($scope.gridOptions.data);
                    console.log($scope.gridOptions.data[$scope.gridOptions.data.length - 1], $scope.gridOptions.columnDefs[0]);
                    
                    $scope.gridApi.core.scrollTo($scope.gridOptions.data[$scope.gridOptions.data.length - 1], $scope.gridOptions.columnDefs[0]);
                }, 1);
            }
            var editableCellTemplateUsingDatePicker = '<input ng-class="\'colt\' + col.index" type="text" ng-model="COL_FIELD"  ng-input="COL_FIELD" class="form-control" ng-blur="updateEntity(row)"  />';
            console.log(editableCellTemplateUsingDatePicker);
            var temp = '<input id="test"  ng-class="\'colt\' + col.index" type="text" class="form-control datepicker" datepicker-popup="dd/MM/yyyy" datepicker-append-to-body=true ng-model="COL_FIELD" ng-blur="updateEntity(row)" />';
            $scope.mySelections = [];
            $scope.gridOptions = {
                enableColumnMenus :false,
                enableCellEditOnFocus: true,
                enableFiltering: true,
                data: 'approvalMatrixCollection',
                 /* enableRowSelection:false,
                enableCellSelection: true,
                selectedItems: $scope.mySelections,
                enableCellEditOnFocus: true,
                multiSelect: false,*/
                rowHeight:40,

               /* afterSelectionChange: function (rowItem, event) {
                    console.log($scope.mySelections);
                    $scope.selectedIDs = [];
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
                    field: 'Role',
                    name: 'Role',/*
                    enableCellEditOnFocus: true,*/
                    editableCellTemplate: 'ui-grid/dropdownEditor',
                    editDropdownValueLabel: 'Role',
                    editDropdownIdLabel: 'Role',
                    editDropDownChange: 'checkRole(COL_FIELD)'
                    /* editDropdownOptionsArray: $scope.RoleCollection*/
                    /*cellFilter : 'mapStatus'*/

                },{
                    field: 'Cost',
                    name: 'Cost',
                    enableCellEdit: true,
                    type: 'text',
                        cellFilter: 'currency',
                        cellClass: 'c-col-Num' //Manasi

                    /*enableCellEditOnFocus: true,
                    editableCellTemplate: $scope.cellInputEditableTemplate,*/
                    //cellFilter :'currency:$:0'
                    //cellFilter : 'customCurrency:row.entity'

                    },
                // Jignesh 23-11-2020
                //    {
                //    field:'Schedule',
                //    name:'Schedule',
                //   /* editableCellTemplate:editableCellTemplateUsingDatePicker,*/
                //    //cellFilter: 'date:"dd/MM/yyyy"',
                //    placeholder : '50 days',
                //        enableCellEdit: true,
                //        cellClass: 'c-col-Num' //Manasi

                //},
                    {
                        field:'checkBox',
                        displayName: '',
                        enableCellEdit: false,
                        enableFiltering: false,
                        width:35,
                        cellTemplate:'<input id="checkboxId[row.entity.displayId]" ng-model="checkList[row.entity.displayId]" type="checkbox" class = "c-col-check" ng-click="grid.appScope.check(row,col)" style="text-align: center;vertical-align: middle;">'

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
                   //$scope.checkList[row.entity.displayId] = true;
                    row.config.enableRowSelection = true;
                }else{
                   // $scope.checkList[row.entity.displayId] = false;
                    row.entity.checkbox = false;
                }
            }
            $scope.clicked = function(row,col){
                $scope.row = row;
                console.log(row);
                $scope.col = col;
                $scope.idToDelete = row.entity.Id;
                var s = $('#checkboxId[' + row.entity.id + ']');
                console.log(s);

            }
            $scope.save = function () {
                $("#save_Approval_Matrix").attr("disabled", true);
                var isExist = checkForDuplicateRole();
                if (isExist == true) {
                    $("#save_Approval_Matrix").attr("disabled", false);
                    dhtmlx.alert({text:"Roles are duplicated. Please check that all roles are unique!",
                                    width:'400px'})
                    return;
                }
                var isReload = false;
                var isChanged = true;
                var isFilled  = true;
                var listToSave = [];
                angular.forEach($scope.approvalMatrixCollection, function (value, key, obj) {
                    //debugger;
                    //if (value.Role == "" || value.Schedule == "") {
                    if (value.Role == "" || value.Cost == "") {
                        $("#save_Approval_Matrix").attr("disabled", false);
                        dhtmlx.alert({
                            text: "Please fill data to all required fields before save (Row " + value.displayId + ")",
                            width: "400px"
                        });
                        isFilled = false;
                        return;
                    }

                    if(isFilled == false){
                        return;
                    }

                    var isChanged = false;
                    if(value.new === true) {

                        isReload = true;
                        var dataObj = {
                            Operation: '1',
                            Role: value.Role,
                            Cost: value.Cost,
                            Schedule: value.Schedule
                        }
                     listToSave.push(dataObj);
                    }
                    else {
                        isChanged = true;
                        angular.forEach($scope.originalApprovalMatrix,function(orgItem){
                            if(value.Role == orgItem.Role && value.Cost == orgItem.Cost && value.Schedule == orgItem.Schedule){

                                //Do nothing on unchanged Item
                                isChanged = false;

                            }
                        });
                        if(isChanged == true) {

                            var temp = '';
                            isReload = true;
                            if (typeof value.Role == 'string') {
                                temp = value.Role;
                            } else {
                                temp = value.Role;
                            }
                            var dataObj = {
                                Operation: '2',
                                Id: value.Id,
                                Role: temp,
                                Cost: value.Cost,
                                Schedule: value.Schedule
                            }

                        }else{
                            var temp = '';
                            isReload = true;
                            if (typeof value.Role == 'string') {
                                temp = value.Role;
                            } else {
                                temp = value.Role;
                            }
                            var dataObj = {
                                Operation: '4',
                                Id: value.Id,
                                Role: temp,
                                Cost: value.Cost,
                                Schedule: value.Schedule
                            }
                        }
                        listToSave.push(dataObj);
                    }

                })


                    angular.forEach($scope.listToDelete, function(item){
                       listToSave.push(item);
                    });


                if(isFilled == false){
                    return;
                }else {
                    //------------------------------ Swapnil 30/11/2020 --------------------------------------------------
                    $http.post(url, listToSave)
                        .then(function success(response) {
                            $("#save_Approval_Matrix").attr("disabled", false);
                            console.log(response);
                            // console.log("Add new Succesfully");
                            $state.reload();
                            response.data.result.replace(/[\r]/g, '\n');

                            if (response.data.result == "") {
                                $("#save_Approval_Matrix").attr("disabled", false);
                                response.data.result = "No changes to be saved.";
                            }

                            var IsMFAEnabled = 0;
                            if ($('#chkEnableMFA').is(':checked')) {
                                IsMFAEnabled = 1;
                            }
                            var ReminderMailDays = $("#txtNoDaysForReminderMail").val();
                            var ApprovalCodeValidity = $("#txtNoOfMinForValidityCode").val();

                            if (ReminderMailDays == "" || ReminderMailDays == "0") {
                                $("#save_Approval_Matrix").attr("disabled", false);
                                ReminderMailDays = 0;
                                dhtmlx.alert("Please enter valid value");
                                return;

                            }

                            if (ApprovalCodeValidity == "" || ApprovalCodeValidity == "0") {
                                $("#save_Approval_Matrix").attr("disabled", false);
                                ReminderMailDays = 0;
                                dhtmlx.alert("Please enter valid code validity time");
                                return;
                            }



                            mfaDetailsObj = {
                                "MFAConfigID": 1,
                                "IsMFAEnabled": IsMFAEnabled,
                                "ReminderMailDays": parseInt(ReminderMailDays),
                                "ApprovalCodeValidity": parseInt(ApprovalCodeValidity)
                            }
                            if (mfaDetails.MFAConfigID != mfaDetailsObj.MFAConfigID ||
                                mfaDetails.IsMFAEnabled != mfaDetailsObj.IsMFAEnabled ||
                                mfaDetails.ReminderMailDays != mfaDetailsObj.ReminderMailDays ||
                                mfaDetails.ApprovalCodeValidity != mfaDetailsObj.ApprovalCodeValidity) {
                                MFAConfiguration.persist().save(mfaDetailsObj, function (res) {
                                    $("#save_Approval_Matrix").attr("disabled", false);
                                    if (res.result.trim() == 'MFA details has been updated successfully.') {
                                        dhtmlx.alert("Changes saved successfully.");
                                        return;
                                    }
                                });
                            } else {
                                if (response.data.result == "No changes to be saved.") {
                                    $("#save_Approval_Matrix").attr("disabled", false);
                                    dhtmlx.alert("No changes to be saved.");
                                } else {
                                    $("#save_Approval_Matrix").attr("disabled", false);
                                    dhtmlx.alert("Changes saved successfully.");
                                }
                            }

                        }, function error(response) {
                            $("#save_Approval_Matrix").attr("disabled", false);
                            dhtmlx.alert("Failed to save. Please contact your Administrator.");
                        });
                    //----------------------------------------------------------------------------------------------------------------------------
                }
            }
            $scope.delete = function(){
                var isChecked = false;
                var unSavedChanges = false;
                var listToSave = [];
                var newList = [];
                var selectedRow = false;
                $scope.listToDelete = [];
                angular.forEach($scope.approvalMatrixCollection,function(item,key){
                    isChecked = false;
                    if(item.checkbox==true) {
                        selectedRow = true;
                        if(item.new ===true){
                            unSavedChanges = true;
                          newList.push(item);

                            // $state.reload();
                        }else {
                            isChecked = true;
                            var dataObj = {
                                Operation: '3',
                                Id: item.Id,
                                displayId: item.displayId,
                                Role: item.Role
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
                        angular.forEach($scope.approvalMatrixCollection, function (item, index) {
                            if (item.displayId == newList[i].displayId) {
                                item.checkbox = false;

                                ind = index;
                            }
                        });
                        if(ind != -1){
                            $scope.checkList.splice(newList[i].displayId,1);
                            $scope.approvalMatrixCollection.splice(ind,1);
                        }
                    }
                }
                if(listToSave.length != 0){
                    for(var i = 0; i < listToSave.length; i++) {
                        var ind = -1;
                        angular.forEach($scope.approvalMatrixCollection, function (item, index) {
                            if (item.displayId == listToSave[i].displayId) {
                                item.checkbox = false;
                                ind = index;
                            }
                        });
                        if(ind != -1){
                            $scope.checkList.splice(listToSave[i].displayId,1);
                            $scope.approvalMatrixCollection.splice(ind,1);
                        }
                    }

                }
                //if(listToSave.length != 0) {
                //    $http.post(url,listToSave).success(function(){
                //        console.log($scope.approvalMatrixCollection);
                //        $scope.originalApprovalMatrix = angular.copy($scope.approvalMatrixCollection);
                //        $state.reload();
                //    }).error(function(){
                //        alert('Failed To delete');
                //    });
                //
                //}
            }
            $scope.checkForChanges = function(){
                var unSavedChanges = false;
                var originalCollection = $scope.originalApprovalMatrix;
                var currentCollection = $scope.approvalMatrixCollection;
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
                                if(originalObject.Role !== currentObject.Role ||
                                    originalObject.Cost !== currentObject.Cost ||
                                    originalObject.Schedule !== currentObject.Schedule)
                                {
                                    // alert if a change has not been saved
                                    //alert("unsaved change on " + originalObject.Role);
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

            //$scope.updateEntity = function(row) {
            //    if (!$scope.save) {
            //        $scope.save = {
            //            promise: null,
            //            pending: false,
            //            row: null
            //        };
            //    }
            //
            //    $scope.save.row = row.rowIndex;
            //    if (!$scope.save.pending) {
            //        console.log($scope.list[$scope.save.row]);
            //        $scope.save.pending = true;
            //        $scope.save.promise = $timeout(function() {
            //            // $scope.list[$scope.save.row].$update();
            //            console.log("Here you'd save your record to the server, we're updating row: " + $scope.save.row + " to be: " +
            //            $scope.list[$scope.save.row].PositionDescription + "," + $scope.list[$scope.save.row].Check +
            //            "," + $scope.list[$scope.save.row].Cost + ","  + $scope.list[$scope.save.row].Schedule);
            //            $scope.save.pending = false;
            //        }, 500);
            //    }
            //};

        }])

.filter('customCurrency',function($filter){
        return function (input, entity) {
            var str = input.replace( /,/g, "" );
            str = str.replace(/\$/g, '');

            var return_value = $filter('currency')(str, '$',0);
          return return_value;
        };
    });