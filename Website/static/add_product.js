const socket = new WebSocket('ws://118.150.125.153:8765');

// 當接收到訊息時更新網頁內容
socket.onmessage = function(event) {
    try {
        const data = JSON.parse(event.data);
        const message_type = data.type;
        const resultsList = document.getElementById('results');
        resultsList.innerHTML = data.message[2];
        
        /*if (message_type === 'query') {
            const imageData = data.message;
            imageData_string = imageData.substring(2, imageData.length - 2);
            imageData_string = imageData_string.split(',')
            imageData_string = imageData_string[9]

            // 解碼 Base64 資料為 Uint8Array
            const decodedData = atob(imageData_string);

            // 將 Uint8Array 轉換為 Uint8Array 視圖
            const uint8Array = new Uint8Array(decodedData.length);
            for (let i = 0; i < decodedData.length; i++) {
                uint8Array[i] = decodedData.charCodeAt(i);
            }

            // 建立 Blob 物件
            const blob = new Blob([uint8Array], { type: 'image/jpeg' });

            // 建立 URL 物件，並設置圖片的 Blob URL 作為 src
            const imageUrl = URL.createObjectURL(blob);
            const imageDisplay = document.getElementById('imageDisplay');
            imageDisplay.src = imageUrl;
        }*/
        
    } catch (error) {
        console.error("Error parsing JSON:", error);
    }
};

// 點擊按鈕時發送訊息給伺服器
function sendQuery() {
  console.log("SendQuery");
  socket.send('{"type":"query"}');
};

// 點擊按鈕時將新資料加入伺服器
async function addData() {
  console.log("SendAdd");

  const imageInput = document.getElementById('picture');
  const selectedImage = imageInput.files[0];

  const reader = new FileReader();
  reader.onload = async function(event) {
      const imageData = event.target.result;

      // 從輸入欄位獲取資料
      const name = document.getElementById('title').value;
      // const number = document.getElementById('number').value;
      
      const price = document.getElementById('price').value;
      // const imagePath = document.getElementById('imagePathInput').value;
      
      // 取得家具size
      const depth = document.getElementById('depth').value;
      const width = document.getElementById('width').value;
      const height = document.getElementById('height').value;
      const size = depth + 'x' + width + 'x' + height;

      // 取得分類標籤
      const selectedCategories = [];
      const checkboxes = document.querySelectorAll('input[name="category"]:checked');
      checkboxes.forEach(function(checkbox) {
          selectedCategories.push(checkbox.value);
      });
      // console.log('選中的分類：', selectedCategories);

      const description = document.getElementById('description').value;
      const material = document.getElementById('material').value;

      

      // 上傳圖片到 Imgur
      const formData = new FormData();
      formData.append('image', selectedImage);

      try {
          const imgurResponse = await fetch('https://api.imgur.com/3/image', {
              method: 'POST',
              headers: {
                  Authorization: 'Client-ID f46069301854f94', // Replace with your Imgur Client ID
              },
              body: formData,
          });

          if (imgurResponse.ok) {
              const imgurData = await imgurResponse.json();
              const imgurImageUrl = imgurData.data.link;
              console.log('Imgur Image URL:', imgurImageUrl);

              // 建立資料物件，包含 Imgur 圖片 URL
              const newData = {
                  type: 'add',
                  Name: name,
                  // Number: number, 應該可移除
                  Price: price,
                  // ImagePath: imagePath, 應該可移除
                  Size: size,
                  Tags:selectedCategories,
                  Description: description,
                  Material: material,
                  // Manufacturer: manufacturer, 不知加在哪  
                  ImageUrl: imgurImageUrl, // Add Imgur Image URL
                  // ModelUrl: imgurModelUrl, // Add Imgur Model URL 未實裝
              };

              // 將資料物件轉成 JSON 格式
              const jsonNewData = JSON.stringify(newData);

              // 發送資料給伺服器
              socket.send(jsonNewData);
          } else {
              console.error('Imgur Upload Error:', imgurResponse.statusText);
          }
      } catch (error) {
          console.error('Error during Imgur upload:', error);
      }
  };

  // 讀取文件為二進制數據
  reader.readAsArrayBuffer(selectedImage);
}

function myFunction() {
    var text = "確定要登出?";
    if (confirm(text) == true) {
      window.location.href = "/";
    }
}

function checkInputLength(inputElement, maxLength) {
  const inputValue = inputElement.value;
  const currentLength = inputValue.length;

  if (currentLength > maxLength) {
      inputElement.value = inputValue.substring(0, maxLength); // 截斷輸入超過的部分
      alert(`超過最大字數限制（${maxLength}字），多餘的字將被移除。`);
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
    { id: 'title', name: '名稱', maxLength: 50 }, // 最大 50 個字
    { id: 'price', name: '售價', maxLength: 10 }, // 最大 10 個字
    { id: 'depth', name: '深度', maxLength: 9 }, // 最大 9 個字
    { id: 'width', name: '寬度', maxLength: 9 }, // 最大 9 個字
    { id: 'height', name: '高度', maxLength: 9 }, // 最大 9 個字
    { id: 'material', name: '材質', maxLength: 50 }, // 最大 50 個字
    { id: 'description', name: '描述', maxLength: 100 }, // 最大 100 個字
    // 添加其他欄位...
  ];

  let hasError = false;
  let invalidCharsText = '';

  fields.forEach(field => {
    const inputElement = document.getElementById(field.id);
    const inputValue = inputElement.value.trim(); // 去掉首尾空白
    const invalidChars = [];

    if (!inputValue) {
        hasError = true;
        const fieldErrorText = `${field.name}為必填欄位`;
        if (invalidCharsText === '') {
            invalidCharsText = fieldErrorText;
        } else {
            invalidCharsText += `<br>${fieldErrorText}`;
        }
    } else {
        inputValue.split('').forEach(char => {
            if (!unicode_hex_ranges.flat().includes(char.charCodeAt(0)) || char === '&') {
                invalidChars.push(char);
                hasError = true;
            }
        });

        if (inputValue.length > field.maxLength) {
            hasError = true;
            const fieldErrorText = `${field.name}欄位超過字數限制，最多允許${field.maxLength}個字`;
            if (invalidCharsText === '') {
                invalidCharsText = fieldErrorText;
            } else {
                invalidCharsText += `<br>${fieldErrorText}`;
            }
        } else if (invalidChars.length > 0) {
            const fieldErrorText = `${field.name}欄位中包含不允許的字符: ${invalidChars.join(', ')}`;
            if (invalidCharsText === '') {
                invalidCharsText = fieldErrorText;
            } else {
                invalidCharsText += `<br>${fieldErrorText}`;
            }
        }
    }
  });
  
  const pictureInput = document.getElementById('picture');
  const modelInput = document.getElementById('model');

  if (!pictureInput.files.length) {
      hasError = true;
      const fieldErrorText = '請選擇圖片';
      if (invalidCharsText === '') {
          invalidCharsText = fieldErrorText;
      } else {
          invalidCharsText += `<br>${fieldErrorText}`;
      }
  }

  if (!modelInput.files.length) {
      hasError = true;
      const fieldErrorText = '請選擇模型';
      if (invalidCharsText === '') {
          invalidCharsText = fieldErrorText;
      } else {
          invalidCharsText += `<br>${fieldErrorText}`;
      }
  }

  const categoryCheckboxes = document.querySelectorAll('input[name="category"]');
  let categorySelected = false;

  categoryCheckboxes.forEach(checkbox => {
      if (checkbox.checked) {
          categorySelected = true;
      }
  });

  if (!categorySelected) {
      hasError = true;
      const fieldErrorText = '請選擇至少一個分類';
      if (invalidCharsText === '') {
          invalidCharsText = fieldErrorText;
      } else {
          invalidCharsText += `<br>${fieldErrorText}`;
      }
  }

  if (hasError) {
    const errorElement = document.getElementById('inputError');
    errorElement.innerHTML = invalidCharsText; // 使用 innerHTML 插入換行
    errorElement.style.display = 'inline-block'; // 顯示錯誤訊息
  } else {
    // 執行上架動作
    // TODO: 在這裡執行上架的相關操作
    addData();
    console.log('商品已上架');
    
    // 清除錯誤訊息
    const errorElement = document.getElementById('inputError');
    errorElement.style.display = 'none';
  }
}