angular.module('cpp.controllers').
    //User Controller
    controller('UserCtrl', ['$state', '$http', '$uibModal', 'User', '$rootScope', '$scope', 'Page', 'ProjectTitle', 'TrendStatus', '$location', 'AllEmployee', 'ProjectClass', '$timeout',
        function ($state, $http, $uibModal, User, $rootScope, $scope, Page, ProjectTitle, TrendStatus, $location, AllEmployee, ProjectClass, $timeout) {
            Page.setTitle('Users');
            ProjectTitle.setTitle('');
            TrendStatus.setStatus('');
            var empArray = [];
            var dptArray = []; //Narayan - 03/08/22 
            $scope.rolesArrayList = [];
            $scope.rolesArray = [];
            $scope.example14settings = {
                scrollableHeight: '100px', scrollable: true,
                enableSearch: false
            };

            $http.get(serviceBasePath + 'request/role').then(function (response) {
                $scope.RoleCollection = response.data.result;
                for (i in $scope.RoleCollection) {
                    $scope.rolesArrayList.push($scope.RoleCollection[i].Role);
                }
                
                angular.forEach($scope.rolesArrayList, function (item) {
                    //$scope.positionArray.push({ ID: item.Id, value: item.PositionDescription });
                    //$rootScope.positionArray.push({ ID: item.Id, value: item.PositionDescription });
                    $scope.rolesArray.push(item);
                    $scope.rolesArray11 = $scope.rolesArray.concat(item);
                }
                );
                console.log($scope.rolesArray11);
               /* angular.forEach($scope.rolesArrayList, function (value, key) {
                    debugger;
                    $scope.rolesArray = $scope.rolesArray.concat(value);
                });*/
                //$scope.gridOptions.columnDefs[6].editDropdownOptionsArray = response.data.result;
                //$scope.gridOptions.columnDefs[6].editDropdownOptionsArray = $scope.rolesArrayList;
                
                console.log($scope.RoleCollection);
            })


            //$http.get(serviceBasePath + 'Request/ProjectClass').then(function (response) {
            //    $scope.DeparmentCollection = response.data.result;

            //    $scope.gridOptions.columnDefs[7].editDropdownOptionsArray = response.data.result;
            //    console.log($scope.RoleCollection);
            //})//added by vaishnavi
            var newOrEdit = "";
            var url = serviceBasePath + "response/user";
            $scope.$on('ngGridEventEndCellEdit', function (data) {
                console.log(data.targetScope.row.entity.status);
                //data.targetScope.row.entity.status = 'Modified';
                console.log($scope.userCollection);
            });

            //Get all employees - filter by organization later
            AllEmployee.get({}, function (employees) {
                $scope.employeeCollection = employees.result;
                console.log(employees.result);

                angular.forEach($scope.employeeCollection, function (employee) {
                    employee.EmployeeName = employee.Name; //here
                });


                angular.forEach(employees.result, function (item) {
                    //$scope.positionArray.push({ ID: item.Id, value: item.PositionDescription });
                    //$rootScope.positionArray.push({ ID: item.Id, value: item.PositionDescription });
                    empArray.push({ ID: item.ID, value: item.Name });
                }
                );

                //$scope.gridOptions.columnDefs[7].editDropdownOptionsArray = $scope.employeeCollection;

                console.log($scope.employeeCollection);


                console.log(AllEmployee);
                console.log(User);

                // Narayan - 03/08/22 - Get all department api call
                ProjectClass.get({}, function (departments) {
                    $scope.departmentsData = departments.result;
                    console.log(departments.result);

                    angular.forEach($scope.departmentsData, function (department) {
                        department.DepartmentName = department.ProjectClassName;
                    });


                    angular.forEach(departments.result, function (item) {
                        //$scope.positionArray.push({ ID: item.Id, value: item.PositionDescription });
                        //$rootScope.positionArray.push({ ID: item.Id, value: item.PositionDescription });
                        dptArray.push({ ID: item.ProjectClassID, value: item.ProjectClassName });
                    }
                    );

                    //$scope.gridOptions.columnDefs[7].editDropdownOptionsArray = $scope.employeeCollection;

                    console.log($scope.departmentsData);


                    console.log(dptArray);
                    console.log(User);

                //Get all Users
                User.get({}, function (Users) {
                    $scope.checkList = [];
                    $scope.userCollection = Users.result;
                    $scope.orgUserCollection = angular.copy(Users.result);
                    console.log(Users);
                    angular.forEach($scope.userCollection, function (user, key) {
                        user.status = 'Unchanged';
                    });
                    $scope.orgUserCollection = angular.copy(Users.result);

                    //Declare true false dropdown list
                    $scope.trueFalseDropDown = [{ PasswordChangeRequiredName: "True" }, { PasswordChangeRequiredName: "False" }];
                    $scope.gridOptions.columnDefs[9].editDropdownOptionsArray = $scope.trueFalseDropDown; // Narayan - 03/08/22 - change index of password col 

                    addIndex($scope.userCollection);
                    angular.forEach($scope.userCollection, function (item, index) {
                        $scope.checkList[index + 1] = false;
                        item.checkbox = false;
                        
                       // var role[];
                        //var role_classDropdown = $('#uiSelect').find('#role_class');
                        //for (var i = 0; i < item.lstUserRole.length; i++) {
                        //    role_classDropdown.append('<option value=' + item.lstUserRole[i].Id + '>' + item.lstUserRole[i].Role + '</option>');
                        //    //if (item.lstUserRole[i].isSelected == true) {
                        //    //    item.RoleList.push(item.lstUserRole[i].Role);
                        //    //}
                        //}
                        //$('#role_class').multiselect({
                        //    // columns: 5,
                        //    clearButton: true,
                        //    search: false,
                        //    selectAll: false,
                        //    // rebuild : true,
                        //    nonSelectedText: '-- Select --',
                        //    numberDisplayed: 1

                        //});
                        //role_classDropdown.val(item.lstUserRole.RoleList);
                        //role_classDropdown.multiselect('refresh');
                        //Find password change name
                        if (item.PasswordChangeRequired) {
                            item.PasswordChangeRequiredName = "True";
                        } else {
                            item.PasswordChangeRequiredName = "False";
                        }

                        //Find employee
                        for (var x = 0; x < $scope.employeeCollection.length; x++) {
                            console.log($scope.employeeCollection[x].ID, item.EmployeeID);
                            if ($scope.employeeCollection[x].ID == item.EmployeeID) {
                                item.EmployeeName = $scope.employeeCollection[x].Name;
                                //$scope.userCollection.EmployeeID = item.EmployeeID;
                                //$scope.userCollection.EmployeeName = item.EmployeeName;
                            }
                        }
                        
                            
                        //Find Department
                        for (var i = 0; i < $scope.departmentsData.length; i++) {
                            if ($scope.departmentsData[i].ProjectClassID == item.DepartmentID) {
                                item.DepartmentName = $scope.departmentsData[i].DepartmentName;
                            }
                        }

                    });
                    
                    
                    $scope.gridOptions.data = $scope.userCollection;
                    console.log($scope.userCollection);
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
            //$scope.cellInputEditableTemplate = '<input ng-class="\'colt\' + col.index" ng-input="COL_FIELD" ng-model="COL_FIELD" ng-blur="updateEntity(row)" />';
            $scope.cellPasswordEditableTemplate = '<input type = "password" ng-class="\'colt\' + col.index" ng-input="COL_FIELD" ng-model="COL_FIELD" ng-blur="updateEntity(row)" />';
            $scope.cellSelectEditableTemplate = '<select ng-class="\'colt\' + col.index" ng-input="COL_FIELD" ng-model="COL_FIELD" ng-click = "test(COL_FIELD)" ng-options="role.Role for role in RoleCollection" />';
            $scope.mySelections = [];
            $scope.test = function (test) {
                console.log(test);
            }
            $scope.cellCheckEditTableTemplate = '<input class="c-approval-matrix-check"  type="checkbox" ng-input="COL-FIELD" ng-model="COL_FIELD"/>';
            $scope.addRow = function () {
                var x = Math.max.apply(Math, $scope.userCollection.map(function (o) {

                    return o.displayId;
                }))

                if (x < 0) {
                    console.log(x);
                    x = 0;
                }

                $scope.userCollection.splice(x, 0, {
                    displayId: ++x,
                    UserID: '',
                    FirstName: '',
                    MiddleName: '',
                    LastName: '',
                    Email: '',
                    lstUserRole:'Click to Select',
                    //Role: 'Click to Select',
                    EmployeeID: '',
                    PasswordChangeRequired: '',
                    DepartmentID: '',
                    DepartmentName: '',
                    EmployeeName: '',
                   // Password: '',
                    LoginPassword:'',
                    checkbox: false,
                    new: true
                });

                console.log($scope.userCollections);
                $scope.gridApi.core.clearAllFilters();//Nivedita-T on 17/11/2021
                $timeout(function () {

                    $scope.gridApi.core.scrollTo($scope.gridOptions.data[$scope.gridOptions.data.length - 1], $scope.gridOptions.columnDefs[0]);
                }, 1);
            }

            $scope.gridOptions = {
                enableColumnMenus: false,
                enableCellEditOnFocus: true,
                enableFiltering: true,
                enableAutoFitColumns: false,
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
                    name: 'ID',
                    enableCellEdit: false,
                    //cellClass: 'c-col-id',
                    width: 50,
                    cellClass: 'c-col-Num' //Manasi

                    /*cellTemplate: '<div ng-class="c-col-id" style="margin-top:15%;" ng-click="clicked(row,col)">{{row.getProperty(col.field)}}</div>'*/

                }, {
                    field: 'UserID',
                    name: 'User Id*'
                    /*enableCellEditOnFocus: true,
                    editableCellTemplate: $scope.cellInputEditableTemplate*/

                }, {
                    field: 'FirstName',
                    name: 'First Name*'
                    /*enableCellEditOnFocus: true,
                    editableCellTemplate: $scope.cellInputEditableTemplate*/


                },
                {
                    field: 'MiddleName',
                    name: 'Middle Name'
                    /* enableCellEditOnFocus: true,
                     editableCellTemplate: $scope.cellInputEditableTemplate*/


                },
                {
                    field: 'LastName',
                    name: 'Last Name*'
                    /*enableCellEditOnFocus: true,
                    editableCellTemplate: $scope.cellInputEditableTemplate*/


                },
                {
                    field: 'Email',
                    name: 'Email*',
                    /*enableCellEditOnFocus: true,
                    editableCellTemplate: $scope.cellInputEditableTemplate,*/
                    width: 200


                },
                {
                    field: 'RoleList',
                    name: 'Role*',
                    width: 200,
                    cellClass: 'roleListCol',
                   //cellTemplate: 'multiCell',
                   // editableCellTemplate: 'uiSelect',
                    //editableCellTemplate: 'ui-grid/dropdownEditor',
                    cellTemplate: 'multiCell',
                    editableCellTemplate: 'uiSelect',
                    //editDropdownValueLabel: 'Role',
                    //editDropdownIdLabel: 'RoleId',
                    //editDropDownChange: 'test',
                    editDropdownOptionsArray: $scope.rolesArray,
                  //  cellTemplate: 'multiCell',
                  //  editableCellTemplate: 'ui-grid/dropdownEditor',
                    //  editDropdownOptionsArray :
                    /* enableCellEditOnFocus: true,
                     editableCellTemplate: $scope.cellSelectEditableTemplate,*/
                    //cellFilter: 'mapRole',
                    
                }
                    ,
                /*    {
                        field: 'Role',
                        name: 'Role*',
                        editableCellTemplate: 'ui-grid/dropdownEditor',
                        editDropdownValueLabel: 'Role',
                        editDropdownIdLabel: 'RoleId',
                        editDropDownChange: 'test',
                       /* enableCellEditOnFocus: true,
                        editableCellTemplate: $scope.cellSelectEditableTemplate,
                        cellFilter: 'mapRole',
                        width: 200
                    },*/
                    {
                        field: 'DepartmentName',
                        name: 'Department*',
                        editableCellTemplate: 'ui-grid/dropdownEditor',
                        //editDropdownValueLabel: 'ProjectClassName',
                        //editDropdownIdLabel: 'ProjectClassName',
                        editDropdownIdLabel: 'ID',
                        editDropdownValueLabel: 'value',
                        editDropdownOptionsArray: dptArray,
                        //editDropDownChange: 'test',
                        /* enableCellEditOnFocus: true,
                         editableCellTemplate: $scope.cellSelectEditableTemplate,*/
                        cellFilter: 'customFilter:this',
                        cellClass: 'c-col-Num',
                        width: 200
                    },
                    {
                        field: 'EmployeeName',
                        name: 'Employee*',
                        editableCellTemplate: 'ui-grid/dropdownEditor',
                        editDropdownIdLabel: 'ID',
                        editDropdownValueLabel: 'value',
                        editDropdownOptionsArray: empArray,
                        //editDropDownChange: 'test',
                        //cellFilter: 'mapRole',
                        cellFilter: 'customFilter:this',
                        cellClass: 'c-col-Num',  
                        width: 200
                    },
                    {
                        field: 'PasswordChangeRequiredName',
                        name: 'Pwd Change*',
                        editableCellTemplate: 'ui-grid/dropdownEditor',
                        editDropdownValueLabel: 'PasswordChangeRequiredName',
                        editDropdownIdLabel: 'PasswordChangeRequiredName',
                        editDropDownChange: 'test',
                        cellFilter: 'mapRole',
                        width: 120
                    }
                    ,
                {
                    field: 'LoginPassword',
                    name: 'Password',
                    type: 'password',
                    validators: {
                        required: false
                    },
                    /*enableCellEditOnFocus: true,
                    editableCellTemplate: true,*/
                    width: 200
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
                    //$('div.ui-grid-cell form').find('ui-select').putCursorAtEnd();
                    
                    //$('div.ui-grid-cell form').find('input').putCursorAtEnd();
                    //$('div.ui-grid-cell form').find('select').putCursorAtEnd();
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
                    //row.config.enableRowSelection = true;
                } else {
                    row.entity.checkbox = false;
                }
            }
            $scope.clicked = function (row, col) {
                $scope.orgRow = row;
                $scope.col = col;
                // $scope.row = row.entity;
            }
            $scope.save = function () {
                $("#save_User_Management").attr("disabled", true);
                var isReload = false;
                //
                var isReload = false;
                var isChanged = true;
                var isFilled = true;
                var listToSave = [];
                debugger;
                console.log($scope.userCollection, $scope.orgUserCollection);
                angular.forEach($scope.userCollection, function (user, key) {
                    console.log(user);
                    //Find employee id for a user
                    for (var x = 0; x < $scope.employeeCollection.length; x++) {
                        if ($scope.employeeCollection[x].ID == user.EmployeeName) {
                            user.EmployeeID = $scope.employeeCollection[x].ID;
                            user.EmployeeName = $scope.employeeCollection[x].Name;
                            break;
                        }
                    }
                    
                    //debugger;
                    $scope.roleId = [];

                   // for (var x = 0; x < $scope.RoleCollection.length; x++) {
                        
                   //     //angular.forEach(user.Role, function (item) { 
                   //         if (user.Role.includes($scope.RoleCollection[x].Role)) {
                   //             //$scope.roleId.push($scope.RoleCollection[x].Id);
                   //             //user.RoleList.push($scope.RoleCollection[x].Id);
                                
                   //         //break;
                   //     }
                        
                   //// });
                    // }
                    $scope.lstRole = $scope.RoleCollection;
                   
                    if (user.new === true)
                    {
                        angular.forEach($scope.lstRole, function (item) {
                            
                            item.isSelected = false;
                            if (user.RoleList != undefined) {    //vaishnavi 10-06-2022
                                if (user.RoleList.includes(item.Role)) {
                                    item.isSelected = true;
                                }
                            }
                            
                        });
                    }
                    else {
                        //angular.forEach(user.lstUserRole, function (item) {
                        //    item.isSelected = false;
                        //    if (user.Role.includes(item.Role)) {
                        //        item.isSelected = true;
                        //    }
                        //});

                        angular.forEach(user.lstUserRole, function (item) {
                            item.isSelected = false;
                            if (user.RoleList.includes(item.Role)) {
                                item.isSelected = true;
                            }
                        });
                    }
                        //for (var x = 0; x < $scope.employeeCollection.length; x++) {
                        //    if ($scope.employeeCollection[x].Name == user.EmployeeName) {
                        //        user.EmployeeID = $scope.employeeCollection[x].ID;
                        //    }
                        //}

                    //Find Department id for user
                    for (var i = 0; i < $scope.departmentsData.length; i++) {
                        if ($scope.departmentsData[i].ProjectClassID == user.DepartmentName) {
                            user.DepartmentID = $scope.departmentsData[i].ProjectClassID;
                            user.DepartmentName = $scope.departmentsData[i].DepartmentName;
                            break;
                        }
                    }
                        
                    
                    
                    
                    
                    console.log(user.PasswordChangeRequired);
                    //Find password change required for a user
                    if (user.PasswordChangeRequiredName == "True") {
                        user.PasswordChangeRequired = true;
                    } else {
                        user.PasswordChangeRequired = false;
                    }
                    console.log(user.PasswordChangeRequired);


                    //Detech change
                    isChanged = true;
                    angular.forEach($scope.orgUserCollection, function (orgUser) {
                        console.log(user.PasswordChangeRequired === orgUser.PasswordChangeRequired);
                      
                        if (user.Id === orgUser.Id &&
                            user.UserID === orgUser.UserID &&
                            user.FirstName === orgUser.FirstName &&
                            user.MiddleName === orgUser.MiddleName &&
                            user.LastName === orgUser.LastName &&
                            user.EmployeeID === orgUser.EmployeeID &&
                            user.DepartmentID === orgUser.DepartmentID &&
                            user.LoginPassword === orgUser.LoginPassword &&
                            orgUser.RoleList.length == user.RoleList.length &&
                            user.Email === orgUser.Email &&
                            user.PasswordChangeRequired === orgUser.PasswordChangeRequired) {
                            isChanged = false;


                            
                            angular.forEach(orgUser.RoleList, function (item) {

                                if (user.RoleList.includes(item)) {

                                }
                                else {
                                    isChanged = true;
                                }
                            });
                        }
                        
                        
                        //angular.forEach(user.lstUserRole, function (item) {
                        //    item.isSelected = false;
                        //    if (user.RoleList.includes(item.Role)) {
                        //        item.isSelected = true;
                        //    }
                        //});
                    });

                    if (isChanged && (user.UserID == "" || user.UserID == null
                       // || user.LoginPassword == "" || user.LoginPassword == null
                        || user.FirstName == "" || user.FirstName == null
                        || user.LastName == "" || user.LastName == null
                        || user.Email == "" || user.Email == null
                        || user.RoleList == undefined || user.RoleList.length == 0 || user.RoleList.length == null       //vaishnavi 10-06-2022
                        || user.DepartmentID == "" || user.DepartmentID == null
                        || user.EmployeeID == "" || user.EmployeeID == null
                        || user.PasswordChangeRequired == undefined || user.PasswordChangeRequired == null)) {
                        $("#save_User_Management").attr("disabled", false);
                        dhtmlx.alert({
                            text: "UserID, First Name, Last Name, Email, Role, Employee, Department and Password cannot be empty (Row " + user.displayId + ")",
                            width: "300px"
                        });
                        isFilled = false;
                        return;
                    }

                    if (isFilled == false) {
                        return;
                    }
                    //--------------------------Swapnil 01-09-2020 email validation--------------------------------------------------------------------------
                    const regex = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;

                    if (isChanged && !regex.test(user.Email)) {
                        $("#save_User_Management").attr("disabled", false);
                        dhtmlx.alert({
                            text: "Enter valid email address (Row " + user.displayId + ")",
                            width: "300px"
                        });
                        isFilled = false;
                        return;
                    }
                    //------------------------------------------------------------------------------------------------------
                    //-----------------------------Nivedita 09-11-2021 password validation-------------------------------------------------------------------------

                    //vaishnavi 10-06-2022

                    if (user.new === true && user.LoginPassword == "") {
                        $("#save_User_Management").attr("disabled", false);
                        dhtmlx.alert({
                            text: "Password Cannot be Empty. (Row " + user.displayId + ")",
                            width: "300px"
                        });
                        isFilled = false;
                        return;
                    }

                    const regexPassword = /^(?=.*[A-Z])(?=.*\d)(?=.*[$@$!%*#?&])[A-Za-z\d$@$!%*#?&]{8,20}$/;
                    if (isChanged && !regexPassword.test(user.LoginPassword) && user.LoginPassword != "") {
                        $("#save_User_Management").attr("disabled", false);
                        dhtmlx.alert({
                            text: "Enter valid password.<br/> Password should be at least 8 characters long, should contain atleast one number,one capital character and one special character. (Row " + user.displayId + ")",
                            width: "300px"
                        });
                        isFilled = false;
                        return;
                    }
                    //------------------------------------------------------------------------------------------------------------------------
                    if (user.new === true) {
                        var dataObj = {
                            Operation: '1',
                            UserID: user.UserID,
                            FirstName: user.FirstName,
                            MiddleName: user.MiddleName,
                            LastName: user.LastName,
                            Email: user.Email,
                            //Role: user.Role,
                            EmployeeID: user.EmployeeID,
                            LoginPassword: user.LoginPassword,
                            PasswordChangeRequired: user.PasswordChangeRequired,
                            DepartmentID: user.DepartmentID, // addded by vaishnavi
                            lstUserRole: $scope.lstRole
                        }
                        debugger;
                        listToSave.push(dataObj);
                    }
                    else {

                        if (isChanged) {
                            debugger;
                            isChanged = true;
                            if (typeof user.Role == 'string') {
                                temp = user.Role;
                            } else {
                                temp = user.Role;
                            }
                            console.log(user);

                            var dataObj = {
                                Operation: '2',
                                Id: user.Id,
                                UserID: user.UserID,
                                FirstName: user.FirstName,
                                MiddleName: user.MiddleName,
                                LastName: user.LastName,
                                Email: user.Email,
                                EmployeeID: user.EmployeeID,
                                PasswordChangeRequired: user.PasswordChangeRequired,
                                DepartmentID: user.DepartmentID,
                                lstUserRole: user.lstUserRole

                            }

                            //
                            if (!(user.LoginPassword === "" || user.LoginPassword == null))
                                dataObj.LoginPassword = user.LoginPassword;
                            debugger;
                            listToSave.push(dataObj);


                        } else {
                            //delete

                        }
                    }
                });

                angular.forEach($scope.listToDelete, function (item) {
                    debugger;
                    listToSave.push(item);
                });

                if (isFilled == false) {
                    return;
                } else {
                    debugger;
                    console.log(listToSave)
                    $http({
                        url: url,
                        //url: 'http://localhost:29986/api/response/phasecode',
                        method: "POST",
                        data: JSON.stringify(listToSave),
                        headers: { 'Content-Type': 'application/json' }
                    }).then(function success(response) {
                        $("#save_User_Management").attr("disabled", false);
                        response.data.result.replace(/[\r]/g, '\n');

                        if (response.data.result) {
                            $("#save_User_Management").attr("disabled", false);
                            dhtmlx.alert(response.data.result);
                        } else {
                            $("#save_User_Management").attr("disabled", false);
                            dhtmlx.alert('No changes to be saved.');
                        }

                        $state.reload();

                        //Luan here - 
                        User.get({}, function (userData) {
                            $scope.checkList = [];
                            $scope.userCollection = userData.result;
                            $scope.orgUserCollection = angular.copy(userData.result);
                            addIndex($scope.userCollection);
                            angular.forEach($scope.userCollection, function (item, index) {
                                item.checkbox = false;
                                $scope.checkList[index + 1] = false;

                                //Find employee name for a user
                                for (var x = 0; x < $scope.employeeCollection.length; x++) {
                                    if ($scope.employeeCollection[x].ID == item.EmployeeID) {
                                        item.EmployeeName = $scope.employeeCollection[x].Name;
                                        //$scope.userCollection.EmployeeID = item.EmployeeID;
                                        //$scope.userCollection.EmployeeName = item.EmployeeName;
                                    }
                                }

                                //Find Department
                                for (var i = 0; i < $scope.departmentsData.length; i++) {
                                    if ($scope.departmentsData[i].ProjectClassID == item.DepartmentID) {
                                        item.DepartmentName = $scope.departmentsData[i].DepartmentName;
                                    }
                                }


                                //Find role for a user
                                
                                //for (var x = 0; x < $scope.roleCollection.length; x++) {
                                //    debugger;
                                //    if ($scope.roleCollection[x].RoleId == item.Role) {
                                //        item.Role = $scope.roleCollection[x].Role;
                                //        //$scope.userCollection.EmployeeID = item.EmployeeID;
                                //        //$scope.userCollection.EmployeeName = item.EmployeeName;
                                //    }
                                //}


                                //Find required password change
                                if (item.PasswordChangeRequired) {
                                    item.PasswordChangeRequiredName = "True";
                                } else {
                                    item.PasswordChangeRequiredName = "False";
                                }
                            });
                            $scope.gridOptions.data = $scope.userCollection;

                            console.log($scope.userCollection);
                        });

                    }, function error(response) {
                        $("#save_User_Management").attr("disabled", false);
                        dhtmlx.alert("Failed to save. Information missing!");
                        console.log("Failed to save");
                    });
                }





            }
            $scope.delete = function () {
                var isChecked = false;
                var unSavedChanges = false;
                var listToSave = [];
                var newList = [];
                var selectedRow = false;
                $scope.listToDelete = [];
                angular.forEach($scope.userCollection, function (item) {
                    if (item.checkbox == true) {
                        selectedRow = true;
                        if (item.new === true) {
                            unSavedChanges = true;
                            newList.push(item);
                        }
                        else {
                            isChecked = true;
                            var dataObj = {
                                Operation: '3',
                                UserID: item.UserID,
                                Id: item.Id,
                                FirstName: item.FirstName,
                                MiddleName: item.MiddleName,
                                LastName: item.LastName,
                                Email: item.Email,
                                Role: item.Role,
                                EmployeeID: item.EmployeeID,
                                PasswordChangeRequired: item.PasswordChangeRequired,
                                displayId: item.displayId,
                                DepartmentID: item.DepartmentID

                            }
                            listToSave.push(dataObj);
                            $scope.listToDelete.push(dataObj);
                            //dhtmlx.alert("Record Deleted");
                        }
                    }
                });
                if (!selectedRow) {
                    dhtmlx.alert("Please select a record to delete.");
                }
                if (newList.length != 0) {
                    for (var i = 0; i < newList.length; i++) {
                        var ind = -1;
                        angular.forEach($scope.userCollection, function (item, index) {
                            if (item.displayId == newList[i].displayId) {
                                item.checkbox = false;

                                ind = index;
                            }
                        });
                        if (ind != -1) {
                            $scope.checkList.splice(newList[i].displayId, 1);
                            $scope.userCollection.splice(ind, 1);
                        }
                    }

                }
                if (listToSave.length != 0) {

                    for (var i = 0; i < listToSave.length; i++) {
                        var ind = -1;
                        angular.forEach($scope.userCollection, function (item, index) {
                            if (item.displayId == listToSave[i].displayId) {
                                item.checkbox = false;

                                ind = index;
                            }
                        });
                        if (ind != -1) {
                            $scope.checkList.splice(listToSave[i].displayId, 1);
                            $scope.userCollection.splice(ind, 1);
                        }
                    }
                }

            }

            //$scope.setUser = function(u){
            //    $scope.userItem = u;
            //}
            //
            //$scope.newUser = function(){
            //
            //    newOrEdit = 'new';
            //    var scope = $rootScope.$new();
            //    $scope.userItem = null;
            //    scope.params = {userItem : $scope.userItem, newOrEdit:newOrEdit}
            //    $rootScope.modalInstance = $uibModal.open({
            //        scope: scope,
            //        controller: "UserModalCtrl",
            //        templateUrl : "app/views/modal/users_modal.html",
            //        size : "md"
            //    } );
            //    $rootScope.modalInstance.result.then(function(response){
            //        User.get({},function(Users){
            //            $scope.userCollection = Users.result;
            //            console.log($scope.userCollection);
            //        })
            //    });
            //}
            //$scope.editUser = function(){
            //
            //    newOrEdit = 'edit';
            //    var scope = $rootScope.$new();
            //    scope.params = {userItem : $scope.userItem, newOrEdit : newOrEdit}
            //    $rootScope.modalInstance = $uibModal.open({
            //       scope:scope,
            //        templateUrl : "app/views/modal/users_modal.html",
            //        size : 'md',
            //        controller : 'UserModalCtrl'
            //    });
            //
            //    $rootScope.modalInstance.result.then(function(response){
            //        User.get({},function(Users){
            //            $scope.userCollection = Users.result;
            //            console.log($scope.userCollection);
            //        })
            //    });
            //}
            //$scope.deleteUser = function(){
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
            //            var dataObj = {
            //                'Operation': '3',
            //                'FullName' : "",
            //                'UserID' : $scope.userItem.UserID,
            //                'AccessControlList' : "",
            //                'LoginPassword' : "",
            //                'Email': $scope.userItem.Email
            //
            //            }
            //
            //            var index = $scope.userCollection.indexOf($scope.userItem);
            //            $http.post(url,dataObj).then(function(response){
            //                if(response.data.result==="Success"){
            //                    //refresh on delete
            //                    User.get({},function(Users){
            //                        $scope.userCollection = Users.result;
            //                        console.log($scope.userCollection);
            //                    })
            //                    if(index !== -1){
            //                        $scope.userCollection.splice(index,1);
            //                        $scope.userItem = null;
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
            //}

            $scope.checkForChanges = function () {
                var unSavedChanges = false;
                var originalCollection = $scope.orgUserCollection;
                var currentCollection = $scope.userCollection;

            if (currentCollection.length != originalCollection.length) {
                unSavedChanges = true;
                return unSavedChanges;
            } else {
                angular.forEach(currentCollection, function (currentObject) {
                    for (var i = 0, len = originalCollection.length; i < len; i++) {
                        if (unSavedChanges) {
                            return unSavedChanges; // no need to look through the rest of the original array
                        }
                        if (originalCollection[i].Id == currentObject.Id) {
                            var originalObject = originalCollection[i];
                            // compare relevant data
                            if (originalObject.UserID !== currentObject.UserID ||
                                originalObject.FirstName !== currentObject.FirstName ||
                                originalObject.LastName !== currentObject.LastName ||
                                originalObject.Email !== currentObject.Email ||
                                originalObject.EmployeeID !== currentObject.EmployeeID ||
                                originalObject.DepartmentID !== currentObject.DepartmentID ||
                                originalObject.Role !== currentObject.Role) {
                                // alert if a change has not been saved
                                //alert("unsaved change on line" + currentCollection.displayId);
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
                if (!$scope.checkForChanges()) return;    //luan here
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
    .directive('uiSelectWrap', uiSelectWrap)
    ;
//.angular.module('app', ['ui.grid', 'ui.grid.edit', 'ui.select'])
uiSelectWrap.$inject = ['$document', 'uiGridEditConstants']; //uiGridEditConstants
function uiSelectWrap($document, uiGridEditConstants) {
    return function link($scope, $elm, $attr) {
        
        
        //$scope.$parent.col.grid.options.rowHeight=100;
        $document.on('click', docClick);
        //var target = $(event.target);
        function docClick(evt) {
           // $scope.$parent.col.grid.options.rowHeight = 40;
            if ($(evt.target).closest('.ui-select-container').size() === 0) {
                $scope.$emit(uiGridEditConstants.events.END_CELL_EDIT);
               
                $document.off('click', docClick);
               
            }
        }
    };
}
