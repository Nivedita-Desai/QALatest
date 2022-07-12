angular.module('cpp.controllers').
    //Employee Controller
    controller('EmployeeCtrl', ['$state', '$http', '$uibModal', 'Employee', 'Organization', 'fteposition', '$rootScope', '$scope', 'Page', 'ProjectTitle', 'TrendStatus', '$location', '$timeout', 'UniqueIdentityNumber',
        function ($state, $http, $uibModal, Employee, Organization, fteposition, $rootScope, $scope, Page, ProjectTitle, TrendStatus, $location, $timeout, UniqueIdentityNumber) {
            Page.setTitle('Users');
            ProjectTitle.setTitle('');
            TrendStatus.setStatus('');

            var positionArray = [];
            Organization.lookup().get({}, function (organizationData) {
                $scope.organizationList = organizationData.result;
                $scope.setOrganization(($scope.organizationList[0]));
                //console.log("Get Org List");
                //console.log($scope.organizationList);
                $scope.filterOrg = $scope.organizationList[0];

                Employee.get({ OrganizationID: $scope.filterOrg.OrganizationID }, function (employeeData) {
                    console.log(employeeData);
                    $scope.checkList = [];
                    $scope.employeeCollection = employeeData.result;
                    $scope.orgEmployeeCollection = angular.copy(employeeData.result);
                    addIndex($scope.employeeCollection);
                    angular.forEach($scope.employeeCollection, function (item, index) {
                        item.checkbox = false;
                        $scope.checkList[index + 1] = false;
                    });
                    $scope.gridOptions.data = $scope.employeeCollection;
                    console.log('employeeCollection');
                    console.log($scope.employeeCollection);
                    console.log('orgEmployeeCollection');
                    console.log($scope.orgEmployeeCollection);
                });
            });

            fteposition.get({}, function (PosistionsData) {
                $scope.positionCollection = PosistionsData.result;

                //$scope.gridOptions.columnDefs[2].editDropdownOptionsArray = PosistionsData.result;

                //$rootScope.positionArray = [];
                angular.forEach(PosistionsData.result, function (item) {
                    //$scope.positionArray.push({ ID: item.Id, value: item.PositionDescription });
                    //$rootScope.positionArray.push({ ID: item.Id, value: item.PositionDescription });
                    positionArray.push({ ID: item.Id, value: item.PositionDescription });
                }
                );
                console.log('positionArray...');
                console.log(positionArray);
            });


            $scope.setOrganization = function (org) {

                $scope.selectedOrg = org;
                console.log("Here");
                console.log(org);
                console.log("End here");
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

            $scope.filterChangeOrg = function () {
                var orgId = $("#selectOrg").val();
                console.log(orgId);
                angular.forEach($scope.organizationList, function (org) {
                    if (orgId === org.OrganizationID) {
                        $scope.setOrganization(org);
                    }
                });

                console.log('filterChangeOrg');
                console.log(orgId);

                Employee.get({ OrganizationID: orgId }, function (employeeData) {
                    console.log(employeeData);
                    console.log($scope.checkForChanges());
                    $scope.checkList = [];
                    $scope.employeeCollection = employeeData.result;
                    $scope.orgEmployeeCollection = angular.copy(employeeData.result);
                    addIndex($scope.employeeCollection);
                    angular.forEach($scope.employeeCollection, function (item, index) {
                        item.checkbox = false;
                        $scope.checkList[index + 1] = false;
                    });
                    $scope.gridOptions.data = $scope.employeeCollection;

                    console.log('employeeCollection');
                    console.log($scope.employeeCollection);
                });
            };


            var newOrEdit = "";
            var url = serviceBasePath + "response/employee";
            $scope.$on('ngGridEventEndCellEdit', function (data) {
                console.log(data.targetScope.row.entity.status);
                //data.targetScope.row.entity.status = 'Modified';  //luan here - inaccurate
                console.log($scope.userCollection);
            });

            function refreshEmployeeList() {
            	var orgId = $("#selectOrg").val();
            	Employee.get({ OrganizationID: orgId }, function (employeeData) {
            		$scope.checkList = [];
            		$scope.employeeCollection = employeeData.result;
            		$scope.orgEmployeeCollection = angular.copy(employeeData.result);
            		addIndex($scope.employeeCollection);
            		angular.forEach($scope.employeeCollection, function (item, index) {
            			item.checkbox = false;
            			$scope.checkList[index + 1] = false;
            		});
            		$scope.gridOptions.data = $scope.employeeCollection;

            		console.log('employeeCollection');
            		console.log($scope.employeeCollection);
            	});
            }

            var addIndex = function (data) {
                var i = 1;
                angular.forEach(data, function (value, key, obj) {
                    value.displayId = i;
                    i = i + 1;
                    if (value.Schedule === "0001-01-01T00:00:00") {
                        value.Schedule = "";
                    }
                });
            };

            $scope.cellInputEditableTemplate = '<input ng-class="\'colt\' + col.index" ng-input="COL_FIELD" ng-model="COL_FIELD" ng-blur="updateEntity(row)" />';
            $scope.cellPasswordEditableTemplate = '<input type = "password" ng-class="\'colt\' + col.index" ng-input="COL_FIELD" ng-model="COL_FIELD" ng-blur="updateEntity(row)" />';
            $scope.cellSelectEditableTemplateOrganization = '<select ng-class="\'colt\' + col.index" ng-input="COL_FIELD" ng-model="COL_FIELD" ng-click = "test(COL_FIELD)" ng-options="org.OrganizationName for org in organizationCollection" />';
            $scope.mySelections = [];
            $scope.test = function (test) {
                console.log('test...');
                console.log(test);
            };


            var isFresh = true;
            var currentPoint = "";

            //$scope.cellCheckEditTableTemplate = '<input class="c-approval-matrix-check"  type="checkbox" ng-input="COL-FIELD" ng-model="COL_FIELD"/>';
            $scope.addRow = function () {
                var x = Math.max.apply(Math, $scope.employeeCollection.map(function (o) {

                    return o.displayId;
                }));

                if (x < 0) {
                    console.log(x);
                    x = 0;
                }

                $scope.employeeCollection.splice(x, 0, {
                    displayId: ++x,
                    ID: '',
                    OrganizationID: $("#selectOrg").val(),
                    FTEPositionID: 'Click to Select',
                    Name: '',
                    FirstName: '',
                    LastName: '',
                    MiddleName: '',
                    HourlyRate: '0.00',
                    isActive: 1,
                    ReferenceID: '',
                    UniqueIdentityNumber: '',
                    checkbox: false,
                    new: true
                });

                console.log($scope.employeeCollection);

                if (isFresh) {
                	UniqueIdentityNumber.get({
                		NumberType: 'Employee',
                		'OrganizationID': 0,
                		'PhaseID': 0,
                		'CategoryID': 0
                	}, function (response) {
                		$scope.employeeCollection[$scope.employeeCollection.length - 1].UniqueIdentityNumber = response.result;
                		isFresh = false;
                		currentPoint = response.result;
                	});
                } else {
                	currentPoint = "BE" + ((parseInt(currentPoint.substr(2)) + 1)).toString().padStart(5, '0');

                	$scope.employeeCollection[$scope.employeeCollection.length - 1].UniqueIdentityNumber = currentPoint;
                }
                $scope.gridApi.core.clearAllFilters();//Nivedita-T on 17/11/2021
                $timeout(function () {
                    console.log($scope.gridOptions.data[$scope.gridOptions.data.length - 1], $scope.gridOptions.columnDefs[0]);
                    
                    $scope.gridApi.core.scrollTo($scope.gridOptions.data[$scope.gridOptions.data.length - 1], $scope.gridOptions.columnDefs[0]);
                }, 1);
            };

            $scope.gridOptions = {
                enableColumnMenus: false,
                enableCellEditOnFocus: true,
                enableFiltering: true,
                /*data: 'userCollection',
                enableRowSelection: false,
                enableCellSelection: true,
                selectedItems: $scope.mySelections,
                enableCellEditOnFocus: true,
                multiSelect: false,*/
                rowHeight: 40,
                /*afterSelectionChange: function (rowItem, event) {
                    console.log($scope.mySelections);
                    $scope.selectedIDs = [];
                    console.log(rowItem);
                    angular.forEach($scope.mySelections, function (item) {
                        $scope.selectedIDs.push(item.id)
                    });

                },*/
                columnDefs: [{
                    field: 'displayId',
                    name: 'display ID',
                    displayName: 'Id',
                    enableCellEdit: false,
                    //cellClass: 'c-col-id',
                    cellClass: 'c-col-Num',  //Manasi 
                    width: 50
                    /*cellTemplate: '<div ng-class="c-col-id" style="margin-top:15%;" ng-click="clicked(row,col)">{{row.getProperty(col.field)}}</div>'*/

                },
                //{
                //    field: 'ID',
                //    name: 'ID',
                //    displayName: 'Emp Id',
                //    enableCellEdit: false,
                //    cellClass: 'c-col-id',
                //    width: 100
                //    //visible: false
                //    /*enableCellEditOnFocus: true,
                //    editableCellTemplate: $scope.cellInputEditableTemplate*/

                //},

                //{
                //    field: 'Name',
                //    name: 'Name',
                //    displayName: 'Name'
                //    /*enableCellEditOnFocus: true,
                //    editableCellTemplate: $scope.cellInputEditableTemplate*/


                //},

                {
                	field: 'FirstName',
                	name: 'FirstName',
                	displayName: 'First Name*'
                	/*enableCellEditOnFocus: true,
                    editableCellTemplate: $scope.cellInputEditableTemplate*/


                },

                {
                	field: 'LastName',
                	name: 'LastName',
                	displayName: 'Last Name*'
                	/*enableCellEditOnFocus: true,
                    editableCellTemplate: $scope.cellInputEditableTemplate*/


                },

                {
                	field: 'MiddleName',
                	name: 'MiddleName',
                	displayName: 'Middle Name'
                	/*enableCellEditOnFocus: true,
                    editableCellTemplate: $scope.cellInputEditableTemplate*/


                },

                {

                    field: 'FTEPositionID',
                    name: 'FTEPositionID',
                    displayName: 'FTE Position*',
                    editableCellTemplate: 'ui-grid/dropdownEditor',
                    cellFilter: 'customFilter:this',
                    editDropdownIdLabel: 'ID',
                    editDropdownValueLabel: 'value',
                    editDropdownOptionsArray: positionArray,
                    cellClass: 'c-col-Num',  //Manasi 

                },
                {
                    field: 'HourlyRate',
                    name: 'HourlyRate',
                    displayName: 'Hourly Rate*',
                    enableCellEdit: true,
                    type: 'text',
                    cellFilter: 'currency',
                    width: 150,
                    cellClass: 'c-col-Num',  //Manasi         
                    /*enableCellEditOnFocus: true,
                    editableCellTemplate: $scope.cellInputEditableTemplate,*/
                    //width: 200


                },
                    {
                        name: 'isActive',
                        displayName: 'Active',
                        width: 75,
                        type: 'boolean',
                        cellClass: 'text-center', /*changes by shweta*/
                        cellTemplate: '<input type="checkbox" ng-model="row.entity.isActive" ng-true-value="1" ng-false-value="0">'

                    },
                {
                    field: 'ReferenceID',
                    name: 'ReferenceID',
                    displayName: 'Reference ID',
                    enableCellEditOnFocus: true,
                    cellClass: 'c-col-Num'  //Manasi 
                    /*enableCellEditOnFocus: true,
                    editableCellTemplate: $scope.cellInputEditableTemplate*/
                    //cellTemplate: '<input type="text" ng-click="grid.appScope.cellClicked(row,col)">'


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

            $scope.check = function (row, col) {
                if (row.entity.checkbox === false) {
                    row.entity.checkbox = true;
                    row.config.enableRowSelection = true;
                } else {
                    row.entity.checkbox = false;
                }
            };

            $scope.clicked = function (row, col) {
                $scope.orgRow = row;
                $scope.col = col;
                // $scope.row = row.entity;
            };

            var saved = false; //if table has been saved, then true

            $scope.save = function () {
                $("#save_Employee").attr("disabled", true);
                var isReload = false;
                var isChanged = true;
                var isFilled = true;
                var listToSave = [];
                var dataObj;
                angular.forEach($scope.employeeCollection, function (employee, key) {
                    console.log(employee);
                    if (isFilled === false) {
                        return;
                    }
                    if (employee.FirstName === "" || employee.LastName === "") {
                        $("#save_Employee").attr("disabled", false);
                        dhtmlx.alert({
                            text: "Employee first and last name cannot be empty! (Row " + employee.displayId + ")",
                            width: "300px"
                        });
                        isFilled = false;
                        return;
                    }

                    if (employee.FTEPositionID === 'Click to Select') {
                        $("#save_Employee").attr("disabled", false);
                        dhtmlx.alert({
                            text: "Please select FTE Position from the drop down list! (Row " + employee.displayId + ")",
                            width: "300px"
                        });
                        isFilled = false;
                        return;
                    }

                    console.log(Math.sign(employee.HourlyRate), isNaN(employee.HourlyRate));
                    if (isNaN(employee.HourlyRate) || Math.sign(employee.HourlyRate) < 0) {
                        $("#save_Employee").attr("disabled", false);
                        dhtmlx.alert({
                            text: "Hourly Rate needs to be a valid number! (Row " + employee.displayId + ")",
                            width: "300px"
                        });
                        isFilled = false;
                        return;
                    }
                    if (employee.UniqueIdentityNumber === "" || employee.UniqueIdentityNumber == null) {
                        $("#save_Employee").attr("disabled", false);
                        dhtmlx.alert({
                            text: "Unique Identifier cannot be empty! (Row " + employee.displayId + ")",
                            width: "300px"
                        });
                        isFilled = false;
                        return;
                    }


                    if (!(/(BE[0-9]{5})/.test(employee.UniqueIdentityNumber) && employee.UniqueIdentityNumber.length == 7)) {
                        $("#save_Employee").attr("disabled", false);
                    	dhtmlx.alert({
                    		text: "Unique identifier must be in the format of BExxxxx (Row " + employee.displayId + ")",
                    		width: "400px"
                    	});
                    	okToExit = false;
                    	isFilled = false;
                    	return;
                    }

                    if (employee.new === true) {
                        dataObj = {
                            Operation: '1',
                            ID: employee.ID,
                            OrganizationID: $("#selectOrg").val(),
                            FTEPositionID: employee.FTEPositionID,
                            Name: employee.LastName + ', ' + employee.FirstName,
                            FirstName: employee.FirstName,
                            LastName: employee.LastName,
                            MiddleName: employee.MiddleName,
                            HourlyRate: employee.HourlyRate,
                            isActive: employee.isActive,
                            ReferenceID: employee.ReferenceID,
                            UniqueIdentityNumber: employee.UniqueIdentityNumber
                        };

                        listToSave.push(dataObj);
                        saved = true;
                    }
                    else {
                        //Compare changes
                        isChanged = true;
                        console.log(employee);
                        angular.forEach($scope.orgEmployeeCollection, function (orgItem) {
                            if (employee.ID === orgItem.ID &&
                            	//employee.Name === orgItem.Name && 
                            	employee.FirstName === orgItem.FirstName && 
                            	employee.LastName === orgItem.LastName && 
                            	employee.MiddleName === orgItem.MiddleName && 
                                employee.FTEPositionID === orgItem.FTEPositionID &&
                                employee.HourlyRate === orgItem.HourlyRate &&
                                employee.isActive === orgItem.isActive &&
                                employee.ReferenceID === orgItem.ReferenceID &&
                                employee.UniqueIdentityNumber === orgItem.UniqueIdentityNumber) {
                                isChanged = false;
                            }
                        });

                        if (isChanged) {
                            isChanged = true;
                            
                            console.log(employee);

                            dataObj = {
                                Operation: '2',
                                ID: employee.ID,
                                OrganizationID: $("#selectOrg").val(),
                                FTEPositionID: employee.FTEPositionID,
                                Name: employee.LastName + ', ' + employee.FirstName,
                                FirstName: employee.FirstName,
                                LastName: employee.LastName,
                                MiddleName: employee.MiddleName,
                                HourlyRate: employee.HourlyRate,
                                isActive: employee.isActive,
                                ReferenceID: employee.ReferenceID,
                                UniqueIdentityNumber: employee.UniqueIdentityNumber
                            };

                            listToSave.push(dataObj);
                            saved = true;
                        } else {
                                dataObj = {
                                Operation: '4',
                                ID: employee.ID,
                                OrganizationID: $("#selectOrg").val(),
                                FTEPositionID: employee.FTEPositionID,
                                Name: employee.LastName + ', ' + employee.FirstName,
                                FirstName: employee.FirstName,
                                LastName: employee.LastName,
                                MiddleName: employee.MiddleName,
                                HourlyRate: employee.HourlyRate,
                                isActive: employee.isActive,
                                ReferenceID: employee.ReferenceID,
                                UniqueIdentityNumber: employee.UniqueIdentityNumber
                            };

                                listToSave.push(dataObj);
                                saved = true;
                        }
                    }
                });

                angular.forEach($scope.listToDelete, function (item) {
                    listToSave.push(item);
                });

                if (isFilled === false) {
                    return;
                } else if (listToSave.length === 0) {
                    dhtmlx.alert("Nothing changed.");
                } else {
                    console.log(listToSave);
                    $http({
                        url: url,
                        //url: 'http://localhost:29986/api/response/phasecode',
                        method: "POST",
                        data: JSON.stringify(listToSave),
                        headers: { 'Content-Type': 'application/json' }
                    }).then(function success(response) {
                        $("#save_Employee").attr("disabled", false);
                        response.data.result.replace(/[\r]/g, '\n');

                        if (response.data.result) {
                            $("#save_Employee").attr("disabled", false);
                            dhtmlx.alert(response.data.result);
                        } else {
                            $("#save_Employee").attr("disabled", false);
                            dhtmlx.alert('No changes to be saved.');
                        }

                        $state.reload();
                        //clear array
                        //listToSave = [];
                        //$scope.listToDelete = [];

                        console.log($scope.filterOrg);

                        Employee.get({ OrganizationID: $scope.filterOrg.OrganizationID }, function (employeeData) {
                            $scope.checkList = [];
                            $scope.employeeCollection = employeeData.result;
                            $scope.orgEmployeeCollection = angular.copy(employeeData.result);
                            addIndex($scope.employeeCollection);
                            angular.forEach($scope.employeeCollection, function (item, index) {
                                item.checkbox = false;
                                $scope.checkList[index + 1] = false;
                            });
                            $scope.gridOptions.data = $scope.employeeCollection;

                            console.log('employeeCollection');
                            console.log($scope.employeeCollection);
                        });
                    }, function error(response) {
                        $("#save_Employee").attr("disabled", false);
                        dhtmlx.alert("Failed to save. Information missing!");
                        console.log("Failed to save");
                    });
                }
            };

            $scope.delete = function () {
                var isChecked = false;
                var unSavedChanges = false;
                var listToSave = [];
                var selectedRow = false;
                var newList = [];
                $scope.listToDelete = [];
                var dataObj;
                angular.forEach($scope.employeeCollection, function (item) {
                    if (item.checkbox === true) {
                        selectedRow = true;
                        if (item.new === true) {
                            unSavedChanges = true;
                            newList.push(item);
                        }
                        else {
                            isChecked = true;
                            dataObj = {
                                Operation: '3',
                                ID: item.ID,
                                OrganizationID: $("#selectOrg").val(),
                                FTEPositionID: item.FTEPositionID,
                                Name: item.Name,
                                FirstName: item.FirstName,
                                LastName: item.LastName,
                                MiddleName: item.MiddleName,
                                HourlyRate: item.HourlyRate,
                                isActive: item.isActive,
                                ReferenceID: item.ReferenceID,
                                displayId: item.displayId,
                                UniqueIdentityNumber: 0

                            };
                            listToSave.push(dataObj);
                            $scope.listToDelete.push(dataObj);
                            //dhtmlx.alert("Record Deleted");
                        }
                    }
                });

                if (!selectedRow) {
                    dhtmlx.alert("Please select a record to delete.");
                }

                if (newList.length !== 0) {
                    for (var i = 0; i < newList.length; i++) {
                        var ind = -1;
                        angular.forEach($scope.employeeCollection, function (item, index) {
                            if (item.displayId === newList[i].displayId) {
                                item.checkbox = false;

                                ind = index;
                            }
                        });
                        if (ind !== -1) {
                            $scope.checkList.splice(newList[i].displayId, 1);
                            $scope.employeeCollection.splice(ind, 1);
                        }
                    }

                }
                if (listToSave.length !== 0) {

                    for (var i = 0; i < listToSave.length; i++) {
                        var ind = -1;
                        angular.forEach($scope.employeeCollection, function (item, index) {
                            if (item.displayId === listToSave[i].displayId) {
                                item.checkbox = false;

                                ind = index;
                            }
                        });
                        if (ind !== -1) {
                            $scope.checkList.splice(listToSave[i].displayId, 1);
                            $scope.employeeCollection.splice(ind, 1);
                        }
                    }
                }
            };


            $scope.checkForChanges = function () {
                var unSavedChanges = false;
                var originalCollection = $scope.orgEmployeeCollection;
                var currentCollection = $scope.employeeCollection;
                if (currentCollection.length !== originalCollection.length) {
                    unSavedChanges = true;
                    return unSavedChanges;
                } else {
                    angular.forEach(currentCollection, function (currentObject) {
                        for (var i = 0, len = originalCollection.length; i < len; i++) {
                            if (unSavedChanges) {
                                return unSavedChanges; // no need to look through the rest of the original array
                            }
                            if (originalCollection[i].ID === currentObject.ID) {
                                var originalObject = originalCollection[i];
                                console.log(originalObject, currentObject);
                                // compare relevant data
                                if (originalObject.FirstName !== currentObject.FirstName ||
                                    originalObject.LastName !== currentObject.LastName ||
                                    originalObject.Email !== currentObject.Email ||
                                    originalObject.Role !== currentObject.Role) {
                                    // alert if a change has not been saved
                                    //alert("unsaved change on line" + currentCollection.displayId);
                                    unSavedChanges = true;
                                    return unSavedChanges;
                                }
                                break; //no need to check any further, go to next object in new collection
                            }
                            else {
                                unsavedChanges = false;
                            }
                        }
                    });
                }
                return unSavedChanges;
            };


            onRouteChangeOff = $scope.$on('$locationChangeStart', function (event) {
                var newUrl = $location.path();
                console.log($scope.checkForChanges());
                if (!$scope.checkForChanges()) {
                    saved = true;
                }
                if (saved) return;
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

    .filter('customFilter', function () {
        

        return function (input, context) {
            //if (typeof context !== 'string') {
                var map = context.col.colDef.editDropdownOptionsArray;
                var idField = context.col.colDef.editDropdownIdLabel;
                var valueField = context.col.colDef.editDropdownValueLabel;
                var initial = context.row.entity[context.col.field];
                if (typeof map !== "undefined") {
                    for (var i = 0; i < map.length; i++) {
                        if (map[i][idField] === input) {
                            return map[i][valueField];
                        }
                    }
                } else if (initial) {
                    return initial;
                }
            //}
            return input;
        };
    })
    ;

