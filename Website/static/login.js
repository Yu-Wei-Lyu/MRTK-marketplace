$('#admin_login').click(
    function(){
        var request_data = new FormData();
        request_data.append('account',$("#account").val());
        request_data.append('password',$("#password").val());
        request_data.append('identity','admin');
        
        $("#account").val("");
        $("#password").val("");
        console.log(request_data);
        
        $.ajax({
            type: "POST",
            url: "login_send",
            data: request_data,
            success: function(response_data){
            console.log(response_data.Error)
                if (response_data.Error !== ""){
                    if (response_data.Error === "account error"){
                        alert("此帳號不存在!")
                    }
                    else if (response_data.Error === "password error"){
                        alert("密碼錯誤!")
                    }
                    else if (response_data.Error === "identity error"){
                        alert("此帳號的身分不是管理員!")
                    }
                }
                else{
                    window.location.href='/admin';
                }
            },
            dataType: "json",
            contentType: false,
            processData: false
        });
    }
);

$('#member_login').click(
    function(){
        var request_data = new FormData();
        request_data.append('account',$("#account").val());
        request_data.append('password',$("#password").val());
        request_data.append('identity','member');
        
        $("#account").val("");
        $("#password").val("");
        console.log(request_data);
        
        $.ajax({
            type: "POST",
            url: "login_send",
            data: request_data,
            success: function(response_data){
            console.log(response_data.Error)
                if (response_data.Error !== ""){
                    if (response_data.Error === "account error"){
                        alert("此帳號不存在!")
                    }
                    else if (response_data.Error === "password error"){
                        alert("密碼錯誤!")
                    }
                    else if (response_data.Error === "identity error"){
                        alert("此帳號的身分不是會員!")
                    }
                }
                else{
                    window.location.href='/member';
                }
            },
            dataType: "json",
            contentType: false,
            processData: false
        });
    }
);