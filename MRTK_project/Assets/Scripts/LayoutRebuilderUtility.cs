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
            for (var index = 0; index < parentRectTransform.childCount; index++)
            {
                var child = parentRectTransform.GetChild(index);
                var contentSizeFitter = child.GetComponent<ContentSizeFitter>();
                var rectTransform = child.GetComponent<RectTransform>();
                if (contentSizeFitter != null && rectTransform != null)
                    LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
            }
        }
    }
}