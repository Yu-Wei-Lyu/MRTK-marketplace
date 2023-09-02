using Microsoft.MixedReality.Toolkit.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class FurnitureObjectReference : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _nameDisplayObject;
        [SerializeField]
        private TMP_Text _quantityDisplayObject;
        [SerializeField]
        private TMP_Text _unitPriceDisplayObject;
        [SerializeField]
        private TMP_Text _priceDisplayObject;
        [SerializeField]
        private Image _imageDisplayObject;
        [SerializeField]
        private PressableButtonHoloLens2 _button;

        public int ID {
            get; set;
        }
        public PressableButtonHoloLens2 Button => _button;

        // Set the name of furniture
        public void SetName(string furnitureName)
        {
            if (_nameDisplayObject != null)
                _nameDisplayObject.text = furnitureName;
        }

        // Set the quantity of furniture
        public void SetQuantity(string furnitureQuantity)
        {
            if (_quantityDisplayObject != null)
                _quantityDisplayObject.text = furnitureQuantity;
        }

        // Set unit price
        public void SetUnitPrice(string furnitureUnitPrice)
        {
            if (_unitPriceDisplayObject != null)
                _unitPriceDisplayObject.text = furnitureUnitPrice;
        }


        // Set price
        public void SetPrice(string furniturePrice)
        {
            if (_priceDisplayObject != null)
                _priceDisplayObject.text = furniturePrice;
        }

        // Set image sprite
        public void SetImage(Sprite furnitureSprite)
        {
            if (_imageDisplayObject != null)
                _imageDisplayObject.sprite = furnitureSprite;
        }
    }
}