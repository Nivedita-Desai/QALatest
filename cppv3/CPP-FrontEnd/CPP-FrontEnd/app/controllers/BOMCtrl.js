angular.module('cpp.controllers').
    controller('BOMCtrl', ['$state', 'ProjectTitle', 'UserName', '$http', '$location', '$scope', '$rootScope', '$uibModal', '$sce',
        'Page', 'myLocalStorage', 'localStorageService',
        '$location', '$stateParams', '$window', 'ProgramFund', 'usSpinnerService', '$filter',
        'ProjectScope', '$timeout', 'BOMRequest', 'Manufacturer', 'ProgramElement', 'Project', 'Material', 'UnitType', 'Inventory', 'Vendor',
        function ($state, ProjectTitle, UserName, $http, $location, $scope, $rootScope, $uibModal, $sce, Page, myLocalStorage, localStorageService,
            $location, $stateParams, $window, ProgramFund, usSpinnerService, $filter, ProjectScope, $timeout, BOMRequest, Manufacturer, ProgramElement, Project, Material, UnitType, Inventory, Vendor) {
            var pendingStatus = "Entering Pin";
            Page.setTitle('Bill of Materials');
            ProjectTitle.setTitle('');
            var grid;

            $scope.allProgramElementList = [];
            $scope.allProjectList = [];
            $scope.allManufacturerList = [];
            $scope.allMaterialList = [];
            $scope.allInventoryList = [];
            $scope.allVendorList = [];

            //Get selected object based on ID
            $scope.getProgramElement = function (list, id) {
                for (var x = 0; x < list.length; x++) {
                    if (list[x].ProgramElementID == id) {
                        return list[x];
                    }
                }

                return {};
            }

            //Get selected object based on ID
            $scope.getProject = function (list, id) {
                for (var x = 0; x < list.length; x++) {
                    if (list[x].ProjectID == id) {
                        return list[x];
                    }
                }

                return {};
            }

            //Get selected object based on ID
            $scope.getManufacturer = function (list, id) {
                for (var x = 0; x < list.length; x++) {
                    if (list[x].ManufacturerID == id) {
                        return list[x];
                    }
                }

                return {};
            }

            //Get selected object based on ID
            $scope.getMaterial = function (list, id) {
                for (var x = 0; x < list.length; x++) {
                    if (list[x].MaterialID == id) {
                        return list[x];
                    }
                }

                return {};
            }

            //Get selected object based on ID
            $scope.getVendor = function (list, id) {
                for (var x = 0; x < list.length; x++) {
                    if (list[x].VendorID == id) {
                        return list[x];
                    }
                }

                return {};
            }

            //initialize list of program elements
            ProgramElement.lookup().get({}, function (response) {
                console.log(response);
                $scope.allProgramElementList = response.result;
                for (var x = 0; x < $scope.allProgramElementList.length; x++) {
                    $scope.allProgramElementList[x].DisplayName = $scope.allProgramElementList[x].ProjectNumber + " - " + $scope.allProgramElementList[x].ProgramElementName;
                }
            });

            //initialize list of projects
            Project.lookup().get({}, function (response) {
                console.log(response);
                $scope.allProjectList = response.result;
                for (var x = 0; x < $scope.allProjectList.length; x++) {
                    $scope.allProjectList[x].DisplayName = $scope.allProjectList[x].ProjectElementNumber + " - " + $scope.allProjectList[x].ProjectName;
                }
            });

            //initialize list of manufacturers
            Manufacturer.get({}, function (response) {
                console.log(response);
                $scope.allManufacturerList = response.result;
            });

            //initialize list of materials
            Material.get({}, function (response) {
                console.log(response);
                $scope.allMaterialList = response.result;

                UnitType.get({}, function (response) {
                    var allUnitTypeList = response.result;
                    for (var x = 0; x < $scope.allMaterialList.length; x++) {
                        for (var y = 0; y < allUnitTypeList.length; y++) {
                            if ($scope.allMaterialList[x].UnitTypeID == allUnitTypeList[y].UnitID) {
                                $scope.allMaterialList[x].UnitType = allUnitTypeList[y].UnitName;
                            }
                        }
                    }
                });
            });

            //initialize list of vendors
            Vendor.get({}, function (response) {
                console.log(response);
                $scope.allVendorList = response.result;
            });

            //initialize list of inventories
            Inventory.get({}, function (response) {
                console.log(response);
                $scope.allInventoryList = response.result;
            });

            //initialize list of bomrequests
            var initializeBOM = function () {
                BOMRequest.get({}, function (response) {
                    console.log(response);
                    $scope.data = response.result;

                    for (var x = 0; x < $scope.data.length; x++) {
                        $scope.data[x].IsChecked = true;
                    }


                    var projectList = ["1234", "5678"];
                    var manufacturerList = ["Sony"];
                    var currentPartNumberList = ["Sony SNC-WR600", "Sony UNI-IRL7T2", "Sony SNC-WR632C", "Sony YT-LDR632S"];

                    grid = new dhx.Grid("grid", {
                        columns: [
                            { width: 100, id: "BOMRequestID", type: "string", header: [{ text: "ID" }, { content: "inputFilter" }], sortable: false },
                            { width: 50, id: "IsChecked", type: "boolean", header: [{ text: "" }], editable: true},
                            { width: 125, id: "ProjectNumber", type: "string", editorType: "select", header: [{ text: "Project Number" }, { content: "selectFilter" }], options: projectList, sortable: false },
                            { width: 125, id: "ProjectElementNumber", type: "string", header: [{ text: "Element Number" }, { content: "selectFilter" }], sortable: false },
                            { width: 125, id: "Manufacturer", type: "string", editorType: "select", header: [{ text: "Manufacturer" }, { content: "inputFilter" }], options: manufacturerList, sortable: false },
                            { width: 150, id: "PartNumber", type: "string", editorType: "select", header: [{ text: "Part #" }, { content: "inputFilter" }], options: currentPartNumberList, sortable: false },
                            { width: 150, id: "SerialNumber", header: [{ text: "Serial #" }, { content: "inputFilter" }], sortable: false },
                            { width: 200, id: "Description", header: [{ text: "Description" }, { content: "inputFilter" }], sortable: false },
                            { width: 75, id: "UM", header: [{ text: "U/M" }, { content: "inputFilter" }], sortable: false },
                            { width: 100, id: "UnitCost", type: "number", header: [{ text: "Unit Cost" }, { content: "inputFilter" }], sortable: false },
                            { width: 100, id: "ExtCost", type: "number", header: [{ text: "Ext. Cost" }, { content: "inputFilter" }], sortable: false },
                            { width: 105, id: "InitialQty", type: "number", header: [{ text: "Initial Qty." }, { content: "inputFilter" }], sortable: false },
                            { width: 125, id: "AvailableQty", type: "number", header: [{ text: "Available Qty." }, { content: "inputFilter" }], sortable: false },
                            { width: 145, id: "RequestedQty", type: "number", header: [{ text: "Requested Qty." }, { content: "inputFilter" }], sortable: false },
                        ],
                        data: $scope.data,
                        height: 800,
                        //width: 1800,
                        editable: false,
                        //  autoWidth: true,
                        selection: "row",
                        //splitAt: 2,
                        rowHeight: 50,
                        resizable: true,
                        keyNavigation: true,
                        css: "dhx-round-corner"


                        //adjust: true
                    });

                    $scope.columns = grid.config.columns;

                }, function (rejected) {
                    console.log('Error: error occurred on initial gate selection')
                });
            }
            initializeBOM();


            //Click on new request button
            $scope.newRequest = function () {
                var scope = $rootScope.$new();
                scope.params = {};
                scope.params.allManufacturerList = $scope.allManufacturerList;
                scope.params.allMaterialList = $scope.allMaterialList;
                scope.params.allProgramElementList = $scope.allProgramElementList;
                scope.params.allProjectList = $scope.allProjectList;
                scope.params.allInventoryList = $scope.allInventoryList;
                scope.params.allVendorList = $scope.allVendorList;
                scope.params.mode = 'new';
                //scope.params.bomRequest = $scope.data[0];

                $rootScope.modalInstance = $uibModal.open({
                    backdrop: 'static',
                    keyboard: false,
                    scope: scope,
                    templateUrl: "app/views/modal/add_bom_request_modal.html",
                    size: "md",
                    controller: "AddBOMRequestModalCtrl"
                });
                $rootScope.modalInstance.result.then(function (response) {
                    console.log(response);
                    //alert('initializting omb');
                    //grid.clearAll(true);
                    //initializeBOM();
                    if (response.status == 'Success') {
                        
                    } else {

                    }
                }, function error(response) {
                    console.log(response);
                });
            };

            $scope.updateRequest = function () {
                var row = grid.selection._selectedCells[0].row;

                console.log(row);

                var scope = $rootScope.$new();
                scope.params = {};
                scope.params.allManufacturerList = $scope.allManufacturerList;
                scope.params.allMaterialList = $scope.allMaterialList;
                scope.params.allProgramElementList = $scope.allProgramElementList;
                scope.params.allProjectList = $scope.allProjectList;
                scope.params.allInventoryList = $scope.allInventoryList;
                scope.params.allVendorList = $scope.allVendorList;
                scope.params.selectedProgramElement = $scope.getProgramElement($scope.allProgramElementList, row.ProgramElementID);
                scope.params.selectedProject = $scope.getProject($scope.allProjectList, row.ProjectID);
                scope.params.selectedManufacturer = $scope.getManufacturer($scope.allManufacturerList, row.ManufacturerID);
                scope.params.selectedMaterial = $scope.getMaterial($scope.allMaterialList, row.MaterialID);
                scope.params.selectedVendor = {};
                scope.params.row = row;
                scope.params.mode = 'update';
                //scope.params.bomRequest = $scope.data[0];

                $rootScope.modalInstance = $uibModal.open({
                    backdrop: 'static',
                    keyboard: false,
                    scope: scope,
                    templateUrl: "app/views/modal/add_bom_request_modal.html",
                    size: "md",
                    controller: "AddBOMRequestModalCtrl"
                });
                $rootScope.modalInstance.result.then(function (response) {
                    console.log(response);
                    if (response.status == 'Success') {
                        
                    } else {

                    }
                }, function error(response) {
                    console.log(response);
                });
            };

            $scope.received = function () {
                var row = grid.selection._selectedCells[0].row;

                var scope = $rootScope.$new();
                scope.params = {};
                scope.params.allManufacturerList = $scope.allManufacturerList;
                scope.params.allMaterialList = $scope.allMaterialList;
                scope.params.allProgramElementList = $scope.allProgramElementList;
                scope.params.allProjectList = $scope.allProjectList;
                scope.params.allInventoryList = $scope.allInventoryList;
                scope.params.allVendorList = $scope.allVendorList;
                scope.params.selectedProgramElement = $scope.getProgramElement($scope.allProgramElementList, row.ProgramElementID);
                scope.params.selectedProject = $scope.getProject($scope.allProjectList, row.ProjectID);
                scope.params.selectedManufacturer = $scope.getManufacturer($scope.allManufacturerList, row.ManufacturerID);
                scope.params.selectedMaterial = $scope.getMaterial($scope.allMaterialList, row.MaterialID);
                scope.params.selectedVendor = $scope.getVendor($scope.allVendorList, row.VendorID);
                scope.params.row = row;
                scope.params.mode = 'received';
                //scope.params.bomRequest = $scope.data[0];

                $rootScope.modalInstance = $uibModal.open({
                    backdrop: 'static',
                    keyboard: false,
                    scope: scope,
                    templateUrl: "app/views/modal/add_bom_request_modal.html",
                    size: "md",
                    controller: "AddBOMRequestModalCtrl"
                });
                $rootScope.modalInstance.result.then(function (response) {
                    console.log(response);
                    if (response.status == 'Success') {
                        
                    } else {

                    }
                }, function error(response) {
                    console.log(response);
                });
            };







            $scope.changeFreezeColumn = function (colIndex, data) {
                console.log(colIndex);
                console.log(data);
                console.log(grid.config.columns);

                angular.forEach(grid.config.columns, function (value, key) {
                    console.log(value);
                    console.log(key);
                    if (value.header[0].text == data) {

                        grid.config.splitAt = key + 1;
                        return;
                    }
                });
            };

            $scope.runReport = function (param) {
                if (param == "Excel") {
                    alert();
                    grid.export.xlsx({
                        name: "grid_data",
                        url: "//export.dhtmlx.com/excel"
                    });
                } else if (param == "CSV") {
                    grid.export.csv({
                        name: "grid_data", // grid data will be exported to a CSV file named "grid_data"
                        columnDelimiter: ";" // the semicolon delimiter will be used to separate columns
                    });
                } else {
                    alert("Report not implemented");
                }

            }

            $scope.changeFilter = function (param) {
                alert('test');
                $http({
                    method: "POST",
                    //url: serviceBasePath + "/Request/GetGuardPostNames"
                    url: serviceBasePath + "Api/DoorMatrix/search",
                    data: { value: param },
                    headers: {
                        'Content-Type': 'application/json'
                    },
                }).then(function (response) {
                    grid.config.data = response.data;
                    grid.data.parse(response.data);
                    grid.paint();
                    console.log(response);
                }, function error(response) {
                    console.log(response);
                });
            }


        }])
    .directive("enterKey", function () {
        return function (scope, element, attrs) {
            element.bind("keydown keypress", function (event) {
                //console.log(event);
                if (event.which === 13) {
                    scope.$apply(function () {
                        scope.$eval(attrs.enterKey);
                    });
                    event.preventDefault();
                }
            });
        }
    }
);
