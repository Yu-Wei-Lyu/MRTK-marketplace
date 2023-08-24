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

// 假設你有一個包含商品資料的陣列，每個元素都是一個商品物件
const products = [
  { id: 1, title: "商品名稱1", imageUrl: "ProductImages/Example1.jpg" },
  { id: 2, title: "商品名稱2", imageUrl: "ProductImages/Example2.jpg" },
  { id: 3, title: "商品名稱3", imageUrl: "ProductImages/Example3.jpg" },
  { id: 4, title: "商品名稱4", imageUrl: "ProductImages/Example4.jpg" },
  { id: 5, title: "商品名稱5", imageUrl: "ProductImages/Example5.jpg" },
  { id: 6, title: "商品名稱6", imageUrl: "ProductImages/Example6.jpg" },
  { id: 7, title: "商品名稱7", imageUrl: "ProductImages/Example7.jpg" },
  // ...更多商品資料...
];

// 找到需要加入商品範例的區塊
const additionalArea = document.getElementById("additionalArea");

// 遍歷商品資料陣列，生成並插入每個商品範例
products.forEach(product => {
  const productHtml = `
    <div class="col mb-5" id="field${product.id}">
      <div class="card h-100">
        <img class="card-img-top" src="${product.imageUrl}" alt="..." />
        <div class="card-body p-4">
          <div class="text-center">
            <h3 id="title${product.id}" class="fw-bolder">${product.title}</h3>
          </div>
        </div>
        <div class="card-footer p-4 pt-0 border-top-0 bg-transparent">
          <div class="text-center">
            <a id="detail${product.id}" class="btn btn-outline-dark mt-auto" href="product_detail.html" onclick="reserve_book(this.id)">詳細資訊</a>
          </div>
        </div>
      </div>
    </div>
  `;

  // 將生成的商品範例插入到 additionalArea 區塊中
  additionalArea.innerHTML += productHtml;
});

function myFunction() {
  var text = "確定要登出?";
  if (confirm(text) == true) {
    window.location.href = "/";
  }
}
