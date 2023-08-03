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
  Number VARCHAR(10),
  Price DECIMAL(10, 2),
  ImagePath VARCHAR(100),
  Size VARCHAR(20),
  Description VARCHAR(100),
  Material VARCHAR(50),
  manufacturers VARCHAR(50)
);

ALTER TABLE Furniture
ADD ImagePath VARCHAR(255);

ALTER TABLE History MODIFY HistoryID INT PRIMARY KEY AUTO_INCREMENT;

ALTER TABLE Furniture MODIFY ID INT AUTO_INCREMENT PRIMARY KEY;

ALTER TABLE Furniture MODIFY manufacturers varchar(50);


-- 插入家具資料
INSERT INTO Furniture (Name, Number, Price, ImagePath, Size, Description, Material)
VALUES
  ('椅子', 'C001', 1000, 'chair.jpg', '60x60x80', '舒適的布藝椅子', '布料、木材'),
  ('沙發', 'S001', 5000, 'sofa.jpg', '180x80x70', '現代風格皮革沙發', '皮革、鋼架'),
  ('書桌', 'D001', 800, 'desk.jpg', '120x60x75', '簡約風格書桌', '木材'),
  ('床架', 'B001', 3000, 'bed.jpg', '160x200', '現代風格床架', '木材、金屬'),
  ('餐桌', 'T001', 1500, 'dining_table.jpg', '120x80x75', '實木餐桌', '木材'),
  ('衣櫃', 'W001', 2000, 'wardrobe.jpg', '180x60x200', '現代風格衣櫃', '木材、鏡子'),
  ('電視櫃', 'TV001', 1200, 'tv_stand.jpg', '160x40x50', '簡約風格電視櫃', '木材、玻璃'),
  ('書架', 'B002', 600, 'bookshelf.jpg', '80x30x180', '經典木質書架', '木材'),
  ('檯燈', 'L001', 200, 'desk_lamp.jpg', '30x30x50', '現代風格檯燈', '金屬、塑膠'),
  ('茶几', 'C002', 800, 'coffee_table.jpg', '80x80x40', '實木茶几', '木材');

-- 創建 users 表格
CREATE TABLE users (
  id INT PRIMARY KEY AUTO_INCREMENT,
  username VARCHAR(50) UNIQUE NOT NULL,
  password VARCHAR(255) NOT NULL,
  role VARCHAR(50) NOT NULL
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

UPDATE Furniture
SET Name='Test', Number='123', Price=243, ImagePath='ImagePath/Image.jpg', Size='200x200x100', Description='Test', Material='Test'
WHERE ID = 12;


SHOW PROCESSLIST;
Kill 33;
