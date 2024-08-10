using TMPro;
using UnityEngine;
using UnityEngine.Localization;

public class HoverHintView : BaseView
{
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private TMP_Text _hintText;    

    public void Initialize(LocalizedString hintText, Vector2 pivot, Vector2 anchorMin, Vector2 anchorMax, Vector2 anchoredPosition, Vector3 scale)
    {
        _hintText.text = hintText.GetLocalizedString();
        _rectTransform.pivot = pivot;
        _rectTransform.anchorMin = anchorMin;
        _rectTransform.anchorMax = anchorMax;
        _rectTransform.anchoredPosition = anchoredPosition;
        _rectTransform.localScale = scale;
    }
}
