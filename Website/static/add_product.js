$('#add').click(
    function(){
        var request_data = new FormData();
        var Title = $("#title").val()
        request_data.append('title',$("#title").val());
        request_data.append('number',$("#number").val());
        request_data.append('price',$("#price").val());
        request_data.append('picture',$("#picture").val());
        request_data.append('size',$("#size").val());
        request_data.append('description',$("#description").val());
        request_data.append('quality',$("#quality").val());
        
        if ($("#title").val() === ""){
            alert("【名稱】未輸入!")
        }
        else if ($("#number").val() === ""){
            alert("【編號】未輸入!")
        }
        else if ($("#price").val() === ""){
            alert("【售價】未輸入!")
        }
        else if ($("#picture").val() === ""){
            alert("【圖片】未上傳!")
        }
        else if ($("#size").val() === ""){
            alert("【大小】未輸入!")
        }
        else if ($("#description").val() === ""){
            alert("【描述】未輸入!")
        }
        else if ($("#quality").val() === ""){
            alert("【材質】未輸入!")
        }
        else{
            if(confirm('確定要上架【' + Title + '】這個商品嗎?') == true){
                $.ajax({
                    type: "POST",
                    url: "add_product_send",
                    data: request_data,
                    success: function(response_data){
                        
                    },
                    dataType: "json",
                    contentType: false,
                    processData: false
                });
                alert("成功上架書籍【" + Title + "】!")
                window.location.href='/admin';
            }
        }
    }
);