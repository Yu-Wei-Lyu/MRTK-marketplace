using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit.UI;
using TMPro;
using UnityEngine;

namespace Assets.Scripts
{
    public class PlateShoppingListController : Plate
    {
        private const string DELETE_REQUEST_TITLE = "確定要刪除？";
        private const string DELETE_CONFIRM_TITLE = "刪除成功！";
        private const string FURNITURE_NAME_MESSAGE = "商品：\n\t{0}";
        private const string PRICE_FORMAT_TYPE = "N0";

        [SerializeField]
        private DataManager _dataManager;
        [SerializeField]
        private PopupDialog _dialogController;
        [SerializeField]
        private ButtonIconController _plateToggleButton;
        [SerializeField]
        private GameObject _sampleFurnitureEntry;
        [SerializeField]
        private Transform _entryArea;
        [SerializeField]
        private TMP_Text _totalPriceText;

        private ShoppingCart _shoppingCart;
        private int _cacheFurnitureID = -1;
        private int _previousStateID = -1;

        // Awake is called when the script instance is being loaded.
        public void Awake()
        {
            _shoppingCart = _dataManager.GetShoppingCart();
            _sampleFurnitureEntry.SetActive(false);
        }

        // Set the plate activation state
        public override void SetActive(bool value)
        {
            if (value)
            {
                _previousStateID = _dataManager.QueryID;
            }
            else
            {
                _dataManager.QueryID = _previousStateID;
                _previousStateID = -1;
            }
            base.SetActive(value);
            _plateToggleButton.ForceToggle(value);
        }

        // Contains the plate's elements which need to be initialized
        public override void Initialize()
        {
            FurnitureData furnitureData;
            string totalPriceFormat;
            double totalPrice = 0;
            var shoppingIDList = _shoppingCart.GetIDList();
            DestroyAllListEntry();
            shoppingIDList.ForEach(furnitureID =>
            {
                var shoppingItem = _shoppingCart.GetQuantityByID(furnitureID);
                furnitureData = _dataManager.GetFurnitureDataById(furnitureID);
                totalPrice += furnitureData.Price * shoppingItem;
                ConfigureFurnitureZone(furnitureData, shoppingItem);
            });
            totalPriceFormat = totalPrice.ToString(PRICE_FORMAT_TYPE);
            _totalPriceText.text = $"待付款金额 NT$ {totalPriceFormat}";
        }

        // Destroy all the list Entry
        private void DestroyAllListEntry()
        {
            foreach (Transform childTransform in _entryArea)
                Destroy(childTransform.gameObject);
        }

        // Configure shopping item zone
        private void ConfigureFurnitureZone(FurnitureData furnitureData, int quantity)
        {
            var copiedObject = CopyObjectWithChildren();
            var furnitureEntry = copiedObject.GetComponent<FurnitureObjectReference>();
            var unitPrice = GetPriceFormat(furnitureData.Price);
            var furniturePriceFormat = GetPriceFormat(furnitureData.Price * quantity);
            var entryButton = furnitureEntry.Button;
            furnitureEntry.SetName(furnitureData.Name);
            furnitureEntry.SetQuantity(quantity.ToString());
            furnitureEntry.SetUnitPrice(unitPrice);
            furnitureEntry.SetPrice(furniturePriceFormat);
            entryButton.ButtonReleased.AddListener(() => OnDeleteButtonReleased(furnitureData.ID));
            copiedObject.SetActive(true);
        }

        // Set number to string by format "N0"
        private string GetPriceFormat(double price)
        {
            return price.ToString(PRICE_FORMAT_TYPE);
        }

        // Copy object with children
        public GameObject CopyObjectWithChildren()
        {
            var copiedObject = Instantiate(_sampleFurnitureEntry, _entryArea);
            return copiedObject;
        }

        // Handling the Delete Furniture button released event
        public void OnDeleteButtonReleased(int furnitureID)
        {
            var furnitureData = _dataManager.GetFurnitureDataById(furnitureID);
            _cacheFurnitureID = furnitureData.ID;
            _dialogController.AddToBeDeactived(transform.parent.gameObject);
            _dialogController.SetTexts(DELETE_REQUEST_TITLE, string.Format(FURNITURE_NAME_MESSAGE, furnitureData.Name));
            _dialogController.SetKeepOpen();
            _dialogController.WaitingResponseDialog(HandleDeleteRequest, true);
        }

        // Handling the request for the removal of furniture
        private void HandleDeleteRequest(PopupDialog.Response response, int deleteAmount)
        {
            if (response == PopupDialog.Response.Confirm)
            {
                _shoppingCart.DecreaseFurnitureByID(_cacheFurnitureID, deleteAmount);
                _cacheFurnitureID = -1;
                _ = _dialogController.DelayCloseDialog(DELETE_CONFIRM_TITLE);
                Initialize();
            }
        }
    }
}