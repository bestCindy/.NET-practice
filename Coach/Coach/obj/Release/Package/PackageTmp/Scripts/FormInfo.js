window.onload = function () {
    var userName = GetQueryString('userName');
    var pwd = GetQueryString('pwd');
    $.ajax({
        url: '/FormInfo/CheckSession',
        type: 'GET',
        dataype: 'JSON',
        data: { userName: userName, pwd: pwd },
        success: function (result) {
            var pwds;
            var flag;
            $.each(result, function (key, val) {
                if (key == 'Data') {
                    flag = val.flag;
                    pwds = val.pwd;
                }
            });
            //alert(userName);
            //alert(pwds);
        },
        error: function () {
            alert('error');
        }
    });
}
function GetQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) {
        return (r[2]);
    }
    return null;
}
function HandelTime(strTime) {
    var obj = new Date(parseInt(strTime.replace("/Date(", "").replace(")/", ""), 10));
    return obj.getFullYear() + "-" + obj.getMonth() + "-" + obj.getDate();
}
//邮件重发
function PersonnelSentMail() {
    var dateStart = $('#personnelDate-start').val();
    var dateEnd = $('#personnelDate-end').val();
    //alert(dateStart);
    //alert(dateEnd);
    if ((new Date(dateStart)) > (new Date(dateEnd))) {
        alert("error");
    }
    $.ajax({
        url: '/FormInfo/SentMail',
        method: 'POST',
        dataype: 'JSON',
        data: { fileName: 'PersonnelInfo',dateStartString: dateStart, dateEndString: dateEnd }
    });
}

function ResignationSentMail() {
    debugger;
    var dateStart = $('#resignationDate-start').val();
    var dateEnd = $('#resignationDate-end').val();
    //alert(dateStart);
    //alert(dateEnd);
    if ((new Date(dateStart)) > (new Date(dateEnd))) {
        alert("error");
    }
    $.ajax({
        url: '/FormInfo/SentMail',
        method: 'POST',
        dataype: 'JSON',
        data: { fileName: 'ResignationInfo', dateStartString: dateStart, dateEndString: dateEnd }
    });
}

function OnetimeSentMail() {
    var dateStart = $('#OnetimeDate-start').val();
    var dateEnd = $('#OnetimeDate-end').val();
    //alert(dateStart);
    //alert(dateEnd);
    if ((new Date(dateStart)) > (new Date(dateEnd))) {
        alert("error");
    }
    $.ajax({
        url: '/FormInfo/SentMail',
        method: 'POST',
        dataype: 'JSON',
        data: { fileName: 'OnetimeAllowance', dateStartString: dateStart, dateEndString: dateEnd }
    });
}

function FixedSentMail() {
    var dateStart = $('#fixedDate-start').val();
    var dateEnd = $('#fixedDate-end').val();
    //alert(dateStart);
    //alert(dateEnd);
    if ((new Date(dateStart)) > (new Date(dateEnd))) {
        alert("error");
    }
    $.ajax({
        url: '/FormInfo/SentMail',
        method: 'POST',
        dataype: 'json',
        data: { fileName: 'FixedAllowance', dateStartString: dateStart, dateEndString: dateEnd }
    });
}

function SalarySentMail() {
    var dateStart = $('#salaryDate-start').val();
    var dateEnd = $('#salaryDate-end').val();
    //alert(dateStart);
    //alert(dateEnd);
    if ((new Date(dateStart)) > (new Date(dateEnd))) {
        alert("error");
    }
    $.ajax({
        url: '/FormInfo/SentMail',
        method: 'POST',
        dataype: 'JSON',
        data: { fileName: 'SalaryChange', dateStartString: dateStart, dateEndString: dateEnd }
    });
}