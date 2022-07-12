angular.module('cpp.controllers').
    controller('AddEmployeeModalCtrl', ['$scope', '$timeout', '$uibModal', '$rootScope', '$http', '$uibModalInstance', 'UniqueIdentityNumber',
        function ($scope, $timeout, $uibModal, $rootScope, $http, $uibModalInstance, UniqueIdentityNumber) {

            $('.modal-backdrop').hide();

            (function () {
                if (window.history && window.history.pushState) {
                    $(window).on('popstate', function () {
                        $scope.$close('close');
                    });
                }
            })();

            $scope.employee = {};
            var url = serviceBasePath + 'response/employee';

            //When clicked on back button or X
            $scope.goBack = function (param) {
                $scope.$close(param);
            }

            console.log('here i am', $scope.params);

        	//Suggests next unique identity number
            UniqueIdentityNumber.get({
            	NumberType: 'Employee',
            	'OrganizationID': 0,
            	'PhaseID': 0,
            	'CategoryID': 0
            }, function (response) {
            	console.log(response);
            	$scope.employee.UniqueIdentityNumber = response.result;
            });

            //Iniitialization
            $scope.employee.FirstName = "";
            $scope.employee.LastName = "";
            $scope.employee.MiddleName = "";

            //When clicked on save button
            $scope.save = function () {
                console.log($scope.employee);

                //if ($scope.employee.Name == undefined || $scope.employee.Name == null || $scope.employee.Name == '') {
                //	dhtmlx.alert('Please enter a name');
                //	return;
                //}

                if ($scope.employee.FirstName == undefined || $scope.employee.FirstName == null || $scope.employee.FirstName == '') {
                    dhtmlx.alert('Please enter a first name');
                    return;
                }

                if ($scope.employee.LastName == undefined || $scope.employee.LastName == null || $scope.employee.LastName == '') {
                	dhtmlx.alert('Please enter a last name');
                	return;
                }

                if ($scope.employee.FirstName.includes('\'') || $scope.employee.FirstName.includes('\"') || $scope.employee.FirstName.includes('\\') || $scope.employee.FirstName.includes(',')
                    || $scope.employee.LastName.includes('\'') || $scope.employee.LastName.includes('\"') || $scope.employee.LastName.includes('\\') || $scope.employee.LastName.includes(',')
                    || $scope.employee.MiddleName.includes('\'') || $scope.employee.MiddleName.includes('\"') || $scope.employee.MiddleName.includes('\\') || $scope.employee.MiddleName.includes(',')) {
                    dhtmlx.alert('Special characters of single quote, double quote, backslash, and comma are not allowed');
                    return;
                }

                //if ($scope.employee.HourlyRate == undefined || $scope.employee.HourlyRate == null || $scope.employee.HourlyRate == ''
                //    || isNaN($scope.employee.HourlyRate) || Math.sign($scope.employee.HourlyRate) < 0) {
                //    dhtmlx.alert('Please enter a valid hourly rate');
                //    return;
                //}

                if (($scope.employee.UniqueIdentityNumber == undefined || $scope.employee.UniqueIdentityNumber == null || $scope.employee.UniqueIdentityNumber == '')) {
                    dhtmlx.alert('Please enter a unique identifier');
                    return;
                }

                if (!(/(BE[0-9]{5})/.test($scope.employee.UniqueIdentityNumber) && $scope.employee.UniqueIdentityNumber.length == 7)) {
                	dhtmlx.alert({
                		text: "Unique identifier must be in the format of BExxxxx",
                		width: "400px"
                	});
                	return;
                }

                $scope.employee.Operation = 1;  //1 means to create
                $scope.employee.FTEPositionID = $scope.params.position.value;   //set the position id
                $scope.employee.OrganizationID = $scope.params.organizationID;
                $scope.employee.isActive = 1;
                $scope.employee.Name = $scope.employee.LastName + ', ' + $scope.employee.FirstName;

                var listToSave = [];
                var param = { status: '', objectSaved: $scope.employee };

                listToSave.push($scope.employee);

                console.log(listToSave);

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
                    		NumberType: 'Employee',
                    		'OrganizationID': 0,
                    		'PhaseID': 0,
                    		'CategoryID': 0
                    	}, function (response) {
                    		console.log(response);
                    		$scope.employee.UniqueIdentityNumber = response.result;
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