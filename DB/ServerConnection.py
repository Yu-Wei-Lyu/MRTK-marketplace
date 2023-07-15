import pyodbc

# 連線字串
conn_str = {"Driver={SQL Server}; Server=118.150.125.153 ; Database=Project ; UID=109590037 ; PWD=109590037" }
# 建立連線
conn = pyodbc.connect(conn_str)

# 建立遊標物件
cursor = conn.cursor()

# 建立資料庫指令
create_db_query = 'CREATE DATABASE MyDatabase'
cursor.execute(create_db_query)

# 建立表格指令
create_table_query = 'CREATE TABLE MyTable (ID INT PRIMARY KEY, Name VARCHAR(50))'
cursor.execute(create_table_query)

# 提交變更
conn.commit()

# 關閉連線
conn.close()
