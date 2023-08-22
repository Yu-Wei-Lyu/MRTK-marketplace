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

 // 點擊按鈕時向伺服器發送刪除資料的請求
 function deleteData() {
  console.log("SendDelete");
  // 從輸入欄位獲取要刪除的資料的 ID
  const deleteId = document.getElementById('deleteIdInput').value;

  // 建立要傳送的資料物件
  const requestData = {
      type: 'delete',
      id: deleteId
  };

  // 將資料物件轉成 JSON 格式
  const jsonRequestData = JSON.stringify(requestData);

  // 發送資料給伺服器
  socket.send(jsonRequestData);
}

// 假設你從後端獲取的 JSON 數據長這樣
const productData = {
  title: "商品名稱",
  category: ["書房．辦公家具","收納用品","哭阿測試"],
  price: "99",
  depth: "10 cm",
  width: "20 cm",
  height: "30 cm",
  material: "木頭，其實是鐵",
  description: "這是一個描述商品的範例文字。",
  imageURL: "ProductImages/Example2.jpg", //暫時連結
  modelURL: "https://raw.githubusercontent.com/KhronosGroup/glTF-Sample-Models/master/2.0/BoomBox/glTF-Binary/BoomBox.glb"
};

// 使用獲取到的數據填充 HTML 元素的內容
document.getElementById("productTitle").textContent = productData.title;
document.getElementById("category").textContent = `分類：${productData.category.join("、")}`;
document.getElementById("price").textContent = `售價：NT$ ${productData.price}`;
document.getElementById("depth").textContent = `深度:${productData.depth}`;
document.getElementById("width").textContent = `寬度:${productData.width}`;
document.getElementById("height").textContent = `高度:${productData.height}`;
document.getElementById("material").textContent = `材質:${productData.material}`;
document.getElementById("description").textContent = `描述：${productData.description}`;
document.getElementById("image").src = productData.imageURL;
document.getElementById("model").href = productData.modelURL;

// 登出問題
function myFunction() {
  var text = "確定要登出?";
  if (confirm(text) == true) {
    window.location.href = "/";
  }
}