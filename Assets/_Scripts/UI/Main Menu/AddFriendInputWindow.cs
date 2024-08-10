using System;
using System.Text.RegularExpressions;
using UnityEngine.Localization;

public class AddFriendInputWindow : InputWindow
{
    private const byte MinPlayerNameLength = 8;
    private const byte MaxPlayerNameLength = 55;
    private const string CorrectFriendNameFormat = "[A-Za-z]+#[0-9]{4}";

    public override void Initialize(Action<string> onInputSubmited, Action onCancel,
        LocalizedString description, LocalizedString confirmButtonText, LocalizedString cancelButtonText)
    {
        DisableConfirmButton();
        base.Initialize(onInputSubmited, onCancel, description, confirmButtonText, cancelButtonText);
        Input.onValueChanged.AddListener(FriendNameInputValueChanged);
    }

    private void FriendNameInputValueChanged(string friendName)
    {
        if (Regex.IsMatch(friendName, "\\s"))
        {
            ValidationMessage.text = "Friend name can't contain a space";
            DisableConfirmButton();
            return;
        }
        if (friendName.Length < MinPlayerNameLength || friendName.Length > MaxPlayerNameLength)
        {
            ValidationMessage.text = $"The friend name in right format must be at least {MinPlayerNameLength} characters and less than {MaxPlayerNameLength} characters";
            DisableConfirmButton();
            return;
        }
        if (Regex.IsMatch(friendName, CorrectFriendNameFormat) == false)
        {
            ValidationMessage.text = "Friend name must be in format User#1234";
            DisableConfirmButton();
            return;
        }
        ValidationMessage.text = "";
        EnableConfirmButton();
    }

    private void OnDestroy()
    {
        Input.onValueChanged.RemoveListener(FriendNameInputValueChanged);
    }
}