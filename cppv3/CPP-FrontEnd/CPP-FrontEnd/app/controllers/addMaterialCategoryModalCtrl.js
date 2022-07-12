angular.module('cpp.controllers').
    controller('AddMaterialCategoryModalCtrl', ['$scope', '$timeout', '$uibModal', '$rootScope', '$http', '$uibModalInstance', 'UniqueIdentityNumber',
        function ($scope, $timeout, $uibModal, $rootScope, $http, $uibModalInstance, UniqueIdentityNumber) {

            $('.modal-backdrop').hide();

            (function () {
                if (window.history && window.history.pushState) {
                    $(window).on('popstate', function () {
                        $scope.$close('close');
                    });
                }
            })();

            $scope.materialCategory = {};
            var url = serviceBasePath + 'response/MaterialCategory/';

            //When clicked on back button or X
            $scope.goBack = function (param) {
                $scope.$close(param);
            }

        	//Suggests next unique identity number
            UniqueIdentityNumber.get({
            	NumberType: 'MaterialCategory',
            	'OrganizationID': 0,
            	'PhaseID': 0,
            	'CategoryID': 0
            }, function (response) {
            	console.log(response);
            	$scope.materialCategory.UniqueIdentityNumber = response.result;
            });

            //Initialization
            $scope.materialCategory.Name = "";
            $scope.materialCategory.Description = "";

            //When clicked on save button
            $scope.save = function () {
                console.log($scope.materialCategory);

                if ($scope.materialCategory.Name == undefined || $scope.materialCategory.Name == null || $scope.materialCategory.Name == '') {
                    dhtmlx.alert('Please enter a name');
                    return;
                }

                if ($scope.materialCategory.Name.includes('\'') || $scope.materialCategory.Name.includes('\"') || $scope.materialCategory.Name.includes('\\') || $scope.materialCategory.Name.includes(',')
                    || $scope.materialCategory.Description.includes('\'') || $scope.materialCategory.Description.includes('\"') || $scope.materialCategory.Description.includes('\\') || $scope.materialCategory.Description.includes(',')) {
                    dhtmlx.alert('Special characters of single quote, double quote, backslash, and comma are not allowed');
                    return;
                }

                if (($scope.materialCategory.UniqueIdentityNumber == undefined || $scope.materialCategory.UniqueIdentityNumber == null || $scope.materialCategory.UniqueIdentityNumber == '')) {
                	dhtmlx.alert('Please enter a unique identifier');
                	return;
                }

                if (!(/(BMC[0-9]{5})/.test($scope.materialCategory.UniqueIdentityNumber) && $scope.materialCategory.UniqueIdentityNumber.length == 8)) {
                	dhtmlx.alert({
                		text: "Unique identifier must be in the format of BMCxxxxx",
                		width: "400px"
                	});
                	return;
                }

                $scope.materialCategory.Operation = 1;  //1 means to create

                var listToSave = [];
                var param = { status: '', objectSaved: $scope.materialCategory };

                listToSave.push($scope.materialCategory);

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
                    		NumberType: 'MaterialCategory',
                    		'OrganizationID': 0,
                    		'PhaseID': 0,
                    		'CategoryID': 0
                    	}, function (response) {
                    		console.log(response);
                    		$scope.materialCategory.UniqueIdentityNumber = response.result;
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