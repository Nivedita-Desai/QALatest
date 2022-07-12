angular.module('cpp.controllers').
    controller('FiledownloadCtrl', ['$http', 'Page', '$state', 'UpdateLocation', '$uibModal', 'Location', '$scope', '$rootScope', 'ProjectTitle', 'TrendStatus', '$location', '$timeout',
        function ($http, Page, $state, UpdateLocation, $uibModal, Location, $scope, $rootScope, ProjectTitle, TrendStatus, $location, $timeout) {
            var okToExit = true;
            Page.setTitle('File Download');
            ProjectTitle.setTitle('');
            TrendStatus.setStatus('');
            $scope.checkedRow = [];

            $scope.gridOPtions = {};
            $scope.myExternalScope = $scope;

            console.log('luan test');
            var formdata = new FormData();
            $('#uploadBtnProject').unbind('click').on('click', function ($files) {
                //alert('Ready to Uplaod. Missing reference $http');
                //return;
                console.log('get files', $files);
                var docTypeID = '0';
                var files = fileUpload.files;
                if (files.length == 0 || !files.length || !docTypeID) {
                    dhtmlx.alert('Please chose a doc type and select a file.');
                    return;
                }
                if (files[0].size / 1024 / 1024 > 128) {
                    dhtmlx.alert('File size exceed 128MB. Please select a smaller size file.');
                    return;
                }
                $('#uploadBtnProject').prop('disabled', true);
                $('#spinRow').show();

                angular.forEach(fileUpload.files, function (value, key) {
                    //$scope.selectedFileName = $files[0].name;
                    formdata.append(key, value);
                	//$('#uploadBtnProject').prop('disabled', false);
                });

                var request = {
                    method: 'POST',
                    url: serviceBasePath + '/uploadFiles/Post/' + "0" + '/' + docTypeID,
                    data: formdata, //fileUpload.files, //$scope.
                    ignore: true,
                    headers: {
                        'Content-Type': undefined
                    }
                };

                /* Needs $http */
                // SEND THE FILES.
                $http(request).then(function success(d){
                    console.log(d);
                  //  alert('File has been created successfully.');
                    //var gridUploadedDocument = modal.find('.modal-body #gridUploadedDocument tbody');
                   // gridUploadedDocument.empty();

                    //wbsTree.getDocument().getDocumentByProjID().get({ ProjectID: _selectedProjectID }, function (response) {
                    //    wbsTree.setDocumentList(response.result);
                    //    for (var x = 0; x < _documentList.length; x++) {
                    //        gridUploadedDocument.append('<tr><td style="width: 20px"><input type="checkbox" name="record"></td><td style="display:none">' + _documentList[x].DocumentID + '</td><td><a href="' + serviceBasePath + '/Request/DocumentByDocID/' + _documentList[x].DocumentID + '" download>' + _documentList[x].DocumentName + '</a></td><td>' + _documentList[x].DocumentTypeName + '</td><tr>');
                    //    }
                    $http.get(serviceBasePath + "Request/Document/" + "0")
                        .then(function success(response) {
                            console.log(response);
                            wbsTree.setDocumentList(response.data.result);
                            for (var x = 0; x < _documentList.length; x++) {
                                gridUploadedDocument.append('<tr id="' + _documentList[x].DocumentID + '"><td style="width: 20px"><input type="checkbox" name="record"></td><td><a href="' + serviceBasePath + 'Request/DocumentByDocID/' + _documentList[x].DocumentID + '">' + _documentList[x].DocumentName + '</a></td><td>' + _documentList[x].DocumentTypeName + '</td><td>' + moment(_documentList[x].CreatedDate).format('MM/DD/YYYY') + '</td><tr>');
                            }

                           var deleteDocBtn = modal.find('.modal-body #delete-doc');
                            deleteDocBtn.attr('disabled', _documentList.length > 0 ? false : true);
                        },function error(){}).
                        finally(function () {
                            console.log($rootScope);
                            // $rootScope.buttonDisabled =false;
                        })

                    dhtmlx.alert(d.data);
                },function error(d){
                    dhtmlx.alert(d.ExceptionMessage);
                }).finally(function (){
                    //Clear selected files
                    fileUpload.value = "";
                    formdata = new FormData();
                    $('#uploadBtnProject').prop('disabled', false);
                    $('#spinRow').hide();
                })
                    
                ;
                  
            });
        }]);