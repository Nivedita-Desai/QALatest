angular.module('cpp.controllers').
    controller('BackupCtrl', ['$scope', '$timeout', '$uibModal', '$rootScope', '$http', 'Program', 'ProgramElement', 'Project', 'Organization', 'Page',
        function ($scope, $timeout, $uibModal, $rootScope, $http, Program, ProgramElement, Project, Organization, Page) {

            Page.setTitle('Back up');

            $('.modal-backdrop').hide();

            (function () {
                if (window.history && window.history.pushState) {
                    $(window).on('popstate', function () {
                        $scope.$close('close');
                    });
                }
            })();

          

            getBackup();
           
            //Call to initialize all lists for the filters, making backend calls
            function getBackup() {
                //alert("Hi");
                var request = {
                    method: 'GET',
                    url: serviceBasePath + 'Request/Backup',
                    data: '', //fileUploadProject.files, //$scope.
                    ignore: true,
                    headers: {
                        'Content-Type': undefined
                    }
                };
                $http(request).then(function success(response) {
                    alert(response.data);
                    window.location.hash = '#/app/wbs';
                });
            }

            
        }
    ]);