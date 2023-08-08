$('#add').click(
    function(){
        var request_data = new FormData();
        var Title = $("#title").val()
        request_data.append('title',$("#title").val());
        request_data.append('number',$("#number").val());
        request_data.append('price',$("#price").val());
        request_data.append('picture',$("#picture").val());
        request_data.append('depth',$("#depth").val());
        request_data.append('width',$("#width").val());
        request_data.append('height',$("#height").val());
        request_data.append('quality',$("#quality").val());
        request_data.append('description',$("#description").val());
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
        else if ($("#depth").val() === ""){
            alert("【深度】未輸入!")
        }
        else if ($("#width").val() === ""){
            alert("【寬度】未輸入!")
        }
        else if ($("#height").val() === ""){
            alert("【高度】未輸入!")
        }
        else if ($("#quality").val() === ""){
            alert("【材質】未輸入!")
        }
        else if ($("#description").val() === ""){
            alert("【描述】未輸入!")
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
                alert("成功上架商品【" + Title + "】!")
                window.location.href='/admin';
            }
        }
    }
);

function myFunction() {
    var text = "確定要登出?";
    if (confirm(text) == true) {
      window.location.href = "/";
    }
}

function displayFileName() {
    const fileInput = document.getElementById("picture");
    const fileNameSpan = document.getElementById("fileName");
    fileNameSpan.textContent = fileInput.files[0]?.name || "";
}