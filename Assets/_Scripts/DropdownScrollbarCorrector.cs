using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropdownScrollbarCorrector : MonoBehaviour, ISelectHandler
{
    private ScrollRect _scrollRect;
    private float _scrollPosition;

    private void Start()
    {
        _scrollRect = GetComponent<ScrollRect>();
        int childCount = _scrollRect.content.childCount - 1;
        int childIndex = transform.GetSiblingIndex();
        childIndex = childIndex < ((float)childCount / 2) ? childIndex - 1: childIndex;
        _scrollPosition = 1 - ((float)childIndex / childCount);
    }

    public void OnSelect(BaseEventData eventData)
    {
        _scrollRect.verticalScrollbar.value = _scrollPosition;
    }
}