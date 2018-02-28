﻿$(window).keydown(function (event) {
    if (event.keyCode == 13) {
        Login();
    }
});
function Login() {
    var userName = $('#userName').val();
    userName = $.trim(userName);
    var pwd = $('#pwd').val();
    console.log(userName);
    pwd = $.trim(pwd);
    if ((userName == null || userName == '') || (pwd == null || pwd == ''))
    {
        alert('用户名或密码不能为空！');
        return;
    }
    else
    {
        $.ajax({
            url: '/Index/Login',
            type: 'GET',
            datatype: 'JSON',
            data: { userName: userName, pwd :pwd },
            success: function (result)
            {                
                var pwds;
                var flag;              
                $.each(result, function (key, val) {
                    if (key == 'Data') {
                        flag = val.flag;
                        pwds = val.pwd;
                    }
                });
                if (flag == 'A')
                {
                    var url = encodeURI('../FormInfo/FormInfo?userName=' + userName + '&pwd=' + pwds);
                    window.location.href = url;
                }
                else if (flag == 'B')
                {
                   window.location.href = '../Index/Index';
                }
            },
            error: function ()
            {
                alert('error');
            }
        });
    }
}
