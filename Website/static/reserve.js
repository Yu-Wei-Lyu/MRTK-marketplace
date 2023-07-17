$.ajax({
    type: "POST",
    url: "reserve_load",
    dataType: "json",
    success: function(response_data){
        Object.keys(response_data).forEach(function(doc){
            console.log(response_data[doc][0]);
            
            var ssn = doc;
            var title = response_data[doc][0];
            var author = response_data[doc][1];
            var isbn = response_data[doc][2];
            var date = response_data[doc][3];
            
            const col = `
              <tr>
                <th scope="row">${ssn}</th>
                <td id="title${ssn}">${title}</td>
                <td>${author}</td>
                <td>${isbn}</td>
                <td>${date}</td>
                <td>
                  <button id="${ssn}" type="button" class="btn btn-success btn-block" onclick="cancel_reserve(this.id)">
                    取消
                  </button>
                </td>
              </tr>`;
            $("#reserve_field").append(col)
        });
    }
});

function cancel_reserve(ssn){
    var request_data = new FormData();
    var Title = $("#title"+ssn).text();
    request_data.append('ssn',ssn);
    console.log(ssn)
    if(confirm('確定要取消預約【' + Title + '】(' + ssn + ')這本書嗎?') == true){
         $.ajax({
             type: "POST",
             url: "cancel_reserve_send",
             data: request_data,
             success: function(response_data){
                 
             },
             dataType: "json",
             contentType: false,
             processData: false
         });
         alert("成功取消預約" + '【' + Title + '】!')
    }
    window.location.href='/reserve';
}