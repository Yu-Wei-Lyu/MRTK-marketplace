using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.MixedReality.Toolkit.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts
{
    public class PlateMainController : Plate
    {
        [SerializeField]
        private DataManager _dataManager;
        [SerializeField]
        private GameObject _sampleFurnitureEntry;
        [SerializeField]
        private GameObject _furnitureArea;
        [SerializeField]
        private TMP_Text _pageLabel;
        [SerializeField]
        private GameObject _previousPageButton;
        [SerializeField]
        private GameObject _nextPageButton;
        [SerializeField]
        private GameObject _loadingIcon;

        private const int itemPerPage = 12;
        private int _currentPage = 0;
        private int _currentLoaded = 0;
        private int _loadCompletedAmount = 0;

        // Update furniture list and display on plate
        public override void Initialize()
        {
            _currentPage = 0;
            UpdateListByPage(_currentPage);
        }

        // Destroy all item in the list 
        private void DestroyAllListItem()
        {
            foreach (Transform childTransform in _furnitureArea.transform)
            {
                var childObject = childTransform.gameObject;
                childObject.SetActive(false);
                Destroy(childObject);
            }
        }

        // Start configureZones
        private void SetFurnitureAreaVisible(bool isImageAllLoaded)
        {
            _furnitureArea.SetActive(isImageAllLoaded);
            _loadingIcon.SetActive(!isImageAllLoaded);
        }

        // Loaded procesor
        private void ConfigureZoneFinished()
        {
            _currentLoaded += 1;
            if (_currentLoaded == _loadCompletedAmount)
            {
                _currentLoaded = 0;
                _loadCompletedAmount = 0;
                SetFurnitureAreaVisible(true);
            }
        }

        // On button pressed, tell DataManager the ID of furniture
        private void OnButtonPressed(int referenceID)
        {
            _dataManager.QueryID = referenceID;
        }

        // Copy object with children
        private GameObject CopyObjectWithChildren()
        {
            var copiedObject = Instantiate(_sampleFurnitureEntry, _furnitureArea.transform);
            return copiedObject;
        }

        // Configure furniture item zone
        private async Task ConfigureFurnitureZone(FurnitureData furnitureData)
        {
            var copiedObject = CopyObjectWithChildren();
            var furnitureEntry = copiedObject.GetComponent<FurnitureObjectReference>();
            var entryButton = furnitureEntry.Button;
            if (!furnitureData.IsImageDownloaded())
            {
                await furnitureData.SetImageSpriteAsync();
            }
            furnitureEntry.SetName(furnitureData.Name);
            furnitureEntry.SetImage(furnitureData.GetImageSprite());
            entryButton.ButtonPressed.AddListener(() => OnButtonPressed(furnitureData.ID));
            copiedObject.SetActive(true);
            ConfigureZoneFinished();
        }

        // Page button activated configure
        public void ConfigurePageButton(bool isFirstPage, bool isLastPage)
        {
            _previousPageButton.SetActive(!isFirstPage);
            _nextPageButton.SetActive(!isLastPage);
        }

        // Update by page
        public void UpdateListByPage(int page)
        {
            DestroyAllListItem();
            var startIndex = page * itemPerPage;
            var endIndex = (page + 1) * itemPerPage;
            var isFirstPage = page == 0;
            var isLastPage = endIndex > _dataManager.GetFurnitureCount();
            var currentPageLabel = page + 1;
            var lastPageLabel = _dataManager.GetFurnitureCount() / itemPerPage + 1;
            _pageLabel.text = $"頁面 {currentPageLabel}/{lastPageLabel}";
            ConfigurePageButton(isFirstPage, isLastPage);
            if (isLastPage)
            {
                endIndex = _dataManager.GetFurnitureCount();
            }
            _loadCompletedAmount = endIndex - startIndex;
            SetFurnitureAreaVisible(false);
            for (var index = startIndex; index < endIndex; ++index)
            {
                var furnitureData = _dataManager.GetFurnitureDataByIndex(index);
                _ = ConfigureFurnitureZone(furnitureData);
            }
            _dataManager.ResetRecentlyQueriedIndex();
        }

        // Previous page button OnClick event
        public void PreviousPageOnClick()
        {
            _currentPage -= 1;
            UpdateListByPage(_currentPage);
        }

        // Next page button OnClick event
        public void NextPageOnClick()
        {
            _currentPage += 1;
            UpdateListByPage(_currentPage);
        }
    }
}