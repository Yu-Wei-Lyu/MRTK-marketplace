$.ajax({
    type: "POST",
    url: "edit_product_load",
    dataType: "json",
    success: function(response_data){
        Object.keys(response_data).forEach(function(doc){
            // console.log(response_data[doc][0]);
            
            var id = doc;
            var title = response_data[doc][0];
            var number = response_data[doc][1];
            var price = response_data[doc][2];
            var picture = response_data[doc][3];
            var depth = response_data[doc][4];
            var width = response_data[doc][5];
            var height = response_data[doc][6];
            var quality = response_data[doc][7];
            var description = response_data[doc][8];

            $("#title").val(title);
            $("#number").val(number);
            $("#price").val(price);
            $("#picture").val(picture);
            $("#depth").val(depth);
            $("#width").val(width);
            $("#height").val(height);
            $("#quality").val(quality);
            $("#description").val(description);
            
            const col = `
                <tr>
                  <td id="Title">${title}</td>
                  <td>${number}</td>
                  <td>${price}</td>
                  <td>${picture}</td>
                  <td>${depth}</td>
                  <td>${width}</td>
                  <td>${height}</td>
                  <td>${quality}</td>
                  <td>${description}</td>
                </tr>`;
            $("#product_field").append(col)
        });
    }
});

$('#remove').click(
    function(){
        var request_data = new FormData();
        var Title = $("#Title").text();
        if(confirm('確定要下架【' + Title + '】這件商品嗎?') == true){
            $.ajax({
                type: "POST",
                url: "remove_product_send",
                data: request_data,
                success: function(response_data){
                    
                },
                dataType: "json",
                contentType: false,
                processData: false
            });
            alert("成功下架【" + Title + "】!")
            window.location.href='/admin';
        }
    }
);


$('#save').click(
    function(){
        var request_data = new FormData();
        var Title = $("#Title").text();
        request_data.append('title',$("#title").val());
        request_data.append('number',$("#number").val());
        request_data.append('price',$("#price").val());
        request_data.append('picture',$("#picture").val());
        request_data.append('depth',$("#depth").val());
        request_data.append('width',$("#width").val());
        request_data.append('height',$("#height").val());
        request_data.append('quality',$("#quality").val());
        request_data.append('description',$("#description").val());
        
        // console.log($("#title").val())
        // console.log($("#count").val())
        // console.log($("#isbn").val())
        // console.log($("#author").val())
        
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
            if(confirm('確定要儲存【' + Title + '】商品的編輯動作嗎?') == true){
                $.ajax({
                    type: "POST",
                    url: "save_edit_product_send",
                    data: request_data,
                    success: function(response_data){
                        
                    },
                    dataType: "json",
                    contentType: false,
                    processData: false
                });
                alert("成功編輯【" + Title + "】!")
                window.location.href='/admin';
            }
        }
    }
);
