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
        private PopupDialog _dialogController;
        [SerializeField]
        private ButtonIconController _plateToggleButton;
        [SerializeField]
        private GameObject _sampleFurnitureEntry;
        [SerializeField]
        private Transform _entryArea;
        [SerializeField]
        private TMP_Text _quantityDisplay;

        private GlbModelManager _modelManager;

        // Awake is called when the script instance is being loaded.
        public override void Awake()
        {
            base.Awake();
            _modelManager = _dataManager.GetModelManager();
            _sampleFurnitureEntry.SetActive(false);
        }

        // Set the plate activation state
        public override void SetActive(bool value)
        {
            base.SetActive(value);
            _plateToggleButton.ForceToggle(value);
        }

        // Contains the plate's elements which need to be initialized
        public override void Initialize()
        {
            DestroyAllListEntry();
            var modelQuantity = _modelManager.Count;
            _quantityDisplay.text = string.Format(QUANTITY_FORMAT, modelQuantity);
            _sampleFurnitureEntry.SetActive(true);
            for (var index = 0; index < modelQuantity; ++index)
            {
                var modelData = _modelManager.GetModelAt(index);
                var furnitureData = _dataManager.GetFurnitureDataById(modelData.FurnitureID);
                ConfigureFurnitureZone(furnitureData, index);
            }
            _sampleFurnitureEntry.SetActive(false);
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
            var copiedObject = CopyObjectWithChildren();
            var furnitureEntry = copiedObject.GetComponent<FurnitureObjectReference>();
            var entryButton = furnitureEntry.Button;
            furnitureEntry.SetName(furnitureData.Name);
            furnitureEntry.SetImage(furnitureData.GetImageSprite());
            entryButton.ButtonPressed.AddListener(() => OnButtonPressed(modelIndex));
            copiedObject.SetActive(true);
        }

        // On button pressed, tell DataManager the ID of furniture
        private void OnButtonPressed(int referenceID)
        {
            _dataManager.QueryID = referenceID;
        }

        // Copy object with children
        public GameObject CopyObjectWithChildren()
        {
            var copiedObject = Instantiate(_sampleFurnitureEntry, _entryArea);
            return copiedObject;
        }
    }
}