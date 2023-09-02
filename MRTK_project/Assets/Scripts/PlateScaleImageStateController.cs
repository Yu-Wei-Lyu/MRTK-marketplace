using Assets.Scripts;
using UnityEngine;

public class PlateScaleImageStateController : Plate
{
    [SerializeField]
    private DataManager _databaseManager;

    private FurnitureObjectReference _furnitureEntry;

    // Awake is called when the script instance is being loaded.
    public override void Awake()
    {
        base.Awake();
        _furnitureEntry = this.GetComponent<FurnitureObjectReference>();
    }

    // Contains the plate's elements which need to be initialized
    public override void Initialize()
    {
        FurnitureData furnitureData = _databaseManager.GetCacheFurnitureData();
        _furnitureEntry.SetName(furnitureData.Name);
        _furnitureEntry.SetImage(furnitureData.GetImageSprite());
    }
}
