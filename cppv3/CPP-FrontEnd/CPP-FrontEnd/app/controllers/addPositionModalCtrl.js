angular.module('cpp.controllers').
    controller('AddPositionModalCtrl', ['$scope', '$timeout', '$uibModal', '$rootScope', '$http', '$uibModalInstance', 'UniqueIdentityNumber',
        function ($scope, $timeout, $uibModal, $rootScope, $http, $uibModalInstance, UniqueIdentityNumber) {

            $('.modal-backdrop').hide();

            (function () {
                if (window.history && window.history.pushState) {
                    $(window).on('popstate', function () {
                        $scope.$close('close');
                    });
                }
            })();

            $scope.position = {};
            var url = serviceBasePath + 'response/FTEPosition/';

            //When clicked on back button or X
            $scope.goBack = function (param) {
                $scope.$close(param);
            }

        	//Suggests next unique identity number
            UniqueIdentityNumber.get({
            	NumberType: 'FTEPosition',
            	'OrganizationID': 0,
            	'PhaseID': 0,
            	'CategoryID': 0
            }, function (response) {
            	console.log(response);
            	$scope.position.UniqueIdentityNumber = response.result;
            });

            //When clicked on save button
            $scope.save = function () {
                console.log($scope.position);

                if ($scope.position.PositionDescription == undefined || $scope.position.PositionDescription == null || $scope.position.PositionDescription == '') {
                    dhtmlx.alert('Please enter a position title');
                    return;
                }

                if ($scope.position.PositionDescription.includes('\'') || $scope.position.PositionDescription.includes('\"') || $scope.position.PositionDescription.includes('\\') || $scope.position.PositionDescription.includes(',')) {
                    dhtmlx.alert('Special characters of single quote, double quote, backslash, and comma are not allowed');
                    return;
                }

                if ($scope.position.CurrentHourlyRate == undefined || $scope.position.CurrentHourlyRate == null || $scope.position.CurrentHourlyRate == ''
                    || isNaN($scope.position.CurrentHourlyRate) || Math.sign($scope.position.CurrentHourlyRate) < 0) {
                    dhtmlx.alert('Please enter a valid hourly rate');
                    return;
                }

                if (($scope.position.UniqueIdentityNumber == undefined || $scope.position.UniqueIdentityNumber == null || $scope.position.UniqueIdentityNumber == '')) {
                	dhtmlx.alert('Please enter a unique identifier');
                	return;
                }

                if (!(/(BEP[0-9]{5})/.test($scope.position.UniqueIdentityNumber) && $scope.position.UniqueIdentityNumber.length == 8)) {
                	dhtmlx.alert({
                		text: "Unique identifier must be in the format of BEPxxxxx",
                		width: "400px"
                	});
                	return;
                }

                $scope.position.Operation = 1;  //1 means to create

                var listToSave = [];
                var param = { status: '', objectSaved: $scope.position };
                
                listToSave.push($scope.position);

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
                    		NumberType: 'FTEPosition',
                    		'OrganizationID': 0,
                    		'PhaseID': 0,
                    		'CategoryID': 0
                    	}, function (response) {
                    		console.log(response);
                    		$scope.position.UniqueIdentityNumber = response.result;
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