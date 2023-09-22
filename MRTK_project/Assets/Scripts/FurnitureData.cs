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

        private Sprite _imageSprite;
        private string _imageURL;

        public string ImageURL
        {
            set
            {
                _imageURL = value;
            }
            get => _imageURL;
        }

        public async Task SetImageSpriteAsync()
        {
            if (_imageURL == null)
            {
                return;
            }
            var imageDownloader = new ImageDownloader();
            _imageSprite = await imageDownloader.RequestImageByUriAsync(_imageURL);
        } 

        // Get image sprite
        public Sprite GetImageSprite()
        {
            return _imageSprite;
        }
    }
}