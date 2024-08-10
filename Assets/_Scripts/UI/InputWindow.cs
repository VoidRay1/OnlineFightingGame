using System;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;

public class InputWindow : ConfirmationWindow
{
    [SerializeField] private TMP_InputField _input;
    [SerializeField] private TMP_Text _validationMessage;

    private event Action<string> _onInputSubmited;

    public TMP_InputField Input => _input;
    public TMP_Text ValidationMessage => _validationMessage;

    public virtual void Initialize(Action<string> onInputSubmited, Action onCancel,
        LocalizedString description, LocalizedString confirmButtonText, LocalizedString cancelButtonText) 
    {
        _onInputSubmited = onInputSubmited;
        Initialize(InputSubmited, onCancel, description, confirmButtonText, cancelButtonText);
        _input.Select();
    }

    public override void EnableConfirmButton()
    {
        _input.SetSelectableOnDown(ConfirmButton);
        ConfirmButton.SetSelectableOnUp(_input);
        base.EnableConfirmButton();
    }

    public override void DisableConfirmButton()
    {
        _input.SetSelectableOnDown(CancelButton);
        base.DisableConfirmButton();
    }

    private void InputSubmited()
    {
        _onInputSubmited?.Invoke(_input.text);
    }
}