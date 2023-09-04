using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts
{
    public class PlateMainController : Plate
    {
        [SerializeField]
        private DataManager _dataManager;
        [SerializeField]
        private GameObject _sampleFurnitureEntry;
        [SerializeField]
        private Transform _entryArea;

        // Start is called before the first frame update
        public override void Start()
        {
            UpdateFurnitures();
        }

        // Destroy all the list Entry
        private void DestroyAllListEntry()
        {
            foreach (Transform childTransform in _entryArea)
            {
                Destroy(childTransform.gameObject);
            }
        }

        // Copy object with children
        private GameObject CopyObjectWithChildren()
        {
            var copiedObject = Instantiate(_sampleFurnitureEntry, _entryArea);
            return copiedObject;
        }

        // Update furniture list and display on plate
        private void UpdateFurnitures()
        {
            DestroyAllListEntry();
            _sampleFurnitureEntry.SetActive(true);
            for (var index = 0; index < _dataManager.GetFurnitureCount(); ++index)
            {
                var furnitureData = _dataManager.GetFurnitureDataByIndex(index);
                ConfigureFurnitureZone(furnitureData);
            }
            _sampleFurnitureEntry.SetActive(false);
            _dataManager.ResetRecentlyQueriedIndex();
        }

        // Configure furniture item zone
        private void ConfigureFurnitureZone(FurnitureData furnitureData)
        {
            var copiedObject = CopyObjectWithChildren();
            var furnitureEntry = copiedObject.GetComponent<FurnitureObjectReference>();
            var entryButton = furnitureEntry.Button;
            furnitureEntry.SetName(furnitureData.Name);
            furnitureEntry.SetImage(furnitureData.GetImageSprite());
            entryButton.ButtonPressed.AddListener(() => OnButtonPressed(furnitureData.ID));
            copiedObject.SetActive(true);
        }

        // On button pressed, tell DataManager the ID of furniture
        private void OnButtonPressed(int referenceID)
        {
            _dataManager.QueryID = referenceID;
        }
    }
}