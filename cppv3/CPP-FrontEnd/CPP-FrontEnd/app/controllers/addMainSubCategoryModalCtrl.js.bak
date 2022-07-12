angular.module('cpp.controllers').
    controller('AddMainSubCategoryModalCtrl', ['$scope', '$timeout', '$uibModal', '$rootScope', '$http', '$uibModalInstance', 'UniqueIdentityNumber',
        function ($scope, $timeout, $uibModal, $rootScope, $http, $uibModalInstance, UniqueIdentityNumber) {

            $('.modal-backdrop').hide();

            //alert($('.gantt_cal_light').css('zIndex'));
            $('.gantt_cal_light').css('z-index', '1000');
            $('.gantt_cal_cover').hide();

            /* Added by Jignesh 06-10-2020 */
            $scope.subCategoryUpdatedList = [];
            var subCategoryList = [];
            var subCategory;
            /* End */

            (function () {
                if (window.history && window.history.pushState) {
                    $(window).on('popstate', function () {
                        $scope.$close('close');
                    });
                }
            })();


            var url = serviceBasePath + 'response/SingleActivityCategory/';

            //When clicked on back button or X
            $scope.goBackButtonCliked = function() {
                var param = { status: 'Cancel' };
                $scope.$close(param);
            }

            $scope.goBack = function (param) {
                $scope.$close(param);
            }

            console.log('here i am', $scope.params);
            //Initializations
            $scope.category = {};
            if ($scope.params.isSubCategory) {
                $scope.category.CategoryDescription = $scope.params.mainCategory;
                $scope.category.CategoryID = $scope.params.mainCategoryID;

                console.log($scope.params.phase);

            	//Suggests next subcategoryid
                UniqueIdentityNumber.get({
                	'NumberType': 'SubCategoryID',
                	'OrganizationID': $scope.params.organizationID,
                	'PhaseID': $scope.params.phase.PhaseID,
                	'CategoryID': $scope.params.mainCategoryID
                }, function (response) {
                	console.log(response);
                	$scope.category.SubCategoryID = response.result;
                });
            } else {
            	//Suggests next categoryid
            	UniqueIdentityNumber.get({
            		'NumberType': 'CategoryID',
            		'OrganizationID': $scope.params.organizationID,
            		'PhaseID': $scope.params.phase.PhaseID,
            		'CategoryID': 0
            	}, function (response) {
            		console.log(response);
            		$scope.category.CategoryID = response.result;

                    $scope.uniquecategoryid = $scope.category.CategoryID;  //Manasi 11-02-2021

            		if (parseInt($scope.category.CategoryID) < 1000) {
            			$scope.category.CategoryID = '1000';
            		}

            		$scope.category.SubCategoryID = '1000';
            	});
            }

            //When clicked on save button
            $scope.save = function () {
                console.log($scope.category);

                if ($scope.category.CategoryDescription == undefined || $scope.category.CategoryDescription == null || $scope.category.CategoryDescription == '') {
                    //dhtmlx.alert('Please enter main category');
                    dhtmlx.alert('Please enter main task'); //Manasi
                    return;
                }

                if (!Number.isInteger(parseFloat($scope.category.CategoryID)) || parseFloat($scope.category.CategoryID) < 0
                    || $scope.category.CategoryID.length < 4 || $scope.category.CategoryID.indexOf('.') >= 0) {
                    //dhtmlx.alert('Category ID must be integer values greater than or equal to 0. Length greater than or equal to 4.');
                    dhtmlx.alert('Task ID must be integer values greater than or equal to 0. Length greater than or equal to 4.'); //Manasi
                    return;
                }

                if ($scope.category.SubCategoryDescription == undefined || $scope.category.SubCategoryDescription == null || $scope.category.SubCategoryDescription == '') {
                    //dhtmlx.alert('Please enter a sub category');
                    dhtmlx.alert('Please enter a sub task'); //Manasi
                    return;
                }

                if (!Number.isInteger(parseFloat($scope.category.SubCategoryID)) || parseFloat($scope.category.SubCategoryID) < 0
                    || $scope.category.SubCategoryID.length < 4 || $scope.category.SubCategoryID.indexOf('.') >= 0) {
                    //dhtmlx.alert('SubCategory ID must be integer values greater than or equal to 0. Length greater than or equal to 4.');
                    dhtmlx.alert('SubTask ID must be integer values greater than or equal to 0. Length greater than or equal to 4.'); //Manasi
                    return;
                }

                if ($scope.category.CategoryDescription.includes('\'') || $scope.category.CategoryDescription.includes('\"') || $scope.category.CategoryDescription.includes('\\')
                    || $scope.category.SubCategoryDescription.includes('\'') || $scope.category.SubCategoryDescription.includes('\"') || $scope.category.SubCategoryDescription.includes('\\')) {
                    dhtmlx.alert('Special characters of single quote, double quote, and backslash are not allowed');
                    return;
                }

                $scope.category.Operation = 1;  //1 means to create

                $scope.category.VersionId = parseInt(projectData.VersionId);

                var phaseCode = '';
                for (var x = 0; x < $scope.params.phaseList.length; x++) {
                    if ($scope.params.phase.text == $scope.params.phaseList[x].PhaseDescription) {
                        phaseCode = $scope.params.phaseList[x].Code;
                    }
                }
                $scope.category.Phase = phaseCode;   //set the phase code abbreviation thingy
                $scope.category.OrganizationID = $scope.params.organizationID;

                var listToSave = [];
                var param = { status: '', objectSaved: $scope.category, message: '' };

                listToSave.push($scope.category);

                $http({
                    url: url,
                    method: "POST",
                    data: JSON.stringify(listToSave),
                    headers: { 'Content-Type': 'application/json' }
                }).then(function success(response) {
                    response.data.result.replace(/[\r]/g, '\n');

                    if (response.data.result.indexOf('successfully') >= 0) {
                        param.status = 'Success';
                        param.message = response.data.result;
                        param.subCategoryData = subCategory; // Jignesh 06-10-2020
                        $scope.goBack(param);
                    } else {
                        param.status = 'Failed';
                        dhtmlx.alert(response.data.result);
                        $('div.gantt_modal_box.dhtmlx_modal_box.gantt-confirm.dhtmlx-confirm').css('z-index', '100000001');
                    }
                }, function error(response) {
                    param.status = 'Failed';
                    dhtmlx.alert('Failed to save. Please contact your Administrator.');
                    $('div.gantt_modal_box.dhtmlx_modal_box.gantt-confirm.dhtmlx-confirm').css('z-index', '100000001');
                });
            }

            // added by Jignesh 06-10-20
            $scope.onMainTaskChange = function () {
                main_category = $scope.category.CategoryDescription;
                for (var x = 0; x < $scope.params.mainCategory.length; x++) {
                    if (main_category == $scope.params.mainCategory[x].CategoryDescription) {
                        $scope.category.CategoryID = $scope.params.mainCategory[x].CategoryID;
                        $http.get(serviceBasePath + "Request/SubActivityCategory/" + $scope.params.organizationID + "/" + $scope.params.phase.PhaseID + "/" + $scope.category.CategoryID + "/" + $scope.category.VersionId)
                            .then(function (response) {
                                subCategory = response.data.result;


                                if (subCategory.length > 0) {
                                    for (var i = 0; i < subCategory.length; i++) {
                                        subCategoryList.push(Number(subCategory[i].SubCategoryID));
                                    }
                                    var maxSubCategoryNum = $scope.biggestNumberInArray(subCategoryList);
                                    maxSubCategoryNum;
                                    $scope.category.SubCategoryID = (maxSubCategoryNum + 1001).toString();
                                }
                                else {
                                    $scope.category.SubCategoryID = '1000';
                                }

                            });
                        return;
                    }
                    else {
                        $scope.category.CategoryID = $scope.uniquecategoryid;
                    }
                }
            }
            //added by jignesh 06-10-2020
            $scope.biggestNumberInArray = function (arr) {
                var largest = arr[0] || null;
                // Current number, handled by the loop
                var number = null;
                for (var i = 0; i < arr.length; i++) {
                    // Update current number
                    number = arr[i];
                    // Compares stored largest number with current number, stores the largest one
                    largest = Math.max(largest, number);
                }
                return largest;
            }
        }
    ]);