using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MultipleTargetGraphicsDropdown : TMP_Dropdown
{
    [SerializeField] private List<Graphic> _targetGraphics;

    private SelectState _state = SelectState.NotSelected;

    public override void OnSelect(BaseEventData eventData)
    {
        if (_state == SelectState.SelectedByMouse)
        {
            return;
        }
        _state = SelectState.SelectedByNavigation;
        base.OnSelect(eventData);
        ChangeColorOnSelected();
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (_state == SelectState.SelectedByNavigation)
        {
            return;
        }
        _state = SelectState.SelectedByMouse;
        base.OnPointerEnter(eventData);
        ChangeColorOnSelected();
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        if (_state == SelectState.SelectedByMouse)
        {
            return;
        }
        _state = SelectState.NotSelected;
        base.OnDeselect(eventData);
        ChangeColorOnDeselected();
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        if (_state == SelectState.SelectedByNavigation)
        {
            return;
        }
        _state = SelectState.NotSelected;
        base.OnPointerExit(eventData);
        ChangeColorOnDeselected();
    }

    private void ChangeColorOnSelected()
    {
        foreach (Graphic graphic in _targetGraphics)
        {
            graphic.color = colors.selectedColor;
        }
    }

    private void ChangeColorOnDeselected()
    {
        foreach (Graphic graphic in _targetGraphics)
        {
            graphic.color = colors.normalColor;
        }
    }
}
