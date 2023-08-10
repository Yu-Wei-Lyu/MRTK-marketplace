using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShoppingListProductEntry : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro nameText;
    [SerializeField]
    private TextMeshPro priceText;

    private string productName;
    private string productPrice;

    public string Name
    {
        get
        {
            return this.productName;
        }
        set
        {
            this.productName = value;
            this.nameText.text = value;
        }
    }

    public string Price
    {
        get
        {
            return this.productPrice;
        }
        set
        {
            this.productPrice = value;
            this.priceText.text = value;
        }
    }
}
