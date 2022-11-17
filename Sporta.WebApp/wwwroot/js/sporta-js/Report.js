let drive_id = 0, drive_name,
    navbar_reports = 'navbar-reports';


$(document).ready(function () {
    toogleNavbar(navbar_reports);
    SportaForms.InitializeFormStyle("#driveSearchReportForm");

    var url = userRole == 1 ? '/Drive/GetAllDrives_Dropdown' : '/Drive/GetDrivesByExecutive_Dropdown';

    $.ajax({
        url: url,
        type: 'POST',
        success: function (response) {
            $('#categories-list-Reports').empty();
            $.each(response.data, function (index, item) {
                $('#categories-list-Reports').append("<option value=" + item.id + ">" + item.name + "</option>");
            });
            $('#categories-list-Reports').selectpicker('refresh');

            if (response.data[0] && drive_id == 0) {
                drive_id = response.data[0].id;
                loadReport();
            }
        },
        error: function (resposne) { },
        complete: function () {
            if (drive_id)
                $('#categories-list-Reports').val(drive_id).change();
        }
    });
});


$("#driveSearchReportForm").submit(function (e) {
    e.preventDefault();
    drive_id = $('#categories-list-Reports').val();
    loadReport(drive_id);
});


function loadReport(dId) {

    drive_id ? drive_id : drive_id = dId; //This line is for redirection from drive

    $('[data-toggle="tooltip"]').tooltip();
    $('#reportTabDiv').show();
    $.get("/Report/GetReportBydrive", "driveId=" + drive_id, function (response) {
        drive_name = response.data[0]?.driveName?.toUpperCase();
        if (drive_name) {
            $("#btn-download-driveDetail").show();
        } else {
            $("#btn-download-driveDetail").hide();
        }
        $("#driveReport").html(drive_name ? drive_name : "");
        var tableColumns = [
            { data: "name" },
            { data: "userName" },
            { data: "rollNo" },
            { data: "branch" },
            { data: 'contactNumber' },
            { data: 'status' },
            { data: 'totalQuestions' },
            { data: 'attempted' },
            {
                data: null, render: function (data) { return `<span data-toggle="tooltip" title="${scoreTooltip(data.isPass, data.status, data.score, data.totalQuestions)}">${data.score}</span>`; }
            },
            { data: null, render: usersActionButtons }
        ]
        var table = SportaDataTable.CreateDataTable("#report-list", response.data, tableColumns, { orderBy: 8, orderSequence: 'desc' });

        table.rows().every(function () {
            var data = this.data();
            if (data.isPass) {
                $(this.node()).css("background-color", "#86ce86");
            }
        });
    });

}


function usersActionButtons(data, type, column) {
    var _buttons = '<div class="btn-group">'
        + `<button class="btn btn-sm btn-outline-theme" data-toggle="tooltip" data-placement="top" title="Detail view of result"onclick="openResultDetail('${data.id}')"><i class="fas fa-info-circle"></i></button>`
        + `</div>`;
    return _buttons;
}


function downloadReport() {
    $.get('/Report/GetResultDetailByDriveId', 'driveId=' + drive_id, function (response) {
        JSONToCSVConvertor(response.data, drive_name + '_Report')
    })
}


function openResultDetail(ResultId) {
    $.get("/Report/GetResultDetailById", "ResultId=" + ResultId, function (response) {
        var len = response.data.length, data = response.data;
        var result = "";
        $("#tableBody").empty();

        $("#txt_name").html(data[0].name), $("#txt_email").html(data[0].userName), $("#txt_branch").html(data[0].branch), $("#txt_contact").html(data[0].contactNumber);
        for (var i = 0; i < len; i++) {
            result = "";
            let focusLost = response.data[i].timeTaken == "0" ? "" : response.data[i].focusLost;
            result = result + "<tr  >";
            result = result + "<td>" + response.data[i].categoryName + "</td>";
            result = result + "<td>" + response.data[i].totalQuestions + "</td>";
            result = result + "<td>" + response.data[i].attempted + "</td>";
            result = result + "<td>" + response.data[i].allotedTime + "</td>";
            result = result + "<td>" + response.data[i].timeTaken + "</td>";
            result = result + "<td>" + focusLost + "</td>";
            result = result + "<td>" + response.data[i].score + "</td>";
            result = result + ("</tr>");
            $("#tableBody").append(result);
        }
    });
    $("#tableModal").modal('show');
}


function JSONToCSVConvertor(JSONData, ReportTitle) {
    //If JSONData is not an object then JSON.parse will parse the JSON string in an Object
    var arrData = typeof JSONData != 'object' ? JSON.parse(JSONData) : JSONData;

    var CSV = 'sep=,' + '\r\n\n';

    //This condition will generate the Label/Header

    var row = "";

    //This loop will extract the label from 1st index of on array
    for (var index in arrData[0]) {

        //Now convert each value to string and comma-seprated
        row += index + ',';
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
    var fileName = ReportTitle.replace(/ /g, "_");

    //Initialize file format you want csv or xls
    var uri = 'data:text/csv;charset=utf-8,' + escape(CSV);

    // Now the little tricky part.
    // you can use either>> window.open(uri);
    // but this will not work in some browsers
    // or you will not get the correct file extension    

    //this trick will generate a temp <a /> tag
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


function scoreTooltip(isPass, status, score, total) {
    if (status == 'Test Submitted') {
        return isPass ? 'Qualified with ' + calcPercentage(score, total) + '%' : 'Not Qualified ';
    } else {
        return status == 'Test Ongoing' ? status : 'Test not yet started';
    }
}


function calcPercentage(score, total) {
    return ((100 * score) / total).toFixed(1);
}