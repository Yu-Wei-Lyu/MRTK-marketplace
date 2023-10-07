using Microsoft.MixedReality.Toolkit.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts
{
    public class PlateModelStatusController : Plate
    {
        private const string QUANTITY_FORMAT = "已載入模型數量 {0}";
        [SerializeField]
        private DataManager _dataManager;
        [SerializeField]
        private ButtonIconController _plateToggleButton;
        [SerializeField]
        private ModelOperatorDialog _modelDialog;
        [SerializeField]
        private GameObject _sampleFurnitureEntry;
        [SerializeField]
        private Transform _entryArea;
        [SerializeField]
        private TMP_Text _quantityDisplay;

        private GlbModelManager _modelManager;
        private int _previousStateID = -1;

        // Awake is called when the script instance is being loaded.
        public void Awake()
        {
            _modelManager = _dataManager.GetModelManager();
            _sampleFurnitureEntry.SetActive(false);
        }

        // Set the plate activation state
        public override void SetActive(bool value)
        {
            if (value)
            {
                StorePreivousID();
            }
            else
            {
                RestorePreviousID();
            }
            base.SetActive(value);
            _plateToggleButton.ForceToggle(value);
        }

        // Contains the plate's elements which need to be initialized
        public override void Initialize()
        {
            DestroyAllListEntry();
            int modelQuantity = _modelManager.Count;
            _quantityDisplay.text = string.Format(QUANTITY_FORMAT, modelQuantity);
            _sampleFurnitureEntry.SetActive(true);
            StorePreivousID();
            for (int index = 0; index < modelQuantity; ++index)
            {
                GlbModel modelData = _modelManager.GetModelAt(index);
                FurnitureData furnitureData = _dataManager.GetFurnitureDataById(modelData.FurnitureID);
                ConfigureFurnitureZone(furnitureData, index);
            }
            RestorePreviousID();
            _sampleFurnitureEntry.SetActive(false);
        }

        // Store previous state queried ID
        private void StorePreivousID()
        {
            _previousStateID = _dataManager.QueryID;
        }

        // Restore previous state queried ID
        private void RestorePreviousID()
        {
            _dataManager.QueryID = _previousStateID;
        }

        // Destroy all the list Entry
        private void DestroyAllListEntry()
        {
            foreach (Transform childTransform in _entryArea)
                Destroy(childTransform.gameObject);
        }

        // Configure shopping item zone
        private void ConfigureFurnitureZone(FurnitureData furnitureData, int modelIndex)
        {
            GameObject copiedObject = CopyObjectWithChildren();
            FurnitureObjectReference furnitureEntry = copiedObject.GetComponent<FurnitureObjectReference>();
            PressableButtonHoloLens2 entryButton = furnitureEntry.Button;
            furnitureEntry.SetName(furnitureData.Name);
            furnitureEntry.SetImage(furnitureData.GetImageSprite());
            entryButton.ButtonPressed.AddListener(() => OnButtonPressed(modelIndex));
            copiedObject.SetActive(true);
        }

        // On button pressed, tell DataManager the ID of furniture
        private void OnButtonPressed(int modelIndex)
        {
            Transform parent = gameObject.transform.parent;
            _modelManager.CacheIndex = modelIndex;
            _modelDialog.AddToBeDeactived(parent.gameObject);
            _modelDialog.SetKeepOpen();
            _modelDialog.SetActive(true);
        }

        // Copy object with children
        public GameObject CopyObjectWithChildren()
        {
            GameObject copiedObject = Instantiate(_sampleFurnitureEntry, _entryArea);
            return copiedObject;
        }
    }
}