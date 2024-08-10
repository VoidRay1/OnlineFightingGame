using System.Threading.Tasks;
using UnityEngine;

public class PopupMessageProvider : AddressableLoader
{
    private PopupMessage _popupMessage;
    private Transform _parent;

    public void SetParent(Transform parent)
    {
        _parent = parent;
    }

    public async void ShowSuccessfullMessage(string text)
    {
        await LoadPopupMessage();
        _popupMessage.ShowSuccessfulMessage(text);
    }

    public async void ShowNeutralMessage(string text)
    {
        await LoadPopupMessage();
        _popupMessage.ShowNeutralMessage(text);
    }

    public async void ShowErrorMessage(string text)
    {
        await LoadPopupMessage();
        _popupMessage.ShowErrorMessage(text);
    }

    private async Task LoadPopupMessage()
    {
        if (_popupMessage != null) 
        {
            UnloadPopupMessage();
        }
        _popupMessage = await Load<PopupMessage>(Constants.Addressables.Keys.PopupMessage, _parent);
        _popupMessage.OnFadeInMessageEnded += UnloadPopupMessage;
    }

    private void UnloadPopupMessage()
    {
        _popupMessage.OnFadeInMessageEnded -= UnloadPopupMessage;
        _popupMessage = null;
        UnloadCachedGameObject();
    }
}