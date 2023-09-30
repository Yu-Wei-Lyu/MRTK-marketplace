using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class SceneViewer : MonoBehaviour
    {
        [SerializeField]
        private GameObject _handMenu;

        private readonly List<GameObject> _childList = new List<GameObject>();

        // Start is called before the first frame update
        public void Start()
        {
            Initialize();
        }

        // Initialize child list
        private void Initialize()
        {
            var childCount = transform.childCount;
            _childList.Clear();
            for (var index = 0; index < childCount; ++index)
            {
                var childTransform = transform.GetChild(index);
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
    }
}