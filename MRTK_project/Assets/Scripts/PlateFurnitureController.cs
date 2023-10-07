using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class PlateFurnitureController : Plate
    {
        private const string DETAILFORMAT = "價格：\tNT$ {0}\n尺寸：\t{1}cm\n材料：\t{2}\n供應商：\t{3}\n描述：\t{4}";
        private const string ADD_REQUEST_TITLE = "是否要將下述商品加入購物清單？";
        private const string ADD_SUCCESS_TITLE = "成功加入購物清單";
        private const string NO_ACTION_CONFIRM_TITLE = "沒有任何商品被加入";
        private const string FURNITURE_NAME_MESSAGE = "商品：\n\t{0}";
        private const string DIALOG_QUANTITY_HINT = "加入數量";

        [SerializeField]
        private DataManager _dataManager;
        [SerializeField]
        private PopupDialog _dialogController;
        [SerializeField]
        private TextMeshProUGUI _furnitureName;
        [SerializeField]
        private Image _furnitureImage;
        [SerializeField]
        private TextDisplayController _furnitureDetailManager;
        [SerializeField]
        private GameObject _expandDetailButton;
        [SerializeField]
        private RectTransform _rebuilderUtilityParentTarget;

        private ShoppingCart _shoppingCart;
        private bool _isLayoutChanged = false;
        private string _furnitureModelUri;
        private int _cacheFurnitureID = -1;

        // Awake is called when the script instance is being loaded.
        public void Awake()
        {
            _shoppingCart = _dataManager.GetShoppingCart();
        }

        // Update is called once per frame
        public void Update()
        {
            _expandDetailButton.SetActive(_furnitureDetailManager.IsTextOverflowing());
            if (_isLayoutChanged)
            {
                RebuildLayouts();
                _isLayoutChanged = false;
            }
        }

        // Contains the plate's elements which need to be initialized
        public override void Initialize()
        {
            _furnitureDetailManager.DefaultPerformance();
            UpdateFurnitureDisplay();
            _isLayoutChanged = true;
        }

        // Get shopping cart reference
        public void GetShoppingCart(ShoppingCart linkedShoppingCart)
        {
            _shoppingCart = linkedShoppingCart;
        }

        // Set product data to cache
        private void UpdateFurnitureDisplay()
        {
            FurnitureData cacheFurnitureData = _dataManager.GetCacheFurnitureData();
            _cacheFurnitureID = _dataManager.QueryID;
            if (cacheFurnitureData != null)
            {
                _furnitureName.text = cacheFurnitureData.Name;
                _furnitureDetailManager.SetText(string.Format(DETAILFORMAT,
                    cacheFurnitureData.Price,
                    cacheFurnitureData.Size,
                    cacheFurnitureData.Material,
                    cacheFurnitureData.Manufacturer,
                    cacheFurnitureData.Description
                ));
                _furnitureImage.sprite = cacheFurnitureData.GetImageSprite();
                _furnitureModelUri = cacheFurnitureData.ModelURL;
            }
        }

        // Rebuild layouts request handler
        private void RebuildLayouts()
        {
            LayoutRebuilderUtility.RebuildLayoutsWithContentSizeFitter(_rebuilderUtilityParentTarget);
        }

        // Trigger request of adding furniture to shopping list dialog
        public void AddingToListDialog()
        {
            FurnitureData cacheDataObject = _dataManager.GetFurnitureDataById(_cacheFurnitureID);
            SceneViewer sceneViewer = _dataManager.GetSceneViewer();
            GameObject mainSlate = sceneViewer.GetMainSlate();
            _cacheFurnitureID = cacheDataObject.ID;
            _dialogController.AddToBeDeactived(mainSlate);
            _dialogController.SetTexts(ADD_REQUEST_TITLE, string.Format(FURNITURE_NAME_MESSAGE, cacheDataObject.Name));
            _dialogController.SetKeepOpen();
            _dialogController.ResponseQuantityDialog(HandleAddRequest, DIALOG_QUANTITY_HINT);
        }

        // Handling the request for adding furniture to shopping list
        private void HandleAddRequest(PopupDialog.Response response, int addingQuantity)
        {
            if (response == PopupDialog.Response.Confirm)
            {
                string reactText;
                if (addingQuantity == 0)
                {
                    reactText = NO_ACTION_CONFIRM_TITLE;
                }
                else
                {
                    reactText = ADD_SUCCESS_TITLE;
                    _shoppingCart.AddFurnitures(_cacheFurnitureID, addingQuantity);
                }
                _cacheFurnitureID = -1;
                _ = _dialogController.DelayCloseDialog(reactText);
            }
        }

        // Trigger layout update, activated only when clicking the "Show More Details" button
        public void TriggerLayoutUpdate()
        {
            _isLayoutChanged = true;
        }

        // Call model to scene, activated only when clicking the "Display model" button
        public void CallModelToScene()
        {
            GlbLoader glbLoader = new GlbLoader();
            glbLoader.SetPopupDialog(_dialogController);
            glbLoader.SetModelManager(_dataManager.GetModelManager());
            glbLoader.SetFurnitureID(_cacheFurnitureID);
            _dialogController.AddToBeDeactived(gameObject.transform.parent.gameObject);
            _ = glbLoader.LoadModelUri(_furnitureModelUri);
        }
    }
}