using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextDisplayController : MonoBehaviour
{
    private TextMeshProUGUI textMeshProUGUI;
    private ContentSizeFitter contentSizeFitter;
    private RectTransform rectTransform;

    // Awake is called when the script instance is being loaded.
    void Awake()
    {
        this.textMeshProUGUI = this.GetComponent<TextMeshProUGUI>();
        this.contentSizeFitter = this.GetComponent<ContentSizeFitter>();
        this.rectTransform = this.GetComponent<RectTransform>();
    }

    // Expand associated text object vertically
    public void ExpandTextVertically()
    {
        if (this.textMeshProUGUI.isTextOverflowing)
        {
            this.textMeshProUGUI.overflowMode = TextOverflowModes.Overflow;
            this.contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            LayoutRebuilderUtility.RebuildLayoutsWithContentSizeFitter(this.rectTransform);
        }
    }
}
