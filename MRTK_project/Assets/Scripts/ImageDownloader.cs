using System;
using System.IO;
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
        public void RequestImageByUri(string x, Action<Sprite> onImageLoaded)
        {
            var webRequest = UnityWebRequestTexture.GetTexture(x);
            webRequest.SendWebRequest().completed += operation =>
            {
                if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                    webRequest.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.Log($"Error happened when receiving an image request, returned default image\nError message:{webRequest.error}");
                    onImageLoaded?.Invoke(_defaultImage);
                }
                else
                {
                    var img = ((DownloadHandlerTexture)webRequest.downloadHandler).texture;
                    var sprite = Sprite.Create(img, new Rect(0, 0, img.width, img.height), Vector2.zero);
                    onImageLoaded?.Invoke(sprite);
                }
            };
        }
    }
}