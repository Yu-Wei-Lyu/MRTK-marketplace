using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit.UI;
using TMPro;
using UnityEngine;

namespace Assets.Scripts
{
    public class ModelOperatorDialog : DialogBase
    {
        private const string DELETE_SUCCESS_TITLE = "已移除目標家具";
        [SerializeField]
        private DataManager _dataManager;
        [SerializeField]
        private Plate _callbackPlate;
        [SerializeField]
        private PinchSlider _pinchSlider;
        [SerializeField]
        private float _valueMultiplier = 1;
        [SerializeField]
        private TMP_Text _sliderValueObject;
        [SerializeField]
        private TMP_Text _furnitureNameObject;

        private GlbModelManager _modelManager;
        private GameObject _referenceObject = null;
        private bool _isInitFinished = false;
        private int _previousStateID = -1;

        // Awake is called when the script instance is being loaded.
        public override void Awake()
        {
            base.Awake();
            _modelManager = _dataManager.GetModelManager();
            gameObject.SetActive(false);
        }

        // Set the activation state of this gameObject
        public override void SetActive(bool value)
        {
            base.SetActive(value);
            if (value)
            {
                GlbModel model = _modelManager.GetCacheModel();
                FurnitureData furnitureData = _dataManager.GetFurnitureDataById(model.FurnitureID);
                _furnitureNameObject.text = furnitureData.Name;
                _referenceObject = model.ModelObject;
                Vector3 currentEulerAngles = _referenceObject.transform.eulerAngles;
                _pinchSlider.SliderValue = currentEulerAngles.y / _valueMultiplier;
                _previousStateID = _dataManager.QueryID;
            }
            else
            {
                _dataManager.QueryID = _previousStateID;
            }
            _isInitFinished = value;
            gameObject.SetActive(value);
        }

        // Update is called once per frame
        public void Update()
        {
            if (_isInitFinished)
            {
                Vector3 currentEulerAngles = _referenceObject.transform.eulerAngles;
                if (_pinchSlider.SliderValue != 1)
                {
                    _pinchSlider.SliderValue = currentEulerAngles.y / _valueMultiplier;
                }
            }
            Debug.Log("Model Operator Updating.");
        }

        // Slider updated event handler
        public void OnSliderUpdated(SliderEventData eventData)
        {
            if (!_isInitFinished)
            {
                return;
            }
            float newAngle = eventData.NewValue * _valueMultiplier;
            _sliderValueObject.text = $"{newAngle:F0}";
            Transform transform = _referenceObject.transform;
            Vector3 currentEulerAngles = transform.eulerAngles;
            currentEulerAngles.y = newAngle;
            _referenceObject.transform.rotation = Quaternion.Euler(currentEulerAngles);
        }

        // Remove object from scene
        public void RemoveObjectFromScene()
        {
            Destroy(_referenceObject);
            int removeIndex = _modelManager.CacheIndex;
            PopupDialog popupDialog = _dataManager.GetDialogController();
            _modelManager.Remove(removeIndex);
            _ = popupDialog.DelayCloseDialog(DELETE_SUCCESS_TITLE);
            CloseDialog();
        }

        // Close dialog
        public void CloseDialog()
        {
            SetActive(false);
            _callbackPlate.SetActive(true);
        }
    }
}