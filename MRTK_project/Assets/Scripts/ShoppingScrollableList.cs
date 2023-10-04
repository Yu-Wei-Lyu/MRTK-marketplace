using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine;

namespace Assets.Scripts
{
    public class ShoppingScrollableList : ScrollableListPopulator
    {
        private const string TOTAL_PRICE_PREFIX = "待付款金额 NT$ ";
        private const string DELETE_REQUEST_TITLE = "確定要刪除？";
        private const string DELETE_CONFIRM_TITLE = "刪除成功！";
        private const string NO_ACTION_CONFIRM_TITLE = "沒有任何商品被刪除";
        private const string FURNITURE_NAME_MESSAGE = "商品：\n\t{0}";
        private const string PRICE_FORMAT_TYPE = "N0";

        [SerializeField]
        private DataManager _dataManager;

        private PopupDialog _dialogController;
        private ShoppingCart _shoppingCart;
        private int _cacheFurnitureID = -1;
        private string _totalPrice;

        // Awake is called when the script instance is being loaded.
        public void Awake()
        {
            _shoppingCart = _dataManager.GetShoppingCart();
            _dialogController = _dataManager.GetDialogController();
            DynamicItem.SetActive(false);
        }

        // Initliaze list
        public void Initialize()
        {
            if (ItemCollection != null)
            {
                DestroyScrollView();
            }
            MakeScrollingList();
        }

        // Generate list items
        public override void GenerateListItems()
        {
            FurnitureData furnitureData;
            string totalPriceFormat;
            double totalPrice = 0;
            List<int> shoppingIDList = _shoppingCart.GetIDList();
            shoppingIDList.ForEach(furnitureID =>
            {
                int shoppingItem = _shoppingCart.GetQuantityByID(furnitureID);
                furnitureData = _dataManager.GetFurnitureDataById(furnitureID);
                totalPrice += furnitureData.Price * shoppingItem;
                ConfigureFurnitureZone(furnitureData, shoppingItem);
            });
            totalPriceFormat = totalPrice.ToString(PRICE_FORMAT_TYPE);
            _totalPrice = TOTAL_PRICE_PREFIX + totalPriceFormat;
        }

        // Configure shopping item zone
        private void ConfigureFurnitureZone(FurnitureData furnitureData, int quantity)
        {
            GameObject copiedObject = MakeItem(DynamicItem);
            FurnitureObjectReference furnitureEntry = copiedObject.GetComponent<FurnitureObjectReference>();
            string unitPrice = GetPriceFormat(furnitureData.Price);
            string furniturePriceFormat = GetPriceFormat(furnitureData.Price * quantity);
            PressableButtonHoloLens2 entryButton = furnitureEntry.Button;
            furnitureEntry.SetName(furnitureData.Name);
            furnitureEntry.SetQuantity(quantity.ToString());
            furnitureEntry.SetUnitPrice(unitPrice);
            furnitureEntry.SetPrice(furniturePriceFormat);
            entryButton.ButtonReleased.AddListener(() => OnDeleteButtonReleased(furnitureData.ID));
        }

        // Set number to string by format "N0"
        private string GetPriceFormat(double price)
        {
            return price.ToString(PRICE_FORMAT_TYPE);
        }

        // Handling the Delete Furniture button released event
        public void OnDeleteButtonReleased(int furnitureID)
        {
            FurnitureData furnitureData = _dataManager.GetFurnitureDataById(furnitureID);
            SceneViewer sceneViewer = _dataManager.GetSceneViewer();
            GameObject mainSlate = sceneViewer.GetMainSlate();
            _cacheFurnitureID = furnitureData.ID;
            _dialogController.AddToBeDeactived(mainSlate);
            _dialogController.SetTexts(DELETE_REQUEST_TITLE, string.Format(FURNITURE_NAME_MESSAGE, furnitureData.Name));
            _dialogController.SetKeepOpen();
            _dialogController.WaitingResponseDialog(HandleDeleteRequest, true);
        }

        // Handling the request for the removal of furniture
        private void HandleDeleteRequest(PopupDialog.Response response, int deleteQuantity)
        {
            if (response == PopupDialog.Response.Confirm)
            {
                string reactText;
                if (deleteQuantity == 0)
                {
                    reactText = NO_ACTION_CONFIRM_TITLE;
                }
                else
                {
                    reactText = DELETE_CONFIRM_TITLE;
                    _shoppingCart.DecreaseFurnitureByID(_cacheFurnitureID, deleteQuantity);
                }
                _cacheFurnitureID = -1;
                _ = _dialogController.DelayCloseDialog(reactText);
                Initialize();
            }
        }

        // Destroy all the list Entry
        private void DestroyScrollView()
        {
            ScrollView.gameObject.SetActive(false);
            Destroy(ScrollView.gameObject);
            ScrollView = null;
        }

        // Get total price text
        public string GetTotalPrice()
        {
            return _totalPrice;
        }
    }
}