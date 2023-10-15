using System.Collections.Generic;
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

        public string ImageURL
        {
            set; get;
        }

        private const string PRICE_FORMAT_TYPE = "N0";
        private Sprite _imageSprite = null;

        // Set image sprite asynchronous
        public async Task DownloadImageAsync()
        {
            if (ImageURL == null)
            {
                return;
            }
            ImageDownloader imageDownloader = new ImageDownloader();
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

        // Merge website IP and model URL
        public void MergePrefixIP(string ip)
        {
            ModelURL = ip + ModelURL.Replace("\\", "/");
        }

        // Set number to string by format "N0"
        public string GetPriceFormat()
        {
            return "$ " + Price.ToString(PRICE_FORMAT_TYPE);
        }

        // Is contain substring
        public bool IsContainString(string substring)
        {
            List<string> compareStrings = new List<string> { Name };
            bool isContain = false;
            for (int compareIndex = 0; compareIndex < compareStrings.Count; ++compareIndex)
            {
                var compareString = compareStrings[compareIndex];
                if (compareString.Contains(substring))
                {
                    isContain = true;
                    break;
                }
            }
            return isContain;
        }
    }
}