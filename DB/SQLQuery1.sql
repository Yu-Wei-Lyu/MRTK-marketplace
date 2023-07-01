-- 建立家具資料表
CREATE TABLE furniture (
  id VARCHAR(10) PRIMARY KEY,
  name VARCHAR(100),
  description VARCHAR(500),
  price DECIMAL(10, 2),
  material VARCHAR(100),
  color VARCHAR(50),
  image VARCHAR(200),
  category VARCHAR(50),
  brand VARCHAR(100)
);

-- 插入家具資料
INSERT INTO furniture (id, name, description, price, material, color, image, category, brand) VALUES
('FUR001', '現代風格皮質沙發', '這款皮質沙發結合了現代風格和舒適設計，適合放置在客廳或休息室。', 2999, '皮革', '黑色', 'https://example.com/images/furniture/fur001.jpg', '沙發', '家具之家'),
('FUR002', '簡約木質餐桌', '這款餐桌採用簡約的木質設計，適合家庭用餐或小型聚會。', 1499, '木材', '原木色', 'https://example.com/images/furniture/fur002.jpg', '餐桌', '居家設計'),
('FUR003', '摺疊休閒椅', '這款休閒椅設計簡單輕巧，適合戶外使用，方便摺疊收納。', 399, '金屬和塑料', '白色', 'https://example.com/images/furniture/fur003.jpg', '椅子', '生活休閒'),
('FUR004', '經典純棉床墊', '這款床墊由純棉材質製成，提供舒適的睡眠體驗。', 1999, '棉花', '白色', 'https://example.com/images/furniture/fur004.jpg', '床墊', '舒眠居家'),
('FUR005', '現代風格儲物櫃', '這款儲物櫃以現代簡約的風格設計，提供實用的收納空間。', 899, '木材和金屬', '灰色', 'https://example.com/images/furniture/fur005.jpg', '儲物櫃', '居家實用'),
('FUR006', '北歐風格懶人沙發', '這款懶人沙發設計簡約舒適，適合放鬆休息和觀賞影片。', 2499, '布料', '淺灰色', 'https://example.com/images/furniture/fur006.jpg', '沙發', '時尚居家'),
('FUR007', '經典風格書桌', '這款書桌以經典風格設計，提供寬敞的桌面和儲物空間。', 799, '木材', '橡木色', 'https://example.com/images/furniture/fur007.jpg', '書桌', '居家工作'),
('FUR008', '摩登設計電視櫃', '這款電視櫃結合了摩登設計和實用功能，適合放置平面電視和多媒體設備。', 1299, '木材和玻璃', '黑色', 'https://example.com/images/furniture/fur008.jpg', '電視櫃', '家居電影院'),
('FUR009', '輕便折疊桌椅組', '這組桌椅組包括一張折疊桌子和四把輕便折疊椅子，方便攜帶和存放。', 599, '金屬和塑料', '藍色', 'https://example.com/images/furniture/fur009.jpg', '桌椅組', '便攜式家具'),
('FUR010', '經典風格床頭櫃', '這款床頭櫃以經典風格設計，提供便利的放置空間和抽屜。', 499, '木材', '白色', 'https://example.com/images/furniture/fur010.jpg', '床頭櫃', '夢幻居家');


-- 建立顧客資料表
CREATE TABLE customers (
  id INT PRIMARY KEY AUTO_INCREMENT,
  name VARCHAR(50),
  address VARCHAR(100),
  phone VARCHAR(20),
  birthday DATE,
  id_number VARCHAR(20)
);

-- 插入顧客資料
INSERT INTO customers (name, address, phone, birthday, id_number) VALUES
('陳小明', '台北市中山區', '0912345678', '1990-05-10', 'A123456789'),
('林美美', '新北市板橋區', '0923456789', '1985-09-20', 'B234567890'),
('王大雄', '台中市西屯區', '0934567890', '1992-12-15', 'C345678901'),
('張小華', '高雄市鳳山區', '0945678901', '1988-07-05', 'D456789012'),
('李明哲', '台南市北區', '0956789012', '1995-02-28', 'E567890123'),
('陳美玲', '桃園市中壢區', '0967890123', '1991-11-18', 'F678901234'),
('黃大勇', '台中市南區', '0978901234', '1987-03-25', 'G789012345'),
('蔡小琪', '新北市三重區', '0989012345', '1993-08-12', 'H890123456'),
('劉春花', '高雄市苓雅區', '0990123456', '1994-04-30', 'I901234567'),
('林志偉', '台北市信義區', '0901234567', '1989-01-05', 'J012345678');

DECLARE @sql NVARCHAR(MAX) = N'';

SELECT @sql += N'DROP TABLE ' + QUOTENAME(TABLE_SCHEMA) + N'.' + QUOTENAME(TABLE_NAME) + N';'
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_TYPE = 'BASE TABLE' AND TABLE_CATALOG = 'shopdbitem.database.windows.net'; -- 將 'YourDatabaseName' 替換為你的資料庫名稱

EXEC sp_executesql @sql;

SELECT
    i.name AS IndexName,
    OBJECT_NAME(i.object_id) AS TableName
FROM
    sys.indexes i
WHERE
    OBJECT_NAME(i.object_id) = 'Address'; -- 將 'YourTableName' 替換為欲刪除的資料表名稱


DROP INDEX PK_Address_AddressID ON SalesLT.Address; -- 將 'YourIndexName' 替換為欲刪除的索引名稱，'YourTableName' 替換為欲刪除索引的資料表名稱

ALTER TABLE SalesLT.Address NOCHECK CONSTRAINT All ;

ALTER TABLE SalesOrderHeader NOCHECK CONSTRAINT FK_SalesOrderHeader_Address_ShipTo_AddressID;


