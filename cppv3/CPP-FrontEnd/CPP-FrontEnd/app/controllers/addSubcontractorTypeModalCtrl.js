angular.module('cpp.controllers').
    controller('AddSubcontractorTypeModalCtrl', ['$scope', '$timeout', '$uibModal', '$rootScope', '$http', '$uibModalInstance', 'UniqueIdentityNumber',
        function ($scope, $timeout, $uibModal, $rootScope, $http, $uibModalInstance, UniqueIdentityNumber) {

            $('.modal-backdrop').hide();

            (function () {
                if (window.history && window.history.pushState) {
                    $(window).on('popstate', function () {
                        $scope.$close('close');
                    });
                }
            })();

            $scope.subcontractorType = {};
            var url = serviceBasePath + 'response/SubcontractorType/';

            //When clicked on back button or X
            $scope.goBack = function (param) {
                $scope.$close(param);
            }

        	//Suggests next unique identity number
            UniqueIdentityNumber.get({
            	NumberType: 'SubcontractorType',
            	'OrganizationID': 0,
            	'PhaseID': 0,
            	'CategoryID': 0
            }, function (response) {
            	console.log(response);
            	$scope.subcontractorType.UniqueIdentityNumber = response.result;
            });

            //Initialization
            $scope.subcontractorType.SubcontractorTypeName = "";
            $scope.subcontractorType.SubcontractorTypeDescription = "";

            //When clicked on save button
            $scope.save = function () {
                console.log($scope.subcontractorType);

                if ($scope.subcontractorType.SubcontractorTypeName == undefined || $scope.subcontractorType.SubcontractorTypeName == null || $scope.subcontractorType.SubcontractorTypeName == '') {
                    dhtmlx.alert('Please enter a name');
                    return;
                }

                if ($scope.subcontractorType.SubcontractorTypeName.includes('\'') || $scope.subcontractorType.SubcontractorTypeName.includes('\"') || $scope.subcontractorType.SubcontractorTypeName.includes('\\') || $scope.subcontractorType.SubcontractorTypeName.includes(',')
                    || $scope.subcontractorType.SubcontractorTypeDescription.includes('\'') || $scope.subcontractorType.SubcontractorTypeDescription.includes('\"') || $scope.subcontractorType.SubcontractorTypeDescription.includes('\\') || $scope.subcontractorType.SubcontractorTypeDescription.includes(',')) {
                    dhtmlx.alert('Special characters of single quote, double quote, backslash, and comma are not allowed');
                    return;
                }

                if (($scope.subcontractorType.UniqueIdentityNumber == undefined || $scope.subcontractorType.UniqueIdentityNumber == null || $scope.subcontractorType.UniqueIdentityNumber == '')) {
                	dhtmlx.alert('Please enter a unique identifier');
                	return;
                }

                if (!(/(BST[0-9]{5})/.test($scope.subcontractorType.UniqueIdentityNumber) && $scope.subcontractorType.UniqueIdentityNumber.length == 8)) {
                	dhtmlx.alert({
                		text: "Unique identifier must be in the format of BSTxxxxx",
                		width: "400px"
                	});
                	return;
                }

                $scope.subcontractorType.Operation = 1;  //1 means to create

                var listToSave = [];
                var param = { status: '', objectSaved: $scope.subcontractorType };

                listToSave.push($scope.subcontractorType);

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
                    		NumberType: 'SubcontractorType',
                    		'OrganizationID': 0,
                    		'PhaseID': 0,
                    		'CategoryID': 0
                    	}, function (response) {
                    		console.log(response);
                    		$scope.subcontractorType.UniqueIdentityNumber = response.result;
                    	});
                    } else {
                        dhtmlx.alert(response.data.result);
                    }
                }, function error(response) {
                    dhtmlx.alert("Failed to save. Please contact your Administrator.");
                });
            }
        }
    ]);