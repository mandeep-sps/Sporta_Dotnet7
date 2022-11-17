//CRUD URLs
let getAllUserUrl = "/ApplicationUser/GetAllUsers",
    getAllActiveExecutives = "/ApplicationUser/GetAllActiveExecutives",
    getUserUrl = "/ApplicationUser/UserDetails",
    addUserUrl = "/ApplicationUser/AddUser",
    updateUserUrl = "/ApplicationUser/UpdateUser",
    deleteUserUrl = "/ApplicationUser/RemoveUser",
    navbar_users = 'navbar-users';

//UI varaibles
let form_id = "#user-form",
    application_user_list = "#application-user-list";

//Forms
let txt_id = "#txt-id",
    txt_name = "#txt-name",
    txt_email = "#txt-email-id",
    btn_create_new = "#btn-create-new",
    btn_submit = "#btn-submit";


$(document).ready(function () {
    toogleNavbar(navbar_users);
    loadApplicationUsers();
});

// Downloading Executives Details
function DownloadExecutivesDetails() {
    $.ajax({
        url: getAllActiveExecutives,
        type: 'Get',
        success: function (response) {
            if (response.isSuccess) {
                JSONToCSVConvertor(response.data)
            }
            else {

                SportaUtil.MessageBoxDanger(response.message)
            }
        },
        error: function (resposne) {
            alert(resposne);
        }
    });
}
function JSONToCSVConvertor(JSONData) {

    //If JSONData is not an object then JSON.parse will parse the JSON string in an Object
    var arrData = typeof JSONData != 'object' ? JSON.parse(JSONData) : JSONData;
    var CSV = '';

    var row = "";

    //This loop will extract the label from 1st index of on array
    for (var index in arrData[0]) {

        //Now convert each value to string and comma-seprated
        row += index.toUpperCase() + ',';

    }

    row = row.slice(0, -1);

    //append Label row with line break
    CSV += row + '\r\n';


    //1st loop is to extract each row
    for (var i = 0; i < arrData.length; i++) {
        var row = "";
        //2nd loop will extract each column and convert it in string comma-seprated
        for (var index in arrData[i]) {
            row += '"' + arrData[i][index] + '",';
        }

        row.slice(0, row.length - 1);

        //add a line break after each row
        CSV += row + '\r\n';
    }

    if (CSV == '') {
        alert("Invalid data");
        return;
    }

    //this will remove the blank-spaces from the title and replace it with an underscore
    var fileName = "ExecutivesDetails_Sporta";

    //Initialize file format you want csv or xls
    var uri = 'data:text/csv;charset=utf-8,' + escape(CSV);
   
    var link = document.createElement("a");
    link.href = uri;

    //set the visibility hidden so it will not effect on your web-layout
    link.style = "visibility:hidden";
    link.download = fileName + ".csv";

    //this part will append the anchor tag and remove it after automatic click
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
}


//CRUD Functions
// Add and update a User
function addUpdateUser(data) {
    var url = data.Id == 0 ? addUserUrl : updateUserUrl;
    $.ajax({
        url: url,
        data: data,
        type: 'POST',
        success: function (response) {
            if (response.isSuccess) {
                SportaUtil.MessageBoxSuccess(`${response.data.name} as ${response.message}`);
                loadApplicationUsers();
                closeUserForm();
            }
            else {
                SportaUtil.MessageBoxDanger(response.message)
            }
        },
        error: function (resposne) {

        }
    });
}

// Delete a User

function deleteUser(id, username, roleId) {
    SportaUtil.ConfirmDialogue(`Are you sure  to delete ${username}?`, null, function () {
        $.get(deleteUserUrl, { 'userId': id, 'roleId': roleId }, function (response) {            
            if (response.isSuccess)
                SportaUtil.MessageBoxSuccess(response.message);
            else 
                SportaUtil.MessageBoxDanger(response.message);
            
            loadApplicationUsers()
        })
    })
}

// Validating u user form 
function validateUser() {
    SportaForms.ClearValidataionErrors(form_id);

    var blankChecks = [txt_name, txt_email];
    SportaForms.BlankInputChecks(blankChecks);
    return SportaForms.FormValidationStatus(form_id);
}

// binding All Users to a Grid
function loadApplicationUsers() {
    $.get(getAllUserUrl, function (response) {
        var tableColumns = [
            { data: "name" },
            { data: "username" },
            { data: 'applicationRole' },
            { data: null, render: usersActionButtons }
        ]
        SportaDataTable.CreateDataTable(application_user_list, response.data, tableColumns)
    });
}

// opening Create User Form Or Edit User Form
function openUserForm(id, roleId) {
    SportaForms.ResetForm(form_id);
    if (id) {
        $.get(getUserUrl, { 'userId': id, 'roleId': roleId }, function (response) {
            var data = response.data;
            $(txt_id).val(data.id);
            $(txt_name).val(data.name);
            $(txt_email).val(data.username).prop('readonly', true);
            $(btn_submit).text('Update')
            $(form_id).fadeIn();
            SportaForms.InitializeFormStyle(form_id)
        })
    }
    else {
        $(txt_id).val(0);
        $(txt_email).prop('readonly', false);
        $(btn_submit).text('Save')

        $(form_id).fadeIn();
        SportaForms.InitializeFormStyle(form_id)
    }

}

function openExecutiveDetail(ExecutiveId) {
    $.get("/ApplicationUser/GetExecutiveDetailById", "ExecutiveId=" + ExecutiveId, function (response) {
        if (response.data == null) {
            $("#tableBody").empty();
            $("#executive_name").hide();
            $("#tableBody").append("<h4>" + response.message + "</h4>");
            $("#tableModal").modal('show');
        }
        else {
            var len = response.data.length, data = response.data;
            var result = "";
            $("#tableBody").empty();
            $("#executive_name").show();
            $("#executive_name").html(data[0].executiveName);
            for (var i = 0; i < len; i++) {
                result = "";
                result = result + "<tr>";
                result = result + "<td>" + "<a target=blank data-toggle=tooltip title=" + data[i].driveName + " drive enrolled " + data[i].enrolledCount + " candidates href=/Report/Index/" + data[i].driveId + ">" + data[i].driveName + "</a>" + "</td>";
                result = result + "<td>" + response.data[i].scheduledTime + "</td>";
                result = result + "<td>" + "<a target=blank data-toggle=tooltip title=" + data[i].enrolledCount + " drive enrolled "   + data[i].enrolledCount+ "candidates href=/ApplicationUser/Candidates/"+ data[i].driveId +">"+data[i].enrolledCount+"</a>" + "</td>";
                result = result + "<td>" + response.data[i].token + "</td>";
                result = result + ("</tr>");
                $("#tableBody").append(result);
            }
        }
        });
    $("#tableModal").modal('show');
        
}

// Closing User Form
function closeUserForm() {
    SportaForms.ResetForm(form_id);
    $(txt_id).val(0);
    $(txt_email).prop('readonly', false);
    $(btn_submit).text('Save')
    $(form_id).fadeOut();
    $(btn_create_new).show();
}

// Buttons Added to Grid
function usersActionButtons(data, type, column) {
    var _buttons = '<div class="btn-group">'
        + `<button class="btn btn-sm btn-outline-grey" onclick="openUserForm('${data.id}','${data.applicationRoleId}')"><i class="fas fa-pencil-alt"></i></button>`
        + `<button class="btn btn-sm btn-outline-grey" onclick="deleteUser('${data.id}','${data.username}','${data.applicationRoleId}')"><i class="far fa-trash-alt"></i></button>`
        + `<button class="btn btn-sm btn-outline-theme" data-toggle="tooltip" data-placement="top" title="Detail view of result" onclick="openExecutiveDetail('${data.id}')"><i class="fas fa-info-circle"></i></button>`
        + `</div>`;
    return _buttons;
}


//Events
$(form_id).unbind('submit').bind('submit', function (e) {
    e.preventDefault();
    SportaForms.EnableLiveValidation(form_id, validateUser);
    if (validateUser()) {
        var requestModel = {
            Id: $(txt_id).val(),
            Name: $(txt_name).val(),
            EmailId: $(txt_email).val(),
            ApplicationRoleId: eApplicationUserRoles.Executive,
            UserPassword: 'softprodigy'
        }
        if (validateEmail($(txt_email).val())) {
            addUpdateUser(requestModel);
        } else {
            SportaUtil.MessageBoxDanger('Please enter valid email!')
        }
    }
});

// Validate Email is valid
function validateEmail(email) {
    const re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(email);
}