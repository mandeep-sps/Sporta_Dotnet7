//Enums
const eSwitchType = Object.freeze({
    "Active_InActive": 1,
    "Yes_No": 2,
    "On_Off": 3
});


//********************** DATATABEL START **************************************//
// Application for DataTable Version 1.10.21

class SportaDataTable {
    constructor() { }
    static CreateDataTable(tableId, tableData, tableColumns, settings) {
        $.fn.DataTable.ext.pager.numbers_length = 5;
        //Set Defaults
        var defaults = {
            tableId: tableId,       //id to bind datatable,
            exportButton: true,     //To enable/disable pdf and excel export buttons
            initialOrder: true,     //To maintain server side sorting initially
            showPaging: true,       //Enable/disable pagination
            orderBy: 0,             //set order by column index for initial ordering (initialOrder must be true)
            orderSequence: 'asc',   //specify order direction
            tableHeight: false,     //table height (either boolean value or set specific height in px/%/vh)
            enableSearch: true,     //Enable/Disable search(filter) option.
            columnWiseSearch: false, //If yes then  <tfoot> element need to be added in the table with the same number of columns as <thead> 
            showTableInfo: true,    //Enable table summary 
            noOfFixedRightColumns: 0,
            responsive: true,
            exportFileName: '',
        };
        if (tableData.length == 0) {
            defaults = {
                tableId: tableId,       //id to bind datatable,
                exportButton: false,     //To enable/disable pdf and excel export buttons
                initialOrder: true,     //To maintain server side sorting initially
                showPaging: true,       //Enable/disable pagination
                orderBy: 0,             //set order by column index for initial ordering (initialOrder must be true)
                orderSequence: 'asc',   //specify order direction
                tableHeight: false,     //table height (either boolean value or set specific height in px/%/vh)
                enableSearch: true,     //Enable/Disable search(filter) option.
                columnWiseSearch: false, //If yes then  <tfoot> element need to be added in the table with the same number of columns as <thead> 
                showTableInfo: true,    //Enable table summary 
                noOfFixedRightColumns: 0,
                responsive: true,
                exportFileName: '',
            }
        }
        settings = $.extend(defaults, settings);
        var dom_pagination = '';
        var dom_button = '';
        var dom_search = '';
        var dom_info = '';
        var dom_bottom_position = '';
        var dom_top_position = '';
        var dark_mode = '';
        //If search is disable then column wise search option will also disable
        if (!settings.enableSearch)
            settings.columnWiseSearch = false;


        if (tableData && !tableColumns)
            alert('Columns are not specified for table having an Id : ' + settings.tableId);
        else if (!tableData && tableColumns)
            alert('Data not provided for table having an Id : ' + settings.tableId);



        //Declaring DOM elements for table refer https://datatables.net/reference/option/dom for more info
        if (settings.showPaging)
            dom_pagination = "p";

        if (settings.exportButton)
            dom_button = "B";

        if (!settings.columnWiseSearch)
            dom_search = "f";

        if (settings.showTableInfo)
            dom_info = "i";



        //Maintaining the position of dom elements
        if (settings.showPaging && settings.exportButton)
            dom_bottom_position = '"d-sm-flex justify-content-sm-between"';
        else
            dom_bottom_position = '"d-sm-flex justify-content-sm-end"';


        if (!settings.columnWiseSearch && settings.showTableInfo)
            dom_top_position = '"d-sm-flex justify-content-sm-between"';
        else
            dom_top_position = '"d-sm-flex justify-content-sm-start"';


        //Customization for dark theme table
        if ($(settings.tableId).hasClass('table-dark')) {
            dark_mode = 'bg-dark text-white border-secondary'
        };


        //Adding search input in the <tfoot> element
        if (settings.columnWiseSearch) {
            $(settings.tableId + ' tfoot th').each(function () {
                if ($(this).text() !== '')
                    $(this).html('<input class="form-control form-control-sm column-search-button ' + dark_mode + '" style="border:0;border-bottom:2px solid #d8d8d8" type="text" placeholder="Search" />');
            });
        } else {
            $(settings.tableId + ' tfoot').remove();
        }

        if (tableData) {
            //Initializing
            var table = $(settings.tableId).DataTable({
                destroy: true,                   //To destory previous initialized table of same id
                processing: true,
                responsive: settings.responsive,
                data: tableData,
                //scrollX: true,                   // enable horzontal scrollbar
                //scrollY: settings.tableHeight,
                paging: settings.showPaging,
                //fixedHeader: true,               // if true, header will not move while scrolling horizontally.
                scrollCollapse: false,            //Table height will change dynamically as per table rows (will not exceed max height set in tableHeight property).
                autoWidth: true,
                dom: "<" + dom_top_position + dom_info + dom_search + ">t<" + dom_bottom_position + dom_button + dom_pagination + ">",
                searching: settings.enableSearch,
                buttons: [{
                    extend: 'pdf',
                    title: settings.exportFileName,
                    text: '<span title="Download PDF"><i class="far fa-file-pdf fa-2x"></i></span>',
                    pageSize: 'A4',
                    orientation: 'portrait',
                    exportOptions: {
                        columns: "thead th:not(.doNotExport)"
                    },
                },
                {
                    extend: 'excel',
                    title: settings.exportFileName,
                    text: '<span title="Download Excel"><i class="far fa-file-excel fa-2x"></i></span>',
                    exportOptions: {
                        columns: "thead th:not(.doNotExport)"
                    },
                },
                ],
                columns: tableColumns,
                //fixedColumns: {
                //    leftColumns: 0,
                //    rightColumns: settings.noOfFixedRightColumns
                //},


                initComplete: function () {
                    var btns = $('.buttons-html5');
                    var excelbtn = $('.buttons-excel');
                    btns.removeClass('btn-secondary buttons-copy buttons-html5 buttons-excel').addClass('btn btn-sm btn-outline-danger border-0');
                    excelbtn.removeClass('btn-outline-danger').addClass('btn-outline-success border-0');

                },
                drawCallback: function (settings) {
                    $('[data-toggle="tooltip"]').tooltip();
                }

            });
        }
        else {
            //Initializing
            var table = $(settings.tableId).DataTable({
                destroy: true,                   //To destory previous initialized table of same id
                processing: true,
                responsive: settings.responsive,
                // scrollX: true,                   // enable horzontal scrollbar
                // scrollY: settings.tableHeight,
                paging: settings.showPaging,
                //fixedHeader: false,               // if true, header will not move while scrolling horizontally.
                scrollCollapse: false,            //Table height will change dynamically as per table rows (will not exceed max height set in tableHeight property).
                autoWidth: true,
                dom: "<" + dom_top_position + dom_info + dom_search + ">t<" + dom_bottom_position + dom_button + dom_pagination + ">",
                searching: settings.enableSearch,
                buttons: [{
                    extend: 'pdf',
                    text: '<span title="Download PDF"><i class="far fa-file-pdf fa-2x"></i></span>',
                    pageSize: 'A4',
                    orientation: 'portrait',
                    exportOptions: {
                        columns: "thead th:not(.doNotExport)"
                    },
                },
                {
                    extend: 'excel',
                    text: '<span title="Download Excel"><i class="far fa-file-excel fa-2x"></i></span>',
                    exportOptions: {
                        columns: "thead th:not(.doNotExport)"
                    },
                },
                ],


                initComplete: function () {
                    var btns = $('.buttons-html5');
                    var excelbtn = $('.buttons-excel');
                    btns.removeClass('btn-secondary buttons-copy buttons-html5 buttons-excel').addClass('btn btn-sm btn-outline-danger border-0');
                    excelbtn.removeClass('btn-outline-danger').addClass('btn-outline-success border-0');

                },
                drawCallback: function (settings) {
                    $('[data-toggle="tooltip"]').tooltip();
                }
            });
        }


        //function to order initially
        if (settings.initialOrder)
            table.order([settings.orderBy, settings.orderSequence]).draw();
        else
            table.order([]).draw();



        //function to search column wise
        table.columns().every(function () {
            var columnSearched = this;
            $('input.column-search-button', this.footer()).on('keyup change', function () {
                if (columnSearched.search() !== this.value) {
                    columnSearched
                        .search(this.value)
                        .draw();
                }
            });
        });

        table.scroller.measure();
        table.columns.adjust();

        return table;
    }


}

//********************** DATATABEL START END **************************************//



//********************** FORMS START **************************************//
class SportaForms {
    constructor() { }

    static InitializeFormStyle(formId, focus, colorScheme) {
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

        //Overriding default values
        userInputs = $.extend(defaults, userInputs);


        //enable bootstrap-select styling for all select input
        if ($(formId + ' select.selectpicker').length > 0) {
            $(formId + ' select.selectpicker').selectpicker();
        }

        //adding style on input and labels having .form-control class
        $('.form-group .form-control').focus(function () {
            $(this).parent().find('label.floating-label').addClass('has-value has-value-' + userInputs.colorScheme + '');
            $(this).addClass('has-value-input has-value-input-' + userInputs.colorScheme + '');
        }).trigger('focus');

        //removing style on input and labels having .form-control class
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

        //Initializing bootstrap's tooltip and popover functionality.
        $('[data-toggle="tooltip"]').tooltip();
        $('[data-toggle="popover"]').popover();


        // event to change file name in text control
        $('.custom-file-input').on('change', function () {
            var fileName = $(".custom-file-input").val().split("\\").pop();
            $(".custom-file-input").siblings(".custom-file-label").addClass("selected").html(fileName);
        });

    }


    //Toggle switch types : yes/no, active/in-active, on/off
    static ToggleSwitch(switchId, switchType, callback) {
        var label = '',
            className = '';
        switch (switchType) {
            case eSwitchType.Active_InActive:
                label = $(switchId).prop('checked') ? 'Active' : 'In-Active';
                break;

            case eSwitchType.Yes_No:
                label = $(switchId).prop('checked') ? 'Yes' : 'No';
                break;

            case eSwitchType.On_Off:
                label = $(switchId).prop('checked') ? 'On' : 'Off';
                break;

        }
        className = $(switchId).prop('checked') ? 'text-success' : 'text-danger';
        $(switchId + '-label').html('<strong><span class="' + className + '">' + label + '</span></strong>');
        callback()
    }

    //Showing validation messages: (must use div with ".invalid-input-feedback" class beside inputs/controls)
    static FormValidationStatus(formId) {
        $(formId + ' input.form-control, ' + formId + ' textarea.form-control').addClass('valid-input');
        $(formId + ' select').siblings('button').addClass('valid-input');
        $('.invalid-input').siblings('.invalid-input-feedback').show();
        $('.invalid-input').parent().siblings('.invalid-input-feedback').show();


        if ($(formId + ' .form-control, ' + formId + ' button').hasClass('invalid-input'))
            $(formId + ' button[type="submit"]').prop('disabled', true);
        else
            $(formId + ' button[type="submit"]').prop('disabled', false)


        return !$(formId + ' .form-control, ' + formId + ' button').hasClass('invalid-input');

    }

    static ClearValidataionErrors(formId) {
        $('select').siblings('button').removeClass('invalid-input valid-input');
        $('.form-control').removeClass('invalid-input valid-input');
        $(formId + ' .invalid-input-feedback').html('');
    }

    static ValidateInput(inputId, condition, message) {
        condition ? this.AddValidation(inputId, message) : this.RemoveValidation(inputId);
    }

    //Live validations of inputs (create body for validateForm() function in your views for specific validations)
    static EnableLiveValidation(formId, validationFuncion) {
        $(formId + ' .form-control').on('input.validate change.validate', function () {
            validationFuncion();
        });
    }


    static BlankInputChecks(controls) {
        var radioChecked = false;
        $.each(controls, function (index, inputId) {
            if ($(inputId).is('select')) {
                SportaForms.ValidateInput(inputId, !$(inputId + ' option:selected').val(), "Choose at least one from the list");
            }
            else if ($(inputId).is('input[type="radio"]')) {
                $($(inputId)).each(function () {
                    if ($(this).prop('checked'))
                        radioChecked = true;
                });

                SportaForms.ValidateInput(inputId, !radioChecked, "Choose any option!")
            }
            else
                SportaForms.ValidateInput(inputId, !$(inputId).val(), "This field can not be blank!");
        });
    }


    static AddValidation(inputId, message) {
        if ($(inputId).is('select')) {
            $(inputId).siblings('button').addClass('invalid-input').removeClass('valid-input')
            $(inputId).parent().siblings('div.invalid-input-feedback').html(message);
        }
        else {
            $(inputId).addClass('invalid-input').removeClass('valid-input')
            $(inputId).siblings('div.invalid-input-feedback').html(message);
        }

    }

    static RemoveValidation(inputId) {
        if ($(inputId).is('select'))
            $(inputId).siblings('button').removeClass('invalid-input').addClass('valid-input');
        else
            $(inputId).removeClass('invalid-input').addClass('valid-input');

        $(inputId).siblings('div.invalid-input-feedback').html('');
    }

    static DisableLiveValidation(formId) {
        $(formId + ' .form-control').off('input.validate change.validate submit');
    }


    //Resetting/Clearing form inputs
    static ResetForm(formId) {
        $(formId).each(function () {
            this.reset();
        });
        $(formId + ' select.selectpicker').val('').selectpicker('refresh');
        $(".custom-file-input").val('');
        $(".custom-file-input").siblings(".custom-file-label").removeClass("selected").html('Choose file');

        //removing applied styles
        this.ClearValidataionErrors(formId);
        this.DisableLiveValidation(formId)
        $('button[type="submit"]').prop('disabled', false);
        $(formId + ' :input:enabled:visible:first').focus();
    }


}

//********************** FORMS END **************************************//

class SportaUtil {
    constructor() { }
    static MessageBox(message, settings, callback, callback_2) {
        //Default Settings/Parameters values
        var defaults = {
            modalId: '#sporta-modal-dialog',
            divId: '#sporta-modal-dialog-div', //add div element with this id for injection.
            modalSize: 'modal-md', //variants: modal-md(medium size), modal-lg(large size)
            type: 'info', // four types (int/string): 1/'success', 2/'error', 3/'warinng', 4/'info'
            heading: 'Message Box',
            heading_icon: '',
            message: message,
            buttonText: ['OK'],
        };

        let globalSettings = $.extend(defaults, settings);
        //To remove any previous shown popup shadow    
        $('.modal-backdrop').remove();
        $('.customized').remove();

        let icon,
            btnClass,
            borderClass


        //Type Settings
        switch (globalSettings.type) {
            case 'success':
            case 1:
                if (!globalSettings.heading)
                    globalSettings.heading = "Successful!";

                if (!globalSettings.heading_icon)
                    globalSettings.heading_icon = "<i class='text-success fas fa-check-circle'></i> ";

                btnClass = "btn-outline-success";
                icon = "<h4>" + globalSettings.heading_icon + "&nbsp; " + globalSettings.heading + "</h4>";
                borderClass = 'border-success';
                break;

            case 'danger':
            case 'error':
            case 2:
                if (!globalSettings.heading)
                    globalSettings.heading = "Error!";

                if (!globalSettings.heading_icon)
                    globalSettings.heading_icon = "<i class='text-red fas fa-times-circle'></i> ";

                btnClass = 'btn-outline-red';
                icon = "<h4>" + globalSettings.heading_icon + "&nbsp; " + globalSettings.heading + "</h4>";
                borderClass = 'border-red';
                break;

            case 'warning':
            case 3:
                if (!globalSettings.heading)
                    globalSettings.heading = "Warning!";

                if (!globalSettings.heading_icon)
                    globalSettings.heading_icon = "<i class='text-warning fas fa-exclamation-circle'></i> ";

                btnClass = 'btn-outline-warning';
                icon = "<h4>" + globalSettings.heading_icon + "&nbsp; " + globalSettings.heading + "</h4>";
                borderClass = 'border-warning';
                break;

            case 'info':
            case 4:
                if (!globalSettings.heading)
                    globalSettings.heading = "Information";

                if (!globalSettings.heading_icon)
                    globalSettings.heading_icon = "<i class='text-info fas fa-info-circle'></i> ";


                btnClass = 'btn-outline-info';
                icon = "<h4>" + globalSettings.heading_icon + "&nbsp; " + globalSettings.heading + "</h4>";
                borderClass = 'border-info';
                break;
        }

        //Modal Creation
        var modalHtml = "<div id='" + globalSettings.modalId.replace('#', '') + "' class='modal customized shadow' style='z-index:9990' role='dialogue'>"
            + "<div class='modal-dialog modal-dialog-centered modal-dialog-scrollable " + globalSettings.modalSize + "'>"
            + "<div class='modal-content'>"
            + "<div class='modal-header justify-content-center " + borderClass + "'>"
            + "" + icon + ""
            + "</div>"
            + "<div class='modal-body text-center'>"
            + "<p>" + globalSettings.message + "</p>"
            + "</div>";

        modalHtml += "<div class='modal-footer justify-content-center " + borderClass + "'>";
        switch (globalSettings.buttonText.length) {
            case 1:
                modalHtml += "<button type='button' id='btn-callback'  class='btn btn-sm " + btnClass + "' style='border-radius:5px!important;width:25%' data-dismiss='modal'>" + globalSettings.buttonText[0] + "</button>";
                break;

            case 2:
                modalHtml += "<button type='button' id='btn-callback' class='btn btn-sm " + btnClass + "' style='border-radius:5px!important;width:25%' data-dismiss='modal'>" + globalSettings.buttonText[0] + "</button > "
                    + "<button type='button'  class='btn btn-sm btn-outline-secondary'  style='border-radius:5px!important;width:25%' data-dismiss='modal'>" + globalSettings.buttonText[1] + "</button > ";
                break;

            case 3:
                modalHtml += "<button type='button' id='btn-callback' class='btn btn-sm " + btnClass + "' style='border-radius:5px!important;width:25%' data-dismiss='modal'>" + globalSettings.buttonText[0] + "</button > "
                    + "<button type='button' id='btn-callback-2' class='btn btn-sm " + btnClass + "' style='border-radius:5px!important;width:25%' data-dismiss='modal'>" + globalSettings.buttonText[1] + "</button > "
                    + "<button type='button'  class='btn btn-sm btn-outline-secondary'  style='border-radius:5px!important;width:25%' data-dismiss='modal'>" + globalSettings.buttonText[2] + "</button > ";
                break;

        }
        modalHtml += "</div>"
            + "</div>"
            + "</div>"
            + "</div>";

        //Injecting the Modal in div
        $(globalSettings.divId).html(modalHtml);
        $(globalSettings.modalId).modal({ backdrop: 'static', keyboard: true });
        $(globalSettings.modalId).modal('show');


        //Setting the focus on load
        $(globalSettings.modalId).on('shown.bs.modal', function (event) {
            $("#btn-callback").focus();
        });


        $('#btn-callback').on('click', function () {
            if (callback !== undefined)
                callback();
            else
                $(globalSettings.modalId).modal('hide');
        });


        $('#btn-callback-2').on('click', function () {
            if (callback_2 !== undefined)
                callback_2();
            else if (globalSettings.buttonText.length > 2)
                alert('Callback function is not defined for' + globalSettings.buttonText[1] + ' button')
        });

    }

    static MessageBoxInfo(message, heading, callback) {
        let settings = {
            type: 'info',
            heading: heading ? heading : 'Information',
            heading_icon: '<i class="fas text-info fa-exclamation-circle"></i>',
            message: message,
            buttonText: ['OK'],
        };
        this.MessageBox(message, settings, callback);
    }
    static MessageBoxWarning(message, heading, callback) {
        let settings = {
            type: 'warning',
            heading: heading ? heading : 'Warning',
            heading_icon: '<i class="fas text-warning fa-exclamation-triangle"></i>',
            message: message,
            buttonText: ['OK'],
        };
        this.MessageBox(message, settings, callback);
    }
    static MessageBoxSuccess(message, heading, callback) {
        let settings = {
            type: 'success',
            heading: heading ? heading : 'Success',
            heading_icon: '<i class="fas text-success fa-check-circle"></i>',
            message: message,
            buttonText: ['OK'],
        };
        this.MessageBox(message, settings, callback);
    }
    static MessageBoxDanger(message, heading, callback) {
        let settings = {
            type: 'error',
            heading: heading ? heading : 'Error',
            heading_icon: '<i class="fas text-danger fa-times-circle"></i>',
            message: message,
            buttonText: ['OK'],
        };

        this.MessageBox(message, settings, callback);
    }

    static ConfirmDialogue(message, settings, callback, callback_2) {
        let defaults = {
            type: 'danger',
            heading: 'Confirmation',
            heading_icon: '<i class="fas text-danger fa-exclamation-circle"></i>',
            message: message,
            buttonText: ['YES', 'NO'],
        };
        settings = $.extend(defaults, settings);

        this.MessageBox(message, settings, callback, callback_2);
    }
}


function WorkInProgress() {
    SportaUtil.MessageBoxInfo('This functionality is in progess', 'In Development!')
}


$('button.close, a.close').on('click', function () {
    if ($(this).data('dismiss') == 'sidebar')
        closeSideBar();
});


function closeSideBar(formId) {
    if (formId)
        SportaForms.ResetForm(formId)

    $('.sidebar').slideUp();
}

function openSidebar() {
    $('.sidebar').slideDown();
}