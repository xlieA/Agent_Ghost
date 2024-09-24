using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DynamicTextScroller : MonoBehaviour
{
    public ScrollRect scrollRect;
    public TMP_Text textField;
    public RectTransform contentRect;
    public float padding = 15f;

    void Start()
    {
        UpdateContentSize();
    }

    void Update()
    {
        LimitScrollPosition();
    }

    public void UpdateContentSize()
    {
        float textHeight = textField.preferredHeight;
        contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, textHeight + padding);

        LimitScrollPosition();
        KeepScrollAtBottom();
    }

    private void LimitScrollPosition()
    {
        float viewportHeight = scrollRect.viewport.rect.height;

        // if content is smaller than the viewport, no scrolling should be allowed
        if (contentRect.sizeDelta.y <= viewportHeight)
        {
            scrollRect.vertical = false;
            scrollRect.verticalNormalizedPosition = 1f;
        }
        else
        {
            scrollRect.vertical = true;
        }

        // clamp the scroll position (no infinitely scrolling beyond the text bounds)
        float contentHeight = contentRect.rect.height;
        float maxScrollY = contentHeight - viewportHeight;

        if (scrollRect.verticalNormalizedPosition < 0f)
        {
            scrollRect.verticalNormalizedPosition = 0f; // below the bottom
        }
        else if (scrollRect.verticalNormalizedPosition > 1f)
        {
            scrollRect.verticalNormalizedPosition = 1f; // above the top
        }
    }

    private void KeepScrollAtBottom()
    {
        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 0f; // 0f means bottom, 1f means top
    }
}
