angular.module('cpp.controllers').
    //Project Class Phase Controller
    //project class       <--> material category
    //phase code          <--> unit type
    //project class phase <--> material
    controller('ProjectClassPhaseCtrl', ['ProjectClass', 'ProjectClassPhase', 'PhaseCode', '$state', '$scope', '$rootScope', 'Category', '$uibModal', 'UpdateCategory', '$http', 'Page', 'ProjectTitle', 'TrendStatus', '$location', '$timeout',
        function (ProjectClass, ProjectClassPhase, PhaseCode, $state, $scope, $rootScope, Category, $uibModal, UpdateCategory, $http, Page, ProjectTitle, TrendStatus, $location, $timeout) {
            Page.setTitle('Department to Phase Mapping');
            ProjectTitle.setTitle('');
            TrendStatus.setStatus('');



            var url = serviceBasePath + 'response/ProjectClassPhase/';
            $scope.$on('ngGridEventEndCellEdit', function (data) {
                console.log(data.targetScope.row.entity.status);
                data.targetScope.row.entity.status = 'Modified';
                console.log($scope.userCollection);
            });

            $scope.applicableList = [];

            //Get all project class
            ProjectClass.get({}, function (projectClasses) {
                $scope.projectClassCollection = projectClasses.result;
                console.log(projectClasses.result);

                angular.forEach($scope.projectClassCollection, function (projectClass) {
                    projectClass.ProjectClassName = projectClass.ProjectClassName; //here
                });
                $scope.gridOptions.columnDefs[1].editDropdownOptionsArray = $scope.projectClassCollection;

                //Get all phase codes
                PhaseCode.get({}, function (phaseCodeData) {
                    $scope.phaseCodeCollection = phaseCodeData.result;

                    angular.forEach($scope.phaseCodeCollection, function (phaseCode) {
                        phaseCode.PhaseCodeName = phaseCode.PhaseDescription + "  - " + phaseCode.Code; //here
                        phaseCode.PhaseDescription = phaseCode.PhaseDescription + "  - " + phaseCode.Code; //here
                    });
                    $scope.gridOptions.columnDefs[2].editDropdownOptionsArray = $scope.phaseCodeCollection;

                    //Get Project class phase mappings
                    ProjectClassPhase.get({}, function (projectClassPhaseData) {

                        console.log($scope.projectClassCollection);
                        console.log($scope.phaseCodeCollection);
                        console.log($scope.projectClassPhaseCollection);
                        console.log($scope.gridOptions.columnDefs);

                        $scope.checkList = [];
                        $scope.projectClassPhaseCollection = projectClassPhaseData.result;
                        //$scope.orgProjectClassPhaseCollection = angular.copy(projectClassPhaseData.result);  // Manasi 15-07-2020
                        addIndex($scope.projectClassPhaseCollection);
                        angular.forEach($scope.projectClassPhaseCollection, function (item, index) {
                            item.checkbox = false;
                            $scope.checkList[index + 1] = false;

                            //Find project class name for a project class phase mapping
                            for (var x = 0; x < $scope.projectClassCollection.length; x++) {
                                if ($scope.projectClassCollection[x].ProjectClassID == item.ProjectClassID) {
                                    item.ProjectClassName = $scope.projectClassCollection[x].ProjectClassName;
                                }
                            }

                            //Find phase code for a project class phase mapping
                            for (var x = 0; x < $scope.phaseCodeCollection.length; x++) {
                                if ($scope.phaseCodeCollection[x].PhaseID == item.PhaseID) {
                                   // item.PhaseCodeName = $scope.phaseCodeCollection[x].PhaseDescription + ' - ' + $scope.phaseCodeCollection[x].PhaseCodeName ;
                                    item.PhaseCodeName =  $scope.phaseCodeCollection[x].PhaseCodeName;
                                }
                            }
                        });
                        $scope.gridOptions.data = $scope.projectClassPhaseCollection;
                        $scope.orgProjectClassPhaseCollection = angular.copy($scope.projectClassPhaseCollection);   //Manasi 15-07-2020
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
                var x = Math.max.apply(Math, $scope.projectClassPhaseCollection.map(function (o) {

                    return o.displayId;
                }))

                if (x < 0) {
                    console.log(x);
                    x = 0;
                }

                $scope.checkList[++x] = false;
                $scope.projectClassPhaseCollection.splice(x, 0, {
                    displayId: x,
                    ID: '',
                    ProjectClassID: '',
                    PhaseID: '',
                    ProjectClassName: '',
                    PhaseCodeName: '',
                    Order: '',
                    checkbox: false,
                    new: true
                });
                $scope.gridApi.core.clearAllFilters();//Nivedita-T on 17/11/2021
                $timeout(function () {
                    console.log($scope.gridOptions.data[$scope.gridOptions.data.length - 1], $scope.gridOptions.columnDefs[0]);
                    
                    $scope.gridApi.core.scrollTo($scope.gridOptions.data[$scope.gridOptions.data.length - 1], $scope.gridOptions.columnDefs[0]);
                }, 1);

            }

            $scope.$watch('projectClassPhaseCollection', function (newvalue, oldvalue) {
                console.log($scope.projectClassPhaseCollection);
                console.log(newvalue, oldvalue);

                //if ($scope.projectClassPhaseCollection.length == oldvalue.length) {
                //    for (var x = 0; x < $scope.projectClassPhaseCollection.length; x++) {
  
                //    }
                //}
            }, true);

            $scope.gridOptions = {
                enableColumnMenus: false,
                enableCellEditOnFocus: true,
                enableFiltering: true,
                rowHeight: 40,
                width: 1200,
                columnDefs: [{
                    field: 'displayId',
                    name: '#',
                    enableCellEdit: false,
                    width: 50,
                    cellClass: 'c-col-Num' //Manasi

                }, {
                    field: 'ProjectClassName',
                    name: 'Department',
                    editableCellTemplate: 'ui-grid/dropdownEditor',
                    editDropdownValueLabel: 'ProjectClassName', //code
                    editDropdownIdLabel: 'ProjectClassName',    //phase
                    editDropDownChange: 'test',
                    cellFilter: 'mapPhase',
                    width: 300
                }, {
                    field: 'PhaseCodeName',
                    name: 'Phase',
                    editableCellTemplate: 'ui-grid/dropdownEditor',
                    editDropdownValueLabel: 'PhaseCodeName', //code
                    editDropdownIdLabel: 'PhaseCodeName',    //phase
                    editDropDownChange: 'test',
                    cellFilter: 'mapPhase',
                    width: 400
                }, {
                    field: 'Order',
                    name: 'Order',
                        width: 75,
                        cellClass: 'c-col-Num' //Manasi

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
                var isReload = false;
                var isChanged = true;
                var isFilled = true;
                var listToSave = [];
                //angular.forEach($scope.projectClassPhaseCollection, function (value, key, obj) {
                for (var i = 0; i < $scope.projectClassPhaseCollection.length; i++) {
                   
                        ////Find project class id for a project class phase mapping

                        for (var x = 0; x < $scope.projectClassCollection.length; x++) {
                            if ($scope.projectClassCollection[x].ProjectClassName == $scope.projectClassPhaseCollection[i].ProjectClassName) {
                                $scope.projectClassPhaseCollection[i].ProjectClassID = $scope.projectClassCollection[x].ProjectClassID;
                            }
                        }

                        //Find phase id for a project class phase mapping
                        //console.log($scope.phaseCodeCollection, value);
                        for (var x = 0; x < $scope.phaseCodeCollection.length; x++) {
                            if ($scope.phaseCodeCollection[x].PhaseDescription == $scope.projectClassPhaseCollection[i].PhaseCodeName) {
                                $scope.projectClassPhaseCollection[i].PhaseID = $scope.phaseCodeCollection[x].PhaseID;
                                //$scope.orgProjectClassPhaseCollection[i].PhaseID = $scope.phaseCodeCollection[x].PhaseID;
                            }
                        }

                        if (!Number.isInteger(parseFloat($scope.projectClassPhaseCollection[i].Order))
                            || Number.parseFloat($scope.projectClassPhaseCollection[i].Order) < 1
                            || $scope.projectClassPhaseCollection[i].Order.length < 1) {
                            dhtmlx.alert('Order must be a valid integer greater than 0. (Row ' + $scope.projectClassPhaseCollection[i].displayId + ')');
                            isFilled = false;
                            return;
                        }

                        if ($scope.projectClassPhaseCollection[i].ProjectClassName == "" ||
                            $scope.projectClassPhaseCollection[i].ProjectClassID == "" ||
                            $scope.projectClassPhaseCollection[i].PhaseCodeName == "" ||
                            $scope.projectClassPhaseCollection[i].PhaseID == "" ||
                            $scope.projectClassPhaseCollection[i].Order == "") {
                            dhtmlx.alert({
                                text: "Please fill data to all required fields before save (Row " + $scope.projectClassPhaseCollection[i].displayId + ")",
                                width: "300px"
                            });
                            isFilled = false;
                            return;
                        }

                        //console.log(value);
                        if (isFilled == false) {
                            return;
                        }
                        //New Item
                        if ($scope.projectClassPhaseCollection[i].new === true) {
                        //console.log(value);
                        isReload = true;
                        console.log("#selectOrg");
                        console.log($("#selectOrg").val() === "0");
                        var dataObj = {
                            Operation: '1',
                            //ID: value.ID,
                            //Description: value.Description,
                            //Name: value.Name,
                            ProjectClassID: $scope.projectClassPhaseCollection[i].ProjectClassID,
                            PhaseID: $scope.projectClassPhaseCollection[i].PhaseID,
                            Order: $scope.projectClassPhaseCollection[i].Order
                        }
                        console.log(dataObj);
                        listToSave.push(dataObj);
                    }
                    else {
                        isChanged = false;
                        //angular.forEach($scope.orgProjectClassPhaseCollection, function (orgItem) {
                        for (var j = i; j < $scope.orgProjectClassPhaseCollection.length; j++) {
                            if ($scope.projectClassPhaseCollection[i].ID === $scope.orgProjectClassPhaseCollection[j].ID &&
                                $scope.projectClassPhaseCollection[i].ProjectClassID === $scope.orgProjectClassPhaseCollection[j].ProjectClassID &&
                                $scope.projectClassPhaseCollection[i].PhaseID === $scope.orgProjectClassPhaseCollection[j].PhaseID &&
                                $scope.projectClassPhaseCollection[i].Order === $scope.orgProjectClassPhaseCollection[j].Order) {
                                //Do nothing on unchanged Item
                                isChanged = false;
                                break;
                            }
                            else {
                                isChanged = true;
                                break;
                            }
                        }
                        //});
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
                                ID: $scope.projectClassPhaseCollection[i].ID,
                                ProjectClassID: $scope.projectClassPhaseCollection[i].ProjectClassID,
                                PhaseID: $scope.projectClassPhaseCollection[i].PhaseID,
                                Order: $scope.projectClassPhaseCollection[i].Order
                            }
                            console.log(dataObj);
                            listToSave.push(dataObj); ///Manasi 14 - 07 - 2020
                        }
                        //else {
                        //    isReload = true;
                        //    var temp = '';
                        //    //if (typeof value.Phase == 'string') {
                        //    //    temp = value.Phase;
                        //    //} else {
                        //    //    temp = value.Phase.Code;
                        //    //}
                        //    var dataObj = {
                        //        Operation: '4',
                        //        ID: value.ID,
                        //        ProjectClassID: value.ProjectClassID,
                        //        PhaseID: value.PhaseID,
                        //        Order: value.Order
                        //    }
                        //    console.log(dataObj);
                        //}
                        //listToSave.push(dataObj);   //Manasi 14-07-2020
                    }
                }
                //});

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
                        console.log(response);
                        response.data.result.replace(/[\r]/g, '\n');

                        if (response.data.result) {
                            dhtmlx.alert(response.data.result);
                            listToSave = [];
                            $scope.listToDelete = [];
                        } else {
                            dhtmlx.alert('No changes to be saved.');
                        }

                        //$state.reload();
                        ProjectClassPhase.get({}, function (projectClassPhaseData) {
                            $scope.checkList = [];
                            $scope.projectClassPhaseCollection = projectClassPhaseData.result;
                            //$scope.orgProjectClassPhaseCollection = angular.copy(projectClassPhaseData.result);  //Manasi 15-07-2020
                            addIndex($scope.projectClassPhaseCollection);
                            angular.forEach($scope.projectClassPhaseCollection, function (item, index) {
                                item.checkbox = false;
                                $scope.checkList[index + 1] = false;

                                //Find project class name for a project class phase
                                for (var x = 0; x < $scope.projectClassCollection.length; x++) {
                                    if ($scope.projectClassCollection[x].ProjectClassID == item.ProjectClassID) {
                                        item.ProjectClassName = $scope.projectClassCollection[x].ProjectClassName;
                                    }
                                }

                                //Find phase name for a project class phase
                                for (var x = 0; x < $scope.phaseCodeCollection.length; x++) {
                                    if ($scope.phaseCodeCollection[x].PhaseID == item.PhaseID) {
                                        item.PhaseCodeName = $scope.phaseCodeCollection[x].PhaseDescription;
                                    }
                                }
                            });
                            $scope.gridOptions.data = $scope.projectClassPhaseCollection;
                            $scope.orgProjectClassPhaseCollection = angular.copy($scope.projectClassPhaseCollection);   //Manasi 15-07-2020
                        });
                    }, function error(response) {
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
                angular.forEach($scope.projectClassPhaseCollection, function (item) {
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
                                ProjectClassID: item.ProjectClassID,
                                PhaseID: item.PhaseID,
                                Order: item.Order,
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
                    return;
                }
                if (newList.length != 0) {
                    for (var i = 0; i < newList.length; i++) {
                        var ind = -1;
                        angular.forEach($scope.projectClassPhaseCollection, function (item, index) {
                            if (item.displayId == newList[i].displayId) {
                                item.checkbox = false;
                                ind = index;
                            }
                        });
                        if (ind != -1) {
                            $scope.checkList.splice(newList[i].displayId, 1);
                            $scope.projectClassPhaseCollection.splice(ind, 1);
                        }
                    }

                }
                if (listToSave.length != 0) { }
                for (var i = 0; i < listToSave.length; i++) {
                    var ind = -1;
                    angular.forEach($scope.projectClassPhaseCollection, function (item, index) {
                        if (item.displayId == listToSave[i].displayId) {
                            item.checkbox = false;
                            ind = index;
                        }
                    });
                    if (ind != -1) {
                        $scope.checkList.splice(listToSave[i].displayId, 1);
                        $scope.projectClassPhaseCollection.splice(ind, 1);
                    }
                }
            }
            $scope.checkForChanges = function () {
                var unSavedChanges = false;
                var originalCollection = $scope.orgProjectClassPhaseCollection;
                var currentCollection = $scope.projectClassPhaseCollection;
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
                                if (originalObject.ID !== currentObject.ID ||
                                    originalObject.ProjectClassID !== currentObject.ProjectClassID ||
                                    originalObject.PhaseID !== currentObject.PhaseID ||
                                    originalObject.Order !== currentObject.Order) {
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
                        //onRouteChangeOff();
                        //$location.path(newUrl);
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