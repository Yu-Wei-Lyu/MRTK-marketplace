using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine;

namespace Assets.Scripts
{
    public class ScrollableListPopulator : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("The ScrollingObjectCollection to populate, if left empty. the populator will create on your behalf.")]
        private ScrollingObjectCollection scrollView;

        /// <summary>
        /// The ScrollingObjectCollection to populate, if left empty. the populator will create on your behalf.
        /// </summary>
        public ScrollingObjectCollection ScrollView
        {
            get
            {
                return scrollView;
            }
            set
            {
                scrollView = value;
            }
        }

        [SerializeField]
        [Tooltip("Object to duplicate in ScrollCollection")]
        private GameObject dynamicItem;

        /// <summary>
        /// Object to duplicate in <see cref="ScrollView"/>. 
        /// </summary>
        public GameObject DynamicItem
        {
            get
            {
                return dynamicItem;
            }
            set
            {
                dynamicItem = value;
            }
        }

        [SerializeField]
        private float cellWidth = 0.032f;

        [SerializeField]
        private float cellHeight = 0.032f;

        [SerializeField]
        private float cellDepth = 0.032f;

        [SerializeField]
        private int cellsPerTier = 3;

        [SerializeField]
        private int tiersPerPage = 5;

        [SerializeField]
        private Transform scrollPositionRef = null;

        public GridObjectCollection ItemCollection 
        { 
            get;
            set; 
        }

        // This function is called when the object becomes enabled and active.
        private void OnEnable()
        {
            // Make sure we find a collection
            if (scrollView == null)
            {
                scrollView = GetComponentInChildren<ScrollingObjectCollection>();
            }
        }

        // Generate scrolling list dynamically
        public void MakeScrollingList()
        {
            if (scrollView == null)
            {
                GameObject newScroll = new GameObject("Scrolling Object Collection");
                newScroll.transform.parent = scrollPositionRef ? scrollPositionRef : transform;
                newScroll.transform.localPosition = Vector3.zero;
                newScroll.transform.localRotation = Quaternion.identity;
                newScroll.SetActive(false);
                scrollView = newScroll.AddComponent<ScrollingObjectCollection>();

                // Prevent the scrolling collection from running until we're done dynamically populating it.
                scrollView.CellWidth = cellWidth;
                scrollView.CellHeight = cellHeight;
                scrollView.CellDepth = cellDepth;
                scrollView.CellsPerTier = cellsPerTier;
                scrollView.TiersPerPage = tiersPerPage;
            }

            ItemCollection = scrollView.GetComponentInChildren<GridObjectCollection>();

            if (ItemCollection == null)
            {
                GameObject collectionGameObject = new GameObject("Grid Object Collection");
                collectionGameObject.transform.SetPositionAndRotation(scrollView.transform.position, scrollView.transform.rotation);

                ItemCollection = collectionGameObject.AddComponent<GridObjectCollection>();
                ItemCollection.CellWidth = cellWidth;
                ItemCollection.CellHeight = cellHeight;
                ItemCollection.SurfaceType = ObjectOrientationSurfaceType.Plane;
                ItemCollection.Layout = LayoutOrder.ColumnThenRow;
                ItemCollection.Columns = cellsPerTier;
                ItemCollection.Anchor = LayoutAnchor.UpperLeft;

                scrollView.AddContent(collectionGameObject);
            }

            GenerateListItems();
            scrollView.gameObject.SetActive(true);
            ItemCollection.UpdateCollection();
        }

        // Make item
        public GameObject MakeItem(GameObject item)
        {
            GameObject itemInstance = Instantiate(item, ItemCollection.transform);
            itemInstance.SetActive(true);
            return itemInstance;
        }

        // Make list of item
        public virtual void GenerateListItems()
        {
            for (int i = 0; i < 12; ++i)
            {
                MakeItem(dynamicItem);
            }
        }
    }
}