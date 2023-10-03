using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit.UI;
using TMPro;
using UnityEngine;

namespace Assets.Scripts
{
    public class PlateShoppingListController : Plate
    {
        [SerializeField]
        private DataManager _dataManager;
        [SerializeField]
        private ButtonIconController _plateToggleButton;
        [SerializeField]
        private ShoppingScrollableList _shoppingScrollableList;
        [SerializeField]
        private TMP_Text _totalPriceText;

        private int _previousStateID = -1;

        // Set the plate activation state
        public override void SetActive(bool value)
        {
            if (value)
            {
                _previousStateID = _dataManager.QueryID;
            }
            else
            {
                _dataManager.QueryID = _previousStateID;
            }
            base.SetActive(value);
            _plateToggleButton.ForceToggle(value);
        }

        // Contains the plate's elements which need to be initialized
        public override void Initialize()
        {
            _shoppingScrollableList.Initialize();
            _totalPriceText.text = _shoppingScrollableList.GetTotalPrice();
        }
    }
}