import asyncio
import websockets
import mysql.connector
import json

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
        try:
            print(message)
            data = json.loads(message)
            message_type = data.get('type')
            print(message_type)

            if message_type == 'query':
                # 執行 SQL 查詢
                query = "SELECT * FROM furniture;"
                cursor.execute(query)

                # 取得查詢結果
                result = cursor.fetchall()

                # 將查詢結果轉成字串格式
                result_str = str(result)

                response = {'type': 'query', 'message': result_str}

            elif message_type == 'add':
                # 取得要新增的資料
                name = data.get('Name')
                number = data.get('Number')
                price = data.get('Price')
                imagePath = data.get('ImagePath')
                size = data.get('Size')
                description = data.get('Description')
                material = data.get('Material')

                # 執行 SQL 新增資料
                query = f"INSERT INTO furniture (Name, Number, Price, ImagePath, Size, Description, Material) VALUES ('{name}', '{number}', {price}, '{imagePath}', '{size}', '{description}', '{material}');"
                cursor.execute(query)
                conn.commit()

                response = {'type': 'add', 'message': 'Data added successfully'}

            elif message_type == 'update':
                
                # 取得要更新的資料的 ID
                update_id = int(data.get('id'))
                name = data.get('Name')
                number = int(data.get('Number'))
                price = data.get('Price')
                imagePath = data.get('ImagePath')
                size = data.get('Size')
                description = data.get('Description')
                material = data.get('Material')

                # 請根據需求在此處執行 SQL 更新資料的指令，例如：
                query = f"UPDATE Furniture SET Name='{name}', Number='{number}', Price={price}, ImagePath='{imagePath}', Size='{size}', Description='{description}', Material='{material}' WHERE ID = {update_id};"
                cursor.execute(query)
                conn.commit()

                response = {'type': 'update', 'message': 'Data updated successfully'}

            elif message_type == 'delete':
                 # 取得要刪除的資料的 ID
                delete_id = data.get('id')

                # 執行 SQL 刪除資料
                query = f"DELETE FROM furniture WHERE ID = {delete_id};"
                cursor.execute(query)
                conn.commit()
                response = {'type': 'delete', 'message': 'Data deleted successfully'}

            else:
                response = {'type': 'error', 'message': 'Invalid message type'}

            # 將結果回傳給前端
            await websocket.send(json.dumps(response))
        except json.JSONDecodeError as e:
            print(f"Error parsing JSON: {e}")
            # 發生錯誤時回傳錯誤訊息
            response = {'type': 'error', 'message': 'Invalid JSON format'}
            await websocket.send(json.dumps(response))


# 設定伺服器的公有 IP 和埠號
public_ip = '0.0.0.0'
port = 8765

# 以公有 IP 和埠號啟動 WebSocket 伺服器
start_server = websockets.serve(handle_connection, public_ip, port)

# 開始運行伺服器
asyncio.get_event_loop().run_until_complete(start_server)
asyncio.get_event_loop().run_forever()