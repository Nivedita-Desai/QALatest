angular.module('cpp.controllers').
    controller('ReceivedBOMModalCtrl', ['$scope', '$timeout', '$uibModal', '$rootScope', '$http', '$uibModalInstance', 'UniqueIdentityNumber',
        function ($scope, $timeout, $uibModal, $rootScope, $http, $uibModalInstance, UniqueIdentityNumber) {

            $('.modal-backdrop').hide();

            (function () {
                if (window.history && window.history.pushState) {
                    $(window).on('popstate', function () {
                        $scope.$close('close');
                    });
                }
            })();

            $scope.material = {};
            var url = serviceBasePath + 'response/Material';

            //When clicked on back button or X
            $scope.goBack = function (param) {
                $scope.$close(param);
            }

            console.log('here i am', $scope.params);

            //Initialization
            $scope.material.Name = "";
            $scope.material.Description = "";

            //When clicked on save button
            $scope.save = function () {

                console.log($scope.material);
                if ($scope.material.VendorID == undefined || $scope.material.VendorID == null || $scope.material.VendorID == '') {
                    dhtmlx.alert('Please enter a Vendor name');
                    return;
                }
                if ($scope.material.Name == undefined || $scope.material.Name == null || $scope.material.Name == '') {
                    dhtmlx.alert('Please enter a name');
                    return;
                }

                if ($scope.material.Name.includes('\'') || $scope.material.Name.includes('\"') || $scope.material.Name.includes('\\') || $scope.material.Name.includes(',')
                    || $scope.material.Description.includes('\'') || $scope.material.Description.includes('\"') || $scope.material.Description.includes('\\') || $scope.material.Description.includes(',')) {
                    dhtmlx.alert('Special characters of single quote, double quote, backslash, and comma are not allowed');
                    return;
                }

                if ($scope.material.Cost == undefined || $scope.material.Cost == null || $scope.material.Cost == ''
                    || isNaN($scope.material.Cost) || Math.sign($scope.material.Cost) < 0) {
                    dhtmlx.alert('Please enter a cost');
                    return;
                }
                if ($scope.material.UnitTypeID == undefined || $scope.material.UnitTypeID == null || $scope.material.UnitTypeID == '') {
                    dhtmlx.alert('Please enter a unit type');
                    return;
                }
                if (($scope.material.UniqueIdentityNumber == undefined || $scope.material.UniqueIdentityNumber == null || $scope.material.UniqueIdentityNumber == '')) {
                    dhtmlx.alert('Please enter a unique identifier');
                    return;
                }

                if (!(/(BM[0-9]{5})/.test($scope.material.UniqueIdentityNumber) && $scope.material.UniqueIdentityNumber.length == 7)) {
                    dhtmlx.alert({
                        text: "Unique identifier must be in the format of BMxxxxx",
                        width: "400px"
                    });
                    return;
                }


                $scope.material.Operation = 1;  //1 means to create
                $scope.material.MaterialCategoryID = $scope.params.materialCategory.value;   //set the material category id

                var listToSave = [];
                var param = { status: '', objectSaved: $scope.material };

                listToSave.push($scope.material);

                $http({
                    url: url,
                    method: "POST",
                    data: JSON.stringify(listToSave),
                    headers: { 'Content-Type': 'application/json' }
                }).then(function success(response) {
                    response.data.result.replace(/[\r]/g, '\n');

                    if (response.data.result.indexOf("successfully") >= 0) {
                        dhtmlx.alert(response.data.result.replace(/&/g, '&#38').replace(/</g, '&#60').replace(/>/g, '&#62'));
                        param.status = 'Success';
                        $scope.goBack(param);
                    } else if (response.data.result.indexOf("duplicate unique identifier") >= 0) {
                        dhtmlx.alert("Duplicate unique identifier found during the time of saving. A new one has been generated. Please try to save again");
                        //Suggests next unique identity number
                        UniqueIdentityNumber.get({
                            NumberType: 'Material',
                            'OrganizationID': 0,
                            'PhaseID': 0,
                            'CategoryID': 0
                        }, function (response) {
                            console.log(response);
                            $scope.material.UniqueIdentityNumber = response.result;
                        });
                    }
                    else {
                        dhtmlx.alert(response.data.result);
                    }
                }, function error(response) {
                    dhtmlx.alert("Failed to save. Please contact your Administrator.");
                });
            }
        }
    ]);