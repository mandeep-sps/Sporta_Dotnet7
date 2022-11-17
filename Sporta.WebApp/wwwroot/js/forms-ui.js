//Applying floating label and styles to form inputs
function applyFormInputStyle(formId, focus, colorScheme) {
    var defaults = {
        formId: 'form',
        focus: true,
        colorScheme: 'theme'
    };

    var userInputs = {
        formId: formId,
        focus: focus,
        colorScheme: colorScheme
    }
    userInputs = $.extend(defaults, userInputs);


    //enable bootstrap-select styling for all select input
    $(formId + ' select').selectpicker();
    //for .form-controls class
    $('.form-group .form-control').focus(function () {
        $(this).parent().find('label.floating-label').addClass('has-value has-value-' + userInputs.colorScheme + '');
        $(this).addClass('has-value-input has-value-input-' + userInputs.colorScheme + '');

    }).trigger('focus');

    $('.form-group .form-control').blur(function () {
        if (!$(this).val()) {
            $(this).parent().find('label:not(.label-date)').removeClass('has-value has-value-' + userInputs.colorScheme + '');
        }
        $(this).removeClass('has-value-input has-value-input-' + userInputs.colorScheme + '');
    }).trigger('blur');



    //for bootstrap-select elements
    $('.form-group .bootstrap-select .btn-light').focus(function () {
        $(this).parents('.form-group').find('label.floating-label').addClass('has-value has-value-' + userInputs.colorScheme + '');
        $(this).addClass('has-value-input has-value-input-' + userInputs.colorScheme + '');
    }).trigger('focus');

    $('.form-group .bootstrap-select .btn-light').blur(function () {
        $(this).removeClass('has-value-input has-value-input-' + userInputs.colorScheme + '');
    }).trigger('blur');



    //setting focus on 1st visible input on form
    if (userInputs.focus)
        $(userInputs.formId + ' input:enabled:visible:first').focus();

    $('[data-toggle="tooltip"]').tooltip();
    $('[data-toggle="popover"]').popover();

    // event to change file name in text control
    $('.custom-file-input').on('change', function () {
        var fileName = $(".custom-file-input").val().split("\\").pop();
        $(".custom-file-input").siblings(".custom-file-label").addClass("selected").html(fileName);
    });

}



//Toggle switch types : yes/no, active/in-active, on/off
function switchStatus(switchId, switchType) {
    switch (switchType) {
        case eSwitchType.Active_InActive:
            if ($(switchId).prop('checked')) {
                $(switchId+'-label').html('<strong><span class="text-success">Active</span></strong>');
                $(switchId).val(true);
            }
            else {
                $(switchId + '-label').html('<strong><span class="text-danger">In-Active</span></strong>').val(false);
                $(switchId).val(false);

            }
            break;

        case eSwitchType.Yes_No:
            if ($(switchId).prop('checked')) {
                $(switchId + '-label').html('<strong><span class="text-success">Yes</span></strong>');
                $(switchId).val(true);
            }
            else {
                $(switchId + '-label').html('<strong><span class="text-danger">No</span></strong>').val(false);
                $(switchId).val(false);

            }
            break;

        case eSwitchType.Yes_No:
            if ($(switchId).prop('checked')) {
                $(switchId + '-label').html('<strong><span class="text-success">On</span></strong>');
                $(switchId).val(true);
            }
            else {
                $(switchId + '-label').html('<strong><span class="text-danger">Off</span></strong>').val(false);
                $(switchId).val(false);

            }
            break;
    }

}


//
//  ********** Form Validations ***********
//

//Showing validation messages: (must use div with ".invalid-input-feedback" class beside inputs/controls)
function formValidated(formId) {
    $(formId + ' input.form-control, ' + formId + ' textarea.form-control').addClass('valid-input');
    $(formId + ' select').siblings('button').addClass('valid-input');
    $('.invalid-input').siblings('.invalid-input-feedback').show();
    $('.invalid-input').parent().siblings('.invalid-input-feedback').show();

    formStatus(formId);
}

function formStatus(formId) {
    if ($(formId + ' .form-control, ' + formId + ' button').hasClass('invalid-input'))
        disableSubmit(formId);
    else
        enableSubmit(formId);
}


function disableSubmit(formId) {
    $(formId + ' button[type="submit"]').prop('disabled', true);
}

function enableSubmit(formId) {
    $(formId + ' button[type="submit"]').prop('disabled', false)
}

//Live validations of inputs (create body for validateForm() function in your views for specific validations)
function addLiveFormValidation(formId, validationFuncion) {
    $(formId + ' .form-control').on('input.validate change.validate', function () {
        validationFuncion();
    });
}

//Removing validation sytyles
function removeValidation(formId) {
    $('select').siblings('button').removeClass('invalid-input valid-input');
    $('.form-control').removeClass('invalid-input valid-input');
    $(formId + ' .invalid-input-feedback').hide();
}



function addRemoveValidations(inputId, condition) {
    condition ? addInvalidClass(inputId) : removeInValidClass(inputId);
}

function invalidIfBlank(controls) {
    var radioChecked = false;
    $.each(controls, function (index, inputId) {
        if ($(inputId).is('select')) {
            addRemoveValidations(inputId, !$(inputId + ' option:selected').val());
        }
        else if ($(inputId).is('input[type="radio"]')) {
            $($(inputId)).each(function () {
                if ($(this).prop('checked'))
                    radioChecked = true;
            });
            addRemoveValidations(inputId, !radioChecked)
        }
        else
            addRemoveValidations(inputId, !$(inputId).val());
    });
}

function addInvalidClass(inputId) {
    if ($(inputId).is('select'))
        $(inputId).siblings('button').addClass('invalid-input').removeClass('valid-input')
    else
        $(inputId).addClass('invalid-input').removeClass('valid-input')
}


function removeInValidClass(inputId) {
    if ($(inputId).is('select'))
        $(inputId).siblings('button').removeClass('invalid-input').addClass('valid-input');
    else
        $(inputId).removeClass('invalid-input').addClass('valid-input');
}


function formValidStatus(formId) {
    return !$(formId + ' .form-control, ' + formId + ' button').hasClass('invalid-input');
}


function offLiveValidate(formId) {
    $(formId + ' .form-control').off('input.validate change.validate submit');
}

//Resetting/Clearing form inputs
function resetFormInputs(formId) {

    $(formId).each(function () {
        this.reset();
    });
    $('select').val('').selectpicker('refresh');
    $(".custom-file-input").val('');
    $(".custom-file-input").siblings(".custom-file-label").removeClass("selected").html('Choose file');

    //removing applied styles
    removeValidation(formId);
    offLiveValidate(formId)
    $('button[type="submit"]').text('Save').prop('disabled', false);
    $(formId + ' :input:enabled:visible:first').focus();
}

