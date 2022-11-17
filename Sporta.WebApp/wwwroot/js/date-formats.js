function dd_mm_yyyy(value) {
    var fomatted_date = value.replace(/-/g,'/')
    var date_parts = fomatted_date.split("/");
    var date = new Date(+date_parts[2], date_parts[1] - 1, +date_parts[0])
    return String(date.getDate()).padStart(2, '0') + '/' + String(date.getMonth() + 1).padStart(2, '0') + '/' + date.getFullYear();
};

function getCurrentDate() {
    var CurrentDate = new Date();
    var day = ("0" + CurrentDate.getDate()).slice(-2);
    var month = ("0" + (CurrentDate.getMonth() + 1)).slice(-2);
    var today = CurrentDate.getFullYear() + "/" + (month) + "/" + (day);
    return today
}

function yyyy_mm_dd(value) {
    var date = new Date(value)
    var day = ("0" + date.getDate()).slice(-2);
    var month = ("0" + (date.getMonth() + 1)).slice(-2);
    return date.getFullYear() + "/" + (month) + "/" + (day);
};