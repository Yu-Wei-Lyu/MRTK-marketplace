using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.MixedReality.Toolkit.Experimental.UI;
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
        [SerializeField]
        private MRTKTMPInputField _searchText;
        [SerializeField]
        private GameObject _searchClearButton;

        private const int itemPerPage = 12;
        private int _currentPage = -1;
        private readonly List<FurnitureData> _searchResultList = new List<FurnitureData>();
        private bool _onFirstActivated = true;

        // Update furniture list and display on plate
        public override void Initialize()
        {
            if (_currentPage == -1)
            {
                _currentPage = 0;
            }
            if (_onFirstActivated)
            {
                SetSearchResult("");
                _searchClearButton.SetActive(false);
                _onFirstActivated = false;
            }
            UpdateListByPage(_currentPage);
        }

        // Destroy all item in the list 
        private void DestroyAllListItem()
        {
            foreach (Transform childTransform in _furnitureArea.transform)
            {
                GameObject childObject = childTransform.gameObject;
                childObject.SetActive(false);
                Destroy(childObject);
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
            GameObject copiedObject = Instantiate(_sampleFurnitureEntry, _furnitureArea.transform);
            return copiedObject;
        }

        // Configure furniture item zone
        private async Task ConfigureFurnitureZone(FurnitureData furnitureData)
        {
            GameObject copiedObject = CopyObjectWithChildren();
            FurnitureObjectReference furnitureEntry = copiedObject.GetComponent<FurnitureObjectReference>();
            PressableButtonHoloLens2 entryButton = furnitureEntry.Button;
            if (!furnitureData.IsImageDownloaded())
            {
                await furnitureData.DownloadImageAsync();
            }
            furnitureEntry.SetName(furnitureData.Name);
            furnitureEntry.SetImage(furnitureData.GetImageSprite());
            furnitureEntry.SetPrice(furnitureData.GetPriceFormat());
            entryButton.ButtonPressed.AddListener(() => OnButtonPressed(furnitureData.ID));
            copiedObject.SetActive(true);
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
            int startIndex = page * itemPerPage;
            int endIndex = (page + 1) * itemPerPage;
            bool isFirstPage = page == 0;
            bool isLastPage = endIndex > _searchResultList.Count;
            int currentPageLabel = page + 1;
            int lastPageLabel = _searchResultList.Count / itemPerPage + 1;
            _pageLabel.text = $"頁面 {currentPageLabel}/{lastPageLabel}";
            ConfigurePageButton(isFirstPage, isLastPage);
            if (isLastPage)
            {
                endIndex = _searchResultList.Count;
            }
            for (int index = startIndex; index < endIndex; ++index)
            {
                FurnitureData furnitureData = _searchResultList[index];
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

        // On search button pressed
        public void OnSearchButtonPressed()
        {
            _currentPage = 0;
            if (_searchText.text != "")
            {
                _searchClearButton.SetActive(true);
            }
            else
            {
                _searchClearButton.SetActive(false);
            }
            SetSearchResult(_searchText.text);
            UpdateListByPage(_currentPage);
        }

        // On search clear button pressed
        public void OnSearchClearButtonPressed()
        {
            _searchText.text = "";
            OnSearchButtonPressed();
        }

        // Set search result ID list by search input field
        public void SetSearchResult(string searchText)
        {
            _searchResultList.Clear();
            for (int queryIndex = 0; queryIndex < _dataManager.GetFurnitureCount(); ++queryIndex)
            {
                FurnitureData furnitureData = _dataManager.GetFurnitureDataByIndex(queryIndex);
                if (searchText == "")
                {
                    _searchResultList.Add(furnitureData);
                }
                else
                {
                    if (furnitureData.IsContainString(searchText))
                    {
                        _searchResultList.Add(furnitureData);
                    }
                }
            }
        }
    }
}