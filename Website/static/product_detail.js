const socket = new WebSocket("ws://118.150.125.153:8765");

var furniture_ID;

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
    const dataList = data.message;

    if (message_type == "query_ID") {
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

function showdetails(productData) {
  console.log(productData);
  // 使用獲取到的數據填充 HTML 元素的內容
  document.getElementById("productTitle").textContent = productData.Name;
  document.getElementById("Tags").textContent = `分類：${productData.Tags}`;
  document.getElementById("price").textContent = `售價：NT$ ${productData.Price}`;
  var sizeParts = productData.Size.split("x");
  var width = sizeParts[0];
  var depth = sizeParts[1];
  var height = sizeParts[2];
  document.getElementById("width").textContent = `寬度：${width} cm`;
  document.getElementById("depth").textContent = `深度：${depth} cm`;
  document.getElementById("height").textContent = `高度：${height} cm`;
  document.getElementById("material").textContent = `材質：${productData.Material}`;
  document.getElementById("description").textContent = `描述：${productData.Description}`;
  document.getElementById("image").src = productData.ImageURL;
  // document.getElementById("image").src = "ProductImages/Example2.jpg";
  document.getElementById("model").href = productData.ModelURL;
}

function editProduct() {

  var url =
    "/Website/templates/edit_product.html?variable=" +
    encodeURIComponent(furniture_ID);
  // var url =
  //   "/templates/edit_product.html?variable=" +
  //   encodeURIComponent(23);
  window.location.href = url;
  // console.log("Clicked on detail for ID: " + furniture_ID);
}

// 登出問題
function myFunction() {
  var text = "確定要登出?";
  if (confirm(text) == true) {
    window.location.href = "/";
  }
}
