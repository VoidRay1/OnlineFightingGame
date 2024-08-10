using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization;

public class HoverHintPresenter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    [SerializeField] private RectTransform _hintParent;
    [SerializeField] private RectTransform.Edge _hintSide;
    [SerializeField] private Vector2 _hintOffset;
    [SerializeField] private LocalizedString _hintText;
    [SerializeField, Range(0.05f, 1.0f)] private float _timeToShowHint;

    private readonly HoverHintProvider _hoverHintProvider = new HoverHintProvider();
    private HoverHintView _currentHintView;
    private HoverHintState _hoverHintState = HoverHintState.WaitingPointerEnter;
    private float _currentTime;
    private bool _hasFocus;

    private void Update()
    {
        if (IsHoverHintExists() && _hasFocus == false)
        {
            TryUnloadHoverHint();
        }
        if (_hoverHintState == HoverHintState.WaitingTimeToShow)
        {
            _currentTime += Time.deltaTime;
            if (_currentTime > _timeToShowHint)
            {
                TryShowHoverHint();
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _hasFocus = true;
        if (_hoverHintState == HoverHintState.Show)
        {
            return;
        }
        _hoverHintState = HoverHintState.WaitingTimeToShow;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (EventSystem.current.currentSelectedGameObject != gameObject)
        {
            _hasFocus = false;
            TryUnloadHoverHint();
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        _hasFocus = true;
        if (IsHoverHintExists() == false)
        {
            TryShowHoverHint();
        }
    }

    public void OnDeselect(BaseEventData eventData)
    {
        _hasFocus = false;
        TryUnloadHoverHint();
    }

    public void HideHoverHint()
    {
        if (IsHoverHintExists())
        {
            _currentHintView.Hide();
        }
    }

    public void ShowHoverHint()
    {
        if (IsHoverHintExists())
        {
            _currentHintView.Show();
        }
    }

    public void TryUnloadHoverHint()
    {
        if (IsHoverHintExists())
        {
            _currentHintView = null;
            _hoverHintProvider.UnloadHoverHint();
        }
        _currentTime = 0.0f;
        _hoverHintState = HoverHintState.WaitingPointerEnter;
    }

    private async void TryShowHoverHint()
    {
        if (IsHoverHintExists() == false)
        {
            _hoverHintState = HoverHintState.Show;
            _currentHintView = await _hoverHintProvider.ShowHoverHint(_hintText, GetHintPosition(), Quaternion.identity, _hintParent,
                new Vector2(0.5f, 0.5f), new Vector2(1.0f, 1.0f), new Vector2(1.0f, 1.0f), new Vector3(1.0f, 1.0f, 1.0f));
        }
    }

    private Vector3 GetHintPosition()
    {
        switch (_hintSide)
        {
            case RectTransform.Edge.Top:
                return new Vector2(-_hintParent.sizeDelta.x / 2, _hintParent.sizeDelta.y) + _hintOffset;
            case RectTransform.Edge.Bottom:
                return new Vector2(-_hintParent.sizeDelta.x / 2, -_hintParent.sizeDelta.y) + _hintOffset;
            case RectTransform.Edge.Left:
                return new Vector2(-_hintParent.sizeDelta.x * 2, -_hintParent.sizeDelta.y / 2) + _hintOffset;
            case RectTransform.Edge.Right:
                return new Vector2(_hintParent.sizeDelta.x * 2, -_hintParent.sizeDelta.y / 2) + _hintOffset;
        }
        return Vector3.zero;
    }

    private bool IsHoverHintExists()
    {
        return _currentHintView != null;
    }
}