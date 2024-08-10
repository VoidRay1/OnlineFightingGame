using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Unity.Services.CloudSave;
using UnityEngine;
#if SERVER == false
using Unity.FileBrowser;
#endif
using System;

public class CustomAvatarService
{
    private readonly PlayerProfileView _playerProfileView;
    private readonly CustomAvatarSaver _customAvatarSaver = new CustomAvatarSaver(GameContext.Instance.AvatarsDataList);
    private readonly UnityWebRequestService _webRequestService;
#if SERVER == false
    private readonly FileBrowser _fileBrowser = new FileBrowser();
#endif
    private AvatarsTab _avatarsTab;
    private PopupMessageProvider PopupMessageProvider => GameContext.Instance.PopupMessageProvider;
    private IReadOnlyList<AvatarData> AvatarsData => GameContext.Instance.AvatarsDataList.AvatarsData;

    private const byte MaxAvatarsPerPlayer = 10;
    private const short MaxImageWidth = 512;
    private const short MaxImageHeight = 512;

    public CustomAvatarService(PlayerProfileView playerProfileView, UnityWebRequestService webRequestService)
    {
        _playerProfileView = playerProfileView;
        _webRequestService = webRequestService;
        SubscribeOnPlayerProfileEvents();
    }

    public void SetAvatarsTab(AvatarsTab avatarsTab)
    {
        _avatarsTab = avatarsTab;
        SubscribeOnAvatarsTabEvents();
    }

    private void TrySaveAvatar(string fileName, Texture2D avatar)
    {
        AvatarData avatarData = _customAvatarSaver.SaveAvatar(fileName, avatar);
        if (avatarData != null)
        {
            avatarData.Texture = avatar;
            SetAvatar(avatarData);
        }
        else
        {
            PopupMessageProvider.ShowErrorMessage("An error occurred when saving the avatar");
        }
    }

    private async void SetAvatar(AvatarData avatarData)
    {
        if (IsSelectedAvatar(avatarData.Id))
        {
            return;
        }
        _playerProfileView.SetAvatar(avatarData.Texture);
        GameContext.Instance.PlayerData.AvatarId = avatarData.Id;
        await CloudSaveService.Instance.Data.Player.SaveAsync(new Dictionary<string, object>
        {
            [Constants.CloudSave.Keys.PlayerAvatarId] = avatarData.Id
        });
        PopupMessageProvider.ShowNeutralMessage("Avatar successfully set to profile");
    }

    private void DeleteAvatar(AvatarData avatarData)
    {
        if (IsSelectedAvatar(avatarData.Id))
        {
            _playerProfileView.SetAvatar(null);
            GameContext.Instance.PlayerData.AvatarId = "";
        }
        _customAvatarSaver.DeleteAvatar(avatarData);
        _avatarsTab.ActiveButton.Select();
        _avatarsTab.RemoveAvatar(avatarData);
        PopupMessageProvider.ShowNeutralMessage("Avatar successfully deleted");
    }

    private void UploadCustomAvatar()
    {
#if SERVER == false
        var browserProperties = new BrowserProperties();
        browserProperties.filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
        browserProperties.filterIndex = 0;
        browserProperties.title = "Select Avatar";
        _playerProfileView.HideUploadAvatarButtonHoverHint();
        _fileBrowser.OpenFileBrowser(browserProperties, Cancel, AvatarSelected);
#endif
    }

    private void Cancel()
    {
        _playerProfileView.ShowUploadAvatarButtonHoverHint();
    }

    private void AvatarSelected(string filePath)
    {
        _playerProfileView.ShowUploadAvatarButtonHoverHint();
        if (ValidateImageSize(filePath) == false)
        {
            PopupMessageProvider.ShowErrorMessage($"The max size of the image can be {MaxImageWidth} by {MaxImageHeight}");
            return;
        }
        if (ValidateAvatarsCount() == false)
        {
            PopupMessageProvider.ShowErrorMessage($"The maximum number of avatars can be {MaxAvatarsPerPlayer}");
            return;
        }
        _webRequestService.GetTexture(filePath, AvatarReceived, AvatarFailed);
    }

    private void AvatarReceived(string filePath, Texture2D texture)
    {
        string fileName = Path.GetFileName(filePath);
        TrySaveAvatar(fileName, texture);
    }

    private void AvatarFailed()
    {
        PopupMessageProvider.ShowErrorMessage("An error occurred while importing an avatar");
    }

    private bool IsSelectedAvatar(string avatarId)
    {
        return avatarId == GameContext.Instance.PlayerData.AvatarId;
    }

    private bool ValidateAvatarsCount()
    {
        return AvatarsData.Count < MaxAvatarsPerPlayer;
    }

    private bool ValidateImageSize(string filePath)
    {
        Size imageSize = Image.FromFile(filePath).Size;
        return (imageSize.Width > MaxImageWidth || imageSize.Height > MaxImageHeight) == false;
    }

    private void SubscribeOnAvatarsTabEvents()
    {
        _avatarsTab.OnSetAvatar += SetAvatar;
        _avatarsTab.OnDeleteAvatar += DeleteAvatar;
        _avatarsTab.OnAvatarsTabDestroyed += AvatarsTabDestroyed;
    }

    private void SubscribeOnPlayerProfileEvents()
    {
        _playerProfileView.OnUploadCustomAvatar += UploadCustomAvatar;
        _playerProfileView.OnPlayerProfileDestroyed += PlayerProfileDestroyed;
    }

    private void UnSubscribeOnPlayerProfileEvents()
    {
        _playerProfileView.OnUploadCustomAvatar -= UploadCustomAvatar;
        _playerProfileView.OnPlayerProfileDestroyed -= PlayerProfileDestroyed;
    }

    private void UnSubscribeOnAvatarsTabEvents()
    {
        _avatarsTab.OnSetAvatar -= SetAvatar;
        _avatarsTab.OnDeleteAvatar -= DeleteAvatar;
        _avatarsTab.OnAvatarsTabDestroyed -= AvatarsTabDestroyed;
    }

    private void AvatarsTabDestroyed()
    {
        UnSubscribeOnAvatarsTabEvents();
    }

    private void PlayerProfileDestroyed()
    {
        UnSubscribeOnPlayerProfileEvents();
    }
}