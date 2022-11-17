let get_total_drive_count = '/Dashboard/GetTotalDriveCount',
    get_live_drive_count = '/Dashboard/GetLiveDriveCount',
    get_upcoming_drive_count = '/Dashboard/GetUpcomingDriveCount',
    get_total_candidates_count = '/Dashboard/GetTotalCandidatesCount',
    navbar_dashboard = 'navbar-dashboard',
    get_total_candidates_count_enrolled_by_date = '/Dashboard/GetTotalCandidatesCountByDateTime',
    get_total_drive_count_by_date = '/Dashboard/GetTotalDriveCountByDateTime',
    get_category_percentage_doughnut_bar = '/Dashboard/GetCategoryPercent',
    get_drive_percentage_bar_graph = '/Dashboard/GetDrivePercent'





// Intializing function for Dashboard Page
$(document).ready( function () {
    toogleNavbar(navbar_dashboard);
    totalDriveCount();
    totalLiveDriveCount();
    totalUpcomingDriveCount();
    totalCandidatesCount();
    candidatesCounts();
    driveCounts();
    GetCategorypercent();
    GetDrivepercent();
});

//Total Drive Count
  function totalDriveCount() {
    $("#driveCount").empty();
    $.get(get_total_drive_count, function (response) {
        $("#driveCount").append("<h5>" + response.data + "</h5>");
    });
}

//Current Drives Count
function totalLiveDriveCount() {
    $.get(get_live_drive_count, function (response) {
        $("#liveDriveCount").append("<h5>" + response.data + "</h5>");
    });
}

//Upcoming Drives Count
function totalUpcomingDriveCount() {
    $.get(get_upcoming_drive_count, function (response) {
        $("#upcomingDriveCount").append("<h5>" + response.data + "</h5>");
    });
}

//Total Candidates Count
function totalCandidatesCount() {
    $.get(get_total_candidates_count, function (response) {
        $("#candidateCount").append("<h5>" + response.data + "</h5>");
    });
}

//Total Candidates Count By Date
function candidatesCounts() {
    $.get(get_total_candidates_count_enrolled_by_date, function (response) {
        
        if (response) {
            var result = "";
            $("#tableBody").empty();
            result = "";
            result = result + "<td>" + response.data[0].todayCount + "</td>";
            result = result + "<td>" + response.data[0].yesterDayCount + "</td>";
            result = result + "<td>" + response.data[0].last7DaysCount + "</td>";
            result = result + "<td>" + response.data[0].last30DaysCount + "</td>";
            result = result + "<td>" + response.data[0].last90DaysCount + "</td>";
            $("#tableBody").append(result);
        }
    });
}

//Total Drive Count By Date
function driveCounts() {
    $.get(get_total_drive_count_by_date, function (response) {
        if (response) {
            var result = "";
            $("#tableBody1").empty();
            result = "";
            result = result + "<td>" + response.data[0].todayCount + "</td>";
            result = result + "<td>" + response.data[0].yesterDayCount + "</td>";
            result = result + "<td>" + response.data[0].last7DaysCount + "</td>";
            result = result + "<td>" + response.data[0].last30DaysCount + "</td>";
            result = result + "<td>" + response.data[0].last90DaysCount + "</td>";
            $("#tableBody1").append(result);
        }
    });
}

//drive created percentage to show in doughnut graph
function GetDrivepercent() {
    var barColors = [
        "#b91d47",
        "#00aba9",
        "#2b5797",
        "#e8c3b9",
        "#1e7145"
    ];

    $.get(get_drive_percentage_bar_graph, function (response) {
        if (response) {
            
            var len = response.data.length;
            var xValues = ["January - March", "April - June", "July - September", "October - December"],
                yValues = [0,0,0,0];

            for (var i = 0; i < len; i++) {
                if (response.data[i].month_date >= 1 && response.data[i].month_date <= 3)
                {
                    yValues[0] = response.data[i].drive_percent + yValues[0];
                }
                else if (response.data[i].month_date >= 4 && response.data[i].month_date <= 6)
                {
                    yValues[1] = response.data[i].drive_percent + yValues[1];
                }
                else if (response.data[i].month_date >= 7 && response.data[i].month_date <= 9)
                {
                    yValues[2] = response.data[i].drive_percent + yValues[2];
                }
                else
                {
                    yValues[3] = response.data[i].drive_percent + yValues[3];
                }
            }
            var ctx = document.getElementById("myBarChart");
            new Chart(ctx, {
                type: "bar",
                data: {
                    labels: xValues,
                    datasets: [{
                        backgroundColor: barColors,
                        data: yValues
                    }]
                },
                options: {
                    legend: { display: false },
                    title: {
                        display: true,
                        text: "World Wine Production 2018"
                    }
                }
            });

        }
    });
}

//Category percentage to show in bar graph
function GetCategorypercent() {
    var barColors = [
        "#b91d47",
        "#00aba9",
        "#2b5797",
        "#e8c3b9",
        "#1e7145"
    ];

    $.get(get_category_percentage_doughnut_bar, function (response) {
        if (response) {
            var len = response.data.length;
            var xValues = [], yValues = [];

            for (var i = 0; i < len; i++) {
                xValues.push(response.data[i].categoryName);
                yValues.push(response.data[i].category_percent);
            }

            var doughnut = document.getElementById("mydoughnutChart");
            new Chart(doughnut, {
                type: "doughnut",
                data: {
                    labels: xValues,
                    datasets: [{
                        backgroundColor: barColors,
                        data: yValues
                    }],
                },
                options: {
                    title: {
                        display: true,
                        text: "Category Used in the Drives"
                    }
                }
            });
        }
    });
}

//To refresh the Dashboard page
function refreshDashboard() {
    location.reload();
}