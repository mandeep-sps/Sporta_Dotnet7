
function customizeDataTable(settings, tableData, tableColumns) {
    $.fn.DataTable.ext.pager.numbers_length = 5;
    //Set Defaults
    var defaults = {
        tableId: '#data-table', //can be id or class,
        exportButton: true,     // To enable pdf and excel export buttons
        initialOrder: true,    //To maintain server side sorting initially
        showPaging: true,       //Enable pagination
        orderBy: 0,             //set order by column index for initial ordering (initialOrder must be true)
        orderSequence: 'asc',   //specify order direction
        tableHeight: false,    //table height 
        enableSearch: true,     //Enable/Disable search(filter) option.
        columnWiseSearch: true, //If yes then  <tfoot> element need to be added in the table with the same number of columns as <thead> 
        showTableInfo: true,    //Enable table summary 
        noOfFixedRightColumns: 0,
        responsive: true,
        exportFileName: ''
    };

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


