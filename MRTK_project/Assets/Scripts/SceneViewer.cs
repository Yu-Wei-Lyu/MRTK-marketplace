using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class SceneViewer : MonoBehaviour
    {
        [SerializeField]
        private GameObject _handMenu;
        [SerializeField]
        private GameObject _mainSlate;

        private readonly List<GameObject> _childList = new List<GameObject>();

        // Start is called before the first frame update
        public void Start()
        {
            Initialize();
        }

        // Initialize child list
        private void Initialize()
        {
            int childCount = transform.childCount;
            _childList.Clear();
            for (int index = 0; index < childCount; ++index)
            {
                Transform childTransform = transform.GetChild(index);
                _childList.Add(childTransform.gameObject);
            }
        }

        // Deactive all child object excepted PopupDialog
        public void DeactiveAll()
        {
            _childList.ForEach(childObject => childObject.SetActive(false));
        }

        // Activate hand menu
        public void ActivateHandMenu()
        {
            _handMenu.SetActive(true);
        }

        // Activate hand menu
        public void ActivateMainSlate()
        {
            _mainSlate.SetActive(true);

        }

        // Get main slate object
        public GameObject GetMainSlate()
        {
            return _mainSlate;
        }
    }
}