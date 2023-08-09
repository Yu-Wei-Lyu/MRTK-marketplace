using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlateProductStateInitializer : Plate
{
    [SerializeField]
    private DatabaseManager databaseManager;
    [SerializeField]
    private TextMeshProUGUI productNameTextMesh;
    [SerializeField]
    private Image productImage;
    [SerializeField]
    private GameObject productDetailObject;
    [SerializeField]
    private GameObject expandDetailButton;
    [SerializeField]
    private float productDetailHeightInit;
    [SerializeField]
    private RectTransform rebuilderUtilityParentTarget;
    [SerializeField]
    private PopupDialogController associatedPopupDialog;

    private TextMeshProUGUI productDetailTextMesh;
    private ContentSizeFitter productDetailContentSize;
    private RectTransform productDetailRectTransform;
    private AudioSource audioSource;
    private bool isLayoutChanged = false;
    private const string productDetailStringFormat = "價格：\tNT$ {0}\n尺寸：\t{1}\n材料：\t{2}\n供應商：\t{3}\n描述：\t{4}";

    // Set product data to cache
    private void SetProductDataToCache()
    {
        DataObject cacheDataObject = this.databaseManager.CacheDataObject;
        if (cacheDataObject != null)
        {
            this.productNameTextMesh.text = cacheDataObject.Name;
            this.productDetailTextMesh.text = string.Format(productDetailStringFormat,
                cacheDataObject.Price,
                cacheDataObject.Size,
                cacheDataObject.Material,
                cacheDataObject.Manufacturer,
                cacheDataObject.Description
            );
            this.productImage.sprite = cacheDataObject.Image;
        }
    }

    // Default layout setting
    private void DefaultLayoutSetting()
    {
        this.productDetailTextMesh.overflowMode = TextOverflowModes.Ellipsis;
        this.productDetailContentSize.verticalFit = ContentSizeFitter.FitMode.Unconstrained;
        this.productDetailRectTransform.sizeDelta = new Vector2(productDetailRectTransform.sizeDelta.x, productDetailHeightInit);
    }

    // Rebuild layouts request handler
    private void RebuildLayouts()
    {
        LayoutRebuilderUtility.RebuildLayoutsWithContentSizeFitter(rebuilderUtilityParentTarget);
    }

    // Wait for sound played
    private IEnumerator WaitForSoundPlayed()
    {
        if (this.audioSource.isPlaying)
        {
            yield return new WaitWhile(() => this.audioSource.isPlaying);
            this.audioSource = null;
        }
    }

    // SetAudioSourcePlaying
    public void SetAudioSourcePlaying(AudioSource source)
    {
        this.audioSource = source;
    }

    // 
    public void AddShoppingListPopupDialog()
    {
        if (this.audioSource != null)
        {
            StartCoroutine(WaitForSoundPlayed());
        }
        DataObject cacheDataObject = this.databaseManager.CacheDataObject;
        if (cacheDataObject != null)
        {
            associatedPopupDialog.SetTitleMessage("商品已新增至購物清單", $"商品：\n{cacheDataObject.Name}");
            associatedPopupDialog.SetActive(true);
        }
    }

    // Awake is called when the script instance is being loaded.
    public override void Awake()
    {
        base.Awake(); // Call the base class's Awake() method
        this.productDetailTextMesh = productDetailObject.GetComponent<TextMeshProUGUI>();
        this.productDetailContentSize = productDetailObject.GetComponent<ContentSizeFitter>();
        this.productDetailRectTransform = productDetailObject.GetComponent<RectTransform>();
        this.DefaultLayoutSetting();
        isLayoutChanged = true;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        expandDetailButton.SetActive(productDetailTextMesh.isTextOverflowing);
        if (this.isLayoutChanged)
        {
            this.RebuildLayouts();
            this.isLayoutChanged = false;
        }
    }

    // Contains the plate's elements which need to be initialized
    public override void Initialize()
    {
        this.DefaultLayoutSetting();
        this.SetProductDataToCache();
        this.isLayoutChanged = true;
    }

    // Is layout changed property
    public bool IsLayoutChanged
    {
        set 
        { 
            this.isLayoutChanged = value;
        }
    }
}
