-- �إ߮a���ƪ�
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

-- ���J�a����
INSERT INTO furniture (id, name, description, price, material, color, image, category, brand) VALUES
('FUR001', '�{�N����ֽ�F�o', '�o�ڥֽ�F�o���X�F�{�N����M�ξA�]�p�A�A�X��m�b���U�Υ𮧫ǡC', 2999, '�֭�', '�¦�', 'https://example.com/images/furniture/fur001.jpg', '�F�o', '�a�㤧�a'),
('FUR002', '²������\��', '�o���\��ĥ�²�������]�p�A�A�X�a�x���\�Τp���E�|�C', 1499, '���', '����', 'https://example.com/images/furniture/fur002.jpg', '�\��', '�~�a�]�p'),
('FUR003', '�P�|�𶢴�', '�o�ڥ𶢴ȳ]�p²�滴���A�A�X��~�ϥΡA��K�P�|���ǡC', 399, '���ݩM���', '�զ�', 'https://example.com/images/furniture/fur003.jpg', '�Ȥl', '�ͬ���'),
('FUR004', '�g��´֧ɹ�', '�o�ڧɹԥѯ´֧���s���A���ѵξA���ίv����C', 1999, '�֪�', '�զ�', 'https://example.com/images/furniture/fur004.jpg', '�ɹ�', '�ίv�~�a'),
('FUR005', '�{�N�����x���d', '�o���x���d�H�{�N²��������]�p�A���ѹ�Ϊ����ǪŶ��C', 899, '����M����', '�Ǧ�', 'https://example.com/images/furniture/fur005.jpg', '�x���d', '�~�a���'),
('FUR006', '�_�ڭ����i�H�F�o', '�o���i�H�F�o�]�p²���ξA�A�A�X���P�𮧩M�[��v���C', 2499, '����', '�L�Ǧ�', 'https://example.com/images/furniture/fur006.jpg', '�F�o', '�ɩ|�~�a'),
('FUR007', '�g�孷��Ѯ�', '�o�ڮѮ�H�g�孷��]�p�A���Ѽe�����ୱ�M�x���Ŷ��C', 799, '���', '����', 'https://example.com/images/furniture/fur007.jpg', '�Ѯ�', '�~�a�u�@'),
('FUR008', '���n�]�p�q���d', '�o�ڹq���d���X�F���n�]�p�M��Υ\��A�A�X��m�����q���M�h�C��]�ơC', 1299, '����M����', '�¦�', 'https://example.com/images/furniture/fur008.jpg', '�q���d', '�a�~�q�v�|'),
('FUR009', '���K���|��Ȳ�', '�o�ծ�Ȳե]�A�@�i���|��l�M�|�⻴�K���|�Ȥl�A��K��a�M�s��C', 599, '���ݩM���', '�Ŧ�', 'https://example.com/images/furniture/fur009.jpg', '��Ȳ�', '�K�⦡�a��'),
('FUR010', '�g�孷����Y�d', '�o�ڧ��Y�d�H�g�孷��]�p�A���ѫK�Q����m�Ŷ��M��P�C', 499, '���', '�զ�', 'https://example.com/images/furniture/fur010.jpg', '���Y�d', '�ڤ۩~�a');


-- �إ��U�ȸ�ƪ�
CREATE TABLE customers (
  id INT PRIMARY KEY AUTO_INCREMENT,
  name VARCHAR(50),
  address VARCHAR(100),
  phone VARCHAR(20),
  birthday DATE,
  id_number VARCHAR(20)
);

-- ���J�U�ȸ��
INSERT INTO customers (name, address, phone, birthday, id_number) VALUES
('���p��', '�x�_�����s��', '0912345678', '1990-05-10', 'A123456789'),
('�L����', '�s�_���O����', '0923456789', '1985-09-20', 'B234567890'),
('���j��', '�x������ٰ�', '0934567890', '1992-12-15', 'C345678901'),
('�i�p��', '��������s��', '0945678901', '1988-07-05', 'D456789012'),
('������', '�x�n���_��', '0956789012', '1995-02-28', 'E567890123'),
('������', '��饫���c��', '0967890123', '1991-11-18', 'F678901234'),
('���j�i', '�x�����n��', '0978901234', '1987-03-25', 'G789012345'),
('���p�X', '�s�_���T����', '0989012345', '1993-08-12', 'H890123456'),
('�B�K��', '�������d����', '0990123456', '1994-04-30', 'I901234567'),
('�L�Ӱ�', '�x�_���H�q��', '0901234567', '1989-01-05', 'J012345678');

DECLARE @sql NVARCHAR(MAX) = N'';

SELECT @sql += N'DROP TABLE ' + QUOTENAME(TABLE_SCHEMA) + N'.' + QUOTENAME(TABLE_NAME) + N';'
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_TYPE = 'BASE TABLE' AND TABLE_CATALOG = 'shopdbitem.database.windows.net'; -- �N 'YourDatabaseName' �������A����Ʈw�W��

EXEC sp_executesql @sql;

SELECT
    i.name AS IndexName,
    OBJECT_NAME(i.object_id) AS TableName
FROM
    sys.indexes i
WHERE
    OBJECT_NAME(i.object_id) = 'Address'; -- �N 'YourTableName' ���������R������ƪ�W��


DROP INDEX PK_Address_AddressID ON SalesLT.Address; -- �N 'YourIndexName' ���������R�������ަW�١A'YourTableName' ���������R�����ު���ƪ�W��

ALTER TABLE SalesLT.Address NOCHECK CONSTRAINT All ;

ALTER TABLE SalesOrderHeader NOCHECK CONSTRAINT FK_SalesOrderHeader_Address_ShipTo_AddressID;


