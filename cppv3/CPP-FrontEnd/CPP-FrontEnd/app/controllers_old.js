'use strict';

//google.load('visualization', '1', {
//    packages: ['corechart']
//});
//
//google.setOnLoadCallback(function() {
//    angular.bootstrap(document.body, ['xenon-app']);
//});
$(document).ready(function(){
    angular.bootstrap(document.body, ['xenon-app']);
});
angular.module('xenon.controllers', []).

    controller('LoginLightCtrl', function ($scope, $rootScope) {
        $rootScope.isLoginPage = true;
        $rootScope.isLightLoginPage = true;
        $rootScope.isLockscreenPage = false;
        $rootScope.isMainPage = false;
    }).
    controller('LockscreenCtrl', function ($scope, $rootScope) {
        $rootScope.isLoginPage = false;
        $rootScope.isLightLoginPage = false;
        $rootScope.isLockscreenPage = true;
        $rootScope.isMainPage = false;
    }).
    controller('MainCtrl', ['$uibModal', '$scope', '$rootScope', '$location', '$layout', '$layoutToggles', '$pageLoadingBar', 'Fullscreen'
        ,'authService','UserName','localStorageService', 'TrendStatus', 'Page','ProjectTitle', '$transitions',
        function ($uibModal, $scope, $rootScope, $location, $layout, $layoutToggles, $pageLoadingBar, Fullscreen,
                  authService,UserName,localStorageService,TrendStatus, Page, ProjectTitle,$transitions) {
        $rootScope.isLoginPage = false;
        $rootScope.isLightLoginPage = false;
        $rootScope.isLockscreenPage = false;
        $rootScope.isMainPage = true;

        $rootScope.layoutOptions = {
            horizontalMenu: {
                isVisible: false,
                isFixed: true,
                minimal: false,
                clickToExpand: false,

                isMenuOpenMobile: false
            },
            sidebar: {
                isVisible: true,
                isCollapsed: true,
                toggleOthers: true,
                isFixed: true,
                isRight: false,
                isMenuOpenMobile: false,
                // Added in v1.3
                userProfile: true
            },
            chat: {
                isOpen: false
            },
            settingsPane: {
                isOpen: false,
                useAnimation: true
            },
            container: {
                isBoxed: false
            },
            skins: {
                sidebarMenu: '',
                horizontalMenu: '',
                userInfoNavbar: ''
            },
            pageTitles: true,
            userInfoNavVisible: false
        };

        $layout.loadOptionsFromCookies(); // remove this line if you don't want to support cookies that remember layout changes

            var state  = $location.path().split('/');

            $scope.gantt = state[2];
            $scope.logout = function(){

            }

            $scope.Page = Page;
            $scope.ProjectTitle = ProjectTitle;
            $scope.status = TrendStatus;

        //$scope.$on('USERNAME', function(event, args) {
        //    // do what you want to do
        //});
        $scope.updatePsScrollbars = function () {
            var $scrollbars = jQuery(".ps-scrollbar:visible");

            $scrollbars.each(function (i, el) {
                if (typeof jQuery(el).data('perfectScrollbar') == 'undefined') {
                    jQuery(el).perfectScrollbar();
                }
                else {
                    jQuery(el).perfectScrollbar('update');
                }
            })
        };


        // Define Public Vars
        public_vars.$body = jQuery("body");

       // $scope.status = TrendStatus.getStatus();
        // Init Layout Toggles
        $layoutToggles.initToggles();

        $scope.$on('user',function(event,obj){

            $scope.user = obj.userName;

        })
        var auth = localStorageService.get("authorizationData");
        if(auth){

            console.log(auth);
            $scope.user = auth.userName;
        }
        // Other methods
        $scope.setFocusOnSearchField = function () {
            public_vars.$body.find('.search-form input[name="s"]').focus();

            setTimeout(function () {
                public_vars.$body.find('.search-form input[name="s"]').focus()
            }, 100);
        };


        // Watch changes to replace checkboxes
        $scope.$watch(function () {
            cbr_replace();
        });

        // Watch sidebar status to remove the psScrollbar
        $rootScope.$watch('layoutOptions.sidebar.isCollapsed', function (newValue, oldValue) {
            if (newValue != oldValue) {
                if (newValue == true) {
                    public_vars.$sidebarMenu.find('.sidebar-menu-inner').perfectScrollbar('destroy')
                }
                else {
                    public_vars.$sidebarMenu.find('.sidebar-menu-inner').perfectScrollbar({wheelPropagation: public_vars.wheelPropagation});
                }
            }
        });


        // Page Loading Progress (remove/comment this line to disable it)
        $pageLoadingBar.init();

        $scope.showLoadingBar = showLoadingBar;
        $scope.hideLoadingBar = hideLoadingBar;


            // Set Scroll to 0 When page is changed
        $transitions.onStart({}, function () {
            var obj = { pos: jQuery(window).scrollTop() };

            TweenLite.to(obj, .25, {
                pos: 0, ease: Power4.easeOut, onUpdate: function () {
                    $(window).scrollTop(obj.pos);
                }
            });
        });
        //$rootScope.$on('$stateChangeStart', function () {
            
        //});
        $transitions.onStart({}, function () {
            detectRoute();
        });
        //$rootScope.$on('$stateChangeStart',detectRoute); deprecated
       function detectRoute(){
           //add back to implement submit for approval  : changetemp to cost-gantt
            $scope.active = $location.path().match(new RegExp('/temp'))?true:false;

        }
        $scope.proceed=function() {

            $rootScope.modalInstance = $uibModal.open({
                templateUrl: 'app/views/modal/approval_modal.html',
                controller: 'ApprovalModalCtrl',
                size: 'md'
            });
        }
            // Full screen feature added in v1.3
        $scope.newPopup =function(url) {
            popupWindow = window.open(
                url, 'popUpWindow', 'height=300,width=400,left=10,top=10,resizable=yes,scrollbars=yes,toolbar=yes,menubar=no,location=no,directories=no,status=yes')
        }
        $scope.isFullscreenSupported = Fullscreen.isSupported();
        $scope.isFullscreen = Fullscreen.isEnabled() ? true : false;
        $scope.removeClass=function(){
            $("div").removeClass('dropdown-backdrop');
        }

        $("#fullscreen").on('click', function () { alert(); })

     //   $scope.generateReport = function () {
       //     alert('test');
        //}
        $('#uploadBtnProject').unbind('click').on('click', function ($files) {
                //alert('Ready to Uplaod. Missing reference $http');
                //return;
                console.log('get files', $files);
                var docTypeID = $("#document_type  option").filter(":selected").val();
                var files = fileUpload.files;

                if (files.length == 0 || !files.length || !docTypeID) {
                    dhtmlx.alert('Please chose a doc type and select a file.');
                    return;
                }
                if (files[0].size / 1024 / 1024 > 128) {
                    dhtmlx.alert('File size exceed 128MB. Please select a smaller size file.');
                    return;
                }
                $('#uploadBtnProject').prop('disabled', false);
                $('#spinRow').show();

                angular.forEach(fileUpload.files, function (value, key) {
                    //$scope.selectedFileName = $files[0].name;
                    formdata.append(key, value);
                	//$('#uploadBtnProject').prop('disabled', false);
                });

                var request = {
                    method: 'POST',
                    url: serviceBasePath + '/uploadFiles/Post/' + wbsTree.getSelectedProjectID() + '/' + docTypeID,
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
                    var gridUploadedDocument = modal.find('.modal-body #gridUploadedDocument tbody');
                    gridUploadedDocument.empty();

                    //wbsTree.getDocument().getDocumentByProjID().get({ ProjectID: _selectedProjectID }, function (response) {
                    //    wbsTree.setDocumentList(response.result);
                    //    for (var x = 0; x < _documentList.length; x++) {
                    //        gridUploadedDocument.append('<tr><td style="width: 20px"><input type="checkbox" name="record"></td><td style="display:none">' + _documentList[x].DocumentID + '</td><td><a href="' + serviceBasePath + '/Request/DocumentByDocID/' + _documentList[x].DocumentID + '" download>' + _documentList[x].DocumentName + '</a></td><td>' + _documentList[x].DocumentTypeName + '</td><tr>');
                    //    }
                    $http.get(serviceBasePath + "Request/Document/Project/" + wbsTree.getSelectedProjectID())
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

             $scope.generateReport = function () {
                 //            baseUrl = serviceBasePath + 'Request/MasterInventoryExportFromCPPIntoJonasReport';
                 //  var pdfUrl = baseUrl
                 //     + '?CostType=' + '3'
                 //      + '&FileType=' + 'PDF';
                 //   var excelUrl = baseUrl
                 //       + '?CostType=' + '3'
                 //         + '&FileType=' + 'excel';
                 // const proxyurl = "https://cors-anywhere.herokuapp.com/";
                 //   const url = "https://localhost:54364/index.html#/app/wbs"; // site that doesn’t send Access-Control-*
                 //          fetch(proxyurl + url)
                 //       openPDFViewerModal();
                 //  openReportViewer(baseUrl, pdfUrl, excelUrl, $scope.reportTypeFilter.fileName);
        
                 //               function openReportViewer(baseUrl, pdfUrl, excelUrl, fileName) {
                 //    $http.get(pdfUrl).then(function success(response) {
                 //                   console.log(response);

                 //                 var scope = $rootScope.$new();

                 //Declare parameters for pdf viewer modal
                 //                      scope.params = {
                 //                        content: response.data,
                 //                      excelUrl: excelUrl,
                 //                    baseUrl: baseUrl,
                 //                  fileName: fileName,
                 //                contentType: 'base64',
                 //              excelDownloadable: true,
                 //            pdfDownloadable: true
                 //    };

                 //Open pdf viewer modal
                 //      openPDFViewerModal();

                 //          }, function error(response) {
                 //            console.log(response);
                 //      });
                 // }
                 //  pddUrl = 'https://code.angularjs.org/1.7.7/docs/error/$controller/ctrlreg?p0=WBSCtrl'
                 //     function getBase64(file) {
                 //       return new Promise((resolve, reject) => {
                 //         const reader = new FileReader();
                 //       reader.readAsDataURL(file);
                 //     reader.onload = () => resolve(reader.result);
                 //   reader.onerror = error => reject(error);
                 // });
                 // }
                 // $http.get( serviceBasePath+"Request/Document/"+'346' +'/' + '4').then(function(response) {                              
                 var scope = $rootScope.$new();
                 //    function openPDFViewerModal() {
                 scope.params = {
                     content: 'JVBERi0xLjYNJeLjz9MNCjM3IDAgb2JqIDw8L0xpbmVhcml6ZWQgMS9MIDIwNTk3L08gNDAvRSAxNDExNS9OIDEvVCAxOTc5NS9IIFsgMTAwNSAyMTVdPj4NZW5kb2JqDSAgICAgICAgICAgICAgICAgDQp4cmVmDQozNyAzNA0KMDAwMDAwMDAxNiAwMDAwMCBuDQowMDAwMDAxMzg2IDAwMDAwIG4NCjAwMDAwMDE1MjIgMDAwMDAgbg0KMDAwMDAwMTc4NyAwMDAwMCBuDQowMDAwMDAyMjUwIDAwMDAwIG4NCjAwMDAwMDIyNzQgMDAwMDAgbg0KMDAwMDAwMjQyMyAwMDAwMCBuDQowMDAwMDAyODQ0IDAwMDAwIG4NCjAwMDAwMDI4ODggMDAwMDAgbg0KMDAwMDAwMjkzMiAwMDAwMCBuDQowMDAwMDA0MTEzIDAwMDAwIG4NCjAwMDAwMDQxNDcgMDAwMDAgbg0KMDAwMDAwNDIxMSAwMDAwMCBuDQowMDAwMDA2ODgwIDAwMDAwIG4NCjAwMDAwMDcwMjMgMDAwMDAgbg0KMDAwMDAwNzE3MiAwMDAwMCBuDQowMDAwMDA3MzEyIDAwMDAwIG4NCjAwMDAwMDc0NTUgMDAwMDAgbg0KMDAwMDAwODE3NiAwMDAwMCBuDQowMDAwMDA4NTY2IDAwMDAwIG4NCjAwMDAwMDkwNjYgMDAwMDAgbg0KMDAwMDAxMjUxOCAwMDAwMCBuDQowMDAwMDEyNjY3IDAwMDAwIG4NCjAwMDAwMTI4MDMgMDAwMDAgbg0KMDAwMDAxMjkzOSAwMDAwMCBuDQowMDAwMDEzMDcyIDAwMDAwIG4NCjAwMDAwMTMyMDggMDAwMDAgbg0KMDAwMDAxMzM0NCAwMDAwMCBuDQowMDAwMDEzNDgwIDAwMDAwIG4NCjAwMDAwMTM2MzIgMDAwMDAgbg0KMDAwMDAxMzgxOCAwMDAwMCBuDQowMDAwMDE0MDM5IDAwMDAwIG4NCjAwMDAwMDEyMjAgMDAwMDAgbg0KMDAwMDAwMTAwNSAwMDAwMCBuDQp0cmFpbGVyDQo8PC9TaXplIDcxL1ByZXYgMTk3ODQvWFJlZlN0bSAxMjIwL1Jvb3QgMzkgMCBSL0VuY3J5cHQgMzggMCBSL0luZm8gNiAwIFIvSURbPEMyMUYyMUVBNDRDMUUyRUQyNTgxNDM1RkE1QTJEQ0NFPjwxNTM0OTEwNkQ5ODVEQTQ0OTkxMDk5RjlDMENCRjAwND5dPj4NCnN0YXJ0eHJlZg0KMA0KJSVFT0YNCiAgICAgICAgICAgICAgIA0KNzAgMCBvYmo8PC9MZW5ndGggMTIzL0MgMTI4L0ZpbHRlci9GbGF0ZURlY29kZS9JIDE1MS9MIDExMi9TIDQwPj5zdHJlYW0NCjA+v2UNc4Zmn6u4IiQguoMZnwg0NH0ymtRAWZUZqNfHLCiMQS0kyMfdiuvi04hlG2GJLNJzPFccsqvk0nZ7AHI4uCBvKj3L7sGXnAk1tHgOFgzOJjqRioIZMmIdwW51On/CmLK6+gsvbo3ivOa3aWeWo5GxxRL0DzNdRQ0KZW5kc3RyZWFtDWVuZG9iag02OSAwIG9iajw8L0xlbmd0aCAyMC9GaWx0ZXIvRmxhdGVEZWNvZGUvV1sxIDEgMV0vSW5kZXhbNyAzMF0vRGVjb2RlUGFybXM8PC9Db2x1bW5zIDMvUHJlZGljdG9yIDEyPj4vU2l6ZSAzNy9UeXBlL1hSZWY+PnN0cmVhbQ0KeNpiYmJkYGJgYKQ3BggwABbZAF0NCmVuZHN0cmVhbQ1lbmRvYmoNMzggMCBvYmo8PC9MZW5ndGggMTI4L0ZpbHRlci9TdGFuZGFyZC9PKJ6imv11rrw5sF4j3R+ObJ1lZ2RcbuwZDDZAs8jdl58OFSkvUCAtMTM0MC9SIDMvVSjj41C/LnKptSQ/7nBNpOwWAAAAAAAAAAAAAAAAAAAAACkvViAyPj4NZW5kb2JqDTM5IDAgb2JqPDwvTWFya0luZm88PC9MZXR0ZXJzcGFjZUZsYWdzIDAvTWFya2VkIHRydWU+Pi9NZXRhZGF0YSA1IDAgUi9QaWVjZUluZm88PC9NYXJrZWRQREY8PC9MYXN0TW9kaWZpZWQoCM1RrvFnz1Fb5exxrGnDVyk+Pj4+L1BhZ2VzIDQgMCBSL1BhZ2VMYXlvdXQvT25lQ29sdW1uL1N0cnVjdFRyZWVSb290IDcgMCBSL1R5cGUvQ2F0YWxvZy9MYW5nKAm5TsuSKS9MYXN0TW9kaWZpZWQoCM1RrvFnz1Fb5exxrGnDVykvUGFnZUxhYmVscyAyIDAgUj4+DWVuZG9iag00MCAwIG9iajw8L0Nyb3BCb3hbMCAwIDYxMiA3OTJdL0Fubm90cyA0MSAwIFIvUGFyZW50IDQgMCBSL1N0cnVjdFBhcmVudHMgMC9Db250ZW50cyA0NiAwIFIvUm90YXRlIDAvTWVkaWFCb3hbMCAwIDYxMiA3OTJdL1Jlc291cmNlczw8L1hPYmplY3Q8PC9JbTEwIDUwIDAgUi9JbTExIDUxIDAgUi9JbTEyIDUyIDAgUi9JbTEzIDUzIDAgUi9JbTE0IDU1IDAgUi9JbTAgNTcgMCBSL0ltMSA1OCAwIFIvSW0yIDU5IDAgUi9JbTMgNjAgMCBSL0ltNCA2MSAwIFIvSW01IDYyIDAgUi9JbTYgNjMgMCBSL0ltNyA2NCAwIFIvSW04IDY1IDAgUi9JbTkgNjYgMCBSPj4vQ29sb3JTcGFjZTw8L0NTMCA0NCAwIFIvQ1MxIDQ3IDAgUi9DUzIgNDUgMCBSPj4vRm9udDw8L1RUMCA0MyAwIFI+Pi9Qcm9jU2V0Wy9QREYvVGV4dC9JbWFnZUMvSW1hZ2VJXS9FeHRHU3RhdGU8PC9HUzAgNjggMCBSPj4+Pi9UeXBlL1BhZ2U+Pg1lbmRvYmoNNDEgMCBvYmpbNDIgMCBSXQ1lbmRvYmoNNDIgMCBvYmo8PC9SZWN0WzIyMC42OCA0NjcuODggMzg5LjQ2MSA0ODMuODUyXS9TdWJ0eXBlL0xpbmsvQlM8PC9TL1MvVyAwL1R5cGUvQm9yZGVyPj4vQSA0OCAwIFIvSC9JL1N0cnVjdFBhcmVudCAxL0JvcmRlclswIDAgMF0vVHlwZS9Bbm5vdD4+DWVuZG9iag00MyAwIG9iajw8L1N1YnR5cGUvVHJ1ZVR5cGUvRm9udERlc2NyaXB0b3IgNjcgMCBSL0xhc3RDaGFyIDEyMS9XaWR0aHNbMjc4IDI3OCAwIDAgMCAwIDAgMCAzMzMgMzMzIDAgMCAyNzggMCAyNzggMjc4IDU1NiA1NTYgNTU2IDU1NiAwIDAgNTU2IDU1NiAwIDAgMjc4IDAgMCAwIDAgMCAwIDY2NyA2NjcgNzIyIDcyMiA2NjcgNjExIDAgMCAwIDAgMCAwIDAgMCAwIDY2NyAwIDAgMCA2MTEgMCAwIDk0NCAwIDY2NyAwIDAgMCAwIDAgMCAwIDU1NiA1NTYgNTAwIDU1NiA1NTYgMjc4IDU1NiA1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aWdodCAzL1R5cGUvWE9iamVjdD4+c3RyZWFtDQo3R633u5PILuNh2uqR/GEFIj0vDQplbmRzdHJlYW0NZW5kb2JqDTU5IDAgb2JqPDwvU3VidHlwZS9JbWFnZS9MZW5ndGggNy9CaXRzUGVyQ29tcG9uZW50IDgvQ29sb3JTcGFjZSA0NyAwIFIvV2lkdGggMS9IZWlnaHQgMi9UeXBlL1hPYmplY3Q+PnN0cmVhbQ0K14/pGKyXxA0KZW5kc3RyZWFtDWVuZG9iag02MCAwIG9iajw8L1N1YnR5cGUvSW1hZ2UvTGVuZ3RoIDcvQml0c1BlckNvbXBvbmVudCA4L0NvbG9yU3BhY2UgNDcgMCBSL1dpZHRoIDIvSGVpZ2h0IDEvVHlwZS9YT2JqZWN0Pj5zdHJlYW0NCrN3Ds5BHkkNCmVuZHN0cmVhbQ1lbmRvYmoNNjEgMCBvYmo8PC9TdWJ0eXBlL0ltYWdlL0xlbmd0aCA0L0JpdHNQZXJDb21wb25lbnQgOC9Db2xvclNwYWNlIDQ3IDAgUi9XaWR0aCAxL0hlaWdodCAxL1R5cGUvWE9iamVjdD4+c3RyZWFtDQrx8DB4DQplbmRzdHJlYW0NZW5kb2JqDTYyIDAgb2JqPDwvU3VidHlwZS9JbWFnZS9MZW5ndGggNy9CaXRzUGVyQ29tcG9uZW50IDgvQ29sb3JTcGFjZSA0NyAwIFIvV2lkdGggMi9IZWlnaHQgMS9UeXBlL1hPYmplY3Q+PnN0cmVhbQ0K9/8xai1TaA0KZW5kc3RyZWFtDWVuZG9iag02MyAwIG9iajw8L1N1YnR5cGUvSW1hZ2UvTGVuZ3RoIDcvQml0c1BlckNvbXBvbmVudCA4L0NvbG9yU3BhY2UgNDcgMCBSL1dpZHRoIDIvSGVpZ2h0IDEvVHlwZS9YT2JqZWN0Pj5zdHJlYW0NCiN83Skmd7gNCmVuZHN0cmVhbQ1lbmRvYmoNNjQgMCBvYmo8PC9TdWJ0eXBlL0ltYWdlL0xlbmd0aCA3L0JpdHNQZXJDb21wb25lbnQgOC9Db2xvclNwYWNlIDQ3IDAgUi9XaWR0aCAyL0hlaWdodCAxL1R5cGUvWE9iamVjdD4+c3RyZWFtDQpDpbHuCaveDQplbmRzdHJlYW0NZW5kb2JqDTY1IDAgb2JqPDwvU3VidHlwZS9J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MDAwMDAwIDY1NTM1IGYNCjAwMDAwMTQxMTUgMDAwMDAgbg0KMDAwMDAxNDk2NSAwMDAwMCBuDQowMDAwMDE0OTk4IDAwMDAwIG4NCjAwMDAwMTUwMjEgMDAwMDAgbg0KMDAwMDAxNTA3MiAwMDAwMCBuDQowMDAwMDE5NDg0IDAwMDAwIG4NCjAwMDAwMDAwMDAgNjU1MzUgZg0KMDAwMDAwMDAwMCA2NTUzNSBmDQowMDAwMDAwMDAwIDY1NTM1IGYNCjAwMDAwMDAwMDAgNjU1MzUgZg0KMDAwMDAwMDAwMCA2NTUzNSBmDQowMDAwMDAwMDAwIDY1NTM1IGYNCjAwMDAwMDAwMDAgNjU1MzUgZg0KMDAwMDAwMDAwMCA2NTUzNSBmDQowMDAwMDAwMDAwIDY1NTM1IGYNCjAwMDAwMDAwMDAgNjU1MzUgZg0KMDAwMDAwMDAwMCA2NTUzNSBmDQowMDAwMDAwMDAwIDY1NTM1IGYNCjAwMDAwMDAwMDAgNjU1MzUgZg0KMDAwMDAwMDAwMCA2NTUzNSBmDQowMDAwMDAwMDAwIDY1NTM1IGYNCjAwMDAwMDAwMDAgNjU1MzUgZg0KMDAwMDAwMDAwMCA2NTUzNSBmDQowMDAwMDAwMDAwIDY1NTM1IGYNCjAwMDAwMDAwMDAgNjU1MzUgZg0KMDAwMDAwMDAwMCA2NTUzNSBmDQowMDAwMDAwMDAwIDY1NTM1IGYNCjAwMDAwMDAwMDAgNjU1MzUgZg0KMDAwMDAwMDAwMCA2NTUzNSBmDQowMDAwMDAwMDAwIDY1NTM1IGYNCjAwMDAwMDAwMDAgNjU1MzUgZg0KMDAwMDAwMDAwMCA2NTUzNSBmDQowMDAwMDAwMDAwIDY1NTM1IGYNCjAwMDAwMDAwMDAgNjU1MzUgZg0KMDAwMDAwMDAwMCA2NTUzNSBmDQowMDAwMDAwMDAwIDY1NTM1IGYNCnRyYWlsZXINCjw8L1NpemUgMzcvRW5jcnlwdCAzOCAwIFI+Pg0Kc3RhcnR4cmVmDQoxMTYNCiUlRU9GDQo=',
                     excelUrl: '',
                     baseUrl:  '',
                     fileName: 'ReadME',
                     contentType: 'base64',
                     excelDownloadable: false,
                     pdfDownloadable: true
                 };

                 $rootScope.modalInstance = $uibModal.open({
                     backdrop: 'static',
                     keyboard: false,
                     scope: scope,
                     templateUrl: "app/views/modal/PDFViewerModal.html",
                     size: "md",
                     controller: "PDFViewerCtrl"
                 });
                 //  }
             }


        $scope.logout = function () {

            console.log("logout function: controllers.js");
            authService.logOut();
            $location.path('/login');
        };

        $scope.changePassword = function () {
            console.log('change password function: controllers.js');
            var scope = $rootScope.$new();
            var newOrEdit = "new";
            scope.params = {
                selectedOrg: null,
                newOrEdit: newOrEdit
            };
            console.log(scope.params);
            $rootScope.modalInstance = $uibModal.open({
                scope: scope,
                backdrop: 'static',
                keyboard: false,
                templateUrl: 'app/views/modal/change_password_modal.html',
                controller: 'ChangePasswordCtrl',
                size: 'md'
            });
            $rootScope.modalInstance.result.then(function () {

            });
        };

        $scope.viewBom = function () {
            console.log('change password function: controllers.js');
            var scope = $rootScope.$new();
            var newOrEdit = "new";
            scope.params = {
                selectedOrg: null,
                newOrEdit: newOrEdit
            };
            console.log(scope.params);
            $rootScope.modalInstance = $uibModal.open({
                scope: scope,
                backdrop: 'static',
                keyboard: false,
                templateUrl: 'app/views/modal/change_password_modal.html',
                controller: 'BOMCtrl',
                size: 'md'
            });
            $rootScope.modalInstance.result.then(function () {

            });
        };

        $scope.test = "AFAF";
        $scope.goFullscreen = function () {
           // $scope.logout();
            //if (Fullscreen.isEnabled())
            //    Fullscreen.cancel();
            //else
            //    Fullscreen.all();
            //
            //$scope.isFullscreen = Fullscreen.isEnabled() ? true : false;
        }
        $(".fa-lock").on('click',function(){
           alert();
        });


    }]).
    controller('SidebarMenuCtrl', function ($scope, $rootScope, $menuItems, $timeout, $location, $state, $layout, $transitions) {

        // Menu Items
        $scope.collapse= function(item){
          console.log($('#main-menu').find('li:eq(1)').hasClass('expanded'));
            var isExpanded = $('#main-menu').find('li:eq(1)').hasClass('expanded');
            var adminList = $('#main-menu').find('li:eq(1)');
            console.log(isExpanded);

            //if(isExpanded == true){
            //    adminList.removeClass('expanded');
            //}else{
            //    adminList.addClass('expanded');
            //}
            //console.log($('#main-menu').find('li:eq(1)'));

        }
        var $sidebarMenuItems = $menuItems.instantiate();

        $scope.menuItems = $sidebarMenuItems.prepareSidebarMenu().getAll();
        console.log($scope.menuItems);
            var isFirstClick=true;

            $scope.goHome = function(){
                if(isFirstClick) {
                    isFirstClick=false;
                    $timeout(function () {
                        isFirstClick=true;
                        if ($location.path() == "/app/wbs") {
                            $state.reload();
                        }else{
                            $location.path("/app/wbs");
                        }
                    }, 500);
                }
                //$scope.buttonDisabled = true;
                //if ($location.path() == "/app/wbs") {
                //                $state.reload();
                //
                //            }

            }
        // Set Active Menu Item

        $sidebarMenuItems.setActive($location.path());
        $transitions.onSuccess({}, function () {
            $sidebarMenuItems.setActive($state.current.name);
        });
        //$rootScope.$on('$stateChangeSuccess', function () {

        //    $sidebarMenuItems.setActive($state.current.name);


        //});

        // Trigger menu setup
        public_vars.$sidebarMenu = public_vars.$body.find('.sidebar-menu');

        $timeout(setup_sidebar_menu, 1);

        ps_init(); // perfect scrollbar for sidebar
    }).
    controller('HorizontalMenuCtrl', function ($scope, $rootScope, $menuItems, $timeout, $location, $state,$transitions) {
        var $horizontalMenuItems = $menuItems.instantiate();

        $scope.menuItems = $horizontalMenuItems.prepareHorizontalMenu().getAll();

        // Set Active Menu Item
        $horizontalMenuItems.setActive($location.path());

        $transitions.onSuccess({}, function () {

                $horizontalMenuItems.setActive($state.current.name);

                $(".navbar.horizontal-menu .navbar-nav .hover").removeClass('hover'); // Close Submenus when item is selected

        });


        //$rootScope.$on('$stateChangeSuccess', function () {
        //    $horizontalMenuItems.setActive($state.current.name);

        //    $(".navbar.horizontal-menu .navbar-nav .hover").removeClass('hover'); // Close Submenus when item is selected
        //});

        // Trigger menu setup
        $timeout(setup_horizontal_menu, 1);
    }).
    controller('SettingsPaneCtrl', function ($rootScope) {
        // Define Settings Pane Public Variable
        public_vars.$settingsPane = public_vars.$body.find('.settings-pane');
        public_vars.$settingsPaneIn = public_vars.$settingsPane.find('.settings-pane-inner');
    }).
/*
    controller('ChatCtrl', function ($scope, $element) {
        var $chat = jQuery($element),
            $chat_conv = $chat.find('.chat-conversation');

        $chat.find('.chat-inner').perfectScrollbar(); // perfect scrollbar for chat container


        // Chat Conversation Window (sample)
        $chat.on('click', '.chat-group a', function (ev) {
            ev.preventDefault();

            $chat_conv.toggleClass('is-open');

            if ($chat_conv.is(':visible')) {
                $chat.find('.chat-inner').perfectScrollbar('update');
                $chat_conv.find('textarea').autosize();
            }
        });

        $chat_conv.on('click', '.conversation-close', function (ev) {
            ev.preventDefault();

            $chat_conv.removeClass('is-open');
        });
    }).
    controller('UIModalsCtrl', function ($scope, $rootScope, $uibModal, $sce) {
        // Open Simple Modal
        $scope.openModal = function (modal_id, modal_size, modal_backdrop) {
            $rootScope.currentModal = $uibModal.open({
                templateUrl: modal_id,
                size: modal_size,
                backdrop: typeof modal_backdrop == 'undefined' ? true : modal_backdrop
            });
        };
        // Loading AJAX Content
        $scope.openAjaxModal = function (modal_id, url_location) {
            $rootScope.currentModal = $uibModal.open({
                templateUrl: modal_id,
                resolve: {
                    ajaxContent: function ($http) {
                        return $http.get(url_location).then(function (response) {
                            $rootScope.modalContent = $sce.trustAsHtml(response.data);
                        }, function (response) {
                            $rootScope.modalContent = $sce.trustAsHtml('<div class="label label-danger">Cannot load ajax content! Please check the given url.</div>');
                        });
                    }
                }
            });

            $rootScope.modalContent = $sce.trustAsHtml('Modal content is loading...');
        }
    }).
    controller('PaginationDemoCtrl', function ($scope) {
        $scope.totalItems = 64;
        $scope.currentPage = 4;

        $scope.setPage = function (pageNo) {
            $scope.currentPage = pageNo;
        };

        $scope.pageChanged = function () {
            console.log('Page changed to: ' + $scope.currentPage);
        };

        $scope.maxSize = 5;
        $scope.bigTotalItems = 175;
        $scope.bigCurrentPage = 1;
    }).
*/
/*
    controller('LayoutVariantsCtrl', function ($scope, $layout, $cookies) {
        $scope.opts = {
            sidebarType: null,
            fixedSidebar: null,
            sidebarToggleOthers: null,
            sidebarVisible: null,
            sidebarPosition: null,

            horizontalVisible: null,
            fixedHorizontalMenu: null,
            horizontalOpenOnClick: null,
            minimalHorizontalMenu: null,

            sidebarProfile: null
        };

        $scope.sidebarTypes = [
            {
                value: ['sidebar.isCollapsed', false],
                text: 'Expanded',
                selected: $layout.is('sidebar.isCollapsed', false)
            },
            {
                value: ['sidebar.isCollapsed', true],
                text: 'Collapsed',
                selected: $layout.is('sidebar.isCollapsed', true)
            },
        ];

        $scope.fixedSidebar = [
            {value: ['sidebar.isFixed', true], text: 'Fixed', selected: $layout.is('sidebar.isFixed', true)},
            {value: ['sidebar.isFixed', false], text: 'Static', selected: $layout.is('sidebar.isFixed', false)},
        ];

        $scope.sidebarToggleOthers = [
            {value: ['sidebar.toggleOthers', true], text: 'Yes', selected: $layout.is('sidebar.toggleOthers', true)},
            {value: ['sidebar.toggleOthers', false], text: 'No', selected: $layout.is('sidebar.toggleOthers', false)},
        ];

        $scope.sidebarVisible = [
            {value: ['sidebar.isVisible', true], text: 'Visible', selected: $layout.is('sidebar.isVisible', true)},
            {value: ['sidebar.isVisible', false], text: 'Hidden', selected: $layout.is('sidebar.isVisible', false)},
        ];

        $scope.sidebarPosition = [
            {value: ['sidebar.isRight', false], text: 'Left', selected: $layout.is('sidebar.isRight', false)},
            {value: ['sidebar.isRight', true], text: 'Right', selected: $layout.is('sidebar.isRight', true)},
        ];

        $scope.horizontalVisible = [
            {
                value: ['horizontalMenu.isVisible', true],
                text: 'Visible',
                selected: $layout.is('horizontalMenu.isVisible', true)
            },
            {
                value: ['horizontalMenu.isVisible', false],
                text: 'Hidden',
                selected: $layout.is('horizontalMenu.isVisible', false)
            }
        ];

        $scope.fixedHorizontalMenu = [
            {
                value: ['horizontalMenu.isFixed', true],
                text: 'Fixed',
                selected: $layout.is('horizontalMenu.isFixed', true)
            },
            {
                value: ['horizontalMenu.isFixed', false],
                text: 'Static',
                selected: $layout.is('horizontalMenu.isFixed', false)
            }
        ];

        $scope.horizontalOpenOnClick = [
            {
                value: ['horizontalMenu.clickToExpand', false],
                text: 'No',
                selected: $layout.is('horizontalMenu.clickToExpand', false)
            },
            {
                value: ['horizontalMenu.clickToExpand', true],
                text: 'Yes',
                selected: $layout.is('horizontalMenu.clickToExpand', true)
            }
        ];

        $scope.minimalHorizontalMenu = [
            {
                value: ['horizontalMenu.minimal', false],
                text: 'No',
                selected: $layout.is('horizontalMenu.minimal', false)
            },
            {
                value: ['horizontalMenu.minimal', true],
                text: 'Yes',
                selected: $layout.is('horizontalMenu.minimal', true)
            }
        ];

        $scope.chatVisibility = [
            {value: ['chat.isOpen', false], text: 'No', selected: $layout.is('chat.isOpen', false)},
            {value: ['chat.isOpen', true], text: 'Yes', selected: $layout.is('chat.isOpen', true)},
        ];

        $scope.boxedContainer = [
            {value: ['container.isBoxed', false], text: 'No', selected: $layout.is('container.isBoxed', false)},
            {value: ['container.isBoxed', true], text: 'Yes', selected: $layout.is('container.isBoxed', true)},
        ];

        $scope.sidebarProfile = [
            {value: ['sidebar.userProfile', false], text: 'No', selected: $layout.is('sidebar.userProfile', false)},
            {value: ['sidebar.userProfile', true], text: 'Yes', selected: $layout.is('sidebar.userProfile', true)},
        ];

        $scope.resetOptions = function () {
            $layout.resetCookies();
            window.location.reload();
        };

        var setValue = function (val) {
            if (val != null) {
                val = eval(val);
                $layout.setOptions(val[0], val[1]);
            }
        };

        $scope.$watch('opts.sidebarType', setValue);
        $scope.$watch('opts.fixedSidebar', setValue);
        $scope.$watch('opts.sidebarToggleOthers', setValue);
        $scope.$watch('opts.sidebarVisible', setValue);
        $scope.$watch('opts.sidebarPosition', setValue);

        $scope.$watch('opts.horizontalVisible', setValue);
        $scope.$watch('opts.fixedHorizontalMenu', setValue);
        $scope.$watch('opts.horizontalOpenOnClick', setValue);
        $scope.$watch('opts.minimalHorizontalMenu', setValue);

        $scope.$watch('opts.chatVisibility', setValue);

        $scope.$watch('opts.boxedContainer', setValue);

        $scope.$watch('opts.sidebarProfile', setValue);
    }).
    controller('ThemeSkinsCtrl', function ($scope, $layout) {
        var $body = jQuery("body");

        $scope.opts = {
            sidebarSkin: $layout.get('skins.sidebarMenu'),
            horizontalMenuSkin: $layout.get('skins.horizontalMenu'),
            userInfoNavbarSkin: $layout.get('skins.userInfoNavbar')
        };

        $scope.skins = [
            {value: '', name: 'Default', palette: ['#2c2e2f', '#EEEEEE', '#FFFFFF', '#68b828', '#27292a', '#323435']},
            {value: 'aero', name: 'Aero', palette: ['#558C89', '#ECECEA', '#FFFFFF', '#5F9A97', '#558C89', '#255E5b']},
            {value: 'navy', name: 'Navy', palette: ['#2c3e50', '#a7bfd6', '#FFFFFF', '#34495e', '#2c3e50', '#ff4e50']},
            {
                value: 'facebook',
                name: 'Facebook',
                palette: ['#3b5998', '#8b9dc3', '#FFFFFF', '#4160a0', '#3b5998', '#8b9dc3']
            },
            {
                value: 'turquoise',
                name: 'Truquoise',
                palette: ['#16a085', '#96ead9', '#FFFFFF', '#1daf92', '#16a085', '#0f7e68']
            },
            {value: 'lime', name: 'Lime', palette: ['#8cc657', '#ffffff', '#FFFFFF', '#95cd62', '#8cc657', '#70a93c']},
            {
                value: 'green',
                name: 'Green',
                palette: ['#27ae60', '#a2f9c7', '#FFFFFF', '#2fbd6b', '#27ae60', '#1c954f']
            },
            {
                value: 'purple',
                name: 'Purple',
                palette: ['#795b95', '#c2afd4', '#FFFFFF', '#795b95', '#27ae60', '#5f3d7e']
            },
            {
                value: 'white',
                name: 'White',
                palette: ['#FFFFFF', '#666666', '#95cd62', '#EEEEEE', '#95cd62', '#555555']
            },
            {
                value: 'concrete',
                name: 'Concrete',
                palette: ['#a8aba2', '#666666', '#a40f37', '#b8bbb3', '#a40f37', '#323232']
            },
            {
                value: 'watermelon',
                name: 'Watermelon',
                palette: ['#b63131', '#f7b2b2', '#FFFFFF', '#c03737', '#b63131', '#32932e']
            },
            {
                value: 'lemonade',
                name: 'Lemonade',
                palette: ['#f5c150', '#ffeec9', '#FFFFFF', '#ffcf67', '#f5c150', '#d9a940']
            },
        ];

        $scope.$watch('opts.sidebarSkin', function (val) {
            if (val != null) {
                $layout.setOptions('skins.sidebarMenu', val);

                $body.attr('class', $body.attr('class').replace(/\sskin-[a-z]+/)).addClass('skin-' + val);
            }
        });

        $scope.$watch('opts.horizontalMenuSkin', function (val) {
            if (val != null) {
                $layout.setOptions('skins.horizontalMenu', val);

                $body.attr('class', $body.attr('class').replace(/\shorizontal-menu-skin-[a-z]+/)).addClass('horizontal-menu-skin-' + val);
            }
        });

        $scope.$watch('opts.userInfoNavbarSkin', function (val) {
            if (val != null) {
                $layout.setOptions('skins.userInfoNavbar', val);

                $body.attr('class', $body.attr('class').replace(/\suser-info-navbar-skin-[a-z]+/)).addClass('user-info-navbar-skin-' + val);
            }
        });
    }).
    // Added in v1.3
*/


/*
    controller('FooterChatCtrl', function ($scope, $element) {
        $scope.isConversationVisible = false;

        $scope.toggleChatConversation = function () {
            $scope.isConversationVisible = !$scope.isConversationVisible;

            if ($scope.isConversationVisible) {
                setTimeout(function () {
                    var $el = $element.find('.ps-scrollbar');

                    if ($el.hasClass('ps-scroll-down')) {
                        $el.scrollTop($el.prop('scrollHeight'));
                    }

                    $el.perfectScrollbar({
                        wheelPropagation: false
                    });

                    $element.find('.form-control').focus();

                }, 300);
            }
        }
    }).
    controller('signupController', ['$scope', '$location', '$timeout', 'authService', function ($scope, $location, $timeout, authService) {

        $scope.savedSuccessfully = false;
        $scope.message = "";

        $scope.registration = {
            userName: "",
            password: "",
            confirmPassword: ""
        };

        $scope.signUp = function () {

            authService.saveRegistration($scope.registration).then(function (response) {

                    $scope.savedSuccessfully = true;
                    $scope.message = "User has been registered successfully, you will be redicted to login page in 2 seconds.";
                    startTimer();

                },
                function (response) {
                    var errors = [];
                    for (var key in response.data.modelState) {
                        for (var i = 0; i < response.data.modelState[key].length; i++) {
                            errors.push(response.data.modelState[key][i]);
                        }
                    }
                    $scope.message = "Failed to register user due to:" + errors.join(' ');
                });
        };

        var startTimer = function () {
            var timer = $timeout(function () {
                $timeout.cancel(timer);
                $location.path('/login');
            }, 2000);
        }

    }]).
    controller('loginController', ['$scope','$rootScope', '$location', 'authService', function ($scope,$rootScope, $location, authService) {

        $scope.loginData = {
            userName: "",
            password: ""
        };

        $scope.message = "";

        $scope.login = function () {

              console.log($scope.loginData);
            authService.login($scope.loginData).then(function (response) {
                    window.location.hash = '#/app/wbs';
                },
                function (err) {
                    $scope.message = err.error_description;
                });
        };

    }]).
*/

    controller('navbarCtrl', function ($scope, Page,ProjectTitle,TrendStatus,$location) {
        var state  = $location.path().split('/');

        console.log(state);
        $scope.gantt = state[2];
        $scope.logout = function(){
            alert();
        }
        //$scope.isTrend = false;
        //if(gantt     === "cost-gantt"){
        //    alert()
        //    $scope.isTrend = true;
        //}
        $scope.Page = Page;
        $scope.ProjectTitle = ProjectTitle;
        $scope.status = TrendStatus;
    });

