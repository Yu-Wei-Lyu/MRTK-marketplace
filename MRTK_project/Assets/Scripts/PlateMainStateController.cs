using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts
{
    public class PlateMainStateController : Plate
    {
        [SerializeField]
        private DataManager _dataManager;
        [SerializeField]
        private GameObject _furnitureSampleBlock;
        [SerializeField]
        private Transform _furnitureArea;

        // Start is called before the first frame update
        public override void Start()
        {
            UpdateFurnitures();
        }

        // Destroy all the list Entry
        private void DestroyAllListEntry()
        {
            foreach (Transform childTransform in _furnitureArea)
                Destroy(childTransform.gameObject);
        }

        // Copy object with children
        private GameObject CopyObjectWithChildren()
        {
            var copiedObject = Instantiate(_furnitureSampleBlock, _furnitureArea);
            return copiedObject;
        }

        // Update furniture list and display on plate
        private void UpdateFurnitures()
        {
            DestroyAllListEntry();
            _furnitureSampleBlock.SetActive(true);
            for (var index = 0; index < _dataManager.GetFurnitureCount(); ++index)
            {
                var copiedObject = CopyObjectWithChildren();
                var furnitureEntry = copiedObject.GetComponent<FurnitureObjectReference>();
                var pressableButton = copiedObject.GetComponent<PressableButtonHoloLens2>();
                var furnitureData = _dataManager.GetFurnitureDataByIndex(index);
                copiedObject.SetActive(true);
                furnitureEntry.SetName(furnitureData.Name);
                furnitureEntry.SetImage(furnitureData.GetImageSprite());
                pressableButton.ButtonPressed.AddListener(() => OnButtonPressed(furnitureData.ID));
            }
            _furnitureSampleBlock.SetActive(false);
            _dataManager.ResetRecentlyQueriedIndex();
        }

        // On button pressed, tell DataManager the ID of furniture
        private void OnButtonPressed(int referenceID)
        {
            _dataManager.SetQueryIndex(referenceID);
        }
    }
}