$('#register').click(
    function(){
        var request_data = new FormData();
        request_data.append('account',$("#account").val());
        request_data.append('password',$("#password").val());
        request_data.append('name',$("#name").val());
        request_data.append('username',$("#username").val());
        request_data.append('email',$("#email").val());
        request_data.append('address',$("#address").val());
        request_data.append('phone',$("#phone").val());
        request_data.append('country',$("#country").val());
        request_data.append('date',$("#date").val());
        

        if ($("#account").val() === ""){
            alert("【account】未輸入!")
        }
        else if ($("#password").val() === ""){
            alert("【password】未輸入!")
        }
        else if ($("#name").val() === ""){
            alert("【name】未輸入!")
        }
        else if ($("#username").val() === ""){
            alert("【username】未輸入!")
        }
        else if ($("#email").val() === ""){
            alert("【email】未輸入!")
        }
        else if ($("#address").val() === ""){
            alert("【address】未輸入!")
        }
        else if ($("#phone").val() === ""){
            alert("【phone】未輸入!")
        }
        else if ($("#country").val() === ""){
            alert("【country】未輸入!")
        }
        else if ($("#date").val() === ""){
            alert("【date】未輸入!")
        }
        else{
            $.ajax({
                type: "POST",
                url: "register_send",
                data: request_data,
                success: function(response_data){
                    
                },
                dataType: "json",
                contentType: false,
                processData: false
            });
            alert("成功註冊帳號!")
            window.location.href='/';
        }
    }
);