show databases;
CREATE DATABASE IF NOT EXISTS mydatabase;

USE mydatabase;

Drop Table Furniture;

show tables;

Drop Table users;
Drop Table orders;
Drop Table order_items;
Drop Table history;
Drop Table furniture;

-- 建立家具表格
CREATE TABLE Furniture (
  ID INT AUTO_INCREMENT PRIMARY KEY,
	Name VARCHAR(50),
	Price DECIMAL(10),
	Size VARCHAR(30),
	Tags VARCHAR(100),
	Description VARCHAR(100),
	Material VARCHAR(50),
	Manufacturer VARCHAR(50),
	ImageURL Text,
	ModelURL Text
);

ALTER TABLE Furniture
ADD ImagePath VARCHAR(255);

ALTER TABLE History MODIFY HistoryID INT PRIMARY KEY AUTO_INCREMENT;

ALTER TABLE Furniture MODIFY ID INT AUTO_INCREMENT PRIMARY KEY;

ALTER TABLE Furniture MODIFY manufacturers varchar(50);

ALTER TABLE Furniture ADD ImageURL VARCHAR(100);

ALTER TABLE Furniture DROP COLUMN ImageURL;

-- 插入家具資料
INSERT INTO Furniture (Name, Price, Size, Tags, Description, Material, Manufacturer, ImageURL, ModelURL)
VALUES
    ('椅子', 1000.00, '60x60x80', '', '舒適的布藝椅子', '布料、木材', '', '', ''),
    ('沙發', 5000.00, '180x80x70', '', '現代風格皮革沙發', '皮革、鋼架', '', '', ''),
    ('書桌', 800.00, '120x60x75', '', '簡約風格書桌', '木材', '', '', ''),
    ('床架', 3000.00, '160x200', '', '現代風格床架', '木材、金屬', '', '', ''),
    ('餐桌', 1500.00, '120x80x75', '', '實木餐桌', '木材', '', '', ''),
    ('衣櫃', 2000.00, '180x60x200', '', '現代風格衣櫃', '木材、鏡子', '', '', ''),
    ('電視櫃', 1200.00, '160x40x50', '', '簡約風格電視櫃', '木材、玻璃', '', '', ''),
    ('書架', 600.00, '80x30x180', '', '經典木質書架', '木材', '', '', ''),
    ('檯燈', 200.00, '30x30x50', '', '現代風格檯燈', '金屬、塑膠', '', '', ''),
    ('茶几', 800.00, '80x80x40', '', '實木茶几', '木材', '', '', '');

-- 使用者範例資料
INSERT INTO Users (Username, Password, Email, Department)
VALUES	('109590037', '109590037', 'user1@example.com', 'ALL'),
		('109590038', '109590038', 'user2@example.com', 'ALL'),
		('109590039', '109590039', 'user3@example.com', 'ALL');

CREATE TABLE Users (
    UserID INT AUTO_INCREMENT PRIMARY KEY,
    Username VARCHAR(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
    Password VARCHAR(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
    Email VARCHAR(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci,
    Department VARCHAR(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci,
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE History (
  HistoryID INT PRIMARY KEY,
  UserID INT,
  Event VARCHAR(100),
  -- 其他歷史紀錄相關欄位...
  FOREIGN KEY (UserID) REFERENCES users(id)
);

CREATE TABLE Orders (
  OrderID INT PRIMARY KEY AUTO_INCREMENT,
  CustomerID INT NOT NULL,
  OrderDate DATE NOT NULL,
  DeliveryDate DATE,
  OrderStatus VARCHAR(50) NOT NULL,
  FOREIGN KEY (CustomerID) REFERENCES users(id)
);

CREATE TABLE Order_Items (
  ItemID INT PRIMARY KEY AUTO_INCREMENT,
  OrderID INT NOT NULL,
  FurnitureID INT NOT NULL,
  Quantity INT NOT NULL,
  Price DECIMAL(10, 2) NOT NULL,
  FOREIGN KEY (OrderID) REFERENCES Orders(OrderID),
  FOREIGN KEY (FurnitureID) REFERENCES Furniture(id)
);


DELIMITER //
CREATE TRIGGER limit_history
AFTER INSERT ON History
FOR EACH ROW
BEGIN
  DECLARE history_count INT;
  DECLARE history_id_to_delete INT;
  
  -- 計算特定UserID的歷史紀錄數量
  SELECT COUNT(*) INTO history_count FROM History WHERE UserID = NEW.UserID;
  
  -- 如果歷史紀錄數量超過五筆，則找到最早的 HistoryID
  IF history_count > 5 THEN
    SELECT MIN(HistoryID) INTO history_id_to_delete
    FROM History WHERE UserID = NEW.UserID
    ORDER BY HistoryID;
    
    -- 刪除最早的歷史紀錄
    DELETE FROM History WHERE HistoryID = history_id_to_delete;
  END IF;
END //
DELIMITER ;

-- 插入帳號
INSERT INTO users (username, password, role)
VALUES  ('109590037', '109590037', 'admin'),
		('109590004', '109590004', 'admin'),
		('109590003', '109590003', 'admin'),
        ('JohnDoe', 'password1', 'member'),
		('JaneSmith', 'password2', 'member'),
		('MikeJohnson', 'password3', 'member'),
		('EmilyBrown', 'password4', 'member'),
		('DavidLee', 'password5', 'member');

Select * from Furniture;
SELECT * FROM furniture;
select * from order_items;
Select * from history;
Select * from users;

describe furniture;
describe order_items;
describe orders;

UPDATE Furniture
SET Name='Test', Number='123', Price=243, ImagePath='ImagePath/Image.jpg', Size='200x200x100', Description='Test', Material='Test'
WHERE ID = 12;

DELETE FROM furniture WHERE id = 15;

SHOW PROCESSLIST;
Kill 55;

INSERT INTO Furniture (Name, Price, Size, Tags, Description, Material, Manufacturer, ImageURL, ModelURL)
VALUES ('典雅空間櫥櫃', 59999, '71.2x196x244', '廚房家具', '現代櫥櫃廚具，簡約設計融合實用功能，高品質材料與創新設計完美結合，為現代生活注入風格與便利。', '大理石、高壓塑膠板、金屬', 'Unreal Factory', 'http://26.122.221.31:8765/modern_cabinet_hutch.jpg', 'http://26.122.221.31:8765/modern_cabinet_hutch.glb');

INSERT INTO Furniture (Name, Price, Size, Tags, Description, Material, Manufacturer, ImageURL, ModelURL)
VALUES ('古董立鏡', 12999, '62.75x89.46x194.12', '其他', '歷史情懷中的精緻古董立鏡，華美雕飾和雙面設計，為您的空間增添典雅氛圍。', '實木框架、金屬飾件、玻璃鏡面', 'Unreal Factory', 'http://26.122.221.31:8765/antique_standing_mirror.png', 'http://26.122.221.31:8765/antique_standing_mirror.glb');

INSERT INTO Furniture (Name, Price, Size, Tags, Description, Material, Manufacturer, ImageURL, ModelURL)
VALUES ('古典歐洲風格奢華實木雕花紅木大理石餐桌椅家具套裝，室內裝潢設計師首選', 800, '120x60x75', '', '當我們面臨眾多選擇時，有時會感到困惑，就像置身於知識的大海，每一波浪都代表一種可能性，等待我們去探索。生活就像一本永無止境的書，每一頁都充滿著不同的故事和冒險，等待我們翻閱。', '木材', NULL, NULL);
