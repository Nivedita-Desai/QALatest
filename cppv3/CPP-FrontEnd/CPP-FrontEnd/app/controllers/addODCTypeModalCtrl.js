angular.module('cpp.controllers').
    controller('AddODCTypeModalCtrl', ['$scope', '$timeout', '$uibModal', '$rootScope', '$http', '$uibModalInstance', 'UniqueIdentityNumber',
function ($scope, $timeout, $uibModal, $rootScope, $http, $uibModalInstance, UniqueIdentityNumber) {

            $('.modal-backdrop').hide();

            (function () {
                if (window.history && window.history.pushState) {
                    $(window).on('popstate', function () {
                        $scope.$close('close');
                    });
                }
            })();

            $scope.odcType = {};
            var url = serviceBasePath + 'response/ODCType/';

            //When clicked on back button or X
            $scope.goBack = function (param) {
                $scope.$close(param);
            }

        	//Suggests next unique identity number
            UniqueIdentityNumber.get({
            	NumberType: 'ODCType',
            	'OrganizationID': 0,
            	'PhaseID': 0,
            	'CategoryID': 0
            }, function (response) {
            	console.log(response);
            	$scope.odcType.UniqueIdentityNumber = response.result;
            });

            //Initialization
            $scope.odcType.ODCTypeName = "";
            $scope.odcType.ODCDescription = "";

            //When clicked on save button
            $scope.save = function () {
                console.log($scope.odcType);

                if ($scope.odcType.ODCTypeName == undefined || $scope.odcType.ODCTypeName == null || $scope.odcType.ODCTypeName == '') {
                    dhtmlx.alert('Please enter a name');
                    return;
                }

                if ($scope.odcType.ODCTypeName.includes('\'') || $scope.odcType.ODCTypeName.includes('\"') || $scope.odcType.ODCTypeName.includes('\\') || $scope.odcType.ODCTypeName.includes(',')
                    || $scope.odcType.ODCDescription.includes('\'') || $scope.odcType.ODCDescription.includes('\"') || $scope.odcType.ODCDescription.includes('\\') || $scope.odcType.ODCDescription.includes(',')) {
                    dhtmlx.alert('Special characters of single quote, double quote, backslash, and comma are not allowed');
                    return;
                }

                if (($scope.odcType.UniqueIdentityNumber == undefined || $scope.odcType.UniqueIdentityNumber == null || $scope.odcType.UniqueIdentityNumber == '')) {
                	dhtmlx.alert('Please enter a unique identifier');
                	return;
                }

                if (!(/(BO[0-9]{5})/.test($scope.odcType.UniqueIdentityNumber) && $scope.odcType.UniqueIdentityNumber.length == 7)) {
                	dhtmlx.alert({
                		text: "Unique identifier must be in the format of BOxxxxx",
                		width: "400px"
                	});
                	return;
                }

                $scope.odcType.Operation = 1;  //1 means to create

                var listToSave = [];
                var param = { status: '', objectSaved: $scope.odcType };

                listToSave.push($scope.odcType);

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
                    		NumberType: 'ODCType',
                    		'OrganizationID': 0,
                    		'PhaseID': 0,
                    		'CategoryID': 0
                    	}, function (response) {
                    		console.log(response);
                    		$scope.odcType.UniqueIdentityNumber = response.result;
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