angular.module('cpp.controllers').
    controller('AddSubcontractorModalCtrl', ['$scope', '$timeout', '$uibModal', '$rootScope', '$http', '$uibModalInstance', 'UniqueIdentityNumber',
        function ($scope, $timeout, $uibModal, $rootScope, $http, $uibModalInstance, UniqueIdentityNumber) {

            $('.modal-backdrop').hide();

            (function () {
                if (window.history && window.history.pushState) {
                    $(window).on('popstate', function () {
                        $scope.$close('close');
                    });
                }
            })();

            $scope.subcontractor = {};
            var url = serviceBasePath + 'response/Subcontractor';

            //When clicked on back button or X
            $scope.goBack = function (param) {
                $scope.$close(param);
            }

            console.log('here i am', $scope.params);

            //Suggests next unique identity number
            UniqueIdentityNumber.get({
            	NumberType: 'Subcontractor',
            	'OrganizationID': 0,
            	'PhaseID': 0,
            	'CategoryID': 0
            }, function (response) {
                console.log(response);
                $scope.subcontractor.UniqueIdentityNumber = response.result;
            });

            //Initialization
            $scope.subcontractor.SubcontractorName = "";
            $scope.subcontractor.SubcontractorDescription = "";

            //When clicked on save button
            $scope.save = function () {
                console.log($scope.subcontractor);

                if ($scope.subcontractor.SubcontractorName == undefined || $scope.subcontractor.SubcontractorName == null || $scope.subcontractor.SubcontractorName == '') {
                    dhtmlx.alert('Please enter a name');
                    return;
                }

                if ($scope.subcontractor.SubcontractorName.includes('\'') || $scope.subcontractor.SubcontractorName.includes('\"') || $scope.subcontractor.SubcontractorName.includes('\\') || $scope.subcontractor.SubcontractorName.includes(',')
                    || $scope.subcontractor.SubcontractorDescription.includes('\'') || $scope.subcontractor.SubcontractorDescription.includes('\"') || $scope.subcontractor.SubcontractorDescription.includes('\\') || $scope.subcontractor.SubcontractorDescription.includes(',')) {
                    dhtmlx.alert('Special characters of single quote, double quote, backslash, and comma are not allowed');
                    return;
                }

                if (($scope.subcontractor.UniqueIdentityNumber == undefined || $scope.subcontractor.UniqueIdentityNumber == null || $scope.subcontractor.UniqueIdentityNumber == '')) {
                    dhtmlx.alert('Please enter a unique identifier');
                    return;
                }

                if (!(/(BS[0-9]{5})/.test($scope.subcontractor.UniqueIdentityNumber) && $scope.subcontractor.UniqueIdentityNumber.length == 7)) {
                    dhtmlx.alert({
                        text: "Unique identifier must be in the format of BSxxxxx",
                        width: "400px"
                    });
                    return;
                }

                $scope.subcontractor.Operation = 1;  //1 means to create
                $scope.subcontractor.SubcontractorTypeID = $scope.params.subcontractorType.value;   //set the subcontractor type id

                var listToSave = [];
                var param = { status: '', objectSaved: $scope.subcontractor };

                listToSave.push($scope.subcontractor);

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
                    		NumberType: 'Subcontractor',
                    		'OrganizationID': 0,
                    		'PhaseID': 0,
                    		'CategoryID': 0
                    	}, function (response) {
                    		console.log(response);
                    		$scope.subcontractor.UniqueIdentityNumber = response.result;
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