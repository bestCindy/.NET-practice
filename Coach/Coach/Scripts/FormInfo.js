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
    if ((new Date(dateStart)) > (new Date(dateEnd))) {
        alert("开始日期不能小于结束日期！");
        return;
    }
    $.ajax({
        url: '/FormInfo/SentMail',
        method: 'GET',
        dataype: 'JSON',
        data: { fileName: 'PersonnelInfo',dateStartString: dateStart, dateEndString: dateEnd },
        success: function (result) {
            alert(result);
        },
        error: function () {
            alert('发送失败！');
        }
    });
}

function ResignationSentMail() {
    debugger;
    var dateStart = $('#resignationDate-start').val();
    var dateEnd = $('#resignationDate-end').val();
    if ((new Date(dateStart)) > (new Date(dateEnd))) {
        alert("开始日期不能小于结束日期");
        return;
    }
    $.ajax({
        url: '/FormInfo/SentMail',
        method: 'GET',
        dataype: 'JSON',
        data: { fileName: 'ResignationInfo', dateStartString: dateStart, dateEndString: dateEnd },
        success: function (result) {
            alert(result);
        },
        error: function () {
            alert('发送失败！');
        }
    });
}

function OnetimeSentMail() {
    var dateStart = $('#OnetimeDate-start').val();
    var dateEnd = $('#OnetimeDate-end').val();
    if ((new Date(dateStart)) > (new Date(dateEnd))) {
        alert("开始日期不能小于结束日期");
        return;
    }
    $.ajax({
        url: '/FormInfo/SentMail',
        method: 'GET',
        dataype: 'JSON',
        data: { fileName: 'OnetimeAllowance', dateStartString: dateStart, dateEndString: dateEnd },
        success: function (result) {
            alert(result);
        },
        error: function () {
            alert('发送失败！');
        }
    });
}

function FixedSentMail() {
    var dateStart = $('#fixedDate-start').val();
    var dateEnd = $('#fixedDate-end').val();
    if ((new Date(dateStart)) > (new Date(dateEnd))) {
        alert("开始日期不能小于结束日期");
        return;
    }
    $.ajax({
        url: '/FormInfo/SentMail',
        method: 'GET',
        dataype: 'JSON',
        data: { fileName: 'FixedAllowance', dateStartString: dateStart, dateEndString: dateEnd },
        success: function (result) {
            alert(result);
        },
        error: function () {
            alert('发送失败！');
        }
    });
}

function SalarySentMail() {
    var dateStart = $('#salaryDate-start').val();
    var dateEnd = $('#salaryDate-end').val();
    if ((new Date(dateStart)) > (new Date(dateEnd))) {
        alert("开始日期不能小于结束日期");
        return;
    }
    $.ajax({
        url: '/FormInfo/SentMail',
        method: 'GET',
        dataype: 'JSON',
        data: { fileName: 'SalaryChange', dateStartString: dateStart, dateEndString: dateEnd },
        success: function (result) {
            alert(result);
        },
        error: function () {
            alert('发送失败！');
        }
    });
}
//导入邮件
function uploadFile() {
    var formData = new FormData();
    var fileCon = $('#filecon')[0].files[0];
    if (fileCon == undefined) {
        alert('未获取文件！');
        return;
    }
    var fileName = fileCon.name;
    if (fileName == 'PersonnelInfo.xlsx' || fileName == 'PersonnelInfo.xls' || fileName == 'ResignationInfo.xlsx' || fileName == 'ResignationInfo.xls' || fileName == 'OnetimeAllowance.xlsx' || fileName == 'OnetimeAllowance.xls' || fileName == 'FixedAllowance.xlsx' || fileName == 'FixedAllowance.xls' || fileName == 'SalaryChange.xlsx' || fileName == 'SalaryChange.xls') {
        formData.append('file', fileCon);
        $.ajax({
            url: '/FormInfo/uploadFile',
            method: 'POST',
            dataype: 'JSON',
            data: formData,
            contentType: false,
            processData: false,
            success: function (result) {
                alert(result);
            },
            error: function () {
                alert('文件上传失败！');
            }
        }); 
    }
    else {
        alert('文件名称错误！');
    }
}
