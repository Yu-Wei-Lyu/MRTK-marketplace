const socket = new WebSocket("ws://118.150.125.153:8765");

var furniture_ID;
var imageURL;
let isFileChange = false;

socket.addEventListener("open", function (event) {
  // 連接成功
  // 獲取URL中的查詢字串
  var queryString = window.location.search;

  // 解析字串獲得家具ID
  var params = new URLSearchParams(queryString);
  furniture_ID = params.get("variable");

  console.log("Received furniture_ID: " + furniture_ID);
  // 建立要傳送的資料物件
  const requestData = {
    type: "query_ID",
    ID: furniture_ID,
  };

  // 將資料物件轉成 JSON 格式
  const jsonRequestData = JSON.stringify(requestData);
  socket.send(jsonRequestData);
  // socket.send('{"type":"query_website"}');
});

// 當接收到訊息時更新網頁內容
socket.onmessage = function (event) {
  try {
    const data = JSON.parse(event.data);
    const message_type = data.type;
    // 假設 data 是您接收到的物件陣列
    const dataList = data.message; // 如果 'message' 包含資料物件

    if (message_type == "query_ID") {
      // 取得要顯示資料的容器
      console.log(dataList[0]);
      showdetails(dataList[0]);
    }
  } catch (error) {
    console.error("Error parsing JSON:", error);
  }
};

function showdetails(productData) {
  console.log(productData);
  // 使用獲取到的數據填充 HTML 元素的內容
  document.getElementById("productTitle").value = productData.Name;
  document.getElementById("price").value = `售價：NT$ ${productData.Price}`;
  var sizeParts = productData.Size.split("x");
  var depth = sizeParts[0];
  var width = sizeParts[1];
  var height = sizeParts[2];
  document.getElementById("depth").value = `深度：${depth} cm`;
  document.getElementById("width").value = `寬度：${width} cm`;
  document.getElementById("height").value = `高度：${height} cm`;
  document.getElementById("material").value = `材質：${productData.Material}`;
  document.getElementById("description").value = `描述：${productData.Description}`;
  imageURL = productData.ImageURL;
  document.getElementById("pictureName").textContent = imageURL;
  // 從後端取得的標籤資料，這是一個包含標籤的陣列
  var tagsFromBackend = productData.Tags.split("、");

  // 遍歷所有 checkbox 元素
  var checkboxes = document.querySelectorAll('input[type="checkbox"]');
  checkboxes.forEach(function (checkbox) {
    // 取得 checkbox 的值，這個值應該與後端傳遞的標籤資料匹配
    var checkboxValue = checkbox.value;

    // 如果 checkbox 的值在後端傳遞的標籤資料中，則勾選它
    if (tagsFromBackend.includes(checkboxValue)) {
      checkbox.checked = true;
    }
  });
}

// 點擊按鈕時向伺服器發送更新資料的請求
async function updateData() {
  console.log("SendUpdate");

  // 從輸入欄位獲取資料
  const name = document.getElementById("title").value;
  const price = document.getElementById("price").value;
  // 取得家具size
  const depth = document.getElementById("depth").value;
  const width = document.getElementById("width").value;
  const height = document.getElementById("height").value;
  const size = depth + "x" + width + "x" + height;
  // 取得分類標籤
  const selectedCategories = [];
  const checkboxes = document.querySelectorAll(
    'input[name="category"]:checked'
  );
  checkboxes.forEach(function (checkbox) {
    selectedCategories.push(checkbox.value);
  });
  // console.log("選中的分類：", selectedCategories);
  const description = document.getElementById("description").value;
  const material = document.getElementById("material").value;
  // const manufacturer = getManufacturerFromLocalStorage();
  if (isFileChange) {
    const imageInput = document.getElementById("picture");
    const selectedImage = imageInput.files[0];
    const reader = new FileReader();
    reader.onload = async function (event) {
      const imageData = event.target.result;
      // 上傳圖片到 Imgur
      const formData = new FormData();
      formData.append("image", selectedImage);
      try {
        const imgurResponse = await fetch("https://api.imgur.com/3/image", {
          method: "POST",
          headers: {
            Authorization: "Client-ID a4764610882ef96", // Replace with your Imgur Client ID
          },
          body: formData,
        });
        if (imgurResponse.ok) {
          const imgurData = await imgurResponse.json();
          const imgurImageUrl = imgurData.data.link;
          console.log("Imgur Image URL:", imgurImageUrl);
          // 資料物件
          const dataToSend = {
            type: "update",
            ID: furniture_ID,
            Name: name,
            Price: price,
            Size: size,
            Tags: selectedCategories,
            Description: description,
            Material: material,
            // Manufacturer: manufacturer,
            ImageUrl: imgurImageUrl, // Add Imgur Image URL
          };
          // 將資料物件轉成 JSON 格式
          const jsonRequestData = JSON.stringify(dataToSend);
          // 發送資料給伺服器
          socket.send(jsonRequestData);
        } else {
          console.error("Imgur Upload Error:", imgurResponse.statusText);
        }
      } catch (error) {
        console.error("Error during Imgur upload:", error);
      }
    };
    // 讀取文件為二進制數據
    reader.readAsArrayBuffer(selectedImage);
  } else {
    // 資料物件
    const dataToSend = {
      type: "update",
      ID: furniture_ID,
      Name: name,
      Price: price,
      Size: size,
      Tags: selectedCategories,
      Description: description,
      Material: material,
      // Manufacturer: manufacturer,
      ImageUrl: imageURL,
    };
    // 將資料物件轉成 JSON 格式
    const jsonRequestData = JSON.stringify(dataToSend);
    // 發送資料給伺服器
    socket.send(jsonRequestData);
  }
}

// 從LocalStorage獲得UserID
function getUserIDFromLocalStorage() {
  return localStorage.getItem("UserID");
}

// 從LocalStorage獲得Manufacturer
function getManufacturerFromLocalStorage() {
  return localStorage.getItem("Manufacturer");
}

// 點擊按鈕時向伺服器發送刪除資料的請求
function deleteData() {
  console.log("SendDelete");

  // 建立要傳送的資料物件
  const requestData = {
    type: "delete",
    ID: furniture_ID,
  };

  // 將資料物件轉成 JSON 格式
  const jsonRequestData = JSON.stringify(requestData);

  // 發送資料給伺服器
  socket.send(jsonRequestData);
}

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
  if (!isFileChange){
    isFileChange = !isFileChange;
  }
}

const unicode_hex_ranges = [
  Array.from(range(0x0020, 0x007e + 1)),
  Array.from(range(0x3000, 0x303f + 1)),
  Array.from(range(0x4e00, 0x9fff + 1)),
  Array.from(range(0xff00, 0xffef + 1)),
];

function range(start, end) {
  return Array(end - start + 1)
    .fill()
    .map((_, idx) => start + idx);
}

function handleAddClick() {
  const fields = [
    { id: "title", name: "名稱", maxLength: 50 }, // 最大 50 個字
    { id: "price", name: "售價", maxLength: 10 }, // 最大 10 個字
    { id: "depth", name: "深度", maxLength: 9 }, // 最大 9 個字
    { id: "width", name: "寬度", maxLength: 9 }, // 最大 9 個字
    { id: "height", name: "高度", maxLength: 9 }, // 最大 9 個字
    { id: "material", name: "材質", maxLength: 50 }, // 最大 50 個字
    { id: "description", name: "描述", maxLength: 100 }, // 最大 100 個字
    // 添加其他欄位...
  ];

  let hasError = false;
  let invalidCharsText = "";

  fields.forEach((field) => {
    const inputElement = document.getElementById(field.id);
    const inputValue = inputElement.value.trim(); // 去掉首尾空白
    const invalidChars = [];

    if (!inputValue) {
      hasError = true;
      const fieldErrorText = `${field.name}為必填欄位`;
      if (invalidCharsText === "") {
        invalidCharsText = fieldErrorText;
      } else {
        invalidCharsText += `<br>${fieldErrorText}`;
      }
    } else {
      inputValue.split("").forEach((char) => {
        if (
          !unicode_hex_ranges.flat().includes(char.charCodeAt(0)) ||
          char === "&"
        ) {
          invalidChars.push(char);
          hasError = true;
        }
      });

      if (inputValue.length > field.maxLength) {
        hasError = true;
        const fieldErrorText = `${field.name}欄位超過字數限制，最多允許${field.maxLength}個字`;
        if (invalidCharsText === "") {
          invalidCharsText = fieldErrorText;
        } else {
          invalidCharsText += `<br>${fieldErrorText}`;
        }
      } else if (invalidChars.length > 0) {
        const fieldErrorText = `${
          field.name
        }欄位中包含不允許的字符: ${invalidChars.join(", ")}`;
        if (invalidCharsText === "") {
          invalidCharsText = fieldErrorText;
        } else {
          invalidCharsText += `<br>${fieldErrorText}`;
        }
      }
    }
  });

  const pictureInput = document.getElementById("picture");
  const modelInput = document.getElementById("model");

  if (!pictureInput.files.length) {
    hasError = true;
    const fieldErrorText = "請選擇圖片";
    if (invalidCharsText === "") {
      invalidCharsText = fieldErrorText;
    } else {
      invalidCharsText += `<br>${fieldErrorText}`;
    }
  }

  if (!modelInput.files.length) {
    hasError = true;
    const fieldErrorText = "請選擇模型";
    if (invalidCharsText === "") {
      invalidCharsText = fieldErrorText;
    } else {
      invalidCharsText += `<br>${fieldErrorText}`;
    }
  }

  const categoryCheckboxes = document.querySelectorAll(
    'input[name="category"]'
  );
  let categorySelected = false;

  categoryCheckboxes.forEach((checkbox) => {
    if (checkbox.checked) {
      categorySelected = true;
    }
  });

  if (!categorySelected) {
    hasError = true;
    const fieldErrorText = "請選擇至少一個分類";
    if (invalidCharsText === "") {
      invalidCharsText = fieldErrorText;
    } else {
      invalidCharsText += `<br>${fieldErrorText}`;
    }
  }

  if (hasError) {
    const errorElement = document.getElementById("inputError");
    errorElement.innerHTML = invalidCharsText; // 使用 innerHTML 插入換行
    errorElement.style.display = "inline-block"; // 顯示錯誤訊息
  } else {
    // 執行上架動作
    // TODO: 在這裡執行上架的相關操作
    updateData();
    console.log("商品已更新");

    // 清除錯誤訊息
    const errorElement = document.getElementById("inputError");
    errorElement.style.display = "none";
  }
}
