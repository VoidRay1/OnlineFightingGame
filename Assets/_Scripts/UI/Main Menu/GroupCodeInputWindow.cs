using System.Text.RegularExpressions;
using System;
using UnityEngine.Localization;

public class GroupCodeInputWindow : InputWindow
{
    private const byte GroupCodeLength = 6;

    public override void Initialize(Action<string> onInputSubmited, Action onCancel,
        LocalizedString description, LocalizedString confirmButtonText, LocalizedString cancelButtonText)
    {
        DisableConfirmButton();
        base.Initialize(onInputSubmited, onCancel, description, confirmButtonText, cancelButtonText);
        Input.onValueChanged.AddListener(GroupCodeInputValueChanged);
    }

    private void GroupCodeInputValueChanged(string groupCode)
    {
        if (Regex.IsMatch(groupCode, "\\s"))
        {
            ValidationMessage.text = "Group code can't contain a space";
            DisableConfirmButton();
            return;
        }
        if (groupCode.Length < GroupCodeLength || groupCode.Length > GroupCodeLength)
        {
            ValidationMessage.text = $"The group code length must be {GroupCodeLength} characters";
            DisableConfirmButton();
            return;
        }
        ValidationMessage.text = "";
        EnableConfirmButton();
    }

    private void OnDestroy()
    {
        Input.onValueChanged.RemoveListener(GroupCodeInputValueChanged);
    }
}