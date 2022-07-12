angular.module('cpp.controllers').
    //Material Controller
    controller('MaterialCtrl', ['MaterialCategory', 'Material', 'UnitType', '$state', '$scope', '$rootScope', 'Category', '$uibModal', 'UpdateCategory', '$http', 'Page', 'ProjectTitle', 'TrendStatus', '$location', '$timeout', 'UniqueIdentityNumber', 'Vendor',
        function (MaterialCategory, Material, UnitType, $state, $scope, $rootScope, Category, $uibModal, UpdateCategory, $http, Page, ProjectTitle, TrendStatus, $location, $timeout, UniqueIdentityNumber, Vendor) {
            Page.setTitle('Material');
            ProjectTitle.setTitle('');
            TrendStatus.setStatus('');

            var url = serviceBasePath + 'response/Material/'; //here
            $scope.$on('ngGridEventEndCellEdit', function (data) {
                console.log(data.targetScope.row.entity.status);
                data.targetScope.row.entity.status = 'Modified';
                console.log($scope.userCollection);
            });

            $scope.openReports = function (reportManagerType) {
                var scope = $rootScope.$new();

                scope.params = {
                    ProjectID: 0,   //delayedData[2].result[0].ProjectID,
                    TrendNumber: 0,  //delayedData[3]
                    ReportManagerType: 'material'

                }

                $rootScope.modalInstance = $uibModal.open({
                    backdrop: 'static',
                    keyboard: false,
                    scope: scope,
                    templateUrl: "app/views/modal/ReportManagerModal.html",
                    size: "md",
                    controller: "ReportManagerCtrl"
                });
            }

            $scope.applicableList = [];

            //Get all material categories
            MaterialCategory.get({}, function (materialCategories) {
                $scope.materialCategoryCollection = materialCategories.result;
                console.log(materialCategories.result);

                angular.forEach($scope.materialCategoryCollection, function (materialCategory) {
                    materialCategory.MaterialCategoryName = materialCategory.Name; //here
                });
                $scope.gridOptions.columnDefs[3].editDropdownOptionsArray = $scope.materialCategoryCollection;

                //Get all unit types
                UnitType.get({}, function (unitTypeData) {
                    $scope.unitTypeCollection = unitTypeData.result;

                    angular.forEach($scope.unitTypeCollection, function (unitType) {
                        unitType.UnitTypeName = unitType.UnitName; //here
                    });
                    $scope.gridOptions.columnDefs[5].editDropdownOptionsArray = $scope.unitTypeCollection;

                    //Get all Vendor Names
                    Vendor.get({}, function (VendorName) {
                        $scope.VendorCollection = VendorName.result;
                        console.log(VendorName.result);

                        angular.forEach($scope.VendorCollection, function (Vendor) {
                            Vendor.VendorName = Vendor.VendorName; //here
                        });
                        $scope.gridOptions.columnDefs[4].editDropdownOptionsArray = $scope.VendorCollection;

                        //Get all materials
                        Material.get({}, function (materialData) {
                            $scope.checkList = [];
                            $scope.materialCollection = materialData.result;
                            $scope.orgMaterialCollection = angular.copy(materialData.result);
                            addIndex($scope.materialCollection);
                            angular.forEach($scope.materialCollection, function (item, index) {
                                item.checkbox = false;
                                $scope.checkList[index + 1] = false;

                                //Find material category name for a material
                                for (var x = 0; x < $scope.materialCategoryCollection.length; x++) {
                                    if ($scope.materialCategoryCollection[x].ID == item.MaterialCategoryID) {
                                        item.MaterialCategoryName = $scope.materialCategoryCollection[x].Name;
                                    }
                                }

                                //Find unit type for a material
                                for (var x = 0; x < $scope.unitTypeCollection.length; x++) {
                                    if ($scope.unitTypeCollection[x].UnitID == item.UnitTypeID) {
                                        item.UnitTypeName = $scope.unitTypeCollection[x].UnitName;
                                    }
                                }

                                // Find vendor names for a material
                                for (var x = 0; x < $scope.VendorCollection.length; x++) {
                                    console.log($scope.VendorCollection[x].VendorID, item.VendorID)
                                    if ($scope.VendorCollection[x].VendorID == item.VendorID) {
                                        item.VendorName = $scope.VendorCollection[x].VendorName;
                                    }
                                }
                            });
                            $scope.gridOptions.data = $scope.materialCollection;
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

            var isFresh = true;
            var currentPoint = "";

            $scope.addRow = function () {
                var x = Math.max.apply(Math, $scope.materialCollection.map(function (o) {

                    return o.displayId;
                }))

                if (x < 0) {
                    console.log(x);
                    x = 0;
                }

                $scope.checkList[++x] = false;
                $scope.materialCollection.splice(x, 0, {
                    displayId: x,
                    ID: '',
                    Name: '',
                    Description: '',
                    MaterialCategoryName: '',
                    Vendor: '',
                    UnitTypeID: '',
                    Cost: '',
                    UniqueIdentityNumber: '',
                    checkbox: false,
                    new: true
                });

                if (isFresh) {
                	UniqueIdentityNumber.get({
                		NumberType: 'Material',
                		'OrganizationID': 0,
                		'PhaseID': 0,
                		'CategoryID': 0
                	}, function (response) {
                        $scope.materialCollection[$scope.materialCollection.length - 1].UniqueIdentityNumber = response.result;
                        isFresh = false;
                        currentPoint = response.result;
                    });
                } else {
                    currentPoint = "BM" + ((parseInt(currentPoint.substr(2)) + 1)).toString().padStart(5, '0');

                    $scope.materialCollection[$scope.materialCollection.length - 1].UniqueIdentityNumber = currentPoint;
                }

                console.log($scope.materialCollection[$scope.materialCollection.length - 1]);
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
                    field: 'Name',
                    name: 'Name*',
                    width: 200
                }, {
                    field: 'Description',
                    name: 'Description',
                    width: 300
                }, {
                    field: 'MaterialCategoryName',
                    name: 'Category*',
                    editableCellTemplate: 'ui-grid/dropdownEditor',
                    editDropdownValueLabel: 'MaterialCategoryName', //code
                    editDropdownIdLabel: 'MaterialCategoryName',    //phase
                    editDropDownChange: 'test',
                    cellFilter: 'mapPhase',
                    width: 170
                }, {
                    field: 'VendorName',
                    name: 'Vendor',
                    editableCellTemplate: 'ui-grid/dropdownEditor',
                    editDropdownValueLabel: 'VendorName', //code
                    editDropdownIdLabel: 'VendorName',    //phase
                    editDropDownChange: 'Gucci',
                    cellFilter: 'mapPhase',
                    width: 180
                }, {
                    field: 'UnitTypeName',
                    name: 'Unit Type*',
                    editableCellTemplate: 'ui-grid/dropdownEditor',
                    editDropdownValueLabel: 'UnitTypeName', //code
                    editDropdownIdLabel: 'UnitTypeName',    //phase
                    editDropDownChange: 'test',
                    cellFilter: 'mapPhase',
                    width: 100
                }, {
                    field: 'Cost',
                    name: 'Cost*',
                        width: 140,
                        cellClass: 'c-col-Num', //Manasi
                        cellFilter: 'currency' //Manasi
                }, {
                    field: 'UniqueIdentityNumber',
                    name: 'Unique Identifier',
                        width: 200,
                        enableCellEdit: false,
                    //  editableCellTemplate: $scope.cellInputEditableTemplate,
                    //cellFilter :'mapStatus'


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
                $("#save_Material").attr("disabled", true);
                var isReload = false;
                var isChanged = true;
                var isFilled = true;
                var listToSave = [];
                
                angular.forEach($scope.materialCollection, function (value, key, obj) {


                    //Find material category id for a material
                    for (var x = 0; x < $scope.materialCategoryCollection.length; x++) {
                        if ($scope.materialCategoryCollection[x].Name == value.MaterialCategoryName) {
                            value.MaterialCategoryID = $scope.materialCategoryCollection[x].ID;
                        }
                    }

                    //Find unit type id for a material
                    console.log($scope.unitTypeCollection, value);
                    for (var x = 0; x < $scope.unitTypeCollection.length; x++) {
                        if ($scope.unitTypeCollection[x].UnitName == value.UnitTypeName) {
                            value.UnitTypeID = $scope.unitTypeCollection[x].UnitID;
                        }
                    }
                   
                    //Find vendor id for a material

                    for (var x = 0; x < $scope.VendorCollection.length; x++) {
                        if ($scope.VendorCollection[x].VendorName == value.VendorName) {
                            value.VendorID = $scope.VendorCollection[x].VendorID;
                        }
                        
                    }
                   
                    //Find vendor id for a material
               //     console.log($scope.vendorIDCollection, value);
                 //   for (var x = 0; x < $scope.vendorIDCollection.length; x++) {
                   //     if ($scope.vendorIDCollection[x].UnitName == value.VendorName) {
                     //       value.VendorID = $scope.vendorIDCollection[x].UnitID;
                       // }
//                    }

                    if (isNaN(value.Cost) || Number.parseFloat(value.Cost) <= 0) {
                        $("#save_Material").attr("disabled", false);
                        dhtmlx.alert('Cost must be a valid number greater than 0 (Row ' + value.displayId + ')');
                        isFilled = false;
                        return;
                    } else if (
                        //value.Description == "" ||  // Aditya 3-2-2022 
                        value.MaterialCategoryName == "" ||
                        value.MaterialCategoryID == "" || value.Name == "" || value.UnitTypeID == "" || value.Cost == "" || value.UniqueIdentityNumber == "" || value.VendorID == ""
                    ) {
                        $("#save_Material").attr("disabled", false);
                        dhtmlx.alert({
                            text: "Please fill data to all required fields before save (Row " + value.displayId + ")",
                            width: "300px"
                        });
                        isFilled = false;
                        return;
                    }

                    if (!(/(BM[0-9]{5})/.test(value.UniqueIdentityNumber) && value.UniqueIdentityNumber.length == 7)) {
                        $("#save_Material").attr("disabled", false);
                        dhtmlx.alert({
                            text: "Unique identifier must be in the format of BMxxxxx (Row " + value.displayId + ")",
                            width: "400px"
                        });
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
                            Description: value.Description,
                            Name: value.Name,
                            MaterialCategoryID: value.MaterialCategoryID,
                            UnitTypeID: value.UnitTypeID,
                            UniqueIdentityNumber: value.UniqueIdentityNumber,
                            Cost: value.Cost,
                            VendorID: value.VendorID
                        }
                        console.log(dataObj);
                        listToSave.push(dataObj);
                    }
                    else {
                        isChanged = true;
                        angular.forEach($scope.orgMaterialCollection, function (orgItem) {
                            if (value.ID === orgItem.ID && value.Description === orgItem.Description
                                && value.Name === orgItem.Name && value.MaterialCategoryID === orgItem.MaterialCategoryID
                                && value.UnitTypeID === orgItem.UnitTypeID && value.Cost === orgItem.Cost
                                && value.VendorID == orgItem.VendorID
                                && value.UniqueIdentityNumber === orgItem.UniqueIdentityNumber) {
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
                                Description: value.Description,
                                Name: value.Name,
                                MaterialCategoryID: value.MaterialCategoryID,
                                UnitTypeID: value.UnitTypeID,
                                UniqueIdentityNumber: value.UniqueIdentityNumber,
                                Cost: value.Cost,
                                VendorID: value.VendorID
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
                                Description: value.Description,
                                Name: value.Name,
                                MaterialCategoryID: value.MaterialCategoryID,
                                UnitTypeID: value.UnitTypeID,
                                UniqueIdentityNumber: value.UniqueIdentityNumber,
                                Cost: value.Cost,
                                VendorID: value.VendorID
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
                        $("#save_Material").attr("disabled", false);
                        isFresh = true;

                        console.log(response);

                        response.data.result.replace(/[\r]/g, '\n');

                        if (response.data.result) {
                            $("#save_Material").attr("disabled", false);
                            dhtmlx.alert(response.data.result);
                        } else {
                            $("#save_Material").attr("disabled", false);
                            dhtmlx.alert('No changes to be saved.');
                        }

                        //$state.reload();
                        Material.get({}, function (materialData) {
                            $scope.listToDelete = [];
                            $scope.checkList = [];
                            $scope.materialCollection = materialData.result;
                            $scope.orgMaterialCollection = angular.copy(materialData.result);
                            addIndex($scope.materialCollection);
                            angular.forEach($scope.materialCollection, function (item, index) {
                                item.checkbox = false;
                                $scope.checkList[index + 1] = false;

                                //Find material category name for a material
                                for (var x = 0; x < $scope.materialCategoryCollection.length; x++) {
                                    if ($scope.materialCategoryCollection[x].ID == item.MaterialCategoryID) {
                                        item.MaterialCategoryName = $scope.materialCategoryCollection[x].Name;
                                    }
                                }

                                //Find unit type for a material
                                for (var x = 0; x < $scope.unitTypeCollection.length; x++) {
                                    if ($scope.unitTypeCollection[x].UnitID == item.UnitTypeID) {
                                        item.UnitTypeName = $scope.unitTypeCollection[x].UnitName;
                                    }
                                }
                                //Find Venor Name for a material
                                for (var x = 0; x < $scope.VendorCollection.length; x++) {
                                    if ($scope.VendorCollection[x].VendorID == item.VendorID) {
                                        item.VendorName = $scope.VendorCollection[x].VendorName;
                                    }
                                }
                            });
                            $scope.gridOptions.data = $scope.materialCollection;

                            console.log($scope.materialCollection);
                        });
                    }, function error(response) {
                        $("#save_Material").attr("disabled", false);
                        dhtmlx.alert("Failed to save. Please contact your Administrator.");
                    });
                }
            }
            $scope.delete = function () {
                var isChecked = false;
                var unSavedChanges = false;
                var listToSave = [];
                var selectedRow = false;
                $scope.listToDelete = [];
                var newList = [];
                angular.forEach($scope.materialCollection, function (item) {
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
                                Name: item.Name,
                                Description: item.Description,
                                MaterialCategoryID: item.MaterialCategoryID,
                                UnitTypeID: item.UnitTypeID,
                                UniqueIdentityNumber: item.UniqueIdentityNumber,
                                Cost: item.Cost,
                                VendorID: item.VendorID,
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
                        angular.forEach($scope.materialCollection, function (item, index) {
                            if (item.displayId == newList[i].displayId) {
                                item.checkbox = false;
                                ind = index;
                            }
                        });
                        if (ind != -1) {
                            $scope.checkList.splice(newList[i].displayId, 1);
                            $scope.materialCollection.splice(ind, 1);
                        }
                    }

                }
                if (listToSave.length != 0) { }
                for (var i = 0; i < listToSave.length; i++) {
                    var ind = -1;
                    angular.forEach($scope.materialCollection, function (item, index) {
                        if (item.displayId == listToSave[i].displayId) {
                            item.checkbox = false;
                            ind = index;
                        }
                    });
                    if (ind != -1) {
                        $scope.checkList.splice(listToSave[i].displayId, 1);
                        $scope.materialCollection.splice(ind, 1);
                    }
                }
            }
            $scope.checkForChanges = function () {
                var unSavedChanges = false;
                var originalCollection = $scope.orgMaterialCollection;
                var currentCollection = $scope.materialCollection;
                if (currentCollection.length != originalCollection.length) {
                    unSavedChanges = true;
                    return unSavedChanges;
                } else {
                    angular.forEach(currentCollection, function (currentObject) {
                        for (var i = 0, len = originalCollection.length; i < len; i++) {
                            if (unSavedChanges) {
                                return unSavedChanges; // no need to look through the rest of the original array
                            }
                            if (originalCollection[i].ID == currentObject.ID) { //subcategoryID
                                var originalObject = originalCollection[i];
                                // compare relevant data
                                if (originalObject.ID !== currentObject.ID ||
                                    originalObject.MaterialCategoryID !== currentObject.MaterialCategoryID ||
                                    originalObject.Description !== currentObject.Description ||
                                    originalObject.Name !== currentObject.Name ||
                                    originalObject.UnitTypeID !== currentObject.UnitTypeID ||
                                    originalObject.VendorID !== currentObject.VendorID ||
                                    originalObject.Cost !== currentObject.Cost ||
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