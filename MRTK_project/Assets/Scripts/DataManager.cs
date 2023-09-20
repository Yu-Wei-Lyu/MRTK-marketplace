using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace Assets.Scripts
{
    public class DataManager : MonoBehaviour
    {
        public delegate void SetShoppingCartDelegate(ShoppingCart cart);

        private const string WEBSOCKET_QUERY_TYPE = "query";
        private const string WEBSOCKET_QUERY_MESSAGE = "Hello from Hololens!";
        private const string LOADING_DATA_TITLE = "應用程式初始化";
        private const string LOADING_DATA_MESSAGE = "向資料庫請求資料中...";
        private const string LOADING_DATA_SUCCESS_TITLE = "應用程式初始化完成！";
        private const string LOADING_DATA_FAILED_TITLE = "資料載入失敗";
        private const string LOADING_DATA_FAILED_MESSAGE = "請檢查網路連線或聯絡管理員修正錯誤\nexample@gmail.com";
        private const string BACKUP_FILE = "database_query.txt";
        private const string FAKE_DATA_FILE = "fake_query_data.json";

        [SerializeField]
        private bool _isDatabaseOnline;
        [SerializeField]
        private string _webSocketUri;
        [SerializeField]
        private string _websiteRootUrl;
        [SerializeField]
        private string _offlineWebsiteRootUrl;
        [SerializeField]
        private PopupDialog _dialogController;

        private readonly GlbModelManager _glbModelList = new GlbModelManager();
        private readonly ShoppingCart _shoppingCart = new ShoppingCart();
        private List<FurnitureData> _furnitureDataList;

        public int QueryID { get; set; } = -1;

        // Start is called before the first frame update
        public void Start()
        {
            if (_isDatabaseOnline)
            {
                Debug.Log($"[DataManager] Data is in online mode");
                _ = GetFurnitureDataAsync(_webSocketUri);
            }
            else
            {
                Debug.Log($"[DataManager] Data is in offline mode");
                _ = GetFakeData();
            }
        }

        // Write data to file synchronously
        private async Task WriteToFileAsync(string filePath, string content)
        {
            try
            {
                using var writer = new StreamWriter(filePath);
                await writer.WriteAsync(content);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error happened when writing file: {ex.Message}");
            }
        }

        // Get furniture datas from database
        public async Task GetFurnitureDataAsync(string uri)
        {
            var sendData = new { type = WEBSOCKET_QUERY_TYPE, message = WEBSOCKET_QUERY_MESSAGE };
            var jsonStr = JsonConvert.SerializeObject(sendData);
            _dialogController.LoadingDialog(LOADING_DATA_TITLE, LOADING_DATA_MESSAGE);
            var receivedMessage = await WebSocketHandler.SendAndListenAsync(uri, jsonStr);
            if (receivedMessage != null)
            {
                _furnitureDataList = JsonConvert.DeserializeObject<List<FurnitureData>>(receivedMessage);
                _furnitureDataList.ForEach(data =>
                {
                    var modelURL = data.ModelURL;
                    modelURL = _websiteRootUrl + modelURL.Replace("\\", "/");
                    data.ModelURL = modelURL;
                });
                Debug.Log($"[DataManager] Received:\n{receivedMessage}");
                await WriteToFileAsync(Path.Combine(Application.streamingAssetsPath, BACKUP_FILE), receivedMessage);
                await _dialogController.DelayCloseDialog(LOADING_DATA_SUCCESS_TITLE);
            }
            else
            {
                _dialogController.ConfirmDialog(LOADING_DATA_FAILED_TITLE, LOADING_DATA_FAILED_MESSAGE);
                Debug.LogError($"[DataManager] Error happened when receiving data from websocket");
            }
        }

        // Only for database and website is offline
        public async Task GetFakeData()
        {
            _dialogController.LoadingDialog(LOADING_DATA_TITLE, LOADING_DATA_MESSAGE);
            var filePath = Path.Combine(Application.streamingAssetsPath, FAKE_DATA_FILE);
            if (File.Exists(filePath))
            {
                var socketContent = File.ReadAllText(filePath);
                _furnitureDataList = JsonConvert.DeserializeObject<List<FurnitureData>>(socketContent);
                _furnitureDataList.ForEach(data =>
                {
                    var modelURL = data.ModelURL;
                    modelURL = _offlineWebsiteRootUrl + modelURL.Replace("\\", "/");
                    data.ModelURL = modelURL;
                });
                Debug.Log("[DataManager] Read file successfully\n" + socketContent);
                await _dialogController.DelayCloseDialog(LOADING_DATA_SUCCESS_TITLE);
            }
            else
            {
                Debug.LogError("File not found");
                _dialogController.ConfirmDialog(LOADING_DATA_FAILED_TITLE, LOADING_DATA_FAILED_MESSAGE);
            }
        }

        // Determine whether the index is out of range
        private FurnitureData IsExistingFurnitureID(int id)
        {
            return _furnitureDataList.Find(data => data.ID == id);
        }

        // Get furniture counts
        public int GetFurnitureCount()
        {
            return _furnitureDataList.Count;
        }

        // Get furniture data by index
        public FurnitureData GetFurnitureDataByIndex(int index)
        {
            if (index < 0 || index >= _furnitureDataList.Count)
            {
                Debug.LogError($"[DataManager] No furniture data has index {index}");
                return null;
            }
            var furnitureData = _furnitureDataList[index];
            QueryID = furnitureData.ID;
            return furnitureData;
        }

        // Get furniture data by index
        public FurnitureData GetFurnitureDataById(int id)
        {
            var furnitureData = IsExistingFurnitureID(id);
            if (furnitureData == null)
            {
                Debug.LogError($"[DataManager] No furniture data has id {id}");
                return null;
            }
            QueryID = furnitureData.ID;
            return furnitureData;
        }

        // Get data from the most recently queried index value
        public FurnitureData GetCacheFurnitureData()
        {
            return GetFurnitureDataById(QueryID);
        }

        // Reset recently queried index
        public void ResetRecentlyQueriedIndex()
        {
            QueryID = -1;
        }

        // Get shopping cart
        public ShoppingCart GetShoppingCart()
        {
            return _shoppingCart;
        }

        // Get model list
        public GlbModelManager GetModelManager()
        {
            return _glbModelList;
        }

        // Get dialog controller
        public PopupDialog GetDialogController()
        {
            return _dialogController;
        }

        // for test
        public void LoadModelByUri(string uri)
        {
            var glbLoader = new GlbLoader();
            glbLoader.SetPopupDialog(_dialogController);
            glbLoader.SetModelManager(GetModelManager());
            glbLoader.SetFurnitureID(-1);
            _ = glbLoader.LoadModelUri(uri);
        }
    }
}