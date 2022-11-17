let getAllCandidateUrl = "/Candidate/GetCandidates",
    deleteCandidateUrl = "/ApplicationUser/RemoveUser",
    getUserUrl = "/ApplicationUser/UserDetails",
    updateCandidateUrl = "/Candidate/UpdateCandidate",
    driveId = 0, navbar_users = 'navbar-users';


let candidate_list = "#candidate-list",
    deiveH2 = '#driveSpan',
    candidateForm = '#edit-candidate-form',
    drive_list = '#drive-list';


//Forms
let txt_name = '#txt-name',
    txt_id = '#txt-id',
    txt_driveId = '#txt-driveId',
    txt_email = '#txt-email-id',
    txt_contact_no = '#txt-contact-no',
    txt_rollNo = '#txt-rollNo',
    branches_list = '#branches-list',
    btn_submit = "#updateCandidate";


$(document).ready(function () {
    toogleNavbar(navbar_users);

    var url = userRole == 1 ? '/Drive/GetAllDrives_Dropdown' : '/Drive/GetDrivesByExecutive_Dropdown';

    $.ajax({
        url: url,
        type: 'POST',
        success: function (response) {
            $(drive_list).empty();
            $.each(response.data, function (index, item) {
                $(drive_list).append("<option value=" + item.id + ">" + item.name + "</option>");
            });
            $(drive_list).selectpicker('refresh');
            
            if (response.data[0] && driveId == 0) {
                driveId = response.data[0].id;
            }
        },
        error: function (resposne) {
        },
        complete: function () {
            if (driveId > 0)
                $(drive_list).val(driveId).change();
        }
    });


    $.get('/ApplicationUser/GetBranches', function (response) {
        $(branches_list).empty();
        $.each(response.data, function (index, item) {
            $(branches_list).append("<option value=" + item.name + ">" + item.name + "</option>");
        });
        $(branches_list).selectpicker('refresh');
    });

});

// Loading candidates acccording to Drive
$(drive_list).on('change', function (e) {
    e.preventDefault();
    driveId = $(this).val();
    loadcandidates();
});


function viewEnrolled(dId) {
    driveId = dId;
}

// Candidate Grid Binding
function loadcandidates() {

    $.get(getAllCandidateUrl, "driveId=" + driveId, function (response) {
        $('#candidateTabDiv').show();
        var driveName = response.data[0]?.drive?.toUpperCase();
        $(deiveH2).html(driveName ? driveName : "");
        if (userRole == 1) {
            var tableColumns = [
                { data: "name" },
                { data: "username" },
                { data: "rollNo" },
                { data: "branch" },
                { data: 'contactNumber' },
                { data: null, render: usersActionButtons }
            ]
        } else {
            var tableColumns = [
                { data: "name" },
                { data: "username" },
                { data: "rollNo" },
                { data: "branch" },
                { data: 'contactNumber' },
            ]
        }
        SportaDataTable.CreateDataTable(candidate_list, response.data, tableColumns);
    });

}
//Buttons Added to Grid

function usersActionButtons(data, type, column) {
    var _buttons = '<div class="btn-group">'
        + `<button class="btn btn-sm btn-outline-grey" onclick="openCandidateForm('${data.id}','${data.applicationRoleId}')"><i class="fas fa-pencil-alt"></i></button>`
        + `<button class="btn btn-sm btn-outline-grey" onclick="deleteCandidate('${data.id}','${data.name}','${data.applicationRoleId}')"><i class="far fa-trash-alt"></i></button>`
        + `</div>`;

    return _buttons;
}

// Delete Candidate Function
function deleteCandidate(id, name, roleId) {
    SportaUtil.ConfirmDialogue(`Are you sure  to delete ${name}?`, null, function () {
        $.get(deleteCandidateUrl, { 'userId': id, 'roleId': roleId }, function (response) {
            if (response.isSuccess)
                SportaUtil.MessageBoxSuccess(response.message)
            loadcandidates()
        })
    })
}

// OPening Candidate Form
function openCandidateForm(id, roleId) {
    if (id) {
        $.get(getUserUrl, { 'userId': id, 'roleId': roleId }, function (response) {
            SportaForms.ResetForm(candidateForm);
            var data = response.data;
            $(txt_id).val(data.id);
            $(txt_name).val(data.name);
            $(txt_email).val(data.username).prop('readonly', true);
            $(txt_contact_no).val(data.candidateRequestModel.contactNumber);
            $(txt_driveId).val(data.candidateRequestModel.driveId);
            $(txt_rollNo).val(data.candidateRequestModel.rollNo);
            $(branches_list).selectpicker('val', data.candidateRequestModel.branch);

            SportaForms.InitializeFormStyle(candidateForm)
        });
        openSidebar();
    }
}

// Validating Blank Checks in Form
function validateUser() {
    SportaForms.ClearValidataionErrors(candidateForm);

    var blankChecks = [txt_name, txt_contact_no];
    SportaForms.BlankInputChecks(blankChecks);
    return SportaForms.FormValidationStatus(candidateForm);
}

// Closing Edit Form
function closeEditForm() {
    SportaForms.ResetForm(candidateForm);
    closeSideBar();
}


$(candidateForm).unbind('submit').bind('submit', function (e) {
    e.preventDefault();
    SportaForms.EnableLiveValidation(candidateForm, validateUser);
    if (validateUser()) {
        $.ajax({
            url: updateCandidateUrl,
            data: $(this).serialize(),
            type: 'POST',
            success: function (response) {
                if (response.isSuccess) {
                    SportaUtil.MessageBoxSuccess(`${response.data} as ${response.message}`);
                    closeEditForm()
                    loadcandidates()
                }
                else {
                    SportaUtil.MessageBoxDanger(response.message)
                }
            },
            error: function (resposne) {

            }
        });

    }
});