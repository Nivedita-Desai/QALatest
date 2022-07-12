angular.module('cpp.controllers').
    controller('PDFViewerCtrl', ['$scope', '$window', '$uibModal', '$rootScope', '$sce', '$http', 'usSpinnerService',
        function ($scope, $window, $uibModal, $rootScope, $sce, $http, usSpinnerService) {

            (function () {
                if (window.history && window.history.pushState) {
                    $(window).on('popstate', function () {
                        $scope.$close('close');
                    });
                }
            })();

            console.log($scope.params);

            //$scope.pdfTitle = $scope.params.pdfTitle;

            var blobtest = null;

            if ($scope.params.contentType == 'base64') {
                var dedcodedRaw = atob($scope.params.content);

                var array = new Uint8Array(dedcodedRaw.length);

                for (i = 0; i < dedcodedRaw.length; i++) {
                    array[i] = dedcodedRaw.charCodeAt(i);
                }

                blobtest = new Blob([array], {
                    type: 'application/pdf',
                    name: 'test',
                });
            } else if($scope.params.contentType == 'blob') {
                blobtest = $scope.params.content;
            }

            if ($scope.params.contentType != 'url') {   //base64 and blob
                blobtesturl = URL.createObjectURL(blobtest);

                $scope.contentData = $sce.trustAsResourceUrl(blobtesturl);

                $scope.contentData = 'web/viewer.html?file=' + $scope.contentData;
            } else {    //url
                $scope.contentData = $sce.trustAsResourceUrl($scope.params.content);
            }


            $scope.downloadAsExcel = function () {
                if (!$scope.params.excelDownloadable) {
                    return;
                }

                console.log('downloading as excel');

                console.log($scope.params);

                $http.get($scope.params.excelUrl)
                .then(function success(response) {
                    console.log(response);
                    var dedcodedRaw = atob(response.data);
                    console.log('attempt excel blob');

                    var array = new Uint8Array(dedcodedRaw.length);

                    for (i = 0; i < dedcodedRaw.length; i++) {
                        array[i] = dedcodedRaw.charCodeAt(i);
                    }

                    var blobtest = new Blob([array], {
                        //type: 'application/vnd.ms-excel;charset=charset=utf-8'
                    });

                    //saveAs comes from FileSaver.js, faster method of saving files
                    saveAs(blobtest, $scope.params.fileName + '.xls');

                }, function error(response) {
                    console.log(response);
                });
            }

            $scope.downloadAsPDF = function () {
                if ($scope.params.contentType == 'blob') {
                    //saveAs comes from FileSaver.js, faster method of saving files
                    saveAs($scope.params.content, $scope.params.fileName);
                    return;
                }

                var dedcodedRaw = atob($scope.params.content);

                var array = new Uint8Array(dedcodedRaw.length);

                for (i = 0; i < dedcodedRaw.length; i++) {
                    array[i] = dedcodedRaw.charCodeAt(i);
                }

                var blobtest = new Blob([array], {
                    type: 'application/pdf'
                });

                //saveAs comes from FileSaver.js, faster method of saving files
                saveAs(blobtest, $scope.params.fileName);
            }

            $scope.goBack = function () {
                $scope.$close();
            };
        }]);
