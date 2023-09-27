using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts
{
    public class ImageDownloader
    {
        private const int DEFAULT_IMAGE_WIDTH = 20;
        private const int DEFAULT_IMAGE_HEIGHT = 20;
        private const string DEFAULT_IMAGE_FILE_NAME = "FurnitureDefault.jpg";
        private Sprite _defaultImage;

        public ImageDownloader()
        {
            var defaultImageFilePath = Path.Combine(Application.streamingAssetsPath, DEFAULT_IMAGE_FILE_NAME);
            SetDefaultImage(defaultImageFilePath);
        }

        // Set default image by reading image file
        private void SetDefaultImage(string defaultImagePath)
        {
            var fileData = File.ReadAllBytes(defaultImagePath);
            var defaultTexture = new Texture2D(DEFAULT_IMAGE_WIDTH, DEFAULT_IMAGE_HEIGHT);
            defaultTexture.LoadImage(fileData);
            _defaultImage = Sprite.Create(defaultTexture, new Rect(0, 0, defaultTexture.width, defaultTexture.height), Vector2.zero);
        }

        // Request image by uri
        public async Task<Sprite> RequestImageByUriAsync(string imageUrl)
        {
            var webRequest = UnityWebRequestTexture.GetTexture(imageUrl);
            var operation = webRequest.SendWebRequest();

            while (!operation.isDone)
            {
                await Task.Delay(100); // 等待100毫秒，然后继续检查是否完成
            }

            if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogWarning($"Error happened when receiving an image request, returned default image\nError message:{webRequest.error}");
                return _defaultImage;
            }
            else
            {
                var img = ((DownloadHandlerTexture)webRequest.downloadHandler).texture;
                var sprite = Sprite.Create(img, new Rect(0, 0, img.width, img.height), Vector2.zero);
                return sprite;
            }
        }
    }
}