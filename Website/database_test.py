import asyncio
import websockets
import mysql.connector
import json
import base64
import re
import requests
import io
import os

# 定義檔案分割傳輸每一段的大小(8192 = 2^13)
chunkSize = 8192

def get_imgur_image_url(image_url):
    response = requests.get(image_url)

    if response.status_code == 200:
        image_content = response.content
        return image_content
    else:
        print('Error fetching Imgur image:', response.status_code)
        return None

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
    filename = None
    # 接收檔案分段內容的list
    content_chunks = []

    async for message in websocket:
        try:
            data = json.loads(message)
            message_type = data.get('type')
            print(message_type)

            if message_type == 'query':
                # 執行 SQL 查詢
                query = "SELECT * FROM furniture;"
                cursor.execute(query)

                # 取得查詢結果
                result = cursor.fetchall()

                result_data = []
        
                for row in result:  # 假設您有一個 result 包含查詢結果
        
                    # 將資料整理成字典，包括 ImageUrl
                    item = {
                        'ID': row[0],
                        'Name': row[1],
                        'Price': float(row[2]),
                        'Size': row[3],
                        'Tags': row[4],
                        'Description': row[5],
                        'Material': row[6],
                        'Manufacturer': row[7],
                        'ImageURL': row[8],
                        'ModelURL': row[9]
                    }
                    result_data.append(item)
                response = result_data
        
                # 將處理後的資料發送回前端網頁
                await websocket.send(json.dumps(response))

            elif message_type == 'query_website':
                # 執行 SQL 查詢
                query = "SELECT * FROM furniture;"
                cursor.execute(query)

                # 取得查詢結果
                result = cursor.fetchall()

                result_data = []
        
                for row in result:  # 假設您有一個 result 包含查詢結果
        
                    # 將資料整理成字典，包括 ImageUrl
                    item = {
                        'ID': row[0],
                        'Name': row[1],
                        'Price': float(row[2]),
                        'Size': row[3],
                        'Tags': row[4],
                        'Description': row[5],
                        'Material': row[6],
                        'Manufacturer': row[7],
                        'ImageURL': row[8],
                        'ModelURL': row[9]
                    }
                    result_data.append(item)
                response = {'type': 'query_webiste', 'message': result_data}
        
                # 將處理後的資料發送回前端網頁
                await websocket.send(json.dumps(response))

            elif message_type == 'query_user':
                Material = data.get('Material')
                # 執行 SQL 查詢
                query = "SELECT * FROM furniture WHERE Material = %s;"
                cursor.execute(query, Material)

                # 取得查詢結果
                result = cursor.fetchall()

                result_data = []
        
                for row in result:  # 假設您有一個 result 包含查詢結果
        
                    # 將資料整理成字典，包括 ImageUrl
                    item = {
                        'ID': row[0],
                        'Name': row[1],
                        'Price': float(row[2]),
                        'Size': row[3],
                        'Tags': row[4],
                        'Description': row[5],
                        'Material': row[6],
                        'Manufacturer': row[7],
                        'ImageURL': row[8],
                        'ModelURL': row[9]
                    }
                    result_data.append(item)
                response = {'type': 'query_webiste', 'message': result_data}

            elif message_type == 'add':
                # 如果filename當下不存在，才會接收資料，並且防止多次儲存。
                # 因為檔案上傳過程中以下數據也會不停地被重複送來
                if filename is None:
                    filename = data['filename']
                    name = data.get('Name') # or data['Name']
                    price = data.get('Price')
                    size = data.get('Size')
                    tags = data.get('Tags')
                    description = data.get('Description')
                    material = data.get('Material')
                    imageUrl = data.get('ImageUrl')  # 從前端取得圖片連結
                    ModalUrl = "\\Uploads\\" + filename
                    print(f'商品名稱:{name}\n價格:{price}\n大小:{size}\n分類:{tags}\n描述:{description}\n材質:{material}\n圖片URL:{imageUrl}\n模型檔案名稱:{filename}\n')

                    #執行 SQL 新增資料
                    #query = "INSERT INTO furniture (Name, Price, ImagePath, Size, Description, Material, ImageUrl, ModelURL) VALUES (%s, %s, %s, %s, %s, %s, %s, %s, %s);"
                    #values = (name, price, imagePath, size, description, material, imageUrl, ModelURL)
                    #cursor.execute(query, values)
                    #conn.commit()

                # 如果有filename，content_chunks才會持續接收分段內容。
                if filename:
                    content_chunks.append(bytes(data['content']))
                    
                    # 判斷是不是最後一個chunk
                    if len(data['content']) < chunkSize:
                        # 將所有分割的數據結合成完整的數據
                        content = b''.join(content_chunks)
                        # 將檔案存在本地端與檔案同目錄的uploads資料夾之內。
                        # 請確保有uploads資料夾
                        with open(os.path.join('Uploads', filename), 'wb') as f:
                            f.write(content)
                        filename = None
                        content_chunks = []
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
                imageData = data.get('ImageData')

                # 先解碼 Base64 編碼的圖片數據
                imageData_base64 = data.get('ImageData')
                imageData = base64.b64decode(imageData_base64)

                # 請根據需求在此處執行 SQL 更新資料的指令，例如：
                query = f"UPDATE Furniture SET Name='{name}', Number='{number}', Price={price}, ImagePath='{imagePath}', Size='{size}', Description='{description}', Material='{material}', ImageData='{imageData}' WHERE ID = {update_id};"
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

            elif message_type == 'addUser':
                #新增使用者
                username = data['id']
                password = data['password']
                email = data['email']
                department = data['department']

                 # 插入用户数据到 MySQL 数据库
                query = "INSERT INTO Users (Username, Password, Email, Department) VALUES (%s, %s, %s, %s)"
                values = (username, password, email, department)
                cursor.execute(query, values)
                conn.commit()

                response = {'type': 'user_created', 'message': 'User created successfully'}

            elif message_type == 'Login':
                #登入使用者資料
                username = data['id']
                password = data['password']

                # 查询数据库以验证用户名和密码
                query = "SELECT * FROM Users WHERE Username = %s AND Password = %s"
                values = (username, password)
                cursor.execute(query, values)
                user = cursor.fetchone()

                if user:
                    print("Login successful.")
                    response = {'type': 'LoginSuccess','message': data['id']}
                else:
                    print("Login failed. User not found or incorrect password.")
                    response = {'type': 'LoginFall'}


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