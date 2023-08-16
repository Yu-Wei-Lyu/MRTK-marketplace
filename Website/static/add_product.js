
// jQuery add
/* $('#add').click(
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
);*/

function myFunction() {
    var text = "確定要登出?";
    if (confirm(text) == true) {
      window.location.href = "/";
    }
}

function displayPictureName() {
    const PictureInput = document.getElementById("picture");
    const PictureNameSpan = document.getElementById("pictureName");
    PictureNameSpan.textContent = PictureInput.files[0]?.name || "";
}

function displayModelName() {
    const ModelInput = document.getElementById("model");
    const ModelNameSpan = document.getElementById("modelName");
    ModelNameSpan.textContent = ModelInput.files[0]?.name || "";
}

const unicode_hex_ranges = [
    Array.from(range(0x0020, 0x007E + 1)),
    Array.from(range(0x3000, 0x303F + 1)),
    Array.from(range(0x4E00, 0x9FFF + 1)),
    Array.from(range(0xFF00, 0xFFEF + 1))
  ];

  function range(start, end) {
    return Array(end - start + 1).fill().map((_, idx) => start + idx);
  }

  function checkInput(inputElement) {
    const inputValue = inputElement.value;

    // 檢查輸入是否包含不允許的字符或 &
    const hasInvalidChars = inputValue.split('').some(char => {
      return !unicode_hex_ranges.flat().includes(char.charCodeAt(0)) || char === '&';
    });

    // 根據檢查結果顯示或隱藏錯誤訊息
    const errorElement = document.getElementById('inputError');
    errorElement.style.display = hasInvalidChars ? 'inline-block' : 'none';
  }
  
  function handleAddClick() {
    const fields = [
      { id: 'title', name: '名稱' },
      { id: 'number', name: '編號' },
      { id: 'price', name: '售價' },
      { id: 'depth', name: '深度'},
      { id: 'width', name: '寬度'},
      { id: 'height', name: '高度'},
      { id: 'material', name: '材質'},
      { id: 'description', name: '描述'},
      // 添加其他欄位...
    ];
  
    let hasError = false;
    let invalidCharsText = '';
  
    fields.forEach(field => {
      const inputElement = document.getElementById(field.id);
      const inputValue = inputElement.value;
      const invalidChars = [];
  
      inputValue.split('').forEach(char => {
        if (!unicode_hex_ranges.flat().includes(char.charCodeAt(0)) || char === '&') {
          invalidChars.push(char);
          hasError = true;
        }
      });
  
      if (invalidChars.length > 0) {
        const errorElement = document.getElementById('inputError');
        errorElement.style.display = 'inline-block';
  
        const fieldErrorText = `${field.name}欄位中包含不允許的字符: ${invalidChars.join(', ')}`;
        if (invalidCharsText === '') {
          invalidCharsText = fieldErrorText;
        } else {
          invalidCharsText += `<br>${fieldErrorText}`;
        }
      }
    });
  
    if (hasError) {
      const errorElement = document.getElementById('inputError');
      errorElement.innerHTML = invalidCharsText; // 使用 innerHTML 插入換行
    } else {
      // 執行上架動作
      // TODO: 在這裡執行上架的相關操作
      console.log('商品已上架');
      
      // 清除錯誤訊息
      const errorElement = document.getElementById('inputError');
      errorElement.style.display = 'none';
    }
  }