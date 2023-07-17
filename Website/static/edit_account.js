$.ajax({
    type: "POST",
    url: "edit_account_load",
    dataType: "json",
    success: function(response_data){
        Object.keys(response_data).forEach(function(doc){
            console.log(response_data[doc][0]);
            
            var personal_id = doc;
            var name = response_data[doc][0];
            var user_name = response_data[doc][1];
            var email = response_data[doc][2];
            var address = response_data[doc][3];
            var country = response_data[doc][4];
            var date = response_data[doc][5];
            var phone = response_data[doc][6];
            
            const col = `
              <tr>
                <th scope="row">${personal_id}</th>
                <td>${name}</td>
                <td id="user_name${personal_id}">${user_name}</td>
                <td>${email}</td>
                <td>${address}</td>
                <td>${country}</td>
                <td>${date}</td>
                <td>${phone}</td>
                <td>
                  <button id="${personal_id}" type="button" class="btn btn-success btn-block" onclick="delete_account(this.id)">
                    刪除
                  </button>
                </td>
              </tr>`;
            $("#edit_account_field").append(col)
        });
    }
});

function delete_account(personal_id){
    var request_data = new FormData();
    var Title = $("#user_name"+personal_id).text();
    request_data.append('personal_id',personal_id);
    console.log(personal_id)
    if(confirm('確定要刪除【' + Title + '】(' + personal_id + ')這個帳號嗎?') == true){
         $.ajax({
             type: "POST",
             url: "delete_account_send",
             data: request_data,
             success: function(response_data){
                 
             },
             dataType: "json",
             contentType: false,
             processData: false
         });
         alert("成功刪除" + '【' + Title + '】帳號!')
    }
    window.location.href='/edit_account';
}