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

        private Sprite _imageSprite;
        private string _imageURL;

        public string ImageURL
        {
            set
            {
                var imageDownloader = new ImageDownloader();
                _imageURL = value;
                imageDownloader.RequestImageByUri(value, (loadedSprite) => _imageSprite = loadedSprite);
            }
            get => _imageURL;
        }

        // Get image sprite
        public Sprite GetImageSprite()
        {
            return _imageSprite;
        }
    }
}