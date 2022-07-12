angular.module('cpp.controllers').
    controller('holidayCtrl', ['$http', 'Page', '$state', 'UpdateHoliday', '$uibModal', '$scope', '$rootScope', 'TrendStatus', '$location', '$timeout', 'Holiday','Holidaysbyyear',
        function ($http, Page, $state, UpdateHoliday, $uibModal, $scope, $rootScope, TrendStatus, $location, $timeout, Holiday, Holidaysbyyear) {
            var okToExit = true;
            Page.setTitle('Holiday');
            //ServiceTitle.setTitle('');
            TrendStatus.setStatus('');
            $scope.checkedRow = [];
            $scope.years = [1,2,3];
            $scope.yeardefault = new Date().getFullYear();
            $scope.yeardd = [];
            $scope.gridOPtions = {};
            $scope.myExternalScope = $scope;
            $scope.filteryear = $scope;
            $scope.TypeOfHoliday = [];
            $scope.TypeOfHoliday.push({ ID: "P", value: "P" });
            $scope.TypeOfHoliday.push({ ID: "T", value: "T" });
           
            $scope.startdateofyear = $scope.yeardefault + '-01-01';
            $scope.enddateofyear = $scope.yeardefault +'-12-31';
            var defaultyear = $scope.yeardefault;

            console.log('luan test');  
          
         
            $scope.numbers = [];
            $scope.startYear;
            $scope.currentYear = 2022;
                $scope.startYear = 2050;
                while ($scope.currentYear <= $scope.startYear) {
                    
                    $scope.numbers.push($scope.currentYear++);;
                }
           
            
          
          
            var url = serviceBasePath + 'Response/Holiday/';
           
            Holidaysbyyear.lookup().get({ year: $scope.yeardefault }, function (response) {
              
                if (defaultyear== "2022") {
                  
                    div = document.getElementById('chkholiday');
                    div = document.getElementById('lblholiday');
                    div.style.display = "none";
                    div.style.display = "none";
                }
                else {
                    div = document.getElementById('chkholiday');
                    div = document.getElementById('lblholiday');
                    div.style.display = "block";
                    div.style.display = "block";
                    }
                
                console.log(response)
                $scope.checkList = [];
                $scope.ProjectClassCollection = response.result;
                console.log($scope.ProjectClassCollection);
                $scope.orgProjectClassCollection = angular.copy(response.result);
                addIndex($scope.ProjectClassCollection);
                angular.forEach($scope.ProjectClassCollection, function (item, index) {
                  
                    $scope.checkList[index + 1] = false;
                    item.checkbox = false;
                });
                console.log($scope.ProjectClassCollection);
                $scope.gridOptions.data = $scope.ProjectClassCollection;
            });

          
            var addIndex = function (data) {
                var i = 1;
                angular.forEach(data, function (value, key, obj) {
                    value.displayId = i;
                    i = i + 1;
                    if (value.Schedule === "0001-01-01T00:00:00") {
                        value.Schedule = "";
                    }
                });
            }
            $scope.cellInputEditableTemplate = '<input ng-class="\'colt\' + col.index" ng-input="COL_FIELD" ng-model="COL_FIELD" ng-blur="updateEntity(row)" />';
            $scope.cellSelectEditableTemplate = '<select ng-class="\'colt\' + col.index" ng-input="COL_FIELD" ng-model="COL_FIELD" ng-options="id.PositionDescription for id in positionCollection track by id.PositionID" ng-blur="updateEntity(row)" />';
            $scope.cellCheckEditTableTemplate = '<input class="c-approval-matrix-check"  type="checkbox" ng-input="COL-FIELD" ng-model="COL_FIELD"/>';
            $scope.cellSelect = '<select ng-model="row.entity.text" data-ng-options="d as d.id for d in tempList">';
            $scope.addRow = function () {
                var x = Math.max.apply(Math, $scope.ProjectClassCollection.map(function (o) {

                    return o.displayId;
                }))

                if (x < 0) {
                    console.log(x);
                    x = 0;
                }

                $scope.checkList[++x] = false;
                $scope.ProjectClassCollection.splice(x, 0, {
                    displayId: x,
                    //Type: '',
                    Name: '',
                    checkbox: false,
                    new: true
                });
                $scope.gridApi.core.clearAllFilters();//Nivedita-T on 16/11/2021
                $timeout(function () {
                    console.log($scope.gridOptions.data[$scope.gridOptions.data.length - 1], $scope.gridOptions.columnDefs[0]);

                    $scope.gridApi.core.scrollTo($scope.gridOptions.data[$scope.gridOptions.data.length - 1], $scope.gridOptions.columnDefs[0]);
                }, 1);
            }


            $scope.gridOptions = {
                enableColumnMenus: false,
                enableCellEditOnFocus: true,
                enableFiltering: true,
                //enableRowSelection:false,
                //enableCellSelection: true,
                //selectedItems: $scope.mySelections,

                //multiSelect: false,
                rowHeight: 40,
                width: 400,
                //afterSelectionChange: function (rowItem, event) {
                //    console.log($scope.mySelections);
                //    $scope.selectedIDs = [];
                //    console.log(rowItem);
                //    angular.forEach($scope.mySelections, function ( item ) {
                //        $scope.selectedIDs.push( item.id )
                //    });
                //
                //},
                columnDefs: [
                    {
                        field: 'displayId',
                        name: 'ID',
                        enableCellEdit: false,
                        //cellClass: 'c-col-id',
                        width: 50,
                        cellClass: 'c-col-Num' //Manasi

                        //cellTemplate:'<div ng-class="c-col-id" style="margin-top:15%;" ng-click="clicked(row,col)">{{row.getProperty(col.field)}}</div>'

                    },

                  
                    {
                        field: 'HolidayDate',
                        name: 'Date Selection*',
                        width: 250,
                        height:1000,
                        enableCellEdit: true,
                        type: 'date',
                        cellFilter: 'date:"MM/dd/yyyy"',
                        cellClass: 'c-col-Date',
                        min: $scope.startdateofyear,
                        max: $scope.enddateofyear
                        
                    
                      
                    },
                    {
                        field: 'Description',
                        name: 'Description*',

                        //  editableCellTemplate: $scope.cellInputEditableTemplate,
                        //cellFilter :'mapStatus'


                    },
                    {
                        field: 'Type_Of_Holiday',
                        name: 'Type_Of_Holiday*',
                        editableCellTemplate: 'ui-grid/dropdownEditor',
                        editDropdownIdLabel: 'ID',
                        editDropdownValueLabel: 'value',
                        editDropdownOptionsArray: $scope.TypeOfHoliday,
                        cellFilter: 'customFilter:this',
                        cellClass: 'c-col-Num',
                        width: 80,
                    },

                    {
                        field: 'checkBox',
                        name: '',
                        //
                        enableCellEdit: false,
                        enableFiltering: false,
                        width: 35,
                        cellTemplate: '<input type="checkbox" ng-model="checkList[row.entity.displayId]" class = "c-col-check" ng-click="grid.appScope.check(row,col)" style="text-align: center;vertical-align: middle;">'

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

            $scope.check = function (row, col) {
                if (row.entity.checkbox == false) {
                    row.entity.checkbox = true;
                    $scope.checkList[row.entity.displayId] = true;
                    row.config.enableRowSelection = true;
                } else {
                    $scope.checkList[row.entity.displayId] = false;
                    row.entity.checkbox = false;
                }
                $scope.checkedRow.push(row);
            }

            $scope.FetchYear = function (e) {
                var selectedYear = $('#selectYr').val();
                var previousYear = selectedYear - 1;
                var sDate = new Date(selectedYear).toDateString();
                
                var listToSave = [];
                if ($("#HolidayConfirmation").prop('checked') == true) {

                    dhtmlx.confirm("Are you Sure. Want to Continue?", function (result) {
                        var dataObj = {
                            Operation: '5',
                            Description: '',
                            Type_Of_Holiday: '',
                            HolidayDate: sDate,
                            ID: ''
                        }
                        listToSave.push(dataObj);
                        
                        if (result) {
                            $http({
                        url: url,
                        method: "POST",
                        data: JSON.stringify(listToSave),
                        headers: { 'Content-Type': 'application/json' }
                    }).then(function success(response) {
                       // response.data.result.replace(/[\r]/g, '\n');

                        if (response.data.result) {
                            
                            $scope.filterYear(selectedYear);
                            if (response.data.result.length != 0) {
                                dhtmlx.alert("Holidays Fetched Successfully");
                            }
                            else {
                                dhtmlx.alert("No Permanent Holidays are available in " + previousYear);
                            }
                            $('#HolidayConfirmation').prop('checked', false);
                        } else {
                            dhtmlx.alert('No changes to be saved.');
                        }

                      
                    }, function error(response) {
                        dhtmlx.alert("Failed to save. Please contact your Administrator.");
                    });
                        }
                    else
                        {
                            $('#HolidayConfirmation').prop('checked', false);
                        }
                    });
                }
                
            }


            
           
             
            //}).on('show', function () {
            //    // remove the year from the date title before the datepicker show
            //    var dateText = $(".datepicker-days .datepicker-switch").text().split(" ");
            //    var dateTitle = dateText[0];
            //    $(".datepicker-days .datepicker-switch").text(dateTitle);
            //    $(".datepicker-months .datepicker-switch").css({ "visibility": "hidden" });
          //  });

            $scope.filterYear = function (selectyr) {
               
                var selectedYear = $('#selectYr').val();
                defaultyear = selectedYear;
                if (defaultyear == "2022") {

                    div = document.getElementById('chkholiday');
                    div = document.getElementById('lblholiday');
                    div.style.display = "none";
                    div.style.display = "none";
                }
                else {
                    div = document.getElementById('chkholiday');
                    div = document.getElementById('lblholiday');
                    div.style.display = "block";
                    div.style.display = "block";
                }
                $scope.startdateofyear = '';
                $scope.enddateofyear = '';
                Holidaysbyyear.lookup().get({ year: selectedYear }, function (response) {
                    console.log(response)
                    $scope.checkList = [];
                    $scope.ProjectClassCollection = response.result;
                    console.log($scope.ProjectClassCollection);
                    $scope.orgProjectClassCollection = angular.copy(response.result);
                    addIndex($scope.ProjectClassCollection);
                    angular.forEach($scope.ProjectClassCollection, function (item, index) {
                        $scope.checkList[index + 1] = false;
                        item.checkbox = false;
                    });
                    console.log($scope.ProjectClassCollection);
                    $scope.startdateofyear = selectedYear + '-01-01';
                    $scope.enddateofyear = selectedYear + '-12-31';
                    $scope.gridOptions.data = $scope.ProjectClassCollection;
                });
             
            }
            $scope.clicked = function (row, col) {
                console.log(row);
                $scope.orgRow = row;
                $scope.col = col;
                $scope.row = row.entity;
                console.log(col);
            }
            $scope.save = function () {
                okToExit = false;
                var isReload = false;
                var isChanged = true;
                var isFilled = true;
                var listToSave = [];
                var isInvalidList = false;
                angular.forEach($scope.ProjectClassCollection, function (value, key, obj) {
                    var HolidayDate = new Date(value.HolidayDate).toDateString();
                    if (!isInvalidList) {
                        //if (!Number.isInteger(parseFloat(value.ID))
                        //    || Number.parseFloat(value.ID) < 0) {
                        //    dhtmlx.alert('Department Type must be integer. (Row ' + value.displayId + ')');
                        //    isInvalidList = true;
                        //    return;
                        //}



                        if (
                            //value.Type == "" ||
                            value.Description == "" || value.Type_Of_Holiday == ""  || value.Description == null || value.Type_Of_Holiday == null) {
                            dhtmlx.alert({
                                text: "Please fill data to all required fields before save (Row " + value.displayId + ")",
                                width: "400px"
                            });
                            okToExit = false;
                            isFilled = false;
                            return;
                        }

                        if (isFilled == false) {
                            return;
                        }

                        //New Item
                        if (value.new === true) {
                            okToExit = false;
                            isReload = true;
                            var dataObj = {
                                Operation: '1',
                                //Type: value.Type,
                                Description: value.Description,
                                Type_Of_Holiday: value.Type_Of_Holiday,
                                HolidayDate: HolidayDate
                            }
                            listToSave.push(dataObj);
                            okToExit = true;

                        }
                        else {//Update Item if there is changes
                            okToExit = false;
                            isChanged = true;
                            angular.forEach($scope.orgProjectClassCollection, function (orgItem) {
                                if (value.ID === orgItem.ID &&
                                    //value.Type === orgItem.Type &&
                                    value.Description === orgItem.Description &&
                                    value.HolidayDate == orgItem.HolidayDate &&
                                    value.Type_Of_Holiday == orgItem.Type_Of_Holiday) {
                                    isChanged = false;
                                    okToExit = true;
                                }
                            });
                            if (isChanged == true) {
                                var dataObj = {
                                    Operation: '2',
                                    Description: value.Description,
                                    Type_Of_Holiday: value.Type_Of_Holiday,
                                    HolidayDate: HolidayDate,
                                    ID: value.ID
                                }
                            } else {
                                var dataObj = {
                                    Operation: '4',
                                    Description: value.Description,
                                    Type_Of_Holiday: value.Type_Of_Holiday,
                                    HolidayDate: HolidayDate,
                                    ID: value.ID
                                }
                            }
                            listToSave.push(dataObj)
                            okToExit = true;
                        }
                    }

                });

                if (isInvalidList) {
                    return;
                }

                angular.forEach($scope.listToDelete, function (item) {
                    listToSave.push(item);
                });
                if (isFilled == false) {
                    return;
                }
                else {
                    $http({
                        url: url,
                        //url: 'http://localhost:29986/api/response/phasecode',
                        method: "POST",
                        data: JSON.stringify(listToSave),
                        headers: { 'Content-Type': 'application/json' }
                    }).then(function success(response) {
                        response.data.result.replace(/[\r]/g, '\n');

                        if (response.data.result) {
                            dhtmlx.alert(response.data.result);
                        } else {
                            dhtmlx.alert('No changes to be saved.');
                        }

                        if (isTest == true) {
                            //   var newUrl = $location.path();
                            console.log(newUrl);
                            // onRouteChangeOff();
                            window.location.href = "#" + newUrl;

                        }
                        $scope.orgProjectClassCollection = angular.copy($scope.ProjectClassCollection);
                        okToExit = true;
                        $state.reload();

                    }, function error(response) {
                        dhtmlx.alert("Failed to save. Please contact your Administrator.");
                    });
                }
                if (isReload == true || isChanged == true) {
                    //$state.reload();
                }
            }
            $scope.delete = function () {
                //dhtmlx.alert("Deletion is not avaiable at this time.");
                //return;

                var isChecked = false;
                var unSavedChanges = false;
                var listToSave = [];
                var newList = [];
                var selectedRow = false;
                $scope.listToDelete = [];
                console.log($scope.ProjectClassCollection);
                angular.forEach($scope.ProjectClassCollection, function (item) {
                    isChecked = false;
                    if (item.checkbox == true) {
                        selectedRow = true;
                        if (item.new === true) {
                            unSavedChanges = true;
                            newList.push(item);

                        } else {
                            ischecked = true;
                            var dataObj = {
                                Operation: '3',
                                Description: item.Description,
                                HolidayDate: item.HolidayDate,
                                Type_Of_Holiday: item.Type_Of_Holiday,
                                //ProjectClassLineItemid: item.ProjectClassLineItemid,
                                ID: item.ID,
                                displayId: item.displayId
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


                if (newList.length != 0) {
                    for (var i = 0; i < newList.length; i++) {
                        var ind = -1;
                        angular.forEach($scope.ProjectClassCollection, function (item, index) {
                            if (item.displayId == newList[i].displayId) {
                                item.checkbox = false;

                                ind = index;
                            }
                        });
                        if (ind != -1) {
                            $scope.checkList.splice(newList[i].displayId, 1);
                            $scope.ProjectClassCollection.splice(ind, 1);
                        }
                    }

                }
                if (listToSave.length != 0) {

                    for (var i = 0; i < listToSave.length; i++) {
                        var ind = -1;
                        angular.forEach($scope.ProjectClassCollection, function (item, index) {
                            if (item.displayId == listToSave[i].displayId) {
                                item.checkbox = false;

                                ind = index;
                            }
                        });
                        if (ind != -1) {
                            $scope.checkList.splice(listToSave[i].displayId, 1);
                            $scope.ProjectClassCollection.splice(ind, 1);
                        }
                    }
                }

                //if(isChecked == true || unSavedChanges == true){
                //    angular.forEach(scope.SubcontractorTypeCollection,function(item){})
                //}else{
                //    alert("No row selected for delete");
                //}
                //$state.reload();        //Temporary Solution
            }
            $scope.checkForChanges = function () {
                var unSavedChanges = false;
                var originalCollection = $scope.orgProjectClassCollection;
                var currentCollection = $scope.ProjectClassCollection;
                if (currentCollection.length != originalCollection.length) {
                    unSavedChanges = true;
                    return unSavedChanges;
                } else {
                    angular.forEach(currentCollection, function (currentObject) {
                        for (var i = 0, len = originalCollection.length; i < len; i++) {
                            if (unSavedChanges) {
                                return unSavedChanges; // no need to look through the rest of the original array
                            }
                            if (originalCollection[i].ID == currentObject.ID) {
                                var originalObject = originalCollection[i];
                                // compare relevant data
                                if (originalObject.Description !== currentObject.Description ||
                                    originalObject.TypeOfHoliday !== currentObject.Type_Of_Holiday ||
                                    originalObject.HolidayDate !== currentObject.HolidayDate) {
                                    // alert if a change has not been saved
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
            var isTest = false;
            var newUrl = "";
            onRouteChangeOff = $scope.$on('$locationChangeStart', function (event) {
                newUrl = $location.url();
                // alert(newUrl)
                if (!$scope.checkForChanges()) return;
                $scope.confirm = "";
                var scope = $rootScope.$new();
                scope.params = { confirm: $scope.confirm };
                $rootScope.modalInstance = $uibModal.open({
                    scope: scope,
                    templateUrl: 'app/views/Modal/exit_confirmation_modal.html',
                    controller: 'exitConfirmation',
                    size: 'md',
                    backdrop: true
                });
                $rootScope.modalInstance.result.then(function (data) {
                    console.log(scope.params.confirm);
                    if (scope.params.confirm === "exit") {
                        onRouteChangeOff();
                        //alert(newUrl);
                        $location.path(newUrl);
                    }
                    else if (scope.params.confirm === "save") {
                        isTest = true;
                        $scope.save();
                        //alert("");
                        //if(okToExit){
                        //    onRouteChangeOff();
                        //    $location.path(newUrl);
                        //}
                        //onRouteChangeOff();
                        //$location.path(newUrl);
                    }
                    else if (scope.params.confirm === "back") {
                        //do nothing
                    }
                });
                event.preventDefault();
            });

        }]);


