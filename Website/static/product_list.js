const socket = new WebSocket("ws://118.150.125.153:8765");

socket.addEventListener("open", function (event) {
  // 連接成功發送消息
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
      showProductList(dataList);
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

// 假設你有一個包含商品資料的陣列，每個元素都是一個商品物件
// const products = [
//   { id: 1, title: "商品名稱1", imageUrl: "ProductImages/Example1.jpg" },
//   { id: 2, title: "商品名稱2", imageUrl: "ProductImages/Example2.jpg" },
//   { id: 3, title: "商品名稱3", imageUrl: "ProductImages/Example3.jpg" },
//   { id: 4, title: "商品名稱4", imageUrl: "ProductImages/Example4.jpg" },
//   { id: 5, title: "商品名稱5", imageUrl: "ProductImages/Example5.jpg" },
//   { id: 6, title: "商品名稱6", imageUrl: "ProductImages/Example6.jpg" },
//   { id: 7, title: "商品名稱7", imageUrl: "ProductImages/Example7.jpg" },
//   // ...更多商品資料...
// ];

function showProductList(products) {
  // 找到需要加入商品範例的區塊
  const additionalArea = document.getElementById("additionalArea");

  // 遍歷商品資料陣列，生成並插入每個商品範例
  products.forEach((product) => {
    const productHtml = `
    <div class="col mb-5" id="field_${product.ID}">
      <div class="card h-100">
        <img class="card-img-top" src="${product.ImageURL}" alt="..." />
        <div class="card-body p-4">
          <div class="text-center">
            <h3 id="title_${product.ID}" class="fw-bolder">${product.Name}</h3>
          </div>
        </div>
        <div class="card-footer p-4 pt-0 border-top-0 bg-transparent">
          <div class="text-center">
            <a id="detail_${product.ID}" class="btn btn-outline-dark mt-auto" onclick="viewDetails(this.id)">詳細資訊</a>
          </div>
        </div>
      </div>
    </div>
  `;

    // 將生成的商品範例插入到 additionalArea 區塊中
    additionalArea.innerHTML += productHtml;
  });
}

function viewDetails(id) {
  // 提取所需的部分，即detail_后面的内容
  var furniture_ID = id.split("_")[1];
  // 把家具ID加入URL的後面，為了傳遞變數給下一個網頁
  var url = "product_detail.html?variable=" + encodeURIComponent(furniture_ID);
  window.location.href = url;
  // console.log("Clicked on detail for ID: " + furniture_ID);
}

function myFunction() {
  var text = "確定要登出?";
  if (confirm(text) == true) {
    window.location.href = "/";
  }
}
