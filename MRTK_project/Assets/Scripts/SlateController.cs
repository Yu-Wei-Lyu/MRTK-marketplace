using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit.Utilities.Solvers;
using TMPro;
using UnityEngine;

namespace Assets.Scripts
{
    public class SlateController : MonoBehaviour
    {
        [SerializeField]
        private DataManager _dataManager;
        [SerializeField]
        private TextMeshPro _titlebarText;
        [SerializeField]
        private ButtonIconController _followButton;
        [SerializeField]
        private ButtonIconController _modelStateButton;
        [SerializeField]
        private GameObject _backButton;
        [SerializeField]
        private List<Plate> _plates;

        private Plate _currentPlate;
        private Plate _primaryPlate;
        private Stack<Plate> _previousPlateStack;
        private RadialView _radialView;
        private const int PRIMARY_PLATE_INDEX = 0;
        private const int SECONDARY_PLATE_BEGIN_INDEX = 1;

        // Awake is called when the script instance is being loaded.
        public void Awake()
        {
            _previousPlateStack = new Stack<Plate>();
            _radialView = GetComponentInChildren<RadialView>();
            _radialView.enabled = true;
        }

        // Start is called before the first frame updates
        public void Start()
        {
            _backButton.SetActive(false);
            InitializePlate();
        }

        // Initialize plate
        private void InitializePlate()
        {
            _primaryPlate = _plates[PRIMARY_PLATE_INDEX];
            _primaryPlate.SetActive(true);
            _titlebarText.text = _primaryPlate.Title;
            _currentPlate = _primaryPlate;
            _followButton.ForceToggle(true);
            for (int index = SECONDARY_PLATE_BEGIN_INDEX; index < _plates.Count; ++index)
                _plates[index].SetActive(false);
        }

        // Return the Plate object that contains the target plate, not found will be null.
        private Plate GetPlateIfContains(GameObject targetPlate)
        {
            return _plates.Find(plate => plate.IsSameReference(targetPlate));
        }

        // Activate target plate and deactivate other plates
        public void SwitchToPlate(GameObject targetPlate)
        {
            gameObject.SetActive(true);
            if (_currentPlate.IsSameReference(targetPlate))
            {
                return;
            }
            _currentPlate.SetActive(false);
            if (_currentPlate.Recordable)
            {
                _previousPlateStack.Push(_currentPlate);
            }
            _currentPlate = GetPlateIfContains(targetPlate);
            _currentPlate.SetActive(true);
            _titlebarText.text = _currentPlate.Title;
            if (_currentPlate == _primaryPlate)
            {
                _previousPlateStack.Clear();
                _backButton.SetActive(false);
            }
            else
            {
                _backButton.SetActive(true);
            }
        }

        // Activate Previous plate and deactivate current plates
        public void SwitchToPreviousPlate()
        {
            _currentPlate.SetActive(false);
            _currentPlate = _previousPlateStack.Pop();
            _currentPlate.SetActive(true);
            _titlebarText.text = _currentPlate.Title;
            if (_currentPlate == _primaryPlate)
            {
                _backButton.SetActive(false);
                _dataManager.ResetRecentlyQueriedIndex();
            }
            else
                _backButton.SetActive(true);
        }

        // Do something before setting activated
        public void SetActive(bool value)
        {
            gameObject.SetActive(value);
            _radialView.enabled = value;
            _followButton.ForceToggle(value);
        }

        // Toggle the radial view state
        public void ToggleRadialViewState()
        {
            _radialView.enabled = !_radialView.enabled;
        }

        // Activate back button
        public void SetTitleBarRegularMode()
        {
            _backButton.SetActive(true);
            _modelStateButton.gameObject.SetActive(true);
        }

        // Order information title bar mode
        public void SetTitleBarOrderMode()
        {
            _backButton.SetActive(false);
            _modelStateButton.gameObject.SetActive(false);
        }
    }
}