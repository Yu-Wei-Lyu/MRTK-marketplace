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
        private Transform _entryArea;
        [SerializeField]
        private TMP_Text _pageLabel;
        [SerializeField]
        private GameObject _previousPageButton;
        [SerializeField]
        private GameObject _nextPageButton;

        private const int itemPerPage = 12;
        private int _currentPage = 0;

        // Update furniture list and display on plate
        public override void Initialize()
        {
            _currentPage = 0;
            UpdateListByPage(_currentPage);
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

        // Page button activated configure
        public void ConfigurePageButton(bool isFirstPage, bool isLastPage)
        {
            _previousPageButton.SetActive(!isFirstPage);
            _nextPageButton.SetActive(!isLastPage);
        }

        // Update by page
        public void UpdateListByPage(int page)
        {
            DestroyAllListEntry();
            var startIndex = page * itemPerPage;
            var endIndex = (page + 1) * itemPerPage;
            var isLastPage = endIndex > _dataManager.GetFurnitureCount();
            _pageLabel.text = $"頁面 {page + 1}/{_dataManager.GetFurnitureCount() / itemPerPage + 1}";
            ConfigurePageButton(page == 0, isLastPage);
            if (isLastPage)
            {
                endIndex = _dataManager.GetFurnitureCount();
            }
            for (var index = startIndex; index < endIndex; ++index)
            {
                var furnitureData = _dataManager.GetFurnitureDataByIndex(index);
                _ = ConfigureFurnitureZone(furnitureData);
            }
            _dataManager.ResetRecentlyQueriedIndex();
        }

        // Destroy all the list Entry
        private void DestroyAllListEntry()
        {
            foreach (Transform childTransform in _entryArea)
            {
                childTransform.gameObject.SetActive(false);
                Destroy(childTransform.gameObject);
            }
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
        }


        // Copy object with children
        private GameObject CopyObjectWithChildren()
        {
            var copiedObject = Instantiate(_sampleFurnitureEntry, _entryArea);
            return copiedObject;
        }

        // On button pressed, tell DataManager the ID of furniture
        private void OnButtonPressed(int referenceID)
        {
            _dataManager.QueryID = referenceID;
        }
    }
}