using UnityEngine;

namespace Assets.Scripts
{
    public class PlateScaleImageController : Plate
    {
        [SerializeField]
        private DataManager _databaseManager;

        private FurnitureObjectReference _furnitureEntry;

        // Awake is called when the script instance is being loaded.
        public override void Awake()
        {
            base.Awake();
            _furnitureEntry = GetComponent<FurnitureObjectReference>();
        }

        // Contains the plate's elements which need to be initialized
        public override void Initialize()
        {
            var furnitureData = _databaseManager.GetCacheFurnitureData();
            _furnitureEntry.SetName(furnitureData.Name);
            _furnitureEntry.SetImage(furnitureData.GetImageSprite());
        }
    }
}