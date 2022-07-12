angular.module('cpp.controllers').
    controller('ClientPOCCtrl', ['$http', 'Page', '$state', 'UpdateClientPOC', '$uibModal', 'Client', 'ClientPOC', '$scope', '$rootScope', 'ProjectTitle', 'TrendStatus', '$location', '$timeout', 'UniqueIdentityNumber', //Tanmay
        function ($http, Page, $state, UpdateClientPOC, $uibModal, Client, ClientPOC, $scope, $rootScope, ProjectTitle, TrendStatus, $location, $timeout, UniqueIdentityNumber) {     //Tanmay-D
            var okToExit = true;
            Page.setTitle('Clients POC');    //Tanmay
            ProjectTitle.setTitle('');
            var ClientArray = [];
            TrendStatus.setStatus('');
            $scope.checkedRow = [];
            $scope.gridOPtions = {};
            $scope.myExternalScope = $scope;
            var url = serviceBasePath + 'response/ClientPOC/'; 

            Client.get({}, function (clients) {
                $scope.ClientDropdownCollection = clients.result;
                console.log(clients.result);

                angular.forEach(clients.result, function (item) {
                    ClientArray.push({ ID: item.ClientID, Value: item.ClientName });
                });
                ClientPOC.get({}, function (response) {
                    $scope.checkList = [];
                    $scope.ClientCollection = response.result;      //Tanmay-D
                    $scope.orgClientCollection = angular.copy(response.result);
                    addIndex($scope.ClientCollection);
                    angular.forEach($scope.ClientCollection, function (item, index) {
                        $scope.checkList[index + 1] = false;
                        item.checkbox = false;
                        for (var x = 0; x < $scope.ClientDropdownCollection.length; x++) {

                            if ($scope.ClientDropdownCollection[x].ClientID == item.ClientID) {
                                item.ClientName = $scope.ClientDropdownCollection[x].ClientName;
                                //$scope.userCollection.EmployeeID = item.EmployeeID;
                                //$scope.userCollection.EmployeeName = item.EmployeeName;
                            }
                        }
                    });

                    $scope.gridOptions.data = $scope.ClientCollection;
                });
            });     //Tanmay
            

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

            var isFresh = true;
            var currentPoint = "";

            $scope.addRow = function () {
                console.log($scope.ClientCollection);
                var x = Math.max.apply(Math, $scope.ClientCollection.map(function (o) {

                    return o.displayId;
                }))

                if (x < 0) {
                    console.log(x);
                    x = 0;
                }


                $scope.checkList[++x] = false;
                $scope.ClientCollection.splice(x, 0, {
                    displayId: x,
                    ClientPOCID: '',    //Tanmay
                    ClientID: '',       //Tanmay
                    BillingPOC: '',     //Tanmay
                    BillingPOCDescription: '',
                    BillingPOCPhone1: '',     //Tanmay
                    BillingPOCPhone2: '',     //Tanmay
                    BillingPOCEmail: '',     //Tanmay
                    BillingPOCAddressLine1: '',     //Tanmay
                    BillingPOCAddressLine2: '',     //Tanmay
                    BillingPOCCity: '',     //Tanmay
                    BillingPOCState: '',     //Tanmay
                    BillingPOCPONo: '',     //Tanmay
                    UniqueIdentityNumber: '',
                    checkbox: false,
                    new: true
                });

                if (isFresh) {
                    UniqueIdentityNumber.get({
                        NumberType: 'ClientPOC',       //Tanmay-D
                        'OrganizationID': 0,
                        'PhaseID': 0,
                        'CategoryID': 0
                    }, function (response) {
                        $scope.ClientCollection[$scope.ClientCollection.length - 1].UniqueIdentityNumber = response.result;
                        isFresh = false;
                        currentPoint = response.result;
                    });
                } else {
                    currentPoint = "PC" + ((parseInt(currentPoint.substr(2)) + 1)).toString().padStart(5, '0');
                    $scope.ClientCollection[$scope.ClientCollection.length - 1].UniqueIdentityNumber = currentPoint;
                }
                $scope.gridApi.core.clearAllFilters();//Nivedita-T on 17/11/2021

                $timeout(function () {
                    $scope.gridApi.core.scrollTo($scope.gridOptions.data[$scope.gridOptions.data.length - 1], $scope.gridOptions.columnDefs[0]);
                }, 1);
            }
            var disableNew = false;
            $scope.filterInput = "";

            $scope.$watch('filterInput', function (newValue, oldValue) {        //Tanmay-D
                console.log($scope.filterInput);
                console.log(oldValue);
                console.log(newValue);
            });

            function onFilterChanged(value1, value2) {      //Tanmay-D
                console.log($scope.filterInput + $scope.filterInput.length);
                if ($scope.filterInput.length == 1) {
                    disableNew = false;
                }
                else if ($scope.filterInput != "" || $scope.filterInput.length == 0) {
                    disableNew = true;
                }
            }

            $scope.gridOptions = {
                enableColumnMenus: false,
                enableCellEditOnFocus: true,
                enableFiltering: true,

                rowHeight: 40,
                width: 400,

                columnDefs: [       //Tanmay-D
                    {
                        field: 'displayId',
                        name: 'ID',
                        enableCellEdit: false,
                        width: 50,
                        cellClass: 'c-col-Num' //Manasi
                    },
                    {
                        field: 'ClientName',      //Tanmay-D
                        name: 'Client Name*',
                        enableCellEdit: true,
                        editableCellTemplate: 'ui-grid/dropdownEditor',
                        editDropdownIdLabel: 'ID',
                        editDropdownValueLabel: 'Value',
                        editDropdownOptionsArray: ClientArray,
                        //editDropDownChange: 'test',
                        cellFilter: 'customFilter:this',
                        cellClass:'c-col-Num',
                        width: 200
                    },
                    {
                        field: 'BillingPOC',
                        name: 'Billing POC*',
                        enableCellEditOnFocus: true,
                        widith: 300,
                        filter: {
                            condition: function (searchTerm, cellValue, test) {
                                $scope.filterInput = searchTerm;
                                return cellValue.toLowerCase().indexOf(searchTerm.toLowerCase()) >= 0;
                            }
                        }
                    }, {
                        field: 'BillingPOCDescription',
                        name: 'Description',
                        widith: 300,
                        filter: {
                            condition: function (searchTerm, cellValue, test) {
                                $scope.filterInput = searchTerm;
                                return cellValue.toLowerCase().indexOf(searchTerm.toLowerCase()) >= 0;
                            }
                        }
                    }, {
                        field: 'BillingPOCPhone1',
                        name: 'Phone Number 1',
                        enableCellEditOnFocus: true,
                        widith: 300,
                        filter: {
                            condition: function (searchTerm, cellValue, test) {
                                $scope.filterInput = searchTerm;
                                return cellValue.toLowerCase().indexOf(searchTerm.toLowerCase()) >= 0;
                            }
                        }
                    }, {
                        field: 'BillingPOCPhone2',
                        name: 'Phone Number 2',
                        enableCellEditOnFocus: true,
                        widith: 300,
                        filter: {
                            condition: function (searchTerm, cellValue, test) {
                                $scope.filterInput = searchTerm;
                                return cellValue.toLowerCase().indexOf(searchTerm.toLowerCase()) >= 0;
                            }
                        }
                    }, {
                        field: 'BillingPOCEmail',
                        name: 'Email Address',
                        enableCellEditOnFocus: true,
                        widith: 300,
                        filter: {
                            condition: function (searchTerm, cellValue, test) {
                                $scope.filterInput = searchTerm;
                                return cellValue.toLowerCase().indexOf(searchTerm.toLowerCase()) >= 0;
                            }
                        }
                    }, {
                        field: 'BillingPOCAddressLine1',
                        name: 'Address Line 1',
                        enableCellEditOnFocus: true,
                        widith: 300,
                        filter: {
                            condition: function (searchTerm, cellValue, test) {
                                $scope.filterInput = searchTerm;
                                return cellValue.toLowerCase().indexOf(searchTerm.toLowerCase()) >= 0;
                            }
                        }
                    }, {
                        field: 'BillingPOCAddressLine2',
                        name: 'Address Line 2',
                        enableCellEditOnFocus: true,
                        widith: 300,
                        filter: {
                            condition: function (searchTerm, cellValue, test) {
                                $scope.filterInput = searchTerm;
                                return cellValue.toLowerCase().indexOf(searchTerm.toLowerCase()) >= 0;
                            }
                        }
                    }, {
                        field: 'BillingPOCCity',
                        name: 'City',
                        enableCellEditOnFocus: true,
                        widith: 300,
                        filter: {
                            condition: function (searchTerm, cellValue, test) {
                                $scope.filterInput = searchTerm;
                                return cellValue.toLowerCase().indexOf(searchTerm.toLowerCase()) >= 0;
                            }
                        }
                    }, {
                        field: 'BillingPOCState',
                        name: 'State',
                        enableCellEditOnFocus: true,
                        widith: 300,
                        filter: {
                            condition: function (searchTerm, cellValue, test) {
                                $scope.filterInput = searchTerm;
                                return cellValue.toLowerCase().indexOf(searchTerm.toLowerCase()) >= 0;
                            }
                        }
                    }, {
                        field: 'BillingPOCPONo',
                        name: 'Zip Code',
                        enableCellEditOnFocus: true,
                        widith: 300,
                        filter: {
                            condition: function (searchTerm, cellValue, test) {
                                $scope.filterInput = searchTerm;
                                return cellValue.toLowerCase().indexOf(searchTerm.toLowerCase()) >= 0;
                            }
                        }
                    },{
                        field: 'UniqueIdentityNumber',
                        name: 'Unique Identifier',
                        width: 200,
                        //  editableCellTemplate: $scope.cellInputEditableTemplate,
                        //cellFilter :'mapStatus'


                    },{
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
                gridApi.core.on.filterChanged($scope, onFilterChanged);

                $scope.gridApi.edit.on.beginCellEdit($scope, function (rowEntity, colDef) {
                    $('div.ui-grid-cell form').find('input').css('height', '40px');
                    $('div.ui-grid-cell form').find('select').css('height', '40px');
                    $('div.ui-grid-cell form').css('margin', '0px');
                    $('div.ui-grid-cell form').find('input').putCursorAtEnd();
                    $('div.ui-grid-cell form').find('select').putCursorAtEnd();
                    $('div.ui-grid-cell form').find('select').focus();
                });
            };

            $scope.disableNewButton = function () {
                return disableNew;
            }


            $scope.cellClicked = function (row, col) {
                alert('hey!');
            }

            jQuery.fn.putCursorAtEnd = function () {
                console.log(this);
                return this.each(function () {
                    var $el = $(this),
                        el = this;

                    if (!$el.is(":focus")) {
                        $el.focus();
                    }

                    if (el.setSelectionRange) {
                        var ghettoLengthFix = 9999; //luan custom
                        var len = ghettoLengthFix * 2;

                        setTimeout(function () {
                            el.setSelectionRange(len, len);
                            console.log('set to end');
                        }, 1);

                    } else {
                        $el.focus();
                        $el.val($el.val());
                    }
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
                angular.forEach($scope.ClientCollection, function (value, key, obj) {
                  
                    if (value.BillingPOCPhone1.length > 0) {
                        if (value.BillingPOCPhone1.length != 10) {
                            dhtmlx.alert('Enter valid 10 digit Client Phone 1.'); // Jignesh-02-03-2021
                            return true;
                        }
                    }

                    if (value.BillingPOCPhone2.length > 0) {
                        if (value.BillingPOCPhone2.length != 10) {
                            dhtmlx.alert('Enter valid 10 digit Client Phone 2.'); // Jignesh-02-03-2021
                            return true;
                        }
                    }

                    if (value.BillingPOCEmail.length > 0) {
                        var testEmail = /^[A-Z0-9._%+-]+@([A-Z0-9-]+\.)+[A-Z]{2,4}$/i;
                        if (!testEmail.test(value.BillingPOCEmail)) {
                            dhtmlx.alert('Please enter valid Email Address for Billing POC.'); // Jignesh-02-03-2021
                            return;
                        }
                    }
                    //Find client id for a user
                    for (var x = 0; x < $scope.ClientDropdownCollection.length; x++) {
                        if ($scope.ClientDropdownCollection[x].ClientID == value.ClientName) {
                            value.ClientID = $scope.ClientDropdownCollection[x].ClientID;
                            value.ClientName = $scope.ClientDropdownCollection[x].ClientName;
                            break;
                        }
                    }

                    if (value.BillingPOC == "" || value.UniqueIdentityNumber == "") {
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
                            ClientID: value.ClientID,
                            BillingPOC: value.BillingPOC,
                            BillingPOCDescription: value.BillingPOCDescription,
                            BillingPOCPhone1: value.BillingPOCPhone1,
                            BillingPOCPhone2: value.BillingPOCPhone2,
                            BillingPOCEmail: value.BillingPOCEmail,
                            BillingPOCAddressLine1: value.BillingPOCAddressLine1,
                            BillingPOCAddressLine2: value.BillingPOCAddressLine2,
                            BillingPOCCity: value.BillingPOCCity,
                            BillingPOCState: value.BillingPOCState,
                            BillingPOCPONo: value.BillingPOCPONo,
                            UniqueIdentityNumber: value.UniqueIdentityNumber
                        }
                        listToSave.push(dataObj);
                        okToExit = true;

                    }
                    else {//Update Item if there is changes
                        okToExit = false;
                        isChanged = true;
                        angular.forEach($scope.orgClientCollection, function (orgItem) {
                            if (value.ClientPOCID === orgItem.ClientPOCID &&
                                value.ClientID === orgItem.ClientID &&
                                value.BillingPOC === orgItem.BillingPOC &&
                                value.BillingPOCDescription === orgItem.BillingPOCDescription &&
                                value.BillingPOCPhone1 === orgItem.BillingPOCPhone1 &&
                                value.BillingPOCPhone2 === orgItem.BillingPOCPhone2 &&
                                value.BillingPOCEmail === orgItem.BillingPOCEmail &&
                                value.BillingPOCAddressLine1 === orgItem.BillingPOCAddressLine1 &&
                                value.BillingPOCAddressLine2 === orgItem.BillingPOCAddressLine2 &&
                                value.BillingPOCCity === orgItem.BillingPOCCity &&
                                value.BillingPOCState === orgItem.BillingPOCState &&
                                value.BillingPOCPONo === orgItem.BillingPOCPONo &&
                                value.UniqueIdentityNumber === orgItem.UniqueIdentityNumber) {
                                isChanged = false;
                                okToExit = true;
                            }
                        });
                        if (isChanged == true) {
                            var dataObj = {
                                Operation: '2',
                                ClientPOCID: value.ClientPOCID,
                                ClientID: value.ClientID,
                                BillingPOC: value.BillingPOC,
                                BillingPOCDescription: value.BillingPOCDescription,
                                BillingPOCPhone1: value.BillingPOCPhone1,
                                BillingPOCPhone2: value.BillingPOCPhone2,
                                BillingPOCEmail: value.BillingPOCEmail,
                                BillingPOCAddressLine1: value.BillingPOCAddressLine1,
                                BillingPOCAddressLine2: value.BillingPOCAddressLine2,
                                BillingPOCCity: value.BillingPOCCity,
                                BillingPOCState: value.BillingPOCState,
                                BillingPOCPONo: value.BillingPOCPONo,
                                UniqueIdentityNumber: value.UniqueIdentityNumber
                            }
                        } else {
                            var dataObj = {
                                Operation: '4',
                                ClientPOCID: value.ClientPOCID,
                                ClientID: value.ClientID,
                                BillingPOC: value.BillingPOC,
                                BillingPOCDescription: value.BillingPOCDescription,
                                BillingPOCPhone1: value.BillingPOCPhone1,
                                BillingPOCPhone2: value.BillingPOCPhone2,
                                BillingPOCEmail: value.BillingPOCEmail,
                                BillingPOCAddressLine1: value.BillingPOCAddressLine1,
                                BillingPOCAddressLine2: value.BillingPOCAddressLine2,
                                BillingPOCCity: value.BillingPOCCity,
                                BillingPOCState: value.BillingPOCState,
                                BillingPOCPONo: value.BillingPOCPONo,
                                UniqueIdentityNumber: value.UniqueIdentityNumber
                            }
                        }
                        listToSave.push(dataObj)
                        okToExit = true;
                    }
                });

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
                        isFresh = true;

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

                        $scope.orgClientCollection = angular.copy($scope.ClientCollection);
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
                console.log($scope.ClientCollection);
                angular.forEach($scope.ClientCollection, function (item) {
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
                                ClientPOCID: item.ClientPOCID,
                                BillingPOC: item.BillingPOC,
                                BillingPOCDescription: item.BillingPOCDescription,
                                BillingPOCPhone1: item.BillingPOCPhone1,
                                BillingPOCPhone2: item.BillingPOCPhone2,
                                BillingPOCEmail: item.BillingPOCEmail,
                                BillingPOCAddressLine1: item.BillingPOCAddressLine1,
                                BillingPOCAddressLine2: item.BillingPOCAddressLine2,
                                BillingPOCCity: item.BillingPOCCity,
                                BillingPOCState: item.BillingPOCState,
                                BillingPOCPONo: item.BillingPOCPONo,
                                UniqueIdentityNumber: item.UniqueIdentityNumber,
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
                        angular.forEach($scope.ClientCollection, function (item, index) {
                            if (item.displayId == newList[i].displayId) {
                                item.checkbox = false;
                                ind = index;
                            }

                            //Find client name for a user
                            //for (var x = 0; x < $scope.clientCollection.length; x++) {
                            //    if ($scope.clientCollection[x].ID == item.ClientID) {
                            //        item.ClientName = $scope.clientCollection[x].Name;
                            //    }
                            //}
                        });
                        if (ind != -1) {
                            $scope.checkList.splice(newList[i].displayId, 1);
                            $scope.ClientCollection.splice(ind, 1);
                        }
                    }
                }

                if (listToSave.length != 0) {
                    for (var i = 0; i < listToSave.length; i++) {
                        var ind = -1;
                        angular.forEach($scope.ClientCollection, function (item, index) {
                            if (item.displayId == listToSave[i].displayId) {
                                item.checkbox = false;
                                ind = index;
                            }
                        });
                        if (ind != -1) {
                            $scope.checkList.splice(listToSave[i].displayId, 1);
                            $scope.ClientCollection.splice(ind, 1);
                        }
                    }
                }
            }
            $scope.checkForChanges = function () {
                var unSavedChanges = false;
                var originalCollection = $scope.orgClientCollection;
                var currentCollection = $scope.ClientCollection;
                if (currentCollection.length != originalCollection.length) {
                    unSavedChanges = true;
                    return unSavedChanges;
                } else {
                    angular.forEach(currentCollection, function (currentObject) {
                        for (var i = 0, len = originalCollection.length; i < len; i++) {
                            if (unSavedChanges) {
                                return unSavedChanges; // no need to look through the rest of the original array
                            }
                            if (originalCollection[i].ClientPOCID == currentObject.ClientPOCID) {
                                var originalObject = originalCollection[i];
                                // compare relevant data
                                if (originalObject.ClientName !== currentObject.ClientName ||
                                    originalObject.BillingPOCDescription !== currentObject.BillingPOCDescription ||
                                    originalObject.BillingPOCPhone1 !== currentObject.BillingPOCPhone1 ||
                                    originalObject.BillingPOCPhone2 !== currentObject.BillingPOCPhone2 ||
                                    originalObject.BillingPOCEmail !== currentObject.BillingPOCEmail ||
                                    originalObject.BillingPOCAddressLine1 !== currentObject.BillingPOCAddressLine1 ||
                                    originalObject.BillingPOCAddressLine2 !== currentObject.BillingPOCAddressLine2 ||
                                    originalObject.BillingPOCCity !== currentObject.BillingPOCCity ||
                                    originalObject.BillingPOCState !== currentObject.BillingPOCState ||
                                    originalObject.BillingPOCPONo !== currentObject.BillingPOCPONo ||
                                    originalObject.UniqueIdentityNumber !== currentObject.UniqueIdentityNumber) {
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
                    }
                    else if (scope.params.confirm === "back") {
                        //do nothing
                    }
                });
                event.preventDefault();
            });
        }]);