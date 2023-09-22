using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    [System.Serializable]
    public class FurnitureData
    {
        public int ID
        {
            get; set;
        }
        public string Name
        {
            get; set;
        }
        public double Price
        {
            get; set;
        }
        public string Size
        {
            get; set;
        }
        public string Tags
        {
            get; set;
        }
        public string Description
        {
            get; set;
        }
        public string Material
        {
            get; set;
        }
        public string Manufacturer
        {
            get; set;
        }
        public string ModelURL
        {
            get; set;
        }

        private Sprite _imageSprite = null;

        public string ImageURL
        {
            set;
            get;
        }

        // Set image sprite asynchronous
        public async Task SetImageSpriteAsync()
        {
            if (ImageURL == null || _imageSprite != null)
            {
                return;
            }
            var imageDownloader = new ImageDownloader();
            _imageSprite = await imageDownloader.RequestImageByUriAsync(ImageURL);
        }

        // Return bool of _imageSprite is null
        public bool IsImageDownloaded()
        {
            return _imageSprite != null;
        }

        // Get image sprite
        public Sprite GetImageSprite()
        {
            return _imageSprite;
        }
    }
}