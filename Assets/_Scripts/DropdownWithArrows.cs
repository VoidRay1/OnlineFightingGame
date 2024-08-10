using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DropdownWithArrows : MonoBehaviour
{
    [SerializeField] private DropdownTemplate _dropdownTemplate;
    [SerializeField] private InputActionReference _selectInput;
    [SerializeField] private TMP_Dropdown _dropdown;
    [SerializeField] private Button _leftItemButton;
    [SerializeField] private Button _rightItemButton;

    private AudioClip _buttonClickedAudioClip;

    public event Action OnDropdownOpened;
    public event Action OnDropdownClosed;

    public TMP_Dropdown Dropdown => _dropdown;

    public void Initialize(List<string> options)
    {
        _selectInput.action.Enable();
        _dropdown.ClearOptions();
        _dropdown.AddOptions(options);
        _buttonClickedAudioClip = GameContext.Instance.AudioClipFactory.GetAudioClip(AudioClipType.ButtonClicked);
        _dropdownTemplate.OnDropdownOpened += DropdownOpened;
        _dropdownTemplate.OnDropdownClosed += DropdownClosed;
        _leftItemButton.onClick.AddListener(SelectLeftItem);
        _rightItemButton.onClick.AddListener(SelectRightItem);
    }

    private void Update()
    {
        if (IsDropdownSelected() == false)
        {
            return;
        }
        if (_selectInput.action.WasPressedThisFrame())
        {
            sbyte selectDirection = (sbyte)_selectInput.action.ReadValue<Vector2>().x;
            if (selectDirection == -1)
            {
                SelectLeftItem();
            }
            else if (selectDirection == 1)
            {
                SelectRightItem();
            }
        }
    }

    private void DropdownOpened()
    {
        OnDropdownOpened?.Invoke();
    }

    private void DropdownClosed()
    {
        OnDropdownClosed?.Invoke();
    }

    private void SelectLeftItem()
    {
        if (_dropdown.value - 1 >= 0) 
        {
            GameContext.Instance.AudioSourcePlayer.PlayClip(_buttonClickedAudioClip);
            _dropdown.value--;
        }
    }

    private void SelectRightItem()
    {
        if (_dropdown.value + 1 < _dropdown.options.Count)
        {
            GameContext.Instance.AudioSourcePlayer.PlayClip(_buttonClickedAudioClip);
            _dropdown.value++;
        }
    }

    private bool IsDropdownSelected()
    {
        return (EventSystem.current.currentSelectedGameObject != null && EventSystem.current.currentSelectedGameObject.name != name) == false;
    }

    private void OnDestroy()
    {
        _dropdownTemplate.OnDropdownOpened -= DropdownOpened;
        _dropdownTemplate.OnDropdownClosed -= DropdownClosed;
        _leftItemButton.onClick.RemoveListener(SelectLeftItem);
        _rightItemButton.onClick.RemoveListener(SelectRightItem);
    }
}