angular.module('cpp.controllers').
    //budget Category Controller
    controller('BudgetCategoryCtrl', ['PhaseCode', 'ServiceClass', '$state', '$scope', '$rootScope', 'Category', 'Organization', 'VersionDetails', '$uibModal', 'UpdateCategory', '$http', 'Page', 'ProjectTitle', 'TrendStatus', '$location', '$timeout',
        function (PhaseCode, ServiceClass, $state, $scope, $rootScope, Category, Organization, VersionDetails, $uibModal, UpdateCategory, $http, Page, ProjectTitle, TrendStatus, $location, $timeout) {
            //Work Breakdown Structure
            Page.setTitle('Work Breakdown Structure');
            ProjectTitle.setTitle('');
            TrendStatus.setStatus('');

            //luan here - watcher experimental
            $scope.$watch('filterOrg', function (newValue, oldValue) {
                console.log(newValue, oldValue);
            });
            $scope.$watch('filterVersion', function (newValue, oldValue) {
                console.log(newValue, oldValue);
            });

            $scope.selectedOrg;
            $scope.selectedVersion;

            Organization.lookup().get({}, function (organizationData) {
                $scope.organizationList = organizationData.result;
                $scope.setOrganization(($scope.organizationList[0]));
                //console.log("Get Org List");
                //console.log($scope.organizationList);
                $scope.filterOrg = $scope.organizationList[0];
                $scope.filterChangeOrg($scope.filterOrg.OrganizationID);
            });



            $scope.setOrganization = function (org) {

                $scope.selectedOrg = org;
                console.log("Here");
                console.log(org);
                console.log("End here");

            };

            $scope.setVersion = function (version) {

                $scope.selectedVersion = version;
                console.log("Here");
                console.log(version);
                console.log("End here");
            };

            // Aditya: Populate Service Dropdown //06062022
            ServiceClass.get({}, function (projectClasses) {
                $scope.projectClassCollection = projectClasses.result;
                console.log(projectClasses.result);
                $scope.ServiceList = [];
                angular.forEach($scope.projectClassCollection, function (projectClass) {
                    $scope.ServiceList.push({ Id: projectClass.ID, Value: projectClass.Description });
                    //projectClass.Services = projectClass.Description; //here
                });
                //$scope.gridOptions.columnDefs[5].editDropdownOptionsArray = $scope.projectClassCollection;
                $scope.gridOptions.columnDefs[5].editDropdownOptionsArray = $scope.ServiceList;
            });

            $scope.filterChangeOrg = function (filterOrgID) {
                var orgId = null;
                if (filterOrgID) {
                    orgId = filterOrgID;
                } else {
                    orgId = $("#selectOrg").val();
                }
                angular.forEach($scope.organizationList, function (org) {
                    if (orgId == org.OrganizationID) {
                        $scope.setOrganization(org);
                    }
                });

                console.log('filterChangeOrg');
                console.log(orgId);

                VersionDetails.lookup().get({ operation: '0', programElementID: '0', organizationID: orgId }, function (versionData) {
                    if (versionData.result.length > 0) {


                        $scope.versionList = versionData.result;
                        $scope.setVersion(($scope.versionList[0]));
                        //console.log("Get Org List");
                        //console.log($scope.organizationList);
                        $scope.filterVersion = $scope.versionList[0];
                        $("#selectVersion").val($scope.filterVersion.Id);
                        $scope.filterChangeVersion($scope.filterVersion.Id);


                        if (orgId && $scope.filterVersion.Id) {

                            Category.get({ OrganizationID: orgId, VersionID: $scope.filterVersion.Id }, function (categoryData) {
                                $scope.checkList = [];
                                $scope.budgetCollection = categoryData.result;
                                $scope.orgBudgetCollection = angular.copy(categoryData.result);
                                addIndex($scope.budgetCollection);
                                angular.forEach($scope.budgetCollection, function (item, index) {
                                    item.checkbox = false;
                                    $scope.checkList[index + 1] = false;
                                    //Aditya: get Service name //04062022
                                    angular.forEach($scope.ServiceList, function (service, index) {
                                        if (service.Id == item.Services) {
                                            item.Services = service.Value;
                                        }
                                    });
                                });
                                $scope.gridOptions.data = $scope.budgetCollection;

                                console.log('budgetCollection');
                                console.log($scope.budgetCollection);
                            });

                        }
                    }

                });


            }

            $scope.filterChangeVersion = function (filterVersionID) {
                var versionId = null;
                if (filterVersionID) {
                    versionId = filterVersionID;
                } else {
                    versionId = $("#selectVersion").val();
                }
                angular.forEach($scope.versionList, function (version) {
                    if (versionId == version.Id) {
                        $scope.setVersion(version);
                    }
                });

                console.log('filterChangeVersion');
                console.log(versionId);

                if ($("#selectOrg").val() && versionId) {

                    Category.get({ OrganizationID: $("#selectOrg").val(), VersionID: versionId }, function (categoryData) {
                        $scope.checkList = [];
                        $scope.budgetCollection = categoryData.result;
                        $scope.orgBudgetCollection = angular.copy(categoryData.result);
                        addIndex($scope.budgetCollection);
                        angular.forEach($scope.budgetCollection, function (item, index) {
                            item.checkbox = false;
                            $scope.checkList[index + 1] = false;
                            //Aditya: get Service name //04062022
                            angular.forEach($scope.ServiceList, function (service, index) {
                                if (service.Id == item.Services) {
                                    item.Services = service.Value;
                                }
                            });
                        });
                        $scope.gridOptions.data = $scope.budgetCollection;

                        console.log('budgetCollection');
                        console.log($scope.budgetCollection);
                    });

                }

            }

            var url = serviceBasePath + 'response/activityCategory/';
            $scope.$on('ngGridEventEndCellEdit', function (data) {
                console.log(data.targetScope.row.entity.status);
                data.targetScope.row.entity.status = 'Modified';
                console.log($scope.userCollection);
            });
            $scope.budgetCategoryItem;
            $scope.applicableList = [];
            //PhaseCode.get({},function(PhaseCodeData){
            //    $scope.phaseCodeCollection = PhaseCodeData.result;
            //    console.log('PhaseCodeData.result');
            //    console.log(PhaseCodeData.result);

            //    $scope.phaseCodeCollection.splice(0,0,{
            //       Code: "All",
            //        PhaseDescription : "ALL"
            //    });
            //    angular.forEach($scope.phaseCodeCollection,function(phase){
            //            phase.Phase = phase.Code;
            //    });
            //    $scope.gridOptions.columnDefs[5].editDropdownOptionsArray =$scope.phaseCodeCollection;
            //});

            $scope.projectTypeCollection = [{
                Type: "Runway",
                ProjectTypeDescription: "Runway"
            }, {
                Type: "TEST",
                ProjectTypeDescription: "TEST"
            }
            ]

            //var orgID = $scope.selectedOrg;
            console.log('$("#selectOrg").val()');
            console.log($("#selectVersion").val());

            if ($("#selectOrg").val() && $("#selectVersion").val()) {

                Category.get({ OrganizationID: $("#selectOrg").val(), VersionID: $("#selectVersion").val() }, function (categoryData) {
                    $scope.checkList = [];
                    $scope.budgetCollection = categoryData.result;
                    $scope.orgBudgetCollection = angular.copy(categoryData.result);
                    addIndex($scope.budgetCollection);
                    angular.forEach($scope.budgetCollection, function (item, index) {
                        item.checkbox = false;
                        $scope.checkList[index + 1] = false;
                        //Aditya: get Service name //04062022
                        angular.forEach($scope.ServiceList, function (service, index) {
                            if (service.Id == item.Services) {
                                item.Services = service.Value;
                            }
                        });
                    });
                    $scope.gridOptions.data = $scope.budgetCollection;

                    console.log('budgetCollection');
                    console.log($scope.budgetCollection);
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
            }

            //var newOrEdit = "";
            //$scope.setBudgetCategory = function(bc) {
            //
            //    $scope.budgetCategoryItem = bc;
            //
            //};
            ////add new Budget Category
            //$scope.newBudgetCategory = function(){
            //    var scope = $rootScope.$new();
            //
            //    newOrEdit="new";
            //    scope.params={budgetCategoryItem:null,
            //        newOrEdit:newOrEdit};
            //    $rootScope.modalInstance = $uibModal.open({
            //        scope:scope,
            //        backdrop:'static',
            //        keyboard:false,
            //        templateUrl: 'app/views/modal/budget_category_modal.html',
            //        controller:'BudgetCategoryModalCtrl',
            //        size: 'md'
            //    });
            //    $rootScope.modalInstance.result.then(function () {
            //
            //        Category.get({},function(categoryData){
            //            $scope.budgetCollection = categoryData.result;
            //
            //
            //
            //        });
            //
            //    });
            //
            //}
            ////Delete a budget Category Item
            //$scope.deleteBudgetCategory = function(){
            //    var url = serviceBasePath+'response/activityCategory/';
            //    //var url = 'http://localhost:29986/api/response/activityCategory/';
            //    var index = $scope.budgetCollection.indexOf($scope.budgetCategoryItem);
            //    $scope.confirm = "";
            //    var scope = $rootScope.$new();
            //    scope.params = {confirm:$scope.confirm};
            //
            //    $rootScope.modalInstance = $uibModal.open({
            //        scope:scope,
            //        templateUrl: 'app/views/Modal/confirmation_dialog.html',
            //        controller:'ConfirmationCtrl',
            //        size :'sm'
            //    });
            //
            //    $rootScope.modalInstance.result.then(function(data){
            //        if(scope.params.confirm ==='yes'){
            //            var dataObj = {Operation:'3',
            //                CategoryID:$scope.budgetCategoryItem.CategoryID,
            //                CategoryDescription:$scope.budgetCategoryItem.CategoryDescription,
            //                SubCategoryID: $scope.budgetCategoryItem.SubCategoryID,
            //                SubCategoryDescription:$scope.budgetCategoryItem.SubCategoryDescription
            //            }
            //            $http.post(url,dataObj).then(function(response){
            //                if(response.data.result === 'Success'){
            //                    $scope.budgetCollection.splice(index,1);
            //                    $scope.budgetCategoryItem = null;
            //                }
            //                else{
            //                    alert("Delete failed");
            //                }
            //            });
            //        }
            //
            //    });
            //
            //
            //}
            ////open a modal to edit data
            //$scope.editBudgetCategory = function(){
            //    var scope = $rootScope.$new();
            //    newOrEdit = "edit";
            //    scope.params = {budgetCategoryItem:$scope.budgetCategoryItem, newOrEdit:newOrEdit};
            //    $rootScope.modalInstance = $uibModal.open({
            //        scope: scope,
            //
            //        backdrop: 'static',
            //        keyboard: false,
            //        templateUrl: 'app/views/modal/budget_category_modal.html',
            //        controller:'BudgetCategoryModalCtrl',
            //        size: 'md'
            //    });
            //
            //    // after close the popup modal
            //    $rootScope.modalInstance.result.then(function () {
            //        Category.get({},function(categoryData){
            //            $scope.budgetCollection = categoryData.result;
            //
            //        });
            //        $scope.budgetCategoryItem=null;
            //
            //
            //    });
            //};
            ////$scope.addBudgetCategory

            $scope.cellInputEditableTemplate = '<input ng-class="\'colt\' + col.index" ng-input="COL_FIELD" ng-model="COL_FIELD" ng-blur="updateEntity(row)" />';
            //$scope.cellSelectEditableTemplate = '<select ng-class="\'colt\' + col.index" ng-input="COL_FIELD" ng-model="COL_FIELD" ng-click = "test(COL_FIELD)" ng-options="phase.Code for phase in phaseCodeCollection" />';
            $scope.cellCheckEditTableTemplate = '<input class="c-approval-matrix-check"  type="checkbox" ng-input="COL-FIELD" ng-model="COL_FIELD"/>';
            $scope.cellCheckEditTableTemplateApplicable = '<input class="c-approval-matrix-check"  type="checkbox" ng-input="COL-FIELD" ng-model="COL_FIELD"/>';
            //$scope.cellSelectEditableTemplateProjectType = '<select ng-class="\'colt\' + col.index" ng-input="COL_FIELD" ng-model="COL_FIELD" style="width:120px;" ng-options="id.Type for id in phaseCodeCollection" ng-blur="updateEntity(row)" />';
            $scope.addRow = function () {
                //var table = document.getElementById('wbsTable');
                //table.scrollTop = table.scrollHeight;
                //  $scope.gridOptions.selectItem(rowToSelect, true);
                //  var grid = $scope.gridOptions.ngGrid;
                //  grid.$viewport.scrollTop(grid.rowMap[rowToSelect] * grid.config.rowHeight);
                var x = Math.max.apply(Math, $scope.budgetCollection.map(function (o) {

                    return o.displayId;
                }))

                if (x < 0) {
                    console.log(x);
                    x = 0;
                }

                $scope.checkList[++x] = false;
                $scope.budgetCollection.splice(x, 0, {
                    displayId: x,
                    CategoryID: '',
                    CategoryDescription: '',
                    SubCategoryID: '',
                    SubCategoryDescription: '',
                    Services: '', //Aditya 04062022
                    //Phase: '',
                    OrganizationID: '',
                    checkbox: false,
                    new: true
                });
                console.log('$scope.budgetCollection');
                console.log($scope.budgetCollection);
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
                /*data: 'budgetCollection',
                enableRowSelection:false,
                enableCellSelection: true,
                selectedItems: $scope.mySelections,
                enableCellEditOnFocus: true,
                multiSelect: false,*/
                rowHeight: 40,
                width: 700,
                /*afterSelectionChange: function (rowItem, event) {
                    console.log($scope.mySelections);
                    $scope.selectedIDs = [];
                    console.log(rowItem);
                    angular.forEach($scope.mySelections, function ( item ) {
                        $scope.selectedIDs.push( item.id )
                    });

                },*/
                columnDefs: [{
                    field: 'displayId',
                    name: 'ID',
                    enableCellEdit: false,
                    /*cellClass:'c-col-id',*/
                    width: 50,
                    cellClass: 'c-col-Num' //Manasi

                    /*cellTemplate:'<div ng-class="c-col-id" style="margin-top:15%;" ng-click="clicked(row,col)">{{row.getProperty(col.field)}}</div>'*/

                }, {
                    field: 'CategoryID',
                    //name: 'Category Id',
                    name: 'Category Id',  //Manasi
                    width: 120,
                    cellClass: 'c-col-Num' //Manasi

                    /* editableCellTemplate: $scope.cellInputEditableTemplate,
                     cellFilter : 'mapStatus'*/

                }, {
                    field: 'CategoryDescription',
                    //name: 'Category Title',
                    name: 'Category Title',
                    /* enableCellEditOnFocus: true,
                     editableCellTemplate: $scope.cellInputEditableTemplate,
                     cellFilter :'mapStatus',*/
                    width: 300


                }, {
                    field: 'SubCategoryID',
                    //name:'SubCategory Id',
                    name: 'SubCategory Id',
                    width: 150,
                    cellClass: 'c-col-Num' //Manasi

                    /*editableCellTemplate:$scope.cellInputEditableTemplate,
                    cellFilter: 'mapStatus',
                    enableCellEditOnFocus: true*/
                }, {
                    field: 'SubCategoryDescription',
                    //name: 'SubCategory Title',
                    name: 'SubCategory Title',
                    /*editableCellTemplate:$scope.cellInputEditableTemplate,
                    cellFilter: 'mapStatus',
                    enableCellEditOnFocus: true,*/
                    width: 300
                },
                //Aditya: Add Service Column // 04062022
                {
                    field: 'Services',
                    name: 'Service',
                    editableCellTemplate: 'ui-grid/dropdownEditor',
                    editDropdownIdLabel: 'Id', 
                    editDropdownValueLabel: 'Value',
                    editDropDownChange: 'test',
                    cellFilter: 'customFilter:this',
                    width: 300
                },
                //{
                //    field: 'Phase',
                //    name: 'Phase',
                //    editableCellTemplate: 'ui-grid/dropdownEditor',
                //    editDropdownValueLabel: 'Code',
                //    editDropdownIdLabel: 'Phase',
                //    editDropDownChange: 'test',
                //    /* enableCellEditOnFocus: true,
                //     editableCellTemplate: $scope.cellSelectEditableTemplate,*/
                //    cellFilter: 'mapPhase',
                //    width: 200
                //},
                //{
                //    field:'IsApplicable',
                //    displayName: 'Applicable',
                //    enableCellEdit: false,
                //    width:'100px',
                //    cellTemplate:'<input type="checkbox"  class = "c-col-check applicable-format" ng-click="applicableCheck(row,col)" style="text-align: center;vertical-align: middle;">'
                //
                //},
                //{
                //    field:'ProjectTypeDescription',
                //    displayName:'Project Type',
                //    editableCellTemplate: $scope.cellSelectEditableTemplateProjectType ,
                //    cellFilter: 'mapProjectType',
                //    width:'120px',
                //    enableCellEditOnFocus: true
                //},
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
                var isInvalidList = false;
                angular.forEach($scope.budgetCollection, function (value, key, obj) {
                    if (!isInvalidList) {
                        console.log(value.CategoryID, value.SubCategoryID,
                            !Number.isInteger(parseFloat(value.CategoryID)), !Number.isInteger(parseFloat(value.SubCategoryID)),
                            parseFloat(value.CategoryID) < 0, parseFloat(value.SubCategoryID) < 0,
                            value.CategoryID.length < 3, value.SubCategoryID.length < 3);

                        if (!Number.isInteger(parseFloat(value.CategoryID)) || !Number.isInteger(parseFloat(value.SubCategoryID))
                            || parseFloat(value.CategoryID) < 0 || parseFloat(value.SubCategoryID) < 0
                            || value.CategoryID.length < 4 || value.SubCategoryID.length < 4
                            || (value.CategoryID.indexOf('.') >= 0 || value.SubCategoryID.indexOf('.') >= 0)) {
                            //dhtmlx.alert('Category ID and SubCategory ID must be integer values greater than or equal to 0. Length greater than or equal to 4. (Row ' + value.displayId + ')');
                            dhtmlx.alert('Category ID and SubCategory ID must be integer values greater than or equal to 0. Length greater than or equal to 4. (Row ' + value.displayId + ')');
                            isInvalidList = true;
                            return;
                        }

                        if (value.CategoryID == "" || value.CategoryDescription == "" ||
                            value.SubCategoryID == "" || value.SubCategoryDescription == "" /*|| value.phase == ""*/) {
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
                        //Aditya: get service id 06062022
                        angular.forEach($scope.ServiceList, function (orgItem) {
                            if (value.Services == orgItem.Value) {
                                value.Services = orgItem.Id;
                                return false;
                            }
                        });
                        //New Item
                        if (value.new === true) {
                            console.log(value);
                            isReload = true;
                            console.log("#selectOrg");
                            console.log($("#selectOrg").val() === "0");
                            //var ServiceId;

                            var dataObj = {
                                Operation: '1',
                                CategoryID: value.CategoryID,
                                CategoryDescription: value.CategoryDescription,
                                SubCategoryID: value.SubCategoryID,
                                SubCategoryDescription: value.SubCategoryDescription,
                                Services: value.Services, //Aditya 04062022
                                //Phase: value.Phase,
                                OrganizationID: ($("#selectOrg").val() === "0" ? null : $("#selectOrg").val())
                            }
                            console.log(dataObj);
                            listToSave.push(dataObj);
                        }
                        else {
                            isChanged = true;
                            //angular.forEach($scope.orgBudgetCollection, function (orgItem) {
                            //    if (value.ID === orgItem.ID && value.CategoryID === orgItem.CategoryID && value.CategoryDescription === orgItem.CategoryDescription
                            //        && value.SubCategoryID === orgItem.SubCategoryID && value.SubCategoryDescription === orgItem.SubCategoryDescription && value.Services === orgItem.Services
                            //        /*&& value.Phase === orgItem.Phase*/) {
                            //        //Do nothing on unchanged Item
                            //        isChanged = false;
                            //        //isChanged = true;
                            //    }
                            //});
                            //Aditya: 06062022
                            var changeBudget = [];
                            changeBudget = $scope.orgBudgetCollection.filter(orgItem => value.ID === orgItem.ID && value.CategoryID === orgItem.CategoryID && value.CategoryDescription === orgItem.CategoryDescription && value.SubCategoryID === orgItem.SubCategoryID && value.SubCategoryDescription === orgItem.SubCategoryDescription && value.Services == orgItem.Services);
                            if (changeBudget.length > 0) {
                                isChanged = false;
                            }
                            console.log(changeBudget);
                            if (isChanged === true) {
                                var temp = '';
                                isReload = true;
                                /*if (typeof value.Phase == 'string') {
                                    temp = value.Phase;
                                } else {
                                    temp = value.Phase.Code;
                                }*/

                                var dataObj = {
                                    Operation: '2',
                                    CategoryID: value.CategoryID,
                                    CategoryDescription: value.CategoryDescription,
                                    SubCategoryID: value.SubCategoryID,
                                    SubCategoryDescription: value.SubCategoryDescription,
                                    Services: value.Services, //Aditya 04062022
                                    //Phase: temp,
                                    OrganizationID: value.OrganizationID,
                                    ID: value.ID
                                }
                                console.log(dataObj);

                            } else {
                                var temp = '';
                                isReload = true;
                                /*if (typeof value.Phase == 'string') {
                                    temp = value.Phase;
                                } else {
                                    temp = value.Phase.Code;
                                }*/
                                var dataObj = {
                                    Operation: '4',
                                    CategoryID: value.CategoryID,
                                    CategoryDescription: value.CategoryDescription,
                                    SubCategoryID: value.SubCategoryID,
                                    SubCategoryDescription: value.SubCategoryDescription,
                                    Services: value.Services, //Aditay 04062022
                                    //Phase: temp,
                                    OrganizationID: value.OrganizationID,
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
                        response.data.result.replace(/[\r]/g, '\n');

                        if (response.data.result) {
                            dhtmlx.alert(response.data.result);
                            //Aditya: get Service name //04062022
                            angular.forEach($scope.budgetCollection, function (item, index) {
                                angular.forEach($scope.ServiceList, function (service, index) {
                                    if (service.Id == item.Services) {
                                        item.Services = service.Value;
                                    }
                                });
                            });
                        } else {
                            dhtmlx.alert('No changes to be saved.');
                        }

                        $state.reload();
                        if ($("#selectOrg").val() && $("#selectVersion").val()) {

                            Category.get({ OrganizationID: $("#selectOrg").val(), VersionID: $("#selectVersion").val() }, function (categoryData) {
                                $scope.checkList = [];
                                $scope.budgetCollection = categoryData.result;
                                $scope.orgBudgetCollection = angular.copy(categoryData.result);
                                addIndex($scope.budgetCollection);
                                angular.forEach($scope.budgetCollection, function (item, index) {
                                    item.checkbox = false;
                                    $scope.checkList[index + 1] = false;
                                    //Aditya: get Service name //04062022
                                    angular.forEach($scope.ServiceList, function (service, index) {
                                        if (service.Id == item.Services) {
                                            item.Services = service.Value;
                                        }
                                    });
                                });
                                $scope.gridOptions.data = $scope.budgetCollection;

                                console.log('budgetCollection');
                                console.log($scope.budgetCollection);
                            });

                        }

                        //$scope.selectOrg.selected = $("#selectOrg").val();

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
                angular.forEach($scope.budgetCollection, function (item) {
                    if (item.checkbox == true) {
                        selectedRow = true;
                        if (item.new === true) {
                            unSavedChanges = true;
                            newList.push(item);
                        } else {
                            isChecked = true;
                            var dataObj = {
                                Operation: '3',
                                CategoryID: item.CategoryID,
                                CategoryDescription: item.CategoryDescription,
                                SubCategoryID: item.SubCategoryID,
                                SubCategoryDescription: item.SubCategoryDescription,
                                Services: item.Services, //Aditya 04062022
                                //Phase: item.Phase,
                                displayId: item.displayId,
                                ID: item.ID
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
                        angular.forEach($scope.budgetCollection, function (item, index) {
                            if (item.displayId == newList[i].displayId) {
                                item.checkbox = false;
                                ind = index;
                            }
                        });
                        if (ind != -1) {
                            $scope.checkList.splice(newList[i].displayId, 1);
                            $scope.budgetCollection.splice(ind, 1);
                        }
                    }

                }
                if (listToSave.length != 0) { }
                for (var i = 0; i < listToSave.length; i++) {
                    var ind = -1;
                    angular.forEach($scope.budgetCollection, function (item, index) {
                        if (item.displayId == listToSave[i].displayId) {
                            item.checkbox = false;
                            ind = index;
                        }
                    });
                    if (ind != -1) {
                        $scope.checkList.splice(listToSave[i].displayId, 1);
                        $scope.budgetCollection.splice(ind, 1);
                    }
                }
            }
            $scope.checkForChanges = function () {
                var unSavedChanges = false;
                var originalCollection = $scope.orgBudgetCollection;
                var currentCollection = $scope.budgetCollection;
                if (currentCollection.length !== originalCollection.length) {
                    unSavedChanges = true;
                    return unSavedChanges;
                } else {
                    angular.forEach(currentCollection, function (currentObject) {
                        for (var i = 0, len = originalCollection.length; i < len; i++) {
                            if (unSavedChanges) {
                                return unSavedChanges; // no need to look through the rest of the original array
                            }
                            console.log(originalCollection[i].ID === currentObject.ID);
                            if (originalCollection[i].ID === currentObject.ID) {
                                var originalObject = originalCollection[i];
                                console.log(originalObject, currentObject);
                                // compare relevant data
                                if (/*originalObject.Phase !== currentObject.Phase ||*/
                                    originalObject.CategoryID !== currentObject.CategoryID ||
                                    originalObject.CategoryDescription !== currentObject.CategoryDescription ||
                                    originalObject.SubCategoryID !== currentObject.SubCategoryID ||
                                    originalObject.SubCategoryDescription !== currentObject.SubCategoryDescription) {
                                    console.log(originalObject, currentObject);
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
                if (true) return;
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
                        $scope.save();
                        onRouteChangeOff();
                        $location.path(newUrl);
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