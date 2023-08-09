using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlateScaleImageStateInitializer : Plate
{
    [SerializeField]
    private DatabaseManager databaseManager;
    [SerializeField]
    private TextMeshProUGUI productNameTextMesh;
    [SerializeField]
    private Image productImage;

    // Contains the plate's elements which need to be initialized
    public override void Initialize()
    {
        SetProductDataToCache();
    }

    // Set product data to cache
    private void SetProductDataToCache()
    {
        DataObject cacheDataObject = this.databaseManager.CacheDataObject;
        if (cacheDataObject != null)
        {
            this.productNameTextMesh.text = cacheDataObject.Name;
            this.productImage.sprite = cacheDataObject.Image;
        }
    }
}
