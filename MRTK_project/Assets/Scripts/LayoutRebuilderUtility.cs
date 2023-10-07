using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public static class LayoutRebuilderUtility
    {
        // Rebuild layouts with content size fitter (component)
        public static void RebuildLayoutsWithContentSizeFitter(RectTransform parentRectTransform)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(parentRectTransform);
            for (int index = 0; index < parentRectTransform.childCount; index++)
            {
                Transform child = parentRectTransform.GetChild(index);
                ContentSizeFitter contentSizeFitter = child.GetComponent<ContentSizeFitter>();
                RectTransform rectTransform = child.GetComponent<RectTransform>();
                if (contentSizeFitter != null && rectTransform != null)
                    LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
            }
        }
    }
}