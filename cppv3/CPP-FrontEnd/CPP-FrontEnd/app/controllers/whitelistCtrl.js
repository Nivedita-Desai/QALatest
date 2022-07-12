angular.module('cpp.controllers').
    //budget Category Controller
    controller('WhitelistCtrl', ['Employee', 'PhaseCode', '$state', '$scope', '$rootScope', 'Project', 'ProjectWhiteList', '$uibModal', 'UpdateCategory', '$http', 'Page', 'ProjectTitle', 'TrendStatus', '$location', '$timeout',
        function (Employee, PhaseCode, $state, $scope, $rootScope, Project, ProjectWhiteList, $uibModal, UpdateCategory, $http, Page, ProjectTitle, TrendStatus, $location, $timeout) {
            //Whitelist
            Page.setTitle('Whitelist');
            ProjectTitle.setTitle('');
            TrendStatus.setStatus('');

            //luan here - watcher experimental
            $scope.$watch('filterProject', function (newValue, oldValue) {
                console.log(newValue, oldValue);
            });

            $scope.currentProjectID = 0;
            var isChanged = false;
            var isFresh = true;

            Project.lookup().get({}, function (projectData) {
                $scope.projectList = projectData.result;
                console.log($scope.projectList);
                $scope.setProject(($scope.projectList[0]));
                $scope.currentProjectID = $scope.projectList[0].ProjectID;

                populateBlackWhiteList();

                $scope.filterProject = $scope.projectList[0];
                $scope.filterChangeProject($scope.filterProject.ProjectID);
            });

            var populateBlackWhiteList = function () {
                console.log($scope.currentProjectID);
                $scope.whitelist = [];
                $http.get(serviceBasePath + 'Request/AllEmployee').then(function (employeeData) {
                    var employeeInfo = employeeData.data.result;

                    $http.get(serviceBasePath + 'Request/ProjectWhiteList').then(function (response) {
                        var wholeProjectWhitelist = response.data.result;

                        for (var i = 0; i < wholeProjectWhitelist.length; i++) {

                            if (wholeProjectWhitelist[i].ProjectID == $scope.currentProjectID) {
                                var employeeID = wholeProjectWhitelist[i].EmployeeID;
                                for (var j = 0; j < employeeInfo.length; j++) {

                                    if (employeeInfo[j].ID == employeeID) {
                                        $scope.whitelist.push(employeeInfo[j]);
                                        break;
                                    }
                                }
                            }
                        }

                        console.log($scope.whitelist);
                        for (var i = $scope.whitelist.length - 1; i >= 0; i--) {
                            for (var j = i-1; j >= 0; j--) {
                                console.log($scope.whitelist[i]);
                                console.log($scope.whitelist[j]);
                                if ($scope.whitelist[i].ID == $scope.whitelist[j].ID) {
                                    $scope.whitelist.splice(j, 1);
                                    break;
                                }
                            }
                        }

                        for (var i = employeeInfo.length - 1; i >= 0 ; i--) {
                            for (var j = 0; j < $scope.whitelist.length; j++) {
                                if (employeeInfo[i].ID == $scope.whitelist[j].ID) {
                                    employeeInfo.splice(i, 1);
                                    console.log('spliced');
                                    break;
                                }
                            }
                        }

                        $scope.availableBlacklist = employeeInfo;
                        console.log('available blacklist');
                        console.log($scope.availableBlacklist);
                        console.log('whitelist');
                        console.log($scope.whitelist);

                    });

                });
            }


            $scope.setProject = function (project) {

                $scope.selectedProject = project;
                console.log("Here");
                console.log(project);
                console.log("End here");

            };

            $scope.filterChangeProject = function () {
                console.log($scope.filterProject.ProjectID);
                var projectID = null;
                if ($scope.filterProject.ProjectID) {
                    projectID = $scope.filterProject.ProjectID;
                    $scope.currentProjectID = $scope.filterProject.ProjectID;
                } else {
                    projectID = $("#selectProject").val();
                    $scope.currentProjectID = $("selectProject").val();
                }
                angular.forEach($scope.projectList, function (project) {
                    if (projectID == project.ProjectID) {
                        $scope.setProject(project);
                    }
                });
                if (!isFresh) populateBlackWhiteList(); 
                console.log('filterChangeProject ' + projectID);
                isFresh = false;
            }


            $scope.addSelected = function (selectedList) {
                console.log(selectedList.length);
                isChanged = true;
                console.log(isChanged);
                for (var i = 0; i < selectedList.length; i++) {
                    $scope.whitelist.push(selectedList[i]);
                }

                for (var i = $scope.availableBlacklist.length - 1; i >= 0 ; i--) {
                    for (var j = 0; j < $scope.whitelist.length; j++) {
                        if ($scope.availableBlacklist[i].ID == $scope.whitelist[j].ID) {
                            $scope.availableBlacklist.splice(i, 1);
                        }
                    }
                }
            }

            $scope.addAll = function () {
                isChanged = true;
                for (var i = $scope.availableBlacklist.length - 1; i >= 0; i--) {
                    $scope.whitelist.push($scope.availableBlacklist[i])
                    $scope.availableBlacklist.splice(i, 1);
                }
            }

            $scope.removeSelected = function (selectedList) {
                isChanged = true;
                console.log(selectedList, $scope.whitelist);
                for (var i = selectedList.length - 1; i >= 0 ; i--) {
                    $scope.availableBlacklist.push(selectedList[i]);
                    console.log('pushed');
                }

                for (var i = $scope.whitelist.length - 1; i >= 0; i--) {
                    for (var j = 0; j < $scope.availableBlacklist.length; j++) {
                        try {
                            if ($scope.whitelist[i].ID == $scope.availableBlacklist[j].ID) {
                                $scope.whitelist.splice(i, 1);
                                console.log('spliced');
                                break;
                            }
                        }
                        catch (error) {
                            console.log($scope.whitelist[i]);
                            console.log($scope.availableBlacklist[j]);
                        }

                    }
                }
            }

            $scope.removeAll = function () {
                isChanged = true;
                for (var i = $scope.whitelist.length - 1; i >= 0; i--) {
                    $scope.availableBlacklist.push($scope.whitelist[i])
                    $scope.whitelist.splice(i, 1);
                }
            }

            $scope.save = function () {
                console.log($scope.currentProjectID, $scope.whitelist);
                var listToSave = [];

                if (isChanged) {                      
                        for (var i = 0; i < $scope.whitelist.length; i++) {
                            listToSave.push({
                                "Operation": 1,
                                EmployeeID: $scope.whitelist[i].ID,
                                ProjectID: $scope.currentProjectID,
                                UserID: ''
                            });
                        }
                        if (listToSave.length == 0) {
                            listToSave.push({
                                "Operation": 2,
                                EmployeeID: 0,
                                ProjectID: $scope.currentProjectID,
                                UserID: ''
                            });
                        }
                            
                    $http({
                        // url: url,
                        url: 'http://localhost:29986/api/Response/ProjectWhiteList',
                        method: "POST",
                        data: JSON.stringify(listToSave),
                        headers: { 'Content-Type': 'application/json' }
                    }).then(function success(response) {
                        response.data.result.replace(/[\r]/g, '\n');

                        if (response.data.result) {
                            dhtmlx.alert('Saved.');
                        } else {
                            dhtmlx.alert('No changes to be saved.');
                        }
                    });
                    isChanged = false;
                }
                else {
                    dhtmlx.alert('No changes to be saved.')
                }
            }


            onRouteChangeOff = $scope.$on('$locationChangeStart', function (event) {
                var newUrl = $location.path();
                if (!isChanged) return;
                $scope.confirm = "";
                var scope = $rootScope.$new();
                scope.params = { confirm: $scope.confirm };
                $rootScope.modalInstance = $uibModal.open({
                    scope: scope,
                    templateUrl: 'app/views/Modal/exit_confirmation_modal.html',
                    controller: 'exitConfirmation',
                    size: 'md',
                    backdrop: true
                });
                $rootScope.modalInstance.result.then(function (data) {
                    console.log(scope.params.confirm);
                    if (scope.params.confirm === "exit") {
                        onRouteChangeOff();
                        $location.path(newUrl);
                    }
                    else if (scope.params.confirm === "save") {
                        $scope.save();
                        onRouteChangeOff();
                        $location.path(newUrl);
                    }
                    else if (scope.params.confirm === "back") {
                        //do nothing
                    }
                });
                event.preventDefault();
            });

        }])

.filter('mapProjectType', function () {
    return function (input) {
        if (!input)
            return ""
        if (!input.Type) {

            return input;
        } else if (input.Type) {
            return input.Type;
        } else {
            return 'unknown';
        }
        return input.Type;
    }
});

function gridLoad() {
}