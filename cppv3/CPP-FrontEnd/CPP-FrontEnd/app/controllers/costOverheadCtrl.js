angular.module('cpp.controllers').
    //Cost Overhead Controller
    controller('CostOverheadCtrl', ['CostType', 'CostRateType', 'CostTypeRateType', 'CostOverhead', '$state', '$scope', '$rootScope', 'Category', '$uibModal', 'UpdateCategory', '$http', 'Page', 'ProjectTitle', 'TrendStatus', '$location', '$timeout',
        function (CostType, CostRateType, CostTypeRateType, CostOverhead, $state, $scope, $rootScope, Category, $uibModal, UpdateCategory, $http, Page, ProjectTitle, TrendStatus, $location, $timeout) {
            Page.setTitle('Cost Overhead');
            ProjectTitle.setTitle('');
            TrendStatus.setStatus('');

            $scope.$watch('costTypeCollection', function () {

                console.log($scope.costTypeCollection);
            });

            var url = serviceBasePath + 'response/CostOverhead/'; //here
            $scope.$on('ngGridEventEndCellEdit', function (data) {
                console.log(data.targetScope.row.entity.status);
                data.targetScope.row.entity.status = 'Modified';
            });

            $scope.applicableList = [];

            $scope.costTypeFilters = [];

            //Get all cost types
            CostType.get({}, function (costTypeData) {
                $scope.costTypeCollection = costTypeData.result;
                $scope.gridOptions.columnDefs[1].editDropdownOptionsArray = $scope.costTypeCollection;
                console.log(costTypeData.result);
                

                for (var x = 0; x < $scope.costTypeCollection.length; x++) {
                    var temp = {};
                    temp.value = $scope.costTypeCollection[x].Type;
                    temp.id = $scope.costTypeCollection[x].ID;
                    $scope.costTypeFilters.push(temp);
                }

                console.log($scope.costTypeCollection);

                //Get all cost rate types
                CostRateType.get({}, function (costRateTypeData) {
                    $scope.costRateTypeCollection = costRateTypeData.result;
                    $scope.gridOptions.columnDefs[2].editDropdownOptionsArray = $scope.costRateTypeCollection;
                    console.log(costRateTypeData.result);

                    //Get all cost type rate type
                    CostTypeRateType.get({}, function (costTypeRateTypeData) {
                        $scope.costTypeRateTypeCollection = costTypeRateTypeData.result;
                        console.log(costTypeRateTypeData.result);

                        //Get all cost overheads
                        CostOverhead.get({}, function (costOverheadData) {
                            $scope.checkList = [];
                            $scope.costOverheadCollection = costOverheadData.result;
                            console.log(costOverheadData.result);
                            $scope.orgCostOverheadCollection = angular.copy(costOverheadData.result);
                            addIndex($scope.costOverheadCollection);
                            angular.forEach($scope.costOverheadCollection, function (item, index) {
                                item.checkbox = false;
                                $scope.checkList[index + 1] = false;

                                //item.StartDate = '09/09/2014 07:32:11 AM';

                                //Find cost type for a costoverhead
                                for (var x = 0; x < $scope.costTypeCollection.length; x++) {
                                    if ($scope.costTypeCollection[x].ID == item.CostTypeID) {
                                        item.CostTypeName = $scope.costTypeCollection[x].Type;
                                    }
                                }

                                //Find cost rate type for a costoverhead
                                for (var x = 0; x < $scope.costRateTypeCollection.length; x++) {
                                    if ($scope.costRateTypeCollection[x].ID == item.CostRateTypeID) {
                                        item.CostRateTypeName = $scope.costRateTypeCollection[x].RateType;
                                    }
                                }
                            });
                            $scope.gridOptions.data = $scope.costOverheadCollection;
                        });
                    });
                });
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

            $scope.addRow = function () {
                var x = Math.max.apply(Math, $scope.costOverheadCollection.map(function (o) {

                    return o.displayId;
                }))

                if (x < 0) {
                    console.log(x);
                    x = 0;
                }

                $scope.checkList[++x] = false;
                $scope.costOverheadCollection.splice(x, 0, {
                    displayId: x,
                    ID: '',
                    CostTypeID: '',
                    CostTypeName: '',
                    CostRateTypeID: '',
                    CostRateTypeName: '',
                    Markup: '',
                    Description: '',
                    StartDate: '',
                    EndDate: '',
                    checkbox: false,
                    new: true
                });
                $scope.gridApi.core.clearAllFilters();//Nivedita-T on 17/11/2021
                $timeout(function () {
                    console.log($scope.gridOptions.data[$scope.gridOptions.data.length - 1], $scope.gridOptions.columnDefs[0]);
                    
                    $scope.gridApi.core.scrollTo($scope.gridOptions.data[$scope.gridOptions.data.length - 1], $scope.gridOptions.columnDefs[0]);
                }, 1);

            }

            $scope.changeCostType = function () {
                console.log('test');
            }

            $scope.gridOptions = {
                enableColumnMenus: false,
                enableCellEditOnFocus: true,
                enableFiltering: true,
                rowHeight: 40,
                width: 1200,
                columnDefs: [{
                    field: 'displayId',
                    name: 'ID',
                    enableCellEdit: false,
                    width: 50,
                    cellClass: 'c-col-Num'  //Manasi 
                }, {
                    field: 'CostTypeName',
                    name: 'Cost Type*',
                    editableCellTemplate: 'ui-grid/dropdownEditor',
                    editDropdownValueLabel: 'Type', //code
                    editDropdownIdLabel: 'Type',    //phase
                    editDropDownChange: 'changeCostType',
                    //cellFilter: 'mapCostType',
                        width: 200,

                    //filterHeaderTemplate: '<div class="ui-grid-filter-container" ng-repeat="colFilter in col.filters"><div my-custom-dropdown></div></div>',
                    //filter: {
                    //    //term: 1,
                    //    //options: [{ id: 1, value: 'male' }, { id: 2, value: 'female' }]     // custom attribute that goes with custom directive above 
                    //    options: $scope.costTypeFilters     // custom attribute that goes with custom directive above
                    //},
                    //cellFilter: 'mapCostType'
                    //allowCellFocus: true,
                    //enableFocusedCellEdit: true,
                    //enableCellEdit: true,
                    //enableCellEditOnFocus: true,
                }, {
                    field: 'CostRateTypeName',
                    name: 'Rate Type*',
                    editableCellTemplate: 'ui-grid/dropdownEditor',
                    editDropdownValueLabel: 'RateType', //code
                    editDropdownIdLabel: 'RateType',    //phase
                    editDropDownChange: 'test',
                    //cellFilter: 'mapPhase',
                    width: 200
                }, {
                    field: 'Markup',
                    name: 'Mark up*',
                    //allowCellFocus: true,
                    //enableCellEdit: true,
                    //enableFocusedCellEdit: true,
                        width: 100,
                        cellClass: 'c-col-Num'  //Manasi 
                }, {
                    name: 'StartDate',
                    displayName: 'Start Date*',
                    type: 'date',
                        //cellFilter: 'date:"yyyy-MM-dd"',
                        cellFilter: 'date:"MM/dd/yyyy"',   //Jignesh 23-09-2020
                        cellClass: 'c-col-Date' //Manasi
                }, {
                    name: 'EndDate',
                    displayName: 'End Date*',
                    type: 'date',
                        //cellFilter: 'date:"yyyy-MM-dd"',
                        cellFilter: 'date:"MM/dd/yyyy"',   //Jignesh 23-09-2020
                        cellClass: 'c-col-Date' //Manasi
                }, {
                    field: 'checkBox',
                    name: '',
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

                //$scope.gridApi.edit.on.cancelCellEdit($scope, function (rowEntity, colDef) {
                //    alert('cancel');
                //});

                //$scope.gridApi.edit.on.afterCellEdit($scope, function (rowEntity, colDef) {
                //    console.log(rowEntity, colDef);
                //    $('div.ui-grid-cell').blur();
                //    $('div.ui-grid-cell').find('.ui-grid-cell-contents').blur();
                //    $('div.ui-grid-cell').find('.ui-grid-cell-contents').removeClass('ui-grid-cell-focus');
                //    //alert('after');
                //});

                //gridApi.cellNav.on.navigate($scope, function (newRowCol, oldRowCol) {
                //    console.log('navigate' , newRowCol, oldRowCol);
                //});
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
            }

            $scope.applicableCheck = function (row, col) {
                if (row.entity.IsApplicable == false) {
                    row.entity.IsApplicable = true;
                    $scope.applicableList[row.entity.displayId] = true;
                    row.config.enableRowSelection = true;
                } else {
                    $scope.applicableList[row.entity.displayId] = false;
                    row.entity.IsApplicable = false;
                }
            }
            $scope.clicked = function (row, col) {
                $scope.orgRow = row;
                $scope.col = col;
                $scope.row = row.entity;
            }
            $scope.save = function () {
                $("#save_Cost_Overhead").attr("disabled", true);
                var isReload = false;
                var isChanged = true;
                var isFilled = true;
                var listToSave = [];
                var isInvalidList = false;
                angular.forEach($scope.costOverheadCollection, function (value, key, obj) {
                    if (!isInvalidList) {
                        console.log(isNaN(value.Markup), Number.parseFloat(value.Markup) <= 0);
                        //if (isNaN(value.Markup) || Number.parseFloat(value.Markup) <= 0) {
                        if (isNaN(value.Markup) || Number.parseFloat(value.Markup) < 1) {      //Manasi 23-07-2020
                            $("#save_Cost_Overhead").attr("disabled", false);
                            dhtmlx.alert('Markup must be a valid number greater than or equal 1 (Row ' + value.displayId + ')');
                            isInvalidList = true;
                            return;
                        } else if (value.StartDate > value.EndDate) {
                            $("#save_Cost_Overhead").attr("disabled", false);
                            dhtmlx.alert('Start date must come before end date (Row ' + value.displayId +')');
                            isInvalidList = true;
                            return;
                        }
                        if (value.CostTypeName == "" || value.CostRateTypeName == "" || value.Markup == "" || value.StartDate == "" || value.EndDate == "") {
                            $("#save_Cost_Overhead").attr("disabled", false);
                            dhtmlx.alert({
                                text: "Please fill data to all required fields before save (Row " + value.displayId + ")",
                                width: "300px"
                            });
                            isFilled = false;
                            return;
                        }
                        console.log(value);
                        console.log(value.StartDate >= value.EndDate);

                        //Find cost type id for a costoverhead
                        for (var x = 0; x < $scope.costTypeCollection.length; x++) {
                            if ($scope.costTypeCollection[x].Type == value.CostTypeName) {
                                value.CostTypeID = $scope.costTypeCollection[x].ID;
                            }
                        }

                        //Find cost rate type id for a costoverhead
                        for (var x = 0; x < $scope.costRateTypeCollection.length; x++) {
                            if ($scope.costRateTypeCollection[x].RateType == value.CostRateTypeName) {
                                value.CostRateTypeID = $scope.costRateTypeCollection[x].ID;
                            }
                        }

                        console.log(value);
                        if (isFilled == false) {
                            return;
                        }
                        //New Item
                        if (value.new === true) {
                            console.log(value);

                            isReload = true;
                            console.log("#selectOrg");
                            console.log($("#selectOrg").val() === "0");
                            var dataObj = {
                                Operation: '1',
                                //ID: value.ID,
                                CostTypeID: value.CostTypeID,
                                CostRateTypeID: value.CostRateTypeID,
                                Markup: value.Markup,
                                StartDate: value.StartDate,
                                EndDate: value.EndDate
                            }
                            console.log(dataObj);
                            listToSave.push(dataObj);
                        }
                        else {
                            isChanged = true;
                            angular.forEach($scope.orgCostOverheadCollection, function (orgItem) {
                                if (value.CostTypeID === orgItem.CostTypeID &&
                                    value.CostRateTypeID === orgItem.CostRateTypeID &&
                                    value.Markup === orgItem.Markup &&
                                    value.StartDate === orgItem.StartDate &&
                                    value.EndDate === orgItem.EndDate) {
                                    //Do nothing on unchanged Item
                                    isChanged = false;
                                }
                            });
                            console.log(isChanged);
                            if (isChanged == true) {
                                isReload = true;
                                var temp = '';
                                //if (typeof value.Phase == 'string') {
                                //    temp = value.Phase;
                                //} else {
                                //    temp = value.Phase.Code;
                                //}
                                var dataObj = {
                                    Operation: '2',
                                    ID: value.ID,
                                    CostTypeID: value.CostTypeID,
                                    CostRateTypeID: value.CostRateTypeID,
                                    Markup: value.Markup,
                                    StartDate: value.StartDate,
                                    EndDate: value.EndDate
                                }
                                console.log(dataObj);

                            } else {
                                isReload = true;
                                var temp = '';
                                //if (typeof value.Phase == 'string') {
                                //    temp = value.Phase;
                                //} else {
                                //    temp = value.Phase.Code;
                                //}
                                var dataObj = {
                                    Operation: '4',
                                    ID: value.ID,
                                    CostTypeID: value.CostTypeID,
                                    CostRateTypeID: value.CostRateTypeID,
                                    Markup: value.Markup,
                                    StartDate: value.StartDate,
                                    EndDate: value.EndDate
                                }
                                console.log(dataObj);
                            }
                            listToSave.push(dataObj);
                        }
                    }
                });

                if (isInvalidList) {
                    return;
                }

                angular.forEach($scope.listToDelete, function (item) {
                    listToSave.push(item);
                });
                console.log(listToSave);
                if (isFilled == false) {
                    return;
                } else {
                    $http({
                        url: url,
                        method: "POST",
                        data: JSON.stringify(listToSave),
                        headers: { 'Content-Type': 'application/json' }
                    }).then(function success(response) {
                        $("#save_Cost_Overhead").attr("disabled", false);
                        response.data.result.replace(/[\r]/g, '\n');

                        if (response.data.result) {
                            $("#save_Cost_Overhead").attr("disabled", false);
                            dhtmlx.alert(response.data.result);
                        } else {
                            $("#save_Cost_Overhead").attr("disabled", false);
                            dhtmlx.alert('No changes to be saved.');
                        }

                        //$state.reload();
                        //Get all cost overheads
                        CostOverhead.get({}, function (costOverheadData) {
                            $scope.checkList = [];
                            $scope.costOverheadCollection = costOverheadData.result;
                            console.log(costOverheadData.result);
                            $scope.orgCostOverheadCollection = angular.copy(costOverheadData.result);
                            addIndex($scope.costOverheadCollection);
                            angular.forEach($scope.costOverheadCollection, function (item, index) {
                                item.checkbox = false;
                                $scope.checkList[index + 1] = false;

                                //Find cost type for a costoverhead
                                for (var x = 0; x < $scope.costTypeCollection.length; x++) {
                                    if ($scope.costTypeCollection[x].ID == item.CostTypeID) {
                                        item.CostTypeName = $scope.costTypeCollection[x].Type;
                                    }
                                }

                                //Find cost rate type for a costoverhead
                                for (var x = 0; x < $scope.costRateTypeCollection.length; x++) {
                                    if ($scope.costRateTypeCollection[x].ID == item.CostRateTypeID) {
                                        item.CostRateTypeName = $scope.costRateTypeCollection[x].RateType;
                                    }
                                }

                                $scope.gridOptions.data = $scope.costOverheadCollection;
                            });
                        });

                    }, function error(response) {
                        $("#save_Cost_Overhead").attr("disabled", false);
                        dhtmlx.alert("Failed to save. Please contact your Administrator.");
                    });
                    //if (isReload == true)
                    //    $state.reload();
                    //else
                    //    alert("No changes to save");
                }
            }
            $scope.delete = function () {
                var isChecked = false;
                var unSavedChanges = false;
                var listToSave = [];
                var selectedRow = false;
                $scope.listToDelete = [];
                var newList = [];
                console.log($scope.costOverheadCollection);
                angular.forEach($scope.costOverheadCollection, function (item) {
                    if (item.checkbox == true) {
                        selectedRow = true;
                        if (item.new === true) {
                            unSavedChanges = true;
                            newList.push(item);
                        } else {
                            isChecked = true;
                            var dataObj = {
                                Operation: '3',
                                ID: item.ID,
                                CostTypeID: item.CostTypeID,
                                CostRateTypeID: item.CostRateTypeID,
                                Markup: item.Markup,
                                StartDate: item.StartDate,
                                EndDate: item.EndDate,
                                displayId: item.displayId
                            }
                            listToSave.push(dataObj);
                            $scope.listToDelete.push(dataObj);
                            //dhtmlx.alert("Record Deleted.");P
                        }
                    }
                });

                if (!selectedRow) {
                    dhtmlx.alert("Please select a record to delete.");
                }

                console.log(newList, $scope.listToDelete);
                if (newList.length != 0) {
                    for (var i = 0; i < newList.length; i++) {
                        var ind = -1;
                        angular.forEach($scope.costOverheadCollection, function (item, index) {
                            if (item.displayId == newList[i].displayId) {
                                item.checkbox = false;
                                ind = index;
                            }
                        });
                        if (ind != -1) {
                            $scope.checkList.splice(newList[i].displayId, 1);
                            $scope.costOverheadCollection.splice(ind, 1);
                        }
                    }

                }
                if (listToSave.length != 0) { }
                for (var i = 0; i < listToSave.length; i++) {
                    var ind = -1;
                    angular.forEach($scope.costOverheadCollection, function (item, index) {
                        if (item.displayId == listToSave[i].displayId) {
                            item.checkbox = false;
                            ind = index;
                        }
                    });
                    if (ind != -1) {
                        $scope.checkList.splice(listToSave[i].displayId, 1);
                        $scope.costOverheadCollection.splice(ind, 1);
                    }
                }
            }
            $scope.checkForChanges = function () {
                var unSavedChanges = false;
                var originalCollection = $scope.orgCostOverheadCollection;
                var currentCollection = $scope.costOverheadCollection;
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
                                if (originalObject.CostTypeID !== currentObject.CostTypeID ||
                                    originalObject.CostRateTypeID !== currentObject.CostRateTypeID ||
                                    originalObject.Markup !== currentObject.Markup ||
                                    originalObject.StartDate !== currentObject.StartDate ||
                                    originalObject.EndDate !== currentObject.EndDate) {
                                    // alert if a change has not been save
                                    //alert("unsaved change on " + originalObject.CategoryDescription + " - " + originalObject.SubCategoryDescription);
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
            onRouteChangeOff = $scope.$on('$locationChangeStart', function (event) {
                var newUrl = $location.path();
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
                        $location.path(newUrl);
                    }
                    else if (scope.params.confirm === "save") {
                        isTest = true;
                        $scope.save();
                    //    onRouteChangeOff();
                    //    $location.path(newUrl);
                    }
                    else if (scope.params.confirm === "back") {
                        //do nothing
                    }
                });
                event.preventDefault();
            });

        }])

.filter('textDate', ['$filter', function ($filter) {
    return function (input, format) {
        var date = new Date(input);
        return $filter('date')(date, format);
    };
}]).directive('uiGridEditDatepicker', ['$timeout', '$document', 'uiGridConstants', 'uiGridEditConstants', function ($timeout, $document, uiGridConstants, uiGridEditConstants) {
    return {
        template: function (element, attrs) {
            var html = '<div class="datepicker-wrapper" ><input type="text" uib-datepicker-popup datepicker-append-to-body="true" is-open="isOpen" ng-model="datePickerValue" ng-change="changeDate($event)" popup-placement="auto top"/></div>';
            return html;
        },
        require: ['?^uiGrid', '?^uiGridRenderContainer'],
        scope: true,
        compile: function () {
            return {
                pre: function ($scope, $elm, $attrs) {

                },
                post: function ($scope, $elm, $attrs, controllers) {

                    $scope.datePickerValue = new Date($scope.row.entity[$scope.col.field]);
                    $scope.isOpen = true;
                    var uiGridCtrl = controllers[0];
                    var renderContainerCtrl = controllers[1];

                    var onWindowClick = function (evt) {
                        var classNamed = angular.element(evt.target).attr('class');
                        if (classNamed) {
                            var inDatepicker = (classNamed.indexOf('datepicker-calendar') > -1);
                            if (!inDatepicker && evt.target.nodeName !== "INPUT") {
                                $scope.stopEdit(evt);
                            }
                        }
                        else {
                            $scope.stopEdit(evt);
                        }
                    };

                    var onCellClick = function (evt) {
                        angular.element(document.querySelectorAll('.ui-grid-cell-contents')).off('click', onCellClick);
                        $scope.stopEdit(evt);
                    };

                    $scope.changeDate = function (evt) {
                        $scope.row.entity[$scope.col.field] = $scope.datePickerValue;
                        $scope.stopEdit(evt);
                    };

                    $scope.$on(uiGridEditConstants.events.BEGIN_CELL_EDIT, function () {
                        if (uiGridCtrl.grid.api.cellNav) {
                            uiGridCtrl.grid.api.cellNav.on.navigate($scope, function (newRowCol, oldRowCol) {
                                $scope.stopEdit();
                            });
                        } else {
                            angular.element(document.querySelectorAll('.ui-grid-cell-contents')).on('click', onCellClick);
                        }
                        angular.element(window).on('click', onWindowClick);
                    });

                    $scope.$on('$destroy', function () {
                        angular.element(window).off('click', onWindowClick);
                        //$('body > .dropdown-menu, body > div > .dropdown-menu').remove();
                    });

                    $scope.stopEdit = function (evt) {
                        $scope.$emit(uiGridEditConstants.events.END_CELL_EDIT);
                    };

                    $elm.on('keydown', function (evt) {
                        switch (evt.keyCode) {
                            case uiGridConstants.keymap.ESC:
                                evt.stopPropagation();
                                $scope.$emit(uiGridEditConstants.events.CANCEL_CELL_EDIT);
                                break;
                        }
                        if (uiGridCtrl && uiGridCtrl.grid.api.cellNav) {
                            evt.uiGridTargetRenderContainerId = renderContainerCtrl.containerId;
                            if (uiGridCtrl.cellNav.handleKeyDown(evt) !== null) {
                                $scope.stopEdit(evt);
                            }
                        } else {
                            switch (evt.keyCode) {
                                case uiGridConstants.keymap.ENTER:
                                case uiGridConstants.keymap.TAB:
                                    evt.stopPropagation();
                                    evt.preventDefault();
                                    $scope.stopEdit(evt);
                                    break;
                            }
                        }
                        return true;
                    });
                }
            };
        }
    };
}]).filter('mapCostType', function () {
    console.log('here');
    var genderHash = {
        1: 'male',
        2: 'female'
    };

    return function (input) {
        console.log(input);
        if (!input) {
            console.log(input);
            return '';
        } else {
            return input;   //luan here
            return genderHash[input];
        }
    };
}).directive('myCustomDropdown', function () {
    return {
        template: '<select style="height: 20px; padding: 0;" class="form-control" ng-model="colFilter.term" ng-options="option.id as option.value for option in colFilter.options"></select>'
    };
});

function gridLoad() {
}