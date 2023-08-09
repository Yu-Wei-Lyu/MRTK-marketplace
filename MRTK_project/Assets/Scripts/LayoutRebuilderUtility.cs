using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LayoutRebuilderUtility : MonoBehaviour
{
    // Rebuild layouts with content size fitter (component)
    public static void RebuildLayoutsWithContentSizeFitter(RectTransform parentRectTransform)
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(parentRectTransform);
        for (int i = 0; i < parentRectTransform.childCount; i++)
        {
            Transform child = parentRectTransform.GetChild(i);
            ContentSizeFitter contentSizeFitter = child.GetComponent<ContentSizeFitter>();
            RectTransform rectTransform = child.GetComponent<RectTransform>();
            if (contentSizeFitter != null && rectTransform != null)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
            }
        }
    }
}
