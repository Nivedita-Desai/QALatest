angular.module('cpp.controllers').
    controller('POApprovalCtrl', ['$scope', 'Page', '$rootScope', 'ProjectTitle', '$uibModal', '$http', 'Organization', 'PurchaseOrder', 'PurchaseOrderDetail', '$stateParams',
        'localStorageService', 'authService', '$location', 
        function ($scope, Page, $rootScope, ProjectTitle, $uibModal, $http, Organization, PurchaseOrder, PurchaseOrderDetail, $stateParams,
            localStorageService, authService,$location) {
            Page.setTitle('');
            ProjectTitle.setTitle('');

            var auth = localStorageService.get("authorizationData");
            console.log(auth);
            if (auth != null) {
                if (auth.role != null && auth.role != "Accounting") {
                    authService.logOut();
                    $location.path('/login');
                }
            } else {
                authService.logOut();
                $location.path('/login');
            }
            
            var scope = $rootScope.$new();
            console.log('SP Testing');
            console.log($stateParams);
                            PurchaseOrderDetail.getPOList()
                                .get({ ProjectID: '0' }, function success(response) {

                                    var purchaseOrderList = [];
                                    var poList = response.result;

                                    angular.forEach(poList, function (item) {
                                       // if (item.Status == 'Created')
                                            purchaseOrderList.push(item);
                                    });

                                    scope.params = {
                                        //purchaseOrder: purchaseOrder,
                                        //purchaseOrderDetails: purchaseOrderDetails,
                                        organizationID: '75',
                                        POList: purchaseOrderList
                                    }

                                   
                                    $scope.purchaseOrderList = purchaseOrderList;
                                    console.log($scope.purchaseOrderList);
                                    $scope.poIDD = $scope.purchaseOrderList.find(poNum => poNum.PurchaseOrderNumber == $stateParams.poid);
                                    console.log("Filter==");
                                    if ($scope.poIDD != undefined)
                                    $scope.viewPO($scope.poIDD.ID);
                                    //$rootScope.modalInstance = $uibModal.open({
                                    //    backdrop: 'static',
                                    //    keyboard: false,
                                    //    scope: scope,
                                    //    templateUrl: "app/views/po-approval.html",
                                    //    size: "lg",
                                    //    controller: "POApprovalCtrl"
                                    //});
                                    //$rootScope.modalInstance.result.then(function (response) {
                                    //    setTimeout(function () {
                                    //        applyExpandables();
                                    //    }, 1000);
                                    //});

                                }, function error(response) {

                                });

                      
                    //, function error(response) {

                //});

            $scope.viewPO = function (id) {
                console.log("In view");
                console.log(id);

                //var purchaseOrderDetails = [{
                //    "CostType": "U", "ProjectElementId": 771, "PhaseDescription": "Project Initiation", "purchaseOrderNumber": "009090", "purchaseOrderID": 1,
                //    "BudgetCategory": "Request for Proposal", "BudgetSubCategory": "Budget", "CostLineItemID": "13210011301271018311",
                //    "Name": "MODULAR NETWORK ADAPTER",
                //    "Description": "Adapter - MODULAR NETWORK ADAPTER", "UniqueIdentityNumber": "BM00126", "UnitOfMeasurement": "Each",
                //    "UnitPrice": "19.2", "ClassRefFullName": "13.Software Development", "CustomerRefFullName": "BC00001.TBD:13.Software Development:001.RBP4000:01.RBPE4000:27.1018.1001",
                //    "AmountOrQuantity": 920.0, "TCost": 96, "RequestedAmountOrQuantity": 5.0,
                //    "UtilizedAmountOrQuantity": 0.0, "BalancedAmountOrQuantity": 7.0, "TotalCost": 17664.0, "EmployeeID": null
                //},
                //{
                //    "CostType": "U", "ProjectElementId": 771, "PhaseDescription": "Project Initiation", "purchaseOrderNumber": "009090", "purchaseOrderID": 1, "BudgetCategory": "Request for Proposal", "BudgetSubCategory": "Budget", "CostLineItemID": "13210011301271018314",
                //    "Name": "POWERSONIC 12 VDC 7 AH BATTERY F1", "Description": "Battery - POWERSONIC 12 VDC 7 AH BATTERY F1", "UniqueIdentityNumber": "BM00140", "UnitOfMeasurement": "Each", "UnitPrice": "15.59", "RequestedAmountOrQuantity": 5.0,
                //    "ClassRefFullName": "13.Software Development", "CustomerRefFullName": "BC00001.TBD:13.Software Development:001.RBP4000:01.RBPE4000:27.1018.1001", "AmountOrQuantity": 160.0, "TCost": 77.95,
                //    "UtilizedAmountOrQuantity": 0.0, "BalancedAmountOrQuantity": 7.0, "TotalCost": 2494.4, "EmployeeID": null
                //}
                //    , {
                //    "CostType": "L", "ProjectElementId": 771, "PhaseDescription": "Project Initiation", "purchaseOrderNumber": "009091", "purchaseOrderID": 2, "BudgetCategory": "Request for Proposal", "BudgetSubCategory": "Budget", "CostLineItemID": "13210011301271018402",
                //    "Name": "Other", "Description": "ACAMS vendors - Other", "UniqueIdentityNumber": "BS00009", "UnitOfMeasurement": "",
                //    "UnitPrice": "", "ClassRefFullName": "13.Software Development",
                //    "CustomerRefFullName": "BC00001.TBD:13.Software Development:001.RBP4000:01.RBPE4000:27.1018.1001",
                //    "AmountOrQuantity": 3220.0, "TCost": 0, "RequestedAmountOrQuantity": 0.0, "UtilizedAmountOrQuantity": 0.0, "BalancedAmountOrQuantity": 7.0, "TotalCost": 3220.0, "EmployeeID": null
                //}, {
                //    "CostType": "ODC", "ProjectElementId": 771, "PhaseDescription": "Project Initiation", "purchaseOrderNumber": "009090", "purchaseOrderID": 1,
                //    "BudgetCategory": "Request for Proposal", "BudgetSubCategory": "Budget", "CostLineItemID": "13210011301271018209",
                //    "Name": "Food", "Description": "miscellaneous", "UniqueIdentityNumber": "BO00002", "UnitOfMeasurement": "",
                //    "UnitPrice": "", "ClassRefFullName": "13.Software Development",
                //    "CustomerRefFullName": "BC00001.TBD:13.Software Development:001.RBP4000:01.RBPE4000:27.1018.1001",
                //    "AmountOrQuantity": 2184.0, "TCost": 0, "RequestedAmountOrQuantity": 0.0, "UtilizedAmountOrQuantity": 0.0, "BalancedAmountOrQuantity": 7.0, "TotalCost": 2184.0, "EmployeeID": 10557
                //}];

                PurchaseOrderDetail.getPurchaseOrderIDDetail()
                    .get({ PurchaseOrderID: id }, function success(response) {
                        console.log(response.result);
                        var purchaseOrderDetails = response.result.poDetails;
                        $scope.purchaseOrder = response.result.po;

                        console.log("PO details==>");
                        console.log(response.result.poDetails);
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
                            templateUrl: "app/views/Modal/ApproveRejectPOMadal.html",
                            size: "lg",
                            windowClass: "w90p",
                            controller: "ApproveRejectPOModalCtrl"
                        });
                        $rootScope.modalInstance.result.then(function (response) {
                            setTimeout(function () {
                                applyExpandables();
                            }, 1000);
                        });


                    }, function error(response) {

                    });

            }

        }]);