using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class PlateShoppingListStateController : Plate
{
    [SerializeField]
    private DatabaseManager databaseManager;
    [SerializeField]
    private ButtonIconController shoppingListToggleButton;
    [SerializeField]
    private GameObject sampleProductEntry;
    [SerializeField]
    private TextMeshProUGUI totalPriceText;
    [SerializeField]
    private Transform productEntryParentTransform;

    // Destroy all the list Entry
    private void DestroyAllListEntry()
    {
        foreach (Transform childTransform in productEntryParentTransform)
        {
            Destroy(childTransform.gameObject);
        }
    }

    // Set the plate activation state
    public override void SetActive(bool value)
    {
        base.SetActive(value);
        shoppingListToggleButton.ForceSetToggle(value);
        if (value)
        {
            Initialize();
        }
    }

    // Contains the plate's elements which need to be initialized
    public override void Initialize()
    {
        DataObject dataObject;
        float totalPrice = 0;
        this.DestroyAllListEntry();
        sampleProductEntry.SetActive(true);
        for (int index = 0; index < this.databaseManager.GetShoppingListCount(); ++index)
        {
            dataObject = databaseManager.GetShoppingItem(index);
            GameObject gameObject1 = this.CopyObjectWithChildren();
            GameObject copiedObject = gameObject1;
            copiedObject.SetActive(true);
            ShoppingListProductEntry shoppingListProductEntry = copiedObject.GetComponent<ShoppingListProductEntry>();
            shoppingListProductEntry.Name = dataObject.Name;
            shoppingListProductEntry.Price = $"NT$ {dataObject.Price}";
            string originalPriceString = dataObject.Price;
            totalPrice += float.Parse(new string(originalPriceString.Where(c => char.IsDigit(c) || c == '.').ToArray()));
        }
        totalPriceText.text = $"待付款金额 NT$ {totalPrice.ToString("N2")}";
        sampleProductEntry.SetActive(false);
    }

    // c
    public void ClearAllListDataObject()
    {
        for (int index = this.databaseManager.GetShoppingListCount() - 1; index >= 0; --index)
        {
            this.databaseManager.RemoveItemFromShoppingList(index);
        }
        this.Initialize();
    }

    // Copy object with children
    public GameObject CopyObjectWithChildren()
    {
        GameObject copiedObject = Instantiate(sampleProductEntry, productEntryParentTransform);
        return copiedObject;
    }
}
