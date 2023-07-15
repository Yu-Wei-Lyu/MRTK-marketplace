import pyodbc

# 連線字串
conn_str = 'Driver={SQL Server};Server=127.0.0.1;Database=LAPTOP-3IRVH10J;UID=109590037;PWD=109590037'

try:
    # 建立連線
    conn = pyodbc.connect(conn_str)
    print("驅動程式已安裝")
    
    # 進一步操作...
    
    # 關閉連線
    conn.close()
except pyodbc.Error as e:
    print("無法建立連線：", e)
