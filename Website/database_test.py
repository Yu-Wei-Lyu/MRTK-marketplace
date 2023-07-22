import asyncio
import websockets
import mysql.connector

# 連線參數
config = {
    'user': '109590037',
    'password': '109590037',
    'host': '118.150.125.153',
    'port': 8888,  # 預設 MySQL 連接埠
    'database': 'mydatabase',
    'auth_plugin': 'mysql_native_password'
}

# 建立連線
conn = mysql.connector.connect(**config)

# 建立遊標物件
cursor = conn.cursor()

# 定義 WebSocket 連線的處理函式
async def handle_connection(websocket, path):
    async for message in websocket:
        if message == 'query':
            # 執行 SQL 查詢
            query = "SELECT * FROM furniture;"
            cursor.execute(query)

            # 取得查詢結果
            result = cursor.fetchall()

            # 將查詢結果轉成字串格式
            result_str = str(result)

            # 傳送查詢結果到瀏覽器
            await websocket.send(result_str)


# 設定伺服器的公有 IP 和埠號
public_ip = '0.0.0.0'
port = 8765

# 以公有 IP 和埠號啟動 WebSocket 伺服器
start_server = websockets.serve(handle_connection, public_ip, port)

# 開始運行伺服器
asyncio.get_event_loop().run_until_complete(start_server)
asyncio.get_event_loop().run_forever()