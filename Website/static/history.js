$.ajax({
    type: "POST",
    url: "history_load",
    dataType: "json",
    success: function(response_data){
        Object.keys(response_data).forEach(function(doc){
            console.log(response_data[doc][0]);
            
            var ssn = doc;
            var title = response_data[doc][0];
            var borrowed_history = response_data[doc][1];
            var return_history = response_data[doc][2];
            var violated_history = response_data[doc][3];
            
            const col = `
            <tr>
              <th scope="row">${ssn}</th>
              <td>${title}</td>
              <td>${borrowed_history}</td>
              <td>${return_history}</td>
              <td>${violated_history}</td>
            </tr>`;
            $("#book_field").append(col)
        });
    }
});