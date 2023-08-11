using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class UnityWebSocketExample : MonoBehaviour
{
    void Start()
    {
        this.GetQueryData();
    }

    public async void GetQueryData()
    {
        try
        {
            // 設定傳送的訊息
            var sendData = new { type = "query", message = "Hello from Unity!" };
            // 轉換成 Json 格式
            string jsonStr = JsonConvert.SerializeObject(sendData);

            // 與 server 建立連線並交換資料 結束時關閉相關的連線
            using (ClientWebSocket ws = new ClientWebSocket())
            {
                Uri serverUri = new Uri("ws://118.150.125.153:8765");
                CancellationToken cancellationToken = CancellationToken.None;
                await ws.ConnectAsync(serverUri, cancellationToken);
                byte[] sendBuffer = Encoding.UTF8.GetBytes(jsonStr);
                await ws.SendAsync(new ArraySegment<byte>(sendBuffer), WebSocketMessageType.Text, true, cancellationToken);
                // 動態的獲取資料 直到資料完整被接收
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    byte[] receiveBuffer = new byte[1024];
                    WebSocketReceiveResult receiveResult;
                    do
                    {
                        receiveResult = await ws.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), cancellationToken);
                        memoryStream.Write(receiveBuffer, 0, receiveResult.Count);
                    } while (!receiveResult.EndOfMessage);
                    byte[] resultBuffer = memoryStream.ToArray();
                    string receivedMessage = Encoding.UTF8.GetString(resultBuffer);
                    Debug.Log($"[UnityWebSocketExample] Received:\n{receivedMessage}");
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"[UnityWebSocketExample] {ex.Message}");
        }
    }

    /*
     {"type": "query", "message": "[(1, '\u6905\u5b50', 'C001', Decimal('1000.00'), 'chair.jpg', '60x60x80', '\u8212\u9069\u7684\u5e03\u85dd\u6905\u5b50', '\u5e03\u6599\u3001\u6728\u6750', None, None), (2, '\u6c99\u767c', 'S001', Decimal('5000.00'), 'sofa.jpg', '180x80x70', '\u73fe\u4ee3\u98a8\u683c\u76ae\u9769\u6c99\u767c', '\u76ae\u9769\u3001\u92fc\u67b6', None, None), (3, '\u66f8\u684c', 'D001', Decimal('800.00'), 'desk.jpg', '120x60x75', '\u7c21\u7d04\u98a8\u683c\u66f8\u684c', '\u6728\u6750', None, None), (4, '\u5e8a\u67b6', 'B001', Decimal('3000.00'), 'bed.jpg', '160x200', '\u73fe\u4ee3\u98a8\u683c\u5e8a\u67b6', '\u6728\u6750\u3001\u91d1\u5c6c', None, None), (5, '\u9910\u684c', 'T001', Decimal('1500.00'), 'dining_table.jpg', '120x80x75', '\u5be6\u6728\u9910\u684c', '\u6728\u6750', None, None), (6, '\u8863\u6ac3', 'W001', Decimal('2000.00'), 'wardrobe.jpg', '180x60x200', '\u73fe\u4ee3\u98a8\u683c\u8863\u6ac3', '\u6728\u6750\u3001\u93e1\u5b50', None, None), (7, '\u96fb\u8996\u6ac3', 'TV001', Decimal('1200.00'), 'tv_stand.jpg', '160x40x50', '\u7c21\u7d04\u98a8\u683c\u96fb\u8996\u6ac3', '\u6728\u6750\u3001\u73bb\u7483', None, None), (8, '\u66f8\u67b6', 'B002', Decimal('600.00'), 'bookshelf.jpg', '80x30x180', '\u7d93\u5178\u6728\u8cea\u66f8\u67b6', '\u6728\u6750', None, None), (9, '\u6aaf\u71c8', 'L001', Decimal('200.00'), 'desk_lamp.jpg', '30x30x50', '\u73fe\u4ee3\u98a8\u683c\u6aaf\u71c8', '\u91d1\u5c6c\u3001\u5851\u81a0', None, None), (10, '\u8336\u51e0', 'C002', Decimal('800.00'), 'coffee_table.jpg', '80x80x40', '\u5be6\u6728\u8336\u51e0', '\u6728\u6750', None, None)]"}
     */
}
