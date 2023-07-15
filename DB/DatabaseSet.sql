show databases;
CREATE DATABASE IF NOT EXISTS mydatabase;

USE mydatabase;

Drop Table furniture;

show tables;

-- 建立家具表格
CREATE TABLE Furniture (
  ID INT AUTO_INCREMENT PRIMARY KEY,
  Name VARCHAR(50),
  Number VARCHAR(10),
  Price DECIMAL(10, 2),
  ImagePath VARCHAR(100),
  Size VARCHAR(20),
  Description VARCHAR(100),
  Material VARCHAR(50)
);

ALTER TABLE Furniture
ADD ImagePath VARCHAR(255);

ALTER TABLE Furniture MODIFY ImagePath VARCHAR(255) NULL;

ALTER TABLE Furniture MODIFY ID INT AUTO_INCREMENT PRIMARY KEY;

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

-- 插入帳號
INSERT INTO users (username, password, role)
VALUES ('john', 'password123', 'admin');



Select * from Furniture;