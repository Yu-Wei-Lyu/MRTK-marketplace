$.ajax({
    type: "POST",
    url: "borrowed_load",
    dataType: "json",
    success: function(response_data){
        Object.keys(response_data).forEach(function(doc){
            console.log(response_data[doc][0]);
            
            var ssn = doc;
            var title = response_data[doc][0];
            var borrowed_date = response_data[doc][1];
            var deadline = response_data[doc][2];
            var author = response_data[doc][3];
            var isbn = response_data[doc][4];
            
            const col = `
              <tr>
                <th scope="row">${ssn}</th>
                <td id="title${ssn}">${title}</td>
                <td>${borrowed_date}</td>
                <td>${deadline}</td>
                <td>${author}</td>
                <td>${isbn}</td>
                <td>
                  <button id="${ssn}" type="button" class="btn btn-success btn-block" onclick="return_book(this.id)">
                    還書
                  </button>
                </td>
              </tr>`;
            $("#borrowed_field").append(col)
        });
    }
});

function return_book(ssn){
    var request_data = new FormData();
    var Title = $("#title"+ssn).text();
    request_data.append('ssn',ssn);
    console.log(ssn)
    if(confirm('確定要歸還【' + Title + '】(' + ssn + ')這本書嗎?') == true){
         $.ajax({
             type: "POST",
             url: "return_book",
             data: request_data,
             success: function(response_data){
                 
             },
             dataType: "json",
             contentType: false,
             processData: false
         });
         alert("成功歸還" + '【' + Title + '】!')
    }
    window.location.href='/borrowed';
}