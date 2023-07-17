// Your web app's Firebase configuration
$('#searchButton').click(
    function(){
        var request_data = new FormData();
        request_data.append('searchString',$("#searchBox").val());
        console.log(request_data);
        
        $('#ggggg').empty();
        $.ajax({
            type: "POST",
            url: "search_load",
            data: request_data,
            success: function(response_data){
                Object.keys(response_data).forEach(function(doc){
                    console.log(response_data[doc][0]);
                    
                    var id = doc;
                    var path = id;
                    var ISBN = response_data[doc][0];
                    var author = response_data[doc][1];
                    var title = response_data[doc][2];
                    var amount = response_data[doc][3];
                    var state = response_data[doc][4];
                    
                    if (parseInt(path) >= 11){
                        path = "cover"
                    }
                    
                    const col = `
                        <div class="col mb-5" id="field${id}">
                          <div class="card h-100">
                            <!-- Product image-->
                            <img
                              class="card-img-top"
                              src="static/${path}.jpg"
                              alt="..."
                            />
                            <!-- Product details-->
                            <div class="card-body p-4">
                              <div class="text-center">
                                <!-- Product name-->
                                <h3 class="fw-bolder">${title}</h3>
                                <h6 class="fw-bolder">author: ${author}</h6>
                                <h6 class="fw-bolder">ISBN: ${ISBN}</h6>
                              </div>
                            </div>
                            <!-- Product actions-->
                            <div class="card-footer p-4 pt-0 border-top-0 bg-transparent">
                              <div class="text-center">
                                <a id="${id}" class="btn btn-outline-dark mt-auto" href="/edit_book" onclick="edit_book(this.id)"
                                  >編輯</a
                                >
                              </div>
                            </div>
                          </div>
                        </div>
                      </div>
                    </div>`;
                    $("#ggggg").append(col)
                });
            },
            dataType: "json",
            contentType: false,
            processData: false
        });
    }
);


$.ajax({
    type: "POST",
    url: "initial_load",
    dataType: "json",
    success: function(response_data){
        Object.keys(response_data).forEach(function(doc){
            console.log(response_data[doc][0]);
            
            var id = doc;
            var path = id;
            var ISBN = response_data[doc][0];
            var author = response_data[doc][1];
            var title = response_data[doc][2];
            var amount = response_data[doc][3];
            var state = response_data[doc][4];
            
            if (parseInt(path) >= 11){
                path = "cover"
            }
            
            const col = `
                <div class="col mb-5" id="field${id}">
                  <div class="card h-100">
                    <!-- Product image-->
                    <img
                      class="card-img-top"
                      src="static/${path}.jpg"
                      alt="..."
                    />
                    <!-- Product details-->
                    <div class="card-body p-4">
                      <div class="text-center">
                        <!-- Product name-->
                        <h3 class="fw-bolder">${title}</h3>
                        <h6 class="fw-bolder">author: ${author}</h6>
                        <h6 class="fw-bolder">ISBN: ${ISBN}</h6>
                      </div>
                    </div>
                    <!-- Product actions-->
                    <div class="card-footer p-4 pt-0 border-top-0 bg-transparent">
                      <div class="text-center">
                        <a id="${id}" class="btn btn-outline-dark mt-auto" href="/edit_book" onclick="edit_book(this.id)"
                          >編輯</a
                        >
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </div>`;
            $("#ggggg").append(col)
        });
    }
});


function edit_book(book_id){
    var request_data = new FormData();
    request_data.append('book_id',book_id);
    $.ajax({
        type: "POST",
        url: "edit_book_send",
        data: request_data,
        success: function(response_data){
            console.log(book_id)
        },
        dataType: "json",
        contentType: false,
        processData: false
    });
}
