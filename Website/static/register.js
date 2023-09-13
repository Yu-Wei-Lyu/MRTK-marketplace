const socket = new WebSocket("ws://118.150.125.153:8765");

// 當接收到訊息時更新網頁內容
socket.onmessage = function (event) {
  try {
    const data = JSON.parse(event.data);
    const message_type = data.type;
    // 假設 data 是您接收到的物件陣列
    const dataList = data.message; // 如果 'message' 包含資料物件

    if (message_type == "user_created") {
      location.href = "/templates/login.html";
    }
  } catch (error) {
    console.error("Error parsing JSON:", error);
  }
};

// 點擊按鈕時向伺服器發送刪除資料的請求
function addUser() {
  console.log("addUser");
  const account = document.getElementById("account").value;
  const password = document.getElementById("password").value;
  const email = document.getElementById("email").value;
  const manufacturer = document.getElementById("manufacturer").value;

  // 建立要傳送的資料物件
  const requestData = {
    type: "addUser",
    id: account,
    password: password,
    email: email,
    Manufacturer: manufacturer,
  };

  // 將資料物件轉成 JSON 格式
  const jsonRequestData = JSON.stringify(requestData);
  socket.send(jsonRequestData);
}
