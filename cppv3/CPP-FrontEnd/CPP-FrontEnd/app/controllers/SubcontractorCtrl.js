angular.module('cpp.controllers').
    //Subcontractor Controller
    controller('SubcontractorCtrl', ['SubcontractorType', 'Subcontractor', 'UnitType', '$state', '$scope', '$rootScope', 'Category', '$uibModal', 'UpdateCategory', '$http', 'Page', 'ProjectTitle', 'TrendStatus', '$location', '$timeout', 'UniqueIdentityNumber',
        function (SubcontractorType, Subcontractor, UnitType, $state, $scope, $rootScope, Category, $uibModal, UpdateCategory, $http, Page, ProjectTitle, TrendStatus, $location, $timeout, UniqueIdentityNumber) {
            Page.setTitle('Subcontractor');
            ProjectTitle.setTitle('');
            TrendStatus.setStatus('');

            var url = serviceBasePath + 'response/Subcontractor/'; //here
            $scope.$on('ngGridEventEndCellEdit', function (data) {
                console.log(data.targetScope.row.entity.status);
                data.targetScope.row.entity.status = 'Modified';
                console.log($scope.userCollection);
            });

            $scope.applicableList = [];

            //Get all subcontractor types
            SubcontractorType.get({}, function (subcontractorTypes) {
                $scope.subcontractorTypeCollection = subcontractorTypes.result;
                console.log(subcontractorTypes.result);

                angular.forEach($scope.subcontractorTypeCollection, function (subcontractorType) {
                    subcontractorType.SubcontractorTypeName = subcontractorType.SubcontractorTypeName; //here
                });
                $scope.gridOptions.columnDefs[3].editDropdownOptionsArray = $scope.subcontractorTypeCollection;

                //Get all Subcontractors
                Subcontractor.get({}, function (subcontractorData) {
                    $scope.checkList = [];
                    $scope.subcontractorCollection = subcontractorData.result;
                    $scope.orgSubcontractorCollection = angular.copy(subcontractorData.result);
                    addIndex($scope.subcontractorCollection);
                    angular.forEach($scope.subcontractorCollection, function (item, index) {
                        item.checkbox = false;
                        $scope.checkList[index + 1] = false;

                        //Find subcontractor type for a subcontractor
                        for (var x = 0; x < $scope.subcontractorTypeCollection.length; x++) {
                            if ($scope.subcontractorTypeCollection[x].SubcontractorTypeID == item.SubcontractorTypeID) {
                                item.SubcontractorTypeName = $scope.subcontractorTypeCollection[x].SubcontractorTypeName;
                            }
                        }
                    });
                    $scope.gridOptions.data = $scope.subcontractorCollection;
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

            var isFresh = true;
            var currentPoint = "";

            $scope.addRow = function () {
                var x = Math.max.apply(Math, $scope.subcontractorCollection.map(function (o) {

                    return o.displayId;
                }))

                if (x < 0) {
                    console.log(x);
                    x = 0;
                }

                $scope.checkList[++x] = false;
                $scope.subcontractorCollection.splice(x, 0, {
                    displayId: x,
                    SubcontractorID: '',
                    SubcontractorName: '',
                    SubcontractorDescription: '',
                    SubcontractorTypeName: '',
                    SubcontractorTypeID: '',
                    UniqueIdentityNumber: '',
                    checkbox: false,
                    new: true
                });

                if (isFresh) {
                	UniqueIdentityNumber.get({
                		NumberType: 'Subcontractor',
                		'OrganizationID': 0,
                		'PhaseID': 0,
                		'CategoryID': 0
                	}, function (response) {
                        $scope.subcontractorCollection[$scope.subcontractorCollection.length - 1].UniqueIdentityNumber = response.result;
                        isFresh = false;
                        currentPoint = response.result;
                    });
                } else {
                    currentPoint = "BS" + ((parseInt(currentPoint.substr(2)) + 1)).toString().padStart(5, '0');

                    $scope.subcontractorCollection[$scope.subcontractorCollection.length - 1].UniqueIdentityNumber = currentPoint;
                }
                $scope.gridApi.core.clearAllFilters();//Nivedita-T on 17/11/2021
                $timeout(function () {
                    console.log($scope.gridOptions.data[$scope.gridOptions.data.length - 1], $scope.gridOptions.columnDefs[0]);
                    
                    $scope.gridApi.core.scrollTo($scope.gridOptions.data[$scope.gridOptions.data.length - 1], $scope.gridOptions.columnDefs[0]);
                }, 1);

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
                    cellClass: 'c-col-Num' //Manasi

                }, {
                    field: 'SubcontractorName',
                    name: 'Name*',
                    width: 250
                }, {
                    field: 'SubcontractorDescription',
                    name: 'Description',
                    width: 400
                }, {
                    field: 'SubcontractorTypeName',
                    name: 'Category*',
                    editableCellTemplate: 'ui-grid/dropdownEditor',
                    editDropdownValueLabel: 'SubcontractorTypeName', //code
                    editDropdownIdLabel: 'SubcontractorTypeName',    //phase
                    editDropDownChange: 'test',
                    cellFilter: 'mapPhase',
                    width: 400
                }, {
                    field: 'UniqueIdentityNumber',
                    name: 'Unique Identifier',
                        width: 200,
                        enableCellEdit: false,
                    //  editableCellTemplate: $scope.cellInputEditableTemplate,
                    //cellFilter :'mapStatus'


                    },
                    // Jignesh-13-09-2021 Subcontractor Markup
                    {
                        field: 'MarkUp',
                        name: 'Mark Up',
                        width: 75,
                        type: 'boolean',
                        cellClass: 'text-center',
                        cellTemplate: '<input type="checkbox" ng-model="row.entity.MarkUp" >'
                    },
                    {
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
                $("#save_Subcontractor").attr("disabled", true);
                var isReload = false;
                var isChanged = true;
                var isFilled = true;
                var listToSave = [];
                angular.forEach($scope.subcontractorCollection, function (value, key, obj) {


                    //Find subcontractor typeid for a subcontractor
                    for (var x = 0; x < $scope.subcontractorTypeCollection.length; x++) {
                        if ($scope.subcontractorTypeCollection[x].SubcontractorTypeName == value.SubcontractorTypeName) {
                            value.SubcontractorTypeID = $scope.subcontractorTypeCollection[x].SubcontractorTypeID;
                        }
                    }

                    if (
                        //value.SubcontractorDescription == "" || // Aditya 3-2-2022
                        value.SubcontractorTypeID == "" || value.SubcontractorName == "" || value.UniqueIdentityNumber == ""
                        ) {
                        dhtmlx.alert({
                            text: "Please fill data to all required fields before save (Row " + value.displayId + ")",
                            width: "300px"
                        });
                        isFilled = false;
                        return;
                    }

                    if (!(/(BS[0-9]{5})/.test(value.UniqueIdentityNumber) && value.UniqueIdentityNumber.length == 7)) {
                        dhtmlx.alert({
                            text: "Unique identifier must be in the format of BSxxxxx (Row " + value.displayId + ")",
                            width: "400px"
                        });
                        okToExit = false;
                        isFilled = false;
                        return;
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
                            SubcontractorDescription: value.SubcontractorDescription,
                            SubcontractorName: value.SubcontractorName,
                            SubcontractorTypeID: value.SubcontractorTypeID,
                            UniqueIdentityNumber: value.UniqueIdentityNumber,
                            MarkUp: value.MarkUp // Jignesh-13-09-2021 Subcontractor Markup
                        }
                        console.log(dataObj);
                        listToSave.push(dataObj);
                    }
                    else {
                        isChanged = true;
                        angular.forEach($scope.orgSubcontractorCollection, function (orgItem) {
                            if (value.SubcontractorID === orgItem.SubcontractorID &&
                                value.SubcontractorDescription === orgItem.SubcontractorDescription &&
                                value.SubcontractorName === orgItem.SubcontractorName &&
                                value.SubcontractorTypeID === orgItem.SubcontractorTypeID &&
                                value.UniqueIdentityNumber === orgItem.UniqueIdentityNumber &&
                                value.MarkUp === orgItem.MarkUp) { // Jignesh-13-09-2021 Subcontractor Markup
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
                                SubcontractorID: value.SubcontractorID,
                                SubcontractorDescription: value.SubcontractorDescription,
                                SubcontractorName: value.SubcontractorName,
                                SubcontractorTypeID: value.SubcontractorTypeID,
                                UniqueIdentityNumber: value.UniqueIdentityNumber,
                                MarkUp: value.MarkUp // Jignesh-13-09-2021 Subcontractor Markup
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
                                SubcontractorID: value.SubcontractorID,
                                SubcontractorDescription: value.SubcontractorDescription,
                                SubcontractorName: value.SubcontractorName,
                                SubcontractorTypeID: value.SubcontractorTypeID,
                                UniqueIdentityNumber: value.UniqueIdentityNumber,
                                MarkUp: value.MarkUp // Jignesh-13-09-2021 Subcontractor Markup
                            }
                            console.log(dataObj);
                        }
                        listToSave.push(dataObj);
                    }

                });

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
                        $("#save_Subcontractor").attr("disabled", false);
                        isFresh = true;

                        response.data.result.replace(/[\r]/g, '\n');

                        if (response.data.result) {
                            $("#save_Subcontractor").attr("disabled", false);
                            dhtmlx.alert(response.data.result);
                        } else {
                            $("#save_Subcontractor").attr("disabled", false);
                            dhtmlx.alert('No changes to be saved.');
                        }

                        //$state.reload();
                        Subcontractor.get({}, function (subcontractorData) {
                            $scope.checkList = [];
                            $scope.subcontractorCollection = subcontractorData.result;
                            $scope.orgSubcontractorCollection = angular.copy(subcontractorData.result);
                            addIndex($scope.subcontractorCollection);
                            angular.forEach($scope.subcontractorCollection, function (item, index) {
                                item.checkbox = false;
                                $scope.checkList[index + 1] = false;
                                //Find subcontractor type name for a subcontractor
                                for (var x = 0; x < $scope.subcontractorTypeCollection.length; x++) {
                                    if ($scope.subcontractorTypeCollection[x].SubcontractorTypeID == item.SubcontractorTypeID) {
                                        item.SubcontractorTypeName = $scope.subcontractorTypeCollection[x].SubcontractorTypeName;
                                    }
                                }
                            });
                            $scope.gridOptions.data = $scope.subcontractorCollection;

                            console.log($scope.subcontractorCollection);
                        });
                    }, function error(response) {
                        $("#save_Subcontractor").attr("disabled", false);
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
                angular.forEach($scope.subcontractorCollection, function (item) {
                    if (item.checkbox == true) {
                        selectedRow = true;
                        if (item.new === true) {
                            unSavedChanges = true;
                            newList.push(item);
                        } else {
                            isChecked = true;
                            var dataObj = {
                                Operation: '3',
                                SubcontractorID: item.SubcontractorID,
                                SubcontractorName: item.SubcontractorName,
                                SubcontractorDescription: item.SubcontractorDescription,
                                SubcontractorTypeID: item.SubcontractorTypeID,
                                UniqueIdentityNumber: item.UniqueIdentityNumber,
                                displayId: item.displayId,
                                MarkUp: item.MarkUp // Jignesh-13-09-2021 Subcontractor Markup
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

                if (newList.length != 0) {
                    for (var i = 0; i < newList.length; i++) {
                        var ind = -1;
                        angular.forEach($scope.subcontractorCollection, function (item, index) {
                            if (item.displayId == newList[i].displayId) {
                                item.checkbox = false;
                                ind = index;
                            }
                        });
                        if (ind != -1) {
                            $scope.checkList.splice(newList[i].displayId, 1);
                            $scope.subcontractorCollection.splice(ind, 1);
                        }
                    }

                }
                if (listToSave.length != 0) { }
                for (var i = 0; i < listToSave.length; i++) {
                    var ind = -1;
                    angular.forEach($scope.subcontractorCollection, function (item, index) {
                        if (item.displayId == listToSave[i].displayId) {
                            item.checkbox = false;
                            ind = index;
                        }
                    });
                    if (ind != -1) {
                        $scope.checkList.splice(listToSave[i].displayId, 1);
                        $scope.subcontractorCollection.splice(ind, 1);
                    }
                }
            }
            $scope.checkForChanges = function () {
                var unSavedChanges = false;
                var originalCollection = $scope.orgSubcontractorCollection;
                var currentCollection = $scope.subcontractorCollection;
                if (currentCollection.length != originalCollection.length) {
                    unSavedChanges = true;
                    return unSavedChanges;
                } else {
                    angular.forEach(currentCollection, function (currentObject) {
                        for (var i = 0, len = originalCollection.length; i < len; i++) {
                            if (unSavedChanges) {
                                return unSavedChanges; // no need to look through the rest of the original array
                            }
                            if (originalCollection[i].SubcontractorID == currentObject.SubcontractorID) { //subcategoryID
                                var originalObject = originalCollection[i];
                                // compare relevant data
                                if (originalObject.SubcontractorTypeID !== currentObject.SubcontractorTypeID ||
                                    originalObject.SubcontractorDescription !== currentObject.SubcontractorDescription ||
                                    originalObject.SubcontractorName !== currentObject.SubcontractorName ||
                                    originalObject.UniqueIdentityNumber !== currentObject.UniqueIdentityNumber) {
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

.filter('mapProjectType', function () {
    return function (input) {
        if (!input)
            return ""
        if (!input.Type) {

            return input;
        } else if (input.Type) {
            return input.Type;
        } else {
            return 'unknown';
        }
        return input.Type;
    }
});

function gridLoad() {
}