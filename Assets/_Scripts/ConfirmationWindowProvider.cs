using System;
using System.Threading.Tasks;
using UnityEngine.Localization;

public class ConfirmationWindowProvider : AddressableLoader
{
    private ConfirmationWindow _confirmationWindow;

    private const string ConfirmationWindow = "Confirmation Window";

    public async Task<ConfirmationWindow> ShowConfirmationWindow(Action onConfirm, Action onCancel, LocalizedString description,
        LocalizedString confirmButtonText, LocalizedString cancelButtonText)
    {
        _confirmationWindow = await Load<ConfirmationWindow>(ConfirmationWindow);
        _confirmationWindow.Initialize(onConfirm, onCancel, description, confirmButtonText, cancelButtonText);
        _confirmationWindow.Show();
        GameContext.Instance.WindowCloser.AddWindowToHide(_confirmationWindow);
        return _confirmationWindow;
    }

    public void UnloadConfirmationWindow()
    {
        Unload(_confirmationWindow.gameObject);
    }
}