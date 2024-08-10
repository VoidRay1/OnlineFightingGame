using System;
using UnityEngine.Localization;

public class InputWindowProvider : AddressableLoader
{
    private InputWindow _inputWindow;

    public async void ShowInputWindow(string inputWindowId, Action<string> onInputSubmited, Action onCancel,
        LocalizedString description, LocalizedString confirmButtonText, LocalizedString cancelButtonText)
    {
        _inputWindow = await Load<InputWindow>(inputWindowId);
        _inputWindow.Initialize(onInputSubmited, onCancel, description, confirmButtonText, cancelButtonText);
        _inputWindow.Show();
        GameContext.Instance.WindowCloser.AddWindowToHide(_inputWindow);
    }

    public void HideInputWindow()
    {
        _inputWindow.Hide();
    }

    public void ShowInputWindow()
    {
        _inputWindow.Show();
    }

    public void UnloadInputWindow()
    {
        UnloadCachedGameObject();
        _inputWindow = null;
    }
}