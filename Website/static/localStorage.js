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

// 使用範例
// setUserIDInLocalStorage("12345"); // 存UserID
// const userID = getUserIDFromLocalStorage(); // 抓UserID
// console.log(userID); // 印出UserID
