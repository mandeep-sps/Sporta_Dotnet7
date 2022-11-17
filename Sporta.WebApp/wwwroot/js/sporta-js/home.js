
let txt_login_emailID = '#txt-login-email-id',
    txt_login_password = '#txt-login-password';

let name = '#txt-name',
    roll_no = '#txt-rollNo',
    txt_email = '#txt-email-id',
    txt_contactNo = '#txt-contact-no',
    batch = '#branches-list',
    txt_token = '#txt-token',
    sign_up_form_id = '#signup-form',
    login_form_id = '#login-form';


$(document).ready(function () {
    $('#preloader').hide();
    SportaForms.InitializeFormStyle(login_form_id);

    $.get('/ApplicationUser/GetBranches', function (response) {
        $(batch).empty();       
        $.each(response.data, function (index, item) {
            $(batch).append("<option value=" + item.name + ">" + item.name + "</option>");
        });
    });

});


// visibilty of password
$("body").on('click', '.toggle-password', function () {
    $(this).toggleClass("fa-eye fa-eye-slash");
    var input = $("#txt-login-password");

    if (input.attr("type") === "password") {
        input.attr("type", "text");
    } else {
        input.attr("type", "password");
    }
});


// Singup Submit
$(sign_up_form_id).unbind().bind('submit', function (e) {
    e.preventDefault();
    SportaForms.EnableLiveValidation(sign_up_form_id, validateSignupUser)
    if (validateSignupUser(sign_up_form_id)) {
        $.ajax({
            url: '/ApplicationUser/Signup',
            data: $(this).serialize(),
            type: 'POST',
            beforeSend: function () { $('#preloader').show(); },
            success: function (response) {
                $('#preloader').hide();
                if (response.isSuccess) {
                    Login(response.data);
                }
                else {
                    SportaUtil.MessageBoxDanger(response.message);
                }
            },
            error: function (xhr, error, errorMessage) {
                SportaUtil.MessageBoxDanger(errorMessage);
            },
            complete: function () { $('#preloader').hide(); }
        });

    }
});


// need to optimize
function contactNoValidation(str) {
    var aa = /^\d{10}$/.test($(txt_contactNo).val());
    if (isNaN(Number($(txt_contactNo).val()))) {
        // $(txt_contactNo).val('');
        $(txt_contactNo).siblings('.invalid-input-feedback').show().text('Please enter only digits');
    }
    else if ($(txt_contactNo).val().length >= 10) {
        // $(txt_contactNo).attr('maxlength', 10);//[0-9]{9}
    }
    else {
        $(txt_contactNo).siblings('.invalid-input-feedback').hide().text('');
    }
}

// validating login user form blank checks
function validateLoginUser() {
    SportaForms.ClearValidataionErrors(login_form_id);
    var blankChecks = [txt_login_emailID, txt_login_password];
    SportaForms.BlankInputChecks(blankChecks);
    return SportaForms.FormValidationStatus(login_form_id);
}

// validating signup user form
function validateSignupUser() {
    SportaForms.ClearValidataionErrors(sign_up_form_id);
    var blankChecks = [name, txt_email, txt_contactNo, txt_token, batch];
    SportaForms.BlankInputChecks(blankChecks);

    if ($(txt_email).val()) {
        SportaForms.ValidateInput(txt_email,
            !/^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/
                .test($(txt_email).val()),
            "Enter a valid email Id");
    }

    if ($(txt_contactNo).val()) {
        SportaForms.ValidateInput(txt_contactNo,

            ! /^\d{10}$/.test($(txt_contactNo).val()),
            "Enter a valid mobile number");
    }

    return SportaForms.FormValidationStatus(sign_up_form_id);
}


// Login Submit
$(login_form_id).unbind().bind('submit', function (e) {
    e.preventDefault();
    SportaForms.EnableLiveValidation(login_form_id, validateLoginUser)
    var data = $(this).serialize();
    if (validateLoginUser(login_form_id)) {
        Login(data);
    }
})

// View SignUp form
function showSignUpForm() {
    $('.login-form-container').css('display', 'none');
    $('.signup-form-container').css('display', 'flex');
}

// View Login Form
function showLoginForm() {
    $('.login-form-container').css('display', 'flex');
    $('.signup-form-container').css('display', 'none');
}

// Login Function
function Login(data) {
    $.ajax({
        url: '/ApplicationUser/Login',
        data: data,
        type: 'POST',
        beforeSend: function () { $('#preloader').show(); },
        success: function (response) {
            $('#preloader').hide();
            if (response.isSuccess) {
                return location.replace(response.url);
            }
            else {
                SportaUtil.MessageBox(response.message, { type: response.type ? response.type : 'danger', heading: response.type ? "Information" : "Error" });
            }
        },
        error: function (xhr, error, errorMessage) {
            SportaUtil.MessageBoxDanger(errorMessage);
        },
        complete: function () { $('#preloader').hide(); }
    });
}
