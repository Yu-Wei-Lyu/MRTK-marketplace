const socket = new WebSocket("ws://118.150.125.153:8765");

socket.addEventListener("open", function (event) {
  // 連接成功發送消息
  socket.send('{"type":"query_website"}');
  // 預備功能，get廠商localstorage，然後send消息，然後拿資料。
});

let dataList = [];
let isCategoryMode = true;

// 當接收到訊息時更新網頁內容
socket.onmessage = function (event) {
  try {
    const data = JSON.parse(event.data);
    const message_type = data.type;
    // 假設 data 是您接收到的物件陣列
    dataList = data.message; // 如果 'message' 包含資料物件

    if (message_type == "query_website") {
      // 取得要顯示資料的容器
      console.log(dataList);
      showProductList(dataList);
    }
  } catch (error) {
    console.error("Error parsing JSON:", error);
  }
};

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
  const noTagsAdditionalArea = document.getElementById("noTagsAdditionalArea");
  const study_office_furniture = document.getElementById(
    "study_office_furniture"
  );
  const living_room_furniture = document.getElementById(
    "living_room_furniture"
  );
  const dining_room_furniture = document.getElementById(
    "dining_room_furniture"
  );
  const kitchen_furniture = document.getElementById("kitchen_furniture");
  const beds_dressing_tables = document.getElementById("beds_dressing_tables");
  const storage_solutions = document.getElementById("storage_solutions");
  const curtains_window_decor = document.getElementById(
    "curtains_window_decor"
  );
  const children_baby_supplies = document.getElementById(
    "children_baby_supplies"
  );
  const wardrobes_shoe_cabinets = document.getElementById(
    "wardrobes_shoe_cabinets"
  );
  const carpets_mats = document.getElementById("carpets_mats");
  const other = document.getElementById("other");

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

    // 將生成的商品範例插入到 noTagsAdditionalArea 區塊中
    noTagsAdditionalArea.innerHTML += productHtml;

    // 解析商品標籤、插入特定區塊
    const productTags = product.Tags.split("、");
    productTags.forEach((tag) => {
      switch (tag) {
        case "書房．辦公家具":
          study_office_furniture.innerHTML += productHtml;
          break;
        case "客廳家具":
          living_room_furniture.innerHTML += productHtml;
          break;
        case "餐廳家具":
          dining_room_furniture.innerHTML += productHtml;
          break;
        case "廚房家具":
          kitchen_furniture.innerHTML += productHtml;
          break;
        case "床・化妝台":
          beds_dressing_tables.innerHTML += productHtml;
          break;
        case "收納用品":
          storage_solutions.innerHTML += productHtml;
          break;
        case "窗簾．窗飾用品":
          curtains_window_decor.innerHTML += productHtml;
          break;
        case "孩童用品．嬰幼兒用品":
          children_baby_supplies.innerHTML += productHtml;
          break;
        case "衣櫃・鞋櫃":
          wardrobes_shoe_cabinets.innerHTML += productHtml;
          break;
        case "地毯．地墊":
          carpets_mats.innerHTML += productHtml;
          break;
        case "其他":
          other.innerHTML += productHtml;
          break;
        // ...more
        default:
          break;
      }
    });
  });
}

function viewDetails(id) {
  // 提取所需的部分，即detail_后面的内容
  var furniture_ID = id.split("_")[1];
  // 把家具ID加入URL的後面，為了傳遞變數給下一個網頁
  var url =
    "/templates/product_detail.html?variable=" +
    encodeURIComponent(furniture_ID);
  window.location.href = url;
  // console.log("Clicked on detail for ID: " + furniture_ID);
}

function toggleCategoryMode() {
  const categoryModeButton = document.getElementById("categoryModeButton");
  const noTagsSection = document.getElementById("noTagsSection");
  const taggedSection = document.getElementById("taggedSections");

  document.getElementById("searchBox").value = "";
  document.getElementById("productArea").style.display = "block";
  document.getElementById("searchSection").style.display = "none";

  if (isCategoryMode) {
    console.log("1");
    noTagsSection.style.display = "block";
    taggedSection.style.display = "none";
    categoryModeButton.textContent = "無分類模式";
    isCategoryMode = !isCategoryMode;
  } else {
    console.log("2");
    noTagsSection.style.display = "none";
    taggedSection.style.display = "block";
    categoryModeButton.textContent = "分類模式";
    isCategoryMode = !isCategoryMode;
  }
}

// 標籤切換
function setupTagToggler(tagListId, sections) {
  const tagListItems = document.querySelectorAll(
    `#${tagListId} .list-group-item`
  );

  // 把每個標籤加上點擊事件
  tagListItems.forEach((item) => {
    item.addEventListener("click", () => {
      // 移除所有標籤的active狀態
      tagListItems.forEach((li) => {
        li.classList.remove("active");
      });

      // 讓現在點擊的被active
      item.classList.add("active");

      // 拿到section屬性
      const targetSectionId = item.getAttribute("data-section");

      // 根據點擊的標籤決定要顯示啥
      sections.forEach((section) => {
        if (targetSectionId === "all" || section.id === targetSectionId) {
          section.style.display = "block";
        } else {
          section.style.display = "none";
        }
      });
    });
  });
}

// 在網頁load的時候設定tag功能
window.addEventListener("load", () => {
  const sections = document.querySelectorAll("section:not(#noTagsSection)");
  setupTagToggler("tagList", sections);
});

//
//     搜尋功能開始
//

// 獲得DOM
const searchBox = document.getElementById("searchBox");
const searchSection = document.getElementById("searchSection");
const searchArea = document.getElementById("searchArea");
const productArea = document.getElementById("productArea");

// 監聽輸入事件
searchBox.addEventListener("input", function () {
  // 取得keyword
  const keyword = searchBox.value.trim().toLowerCase();

  // 必要性隱藏
  if (keyword !== "") {
    searchSection.style.display = "block";
    productArea.style.display = "none";
  } else {
    searchSection.style.display = "none";
    productArea.style.display = "block";
  }
  // 清空商品的容器
  searchArea.innerHTML = "";

  // 遍歷商品資料，找到對應的
  dataList.forEach((product) => {
    if (product.Name.toLowerCase().includes(keyword)) {
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

      searchArea.innerHTML += productHtml;
    }
  });
});

function myFunction() {
  var text = "確定要登出?";
  if (confirm(text) == true) {
    window.location.href = "/";
  }
}
