angular.module('cpp.controllers').
    controller('AddBOMRequestModalCtrl', ['$scope', '$timeout', '$uibModal', '$rootScope', '$http', '$uibModalInstance', 'UniqueIdentityNumber', 'Inventory',
        function ($scope, $timeout, $uibModal, $rootScope, $http, $uibModalInstance, UniqueIdentityNumber, Inventory) {

            $('.modal-backdrop').hide();

            (function () {
                if (window.history && window.history.pushState) {
                    $(window).on('popstate', function () {
                        $scope.$close('close');
                    });
                }
            })();

            setTimeout(function () {
                $("#requestedDateID").datepicker(); 
                $("#actualReleaseDateID").datepicker();
            }, 1000);
            

            $scope.bomRequest = {};
            var url = serviceBasePath + 'response/BOMRequest';

            //When clicked on back button or X
            $scope.goBack = function (param) {
                $scope.$close(param);
            }

            console.log('here i am', $scope.params);

            $scope.projectList = ["1234", "5678"];
            $scope.manufacturerList = ["Sony", "Tesla"];
            $scope.currentPartNumberList = ["Sony SNC-WR600", "Sony UNI-IRL7T2", "Sony SNC-WR632C", "Sony YT-LDR632S"];
            $scope.statusOptions = ["Requested", "Released", "Received", "RMA"];



            $scope.currentProjectList = [];

            $scope.selectedProgramElement = {};
            $scope.selectedMaterial = {};
            $scope.selectedProject = {};
            $scope.selectedManufacturer = {};
            $scope.extCost = "";
            $scope.initialQty = "";
            $scope.availableQty = "";
            $scope.requestedQty = "";
            $scope.requestedDate = "";
            $scope.actualReleaseDate = "";
            $scope.serialNumber = "";
            $scope.purchaseOrderNumber = "";


            if ($scope.params.mode == 'new') {
                $scope.title = 'Add New Request';
            } else if ($scope.params.mode == 'update') {
                $scope.title = 'Update Request';

                $scope.selectedProgramElement = $scope.params.selectedProgramElement;
                $scope.selectedMaterial = $scope.params.selectedMaterial;
                $scope.selectedProject = $scope.params.selectedProject;
                $scope.selectedManufacturer = $scope.params.selectedManufacturer;
                $scope.extCost = $scope.params.row.ExtCost;
                $scope.initialQty = $scope.params.row.InitialQty;
                $scope.availableQty = $scope.params.row.AvailableQty;
                $scope.requestedQty = $scope.params.row.RequestedQty;
                $scope.requestedDate = $scope.params.row.RequestedDate;
                $scope.actualReleaseDate = $scope.params.row.ActualReleaseDate;
            } else if ($scope.params.mode == 'received') {
                $scope.title = 'Received';

                $scope.selectedProgramElement = $scope.params.selectedProgramElement;
                $scope.selectedMaterial = $scope.params.selectedMaterial;
                $scope.selectedProject = $scope.params.selectedProject;
                $scope.selectedManufacturer = $scope.params.selectedManufacturer;
                $scope.selectedVendor = $scope.params.selectedVendor;
                $scope.extCost = $scope.params.row.ExtCost;
                $scope.initialQty = $scope.params.row.InitialQty;
                $scope.availableQty = $scope.params.row.AvailableQty;
                $scope.requestedQty = $scope.params.row.RequestedQty;
                $scope.requestedDate = $scope.params.row.RequestedDate;
                $scope.actualReleaseDate = $scope.params.row.ActualReleaseDate;
            }


            //When clicked on save button
            $scope.save = function () {

                console.log($scope.bomRequest);
                if ($scope.selectedProgramElement.ProgramElementID == undefined || $scope.selectedProgramElement.ProgramElementID == null || $scope.selectedProgramElement.ProgramElementID == '') {
                    dhtmlx.alert('Please enter a project number');
                    return;
                }
                if ($scope.selectedProject.ProjectID == undefined || $scope.selectedProject.ProjectID == null || $scope.selectedProject.ProjectID == '') {
                    dhtmlx.alert('Please enter an element number');
                    return;
                }

                if ($scope.selectedManufacturer.ManufacturerID == undefined || $scope.selectedManufacturer.ManufacturerID == null || $scope.selectedManufacturer.ManufacturerID == '') {
                    dhtmlx.alert('Please enter a manufacturer');
                    return;
                }

                if ($scope.selectedMaterial.ID == undefined || $scope.selectedMaterial.ID == null || $scope.selectedMaterial.ID == '') {
                    dhtmlx.alert('Please enter a part #');
                    return;
                }

                if ($scope.requestedQty == undefined || $scope.requestedQty == null || $scope.requestedQty == '') {
                    dhtmlx.alert('Please enter a requested quantity');
                    return;
                }

                if ($scope.requestedDate == undefined || $scope.requestedDate == null || $scope.requestedDate == '') {
                    dhtmlx.alert('Please enter a requested date');
                    return;
                }

                if ($scope.actualReleaseDate == undefined || $scope.actualReleaseDate == null || $scope.actualReleaseDate == '') {
                    dhtmlx.alert('Please enter a actual release date');
                    return;
                }

                //Update request button
                if ($scope.params.mode == 'update') {
                    if ($scope.selectedVendor.VendorID == undefined || $scope.selectedVendor.VendorID == null || $scope.selectedVendor.VendorID == '') {
                        dhtmlx.alert('Please enter a vendor');
                        return;
                    }
                    if ($scope.purchaseOrderNumber == undefined || $scope.purchaseOrderNumber == null || $scope.purchaseOrderNumber == '') {
                        dhtmlx.alert('Please enter a purchase order #');
                        return;
                    }
                }

                //Received button
                if ($scope.params.mode == 'received') {
                    if ($scope.selectedVendor.VendorID == undefined || $scope.selectedVendor.VendorID == null || $scope.selectedVendor.VendorID == '') {
                        dhtmlx.alert('Please enter a vendor');
                        return;
                    }
                    if ($scope.purchaseOrderNumber == undefined || $scope.purchaseOrderNumber == null || $scope.purchaseOrderNumber == '') {
                        dhtmlx.alert('Please enter a purchase order #');
                        return;
                    }

                    if ($scope.serialNumber == undefined || $scope.serialNumber == null || $scope.serialNumber == '') {
                        dhtmlx.alert('Please enter a serial number');
                        return;
                    }
                }

                

                var objToSave = {};

                if ($scope.params.mode == 'new') {
                    objToSave.Operation = 1;  //1 means to create
                    objToSave.ProjectNumber = $scope.selectedProgramElement.ProjectNumber;   //set the material category id
                    objToSave.ProjectElementNumber = $scope.selectedProject.ProjectElementNumber;
                    objToSave.Manufacturer = $scope.selectedManufacturer.ManufacturerName;
                    objToSave.Vendor = "";
                    objToSave.PartNumber = $scope.selectedMaterial.Name;
                    objToSave.SerialNumber = "";
                    objToSave.Description = $scope.selectedMaterial.Description;
                    objToSave.UM = $scope.selectedMaterial.UnitType;
                    objToSave.UnitCost = $scope.selectedMaterial.Cost;
                    objToSave.ExtCost = (parseInt($scope.availableQty) - parseInt($scope.requestedQty)) * parseInt($scope.selectedMaterial.Cost);
                    objToSave.InitialQty = $scope.initialQty;
                    objToSave.AvailableQty = parseInt($scope.availableQty) - parseInt($scope.requestedQty)
                    objToSave.RequestedQty = $scope.requestedQty;
                    objToSave.RequestedDate = $scope.requestedDate;
                    objToSave.ActualReleaseDate = $scope.actualReleaseDate;
                    objToSave.PurchaseOrderNumber = "";
                    objToSave.Status = "Requested";
                    objToSave.Invoiced = "False";
                    objToSave.ProgramElementID = $scope.selectedProgramElement.ProjectNumber;
                    objToSave.ProjectID = $scope.selectedProject.ProjectID;
                    objToSave.ManufacturerID = $scope.selectedManufacturer.ManufacturerID;
                    objToSave.MaterialID = $scope.selectedMaterial.ID;
                    objToSave.VendorID = null;
                    objToSave.InventoryID = 1;
                }

                if ($scope.params.mode == 'update') {
                    objToSave.Operation = 2;  //2 means to update
                    objToSave.ProjectNumber = $scope.selectedProgramElement.ProjectNumber;   //set the material category id
                    objToSave.ProjectElementNumber = $scope.selectedProject.ProjectElementNumber;
                    objToSave.Manufacturer = $scope.selectedManufacturer.ManufacturerName;
                    objToSave.Vendor = $scope.selectedVendor.VendorName;
                    objToSave.PartNumber = $scope.selectedMaterial.Name;
                    objToSave.SerialNumber = "";
                    objToSave.Description = $scope.selectedMaterial.Description;
                    objToSave.UM = $scope.selectedMaterial.UnitType;
                    objToSave.UnitCost = $scope.selectedMaterial.Cost;
                    objToSave.ExtCost = $scope.params.row.ExtCost;
                    objToSave.InitialQty = $scope.params.row.InitialQty;
                    objToSave.AvailableQty = $scope.params.row.AvailableQty
                    objToSave.RequestedQty = $scope.requestedQty;
                    objToSave.RequestedDate = $scope.requestedDate;
                    objToSave.ActualReleaseDate = $scope.actualReleaseDate;
                    objToSave.PurchaseOrderNumber = "";
                    objToSave.Status = "Processed";
                    objToSave.Invoiced = "False";
                    objToSave.ProgramElementID = $scope.selectedProgramElement.ProjectNumber;
                    objToSave.ProjectID = $scope.selectedProject.ProjectID;
                    objToSave.ManufacturerID = $scope.selectedManufacturer.ManufacturerID;
                    objToSave.MaterialID = $scope.selectedMaterial.ID;
                    objToSave.VendorID = $scope.selectedVendor.VendorID;
                    objToSave.InventoryID = 1;
                }

                if ($scope.params.mode == 'received') {
                    objToSave.Operation = 2;  //2 means to update
                    objToSave.ProjectNumber = $scope.selectedProgramElement.ProjectNumber;   //set the material category id
                    objToSave.ProjectElementNumber = $scope.selectedProject.ProjectElementNumber;
                    objToSave.Manufacturer = $scope.selectedManufacturer.ManufacturerName;
                    objToSave.Vendor = $scope.selectedVendor.VendorName;
                    objToSave.PartNumber = $scope.selectedMaterial.Name;
                    objToSave.SerialNumber = $scope.serialNumber;
                    objToSave.Description = $scope.selectedMaterial.Description;
                    objToSave.UM = $scope.selectedMaterial.UnitType;
                    objToSave.UnitCost = $scope.selectedMaterial.Cost;
                    objToSave.ExtCost = $scope.params.row.ExtCost;
                    objToSave.InitialQty = $scope.params.row.InitialQty;
                    objToSave.AvailableQty = $scope.params.row.AvailableQty
                    objToSave.RequestedQty = $scope.requestedQty;
                    objToSave.RequestedDate = $scope.requestedDate;
                    objToSave.ActualReleaseDate = $scope.actualReleaseDate;
                    objToSave.PurchaseOrderNumber = $scope.purchaseOrderNumber;
                    objToSave.Status = "Received";
                    objToSave.Invoiced = "False";
                    objToSave.ProgramElementID = $scope.selectedProgramElement.ProjectNumber;
                    objToSave.ProjectID = $scope.selectedProject.ProjectID;
                    objToSave.ManufacturerID = $scope.selectedManufacturer.ManufacturerID;
                    objToSave.MaterialID = $scope.selectedMaterial.ID;
                    objToSave.VendorID = $scope.selectedVendor.VendorID;
                    objToSave.InventoryID = 1;
                }



                var listToSave = [];
                var param = { status: '', objectSaved: $scope.bomRequest };

                listToSave.push(objToSave);

                $http({
                    url: url,
                    method: "POST",
                    data: JSON.stringify(listToSave),
                    headers: { 'Content-Type': 'application/json' }
                }).then(function success(response) {
                    response.data.result.replace(/[\r]/g, '\n');

                    param.status = 'Success';
                    $scope.goBack(param);

                    if (response.data.result.indexOf("successfully") >= 0) {
                        dhtmlx.alert(response.data.result.replace(/&/g, '&#38').replace(/</g, '&#60').replace(/>/g, '&#62'));
                        param.status = 'Success';
                        $scope.goBack(param);
                    } else {
                        dhtmlx.alert(response.data.result);
                    }
                }, function error(response) {
                    dhtmlx.alert("Failed to save. Please contact your Administrator.");
                });
            }

            



            //When selecting project number
            $scope.onProjectNumberSelect = function (programElement) {
                $scope.selectedProgramElement = programElement;
                $scope.selectedMaterial = {};
                $scope.selectedProject = {};
                $scope.selectedManufacturer = {};

                $scope.currentProjectList = [];
                for (var x = 0; x < $scope.params.allProjectList.length; x++) {
                    if ($scope.params.allProjectList[x].ProgramElementID == $scope.selectedProgramElement.ProgramElementID) {
                        $scope.currentProjectList.push($scope.params.allProjectList[x]);
                    }
                }
            }

            //When selecting project element number
            $scope.onProjectElementNumberSelect = function (project) {
                $scope.selectedProject = project;
            }

            //When selecting manufacturer name 
            $scope.onManufacturerNameSelect = function (manufacturer) {
                $scope.selectedManufacturer = manufacturer;
            }

            //When selecting part number
            $scope.onPartNumberSelect = function (material) {
                $scope.selectedMaterial = material;

                for (var x = 0; x < $scope.params.allInventoryList.length; x++) {
                    if ($scope.selectedMaterial.ID == $scope.params.allInventoryList[x].MaterialID
                        && $scope.selectedProgramElement.ProgramElementID == $scope.params.allInventoryList[x].ProgramElementID) {
                        $scope.availableQty = $scope.params.allInventoryList[x].AvailableQty;
                        $scope.initialQty = $scope.params.allInventoryList[x].InitialQty;
                        return;
                    }
                }

                $scope.availableQty = 0;
            }
        }
    ]);