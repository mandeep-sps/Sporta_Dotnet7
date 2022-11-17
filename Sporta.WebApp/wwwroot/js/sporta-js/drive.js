//URLs
//CRUD
let get_all_url = `/Drive/GetDrives?isLive=${false}&isUpcomming=${false}`,
    get_url = '/Drive/GetDrive',
    create_url = '/Drive/CreateDrive',
    update_url = '/Drive/UpdateDrive',
    remove_url = '/Drive/RemoveDrive',
    get_archived_drives_url = '/Drive/FilteredArchivedDrives',
    delete_drive_url = '/Drive/DeleteDrive',
    enable_disable_drive_url = '/Drive/EnableDisableDrive',
    get_Drive_Details = '/Drive/GetDrive'

//Dropdown Helpers
get_categories_dropdown_url = '/Drive/GetCategories_Dropdown',
    get_executives_dropdown_url = '/Drive/GetExecutives_Dropdown';

//Forms
let drive_form_id = "#drive-form",
    archive_filter_form = '#archive-filter-form',
    table_active_drives = '#drive-list',
    table_archived_drives = '#archive-drive-list',
    txt_id = '#txt-id',
    txt_drive_name = '#txt-drive-name',
    txt_scheduled = '#txt-scheduled',
    list_categories = '#categories-list',
    list_assignees = '#assignee-list',
    txt_time_alloted = '#txt-alloted-time',
    txt_total_question = '#txt-total-question',
    btn_create_new = '#btn-create-drive',
    btn_submit = '#btn-submit',
    category_properties = '#category-properties',
    Question_Level = '#Question-Level',
    div_create_drive = '#div-btn-create-new-drive';

var Drive_Id_tab = null,
    candidate_details = '#pills-candidate-details-content';

let is_archived = false,
    active_drive_content = '#pills-active-drive-content',
    navbar_drive = 'navbar-drive';

let easy_questions = '#pills-easy-question',
    medium_questions = '#pills-medium-question',
    hard_questions = '#pills-hard-question'

var questionLevelSaveData = [],
    temp = 0,
    selectedQuestionIds = [],
    easylevelsQuestionResponseData = null,
    mediumlevelsQuestionResponseData = null,
    hardlevelsQuestionResponseData = null,
    selectedQuestionIdsForCoun=[];

//Page Initialization
$(document).ready(function () {
    toogleNavbar(navbar_drive);
    InitializeDrive();

    $('#popupclose').click(function () {
        SportaForms.ResetForm(drive_form_id);
        questionLevelSaveData.length = 0;
        selectedQuestionIds.length = 0;
        temp = 0;
        document.getElementById("mediumCheckAll").checked = false;
        document.getElementById("easycheckAll").checked = false;
        document.getElementById("hardcheckAll").checked = false;

    });

    $('#popCloseQuestionlevel').click(function () {
        document.getElementById("mediumCheckAll").checked = false;
        document.getElementById("easycheckAll").checked = false;
        document.getElementById("hardcheckAll").checked = false;
    });

    $('#btn-submit').click(function () {
        questionLevelSaveData.length = 0;
        selectedQuestionIds.length = 0;
        temp = 0;
    })
});

//For Date & Time
const fpt = flatpickr(".datetime", {
    allowInput: true,
    enableTime: true,
    dateFormat: 'd-m-Y H:i',
});

//For Date only
const fp = flatpickr(" .date", {
    allowInput: true,
    enableTime: false,
    dateFormat: 'd-m-Y',
});

$(document).on('keyup', '.date', function (event) {
    fp.setDate(this.value, false);
});

$(document).on('keyup', '.datetime', function (event) {
    fpt.setDate(this.value, false);
});

// Intializing function for Drive Page
function InitializeDrive() {
    loadDrives();
    populateDriveDropdown();
    $(list_categories).on('change', function () {
        addCategoryProperties();
    });

    $('a[data-toggle="pill"]').on('shown.bs.tab', function (e) {
        if ($(e.target).attr('href') === active_drive_content) {
            loadDrives();
            populateDriveDropdown();
            $(div_create_drive).show();
            $(archive_filter_form).hide();

        }
        else if ($(e.target).attr('href') === '#pills-archived-content') {
            SportaForms.ResetForm(archive_filter_form);
            closeSideBar();
            $(div_create_drive).hide();
            $(archive_filter_form).show();
            SportaForms.InitializeFormStyle(archive_filter_form);
            loadArchivedDrive(null);
        }
    });
}

//Drive name anchor tag PopUp
function DrivePopUp(id) {
    $.get(get_Drive_Details, 'id=' + id, function (response) {
        if (response.isSuccess) {
            drive_name = response.data.driveName?.toUpperCase();
            $("#driveNamefor").html(drive_name ? drive_name : "");
            Drive_Id_tab = id;
            InitializePopUpData();
            $('#tableModalForDrive').modal('show');
        }
        else {
            alert(resposne);
        }
    });
}

//Load PopUp Candidate and Executive Data
function InitializePopUpData() {

    GetCandidateDetails(Drive_Id_tab);
    GetExecutiveDetail(Drive_Id_tab);
    $('a[data-toggle="pill"]').on('shown.bs.tab', function (e) {
        if ($(e.target).attr('href') === candidate_details) {
            $(div_create_drive).show();
            $(archive_filter_form).hide()
            GetCandidateDetails(Drive_Id_tab);
        } else {
            $(div_create_drive).show();
            $(archive_filter_form).hide()
            GetExecutiveDetail(Drive_Id_tab);
        }
    });
}

//Drive Candidate Details
function GetCandidateDetails(DriveId) {
    $.ajax({
        url: '/Drive/GetDriveCandidatesDetails/' + DriveId,
        type: 'Get',
        success: function (response) {
            var tableColumns = [
                { data: "name" },
                { data: "userName" },
                { data: 'contactNumber' },
                { data: 'attempted' },
                { data: 'status' },
            ]
            SportaDataTable.CreateDataTable("#candidate-list", response.data, tableColumns);
        },
        error: function (resposne) {
            alert(resposne);
        }
    });
}

//Drive Executives Details
function GetExecutiveDetail(Drive_id) {
    $.ajax({
        url: '/Drive/GetDriveExecuitvesDetails/' + Drive_id,
        type: 'Get',
        success: function (response) {
            $("#executive-tableBody").empty();
            var len = response.data.length, data = response.data;
            var result = "";
            for (var i = 0; i < len; i++) {
                result = "";
                result = result + "<tr>";
                result = result + "<td>" + response.data[i].username + "</td>";
                result = result + "<td>" + response.data[i].name + "</td>";
                result = result + ("</tr>");
                $("#executive-list").append(result);
            }
        },
        error: function (resposne) {
            alert(resposne);
        }
    });
}

//Load Active Drive Data
function loadDrives() {
    $.get(get_all_url, function (response) {
        var columns = [
            {
                data: null, render: function (data) { return `<a target="_self" data-toggle="tooltip" href="javascript:void(0)" title="click to view ${data.driveName} summery "onclick="DrivePopUp(${data.id})">${data.driveName}</a>`; }
            },
            { data: 'scheduled', render: function (date) { return moment(date).format('DD-MM-YYYY hh:mm A') } },
            { data: 'categories' },
            { data: 'assignees' },
            {
                data: null, render: function (data) { return `<a target="_blank" data-toggle="tooltip" title="${data.driveName} drive enrolled ${data.enrolled} candidates" href="/ApplicationUser/Candidates/${data.id}">${data.enrolled}</a>`; }
            },
            { data: 'token' },
            {
                data: null, render: function (data) {
                    return `<div class="form-group" >` +
                        `<div class="custom-control custom-switch" >`
                        + `<input onchange="SportaForms.ToggleSwitch('#active-status-checkbox-${data.id}',0,function(){driveStatusChangeEvent(${data.id},'${data.scheduled}')})" name="IsActive" id="active-status-checkbox-${data.id}" type="checkbox"  class="custom-control-input" ${data.status ? 'checked="checked"' : ""}>`
                        + `<label  data-toggle="tooltip" title="${data.status ? 'Active' : 'In - active'}" id="active-status-checkbox-${data.id}-label" class="custom-control-label" for="active-status-checkbox-${data.id}"></label>`
                        + `</div>`
                        + `</div>`;
                }, width: '100px'
            },
            { data: null, render: driveActionButtons },
        ]
        SportaDataTable.CreateDataTable(table_active_drives, response.data, columns, { orderBy: 1, orderSequence: 'desc' });
    })
}

// Buttons added to grid
function driveActionButtons(data, type, column) {

    var _buttons = '<div class="btn-group">'
        + `<button class="btn btn-sm btn-outline-grey" data-toggle="tooltip" title="Edit Drive" onclick="openDriveForm('${data.id}')"><i class="fas fa-pencil-alt"></i></button>`
        + `<button class="btn btn-sm btn-outline-grey" data-toggle="tooltip" title="Archive Drive" onclick="deleteDrive('${data.id}')"><i class="fa fa-archive"></i></button>`
        + `<a class="btn btn-sm btn-outline-grey" target="_blank" data-toggle="tooltip" title="View Report" href="/Report/Index/${data.id}" ><i class="fas fa-clipboard-list text-secondary"></i></a>`
        + `</div>`;

    return _buttons;

}


//Load Archived Drive Data
function loadArchivedDrive(data) {
    $.ajax({
        url: get_archived_drives_url,
        method: 'POST',
        data: data,
        beforeSend: function () { $(table_archived_drives).show(); },
        success: function (response) {
            if (userRole == 1) {
                var columns = [
                    { data: 'driveName' },
                    { data: 'scheduled', render: function (date) { return moment(date).format('DD-MM-YYYY hh:mm A') } },
                    { data: 'categories' },
                    { data: 'assignees' },
                    { data: 'enrolled' },
                    { data: 'token' },
                    { data: null, render: archiveDriveActionButtons },
                ]
            } else {
                var columns = [
                    { data: 'driveName' },
                    { data: 'scheduled', render: function (date) { return moment(date).format('DD-MM-YYYY hh:mm A') } },
                    { data: 'categories' },
                    { data: 'assignees' },
                    { data: 'enrolled' },
                    { data: 'token' },
                ]
            }
            SportaDataTable.CreateDataTable(table_archived_drives, response.data, columns);
        },
        error: function () { },
        complete: function () { }
    });
}

// Buttons for Archive drive grid
function archiveDriveActionButtons(data, type, column) {

    var _buttons = '<div class="btn-group">'
        + `<button class="btn btn-sm btn-outline-red" onclick="deleteArchiveDrive('${data.id}')"><i class="far fa-trash-alt"></i></button> &nbsp`
        + `<a class="btn btn-sm btn-outline-grey" target="_blank" data-toggle="tooltip" title="View Report" href="/Report/Index/${data.id}" ><i class="fas fa-clipboard-list text-secondary"></i></a>`
        + `</div>`;

    return _buttons;
}


//Load Dropdowns
function populateDriveDropdown() {
    //Getting categories
    $.get(get_categories_dropdown_url, function (response) {
        $(list_categories).prop("title", "Choose Sections")
        refreshDropdown(list_categories, response.data);
    })

    if (userRole == 1) {
        //Getting Executives
        $.get(get_executives_dropdown_url, function (response) {
            $(list_assignees).prop("title", "Choose Executives");
            refreshDropdown(list_assignees, response.data);
        })
    }
}

// Refreshing the dropdown
function refreshDropdown(element, data) {
    $(element).empty();
    $(element).selectpicker();
    $.each(data, function (index, item) {
        $(element).append("<option value=" + item.id + ">" + item.name + "</option>");
    });
    $(element).selectpicker('refresh')
}


//Form Validations
function valdiateQuestionSet(id, maxLimit) {
    SportaForms.ValidateInput($(`#category-${id}-question`), ($(`#category-${id}-question`).val() > +maxLimit), "This can not be exceed the total question present in this category!")
    SportaForms.FormValidationStatus(drive_form_id)
    $(`#category-${id}-time`).val($(`#category-${id}-question`).val())

}


// Validating Drive Form
function validateDrive() {
    SportaForms.ClearValidataionErrors(drive_form_id);

    var blankChecks = [txt_drive_name, txt_scheduled, list_assignees, list_categories];
    SportaForms.BlankInputChecks(blankChecks);

    return SportaForms.FormValidationStatus(drive_form_id);
}


//Form Submissions (CUD Operations)
$(drive_form_id).unbind().bind('submit', function (e) {
    var actionUrl = $(txt_id).val() == 0 ? create_url : update_url
    e.preventDefault();
    SportaForms.EnableLiveValidation(drive_form_id, validateDrive)
    if (validateDrive()) {
        if (validateDate($("#txt-scheduled").val())) {
            $.ajax({
                url: actionUrl,
                data: $(this).serializeArray(),
                type: 'POST',
                success: function (response) {
                    if (response.isSuccess) {
                        SportaUtil.MessageBoxSuccess(response.message, "Drive Created");
                        closeSideBar(drive_form_id);
                        loadDrives();

                    }
                    else {
                        SportaUtil.MessageBoxDanger(response.message);
                    }
                }

            });
        } else {
            SportaUtil.MessageBoxDanger("date should be greater then present date");
        }
    }
});


$(archive_filter_form).submit(function (e) {
    e.preventDefault();
    var data = $(this).serialize();
    loadArchivedDrive(data);
});

// Delete Drive
function deleteDrive(id) {
    SportaUtil.ConfirmDialogue("Are you sure to delete this drive?", "Delete Drive", function () {
        $.get(remove_url, 'id=' + id, function (response) {
            if (response.isSuccess) {
                SportaUtil.MessageBoxSuccess(response.message);
                loadDrives();
            }
            else {
                SportaUtil.MessageBoxDanger(response.message);
            }
        });
    });
}


function deleteArchiveDrive(id) {
    SportaUtil.ConfirmDialogue("Are you sure to delete this drive permanently?", "Delete Drive", function () {
        $.get(delete_drive_url, 'id=' + id, function (response) {
            if (response.isSuccess) {
                SportaUtil.MessageBoxSuccess(response.message);
                loadArchivedDrive(null);
            }
            else {
                SportaUtil.MessageBoxDanger(response.message);
            }
        });
    });
}

// Disable/Enable Drive Function
function disableEnableDrive(id, status) {
    $.ajax({
        url: enable_disable_drive_url,
        type: 'POST',
        data: { 'id': id, 'status': status },
        success: function (response) {
            if (response.isSuccess) {
                loadDrives()
            }
            else {
                SportaUtil.MessageBoxDanger(response.message)
            }

        },
        error: function (resposne) {

        }
    });
}


//Helpers
function openDriveForm(id) {
    SportaForms.ResetForm(drive_form_id);
    $(category_properties).empty();
    $(Question_Level).empty();
    if (id) {
        $.get(get_url, 'id=' + id, function (response) {
            var drive = response.data;
            var driveData = [];
            $(txt_id).val(drive.id);
            $(txt_drive_name).val(drive.driveName);
            $(txt_scheduled).val(drive.scheduled);
            $(txt_time_alloted).val(drive.allotedTime);
            $(txt_total_question).val(drive.totalQuestion);

            for (var i = 0; i < drive.driveCategory.length; i++) {
                driveData[i] = {
                    Id: drive.driveCategory[i].categoryId,
                    Time: drive.driveCategory[i].categoryTime,
                    Questions: drive.driveCategory[i].categoryQuestion,
                    SelectedIds: drive.driveCategory[i].selectedQuestions
                }
            }
            questionLevelSaveData = driveData;
            $(list_categories).selectpicker('val', drive.driveCategory.map(a => a.categoryId));
            $(list_assignees).selectpicker('val', drive.driveAssignee)

            $.each(drive.driveCategory, function (index, item) {
                $(`#category-${item.categoryId}-time`).val(item.categoryTime)
                $(`#category-${item.categoryId}-question`).val(item.categoryQuestion)
                $(`#category-${item.categoryId}`).val(item.categoryId)


            });
            openSidebar();
            SportaForms.InitializeFormStyle(drive_form_id);
        });
    }
    else {
        $(txt_id).val(0);
        openSidebar();
        SportaForms.InitializeFormStyle(drive_form_id);
    }

}

// Drive Status Change Event
function driveStatusChangeEvent(id, scheduled) {
    var isActive = $(`#active-status-checkbox-${id}`).prop("checked");
    if (!isActive) {
        if (!isGreaterThanToday(scheduled)) {
            $(`#active-status-checkbox-${id}`).prop("checked", !status);
            SportaUtil.ConfirmDialogue("Would You Like To Archive This?", { buttonText: ['Archive', 'Disbaled Only', 'Cancel'] }, function (e) {
                $.get(remove_url, 'id=' + id, function (response) {
                    if (response.isSuccess) {
                        loadDrives();
                    }
                    else {
                        SportaUtil.MessageBoxDanger(response.message);
                    }
                });
            }, function () {
                disableEnableDrive(id, isActive);

            });
        }
        else {
            disableEnableDrive(id, isActive);
        }
    }
    else {
        disableEnableDrive(id, isActive);
    }
}

//Save Category Temporary Data 
function DataSave(id) {
    var time = $(`#category-${id}-time`).val();
    var question = $(`#category-${id}-question`).val();
    document.getElementById("mediumCheckAll").checked = false;
    document.getElementById("easycheckAll").checked = false;
    document.getElementById("hardcheckAll").checked = false;
    questionLevelSaveData[temp] = {
        Id: id,
        Time: time,
        Questions: question,
        SelectedIds: selectedQuestionIds
    };
    temp++;
}

// adding time and questions for each selected category
function addCategoryProperties() {
    var selectedCategory = [];
    var category_properties_html = '';
    $(list_categories + ' :selected').each(function (i, selected) {

        selectedCategory[i] = {
            Id: $(selected).val(),
            Name: $(selected).text(),
            Time: 0,
            Question: 0,
            SelectedIds: ""
        };
    });

    if (questionLevelSaveData.length == 0) {
        for (var i = 0; i < selectedCategory.length; i++) {
            document.getElementById("easycheckAll").checked = false;
            var maxLimit = +selectedCategory[i].Name.substr(selectedCategory[i].Name.length - 5).match(/\(([^)]+)\)/)[1]
            category_properties_html += `
            <div id="drive-category-${selectedCategory[i].Id}">
                <h6 class="text-success">${selectedCategory[i].Name}</h6>
                    <div class="form-row">
                        <div class="col-sm-6"> 
                            <div class="form-group">
                                <label for="category-${selectedCategory[i].Id}-question" class="floating-label">Questions</label>
                                <input readonly type="number" value="${selectedCategory[i].Questions}" oninput="valdiateQuestionSet('${selectedCategory[i].Id}','${maxLimit}',DataSave('${selectedCategory[i].Id}'))"  name="DriveCategories[${i}].CategoryQuestion" id="category-${selectedCategory[i].Id}-question" class="form-control" required max="${maxLimit}" min="1"/>
                                <div class="invalid-input-feedback"></div>
                            </div>
                        </div>

                        <div class="col-sm-6">
                            <div class="form-group">
                                <label for="category-${selectedCategory[i].Id}-time" class="floating-label">Time (in minutes)</label>
                                <input type="number" value="${selectedCategory[i].Time}" name="DriveCategories[${i}].CategoryTime" id="category-${selectedCategory[i].Id}-time"  class="form-control" oninput="DataSave('${selectedCategory[i].Id}')" min="1" required/>
                                <input type="hidden" value="${selectedCategory[i].Id}" name="DriveCategories[${i}].CategoryId" id="category-${selectedCategory[i].Id}" class="form-control" />
                                <input type="hidden" value="${selectedCategory[i].SelectedIds}" name="DriveCategories[${i}].SelectedQuestions" id="category-${selectedCategory[i].Id}-selectedquestions" class="form-control" />
                                <div class="invalid-input-feedback"></div>
                            </div>
                        </div>
                    </div>
                </div>
                    <div class="form-row">
                        <div class="col-sm-3">
                            <div class="form-group">
                               <input type="button" class="btn btn-sm btn-theme border-10 create_btn" onclick="addQuestion(${selectedCategory[i].Id})" value="Add Question">
                            </div>
                        </div>
                   </div>`;

        }

    } else {
        for (var i = 0; i < selectedCategory.length; i++) {
            for (var j = 0; j < questionLevelSaveData.length; j++) {
                if (selectedCategory[i].Id == questionLevelSaveData[j].Id) {
                    selectedCategory[i].Time = questionLevelSaveData[j].Time;
                    selectedCategory[i].Questions = questionLevelSaveData[j].Questions;
                    selectedCategory[i].SelectedIds = questionLevelSaveData[j].SelectedIds;
                }

            }
            document.getElementById("easycheckAll").checked = false;
            var maxLimit = +selectedCategory[i].Name.substr(selectedCategory[i].Name.length - 5).match(/\(([^)]+)\)/)[1]
            category_properties_html += `
            <div id="drive-category-${selectedCategory[i].Id}">
                <h6 class="text-success">${selectedCategory[i].Name}</h6>
                    <div class="form-row">
                        <div class="col-sm-6">
                            <div class="form-group">
                                <label for="category-${selectedCategory[i].Id}-question" class="floating-label">Questions</label>
                                <input readonly type="number" value="${selectedCategory[i].Questions}" oninput="valdiateQuestionSet('${selectedCategory[i].Id}','${maxLimit}'),DataSave('${selectedCategory[i].Id}')" name="DriveCategories[${i}].CategoryQuestion" id="category-${selectedCategory[i].Id}-question" class="form-control" required max="${maxLimit}" min="1"/>
                                <div class="invalid-input-feedback"></div>
                            </div>
                        </div>

                        <div class="col-sm-6">
                            <div class="form-group">
                                <label for="category-${selectedCategory[i].Id}-time" class="floating-label">Time (in minutes)</label>
                                <input type="number" value="${selectedCategory[i].Time}" name="DriveCategories[${i}].CategoryTime" id="category-${selectedCategory[i].Id}-time"  class="form-control" oninput="DataSave('${selectedCategory[i].Id}')" min="1" required/>
                                <input type="hidden" value="${selectedCategory[i].Id}" name="DriveCategories[${i}].CategoryId" id="category-${selectedCategory[i].Id}" class="form-control" />
                                <input type="hidden" value="${selectedCategory[i].SelectedIds}" name="DriveCategories[${i}].SelectedQuestions" id="category-${selectedCategory[i].Id}-selectedquestions" class="form-control" />
                                <div class="invalid-input-feedback"></div>
                            </div>
                        </div>
                    </div>
                </div>
                    <div class="form-row">
                        <div class="col-sm-3">
                            <div class="form-group">
                               <input type="button" class="btn btn-sm btn-theme border-10 create_btn " onclick="addQuestion(${selectedCategory[i].Id})" value="Add Question">
                            </div>
                        </div>
                   </div>`;
        }
        SportaForms.InitializeFormStyle(drive_form_id);
    }

    $(category_properties).html(category_properties_html);
    SportaForms.InitializeFormStyle(drive_form_id);
}

//Add Question In QuestionLevels (Easy,Medium,Hard)
function addQuestion(id) {
    $('#totalCount').empty();
    var save_button = '';
    var ArrayIds = null;
    if ($(`#category-${id}-selectedquestions`).val().length === 0) {
        GetQuestionByEasyLevel(id, ArrayIds);
        GetQuestionByMediumLevel(id, ArrayIds);
        GetQuestionByHardLevel(id, ArrayIds);
    }
    else {
        var QuestionIdString = $(`#category-${id}-selectedquestions`).val();
        ArrayIds = QuestionIdString.split(',');
        GetQuestionByEasyLevel(id, ArrayIds);
        GetQuestionByMediumLevel(id, ArrayIds);
        GetQuestionByHardLevel(id, ArrayIds);
    }

    $('a[data-toggle="pill"]').on('shown.bs.tab', function (e) {

        if ($(e.target).attr('href') == easy_questions) {
            $("#easy-Quesion-list").show();
            $(archive_filter_form).hide();
        }
        else if ($(e.target).attr('href') == medium_questions) {
            $("#medium-Question-list").show();
            $(archive_filter_form).hide();
        }
        else {
            $("#hard-Question-list").show();
            $(archive_filter_form).hide();
        }
    });

    save_button += `
    <div class="savebtn">
        <button class="btn btn-md btn-theme" id="select" onclick="SaveQuestionIds(${id})">Save</button>
    </div>`;

    selectAll();
    $(savebutton).html(save_button);
    SportaForms.InitializeFormStyle(drive_form_id);
    $('#addQuestionModal').modal('show');
    $("#easy-Quesion-list").show();
}

//Select All Checkboxes
function selectAll() {
    $('#easycheckAll').click(function () {
        $('input[id="CheckboxID1"]').prop('checked', this.checked);
    })
    $('#mediumCheckAll').click(function () {
        $('input[id="CheckboxID2"]').prop('checked', this.checked);
    })
    $('#hardcheckAll').click(function () {
        $('input[id="CheckboxID3"]').prop('checked', this.checked);
    })
}

//Save Selected Question Ids
function SaveQuestionIds(id) {

    var selectedIds = [];
    $.each($("input:checkbox[name='ques']:checked"), function () {
        selectedIds.push($(this).val());
    });


    var countIds = selectedIds.length;
    var IdList = selectedIds.join(",");
    selectedQuestionIds = IdList;
    $(`#category-${id}-question`).val(countIds);
    $(`#category-${id}-time`).val($(`#category-${id}-question`).val())
    $(`#category-${id}-selectedquestions`).val(IdList);
    $(`#category-${id}-question`)
    $('#addQuestionModal').modal('hide');
}

//CheckBox Functionality For Easy Question Level
function CheckboxCheckedEasyLevel() {
    var selectedIds = [];
    var saveselectedIdsByLevel = 0;
    var selectedEasyLevelIds=[];
    $.each($("input:checkbox[name='ques']:checked"), function () {
        selectedIds.push($(this).val());
    })
    for (var i = 0; i < selectedIds.length; i++) {

        for (var j = 0; j < easylevelsQuestionResponseData.length; j++) {

            var id = parseInt(selectedIds[i]);
            if (id == easylevelsQuestionResponseData[j].id) {
                saveselectedIdsByLevel++;
            }
        }
    }

    $('#easyQuestionSelectCount').html(saveselectedIdsByLevel)
    if (saveselectedIdsByLevel == easylevelsQuestionResponseData.length) {
        document.getElementById("easycheckAll").checked = true;
    } else {
        document.getElementById("easycheckAll").checked = false;
    }

}

//CheckBox Functionality For Medium Question Level
function CheckboxCheckedMediumLevel() {
    var selectedIds = [];
    var saveselectedIdsByLevel = 0;

    $.each($("input:checkbox[name='ques']:checked"), function () {
        selectedIds.push($(this).val());
    })
    for (var i = 0; i < selectedIds.length; i++) {

        for (var j = 0; j < mediumlevelsQuestionResponseData.length; j++) {

            var id = parseInt(selectedIds[i]);
            if (id == mediumlevelsQuestionResponseData[j].id) {
                saveselectedIdsByLevel++;
            }
        }
    }

    $('#mediumQuestionSelectCount').html(saveselectedIdsByLevel)
    if (saveselectedIdsByLevel == mediumlevelsQuestionResponseData.length) {
        document.getElementById("mediumCheckAll").checked = true;
    } else {
        document.getElementById("mediumCheckAll").checked = false;
    }

}

//CheckBox Functionality For Hard Question Level
function CheckboxCheckedCheHardLevel() {
    var selectedIds = [];
    var saveselectedIdsByLevel = 0;

    $.each($("input:checkbox[name='ques']:checked"), function () {
        selectedIds.push($(this).val());
    })
    for (var i = 0; i < selectedIds.length; i++) {

        for (var j = 0; j < hardlevelsQuestionResponseData.length; j++) {

            var id = parseInt(selectedIds[i]);
            if (id == hardlevelsQuestionResponseData[j].id) {
                saveselectedIdsByLevel++;
            }
        }
    }

    $('#hardQuestionSelectCount').html(saveselectedIdsByLevel)
    if (saveselectedIdsByLevel == hardlevelsQuestionResponseData.length) {
        document.getElementById("hardcheckAll").checked = true;
    } else {
        document.getElementById("hardcheckAll").checked = false;
    }

}

//Remove Html Tags From Question
function questionRemoveRegex(data, type, row) {
    var regX = /(<([^>]+)>)/ig;
    var html = data;
    var findIsStringIshtml = isHTML(html);
    if (findIsStringIshtml) {
        var textQuestion = html.replace(regX, "");
        return type === 'display' && textQuestion.length > 100 ?
            `<span data-toggle="tooltip" title='${textQuestion}'> ${textQuestion.substr(0, 100)}… </span>` : textQuestion;
    }
    else {
        return type === 'display' && html.length > 100 ?
            `<span data-toggle="tooltip" title='${html}'> ${html.substr(0, 100)}… </span>` : html;
    }
}

//Find Is String Contains Html Or Not
function isHTML(str) {
    var a = document.createElement('div');
    a.innerHTML = str;

    for (var c = a.childNodes, i = c.length; i--;) {
        if (c[i].nodeType == 1) return true;
    }

    return false;
}

//Get Question By Easy Level Base Of Category Id
function GetQuestionByEasyLevel(id, QuestionIds) {

    var count = 0;
    var QuesIds = QuestionIds;
    easy = true;
    medium = false;
    hard = false;
    $.ajax({
        url: "/Question/GetQuestionListByLevels",
        type: 'Get',
        data: { 'categoryId': id, 'isEasy': easy, 'isMedium': medium, 'isHard': hard },
        success: function (response) {
            if (response.data.length <= 0) {
                $('#easycheckAll').hide();
            }
            else {
                $('#easycheckAll').show();
            }
            easylevelsQuestionResponseData = response.data;
            if (QuestionIds == null) {
                var columns = [
                    {
                        data: null, render: function (data) {
                            return `<input type = "checkbox" id = "CheckboxID1" name = "ques"  value = ` + data.id + ` onclick="CheckboxCheckedEasyLevel()"></td>`;
                        }
                    },
                    { data: 'question1',render: questionRemoveRegex},
                ]
                SportaDataTable.CreateDataTable("#easy-Quesion-list", response.data, columns, { showPaging: false, exportButton: false, enableSearch: false });
            } else {
                for (var i = 0; i < QuestionIds.length; i++) {
                    for (var j = 0; j < response.data.length; j++) {
                        var QuestionId = parseInt(QuestionIds[i]);
                        if (QuestionId == response.data[j].id) {
                            count++;
                        }
                    }
                }
                if (count == response.data.length) {
                    document.getElementById("easycheckAll").checked = true;
                }
                var columns = [
                    {
                        data: null, render: function (data) {
                            var quesHtml = `<input type = "checkbox" id = "CheckboxID1" name = "ques"  value = ` + data.id + ` onclick="CheckboxCheckedEasyLevel()">`;

                            $.each(QuestionIds, function (index, valueId) {
                                var QuestionIds = parseInt(valueId);
                                if (QuestionIds == data.id) {
                                    quesHtml = null;
                                    quesHtml = `<input type = "checkbox" id = "CheckboxID1" name = "ques"  value = ` + data.id + ` onclick="CheckboxCheckedEasyLevel()" checked>`;
                                }
                            });
                            return quesHtml;
                        }
                    },
                    { data: 'question1', render: questionRemoveRegex },
                ]
                SportaDataTable.CreateDataTable("#easy-Quesion-list", response.data, columns, { showPaging: false, exportButton: false, enableSearch: false });
            }
        },
        error: function (resposne) {
            SportaUtil.MessageBoxDanger(response.message)
        }
    });
}

//Get Question By Medium Level Base Of Category Id
function GetQuestionByMediumLevel(id, QuestionIds) {
    var count = 0;
    easy = false;
    medium = true;
    hard = false;
    $.ajax({
        url: "/Question/GetQuestionListByLevels",
        type: 'Get',
        data: { 'categoryId': id, 'isEasy': easy, 'isMedium': medium, 'isHard': hard },
        success: function (response) {
            if (response.data.length <= 0) {
                $('#mediumcheckAll').hide();
            }
            else {
                $('#mediumcheckAll').show();
            }
            mediumlevelsQuestionResponseData = response.data;
            if (QuestionIds == null) {
                var columns = [
                    {
                        data: null, render: function (data) {

                            return `<input type = "checkbox" id = "CheckboxID2" name = "ques"  value = ` + data.id + ` onclick="CheckboxCheckedMediumLevel()"></td>`;
                        }
                    },
                    { data: 'question1', render: questionRemoveRegex },
                ]
                SportaDataTable.CreateDataTable("#medium-Question-list", response.data, columns, { showPaging: false, exportButton: false, enableSearch: false });
            } else {
                for (var i = 0; i < QuestionIds.length; i++) {
                    for (var j = 0; j < response.data.length; j++) {
                        var QuestionId = parseInt(QuestionIds[i]);
                        if (QuestionId == response.data[j].id) {
                            count++;
                        }
                    }
                }
                if (count == response.data.length) {
                    document.getElementById("mediumCheckAll").checked = true;
                }
                var columns = [
                    {
                        data: null, render: function (data) {
                            var quesHtml = `<input type = "checkbox" id = "CheckboxID2" name = "ques"  value = ` + data.id + ` onclick="CheckboxCheckedMediumLevel()"></td>`;
                            $.each(QuestionIds, function (index, valueId) {
                                var QuestionIds = parseInt(valueId);
                                if (QuestionIds == data.id) {
                                    quesHtml = '';
                                    quesHtml = `<input type = "checkbox" id = "CheckboxID2" name = "ques"  value = ` + data.id + ` onclick="CheckboxCheckedMediumLevel()" checked></td>`;
                                }
                            });
                            return quesHtml;
                        }
                    },
                    { data: 'question1', render: questionRemoveRegex },
                ]
                SportaDataTable.CreateDataTable("#medium-Question-list", response.data, columns, { showPaging: false, exportButton: false, enableSearch: false });
            }
        },
        error: function (resposne) {
            SportaUtil.MessageBoxDanger(response.message)
        }
    });
}

//Get Question By Medium Level Base Of Category Id
function GetQuestionByHardLevel(id, QuestionIds) {

    var count = 0;
    easy = false;
    medium = false;
    hard = true;
    $.ajax({
        url: "/Question/GetQuestionListByLevels",
        type: 'Get',
        data: { 'categoryId': id, 'isEasy': easy, 'isMedium': medium, 'isHard': hard },
        success: function (response) {
            if (response.data.length <= 0) {
                $('#hardcheckAll').hide();
            }
            else {
                $('#hardcheckAll').show();
            }
            hardlevelsQuestionResponseData = response.data;
            if (QuestionIds == null) {
                var columns = [
                    {
                        data: null, render: function (data) {

                            return `<input type = "checkbox" id = "CheckboxID3" name = "ques"  value = ` + data.id + ` onclick="CheckboxCheckedCheHardLevel()"></td>`;
                        }
                    },
                    { data: 'question1', render: questionRemoveRegex },
                ]
                SportaDataTable.CreateDataTable("#hard-Question-list", response.data, columns, { showPaging: false, exportButton: false, enableSearch: false });
            } else {
                for (var i = 0; i < QuestionIds.length; i++) {
                    for (var j = 0; j < response.data.length; j++) {
                        var QuestionId = parseInt(QuestionIds[i]);
                        if (QuestionId == response.data[j].id) {
                            count++;
                        }
                    }
                }
                if (count == response.data.length) {
                    document.getElementById("hardcheckAll").checked = true;
                }
                var columns = [
                    {
                        data: null, render: function (data) {
                            var quesHtml = `<input type = "checkbox" id = "CheckboxID3" name = "ques"  value = ` + data.id + ` onclick="CheckboxCheckedCheHardLevel()"></td>`;
                            $.each(QuestionIds, function (index, valueId) {
                                var QuestionIds = parseInt(valueId);
                                if (QuestionIds == data.id) {
                                    quesHtml = '';
                                    quesHtml = `<input type = "checkbox" id = "CheckboxID3" name = "ques"  value = ` + data.id + ` onclick="CheckboxCheckedCheHardLevel()" checked></td>`;
                                }
                            });
                            return quesHtml;
                        }
                    },
                    { data: 'question1', render: questionRemoveRegex },
                ]
                SportaDataTable.CreateDataTable("#hard-Question-list", response.data, columns, { showPaging: false, exportButton: false, enableSearch: false });
            }
        },
        error: function (resposne) {
            SportaUtil.MessageBoxDanger(response.message)
        }
    });
}



// Validating date should be greater then present date
function validateDate(date) {
    var selectedDate = new Date(date);
    var TodayDate = new Date();
    selectedDate.setHours(0, 0, 0, 0);
    TodayDate.setHours(0, 0, 0, 0);
    if (selectedDate < TodayDate) {
        return false;
    }
    else {
        return true;
    }
}


// closing add executive pop up form
function closeAssigneeForm() {
    $("#assignee-add-form")[0].reset();
    $("#assigneeModal").modal("toggle");
}

// adding executive
function addAssignee() {
    var requestModel = {
        Id: '0',
        Name: $("#txt-name").val(),
        EmailId: $("#txt-email-id").val(),
        ApplicationRoleId: eApplicationUserRoles.Executive,
        UserPassword: 'softprodigy'
    }
    if (validateEmail($("#txt-email-id").val()) && $("#txt-name").val()) {
        addUser(requestModel);
        $("#assignee-add-form")[0].reset();
        $("#assigneeModal").modal("toggle");
    } else {
        SportaUtil.MessageBoxDanger('Please enter valid Inputs!');
    }

}

// add User
function addUser(data) {
    $.ajax({
        url: "/ApplicationUser/AddUser",
        data: data,
        type: 'POST',
        success: function (response) {
            if (response.isSuccess) {
                SportaUtil.MessageBoxSuccess(`${response.data.name} as ${response.message}`);
                populateDriveDropdown();
                $("#assignee-add").empty();
            }
            else {
                SportaUtil.MessageBoxDanger(response.message)
            }
        },
        error: function (resposne) {

        }
    });
}

// validating Email
function validateEmail(email) {
    const re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(email);
}
