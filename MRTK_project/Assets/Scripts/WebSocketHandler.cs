using System;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Assets.Scripts
{
    public static class WebSocketHandler
    {
        // Connect to server by uri
        private static async Task<ClientWebSocket> ConnectToServer(string webSocketUri)
        {
            var ws = new ClientWebSocket();
            var serverUri = new Uri(webSocketUri);
            await ws.ConnectAsync(serverUri, CancellationToken.None);
            return ws;
        }

        // Send message to server request data
        private static async Task SendMessageToServer(ClientWebSocket ws, string data)
        {
            var sendBuffer = Encoding.UTF8.GetBytes(data);
            await ws.SendAsync(new ArraySegment<byte>(sendBuffer), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        // Receive message from server
        private static async Task<string> ReceiveMessageFromServer(ClientWebSocket ws)
        {
            var memoryStream = new MemoryStream();
            var receiveBuffer = new byte[1024];
            WebSocketReceiveResult receiveResult;
            do
            {
                receiveResult = await ws.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), CancellationToken.None);
                memoryStream.Write(receiveBuffer, 0, receiveResult.Count);
            } while (!receiveResult.EndOfMessage);
            var resultBuffer = memoryStream.ToArray();
            return Encoding.UTF8.GetString(resultBuffer);
        }

        // Get furniture datas from database
        public static async Task<string> SendAndListenAsync(string uri, string data)
        {
            string receivedMessage = null;
            try
            {
                var jsonStr = JsonConvert.SerializeObject(data);
                using (var ws = await ConnectToServer(uri))
                {
                    await SendMessageToServer(ws, jsonStr);
                    receivedMessage = await ReceiveMessageFromServer(ws);
                }
                Console.WriteLine($"[DataManager] Received:\n{receivedMessage}");
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DataManager] {ex.Message}");
            }
            return receivedMessage;
        }
    }
}