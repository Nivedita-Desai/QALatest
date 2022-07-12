angular.module('cpp.controllers').
    controller('WBSCtrl', ['$state', 'ProjectTitle', 'UserName', '$http', '$location', '$scope', '$rootScope', '$uibModal', '$sce',
        'Page', 'Organization', 'Program', 'ProgramElement', 'Project', 'Trend', 'currentTrend', 'myLocalStorage', 'localStorageService',
        'RequestApproval', 'TrendStatus', 'FundType', '$location', '$stateParams', '$window', 'ProgramFund', 'usSpinnerService', '$filter',
        'ProjectScope', '$timeout', 'PhaseCode', 'ProgramCategory', 'ProjectType', 'ProjectClass', 'ProjectClassByProgramId', 'ProjectClassByProgramElementId', 'Client', 'Prime', 'Location', 'ProjectNumber',
        'Employee', 'AllEmployee', 'DocumentType', 'Document', 'TrendStatusCode', 'User', 'ProjectElementNumber', 'TrendId', 'LineOfBusiness', 'ProjectWhiteList', 'UpdateProjectWhiteList', 'UpdateContract',
        'Contract', 'ProgramContract', 'Milestone', 'ChangeOrder', 'UpdateMilestone', 'UpdateChangeOrder', 'ServiceClass', 'WbsService', '$cacheFactory', 'ClientPOC', 'UniqueIdentityNumber', 'CertifiedPayroll', 'Wrap',
        function ($state, ProjectTitle, UserName, $http, $location, $scope, $rootScope, $uibModal, $sce, Page, Organization, Program, ProgramElement,
            Project, Trend, currentTrend, myLocalStorage, localStorageService, RequestApproval, TrendStatus, FundType,
            $location, $stateParams, $window, ProgramFund, usSpinnerService, $filter, ProjectScope, $timeout, PhaseCode, ProgramCategory,
            ProjectType, ProjectClass, ProjectClassByProgramId, ProjectClassByProgramElementId, Client, Prime, Location, ProjectNumber, Employee, AllEmployee, DocumentType, Document, TrendStatusCode,
            User, ProjectElementNumber, TrendId, LineOfBusiness, ProjectWhiteList, UpdateProjectWhiteList, UpdateContract, Contract, ProgramContract, Milestone, ChangeOrder,
            UpdateMilestone, UpdateChangeOrder, ServiceClass, WbsService, $cacheFactory, ClientPOC, UniqueIdentityNumber, CertifiedPayroll, Wrap) {
            Page.setTitle('Program Navigation');
            ProjectTitle.setTitle('');
            TrendStatus.setStatus('');
            usSpinnerService.spin('spinner-1');//Use this if want to show spinner on data loading
            $scope.gridOPtions = {};

            //start spinner
            window.startSpin = function () {
                usSpinnerService.spin('spinner-1');
            }
            //stop spinner
            window.stopSpin = function () {
                usSpinnerService.stop('spinner-1');
            }

            $scope.$on('$locationChangeStart', function (event) {
                var newUrl = $location.path();
            });
            //An alternative method for clicking on the node using jquery
            jQuery.fn.d3Click = function () {
                this.each(function (i, e) {
                    //alert('click');
                    var evt = new MouseEvent("click");
                    e.dispatchEvent(evt);
                });
            };
            var formdata = new FormData();
            $scope.deleteDoc = [];
            var _modificationList = null;
            var _wbsTreeData = null;
            var g_program_element_change_order_draft_list = [];
            $('#fileUploadProject').change(function (ev) {
                console.log(fileUploadProject.files);
                $("#document_name_project").val(fileUploadProject.files[0].name);
            });

            $('#uploadBtnProject').unbind('click').on('click', function ($files) {
                debugger
                //alert('Ready to Uplaod. Missing reference $http');
                //return;

                if (wbsTree.getIsProjectNew()) {
                    dhtmlx.alert('Uploading files only work in edit mode.');
                    //$('#loading1').hide();   //Manasi 20-08-2020
                    $('#uploadBtnProjectspinRow').hide(); //Manasi 25-08-2020
                    return;
                }

                console.log('get files', $files);
                var x = wbsTree.getSelectedProjectID();
                var docTypeID = $("#document_type_programPrgElm  option").filter(":selected").val();
                var files = fileUploadProject.files;
                var docTypeName = $("#document_type_programPrgElm  option").filter(":selected").text();
                var SpecialNote = $("#PrgSpecialNotePrgElm").val();
                var DocumentName = $("#document_name_project").val().trim();
                var ExecutionDate = $("#PrgExecutionDatePrgElm").val();
                var DocID = $("#DocElementID").val();
                if (SpecialNote == "" || SpecialNote.length == 0) {
                    dhtmlx.alert('Enter Notes.');
                    return;
                }
                //================= Jignesh-03-03-2021 ==================================
                if (!docTypeID) {
                    dhtmlx.alert('Please choose a Document Type.');
                    return;
                }
                if (!g_editelementdocument) {
                    if (files.length == 0 || !files.length) {
                        dhtmlx.alert('Please select a file.');
                        return;
                    }
                    if (files[0].size / 1024 / 1024 > 128) {
                        dhtmlx.alert('File size exceed 128MB. Please select a smaller size file.');
                        return;
                    }
                    $('#fileUploadProject').prop('disabled', false);
                }
                if (DocumentName == "" || DocumentName.length == 0) {
                    dhtmlx.alert('Enter Document Name.');
                    return;
                }
                if (DocumentName != "" && DocumentName.length >= 1 && DocumentName.charAt(DocumentName.length - 1) == ".") {
                    dhtmlx.alert('A file name cannot end with a period');
                    return;
                }
                //=======================================================================
                if (DocumentName.includes("\\") || DocumentName.includes("/") || DocumentName.includes(":")
                    || DocumentName.includes("*") || DocumentName.includes("?") || DocumentName.includes('"')
                    || DocumentName.includes("<") || DocumentName.includes(">") || DocumentName.includes("|")) {
                    dhtmlx.alert('A file name cannot contain any of the following characters: \ / : * ? " < > |');
                    return;
                }



                // Jignesh-24-02-2021 Comment below sectoin
                //if (ExecutionDate == "" || ExecutionDate.length == 0) {
                //    dhtmlx.alert('Enter Execution Date.');
                //    return;
                //}

                //if (files.length == 0 || !files.length || !docTypeID) {
                //    dhtmlx.alert('Please chose a doc type and select a file.');
                //    return;
                //}
                //------------------------Manasi----------------------------------


                //$('#spinRow').show();
                //document.getElementById("loading1").style.display = "block"; // Manasi 25-08-2020
                document.getElementById("uploadBtnProjectspinRow").style.display = "block"; //Manasi 25-08-2020

                var fileName = "";
                formdata = new FormData();
                angular.forEach(fileUploadProject.files, function (value, key) {
                    console.log(value, key, fileUploadProject.files);
                    fileName = value.name;
                    formdata.append(key, value);
                    //$('#uploadBtnProject').prop('disabled', false);
                });

                console.log(formdata);


                //Dealing with new project element
                if (wbsTree.getIsProjectNew()) {
                    var gridUploadedDocument = $('#gridUploadedDocumentProgramNewPrgElm tbody');
                    gridUploadedDocument.empty();

                    //Insert upload file into draft list and update draft list
                    var draftList = [];
                    for (var x = 0; x < wbsTree.getProjectFileDraft().length; x++) {
                        draftList.push(wbsTree.getProjectFileDraft()[x]);
                    }

                    draftList.push({ docTypeID: docTypeID, formdata: formdata, fileName: fileName, docTypeName: docTypeName, SpecialNote: SpecialNote, ExecutionDate: ExecutionDate });


                    //  draftList.push({ docTypeID: docTypeID, formdata: formdata, fileName: fileName, docTypeName: docTypeName });
                    console.log(draftList);
                    wbsTree.setProjectFileDraft(draftList);

                    //Repopulate the document table - should be unclickable
                    for (var x = 0; x < wbsTree.getProjectFileDraft().length; x++) {
                        var rowFilename = wbsTree.getProjectFileDraft()[x].fileName;
                        var rowDocTypeName = wbsTree.getProjectFileDraft()[x].docTypeName;

                        var rowSpecialNote = wbsTree.getProgramFileDraft()[x].SpecialNote;
                        var rowExecutionDate = wbsTree.getProgramFileDraft()[x].ExecutionDate;
                        gridUploadedDocumentProgram.append('<tr class="contact-row" id="' + x + '"><td style="width: 20px">' +
                            '<input type="radio" group="rbCategoriesPrg" name="record"></td> <td>' + rowFilename + '</td>' +
                            '<td>' + rowSpecialNote + '</td>' +
                            '<td>' + rowExecutionDate + '</td>' +
                            '<td>' + rowDocTypeName + '</td> <td>' + moment().format('MM/DD/YYYY') + '</td> <tr>');
                        // gridUploadedDocument.append('<tr id="' + x + '"><td style="width: 20px"><input type="checkbox" name="record"></td><td>' + rowFilename + '</td><td>' + rowDocTypeName + '</td><td>' + moment().format('MM/DD/YYYY') + '</td><tr>');
                    }

                    return;
                }
                $("#uploadBtnProject").attr("disabled", true);
                if (!g_editelementdocument) {
                    // Jignesh-24-02-2021 Remove ''&ExecutionDate=' + ExecutionDate.replace(/\//g, "") +' from url from below request.
                    var request = {
                        method: 'POST',
                        url: serviceBasePath + '/uploadFilesnew/Postnew/Project/' + wbsTree.getSelectedProjectID() + '/0/0/0/0/' + docTypeID + '?SpecialNote=' + encodeURIComponent(SpecialNote) + '&DocumentName=' + encodeURIComponent(DocumentName),
                        //  url: serviceBasePath + '/uploadFiles/Post/Project/' + wbsTree.getSelectedProjectID() + '/0/0/0/0/' + docTypeID,
                        data: formdata, //fileUploadProject.files, //$scope.
                        ignore: true,
                        headers: {
                            'Content-Type': undefined
                        }
                    };

                    /* Needs $http */
                    // SEND THE FILES.
                    // Disable File Upload Delete and View button
                    $('#DeleteUploadProgramPrgElm').attr('disabled', 'disabled');
                    $('#ViewUploadFileProgramPrgElm').attr('disabled', 'disabled');
                    $('#EditBtnProgramPrgElm').attr('disabled', 'disabled');
                    $('#downloadBtnProgramPrgElm').attr('disabled', 'disabled');  //Manasi

                    $http(request).then(function success(d) {
                        console.log(d);
                        var gridUploadedDocument = $('#gridUploadedDocumentProgramNewPrgElm tbody');
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
                                    //==================== Jignesh-11-03-2021 ===========================================================
                                    if (!_documentList[x].TrendNumber) {
                                        gridUploadedDocument.append('<tr class="contact-row" id="' + _documentList[x].DocumentID + '"><td style="width: 20px">' +
                                            '<input id=rb' + _documentList[x].DocumentID + ' type="radio"  name="rbCategoriesPrgElm" value="' + serviceBasePath + 'Request/DocumentByDocID/' + _documentList[x].DocumentID + '" />' + //jignesh2111
                                            '</td > <td ' +
                                            'style=" overflow: hidden; text-overflow: ellipsis; white-space: nowrap; "' +
                                            '><a>' + _documentList[x].DocumentName + '</a></td> ' +
                                            '<td style=" overflow: hidden; text-overflow: ellipsis; white-space: nowrap; "' +
                                            '>' + _documentList[x].DocumentTypeName + '</td>' +
                                            //'<td><input type="button" name="btnViewDetail"  id="viewDocumentDetail" style="color:white;background-color: #0c50e8;" value="View"/></td>' +
                                            '<td class="text-center"><i class="fa fa-eye btntbl-icon" name="btnViewDetail" title="View Details" id="viewDocumentDetail"/></td>' +
                                            '<td class="docId" style="display:none;"><span>' + _documentList[x].DocumentID + '</span></td>' +
                                            '<tr > ');
                                    }
                                    //=====================================================================================================

                                }

                                var deleteDocBtn = modal.find('.modal-body #delete-doc-project');
                                deleteDocBtn.attr('disabled', _documentList.length > 0 ? false : true);


                                $('input[name=rbCategoriesPrgElm]').on('click', function (event) {
                                    $('#DeleteUploadProgramPrgElm').removeAttr('disabled');
                                    $('#ViewUploadFileProgramPrgElm').removeAttr('disabled');
                                    $('#EditBtnProgramPrgElm').removeAttr('disabled');
                                    $('#downloadBtnProgramPrgElm').removeAttr('disabled');  //Manasi
                                    localStorage.selectedElementDocument = $(this).closest("tr").find(".docId").text();
                                    //document.getElementById("loading1").style.display = "none";  //Manasi 20-08-2020
                                    document.getElementById("uploadBtnProjectspinRow").style.display = "none";  //Manasi 25-08-2020
                                });

                            }, function error() { }).
                            finally(function () {
                                console.log($rootScope);
                                // document.getElementById("loading1").style.display = "none";    //Manasi 20-08-2020
                                document.getElementById("uploadBtnProjectspinRow").style.display = "none";  //Manasi 25-08-2020

                                // $rootScope.buttonDisabled =false;
                            })

                        if (d.data == "File uploaded successfully") {
                            $("#uploadBtnProject").attr("disabled", false);
                            dhtmlx.alert(d.data);
                            fileUploadProject.value = "";
                            $("#PrgSpecialNotePrgElm").val("");
                            $("#PrgExecutionDatePrgElm").val("");
                            $("#document_name_project").val("");
                            formdata = new FormData();
                            $('#uploadBtnProject').prop('disabled', false);
                            document.getElementById("uploadBtnProjectspinRow").style.display = "none";
                            $('#cancel_doc_update_modal_xPrgElm').trigger('click');
                        }
                        else {
                            $("#uploadBtnProject").attr("disabled", false);
                            dhtmlx.alert(d.data);
                        }
                        // dhtmlx.alert(d.data);
                        //if (d.data == "0") {
                        //    dhtmlx.alert("A file with same name is already uploaded. If you want to upload this file, please rename the file and then try again.");
                        //}
                        //else {
                        //    dhtmlx.alert("File Uploaded Successfully.");
                        //    // document.getElementById("loading1").style.display = "none";   //Manasi 20-08-2020
                        //    document.getElementById("uploadBtnProjectspinRow").style.display = "none";  //Manasi 25-08-2020
                        //    $('#cancel_doc_update_modal_xPrgElm').trigger('click');  //Manasi 20-08-2020
                        //}
                        document.getElementById("uploadBtnProjectspinRow").style.display = "none";
                    }, function error(d) {
                        dhtmlx.alert(d.ExceptionMessage);
                    }).finally(function () {
                        //Clear selected files
                        //fileUploadProject.value = "";
                        //$("#PrgSpecialNotePrgElm").val("");
                        //$("#PrgExecutionDatePrgElm").val("");
                        //$("#document_name_project").val("");
                        //formdata = new FormData();
                        //$('#uploadBtnProject').prop('disabled', false);
                        //document.getElementById("uploadBtnProjectspinRow").style.display = "none";  //Manasi 25-08-2020
                    });
                }
                else {
                    var editOperation = "1";
                    var noFile = false;
                    $("#uploadBtnProject").attr("disabled", true);

                    if (fileUploadProject.files.length == 0)
                        noFile = true;
                    var request = {
                        method: 'POST',
                        url: serviceBasePath + '/uploadFilesnew/Postnew/Project/' + wbsTree.getSelectedProjectID() + '/0/0/0/0/' + docTypeID + '?SpecialNote=' + encodeURIComponent(SpecialNote) + '&DocumentName=' + encodeURIComponent(DocumentName) + '&DocID=' + encodeURIComponent(DocID) + '&editOperation=' + encodeURIComponent(editOperation) + '&noFile=' + encodeURIComponent(noFile),
                        //  url: serviceBasePath + '/uploadFiles/Post/Project/' + wbsTree.getSelectedProjectID() + '/0/0/0/0/' + docTypeID,
                        data: formdata, //fileUploadProject.files, //$scope.
                        ignore: true,
                        headers: {
                            'Content-Type': undefined
                        }
                    };

                    /* Needs $http */
                    // SEND THE FILES.
                    // Disable File Upload Delete and View button
                    $('#DeleteUploadProgramPrgElm').attr('disabled', 'disabled');
                    $('#ViewUploadFileProgramPrgElm').attr('disabled', 'disabled');
                    $('#EditBtnProgramPrgElm').attr('disabled', 'disabled');
                    $('#downloadBtnProgramPrgElm').attr('disabled', 'disabled');  //Manasi

                    $http(request).then(function success(d) {
                        console.log(d);
                        var gridUploadedDocument = $('#gridUploadedDocumentProgramNewPrgElm tbody');
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
                                    //==================== Jignesh-11-03-2021 ===========================================================
                                    if (!_documentList[x].TrendNumber) {
                                        gridUploadedDocument.append('<tr class="contact-row" id="' + _documentList[x].DocumentID + '"><td style="width: 20px">' +
                                            '<input id=rb' + _documentList[x].DocumentID + ' type="radio"  name="rbCategoriesPrgElm" value="' + serviceBasePath + 'Request/DocumentByDocID/' + _documentList[x].DocumentID + '" />' + //jignesh2111
                                            '</td > <td ' +
                                            'style=" overflow: hidden; text-overflow: ellipsis; white-space: nowrap; "' +
                                            '><a>' + _documentList[x].DocumentName + '</a></td> ' +
                                            '<td style=" overflow: hidden; text-overflow: ellipsis; white-space: nowrap; "' +
                                            '>' + _documentList[x].DocumentTypeName + '</td>' +
                                            //'<td><input type="button" name="btnViewDetail"  id="viewDocumentDetail" style="color:white;background-color: #0c50e8;" value="View"/></td>' +
                                            '<td class="text-center"><i class="fa fa-eye btntbl-icon" name="btnViewDetail" title="View Details" id="viewDocumentDetail"/></td>' +
                                            '<td class="docId" style="display:none;"><span>' + _documentList[x].DocumentID + '</span></td>' +
                                            '<tr > ');
                                    }
                                    //=====================================================================================================

                                }

                                var deleteDocBtn = modal.find('.modal-body #delete-doc-project');
                                deleteDocBtn.attr('disabled', _documentList.length > 0 ? false : true);


                                $('input[name=rbCategoriesPrgElm]').on('click', function (event) {
                                    $('#DeleteUploadProgramPrgElm').removeAttr('disabled');
                                    $('#ViewUploadFileProgramPrgElm').removeAttr('disabled');
                                    $('#EditBtnProgramPrgElm').removeAttr('disabled');
                                    $('#downloadBtnProgramPrgElm').removeAttr('disabled');  //Manasi
                                    localStorage.selectedElementDocument = $(this).closest("tr").find(".docId").text();
                                    //document.getElementById("loading1").style.display = "none";  //Manasi 20-08-2020
                                    document.getElementById("uploadBtnProjectspinRow").style.display = "none";  //Manasi 25-08-2020
                                });

                            }, function error() { }).
                            finally(function () {
                                console.log($rootScope);
                                // document.getElementById("loading1").style.display = "none";    //Manasi 20-08-2020
                                document.getElementById("uploadBtnProjectspinRow").style.display = "none";  //Manasi 25-08-2020

                                // $rootScope.buttonDisabled =false;
                            })

                        if (d.data == DocumentName + " has been updated successfully.\n") {
                            $("#uploadBtnProject").attr("disabled", false);
                            dhtmlx.alert(d.data);
                            fileUploadProject.value = "";
                            $("#PrgSpecialNotePrgElm").val("");
                            $("#PrgExecutionDatePrgElm").val("");
                            $("#document_name_project").val("");
                            formdata = new FormData();
                            $('#uploadBtnProject').prop('disabled', false);
                            document.getElementById("uploadBtnProjectspinRow").style.display = "none";
                            $('#cancel_doc_update_modal_xPrgElm').trigger('click');
                        }
                        else {
                            $("#uploadBtnProject").attr("disabled", false);
                            dhtmlx.alert(d.data);
                        }
                        // dhtmlx.alert(d.data);
                        //if (d.data == "0") {
                        //    dhtmlx.alert("A file with same name is already uploaded. If you want to upload this file, please rename the file and then try again.");
                        //}
                        //else {
                        //    dhtmlx.alert("File Uploaded Successfully.");
                        //    // document.getElementById("loading1").style.display = "none";   //Manasi 20-08-2020
                        //    document.getElementById("uploadBtnProjectspinRow").style.display = "none";  //Manasi 25-08-2020
                        //    $('#cancel_doc_update_modal_xPrgElm').trigger('click');  //Manasi 20-08-2020
                        //}
                        document.getElementById("uploadBtnProjectspinRow").style.display = "none";
                    }, function error(d) {
                        dhtmlx.alert(d.ExceptionMessage);
                    }).finally(function () {
                        //Clear selected files
                        //fileUploadProject.value = "";
                        //$("#PrgSpecialNotePrgElm").val("");
                        //$("#PrgExecutionDatePrgElm").val("");
                        //$("#document_name_project").val("");
                        //formdata = new FormData();
                        //$('#uploadBtnProject').prop('disabled', false);
                        //document.getElementById("uploadBtnProjectspinRow").style.display = "none";  //Manasi 25-08-2020
                    });
                }

                //$('#cancel_doc_update_modal_xPrgElm').trigger('click');
            });

            //====================== Tanmay ===================================================
            //=================================================================================

            //====================================== Jignesh-TDM-06-01-2020 =======================================
            $('#uploadDocBtnTrend').unbind('click').on('click', function ($files) {

                console.log('get files', $files);
                var x = wbsTree.getSelectedProjectID();
                var docTypeID = $("#document_type_trend  option").filter(":selected").val();
                var files = fileUploadTrend.files;
                var docTypeName = $("#document_type_trend  option").filter(":selected").text();
                var SpecialNote = $("#DocSpecialNoteTrend").val();
                var DocumentName = $("#document_name_trend").val().trim();
                var ExecutionDate = $("#ExecutionDateTrend").val();
                var DocID = $("#DocTrendID").val();

                if (SpecialNote == "" || SpecialNote.length == 0) {
                    dhtmlx.alert('Enter Notes.');
                    return;
                }
                //================ Jignesh-04-03-2021 ========================
                if (!docTypeID) {
                    dhtmlx.alert('Please choose a Document Type.');
                    return;
                }
                if (!g_edittrenddocument) {
                    if (files.length == 0 || !files.length) {
                        dhtmlx.alert('Please select a file.');
                        return;
                    }
                    if (files[0].size / 1024 / 1024 > 128) {
                        dhtmlx.alert('File size exceed 128MB. Please select a smaller size file.');
                        return;
                    }
                    $('#fileUploadTrend').prop('disabled', false);
                }
                if (DocumentName == "" || DocumentName.length == 0) {
                    dhtmlx.alert('Enter Document Name.');
                    return;
                }
                //===============================================================
                if (DocumentName.includes("\\") || DocumentName.includes("/") || DocumentName.includes(":")
                    || DocumentName.includes("*") || DocumentName.includes("?") || DocumentName.includes('"')
                    || DocumentName.includes("<") || DocumentName.includes(">") || DocumentName.includes("|")) {
                    dhtmlx.alert('A file name cannot contain any of the following characters: \ / : * ? " < > |');
                    return;
                }

                if (DocumentName != "" && DocumentName.length >= 1 && DocumentName.charAt(DocumentName.length - 1) == ".") {
                    dhtmlx.alert('A file name cannot end with a period');
                    return;
                }

                // Jignesh-24-02-2021 Comment below sectoin
                //if (ExecutionDate == "" || ExecutionDate.length == 0) {
                //    dhtmlx.alert('Enter Execution Date.');
                //    return;
                //}



                document.getElementById("uploadBtnTrendspinRow").style.display = "block"; //Manasi 25-08-2020

                var fileName = "";
                formdata = new FormData();
                angular.forEach(fileUploadTrend.files, function (value, key) {
                    console.log(value, key, fileUploadTrend.files);
                    fileName = value.name;
                    formdata.append(key, value);
                    //$('#uploadBtnProject').prop('disabled', false);
                });

                console.log(formdata);
                var selectedNodeTrend = wbsTrendTree.getSelectedTreeNode().metadata;
                var trendNumber = selectedNodeTrend.TrendNumber;
                var TrendNumber = trendNumber;
                $("#uploadDocBtnTrend").attr("disabled", true);
                if (!g_edittrenddocument) {

                    // Jignesh-24-02-2021 Remove ''&ExecutionDate=' + ExecutionDate.replace(/\//g, "") +' from url from below request.
                    var request = {
                        method: 'POST',
                        url: serviceBasePath + '/uploadTrendFile/Post/' + wbsTree.getSelectedProjectID() + '/' + TrendNumber + '/' + docTypeID + '?SpecialNote=' + encodeURIComponent(SpecialNote) + '&DocumentName=' + encodeURIComponent(DocumentName),
                        data: formdata, //fileUploadTrend.files, //$scope.
                        ignore: true,
                        headers: {
                            'Content-Type': undefined
                        }
                    };

                    /* Needs $http */
                    // SEND THE FILES.
                    // Disable File Upload Delete and View button
                    $('#DeleteUploadTrend').attr('disabled', 'disabled');
                    $('#ViewUploadFileTrend').attr('disabled', 'disabled');
                    $('#EditBtnTrend').attr('disabled', 'disabled');
                    $('#downloadBtnTrend').attr('disabled', 'disabled');  //Manasi

                    $http(request).then(function success(d) {
                        console.log(d);
                        var gridUploadedDocument = $('#gridUploadedDocumentTrend tbody');
                        gridUploadedDocument.empty();

                        $http.get(serviceBasePath + "Request/Document/Project/" + wbsTree.getSelectedProjectID())
                            .then(function success(response) {
                                console.log(response);
                                wbsTree.setDocumentList(response.data.result);
                                //============ Jignesh-24-02-2021 add below two lines ================
                                var selectedNodeTrend = wbsTrendTree.getSelectedTreeNode().metadata;
                                var trendNumber = selectedNodeTrend.TrendNumber;
                                //====================================================================
                                for (var x = 0; x < _documentList.length; x++) {
                                    if (_documentList[x].TrendNumber == trendNumber) {
                                        //Edited by Jignesh (29-10-2020)
                                        gridUploadedDocument.append('<tr id="' + _documentList[x].DocumentID + '"><td style="width: 20px">' +
                                            '<input id=rb' + _documentList[x].DocumentID + ' type="radio"  name="rbCategoriesTrend" value="' + serviceBasePath + 'Request/DocumentByDocID/' + _documentList[x].DocumentID + '" />' + //jignesh2111
                                            '</td > <td ' +
                                            'style=" overflow: hidden; text-overflow: ellipsis; white-space: nowrap; "' +
                                            '><a>' + _documentList[x].DocumentName + '</a></td> ' +
                                            '<td style=" overflow: hidden; text-overflow: ellipsis; white-space: nowrap; "' +
                                            '>' + _documentList[x].DocumentTypeName + '</td>' +
                                            //'<td><input type="button" name="btnViewDetail"  id="viewDocumentDetail" style="color:white;background-color: #0c50e8;" value="View"/></td>' +
                                            '<td class="text-center"><i class="fa fa-eye btntbl-icon" name="btnViewDetail" title="View Details" id="viewDocumentDetail"/></td>' +
                                            '<td class="docId" style="display:none;"><span>' + _documentList[x].DocumentID + '</span></td>' +
                                            '<tr > ');
                                    }
                                }
                                //================ Jignesh-05-03-2021 =======================================
                                $('input[name=rbCategoriesTrend]').on('click', function (event) {
                                    if (wbsTree.getLocalStorage().acl[6] == 1 && wbsTree.getLocalStorage().acl[7] == 0) {
                                        $('#DeleteUploadTrend').attr('disabled', 'disabled');
                                        $('#updateDMBtnTrend').attr('disabled', 'disabled');
                                        $('#ViewUploadFileTrend').attr('disabled', 'disabled');
                                        $('#EditBtnTrend').attr('disabled', 'disabled');
                                        $('#downloadBtnTrend').attr('disabled', 'disabled');
                                    } else {
                                        $('#DeleteUploadTrend').removeAttr('disabled');
                                        $('#updateDMBtnTrend').removeAttr('disabled');
                                        $('#ViewUploadFileTrend').removeAttr('disabled');
                                        $('#EditBtnTrend').removeAttr('disabled');
                                        $('#downloadBtnTrend').removeAttr('disabled');
                                    }
                                    localStorage.selectedTrendDocument = $(this).closest("tr").find(".docId").text();
                                });
                                //============================================================================
                            }, function error() { }).
                            finally(function () {
                                console.log($rootScope);
                                document.getElementById("uploadBtnTrendspinRow").style.display = "none";
                            })
                        if (d.data == "File uploaded successfully") {
                            $("#uploadDocBtnTrend").attr("disabled", false);
                            dhtmlx.alert(d.data);
                            formdata = new FormData();
                            fileUploadTrend.value = "";
                            $("#DocSpecialNoteTrend").val("");
                            $("#ExecutionDateTrend").val("");
                            $("#document_name_trend").val("");
                            formdata = new FormData();
                            $('#uploadDocBtnTrend').prop('disabled', false);
                            $('#cancel_doc_update_modal_xTrend').trigger('click');
                        }
                        else {
                            $("#uploadDocBtnTrend").attr("disabled", false);
                            dhtmlx.alert(d.data);
                        }
                        document.getElementById("uploadBtnTrendspinRow").style.display = "none";
                        //if (d.data == "0") {
                        //    dhtmlx.alert("A file with same name is already uploaded. If you want to upload this file, please rename the file and then try again.");
                        //}
                        //else {
                        //    dhtmlx.alert("File Uploaded Successfully.");
                        //    document.getElementById("uploadBtnTrendspinRow").style.display = "none";  //Manasi 25-08-2020
                        //    $('#cancel_doc_update_modal_xTrend').trigger('click');  //Manasi 20-08-2020
                        //}
                    }, function error(d) {
                        $("#uploadDocBtnTrend").attr("disabled", false);
                        dhtmlx.alert(d.ExceptionMessage);
                    }).finally(function () {
                        //Clear selected files
                        //fileUploadTrend.value = "";
                        //$("#DocSpecialNoteTrend").val("");
                        //$("#ExecutionDateTrend").val("");
                        //$("#document_name_trend").val("");
                        //formdata = new FormData();
                        //$('#uploadDocBtnTrend').prop('disabled', false);
                        //document.getElementById("uploadBtnTrendspinRow").style.display = "none";  //Manasi 25-08-2020
                    });
                }
                else {
                    var editOperation = "1";
                    var noFile = false;

                    if (fileUploadProgramElement.files.length == 0)
                        noFile = true;
                    $("#uploadDocBtnTrend").attr("disabled", true);
                    var request = {
                        method: 'POST',
                        url: serviceBasePath + '/uploadTrendFile/Post/' + wbsTree.getSelectedProjectID() + '/' + TrendNumber + '/' + docTypeID + '?SpecialNote=' + encodeURIComponent(SpecialNote) + '&DocumentName=' + encodeURIComponent(DocumentName) + '&DocID=' + encodeURIComponent(DocID) + '&editOperation=' + encodeURIComponent(editOperation) + '&noFile=' + noFile,
                        data: formdata, //fileUploadTrend.files, //$scope.
                        ignore: true,
                        headers: {
                            'Content-Type': undefined
                        }
                    };

                    /* Needs $http */
                    // SEND THE FILES.
                    // Disable File Upload Delete and View button
                    $('#DeleteUploadTrend').attr('disabled', 'disabled');
                    $('#ViewUploadFileTrend').attr('disabled', 'disabled');
                    $('#EditBtnTrend').attr('disabled', 'disabled');
                    $('#downloadBtnTrend').attr('disabled', 'disabled');  //Manasi

                    $http(request).then(function success(d) {
                        console.log(d);
                        var gridUploadedDocument = $('#gridUploadedDocumentTrend tbody');
                        gridUploadedDocument.empty();

                        $http.get(serviceBasePath + "Request/Document/Project/" + wbsTree.getSelectedProjectID())
                            .then(function success(response) {
                                console.log(response);
                                wbsTree.setDocumentList(response.data.result);
                                //============ Jignesh-24-02-2021 add below two lines ================
                                var selectedNodeTrend = wbsTrendTree.getSelectedTreeNode().metadata;
                                var trendNumber = selectedNodeTrend.TrendNumber;
                                //====================================================================
                                for (var x = 0; x < _documentList.length; x++) {
                                    if (_documentList[x].TrendNumber == trendNumber) {
                                        //Edited by Jignesh (29-10-2020)
                                        gridUploadedDocument.append('<tr id="' + _documentList[x].DocumentID + '"><td style="width: 20px">' +
                                            '<input id=rb' + _documentList[x].DocumentID + ' type="radio"  name="rbCategoriesTrend" value="' + serviceBasePath + 'Request/DocumentByDocID/' + _documentList[x].DocumentID + '" />' + //jignesh2111
                                            '</td > <td ' +
                                            'style=" overflow: hidden; text-overflow: ellipsis; white-space: nowrap; "' +
                                            '><a>' + _documentList[x].DocumentName + '</a></td> ' +
                                            '<td style=" overflow: hidden; text-overflow: ellipsis; white-space: nowrap; "' +
                                            '>' + _documentList[x].DocumentTypeName + '</td>' +
                                            //'<td><input type="button" name="btnViewDetail"  id="viewDocumentDetail" style="color:white;background-color: #0c50e8;" value="View"/></td>' +
                                            '<td class="text-center"><i class="fa fa-eye btntbl-icon" name="btnViewDetail" title="View Details" id="viewDocumentDetail"/></td>' +
                                            '<td class="docId" style="display:none;"><span>' + _documentList[x].DocumentID + '</span></td>' +
                                            '<tr > ');
                                    }
                                }
                                //================ Jignesh-05-03-2021 =======================================
                                $('input[name=rbCategoriesTrend]').on('click', function (event) {
                                    if (wbsTree.getLocalStorage().acl[6] == 1 && wbsTree.getLocalStorage().acl[7] == 0) {
                                        $('#DeleteUploadTrend').attr('disabled', 'disabled');
                                        $('#updateDMBtnTrend').attr('disabled', 'disabled');
                                        $('#ViewUploadFileTrend').attr('disabled', 'disabled');
                                        $('#EditBtnTrend').attr('disabled', 'disabled');
                                        $('#downloadBtnTrend').attr('disabled', 'disabled');
                                    } else {
                                        $('#DeleteUploadTrend').removeAttr('disabled');
                                        $('#updateDMBtnTrend').removeAttr('disabled');
                                        $('#ViewUploadFileTrend').removeAttr('disabled');
                                        $('#EditBtnTrend').removeAttr('disabled');
                                        $('#downloadBtnTrend').removeAttr('disabled');
                                    }
                                    localStorage.selectedTrendDocument = $(this).closest("tr").find(".docId").text();
                                });
                                //============================================================================
                            }, function error() { }).
                            finally(function () {
                                console.log($rootScope);
                                document.getElementById("uploadBtnTrendspinRow").style.display = "none";
                            })
                        if (d.data == DocumentName + " has been updated successfully.\n") {
                            $("#uploadDocBtnTrend").attr("disabled", false);
                            dhtmlx.alert(d.data);
                            formdata = new FormData();
                            fileUploadTrend.value = "";
                            $("#DocSpecialNoteTrend").val("");
                            $("#ExecutionDateTrend").val("");
                            $("#document_name_trend").val("");
                            formdata = new FormData();
                            $('#uploadDocBtnTrend').prop('disabled', false);
                            $('#cancel_doc_update_modal_xTrend').trigger('click');
                        }
                        else {
                            $("#uploadDocBtnTrend").attr("disabled", false);
                            dhtmlx.alert(d.data);
                        }
                        document.getElementById("uploadBtnTrendspinRow").style.display = "none";
                        //if (d.data == "0") {
                        //    dhtmlx.alert("A file with same name is already uploaded. If you want to upload this file, please rename the file and then try again.");
                        //}
                        //else {
                        //    dhtmlx.alert("File Uploaded Successfully.");
                        //    document.getElementById("uploadBtnTrendspinRow").style.display = "none";  //Manasi 25-08-2020
                        //    $('#cancel_doc_update_modal_xTrend').trigger('click');  //Manasi 20-08-2020
                        //}
                    }, function error(d) {
                        $("#uploadDocBtnTrend").attr("disabled", false);
                        dhtmlx.alert(d.ExceptionMessage);
                    }).finally(function () {
                        //Clear selected files
                        //fileUploadTrend.value = "";
                        //$("#DocSpecialNoteTrend").val("");
                        //$("#ExecutionDateTrend").val("");
                        //$("#document_name_trend").val("");
                        //formdata = new FormData();
                        //$('#uploadDocBtnTrend').prop('disabled', false);
                        //document.getElementById("uploadBtnTrendspinRow").style.display = "none";  //Manasi 25-08-2020
                    });
                }
            });
            //============================================================================================================
            // Pritesh2 delete
            $('#delete-doc-project').unbind('click').on('click', function ($files) {
                dhtmlx.confirm("Delete selected document?", function (result) { // Jignesh-24-02-2021 {Remove 's' from document}
                    //console.log(result);
                    if (result) {
                        var deleteDocBtn = modal.find('.modal-body #delete-doc-project');
                        deleteDocBtn.attr('disabled', true);
                        $("#gridUploadedDocument tbody").find('input[name="record"]').each(function () {
                            if ($(this).is(":checked")) {
                                //alert($(this).parents("tr").attr('id'));
                                console.log($(this).parents("tr").attr('id'));
                                // alert($(this).parents("tr").attr('id'));

                                _Document.delByDocIDs()
                                    .get({ "docIDs": $(this).parents("tr").attr('id').toString() }, function (response) {
                                        console.log(response);
                                        if (response.result == "Deleted") {
                                            wbsTree.setDeleteDocIDs(null);
                                        } else {
                                            dhtmlx.alert('Error trying to delete Uploaded Documents.');
                                        }
                                    });




                                wbsTree.setDeleteDocIDs($(this).parents("tr").attr('id'));
                                $(this).parents("tr").remove();
                            }
                            else {
                                deleteDocBtn.attr('disabled', false);
                            }
                        });
                    }
                });
            });


            // Pritesh Old Code commented for pop up 2 Project

            $('#uploadBtnProgramElementOld').unbind('click').on('click', function ($files) {
                //alert('Ready to Uplaod. Missing reference $http');
                //return;

                if (wbsTree.getIsProgramElementNew()) {
                    dhtmlx.alert('Uploading files only work in edit mode.');
                    return;
                }

                console.log('get files', $files);
                var x = _selectedNode.ProgramElementID;
                console.log(x)
                var docTypeID = $("#document_type_program_element  option").filter(":selected").val();
                var files = fileUploadProgramElement.files;
                var docTypeName = $("#document_type_program_element  option").filter(":selected").text();

                //if (files.length == 0 || !files.length || !docTypeID) {
                //    dhtmlx.alert('Please chose a doc type and select a file.');
                //    return;
                //}

                //------------------------Manasi----------------------------------
                if (!docTypeID) {
                    dhtmlx.alert('Please choose a doc type.');
                    return;
                }

                if (files.length == 0 || !files.length) {
                    dhtmlx.alert('Please select a file.');
                    return;
                }
                //-----------------------------------------------------------------

                if (files[0].size / 1024 / 1024 > 128) {
                    dhtmlx.alert('File size exceed 128MB. Please select a smaller size file.');
                    return;
                }
                $('#fileUploadProgramElement').prop('disabled', false);
                $('#spinRow').show();

                formdata = new FormData();
                var fileName = "";
                angular.forEach(fileUploadProgramElement.files, function (value, key) {
                    //$scope.selectedFileName = $files[0].name;
                    fileName = value.name;
                    formdata.append(key, value);
                    //$('#uploadBtnProject').prop('disabled', false);
                });

                //Dealing with new program element
                if (wbsTree.getIsProgramElementNew()) {
                    var gridUploadedDocumentProgramElement = modal.find('.modal-body #gridUploadedDocumentProgramElement tbody');
                    gridUploadedDocumentProgramElement.empty();

                    //Insert upload file into draft list and update draft list
                    var draftList = [];
                    for (var x = 0; x < wbsTree.getProgramElementFileDraft().length; x++) {
                        draftList.push(wbsTree.getProgramElementFileDraft()[x]);
                    }
                    draftList.push({ docTypeID: docTypeID, formdata: formdata, fileName: fileName, docTypeName: docTypeName });
                    console.log(draftList);
                    wbsTree.setProgramElementFileDraft(draftList);

                    //Repopulate the document table - should be unclickable
                    for (var x = 0; x < wbsTree.getProgramElementFileDraft().length; x++) {
                        var rowFilename = wbsTree.getProgramElementFileDraft()[x].fileName;
                        var rowDocTypeName = wbsTree.getProgramElementFileDraft()[x].docTypeName;
                        gridUploadedDocumentProgramElement.append('<tr id="' + x + '"><td style="width: 20px"><input type="checkbox" name="record"></td><td>' + rowFilename + '</td><td>' + rowDocTypeName + '</td><td>' + moment().format('MM/DD/YYYY') + '</td><tr>');
                    }

                    return;
                }

                var request = {
                    method: 'POST',
                    url: serviceBasePath + '/uploadFiles/Post/ProgramElement/0/' + _selectedNode.ProgramElementID + '/0/0/0/' + docTypeID,
                    data: formdata, //fileUploadProject.files, //$scope.
                    ignore: true,
                    headers: {
                        'Content-Type': undefined
                    }
                };

                /* Needs $http */
                // SEND THE FILES.
                $http(request).then(function success(d) {
                    console.log(d);
                    var gridUploadedDocument = $('#gridUploadedDocumentProgramNewPrg tbody');
                    gridUploadedDocument.empty();

                    //wbsTree.getDocument().getDocumentByProjID().get({ ProjectID: _selectedProjectID }, function (response) {
                    //    wbsTree.setDocumentList(response.result);
                    //    for (var x = 0; x < _documentList.length; x++) {
                    //        gridUploadedDocument.append('<tr><td style="width: 20px"><input type="checkbox" name="record"></td><td style="display:none">' + _documentList[x].DocumentID + '</td><td><a href="' + serviceBasePath + '/Request/DocumentByDocID/' + _documentList[x].DocumentID + '" download>' + _documentList[x].DocumentName + '</a></td><td>' + _documentList[x].DocumentTypeName + '</td><tr>');
                    //    }
                    $http.get(serviceBasePath + "Request/Document/ProgramElement/" + _selectedNode.ProgramElementID)
                        .then(function success(response) {
                            console.log(response);
                            wbsTree.setDocumentList(response.data.result);
                            for (var x = 0; x < _documentList.length; x++) {
                                gridUploadedDocument.append('<tr class="contact-row" id="' + _documentList[x].DocumentID + '"><td style="width: 20px">' +
                                    // '<input type="radio" group="prgrb" name="record">' +
                                    '<input id=rb' + _documentList[x].DocumentID + ' type="radio" name="rbCategoriesPrg" value="' + serviceBasePath + 'Request/DocumentByDocID/' + _documentList[x].DocumentID + '" />' + //jignesh2111
                                    '</td > <td ' +
                                    'style="max-width: 100px; overflow: hidden; text-overflow: ellipsis; white-space: nowrap; width: 60%;"' +
                                    '><a>' + _documentList[x].DocumentName + '</a></td> ' +
                                    '<td style="max-width: 100px; overflow: hidden; text-overflow: ellipsis; white-space: nowrap; width: 40%;"' +
                                    '>' + _documentList[x].ExecutionDate + '</td>' +
                                    '<td style="max-width: 100px; overflow: hidden; text-overflow: ellipsis; white-space: nowrap; width: 40%;"' +
                                    '>' + _documentList[x].DocumentDescription + '</td>' +
                                    '<td style="max-width: 100px; overflow: hidden; text-overflow: ellipsis; white-space: nowrap; width: 40%;"' +
                                    '>' + _documentList[x].DocumentTypeName + '</td>' +
                                    '<td>' + moment(_documentList[x].CreatedDate).format('MM/DD/YYYY') + '</td>' +
                                    '<td>' + _documentList[x].CreatedBy + '</td>' +
                                    '<tr > ');   //MM/DD/YYYY h:mm a'
                                // gridUploadedDocument.append('<tr id="' + _documentList[x].DocumentID + '"><td style="width: 20px"><input type="checkbox" name="record"></td><td><a href="' + serviceBasePath + 'Request/DocumentByDocID/' + _documentList[x].DocumentID + '">' + _documentList[x].DocumentName + '</a></td><td>' + _documentList[x].DocumentTypeName + '</td><td>' + moment(_documentList[x].CreatedDate).format('MM/DD/YYYY') + '</td><tr>');
                            }

                            var deleteDocBtn = modal.find('.modal-body #delete-doc');
                            deleteDocBtn.attr('disabled', _documentList.length > 0 ? false : true);
                        }, function error() { }).
                        finally(function () {
                            console.log($rootScope);
                            // $rootScope.buttonDisabled =false;
                        })

                    dhtmlx.alert(d.data);
                }, function error(d) {
                    dhtmlx.alert(d.ExceptionMessage);
                }).finally(function () {
                    //Clear selected files
                    fileUploadProgramElement.value = "";
                    formdata = new FormData();
                    $('#uploadBtnProgramElement').prop('disabled', false);
                    $('#spinRow').hide();
                })

                    ;

            });

            $('#fileUploadProgramElement').change(function (ev) {
                console.log(fileUploadProgramElement.files);
                $("#document_name_program_element").val(fileUploadProgramElement.files[0].name);
            });

            // Pritesh new code for save 2 pop up Project
            $('#uploadBtnProgramElement').unbind('click').on('click', function ($files) {
                debugger
                //alert('Ready to Uplaod. Missing reference $http');
                //return;
                if (wbsTree.getIsProgramElementNew()) {
                    dhtmlx.alert('Uploading files only work in edit mode.');
                    $('#loading').hide();   //Manasi 20-08-2020
                    return;
                }

                console.log('get files', $files);
                // var x = wbsTree.getSelectedProgramElementID();
                var x = _selectedNode.ProgramElementID;
                console.log(x)
                var docTypeID = $("#document_type_programPrg  option").filter(":selected").val();
                var files = fileUploadProgramElement.files;
                var docTypeName = $("#document_type_programPrg  option").filter(":selected").text();
                var SpecialNote = $("#PrgSpecialNotePrg").val();
                var DocumentName = $("#document_name_program_element").val();
                var ExecutionDate = $("#PrgExecutionDatePrg").val();
                var DocID = $("#DocprojectID").val();
                if (SpecialNote == "" || SpecialNote.length == 0) {
                    dhtmlx.alert('Enter Notes.');
                    return;
                }
                //==================== Jignesh-03-03-2021 ========================================
                if (!docTypeID) {
                    dhtmlx.alert('Please choose a Document Type.');
                    return;
                }
                if (!g_editprojectdocument) {
                    if (files.length == 0 || !files.length) {
                        dhtmlx.alert('Please select a file.');
                        return;
                    }
                    if (files[0].size / 1024 / 1024 > 128) {
                        dhtmlx.alert('File size exceed 128MB. Please select a smaller size file.');
                        return;
                    }
                }

                // $('#fileUploadProgramElement').prop('disabled', false);
                if (DocumentName == "" || DocumentName.length == 0) {
                    dhtmlx.alert('Enter Document Name.');
                    return;
                }
                //=================================================================================
                if (DocumentName.includes("\\") || DocumentName.includes("/") || DocumentName.includes(":")
                    || DocumentName.includes("*") || DocumentName.includes("?") || DocumentName.includes('"')
                    || DocumentName.includes("<") || DocumentName.includes(">") || DocumentName.includes("|")) {
                    dhtmlx.alert('A file name cannot contain any of the following characters: \ / : * ? " < > |');
                    return;
                }

                if (DocumentName != "" && DocumentName.length >= 1 && DocumentName.charAt(DocumentName.length - 1) == ".") {
                    dhtmlx.alert('A file name cannot end with a period');
                    return;
                }


                // Jignesh-24-02-2021 Comment below sectoin
                //if (ExecutionDate == "" || ExecutionDate.length == 0) {
                //    dhtmlx.alert('Enter Execution Date.');
                //    return;
                //}

                //if (files.length == 0 || !files.length || !docTypeID) {
                //    dhtmlx.alert('Please chose a doc type and select a file.');
                //    return;
                //}

                //------------------------Manasi----------------------------------

                //-----------------------------------------------------------------


                //$('#spinRow').show();
                //$('#uploadBtnProgramelmtDMspinRow').show();
                document.getElementById("uploadBtnProgramElementspinRow").style.display = "block"; //Manasi 20-08-2020
                formdata = new FormData();
                var fileName = "";
                angular.forEach(fileUploadProgramElement.files, function (value, key) {
                    //$scope.selectedFileName = $files[0].name;
                    fileName = value.name;
                    formdata.append(key, value);
                    //$('#uploadBtnProject').prop('disabled', false);
                });

                //Dealing with new program element
                if (wbsTree.getIsProgramElementNew()) {
                    var gridUploadedDocumentProgramElement = $('#gridUploadedDocumentProgramNewPrg tbody');
                    gridUploadedDocumentProgramElement.empty();

                    //Insert upload file into draft list and update draft list
                    var draftList = [];
                    for (var x = 0; x < wbsTree.getProgramElementFileDraft().length; x++) {
                        draftList.push(wbsTree.getProgramElementFileDraft()[x]);
                    }
                    draftList.push({ docTypeID: docTypeID, formdata: formdata, fileName: fileName, docTypeName: docTypeName, SpecialNote: SpecialNote, ExecutionDate: ExecutionDate });

                    // draftList.push({ docTypeID: docTypeID, formdata: formdata, fileName: fileName, docTypeName: docTypeName });
                    console.log(draftList);
                    wbsTree.setProgramElementFileDraft(draftList);

                    //Repopulate the document table - should be unclickable
                    for (var x = 0; x < wbsTree.getProgramElementFileDraft().length; x++) {
                        var rowFilename = wbsTree.getProgramElementFileDraft()[x].fileName;
                        var rowDocTypeName = wbsTree.getProgramElementFileDraft()[x].docTypeName;

                        var rowSpecialNote = wbsTree.getProgramFileDraft()[x].SpecialNote;
                        var rowExecutionDate = wbsTree.getProgramFileDraft()[x].ExecutionDate;
                        gridUploadedDocumentProgram.append('<tr class="contact-row" id="' + x + '"><td style="width: 20px">' +
                            '<input type="radio" group="rbCategoriesPrg" name="record"></td> <td>' + rowFilename + '</td>' +
                            '<td>' + rowSpecialNote + '</td>' +
                            '<td>' + rowExecutionDate + '</td>' +
                            '<td>' + rowDocTypeName + '</td> <td>' + moment().format('MM/DD/YYYY') + '</td> <tr>');
                        // gridUploadedDocumentProgramElement.append('<tr id="' + x + '"><td style="width: 20px"><input type="checkbox" name="record"></td><td>' + rowFilename + '</td><td>' + rowDocTypeName + '</td><td>' + moment().format('MM/DD/YYYY') + '</td><tr>');
                    }

                    return;
                }
                $("#uploadBtnProgramElement").attr("disabled", true);
                if (!g_editprojectdocument) {
                    // Jignesh-24-02-2021 Remove ''&ExecutionDate=' + ExecutionDate.replace(/\//g, "") +' from url from below request.
                    var request = {
                        method: 'POST',
                        url: serviceBasePath + '/uploadFilesnew/Postnew/ProgramElement/0/' + _selectedNode.ProgramElementID + '/0/0/0/' + docTypeID + '?SpecialNote=' + encodeURIComponent(SpecialNote) + '&DocumentName=' + encodeURIComponent(DocumentName),
                        // url: serviceBasePath + '/uploadFiles/Post/ProgramElement/0/' + _selectedNode.ProgramElementID + '/0/0/0/' + docTypeID,
                        data: formdata, //fileUploadProject.files, //$scope.
                        ignore: true,
                        headers: {
                            'Content-Type': undefined
                        }
                    };

                    // Disable File Upload Delete and View button
                    $('#DeleteUploadProgramPrg').attr('disabled', 'disabled');
                    $('#ViewUploadFileProgramPrg').attr('disabled', 'disabled');
                    $('#EditBtnProgramPrg').attr('disabled', 'disabled');
                    $('#downloadBtnProgramPrg').attr('disabled', 'disabled');   //Manasi

                    /* Needs $http */
                    // SEND THE FILES.
                    $http(request).then(function success(d) {
                        $("#uploadBtnProgramElement").attr("disabled", false);
                        console.log(d);
                        var gridUploadedDocument = $('#gridUploadedDocumentProgramNewPrg tbody');
                        gridUploadedDocument.empty();

                        //wbsTree.getDocument().getDocumentByProjID().get({ ProjectID: _selectedProjectID }, function (response) {
                        //    wbsTree.setDocumentList(response.result);
                        //    for (var x = 0; x < _documentList.length; x++) {
                        //        gridUploadedDocument.append('<tr><td style="width: 20px"><input type="checkbox" name="record"></td><td style="display:none">' + _documentList[x].DocumentID + '</td><td><a href="' + serviceBasePath + '/Request/DocumentByDocID/' + _documentList[x].DocumentID + '" download>' + _documentList[x].DocumentName + '</a></td><td>' + _documentList[x].DocumentTypeName + '</td><tr>');
                        //    }
                        $http.get(serviceBasePath + "Request/Document/ProgramElement/" + _selectedNode.ProgramElementID)
                            .then(function success(response) {
                                console.log(response);
                                wbsTree.setDocumentList(response.data.result);
                                for (var x = 0; x < _documentList.length; x++) {

                                    // Edited by Jignesh (29-10-2020)
                                    gridUploadedDocument.append('<tr class="contact-row" id="' + _documentList[x].DocumentID + '"><td style="width: 20px">' +
                                        // '<input type="radio" group="prgrb" name="record">' +
                                        '<input id=rb' + _documentList[x].DocumentID + ' type="radio"  name="rbCategoriesPrg" value="' + serviceBasePath + 'Request/DocumentByDocID/' + _documentList[x].DocumentID + '" />' + //jignesh2111
                                        '</td > <td ' +
                                        'style=" overflow: hidden; text-overflow: ellipsis; white-space: nowrap; "' +
                                        '><a>' + _documentList[x].DocumentName + '</a></td> ' +
                                        //'<td style=" overflow: hidden; text-overflow: ellipsis; white-space: nowrap; "' +
                                        //'>' + _documentList[x].ExecutionDate + '</td>' +
                                        //'<td style=" overflow: hidden; text-overflow: ellipsis; white-space: nowrap;"' +
                                        //'>' + _documentList[x].DocumentDescription + '</td>' +
                                        '<td style=" overflow: hidden; text-overflow: ellipsis; white-space: nowrap; "' +
                                        '>' + _documentList[x].DocumentTypeName + '</td>' +
                                        //'<td>' + moment(_documentList[x].CreatedDate).format('MM/DD/YYYY') + '</td>' +
                                        //'<td>' + _documentList[x].CreatedBy + '</td>' +
                                        //'<td><input type="button" name="btnViewDetail"  id="viewDocumentDetail" style="color:white;background-color: #0c50e8;" value="View"/></td>' +
                                        '<td class="text-center"><i class="fa fa-eye btntbl-icon" name="btnViewDetail" title="View Details" id="viewDocumentDetail"/></td>' +
                                        '<td class="docId" style="display:none;"><span>' + _documentList[x].DocumentID + '</span></td>' +
                                        '<tr > ');

                                }

                                var deleteDocBtn = modal.find('.modal-body #delete-doc');
                                deleteDocBtn.attr('disabled', _documentList.length > 0 ? false : true);


                                $('input[name=rbCategoriesPrg]').on('click', function (event) {
                                    $('#DeleteUploadProgramPrg').removeAttr('disabled');
                                    $('#ViewUploadFileProgramPrg').removeAttr('disabled');
                                    $('#EditBtnProgramPrg').removeAttr('disabled');
                                    $('#downloadBtnProgramPrg').removeAttr('disabled');   //Manasi
                                    localStorage.selectedProgramPrgDocument = $(this).closest("tr").find(".docId").text();

                                    //$('#uploadBtnProgramelmtDMspinRow').hide();    //Manasi 20-08-2020
                                    //document.getElementById("loading").style.display = "none";

                                    //$("#cancel_doc_update_modal_xPrg").trigger('click');

                                });
                            }, function error() { }).
                            finally(function () {
                                console.log($rootScope);
                                //$('#uploadBtnProgramelmtDMspinRow').hide();   //Manasi 20-08-2020
                                //document.getElementById("loading").style.display = "none";
                                // $rootScope.buttonDisabled =false;
                            })
                        if (d.data == "File uploaded successfully") {
                            $("#uploadBtnProgramElement").attr("disabled", false);
                            dhtmlx.alert(d.data);
                            fileUploadProgramElement.value = "";
                            formdata = new FormData();
                            $('#uploadBtnProgramElement').prop('disabled', false);
                            //$('#spinRow').hide();
                            //$('#uploadBtnProgramelmtDMspinRow').hide();     //Manasi 20-08-2020
                            $("#PrgSpecialNotePrg").val("");
                            $("#document_name_program_element").val("");
                            $("#PrgExecutionDatePrg").val("");
                            $("#document_name_program_element").val('');
                            $("#cancel_doc_update_modal_xPrg").trigger('click');
                        }
                        else {
                            $("#uploadBtnProgramElement").attr("disabled", false);
                            dhtmlx.alert(d.data);
                        }
                        //if (d.data == "0") {
                        //    dhtmlx.alert("A file with same name is already uploaded. If you want to upload this file, please rename the file and then try again.");
                        //}
                        //else {
                        //    dhtmlx.alert("File uploaded successfully."); // Jignesh-02-03-2021
                        //    $("#cancel_doc_update_modal_xPrg").trigger('click');
                        //}
                        document.getElementById("uploadBtnProgramElementspinRow").style.display = "none";
                    }, function error(d) {
                        dhtmlx.alert(d.ExceptionMessage);
                    }).finally(function () {
                        //Clear selected files

                    });
                }

                else {
                    var editOperation = "1";
                    var noFile = false;

                    if (fileUploadProgramElement.files.length == 0)
                        noFile = true;

                    $("#uploadBtnProgramElement").attr("disabled", true);
                    var request = {
                        method: 'POST',
                        url: serviceBasePath + '/uploadFilesnew/Postnew/ProgramElement/0/' + _selectedNode.ProgramElementID + '/0/0/0/' + docTypeID + '?SpecialNote=' + encodeURIComponent(SpecialNote) + '&DocumentName=' + encodeURIComponent(DocumentName) + '&DocID=' + encodeURIComponent(DocID) + '&editOperation=' + encodeURIComponent(editOperation) + '&noFile=' + encodeURIComponent(noFile),
                        // url: serviceBasePath + '/uploadFiles/Post/ProgramElement/0/' + _selectedNode.ProgramElementID + '/0/0/0/' + docTypeID,
                        data: formdata, //fileUploadProject.files, //$scope.
                        ignore: true,
                        headers: {
                            'Content-Type': undefined
                        }
                    };

                    // Disable File Upload Delete and View button
                    $('#DeleteUploadProgramPrg').attr('disabled', 'disabled');
                    $('#ViewUploadFileProgramPrg').attr('disabled', 'disabled');
                    $('#EditBtnProgramPrg').attr('disabled', 'disabled');
                    $('#downloadBtnProgramPrg').attr('disabled', 'disabled');   //Manasi

                    $http(request).then(function success(d) {
                        console.log(d);
                        var gridUploadedDocument = $('#gridUploadedDocumentProgramNewPrg tbody');
                        gridUploadedDocument.empty();

                        //wbsTree.getDocument().getDocumentByProjID().get({ ProjectID: _selectedProjectID }, function (response) {
                        //    wbsTree.setDocumentList(response.result);
                        //    for (var x = 0; x < _documentList.length; x++) {
                        //        gridUploadedDocument.append('<tr><td style="width: 20px"><input type="checkbox" name="record"></td><td style="display:none">' + _documentList[x].DocumentID + '</td><td><a href="' + serviceBasePath + '/Request/DocumentByDocID/' + _documentList[x].DocumentID + '" download>' + _documentList[x].DocumentName + '</a></td><td>' + _documentList[x].DocumentTypeName + '</td><tr>');
                        //    }
                        $http.get(serviceBasePath + "Request/Document/ProgramElement/" + _selectedNode.ProgramElementID)
                            .then(function success(response) {
                                console.log(response);
                                wbsTree.setDocumentList(response.data.result);
                                for (var x = 0; x < _documentList.length; x++) {

                                    // Edited by Jignesh (29-10-2020)
                                    gridUploadedDocument.append('<tr class="contact-row" id="' + _documentList[x].DocumentID + '"><td style="width: 20px">' +
                                        // '<input type="radio" group="prgrb" name="record">' +
                                        '<input id=rb' + _documentList[x].DocumentID + ' type="radio"  name="rbCategoriesPrg" value="' + serviceBasePath + 'Request/DocumentByDocID/' + _documentList[x].DocumentID + '" />' + //jignesh2111
                                        '</td > <td ' +
                                        'style=" overflow: hidden; text-overflow: ellipsis; white-space: nowrap; "' +
                                        '><a>' + _documentList[x].DocumentName + '</a></td> ' +
                                        //'<td style=" overflow: hidden; text-overflow: ellipsis; white-space: nowrap; "' +
                                        //'>' + _documentList[x].ExecutionDate + '</td>' +
                                        //'<td style=" overflow: hidden; text-overflow: ellipsis; white-space: nowrap;"' +
                                        //'>' + _documentList[x].DocumentDescription + '</td>' +
                                        '<td style=" overflow: hidden; text-overflow: ellipsis; white-space: nowrap; "' +
                                        '>' + _documentList[x].DocumentTypeName + '</td>' +
                                        //'<td>' + moment(_documentList[x].CreatedDate).format('MM/DD/YYYY') + '</td>' +
                                        //'<td>' + _documentList[x].CreatedBy + '</td>' +
                                        //'<td><input type="button" name="btnViewDetail"  id="viewDocumentDetail" style="color:white;background-color: #0c50e8;" value="View"/></td>' +
                                        '<td class="text-center"><i class="fa fa-eye btntbl-icon" name="btnViewDetail" title="View Details" id="viewDocumentDetail"/></td>' +
                                        '<td class="docId" style="display:none;"><span>' + _documentList[x].DocumentID + '</span></td>' +
                                        '<tr > ');

                                }

                                var deleteDocBtn = modal.find('.modal-body #delete-doc');
                                deleteDocBtn.attr('disabled', _documentList.length > 0 ? false : true);


                                $('input[name=rbCategoriesPrg]').on('click', function (event) {
                                    $('#DeleteUploadProgramPrg').removeAttr('disabled');
                                    $('#ViewUploadFileProgramPrg').removeAttr('disabled');
                                    $('#EditBtnProgramPrg').removeAttr('disabled');
                                    $('#downloadBtnProgramPrg').removeAttr('disabled');   //Manasi
                                    localStorage.selectedProgramPrgDocument = $(this).closest("tr").find(".docId").text();
                                    //$('#uploadBtnProgramelmtDMspinRow').hide();    //Manasi 20-08-2020
                                    //document.getElementById("loading").style.display = "none";

                                    //$("#cancel_doc_update_modal_xPrg").trigger('click');

                                });
                            }, function error() { }).
                            finally(function () {
                                console.log($rootScope);
                                //$('#uploadBtnProgramelmtDMspinRow').hide();   //Manasi 20-08-2020
                                // document.getElementById("loading").style.display = "none";
                                // $rootScope.buttonDisabled =false;
                            })
                        if (d.data == DocumentName + " has been updated successfully.\n") {
                            $("#uploadBtnProgramElement").attr("disabled", false);
                            dhtmlx.alert(d.data);
                            fileUploadProgramElement.value = "";
                            formdata = new FormData();
                            $('#uploadBtnProgramElement').prop('disabled', false);
                            //$('#spinRow').hide();
                            //$('#uploadBtnProgramelmtDMspinRow').hide();     //Manasi 20-08-2020
                            $("#PrgSpecialNotePrg").val("");
                            $("#document_name_program_element").val("");
                            $("#PrgExecutionDatePrg").val("");
                            $("#document_name_program_element").val('');
                            $("#cancel_doc_update_modal_xPrg").trigger('click');
                        }
                        else {
                            $("#uploadBtnProgramElement").attr("disabled", false);
                            dhtmlx.alert(d.data);
                        }
                        //if (d.data == "0") {
                        //    dhtmlx.alert("A file with same name is already uploaded. If you want to upload this file, please rename the file and then try again.");
                        //}
                        //else {
                        //    dhtmlx.alert("File uploaded successfully."); // Jignesh-02-03-2021
                        //    $("#cancel_doc_update_modal_xPrg").trigger('click');
                        //}
                        document.getElementById("uploadBtnProgramElementspinRow").style.display = "none";
                    }, function error(d) {
                        dhtmlx.alert(d.ExceptionMessage);
                    }).finally(function () {
                        //Clear selected files

                    });

                }

                //$("#cancel_doc_update_modal_xPrg").trigger('click');
            });

            $('#delete-doc-program-element').unbind('click').on('click', function ($files) {
                dhtmlx.confirm("Delete selected document?", function (result) {// Jignesh-24-02-2021 {Remove 's' from document}
                    //console.log(result);
                    if (result) {
                        var deleteDocBtn = modal.find('.modal-body #delete-doc-program-element');
                        deleteDocBtn.attr('disabled', true);
                        $("#gridUploadedDocument tbody").find('input[name="record"]').each(function () {
                            if ($(this).is(":checked")) {
                                //alert($(this).parents("tr").attr('id'));
                                console.log($(this).parents("tr").attr('id'));
                                wbsTree.setDeleteDocIDs($(this).parents("tr").attr('id'));
                                $(this).parents("tr").remove();
                            }
                            else {
                                deleteDocBtn.attr('disabled', false);
                            }
                        });
                    }
                });
            });



            $('#fileUploadProgram').change(function (ev) {
                console.log(fileUploadProgram.files);
                $("#document_name_program").val(fileUploadProgram.files[0].name);
            });

            // pritesh new code  Pop up 1 save Contract
            $('#uploadBtnProgram').unbind('click').on('click', function ($files) {
                debugger
                //alert('Ready to Uplaod. Missing reference $http');
                //return;
                //var docId = g_selectedProjectDocument;

                if (wbsTree.getIsProgramNew()) {
                    dhtmlx.alert('Uploading files only work in edit mode.');
                    return;
                }

                console.log('get files', $files);
                var x = wbsTree.getSelectedNode().ProgramID;
                console.log(x)
                var docTypeID = $("#document_type_program option").filter(":selected").val();

                var files = fileUploadProgram.files;
                console.log(files);
                var docTypeName = $("#document_type_program option").filter(":selected").text();
                var SpecialNote = $("#PrgSpecialNote").val();
                var DocumentName = $("#document_name_program").val().trim();
                var ExecutionDate = $("#PrgExecutionDate").val();
                var DocID = $("#DocID").val();
                if (SpecialNote == "" || SpecialNote.length == 0) {
                    dhtmlx.alert('Enter Notes.');
                    return;
                }
                //==================================== Jignesh-03-03-2021 ==========================================
                if (!docTypeID) {
                    dhtmlx.alert('Please choose a Document Type.');
                    return;
                }
                if (!g_editdocument) {
                    if (files.length == 0 || !files.length) {
                        dhtmlx.alert('Please select a file.');
                        return;
                    }
                    if (files[0].size / 1024 / 1024 > 128) {
                        dhtmlx.alert('File size exceed 128MB. Please select a smaller size file.');
                        return;
                    }
                    $('#fileUploadProgram').prop('disabled', false);
                }
                if (DocumentName == "" || DocumentName.length == 0) {
                    dhtmlx.alert('Enter Document Name.');
                    return;
                }
                //=========================================================================================================
                if (DocumentName.includes("\\") || DocumentName.includes("/") || DocumentName.includes(":")
                    || DocumentName.includes("*") || DocumentName.includes("?") || DocumentName.includes('"')
                    || DocumentName.includes("<") || DocumentName.includes(">") || DocumentName.includes("|")) {
                    dhtmlx.alert('A file name cannot contain any of the following characters: \ / : * ? " < > |');
                    return;
                }

                if (DocumentName != "" && DocumentName.length >= 1 && DocumentName.charAt(DocumentName.length - 1) == ".") {
                    dhtmlx.alert('A file name cannot end with a period');
                    return;
                }

                // Jignesh-24-02-2021 Comment below sectoin
                //if (ExecutionDate == "" || ExecutionDate.length == 0) {
                //    dhtmlx.alert('Enter Execution Date.');
                //    return;
                //}

                //if (files.length == 0 || !files.length || !docTypeID) {
                //    dhtmlx.alert('Please chose a doc type and select a file.');
                //    return;
                //}

                //------------------------Manasi----------------------------------



                //-----------------------------------------------------------------


                $('#uploadBtnProgramspinRow').show();
                document.getElementById("uploadBtnContModspinRow").style.display = "block";
                formdata = new FormData();
                var fileName = "";
                $('#uploadBtnProgram').attr('disabled', true); //Aditya
                angular.forEach(fileUploadProgram.files, function (value, key) {
                    //$scope.selectedFileName = $files[0].name;
                    fileName = value.name;
                    formdata.append(key, value);
                    //$('#uploadBtnProject').prop('disabled', false);
                });

                //Dealing with new program 
                if (wbsTree.getIsProgramNew()) {
                    var gridUploadedDocumentProgram = $("#gridUploadedDocumentProgramNew tbody")// modal.find('#gridUploadedDocumentProgram tbody');
                    gridUploadedDocumentProgram.empty();

                    //Insert upload file into draft list and update draft list
                    var draftList = [];
                    for (var x = 0; x < wbsTree.getProgramFileDraft().length; x++) {
                        draftList.push(wbsTree.getProgramFileDraft()[x]);
                    }
                    draftList.push({ docTypeID: docTypeID, formdata: formdata, fileName: fileName, docTypeName: docTypeName, SpecialNote: SpecialNote, ExecutionDate: ExecutionDate });
                    console.log(draftList);
                    wbsTree.setProgramFileDraft(draftList);
                    // alert(draftLists);
                    //Repopulate the document table - should be unclickable
                    for (var x = 0; x < wbsTree.getProgramFileDraft().length; x++) {
                        var rowFilename = wbsTree.getProgramFileDraft()[x].fileName;
                        var rowDocTypeName = wbsTree.getProgramFileDraft()[x].docTypeName;

                        var rowSpecialNote = wbsTree.getProgramFileDraft()[x].SpecialNote;
                        var rowExecutionDate = wbsTree.getProgramFileDraft()[x].ExecutionDate;

                        gridUploadedDocumentProgram.append('<tr class="contact-row" class="contact-row" id="' + x + '"><td style="width: 20px">' +
                            '<input type="radio" group="rbCategories" name="record"></td> <td>' + rowFilename + '</td>' +
                            '<td>' + rowSpecialNote + '</td>' +
                            '<td>' + rowExecutionDate + '</td>' +
                            '<td>' + rowDocTypeName + '</td>' +
                            ' <td>' + moment().format('MM/DD/YYYY') + '</td>' +
                            ' <tr>');
                    }


                    return;
                }
                if (!g_editdocument) {

                    // Jignesh-24-02-2021 Remove ''&ExecutionDate=' + ExecutionDate.replace(/\//g, "") +' from url from below request.

                    var request = {
                        method: 'POST',
                        //  url: serviceBasePath + '/uploadFiles/Post/Program/0/0/' + wbsTree.getSelectedNode().ProgramID + '/0/0/' + docTypeID,    
                        //url: serviceBasePath + '/uploadFilesnew/Postnew/Program/0/0/' + wbsTree.getSelectedNode().ProgramID + '/0/0/' + docTypeID + '?SpecialNote=' + encodeURIComponent(SpecialNote) + '&DocumentName=' + encodeURIComponent(DocumentName) + '&DocID=' + encodeURIComponent(DocID),
                        // url: serviceBasePath + '/uploadFilesnew/Postnew/Program/0/0/' + wbsTree.getSelectedNode().ProgramID + '/0/0/' + docTypeID + '?SpecialNote=' + encodeURIComponent(SpecialNote) + '&DocumentName=' + encodeURIComponent(DocumentName) + '&DocID=' + encodeURIComponent(DocID) + '&editOperation=' + encodeURIComponent(editOperation),
                        url: serviceBasePath + '/uploadFilesnew/Postnew/Program/0/0/' + wbsTree.getSelectedNode().ProgramID + '/0/0/' + docTypeID + '?SpecialNote=' + encodeURIComponent(SpecialNote) + '&DocumentName=' + encodeURIComponent(DocumentName),
                        data: formdata, //fileUploadProject.files, //$scope.
                        ignore: true,
                        headers: {
                            'Content-Type': undefined
                        }
                    };

                    /* Needs $http */
                    // SEND THE FILES.
                    $http(request).then(function success(d) {
                        console.log(d);
                        var gridUploadedDocument = $("#gridUploadedDocumentProgramNew tbody")// modal.find('#gridUploadedDocumentProgram tbody');
                        gridUploadedDocument.empty();
                        document.getElementById("uploadBtnContModspinRow").style.display = "none";
                        //wbsTree.getDocument().getDocumentByProjID().get({ ProjectID: _selectedProjectID }, function (response) {
                        //    wbsTree.setDocumentList(response.result);
                        //    for (var x = 0; x < _documentList.length; x++) {
                        //        gridUploadedDocument.append('<tr><td style="width: 20px"><input type="checkbox" name="record"></td><td style="display:none">' + _documentList[x].DocumentID + '</td><td><a href="' + serviceBasePath + '/Request/DocumentByDocID/' + _documentList[x].DocumentID + '" download>' + _documentList[x].DocumentName + '</a></td><td>' + _documentList[x].DocumentTypeName + '</td><tr>');
                        //    }
                        // Disable File Upload Delete and View button
                        $('#DeleteUploadProgram').attr('disabled', 'disabled');
                        $('#ViewUploadFileProgram').attr('disabled', 'disabled');
                        $('#EditBtnProgram').attr('disabled', 'disabled');
                        $('#downloadBtnProgram').attr('disabled', 'disabled');  //Manasi



                        $http.get(serviceBasePath + "Request/Document/Program/" + wbsTree.getSelectedNode().ProgramID)
                            .then(function success(response) {
                                console.log(response);
                                wbsTree.setDocumentList(response.data.result);
                                _Document.getModificationByProgramId().get({ programId: wbsTree.getSelectedNode().ProgramID }, function (response) {
                                    var _modificationList = response.data;

                                    for (var x = 0; x < _documentList.length; x++) {
                                        var modificatioTitle = "";
                                        for (var i = 0; i < _modificationList.length; i++) {
                                            if (_documentList[x].ModificationNumber == _modificationList[i].ModificationNo) {
                                                modificatioTitle = _modificationList[i].ModificationNo + ' - ' + _modificationList[i].Title
                                            }
                                        }
                                        gridUploadedDocument.append('<tr class="contact-row" id="' + _documentList[x].DocumentID + '"><td style="width: 20px">' +
                                            // '<input type="radio" group="prgrb" name="record">' +
                                            '<input id=rb' + _documentList[x].DocumentID + ' type="radio" name="rbCategories" value="' + serviceBasePath + 'Request/DocumentByDocID/' + _documentList[x].DocumentID + '" />' +
                                            '</td > <td ' +
                                            'style=" overflow: hidden; text-overflow: ellipsis; white-space: nowrap; "' +
                                            '><a>' + _documentList[x].DocumentName + '</a></td> ' +
                                            '<td style=" overflow: hidden; text-overflow: ellipsis; white-space: nowrap; "' +
                                            '>' + _documentList[x].DocumentTypeName + '</td>' +
                                            '<td style=" overflow: hidden; text-overflow: ellipsis; white-space: nowrap; "' +
                                            '>' + modificatioTitle + '</td>' +
                                            //'<td><input type="button" name="btnViewDetail"  id="viewDocumentDetail" style="color:white;background-color: #0c50e8;" value="View"/></td>' +
                                            '<td class="text-center"><i class="fa fa-eye btntbl-icon" name="btnViewDetail" title="View Details" id="viewDocumentDetail"/></td>' +
                                            '<td class="docId" style="display:none;"><span>' + _documentList[x].DocumentID + '</span></td>' +
                                            '<tr > ');   //MM/DD/YYYY h:mm a'

                                    }
                                    var deleteDocBtn = modal.find('.modal-body #delete-doc');
                                    deleteDocBtn.attr('disabled', _documentList.length > 0 ? false : true);

                                    $('input[name=rbCategories]').on('click', function (event) {
                                        $('#DeleteUploadProgram').removeAttr('disabled');
                                        $('#ViewUploadFileProgram').removeAttr('disabled');
                                        $('#EditBtnProgram').removeAttr('disabled');
                                        $('#downloadBtnProgram').removeAttr('disabled');  //Manasi

                                        localStorage.selectedProjectDocument = $(this).closest("tr").find(".docId").text();
                                        //g_selectedProjectDocument = null;
                                        //g_selectedProjectDocument = $(this).closest("tr").find(".docId").text();
                                    });
                                });


                                // <a   href="' + serviceBasePath + 'Request/DocumentByDocID/' + _documentList[x].DocumentID + '" > </a>



                                if (d.data == "File uploaded successfully") {
                                    dhtmlx.alert(d.data);
                                    fileUploadProgram.value = "";
                                    $("#PrgSpecialNote").val("");
                                    $("#document_name_program").val("");
                                    $("#PrgExecutionDate").val("");
                                    formdata = new FormData();
                                    $('#uploadBtnProgram').prop('disabled', false);

                                    $("#cancel_doc_update_modal_x").trigger('click');
                                }
                                else {
                                    $('#uploadBtnProgram').attr('disabled', false); //Aditya
                                    dhtmlx.alert(d.data);
                                }
                                $('#uploadBtnProgramspinRow').hide();

                            }, function error() { }).
                            finally(function () {
                                console.log($rootScope);
                                // $rootScope.buttonDisabled =false;
                            })

                    }, function error(d) {
                        //dhtmlx.alert(d.ExceptionMessage);
                        dhtmlx.alert(d.data);
                    }).finally(function () {
                        //Clear selected files
                        //fileUploadProgram.value = ""; PRitesh commented 13Aug2020
                        //$("#PrgSpecialNote").val(""); PRitesh commented 13Aug2020
                        //$("#document_name_program").val("");  PRitesh commented 13Aug2020
                        //$("#PrgExecutionDate").val(""); PRitesh commented 13Aug2020
                        //formdata = new FormData(); PRitesh commented 13Aug2020
                        //$('#uploadBtnProgram').prop('disabled', false); PRitesh commented 13Aug2020
                        //$('#uploadBtnProgramspinRow').hide(); PRitesh commented 13Aug2020
                    });
                }
                else {

                    var editOperation = "1";
                    var noFile = false;
                    if (fileUploadProgram.files.length == 0)
                        noFile = true;
                    var request = {
                        method: 'POST',
                        //  url: serviceBasePath + '/uploadFiles/Post/Program/0/0/' + wbsTree.getSelectedNode().ProgramID + '/0/0/' + docTypeID,    
                        url: serviceBasePath + '/uploadFilesnew/Postnew/Program/0/0/' + wbsTree.getSelectedNode().ProgramID + '/0/0/' + docTypeID + '?SpecialNote=' + encodeURIComponent(SpecialNote) + '&DocumentName=' + encodeURIComponent(DocumentName) + '&DocID=' + encodeURIComponent(DocID) + '&editOperation=' + encodeURIComponent(editOperation) + '&noFile=' + encodeURIComponent(noFile),
                        data: formdata, //fileUploadProject.files, //$scope.
                        ignore: true,
                        headers: {
                            'Content-Type': undefined
                        }
                    };

                    /* Needs $http */
                    // SEND THE FILES.
                    $http(request).then(function success(d) {
                        console.log(d);
                        var gridUploadedDocument = $("#gridUploadedDocumentProgramNew tbody")// modal.find('#gridUploadedDocumentProgram tbody');
                        gridUploadedDocument.empty();
                        document.getElementById("uploadBtnContModspinRow").style.display = "none";
                        //wbsTree.getDocument().getDocumentByProjID().get({ ProjectID: _selectedProjectID }, function (response) {
                        //    wbsTree.setDocumentList(response.result);
                        //    for (var x = 0; x < _documentList.length; x++) {
                        //        gridUploadedDocument.append('<tr><td style="width: 20px"><input type="checkbox" name="record"></td><td style="display:none">' + _documentList[x].DocumentID + '</td><td><a href="' + serviceBasePath + '/Request/DocumentByDocID/' + _documentList[x].DocumentID + '" download>' + _documentList[x].DocumentName + '</a></td><td>' + _documentList[x].DocumentTypeName + '</td><tr>');
                        //    }
                        // Disable File Upload Delete and View button
                        $('#DeleteUploadProgram').attr('disabled', 'disabled');
                        $('#ViewUploadFileProgram').attr('disabled', 'disabled');
                        $('#EditBtnProgram').attr('disabled', 'disabled');
                        $('#downloadBtnProgram').attr('disabled', 'disabled');  //Manasi



                        $http.get(serviceBasePath + "Request/Document/Program/" + wbsTree.getSelectedNode().ProgramID)
                            .then(function success(response) {
                                console.log(response);
                                wbsTree.setDocumentList(response.data.result);
                                _Document.getModificationByProgramId().get({ programId: wbsTree.getSelectedNode().ProgramID }, function (response) {
                                    var _modificationList = response.data;

                                    for (var x = 0; x < _documentList.length; x++) {
                                        var modificatioTitle = "";
                                        for (var i = 0; i < _modificationList.length; i++) {
                                            if (_documentList[x].ModificationNumber == _modificationList[i].ModificationNo) {
                                                modificatioTitle = _modificationList[i].ModificationNo + ' - ' + _modificationList[i].Title
                                            }
                                        }
                                        gridUploadedDocument.append('<tr class="contact-row" id="' + _documentList[x].DocumentID + '"><td style="width: 20px">' +
                                            // '<input type="radio" group="prgrb" name="record">' +
                                            '<input id=rb' + _documentList[x].DocumentID + ' type="radio" name="rbCategories" value="' + serviceBasePath + 'Request/DocumentByDocID/' + _documentList[x].DocumentID + '" />' +
                                            '</td > <td ' +
                                            'style=" overflow: hidden; text-overflow: ellipsis; white-space: nowrap; "' +
                                            '><a>' + _documentList[x].DocumentName + '</a></td> ' +
                                            '<td style=" overflow: hidden; text-overflow: ellipsis; white-space: nowrap; "' +
                                            '>' + _documentList[x].DocumentTypeName + '</td>' +
                                            '<td style=" overflow: hidden; text-overflow: ellipsis; white-space: nowrap; "' +
                                            '>' + modificatioTitle + '</td>' +
                                            //'<td><input type="button" name="btnViewDetail"  id="viewDocumentDetail" style="color:white;background-color: #0c50e8;" value="View"/></td>' +
                                            '<td class="text-center"><i class="fa fa-eye btntbl-icon" name="btnViewDetail" title="View Details" id="viewDocumentDetail"/></td>' +
                                            '<td class="docId" style="display:none;"><span>' + _documentList[x].DocumentID + '</span></td>' +
                                            '<tr > ');   //MM/DD/YYYY h:mm a'

                                    }
                                    var deleteDocBtn = modal.find('.modal-body #delete-doc');
                                    deleteDocBtn.attr('disabled', _documentList.length > 0 ? false : true);

                                    $('input[name=rbCategories]').on('click', function (event) {
                                        $('#DeleteUploadProgram').removeAttr('disabled');
                                        $('#ViewUploadFileProgram').removeAttr('disabled');
                                        $('#EditBtnProgram').removeAttr('disabled');
                                        $('#downloadBtnProgram').removeAttr('disabled');  //Manasi
                                        localStorage.selectedProjectDocument = $(this).closest("tr").find(".docId").text();
                                    });

                                });


                                // <a   href="' + serviceBasePath + 'Request/DocumentByDocID/' + _documentList[x].DocumentID + '" > </a>



                                if (d.data == DocumentName + " has been updated successfully.\n") {
                                    dhtmlx.alert(d.data);
                                    fileUploadProgram.value = "";
                                    $("#PrgSpecialNote").val("");
                                    $("#document_name_program").val("");
                                    $("#PrgExecutionDate").val("");
                                    formdata = new FormData();
                                    $('#uploadBtnProgram').prop('disabled', false);

                                    $("#cancel_doc_update_modal_x").trigger('click');
                                }
                                else {
                                    dhtmlx.alert(d.data);
                                }
                                $('#uploadBtnProgramspinRow').hide();

                            }, function error() { }).
                            finally(function () {
                                console.log($rootScope);
                                // $rootScope.buttonDisabled =false;
                            })

                    }, function error(d) {
                        //dhtmlx.alert(d.ExceptionMessage);
                        dhtmlx.alert(d.data);
                    }).finally(function () {
                        //Clear selected files
                        //fileUploadProgram.value = ""; PRitesh commented 13Aug2020
                        //$("#PrgSpecialNote").val(""); PRitesh commented 13Aug2020
                        //$("#document_name_program").val("");  PRitesh commented 13Aug2020
                        //$("#PrgExecutionDate").val(""); PRitesh commented 13Aug2020
                        //formdata = new FormData(); PRitesh commented 13Aug2020
                        //$('#uploadBtnProgram').prop('disabled', false); PRitesh commented 13Aug2020
                        //$('#uploadBtnProgramspinRow').hide(); PRitesh commented 13Aug2020
                    });

                }


                // $("#cancel_doc_update_modal_x").trigger('click');  PRitesh commented 13Aug2020
            });


            //======================================= Jignesh-AddNewDocModal-18-02-2021 ==========================================
            $('#btnSaveDocType').unbind('click').on('click', function ($files) {
                $('#btnSaveDocType').prop('disabled',true);
                var docType = $('#txtDocType').val();
                var description = $('#txtDocDescription').val();
                if (docType == "" || docType.length == 0) {
                    $('#btnSaveDocType').prop('disabled', false);
                    dhtmlx.alert('Enter Document Type.');
                    return;
                }
                if (description == "" || description.length == 0) {
                    $('#btnSaveDocType').prop('disabled', false);
                    dhtmlx.alert('Enter Description.');
                    return;
                }
                var listToSave = [];
                var dataObj = {
                    Operation: '1',
                    DocumentTypeName: docType,
                    DocumentTypeDescription: description
                }
                listToSave.push(dataObj);
                $("#btnSaveDocType").attr("disabled", true);
                var url = serviceBasePath + 'response/DocumentType/';
                $http({
                    url: url,
                    method: "POST",
                    data: JSON.stringify(listToSave),
                    headers: { 'Content-Type': 'application/json' }
                }).then(function success(response) {
                    response.data.result.replace(/[\r]/g, '\n');

                    if (response.data.result) {
                        $http({
                            url: serviceBasePath + "Request/DocumentType",
                            method: "GET"
                        }).then(function success(response) {
                            $("#btnSaveDocType").attr("disabled", false);
                            wbsTree.setDocTypeList(response.data.result);
                            var selectedDocTypeDropDown = wbsTree.getSelectedDocTypeDropDown();
                            var docTypeDropDownProgram;
                            if (selectedDocTypeDropDown == 'document_type_programContModification') {
                                modal = $('#DocUpdateModalContModification');
                                docTypeDropDownProgram = modal.find('.modal-body #document_type_programContModification');
                            }
                            else if (selectedDocTypeDropDown == 'document_type_program') {
                                modal = $('#DocUpdateModal');
                                docTypeDropDownProgram = modal.find('.modal-body #document_type_program');
                            }
                            else if (selectedDocTypeDropDown == 'document_type_programPrg') {
                                modal = $('#DocUpdateModalPrg');
                                docTypeDropDownProgram = modal.find('.modal-body #document_type_programPrg');
                            }
                            else if (selectedDocTypeDropDown == 'document_type_programPrgElm') {
                                modal = $('#DocUpdateModalPrgElm');
                                docTypeDropDownProgram = modal.find('.modal-body #document_type_programPrgElm');
                            }
                            //========================== Jignesh-22-02-2021 ===================
                            else if (selectedDocTypeDropDown == 'document_type_trend') {
                                modal = $('#DocUpdateModalTrend');
                                docTypeDropDownProgram = modal.find('.modal-body #document_type_trend');
                            }
                            //==================================================================
                            var docTypeList = wbsTree.getDocTypeList();
                            docTypeDropDownProgram.empty();
                            // Jignesh-25-03-2021
                            //if (wbsTree.getLocalStorage().role === "Admin") {
                            if (wbsTree.getLocalStorage().role.indexOf('Admin') != -1) {
                                docTypeDropDownProgram.append('<option value="Add New"> ----------Add New---------- </option>');
                            }
                            for (var x = 0; x < docTypeList.length; x++) {
                                if (docTypeList[x].DocumentTypeName == docType) {
                                    docTypeDropDownProgram.append('<option value="' + docTypeList[x].DocumentTypeID + '" selected> ' + docTypeList[x].DocumentTypeName + '</option>');
                                }
                                else {
                                    docTypeDropDownProgram.append('<option value="' + docTypeList[x].DocumentTypeID + '"> ' + docTypeList[x].DocumentTypeName + '</option>');
                                }
                            }
                        });
                        $('#cancel_addNewDocumentTypeModal_x').trigger('click');
                        dhtmlx.alert(response.data.result);
                    } else {
                        $("#btnSaveDocType").attr("disabled", false);
                        dhtmlx.alert('No changes to be saved.');
                    }
                    //$state.reload();
                }, function error(response) {
                    $("#btnSaveDocType").attr("disabled", false);
                    dhtmlx.alert("Failed to save. Please contact your Administrator.");
                });
            });
            //=======================================================================================================
            //======================= Aditya 07042022 Prime Add new//
            $('#btnSavePrime').unbind('click').on('click', function ($files) {
                var primeName = $('#txtPrimeName').val();
                $('#btnSavePrime').prop('disabled', true);
                if (primeName == "" || primeName.length == 0) {
                    $('#btnSavePrime').prop('disabled', false);
                    dhtmlx.alert('Enter Prime Name');
                    return;
                }




                var listToSave = [];
                var dataObj = {
                    Operation: '1',
                    Name: primeName,
                }
                listToSave.push(dataObj);

                var url = serviceBasePath + 'Response/prime/';
                $http({
                    url: url,
                    method: "POST",
                    data: JSON.stringify(listToSave),
                    headers: { 'Content-Type': 'application/json' }
                }).then(function success(response) {
                    $('#btnSavePrime').prop('disabled', true);
                    debugger;
                    response.data.result.replace(/[\r]/g, '\n');
                    if (response.data.result) {
                        Prime.get({}, function (response) {
                            debugger;
                        wbsTree.setPrimeList(response.result);
                        var PrimeDropDownProgram;

                        var primeList = wbsTree.getPrimeList();
                        primeList.sort(function (a, b) {                 //vaishnavi
                            return a.Name.localeCompare(b.Name); //vaishnavi
                        });   //vaishnavi
                        PrimeDropDownProgram = modal.find('.modal-body #prime_dd');
                        PrimeDropDownProgram.empty();
                        if (wbsTree.getLocalStorage().role.indexOf('Admin') != -1) {
                            PrimeDropDownProgram.append('<option value="Add New"> ----------Add New---------- </option>');
                        }
                        for (var x = 0; x < primeList.length; x++) {
                            if (primeList[x].Name == primeName) {
                                PrimeDropDownProgram.append('<option value="' + primeList[x].id + '" selected> ' + primeList[x].Name + '</option>');
                            }
                            else {
                                PrimeDropDownProgram.append('<option value="' + primeList[x].id + '"> ' + primeList[x].Name + '</option>');
                            }
                        
                        }
                    });
                $('#cancel_addNewPrimeModal_x').trigger('click');
                        $('#btnSavePrime').prop('disabled', false);
                        dhtmlx.alert(response.data.result);
            } else {
                $('#btnSavePrime').prop('disabled', false);
                        dhtmlx.alert('No changes to be saved.');
                    }
                    //$state.reload();
                }, function error(response) {
                    $('#btnSavePrime').prop('disabled', false);
                    dhtmlx.alert("Failed to save. Please contact your Administrator.");
                });
            });
            //========================================================//
            //======================================= Tanmay-AddNewClientModal-29/12/2021 ==========================================
            $('#btnSaveClient').unbind('click').on('click', function ($files) {
                $('#btnSaveClient').prop('disabled', true);
                var clientName = $('#txtClientName').val();
                var clientPhone = $('#txtClientPhone').val();
                var clientEmail = $('#txtClientEmail').val();
                var clientAddressLine1 = $('#txtClientAddressLine1').val();
                var clientAddressLine2 = $('#txtClientAddressLine2').val();
                var clientCity = $('#txtClientCity').val();
                var clientState = $('#txtClientState').val();
                var clientPONo = $('#txtClientPONo').val();
                var uniqueIdentityNumber = $('#txtUniqueIdentityNumber').val();

                if (clientName == "" || clientName.length == 0) {
                    $('#btnSaveClient').prop('disabled', false);
                    dhtmlx.alert('Enter Client Name');
                    return;
                }
                if (clientPhone == "" || clientPhone.length == 0) {
                    $('#btnSaveClient').prop('disabled', false);
                    dhtmlx.alert('Enter Phone Number.');
                    return;
                }
                if (clientPhone != null) {
                    if (clientPhone.length > 0) {
                        if (clientPhone.length != 12) {
                            $('#btnSaveClient').prop('disabled', false);
                            dhtmlx.alert('Enter valid 10 digit Client Phone.');
                            isFilled = false;
                            return true;
                        }
                    }
                }
                if (clientEmail == "" || clientEmail.length == 0) {
                    $('#btnSaveClient').prop('disabled', false);
                    dhtmlx.alert('Enter Email Address.');
                    return;
                }
                if (clientEmail != null) {
                    if (clientEmail.length > 0) {
                        var testEmail = /^[A-Z0-9._%+-]+@([A-Z0-9-]+\.)+[A-Z]{2,4}$/i;
                        if (!testEmail.test(clientEmail)) {
                            $('#btnSaveClient').prop('disabled', false);
                            dhtmlx.alert('Please enter valid Email Address.');
                            isFilled = false;
                            return;
                        }
                    }
                }
                if (clientAddressLine1 == "" || clientAddressLine1.length == 0) {
                    $('#btnSaveClient').prop('disabled', false);
                    dhtmlx.alert('Enter Address Line 1');
                    return;
                }
                if (clientAddressLine2 == "" || clientAddressLine2.length == 0) {
                    $('#btnSaveClient').prop('disabled', false);
                    dhtmlx.alert('Enter Address Line 2');
                    return;
                }
                if (clientCity == "" || clientCity.length == 0) {
                    $('#btnSaveClient').prop('disabled', false);
                    dhtmlx.alert('Enter City');
                    return;
                }
                if (clientState == "" || clientState.length == 0) {
                    $('#btnSaveClient').prop('disabled', false);
                    dhtmlx.alert('Enter State');
                    return;
                }
                if (clientPONo == "" || clientPONo.length == 0) {
                    $('#btnSaveClient').prop('disabled', false);
                    dhtmlx.alert('Enter Zip Code');
                    return;
                }
                if (clientName == "" || uniqueIdentityNumber == "") {
                    $('#btnSaveClient').prop('disabled', false);
                    dhtmlx.alert({
                        text: "Please fill data to all required fields before save (Row " + value.displayId + ")",
                        width: "400px"
                    });
                    okToExit = false;
                    isFilled = false;
                    return;
                }



                var listToSave = [];
                var dataObj = {
                    Operation: '1',
                    ClientName: clientName,
                    ClientPhone: clientPhone,
                    ClientEmail: clientEmail,
                    ClientAddressLine1: clientAddressLine1,
                    ClientAddressLine2: clientAddressLine2,
                    ClientCity: clientCity,
                    ClientState: clientState,
                    ClientPONo: clientPONo,
                    UniqueIdentityNumber: uniqueIdentityNumber
                }
                listToSave.push(dataObj);

                var url = serviceBasePath + 'response/Client/';
                $http({
                    url: url,
                    method: "POST",
                    data: JSON.stringify(listToSave),
                    headers: { 'Content-Type': 'application/json' }
                }).then(function success(response) {
                    debugger;
                    $('#btnSaveClient').prop('disabled', true);
                    response.data.result.replace(/[\r]/g, '\n');
                    if (response.data.result) {
                        Client.get({}, function (response) {
                            debugger;
                            wbsTree.setClientList(response.result);
                            var clientDropDownProgram;

                            var clientList = wbsTree.getClientList();
                            clientList.sort(function (a, b) {                 //vaishnavi
                                return a.ClientName.localeCompare(b.ClientName); //vaishnavi
                            });   //vaishnavi
                            clientDropDownProgram = modal.find('.modal-body #program_client_poc');
                            clientDropDownProgram.empty();
                            // Jignesh-25-03-2021
                            //if (wbsTree.getLocalStorage().role === "Admin") {
                            if (wbsTree.getLocalStorage().role.indexOf('Admin') != -1) {
                                clientDropDownProgram.append('<option value="Add New"> ----------Add New---------- </option>');
                            }
                            for (var x = 0; x < clientList.length; x++) {
                                if (clientList[x].ClientName == clientName) {
                                    clientDropDownProgram.append('<option value="' + clientList[x].ClientID + '" selected> ' + clientList[x].ClientName + '</option>');
                                }
                                else {
                                    clientDropDownProgram.append('<option value="' + clientList[x].ClientID + '"> ' + clientList[x].ClientName + '</option>');
                                }
                            }
                        });
                        //$http({
                        //    url: serviceBasePath + "Request/Client",
                        //    method: "GET"
                        //}).then(function success(response) {
                        //    wbsTree.setClientList(response.data.result);
                        //    var clientDropDownProgram;

                        //    var clientList = wbsTree.getClientList();
                        //    clientDropDownProgram = modal.find('.modal-body #program_client_poc');
                        //    clientDropDownProgram.empty();
                        //    // Jignesh-25-03-2021
                        //    if (wbsTree.getLocalStorage().role === "Admin") {
                        //        clientDropDownProgram.append('<option value="Add New"> ----------Add New---------- </option>');
                        //    }
                        //    for (var x = 0; x < clientList.length; x++) {
                        //        if (clientList[x].ClientName == clientName) {
                        //            clientDropDownProgram.append('<option value="' + clientList[x].ClientID + '" selected> ' + clientList[x].ClientName + '</option>');
                        //        }
                        //        else {
                        //            clientDropDownProgram.append('<option value="' + clientList[x].ClientID + '"> ' + clientList[x].ClientName + '</option>');
                        //        }
                        //    }
                        //});
                        $('#btnSaveClient').prop('disabled', false);
                        $('#cancel_addNewClientModal_x').trigger('click');
                        dhtmlx.alert(response.data.result);
                    } else {
                        $('#btnSaveClient').prop('disabled', false);
                        dhtmlx.alert('No changes to be saved.');
                    }
                    //$state.reload();
                }, function error(response) {
                    $('#btnSaveClient').prop('disabled', false);
                    dhtmlx.alert("Failed to save. Please contact your Administrator.");
                });
            });
            //=======================================================================================================

            //====================================== Jignesh-24-03-2021 Modification Changes =======================================
            //Aditya chek date 10062022
            function validateDate(dateString, title) {
                let dateformat = /^(0?[1-9]|1[0-2])[\/](0?[1-9]|[1-2][0-9]|3[01])[\/]\d{4}$/;
                // Match the date format through regular expression      
                if (dateString.match(dateformat)) {

                    let operator = dateString.split('/');

                    // Extract the string into month, date and year      
                    let datepart = [];
                    if (operator.length > 1) {
                        datepart = dateString.split('/');
                    }
                    let month = parseInt(datepart[0]);
                    let day = parseInt(datepart[1]);
                    let year = parseInt(datepart[2]);

                    // Create list of days of a month      
                    let ListofDays = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];
                    if (month == 1 || month > 2) {
                        if (day > ListofDays[month - 1]) {
                            ///This check is for Confirming that the date is not out of its range   
                            dhtmlx.alert('Invalid ' + title + ' ');
                            return false;
                        }
                    } else if (month == 2) {
                        let leapYear = false;
                        if ((!(year % 4) && year % 100) || !(year % 400)) {
                            leapYear = true;
                        }
                        if ((leapYear == false) && (day >= 29)) {
                            dhtmlx.alert('Invalid ' + title + ' ');
                            return false;
                        } else
                            if ((leapYear == true) && (day > 29)) {
                                dhtmlx.alert('Invalid ' + title + ' ');
                                return false;
                            }
                    }
                } else {
                    let operator = dateString.split('/');

                    // Extract the string into month, date and year      
                    let datepart = [];
                    if (operator.length > 1) {
                        datepart = dateString.split('/');
                    }
                    let month = parseInt(datepart[0]);
                    let day = parseInt(datepart[1]);

                    if (day > 31) {
                        dhtmlx.alert('Invalid ' + title + ' ');
                        return false;
                    }
                    else {
                        dhtmlx.alert(title + ' Should be in MM/DD/YYYY Format');
                        return false;
                    }
                }
                return true;
            }
            $('#btnSaveModification').unbind().on('click', function (event) {
                var operation = wbsTree.getContractModificationOperation();
                var programId = wbsTree.getSelectedNode().ProgramID;
                var createdBy = _selectedNode.ProgramElementID;
                //var modNumber = $('#modification_number').val();
                var title = $('#modification_title').val();
                var date = $('#modification_date').val();
                var reason = $('#modification_reason').val();
                var description = $('#modification_description').val();
                var modType = $('#ddModificationType').val();
                var pgmcurrentstartdate = $('#program_current_start_date').val();
                var pgmcurrentenddate = $('#program_current_end_date').val();


                var pgmogenddate = $('#program_original_end_date').val(); // Aditya ogDate

                //================ Jignesh-24-03-2021 Modification Changes
                //var durationDate = $('#duration_date').val();
                var scheduleImpact = $('#schedule_impact').val();

                var value = $('#modification_value').val();

                //if (modNumber == "" || modNumber.length == 0) {
                //    dhtmlx.alert('Enter Modification Number.');
                //    return;
                //}

                if (title == "" || title.length == 0) {
                    dhtmlx.alert('Enter Title.');
                    return;
                }
                if (date == "" || date.length == 0) {
                    dhtmlx.alert('Enter Date.');
                    return;
                }

                //Vaishnavi 08-02-2022
                if (date) {
                    //Aditya 10062022
                    var validation = validateDate(date, 'Date');
                    if (validation == false) {
                        return false;
                    }
                    //var testDate = moment(date, 'M/D/YYYY', true).isValid();
                    //if (!testDate){
                    //    dhtmlx.alert('Date Should be in MM/DD/YYYY Format.');
                    //    return;
                    //}
                }
                //Nivedita 03-02-2022
                if (modType != 1) {
                    //if (pgmcurrentstartdate == "" || pgmcurrentstartdate.length == 0) {
                    //    dhtmlx.alert('Enter Contract Start Date.');
                    //    return;
                    //}
                    //Aditya ogDate
                    if (pgmogenddate == "" || pgmogenddate.length == 0) {
                        dhtmlx.alert('Enter Original Contract End Date.');
                        return;

                    }
                }

                //if (_selectedNode.CurrentEndDate == "") {
                //    _selectedNode.CurrentEndDate = pgmcurrentenddate;
                //}

                if (reason == "" || reason.length == 0) {
                    dhtmlx.alert('Enter Reason.');
                    return;
                }
                if (description == "" || description.length == 0) {
                    dhtmlx.alert('Enter Description.');
                    return;
                }
                if (modType != "" || modType.length != 0) {
                    if (modType == 1) {
                        if (value == "" || value.length == 0) {
                            dhtmlx.alert('Enter Value.');
                            return;
                        }
                        scheduleImpact = 0;
                    }
                    else if (modType == 2) {
                        //if (durationDate == "" || durationDate.length == 0) {
                        //    dhtmlx.alert('Enter Duration Date.');
                        //    return;
                        //}
                        if (scheduleImpact == "" || scheduleImpact.length == 0) {
                            dhtmlx.alert('Enter Schedule Impact.');
                            return;
                        }
                    }
                    else if (modType == 3) {
                        if (value == "" || value.length == 0) {
                            dhtmlx.alert('Enter Value.');
                            return;
                        }
                        if (scheduleImpact == "" || scheduleImpact.length == 0) {
                            dhtmlx.alert('Enter Schedule Impact.');
                            return;
                        }
                        //if (durationDate == "" || durationDate.length == 0) {
                        //    dhtmlx.alert('Enter Duration Date.');
                        //    return;
                        //}
                    }
                }
                $("#btnSaveModification").attr('disabled', true); //Aditya
                var contractModification = {
                    //ModificationNo: modNumber,
                    Operation: operation,
                    Title: title,
                    Reason: reason,
                    Description: description,
                    Date: date,
                    ModificationType: modType,
                    Value: value.replace("$", ""),
                    //DurationDate: durationDate,
                    ProgramID: programId,
                    CreatedBy: createdBy,
                    ScheduleImpact: scheduleImpact,
                    originalEndDate: pgmogenddate // Aditya ogDate

                };

                if (modType != 1) {
                    contractModification.ProgramStartDt = pgmcurrentstartdate;
                    contractModification.ProgramEndDt = pgmcurrentenddate;
                    $('#program_current_end_date').attr('disabled', true); // Aditya ogDate
                    $('#program_original_end_date').attr('disabled', true); // Aditya ogDate
                }
                if (operation == 2) {
                    contractModification.Id = $('#primaryKeyId').val();
                    contractModification.ModificationNo = $('#txtModNum').val();
                }

                PerformOperationOnContractModification(contractModification);

                $('#btnDeleteConModification').attr('disabled', 'disabled');
                $('#btnEditConModification').attr('disabled', 'disabled');
                return;
            });

            $('#btnDeleteConModification').unbind().on('click', function () {
                var pgmogenddate = $('#program_original_end_date').val(); // Aditya ogDate
                $("#gridModificationList tbody").find('input[name="rbModHistory"]').each(function () {
                    if ($(this).is(":checked")) {
                        var modID = $(this).parents("tr").attr('id');//
                        if ($(this).closest("tr").find("td:eq(1)").text() == 0) {
                            $('input[name="rbModHistory"]').prop('checked', false);
                            $('#btnDeleteConModification').attr('disabled', 'disabled');
                            $('#btnEditConModification').attr('disabled', 'disabled');
                            dhtmlx.alert('Original Contract Value can not be deleted.');
                            return;
                        }
                        wbsTree.setContractModificationOperation(3);
                        var contractModification = {
                            Operation: 3,
                            Id: $(this).parents("tr").attr('id'),
                            ProgramID: wbsTree.getSelectedNode().ProgramID,
                            originalEndDate: pgmogenddate // Aditya ogDate
                        };
                        // Narayan - show confirmation message before delete - 16/06/2022
                        dhtmlx.confirm("Are you sure you want to delete ?<br/> All linked documents will also get deleted", function (result) {
                            //console.log(result);
                            if (result) {
                                PerformOperationOnContractModification(contractModification);
                            }
                        });
                        $('input[name="rbModHistory"]').prop('checked', false);
                        $('#btnDeleteConModification').attr('disabled', 'disabled');
                        $('#btnEditConModification').attr('disabled', 'disabled');
                    }
                });
            });

            function PerformOperationOnContractModification(contractModification) {
                debugger;
                var request = {
                    method: 'POST',
                    url: serviceBasePath + 'contractModification/saveContractModification',
                    data: contractModification
                };

                $http(request).then(function success(d) {
                    if (d.data.result == "success") {
                        wbsTree.getContractModificationOperation();
                        if (wbsTree.getContractModificationOperation() == 1) {
                            $("#btnSaveModification").attr('disabled', false); //Aditya
                            dhtmlx.alert("Modification Added Successfully!!!.");
                        }
                        else if (wbsTree.getContractModificationOperation() == 2) {
                            $("#btnSaveModification").attr('disabled', false); //Aditya
                            dhtmlx.alert("Modification Updated Successfully!!!.");
                        }
                        else if (wbsTree.getContractModificationOperation() == 3) {
                            $("#btnSaveModification").attr('disabled', false); //Aditya
                            dhtmlx.alert("Modification Deleted Successfully!!!.");
                        }
                        wbsTree.setContractModificationOperation(1);
                        ResetContModificationFields();
                        var updatedContractEndDate = "";
                        _modificationList = d.data.data;
                        var gridModification = $("#gridModificationList tbody")// modal.find('#gridUploadedDocumentProgram tbody');
                        gridModification.empty();
                        _modificationList.reverse();
                        var ModList = _modificationList;
                        for (var x = 0; x < ModList.length; x++) {
                            var durationDate = "";
                            if (_modificationList[x].DurationDate != null) {
                                durationDate = moment(_modificationList[x].DurationDate).format('MM/DD/YYYY')
                            }
                            // Jignesh-17-02-2021
                            var modificationType = _modificationList[x].ModificationType == 1 ? 'Value' :
                                _modificationList[x].ModificationType == 0 ? 'NA' :
                                    _modificationList[x].ModificationType == 2 ? 'Duration' : 'Value & Duration';

                            gridModification.append('<tr class="contact-row" id="' + _modificationList[x].Id + '">' + '<td style="width: 20px">' +
                                '<input id=rb' + _modificationList[x].Id + ' type="radio" name="rbModHistory" />' + //value="' + serviceBasePath + 'Request/DocumentByDocID/' + _documentList[x].DocumentID + '"
                                '</td >' +
                                '<td style=" overflow: hidden; text-overflow: ellipsis; white-space: nowrap; "' +
                                '><a>' + _modificationList[x].ModificationNo + '</a></td> ' +
                                '<td style=" overflow: hidden; text-overflow: ellipsis; white-space: nowrap; min-width:200px;width:100%;" ' +
                                '>' + _modificationList[x].Title + '</td>' +
                                '<td style=" overflow: hidden; text-overflow: ellipsis; white-space: nowrap;"' +
                                '>' + modificationType + '</td>' +  // Jignesh-17-02-2021
                                '<td style=" overflow: hidden; text-overflow: ellipsis; white-space: nowrap; "' +
                                '>' + '$' + _modificationList[x].Value + '</td>' +
                                '<td style=" overflow: hidden; text-overflow: ellipsis; white-space: nowrap; "' +
                                '>' + _modificationList[x].ScheduleImpact + '</td>' +
                                //'>' + durationDate + '</td>' +
                                '<td>' + moment(_modificationList[x].Date).format('MM/DD/YYYY') + '</td>' +
                                '<tr > ');

                            if (_modificationList[x].ModificationNo == 0) {
                                $('#rb' + _modificationList[x].Id).hide();
                            }
                        }

                        $('input[name=rbModHistory]').on('click', function (event) {
                            $('#btnDeleteConModification').removeAttr('disabled');
                            $('#btnEditConModification').removeAttr('disabled');
                        });
                        //============================ Jignesh-24-02-2021 ===================================
                        if (_modificationList.length > 0) {
                            $('#updateDMBtnContModification').removeAttr('disabled');
                        }
                        else {
                            $('#updateDMBtnContModification').attr('disabled', 'disabled');
                        }
                        //===================================================================================
                        var totalValue = 0;
                        var totalDaysOfScheduleImpact = 0;
                        for (var x = 1; x < _modificationList.length; x++) {
                            _modificationList[x].Value = _modificationList[x].Value.replace("$", "").replaceAll(",", "");
                            totalValue = totalValue + parseFloat((_modificationList[x].ModificationNo != "0" ? (_modificationList[x].Value != "" ? _modificationList[x].Value : 0) : 0));
                            totalDaysOfScheduleImpact = totalDaysOfScheduleImpact + parseInt(_modificationList[x].ScheduleImpact);
                            //if (_modificationList[x].DurationDate != "" && _modificationList[x].DurationDate != null) {
                            //    updatedContractEndDate = _modificationList[x].DurationDate;
                            //}
                        }
                        $('#total_modification').val('$' + totalValue);
                        $('#total_modification').focus(); // Jignesh-ModificationPopUpChanges
                        $('#total_modification').blur(); // Jignesh-ModificationPopUpChanges
                        var ogContractValue = $('#program_contract_value').val().replace("$", "").replaceAll(",", "");
                        var totalContractValue = parseFloat(ogContractValue) + totalValue;
                        $('#current_contract_value').val("$" + totalContractValue);
                        $('#current_contract_value').focus(); // Jignesh 01-12-2020
                        $('#current_contract_value').blur();  // Jignesh 01-12-2020
                        var modal = $('#ProgramModal'); //.format(sqlDateFormat);
                        //Nivedita 03-02-2022
                        if (contractModification.ModificationType != 1) {
                            var temp = wbsTree.getSelectedNode();
                            temp.CurrentEndDate = moment(d.data.CurrentEndDate).format('MM/DD/YYYY');
                            wbsTree.updateTreeNodes(temp);
                            $('#program_current_end_date').attr('disabled', true); // Aditya ogDate
                            $('#program_original_end_date').attr('disabled', true); // Aditya ogDate
                            modal.find('.modal-body #program_original_end_date').val(moment(contractModification.originalEndDate).format('MM/DD/YYYY')); // Aditya ogDate
                            modal.find('.modal-body #program_current_end_date').val(moment(d.data.CurrentEndDate).format('MM/DD/YYYY'));
                        }

                        //modal.find('.modal-body #program_current_end_date').val(_selectedNode.CurrentEndDate ? moment(_selectedNode.CurrentEndDate).add(totalDaysOfScheduleImpact, 'days').format('MM/DD/YYYY') : "");
                        //if (updatedContractEndDate != "" && updatedContractEndDate != null && updatedContractEndDate != undefined) {
                        //    modal.find('.modal-body #program_current_end_date').val(moment(updatedContractEndDate).format('MM/DD/YYYY'));  // Jignesh-26-02-2021
                        //}
                        //else {
                        //    modal.find('.modal-body #program_current_end_date').val(moment(selectedNode.CurrentEndDate).format('MM/DD/YYYY'));  // Jignesh-26-02-2021
                        //}

                        // Narayan - Update Modification & Contract Document Grid for document changes - 16/06/2022
                        _Document.getDocumentByProjID().get({ DocumentSet: 'Program', ProjectID: _selectedNode.ProgramID }, function (response) {
                            wbsTree.setDocumentList(response.result);
                            var gridUploadedModDocument = $('#gridUploadedDocumentContModification tbody');
                            var modId = null;
                            var modTitle = null;
                            gridUploadedModDocument.empty();
                            for (var x = 0; x < _documentList.length; x++) {
                                if (_documentList[x].ModificationNumber >= 0 && _documentList[x].ModificationNumber != null) {
                                    for (var i = 0; i < _ModificationList.length; i++) {
                                        if (_documentList[x].ModificationNumber == _ModificationList[i].ModificationNo) {
                                            modId = _ModificationList[i].ModificationNo;
                                            modTitle = _ModificationList[i].Title;
                                        }
                                    }
                                    gridUploadedModDocument.append('<tr class="contact-row" id="' + _documentList[x].DocumentID + '"><td style="width: 20px">' +
                                        '<input id=rb' + _documentList[x].DocumentID + ' type="radio"  name="rbCategoriesMod" value="' + serviceBasePath + 'Request/DocumentByDocID/' + _documentList[x].DocumentID + '" />' +
                                        '</td > <td ' +
                                        'style=" overflow: hidden; text-overflow: ellipsis; white-space: nowrap; "' +
                                        '><a>' + _documentList[x].DocumentName + '</a></td> ' +
                                        '<td style=" overflow: hidden; text-overflow: ellipsis; white-space: nowrap; "' +
                                        '>' + _documentList[x].DocumentTypeName + '</td>' +
                                        '<td style=" overflow: hidden; text-overflow: ellipsis; white-space: nowrap; "' +
                                        '>' + modId + ' - ' + modTitle + '</td>' +
                                        //'<td><input type="button" name="btnViewDetail"  id="viewDocumentDetail" style="color:white;background-color: #0c50e8;" value="View"/></td>' +
                                        '<td class="text-center"><i class="fa fa-eye btntbl-icon" name="btnViewDetail" title="View Details" id="viewDocumentDetail"/></td>' +
                                        '<td class="docId" style="display:none;"><span>' + _documentList[x].DocumentID + '</span></td>' +
                                        '<tr > ');   //MM/DD/YYYY h:mm a'
                                }

                            }

                            var deleteDocBtn = modal.find('.modal-body #delete-doc');
                            deleteDocBtn.attr('disabled', _documentList.length > 0 ? false : true);

                            $('input[name=rbCategoriesMod]').on('click', function (event) {
                                if (wbsTree.getLocalStorage().acl[0] == 1 && wbsTree.getLocalStorage().acl[1] == 0) {
                                    $('#ViewUploadFileContModification').removeAttr('disabled');
                                }
                                else {
                                    $('#DeleteUploadContModification').removeAttr('disabled');
                                    $('#ViewUploadFileContModification').removeAttr('disabled');
                                    $('#downloadBtnContModification').removeAttr('disabled');
                                }

                            });

                            _Document.getModificationByProgramId().get({ programId: _selectedNode.ProgramID }, function (response) { //Amruta 15022022
                                var _modificationList = response.data;
                                var moda2 = $('#ProgramModal');
                                var gridUploadedContDocument = moda2.find("#gridUploadedDocumentProgramNew tbody");
                                gridUploadedContDocument.empty();
                                for (var x = 0; x < _documentList.length; x++) {
                                    var modificatioTitle = "";
                                    if (_modificationList != undefined) {
                                        for (var i = 0; i < _modificationList.length; i++) {
                                            if (_documentList[x].ModificationNumber == _modificationList[i].ModificationNo) {
                                                modificatioTitle = _modificationList[i].ModificationNo + ' - ' + _modificationList[i].Title
                                            }
                                        }
                                    }
                                    gridUploadedContDocument.append('<tr class="contact-row" id="' + _documentList[x].DocumentID + '"><td style="width: 20px">' +
                                        // '<input type="radio" group="prgrb" name="record">' +
                                        '<input id=rb' + _documentList[x].DocumentID + ' type="radio" name="rbCategories" value="' + serviceBasePath + 'Request/DocumentByDocID/' + _documentList[x].DocumentID + '" />' +
                                        '</td > <td ' +
                                        'style=" overflow: hidden; text-overflow: ellipsis; white-space: nowrap; "' +
                                        '><a>' + _documentList[x].DocumentName + '</a></td> ' +
                                        '<td style=" overflow: hidden; text-overflow: ellipsis; white-space: nowrap; "' +
                                        '>' + _documentList[x].DocumentTypeName + '</td>' +
                                        '<td style=" overflow: hidden; text-overflow: ellipsis; white-space: nowrap; "' +
                                        '>' + modificatioTitle + '</td>' +
                                        //'<td><input type="button" name="btnViewDetail"  id="viewDocumentDetail" style="color:white;background-color: #0c50e8;" value="View"/></td>' +
                                        '<td class="text-center"><i class="fa fa-eye btntbl-icon" name="btnViewDetail"  id="viewDocumentDetail" title="View Details"/></td>' +
                                        '<td class="docId" style="display:none;"><span>' + _documentList[x].DocumentID + '</span></td>' +
                                        '<tr > ');   //MM/DD/YYYY h:mm a'

                                }
                                $('input[name=rbCategories]').on('click', function (event) {
                                    if (wbsTree.getLocalStorage().acl[0] == 1 && wbsTree.getLocalStorage().acl[1] == 0) {
                                        $('#ViewUploadFileProgram').removeAttr('disabled');
                                        $('#EditBtnProgram').removeAttr('disabled');
                                    }
                                    else {
                                        $('#DeleteUploadProgram').removeAttr('disabled');
                                        $('#ViewUploadFileProgram').removeAttr('disabled');
                                        $('#EditBtnProgram').removeAttr('disabled');
                                        $('#downloadBtnProgram').removeAttr('disabled');
                                    }
                                    localStorage.selectedProjectDocument = $(this).closest("tr").find(".docId").text();
                                    //g_selectedProjectDocument = null;
                                    //g_selectedProjectDocument = $(this).closest("tr").find(".docId").text();
                                });
                            });

                        });
                    }
                });
            }

            $('#btnClearModification').unbind().on('click', function (event) {
                wbsTree.setContractModificationOperation(1);
                ResetContModificationFields();
                return;
            });
            function ResetContModificationFields() {
                $('#modification_title').val('');
                $('#modification_date').val('');
                $('#modification_reason').val('');
                $('#modification_description').val('');
                $('#ddModificationType').val(1);
                $('#schedule_impact').val('');
                $('#modification_value').val('');
                $('#divModificationValue').show();
                $('#divModDurationDate').hide();
                $('input[name="rbModHistory"]').prop('checked', false);
                $('#btnDeleteConModification').attr('disabled', 'disabled');
                $('#btnEditConModification').attr('disabled', 'disabled');
            }
            //====================================================================================================================

            //=========================  Jignesh-ModificationPopUpChanges =====================================================
            $('#uploadBtnContModification').unbind('click').on('click', function ($files) {

                modal = $('#DocUpdateModalContModification');
                var modificationDropDown = modal.find('.modal-body #ddModificationType');
                var moddID = modificationDropDown.val();

                console.log('get files', $files);
                var x = wbsTree.getSelectedProjectID();
                var docTypeID = $("#document_type_programContModification  option").filter(":selected").val();
                //var moddID = $("#ddModificationType  option").filter(":selected").val();
                //alert(moddID);
                var files = fileUploadContModification.files;
                var docTypeName = $("#document_type_programContModification  option").filter(":selected").text();
                var SpecialNote = $("#ContModificationSpecialNotePrg").val();
                var DocumentName = $("#document_name_ContModification").val().trim();
                var ExecutionDate = $("#ContModificationExecutionDatePrg").val();

                if (SpecialNote == "" || SpecialNote.length == 0) {
                    dhtmlx.alert('Enter Notes.');
                    return;
                }
                //if (ExecutionDate == "" || ExecutionDate.length == 0) {
                //    dhtmlx.alert('Enter Execution Date.');
                //    return;
                //}
                if (!docTypeID) {
                    dhtmlx.alert('Please choose a Document Type.');
                    return;
                }
                if (!moddID) {
                    dhtmlx.alert('Please select a Modification.');
                    return;
                }
                //==================== Jignesh-01-03-2021 =======================================
                if (files.length == 0 || !files.length) {
                    dhtmlx.alert('Please select a file.');
                    return;
                }
                if (files[0].size / 1024 / 1024 > 128) {
                    dhtmlx.alert('File size exceed 128MB. Please select a smaller size file.');
                    return;
                }
                //===============================================================================
                if (DocumentName == "" || DocumentName.length == 0) {
                    dhtmlx.alert('Enter Document Name.');
                    return;
                }

                if (DocumentName.includes("\\") || DocumentName.includes("/") || DocumentName.includes(":")
                    || DocumentName.includes("*") || DocumentName.includes("?") || DocumentName.includes('"')
                    || DocumentName.includes("<") || DocumentName.includes(">") || DocumentName.includes("|")) {
                    dhtmlx.alert('A file name cannot contain any of the following characters: \ / : * ? " < > |');
                    return;
                }

                if (DocumentName != "" && DocumentName.length >= 1 && DocumentName.charAt(DocumentName.length - 1) == ".") {
                    dhtmlx.alert('A file name cannot end with a period');
                    return;
                }



                $('#fileUploadTrend').prop('disabled', false);

                document.getElementById("uploadBtnContMod2spinRow").style.display = "block"; //Jignesh-17-02-2021

                var fileName = "";
                formdata = new FormData();
                angular.forEach(fileUploadContModification.files, function (value, key) {
                    fileName = value.name;
                    formdata.append(key, value);
                    //$('#uploadBtnProject').prop('disabled', false);
                });


                // Jignesh-24-02-2021 Remove ''&ExecutionDate=' + ExecutionDate.replace(/\//g, "") +' from url from below request.
                var request = {
                    method: 'POST',
                    url: serviceBasePath + 'uploadModificationDoc/Post/' + wbsTree.getSelectedNode().ProgramID + '/' + moddID + '/' + docTypeID + '?SpecialNote=' + encodeURIComponent(SpecialNote) + '&DocumentName=' + encodeURIComponent(DocumentName),
                    data: formdata, //fileUploadTrend.files, //$scope.
                    ignore: true,
                    headers: {
                        'Content-Type': undefined
                    }
                };

                /* Needs $http */
                // SEND THE FILES.
                // Disable File Upload Delete and View button
                $('#DeleteUploadTrend').attr('disabled', 'disabled');
                $('#ViewUploadFileTrend').attr('disabled', 'disabled');
                $('#EditBtnTrend').attr('disabled', 'disabled');
                $('#downloadBtnTrend').attr('disabled', 'disabled');  //Manasi

                $http(request).then(function success(d) {
                    console.log(d);
                    //var gridUploadedModDocument = $('#gridUploadedDocumentContModification tbody');
                    //gridUploadedModDocument.empty();

                    $http.get(serviceBasePath + "Request/Document/Program/" + wbsTree.getSelectedNode().ProgramID)
                        .then(function success(response) {
                            console.log(response);
                            wbsTree.setDocumentList(response.data.result);
                            //var gridUploadedModDocument = $('#gridUploadedDocumentContModification tbody');
                            var modal = $('#mdlContractModification');
                            var moda2 = $('#ProgramModal');
                            var gridUploadedModDocument = modal.find('#gridUploadedDocumentContModification tbody');
                            var modId = null;
                            var modTitle = null;
                            gridUploadedModDocument.empty();
                            var gridUploadedContDocument = moda2.find("#gridUploadedDocumentProgramNew tbody");
                            gridUploadedContDocument.empty();
                            _Document.getModificationByProgramId().get({ programId: wbsTree.getSelectedNode().ProgramID }, function (response) {
                                _modificationList = response.data;
                                for (var x = 0; x < _documentList.length; x++) {
                                    if (_documentList[x].ModificationNumber >= 0 && _documentList[x].ModificationNumber != null) {
                                        for (var i = 0; i < _modificationList.length; i++) {
                                            if (_documentList[x].ModificationNumber == _modificationList[i].ModificationNo) {
                                                modId = _modificationList[i].ModificationNo;
                                                modTitle = _modificationList[i].Title;
                                            }
                                        }
                                        gridUploadedModDocument.append('<tr class="contact-row" id="' + _documentList[x].DocumentID + '"><td style="width: 20px">' +
                                            '<input id=rb' + _documentList[x].DocumentID + ' type="radio"  name="rbCategoriesMod" value="' + serviceBasePath + 'Request/DocumentByDocID/' + _documentList[x].DocumentID + '" />' +
                                            '</td > <td ' +
                                            'style=" overflow: hidden; text-overflow: ellipsis; white-space: nowrap; "' +
                                            '><a>' + _documentList[x].DocumentName + '</a></td> ' +
                                            '<td style=" overflow: hidden; text-overflow: ellipsis; white-space: nowrap; "' +
                                            '>' + _documentList[x].DocumentTypeName + '</td>' +
                                            '<td style=" overflow: hidden; text-overflow: ellipsis; white-space: nowrap; "' +
                                            '>' + modId + ' - ' + modTitle + '</td>' +
                                            //'<td><input type="button" name="btnViewDetail"  id="viewDocumentDetail" style="color:white;background-color: #0c50e8;" value="View"/></td>' +
                                            '<td class="text-center"><i class="fa fa-eye btntbl-icon" name="btnViewDetail" title="View Details" id="viewDocumentDetail"/></td>' +
                                            '<td class="docId" style="display:none;"><span>' + _documentList[x].DocumentID + '</span></td>' +
                                            '<tr > ');   //MM/DD/YYYY h:mm a'

                                    }
                                    var modificatioTitle = "";
                                    for (var i = 0; i < _modificationList.length; i++) {
                                        if (_documentList[x].ModificationNumber == _modificationList[i].ModificationNo) {
                                            modificatioTitle = _modificationList[i].ModificationNo + ' - ' + _modificationList[i].Title
                                        }
                                    }
                                    gridUploadedContDocument.append('<tr class="contact-row" id="' + _documentList[x].DocumentID + '"><td style="width: 20px">' +
                                        // '<input type="radio" group="prgrb" name="record">' +
                                        '<input id=rb' + _documentList[x].DocumentID + ' type="radio" name="rbCategories" value="' + serviceBasePath + 'Request/DocumentByDocID/' + _documentList[x].DocumentID + '" />' +
                                        '</td > <td ' +
                                        'style=" overflow: hidden; text-overflow: ellipsis; white-space: nowrap; "' +
                                        '><a>' + _documentList[x].DocumentName + '</a></td> ' +
                                        '<td style=" overflow: hidden; text-overflow: ellipsis; white-space: nowrap; "' +
                                        '>' + _documentList[x].DocumentTypeName + '</td>' +
                                        '<td style=" overflow: hidden; text-overflow: ellipsis; white-space: nowrap; "' +
                                        '>' + modificatioTitle + '</td>' +
                                        //'<td><input type="button" name="btnViewDetail"  id="viewDocumentDetail" style="color:white;background-color: #0c50e8;" value="View"/></td>' +
                                        '<td class="text-center"><i class="fa fa-eye btntbl-icon" name="btnViewDetail" title="View Details" id="viewDocumentDetail"/></td>' +
                                        '<td class="docId" style="display:none;"><span>' + _documentList[x].DocumentID + '</span></td>' +
                                        '<tr > ');   //MM/DD/YYYY h:mm a'

                                }
                                //================ Jignesh-10-02-2021 Please put this section here ==================================================
                                $('input[name=rbCategoriesMod]').on('click', function (event) {
                                    $('#DeleteUploadContModification').removeAttr('disabled');
                                    $('#ViewUploadFileContModification').removeAttr('disabled');
                                    $('#downloadBtnContModification').removeAttr('disabled');
                                    document.getElementById("spinRowMod").style.display = "none";
                                });
                                $('input[name=rbCategories]').on('click', function (event) {
                                    $('#DeleteUploadProgram').removeAttr('disabled');
                                    $('#ViewUploadFileProgram').removeAttr('disabled');
                                    $('#EditBtnProgram').removeAttr('disabled');
                                    $('#downloadBtnProgram').removeAttr('disabled');
                                    localStorage.selectedProjectDocument = $(this).closest("tr").find(".docId").text();
                                });
                                //======================================================================================
                            });


                            //$scope.bindContractDocument(_documentList);

                            var deleteDocBtn = modal.find('.modal-body #delete-doc-project');
                            deleteDocBtn.attr('disabled', _documentList.length > 0 ? false : true);




                            $('input[name=rbCategoriesMod]').on('click', function (event) {
                                $('#DeleteUploadContModification').removeAttr('disabled');
                                $('#ViewUploadFileContModification').removeAttr('disabled');
                                $('#downloadBtnContModification').removeAttr('disabled');  //Manasi
                                document.getElementById("spinRowMod").style.display = "none";  //Manasi 25-08-2020
                            });

                        }, function error() { }).
                        finally(function () {
                            console.log($rootScope);
                            document.getElementById("uploadBtnContMod2spinRow").style.display = "none";
                        })
                    if (d.data == "File uploaded successfully") {
                        dhtmlx.alert(d.data);
                        fileUploadTrend.value = "";
                        $("#ContModificationSpecialNotePrg").val("");
                        $("#ContModificationExecutionDatePrg").val("");
                        $("#document_name_ContModification").val("");
                        formdata = new FormData();
                        $('#uploadDocBtnTrend').prop('disabled', false);
                        document.getElementById("uploadBtnContModspinRow").style.display = "none";
                        $('#cancel_doc_update_modal_xContModification').trigger('click');
                    }
                    else {
                        dhtmlx.alert(d.data);
                    }
                    //if (d.data == "0") {
                    //    dhtmlx.alert("A file with same name is already uploaded. If you want to upload this file, please rename the file and then try again.");
                    //}
                    //else {
                    //    dhtmlx.alert("File uploaded successfully.");
                    //    document.getElementById("uploadBtnTrendspinRow").style.display = "none";  //Manasi 25-08-2020
                    //    $('#cancel_doc_update_modal_xContModification').trigger('click');  //Manasi 20-08-2020
                    //}
                    document.getElementById("uploadBtnContModspinRow").style.display = "none";
                }, function error(d) {
                    dhtmlx.alert(d.ExceptionMessage);
                }).finally(function () {
                    //Clear selected files
                    //fileUploadTrend.value = "";
                    //$("#ContModificationSpecialNotePrg").val("");
                    //$("#ContModificationExecutionDatePrg").val("");
                    //$("#document_name_ContModification").val("");
                    //formdata = new FormData();
                    //$('#uploadDocBtnTrend').prop('disabled', false);
                    //document.getElementById("uploadBtnContModspinRow").style.display = "none";  // Jignesh-17-02-2021
                });
            });

            $('#fileUploadContModification').change(function (ev) {
                console.log(fileUploadProgram.files);
                $("#document_name_ContModification").val(fileUploadContModification.files[0].name);
            });

            //=================  Jignesh-ModificationPopUpChanges Ends here ==================================



            //============================================================================================================

            //====================================== Created By Jignesh 28-10-2020 =======================================
            //$('#program_contract_value').unbind('change').on('change', function () {
            //    isFieldValueChanged = true;
            //    var totalModification = $('#total_modification').val().replace("$", "");
            //    var ogContractValue = $('#program_contract_value').val().replace("$", "").replaceAll(",", "");
            //    var totalContractValue = parseFloat(ogContractValue) + parseFloat(totalModification);
            //    $('#current_contract_value').val(totalContractValue);
            //    $('#current_contract_value').focus(); // Jignesh 01-12-2020
            //    $('#current_contract_value').blur(); // Jignesh 01-12-2020
            //});
            //============================================================================================================

            // Previous working Function pritesh  Click working functionality name changed to old
            $('#uploadBtnProgramOLd').unbind('click').on('click', function ($files) {
                //alert('Ready to Uplaod. Missing reference $http');
                //return;

                if (wbsTree.getIsProgramNew()) {
                    dhtmlx.alert('Uploading files only work in edit mode.');
                    return;
                }

                console.log('get files', $files);
                var x = wbsTree.getSelectedNode().ProgramID;
                console.log(x)
                var docTypeID = $("#document_type_program option").filter(":selected").val();
                var files = fileUploadProgram.files;
                var docTypeName = $("#document_type_program option").filter(":selected").text();

                //if (files.length == 0 || !files.length || !docTypeID) {
                //    dhtmlx.alert('Please chose a doc type and select a file.');
                //    return;
                //}

                //------------------------Manasi----------------------------------
                if (!docTypeID) {
                    dhtmlx.alert('Please choose a doc type.');
                    return;
                }

                if (files.length == 0 || !files.length) {
                    dhtmlx.alert('Please select a file.');
                    return;
                }
                //-----------------------------------------------------------------

                if (files[0].size / 1024 / 1024 > 128) {
                    dhtmlx.alert('File size exceed 128MB. Please select a smaller size file.');
                    return;
                }
                $('#fileUploadProgram').prop('disabled', false);
                $('#spinRow').show();

                formdata = new FormData();
                var fileName = "";
                angular.forEach(fileUploadProgram.files, function (value, key) {
                    //$scope.selectedFileName = $files[0].name;
                    fileName = value.name;
                    formdata.append(key, value);
                    //$('#uploadBtnProject').prop('disabled', false);
                });

                //Dealing with new program 
                if (wbsTree.getIsProgramNew()) {
                    var gridUploadedDocumentProgram = modal.find('.modal-body #gridUploadedDocumentProgram tbody');
                    gridUploadedDocumentProgram.empty();

                    //Insert upload file into draft list and update draft list
                    var draftList = [];
                    for (var x = 0; x < wbsTree.getProgramFileDraft().length; x++) {
                        draftList.push(wbsTree.getProgramFileDraft()[x]);
                    }
                    draftList.push({ docTypeID: docTypeID, formdata: formdata, fileName: fileName, docTypeName: docTypeName });
                    console.log(draftList);
                    wbsTree.setProgramFileDraft(draftList);

                    //Repopulate the document table - should be unclickable
                    for (var x = 0; x < wbsTree.getProgramFileDraft().length; x++) {
                        var rowFilename = wbsTree.getProgramFileDraft()[x].fileName;
                        var rowDocTypeName = wbsTree.getProgramFileDraft()[x].docTypeName;
                        //gridUploadedDocumentProgram.append('<tr id="' + x + '"><td style="width: 20px"><input type="checkbox" name="record"></td><td>' + rowFilename + '</td><td>' + rowDocTypeName + '</td><td>' + moment().format('MM/DD/YYYY') + '</td><tr>');
                        gridUploadedDocumentProgram.append('<tr  id="' + x +
                            '"><td style="width: 20px"><input type="checkbox" name="record"></td><td>' + rowFilename +
                            '</td><td>' + rowDocTypeName + '</td><td>' + moment().format('MM/DD/YYYY') + '</td><tr>');
                    }

                    return;
                }

                var request = {
                    method: 'POST',
                    url: serviceBasePath + '/uploadFiles/Post/Program/0/0/' + wbsTree.getSelectedNode().ProgramID + '/0/0/' + docTypeID,
                    data: formdata, //fileUploadProject.files, //$scope.
                    ignore: true,
                    headers: {
                        'Content-Type': undefined
                    }
                };

                /* Needs $http */
                // SEND THE FILES.
                $http(request).then(function success(d) {
                    console.log(d);
                    var gridUploadedDocument = modal.find('.modal-body #gridUploadedDocumentProgram tbody');
                    gridUploadedDocument.empty();

                    //wbsTree.getDocument().getDocumentByProjID().get({ ProjectID: _selectedProjectID }, function (response) {
                    //    wbsTree.setDocumentList(response.result);
                    //    for (var x = 0; x < _documentList.length; x++) {
                    //        gridUploadedDocument.append('<tr><td style="width: 20px"><input type="checkbox" name="record"></td><td style="display:none">' + _documentList[x].DocumentID + '</td><td><a href="' + serviceBasePath + '/Request/DocumentByDocID/' + _documentList[x].DocumentID + '" download>' + _documentList[x].DocumentName + '</a></td><td>' + _documentList[x].DocumentTypeName + '</td><tr>');
                    //    }
                    $http.get(serviceBasePath + "Request/Document/Program/" + wbsTree.getSelectedNode().ProgramID)
                        .then(function success(response) {
                            console.log(response);
                            wbsTree.setDocumentList(response.data.result);
                            for (var x = 0; x < _documentList.length; x++) {
                                gridUploadedDocument.append('<tr id="' + _documentList[x].DocumentID + '"><td style="width: 20px"><input type="checkbox" name="record"></td><td><a href="' + serviceBasePath + 'Request/DocumentByDocID/' + _documentList[x].DocumentID + '">' + _documentList[x].DocumentName + '</a></td><td>' + _documentList[x].DocumentTypeName + '</td><td>' + moment(_documentList[x].CreatedDate).format('MM/DD/YYYY') + '</td><tr>');
                            }

                            var deleteDocBtn = modal.find('.modal-body #delete-doc');
                            deleteDocBtn.attr('disabled', _documentList.length > 0 ? false : true);
                        }, function error() { }).
                        finally(function () {
                            console.log($rootScope);
                            // $rootScope.buttonDisabled =false;
                        })

                    dhtmlx.alert(d.data);
                }, function error(d) {
                    dhtmlx.alert(d.ExceptionMessage);
                }).finally(function () {
                    //Clear selected files
                    fileUploadProgram.value = "";
                    formdata = new FormData();
                    $('#uploadBtnProgram').prop('disabled', false);
                    $('#spinRow').hide();
                })

                    ;


            });

            //Pritesh delete
            $('#delete-doc-program').unbind('click').on('click', function ($files) {
                dhtmlx.confirm("Delete selected document?", function (result) { // Jignesh-24-02-2021 {Remove 's' from document}
                    //console.log(result);
                    if (result) {
                        var deleteDocBtn = modal.find('.modal-body #delete-doc-program');
                        deleteDocBtn.attr('disabled', true);
                        $("#gridUploadedDocument tbody").find('input[name="record"]').each(function () {
                            if ($(this).is(":checked")) {
                                //alert($(this).parents("tr").attr('id'));
                                console.log($(this).parents("tr").attr('id'));
                                wbsTree.setDeleteDocIDs($(this).parents("tr").attr('id'));
                                $(this).parents("tr").remove();
                            }
                            else {
                                deleteDocBtn.attr('disabled', false);
                            }
                        });
                    }
                });
            });

            //----------------------------Vaishnavi 30-03-2022 Start Here-------------------------------------------------//
            $('#closed_button').unbind('click').on('click', function () {
                var selectedNode = wbsTree.getSelectedNode();
                var type = $("#contextMenu").attr('contextType');
                //if(selectedNode.level === "Root"){ Ivan here
                //    modal_mode = "Update";
                //}
                // business logic...
                $('#ClosedModal').modal('hide');
                if (typeof modal_mode == 'undefined') {
                }

                if (modal_mode == 'Create') {

                }

                else {
                    var parentNode = selectedNode.parent;

                    //

                    $('#ClosedModal').appendTo("body");
                    if (wbsTree.getScope().trend) {
                        console.log(wbsTree.getScope().trend);
                        console.log(selectedNode);
                        if (wbsTree.getScope().trend.metadata.level = "FutureTrend") {
                            var obj = {
                                "Operation": 6,
                                "OrganizationID": wbsTree.getScope().trend.metadata.OrganizationID,
                                "ProjectID": wbsTree.getScope().trend.metadata.ProjectID,
                                "ProjectName": wbsTree.getScope().trend.metadata.ProjectName,
                                "TrendNumber": wbsTree.getScope().trend.metadata.TrendNumber,
                                //Added by Nivedita on 23022022 for soft delete
                                "DeletedBy": wbsTree.getLocalStorage().userName

                            };
                            _Trend.persist().save(obj, function (response) {
                                //$('#FutureTrendModal').modal('hide');
                                //$('#DeleteModal').modal('hide');

                                wbsTree.getProgramFund().lookup().get({ "ProgramID": selectedNode.parent.parent.ProgramID }, function (response) {
                                    selectedNode.parent.parent.programFunds = response.result;
                                    if ($('#FutureTrendModal').hasClass('in'))
                                        $('#FutureTrendModal').css({ "opacity": "1" }).modal('toggle');
                                    wbsTree.getWBSTrendTree().setSelectedTreeNode(null);
                                    wbsTree.getWBSTrendTree().trendGraph(true);  //Manasi
                                    wbsTree.getScope().trend = null;
                                    // window.location.reload();
                                    var pgmId = $("#selectProgram").val();
                                    var orgId = $("#selectOrg").val();
                                    $scope.loadWBSData(orgId, pgmId, null, null, null, null, null);

                                });


                            });

                            //updateTreeNodes(parentNode);
                        }
                    }
                    else {
                        if (selectedNode.level === "Root") {
                            console.log(selectedNode);
                            var s = $("#update_organization");
                            console.debug("UPDATE", s);
                            var rootScope = wbsTree.getRootScope();
                            var Organization = wbsTree.getOrganizationService();
                            var scope = wbsTree.getScope();
                            Organization.persist().save({
                                "Operation": 3,
                                "OrganizationID": selectedNode.organizationID,
                                "OrganizationName": selectedNode.name
                            }).$promise.then(function (response) {
                                if (response.result === 'Success') {
                                    //  $scope.organizationList.splice(index,1);

                                    console.log(scope);
                                    var orgList = wbsTree.getOrganizationList();
                                    var index = 0;
                                    for (var i = 0; i < orgList.length; i++) {
                                        if (orgList[i].OrganizationName == selectedNode.name) {
                                            index = i;

                                        }
                                    }
                                    console.debug("orgList", orgList);
                                    orgList.splice(index, 1);
                                    // wbsTree.setOrganizationList(orgList);
                                    //  $("#selectOrg").val(orgList[0].OrganizationID);
                                    console.debug("orgList", orgList);
                                    scope.filterOrg = orgList[0].OrganizationID;
                                    $("#selectOrg").val(orgList[0].OrganizationID);
                                    scope.filterChangeOrg();
                                    //$scope.selectedOrg = null;
                                    //$scope.init();
                                    //console.log("-------DELETED ORGANIZATION--------");
                                    //wbsTree.updateTreeNodes(selectedNode);
                                    //if (!displayMap)
                                    wbsTree.loadFullGridView();
                                    //wbsTree.loadContextMenu();
                                    rootScope.modalInstance.close();
                                    //wbsTree.getWBSTrendTree().trendGraph();
                                }
                                else {
                                    dhtmlx.alert("Close failed");
                                }
                            });


                        } else
                            if (selectedNode.level === "Program") {
                                wbsTree.getProgram().persist().save({
                                    "Operation": 5,
                                    "ProgramID": selectedNode.ProgramID,
                                    "ProgramName": selectedNode.name,
                                    "ProgramManager": selectedNode.ProgramManager,
                                    "ProgramSponsor": selectedNode.ProgramSponsor,
                                    "programFunds": selectedNode.programFunds,
                                    "DeletedBy": wbsTree.getLocalStorage().userName

                                }, function (response) {
                                    console.log("-------DELETED PROGRAM--------");
                                    if ($('#ProgramModal').hasClass('in'))
                                        $('#ProgramModal').css({ "opacity": "1" }).modal('toggle');
                                    // wbsTree.updateTreeNodes(selectedNode.parent);
                                    ////if (!displayMap)
                                    //wbsTree.loadFullGridView();
                                    //wbsTree.getWBSTrendTree().trendGraph();

                                    var pgmId = $("#selectProgram").val();
                                    var orgId = $("#selectOrg").val();
                                    $scope.loadWBSData(orgId, pgmId, null, null, null, null, null);
                                });
                            } else if (selectedNode.level === "ProgramElement") {
                                wbsTree.getProgramElement().persist().save({
                                    "Operation": 5,
                                    "ProgramID": selectedNode.ProgramID,
                                    "ProgramElementID": selectedNode.ProgramElementID,
                                    "ProgramElementName": selectedNode.name,
                                    "ProgramElementManager": selectedNode.ProgramElementManager,
                                    "ProgramElementSponsor": selectedNode.ProgramElementSponsor,
                                    "DeletedBy": wbsTree.getLocalStorage().userName

                                }, function (response) {
                                    console.log("-------DELETED PROGRAM ELEMENT--------");


                                    if ($('#ProgramElementModal').hasClass('in'))
                                        $("#ProgramElementModal").css({ "opacity": "1" }).modal('toggle');
                                    //if (!displayMap)
                                    //wbsTree.loadFullGridView();
                                    //wbsTree.getWBSTrendTree().trendGraph();
                                    //// wbsTree.loadFullGridView();
                                    //window.location.reload();
                                    var pgmId = $("#selectProgram").val();
                                    var orgId = $("#selectOrg").val();
                                    $scope.loadWBSData(orgId, pgmId, null, null, null, null, null);

                                })
                            } else if (selectedNode.level === "Project" && !wbsTree.getScope().trend) {
                                wbsTree.getProject().persist().save({
                                    "Operation": 5,
                                    "ProjectID": selectedNode.ProjectID,
                                    "ProjectName": selectedNode.name,
                                    "ProjectManager": selectedNode.ProjectManager,
                                    "ProjectSponsor": selectedNode.ProjectSponsor,
                                    "LatLong": wbsTree.getProjectMap().getCoordinates(),
                                    "DeletedBy": wbsTree.getLocalStorage().userName
                                }, function (response) {
                                    console.log("-------DELETED PROJECT--------");
                                    console.log(selectedNode);

                                    var pgmId = $("#selectProgram").val();
                                    var orgId = $("#selectOrg").val();
                                    $scope.loadWBSData(orgId, pgmId, null, null, null, null, null);
                                    var firstGNode = $('#trendSvg').children()[0];
                                    //$(firstGNode).children().remove();
                                    // window.location.reload();
                                });
                            }
                        //Find  index of selected node
                        if (selectedNode.level == "Root") return;
                        var i = parentNode.children.indexOf(selectedNode);
                        //Remove selected child index from parent
                        parentNode.children.splice(i, 1);
                        var tooltip = d3.select("#toolTip").style("opacity", 0);

                    }
                }
                _selectedProjectID = null;
                //$('#project_name').hide();
                // $('svg.trendTree g').hide();

            });
            //----------------------------Vaishnavi 30-03-2022 Ends Here-------------------------------------------------//






            /* Do this in backend */
            //$('#delete-doc-program-contrat-modal').unbind('click').on('click', function ($files) {
            //	dhtmlx.confirm("Delete selected documents?", function (result) {
            //		//console.log(result);
            //		if (result) {
            //			var deleteDocBtn = modal.find('.modal-body #delete-doc-program-element');
            //			deleteDocBtn.attr('disabled', true);
            //			$("#gridUploadedDocument tbody").find('input[name="record"]').each(function () {
            //				if ($(this).is(":checked")) {
            //					//alert($(this).parents("tr").attr('id'));
            //					console.log($(this).parents("tr").attr('id'));
            //					wbsTree.setDeleteDocIDs($(this).parents("tr").attr('id'));
            //					$(this).parents("tr").remove();
            //				}
            //				else {
            //					deleteDocBtn.attr('disabled', false);
            //				}
            //			});
            //		}
            //	});
            //});

            $("#fileUploadProgramElementChangeOrderModal").unbind('click').on('click', function (e) {
                // alert("Pritesh");
                $('#program_element_change_order_file_name').empty();
            });

            $('#uploadBtnProgramElementChangeOrderModal').unbind('click').on('click', function (e) {
                e.preventDefault();
                $("#document_type_program_element_change_order_modal  option").filter(":selected").val("1");

                var docTypeID = $("#document_type_program_element_change_order_modal  option").filter(":selected").val();
                var files = fileUploadProgramElementChangeOrderModal.files;
                var docTypeName = $("#document_type_program_element_change_order option").filter(":selected").text();
                if (files.length == 0 || !files.length) { // || !docTypeID Temporary commented 
                    dhtmlx.alert('Please select a file.');
                    return;
                }
                if (files[0].size / 1024 / 1024 > 128) {
                    dhtmlx.alert('File size exceed 128MB. Please select a smaller size file.');
                    return;
                }
                $('#fileUploadProgramElementChangeOrderModal').prop('disabled', false);
                //$('#spinRow').show();
                $('#uploadBtnProgramelmtCOspinRow').show();    //Manasi 20-08-2020
                $("#update_program_element_change_order_modal").attr('disabled', 'disabled');
                formdata = new FormData();
                var fileName = "";
                angular.forEach(fileUploadProgramElementChangeOrderModal.files, function (value, key) {
                    fileName = value.name;
                    formdata.append(key, value);
                });

                console.log(!wbsTree.getIsProgramElementNew(), wbsTree.getIsProgramElementChangeOrderNew());
                docTypeID = 1;
                //Updating program element but new program element change order
                if (wbsTree.getIsProgramElementChangeOrderNew() || (wbsTree.getIsProgramElementNew() && !wbsTree.getIsProgramElementChangeOrderNew())) {	//!wbsTree.getIsProgramNew() && wbsTree.getIsProgramContractNew()
                    //Insert upload file into draft list and update draft list
                    var draftList = [];
                    //for(var x = 0; x < wbsTree.getProgramElementFileDraft().length; x++) {
                    //	draftList.push(wbsTree.getProgramElementFileDraft()[x]);
                    //}

                    draftList.push({ docTypeID: docTypeID, formdata: formdata, fileName: fileName, docTypeName: docTypeName });
                    console.log(draftList);
                    wbsTree.setProgramElementChangeOrderFileDraft(draftList);

                    $('#program_element_change_order_file_name').empty();
                    $('#program_element_change_order_file_name').prepend('<label>' + fileName + '</a>')
                    $('#uploadBtnProgramelmtCOspinRow').hide();
                    $("#update_program_element_change_order_modal").removeAttr('disabled');
                    //  document.getElementById("fileUploadProgramElementChangeOrderModal").value = "";

                    return;
                }

                console.log(wbsTree.getSelectedProgramElementChangeOrder());

                var request = {
                    method: 'POST',
                    url: serviceBasePath + '/uploadFiles/Post/ProgramElementChangeOrder/0/0/0/0/' + wbsTree.getSelectedProgramElementChangeOrder().ChangeOrderID + '/' + docTypeID,
                    data: formdata, //fileUploadProject.files, //$scope.
                    ignore: true,
                    headers: {
                        'Content-Type': undefined
                    }
                };

                /* Needs $http */
                // SEND THE FILES.
                $http(request).then(function success(d) {
                    console.log(d);

                    // Pritesh Start

                    $('#program_element_change_order_file_name').empty();
                    $('#program_element_change_order_file_name').prepend('<label>' + fileName + '</a>')

                    $("#update_program_element_change_order_modal").removeAttr('disabled');
                    fileUploadProgramElementChangeOrderModal.value = "";
                    // formdata = new FormData();
                    $('#uploadBtnProgramElementChangeOrderModal').prop('disabled', false);
                    $('#uploadBtnProgramelmtCOspinRow').hide();


                    // Pritesh Commented start
                    //// var gridUploadedDocument = modal.find('.modal-body #gridUploadedDocument tbody');
                    //// gridUploadedDocument.empty();

                    // wbsTree.setSelectedProgramElementChangeOrderDocument({
                    //     DocumentID: d.data,
                    //     DocumentName: files[0].name
                    // });

                    // $http.get(serviceBasePath + "Request/Document/ProgramElementChangeOrder/" + wbsTree.getSelectedProgramElementChangeOrder().ChangeOrderID)
                    //     .then(function success(response) {
                    //         console.log(response);
                    //         wbsTree.setDocumentList(response.data.result);

                    //         console.log(_documentList);

                    //         if (_documentList.length > 0) {
                    //             $('#program_element_change_order_file_name').empty();
                    //             $('#program_element_change_order_file_name').prepend('<a href="' + serviceBasePath + 'Request/DocumentByDocID/' + _documentList[0].DocumentID + '">' + _documentList[0].DocumentName + '</a>')
                    //         }
                    //         $("#update_program_element_change_order_modal").removeAttr('disabled');
                    //         fileUploadProgramElementChangeOrderModal.value = "";
                    //        // formdata = new FormData();
                    //         $('#uploadBtnProgramElementChangeOrderModal').prop('disabled', false);
                    //         $('#spinRow').hide();

                    //        // var deleteDocBtn = modal.find('.modal-body #delete-doc');
                    //        // deleteDocBtn.attr('disabled', _documentList.length > 0 ? false : true);
                    //     }, function error() { }).
                    //     finally(function () {
                    //         console.log($rootScope);
                    //         // $rootScope.buttonDisabled =false;
                    //     })


                    // Pritesh Commented End
                    // dhtmlx.alert(d.data);
                }, function error(d) {
                    dhtmlx.alert(d.ExceptionMessage);
                }).finally(function () {
                    //Clear selected files
                    //fileUploadProgramElementChangeOrderModal.value = "";
                    //formdata = new FormData();
                    //$('#uploadBtnProgramElementChangeOrderModal').prop('disabled', false);
                    //$('#spinRow').hide();
                })

                    ;

            });

            //03-05-2022
            ////Program element change order
            //function populateProgramElementChangeOrderTableNew() {
            //    $('#program_element_change_order_table_id').empty();
            //    //  alert(FileName);
            //    for (var x = 0; x < g_program_element_change_order_draft_list.length; x++) {
            //        var singeChangeOrder = {};

            //        singeChangeOrder = g_program_element_change_order_draft_list[x];

            //        console.log(singeChangeOrder);
            //        //  alert(singeChangeOrder.DocumentName);

            //        $('#program_element_change_order_table_id').append(
            //            '<tr id="' + singeChangeOrder.ChangeOrderID + '" class="fade-selection-animation clickable-row">' +
            //            '<td class="class-td-LiveView" style="font-family:Verdana, Arial, sans-serif !important;color:#333 !important;text-overflow: ellipsis;white-space: nowrap;">' + singeChangeOrder.DocumentName + '</td>' + /**/
            //            '<td class="class-td-LiveView" style="font-family:Verdana, Arial, sans-serif !important;color:#333 !important;text-overflow: ellipsis;white-space: nowrap;">' + singeChangeOrder.ChangeOrderName + '</td>' +
            //            '<td class="class-td-LiveView" style="font-family:Verdana, Arial, sans-serif !important;color:#333 !important;text-overflow: ellipsis;white-space: nowrap;">' + singeChangeOrder.OrderType + '</td>' +
            //            '<td class="class-td-LiveView" style="font-family:Verdana, Arial, sans-serif !important;color:#333 !important;text-overflow: ellipsis;white-space: nowrap;">' + singeChangeOrder.ChangeOrderNumber + '</td>' +
            //            '<td class="class-td-LiveView" style="font-family:Verdana, Arial, sans-serif !important;color:#333 !important;text-overflow: ellipsis;white-space: nowrap;">' + singeChangeOrder.ChangeOrderAmount + '</td>' +
            //            '<td class="class-td-LiveView" style="font-family:Verdana, Arial, sans-serif !important;color:#333 !important;text-overflow: ellipsis;white-space: nowrap;">' + singeChangeOrder.OrderDate + '</td>' +
            //            '<td class="class-td-LiveView" style="font-family:Verdana, Arial, sans-serif !important;color:#333 !important;text-overflow: ellipsis;white-space: nowrap;">' + singeChangeOrder.ChangeOrderScheduleChange + '</td>' +
            //            '</tr>'
            //        );



            //        //$('#program_element_change_order_table_id').append(
            //        //    '<tr id="' + singeChangeOrder.ChangeOrderName + '" class="fade-selection-animation clickable-row">' +
            //        //   // '<td class="class-td-LiveView" style="width:20%;">' + singeChangeOrder.DocumentName + '</td>' +
            //        //    '<td class="class-td-LiveView" style="width:20%;">' + singeChangeOrder.ChangeOrderName + '</td>' +
            //        //    '<td class="class-td-LiveView" style="width:20%;">' + singeChangeOrder.OrderType + '</td>' +
            //        //    '<td class="class-td-LiveView" style="width:15%;">' + singeChangeOrder.ChangeOrderNumber + '</td>' +
            //        //    '<td class="class-td-LiveView" style="width:15%;">' + singeChangeOrder.ChangeOrderAmount + '</td>' +
            //        //    '<td class="class-td-LiveView" style="width:20%;">' + singeChangeOrder.OrderDate + '</td>' +
            //        //    '<td class="class-td-LiveView" style="width:50%;">' + singeChangeOrder.ChangeOrderScheduleChange + '</td>' +
            //        //    '</tr>'
            //        //);
            //    }
            //}

            //03-05-2022
            //$('#update_program_element_change_order_modal').unbind('click').on('click', function ($files) {
            //    //03-05-2022
            //    // $('#fileUploadProgramElementChangeOrderModal').prop('disabled', false);  //Manasi 20-08-2020
            //    //$('#uploadBtnProgramelmtCOspinRow').show();   //Manasi 20-08-2020
            //    debugger;
            //    var selectedNode = wbsTree.getSelectedNode();
            //    if (selectedNode.level == "Program") {
            //        dhtmlx.alert('Change Order only work in edit mode.');
            //        return;
            //    }
            //    console.log('get files', $files);
            //    console.log(g_newProgramElementChangeOrder);
            //    // var files = fileUploadProgramElementChangeOrderModal.files;
            //    // var fileName = "";
            //    //if (files.length != 0 || files.length) {
            //    //    angular.forEach(fileUploadProgramElementChangeOrderModal.files, function (value, key) {
            //    //        fileName = value.name;
            //    //    });
            //    //}
            //    var g_newProgramElement = $("#g_newProgramElement").val();
            //    var CoTitle = $("#program_element_change_order_name_modal").val();
            //    var ChangeOrderTypedd = $("#program_element_change_order_ddModificationType").val();;
            //    var OrderNo = $("#program_element_change_order_number_modal").val();
            //    var OrderDte = $("#ChangeOrderDate").val();
            //    var AmtOrder = $("#program_element_change_order_amount_modal").val();
            //    var SpNote = $("#program_element_change_order_schedule_change_modal").val();
            //    var Reason = $("#program_element_change_order_Reason_modal").val();
            //    var modType = $('#program_element_change_order_ddModificationType').val();
            //    //var durationDate = $('#program_element_change_order_duration_date').val(); // Jignesh-24-03-2021
            //    var scheduleImpact = $('#program_element_change_order_schedule_impact').val(); // Jignesh-24-03-2021

            //    if (CoTitle == "" || CoTitle.length == 0) {
            //        dhtmlx.alert('Enter Title.');
            //        return;
            //    }
            //    if (OrderDte == "" || OrderDte.length == 0) {
            //        dhtmlx.alert('Enter Date.');
            //        return;
            //    }
            //    //Vaishnavi 08-02-2022
            //    if (OrderDte) {

            //        var testDate = moment(OrderDte, 'M/D/YYYY', true).isValid();
            //        if (!testDate) {
            //            dhtmlx.alert('Date Should be in MM/DD/YYYY Format.');
            //            return;
            //        }
            //    }

            //    if (Reason == "" || Reason.length == 0) {
            //        dhtmlx.alert('Enter Reason.');
            //        return;
            //    }
            //    if (SpNote == "" || SpNote.length == 0) {
            //        dhtmlx.alert('Enter Description.'); // Jignesh-18-02-2021
            //        return;
            //    }
            //    if (OrderNo == "" || OrderNo.length == 0) {
            //        dhtmlx.alert('Enter Client Change Order No.'); // Jignesh-08-02-2021
            //        return;
            //    }
            //    if (ChangeOrderTypedd == null || ChangeOrderTypedd == 0) {
            //        dhtmlx.alert('Select Order Type.');
            //        return;
            //    }
            //    //if (AmtOrder == "" || AmtOrder.length == 0) {
            //    //    dhtmlx.alert('Enter Amount.');
            //    //    return;
            //    //}

            //    if (modType != "" || modType.length != 0) {
            //        if (modType == 1) {
            //            if (AmtOrder == "" || AmtOrder.length == 0) {
            //                dhtmlx.alert('Enter Value.');
            //                return;
            //            }
            //        }
            //        else if (modType == 2) {//changeOrderScheduleImpact
            //            // Jignesh-24-03-2021
            //            //if (durationDate == "" || durationDate.length == 0) {
            //            //    dhtmlx.alert('Enter Duration Date.');
            //            //    return;
            //            //}
            //            if (scheduleImpact == "" || scheduleImpact.length == 0) {
            //                dhtmlx.alert('Enter Schedule Impact.');
            //                return;
            //            }
            //        }
            //        else if (modType == 3) {
            //            if (AmtOrder == "" || AmtOrder.length == 0) {
            //                dhtmlx.alert('Enter Value.'); // Jignesh-18-02-2021
            //                return;
            //            }
            //            // Jignesh-24-03-2021
            //            if (scheduleImpact == "" || scheduleImpact.length == 0) {
            //                dhtmlx.alert('Enter Schedule Impact.');
            //                return;
            //            }
            //            //if (durationDate == "" || durationDate.length == 0) {
            //            //    dhtmlx.alert('Enter Duration Date.');
            //            //    return;
            //            //}
            //        }
            //    }
            //    //  alert(fileName);
            //    // alert(g_newProgramElementChangeOrder);
            //    if (!g_newProgramElementChangeOrder) {   //Update  ppuu
            //        var updatedChangeOrder = g_selectedProgramElementChangeOrder;
            //        var isModified = true;

            //        updatedChangeOrder.ChangeOrderName = modal.find('.modal-body #program_element_change_order_name_modal').val();
            //        updatedChangeOrder.ChangeOrderNumber = modal.find('.modal-body #program_element_change_order_number_modal').val();
            //        updatedChangeOrder.ChangeOrderAmount = modal.find('.modal-body #program_element_change_order_amount_modal').val();
            //        updatedChangeOrder.ChangeOrderScheduleChange = modal.find('.modal-body #program_element_change_order_schedule_change_modal').val();
            //        updatedChangeOrder.OrderType = modal.find('.modal-body #ChangeOrderType').val();
            //        updatedChangeOrder.OrderDate = modal.find('.modal-body #ChangeOrderDate').val();

            //        //================== Jignesh-ChangeOrderPopUpChanges ====================================
            //        updatedChangeOrder.Reason = modal.find('.modal-body #program_element_change_order_Reason_modal').val();
            //        updatedChangeOrder.ModificationTypeId = modal.find('.modal-body #program_element_change_order_ddModificationType').val();
            //        //updatedChangeOrder.DurationDate = modal.find('.modal-body #program_element_change_order_duration_date').val(); // Jignesh-24-03-2021
            //        updatedChangeOrder.ScheduleImpact = modal.find('.modal-body #program_element_change_order_schedule_impact').val(); // Jignesh-24-03-2021
            //        //=======================================================================================

            //        if (g_newProgramElement) {
            //            //Find the one in the draft list
            //            for (var x = 0; x < g_program_element_change_order_draft_list.length; x++) {
            //                if (g_program_element_change_order_draft_list[x].ChangeOrderName == g_selectedProgramElementChangeOrder.ChangeOrderName
            //                    && g_program_element_change_order_draft_list[x].ChangeOrderNumber == g_selectedProgramElementChangeOrder.ChangeOrderNumber
            //                    && g_program_element_change_order_draft_list[x].ChangeOrderAmount == g_selectedProgramElementChangeOrder.ChangeOrderAmount
            //                    && g_program_element_change_order_draft_list[x].ChangeOrderScheduleChange == g_selectedProgramElementChangeOrder.ChangeOrderScheduleChange) {
            //                    //  g_program_element_change_order_draft_list[x].DocumentName = fileName;
            //                    g_program_element_change_order_draft_list[x].ChangeOrderName = updatedChangeOrder.ChangeOrderName;
            //                    g_program_element_change_order_draft_list[x].ChangeOrderNumber = updatedChangeOrder.ChangeOrderNumber;
            //                    g_program_element_change_order_draft_list[x].ChangeOrderAmount = updatedChangeOrder.ChangeOrderAmount;
            //                    g_program_element_change_order_draft_list[x].ChangeOrderScheduleChange = updatedChangeOrder.ChangeOrderScheduleChange;
            //                    g_program_element_change_order_draft_list[x].OrderType = updatedChangeOrder.OrderType;
            //                    g_program_element_change_order_draft_list[x].OrderDate = updatedChangeOrder.OrderDate;
            //                    //================== Jignesh-ChangeOrderPopUpChanges ====================================
            //                    g_program_element_change_order_draft_list[x].Reason = updatedChangeOrder.Reason;
            //                    g_program_element_change_order_draft_list[x].ModificationTypeId = updatedChangeOrder.ModificationTypeId;
            //                    //g_program_element_change_order_draft_list[x].DurationDate = updatedChangeOrder.DurationDate; // Jignesh-24-03-2021
            //                    g_program_element_change_order_draft_list[x].ScheduleImpact = updatedChangeOrder.ScheduleImpact; // Jignesh-24-03-2021
            //                    //=======================================================================================

            //                    if (wbsTree.getProgramElementChangeOrderFileDraft().length > 0) {
            //                        g_program_element_change_order_draft_list[x].DocumentDraft = wbsTree.getProgramElementChangeOrderFileDraft();
            //                    }
            //                }
            //            }
            //            populateProgramElementChangeOrderTableNew();
            //            $('#ProgramElementChangeOrderModal').modal('hide');
            //            $("#ProgramElementModal").css({ "opacity": "1" });
            //            return;
            //        }

            //        debugger;
            //        //update- Added by Amruta to save end date on change order

            //        if (updatedChangeOrder.ScheduleImpact != "") {
            //            var curendt = new Date($('#program_element_PEnd_Date').val());
            //            curendt.setDate(curendt.getDate() - parseInt(progelem_scheduleImp));
            //            curendt.setDate(curendt.getDate() + parseInt(updatedChangeOrder.ScheduleImpact));
            //            $('#program_element_PEnd_Date').val(moment(curendt).format('MM/DD/YYYY')); //.change(); //Added by Amruta for confirmation popup
            //        }
            //        debugger;
            //        var pendDate = $('#program_element_PEnd_Date').val();
            //        var projectEndDate = moment(pendDate).format('MM/DD/YYYY');

            //        var obj = {
            //            "Operation": 2,
            //            "ChangeOrderID": updatedChangeOrder.ChangeOrderID,
            //            "ChangeOrderName": updatedChangeOrder.ChangeOrderName,
            //            "ChangeOrderNumber": updatedChangeOrder.ChangeOrderNumber,
            //            "ChangeOrderAmount": updatedChangeOrder.ChangeOrderAmount.replace('$', ''),
            //            "ChangeOrderScheduleChange": updatedChangeOrder.ChangeOrderScheduleChange,
            //            "ProgramElementID": selectedNode.ProgramElementID,
            //            "ProjectEndDateCO": projectEndDate,
            //            "OrderType": updatedChangeOrder.OrderType,
            //            "OrderDate": updatedChangeOrder.OrderDate,
            //            //================== Jignesh-ChangeOrderPopUpChanges ====================================
            //            "Reason": updatedChangeOrder.Reason,
            //            "ModificationTypeId": updatedChangeOrder.ModificationTypeId,
            //            //"DurationDate": updatedChangeOrder.DurationDate // Jignesh-24-03-2021
            //            "ScheduleImpact": updatedChangeOrder.ScheduleImpact // Jignesh-24-03-2021
            //            //=======================================================================================
            //        }

            //        var listToSave = [];
            //        listToSave.push(obj);

            //        //API to Insert/Update ppbb
            //        wbsTree.getUpdateChangeOrder({ ProjectID: 1 }).save(listToSave, function (response) {
            //            //alert(response.result.split(',')[0]); //Manasi
            //            //if (response.result.split(',')[0].trim() === "Success") {successfully
            //            if (response.result.indexOf('successfully') >= 0) {  //Manasi
            //                //Added by Amruta for populating the end date post exit modal -1
            //                selectedNode.ProjectPEndDate = $('#ProgramElementModal').find('.modal-body #program_element_PEnd_Date').val();
            //                wbsTree.updateTreeNodes(selectedNode);
            //                dhtmlx.alert({
            //                    text: response.result,
            //                    width: '500px'
            //                });
            //                //-------Manasi
            //                $('#ProgramElementChangeOrderModal').modal('hide');
            //                $("#ProgramElementModal").css({ "opacity": "1" });

            //                //Manu: 11/01/2022 
            //                /*  var curendt = new Date($('#program_element_PEnd_Date').val());
            //                  curendt.setDate(curendt.getDate() - parseInt(progelem_scheduleImp));
            //                  curendt.setDate(curendt.getDate() + parseInt(updatedChangeOrder.ScheduleImpact));
            //                  $('#program_element_PEnd_Date').val(moment(curendt).format('MM/DD/YYYY')).change();*/ //Added by Amruta for confirmation popup


            //                //$('#ProgramModal').modal('hide');
            //            } else {
            //                if (response.result == '' || response.result == null || response.result == undefined)
            //                    dhtmlx.alert('Something went wrong. Please try again..');
            //                else {
            //                    dhtmlx.alert({
            //                        text: response.result,
            //                        width: '500px'
            //                    });
            //                }

            //                return;  //Manasi
            //            }

            //            $('#ProgramElementChangeOrderModal').modal('hide');
            //            $("#ProgramElementModal").css({ "opacity": "1" });
            //            debugger;
            //            populateProgramElementChangeOrderTable(selectedNode.ProgramElementID);

            //            g_selectedProgramElementChangeOrder = null; 	//Manasi
            //            //$('#uploadBtnProgramelmtCOspinRow').hide()     //Manasi 20-08-2020
            //            document.getElementById("uploadBtnProgramelmtCOspinRow").style.display = "none";   //Manasi 20-08-2020
            //        });

            //    }

            //    if (g_newProgramElementChangeOrder) {
            //        var newNode = { name: "New Program Element Change Order" };
            //        var newChangeOrder = {};

            //        newChangeOrder.ChangeOrderName = modal.find('.modal-body #program_element_change_order_name_modal').val();
            //        newChangeOrder.ChangeOrderNumber = modal.find('.modal-body #program_element_change_order_number_modal').val();
            //        newChangeOrder.ChangeOrderAmount = modal.find('.modal-body #program_element_change_order_amount_modal').val();
            //        newChangeOrder.ChangeOrderScheduleChange = modal.find('.modal-body #program_element_change_order_schedule_change_modal').val();
            //        newChangeOrder.OrderType = modal.find('.modal-body #ChangeOrderType').val();
            //        newChangeOrder.OrderDate = modal.find('.modal-body #ChangeOrderDate').val();
            //        //================== Jignesh-ChangeOrderPopUpChanges ====================================
            //        newChangeOrder.Reason = modal.find('.modal-body #program_element_change_order_Reason_modal').val();
            //        newChangeOrder.ModificationTypeId = modal.find('.modal-body #program_element_change_order_ddModificationType').val();
            //        //newChangeOrder.DurationDate = modal.find('.modal-body #program_element_change_order_duration_date').val(); // Jignesh-24-03-2021
            //        newChangeOrder.ScheduleImpact = modal.find('.modal-body #program_element_change_order_schedule_impact').val(); // Jignesh-24-03-2021
            //        //=======================================================================================
            //        var orgendt = new Date($('#program_element_PEnd_Date').val());

            //        if (newChangeOrder.ScheduleImpact != "") {
            //            var curendt = new Date($('#program_element_PEnd_Date').val());
            //            curendt.setDate(curendt.getDate() - parseInt(progelem_scheduleImp));
            //            curendt.setDate(curendt.getDate() + parseInt(newChangeOrder.ScheduleImpact));
            //            $('#program_element_PEnd_Date').val(moment(curendt).format('MM/DD/YYYY'));//.change(); Added by Amruta for confirmation popup-1
            //            debugger;
            //        }

            //        var pendDate = $('#program_element_PEnd_Date').val();
            //        var projectEndDate = moment(pendDate).format('MM/DD/YYYY');

            //        var obj = {
            //            "Operation": 1,
            //            "ChangeOrderID": 0,
            //            "ChangeOrderName": newChangeOrder.ChangeOrderName,
            //            "ChangeOrderNumber": newChangeOrder.ChangeOrderNumber,
            //            "ChangeOrderAmount": newChangeOrder.ChangeOrderAmount.replace('$', ''),
            //            "ChangeOrderScheduleChange": newChangeOrder.ChangeOrderScheduleChange,
            //            "ProgramElementID": selectedNode.ProgramElementID,
            //            "ProjectEndDateCO": projectEndDate,
            //            "OrderType": newChangeOrder.OrderType,
            //            "OrderDate": newChangeOrder.OrderDate, //.replace(/\//g, ""),
            //            //================== Jignesh-ChangeOrderPopUpChanges ====================================
            //            "Reason": newChangeOrder.Reason,
            //            "ModificationTypeId": newChangeOrder.ModificationTypeId,
            //            //"DurationDate": newChangeOrder.DurationDate // Jignesh-24-03-2021
            //            "ScheduleImpact": newChangeOrder.ScheduleImpact // Jignesh-24-03-2021
            //            //=======================================================================================
            //        }

            //        var listToSave = [];
            //        listToSave.push(obj);

            //        //   alert(g_newProgramElement);
            //        if (g_newProgramElement) {
            //            g_program_element_change_order_draft_list.push({
            //                "Operation": 1,
            //                "ChangeOrderID": 0,
            //                //"DocumentName": fileName,
            //                "ChangeOrderName": newChangeOrder.ChangeOrderName,
            //                "ChangeOrderNumber": newChangeOrder.ChangeOrderNumber,
            //                "ChangeOrderAmount": newChangeOrder.ChangeOrderAmount.replace('$', ''),
            //                "ChangeOrderScheduleChange": newChangeOrder.ChangeOrderScheduleChange,
            //                "ProgramElementID": wbsTree.getSelectedProgramElementID(),
            //                "ProjectEndDateCO": projectEndDate,
            //                "OrderType": newChangeOrder.OrderType,
            //                "OrderDate": newChangeOrder.OrderDate,
            //                //================== Jignesh-ChangeOrderPopUpChanges ====================================
            //                "Reason": newChangeOrder.Reason,
            //                "ModificationTypeId": newChangeOrder.ModificationTypeId,
            //                //"DurationDate": newChangeOrder.DurationDate // Jignesh-24-03-2021
            //                "ScheduleImpact": newChangeOrder.ScheduleImpact // Jignesh-24-03-2021
            //                //=======================================================================================
            //            });

            //            // alert(fileName);
            //            //ppaaa
            //            populateProgramElementChangeOrderTableNew();
            //            $('#ProgramElementChangeOrderModal').modal('hide');
            //            $("#ProgramElementModal").css({ "opacity": "1" });
            //            return;
            //        }

            //        console.log(listToSave);

            //        //API to Insert Program Element CHANGE ORDER
            //        wbsTree.getUpdateChangeOrder({ ProjectID: 1 }).save(listToSave,
            //            function (response) {
            //                console.log(response);
            //                //var newChangeOrderID = response.result.split(',')[1].trim();
            //                if (response.result.split(',')[0].trim() === "Success") {
            //                    //Added by Amruta for populating the end date post exit modal -2
            //                    selectedNode.ProjectPEndDate = $('#ProgramElementModal').find('.modal-body #program_element_PEnd_Date').val();
            //                    wbsTree.updateTreeNodes(selectedNode);
            //                    var newChangeOrderID = response.result.split(',')[1].trim();

            //                    //Upload draft documents
            //                    var index = 0;

            //                    var apiUpload = function () {
            //                        //03-05-2022
            //                        //if ($('#program_element_change_order_file_name').html().length == 0) {
            //                        //    return;
            //                        //}
            //                        ////if (index >= wbsTree.getProgramElementChangeOrderFileDraft().length) {
            //                        ////    return;
            //                        ////}
            //                        //docTypeID = 1;
            //                        //// docTypeID = wbsTree.getProgramElementChangeOrderFileDraft()[index].docTypeID;
            //                        ////  formdata = wbsTree.getProgramElementChangeOrderFileDraft()[index].formdata;

            //                        //formdata = new FormData();
            //                        //var fileName = "";

            //                        //angular.forEach(fileUploadProgramElementChangeOrderModal.files, function (value, key) {
            //                        //    fileName = value.name;
            //                        //    formdata.append(key, value);
            //                        //});
            //                        //var request = {
            //                        //    method: 'POST',
            //                        //    url: serviceBasePath + '/uploadFiles/Post/ProgramElementChangeOrder/0/0/0/0/' + newChangeOrderID + '/' + docTypeID,
            //                        //    data: formdata, //fileUploadProject.files, //$scope.
            //                        //    ignore: true,
            //                        //    headers: {
            //                        //        'Content-Type': undefined
            //                        //    }
            //                        //};

            //                        //var angularHttp = wbsTree.getAngularHttp();
            //                        //angularHttp(request).then(function success(d) {
            //                        //    console.log(d);
            //                        //    debugger;
            //                        //    populateProgramElementChangeOrderTable(selectedNode.ProgramElementID);  //Manasi
            //                        //    $('#program_element_change_order_file_name').empty();

            //                        //    document.getElementById("fileUploadProgramElementChangeOrderModal").value = "";

            //                        //    //$('#uploadBtnProgramelmtCOspinRow').hide();     //Manasi 20-08-2020
            //                        //    document.getElementById("uploadBtnProgramelmtCOspinRow").style.display = "none";   //Manasi 20-08-2020
            //                        //});
            //                    }

            //                    apiUpload();// Paa

            //                    populateProgramElementChangeOrderTableNew(); //Manasi
            //                    $('#ProgramElementChangeOrderModal').modal('hide');
            //                    $("#ProgramElementModal").css({ "opacity": "1" });

            //                    //Manu: 11/01/2022
            //                   /* var curendt = new Date($('#program_element_PEnd_Date').val());
            //                    curendt.setDate(curendt.getDate() - parseInt(progelem_scheduleImp));
            //                    curendt.setDate(curendt.getDate() + parseInt(newChangeOrder.ScheduleImpact));
            //                    $('#program_element_PEnd_Date').val(moment(curendt).format('MM/DD/YYYY')).change()*/;//Added by Amruta for confirmation popup-1
            //                    //

            //                } else {
            //                    //$('#uploadBtnProgramelmtCOspinRow').hide();     //Manasi 20-08-2020
            //                    document.getElementById("uploadBtnProgramelmtCOspinRow").style.display = "none";   //Manasi 20-08-2020
            //                    debugger;
            //                    if (response.result == '' || response.result == null || response.result == undefined) {
            //                        $('#program_element_PEnd_Date').val(moment(orgendt).format('MM/DD/YYYY'));
            //                        dhtmlx.alert('Something went wrong. Please try again..');
            //                    }

            //                    else {
            //                        dhtmlx.alert({ text: response.result, width: '500px' });
            //                    }
            //                    //$('#ProgramElementChangeOrderModal').modal('hide');  Manasi
            //                    //$("#ProgramElementModal").css({ "opacity": "1" });
            //                    return; //Manasi
            //                }
            //                $('#ProgramElementChangeOrderModal').modal('hide');
            //                $("#ProgramElementModal").css({ "opacity": "1" });
            //                debugger;
            //                populateProgramElementChangeOrderTable(selectedNode.ProgramElementID);

            //                g_selectedProgramElementChangeOrder = null; 	//Manasi
            //                //  $('#program_element_change_order_file_name').empty(); // manasi 7 jul 2020
            //            });

            //        //$('#program_element_change_order_file_name').empty();

            //        //document.getElementById("fileUploadProgramElementChangeOrderModal").value = ""; prith commented on 24jul2020 as was not abe to save
            //    }



            //});

            $scope.getTheFiles = function ($files) {
                console.log('get files', $files);
                $scope.files = $files;
                $('#uploadBtnProject').prop('disabled', false);
                angular.forEach($files, function (value, key) {
                    //$scope.selectedFileName = $files[0].name;
                    formdata.append(key, value);
                    $('#uploadBtnProject').prop('disabled', false);
                });

                //$scope.uploadFiles();
            };



            //$("#cancel_futuretrend").on('click', function () {
            //    $("#FutureTrendModal").modal('hide');
            //});
            //$("#cancel_trend").on('click', function () {
            //    $("#FutureTrendModal").modal('hide');
            //});
            $("#generate").on('click', function () {
                //$window.location.reload();
                //$state.reload();
            });
            onRouteChangeOff = $scope.$on('$locationChangeStart', function (event) {
                var newUrl = $location.path();

            });
            $(window).unbind('resize').resize(function () {
                //  $scope.loadOrganizations();
                var viewerWidth = $("#program_navigation").width();
                var viewerHeight = $("#program_navigation").height();
                wbsTree.setViewerWidth(viewerWidth);
                wbsTree.setViewerHeight(viewerHeight);
                var selNode = wbsTree.getSelectedNode();
                var modal;
                var modal_mode;
                //var s = $('line');
                $('line').remove();
                tree = d3.layout.tree().size([viewerHeight, viewerWidth]);
                wbsTree.renderTreeGrid(s, svgGroup, zoomListener, $scope);

                wbsTree.bindEventsForWBSNodes($scope);
                console.debug("SEl Node", selNode);
                if (selNode) {
                    if (selNode.level == "Project") {
                        $timeout(function () {
                            wbsTree.click(wbsTree, selNode);

                        }, 250);
                    }
                }

            });

            var projectMap = new ProjectMap();
            projectMap.setScope($scope);
            projectMap.setRootScope($rootScope);

            var wbsTree = new WBSTree();
            wbsTree.setScope($scope);

            //set angular http
            wbsTree.setAngularHttp($http)

            console.debug(wbsTree);
            wbsTree.setOrganizationService(Organization);
            wbsTree.setProgram(Program);
            wbsTree.setProgramElement(ProgramElement);
            wbsTree.setProject(Project);
            wbsTree.setTrend(Trend);
            wbsTree.setFundType(FundType);
            wbsTree.setProgramCategory(ProgramCategory);
            wbsTree.setPhaseCode(PhaseCode);
            wbsTree.setTrendStatusCode(TrendStatusCode);
            wbsTree.setProgramFund(ProgramFund);
            wbsTree.setFilter($filter);
            wbsTree.setRootScope($rootScope);
            wbsTree.setModal($uibModal);
            wbsTree.setLocalStorage(localStorageService.get('authorizationData'));
            wbsTree.setProjectScope(ProjectScope);
            wbsTree.setDocument(Document);
            wbsTree.setProjectWhiteList(ProjectWhiteList);
            wbsTree.setProjectWhiteListService(ProjectWhiteList);
            wbsTree.setUpdateProjectWhiteList(UpdateProjectWhiteList);
            wbsTree.setUpdateContract(UpdateContract);
            wbsTree.setUpdateMilestone(UpdateMilestone);
            wbsTree.setUpdateChangeOrder(UpdateChangeOrder);
            wbsTree.setContract(Contract);
            wbsTree.setProgramContract(ProgramContract);
            wbsTree.setMilestone(Milestone);
            wbsTree.setChangeOrder(ChangeOrder);

            //luan here
            Contract.get({}, function (response) {
                console.log(response);
                wbsTree.setContractList(response.result);
            });
            ProgramContract.get({}, function (response) {
                console.log(response);
                wbsTree.setProgramContractList(response.result);
            });
            Milestone.get({}, function (response) {
                console.log(response);
                wbsTree.setMilestoneList(response.result);
            });
            ChangeOrder.get({}, function (response) {
                console.log(response);
                wbsTree.setChangeOrderList(response.result);
            });
            ProjectType.get({}, function (response) {
                console.log(response);
                wbsTree.setProjectTypeList(response.result);
            });
            ProjectClass.get({}, function (response) {
                console.log(response);
                wbsTree.setProjectClassList(response.result);
            });
            ServiceClass.get({}, function (response) {
                console.log(response);
                wbsTree.setServiceClassList(response.result);
            });
            LineOfBusiness.get({}, function (response) {
                console.log(response);
                wbsTree.setLineOfBusinessList(response.result);
            });
            Client.get({}, function (response) {
                console.log(response);
                wbsTree.setClientList(response.result);
            });
            ClientPOC.get({}, function (response) {                      //Tanmay - 15/12/2021
                console.log(response);
                wbsTree.setClientPOCList(response.result);
            });
            Prime.get({}, function (response) {
                console.log(response);
                wbsTree.setPrimeList(response.result);
            });
            Location.get({}, function (response) {
                console.log(response);
                wbsTree.setLocationList(response.result);
            });
            User.get({}, function (response) {
                console.log(response);
                wbsTree.setUserList(response.result);
            });
            AllEmployee.get({}, function (response) {
                console.log(response);
                wbsTree.setEmployeeList(response.result);
            });
            DocumentType.get({}, function (response) {
                console.log('Doc Type...');
                console.log(response);
                wbsTree.setDocTypeList(response.result);
            });
            ProjectWhiteList.get({}, function (response) {
                console.log(response);
                wbsTree.setProjectWhiteList(response.result);
            });
            //vaishnavi 12-04-2022
            CertifiedPayroll.get({}, function (response) {
                console.log(response);
                wbsTree.setCertifiedPayrollList(response.result);
            });
            Wrap.get({}, function (response) {
                console.log(response);
                wbsTree.setWrapList(response.result);
            });

            //Document.get({ ProjectID: localStorage.getItem('selectProjectIdDash') }, function (response) { // _selectedProjectID
            //    alert('selected project id ' + localStorage.getItem('selectProjectIdDash')); //+ _selectedProjectID
            //    console.log('Doc ...');
            //    console.log(response);
            //    wbsTree.setDocList(response.result);
            //});
            wbsTree.setProjectNumber(ProjectNumber);
            wbsTree.setProjectElementNumber(ProjectElementNumber);
            wbsTree.setTrendId(TrendId);

            var wbsTrendTree = new WBSTrendTree();
            wbsTrendTree.setScope($scope);
            wbsTrendTree.setWBSTree(wbsTree);
            wbsTrendTree.setHttpProvider($http);
            wbsTrendTree.setTrend(Trend);
            wbsTrendTree.setApprovalThresholdInfo(localStorageService.get('authorizationData'));
            wbsTrendTree.setRequestApproval(RequestApproval);
            wbsTrendTree.setTrendStatusCode(TrendStatusCode);
            wbsTrendTree.setProjectWhiteListService(ProjectWhiteList);
            wbsTree.setWBSTrendTree(wbsTrendTree);
            wbsTree.setProjectMap(projectMap);

            var tree;
            var baseSvg;
            var zoomListener;

            var options = {

                // Boolean - Whether to animate the chart
                animation: true,
                // Number - Number of animation steps
                animationSteps: 60,
                animationEasing: "easeOutQuart",
                bezierCurve: false,
                // Boolean - If we should show the scale at all
                showScale: true,
                // Boolean - If we want to override with a hard coded scale
                scaleOverride: false,
                // ** Required if scaleOverride is true **
                // Number - The number of steps in a hard coded scale
                scaleSteps: null,
                // Number - The value jump in the hard coded scale
                scaleStepWidth: null,
                // Number - The scale starting value
                scaleStartValue: null,
                // String - Colour of the scale line
                scaleLineColor: "rgba(0,0,0,.1)",
                // Number - Pixel width of the scale line
                scaleLineWidth: 1,
                // Boolean - Whether to show labels on the scale
                scaleShowLabels: true,
                // Interpolated JS string - can access value
                scaleLabel: "<%=value%>",
                // Boolean - Whether the scale should stick to integers, not floats even if drawing space is there
                scaleIntegersOnly: true,
                // Boolean - Whether the scale should start at zero, or an order of magnitude down from the lowest value
                scaleBeginAtZero: false,
                // String - Scale label font declaration for the scale label
                scaleFontFamily: "'Helvetica Neue', 'Helvetica', 'Arial', sans-serif",
                // Number - Scale label font size in pixels
                scaleFontSize: 12,
                // String - Scale label font weight style
                scaleFontStyle: "normal",
                // String - Scale label font colour
                scaleFontColor: "#666",
                // Boolean - whether or not the chart should be responsive and resize when the browser does.
                responsive: false,
                // Boolean - whether to maintain the starting aspect ratio or not when responsive, if set to false, will take up entire container
                maintainAspectRatio: true,
                // Boolean - Determines whether to draw tooltips on the canvas or not
                showTooltips: true,
                // Function - Determines whether to execute the customTooltips function instead of drawing the built in tooltips (See [Advanced - External Tooltips](#advanced-usage-custom-tooltips))
                customTooltips: false,
                // Array - Array of string names to attach tooltip events
                tooltipEvents: ["mousemove", "touchstart", "touchmove"],
                // String - Tooltip background colour
                tooltipFillColor: "rgba(0,0,0,0.8)",
                // String - Tooltip label font declaration for the scale label
                tooltipFontFamily: "'Helvetica Neue', 'Helvetica', 'Arial', sans-serif",
                // Number - Tooltip label font size in pixels
                tooltipFontSize: 14,
                // String - Tooltip font weight style
                tooltipFontStyle: "normal",
                // String - Tooltip label font colour
                tooltipFontColor: "#fff",
                // String - Tooltip title font declaration for the scale label
                tooltipTitleFontFamily: "'Helvetica Neue', 'Helvetica', 'Arial', sans-serif",
                // Number - Tooltip title font size in pixels
                tooltipTitleFontSize: 14,
                // String - Tooltip title font weight style
                tooltipTitleFontStyle: "bold",
                // String - Tooltip title font colour
                tooltipTitleFontColor: "#fff",
                // Number - pixel width of padding around tooltip text
                tooltipYPadding: 6,
                // Number - pixel width of padding around tooltip text
                tooltipXPadding: 6,
                // Number - Size of the caret on the tooltip
                tooltipCaretSize: 8,
                // Number - Pixel radius of the tooltip border
                tooltipCornerRadius: 6,
                // Number - Pixel offset from point x to tooltip edge
                tooltipXOffset: 10,
                // String - Template string for single tooltips
                tooltipTemplate: "<%if (label){%><%=label%>: <%}%><%= value %>",
                // String - Template string for multiple tooltips
                multiTooltipTemplate: "<%= value %>",
                // Function - Will fire on animation progression.
                onAnimationProgress: function () {
                },
                // Function - Will fire on animation completion.
                onAnimationComplete: function () {
                }
            };

            var data = {
                labels: ["2012", "2013", "2014", "2015", "2016"],
                datasets: [
                    {
                        label: "My First dataset",
                        fillColor: "rgba(256,0,0,0.2)",
                        strokeColor: "red",
                        pointColor: "red",
                        pointStrokeColor: "#fff",
                        pointHighlightFill: "#fff",
                        pointHighlightStroke: "rgba(220,220,220,1)",
                        data: [0, 5, 20, 37, 47]
                    },
                    {
                        label: "My Second dataset",
                        fillColor: "rgba(0,256,0,0.2)",
                        strokeColor: "green",
                        pointColor: "green",
                        pointStrokeColor: "#fff",
                        pointHighlightFill: "#fff",
                        pointHighlightStroke: "rgba(151,187,205,1)",
                        data: [0, 5, 20, 37]
                    }
                ]
            };

            // Selected Organization and Programs
            $scope.filterOrgId;
            console.log($scope.filterOrgId);
            $scope.filterProgramId;
            $scope.filterProgramElement;
            $scope.filterProject;
            $scope.defaultProjectElement;

            var s;
            $scope.getDefaultFirstProjectElement = function (treeData) {
                var organization = treeData;
                if (organization.children.length > 0 && organization.children[0].children.length > 0
                    && organization.children[0].children[0].children.length > 0) {
                    var projectElement = organization.children[0].children[0].children[0];
                    $scope.defaultProjectElement = projectElement.ProjectElementNumber + ". " + projectElement.name;
                }
            }
            $scope.retrieveProgramList = function (wbsData) {
                $scope.programList = wbsData.data.children;
                angular.forEach($scope.programList, function (item) {
                    item.ProgramName = item.name;
                })
            }
            //Jignesh-22-10-2021 Quick Search in Program Navigation

            $('#txtQuickSearch').unbind('keypress').on('keypress', function (e) {

                var key = e.which;
                if (key == 13) {
                    $('#btnQuickSearch').click();
                }

            });

            $('#btnQuickSearch').unbind('click').on('click', function () {
                var pagedata = Page;
                $("#btnQuickSearch").prop('disabled', 'disabled');  //vaishnavi 02-03-2022
                $("#FilterTrend").prop('disabled', 'disabled');   //vaishnavi 02-03-2022
                //console.log('Called Org...');
                //Narayan - clear trend map before search
                var firstGNode = $('#trendSvg').children()[0];
                $(firstGNode).children().remove();
                $scope.filterProgramId = "";
                $scope.filterProgramElement = "";
                $scope.filterProject = "";

                $scope.programElementList = "";
                $scope.projectList = "";

                localStorage.removeItem('orgId');
                localStorage.removeItem('pgmId')
                localStorage.removeItem('pgmEltId');
                localStorage.removeItem('SearchText');

                var orgId = $("#selectOrg").val();
                console.log(orgId);
                if (orgId != null && orgId != "") {
                    wbsTree.setSelectedOrganizationID(orgId);
                    Program.lookup().get({ OrganizationID: orgId }, function (programData) {
                        console.log(programData.result);
                        $scope.programList = programData.result;
                    });
                }
                else {
                    orgId = $scope.organizationList[0].OrganizationID;
                    wbsTree.setSelectedOrganizationID(orgId);
                }

                myLocalStorage.set('userSelectedOrgId', orgId);

                var oldsvg = d3.select("#wbs-tree");
                oldsvg.selectAll("*").remove();

                usSpinnerService.spin('spinner-8'); // Narayan - start spinner on click search btn - 11/07/2022

                $("#project_name").html("");
                //  $("#project_name").hide();
                wbsTree.setSelectedNode(null);
                $http.get(serviceBasePath + "Request//ProjectByOid/" + orgId)
                    .then(function (response) {
                        if (response.data.result.length > 0) {
                            var projectNameOnLoad = null;
                            projectNameOnLoad = (localStorage.getItem('selectProjectNameDash'))
                                ? localStorage.getItem('selectProjectNameDash')
                                : response.data.result[0].ProjectElementNumber + ". " + response.data.result[0].ProjectName;
                            localStorage.setItem('selectProjectIdDash', response.data.result[0].ProjectID);
                            localStorage.setItem('selectProjectNameDash', projectNameOnLoad);

                            //For App Sec
                            myLocalStorage.set('selectProjectDataDash', response.data.result[0]);
                            localStorage.setItem('selectProjectProjectManagerIDDash', response.data.result[0].ProjectManagerID);
                            localStorage.setItem('selectProjectDirectorIDDash', response.data.result[0].DirectorID);
                            localStorage.setItem('selectProjectSchedulerIDDash', response.data.result[0].SchedulerID);
                            localStorage.setItem('selectProjectVicePresidentIDDash', response.data.result[0].VicePresidentID);
                            localStorage.setItem('selectProjectFinancialAnalystIDDash', response.data.result[0].FinancialAnalystID);
                            localStorage.setItem('selectProjectCapitalProjectAssistantIDDash', response.data.result[0].CapitalProjectAssistantID);
                            //dhtmlx('at #project_name $http.get orgId:' + orgId);
                        }
                        else {
                            localStorage.setItem('selectProjectIdDash', null);
                            localStorage.setItem('selectProjectNameDash', "");

                            //For App Sec
                            myLocalStorage.set('selectProjectDataDash', null);
                            localStorage.setItem('selectProjectProjectManagerIDDash', null);
                            localStorage.setItem('selectProjectDirectorIDDash', null);
                            localStorage.setItem('selectProjectSchedulerIDDash', null);
                            localStorage.setItem('selectProjectVicePresidentIDDash', null);
                            localStorage.setItem('selectProjectFinancialAnalystIDDash', null);
                            localStorage.setItem('selectProjectCapitalProjectAssistantIDDash', null);
                            //dhtmlx('at #project_name  $http.get orgId:' + orgId + ' else...');
                        }
                        usSpinnerService.stop('spinner-8'); // Narayan - stop spinner on data get api sucess - 11/07/2022
                    }).catch(function (err) {
                        usSpinnerService.stop('spinner-8'); // Narayan - start spinner on data get api fail - 11/07/2022
                    });
                var sreachTxt = $('#txtQuickSearch').val();
                $scope.SearchText = sreachTxt;
                localStorage.setItem('SearchText', sreachTxt);

                var loadfunc = $scope.loadWBSData(orgId, null, null, null, $scope.SearchText, '1', null);
            });
            $scope.FilterTrend = function (e) {
                var oldsvg = d3.select("#wbs-tree");
                oldsvg.selectAll("*").remove();
                var orgId = myLocalStorage.get('userSelectedOrgId');
                $("#FilterTrend").prop('disabled', 'disabled');  //vaishnavi 02-03-2022
                $("#btnQuickSearch").prop('disabled', 'disabled');  //vaishnavi 02-03-2022
                $("#btnQuickSearch").css('background-color', 'darkgrey'); //vaishnavi 02-03-2022
                if ($("#FilterTrend").prop('checked') == true) {

                    $("#FilterTrend").prop('disabled', 'disabled');   //vaishnavi 02-03-2022
                    $("#btnQuickSearch").prop('disabled', 'disabled');   //vaishnavi 02-03-2022
                    $("#btnQuickSearch").css('background-color', 'darkgrey');   //vaishnavi 02-03-2022
                    $scope.loadWBSData(orgId, null, null, null, null, '0', null);


                } else {
                    $("#FilterTrend").prop('disabled', 'disabled');   //vaishnavi 02-03-2022
                    $("#btnQuickSearch").prop('disabled', 'disabled');   //vaishnavi 02-03-2022
                    $("#btnQuickSearch").css('background-color', 'darkgrey'); //vaishnavi 02-03-2022
                    $scope.loadWBSData(orgId, null, null, null, null, '1', null);


                }

            }

            $scope.loadWBSData = function (orgId, pgmId, pgmEltId, projId, searchText, allData, deptID) {
                $("#btnQuickSearch").prop('disabled', 'disabled');  //vaishnavi 02-03-2022
                $("#FilterTrend").prop('disabled', 'disabled');   //vaishnavi 02-03-2022
                $("#btnQuickSearch").css('background-color', 'darkgrey');   //vaishnavi 02-03-2022
                searchText = myLocalStorage.get('SearchText');
                $scope.SearchText = searchText;
                var pagedata = Page;
                $('#wbsGridView').html('');
                $scope.showSpinner = true;
                $scope.trendSpinner = false;
                $('#spin').addClass('fademe');
                $('#spinTrend').removeClass('fademe');
                //********** Jignesh-18-10-2021 ************
                //var userList = wbsTree.getUserList();
                //var uName = _localStorage.userName;
                var uID = _localStorage.userId;
                //$.each(userList, function (i, data) {
                //    if (data.UserID == uName) {
                //        uID = data.Id; 
                //    }
                //});
                if (searchText == 'undefined' || searchText == null || searchText == '') {
                    searchText = null;
                }
                if (pgmId == "" || pgmId == '') {
                    pgmId = null;
                }
                if (pgmEltId == "" || pgmEltId == '') {
                    pgmEltId = null;
                }
                if (projId == "" || projId == '') {
                    projId = null;
                }
                //*******************************************
                //$http.get(serviceBasePath + "Request/WBS/" + uID + "/" + orgId + "/" + pgmId + "/" + pgmEltId + "/" + projId + "/null/null/null/null/null/" + searchText + "/" + allData,
                WbsService.getWBS(uID, orgId, pgmId, pgmEltId, projId, searchText, allData, deptID).get({})
                    .$promise.then(function (response) {
                        $("#FilterTrend").removeAttr('disabled');   //vaishnavi 02-03-2022
                        usSpinnerService.spin('spinner-1');
                        $("#btnQuickSearch").removeAttr('disabled');   //vaishnavi 02-03-2022

                        //Add loader stop code here
                        usSpinnerService.stop('spinner-1');
                        $scope.showSpinner = false;
                        $('#spin').removeClass('fademe');


                        $("#btnQuickSearch").css('background-color', 'black');   //vaishnavi 02-03-2022
                        $('#wbs-tree').html('');    //vaishnavi 02-03-2022
                        $('#wbsGridView').html('');   //vaishnavi 02-03-2022
                        console.log(response);
                        _wbsTreeData = response;
                        var treeData = response;
                        $scope.getDefaultFirstProjectElement(treeData);
                        $scope.treeData = treeData;
                        // $scope.retrieveProgramList(response);
                        console.log($scope.programList);
                        var isProjectIdSet = false;
                        //$rootScope.fromWBS = true;
                        // Rename the project and project element nodes to include project number and project element number.
                        var organization = response;
                        var str = "<div class='row row-padding'>" +
                            "<div class='gadget color-my' style = 'height: 733px;' >" +
                            //  "<div class='gadget-head' style='display: block'>"+
                            // "<h3 style='margin-top:1px;'>"+
                            //   "   Interface Definition"+
                            //"<small style='color:white;' ng-show='data.length' class='page-title-subtle-white'>[ {{ data.length }}  Records(s) ]</small>"+
                            //   "</h3>"+


                            //   "<input class='wild-card-search pull-right' ng-model='SearchValue'>"+
                            //    "           <span class='pull-right' style='margin-right:5px;font-weight:bold;'>Search:</span>"+

                            // "</div>"+
                            " <div class='gadget-content' style='height: 91%;'>" +
                            "    <table class='table  table-condensed'>  <thead style='position: -webkit-sticky;position: sticky;top:0; '>" +
                            "<tr><th class='th-c sortable' scope='col' style='position: relative; width: 14.28%;' data-sortable='true'>Organization</th>" +
                            "<th class='th-c sortable' scope='col' style='position: relative; width: 14.28%;' data-sortable='true'> Contract</th>" +
                            "<th class='th-c sortable' scope='col' style='position: relative; width: 14.28%;' data-sortable='true'>Project</th>" +
                            "<th class='th-c sortable' scope='col' style='position: relative; width: 14.28%;' data-sortable='true'>Department</th>" +
                            "<th class='th-c sortable' scope='col' style='position: relative; width: 14.28%;' data-sortable='true'>Project Element</th>" +
                            "<th class='th-c sortable' scope='col' style='position: relative; width: 14.28%;' data-sortable='true'>Service</th>" +
                            "<th class='th-c sortable' scope='col' style='position: relative; width: 14.28%;' data-sortable='true'>Value</th>" +
                            "</tr>  <thead><tfoot></tfoot> " +
                            " <tbody>" +
                            "<tr style = 'background-color:white !important;'>" +
                            "    <td colspan='7'>" +
                            "       <div class='scrolable-table' style='overflow:auto;overflow-x:hidden;height: 100% !important;'>" +
                            "          <table id='gridTable'; class='table striped table-condensed table-responsive table-bordered'>" +
                            "             <tbody>";
                        var currentProgramCost, currentProjectCost, currentProjectElementCost;

                        for (programI = 0; programI < organization.children.length; programI++) {
                            var CalCulateProgramCost = 0;
                            //Nivedita
                            var program = organization.children[programI];
                            if (program.CurrentCost == null || program.CurrentCost == "" || program.CurrentCost == 0 || isNaN(program.CurrentCost)) {
                                currentProgramCost = "";
                            } else {
                                for (var i = 0; i < program.children.length; i++) {
                                    var CalCulateProjectCost = 0;
                                    var project = program.children[i];
                                    for (var j = 0; j < project.children.length; j++) {
                                        var projectElement = project.children[j];
                                        CalCulateProjectCost += Number(projectElement.CurrentCost);
                                        // CalCulateProgramCost += Number(projectElement.CurrentCost);
                                        //alert(program.ProgramID + 'Cost=' + CalCulateProgramCost);
                                        //CalCulateProjectCost += CalCulateProgramCost;
                                        //alert(CalCulateProjectCost);
                                    }
                                    CalCulateProgramCost += Number(CalCulateProjectCost);
                                    if (CalCulateProjectCost == 0) {
                                        currentProjectCost = "";
                                        project.CurrentCost = 0;
                                    }
                                    else {
                                        project.CurrentCost = CalCulateProjectCost;
                                    }

                                }
                                if (CalCulateProgramCost == 0) {
                                    currentProgramCost = "";
                                    program.CurrentCost = 0;
                                }
                                else {
                                    currentProgramCost = CalCulateProgramCost;
                                    program.CurrentCost = CalCulateProgramCost;
                                    currentProgramCost = "$" + addCommas(currentProgramCost);
                                }

                            }

                            console.log("Check program details===>");
                            console.log(program);

                            if (program.children.length == 0) {
                                str +=
                                    "<tr class='fade-selection-animation'> " +
                                    "<td class='my-word-wrap orgcell' style='width: 14.28%;'><a level=" + organization.level + " OrganizationId=" + organization.organizationID + ">" + organization.name + "</a></td>" +
                                    "<td class='my-word-wrap' style='width: 14.28%;'><a level=" + program.level + " ProgramId=" + program.ProgramID + " title=" + currentProgramCost + ">" + program.name + "</a></td>" +
                                    "<td class='my-word-wrap' style='width: 14.28%;'></td>" +
                                    "<td class='my-word-wrap' style='width: 14.28%;'></td>" +
                                    "<td class='my-word-wrap' style='width: 14.28%;'></td>" +
                                    "<td class='my-word-wrap' style='width: 14.28%;'></td>" +
                                    "<td class='my-word-wrap' style='width: 14.28%;'></td></tr >";
                            }
                            for (projectI = 0; projectI < program.children.length; projectI++) {
                                var project = program.children[projectI];
                                if (project.CurrentCost == null || project.CurrentCost == "" || project.CurrentCost == 0 || isNaN(project.CurrentCost)) {
                                    currentProjectCost = "";
                                } else {
                                    currentProjectCost = "$" + addCommas(project.CurrentCost);
                                }




                                //organization --- contract -- project

                                console.log("Project Details====>");
                                console.log(project);
                                if (project.children.length == 0) {
                                    var projectClassName = '';
                                    // ProjectClass.get({}, function (Projectresponse) {
                                    //  console.log(Projectresponse);
                                    var projectClassList = wbsTree.getProjectClassList();
                                    for (var x = 0; x < projectClassList.length; x++) {
                                        if (projectClassList[x].ProjectClassID == project.ProjectClassID) {
                                            projectClassName = projectClassList[x].ProjectClassName
                                        }
                                    }
                                    //});
                                    str += "<tr class='fade-selection-animation'>" +
                                        "<td class='my-word-wrap orgcell' style='width: 14.28%;'><a level=" + organization.level + "  OrganizationId=" + organization.organizationID + ">" + organization.name + "</a></td>" +
                                        "<td class='my-word-wrap' style='width: 14.28%;'><a level=" + program.level + " ProgramId=" + program.ProgramID + " title=" + currentProgramCost + ">" + program.name + "</a></td>" +
                                        "<td class='my-word-wrap' style='width: 14.28%;'><a level=" + project.level + " ProgramelementId=" + project.ProgramElementID + " title=" + currentProjectCost + ">" + project.name + "</a></td>" +
                                        "<td class='my-word-wrap' style='width: 14.28%;' ProjectClassId=" + project.ProgramElementID + ">" + project.ProjectClassName + "</td>" +
                                        "<td class='my-word-wrap' style='width: 14.28%;'></td>" +
                                        "<td class='my-word-wrap' style='width: 14.28%;' ></td>" +
                                        "<td class='my-word-wrap' style='width: 14.28%;'></td></tr>";
                                }
                                project.name = project.ProjectNumber + ". " + project.name;
                                for (projectElementI = 0; projectElementI < project.children.length; projectElementI++) {
                                    var projectElement = project.children[projectElementI];
                                    if (projectElement.CurrentCost == null || projectElement.CurrentCost == "" || projectElement.CurrentCost == 0 || isNaN(projectElement.CurrentCost)) {
                                        currentProjectElementCost = "";
                                    } else {
                                        currentProjectElementCost = "$" + addCommas(projectElement.CurrentCost);
                                    }


                                    console.log("Project Element Details====>");
                                    console.log(projectElement);
                                    var projectServiceName = '';
                                    // ServiceClass.get({}, function (response) {
                                    //  console.log(response);

                                    // var serviceClassList = response.result;
                                    var serviceClassList = wbsTree.getServiceClassList();

                                    for (var x = 0; x < serviceClassList.length; x++) {
                                        if (serviceClassList[x].ID == projectElement.ProjectClassID) {
                                            projectServiceName = serviceClassList[x].Description
                                        }
                                    }


                                    // });
                                    str += "<tr class='fade-selection-animation'>" +
                                        "<td class='my-word-wrap orgcell' style='width: 14.28%;'><a level=" + organization.level + "  OrganizationId=" + organization.organizationID + ">" + organization.name + "</a></td>" +
                                        "<td class='my-word-wrap' style='width: 14%;'><a level=" + program.level + " ProgramId=" + program.ProgramID + " title=" + currentProgramCost + ">" + program.name + "</a></td>" +
                                        "<td class='my-word-wrap' style='width: 14.50%;'><a level=" + project.level + " ProgramelementId=" + project.ProgramElementID + " title=" + currentProjectCost + ">" + project.name + "</a></td>" +
                                        "<td class='my-word-wrap' style='width: 14.50%;' ProjectClassId=" + project.ProgramElementID + ">" + project.ProjectClassName + "</td>" +
                                        "<td class='my-word-wrap' style='width: 14.60%;'><a level=" + projectElement.level + " ProjectId=" + projectElement.ProjectID + " title=" + currentProjectElementCost + ">" + projectElement.name + "</a></td>" +
                                        "<td class='my-word-wrap' style='width: 14.40%;'>" + projectElement.ServiceName + "</td>" +
                                        "<td class='my-word-wrap' style='width: 13.94%;'>" + currentProjectElementCost + "</td></tr>";


                                    projectElement.name = projectElement.ProjectElementNumber + ". " + projectElement.name;
                                    if (!isProjectIdSet) {

                                        if (projectElementI == 0) {

                                            wbsTree.setSelectedProjectID(project.children[projectElementI].ProjectID);
                                            isProjectIdSet = true;
                                            //$rootScope.fromWBS = false;
                                            wbsTree.getWBSTrendTree().trendGraph(true);

                                        }

                                    }


                                }

                            }
                        }

                        /*var columns = ["Customer", "Rev Code", "Date Submitted",
                            "Industry", "Customer Size", "# of Lanes",
                            "Total Volumne", "Result"]

                        var data = [
                            { "date": "2013-01-01", "close": 45 },
                            { "date": "2013-02-01", "close": 50 },
                            { "date": "2013-03-01", "close": 55 },
                            { "date": "2013-04-01", "close": 50 },
                            { "date": "2013-05-01", "close": 45 },
                            { "date": "2013-06-01", "close": 50 },
                            { "date": "2013-07-01", "close": 50 },
                            { "date": "2013-08-01", "close": 55 }
                        ];*/

                        //var table = tabulate(data, columns);

                        //function tabulate(data, columns) {
                        //    const keys = Object.keys(data[0]);
                        //    var table = d3.select("#tree-container").select("#wbsGridView").append("table"), 
                        //        thead = table.append("thead"),
                        //        tbody = table.append("tbody");

                        //    // append the header row
                        //    thead.append("tr")
                        //        .selectAll("th")
                        //        .data(keys)
                        //        .enter()
                        //        .append("th")
                        //        .text(function (column) { return column; });

                        //    // create a row for each object in the data
                        //    var rows = tbody.selectAll("tr")
                        //        .data(data)
                        //        .enter()
                        //        .append("tr");

                        //    // create a cell in each row for each column
                        //    var cells = rows.selectAll("td")
                        //        .data(d => Object.values(d))
                        //        .enter()
                        //        .append("td")
                        //        .text(d => d);

                        //    return table;
                        //}

                        str += "</tbody></table ></div ></td ></tr > </tbody ></table ></div></div></div>";
                        $('#wbsGridView').append(str);

                        var column1 = $('#wbsGridView Table td:first-child');
                        var column2 = $('#wbsGridView Table td:nth-child(2)');
                        var column3 = $('#wbsGridView Table td:nth-child(3)');
                        var column4 = $('#wbsGridView Table td:nth-child(4)');

                        //modifyTableFirstColumnRowspan(column1, column2);

                        modifyTableRowspan(column1);
                        //modifyTableRowspan(column2);
                        //  modifyTableRowspan(column3);
                        // modifyTableRowspan(column4);

                        function modifyTableRowspan(column) {
                            
                            var topMatchTd;
                            var previousValue = "";
                            var rowSpan = 1;
                            column.each(function () {
                                if ($(this).text() != "") {

                                    if ($(this).text() == previousValue) {
                                        rowSpan++;
                                        $(topMatchTd).attr('rowspan', rowSpan);
                                        $(this).remove();
                                    }
                                    else {
                                        topMatchTd = $(this);
                                        rowSpan = 1;
                                    }

                                }


                                previousValue = $(this).text();
                            });

                        }

                        //function modifyTableRowspan(column) {

                        //    var prevText = "";
                        //    var counter = 0;

                        //    column.each(function (index) {


                        //        var textValue = $(this).text();

                        //        if (index === 0) {
                        //            prevText = textValue;
                        //        }
                        //        if (textValue == "" && prevText == "") {
                        //            prevText = "@@@";
                        //        }
                        //        if (textValue !== prevText || index === column.length - 1) {

                        //            var first = index - counter;

                        //            if (index === column.length - 1) {
                        //                counter = counter + 1;
                        //            }

                        //            column.eq(first).attr('rowspan', counter);


                        //            if (index === column.length - 1) {
                        //                for (var j = index; j > first; j--) {
                        //                    column.eq(j).remove();
                        //                }
                        //            }

                        //            else {

                        //                for (var i = index - 1; i > first; i--) {
                        //                    column.eq(i).remove();
                        //                }
                        //            }

                        //            prevText = textValue;
                        //            counter = 0;
                        //        }

                        //        counter++;

                        //    });

                        //}

                        //function modifyTableFirstColumnRowspan(column1, column2) {

                        //    var prevText = "";
                        //    var counter = 0;

                        //    column2.each(function (index) {


                        //        var textValue = $(this).text();

                        //        if (index === 0) {
                        //            prevText = textValue;
                        //        }
                        //        if (textValue == "" && prevText == "") {
                        //            prevText = "@@@";
                        //        }
                        //        if (textValue !== prevText || index === column2.length - 1) {

                        //            var first = index - counter;

                        //            if (index === column2.length - 1) {
                        //                counter = counter + 1;
                        //            }

                        //            column1.eq(first).attr('rowspan', counter);


                        //            if (index === column2.length - 1) {
                        //                for (var j = index; j > first; j--) {
                        //                    column1.eq(j).remove();
                        //                }
                        //            }

                        //            else {

                        //                for (var i = index - 1; i > first; i--) {
                        //                    column1.eq(i).remove();
                        //                }
                        //            }

                        //            prevText = textValue;
                        //            counter = 0;
                        //        }

                        //        counter++;

                        //    });

                        //}
                        function modifyTableFirstColumnRowspan(column1, column2) {

                            var topMatchTd;
                            var previousValue = "";
                            var rowSpan = 1;
                            column2.each(function (index) {

                                if ($(this).text() == previousValue) {
                                    rowSpan++;
                                    $(topMatchTd).attr('rowspan', rowSpan);
                                    column1[index].remove();
                                }
                                else {
                                    topMatchTd = column1[index];
                                    rowSpan = 1;
                                }

                                previousValue = $(this).text();
                            });
                        }




                        //==================Manasi 27-03-2021==================================
                        //if ($rootScope.fromWBS == true) {
                        //    //alert('hi');
                        //    wbsTree.setSelectedProjectID($rootScope.ProjectID);
                        //    $rootScope.fromWBS = false;

                        //    wbsTree.getWBSTrendTree().trendGraph(true);

                        //}
                        //==================Manasi 27-03-2021==================================

                        s = treeData;


                        // Calculate total nodes, max label length
                        var totalNodes = 0;
                        var maxLabelLength = 0;
                        // variables for drag/drop

                        var draggingNode = null;
                        // panning variables
                        var panSpeed = 200;
                        var panBoundary = 20; // Within 20px from edges will pan when dragging.

                        //size of the diagram
                        var viewerWidth = $("#program_navigation").width();
                        var viewerHeight = $("#program_navigation").height();
                        wbsTree.setViewerWidth(viewerWidth);
                        wbsTree.setViewerHeight(viewerHeight);

                        var modal;
                        var modal_mode;

                        tree = d3.layout.tree().size([viewerHeight, viewerWidth]);
                        wbsTree.setTree(tree);


                        // define a d3 diagonal projection for use by the node paths later on.
                        diagonal = d3.svg.diagonal()
                            .projection(function (d) {
                                return [d.y, d.x];
                            });
                        wbsTree.setDiagonal(diagonal);


                        // Call visit function to establish maxLabelLength
                        visit(treeData, function (d) {
                            totalNodes++;
                            //maxLabelLength = Math.max(d.name.length, maxLabelLength);
                            maxLabelLength = 25;

                        }, function (d) {
                            return d.children && d.children.length > 0 ? d.children : null;
                        });

                        var drag = d3.behavior.drag();
                        // define the zoomListener which calls the zoom function on the "zoom" event constrained within the scaleExtents
                        zoomListener = d3.behavior.zoom().scaleExtent([0.1, 3]).on("zoom", zoom);
                        // define the baseSvg, attaching a class for styling and the zoomListener
                        baseSvg = d3.select("#tree-container").select("svg")
                            .attr("width", viewerWidth)
                            .attr("height", viewerHeight)
                            .attr("class", "overlay")
                            .call(zoomListener)
                            //.on(".drag", null) //disabled drag event
                            .on("dblclick.zoom", function () {
                                //alert();
                            });

                        // Append a group which holds all nodes and which the zoom Listener can act upon.
                        svgGroup = baseSvg.append("g")
                            .on("click", function () {
                                //d3.select("#toolTip").style("opacity", 0);
                            });

                        var slider = d3.select("#tree-container").select("div").select("p").append("input")
                            .datum({})
                            .attr("type", "range")
                            .attr("value", 1)
                            .attr("min", zoomListener.scaleExtent()[0])
                            .attr("max", zoomListener.scaleExtent()[1])
                            .attr("step", (zoomListener.scaleExtent()[1] - zoomListener.scaleExtent()[0]) / 100)
                            .on("input", slided);
                        wbsTree.renderTreeGrid(treeData, svgGroup, zoomListener, $scope);
                        wbsTree.bindEventsForWBSNodes($scope);
                        //=====================Manasi 27-03-2021===========================================
                        //var positionX = wbsTree.getNodeXPosition();
                        //var positionY = wbsTree.getNodeYPosition();
                        ////var div = document.getElementById('wbs-tree');
                        ////div.scroll(positionX, positionY); 
                        ////div.scrollLeft(positionX);
                        ////div.scrollTop(positionY);
                        ////angular.element("#wbs-tree")[0].scrollTop = positionY;
                        //$("#mindmap").scrollTop(positionX);
                        //================================================================================

                        //select node 
                        if ($rootScope.fromWBS == true) {
                            setUserNode();
                            $rootScope.fromWBS = false;
                        }
                    }).
                    finally(function () {
                        console.log($rootScope);
                        // $rootScope.buttonDisabled =false;
                        //usSpinnerService.stop('spinner-1');
                        //$scope.showSpinner = false;
                        //$('#spin').removeClass('fademe');
                    })
            }

            function setUserNode() {
                /*IK 04/01/2021 */
                var selNode = null;
                $("#mindmap").scrollTop(0);
                $('#wbs-tree').find('.node').each(function () {
                    var tspan = $(this).find('tspan');
                    var s = '';
                    $.each($(tspan), function () {
                        s = s + $(this).html() + " ";
                    });
                    if (s.toString().replace(/\s*\(.*?\)\s*/g, '').trim()
                        === localStorage.getItem('selectProjectNameDash').toString().trim()) {
                        selNode = this;
                    }
                });

                if (selNode != null) {
                    //After the wbstree is rendered, need to wait for the transform to be finished as well.
                    //The transform method is being called somewhere else, put a time delay here for the temp fix
                    $timeout(function () {
                        var yPosition = d3.select(selNode).attr("transform").split(/[()]/)[1].split(',')[1];
                        if (!checkInView(selNode, false)) //only scroll if node is not in view
                            $("#mindmap").scrollTop(yPosition - 30);
                        $(selNode).d3Click();
                    }, 500);
                }
                else {
                    //Select first project by default
                    $('#wbs-tree').find('.node').each(function () {
                        var tspan = $(this).find('tspan');
                        var s = '';
                        $.each($(tspan), function () {
                            s = s + $(this).html() + " ";
                        });
                        if (s.toString().replace(/\s*\(.*?\)\s*/g, '').trim()
                            === $scope.defaultProjectElement.toString().trim()) {
                            selNode = this;
                            // alert();
                        }
                    });
                    if (selNode != null) {
                        $timeout(function () {
                            var yPosition = d3.select(selNode).attr("transform").split(/[()]/)[1].split(',')[1];
                            if (!checkInView(selNode, false)) //only scroll if node is not in view
                                $("#mindmap").scrollTop(yPosition - 30);
                            $(selNode).d3Click();
                        }, 500);
                    } else {
                        var firstGNode = $('#trendSvg').children()[0];
                        $(firstGNode).children().remove();
                    }
                }
            }

            //Check if the node is in view
            function checkInView(elem, partial) {
                var container = $("#mindmap");
                var contHeight = container.height();
                var contTop = container.scrollTop();
                var contBottom = contTop + contHeight;

                var elemTop = $(elem).offset().top - container.offset().top;
                var elemBottom = elemTop + $(elem).height();

                var isTotal = (elemTop >= 0 && elemBottom <= contHeight);
                var isPart = ((elemTop < 0 && elemBottom > 0) || (elemTop > 0 && elemTop <= container.height())) && partial;

                return isTotal || isPart;
            }
            var visit = function (parent, visitFn, childrenFn) {
                if (!parent) return;

                visitFn(parent);

                var children = childrenFn(parent);
                if (children) {
                    var count = children.length;
                    for (var i = 0; i < count; i++) {
                        visit(children[i], visitFn, childrenFn);
                    }
                }
            }

            // sort the tree according to the node names
            var sortTree = function () {
                tree.sort(function (a, b) {
                    return b.name.toLowerCase() < a.name.toLowerCase() ? 1 : -1;
                });
            }

            // Sort the tree initially incase the JSON isn't in a sorted order.
            //sortTree();

            // //Manasi 26-03-2021
            var currentTranslate = d3.event != null ? d3.event.translate[0] : 0;//get current zoom level

            var zoom = function () {
                //disabled zooming for main tree view
                //svgGroup.attr("transform", "translate(" + d3.event.translate + ")scale(" + d3.event.scale + ")");
                //slider.property("value", d3.event.scale);
                //var translate1 = 0;
                var scale = d3.event.scale;
                var translate = d3.event.translate;
                //console.log('Scale: ' + scale);
                //console.log('translate: ' + translate);
                var scrolledPosition = $("#mindmap").scrollTop();
                //console.log("Scrolled Position " + scrolledPosition);
                //console.log(d3.event.sourceEvent.wheelDelta);
                //console.log('Wheel data' + d3.event.wheelDelta);
                //console.log("zoom");

                //Wierd behavior in chrome - Doulbe right click triggers the zoom event
                //do not scroll if not a mouse scroll
                if (!d3.event.sourceEvent.wheelDelta)
                    return;
                if (d3.event.sourceEvent.wheelDelta < 0) {
                    //Scroll Down
                    scrolledPosition += 100;
                } else {
                    scrolledPosition -= 100;
                }
                $("#mindmap").scrollTop(scrolledPosition);
                // scrolledPosition = $("#mindmap").scrollTop();
                currentTranslate = translate[0];
            }


            var slided = function (d) {
                zoomListener.scale(d3.select(this).property("value"))
                    .event(baseSvg);
            }

            var isOrgExisted = function (orgID, orgList) {

                var isExist = false;
                angular.forEach(orgList, function (item) {


                    if (item.OrganizationID == orgID) {
                        isExist = true;
                    }
                });

                return isExist;
            }


            $scope.loadOrganizations = function () {
                Organization.lookup().get({}, function (organizationData) {
                    $scope.organizationList = organizationData.result;

                    if ($scope.organizationList.length == 0) {
                        //if (localStorageService.get("authorizationData").role == "Admin") {
                        if (localStorageService.get("authorizationData").role.indexOf('Admin') != -1) {
                            window.location.hash = '#/app/admin-organization';
                        }
                    }
                    console.debug("ORganization data", organizationData);
                    if ($scope.organizationList.length == 0) {
                        return;
                    }
                    var orgId = myLocalStorage.get('userSelectedOrgId');
                    console.debug("ORGID", orgId);
                    wbsTree.setOrganizationList($scope.organizationList);

                    console.log($scope.organizationList);
                    if (orgId == null) {
                        orgId = $scope.organizationList[0].OrganizationID;
                        myLocalStorage.set('userSelectedOrgId', orgId);
                        $scope.filterOrgId = myLocalStorage.get('userSelectedOrgId');
                    } else {
                        $('#closed,#approved,#unapproved,#contract,#project').show();
                        var checkIfExisted = isOrgExisted(orgId, $scope.organizationList);
                        if (checkIfExisted == false)
                            orgId = $scope.organizationList[0].OrganizationID;
                        var s = $("#selectOrg").val(orgId);
                        $scope.filterOrgId = orgId;
                        myLocalStorage.set('userSelectedOrgId', orgId);
                        window.setTimeout(function () {
                            $("#selectOrg option[value='" + orgId + "']").attr('selected', true);
                            wbsTree.getProjectMap().initProjectMap(wbsTree.getSelectedNode(), wbsTree.getOrganizationList());
                        }, 500);
                    }

                    Program.lookup().get({ OrganizationID: orgId }, function (programData) {
                        $scope.programList = programData.result;
                        $scope.programList.sort(function (a, b) {
                            return a.ProgramName.localeCompare(b.ProgramName);
                        });
                    });

                    console.log($scope.programList);
                    $http.get(serviceBasePath + "Request/ProjectByOid/" + orgId)
                        .then(function (response) {

                            var projectNameOnLoad = null;
                            projectNameOnLoad = (localStorage.getItem('selectProjectNameDash'))
                                ? localStorage.getItem('selectProjectNameDash')
                                : response.data.result[0].ProjectElementNumber + ". " + response.data.result[0].ProjectName;
                            //if (localStorage.getItem("selectProjectIdDash") === null) {
                            localStorage.setItem('selectProjectIdDash', response.data.result[0].ProjectID);
                            localStorage.setItem('selectProjectNameDash', projectNameOnLoad);

                            //For App Sec
                            myLocalStorage.set('selectProjectDataDash', response.data.result[0]);
                            localStorage.setItem('selectProjectProjectManagerIDDash', response.data.result[0].ProjectManagerID);
                            localStorage.setItem('selectProjectDirectorIDDash', response.data.result[0].DirectorID);
                            localStorage.setItem('selectProjectSchedulerIDDash', response.data.result[0].SchedulerID);
                            localStorage.setItem('selectProjectVicePresidentIDDash', response.data.result[0].VicePresidentID);
                            localStorage.setItem('selectProjectFinancialAnalystIDDash', response.data.result[0].FinancialAnalystID);
                            localStorage.setItem('selectProjectCapitalProjectAssistantIDDash', response.data.result[0].CapitalProjectAssistantID);
                            //dhtmlx('at $scope.loadOrganizations $http.get orgid' + orgId);
                            //}

                        });

                    // $scope.loadWBSData(orgId, localStorage.getItem('pgmId'), null, null);
                    if (localStorage.getItem('projId')) {
                        $scope.filterOrgId = ($scope.organizationList.find(elem => elem.OrganizationID == (localStorage.getItem('userSelectedOrgId')))).OrganizationID;
                        Program.lookup().get({ OrganizationID: $scope.filterOrgId }, function (programData) {
                            $scope.programList = programData.result;
                            var testobj = ($scope.programList.find(elem => elem.ProgramID == parseInt(localStorage.getItem('pgmId'))));
                            $scope.filterProgramId = (testobj.ProgramID).toString();
                            console.log('Prgm:' + $scope.filterProgramId);
                            ProgramElement.lookup().get({ ProgramID: $scope.filterProgramId }, function (programElementData) {
                                $scope.programElementList = programElementData.result;
                                testobj = ($scope.programElementList.find(elem => elem.ProgramElementID == parseInt(localStorage.getItem('pgmEltId'))));
                                $scope.filterProgramElement = (testobj.ProgramElementID).toString();
                                Project.lookup().get({ ProgramID: $scope.filterProgramId, ProgramElementID: $scope.filterProgramElement }, function (projectData) {
                                    $scope.projectList = projectData.result;
                                    testobj = ($scope.projectList.find(elem => elem.ProjectID == parseInt(localStorage.getItem('projId'))));
                                    console.log('TESTPROJ NEW VALUE');
                                    console.log(testobj);
                                    $scope.filterProject = (testobj.ProjectID).toString();
                                    console.log('ProjectId--' + $scope.filterProject);
                                    $scope.loadWBSData(orgId, $scope.filterProgramId, $scope.filterProgramElement, $scope.filterProject, null, '1', null);
                                });
                            });
                        });
                    }
                    else if (localStorage.getItem('pgmEltId')) {
                        $scope.filterOrgId = ($scope.organizationList.find(elem => elem.OrganizationID == (localStorage.getItem('userSelectedOrgId')))).OrganizationID;
                        Program.lookup().get({ OrganizationID: $scope.filterOrgId }, function (programData) {
                            $scope.programList = programData.result;
                            var testobj = ($scope.programList.find(elem => elem.ProgramID == parseInt(localStorage.getItem('pgmId'))));
                            $scope.filterProgramId = (testobj.ProgramID).toString();
                            console.log('Prgm:' + $scope.filterProgramId);
                            ProgramElement.lookup().get({ ProgramID: $scope.filterProgramId }, function (programElementData) {
                                $scope.programElementList = programElementData.result;
                                testobj = ($scope.programElementList.find(elem => elem.ProgramElementID == parseInt(localStorage.getItem('pgmEltId'))));
                                console.log('TESTPROJ NEW VALUE');
                                console.log(testobj);
                                $scope.filterProgramElement = (testobj.ProgramElementID).toString();
                                console.log('PrgmElmnt:' + $scope.filterProgramElement);
                                $scope.loadWBSData(orgId, $scope.filterProgramId, $scope.filterProgramElement, null, null, '1', null);
                                Project.lookup().get({ ProgramID: $scope.filterProgramId, ProgramElementID: $scope.filterProgramElement }, function (projectData) {
                                    $scope.projectList = projectData.result;
                                });
                            });
                        });
                    }

                    else if (localStorage.getItem('pgmId')) {
                        $scope.filterOrgId = ($scope.organizationList.find(elem => elem.OrganizationID == (localStorage.getItem('userSelectedOrgId')))).OrganizationID;
                        Program.lookup().get({ OrganizationID: $scope.filterOrgId }, function (programData) {
                            $scope.programList = programData.result;
                            var testobj = ($scope.programList.find(elem => elem.ProgramID == parseInt(localStorage.getItem('pgmId'))));
                            $scope.filterProgramId = (testobj.ProgramID).toString();
                            $scope.loadWBSData(orgId, $scope.filterProgramId, null, null, null, '1', null);
                            ProgramElement.lookup().get({ ProgramID: $scope.filterProgramId }, function (programElementData) {
                                $scope.programElementList = programElementData.result;
                                $("#selectProgramElement").val("");
                            });

                        });
                    }

                    else
                        $scope.loadWBSData(orgId, null, null, null, null, '1', null);

                });

            }

            $scope.editSelectedOrganization = function () {
                var scope = $rootScope.$new();
                var newOrEdit = "edit";
                var orgId = $("#selectOrg").val();

                for (var i = 0; i < $scope.organizationList.length; i++) {
                    if ($scope.organizationList[i].OrganizationID == parseInt(orgId)) {
                        $scope.selectedOrg = $scope.organizationList[i];
                        break;
                    }
                }

                scope.params = { selectedOrg: $scope.selectedOrg, newOrEdit: newOrEdit };
                $rootScope.modalInstance = $uibModal.open({
                    scope: scope,
                    backdrop: 'static',
                    keyboard: false,

                    templateUrl: 'app/views/modal/organization_modal.html',
                    controller: 'OrganizationModalCtrl',
                    size: 'lg'
                });
                console.log($rootScope.modalInstance);
                // after close the popup modal
                $rootScope.modalInstance.result.then(function (data) {

                    if (data.isCancel == true) {
                        return;
                    }
                    Organization.lookup().get({}, function (organizationData) {
                        //rename the node when renaming from wbstree
                        console.log(organizationData);
                        console.log($scope.selectedOrg);
                        var id = 0;
                        var newNode;
                        var temp = wbsTree.getSelectedNode();
                        var orgId = myLocalStorage.get('userSelectedOrg');

                        angular.forEach(organizationData.result, function (item) {
                            if (item.OrganizationID == $scope.selectedOrg.OrganizationID) {
                                console.log(item);
                                temp.name = item.OrganizationName;
                                $scope.selectedOrg.OrganizationName = item.OrganizationName;
                                //$scope.selectedOrg = item;
                                $scope.selectedOrg.LatLong = item.LatLong;
                                id = $scope.selectedOrg.OrganizationID;
                            }
                        })
                        for (var i = 0; i < $scope.organizationList.length; i++) {
                            if ($scope.organizationList[i].OrganizationID == $scope.selectedOrg.OrganizationID) {

                                $scope.organizationList[i] = $scope.selectedOrg;
                            }
                        }

                        id = Number(id);
                        console.log(id);
                        //$scope.organizationList = organizationData.result;
                        wbsTree.setOrganizationList($scope.organizationList);
                        $("#selectOrg").val(id);
                        //  myLocalStorage.set('userSelectedOrgId',organizationData.result[0].OrganizationID);
                        wbsTree.updateTreeNodes(temp);
                        //$scope.filterOrg = id;

                        //$scope.filterOrg = id;
                        console.log($scope.filterOrg);
                        if ($("#project_name").is(":visible") == false) {

                            if (typeof selectedNode == 'undefined')
                                selectedNode = null;
                            console.log(selectedNode);

                            wbsTree.getProjectMap().initProjectMap(selectedNode, wbsTree.getOrganizationList());
                        }

                    });
                    //$scope.selectedOrg=null;

                });
            }

            $scope.filterChangeOrg = function () {
                console.log('Called Org...');
                $scope.filterProgramId = "";
                $scope.filterProgramElement = "";
                $scope.filterProject = "";

                $scope.programElementList = "";
                $scope.projectList = "";

                localStorage.removeItem('orgId');
                localStorage.removeItem('pgmId')
                localStorage.removeItem('pgmEltId');
                localStorage.removeItem('projId');
                localStorage.removeItem('projId');
                localStorage.removeItem('SearchText');

                var orgId = $("#selectOrg").val();
                console.log(orgId);
                if (orgId != null && orgId != "") {
                    wbsTree.setSelectedOrganizationID(orgId);
                    Program.lookup().get({ OrganizationID: orgId }, function (programData) {
                        console.log(programData.result);
                        $scope.programList = programData.result;
                    });
                }
                else {
                    orgId = $scope.organizationList[0].OrganizationID;
                    wbsTree.setSelectedOrganizationID(orgId);
                }

                myLocalStorage.set('userSelectedOrgId', orgId);

                var oldsvg = d3.select("#wbs-tree");
                oldsvg.selectAll("*").remove();

                $("#project_name").html("");
                //  $("#project_name").hide();
                wbsTree.setSelectedNode(null);
                $http.get(serviceBasePath + "Request//ProjectByOid/" + orgId)
                    .then(function (response) {
                        if (response.data.result.length > 0) {
                            var projectNameOnLoad = null;
                            projectNameOnLoad = (localStorage.getItem('selectProjectNameDash'))
                                ? localStorage.getItem('selectProjectNameDash')
                                : response.data.result[0].ProjectElementNumber + ". " + response.data.result[0].ProjectName;
                            localStorage.setItem('selectProjectIdDash', response.data.result[0].ProjectID);
                            localStorage.setItem('selectProjectNameDash', projectNameOnLoad);

                            //For App Sec
                            myLocalStorage.set('selectProjectDataDash', response.data.result[0]);
                            localStorage.setItem('selectProjectProjectManagerIDDash', response.data.result[0].ProjectManagerID);
                            localStorage.setItem('selectProjectDirectorIDDash', response.data.result[0].DirectorID);
                            localStorage.setItem('selectProjectSchedulerIDDash', response.data.result[0].SchedulerID);
                            localStorage.setItem('selectProjectVicePresidentIDDash', response.data.result[0].VicePresidentID);
                            localStorage.setItem('selectProjectFinancialAnalystIDDash', response.data.result[0].FinancialAnalystID);
                            localStorage.setItem('selectProjectCapitalProjectAssistantIDDash', response.data.result[0].CapitalProjectAssistantID);
                            //dhtmlx('at #project_name $http.get orgId:' + orgId);
                        }
                        else {
                            localStorage.setItem('selectProjectIdDash', null);
                            localStorage.setItem('selectProjectNameDash', "");

                            //For App Sec
                            myLocalStorage.set('selectProjectDataDash', null);
                            localStorage.setItem('selectProjectProjectManagerIDDash', null);
                            localStorage.setItem('selectProjectDirectorIDDash', null);
                            localStorage.setItem('selectProjectSchedulerIDDash', null);
                            localStorage.setItem('selectProjectVicePresidentIDDash', null);
                            localStorage.setItem('selectProjectFinancialAnalystIDDash', null);
                            localStorage.setItem('selectProjectCapitalProjectAssistantIDDash', null);
                            //dhtmlx('at #project_name  $http.get orgId:' + orgId + ' else...');
                        }

                    });
                var loadfunc = $scope.loadWBSData(orgId, null, null, null, null, '1', null);

            }

            $scope.filterChangeProgram = function () {
                console.log('Called program');
                $scope.filterProgramElement = "";
                $scope.filterProject = "";
                var pgmId = $("#selectProgram").val();
                var orgId = $("#selectOrg").val();
                //var pgmId = $("#selectProgram").val();

                $scope.programElementList = "";
                $scope.projectList = "";

                localStorage.removeItem('pgmId')
                localStorage.removeItem('pgmEltId');
                localStorage.removeItem('projId');
                localStorage.removeItem('SearchText');

                if (orgId != null && pgmId != "") {
                    localStorage.setItem('pgmId', pgmId);
                    ProgramElement.lookup().get({ ProgramID: pgmId }, function (programElementData) {
                        $scope.programElementList = programElementData.result;
                        $scope.programElementList.sort(function (a, b) {    //vaishnavi
                            return a.ProgramElementName.localeCompare(b.ProgramElementName);  //vaishnavi
                        }); //vaishnavi
                        $("#selectProgramElement").val("");
                    });

                    debugger;

                    ProjectClassByProgramId.get({ programID: pgmId }, function (response) {
                        debugger;
                        var data = response;
                        $scope.projectClassListDD = response.result;
                        //$("#selectProgramElement").val("");
                    });
                }
                console.log($scope.programElementList);
                var treedataaaa = _wbsTreeData;
                var oldsvg = d3.select("#wbs-tree");
                oldsvg.selectAll("*").remove();
                console.log(pgmId);
                if (pgmId != "") {
                    $http.get(serviceBasePath + "Request/Project/" + pgmId)
                        .then(function (response) {
                            if (response.data.result.length > 0) {
                                var projectNameOnLoad = null;
                                projectNameOnLoad = (localStorage.getItem('selectProjectNameDash'))
                                    ? localStorage.getItem('selectProjectNameDash')
                                    : response.data.result[0].ProjectElementNumber + ". " + response.data.result[0].ProjectName;
                                localStorage.setItem('selectProjectIdDash', response.data.result[0].ProjectID);
                                localStorage.setItem('selectProjectNameDash', projectNameOnLoad);

                                //For App Sec
                                myLocalStorage.set('selectProjectDataDash', response.data.result[0]);
                                localStorage.setItem('selectProjectProjectManagerIDDash', response.data.result[0].ProjectManagerID);
                                localStorage.setItem('selectProjectDirectorIDDash', response.data.result[0].DirectorID);
                                localStorage.setItem('selectProjectSchedulerIDDash', response.data.result[0].SchedulerID);
                                localStorage.setItem('selectProjectVicePresidentIDDash', response.data.result[0].VicePresidentID);
                                localStorage.setItem('selectProjectFinancialAnalystIDDash', response.data.result[0].FinancialAnalystID);
                                localStorage.setItem('selectProjectCapitalProjectAssistantIDDash', response.data.result[0].CapitalProjectAssistantID);
                                //dhtmlx('at $scope.filterChangeProgram $http.get pgmId:' + pgmId);
                            }
                            else {
                                localStorage.setItem('selectProjectIdDash', null);
                                localStorage.setItem('selectProjectNameDash', "");

                                //For App Sec
                                myLocalStorage.set('selectProjectDataDash', null);
                                localStorage.setItem('selectProjectProjectManagerIDDash', null);
                                localStorage.setItem('selectProjectDirectorIDDash', null);
                                localStorage.setItem('selectProjectSchedulerIDDash', null);
                                localStorage.setItem('selectProjectVicePresidentIDDash', null);
                                localStorage.setItem('selectProjectFinancialAnalystIDDash', null);
                                localStorage.setItem('selectProjectCapitalProjectAssistantIDDash', null);
                                //dhtmlx('at $scope.filterChangeProgram $http.get pgmId:' + pgmId + ' else...');
                            }
                            $scope.loadWBSData(orgId, pgmId, null, null, null, null, null);

                        });
                }
                else {
                    $http.get(serviceBasePath + "Request//ProjectByOid/" + orgId)
                        .then(function (response) {
                            var projectNameOnLoad = null;
                            projectNameOnLoad = (localStorage.getItem('selectProjectNameDash'))
                                ? localStorage.getItem('selectProjectNameDash')
                                : response.data.result[0].ProjectElementNumber + ". " + response.data.result[0].ProjectName;
                            localStorage.setItem('selectProjectIdDash', response.data.result[0].ProjectID);
                            localStorage.setItem('selectProjectNameDash', projectNameOnLoad);

                            //For App Sec
                            myLocalStorage.set('selectProjectDataDash', response.data.result[0]);
                            localStorage.setItem('selectProjectProjectManagerIDDash', response.data.result[0].ProjectManagerID);
                            localStorage.setItem('selectProjectDirectorIDDash', response.data.result[0].DirectorID);
                            localStorage.setItem('selectProjectSchedulerIDDash', response.data.result[0].SchedulerID);
                            localStorage.setItem('selectProjectVicePresidentIDDash', response.data.result[0].VicePresidentID);
                            localStorage.setItem('selectProjectFinancialAnalystIDDash', response.data.result[0].FinancialAnalystID);
                            localStorage.setItem('selectProjectCapitalProjectAssistantIDDash', response.data.result[0].CapitalProjectAssistantID);
                            //dhtmlx('at $scope.filterChangeProgram $http.get orgId:' + orgId);
                            $scope.loadWBSData(orgId, pgmId, null, null, null, '1', null);

                        });
                }

            }

            $scope.filterChangeProgramElement = function () {
                console.log('Called program element');
                $scope.filterProject = "";
                $scope.projectList = "";
                $scope.projectClassListDD = "";
                $scope.filterDepartment = "";
                var orgId = $("#selectOrg").val();
                var pgmId = $("#selectProgram").val();
                var pgmEltId = $("#selectProgramElement").val();

                localStorage.removeItem('pgmEltId');
                localStorage.removeItem('projId');
                localStorage.removeItem('SearchText');

                if (pgmEltId != null && pgmEltId != "") {
                    localStorage.setItem('pgmEltId', pgmEltId);
                    Project.lookup().get({ ProgramID: pgmId, ProgramElementID: pgmEltId }, function (projectData) {
                        $scope.projectList = projectData.result;
                        $scope.projectList.sort(function (a, b) {    //vaishnavi
                            return a.ProjectName.localeCompare(b.ProjectName);   //vaishnavi
                        });  //vaishnavi
                    });
                    ProjectClassByProgramElementId.get({ programElemID: pgmEltId }, function (response) {
                        var data = response;
                        $scope.projectClassListDD = response.result;
                    });
                }
                else if (pgmId != null && pgmId != "") {
                    ProjectClassByProgramId.get({ programID: pgmId }, function (response) {
                        $scope.projectClassListDD = response.result;
                    });
                }
                var oldsvg = d3.select("#wbs-tree");
                oldsvg.selectAll("*").remove();
                if (pgmEltId != "") {
                    $http.get(serviceBasePath + "Request/Project/" + pgmId + "/" + pgmEltId)
                        .then(function (response) {
                            // var projectID = response.data.result[0].ProjectID;
                            //
                            console.log(response.data);
                            if (response.data.result.length > 0) {
                                var projectNameOnLoad = null;
                                projectNameOnLoad = (localStorage.getItem('selectProjectNameDash'))
                                    ? localStorage.getItem('selectProjectNameDash')
                                    : response.data.result[0].ProjectElementNumber + ". " + response.data.result[0].ProjectName;
                                localStorage.setItem('selectProjectIdDash', response.data.result[0].ProjectID);
                                localStorage.setItem('selectProjectNameDash', projectNameOnLoad);

                                //For App Sec
                                myLocalStorage.set('selectProjectDataDash', response.data.result[0]);
                                localStorage.setItem('selectProjectProjectManagerIDDash', response.data.result[0].ProjectManagerID);
                                localStorage.setItem('selectProjectDirectorIDDash', response.data.result[0].DirectorID);
                                localStorage.setItem('selectProjectSchedulerIDDash', response.data.result[0].SchedulerID);
                                localStorage.setItem('selectProjectVicePresidentIDDash', response.data.result[0].VicePresidentID);
                                localStorage.setItem('selectProjectFinancialAnalystIDDash', response.data.result[0].FinancialAnalystID);
                                localStorage.setItem('selectProjectCapitalProjectAssistantIDDash', response.data.result[0].CapitalProjectAssistantID);
                                //dhtmlx('at $scope.filterChangeProgramElement $http.get pgmId:' + pgmId + ' pgmEltId:' + pgmEltId);
                            }
                            else {
                                localStorage.setItem('selectProjectIdDash', null);
                                localStorage.setItem('selectProjectNameDash', "");

                                //For App Sec
                                myLocalStorage.set('selectProjectDataDash', null);
                                localStorage.setItem('selectProjectProjectManagerIDDash', null);
                                localStorage.setItem('selectProjectDirectorIDDash', null);
                                localStorage.setItem('selectProjectSchedulerIDDash', null);
                                localStorage.setItem('selectProjectVicePresidentIDDash', null);
                                localStorage.setItem('selectProjectFinancialAnalystIDDash', null);
                                localStorage.setItem('selectProjectCapitalProjectAssistantIDDash', null);
                                //dhtmlx('at $scope.filterChangeProgramElement $http.get pgmId:' + pgmId + ' pgmEltId:' + pgmEltId + ' else...');
                            }
                            $scope.loadWBSData(orgId, pgmId, pgmEltId, null, null, '1', null);
                        });
                } else {
                    $http.get(serviceBasePath + "Request/Project/" + pgmEltId)
                        .then(function (response) {
                            var projectNameOnLoad = null;
                            projectNameOnLoad = (localStorage.getItem('selectProjectNameDash'))
                                ? localStorage.getItem('selectProjectNameDash')
                                : response.data.result[0].ProjectElementNumber + ". " + response.data.result[0].ProjectName;
                            localStorage.setItem('selectProjectIdDash', response.data.result[0].ProjectID);
                            localStorage.setItem('selectProjectNameDash', projectNameOnLoad);

                            //For App Sec
                            myLocalStorage.set('selectProjectDataDash', response.data.result[0]);
                            localStorage.setItem('selectProjectProjectManagerIDDash', response.data.result[0].ProjectManagerID);
                            localStorage.setItem('selectProjectDirectorIDDash', response.data.result[0].DirectorID);
                            localStorage.setItem('selectProjectSchedulerIDDash', response.data.result[0].SchedulerID);
                            localStorage.setItem('selectProjectVicePresidentIDDash', response.data.result[0].VicePresidentID);
                            localStorage.setItem('selectProjectFinancialAnalystIDDash', response.data.result[0].FinancialAnalystID);
                            localStorage.setItem('selectProjectCapitalProjectAssistantIDDash', response.data.result[0].CapitalProjectAssistantID);
                            //dhtmlx('at $scope.filterChangeProgramElement $http.get pgmId:' + pgmId);
                            $scope.loadWBSData(orgId, pgmId, pgmEltId, null, null, '1', null);
                        });
                }
                //filterChangeProject();

            }

            $scope.filterChangeDepartment = function () {

                var orgId = $("#selectOrg").val();
                var pgmId = $("#selectProgram").val();
                var pgmEltId = $("#selectProgramElement").val();
                var deptEltId = $("#selectManagingDepartment").val();

                localStorage.removeItem('pgmEltId');
                localStorage.removeItem('projId');
                localStorage.removeItem('SearchText');

                if (pgmEltId == null || pgmEltId == "") {
                    pgmEltId = null;
                }

                var oldsvg = d3.select("#wbs-tree");
                oldsvg.selectAll("*").remove();
                $scope.loadWBSData(orgId, pgmId, pgmEltId, null, null, '1', deptEltId);
                //filterChangeProject();

            }

            $scope.filterChangeProject = function (proj) {
                console.log('Called project');
                var orgId = $("#selectOrg").val();
                var pgmId = $("#selectProgram").val();
                var pgmEltId = $("#selectProgramElement").val();
                var projId = $("#selectProject").val();

                localStorage.removeItem('projId');
                localStorage.removeItem('SearchText');

                var oldsvg = d3.select("#wbs-tree");
                oldsvg.selectAll("*").remove();
                if (projId != "") {
                    localStorage.setItem('projId', projId);
                    $http.get(serviceBasePath + "Request/Project/" + pgmId + "/" + pgmEltId + "/" + projId)
                        .then(function (response) {
                            var projectNameOnLoad = null;
                            projectNameOnLoad = (localStorage.getItem('selectProjectNameDash'))
                                ? localStorage.getItem('selectProjectNameDash')
                                : response.data.result[0].ProjectElementNumber + ". " + response.data.result[0].ProjectName;
                            localStorage.setItem('selectProjectIdDash', projId);
                            localStorage.setItem('selectProjectNameDash', projectNameOnLoad);

                            //For App Sec
                            myLocalStorage.set('selectProjectDataDash', response.data.result[0]);
                            localStorage.setItem('selectProjectProjectManagerIDDash', response.data.result[0].ProjectManagerID);
                            localStorage.setItem('selectProjectDirectorIDDash', response.data.result[0].DirectorID);
                            localStorage.setItem('selectProjectSchedulerIDDash', response.data.result[0].SchedulerID);
                            localStorage.setItem('selectProjectVicePresidentIDDash', response.data.result[0].VicePresidentID);
                            localStorage.setItem('selectProjectFinancialAnalystIDDash', response.data.result[0].FinancialAnalystID);
                            localStorage.setItem('selectProjectCapitalProjectAssistantIDDash', response.data.result[0].CapitalProjectAssistantID);
                            //dhtmlx('at $scope.filterChangeProject $http.get pgmId:' + pgmId + ' pgmEltId:' + pgmEltId + ' projId:' + projId);

                        });
                } else {
                    $http.get(serviceBasePath + "Request/Project/" + pgmId + "/" + pgmEltId)
                        .then(function (response) {
                            var projectNameOnLoad = null;
                            projectNameOnLoad = (localStorage.getItem('selectProjectNameDash'))
                                ? localStorage.getItem('selectProjectNameDash')
                                : response.data.result[0].ProjectElementNumber + ". " + response.data.result[0].ProjectName;
                            localStorage.setItem('selectProjectIdDash', response.data.result[0].ProjectID);
                            localStorage.setItem('selectProjectNameDash', projectNameOnLoad);

                            //For App Sec
                            myLocalStorage.set('selectProjectDataDash', response.data.result[0]);
                            localStorage.setItem('selectProjectProjectManagerIDDash', response.data.result[0].ProjectManagerID);
                            localStorage.setItem('selectProjectDirectorIDDash', response.data.result[0].DirectorID);
                            localStorage.setItem('selectProjectSchedulerIDDash', response.data.result[0].SchedulerID);
                            localStorage.setItem('selectProjectVicePresidentIDDash', response.data.result[0].VicePresidentID);
                            localStorage.setItem('selectProjectFinancialAnalystIDDash', response.data.result[0].FinancialAnalystID);
                            localStorage.setItem('selectProjectCapitalProjectAssistantIDDash', response.data.result[0].CapitalProjectAssistantID);
                            //dhtmlx('at $scope.filterChangeProject $http.get pgmId:' + pgmId + ' pgmEltId:' + pgmEltId + ' projId:' + projId + ' else...');
                        });
                }

                $scope.loadWBSData(orgId, pgmId, pgmEltId, projId, null, '1', null);

            }

            $scope.getProjectFilter = function () {
                return $scope.filterProject;
            }


            // For Reporting
            $scope.reportType = "";
            $scope.reportList = "";
            $scope.changeType = function () {
            };
            $scope.openReportModalDetails = function () {

                var reportLabel = "";
                if ($scope.reportType == "") {
                    dhtmlx.alert({ text: "Please select a report type first", width: "300px" });
                    return;
                }
                //set reportLabel
                if ($scope.reportType == "AllTrend") {
                    reportLabel = "Project";
                    if (wbsTree.getSelectedNode()) {
                        if (wbsTree.getSelectedNode().depth == 3) {
                            openReport($scope.reportType, wbsTree.getSelectedNode().ProjectID);
                            wbsTree.setSelectedNode(null);
                            return;
                        }
                    }
                    console.log($scope.projectList);
                }
                else if ($scope.reportType == "ProgramBreakdown") {
                    reportLabel = "Program";
                    if (wbsTree.getSelectedNode()) {
                        if (wbsTree.getSelectedNode().depth > 0) {
                            openReport($scope.reportType, wbsTree.getSelectedNode().ProgramID);
                            wbsTree.setSelectedNode(null);
                            return;
                        }
                    }
                    else if ($scope.programList.length == 1) {
                        openReport($scope.reportType, $scope.programList[0].ProgramID);
                        return;
                    }
                }
                else if ($scope.reportType == "FACSum") {
                    reportLabel = "Trend";
                } else if ($scope.reportType == "ForecastTrend") {
                    reportLabel = "ForeCast";
                }
                var scope = $rootScope.$new();
                var newOrEdit = false;
                scope.params = {
                    newOrEdit: newOrEdit,
                    orgId: $("#selectOrg").val(),
                    ReportType: $scope.reportType,
                    ProList: $scope.programList,
                    reportLabel: reportLabel,
                    SelectedNode: wbsTree.getSelectedNode()
                };
                $rootScope.modalInstance = $uibModal.open({
                    backdrop: 'static',
                    keyboard: false,
                    scope: scope,
                    templateUrl: "app/views/Modal/ReportDetailModal.html",
                    size: "md",
                    controller: "ReportDetailModalCtrl"
                });

                $rootScope.modalInstance.result.then(function (response) {

                });
            };

            var openReport = function (rt, p) {
                var scope = $scope.$new();
                var newOrEdit = false;
                scope.params = {
                    newOrEdit: newOrEdit,
                    ReportType: rt,
                    ProjectID: p,
                    TrendNumber: 0
                };
                $rootScope.modalInstance = $uibModal.open({
                    backdrop: 'static',
                    keyboard: false,
                    scope: scope,
                    templateUrl: "app/views/Modal/PdfModal.html",
                    size: "lg",
                    controller: "PdfModalCtrl"
                });
            };

            var addCommas = function (nStr) {
                var num = Number(nStr);
                if (num > 1000000) {
                    //var rndNum = Math.ceil(num/1000000)*1000000;
                    var rndNum = Math.round(num / 100000);
                    rndNum = rndNum / 10;
                    nStr = rndNum + "M";
                } else if (rndNum > 1000) {
                    var rndNum = Math.round(num / 100);
                    rndNum = rndNum / 10;
                    nStr = rndNum + "K";
                }

                nStr += '';
                var x = nStr.split('.');
                var x1 = x[0];
                var x2 = x.length > 1 ? '.' + x[1] : '';
                var rgx = /(\d+)(\d{3})/;
                while (rgx.test(x1)) {
                    x1 = x1.replace(rgx, '$1' + ',' + '$2');
                }
                return x1 + x2;
            }

            //  
            $timeout(function () {
                $("#wbs-tree").on("contextmenu", function (data, index) {
                    //handle right click

                    //stop showing browser menu
                    d3.event.preventDefault();
                });
            }, 1000);

            //File Upload
            $scope.uploadFiles = function () {

                //alert('Hi');
                //return;
                var docTypeID = 1; //$("#uploadBtnProject option").filter(":selected").val();
                var request = {
                    method: 'POST',
                    url: serviceBasePath + '/uploadFiles/Post/' + wbsTree._selectedProjectID + '/' + docTypeID,
                    data: formdata,
                    headers: {
                        'Content-Type': undefined
                    }
                };

                /* Needs $http */
                // SEND THE FILES.
                $http(request)
                    .success(function (d) {
                        dhtmlx.alert(d);
                    })
                    .error(function (d) {
                        dhtmlx.alert(d.ExceptionMessage);
                    })
                    .finally(function () {
                        //Clear selected files
                        fileUpload.value = "";
                        formdata = new FormData();
                        //$('#uploadBtnProject').prop('disabled', true);
                    });

            };
        }]);

