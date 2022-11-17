var guid_empty = '00000000-0000-0000-0000-000000000000';
var date_format = 'DD/MM/YYYY'
var theme_color = '#079992';


const eDataTables = Object.freeze({
    "ApplicatioUsers": 1
});


const eApplicationUserRoles = Object.freeze({
    "Admin": 1,
    "Executive": 2,
    "Candidate": 3
});

const eAlertType = Object.freeze({
    "Success": 1,
    "Danger": 2,
    "Warning": 3,
    "Info": 4
});


//Boolean Date Extensions
//Check post dated
function isGreaterThanToday(value) {
    return value > moment(moment().toDate()).format('YYYY-MM-DD')
}

//Check Equal to less than
function isLessThanOrEqualTo(date1, date2) {
    return moment(date1) <= moment(date2);
}

//Check less than
function isLessThan(date1, date2) {
    return moment(date1) < moment(date2);
}



//Amount in words
var units = ['', 'One ', 'Two ', 'Three ', 'Four ', 'Five ', 'Six ', 'Seven ', 'Eight ', 'Nine ', 'Ten ', 'Eleven ', 'Twelve ', 'Thirteen ', 'Fourteen ', 'Fifteen ', 'Sixteen ', 'Seventeen ', 'Eighteen ', 'Nineteen '];
var tens = ['', '', 'Twenty', 'Thirty', 'Forty', 'Fifty', 'Sixty', 'Seventy', 'Eighty', 'Ninety'];

function inWords(num) {
    if ((num = num.toString()).length > 9) return 'overflow';
    n = ('000000000' + num).substr(-9).match(/^(\d{2})(\d{2})(\d{2})(\d{1})(\d{2})$/);
    if (!n) return; var str = '';
    str += (n[1] != 0) ? (units[Number(n[1])] || tens[n[1][0]] + ' ' + units[n[1][1]]) + 'Crore ' : '';
    str += (n[2] != 0) ? (units[Number(n[2])] || tens[n[2][0]] + ' ' + units[n[2][1]]) + 'Lakh ' : '';
    str += (n[3] != 0) ? (units[Number(n[3])] || tens[n[3][0]] + ' ' + units[n[3][1]]) + 'Thousand ' : '';
    str += (n[4] != 0) ? (units[Number(n[4])] || tens[n[4][0]] + ' ' + units[n[4][1]]) + 'Hundred ' : '';
    str += (n[5] != 0) ? ((str != '') ? 'and ' : '') + (units[Number(n[5])] || tens[n[5][0]] + ' ' + units[n[5][1]]) + 'only ' : '';
    return str += str.includes('only') ? '' : 'only';
}