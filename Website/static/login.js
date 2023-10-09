const socket = new WebSocket("ws://118.150.125.153:8765");

// 當接收到訊息時更新網頁內容
socket.onmessage = function (event) {
  try {
    const data = JSON.parse(event.data);
    const message_type = data.type;
    console.log(data);
    console.log(data.user);
    console.log(data.user.id);

    if (message_type == "LoginSuccess") {
      const userID = data.user.id;
      const Manufacturer = data.user.Manufacturer;
      setDataInLocalStorage(userID, Manufacturer);
      console.log("userid:");
      console.log(getUserIDFromLocalStorage());
      console.log("Manufacturer:");
      console.log(getManufacturerFromLocalStorage());

      location.href = "/Website/templates/main_member.html";
    } else if (message_type == "LoginFail") {
      console.log("Login Fail");
    }
  } catch (error) {
    console.error("Error parsing JSON:", error);
  }
};
// 把UserID存到LocalStorage
function setDataInLocalStorage(userID, Manufacturer) {
  localStorage.setItem("UserID", userID);
  localStorage.setItem("Manufacturer", Manufacturer);
}

// 從LocalStorage獲得UserID
function getUserIDFromLocalStorage() {
  return localStorage.getItem("UserID");
}

// 從LocalStorage獲得UserID
function getManufacturerFromLocalStorage() {
  return localStorage.getItem("Manufacturer");
}

function Login() {
  console.log("login");

  const account = document.getElementById("Enteraccount").value;
  const password = document.getElementById("Enterpassword").value;

  // 建立要傳送的資料物件
  const requestData = {
    type: "Login",
    id: account,
    password: password,
  };

  // 將資料物件轉成 JSON 格式
  const jsonRequestData = JSON.stringify(requestData);
  socket.send(jsonRequestData);
}
