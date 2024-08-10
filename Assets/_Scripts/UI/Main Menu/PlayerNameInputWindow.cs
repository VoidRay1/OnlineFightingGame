using System;
using System.Text.RegularExpressions;
using UnityEngine.Localization;

public class PlayerNameInputWindow : InputWindow
{
    private const byte MinPlayerNameLength = 3;
    private const byte MaxPlayerNameLength = 50;

    public override void Initialize(Action<string> onInputSubmited, Action onCancel,
        LocalizedString description, LocalizedString confirmButtonText, LocalizedString cancelButtonText)
    {
        DisableCancelButton();
        DisableConfirmButton();
        Input.onValueChanged.AddListener(PlayerNameInputValueChanged);
        base.Initialize(onInputSubmited, onCancel, description, confirmButtonText, cancelButtonText);
    }

    private void PlayerNameInputValueChanged(string playerName)
    {
        if (Regex.IsMatch(playerName, "\\s"))
        {
            ValidationMessage.text = "Player name can't contain a space";
            DisableConfirmButton();
            return;
        }
        if (playerName.Length < MinPlayerNameLength || playerName.Length > MaxPlayerNameLength)
        {
            ValidationMessage.text = $"The player name must be at least {MinPlayerNameLength} characters and less than {MaxPlayerNameLength} characters";
            DisableConfirmButton();
            return;
        }
        ValidationMessage.text = "";
        EnableConfirmButton();
    }

    private void OnDestroy()
    {
        Input.onValueChanged.RemoveListener(PlayerNameInputValueChanged);
    }
}