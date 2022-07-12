angular.module('cpp.controllers').
    controller('PurchaseOrderDetailModalCtrl', ['$scope', '$timeout', '$uibModal', '$location', '$rootScope', '$http', '$uibModalInstance', 'CostRow', 'CostType', 'Employee',
        'PurchaseOrder', 'PurchaseOrderDetail', '$state',
        function ($scope, $timeout, $uibModal, $location, $rootScope, $http, $uibModalInstance, CostRow, CostType, Employee
            , PurchaseOrder, PurchaseOrderDetail, $state) {

            $('.modal-backdrop').hide();

            $('[data-toggle="tooltip"]').tooltip();

            var dtFormat = "MM/DD/YYYY";
            (function () {
                if (window.history && window.history.pushState) {
                    $(window).on('popstate', function () {
                        $scope.$close('close');
                    });
                }
            })();
            var calculateAllCostTotal = function () {
                $scope.total = 0;

                angular.forEach($scope.purchaseOrderDetails, function (item) {
                    //$scope.total += item.TotalCost;
                    item.TCost = 0;
                    if (item.RequestedAmountOrQuantity == undefined)
                        item.RequestedAmountOrQuantity = 0;
                    if (item.TCost == undefined)
                        item.TCost = 0;
                });
            }

           // $scope.empids = [];
            $scope.example14settings = {
                scrollableHeight: '100px', scrollable:true,
                enableSearch: false };
           /* $scope.example14data = [{
                "label": "Alabama",
                "id": "AL"
            }, {
                "label": "Alaska",
                "id": "AK"
                }
                ,
                {
                    "label": "Alaska",
                    "id": "AKS"
                },
                {
                    "label": "Alaska",
                    "id": "AKA"
                }

            ]; */
            console.log('selected dropdown elements:');
            console.log($scope.example14model);
            console.log("Organization Details==>");
            console.log($scope.params);
            $scope.organizationID = $scope.params.organizationID;
            $scope.balanceerror = false;
            $scope.purchaseOrderDetails = $scope.params.purchaseOrderDetails;
            $scope.purchaseOrderList = $scope.params.POList;
            $scope.projectId = $scope.params.purchaseOrder.ProjectID;
            var delayedData = $scope.params.DelayedData;


            console.log("PO details");
            
            for (var i in $scope.purchaseOrderDetails) {
                if ($scope.purchaseOrderDetails[i].CostType == 'ODC') {
                    $scope.purchaseOrderDetails[i].EmployeeIDlist = [];
                }
            }
            console.log($scope.purchaseOrderDetails);
            calculateAllCostTotal();


            function checkFields() {

                var isChanges = false;
                
                for (var x = 0; x < $scope.purchaseOrderDetails.length; x++) {

                    if ($scope.purchaseOrderDetails[x].RequestedAmountOrQuantity > 0) {
                        isChanges = true;
                    }

                }

                console.log($scope.purchaseOrderDetails[x]);


                if ($scope.purchaseOrder.Description.length > 0) {
                    isChanges = true;
                }

                if (isChanges) {

                    dhtmlx.confirm("Unsaved data will be lost. Want to Continue?", function (result) {
                        if (result) {
                            if (result) {
                                $scope.$close('close');
                            } else {
                                return;
                            }
                            isChanges = false;
                        }
                    });


                } else {
                    $scope.$close('close');
                }


            }
            //$scope.checkAll = function () {
            //    angular.forEach($scope.purchaseOrderDetails, function (item) {
            //        item.check = true;
            //    });
            //}
            $scope.purchaseOrder = $scope.params.purchaseOrder;
            $scope.purchaseOrder.Description = "";

            $scope.costTypeFilters = [];


            $scope.selectRow = function (row) {
                $scope.selectedRow = row;
            }
            $scope.onMouseLeave = function (row) {
                $scope.selectedRow = null;
            }
            $scope.goBack = function (param) {
                console.log("In back==");
                checkFields();
                //  console.log($scope.row.RequestedAmountOrQuantity);
                //$scope.$close(param);
            }

            $scope.exit = function (param) {
            
                $scope.$close(param);
            }


            $scope.$watch('all', function () {

                console.log($scope.all);
                if ($scope.all) {
                    angular.forEach($scope.purchaseOrderDetails, function (item) {
                        item.check = true
                    });
                } else {
                    angular.forEach($scope.purchaseOrderDetails, function (item) {
                        item.check = false
                    });
                }
                console.log($scope.purchaseOrderDetails);
            });




           

            //Get all cost types
            CostType.get({}, function (response) {
                console.log("Get cost type");
                $scope.costTypeCollection = response.result;
                // $scope.gridOptions.columnDefs[1].editDropdownOptionsArray = $scope.costTypeCollection;
                console.log(response.result);


                for (var x = 0; x < $scope.costTypeCollection.length; x++) {
                    var temp = {};
                    if ($scope.costTypeCollection[x].ID != 1) {
                        temp.value = $scope.costTypeCollection[x].Type;
                        temp.id = $scope.costTypeCollection[x].ID;
                        $scope.costTypeFilters.push($scope.costTypeCollection[x]);
                    }
                }
                console.log($scope.costTypeFilters);
                $scope.CostTypeList = $scope.costTypeFilters;
                console.log("Data==");
                console.log($scope.CostTypeList);
            });
            console.log("Employee Org Id List");
            console.log($scope.organizationID);
            Employee.get({ OrganizationID: $scope.organizationID }, function (EmployeesData) {
                //$scope.newEmployees = [];
                var employee = {};
                var selectedEmployee = {};
                var newlyAddedID = null;
                console.log("Employee List");
                $scope.employeesList = EmployeesData.result;
                $scope.empdata = [];
                for (var empno in $scope.employeesList) {
                    $scope.empdata.push({
                        "id": $scope.employeesList[empno].ID, "label": $scope.employeesList[empno].Name
                    });
                   
                }
                console.log('New emp data-->');
                console.log($scope.empdata);
                console.log($scope.employeesList);

            });


            $scope.setCostType = function (costType) {
                console.log("setCostType");
                console.log(costType);
                console.log($scope.costTypeSelect);

            }

            $scope.setEmployee = function (employeeSelected) {
                console.log("checking set Employee ==>");
                console.log(employeeSelected);

            }

            $scope.calculateprice = function (POdtl) {
                $scope.balanceerror = false;
                console.log('Checking range..');
                console.log(POdtl);
                $scope.total = 0;

                //To update Total Cost and Sum Total
                for (var dtlp in $scope.purchaseOrderDetails) {
                    if (POdtl.CostLineItemID == $scope.purchaseOrderDetails[dtlp].CostLineItemID)
                        $scope.purchaseOrderDetails[dtlp].RequestedAmountOrQuantity = POdtl.RequestedAmountOrQuantity;
                    if (($scope.purchaseOrderDetails[dtlp].RequestedAmountOrQuantity != undefined || $scope.purchaseOrderDetails[dtlp].RequestedAmountOrQuantity != 0)
                        && !($scope.purchaseOrderDetails[dtlp].BalancedAmountOrQuantity < $scope.purchaseOrderDetails[dtlp].RequestedAmountOrQuantity)) {

                        var qtyprice = parseFloat($scope.purchaseOrderDetails[dtlp].UnitPrice);
                        if (isNaN(qtyprice))
                            qtyprice = 1;
                        $scope.purchaseOrderDetails[dtlp].TCost = ($scope.purchaseOrderDetails[dtlp].RequestedAmountOrQuantity) * qtyprice;
                    }
                    else
                        $scope.purchaseOrderDetails[dtlp].TCost = 0;
                    $scope.total = $scope.total + $scope.purchaseOrderDetails[dtlp].TCost;
                    console.log('Qty. Cost --' + $scope.purchaseOrderDetails[dtlp].TCost);
                }

                console.log('Total Cost--' + $scope.total);

                // To display error 
                for (var dtl in $scope.purchaseOrderDetails) {

                    if ($scope.purchaseOrderDetails[dtl].RequestedAmountOrQuantity == undefined) {
                        console.log("undefined==>" + $scope.purchaseOrderDetails[dtl].RequestedAmountOrQuantity)
                        $scope.balanceerror = false;

                    }

                    if ($scope.purchaseOrderDetails[dtl].BalancedAmountOrQuantity < ($scope.purchaseOrderDetails[dtl].RequestedAmountOrQuantity)) {
                        console.log("undefined11==>" + $scope.purchaseOrderDetails[dtl].RequestedAmountOrQuantity);
                        $scope.balanceerror = true;
                        $scope.curcostitemID = $scope.purchaseOrderDetails[dtl].CostLineItemID;
                        dhtmlx.alert("Requested Quantity cannot be more than available quantity for Cost Code:" + $scope.curcostitemID);
                        console.log('Error:' + $scope.curcostitemID);
                        break;

                    }
                }
            }


            $scope.save = function () {
                var purchaseOrderList = [];
                angular.forEach($scope.purchaseOrderDetails, function (item) {
                    if (item.RequestedAmountOrQuantity > 0 && item.RequestedAmountOrQuantity != undefined) {
                        if (item.CostType == 'ODC') {
                            item.EmployeeID = [];
                            for (var empno in item.EmployeeIDlist)
                                item.EmployeeID.push(item.EmployeeIDlist[empno].id);
                            
                        }
                       
                        purchaseOrderList.push(item);
                    }
                });

                if (purchaseOrderList.length == 0) {
                    dhtmlx.alert('Purchase List is empty, please add items');
                    return;
                }
                var data = {
                    purchaseOrder: $scope.purchaseOrder,
                    purchaseOrderDetails: purchaseOrderList
                }
                //data.purchaseOrder.operation = "save";
                //data.purchaseOrder.PurchaseOrderNumber = "";
                //data.purchaseOrder.PurchaseOrderBaseNumber = "";
                console.log('SAVE DATA-->');
                console.log(data);
               PurchaseOrderDetail.persist().save(data)
                    .$promise.then(function success(response) {
                        console.log("Response result data==>");
                        console.log(response.result);
                        dhtmlx.alert('Purchase Order :' + ' ' + response.result + '\n' + 'has been created successfully.');
                        $scope.$close('close');
                        $scope.getPurchaseOrder();  //just for testing
                    }, function error(response) {

                    });

                  

                //SAVE PROCESS--
                /*PurchaseOrderDetail.persist().save(data)
                    .$promise.then(function sucess(response) {
                        console.log(response);

                        var baseUrl = null;
                        baseUrl = serviceBasePath + 'Request/PurchaseOrderDetailReport';

                        var excelUrl = baseUrl
                            + '?PurchaseOrderNumber=' + response.result
                            + '&FileType=' + 'excel';

                        downloadAsExcel(excelUrl, 'PurchaseOrder_' + response.result);

                    }, function error(response) {

                    });

            }

            function downloadAsExcel(excelUrl, fileName) {
                console.log('downloading as excel');

                $http.get(excelUrl)
                    .then(function success(response) {
                        console.log(response);
                        var dedcodedRaw = atob(response.data);
                        console.log('attempt excel blob');

                        var array = new Uint8Array(dedcodedRaw.length);

                        for (i = 0; i < dedcodedRaw.length; i++) {
                            array[i] = dedcodedRaw.charCodeAt(i);
                        }

                        var blobtest = new Blob([array], {
                            //type: 'application/vnd.ms-excel;charset=charset=utf-8'
                        });

                        //saveAs comes from FileSaver.js, faster method of saving files
                        saveAs(blobtest, fileName + '.xls');

                    }, function error(response) {
                        console.log(response);
                    });
            }*/

            }

            $scope.modifyPO = function (id) {
                console.log("In modify");
                console.log(id);

                PurchaseOrderDetail.getPurchaseOrderIDDetail()
                    .get({ PurchaseOrderID: id }, function success(response) {
                        console.log(response.result);
                        var purchaseOrderDetails = response.result.poDetails;
                        $scope.purchaseOrder = response.result.po;
                        $scope.poDetails = [];

                        for (var x = 0; x < purchaseOrderDetails.length; x++) {
                            var temp = {};
                            //if (purchaseOrderDetails[x].purchaseOrderID == id) {

                            $scope.poDetails.push(purchaseOrderDetails[x]);
                            // }
                        }

                        for (var loopin in $scope.poDetails) {
                            $scope.poDetails[loopin].BalancedAmountOrQuantity = $scope.poDetails[loopin].BalancedAmountOrQuantity + $scope.poDetails[loopin].RequestedAmountOrQuantity;
                        }
                        console.log("Updated List==>");
                        console.log($scope.poDetails);
                        var scope = $rootScope.$new();



                        scope.params = {
                            edit: true,
                            purchaseOrderDetails: $scope.poDetails,
                            purchaseOrder: $scope.purchaseOrder,
                            organizationID: $scope.organizationID
                        }



                        console.log(scope.params);

                        $scope.$close('close');

                        //4/25    
                        $rootScope.modalInstance = $uibModal.open({
                            //backdrop: 'static',
                            keyboard: false,
                            scope: scope,
                            templateUrl: "app/views/modal/modify_podetail_modal.html",
                            size: "lg",
                            windowClass: "w90p",
                            controller: "ModifyPODetailModalCtrl"
                        });
                        $rootScope.modalInstance.result.then(function (response) {
                            setTimeout(function () {
                                console.log("return data==>");
                              //  $state.reload();
                                
                                $scope.getPurchaseOrder(); 
                                //  applyExpandables();
                               
                            }, 1000);
                        });

                    }, function error(response) {

                    });
            }

            $scope.getPurchaseOrder = function () {
                console.log("In getPurchaseOrder()");
                console.log($scope.projectId);
                PurchaseOrder.getNewPurchaseOrderNumber()
                    .get({ ProjectID: $scope.projectId }, function success(response) {
                        var purchaseOrder = response.result;
                        PurchaseOrderDetail.getPurchaseOrderDetail()
                            .get({ ProjectID: $scope.projectId }, function success(response) {
                                //console.log(response);
                                var purchaseOrderDetails = response.result;
                                var scope = $rootScope.$new();
                                PurchaseOrderDetail.getPOList()
                                    .get({ ProjectID: $scope.projectId }, function success(response) {
                                        var poList = response.result;
                                        var purchaseOrderList = [];
                                        //var poList = response.result;

                                        angular.forEach(poList, function (item) {
                                            if (item.Status == 'Created' || item.Status == 'Rejected')
                                                purchaseOrderList.push(item);
                                        });
                                        scope.params = {
                                            purchaseOrder: purchaseOrder,
                                            purchaseOrderDetails: purchaseOrderDetails,
                                            organizationID: $scope.params.organizationID,
                                            POList: purchaseOrderList
                                        }

                                        $rootScope.modalInstance = $uibModal.open({
                                            backdrop: 'static',
                                            keyboard: false,
                                            scope: scope,
                                            templateUrl: "app/views/modal/purchase_order_list_modal.html",
                                            size: "lg",
                                            controller: "PurchaseOrderDetailModalCtrl"
                                        });
                                        $rootScope.modalInstance.result.then(function (response) {
                                            setTimeout(function () {
                                                //applyExpandables();
                                            }, 1000);
                                        });

                                    }, function error(response) {

                                    });

                            }, function error(response) {

                            });

                    }, function error(response) {

                    });
            }



            $scope.viewPO = function (id) {
                console.log("In view");
                console.log(id);

                PurchaseOrderDetail.getPurchaseOrderIDDetail()
                    .get({ PurchaseOrderID: id }, function success(response) {
                        console.log(response.result);
                        var purchaseOrderDetails = response.result.poDetails;
                        $scope.purchaseOrder = response.result.po;
                        $scope.poDetails = [];

                        for (var x = 0; x < purchaseOrderDetails.length; x++) {
                            var temp = {};
                            if (purchaseOrderDetails[x].RequestedAmountOrQuantity > 0) {

                                $scope.poDetails.push(purchaseOrderDetails[x]);
                            }
                        }
                        console.log("List==>");
                        console.log($scope.poDetails);

                        var scope = $rootScope.$new();



                        scope.params = {
                            edit: false,
                            purchaseOrderDetails: $scope.poDetails,
                            purchaseOrder: $scope.purchaseOrder,
                            organizationID: $scope.organizationID
                        }



                        console.log(scope.params);



                        //4/25    
                        $rootScope.modalInstance = $uibModal.open({
                            backdrop: 'static',
                            keyboard: false,
                            scope: scope,
                            templateUrl: "app/views/modal/modify_podetail_modal.html",
                            size: "lg",
                            windowClass: "w90p",
                            controller: "ModifyPODetailModalCtrl"
                        });
                        $rootScope.modalInstance.result.then(function (response) {
                            setTimeout(function () {
                                applyExpandables();
                            }, 1000);
                        });


                    }, function error(response) {

                    });

            }

        }
    ]);
