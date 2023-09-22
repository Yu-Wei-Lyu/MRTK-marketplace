const socket = new WebSocket("ws://118.150.125.153:8765");

socket.addEventListener("open", function (event) {
  // 連接成功
  // 獲取URL中的查詢字串
  var queryString = window.location.search;

  // 解析字串獲得家具ID
  var params = new URLSearchParams(queryString);
  var furniture_ID = params.get("variable");

  console.log("Received furniture_ID: " + furniture_ID);
  socket.send('{"type":"query_website"}');
});

// 當接收到訊息時更新網頁內容
socket.onmessage = function (event) {
  try {
    const data = JSON.parse(event.data);
    const message_type = data.type;
    // 假設 data 是您接收到的物件陣列
    const dataList = data.message; // 如果 'message' 包含資料物件

    if (message_type == "query_website") {
      // 取得要顯示資料的容器
      console.log(dataList[0]);
      showdetails(dataList[0]);
    }
  } catch (error) {
    console.error("Error parsing JSON:", error);
  }
};

// 點擊按鈕時發送訊息給伺服器
function sendQuery() {
  console.log("SendQuery");
  socket.send('{"type":"query"}');
}

// 點擊按鈕時向伺服器發送刪除資料的請求
function deleteData() {
  console.log("SendDelete");
  // 從輸入欄位獲取要刪除的資料的 ID
  const deleteId = document.getElementById("deleteIdInput").value;

  // 建立要傳送的資料物件
  const requestData = {
    type: "delete",
    id: deleteId,
  };

  // 將資料物件轉成 JSON 格式
  const jsonRequestData = JSON.stringify(requestData);

  // 發送資料給伺服器
  socket.send(jsonRequestData);
}

// 假設你從後端獲取的 JSON 數據長這樣
// const productData = {
//   title: "商品名稱",
//   category: ["書房．辦公家具", "收納用品", "哭阿測試"],
//   price: "99",
//   depth: "10 cm",
//   width: "20 cm",
//   height: "30 cm",
//   material: "木頭，其實是鐵",
//   description: "這是一個描述商品的範例文字。",
//   imageURL: "ProductImages/Example2.jpg", //暫時連結
//   modelURL:
//     "https://raw.githubusercontent.com/KhronosGroup/glTF-Sample-Models/master/2.0/BoomBox/glTF-Binary/BoomBox.glb",
// };

function showdetails(productData) {
  console.log(productData);
  // 使用獲取到的數據填充 HTML 元素的內容
  document.getElementById("productTitle").textContent = productData.Name;
  document.getElementById("Tags").textContent = `分類：${productData.Tags}`;
  document.getElementById("price").textContent = `售價：NT$ ${productData.Price}`;
  var sizeParts = productData.Size.split("x");
  var depth = sizeParts[0];
  var width = sizeParts[1];
  var height = sizeParts[2];
  document.getElementById("depth").textContent = `深度：${depth} cm`;
  document.getElementById("width").textContent = `寬度：${width} cm`;
  document.getElementById("height").textContent = `高度：${height} cm`;
  document.getElementById("material").textContent = `材質：${productData.Material}`;
  document.getElementById("description").textContent = `描述：${productData.Description}`;
  // document.getElementById("image").src = productData.ImageURL;
  document.getElementById("image").src = "ProductImages/Example2.jpg";
  document.getElementById("model").href = productData.ModelURL;
}

// 登出問題
function myFunction() {
  var text = "確定要登出?";
  if (confirm(text) == true) {
    window.location.href = "/";
  }
}
