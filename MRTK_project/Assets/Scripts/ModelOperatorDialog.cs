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
                var model = _modelManager.GetCacheModel();
                var furnitureData = _dataManager.GetFurnitureDataById(model.FurnitureID);
                _furnitureNameObject.text = furnitureData.Name;
                _referenceObject = model.ModelObject;
                var currentEulerAngles = _referenceObject.transform.eulerAngles;
                _pinchSlider.SliderValue = currentEulerAngles.y / _valueMultiplier;
            }
            _isInitFinished = value;
            gameObject.SetActive(value);
        }

        // Update is called once per frame
        public void Update()
        {
            if (_isInitFinished)
            {
                var currentEulerAngles = _referenceObject.transform.eulerAngles;
                if (_pinchSlider.SliderValue != 1)
                {
                    _pinchSlider.SliderValue = currentEulerAngles.y / _valueMultiplier;
                }
            }
        }

        // Slider updated event handler
        public void OnSliderUpdated(SliderEventData eventData)
        {
            if (!_isInitFinished)
            {
                return;
            }
            var newAngle = eventData.NewValue * _valueMultiplier;
            _sliderValueObject.text = $"{newAngle:F0}";
            var transform = _referenceObject.transform;
            var currentEulerAngles = transform.eulerAngles;
            currentEulerAngles.y = newAngle;
            _referenceObject.transform.rotation = Quaternion.Euler(currentEulerAngles);
        }

        // Remove object from scene
        public void RemoveObjectFromScene()
        {
            Destroy(_referenceObject);
            var removeIndex = _modelManager.CacheIndex;
            var popupDialog = _dataManager.GetDialogController();
            _modelManager.Remove(removeIndex);
            _callbackPlate.Initialize();
            _ = popupDialog.DelayCloseDialog(DELETE_SUCCESS_TITLE);
            SetActive(false);
        }

        // Close dialog
        public void CloseDialog()
        {
            SetActive(false);
        }
    }
}