using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class TextDisplayController : MonoBehaviour
    {
        private const float DETAILHEIGHT = 0.19F;
        private TextMeshProUGUI _textMeshProUGUI;
        private ContentSizeFitter _contentSizeFitter;
        private RectTransform _rectTransform;

        // Awake is called when the script instance is being loaded.
        public void Awake()
        {
            _textMeshProUGUI = GetComponent<TextMeshProUGUI>();
            _contentSizeFitter = GetComponent<ContentSizeFitter>();
            _rectTransform = GetComponent<RectTransform>();
        }

        // Default text performance
        public void DefaultPerformance()
        {
            _textMeshProUGUI.overflowMode = TextOverflowModes.Ellipsis;
            _contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.Unconstrained;
            _rectTransform.sizeDelta = new Vector2(_rectTransform.sizeDelta.x, DETAILHEIGHT);
        }

        // Expand associated text object vertically
        public void ExpandTextVertically()
        {
            if (_textMeshProUGUI.isTextOverflowing)
            {
                _textMeshProUGUI.overflowMode = TextOverflowModes.Overflow;
                _contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
                LayoutRebuilderUtility.RebuildLayoutsWithContentSizeFitter(_rectTransform);
            }
        }

        // Return the bool of the text overflowing or not
        public bool IsTextOverflowing()
        {
            return _textMeshProUGUI.isTextOverflowing;
        }

        // Set text
        public void SetText(string targetString)
        {
            _textMeshProUGUI.text = targetString;
        }
    }
}