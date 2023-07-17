$.ajax({
    type: "POST",
    url: "edit_product_load",
    dataType: "json",
    success: function(response_data){
        Object.keys(response_data).forEach(function(doc){
            console.log(response_data[doc][0]);
            
            var id = doc;
            var title = response_data[doc][0];
            var count = response_data[doc][1];
            var ISBN = response_data[doc][2];
            var author = response_data[doc][3];
            
            $("#title").val(title);
            $("#count").val(count);
            $("#isbn").val(ISBN);
            $("#author").val(author);
            
            const col = `
                <tr>
                  <td id="Title">${title}</td>
                  <td>${count}</td>
                  <td>${ISBN}</td>
                  <td>${author}</td>
                </tr>`;
            $("#product_field").append(col)
        });
    }
});

$('#remove').click(
    function(){
        var request_data = new FormData();
        var Title = $("#Title").text();
        if(confirm('確定要下架【' + Title + '】這本書籍嗎?') == true){
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
        request_data.append('count',$("#count").val());
        request_data.append('isbn',$("#isbn").val());
        request_data.append('author',$("#author").val());
        
        console.log($("#title").val())
        console.log($("#count").val())
        console.log($("#isbn").val())
        console.log($("#author").val())
        
        if ($("#title").val() === ""){
            alert("【書名】未輸入!")
        }
        else if ($("#count").val() === ""){
            alert("【書籍數量】未輸入!")
        }
        else if ($("#isbn").val() === ""){
            alert("【ISBN】未輸入!")
        }
        else if ($("#author").val() === ""){
            alert("【書籍作者】未輸入!")
        }
        else{
            if(confirm('確定要儲存【' + Title + '】書籍的編輯動作嗎?') == true){
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
